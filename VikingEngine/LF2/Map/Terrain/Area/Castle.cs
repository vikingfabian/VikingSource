using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;
using VikingEngine.LF2.Director;
using VikingEngine.LF2.Map.Terrain.Generation;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    /// <summary>
    /// A big place where the evil boss is waiting for you, surrounded with high walls
    /// 
    /// TODO
    /// - Fix weird bug in corners of castle  !!!!!  TODO  !!!!!
    /// - Add torches as wall decorations               (needs fire particles)
    /// - Add drinking fountains (for healing hp)       (needs water)
    /// - Improve AI
    /// - Outer wall windows
    /// - Triangular archways?
    /// - Towers in corners with chests on top instead  (needs better physics + cutthrough shader)
    /// - Roofs                                         (needs cutthrough shader)
    /// </summary>
    class Castle : AbsUrban
    {
        #region Constants
        const int All4Dirs = 4;

        // Random generation parameters for outside.
        const int OuterWallHeight = (ChamberWallHeight + MaxInnerWallHeight) / 2;
        const int OuterWallGroundDepth = 3;
        const int OuterWallThickness = MaxInnerWallThickness + 2;
        const int MerlonWidth = 4;   // Do not change
        const int MerlonSpacing = 4; // Do not change
        const int MinMerlonHeight = 1;
        const int MaxMerlonHeight = 5;
        const int ParapetThickness = MerlonWidth / 2; // Do not change
        const int MinParapetHeight = 1;
        const int MaxParapetHeight = 5;

        // Random generation parameters for inside.
        const int MinArchwayWidth = 2;
        const int MaxArchwayWidth = 3;
        const int MinArchwayOverhang = 0;
        const int MaxArchwayOverhang = 1;
        const int MaxInnerWallThickness = 6;
        const int MinInnerWallThickness = 1;
        const int MinEntranceHeight = Players.Player.CharacterHeight;
        const int MinInnerWallHeight = MinEntranceHeight + MaxArchwayWidth + 1;
        const int MaxInnerWallHeight = MinInnerWallHeight + 4;
        const int MinEntranceWidth = Players.Player.CharacterWidth * 2;
        const int MinCarpetOutlineWidth = 1;
        const int MinWallDecorationCount = 0;
        const int MaxWallDecorationCount = 3;
        const int MinDecorationFloorDistance = 2;
        const int MaxDecorationFloorDistance = 4;

        // Random generation parameters for central chamber.
        const int ChamberWallHeight = (MaxInnerWallHeight * 3) / 2;
        const int PillarRadius = 6;

        // Common
        const int FancyEntranceHeight = 8;
        const int FancyEntranceWidth = 6;
        #endregion

        #region Members
        private AlgoObjects.Labyrinth labyrinth;
        private Rectangle2 inner;
        private IntVector2 chamberEntrancePos;
        private IntVector2 castleEntrancePos;
        public Corner bossKeyCorner;
        private int floorY = WorldPosition.ChunkStandardHeight;

        // All subject to change of course
        private Data.MaterialType floorMaterial1 = Data.MaterialType.stone;
        private Data.MaterialType floorMaterial2 = Data.MaterialType.patterned_stone;
        private Data.MaterialType wallMaterial   = Data.MaterialType.gray_bricks;
        private Data.MaterialType pillarMaterial = Data.MaterialType.marble;
        private Data.MaterialType detailMaterial = Data.MaterialType.black_wood;
        private Data.MaterialType carpetMaterial = Data.MaterialType.red_brown;
        private Data.MaterialType decorationMaterial = Data.MaterialType.gold;

        private List<string> wallMessages = new List<string>
        {
            "lust",
            "gluttony",
            "greed",
            "sloth",
            "wrath",
            "envy",
            "pride"
        };

        // Random generation.
        private int entranceHeight = MinEntranceHeight;
        private int innerWallHeight = MinInnerWallHeight;
        private int entranceWidth = MinEntranceWidth;
        private int innerWallThickness = MinInnerWallThickness;
        private int archwayWidth = MinArchwayWidth;
        private int archwayOverhang = MinArchwayOverhang;
        private int carpetOutlineWidth = MinCarpetOutlineWidth;
        private int decorationFloorDistance = MinDecorationFloorDistance;
        private int maxEntranceWidth = 0;
        private int maxEntranceHeight = 0;
        private int halfCarpetWidth = 0;
        private int parapetHeight = MinParapetHeight;
        private int merlonHeight = MinMerlonHeight;

        private EnvironmentType environment = EnvironmentType.NUM_NON;
        #endregion

        public Castle(IntVector2 position, int level)
            : base(new IntVector2(9), position, level)
        {
            labyrinth = new AlgoObjects.Labyrinth(chunkSize);
            inner = new Rectangle2(chunkSize / PublicConstants.Twice, IntVector2.One);//innerArea();
            inner.AddRadius(1);

            labyrinth.LockArea(inner);
            chamberEntrancePos = inner.Position;
            chamberEntrancePos.X += inner.Width / PublicConstants.Twice;
            //raise it to the affected chunk above
            --chamberEntrancePos.Y;

            castleEntrancePos = new IntVector2(chunkSize.X / 2, 0);
            labyrinth.Generate(castleEntrancePos);
            bossKeyCorner = (Corner)Data.RandomSeed.Instance.Next((int)Corner.NUM);

            IntVector2 squarePos = IntVector2.Zero;
            ForXYLoop detailLoop = new ForXYLoop(IntVector2.Zero, squareSize - 1);
            while (!detailLoop.Done)
            {
                squarePos = detailLoop.Next_Old();
                byte rnd = Data.RandomSeed.Instance.NextByte();
                if (rnd < 40)
                {
                    squares[squarePos.X, squarePos.Y] = (int)UrbanSquareType.RotatingTrap;
                }
            }


            // Start randomizing parameters for castle creation!
            Data.RandomSeedInstance randomizer = Data.RandomSeed.Instance;

            archwayWidth += randomizer.Next(MaxArchwayWidth - MinArchwayWidth);
            archwayOverhang += randomizer.Next(MaxArchwayOverhang - MinArchwayOverhang);
            //archwayWidth = MaxArchwayWidth;               // TESTING
            //archwayOverhang = MaxArchwayOverhang;         // TESTING

            int maxCarpetOutlineWidth = archwayWidth;
            carpetOutlineWidth += randomizer.Next((maxCarpetOutlineWidth + 1) - MinCarpetOutlineWidth);

            innerWallThickness += randomizer.Next(MaxInnerWallThickness - MinInnerWallThickness);
            //innerWallThickness = MaxInnerWallThickness;   // TESTING
            innerWallHeight += randomizer.Next(MaxInnerWallHeight - MinInnerWallHeight);
            //innerWallHeight = MaxInnerWallHeight;         // TESTING
            maxEntranceHeight = innerWallHeight - archwayWidth - 1; // archway below top
            entranceHeight += randomizer.Next(maxEntranceHeight - MinEntranceHeight);
            //entranceHeight = maxEntranceHeight;           // TESTING

            int subtractFromWidth = innerWallThickness + archwayWidth + archwayOverhang + 1;
            maxEntranceWidth = Math.Min(entranceHeight * 2, floorY + entranceHeight) - subtractFromWidth;
            if (maxEntranceWidth > MinEntranceWidth)
                entranceWidth += randomizer.Next(maxEntranceWidth - MinEntranceWidth);
            //entranceWidth = MinEntranceWidth;             // TESTING

            decorationFloorDistance += randomizer.Next((MaxDecorationFloorDistance + 1) - MinDecorationFloorDistance);
            halfCarpetWidth = entranceWidth / 2 + archwayWidth;

            parapetHeight += randomizer.Next((MaxParapetHeight + 1) - MinParapetHeight);
            merlonHeight += randomizer.Next((MaxMerlonHeight + 1) - MinMerlonHeight);
        }

        /// <summary>
        /// Creates a chunk of castle
        /// </summary>
        /// <param name="chunkPos">The chunk to construct upon</param>
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            base.BuildOnChunk(chunk, dataGenerated, gameObjects);
           
            Rectangle2 area = new Rectangle2(IntVector2.Zero, this.chunkSize - 1);
            IntVector2 localPos = chunk.Index - position;
            bool centralChamber = inner.IntersectTilePoint(localPos);
            bool isCorner = area.IsCorner(localPos);

            if (gameObjects)
            {

                if (centralChamber)
                {
                    if (AreaChunkCenter == chunk.Index)
                    {

                        //place boss
                        IntVector2 bossPosition = AreaChunkCenter;
                        LfRef.chunks.GetScreen(bossPosition).WriteProtected = true;
                        new GameObjects.EnvironmentObj.BossLockData(bossPosition, areaLevel); // this is ok, because this method is only called once per castle
                    }
                }
                else
                {
                    if (isCorner)
                    {
                        if (dataGenerated)
                        {
                            Corner c = area.GetCorner(localPos);
                            CreateChest(chunk.Index, c);
                        }
                    }
                    else
                    {
                        bool canCreateEnemiesOnChunk = localPos != castleEntrancePos;

                        if (canCreateEnemiesOnChunk)
                            CreateSpawnerChance(chunk.Index, 150);
                    }
                }
            }
            else
            {
                

                // Only checked once
                if (environment == EnvironmentType.NUM_NON)
                {
                    environment = LfRef.worldOverView.GetChunkData(chunk.Index).Environment;
                    SetCastleMaterials(environment);
                }

                CreateFloor(chunk.Index);

                WorldPosition wp = new WorldPosition();
                wp.ChunkGrindex = chunk.Index;
                wp.Y = WorldPosition.ChunkStandardHeight;

                
                List<AlgoObjects.Wall> walls = labyrinth.GetWalls(localPos);
                bool needsCenterpieceCarpet = true;
               

                foreach (AlgoObjects.Wall wall in walls)
                {
                    Dir4 wallDir = wall.Dir;
                    if (wall.Empty)
                        break;

                    switch (wall.Opening)
                    {
                        default:
                            {
                                // The normal maze rooms
                                if (wall.Opening == AlgoObjects.OpeningType.Open)
                                {
                                    // We need carpets, and a carpet! Also, don't forget the carpet.
                                    // After that's done, add a carpet.
                                    ArchitecturalLib.CreateCarpetInChunk(chunk.Index, wallDir, floorY, halfCarpetWidth,
                                        carpetMaterial, carpetOutlineWidth, decorationMaterial, ref needsCenterpieceCarpet);
                                }
                                else
                                {
                                    CreateDecorationsOnWall(chunk.Index, wallDir);
                                }

                                // Create walls.
                                WorldVolume wallVol = ArchitecturalLib.CreateWallOnChunkEdge(chunk.Index,
                                    floorY, wallDir, innerWallThickness, innerWallHeight,
                                    wallMaterial, true);

                                // Create doorways.
                                if (wall.Opening == AlgoObjects.OpeningType.Open)
                                {
                                    wallVol.SubFromLateralEnds(wallDir, WorldPosition.ChunkHalfWidth - entranceWidth / 2);
                                    wallVol.Size.Y = entranceHeight;
                                    wallVol.CreateArchway(wallDir, detailMaterial, archwayWidth, archwayOverhang);
                                }
                            }
                            break;
                        case AlgoObjects.OpeningType.BorderToLocked:
                            {
                                // Wall closing in the central chamber.
                                WorldVolume chamberWall = ArchitecturalLib.CreateWallOnChunkEdge(
                                    chunk.Index, floorY, wallDir, innerWallThickness + 2, ChamberWallHeight, wallMaterial);

                                if (wall.Position == chamberEntrancePos)
                                {
                                    // Create a fancy entrance!
                                    ArchitecturalLib.CreateFancyArchwayOnWall(wallDir, chamberWall,
                                        detailMaterial, decorationMaterial, FancyEntranceHeight, FancyEntranceWidth);

                                    // Don't forget the carpet!
                                    ArchitecturalLib.CreateCarpetInChunk(chunk.Index, wallDir, floorY, halfCarpetWidth,
                                        carpetMaterial, carpetOutlineWidth, decorationMaterial, ref needsCenterpieceCarpet);
                                }
                            }
                            break;
                        case AlgoObjects.OpeningType.OutsideArea:
                            {
                                // Create an outer wall.
                                WorldVolume outerWall = ArchitecturalLib.CreateWallOnChunkEdge(
                                    chunk.Index, floorY - OuterWallGroundDepth, wallDir, OuterWallThickness,
                                    OuterWallHeight + OuterWallGroundDepth, wallMaterial, false);
                                Dir4 cutDir = Dir4.NUM_NON;
                                if (isCorner)
                                {
                                    switch (area.GetCorner(localPos))
                                    {
                                        case Corner.BottomLeft:
                                            if (wallDir == Dir4.S)
                                                cutDir = Dir4.W;
                                            if (wallDir == Dir4.W)
                                                cutDir = Dir4.S;
                                            break;
                                        case Corner.BottomRight:
                                            if (wallDir == Dir4.S)
                                                cutDir = Dir4.E;
                                            if (wallDir == Dir4.E)
                                                cutDir = Dir4.S;
                                            break;
                                        case Corner.TopLeft:
                                            if (wallDir == Dir4.N)
                                                cutDir = Dir4.W;
                                            if (wallDir == Dir4.W)
                                                cutDir = Dir4.N;
                                            break;
                                        case Corner.TopRight:
                                            if (wallDir == Dir4.N)
                                                cutDir = Dir4.E;
                                            if (wallDir == Dir4.E)
                                                cutDir = Dir4.N;
                                            break;
                                        default: break;
                                    }
                                }
                                CreateOuterWallOrnaments(wallDir, outerWall, cutDir, 1);

                                if (wall.Position == castleEntrancePos)
                                {
                                    //canCreateEnemiesOnChunk = false;
                                    // Create castle entrance.
                                    CreateFloorExtensionUnderEntrance(chunk.Index);
                                    GenerateCarpetExtensionAtEntrance(chunk.Index);

                                    outerWall.SubFromSide(CubeFace.Ynegative, OuterWallGroundDepth);
                                    ArchitecturalLib.CreateFancyArchwayOnWall(wallDir, outerWall, detailMaterial,
                                        wallMaterial, FancyEntranceHeight, FancyEntranceWidth);

                                    // Yay, more carpet! ... Hm, actually this is the first one when you enter the castle :)
                                    ArchitecturalLib.CreateCarpetInChunk(chunk.Index, wallDir, floorY, halfCarpetWidth,
                                        carpetMaterial, carpetOutlineWidth, decorationMaterial, ref needsCenterpieceCarpet);
                                }
                            }
                            break;

                    }
                }

                
                // Central chamber
                if (centralChamber)
                {
                    if (inner.IsTileCorner(localPos) || // Corners
                        AreaChunkCenter - new IntVector2(1, 0) == chunk.Index || AreaChunkCenter + new IntVector2(1, 0) == chunk.Index) // Left and right of center
                    {
                        WorldVolume pillarVolume = new WorldVolume(chunk.Index);
                        pillarVolume.AddToXZEnds(PillarRadius)
                            .SetYPos(floorY + 1)
                            .AddToSide(CubeFace.Ypositive, ChamberWallHeight)
                            .FillPillar(pillarMaterial, decorationMaterial, 2, 1, 1, 1);
                        return;
                    }
                    else
                        if (AreaChunkCenter == chunk.Index)
                        {

                            // Entrance direction is always towards the north from inside the chamber! 
                            ArchitecturalLib.CreateCarpetInChunk(chunk.Index, Dir4.N, floorY, entranceWidth / 2 + archwayWidth,
                                carpetMaterial, carpetOutlineWidth, decorationMaterial, ref needsCenterpieceCarpet);
                        }
                        else if (AreaChunkCenter - new IntVector2(0, 1) == chunk.Index) // chunk between center and entrance
                        {
                            // Generate path from center to entrance.
                            ArchitecturalLib.CreateCarpetInChunk(chunk.Index, Dir4.S, floorY, entranceWidth / 2 + archwayWidth,
                            carpetMaterial, carpetOutlineWidth, decorationMaterial, ref needsCenterpieceCarpet);
                            ArchitecturalLib.CreateCarpetInChunk(chunk.Index, Dir4.N, floorY, entranceWidth / 2 + archwayWidth,
                            carpetMaterial, carpetOutlineWidth, decorationMaterial, ref needsCenterpieceCarpet);
                        }
                }
            }
        }

        private void SetCastleMaterials(EnvironmentType environment)
        {
            switch (environment)
            {
                case EnvironmentType.Burned:
                    carpetMaterial = Data.MaterialType.dark_gray;
                    floorMaterial1 = Data.MaterialType.stone;
                    floorMaterial2 = Data.MaterialType.patterned_stone;
                    decorationMaterial = Data.MaterialType.iron;
                    detailMaterial = Data.MaterialType.dark_gray;
                    pillarMaterial = Data.MaterialType.black;
                    wallMaterial = Data.MaterialType.gray;
                    break;

                case EnvironmentType.Desert:
                    carpetMaterial = Data.MaterialType.red_brown;
                    floorMaterial1 = Data.MaterialType.stone;
                    floorMaterial2 = Data.MaterialType.patterned_stone;
                    decorationMaterial = Data.MaterialType.gold;
                    detailMaterial = Data.MaterialType.black_wood;
                    pillarMaterial = Data.MaterialType.marble;
                    wallMaterial = Data.MaterialType.sand_stone_bricks;
                    break;

                case EnvironmentType.Forest:
                    carpetMaterial = Data.MaterialType.dirt;
                    floorMaterial1 = Data.MaterialType.wood;
                    floorMaterial2 = Data.MaterialType.patterned_gray_wood;
                    decorationMaterial = Data.MaterialType.black;
                    detailMaterial = Data.MaterialType.black_wood;
                    pillarMaterial = Data.MaterialType.marble;
                    wallMaterial = Data.MaterialType.gray;
                    break;

                case EnvironmentType.Mountains:
                    carpetMaterial = Data.MaterialType.gray;
                    floorMaterial1 = Data.MaterialType.cobble_stone;
                    floorMaterial2 = Data.MaterialType.patterned_stone;
                    decorationMaterial = Data.MaterialType.stony_forest;
                    detailMaterial = Data.MaterialType.patterned_mossy_stone;
                    pillarMaterial = Data.MaterialType.stone;
                    wallMaterial = Data.MaterialType.gray;
                    break;

                case EnvironmentType.Swamp:
                    carpetMaterial = Data.MaterialType.dirt;
                    floorMaterial1 = Data.MaterialType.cobble_stone;
                    floorMaterial2 = Data.MaterialType.patterned_stone;
                    decorationMaterial = Data.MaterialType.patterned_growing_wood;
                    detailMaterial = Data.MaterialType.black_wood;
                    pillarMaterial = Data.MaterialType.stone;
                    wallMaterial = Data.MaterialType.gray;
                    break;

                case EnvironmentType.Grassfield: // Fallthrough
                default:
                    carpetMaterial = Data.MaterialType.red_brown;
                    floorMaterial1 = Data.MaterialType.stone;
                    floorMaterial2 = Data.MaterialType.patterned_stone;
                    decorationMaterial = Data.MaterialType.gold;
                    detailMaterial = Data.MaterialType.black_wood;
                    pillarMaterial = Data.MaterialType.marble;
                    wallMaterial = Data.MaterialType.gray;
                    break;
            }
        }

        /// <summary>
        /// Creates flooring in a chesslike tile pattern in the given chunk.
        /// </summary>
        /// <param name="chunkPos">The given chunk</param>
        private void CreateFloor(IntVector2 chunkPos)
        {
            // fill with flat floor
            LfRef.chunks.GetScreen(chunkPos).RemoveAllOrganicMaterialAbove(floorY);
            LfRef.chunks.GetScreen(chunkPos).FillLayer(floorY, floorMaterial1);

            // Create chessboard-like floor
            WorldPosition pos = new WorldPosition(chunkPos, 0, floorY, 0);
            const int Squares = 4;
            const int SquareSize = WorldPosition.ChunkWidth / Squares;
            WorldVolume checkerVolume = new WorldVolume(pos, new IntVector3(SquareSize / 2, 1, SquareSize / 2));
            for (int outerX = 0; outerX != Squares; ++outerX)
            {
                for (int outerY = 0; outerY != Squares; ++outerY)
                {
                    checkerVolume.Position = pos.GetNeighborPos(new IntVector3(outerX * SquareSize, 0, outerY * SquareSize));
                    checkerVolume.Fill(floorMaterial2);
                    checkerVolume.Position = checkerVolume.Position.GetNeighborPos(new IntVector3(SquareSize / 2, 0, SquareSize / 2));
                    checkerVolume.Fill(floorMaterial2);
                }
            }
        }

        /// <summary>
        /// Creates a random number of decorations on a wall
        /// </summary>
        /// <param name="chunkPos">The chunk to build in</param>
        /// <param name="wallDir">Facing the wall</param>
        private void CreateDecorationsOnWall(IntVector2 chunkPos, Dir4 wallDir)
        {
            int wallToCenterDistance = WorldPosition.ChunkHalfWidth - innerWallThickness / 2;
            if (lib.IsOdd(innerWallThickness) || (wallDir == Dir4.S || wallDir == Dir4.E))
            {
                wallToCenterDistance -= 1;
            }
            int wallToWallDistance = (WorldPosition.ChunkWidth - innerWallThickness);

            // Create volume
            WorldVolume decorVol = new WorldVolume(
                new WorldPosition(chunkPos, WorldPosition.ChunkHalfWidth, floorY + decorationFloorDistance + 1, WorldPosition.ChunkHalfWidth),
                IntVector3.One);
            decorVol.AddPosition(wallDir, wallToCenterDistance);
            Dir4 left = lib.GetLateralLeftFacing(wallDir);
            decorVol.AddPosition(left, wallToWallDistance / 2);

            // Randomize decoration count.
            int decorationCount = MinWallDecorationCount;
            decorationCount += Data.RandomSeed.Instance.Next((MaxWallDecorationCount + 1) - MinWallDecorationCount);
            decorationCount = 1; // TEST TODO comment
            int decorationSpacing = (WorldPosition.ChunkWidth - innerWallThickness) / (decorationCount + 1);

            // Create all decorations
            for (int iDec = 0; iDec != decorationCount; ++iDec)
            {
                decorVol.AddPosition(lib.Invert(left), decorationSpacing);

                WallDecorationType decorType = (WallDecorationType)Data.RandomSeed.Instance.Next((int)WallDecorationType.NUM);
                decorType = WallDecorationType.Word; // TEST TODO comment
                while (decorType == WallDecorationType.Word && decorationCount > 1) // Only one word per wall
                    decorType = (WallDecorationType)Data.RandomSeed.Instance.Next((int)WallDecorationType.NUM);

                switch (decorType)
                {
                    case WallDecorationType.DrinkFountain:
                        // TODO
                        break;

                    case WallDecorationType.Word:
                        ArchitecturalLib.CreateWordPainting(wallDir, decorVol, detailMaterial, GetRandomWallMessage());
                        break;

                    case WallDecorationType.Statue:
                        // TODO
                        break;

                    case WallDecorationType.Torch:
                        // TODO
                        break;

                    case WallDecorationType.Window:
                        // TODO
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Creates ornaments for the wall - allure, parapet, merlons...
        /// </summary>
        /// <param name="wallDir">The facing</param>
        /// <param name="outerWall">The base wall volume</param>
        private void CreateOuterWallOrnaments(Dir4 wallDir, WorldVolume outerWall, Dir4 cutAwaySide, int cutAwayAmount)//Dir4 cutAwaySide = Dir4.NO_DIR, int cutAwayAmount = 0)
        {
            // Create allure and parapet
            WorldVolume wallCopy = outerWall;
            wallCopy.SetYPos(wallCopy.GetVerticalTop());
            if (cutAwaySide != Dir4.NUM_NON)
                wallCopy.SubFromSide(cutAwaySide, cutAwayAmount);
            wallCopy.SetVerticalHeight(1).Fill(detailMaterial); // allure
            if (cutAwaySide != Dir4.NUM_NON)
                wallCopy.AddToSide(cutAwaySide, cutAwayAmount);

            wallCopy = wallCopy.SubFromSide(lib.Invert(wallDir), OuterWallThickness - ParapetThickness).
                AddToSide(CubeFace.Ypositive, parapetHeight).AddToSide(CubeFace.Ynegative, 1);
            wallCopy.Fill(wallMaterial); // parapet
            
            // Create merlons
            // ##____####____####____####____## <- 32, merlon formation per chunk
            Dir4 right = lib.GetLateralRightFacing(wallDir);
            wallCopy.SubFromSide(right, WorldPosition.ChunkWidth - MerlonWidth / 2);
            wallCopy.AddToSide(CubeFace.Ypositive, merlonHeight).Fill(wallMaterial);
            wallCopy.AddToSide(right, MerlonWidth / 2);
            wallCopy.AddPosition(right, MerlonWidth / 2 + MerlonSpacing).Fill(wallMaterial);
            for (int i = 0; i != 2; ++i)
                wallCopy.AddPosition(right, MerlonWidth + MerlonSpacing).Fill(wallMaterial);
            wallCopy.AddPosition(right, MerlonWidth + MerlonSpacing).SubFromSide(right, MerlonWidth / 2).Fill(wallMaterial);
        }

        /// <summary>
        /// Creates the floor part under the entrance to the castle.
        /// </summary>
        /// <param name="chunkPos">What chunk</param>
        private void CreateFloorExtensionUnderEntrance(IntVector2 chunkPos)
        {
            WorldVolume floorExtension = new WorldVolume(
                new WorldPosition(chunkPos, WorldPosition.ChunkHalfWidth, floorY, OuterWallThickness),
                new IntVector3(0, 1, 0));
            Dir4 north = Dir4.N;
            floorExtension.AddToSide(north, 2 + OuterWallThickness / 2);
            floorExtension.AddToLateralEnds(north, FancyEntranceWidth / 2 + 8).Fill(floorMaterial1);
            for (int i = 0; i != 5; ++i)
            {
                floorExtension.AddToSide(north, 1);
                floorExtension.SubFromLateralEnds(north, 1).Fill(floorMaterial1);
            }
        }

        /// <summary>
        /// Creates the entrance carpet extension
        /// </summary>
        /// <param name="chunkPos">What chunk</param>
        private void GenerateCarpetExtensionAtEntrance(IntVector2 chunkPos)
        {
            int heightDiff = floorY - WorldPosition.ChunkStandardHeight - 1;
            WorldPosition stairsPos = new WorldPosition(chunkPos,
                WorldPosition.ChunkHalfWidth - halfCarpetWidth * 2, floorY + 1, 0);
            ArchitecturalLib.GenerateStairs(stairsPos, halfCarpetWidth * 2,
                heightDiff, Dir4.N, decorationMaterial, 7);
            stairsPos.WorldGrindex.AddSide(Dir4.E, carpetOutlineWidth * 2);
            ArchitecturalLib.GenerateStairs(stairsPos, halfCarpetWidth * 2 - carpetOutlineWidth * 2,
                heightDiff, Dir4.N, carpetMaterial, 6);
            stairsPos.WorldGrindex.AddSide(Dir4.N, 1);
            ArchitecturalLib.GenerateStairs(stairsPos, halfCarpetWidth * 2 - carpetOutlineWidth * 2,
                heightDiff, Dir4.N, carpetMaterial, 6);
        }

        /// <summary>
        /// Creates a chest in the middle of the given chunk.
        /// </summary>
        /// <param name="chunkPos">The chunk to build in</param>
        /// <param name="corner">The corner in which the chest is (to check for boss key generation)</param>
        private void CreateChest(IntVector2 chunkPos, Corner corner)
        {
            if (LfRef.gamestate.Progress.HasLootCrate(areaLevel, corner))
            {
                WorldPosition wp = new WorldPosition();
                wp.SetChunkAndLocalBlock(chunkPos, WorldPosition.ChunkHalfWidth, WorldPosition.ChunkStandardHeight, WorldPosition.ChunkHalfWidth);
                //GameObjects.EnvironmentObj.ChestData chestData = new GameObjects.EnvironmentObj.ChestData(wp, GameObjects.EnvironmentObj.MapChunkObjectType.Chest);

                new GameObjects.EnvironmentObj.LootCrate(this, corner, wp);
            
                //chestData.AddTreasure(areaLevel + 1);
            //if (corner == bossKeyCorner)
            //{
            //    //add boss key if correct corner
            //    chestData.SetBossKey(areaLevel);
            }
        }

        /// <summary>
        /// Might create a spawn, depending on the provided byte-percent chance
        /// </summary>
        /// <param name="chunkPos">the chunk to check in</param>
        /// <param name="spawnChance">a byte representing relative spawn chance (converted to percent, 255 = 100%)</param>
        private void CreateSpawnerChance(IntVector2 chunkPos, byte spawnChance) //= 120)
        {
            Data.RandomSeed.Instance.SetSeedPosition(112 + chunkPos.X * 3 + chunkPos.Y * 11);

            if (Data.RandomSeed.Instance.BytePercent(spawnChance))
            {
                new CastleEnemySpawner(areaLevel, chunkPos);
              
            }
        }


        /// <summary>
        /// Returns a list of syllables for naming a castle
        /// </summary>
        /// <returns>A list of all syllables available</returns>
        override protected List<string> nameLetters()
        {
            return new List<string>
            {
                "wu", "wa", "re","ra","ru","run","ran","rem","ta","tal","tam","tar", 
                "pa","par","pal","pan","pam","pe","pen","pes","pet",
                "de","den","del","dew","ga","gan","gas","gar","gom","gos","ge","ges","he","ha","ho",
                "la","las","lam","lan","lo","lot","le","les",
                "ze", "za", "zem", "zeh", "zech", "zet", "cha", "che", "chez", 
                "ka", "ke", "ku", "kap", "kaz", "ket", "kem", "kam",
                "gre", "hu", "hum", "hat", "hut", "hun", "hus", "hen"
            };
        }

        /// <summary>
        /// Returns a random wall message from the available in wallMessages
        /// </summary>
        /// <returns>A string of a random wall message</returns>
        private string GetRandomWallMessage()
        {
            int i = Data.RandomSeed.Instance.Next(wallMessages.Count);
            return wallMessages[i];
        }

        /// <summary>
        /// The icon on the minimap
        /// </summary>
        public override SpriteName MiniMapIcon { get { return SpriteName.IconCastle; } }
        /// <summary>
        /// The type of urban construction
        /// </summary>
        override public UrbanType Type { get { return UrbanType.Castle; } }
        /// <summary>
        /// If you can travel to this place
        /// </summary>
        override public bool TravelTo { get { return false || PlatformSettings.TravelEverywhere; } }
    }

    /// <summary>
    /// Spawns enemies
    /// </summary>
    class CastleEnemySpawner : IEnvObjGenerator
    {
        int areaLevel;

        public CastleEnemySpawner(int areaLevel, IntVector2 chunkPos)
        {
            this.areaLevel = areaLevel;
            LfRef.worldOverView.EnvironmentObjectQue.AddGeneratorRequest(new WaitingEnvObjGenerator(this, chunkPos));
        }

        public void GenerateGameObjects(Map.Chunk chunk, Map.WorldPosition chunkCenterPos, bool dataGenerated)
        {
            int enemyTypes = 3;
            if (areaLevel > 1)
            {
                enemyTypes = 4;
                if (areaLevel > 3)
                    enemyTypes = 5;
            }

            switch (Data.RandomSeed.Instance.Next(enemyTypes))
            {
                case 0:
                    LfRef.gamestate.GameObjCollection.AddGameObject(
                        new GameObjects.GameObjectType(GameObjects.Characters.CharacterUtype.Mummy, chunkCenterPos, areaLevel));
                    if (Data.RandomSeed.Instance.BytePercent((byte)(10 + 10 * areaLevel)))
                    {
                        LfRef.gamestate.GameObjCollection.AddGameObject(
                       new GameObjects.GameObjectType(GameObjects.Characters.CharacterUtype.Mummy, chunkCenterPos, areaLevel));
                    }
                    break;
                case 1:
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.GameObjectType(GameObjects.Characters.CharacterUtype.TrapRotating, chunkCenterPos, areaLevel));
                    break;
                case 2:
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.GameObjectType(GameObjects.Characters.CharacterUtype.ShootingTurret, chunkCenterPos, areaLevel));
                    break;
                case 3:
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.GameObjectType(GameObjects.Characters.CharacterUtype.TrapBackNforward, chunkCenterPos, areaLevel));
                    break;
                case 4:
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.GameObjectType(GameObjects.Characters.CharacterUtype.Ghost, chunkCenterPos, areaLevel));
                    break;
            }
        }
    }


    enum CastleStuff
    {
        InnerWallN,
        InnerWallW,
        InnerWallS,
        InnerWallE,
        TrapRotating,
        WallFireBallTrapN,
    }
}

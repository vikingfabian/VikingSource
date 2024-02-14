using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    /// <summary>
    /// A random-generated fort, holding monsters and some loot
    /// 
    /// Ideas for add-ons:
    /// - spawn monsters + a boss monster
    /// - spawn some chests with loot
    /// - bigger fortress = harder monsters, better loot
    /// - wall ladders
    /// - moat: really easy (Gief water Fabian)
    /// - tents
    /// - cooking pot on fire
    /// - maybe side quest items to give to city folk for reward
    /// - add icon (Gief fort icon Fabian)
    /// - add a well in the center!
    /// - type of fort depending on environment (sandstone in desert, wood in forest etc)
    /// - auxillary tower?
    /// 
    /// Here's a good list on castle terms to look up the words beneath in:
    /// http://www.allcrusades.com/CASTLES/GLOSSARY_OF_CASTLE_TERMS/glossary_of_castle_terms.html
    /// </summary>
    class Fort : AbsUrban
    {
        #region Constants
        // TODO unconst constants and random generate the fort when it gets closer to completion
        const int ChunksWide = 3; // must be odd

        // Walls and radius
        const int MinFortOuterRadius = MaxAllureWidth + MaxEntranceWidth / 2; // May not work if lower!
        const int MaxFortOuterRadius = WorldPosition.ChunkHalfWidth * ChunksWide - 1; // Won't work if higher!
        const int MinAllureWidth = 1;
        const int MaxAllureWidth = 7;
        const int MinParapetHeight = 0;
        const int MaxParapetHeight = 2;
        const int MinWallHeight = Players.Player.CharacterHeight + MaxParapetHeight + 2; // 1 for allure and 1 for above allure
        const int MaxWallHeight = 20;
        const int MinMerlonHeight = 1;
        const int MaxMerlonHeight = 6;

        // Entrance
        const int MinEntranceWidth = Players.Player.CharacterWidth;
        const int MinEntranceHeight = Players.Player.CharacterHeight;
        const int MaxEntranceWidth = 8;

        #endregion

        #region Members
        // Environmental
        private WorldPosition centralBlock;
        private int floorHeight;
        private int highestGroundY;
        private Terrain.EnvironmentType environment = EnvironmentType.NUM_NON;
        private Data.MaterialType wallMaterial;
        private Data.MaterialType floorMaterial;

        // Spawns
        private int difficultyLevel;

        // Entrance
        private int entranceWidth = MinEntranceWidth;
        private int entranceHeight = MinEntranceHeight;
        private int farEntranceDistance;
        private int nearEntranceDistance;
        private bool foundEntranceSpot = false;
        private WorldPosition entrancePos = new WorldPosition();

        // Walls
        private int fortOuterRadius = MinFortOuterRadius;
        private int allureWidth = MinAllureWidth;
        private int fortInnerRadius;
        private int wallHeight = MinWallHeight;
        private int parapetHeight = MinParapetHeight;
        private int allureHeight;
        private int merlonHeight = MinMerlonHeight;
        private static bool putMerlonToggle = false;

        #endregion

        /// <summary>
        /// Construct a fort
        /// </summary>
        /// <param name="position">Chunk position</param>
        /// <param name="level">The difficulty level on the monsters</param>
        public Fort(IntVector2 position, int level)
            : base(new IntVector2(ChunksWide), position, level)
        {
            // Find center for the circular fort
            position += ChunksWide / 2;
            centralBlock = new WorldPosition(position);
            centralBlock.CenterAtopOfChunk();

            // Start randomizing...
            Data.RandomSeedInstance randomizer = Data.RandomSeed.Instance;

            // Walls and radius
            fortOuterRadius += randomizer.Next(MaxFortOuterRadius - MinFortOuterRadius);
            allureWidth += randomizer.Next(MaxAllureWidth - MinAllureWidth);
            fortInnerRadius = fortOuterRadius - allureWidth;
            wallHeight += randomizer.Next(MaxWallHeight - MinWallHeight);
            parapetHeight += randomizer.Next(MaxParapetHeight - MinParapetHeight);
            allureHeight = wallHeight - parapetHeight;
            merlonHeight += randomizer.Next(MaxMerlonHeight - MinMerlonHeight);

            // Difficulty
            difficultyLevel = randomizer.Next(fortOuterRadius / 4);
            difficultyLevel += randomizer.Next(fortOuterRadius / 4);

            // Entrance
            entranceDir = (Dir4)randomizer.Next((int)Dir4.NUM_NON);
            entranceWidth += randomizer.Next(MaxEntranceWidth - MinEntranceWidth);
            entranceHeight += randomizer.Next((allureHeight - 1) - MinEntranceHeight); // Minus one for let there be allure above
            farEntranceDistance = (fortOuterRadius - WorldPosition.ChunkWidth) + Players.Player.CharacterWidth;
            nearEntranceDistance = (fortInnerRadius - WorldPosition.ChunkWidth) - Players.Player.CharacterWidth;

            // Height
            int quarterStandardHeight = WorldPosition.ChunkStandardHeight / 4;
            floorHeight = quarterStandardHeight;
            floorHeight += randomizer.Next(quarterStandardHeight * 6);

            // code below does not yet work because world is not generated when this runs...
            // therefore, just set standard value for now :D
            highestGroundY = WorldPosition.ChunkStandardHeight;
            // find highest Y pos on where walls and allure can be on all chunks
            //WorldPosition wp = new WorldPosition(position); // central chunk!
            //IntVector3 pos = IntVector3.Zero;
            //for (pos.Z = -WorldPosition.ChunkWidth * ((ChunksWide / 2) - 1); pos.Z < WorldPosition.ChunkWidth * (ChunksWide / 2); ++pos.Z)
            //{
            //    for (pos.X = -WorldPosition.ChunkWidth * ((ChunksWide / 2) - 1); pos.X < WorldPosition.ChunkWidth * (ChunksWide / 2); ++pos.X)
            //    {
            //        WorldPosition n = wp.GetNeighborPos(pos);
            //        int height = n.Screen.GetHighestYpos(n);
            //        if (height > highestGroundY)
            //            highestGroundY = height;
            //    }
            //}
            return;
        }

        /// <summary>
        /// Builds a fort on the given chunk
        /// </summary>
        /// <param name="chunkPos">The chunk position</param>
        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            base.BuildOnChunk(chunk, dataGenerated, gameObjects);

            if (gameObjects)
            {
                GenerateChunkGameObjects(chunk, dataGenerated);
            }
            else
            {
                // environmental, only checked once
                if (environment == EnvironmentType.NUM_NON)
                {
                    environment = LfRef.worldOverView.GetChunkData(chunk.Index).Environment;
                    SetFortMaterials(environment);
                }

                ChunkCreateFortBase(chunk.Index);
            }
                //if (chunkPos == centralBlock.ChunkGrindex)
            //    BeginGenerateEnvironmentObj(chunkPos, IntVector2.Zero); // Create chaos!
        }

        /// <summary>
        /// Generates a boss, some minions and a loot chest.
        /// Misspelled because of refactoring nightmare due to old base class :(
        /// </summary>
        /// <param name="chunkPos">What chunk</param>
        public void GenerateChunkGameObjects(Map.Chunk chunk, bool dataGenerated)
        {
            //base.GenerateGameObjects(chunkPos, dataGenerated);
            if (chunk.Index == this.position)
            {

                WorldPosition wp = new WorldPosition(chunk.Index);

                // Create a boss monster from diff level
                // diff depends on diff level
                List<GameObjects.Characters.Orc> group = new List<GameObjects.Characters.Orc>();
                LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.Characters.Orc(wp, difficultyLevel, GameObjects.Characters.HumanoidType.Leader, group));

                // Create minions
                // amount depends on diff level
                int numSpawns = Ref.rnd.Int(3);
                for (int i = 0; i < numSpawns; ++i)
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.Characters.Orc(wp, difficultyLevel / 2, GameObjects.Characters.HumanoidType.SwordsMan, group));
                numSpawns = Ref.rnd.Int(3);
                for (int i = 0; i < numSpawns; ++i)
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.Characters.Orc(wp, difficultyLevel / 2, GameObjects.Characters.HumanoidType.Archer, group));
                numSpawns = Ref.rnd.Int(1);
                for (int i = 0; i < numSpawns; ++i)
                    LfRef.gamestate.GameObjCollection.AddGameObject(new GameObjects.Characters.Orc(wp, difficultyLevel / 2, GameObjects.Characters.HumanoidType.Brute, group));

                wp.WorldGrindex.X += WorldPosition.ChunkHalfWidth;
            }
                // Create a loot chest
            //else if (dataGenerated && chunk.Index == this.AreaChunkCenter)
            //{
            //    GameObjects.EnvironmentObj.ChestData chestData = new GameObjects.EnvironmentObj.ChestData(centralBlock, GameObjects.EnvironmentObj.MapChunkObjectType.Chest);
            //    chestData.AddTreasure(difficultyLevel);
            //}
        }

        /// <summary>
        /// Switches on environment type to find matching materials for the fort
        /// </summary>
        /// <param name="type">environment type</param>
        private void SetFortMaterials(EnvironmentType type)
        {
            switch (type)
            {
                case EnvironmentType.Burned:
                    floorMaterial = Data.MaterialType.burnt_ground;
                    wallMaterial = Data.MaterialType.mossy_stone;
                    break;
                case EnvironmentType.Desert:
                    floorMaterial = Data.MaterialType.sand;
                    wallMaterial = Data.MaterialType.sand_stone_bricks;
                    break;
                case EnvironmentType.Forest:
                    floorMaterial = Data.MaterialType.dirt;
                    wallMaterial = Data.MaterialType.wood;
                    break;
                case EnvironmentType.Grassfield:
                    floorMaterial = Data.MaterialType.dirt;
                    wallMaterial = Data.MaterialType.stone;
                    break;
                case EnvironmentType.Mountains:
                    floorMaterial = Data.MaterialType.cobble_stone;
                    wallMaterial = Data.MaterialType.patterned_stone;
                    break;
                case EnvironmentType.Swamp:
                    floorMaterial = Data.MaterialType.mossy_stone;
                    wallMaterial = Data.MaterialType.black_wood;
                    break;
                default:
                    floorMaterial = Data.MaterialType.dirt;
                    wallMaterial = Data.MaterialType.stone;
                    break;
            }
        }

        /// <summary>
        /// Construct walls and plain out the inner area for the given chunk
        /// Also checks for if entrance creation should kick in and calls it
        /// </summary>
        /// <param name="chunkPos">The chunk position</param>
        private void ChunkCreateFortBase(IntVector2 chunkPos)
        {
            WorldPosition wp = new WorldPosition(chunkPos);
            IntVector3 pos = IntVector3.Zero;            

            for (pos.Z = 0; pos.Z < WorldPosition.ChunkWidth; ++pos.Z)
            {
                for (pos.X = 0; pos.X < WorldPosition.ChunkWidth; ++pos.X)
                {
                    WorldPosition n = wp.GetNeighborPos(pos);
                    n.SetFromGroundY(0);
                    int distanceToCenter = (int)(centralBlock.CalculateBlockDistance(n));

                    // Generate circular fort
                    if (distanceToCenter < fortInnerRadius)
                    {
                        // Generate floor
                        pos.Y = floorHeight;
                        n.Screen.FillDownwards(pos, floorMaterial);
                        n.Screen.CreateFlatFloor(pos, floorMaterial);
                    }
                    if (distanceToCenter >= fortInnerRadius &&
                        distanceToCenter < fortOuterRadius)
                    {
                        GenerateAllure(wallMaterial, n, highestGroundY);
                        if (distanceToCenter == fortInnerRadius &&
                            n.WorldGrindex.IsInFrontOf(entranceDir, centralBlock.WorldGrindex))
                        {
                            entrancePos = n;
                            foundEntranceSpot = true;
                        }
                    }
                    else if (distanceToCenter == fortOuterRadius)
                    {
                        GenerateWall(wallMaterial, n, highestGroundY);
                    }
                }
            }

            if (foundEntranceSpot)
            {
                Map.Terrain.Generation.ArchitecturalLib.CutArchedEntrance(entrancePos, entranceDir, allureWidth + 1,
                    entranceWidth, entranceHeight, floorMaterial, floorHeight,
                    WorldPosition.ChunkStandardHeight, true);
            }
        }

        /// <summary>
        /// Generate a single column of allure
        /// </summary>
        /// <param name="WallMaterial">Allure material</param>
        /// <param name="pos">World position</param>
        private void GenerateAllure(Data.MaterialType WallMaterial, WorldPosition pos, int groundHighestY)
        {
            int lowestY = Math.Min(groundHighestY, floorHeight);
            int actualHeight = Math.Max(groundHighestY, floorHeight) - lowestY + allureHeight;
            for (int y = 0; y < actualHeight; ++y)
            {
                pos.WorldGrindex.Y = lowestY + y;
                LfRef.chunks.Set(pos, (byte)WallMaterial);
            }
        }

        /// <summary>
        /// Generate a single column of fort wall
        /// </summary>
        /// <param name="WallMaterial">What material to use</param>
        /// <param name="pos">Where in the world to place it</param>
        private void GenerateWall(Data.MaterialType WallMaterial, WorldPosition pos, int groundHighestY)
        {
            int lowestY = Math.Min(groundHighestY, floorHeight);
            int actualHeight = Math.Max(groundHighestY, floorHeight) - lowestY + wallHeight;

            // Create battlements
            if (putMerlonToggle)
            {
                actualHeight += merlonHeight;
            }
            putMerlonToggle = !putMerlonToggle;

            for (int y = 0; y < actualHeight; ++y)
            {
                pos.WorldGrindex.Y = lowestY + y;
                LfRef.chunks.Set(pos, (byte)WallMaterial);
            }
        }

        /// <summary>
        /// Icon shown on in-game minimap
        /// 
        /// TODO ADD ICON
        /// </summary>
        public override SpriteName MiniMapIcon { get { return SpriteName.IconEnemyOutpost; } }

        /// <summary>
        /// The type of urban construction
        /// </summary>
        public override UrbanType Type { get { return UrbanType.Fort; } }

    }
}

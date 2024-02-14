using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;

namespace VikingEngine.LootFest.BlockMap.Level
{
    class Lootfest1 : AbsLevel
    {
        public Lootfest1()
            : base(LevelEnum.Lootfest1)
        {
            terrain = new AbsLevelTerrain[] { new NormalTerrain(this) };

            /*
             * Väldigt öppen labyrint
             * -by med granpa, healer, 
             * -2säljar utposter (fåglar, ammo, sword up, bow up, hp, 
             * -vägskyldar som visar en enkel karta
             * 1. Hitta rätt staty med hawks, den ger nyckel till boss 1
             * 2. Boss1 lämnar en karta som visar nyckelbäraren, som hittad ger boss2 nyckel
             * 3. Boss2 ger tre-split båge, det finns tre mål man måste träffa samtidigt för nyckel 3
             * -runt om boss1/2 finns tavlor man måste skjuta för att ta bort bossens sköldar, 
             * boss2 har dubbla tavlor efter varann som man skjuter igenom med lvl2 båge
             * boss3 har tavlor som man måste hoppa upp till
             */


            /*
             * Labyrint:
             * Delar upp kartan i en grid av punkter
             * En algorithm drar linjer mellan punkterna utan att möta gamla linjer
             * Vissa rutor fylls av berg/vatten
             */
        }


        protected override List<VoxelModelName> loadTerrainModelsList()
        {
            return new List<VoxelModelName>(NormalDefaultModels);
        }

        Rectangle2 usedArea;
        const int MapGridSquaresW = 8;

        protected override void generateMapAsynch()
        {
            var spawnMonsters = new SuggestedSpawns(new List<SpawnPointData>
            {
                new SpawnPointData( GameObjectType.Hog, 0, 20),
                new SpawnPointData( GameObjectType.Pitbull, 0, 20),
                new SpawnPointData( GameObjectType.SpitChick, 0, 20),
                new SpawnPointData( GameObjectType.Scorpion, 0, 10),
                new SpawnPointData( GameObjectType.Spider, 0, 5),
                new SpawnPointData( GameObjectType.PoisionSpider, 0, 5),
                new SpawnPointData( GameObjectType.SquigGreen, 0, 3),
                new SpawnPointData( GameObjectType.SquigHorned, 0, 3),
                new SpawnPointData( GameObjectType.SquigRed, 0, 3),
                new SpawnPointData( GameObjectType.GreenSlime, 0, 10),
                new SpawnPointData( GameObjectType.Beetle1, 0, 5),
            });

            //Prepare Maze
            const int TilesEdgeOffset = 10;
            const int FullMapChunkWidth = 64;

            int mapWidth = FullMapChunkWidth * BlockMapLib.SquaresPerChunkW;

            int mapGridSquares = (mapWidth - TilesEdgeOffset * 2) / MapGridSquaresW;

            usedArea = new Rectangle2(new IntVector2(TilesEdgeOffset), new IntVector2(mapGridSquares * MapGridSquaresW));

            int mapGridDots = mapGridSquares + 1;

            Grid2D<MazeGridDot> gridDots = new Grid2D<MazeGridDot>(mapGridDots);
            gridDots.LoopBegin();
            while (gridDots.LoopNext())
            {
                gridDots.LoopValueSet(new MazeGridDot());
            }

            placeOpenArea(usedArea, 0.05);

            //ForXYLoop loop = new ForXYLoop(usedArea);
            //BlockMapSquare open = new BlockMapSquare();
            //open.type = MapBlockType.Open;

            //BlockMapSquare terrain =  new BlockMapSquare();
            //terrain.type = MapBlockType.Open;
            //terrain.special = MapBlockSpecialType.TerrainModel;

            //while (loop.Next())
            //{
            //    if (rnd.RandomChance(0.05))
            //    {
            //        squares.Set(loop.Position, terrain);
            //    }
            //    else
            //    {
            //        squares.Set(loop.Position, open);
            //    }
            //}

            //Place special areas
            //-Place water and mountain areas 

            int seaAndMoutainAreas = rnd.Int(6, 8);
            Range seaAndMoutainAreaSizeRange = new Range(2, 4);
            for (int i = 0; i < seaAndMoutainAreas; ++i)
            {
                IntVector2 sz = new IntVector2(seaAndMoutainAreaSizeRange.GetRandom(rnd), seaAndMoutainAreaSizeRange.GetRandom(rnd));

                IntVector2 placementArea = gridDots.Size - sz;

                var pos = placeSpecialArea(sz, ref gridDots);

                BlockMapSquare fillTerrain = new BlockMapSquare();

                if (rnd.Chance(0.6))
                {
                    fillTerrain.type = MapBlockType.Water;
                }
                else
                {
                    fillTerrain.type = MapBlockType.Occupied;
                }

                

                Rectangle2 sqArea = new Rectangle2(mapGridPosToSquarePos(pos), mapGridSzToSquareSz(sz));
                ForXYLoop fillLoop = new ForXYLoop(sqArea);
                while (fillLoop.Next())
                {
                    squares.Set(fillLoop.Position, fillTerrain);
                }
            }


            //Place outer wall
            ForXYEdgeLoop setEdgesLoop = new ForXYEdgeLoop(gridDots.Size);
            while (setEdgesLoop.Next())
            {
                gridDots.Get(setEdgesLoop.Position).wall = true;
            }


            //Generate Maze walls
            const int WallStrokes = 150;

            for (int i = 0; i < WallStrokes; ++i)
            {
                IntVector2 startPos = rnd.IntVector2_Tile(gridDots.Size);

                createWallStroke(startPos, rnd.Dir4(), 0, ref gridDots);
            }


            //Place terrain models and spawns
            squares.LoopBegin();
            while (squares.LoopNext())
            {
                var sq = squares.LoopValueGet();
                if (sq.type == MapBlockType.Open)
                {
                    if (sq.special == MapBlockSpecialType.TerrainModel)
                    {
                        addTerrainModel(squares.LoopPosition, NormalDefaultModels, rnd);
                    }
                    else if (rnd.Chance(0.05))
                    {
                        placeMonsterSpawn(squares.LoopPosition, spawnMonsters);
                    }
                }
            }

            levelEntrance = toWorldXZ(usedArea.pos);

        }

        

        IntVector2 placeSpecialArea(IntVector2 dotGridSz, ref Grid2D<MazeGridDot> gridDots)
        {
            IntVector2 placementArea = gridDots.Size - dotGridSz;

            int loop = 0;
            while (loop++ < 10000)
            {
                IntVector2 tryPos = rnd.IntVector2_Tile(placementArea);

                bool availableArea = true;
                ForXYLoop checkLoop = new ForXYLoop(tryPos, tryPos + dotGridSz);
                while (checkLoop.Next())
                {
                    if (gridDots.Get(checkLoop.Position).usedBySpecialArea)
                    {
                        availableArea = false;
                        break;
                    }
                }

                if (availableArea)
                {
                    //Fill as occupied
                    checkLoop.Reset();
                    while (checkLoop.Next())
                    {
                        var dot = gridDots.Get(checkLoop.Position);
                        dot.usedBySpecialArea = true;
                        dot.wall = true;
                    }

                    //COMPLETE
                    return tryPos;
                }
            }

            throw new EndlessLoopException("placeSpecialArea");
        }

        void createWallStroke(IntVector2 fromPos, Dir4 prevDir, int currentWallLength, ref Grid2D<MazeGridDot> gridDots)
        {
            int dirTrials = rnd.Int(1, 3);

            for (int i = 0; i < dirTrials; ++i)
            {
                Dir4 dir;
                if (rnd.Chance(0.4))
                {
                    dir = prevDir;
                }
                else
                {
                    dir = rnd.Dir4();
                }

                IntVector2 toPos = fromPos + IntVector2.FromDir4(dir);

                if (gridDots.InBounds(toPos) && !gridDots.Get(toPos).wall)
                {
                    //Can draw line
                    gridDots.Get(fromPos).wall = true;
                    gridDots.Get(toPos).wall = true;

                    generateWall(fromPos, toPos);

                    if (currentWallLength < 10 && rnd.Chance(0.9))
                    {
                        //Continue wall
                        createWallStroke(toPos, dir, currentWallLength + 1, ref gridDots);
                    }

                }
            }
        }

        void generateWall(IntVector2 fromPos, IntVector2 toPos)
        {
            BlockMapSquare closed = new BlockMapSquare();

            ForXYLoop loop = new ForXYLoop(Rectangle2.FromTwoPoints(mapGridPosToSquarePos(fromPos), mapGridPosToSquarePos(toPos)));
            while (loop.Next())
            {
                squares.Set(loop.Position, closed);
            }
        }

        IntVector2 mapGridPosToSquarePos(IntVector2 mapGridPos)
        {
            IntVector2 sqPos = usedArea.pos + mapGridPos * MapGridSquaresW;
            return sqPos;
        }
        IntVector2 mapGridSzToSquareSz(IntVector2 mapGridSz)
        {
            return mapGridSz * MapGridSquaresW;
        }

        class MazeGridDot
        {
            public bool wall = false;
            public bool usedBySpecialArea = false;
        }
    }
}

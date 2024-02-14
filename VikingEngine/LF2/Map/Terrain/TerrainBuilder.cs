using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Map.Terrain
{
    //måste ta bort allt

//    class TerrainBuilder
//    {
//        public static DataChunks.Areas AreaData;

//        const int NumWallThreads = 60;
//        public static List<IntVector2> BossCasles;
//        public static List<IntVector2> Villages;
//        //public static IntVector2 FinalBossLocation;

//        public static IntVector2[,] environmentStartPoses;
//        public static EnvironmentType[,] EnvironmentOverViewGrid;
//        Area[,] areas = new Area[WorldPosition.AreasPerWorldX, WorldPosition.AreasPerWorldY];
//        bool[,] usedCorner = new bool[WorldPosition.AreasPerWorldX + 1, WorldPosition.AreasPerWorldY + 1];
//        IntVector2 spawnPos;
        
//        int numSpawnConnectionLoops = 0;

//        public TerrainBuilder()
//        {
//            System.Diagnostics.Debug.WriteLine("Terrainbuilder Start");
//            #region Environment
//            //I will divide the area into different environments
//            //Use 4 of 5 environments
//            List<EnvironmentType> evilenvironmentsInUse = new List<EnvironmentType> 
//                { EnvironmentType.Burned, EnvironmentType.Desert, EnvironmentType.Swamp };
//            //environmentsInUse.Remove(environmentsInUse[Data.RandomSeed.Instance.Next(environmentsInUse.Count)]);

//            List<EnvironmentType> placeOrder = new List<EnvironmentType> { EnvironmentType.Grassfield };
//            //random add before or after
//            placeOrder.Insert(Data.RandomSeed.Instance.Next(1), EnvironmentType.Forrest);
//            //put the bad environments in order
//            List<EnvironmentType> evilenvironments = new List<EnvironmentType>();
//            int addEvilenvironment = Data.RandomSeed.Instance.Next(evilenvironmentsInUse.Count);
//            evilenvironments.Add(evilenvironmentsInUse[addEvilenvironment]);
//            evilenvironmentsInUse.Remove(evilenvironmentsInUse[addEvilenvironment]);
//            addEvilenvironment = Data.RandomSeed.Instance.Next(evilenvironmentsInUse.Count);
//            evilenvironments.Add(evilenvironmentsInUse[addEvilenvironment]);
//            //insert the bad areas
//            bool startOnTopLeftSide = Data.RandomSeed.Bool();
//            placeOrder.InsertRange(startOnTopLeftSide ? 2 : 0, evilenvironments);

//            const int EnvironmentGridWidth = 2;
//            EnvironmentOverViewGrid = new EnvironmentType[EnvironmentGridWidth, EnvironmentGridWidth];
//            bool splitvertically = Data.RandomSeed.Bool();
//            int index = 0;
//            for (int y = 0; y < EnvironmentGridWidth; y++)
//            {
//                for (int x = 0; x < EnvironmentGridWidth; x++)
//                {
//                    if (splitvertically)
//                        EnvironmentOverViewGrid[y, x] = placeOrder[index];
//                    else
//                        EnvironmentOverViewGrid[x, y] = placeOrder[index];
//                    index++;
//                }
//            }
//            //determen the environments sizes
//            environmentStartPoses = new IntVector2[EnvironmentGridWidth, EnvironmentGridWidth];
//            IntVector2 startSize = new IntVector2(WorldPosition.AreasPerWorldX / 2, WorldPosition.AreasPerWorldY / 2);
//            Range envSizeDiff = new Range(-2, 2);
//            int xdiff = Data.RandomSeed.Instance.Next(envSizeDiff);
//            int ydiff = Data.RandomSeed.Instance.Next(envSizeDiff);

//            for (int y = 0; y < EnvironmentGridWidth; y++)
//            {
//                for (int x = 0; x < EnvironmentGridWidth; x++)
//                {
//                    environmentStartPoses[x, y] = new IntVector2(startSize.X * x, startSize.Y * y);
//                    if (x > 0)
//                    {
//                        environmentStartPoses[x, y].X += xdiff;
//                    }
//                    if (y > 0)
//                    {
//                        environmentStartPoses[x, y].Y += ydiff;
//                    }

//                }
//            }
//            #endregion
//            #region AreaTypesAndPaths
//            //Create Areas and give them environment types
//            List<IntVector2> emptyAreas = new List<IntVector2>();
//            List<IntVector2> emptyFriendlyAreas = new List<IntVector2>();
//            List<IntVector2> emptyEvilAreas = new List<IntVector2>();
//            IntVector2 envGridIx = IntVector2.Zero;
//            for (envGridIx.Y = 0; envGridIx.Y < EnvironmentGridWidth; envGridIx.Y++)
//            {
                
//                for (envGridIx.X = 0; envGridIx.X < EnvironmentGridWidth; envGridIx.X++)
//                {
//                    EnvironmentType envType = EnvironmentOverViewGrid[envGridIx.X, envGridIx.Y];
//                    bool friendlyEnv = envType == EnvironmentType.Forrest || envType == EnvironmentType.Grassfield;
//                    IntVector2 start = environmentStartPoses[envGridIx.X, envGridIx.Y];
//                    IntVector2 size = IntVector2.Zero;
//                    if (envGridIx.X == 0)
//                    {
//                        size.X = environmentStartPoses[envGridIx.X + 1, envGridIx.Y].X;
//                    }
//                    else
//                    {
//                        size.X = WorldPosition.AreasPerWorldX - environmentStartPoses[envGridIx.X, envGridIx.Y].X;
//                    }
//                    if (envGridIx.Y == 0)
//                    {
//                        size.Y = environmentStartPoses[envGridIx.X, envGridIx.Y + 1].Y;
//                    }
//                    else
//                    {
//                        size.Y = WorldPosition.AreasPerWorldY - environmentStartPoses[envGridIx.X, envGridIx.Y].Y;
//                    }

//                    IntVector2 areaPos = IntVector2.Zero;
//                    for (int areaY = 0; areaY < size.Y; areaY++)
//                    {
//                        bool endPosY = (envGridIx.Y == 0 && areaY == 0) || 
//                            (envGridIx.Y == (EnvironmentGridWidth - 1) && areaY == (size.Y -1));
                    
                        
                    
//                        areaPos.Y = areaY + start.Y;
//                        for (int areaX = 0; areaX < size.X; areaX++)
//                        {
//                            bool endPosX = (envGridIx.X == 0 && areaX == 0) || 
//                                (envGridIx.X == (EnvironmentGridWidth - 1) && areaX == (size.X -1));

//                            areaPos.X = areaX + start.X;
//                            Area area = new Area();
//                            area.GridIndex = areaPos;
//                            area.EnvironmentGridIndex = envGridIx;
                            
//                            if (endPosX || endPosY)
//                            {
//                                //add sourronding mountains and sea
//                                //area.Type = envGridIx.X == 0 && endPosY ? AreaType.Mountain : AreaType.Sea;
//                                System.Diagnostics.Debug.WriteLine("Block Edge area" + areaPos.ToString());
//                            }
//                            else
//                            { 
//                                emptyAreas.Add(areaPos);
//                                if (friendlyEnv)
//                                    emptyFriendlyAreas.Add(areaPos);
//                                else
//                                    emptyEvilAreas.Add(areaPos);

//                            }
//                            areas[areaPos.X, areaPos.Y] = area;

//                        }
//                    }
                        
//                }
//            }


//            System.Diagnostics.Debug.WriteLine("Set Spawn");
//            //SPAWN
//            //Determen AreaTypes
//            //First pick the spawnPos and the end boss casle
//            const int SpawnPos = 0;
//            int numVillages = 3;
//            int numCasles = 3;

//            spawnPos = addCasleOrVillage(false, SpawnPos, emptyFriendlyAreas);
//            World.SpawnPos = new Vector2(
//                (spawnPos.X + PublicConstants.HALF) * WorldPosition.SquaresPerArea,
//                (spawnPos.Y + PublicConstants.HALF) * WorldPosition.SquaresPerArea);
 
//            emptyFriendlyAreas.Remove(spawnPos);
//            emptyAreas.Remove(spawnPos);
//            areas[spawnPos.X, spawnPos.Y].HaveSpawnContact = true;
//            //open all dirs around spawn
//            //FINAL BOSS
//            IntVector2 finalBossPos = addCasleOrVillage(true, numCasles - 1, emptyEvilAreas);
//            occupyAreaCorners(finalBossPos);
//            emptyEvilAreas.Remove(finalBossPos);
//            emptyAreas.Remove(finalBossPos);
//            //add to hawk
//            BossCasles = new List<IntVector2>(); 

//            //The rest of the VILLAGES
//            Villages = new List<IntVector2> { spawnPos };
//            System.Diagnostics.Debug.WriteLine("Villages");
//            for (int i = 1; i < numVillages; i++)
//            {
//                Villages.Add(addCasleOrVillage(false, i, emptyAreas));
//            }
//            //The rest of the CASLES
//            System.Diagnostics.Debug.WriteLine("Casles");
//            IntVector2 bossPos = IntVector2.Zero;
//            for (int i = 0; i < numCasles - 1; i++)
//            {
//                bossPos = addCasleOrVillage(true, i, emptyAreas);
//                BossCasles.Add(bossPos);
//                emptyAreas.Remove(bossPos);
//                //add casle garden
//                List<IntVector2> sourroundingCasle = GetSourrondingAreas(bossPos, false);    
//                IntVector2 selPos = sourroundingCasle[Data.RandomSeed.Instance.Next(sourroundingCasle.Count)];
//                areas[selPos.X, selPos.Y].Type = AreaType.CasleGarden;
//                //create the opening
//                selPos.X -= bossPos.X;
//                selPos.Y -= bossPos.Y;
//                Dir4 pathDir = selPos.ToDir4();
//                areas[bossPos.X, bossPos.Y].opening = pathDir;
//                setPath(bossPos, pathDir, true);
//                //close all other
//                List<Dir4> closeTHese = ListAll4Dirs();
//                closeTHese.Remove(pathDir);
//                setPaths(bossPos, closeTHese, false);

//                occupyAreaCorners(bossPos);
//            }
//            BossCasles.Add(finalBossPos);
//            //Mountains
//            //int numMountains = 1;//Data.RandomSeed.Instance.Next(new Range(1, 2));
//            //for (int i = 0; i < numMountains; i++)
//            //{
//            //    occupyAreaCorners(setEmptyArea(ref emptyAreas, AreaType.Mountain));
//            //}
//            //Sea
//            int numWaterAreas = Data.RandomSeed.Instance.Next(new Range(1, 2));
//            for (int i = 0; i < numWaterAreas - 1; i++)
//            {
//                occupyAreaCorners(setEmptyArea(ref emptyAreas, AreaType.Sea));
//            }
//            //Enemy outposts
//            int numOutposts = Data.RandomSeed.Instance.Next(new Range(2, 4));
//            System.Diagnostics.Debug.WriteLine("Labyrinth");
//            /* --LABYRINTH--
//             * Varje area hörn håller reda på om den har blivit använd
//             * Påbörja muren på ett slupmässigt hörn
//             * Muren går slumpmässigt i alla väderstrecken, men bara dit dom nya hörnen är oanvända
//             * Blockerande ytor måste placeras först, som vatten
//             */
//            for (int x = 0; x <= WorldPosition.AreasPerWorldX; x++)
//            {
//                usedCorner[x, 0] = true;
//                usedCorner[x, WorldPosition.AreasPerWorldY] = true;
//            }
//            for (int y = 0; y <= WorldPosition.AreasPerWorldY; y++)
//            {
//                usedCorner[0, y] = true;
//                usedCorner[WorldPosition.AreasPerWorldX, y] = true;
//            }
            
//            for (int i = 0; i < NumWallThreads; i++)
//            {
//                IntVector2 randomPos = new IntVector2(Data.RandomSeed.Instance.Next(WorldPosition.AreasPerWorldX), Data.RandomSeed.Instance.Next(WorldPosition.AreasPerWorldY));
//                if (!usedCorner[randomPos.X, randomPos.Y])
//                    startWall(randomPos);
//            }
           

//            #endregion


//            DataChunks.AreaData[,] areaDataGrid = new DataChunks.AreaData[WorldPosition.AreasPerWorldX, WorldPosition.AreasPerWorldY];
//            //draw up som test walls and basic ground
//            //(öppna skärmarna för varje rad och stäng dom efter sig
//            //eller bara stäng dom
//            System.Diagnostics.Debug.WriteLine("BuildAreas");
//            for (finalBossPos.Y = 0; finalBossPos.Y < WorldPosition.AreasPerWorldY; finalBossPos.Y++)
//            {
//                for (finalBossPos.X = 0; finalBossPos.X < WorldPosition.AreasPerWorldX; finalBossPos.X++)
//                {
//                    areas[finalBossPos.X, finalBossPos.Y].BuildArea();
//                    areaDataGrid[finalBossPos.X, finalBossPos.Y] = areas[finalBossPos.X, finalBossPos.Y].GetAreaData();
//                }

//                //compress the two old rows of screens
//                if (finalBossPos.Y > 0)
//                {
//                    WorldPosition wp = new WorldPosition();
//                    wp.ScreenY = (finalBossPos.Y - 1) * WorldPosition.AreaScreensWidth;
//                    for (int i = 0; i < WorldPosition.AreaScreensWidth; i++)
//                    {
//                        for (wp.ScreenIndex.X = 0; wp.ScreenIndex.X < WorldPosition.ScreensPerWorldX; wp.ScreenIndex.X++)
//                        {
//                            LfRef.chunks.GetScreen(wp).CompressData(true);
//                        }
//                        wp.ScreenIndex.Y++;
//                    }
//                }
//            }

//            AreaData = new DataChunks.Areas(areaDataGrid);

        
//        }
//        void occupyAreaCorners(IntVector2 pos)
//        {
//            for (int y = 0; y < 2; y++)
//            {
//                for (int x = 0; x < 2; x++)
//                {
//                    usedCorner[pos.X + x, pos.Y + y] = true;
//                }
//            }
//        }
//        void startWall(IntVector2 pos)
//        {
//            const byte WallChance = 170;
//            for (Dir4 dir = (Dir4)0; dir < Dir4.NUM; dir++)
//            {
//                if (Data.RandomSeed.Instance.BytePercent(WallChance))
//                {
//                    //build wall
//                    //check first
//                    IntVector2 toPos = pos;
//                    toPos.Add(IntVector2.FromDir4(dir));
//                    if (toPos.X >= 0 && toPos.Y >= 0 &&
//                        toPos.X < WorldPosition.AreasPerWorldX && toPos.Y < WorldPosition.AreasPerWorldY)
//                    {//inside safe area
//                        if (!usedCorner[toPos.X, toPos.Y])
//                        {
//                            usedCorner[toPos.X, toPos.Y] = true;
//                            startWall(toPos);
//                            setPath(toPos, dir, false);
//                        }
//                    }
//                }
//            }
//        }
        

//        List<Dir4> ListAll4Dirs()
//        {
//            return new List<Dir4> { Dir4.N, Dir4.E, Dir4.S, Dir4.W };
//        }
//        bool getPath(IntVector2 areaPos, Dir4 dir)
//        {
//            bool northSouth = dir == Dir4.N;
//            if (dir == Dir4.E)
//            {
//                areaPos.X++;
//            }
//            else if (dir == Dir4.S)
//            {
//                areaPos.Y++;
//                northSouth = true;
//            }
//            if (correctAreaPos(areaPos))
//            {
//                if (northSouth)
//                   return !areas[areaPos.X, areaPos.Y].WallN;
//                else
//                   return !areas[areaPos.X, areaPos.Y].WallW;
//            }
//            return false;
//        }
//        void setPaths(IntVector2 areaPos, List<Dir4> dirs, bool open)
//        {
//            foreach (Dir4 dir in dirs)
//            {
//                setPath(areaPos, dir, open);
//            }
//        }
//        void setPath(IntVector2 areaPos, Dir4 dir, bool open)
//        {
//            bool northSouth = dir == Dir4.N;
//            if (dir == Dir4.E)
//            {
//                areaPos.X++;
//            }
//            else if (dir == Dir4.S)
//            {
//                areaPos.Y++;
//                northSouth = true;
//            }
//            if (correctAreaPos(areaPos))
//            {
//                if (northSouth)
//                    areas[areaPos.X, areaPos.Y].WallN = !open;
//                else
//                    areas[areaPos.X, areaPos.Y].WallW = !open;
//            }
//        }
//        bool correctAreaPos(IntVector2 pos)
//        {
//            return pos.X >= 0 && pos.X < WorldPosition.AreasPerWorldX &&
//                pos.Y >= 0 && pos.Y < WorldPosition.AreasPerWorldY;
//        }
//        IntVector2 setEmptyArea(ref List<IntVector2> emptyAreas, AreaType toType)
//        {
//            IntVector2 pos = emptyAreas[Data.RandomSeed.Instance.Next(emptyAreas.Count)];
//            areas[pos.X, pos.Y].Type = toType;
//            emptyAreas.Remove(pos);
//            return pos;
//        }

//        IntVector2 addCasleOrVillage(bool casle, int index, List<IntVector2> emptyAreas)
//        {
//            bool foundPos = false;
//            IntVector2 randomPos = IntVector2.Zero;
//            int numLoops = 0;
//            do 
//            {
//                numLoops++;
//                if (numLoops > 100)
//                {
//#if WINDOWS
//                    throw new Debug.EndlessLoopException("addCasleOrVillage");
//#endif
//                    return randomPos;
//                }
//                int pickIndex = Data.RandomSeed.Instance.Next(emptyAreas.Count);
//                randomPos = emptyAreas[pickIndex];
//                if (areas[randomPos.X, randomPos.Y].Type == AreaType.Empty)
//                {
//                    //check around to see if it is to close to any other casles or villages
//                    List<IntVector2> checkThese = GetSourrondingAreas(randomPos, true);
//                    bool allEmpty = true;
//                    foreach (IntVector2 pos in checkThese)
//                    {
//                        if (areas[pos.X, pos.Y].Type == AreaType.Casle ||
//                            areas[pos.X, pos.Y].Type == AreaType.Village)
//                        {
//                            allEmpty = false;
//                            break;
//                        }
//                    }
//                    if (allEmpty)
//                    {
//                        foundPos = true;
//                        areas[randomPos.X, randomPos.Y].Type = casle ? AreaType.Casle : AreaType.Village;
//                        foreach (IntVector2 pos in checkThese)
//                        {
//                            areas[pos.X, pos.Y].Type = AreaType.CloseToVillageOrCasle;
//                        }
//                    }
//                }
//            } while (!foundPos);

//            return randomPos;
//        }
//        List<IntVector2> GetSourrondingAreas(IntVector2 pos, bool dialonally)
//        {
//            List<IntVector2> result = new List<IntVector2>();
//            IntVector2 checkPos = IntVector2.Zero;
//            for (int y = -1; y <= 1; y++)
//            {
//                checkPos.Y = pos.Y + y;
//                if (checkPos.Y >= 0 && checkPos.Y < WorldPosition.AreasPerWorldY)
//                {
//                    for (int x = -1; x <= 1; x++)
//                    {
//                        if (x != 0 || y != 0)
//                        {
//                            if (dialonally || (x == 0 || y == 0))
//                            {
//                                checkPos.X = pos.X + x;
//                                if (checkPos.X >= 0 && checkPos.X < WorldPosition.AreasPerWorldX)
//                                {
//                                    result.Add(checkPos);
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return result;
//        }

//    }

    //struct AreaCorner
    //{

    //}
    enum CornerType
    {
        Empty = 0,
        Reserved,
        Filled,
    }

    struct AreaWall
    {
        bool Used;


    }

    

}

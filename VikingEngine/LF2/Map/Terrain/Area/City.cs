using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;
using VikingEngine.LF2.Map.Terrain.Generation;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    /// <summary>
    /// Large friendly urbanization with a small wall around it
    /// 
    /// TODO: in order of priority, big endian
    /// Build Town hall + decorations and roof
    /// Build Church + decorations and roof
    /// Build Bank + decorations and roof
    /// New house design
    /// Randomisation on as many parameters as possible.
    /// 
    /// MAYBE:
    /// Two story houses
    /// Walls vary between 2 differently structured versions: stone / wood
    /// Lanterns
    /// </summary>
    class City : AbsUrban
    {
        #region Constants
        // Old stuff
        static readonly Range NumWorkerNPCs = new Range(2, 3);

        // Town hall constants.
        const int TownHallWallHeight = 15;
        const int TownHallWallThickness = 1;
        const int WallOffset = 1; // TODO randomise (However, changing this requires changing how houses are built)
        const int TownHallEntranceHeight = 10;

        // Church constants.
        const int ChurchWallHeight = 13;
        const int ChurchWallThickness = 1;
        const int ChurchEntranceHeight = 8;
        const string ChurchSign = "Praise the loot";

        // Bank constants.
        const int BankWallHeight = 12;
        const int BankWallThickness = 1;
        const int BankEntranceHeight = 7;
        const string BankSign = "Bank";

        // Spawn constants.
        const byte Granpa = 64;
        const byte WarVeteran = 128;

        // Cottage constants.
        const int MinWheatHeight = 1;
        const int MaxWheatHeight = 2;
        const int FenceEdgeOffset = 3;
        const int SoilDepth = 3;
        const int ClearAbove = 5;
        const int FenceWidth = 1;
        const int FencePostHeight = 3;
        const int FenceFloatHeight = 1;
        const int FenceHeight = 1;
        #endregion

        #region Members
        // Road
        int roadHalfWidth = 6; // TODO randomise
        int roadOutlineWidth = 1; // TODO randomise?
        Data.MaterialType roadOutlineMaterial = Data.MaterialType.marble;

        // Walls
        Data.MaterialType wallMaterial = Data.MaterialType.stone;
        Data.MaterialType allureMaterial = Data.MaterialType.wood;
        int parapetThickness = 1; // TODO randomise?
        int parapetHeight = 1; // TODO randomise?
        int merlonHeight = 2; // TODO randomise/
        int halfWallThickness = 3; // TODO randomise?
        int wallHeight = 12; // TODO randomise?

        // Towers
        Data.MaterialType towerFloorMaterial = Data.MaterialType.wood;
        int towerThickness = 2; // TODO randomise?
        int towerHeight = 18; // TODO randomise?
        int towerEntranceWidth = Players.Player.CharacterWidth + 2; // TODO randomise?

        // Town hall members.
        Dictionary<IntVector2, GameObjects.EnvironmentObj.MapChunkObjectType> citySquareObjectDic = new Dictionary<IntVector2, GameObjects.EnvironmentObj.MapChunkObjectType>();
        Data.MaterialType townHallMaterial = Data.MaterialType.marble;
        Data.MaterialType townHallDecorationMaterial = Data.MaterialType.stone;
        Data.MaterialType townHallFloorMaterial = Data.MaterialType.wood;

        // Church members.
        Data.MaterialType churchDecorationMaterial = Data.MaterialType.gold;
        Data.MaterialType churchFloorMaterial = Data.MaterialType.stone;
        Data.MaterialType churchMaterial = Data.MaterialType.wood;

        // Bank members.
        Data.MaterialType bankDecorationMaterial = Data.MaterialType.wood;
        Data.MaterialType bankFloorMaterial = Data.MaterialType.gold;
        Data.MaterialType bankMaterial = Data.MaterialType.gray_bricks;

        // NPCs
        
        bool createGranpa = false;
        bool createWarVeteran = false;

        // Cottage members.
        int wheatHeight;
        Data.MaterialType FenceMaterial = Data.MaterialType.wood;

        // Old stuff
        List<Data.MaterialType> wallMaterials;
        List<Data.MaterialType> wall2Materials;
        List<Data.MaterialType> roofMaterials;
        List<Data.MaterialType> frameMaterials;
        AlgoObjects.HouseSettings houseSettings;
        #endregion

        public City(IntVector2 position, int level, List<Data.Characters.QuestTip> tips, List<GameObjects.EnvironmentObj.MapChunkObjectType> allCitiesWorkerDistribution)
            : base(new IntVector2(7), position, level)
        {
            List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs = new List<GameObjects.EnvironmentObj.MapChunkObjectType>
                {
                    GameObjects.EnvironmentObj.MapChunkObjectType.Salesman,
                    GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport,
                    GameObjects.EnvironmentObj.MapChunkObjectType.Cook,
                };
            List<GameObjects.EnvironmentObj.MapChunkObjectType> selectedCenterSquareWorkers;
            // Randomise which NPCs to spawn.
            InitNPCParams(npcs, allCitiesWorkerDistribution, out selectedCenterSquareWorkers);

            // Randomise materials. OLD
            var villageDoorMaterials = InitMaterialParams(); // Refactor method after general cleanup

            // Keep house settings? OLD
            #region Look over later
            houseSettings = new AlgoObjects.HouseSettings();
            houseSettings.Height = Data.RandomSeed.Instance.GetRandomRange(new Range(6, 7), new Range(0, 2));
            houseSettings.Length = Data.RandomSeed.Instance.GetRandomRange(new Range(10, 14), new Range(0, 2));//new Range(12, 32);
            houseSettings.Width = Data.RandomSeed.Instance.GetRandomRange(new Range(8, 12), new Range(0, 4));//new Range(10, 30);
            houseSettings.RoofHeight = Data.RandomSeed.Instance.GetRandomRange(new Range(2, 4), new Range(0, 4));//new Range(2, 8);

            houseSettings.windowSize.X = 1 + Data.RandomSeed.Instance.Next(1);
            houseSettings.windowSize.Y = houseSettings.windowSize.X + Data.RandomSeed.Instance.Next(1);
            houseSettings.WindowPercentYpos = 0.4f + Data.RandomSeed.Instance.Next(2) * 0.1f;

            houseSettings.DoorMaterial = villageDoorMaterials[Data.RandomSeed.Instance.Next(villageDoorMaterials.Count)];
            if (Data.RandomSeed.Instance.BytePercent(100))
            {
                houseSettings.DoorFrameSize = 1;
            }

            houseSettings.Village = true;
            #endregion

            // Initialise city foundation.
            entranceDir = (Dir4)Data.RandomSeed.Instance.Next(2);

            // Designate walls.
            int right = squareSize.X - 1;
            int bottom = squareSize.Y - 1;

            for (int y = 1; y < bottom; y++)
            {
                squares[0, y] = (byte)UrbanSquareType.OuterWall;
                squareDetail[0, y] = (byte)Dir4.W;
                squares[right, y] = (byte)UrbanSquareType.OuterWall;
                squareDetail[right, y] = (byte)Dir4.E;
            }
            for (int x = 1; x < right; x++)
            {
                squares[x, 0] = (byte)UrbanSquareType.OuterWall;
                squareDetail[x, 0] = (byte)Dir4.N;
                squares[x, bottom] = (byte)UrbanSquareType.OuterWall;
                squareDetail[x, bottom] = (byte)Dir4.S;
            }

            // Designate corner towers.
            squares[0, 0] = (byte)UrbanSquareType.RingTower;
            squares[right, 0] = (byte)UrbanSquareType.RingTower;
            squares[0, bottom] = (byte)UrbanSquareType.RingTower;
            squares[right, bottom] = (byte)UrbanSquareType.RingTower;

            IntVector2 entranceOne = DesignateEntrance(entranceDir);
            IntVector2 entranceTwo = DesignateEntrance(lib.Invert(entranceDir));
            DesignateRoadBetween(entranceOne, entranceTwo);

            DesignateCitySquareAndTownHall(entranceOne, selectedCenterSquareWorkers);

            if (npcs.Contains(GameObjects.EnvironmentObj.MapChunkObjectType.High_priest))
                //|| PlatformSettings.DebugOptions) // == always create during debug
            {
                DesignateChurch(entranceOne);
            }

            if (npcs.Contains(GameObjects.EnvironmentObj.MapChunkObjectType.Banker))
                //|| PlatformSettings.DebugOptions) // == always create during debug
            {
                DesignateBank(entranceOne);
            }

            DesignateStable(entranceOne, entranceTwo);
            DesignateAlleyways(entranceOne, entranceTwo);
            DesignateCook(entranceOne, entranceTwo);
            DesignateGranpaAndWarVeteran(npcs);
            DesignateHouses();
            DesignateCottages();

            createNPCData(npcs, LootfestLib.TipsPerCity, tips);

        } //end city construct

        // Initialisation methods.
        /// <summary>
        /// Initialises all village materials
        /// TODO check this method
        /// </summary>
        /// <returns></returns>
        private List<Data.MaterialType> InitMaterialParams()
        {
            var villageWallMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.gray_bricks, Data.MaterialType.red_bricks, 
                Data.MaterialType.brown, Data.MaterialType.mossy_stone, 
                Data.MaterialType.red_brown, Data.MaterialType.sand_stone_bricks, 
                Data.MaterialType.cobble_stone, Data.MaterialType.wood, 

            };
            var villageRoofMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
                Data.MaterialType.dark_skin, Data.MaterialType.orange, 
                Data.MaterialType.red_orange, Data.MaterialType.wood_growing, 
                Data.MaterialType.red_roof,

            };
            var villageWinFramMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
                Data.MaterialType.dark_skin, Data.MaterialType.wood_growing, 
                Data.MaterialType.wood, Data.MaterialType.black,
                Data.MaterialType.brown, Data.MaterialType.blue_gray,
            };
            var villageDoorMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.blue, Data.MaterialType.wood, 
                Data.MaterialType.red_brown, Data.MaterialType.red_orange, 
                Data.MaterialType.blue_gray,
            };


            const int NumMaterials = 2;
            wallMaterials = new List<Data.MaterialType>();
            wall2Materials = new List<Data.MaterialType>();
            roofMaterials = new List<Data.MaterialType>();
            frameMaterials = new List<Data.MaterialType>();
            for (int i = 0; i < NumMaterials; i++)
            {
                wallMaterials.Add(villageWallMaterials[Data.RandomSeed.Instance.Next(villageWallMaterials.Count)]);
                wall2Materials.Add(villageWallMaterials[Data.RandomSeed.Instance.Next(villageWallMaterials.Count)]);
                roofMaterials.Add(villageRoofMaterials[Data.RandomSeed.Instance.Next(villageRoofMaterials.Count)]);
                if (i == 0 || Data.RandomSeed.Instance.BytePercent(100))
                    frameMaterials.Add(villageWinFramMaterials[Data.RandomSeed.Instance.Next(villageWinFramMaterials.Count)]);
            }
            return villageDoorMaterials;
        }

        /// <summary>
        /// Randomises what professional NPCs will spawn
        /// </summary>
        /// <param name="allCitiesWorkerDistribution">Needed to make sure at least one craftsman of each type is present in the world</param>
        /// <returns></returns>
        private void InitNPCParams(List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs, 
            List<GameObjects.EnvironmentObj.MapChunkObjectType> allCitiesWorkerDistribution, 
            out List<GameObjects.EnvironmentObj.MapChunkObjectType> selectedCenterSquareWorkers)
        {
            var BuildingNPCs = new List<GameObjects.EnvironmentObj.MapChunkObjectType>
            {
                //GameObjects.EnvironmentObj.MapChunkObjectType.High_priest,
                GameObjects.EnvironmentObj.MapChunkObjectType.Banker,
            };
           

            //List of possible workers to show up
            var WorkerNPCs = new List<GameObjects.EnvironmentObj.MapChunkObjectType>
            {
                GameObjects.EnvironmentObj.MapChunkObjectType.Armor_smith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker,
                GameObjects.EnvironmentObj.MapChunkObjectType.Blacksmith,
               
                GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Healer,

                GameObjects.EnvironmentObj.MapChunkObjectType.Tailor,
                //GameObjects.EnvironmentObj.MapChunkObjectType.Wise_Lady,
            };

            selectedCenterSquareWorkers = new List<GameObjects.EnvironmentObj.MapChunkObjectType>();

            if (PlatformSettings.ViewUnderConstructionStuff)
            {
                BuildingNPCs.Add(GameObjects.EnvironmentObj.MapChunkObjectType.High_priest);
            }

            if (areaLevel == 0)
            {
                createGranpa = true;
            }
            else if (areaLevel == 1)
            {
                createWarVeteran = true;
            }

            //Pick random craftsmen that will stand in the city square
            int numWorkers = Data.RandomSeed.Instance.Next(NumWorkerNPCs);
            SelectRandomNPCs(selectedCenterSquareWorkers, WorkerNPCs, allCitiesWorkerDistribution, numWorkers);
            npcs.AddRange(selectedCenterSquareWorkers);

            //Pick random npcs that own their own building
            int numBuildingNPCs = Data.RandomSeed.Instance.Next(BuildingNPCs.Count);
            SelectRandomNPCs(npcs, BuildingNPCs, allCitiesWorkerDistribution, numBuildingNPCs);



           // npcs.AddRange(BuildingNPCs);

        }


        // Designation methods.
        /// <summary>
        /// Designates 2x2 cottages in empty areas
        /// </summary>
        private void DesignateCottages()
        {
            int xLen = squares.GetLength(0);
            int yLen = squares.GetLength(1);
            IntVector2 pos = IntVector2.Zero;
            for (pos.X = 0; pos.X != xLen; ++pos.X)
            {
                for (pos.Y = 0; pos.Y != yLen; ++pos.Y)
                {
                    if (CheckForEmpty(new IntVector2(pos.X, pos.Y)) && CheckForEmpty(new IntVector2(pos.X + 1, pos.Y + 1)) &&
                        CheckForEmpty(new IntVector2(pos.X + 1, pos.Y)) && CheckForEmpty(new IntVector2(pos.X, pos.Y + 1)) &&
                        Ref.rnd.RandomChance(50))
                    {
                        bool special = Ref.rnd.RandomChance(50);
                        squares[pos.X, pos.Y] = (byte)UrbanSquareType.FenceYard;
                        squareDetail[pos.X, pos.Y] =            (byte)(DirectionFlags.North | DirectionFlags.West |
                                                                      (special ? DirectionFlags.Special : DirectionFlags.NONE));
                        squares[pos.X + 1, pos.Y] = (byte)UrbanSquareType.FenceYard;
                        squareDetail[pos.X + 1, pos.Y] =        (byte)(DirectionFlags.North | DirectionFlags.East |
                                                                      (special ? DirectionFlags.Special : DirectionFlags.NONE));
                        squares[pos.X, pos.Y + 1] = (byte)UrbanSquareType.FenceYard;
                        squareDetail[pos.X, pos.Y + 1] =        (byte)(DirectionFlags.South | DirectionFlags.West |
                                                                      (special ? DirectionFlags.Special : DirectionFlags.NONE));
                        squares[pos.X + 1, pos.Y + 1] = (byte)UrbanSquareType.FenceYard;
                        squareDetail[pos.X + 1, pos.Y + 1] =    (byte)(DirectionFlags.South | DirectionFlags.East |
                                                                      (special ? DirectionFlags.Special : DirectionFlags.NONE));
                    }
                }
            }

            wheatHeight = MinWheatHeight + Data.RandomSeed.Instance.Next(MaxWheatHeight - MinWheatHeight + 1);
        }

        /// <summary>
        /// Designates houses by roads.
        /// </summary>
        private void DesignateHouses()
        {
            int xLen = squares.GetLength(0);
            int yLen = squares.GetLength(1);
            for (int x = 0; x != xLen; ++x)
            {
                for (int y = 0; y != yLen; ++y)
                {
                    if (CheckForEmpty(new IntVector2(x, y)) && CheckForRoad(x + 1, y, true) && Ref.rnd.RandomChance(50))
                    {
                        squares[x, y] = (byte)UrbanSquareType.House;
                        squareDetail[x, y] = (byte)Dir4.E;
                    }
                    if (CheckForEmpty(new IntVector2(x, y)) && CheckForRoad(x - 1, y, true) && Ref.rnd.RandomChance(50))
                    {
                        squares[x, y] = (byte)UrbanSquareType.House;
                        squareDetail[x, y] = (byte)Dir4.W;
                    }
                    if (CheckForEmpty(new IntVector2(x, y)) && CheckForRoad(x, y + 1, true) && Ref.rnd.RandomChance(50))
                    {
                        squares[x, y] = (byte)UrbanSquareType.House;
                        squareDetail[x, y] = (byte)Dir4.S;
                    }
                    if (CheckForEmpty(new IntVector2(x, y)) && CheckForRoad(x, y - 1, true) && Ref.rnd.RandomChance(50))
                    {
                        squares[x, y] = (byte)UrbanSquareType.House;
                        squareDetail[x, y] = (byte)Dir4.N;
                    }
                }
            }
        }

        /// <summary>
        /// Designates granpa and war vet at a random position each
        /// </summary>
        private void DesignateGranpaAndWarVeteran(List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs)
        {
            if (createGranpa)
            {
                npcs.Add(GameObjects.EnvironmentObj.MapChunkObjectType.Granpa);
                IntVector2 pos = IntVector2.Zero;
                do
                {
                    pos.X = Data.RandomSeed.Instance.Next(squares.GetLength(0));
                    pos.Y = Data.RandomSeed.Instance.Next(squares.GetLength(1));
                }
                while (squares[pos.X, pos.Y] != (byte)UrbanSquareType.Road);

                squareDetail[pos.X, pos.Y] += Granpa;
            }
            if (createWarVeteran)
            {
                npcs.Add(GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran);
                IntVector2 pos = IntVector2.Zero;
                do
                {
                    pos.X = Data.RandomSeed.Instance.Next(squares.GetLength(0));
                    pos.Y = Data.RandomSeed.Instance.Next(squares.GetLength(1));
                }
                while (squares[pos.X, pos.Y] != (byte)UrbanSquareType.Road);

                squareDetail[pos.X, pos.Y] += WarVeteran;
            }
        }

        /// <summary>
        /// Designates a cook by the main road.
        /// </summary>
        /// <param name="cityEntranceOne">One of the entrances to the city</param>
        /// <param name="cityEntranceTwo">The other city entrance</param>
        private void DesignateCook(IntVector2 cityEntranceOne, IntVector2 cityEntranceTwo)
        {
            var availablePositions = new List<IntVector2>();

            IntVector2 forwardOne = lib.Dir4ToIntVec2(lib.Invert(entranceDir), 1);
            IntVector2 rightOne = lib.Dir4ToIntVec2(lib.GetLateralLeftFacing(entranceDir), 1);
            IntVector2 leftOne = lib.Dir4ToIntVec2(lib.GetLateralRightFacing(entranceDir), 1);

            IntVector2 cookSpot = cityEntranceOne;
            while (cookSpot != cityEntranceTwo)
            {
                cookSpot += forwardOne;
                IntVector2 temp = cookSpot + rightOne;
                if (squares[temp.X, temp.Y] == (byte)UrbanSquareType.NOTHING)
                {
                    availablePositions.Add(temp);
                }
                temp = cookSpot + leftOne;
                if (squares[temp.X, temp.Y] == (byte)UrbanSquareType.NOTHING)
                {
                    availablePositions.Add(temp);
                }
            }

            cookSpot = availablePositions[Data.RandomSeed.Instance.Next(availablePositions.Count)];
            Dir4 direction;
            FindClosestRoad(cityEntranceOne, cookSpot, out direction);

            squares[cookSpot.X, cookSpot.Y] = (byte)UrbanSquareType.Cook;
            squareDetail[cookSpot.X, cookSpot.Y] = (byte)direction;
        }

        /// <summary>
        /// Designates alleyways reaching from the main road toward the walls - one alley for each open area.
        /// </summary>
        /// <param name="cityEntranceOne">One of the entrances to the city</param>
        /// <param name="cityEntranceTwo">The other city entrance</param>
        private void DesignateAlleyways(IntVector2 cityEntranceOne, IntVector2 cityEntranceTwo)
        {
//#if WINDOWS
            Dir4 rightDir = lib.GetLateralRightFacing(entranceDir);
            Dir4 leftDir = lib.GetLateralLeftFacing(entranceDir);
            IntVector2 forwardOne = lib.Dir4ToIntVec2(lib.Invert(entranceDir), 1);
            IntVector2 rightOne = lib.Dir4ToIntVec2(lib.GetLateralRightFacing(entranceDir), 1);
            IntVector2 leftOne = lib.Dir4ToIntVec2(lib.GetLateralLeftFacing(entranceDir), 1);


            var actualRoads = new List<Pair<IntVector2, IntVector2>>();
            var possibleLeftRoads = new List<Pair<IntVector2, IntVector2>>();
            var possibleRightRoads = new List<Pair<IntVector2, IntVector2>>();
            int rightsInARow = 0;
            int leftsInARow = 0;

            IntVector2 mainPos = cityEntranceOne;
            while (mainPos != cityEntranceTwo)
            {
                mainPos += forwardOne;
                if (squares[mainPos.X, mainPos.Y] != (byte)UrbanSquareType.Road)
                {
                    if (rightsInARow > 0)
                    {
                        int index = Data.RandomSeed.Instance.Next(rightsInARow);
                        actualRoads.Add(possibleRightRoads[index]);
                        possibleRightRoads.RemoveRange(0, possibleRightRoads.Count);
                        rightsInARow = 0;
                    }
                    if (leftsInARow > 0)
                    {
                        int index = Data.RandomSeed.Instance.Next(leftsInARow);
                        actualRoads.Add(possibleLeftRoads[index]);
                        possibleLeftRoads.RemoveRange(0, possibleLeftRoads.Count);
                        leftsInARow = 0;
                    }
                    continue;
                }

                IntVector2 temp = mainPos;
                while (true)
                {
                    temp += rightOne;
                    // Positive end.
                    if (squares[temp.X, temp.Y] == (byte)UrbanSquareType.OuterWall)
                    {
                        possibleRightRoads.Add(new Pair<IntVector2, IntVector2>(mainPos, temp));
                        ++rightsInARow;
                        break;
                    }
                    // Negative end.
                    if (squares[temp.X, temp.Y] != (byte)UrbanSquareType.NOTHING && squares[temp.X, temp.Y] != (byte)UrbanSquareType.Road)
                    {
                        if (rightsInARow > 0)
                        {
                            int index = Data.RandomSeed.Instance.Next(rightsInARow);
                            actualRoads.Add(possibleRightRoads[index]);
                            possibleRightRoads.RemoveRange(0, possibleRightRoads.Count);
                            rightsInARow = 0;
                        }
                        break;
                    }
                }
                temp = mainPos;
                while (true)
                {
                    temp += leftOne;
                    // Positive end.
                    if (squares[temp.X, temp.Y] == (byte)UrbanSquareType.OuterWall)
                    {
                        possibleLeftRoads.Add(new Pair<IntVector2, IntVector2>(mainPos, temp));
                        ++leftsInARow;
                        break;
                    }
                    // Negative end.
                    if (squares[temp.X, temp.Y] != (byte)UrbanSquareType.NOTHING && squares[temp.X, temp.Y] != (byte)UrbanSquareType.Road)
                    {
                        if (leftsInARow > 0)
                        {
                            int index = Data.RandomSeed.Instance.Next(leftsInARow);
                            actualRoads.Add(possibleLeftRoads[index]);
                            possibleLeftRoads.RemoveRange(0, possibleLeftRoads.Count);
                            leftsInARow = 0;
                        }
                        break;
                    }
                }
            }

            foreach (var r in actualRoads)
            {
                DesignateRoadBetween(r.Item1, r.Item2);
            }
//#endif
        }

        /// <summary>
        /// Designates a stable near one of the city's entrances.
        /// </summary>
        /// <param name="entranceStart">One of the entrances.</param>
        /// <param name="entranceEnd">The other city entrance.</param>
        private void DesignateStable(IntVector2 entranceStart, IntVector2 entranceEnd)
        {
            var stablePositions = new List<IntVector2>();
            IntVector2 add = entranceStart - lib.Dir4ToIntVec2(entranceDir, 1);
            stablePositions.Add(add + lib.Dir4ToIntVec2(lib.GetLateralRightFacing(entranceDir), 1));
            stablePositions.Add(add + lib.Dir4ToIntVec2(lib.GetLateralLeftFacing(entranceDir), 1));
            add = entranceEnd - lib.Dir4ToIntVec2(lib.Invert(entranceDir), 1);
            stablePositions.Add(add + lib.Dir4ToIntVec2(lib.GetLateralRightFacing(entranceDir), 1));
            stablePositions.Add(add + lib.Dir4ToIntVec2(lib.GetLateralLeftFacing(entranceDir), 1));

            IntVector2 stablePosition = stablePositions[Data.RandomSeed.Instance.Next(4)];
            while (!CheckForEmpty(stablePosition))
            {
                stablePosition = stablePositions[Data.RandomSeed.Instance.Next(4)];
            }
            SetTileType(stablePosition, UrbanSquareType.Stable);
            Dir4 facingRoad;
            FindClosestRoad(entranceStart, stablePosition, out facingRoad);
            SetTileDetail(stablePosition, (byte)facingRoad);
        }

        /// <summary>
        /// Designates an area for the city square
        /// </summary>
        /// <param name="cityEntrancePos">The position of one of the city's entrances</param>
        private void DesignateCitySquareAndTownHall(IntVector2 cityEntrancePos, List<GameObjects.EnvironmentObj.MapChunkObjectType> selectedCenterSquareWorkers)
        {
            Dir4 townHallSide = Dir4.NUM_NON;
            Data.RandomSeedInstance randomizer = Data.RandomSeed.Instance;
            IntVector2 squareCenter = new IntVector2(randomizer.Next(6) + 4, randomizer.Next(6) + 4);

            // Designate main square.
            for (int x = squareCenter.X - 1; x <= squareCenter.X + 1; ++x)
            {
                for (int y = squareCenter.Y - 1; y <= squareCenter.Y + 1; ++y)
                {
                    squares[x, y] = (byte)UrbanSquareType.Road;
                }
            }

            // Designate road to main road.
            IntVector2 closestRoad = FindClosestRoad(cityEntrancePos, squareCenter, out townHallSide);
            townHallSide = lib.Invert(townHallSide);
            DesignateRoadBetween(squareCenter, closestRoad);
            squares[squareCenter.X, squareCenter.Y] = (byte)UrbanSquareType.SquareCenter;
            squareDetail[squareCenter.X, squareCenter.Y] = (byte)lib.Invert(townHallSide);

            // Designate town hall.
            IntVector2 townHallStart = squareCenter;
            int add = 2;
            if (townHallSide != lib.GetPositiveFacing(townHallSide))
                ++add;
            townHallStart += lib.Dir4ToIntVec2(townHallSide, add);
            townHallStart += lib.Dir4ToIntVec2(lib.GetPositivePerpendicularFacing(townHallSide), -1);
            IntVector2 townHallEnd = townHallStart;
            townHallEnd += lib.Dir4ToIntVec2(lib.GetPositiveFacing(townHallSide), 1);
            townHallEnd += lib.Dir4ToIntVec2(lib.GetPositivePerpendicularFacing(townHallSide), 2);
            for (int x = townHallStart.X; x <= townHallEnd.X; ++x)
            {
                for (int y = townHallStart.Y; y <= townHallEnd.Y; ++y)
                {
                    squares[x, y] = (byte)UrbanSquareType.TownHall;
                    if (x == townHallStart.X)
                        squareDetail[x, y] += (byte)DirectionFlags.West;
                    if (x == townHallEnd.X)
                        squareDetail[x, y] += (byte)DirectionFlags.East;
                    if (y == townHallStart.Y)
                        squareDetail[x, y] += (byte)DirectionFlags.North;
                    if (y == townHallEnd.Y)
                        squareDetail[x, y] += (byte)DirectionFlags.South;
                }
            }
            IntVector2 townHallEntrance = squareCenter + lib.Dir4ToIntVec2(townHallSide, 2);
            squareDetail[townHallEntrance.X, townHallEntrance.Y] += (byte)DirectionFlags.Special;

            // Designate stands.
            var standDirections = new List<Dir4> { Dir4.N, Dir4.E, Dir4.W, Dir4.S };
            standDirections.Remove(townHallSide);

            foreach (var dir in standDirections)
            {
                IntVector2 start = squareCenter + lib.Dir4ToIntVec2(dir, 2);
                IntVector2 end = start + lib.Dir4ToIntVec2(lib.GetPositivePerpendicularFacing(dir), 1);
                start -= lib.Dir4ToIntVec2(lib.GetPositivePerpendicularFacing(dir), 1);
                for (int x = start.X; x <= end.X; ++x)
                {
                    for (int y = start.Y; y <= end.Y; ++y)
                    {
                        if (squares[x, y] == (byte)UrbanSquareType.NOTHING)
                        {
                            //lägg in npc workers här
                            if (selectedCenterSquareWorkers.Count > 0)
                            {
                                int index = Data.RandomSeed.Instance.Next(selectedCenterSquareWorkers.Count);
                                GameObjects.EnvironmentObj.MapChunkObjectType npc = selectedCenterSquareWorkers[index];
                                //CreateNPC(npc, wp);
                                citySquareObjectDic.Add(new IntVector2(x, y), npc);
                                selectedCenterSquareWorkers.RemoveAt(index);
                            }
                            else if (Data.RandomSeed.Instance.BytePercent(160))
                            {
                                citySquareObjectDic.Add(new IntVector2(x, y), GameObjects.EnvironmentObj.MapChunkObjectType.Home);
                            }

                            squares[x, y] = (byte)UrbanSquareType.SquareStand;
                            squareDetail[x, y] = (byte)lib.Invert(dir);
                        }
                    }
                }
            }


        }

        private void DesignateRoadBetween(IntVector2 from, IntVector2 to)
        {
            DesignateRoadBetween(from, to, false);
        }

        /// <summary>
        /// Marks a road in the planning array "squares" from point to point, prioritising X-movement unless stated otherwise.
        /// Note that it does not create road at either from nor to - just between.
        /// </summary>
        /// <param name="from">point</param>
        /// <param name="to">point</param>
        /// <param name="Yfirst">Will do Y-movement prior to X-movement</param>
        private void DesignateRoadBetween(IntVector2 from, IntVector2 to, bool Yfirst) //= false)
        {
            // Move once to ignore the from-tile
            if (from.Y > to.Y)
                --from.Y;
            else if (from.Y < to.Y)
                ++from.Y;
            else if (from.X > to.X)
                --from.X;
            else if (from.X < to.X)
                ++from.X;

            while (from != to) // Ignore to-tile
            {
                if (Yfirst)
                {
                    if (from.Y > to.Y)
                        squares[from.X, from.Y--] = (byte)UrbanSquareType.Road;
                    else if (from.Y < to.Y)
                        squares[from.X, from.Y++] = (byte)UrbanSquareType.Road;
                    else if (from.X > to.X)
                        squares[from.X--, from.Y] = (byte)UrbanSquareType.Road;
                    else if (from.X < to.X)
                        squares[from.X++, from.Y] = (byte)UrbanSquareType.Road;
                }
                else
                {
                    if (from.X > to.X)
                        squares[from.X--, from.Y] = (byte)UrbanSquareType.Road;
                    else if (from.X < to.X)
                        squares[from.X++, from.Y] = (byte)UrbanSquareType.Road;
                    else if (from.Y > to.Y)
                        squares[from.X, from.Y--] = (byte)UrbanSquareType.Road;
                    else if (from.Y < to.Y)
                        squares[from.X, from.Y++] = (byte)UrbanSquareType.Road;
                }
            }
        }

        /// <summary>
        /// Designates a city entrance 
        /// </summary>
        /// <param name="entranceFacing">What side of the city</param>
        /// <returns>The position in the grid</returns>
        IntVector2 DesignateEntrance(Dir4 entranceFacing)
        {
            IntVector2 localEntrancePos = squareSize.GetMidSide(entranceFacing);
            IntVector2 entranceSides = IntVector2.Zero;
            if (entranceFacing == Dir4.N || entranceFacing == Dir4.S)
            {
                entranceSides.X = 1;
            }
            else
            {
                entranceSides.Y = 1;
            }

            Dir4 inwards = lib.Invert(entranceFacing);
            // Designate towers.
            IntVector2 side = localEntrancePos + entranceSides;
            squares[side.X, side.Y] = (byte)UrbanSquareType.RingTower;
            squareDetail[side.X, side.Y] = (byte)inwards;
            side = localEntrancePos - entranceSides;
            squares[side.X, side.Y] = (byte)UrbanSquareType.RingTower;
            squareDetail[side.X, side.Y] = (byte)inwards;

            // Designate entrance.
            squares[localEntrancePos.X, localEntrancePos.Y] = (byte)UrbanSquareType.CityEntrance;
            squareDetail[localEntrancePos.X, localEntrancePos.Y] = (byte)entranceFacing;
            return localEntrancePos;
        }

        /// <summary>
        /// Designates a church at a random position.
        /// </summary>
        /// <param name="cityEntrancePos">The position of one of the city's entrances</param>
        private void DesignateChurch(IntVector2 cityEntrancePos)
        {
            // Seek for an available location to hold:
            //  XXX
            //  XXX
            //  XEX   E = Entrance
            Dimensions entranceDim = lib.Dir4ToDimensions(entranceDir);

            IntVector2 churchEntrance = IntVector2.Zero;
            Dir4 churchFacing = Dir4.NUM_NON;
            IntVector2 awayOne = IntVector2.Zero;
            IntVector2 rightOne = IntVector2.Zero;
            IntVector2 leftOne = IntVector2.Zero;
            IntVector2 closestRoad = IntVector2.Zero;
            bool seeking = true;
            while (seeking)
            {
                Data.RandomSeedInstance randomizer = Data.RandomSeed.Instance;
                churchEntrance = new IntVector2(randomizer.Next(6) + 4, randomizer.Next(6) + 4);
                closestRoad = FindClosestRoad(cityEntrancePos, churchEntrance, out churchFacing); // Find facing
                awayOne = lib.Dir4ToIntVec2(lib.Invert(churchFacing), 1);
                rightOne = lib.Dir4ToIntVec2(lib.GetLateralRightFacing(churchFacing), 1);
                leftOne = lib.Dir4ToIntVec2(lib.GetLateralLeftFacing(churchFacing), 1);
                if (CheckForEmpty(churchEntrance) && CheckForEmpty(churchEntrance + leftOne) && CheckForEmpty(churchEntrance + rightOne) &&
                    CheckForEmpty(churchEntrance + awayOne) && CheckForEmpty(churchEntrance + awayOne + leftOne) &&
                    CheckForEmpty(churchEntrance + awayOne + rightOne) && CheckForEmpty(churchEntrance + awayOne * 2) &&
                    CheckForEmpty(churchEntrance + awayOne * 2 + leftOne) && CheckForEmpty(churchEntrance + awayOne * 2 + rightOne))
                {
                    seeking = false;
                }
            }
            // Found!
            DesignateRoadBetween(closestRoad, churchEntrance);
            
            Dir4 toRight = lib.GetLateralRightFacing(churchFacing);
            Dir4 toLeft = lib.GetLateralLeftFacing(churchFacing);
            Dir4 opposite = lib.Invert(churchFacing);

            // Entrance row
            SetTileType(churchEntrance, UrbanSquareType.ChurchEntrance);
            SetTileDetail(churchEntrance, (byte)(lib.Facing4ToDirectionFlags(churchFacing) | DirectionFlags.Special));
            SetTileType(churchEntrance + rightOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + rightOne, (byte)(lib.Facing4ToDirectionFlags(churchFacing) | lib.Facing4ToDirectionFlags(toRight)));
            SetTileType(churchEntrance + leftOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + leftOne, (byte)(lib.Facing4ToDirectionFlags(churchFacing) | lib.Facing4ToDirectionFlags(toLeft)));

            // Middle row
            SetTileType(churchEntrance + awayOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + awayOne, (byte)(DirectionFlags.Special));
            SetTileType(churchEntrance + awayOne + rightOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + awayOne + rightOne, (byte)(lib.Facing4ToDirectionFlags(toRight)));
            SetTileType(churchEntrance + awayOne + leftOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + awayOne + leftOne, (byte)(lib.Facing4ToDirectionFlags(toLeft)));

            // Back wall row
            SetTileType(churchEntrance + awayOne * 2, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + awayOne * 2, (byte)(lib.Facing4ToDirectionFlags(opposite) | DirectionFlags.Special));
            SetTileType(churchEntrance + awayOne * 2 + rightOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + awayOne * 2 + rightOne, (byte)(lib.Facing4ToDirectionFlags(opposite) | lib.Facing4ToDirectionFlags(toRight)));
            SetTileType(churchEntrance + awayOne * 2 + leftOne, UrbanSquareType.Church);
            SetTileDetail(churchEntrance + awayOne * 2 + leftOne, (byte)(lib.Facing4ToDirectionFlags(opposite) | lib.Facing4ToDirectionFlags(toLeft)));
//#if WINDOWS
            //for (int i = 0; i != npcs.Count; ++i)
            //{
            //    if (npcs[i] == GameObjects.EnvironmentObj.MapChunkObjectType.High_priest)
            //    {
            //        npcs.RemoveAt(i--);
            //    }
            //}
            //npcs.RemoveAll(x => x == GameObjects.EnvironmentObj.MapChunkObjectType.High_priest);
//#endif
        }

        /// <summary>
        /// Designates a bank at a random position.
        /// </summary>
        /// <param name="cityEntrancePos">The position of one of the city's entrances</param>
        /// <returns></returns>
        private void DesignateBank(IntVector2 cityEntrancePos)
        {
            // Seek for an available location to hold:
            //  XXX
            //  XEX   E = Entrance
            Dimensions entranceDim = lib.Dir4ToDimensions(entranceDir);

            IntVector2 bankEntrance = IntVector2.Zero;
            Dir4 bankFacing = Dir4.NUM_NON;
            IntVector2 awayOne = IntVector2.Zero;
            IntVector2 rightOne = IntVector2.Zero;
            IntVector2 leftOne = IntVector2.Zero;
            IntVector2 closestRoad = IntVector2.Zero;
            bool seeking = true;
            while (seeking)
            {
                Data.RandomSeedInstance randomizer = Data.RandomSeed.Instance;
                bankEntrance = new IntVector2(randomizer.Next(6) + 4, randomizer.Next(6) + 4);
                closestRoad = FindClosestRoad(cityEntrancePos, bankEntrance, out bankFacing); // Find facing
                awayOne = lib.Dir4ToIntVec2(lib.Invert(bankFacing), 1);
                rightOne = lib.Dir4ToIntVec2(lib.GetLateralRightFacing(bankFacing), 1);
                leftOne = lib.Dir4ToIntVec2(lib.GetLateralLeftFacing(bankFacing), 1);
                if (CheckForEmpty(bankEntrance) && CheckForEmpty(bankEntrance + leftOne) && CheckForEmpty(bankEntrance + rightOne) &&
                    CheckForEmpty(bankEntrance + awayOne) && CheckForEmpty(bankEntrance + awayOne + leftOne) &&
                    CheckForEmpty(bankEntrance + awayOne + rightOne))
                {
                    seeking = false;
                }
            }
            // Found!
            DesignateRoadBetween(closestRoad, bankEntrance);

            Dir4 toRight = lib.GetLateralRightFacing(bankFacing);
            Dir4 toLeft = lib.GetLateralLeftFacing(bankFacing);
            Dir4 opposite = lib.Invert(bankFacing);

            // Entrance row
            SetTileType(bankEntrance, UrbanSquareType.Road);
            SetTileType(bankEntrance + rightOne, UrbanSquareType.Bank);
            SetTileDetail(bankEntrance + rightOne, (byte)(lib.Facing4ToDirectionFlags(bankFacing) | lib.Facing4ToDirectionFlags(toRight) | lib.Facing4ToDirectionFlags(toLeft)));
            SetTileType(bankEntrance + leftOne, UrbanSquareType.Bank);
            SetTileDetail(bankEntrance + leftOne, (byte)(lib.Facing4ToDirectionFlags(bankFacing) | lib.Facing4ToDirectionFlags(toRight) | lib.Facing4ToDirectionFlags(toLeft)));

            // Back wall row
            SetTileType(bankEntrance + awayOne, UrbanSquareType.Bank);
            SetTileDetail(bankEntrance + awayOne, (byte)(lib.Facing4ToDirectionFlags(bankFacing) | DirectionFlags.Special));
            SetTileType(bankEntrance + awayOne + rightOne, UrbanSquareType.Bank);
            SetTileDetail(bankEntrance + awayOne + rightOne, (byte)(lib.Facing4ToDirectionFlags(opposite) | lib.Facing4ToDirectionFlags(toRight)));
            SetTileType(bankEntrance + awayOne + leftOne, UrbanSquareType.Bank);
            SetTileDetail(bankEntrance + awayOne + leftOne, (byte)(lib.Facing4ToDirectionFlags(opposite) | lib.Facing4ToDirectionFlags(toLeft)));

        }


        /// <summary>
        /// The great switch, where everything is built from.
        /// </summary>
        /// <param name="type">The type of object to build.</param>
        /// <param name="detail">Detailed information about what to build. Actual data type depends on type.</param>
        /// <param name="wp">Upper left corner of half chunk-sized square.</param>
        /// <param name="squarePos">The square's local position in the array squares</param>
        /// <param name="detailsPos">The square's local position in the array squareDetail</param>
        protected override void BuildObject(UrbanSquareType type, byte detail, WorldPosition wp, 
            IntVector2 squarePos, IntVector2 detailPos, bool gameObjects)
        {
            switch (type)
            {
                case UrbanSquareType.Road:
                {
                    if ((detail | Granpa) == detail)
                        CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.Granpa, wp, gameObjects);
                    if ((detail | WarVeteran) == detail)
                        CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran, wp, gameObjects);

                    if (gameObjects)
                    {
                        if (LfRef.gamestate.GameObjCollection.SpawnOptionalGameObject() && Ref.rnd.RandomChance(20))
                            CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC, wp, true);
                    }
                    else
                    {
                        CreateRoadPiece(wp, squarePos);
                        wp.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
                        wp.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
                        
                    }
                    break;
                }
                case UrbanSquareType.FenceYard:
                {
                    if (!gameObjects)
                        CreateCottage(detail, wp);
                    break;
                }
                case UrbanSquareType.CityEntrance:
                {
                    Dir4 direction = (Dir4)detail;
                    if (gameObjects && LfRef.gamestate.GameObjCollection.SpawnOptionalGameObject())
                    {
                        wp.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
                        wp.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
                        CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.Guard, wp, Dir4.S, direction, true);
                    }
                    else
                    {
                        CreateRoadPiece(wp, squarePos);
                        CreateCityEntrance(wp, direction);
                    }
                    break;
                }
                case UrbanSquareType.SquareCenter:
                {
                    if (!gameObjects)
                    {
                        CreateRoadPiece(wp, squarePos);
                    }
                    
                    CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.Salesman, wp, Dir4.S, (Dir4)detail, gameObjects);
                    break;
                }
                case UrbanSquareType.OuterWall:
                {
                    if (!gameObjects)
                        CreateWall((Dir4)detail, wp);
                    break;
                }
                case UrbanSquareType.TownHall:
                {
                    if (!gameObjects)
                        CreateTownHall((DirectionFlags)detail, wp);
                    break;
                }
                case UrbanSquareType.ChurchEntrance: // Fallthrough
                case UrbanSquareType.Church:
                {
                    if (!gameObjects)
                    {
                        DirectionFlags dirs = (DirectionFlags)detail;
                        CreateChurch(type, wp, dirs);
                    }
                    break;
                }
                case UrbanSquareType.Bank:
                {
                    DirectionFlags dirs = (DirectionFlags)detail;
                    if (gameObjects)
                    {
                        if (lib.HasFlag((int)dirs, (int)DirectionFlags.Special))//dirs.HasFlag(DirectionFlags.Special))
                            CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.Banker, wp.GetNeighborPos(new IntVector3(8)), true);
                        
                    }
                    else
                    {
                        CreateBank(wp, dirs);
                    }
                    
                    break;
                }
                case UrbanSquareType.Stable:
                {
                    if (gameObjects)
                        CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport, wp, Dir4.E, (Dir4)detail, true);
                    break;
                }
                case UrbanSquareType.SquareStand:
                {
                    GameObjects.EnvironmentObj.MapChunkObjectType objectType;
                    if (citySquareObjectDic.TryGetValue(squarePos, out objectType))
                    {
                        if (objectType == GameObjects.EnvironmentObj.MapChunkObjectType.Home)
                        {
                            goto case UrbanSquareType.House;
                        }
                        else
                        {
                           
                            CreateNPC(NPCdata, objectType, wp, gameObjects);
                        }
                    }
                //    if (npcs.Count > 0)
                //    {
                //        int index = Data.RandomSeed.Instance.Next(npcs.Count);
                //        GameObjects.EnvironmentObj.MapChunkObjectType npc = npcs[index];
                //        CreateNPC(npc, wp);
                //        npcs.RemoveAt(index);
                //    }
                //    else
                //    {
                //        if (Ref.rnd.RandomChance(80))
                //            goto case UrbanSquareType.House;
                //    }
                    break;
                }
                case UrbanSquareType.RingTower:
                {
                    // Corner towers have no lower entrance
                    if (!gameObjects)
                    {
                        Dir4 entranceDirection = (Dir4)detail;
                        if (squarePos.X == 0 && squarePos.Y == 0 ||
                            squarePos.X == 0 && squarePos.Y == squares.GetLength(1) - 1 ||
                            squarePos.X == squares.GetLength(0) - 1 && squarePos.Y == 0 ||
                            squarePos.X == squares.GetLength(0) - 1 && squarePos.Y == squares.GetLength(1) - 1
                            )
                            entranceDirection = Dir4.NUM_NON;
                        CreateTower(entranceDirection, wp, squarePos);
                    }
                    break;
                }
                case UrbanSquareType.Cook:
                {
                    
                    CreateNPC(NPCdata, GameObjects.EnvironmentObj.MapChunkObjectType.Cook, wp, gameObjects);
                    break;
                }
                case UrbanSquareType.House:
                {
                    // house.BuildOnTerrain(wp);
                    //generatehouse(wp, (Dir4)detail);
                    if (!gameObjects)
                    {
                        generatehouse(wp, (Dir4)detail, squarePos, detailPos, houseSettings,
                            wallMaterials, roofMaterials, frameMaterials);
                    }
                    break;
                }
                default:
                {
                    base.BuildObject(type, detail, wp, squarePos, detailPos, gameObjects);
                    break;
                }
            }
        }


        // Creation methods.
        /// <summary>
        /// Creates a cottage at the given position.
        /// </summary>
        /// <param name="detail">A byte holding DirectionFlags. Special means field of wheat - else animals.</param>
        /// <param name="wp">Upper left corner of half-chunk sized square</param>
        private void CreateCottage(byte detail, WorldPosition wp)
        {
            Data.MaterialType straw = Data.MaterialType.straw;

            wp.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
            wp.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
            WorldVolume vol = new WorldVolume(wp, IntVector3.Zero);
            vol.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
            WorldVolume floor = vol;

            var dirs = lib.DirectionFlagsToFacing4((DirectionFlags)detail);
            // Create floor and clear above ground
            foreach (var d in dirs)
            {
                floor.AddToSide(d, WorldPosition.ChunkQuarterWidth - FenceEdgeOffset);
                floor.AddToSide(lib.Invert(d), WorldPosition.ChunkQuarterWidth);
            }

            WorldVolume fence = floor;
            floor.AddToSide(CubeFace.Ypositive, ClearAbove).Fill(Data.MaterialType.NO_MATERIAL);
            floor.SubFromSide(CubeFace.Ypositive, ClearAbove).AddToSide(CubeFace.Ynegative, SoilDepth).Fill(Data.MaterialType.dirt);

            WorldVolume fencePost = floor;

            bool once = true;

            // Create fence and innards.
            foreach (var d in dirs)
            {
                WorldVolume copy = fence;
                copy.SubFromSide(lib.Invert(d), WorldPosition.ChunkHalfWidth - FenceEdgeOffset - FenceWidth);
                copy.AddToSide(CubeFace.Ypositive, FenceHeight);
                copy.AddPosition(CubeFace.Ypositive, FenceFloatHeight);
                copy.Fill(FenceMaterial);
                fencePost.SubFromSide(lib.Invert(d), WorldPosition.ChunkHalfWidth - FenceEdgeOffset - FenceWidth);

                if ((detail | (byte)DirectionFlags.Special) == detail)
                {
                    // Wheat
                    if (once)
                    {
                        int sub = FenceWidth + 1;

                        copy = fence;
                        copy.SubFromSide(d, sub);
                        copy.SubFromSide(dirs[1], sub);
                        copy.AddToSide(CubeFace.Ypositive, wheatHeight);
                        copy.Fill(straw);
                        once = false;
                    }
                }
                else
                {
                    // Animals
                    if (dirs.Contains(Dir4.E) && dirs.Contains(Dir4.S))
                    {
                        wp.WorldGrindex.X -= WorldPosition.ChunkQuarterWidth;
                        wp.WorldGrindex.Z -= WorldPosition.ChunkQuarterWidth;
                        LfRef.chunks.GetScreen(wp).AddChunkObject(new GameObjects.EnvironmentObj.CritterSpawn(wp), true);
                    }
                }
            }

            // Create fence posts.
            fencePost.AddToSide(CubeFace.Ypositive, FencePostHeight).Fill(FenceMaterial);
            foreach (var d in dirs)
            {
                WorldVolume copy = fencePost;
                copy.SubtractPosition(d, WorldPosition.ChunkQuarterWidth);
                copy.Fill(FenceMaterial);
            }
        }

        /// <summary>
        /// Creates a bank at the given position.
        /// </summary>
        /// <param name="wp">Upper left corner of square to build in</param>
        /// <param name="dirs">Directions to put walls in. "Special" denotes entrance</param>
        private void CreateBank(WorldPosition wp, DirectionFlags dirs)
        {
            int BankHalfEntranceWidth = roadHalfWidth - roadOutlineWidth;

            WorldVolume floor = new WorldVolume(wp, new IntVector3(WorldPosition.ChunkHalfWidth, 1, WorldPosition.ChunkHalfWidth));
            WorldVolume copy = floor;
            copy.AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
            floor.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight - 1;

            var facings = lib.DirectionFlagsToFacing4(dirs);

            wp.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
            wp.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
            WorldVolume walls = new WorldVolume(wp, IntVector3.Zero);
            walls.AddToSide(CubeFace.Ypositive, BankWallHeight);

            foreach (var face in facings)
            {
                // Walls.
                copy = walls;
                copy.AddPosition(face, WorldPosition.ChunkQuarterWidth - BankWallThickness - WallOffset);
                copy.AddToSide(face, BankWallThickness);
                copy.AddToLateralEnds(face, WorldPosition.ChunkQuarterWidth);
                copy.Fill(bankMaterial);

                floor.SubFromSide(face, WallOffset + BankWallThickness);
            }
            // Corner pillars.
            var directions = new List<Dir4> { Dir4.N, Dir4.S, Dir4.W, Dir4.E };
            foreach (var d in directions)
            {
                WorldVolume cornerPillar = new WorldVolume(wp, IntVector3.Zero);
                cornerPillar.AddToSide(CubeFace.Ypositive, BankWallHeight);
                Dir4 right = lib.GetLateralRightFacing(d);
                cornerPillar.AddPosition(d, WorldPosition.ChunkQuarterWidth - BankWallThickness - WallOffset);
                cornerPillar.AddPosition(right, WorldPosition.ChunkQuarterWidth - BankWallThickness - WallOffset);
                cornerPillar.AddToSide(d, BankWallThickness + WallOffset);
                cornerPillar.AddToSide(right, BankWallThickness + WallOffset);
                cornerPillar.Fill(bankDecorationMaterial);
            }
            if ((dirs & DirectionFlags.Special) == DirectionFlags.Special)
            {
                Dir4 entranceDir = facings[0];
                Dir4 opposite = lib.Invert(entranceDir);
                
                // Opposite wall as well.
                copy = walls;
                copy.AddPosition(opposite, WorldPosition.ChunkQuarterWidth - BankWallThickness - WallOffset);
                copy.AddToSide(opposite, BankWallThickness);
                copy.AddToLateralEnds(opposite, WorldPosition.ChunkQuarterWidth);
                copy.Fill(bankMaterial);

                floor.SubFromSide(opposite, WallOffset + BankWallThickness);

                // Create entrance.
                copy = walls;
                copy.AddPosition(entranceDir, WorldPosition.ChunkQuarterWidth - BankWallThickness - WallOffset);
                copy.AddToSide(entranceDir, BankWallThickness);
                copy.AddToLateralEnds(entranceDir, BankHalfEntranceWidth);
                copy.Size.Y = BankEntranceHeight;
                copy.CreateArchway(entranceDir, bankDecorationMaterial, roadOutlineWidth, 1);

                // Add a sign.
                WorldVolume signPos = walls;
                signPos.SetLateralWidthCentered(entranceDir, 1);
                signPos.AddPosition(CubeFace.Ypositive, BankEntranceHeight + roadOutlineWidth + 2); // TODO 5 as const var
                signPos.Size.Y = 1;
                signPos.AddPosition(entranceDir, WorldPosition.ChunkQuarterWidth - WallOffset);
                signPos.AddToSide(entranceDir, 1);
                if (entranceDir == Dir4.N || entranceDir == Dir4.W)
                {
                    signPos.AddPosition(lib.GetLateralRightFacing(entranceDir), 1);
                }

                ArchitecturalLib.CreateWordPainting(lib.Invert(entranceDir), signPos, bankDecorationMaterial, BankSign);

                // Floor.
                floor.Fill(bankFloorMaterial);
                // Entrance flooring.
                floor.SubFromLateralEnds(entranceDir, WorldPosition.ChunkQuarterWidth - BankHalfEntranceWidth);
                floor.AddToSide(entranceDir, WallOffset + BankWallThickness);
                floor.Fill(bankFloorMaterial);
            }
            floor.Fill(bankFloorMaterial);
        }

        /// <summary>
        /// Creates a piece of a church on the given world position
        /// </summary>
        /// <param name="type">The type (Church or ChurchEntrance)</param>
        /// <param name="wp">The world position</param>
        /// <param name="dirs">What directions to build walls in. Holds "Special" when middle column (higher roof).</param>
        private void CreateChurch(UrbanSquareType type, WorldPosition wp, DirectionFlags dirs)
        {
            int ChurchHalfEntranceWidth = roadHalfWidth - roadOutlineWidth;

            WorldVolume floor = new WorldVolume(wp, new IntVector3(WorldPosition.ChunkHalfWidth, 1, WorldPosition.ChunkHalfWidth));
            WorldVolume copy = floor;
            copy.AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
            floor.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight - 1;

            var facings = lib.DirectionFlagsToFacing4(dirs);

            wp.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
            wp.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
            WorldVolume walls = new WorldVolume(wp, IntVector3.Zero);
            walls.AddToSide(CubeFace.Ypositive, ChurchWallHeight);

            WorldVolume cornerPillar = walls;
            foreach (var face in facings)
            {
                // Walls.
                copy = walls;
                copy.AddPosition(face, WorldPosition.ChunkQuarterWidth - ChurchWallThickness - WallOffset);
                copy.AddToSide(face, ChurchWallThickness);
                copy.AddToLateralEnds(face, WorldPosition.ChunkQuarterWidth);
                copy.Fill(churchMaterial);

                // Corner pillars.
                cornerPillar.AddPosition(face, WorldPosition.ChunkQuarterWidth - ChurchWallThickness - WallOffset);
                cornerPillar.AddToSide(face, ChurchWallThickness + WallOffset);

                floor.SubFromSide(face, WallOffset + ChurchWallThickness);

                if (type == UrbanSquareType.ChurchEntrance)
                {
                    // Entrance.
                    Dir4 entranceDir = facings[0];
                    Dir4 opposite = lib.Invert(entranceDir);
                    copy.SubFromLateralEnds(entranceDir, WorldPosition.ChunkQuarterWidth - ChurchHalfEntranceWidth);
                    copy.Size.Y = ChurchEntranceHeight;
                    copy.CreateArchway(entranceDir, churchDecorationMaterial, roadOutlineWidth, 1);

                    WorldVolume signPos = walls;
                    signPos.SetLateralWidthCentered(face, 1);
                    signPos.AddPosition(CubeFace.Ypositive, ChurchEntranceHeight + roadOutlineWidth + 2); // TODO 5 as const var
                    signPos.Size.Y = 1;
                    signPos.AddPosition(face, WorldPosition.ChunkQuarterWidth - WallOffset);
                    signPos.AddToSide(face, 1);
                    if (entranceDir == Dir4.N || entranceDir == Dir4.W)
                    {
                        signPos.AddPosition(lib.GetLateralRightFacing(entranceDir), 1);
                    }

                    ArchitecturalLib.CreateWordPainting(lib.Invert(face), signPos, churchDecorationMaterial, ChurchSign);

                    // Floor.
                    floor.Fill(churchFloorMaterial);
                    // Entrance flooring.
                    floor.SubFromLateralEnds(face, WorldPosition.ChunkQuarterWidth - ChurchHalfEntranceWidth);
                    floor.AddToSide(face, WallOffset + ChurchWallThickness);
                    floor.Fill(churchFloorMaterial);
                }
            }
            cornerPillar.Fill(churchDecorationMaterial);
            floor.Fill(churchFloorMaterial);
        }

        /// <summary>
        /// Creates a town hall
        /// </summary>
        /// <param name="dirs">direction flags from squareDetail[index]</param>
        /// <param name="wp">World position of square</param>
        private void CreateTownHall(DirectionFlags dirs, WorldPosition wp)
        {
            int TownHallHalfEntranceWidth = roadHalfWidth - roadOutlineWidth;

            WorldVolume full = new WorldVolume(wp, new IntVector3(WorldPosition.ChunkHalfWidth, 1, WorldPosition.ChunkHalfWidth));
            WorldVolume floor = full;
            WorldVolume copy = floor;
            copy.AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
            floor.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight - 1;

            var facings = lib.DirectionFlagsToFacing4(dirs);

            wp.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
            wp.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
            WorldVolume walls = new WorldVolume(wp, IntVector3.Zero);
            walls.AddToSide(CubeFace.Ypositive, TownHallWallHeight);

            WorldVolume cornerPillar = walls;
            foreach (var face in facings)
            {
                // Walls.
                copy = walls;
                copy.AddPosition(face, WorldPosition.ChunkQuarterWidth - TownHallWallThickness - WallOffset);
                copy.AddToSide(face, TownHallWallThickness);
                copy.AddToLateralEnds(face, WorldPosition.ChunkQuarterWidth);
                copy.Fill(townHallMaterial);

                // Corner pillars.
                cornerPillar.AddPosition(face, WorldPosition.ChunkQuarterWidth - TownHallWallThickness - WallOffset);
                cornerPillar.AddToSide(face, TownHallWallThickness + WallOffset);

                floor.SubFromSide(face, WallOffset + TownHallWallThickness);

                if ((dirs & DirectionFlags.Special) == DirectionFlags.Special)
                {
                    // Entrance.
                    Dir4 entranceDir = facings[0];
                    Dir4 opposite = lib.Invert(entranceDir);
                    copy.SubFromLateralEnds(entranceDir, WorldPosition.ChunkQuarterWidth - TownHallHalfEntranceWidth);
                    copy.Size.Y = TownHallEntranceHeight;
                    copy.CreateArchway(entranceDir, townHallDecorationMaterial, roadOutlineWidth, 1);

                    WorldVolume signPos = walls;
                    signPos.SetLateralWidthCentered(face, 1);
                    signPos.AddPosition(CubeFace.Ypositive, TownHallEntranceHeight + roadOutlineWidth + 2); // TODO 5 as const var
                    signPos.Size.Y = 1;
                    signPos.AddPosition(face, WorldPosition.ChunkQuarterWidth - WallOffset);
                    signPos.AddToSide(face, 1);
                    if (entranceDir == Dir4.N || entranceDir == Dir4.W)
                    {
                        signPos.AddPosition(lib.GetLateralRightFacing(entranceDir), 1);
                    }

                    ArchitecturalLib.CreateWordPainting(lib.Invert(face), signPos, townHallDecorationMaterial, name);

                    // Extended floor.
                    floor.Fill(townHallFloorMaterial);
                    floor.SubFromLateralEnds(face, WorldPosition.ChunkQuarterWidth - TownHallHalfEntranceWidth);
                    floor.AddToSide(face, WallOffset + TownHallWallThickness);
                    floor.Fill(townHallFloorMaterial);
                }
            }
            cornerPillar.Fill(townHallDecorationMaterial);
            floor.Fill(townHallFloorMaterial);

            
            Dir4 facing = lib.GetLateralRightFacing(facings[facings.Count - 1]);
            WorldVolume roof = full;
            roof.AddPosition(CubeFace.Ypositive, TownHallWallHeight);
            roof.AddToSide(CubeFace.Ypositive, 7);
            roof.FillRoof(Data.MaterialType.red, 1.0f, Dir4.E, 0.0f);
        }

        /// <summary>
        /// Creates a city entrance
        /// </summary>
        /// <param name="wp">The position, denoted as topleft corner of half chunk-sized square</param>
        /// <param name="direction">Outward facing</param>
        private void CreateCityEntrance(WorldPosition wp, Dir4 direction)
        {
            CreateWall(direction, wp);
            WorldVolume arch = new WorldVolume(wp, IntVector3.Zero);
            arch.AddPosition(Dir4.E, WorldPosition.ChunkQuarterWidth);
            arch.AddPosition(Dir4.S, WorldPosition.ChunkQuarterWidth);
            arch.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
            arch.AddToLateralEnds(direction, roadHalfWidth);
            arch.AddToLongitudinalEnds(direction, halfWallThickness);
            arch.AddToSide(CubeFace.Ypositive, wallHeight - 3);
            arch.CreateArchway(direction, allureMaterial, 1, 1); // Do not randomise last two parameters (arch width & overhang)
        }

        private static void CreateNPC(List<Data.Characters.NPCdata> npcsData, GameObjects.EnvironmentObj.MapChunkObjectType npcType, WorldPosition wp, bool gameObjects)
        {
            CreateNPC(npcsData,npcType, wp, Dir4.S, Dir4.S, gameObjects);
        }
        /// <summary>
        /// Creates a NPC at the given position
        /// </summary>
        /// <param name="originalDirection">The intial facing of a workstation, if any</param>
        /// <param name="direction">The current facing of the workstation, (Dir4)detail</param>
        /// <param name="npcType">NPC type, i.e. Salesman</param>
        /// <param name="wp">The given position</param>
        private static void CreateNPC(List<Data.Characters.NPCdata> npcsData, GameObjects.EnvironmentObj.MapChunkObjectType npcType, 
            WorldPosition wp, Dir4 originalDirection, Dir4 direction, bool gameObjects)
        {
            Data.Characters.NPCdata data = null;
            if (npcType == GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC || npcType == GameObjects.EnvironmentObj.MapChunkObjectType.Guard)
            {
                data = new Data.Characters.NPCdata(npcType, WorldPosition.EmptyPos, false);
            }
            else
            {
                foreach (Data.Characters.NPCdata d in npcsData)
                {
                    if (d.MapChunkObjectType == npcType)
                    {
                        data = d;
                        break;
                    }
                }
                if (data == null)
                {
                    if (PlatformSettings.ViewErrorWarnings)
                        throw new Exception();
                    
                    return;
                }
            }

            if (gameObjects)
            {
                data.wp = wp;
                data.generate(); //.BeginGeneratorRequest(wp.ChunkGrindex);
            }
            else
            {
                VoxelObjGridData stationImg = data.WorkingStation;
                if (stationImg != null)
                {
                    if (originalDirection != direction)
                    {
                        stationImg.Rotate(((int)originalDirection + stationImg.Rotation + (int)direction) % 4, stationImg.Size - IntVector3.One);
                    }
                    stationImg.BuildOnTerrain(wp);
                }
            }
           
        }

        /// <summary>
        /// Creates a single tile of wall
        /// </summary>
        /// <param name="direction">Outward facing</param>
        /// <param name="wp">Top left corner of half chunk-sized square.</param> 
        private void CreateWall(Dir4 direction, WorldPosition wp)
        {
            // Create wall foundation.
            WorldVolume wall = new WorldVolume(wp, IntVector3.Zero);
            wall.Position.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
            wall.Position.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
            wall.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
            wall.AddToLongitudinalEnds(direction, halfWallThickness);
            wall.AddToLateralEnds(direction, WorldPosition.ChunkQuarterWidth);
            wall.AddToSide(CubeFace.Ypositive, wallHeight).AddToSide(CubeFace.Ynegative, halfWallThickness * 2 - parapetThickness).Fill(wallMaterial);

            // Create allure.
            int floorThickness = 1;
            wall.SubFromSide(CubeFace.Ynegative, wallHeight - floorThickness).SubFromSide(direction, floorThickness).Fill(allureMaterial);

            // Create parapet.
            wall.SubFromSide(lib.Invert(direction), halfWallThickness * 2 - parapetThickness);
            wall.AddToSide(CubeFace.Ypositive, parapetHeight).Fill(wallMaterial);

            // Create merlons.
            Dir4 perp = lib.GetPositivePerpendicularFacing(direction);
            wall.Size.SetDimension(perp, 1);
            wall.AddPosition(CubeFace.Ypositive, 1);
            wall.AddToSide(CubeFace.Ypositive, merlonHeight - 1).Fill(wallMaterial);
            wall.AddPosition(perp, 3);
            wall.AddToSide(perp, 1);
            for (int i = 0; i != 3; ++i)
            {
                wall.Fill(wallMaterial);
                wall.AddPosition(perp, 4);
            }
            wall.SubFromSide(perp, 1).Fill(wallMaterial);
        }

        /// <summary>
        /// Constructs a tower at the given world position, using it as a top left corner.
        /// </summary>
        /// <param name="entranceDirection">Facing of entrance. Pass NO_DIR to avoid creating one.</param>
        /// <param name="wp">Position of top left (north west) corner.</param>
        private void CreateTower(Dir4 entranceDirection, WorldPosition wp, IntVector2 squarePos)
        {
            WorldVolume tower = new WorldVolume(wp, IntVector3.Zero);
            tower.AddPosition(CubeFace.Xpositive, WorldPosition.ChunkQuarterWidth);
            tower.AddPosition(CubeFace.Zpositive, WorldPosition.ChunkQuarterWidth);
            tower.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
            tower.AddToXZEnds(WorldPosition.ChunkQuarterWidth);
            WorldVolume floor = tower;
            // Create tower.
            tower.AddToSide(CubeFace.Ypositive, towerHeight);
            tower.AddToSide(CubeFace.Ynegative, 5).FillCylinder(wallMaterial, CubeFace.Ypositive, CubeFace.NUM, 1.1f);
            tower.SubFromSide(CubeFace.Ynegative, 5);
            tower.SubFromXZEnds(towerThickness).FillCylinder(Data.MaterialType.NO_MATERIAL, CubeFace.Ypositive, CubeFace.NUM, 1.1f);
            floor.AddToSide(CubeFace.Ynegative, 1).FillCylinder(towerFloorMaterial, CubeFace.Ypositive, CubeFace.NUM, 1);
            floor.SubFromXZEnds(towerThickness).AddPosition(CubeFace.Ypositive, wallHeight).FillCylinder(towerFloorMaterial, CubeFace.Ypositive, CubeFace.NUM, 1.1f);

            // Create entrance creation volume.
            WorldVolume entrance = new WorldVolume(wp, IntVector3.Zero);
            entrance.AddPosition(CubeFace.Xpositive, WorldPosition.ChunkQuarterWidth);
            entrance.AddPosition(CubeFace.Zpositive, WorldPosition.ChunkQuarterWidth);
            entrance.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight;
            WorldVolume copy;

            // Construct a main entrance.
            if (entranceDirection != Dir4.NUM_NON)
            {
                copy = entrance;
                copy.AddToSide(CubeFace.Ypositive, Players.Player.CharacterHeight + 1);
                copy.AddToSide(entranceDirection, WorldPosition.ChunkQuarterWidth + towerEntranceWidth - 1);
                copy.AddToLateralEnds(entranceDirection, towerEntranceWidth / 2).FillArch(entranceDirection, Data.MaterialType.NO_MATERIAL);
            }

            entrance.AddPosition(CubeFace.Ypositive, wallHeight);
            // Create wall entrances if beside wall or entrance
            var entrances = new List<Dir4>();
            if (CheckForWall(Math.Max(0, squarePos.X - 1), squarePos.Y))
                entrances.Add(Dir4.W);
            if (CheckForWall(Math.Min(squares.GetLength(0) - 1, squarePos.X + 1), squarePos.Y))
                entrances.Add(Dir4.E);
            if (CheckForWall(squarePos.X, Math.Max(0, squarePos.Y - 1)))
                entrances.Add(Dir4.N);
            if (CheckForWall(squarePos.X, Math.Min(squares.GetLength(1) - 1, squarePos.Y + 1)))
                entrances.Add(Dir4.S);
            foreach (Dir4 dir in entrances)
            {
                copy = entrance;
                copy.AddToSide(CubeFace.Ypositive, Players.Player.CharacterHeight + 1);
                copy.AddToSide(dir, WorldPosition.ChunkQuarterWidth + towerEntranceWidth - 1);
                copy.AddToLateralEnds(dir, towerEntranceWidth / 2).FillArch(dir, Data.MaterialType.NO_MATERIAL);
                copy.SetVerticalHeight(1).AddPosition(CubeFace.Ynegative, 1).Fill(allureMaterial);
            }
        }

        /// <summary>
        /// Creates a single piece of road and connects it to others around it
        /// </summary>
        /// <param name="wp">Top left corner of half chunk-sized squares</param>
        /// <param name="squarePos">What square to build road in</param>
        private void CreateRoadPiece(WorldPosition wp, IntVector2 squarePos)
        {
            // Create a foundational road piece
            WorldVolume road = new WorldVolume(wp, IntVector3.Zero);
            road.Position.WorldGrindex.X += WorldPosition.ChunkQuarterWidth;
            road.Position.WorldGrindex.Z += WorldPosition.ChunkQuarterWidth;
            road.Position.WorldGrindex.Y = WorldPosition.ChunkStandardHeight - 1;
            road.AddToXZEnds(roadHalfWidth);
            road.AddToSide(CubeFace.Ypositive, 1).Fill(roadOutlineMaterial);
            WorldVolume clear = road;
            clear.AddPosition(CubeFace.Ypositive, 1).AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
            WorldVolume copy = road;
            copy.SubFromXZEnds(roadOutlineWidth).Fill(roadMaterial);

            // Find connections
            const byte TopLeft = 1;
            const byte Top = 2;
            const byte TopRight = 4;
            const byte Left = 8;
            const byte Right = 16;
            const byte LowerLeft = 32;
            const byte Lower = 64;
            const byte LowerRight = 128;
            byte neighbors = 0;
            if (CheckForRoad(Math.Max(0, squarePos.X - 1), Math.Max(0, squarePos.Y - 1)))
                neighbors += TopLeft;
            if (CheckForRoad(squarePos.X, Math.Max(0, squarePos.Y - 1)))
                neighbors += Top;
            if (CheckForRoad(Math.Min(squares.GetLength(0) - 1, squarePos.X + 1), Math.Max(0, squarePos.Y - 1)))
                neighbors += TopRight;
            if (CheckForRoad(Math.Max(0, squarePos.X - 1), squarePos.Y))
                neighbors += Left;
            if (CheckForRoad(Math.Min(squares.GetLength(0) - 1, squarePos.X + 1), squarePos.Y))
                neighbors += Right;
            if (CheckForRoad(Math.Max(0, squarePos.X - 1), Math.Min(squares.GetLength(1) - 1, squarePos.Y + 1)))
                neighbors += LowerLeft;
            if (CheckForRoad(squarePos.X, Math.Min(squares.GetLength(1) - 1, squarePos.Y + 1)))
                neighbors += Lower;
            if (CheckForRoad(Math.Min(squares.GetLength(0) - 1, squarePos.X + 1), Math.Min(squares.GetLength(1) - 1, squarePos.Y + 1)))
                neighbors += LowerRight;

            var potential = new List<Dir4>();
            var confirmed = new List<Dir4>();

            if (neighbors == 120)
            {
                bool yay = true;
            }
            var sub = new List<Dir4> { Dir4.E, Dir4.N, Dir4.W, Dir4.S };

            // If top left, top and left then corner, else check top and left individually
            if ((neighbors & (TopLeft + Top + Left)) == (TopLeft + Top + Left))
            {
                copy = road;
                copy.AddToSide(Dir4.W, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.AddToSide(Dir4.N, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.Fill(roadOutlineMaterial);
                clear = copy;
                clear.AddPosition(CubeFace.Ypositive, 1).AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
                sub.Remove(Dir4.W);
                sub.Remove(Dir4.N);
                copy.SubFromSide(Dir4.E, roadOutlineWidth);
                copy.SubFromSide(Dir4.S, roadOutlineWidth);
                copy.Fill(roadMaterial);
            }
            else
            {
                if ((neighbors & Top) == Top)
                    potential.Add(Dir4.N);
                if ((neighbors & Left) == Left)
                    potential.Add(Dir4.W);
            }

            // If top right, top and right then corner, else check top and right individually
            if ((neighbors & (TopRight + Top + Right)) == (TopRight + Top + Right))
            {
                copy = road;
                copy.AddToSide(Dir4.E, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.AddToSide(Dir4.N, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.Fill(roadOutlineMaterial);
                clear = copy;
                clear.AddPosition(CubeFace.Ypositive, 1).AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
                sub.Remove(Dir4.E);
                sub.Remove(Dir4.N);
                if (sub.Contains(Dir4.W))
                    copy.SubFromSide(Dir4.W, roadOutlineWidth);
                if (sub.Contains(Dir4.S))
                    copy.SubFromSide(Dir4.S, roadOutlineWidth);
                copy.Fill(roadMaterial);
            }
            else
            {
                if ((neighbors & Top) == Top && potential.Contains(Dir4.N))
                    confirmed.Add(Dir4.N);
                if ((neighbors & Right) == Right)
                    potential.Add(Dir4.E);
            }

            // If lower left, lower and left then corner, else check lower and left individually
            if ((neighbors & (LowerLeft + Lower + Left)) == (LowerLeft + Lower + Left))
            {
                copy = road;
                copy.AddToSide(Dir4.W, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.AddToSide(Dir4.S, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.Fill(roadOutlineMaterial);
                clear = copy;
                clear.AddPosition(CubeFace.Ypositive, 1).AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
                sub.Remove(Dir4.W);
                sub.Remove(Dir4.S);
                if (sub.Contains(Dir4.E))
                    copy.SubFromSide(Dir4.E, roadOutlineWidth);
                if (sub.Contains(Dir4.N))
                    copy.SubFromSide(Dir4.N, roadOutlineWidth);
                copy.Fill(roadMaterial);
            }
            else
            {
                if ((neighbors & Lower) == Lower)
                    potential.Add(Dir4.S);
                if ((neighbors & Left) == Left && potential.Contains(Dir4.W))
                    confirmed.Add(Dir4.W);
            }

            // If lower right, lower and right then corner, else check lower and right individually
            if ((neighbors & (LowerRight + Lower + Right)) == (LowerRight + Lower + Right))
            {
                copy = road;
                copy.AddToSide(Dir4.E, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.AddToSide(Dir4.S, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.Fill(roadOutlineMaterial);
                clear = copy;
                clear.AddPosition(CubeFace.Ypositive, 1).AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
                sub.Remove(Dir4.E);
                sub.Remove(Dir4.S);
                if (sub.Contains(Dir4.W))
                    copy.SubFromSide(Dir4.W, roadOutlineWidth);
                if (sub.Contains(Dir4.N))
                    copy.SubFromSide(Dir4.N, roadOutlineWidth);
                copy.Fill(roadMaterial);
            }
            else
            {
                if ((neighbors & Lower) == Lower && potential.Contains(Dir4.S))
                    confirmed.Add(Dir4.S);
                if ((neighbors & Right) == Right && potential.Contains(Dir4.E))
                    confirmed.Add(Dir4.E);
            }

            foreach (var dir in confirmed)
            {
                copy = road;
                copy.AddToSide(dir, WorldPosition.ChunkQuarterWidth - roadHalfWidth);
                copy.SubFromSide(lib.Invert(dir), roadHalfWidth * 2 - roadOutlineWidth);
                copy.Fill(roadOutlineMaterial);
                clear = copy;
                clear.AddPosition(CubeFace.Ypositive, 1).AddToSide(CubeFace.Ypositive, 10).Fill(Data.MaterialType.NO_MATERIAL);
                copy.SubFromLateralEnds(dir, roadOutlineWidth).Fill(roadMaterial);
            }
        }

        // Helper methods.
        /// <summary>
        /// Helper method to find the closest point of main road
        /// </summary>
        /// <param name="cityEntrance">One of the city's entrances</param>
        /// <param name="position">Where to find the road from</param>
        /// <param name="roadDirection">The direction in which the main road is found</param>
        /// <returns></returns>
        private IntVector2 FindClosestRoad(IntVector2 cityEntrance, IntVector2 position, out Dir4 roadDirection)
        {
            IntVector2 closestRoad = cityEntrance;
            if (lib.Dir4ToDimensions(entranceDir) == Dimensions.X)
            {
                closestRoad.X = position.X;
                int diff = closestRoad.Y - position.Y;
                if (diff > 0)
                    roadDirection = Dir4.S;
                else
                    roadDirection = Dir4.N;
            }
            else
            {
                closestRoad.Y = position.Y;
                int diff = closestRoad.X - position.X;
                if (diff > 0)
                    roadDirection = Dir4.E;
                else
                    roadDirection = Dir4.W;
            }

            return closestRoad;
        }

        /// <summary>
        /// Returns true if the coordinates denote a wall in the array "squares"
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <returns>Whether the coordinates denote a wall in the array "squares" or not</returns>
        private bool CheckForWall(int x, int y)
        {
            return (squares[x, y] == (byte)UrbanSquareType.OuterWall ||
                    squares[x, y] == (byte)UrbanSquareType.CityEntrance);
        }

        private bool CheckForRoad(int x, int y)
        {
            return CheckForRoad(x, y, false);
        }

        /// <summary>
        /// Returns true if the coordinates denote a road in the array "squares"
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <returns>Whether the coordinates denote a road in the array "squares" or not</returns>
        private bool CheckForRoad(int x, int y, bool onlyPureRoad) //= false)
        {
            if (x < 0 || x > squares.GetLength(0) - 1 ||
                y < 0 || y > squares.GetLength(1) - 1)
                return false;

            bool connectTownHall = squares[x, y] == (byte)UrbanSquareType.TownHall &&
               ((squareDetail[x, y] & (byte)DirectionFlags.Special) == (byte)DirectionFlags.Special);
            bool connectChurch = squares[x, y] == (byte)UrbanSquareType.ChurchEntrance;
            bool connectBank = squares[x, y] == (byte)UrbanSquareType.Bank &&
               ((squareDetail[x, y] & (byte)DirectionFlags.Special) == (byte)DirectionFlags.Special);

            return (squares[x, y] == (byte)UrbanSquareType.Road ||
                       (onlyPureRoad ?
                        false :
                        (squares[x, y] == (byte)UrbanSquareType.SquareCenter ||
                        squares[x, y] == (byte)UrbanSquareType.CityEntrance ||
                        connectTownHall || connectChurch || connectBank)));
        }

        /// <summary>
        /// Returns true if the coordinates denote a town hall in the array "squares"
        /// </summary>
        /// <param name="x">x coord</param>
        /// <param name="y">y coord</param>
        /// <returns>Whether the coordinates denote a town hall in the array "squares" or not</returns>
        private bool CheckForTownHall(int x, int y)
        {
            return squares[x, y] == (byte)UrbanSquareType.TownHall;
        }

        /// <summary>
        /// Checks for emptiness in the given square
        /// </summary>
        /// <param name="pos">local position in array squares</param>
        /// <returns>bool indicating success</returns>
        private bool CheckForEmpty(IntVector2 pos)
        {
            if ((MathHelper.Clamp(pos.X, 0, squares.GetLength(0) - 1) != pos.X) ||
                 (MathHelper.Clamp(pos.Y, 0, squares.GetLength(1) - 1) != pos.Y))
                return false;

            return squares[pos.X, pos.Y] == (byte)UrbanSquareType.NOTHING;
        }

        /// <summary>
        /// Sets the urban type in the given square
        /// </summary>
        /// <param name="pos">local position in array squares</param>
        /// <param name="type">Type to set</param>
        private void SetTileType(IntVector2 pos, UrbanSquareType type)
        {
            if ((MathHelper.Clamp(pos.X, 0, squares.GetLength(0) - 1) != pos.X) ||
                 (MathHelper.Clamp(pos.Y, 0, squares.GetLength(1) - 1) != pos.Y))
                return;

            squares[pos.X, pos.Y] = (byte)type;
        }

        /// <summary>
        /// Sets the detail in the given square
        /// </summary>
        /// <param name="pos">local position in array squareDetail</param>
        /// <param name="type">Detail to set</param>
        private void SetTileDetail(IntVector2 pos, byte detail)
        {
            if ((MathHelper.Clamp(pos.X, 0, squares.GetLength(0) - 1) != pos.X) ||
                 (MathHelper.Clamp(pos.Y, 0, squares.GetLength(1) - 1) != pos.Y))
                return;

            squareDetail[pos.X, pos.Y] = detail;
        }

        public override SpriteName MiniMapIcon { get { return SpriteName.IconTown; } }
        public override UrbanType Type { get { return UrbanType.City; } }
        public override bool MonsterFree { get { return true; } }
    }
}
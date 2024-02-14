using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    /// <summary>
    /// Small friendly outpost
    /// 
    /// TODO: (in order, from top to bottom)
    /// // Structural
    /// - Palisade
    /// - Palisade entrances
    /// - Road generation
    /// - House base (only walls)
    /// - Farms
    /// // Polish
    /// - House roof types
    /// - House windows
    /// - Furniture in houses
    /// </summary>
    class Village : AbsUrban
    {

        List<Data.MaterialType> wallMaterials;
        List<Data.MaterialType> roofMaterials;
        List<Data.MaterialType> frameMaterials;
        AlgoObjects.HouseSettings houseSettings;

        static readonly List<GameObjects.EnvironmentObj.MapChunkObjectType> WorkerNPCs = new List<GameObjects.EnvironmentObj.MapChunkObjectType>
        {
            GameObjects.EnvironmentObj.MapChunkObjectType.Armor_smith,
            GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker,
            GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith,
            GameObjects.EnvironmentObj.MapChunkObjectType.Cook,
        };


        public Village(IntVector2 position, int level, List<Data.Characters.QuestTip> tips,
            List<GameObjects.EnvironmentObj.MapChunkObjectType> allCitiesWorkerDistribution)
            : base(new IntVector2(4), position, level)
        {
            List<Data.MaterialType> villageWallMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.gray_bricks, Data.MaterialType.red_bricks, 
                Data.MaterialType.brown, Data.MaterialType.mossy_stone, 
                Data.MaterialType.red_brown, Data.MaterialType.sand_stone_bricks, 
                Data.MaterialType.cobble_stone, Data.MaterialType.wood, 

            };
            List<Data.MaterialType> villageRoofMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
                Data.MaterialType.dark_skin, Data.MaterialType.orange, 
                Data.MaterialType.red_orange, Data.MaterialType.wood_growing, 
                Data.MaterialType.red_roof,

            };
            List<Data.MaterialType> villageWinFramMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.dark_blue, Data.MaterialType.dark_gray, 
                Data.MaterialType.dark_skin, Data.MaterialType.wood_growing, 
                Data.MaterialType.wood, Data.MaterialType.black,
                Data.MaterialType.brown, Data.MaterialType.blue_gray,
            };
            List<Data.MaterialType> villageDoorMaterials = new List<Data.MaterialType>
            {
                Data.MaterialType.blue, Data.MaterialType.wood, 
                Data.MaterialType.red_brown, Data.MaterialType.red_orange, 
                Data.MaterialType.blue_gray,
            };

            const int NumMaterials = 2;
            wallMaterials = new List<Data.MaterialType>();
            roofMaterials = new List<Data.MaterialType>();
            frameMaterials = new List<Data.MaterialType>();
            for (int i = 0; i < NumMaterials; i++)
            {
                wallMaterials.Add(villageWallMaterials[Data.RandomSeed.Instance.Next(villageWallMaterials.Count)]);
                roofMaterials.Add(villageRoofMaterials[Data.RandomSeed.Instance.Next(villageRoofMaterials.Count)]);
                if (i==0 || Data.RandomSeed.Instance.BytePercent(100))
                    frameMaterials.Add(villageWinFramMaterials[Data.RandomSeed.Instance.Next(villageWinFramMaterials.Count)]);
            }



            List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs = new List<GameObjects.EnvironmentObj.MapChunkObjectType>
                {
                    GameObjects.EnvironmentObj.MapChunkObjectType.Salesman,
                    GameObjects.EnvironmentObj.MapChunkObjectType.Healer,
                    GameObjects.EnvironmentObj.MapChunkObjectType.Guard,
                    GameObjects.EnvironmentObj.MapChunkObjectType.Guard,
                    GameObjects.EnvironmentObj.MapChunkObjectType.Horse_Transport,
                    WorkerNPCs[Data.RandomSeed.Instance.Next(WorkerNPCs.Count)],
                };

            SelectRandomNPCs(npcs, null, allCitiesWorkerDistribution, 0);
     
            //specific captain
            if (level == 0)
            {
                npcs.Add(GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain);
            }

            houseSettings = new AlgoObjects.HouseSettings();
            houseSettings.Height = Data.RandomSeed.Instance.GetRandomRange(new Range(5, 6), new Range(0, 2));
            houseSettings.Length = Data.RandomSeed.Instance.GetRandomRange(new Range(8, 12), new Range(0, 4));//new Range(12, 32);
            houseSettings.Width = Data.RandomSeed.Instance.GetRandomRange(new Range(6, 10), new Range(0, 4));//new Range(10, 30);
            houseSettings.RoofHeight = Data.RandomSeed.Instance.GetRandomRange(new Range(2, 4), new Range(0, 4));//new Range(2, 8);

            houseSettings.windowSize.X = 1 + Data.RandomSeed.Instance.Next(2);
            houseSettings.windowSize.Y = houseSettings.windowSize.X + Data.RandomSeed.Instance.Next(2);
            houseSettings.WindowPercentYpos = 0.4f + Data.RandomSeed.Instance.Next(2) * 0.1f;

            houseSettings.DoorMaterial = villageDoorMaterials[Data.RandomSeed.Instance.Next(villageDoorMaterials.Count)];
            if (Data.RandomSeed.Instance.BytePercent(100))
            {
                houseSettings.DoorFrameSize = 1;
            }

            houseSettings.Village = true;

            

            List<IntVector2> roads = RoadAlgorithm((Dir4)Data.RandomSeed.Instance.Next(4));

            AddNpcsToEmptySpots(npcs);
            createNPCData(npcs, LootfestLib.TipsPerVillage, tips);
            //addQuestTipsToNPCs(npcs, LootfestLib.TipsPerVillage, tips);

            sourroundRoadWithHouses(roads);
            //go through the empty positions and add fences and such
            IntVector2 pos = IntVector2.Zero;
            for (pos.Y = 0; pos.Y < squareSize.Y; pos.Y++)
            {
                for (pos.X = 0; pos.X < squareSize.X; pos.X++)
                {
                    if (squares[pos.X, pos.Y] == 0)
                    {
                        byte rnd = Data.RandomSeed.Instance.NextByte();
                        if (rnd < 50)
                        {
                            squares[pos.X, pos.Y] = (byte)UrbanSquareType.FenceYard;
                        }
                        else if (rnd < 60)
                        {
                            squares[pos.X, pos.Y] = (byte)UrbanSquareType.House;
                            squareDetail[pos.X, pos.Y] = (byte)Data.RandomSeed.Instance.Next(4);
                        }
                        else if (rnd < 80)
                        {
                            squares[pos.X, pos.Y] = (byte)UrbanSquareType.TownsPeople;
                        }
                    }
                }
            }
        }

        
        public override SpriteName MiniMapIcon
        {
            get { return SpriteName.IconVillage; }
        }
        protected override void BuildObject(UrbanSquareType type, byte detail, WorldPosition wp, IntVector2 squarePos, IntVector2 detailPos, bool gameObjects)
        {
            //sand över hela, jord under stängsel

            switch (type)
            {
                case UrbanSquareType.House:
                    if (!gameObjects)
                    {
                        generatehouse(wp, (Dir4)detail, squarePos, detailPos, houseSettings,
                            wallMaterials, roofMaterials, frameMaterials);
                    }
                    break;
                default:
                    base.BuildObject(type, detail, wp, squarePos, detailPos, gameObjects);
                    break;

            }
        }

        protected override void generate()
        {
            //base.generate();
            //generatehouse();
        }

        

        override protected Data.MaterialType roadMaterial
        {
            get { return Data.MaterialType.sand; }
        }
        override public UrbanType Type { get { return UrbanType.Village; } }
        override public bool MonsterFree { get { return true; } }
    }

    enum RoadType
    {
        StraightHori,
        StraightVerti,
        NUM
    }
   
}

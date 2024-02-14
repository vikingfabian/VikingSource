using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Voxels;

namespace VikingEngine.LF2.Map.Terrain.Area
{
    
    abstract class AbsUrban : AbsArea
    {
        #region PreLoaded

        protected static VoxelObjListData fence = null;
        //protected static VoxelObjListData[] cityWall4dir;
        //protected static VoxelObjListData[] cityWallTowers;

        public static VoxelObjGridData homeHouse;
        public static VoxelObjGridData privateHouse;
        public static VoxelObjGridData BuildAreaModel1;
        public static VoxelObjGridData BuildAreaModel2;
        public static VoxelObjGridData BuildAreaModel3;


        public static void LoadImages()
        {
            //cityWall4dir = loadVoxelObjectIn4dir(VoxelModelName.TownWall, false);
            //cityWallTowers = loadVoxelObjectIn4dir(VoxelModelName.TownWallTower, false);
            fence = LoadVoxelObj(VoxelModelName.FenceYard);
            
            homeHouse =  Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.homehouse)[0];
            privateHouse =  Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.privatehouse)[0];

            BuildAreaModel1 = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.buildarea1)[0];
            BuildAreaModel2 = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.buildarea2)[0];
            BuildAreaModel3 = Editor.VoxelObjDataLoader.LoadVoxelObjGrid(VoxelModelName.buildarea3)[0];

        }

        #endregion


        const int SquaresPerChunk = 2;
        const int SquareWidth = WorldPosition.ChunkWidth / SquaresPerChunk;
        protected const int HalfSquare = SquareWidth / PublicConstants.Twice;

        //int level;
        protected byte[,] squares;
        protected byte[,] squareDetail;
        protected IntVector2 squareSize;
        protected IntVector2 chunkSize;
        
        //protected IntVector2 localEntrancePos;
        protected List<Data.Characters.NPCdata> NPCdata = new List<Data.Characters.NPCdata>();


        protected Dir4 entranceDir = Dir4.N;
        protected string name = TextLib.EmptyString;

        public AbsUrban(IntVector2 chunkSize, IntVector2 position, int level)
        {
            this.areaLevel = level;
            this.position = position;
            this.chunkSize = chunkSize;
            squareSize = new IntVector2(chunkSize.X * SquaresPerChunk, chunkSize.Y * SquaresPerChunk);
            squares = new byte[squareSize.X, squareSize.Y];
            squareDetail = new byte[squareSize.X, squareSize.Y];
            //Add to minimap
            MiniMapData.Locations.Add(this);

            //urban name
            List<string> letters = nameLetters();
            int numLetters = 2;
            if (Data.RandomSeed.Instance.Bool())
                numLetters++;
            for (int i = 0; i < numLetters; i++)
            {
                name += letters[Data.RandomSeed.Instance.Next(letters.Count)];
            }

            char first = char.ToUpper(name[0]);
           

            name = Type.ToString() + " of " + first + name.Remove(0, 1);
        }

        //abstract protected string UrbanName

        virtual protected List<string> nameLetters()
        {
            return new List<string>

            {
                "wu", "wa", "re","ra","ru","run","ran","rem","ta","tal","tam","tar", 
                "pa","par","pal","pan","pam","pe","pen","pes","pet",
                "de","den","del","dew","ga","gan","gas","gar","gom","gos","ge","ges","he","ha","ho",
                "la","las","lam","lan","lo","lot","le","les",
                "na","ne","nas","no","nom","ma","me","mas","mo",
                "be","bes","ba","bal", "bo",
            };
        }

        override public void GenerateToStorage() 
        {
            //System.Diagnostics.Debug.WriteLine("GENERATE: " + this.ToString());
            //borde öppna och stänga chunksen som är involverade och spara dom
            IntVector2 pos = IntVector2.Zero;
            LfRef.chunks.OpenChunkArea(new Rectangle2(position, chunkSize), true);
            generate();
            for (pos.X = position.X; pos.X < position.X + chunkSize.X; pos.X++)
            {
                for (pos.Y = position.Y; pos.Y < position.Y + chunkSize.Y; pos.Y++)
                {
                    LfRef.chunks.GetScreen(pos).SaveData(true, null, false); //.SaveUnthreaded();
                    LfRef.chunks.RemoveChunk(pos);
                }
            }
        }
        virtual protected void generate()
        {

        }



        protected void AddNpcsToEmptySpots(List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs)
        {
            //give them tip here
            

            //list squares
            IntVector2 pos = IntVector2.Zero;
            List<IntVector2> freePositions = new List<IntVector2>();
            ForXYLoop loop = new ForXYLoop(IntVector2.Zero, squareSize - 1);
            while (!loop.Done)
            {
                pos = loop.Next_Old();
                if (squares[pos.X, pos.Y] == 0)
                {
                    freePositions.Add(pos);
                }
            }
            if (PlatformSettings.RunningWindows)
            {
                if (freePositions.Count < npcs.Count)
                    throw new Exception();
            }


            for (int i = 0; i < npcs.Count; i++)
            {
                //Data.Characters.NPCdata data = Data.Characters.NPCdata.GetNPC(npcs[i], Data.Characters.NPCdataArgs.GenerateLater);
                ////if (i < numTips)
                ////{
                ////    int tipIx = Data.RandomSeed.Instance.Next(tips.Count);
                ////    data.QuestTip = tips[tipIx];
                ////    tips.RemoveAt(tipIx);
                ////}
                //NPCdata.Add(data);

                
                int ix = Data.RandomSeed.Instance.Next(freePositions.Count);
                pos = freePositions[ix];
                freePositions.RemoveAt(ix);

                squares[pos.X, pos.Y] = (byte)UrbanSquareType.TalkingNPC;
                squareDetail[pos.X, pos.Y] = (byte)i;
                       
            }
        }

        /// <summary>
        /// Giving each npc type a startup data containing if they have a quest tip
        /// </summary>
        protected void createNPCData(List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs, int numTips, List<Data.Characters.QuestTip> tips)
        {
            //List<int> tipGivers = new List<GameObjects.EnvironmentObj.MapChunkObjectType>();
            if (numTips > npcs.Count) throw new Exception("not enough npcs to give tips");
            
            bool[] tipGivers = new bool[npcs.Count];

            do
            {
                int npcIx = Data.RandomSeed.Instance.Next(npcs.Count);
                GameObjects.EnvironmentObj.MapChunkObjectType npc = npcs[npcIx];
                if (!tipGivers[npcIx] &&
                   npc != GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.Guard &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.Guard_Captain &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.Granpa &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.War_veteran &&
                    npc != GameObjects.EnvironmentObj.MapChunkObjectType.High_priest)
                {
                    --numTips;
                    tipGivers[npcIx] = true;
                }
            } while (numTips > 0);

            for (int i = 0; i < npcs.Count; i++)
            {
                Data.Characters.NPCdata data = new Data.Characters.NPCdata(npcs[i], WorldPosition.EmptyPos, false);//Data.Characters.NPCdata.GetNPC(npcs[i], Data.Characters.NPCdataArgs.GenerateLater);
                if (tipGivers[i])
                {
                    int tipIx = Data.RandomSeed.Instance.Next(tips.Count);
                    data.QuestTip = tips[tipIx];
                    tips.RemoveAt(tipIx);
                }
                NPCdata.Add(data);
            }
        }

        //protected void addQuestTipsToNPCs(List<GameObjects.EnvironmentObj.MapChunkObjectType> npcs, int numTips, List<Data.Characters.QuestTip> tips)
        //{
        //    List<GameObjects.EnvironmentObj.MapChunkObjectType> tipGivers = new List<GameObjects.EnvironmentObj.MapChunkObjectType>();
        //    do
        //    {
        //        int npcIx = Data.RandomSeed.Instance.Next(npcs.Count);
        //        GameObjects.EnvironmentObj.MapChunkObjectType npc = npcs[npcIx];
        //        if (npc != GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC &&
        //            npc != GameObjects.EnvironmentObj.MapChunkObjectType.Guard)
        //        {
        //            tipGivers.Add(npc);
        //            npcs.RemoveAt(npcIx);
        //        }
        //    } while (tipGivers.Count < numTips);

        //    npcs.InsertRange(0, tipGivers);
        //    //for (int i = 0; i < npcs.Count || i < numTips; i++)
        //    //{
        //    //    Data.Characters.NPCdata data = Data.Characters.NPCdata.GetNPC(npcs[i], Data.Characters.NPCdataArgs.GenerateLater);
               
        //    //    int tipIx = Data.RandomSeed.Instance.Next(tips.Count);
        //    //    data.QuestTip = tips[tipIx];
        //    //    tips.RemoveAt(tipIx);
                
        //    //    NPCdata.Add(data);
        //    //}
        //}


        protected void sourroundRoadWithHouses(List<IntVector2> roadPieces)
        {
            RangeIntV2 range = new RangeIntV2(IntVector2.Zero, squareSize - 1);
            IntVector2 pos;

            foreach (IntVector2 road in roadPieces)
            {
                //foreach (IntVector2 dir in lib.IntV2FourDirections)
                for (int dir = 0; dir < lib.IntV2FourDirections.Count; dir++)
                {
                    pos = road + lib.IntV2FourDirections[dir];
                    if (range.Interect(pos) && squares[pos.X, pos.Y] == 0 &&
                        Data.RandomSeed.Instance.BytePercent(170))
                    {
                        squares[pos.X, pos.Y] = (byte)UrbanSquareType.House;
                        int houseDir = lib.SetBoundsRollover(dir + 2, 0, 3);
                        squareDetail[pos.X, pos.Y] = (byte)lib.SetBoundsRollover(dir + 2, 0, 3);
                    }
                }
            }
        }

        protected List<IntVector2> RoadAlgorithm(Dir4 entrance)
        {
            List<IntVector2> roadPoses = new List<IntVector2>();

            IntVector2 pos = squareSize / 2; //center
            IntVector2 step = IntVector2.FromDir4(entrance);
            RangeIntV2 range = new RangeIntV2(IntVector2.Zero, squareSize - 1);

            //draw a line from center and out to the entrance
            while (range.Interect(pos))
            {
                roadPoses.Add(pos);
                squares[pos.X, pos.Y] = (byte)UrbanSquareType.Road;
                pos += step;
            }

            //and some random roadpieces, like branches on a tree
            int numRoadBranches = squareSize.SideLength(); /// 2;
            for (int i = 0; i < numRoadBranches; i++)
            {
                //pick random road piece
                pos = roadPoses[Data.RandomSeed.Instance.Next(roadPoses.Count)];
                step = lib.Dir4ToIntVec2((Dir4)Data.RandomSeed.Instance.Next(4), 1);
                pos += step;
                int branchLenght = Data.RandomSeed.Instance.Next(4);
                for (int b = 0; b < branchLenght; b++)
                {
                    if (range.Interect(pos) && squares[pos.X, pos.Y] == 0)
                    {
                        roadPoses.Add(pos);
                        squares[pos.X, pos.Y] = (byte)UrbanSquareType.Road;
                        pos += step;
                    }
                    else
                        break;
                }
            }
            return roadPoses;
        }

        //public override IntVector2 AreaChunkCenter
        //{
        //    get { return new IntVector2(position.X + ChunkSize.X / PublicConstants.Twice, 
        //        position.Y + ChunkSize.Y / PublicConstants.Twice); }
        //}


        override public IntVector2 TravelEntrance { get { return position + chunkSize.GetMidSide(entranceDir); } }

        static int SquareToBlockPos(int squareIx)
        {
            //return HalfSquare + squareIx * SquareWitdh;
            return squareIx * SquareWidth;
        }
        public static WorldPosition SquareToWP(IntVector2 chunkIx, IntVector2 square)
        {
            Map.WorldPosition wp = new WorldPosition(chunkIx);
            wp.X += SquareToBlockPos(square.X);
            wp.Y = Map.WorldPosition.ChunkStandardHeight;
            wp.Z += SquareToBlockPos(square.Y);
            if (PlatformSettings.RunningWindows)
            {
                if (!wp.CorrectPos)
                    throw new Exception();
            }
            return wp;
        }

        public override IntVector2 ChunkSize
        {
            get { return chunkSize; }
        }

        //public IntVector2 MapLocationChunk
        //{ get { return AreaChunkCenter; } }
 


        //abstract public SpriteName MiniMapIcon { get; }
        override public string MapLocationName { get { return name; } }

        public override void BuildOnChunk(Map.Chunk chunk, bool dataGenerated, bool gameObjects)
        {
            IntVector2 startIx = chunk.Index;
            startIx.Sub(position);
            startIx.Multiply(SquaresPerChunk);
            IntVector2 end = startIx;
            end.Add(SquaresPerChunk);

            IntVector2 squarePos = IntVector2.Zero;
            IntVector2 detailPos = IntVector2.Zero;
            WorldPosition wp = WorldPosition.EmptyPos;

            wp.Y = Map.WorldPosition.ChunkStandardHeight;
            int blockz = 0;
            for (squarePos.Y = startIx.Y; squarePos.Y < end.Y; squarePos.Y++)
            {
                detailPos.X = 0;
                wp.Z = SquareToBlockPos(blockz) + chunk.Index.Y * Map.WorldPosition.ChunkWidth;
                blockz++;
                int blockx = 0;
                for (squarePos.X = startIx.X; squarePos.X < end.X; squarePos.X++)
                {
                    if (squares[squarePos.X, squarePos.Y] != 0)
                    {
                        wp.X = SquareToBlockPos(blockx) + chunk.Index.X * Map.WorldPosition.ChunkWidth;
                        this.BuildObject((UrbanSquareType)squares[squarePos.X, squarePos.Y], 
                            squareDetail[squarePos.X, squarePos.Y], wp, squarePos, detailPos, gameObjects);
                    }
                    blockx++;
                    detailPos.X++;
                }
                detailPos.Y++;
            }
        }
        static protected void generatehouse(WorldPosition wp, Dir4 dir,
            IntVector2 squarePos, IntVector2 detailsPos, AlgoObjects.HouseSettings houseSettings,
            List<Data.MaterialType> wallMaterials, List<Data.MaterialType> roofMaterials, List<Data.MaterialType> frameMaterials)
        {
            Data.RandomSeedInstance seedInstance = new Data.RandomSeedInstance();
            seedInstance.SetSeedPosition(squarePos + detailsPos * 3);

            //AlgoObjects.HouseSettings houseSettings = new AlgoObjects.HouseSettings();
            houseSettings.WallMaterial = wallMaterials[Data.RandomSeed.Instance.Next(wallMaterials.Count)];
            houseSettings.RoofMaterial = roofMaterials[Data.RandomSeed.Instance.Next(roofMaterials.Count)];
            houseSettings.windowFrameMaterial = frameMaterials[Data.RandomSeed.Instance.Next(frameMaterials.Count)];

            houseSettings.DoorDir = dir;
            AlgoObjects.House.Generate(wp, houseSettings, seedInstance);
        }
        protected void changeSquareSurface(WorldPosition wp, byte toMaterial)
        {
            Map.Chunk screen = LfRef.chunks.GetScreen(wp);
            IntVector2 pos = IntVector2.Zero;
            IntVector2 startPos = new IntVector2(wp.LocalBlockX, wp.LocalBlockZ);
            IntVector2 maxPos = startPos;
            maxPos.Add(Map.WorldPosition.ChunkHalfWidth);
            //byte to = (byte)roadMaterial;

            for (pos.Y = startPos.Y; pos.Y < maxPos.Y; pos.Y++)
            {
                for (pos.X = startPos.X; pos.X < maxPos.X; pos.X++)
                {
                    if (Data.RandomSeed.Instance.BytePercent(200))
                        screen.ReplaceFirstGroundBlock(pos, toMaterial);
                }
            }
        }


        virtual protected void BuildObject(UrbanSquareType type, byte detail, WorldPosition wp, IntVector2 squarePos, IntVector2 detailPos, bool gameObjects)
        {


            switch (type)
            {
                case UrbanSquareType.Road:


                    if (gameObjects && LfRef.gamestate.GameObjCollection.SpawnOptionalGameObject())
                    {

                        if ((Type == UrbanType.City || Type == UrbanType.Village) && Ref.rnd.RandomChance(40))
                        {
                            //LfRef.chunks.GetScreen(wp).AddChunkObject(new Data.Characters.BasicNPCData(), true);
                            new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC, wp, true);
                        }
                    }
                    else
                    {
                        changeSquareSurface(wp, (byte)roadMaterial);
                    }

                    break;

                case UrbanSquareType.TownsPeople:
                    if (gameObjects && LfRef.gamestate.GameObjCollection.SpawnOptionalGameObject())
                    {
                        new Data.Characters.NPCdata(GameObjects.EnvironmentObj.MapChunkObjectType.BasicNPC, wp, true);
                    }
                    break;

                case UrbanSquareType.TalkingNPC:
                    if (NPCdata[detail] != null)
                    {
                        //Build npc object
                        if (NPCdata[detail].MapChunkObjectType == GameObjects.EnvironmentObj.MapChunkObjectType.Healer)
                        {
                            lib.DoNothing();
                        }

                        if (gameObjects)
                        {
                            NPCdata[detail].wp = wp;
                            NPCdata[detail].generate();
                        }
                        else
                        {
                            VoxelObjGridData stationImg = NPCdata[detail].WorkingStation;
                            if (stationImg != null)
                            {
                                stationImg.BuildOnTerrain(wp);
                            }
                        }
                    }
                    break;

                case UrbanSquareType.FenceYard:
                    if (gameObjects && LfRef.gamestate.GameObjCollection.SpawnOptionalGameObject())
                    {
                        LfRef.chunks.GetScreen(wp).AddChunkObject(new GameObjects.EnvironmentObj.CritterSpawn(wp), true);
                    }
                    else
                    {
                        changeSquareSurface(wp, (byte)Data.MaterialType.dirt);
                        fence.BuildOnTerrain(wp);
                    }
                    

                    
                    

                    break;
            }
        }


        /// <param name="allNpcs">The selected npcs will be added to this list</param>
        /// <param name="rndNPCs">Pick random npcs from this list</param>
        /// <param name="allCitiesWorkerDistribution">pick one oblicatory npc to make sure all show up in the game</param>
        /// <param name="pickRndAmount">Number of random npcs to pick</param>
        protected static void SelectRandomNPCs(
            List<GameObjects.EnvironmentObj.MapChunkObjectType> allNpcs,
            List<GameObjects.EnvironmentObj.MapChunkObjectType> rndNPCs,
            List<GameObjects.EnvironmentObj.MapChunkObjectType> allCitiesWorkerDistribution,
            int pickRndAmount)
        {
            for (int i = 0; i < pickRndAmount; i++)
            {
                allNpcs.Add((GameObjects.EnvironmentObj.MapChunkObjectType)Data.RandomSeed.Instance.RandomListMemeberRemove(rndNPCs));
            }

            if (allCitiesWorkerDistribution.Count > 0)
            {
                GameObjects.EnvironmentObj.MapChunkObjectType rndWorker = (GameObjects.EnvironmentObj.MapChunkObjectType)Data.RandomSeed.Instance.RandomListMemeberRemove(allCitiesWorkerDistribution);
                if (!allNpcs.Contains(rndWorker))
                {
                    allNpcs.Add(rndWorker);
                }
            }
            //return allNpcs;
        }
        virtual protected Data.MaterialType roadMaterial
        {
            get { return Data.MaterialType.cobble_stone; }
        }
        protected static VoxelObjListData LoadVoxelObj(VoxelModelName name)
        {
            return new VoxelObjListData(Editor.VoxelObjDataLoader.LoadVoxelObjGrid(name)[0]);
        }
        protected static VoxelObjListData[] loadVoxelObjectIn4dir(VoxelModelName name, bool chunkSz)
        {
            VoxelObjListData[] obj = new VoxelObjListData[(int)Dir4.NUM_NON];
            for (int i = 0; i < (int)Dir4.NUM_NON; i++)
            {
                obj[i] = LoadVoxelObj(name);
                obj[i].Rotate(i,  chunkSz ? WorldPosition.ChunkLimits.Max : Editor.VoxelObjDataLoader.StandardLimits);
            }
            return obj;
        }
        abstract public UrbanType Type { get; }

        public override string ToString()
        {
            return name;
        }
        //virtual protected int NumTips { get { return 0; } }
        override public bool TravelTo { get { return true; } }
    }
    enum UrbanType
    {
        Village,
        City,
        Castle,
        DebugCity,
        Fort,
    }
    enum UrbanSquareType : byte
    {
        NOTHING,
        Other,
        Road,
        House,
        OuterWall,
        CastleWall,
        InnerWall,
        RingTower,
        CastleTower,
        CastleDoor,
        //GuardPost,
        TalkingNPC,
        TownsPeople,
        FenceYard,
        RotatingTrap,
        TownHall,
        Church,
        ChurchEntrance,
        Bank,
        Stable, 
        SquareCenter,
        SquareStand,
        CityEntrance,
        Cook,

        FortWall,
        NUM
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.Map
{
    /*
     * Kan ha en noise map med prickar för att bestämma mönstret på städer och andra utsridda grejer
     *  -prickarna har alltid mellanrum, prickar med högre värde har större mellanrum
     * -vore annars smart att bygga en stats överblick, en 2dgrid som flera chunks kan dela på
     */
    class WorldOverview
    {
        //public static WorldOverview Instance;

        const int AreasDivitions = 8;
        const int AreasHalfDivitions = AreasDivitions / PublicConstants.Twice;

        public const int WorldChunkEdge = 60;
        public const int AreaRadiusX = ((WorldPosition.WorldChunksX - WorldChunkEdge * PublicConstants.Twice) / AreasDivitions) / PublicConstants.Twice;
        public const int AreaRadiusY = ((WorldPosition.WorldChunksY - WorldChunkEdge * PublicConstants.Twice) / AreasDivitions) / PublicConstants.Twice;

        public Terrain.TopographicSetup Topographics;
        public Director.EnvironmentObjectQue EnvironmentObjectQue;

        Dictionary<Terrain.AreaType, Terrain.Area.AbsArea[]> areas;
        public Dictionary<Terrain.AreaType, Terrain.AreaInfo[]> AreasInfo;
        public IntVector2 StartChunk;
        public Dictionary<Terrain.EnvironmentType, GameObjects.EnvironmentObj.EnvironmentType> EnvironmentToHerbType; 
        Data.Characters.MagicianData[] bosses = new Data.Characters.MagicianData[LootfestLib.BossCount];
        public Data.Characters.MagicianData Boss(int lvl)
        { return bosses[lvl]; }
        
        ChunkData[,] chunks;

        

        public WorldOverview(Map.World world)
        {
            LfRef.worldOverView = this;
            EnvironmentObjectQue = new Director.EnvironmentObjectQue(world);

            Data.RandomSeed.Instance.SetSeedPosition(200);
            //the random herb treasures are locked to an evironment
            EnvironmentToHerbType = new Dictionary<Terrain.EnvironmentType,GameObjects.EnvironmentObj.EnvironmentType>();
            List<Terrain.EnvironmentType> growingAreas = new List<Terrain.EnvironmentType>
            {
                Terrain.EnvironmentType.Desert, 
                Terrain.EnvironmentType.Forest, 
                Terrain.EnvironmentType.Grassfield, 
                Terrain.EnvironmentType.Mountains, 
                Terrain.EnvironmentType.Swamp,

            };
            List<GameObjects.EnvironmentObj.EnvironmentType> herbs = new List<GameObjects.EnvironmentObj.EnvironmentType>
            {
                GameObjects.EnvironmentObj.EnvironmentType.BloodFingerHerb,
                GameObjects.EnvironmentObj.EnvironmentType.BlueRoseHerb,
                GameObjects.EnvironmentObj.EnvironmentType.FireStarHerb,
                GameObjects.EnvironmentObj.EnvironmentType.FrogHeartHerb,
            };

            
            //Will make sure at least on of these craftsmen will be in the world
            List<GameObjects.EnvironmentObj.MapChunkObjectType> allCitiesWorkerDistribution = new List<GameObjects.EnvironmentObj.MapChunkObjectType>
            {
                GameObjects.EnvironmentObj.MapChunkObjectType.Armor_smith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Bow_maker,
                GameObjects.EnvironmentObj.MapChunkObjectType.Weapon_Smith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Volcan_smith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Healer,

                GameObjects.EnvironmentObj.MapChunkObjectType.Blacksmith,
                GameObjects.EnvironmentObj.MapChunkObjectType.Tailor,

            };

            //if (PlatformSettings.ViewUnderConstructionStuff)
            //{
            //    allCitiesWorkerDistribution.Add(GameObjects.EnvironmentObj.MapChunkObjectType.High_priest);
            //}


            foreach (GameObjects.EnvironmentObj.EnvironmentType h in herbs)
            {
                int ix = Data.RandomSeed.Instance.Next(growingAreas.Count);
                EnvironmentToHerbType.Add(growingAreas[ix], h);
                growingAreas.RemoveAt(ix);
            }

            Data.RandomSeed.Instance.SetSeedPosition(100);

            int startCorner = Data.RandomSeed.Instance.Next(4);
            List<IntVector2> cornerPos = new List<IntVector2>
            {
                IntVector2.Zero, new IntVector2(1, 0),
                new IntVector2(0, 1), IntVector2.One,
            };
            Rectangle startArea = Rectangle.Empty;
            startArea.X = cornerPos[startCorner].X * AreasHalfDivitions;
            startArea.Y = cornerPos[startCorner].Y * AreasHalfDivitions;
            startArea.Width = AreasHalfDivitions;
            startArea.Height = AreasHalfDivitions;

            areas = new Dictionary<Terrain.AreaType,Terrain.Area.AbsArea[]>();
            AreasInfo = new Dictionary<Terrain.AreaType, Terrain.AreaInfo[]>();
            MiniMapData.NewWorld();
            chunks = new ChunkData[WorldPosition.WorldChunksX, WorldPosition.WorldChunksY];
            IntVector2 pos = IntVector2.Zero;

            
            Topographics = new Terrain.TopographicSetup();
            //Lägg ut små spots med terräng typer
            //borde göra versions check
            List<Terrain.EnvironmentType> environments = new List<Terrain.EnvironmentType>
            {
                Terrain.EnvironmentType.Mountains,
                Terrain.EnvironmentType.Burned,
                Terrain.EnvironmentType.Swamp,
                Terrain.EnvironmentType.Forest,
                Terrain.EnvironmentType.Desert,
            };


            byte[] drawEdgeChance = new byte[]
            {
                80,
                120,
                180,
            };


            Range numTerrainAreasRange = new Range(  4,9 );

            foreach (Terrain.EnvironmentType env in environments)
            {
                int num = Data.RandomSeed.Instance.Next(numTerrainAreasRange);

                for (int i = 0; i < num; i++)
                {
                    const int EnvironmentSizeAdd = Map.WorldPosition.WorldChunksX / 4;
                    const int EnvironmentMinSize = 6;//2;

                    IntVector2 size = new IntVector2(
                        Data.RandomSeed.Instance.Next(EnvironmentSizeAdd) + EnvironmentMinSize,
                        Data.RandomSeed.Instance.Next(EnvironmentSizeAdd) + EnvironmentMinSize);

                    IntVector2 envStartPos = new IntVector2(
                         Data.RandomSeed.Instance.Next(WorldPosition.WorldChunksX - size.X),
                         Data.RandomSeed.Instance.Next(WorldPosition.WorldChunksX - size.Y));
                    IntVector2 endPos = envStartPos + size - 1;
                    for (pos.Y = envStartPos.Y; pos.Y <= endPos.Y; pos.Y++)
                    {
                        int edgeY = lib.SmallestValue((pos.Y - envStartPos.Y), -(pos.Y - endPos.Y));
                        for (pos.X = envStartPos.X; pos.X <= endPos.X; pos.X++)
                        {
                            int edgeX = lib.SmallestValue((pos.X - envStartPos.X), -(pos.X - endPos.X));
                            int edge = lib.SmallestValue(edgeX, edgeY);
                            if (edge >= 3 || Data.RandomSeed.Instance.NextByte() < drawEdgeChance[edge])
                            {
                                chunks[pos.X, pos.Y] = new ChunkData(env);
                            }
                        }
                    }
                }
            }

#region QUEST_TIPS

            List<Data.Characters.QuestTip> tips = new List<Data.Characters.QuestTip>();
            for (int i = 1; i < LootfestLib.NumVillages; i++)
            {
                tips.Add(new Data.Characters.QuestTip(Data.Characters.QuestTipType.Village, i));
            }
            for (int i = 2; i < LootfestLib.NumCities; i++)
            {
                tips.Add(new Data.Characters.QuestTip(Data.Characters.QuestTipType.City, i));
            }
            for (int i = 1; i < LootfestLib.BossCount; i++)
            {
                tips.Add(new Data.Characters.QuestTip(Data.Characters.QuestTipType.BossImmunity, i));
                tips.Add(new Data.Characters.QuestTip(Data.Characters.QuestTipType.BossLocation, i));
                tips.Add(new Data.Characters.QuestTip(Data.Characters.QuestTipType.BossWeakness, i));
            }

            for (int i = 0; i < LootfestLib.NestsTips; i++)
            {
                tips.Add(new Data.Characters.QuestTip(Data.Characters.QuestTipType.Nest, i + 1));
            }

#endregion
            bool[,] usedArea = new bool[AreasDivitions, AreasDivitions];

            Terrain.AreaType[] generateOrder = new Terrain.AreaType[]
            {
                Terrain.AreaType.HomeBase,
                Terrain.AreaType.FreeBuild,
                Terrain.AreaType.Village,
                Terrain.AreaType.City,
                Terrain.AreaType.PrivateHome,
                Terrain.AreaType.Lumberjack,
                Terrain.AreaType.Miner,

                Terrain.AreaType.EnemySpawn,
                Terrain.AreaType.Castle,
                Terrain.AreaType.FarmOutpost,
                Terrain.AreaType.SalesmanOutpost,
                Terrain.AreaType.SoldierOutpost,
                Terrain.AreaType.TravelOutpost,
                Terrain.AreaType.EndTomb,
                Terrain.AreaType.Fort,
                Terrain.AreaType.WiseLady,
                Terrain.AreaType.ImError,

                //Terrain.AreaType.DebugCity,
            };

            //for (Terrain.AreaType atype = (Terrain.AreaType)0; atype < Terrain.AreaType.NUM_NON; atype++)
            foreach (Terrain.AreaType atype in generateOrder)

            {
                int numAreas;
                int useOnlyStartArea = 0;
                switch (atype)
                {
                    default:
                        numAreas = 3;
                        break;

//#if DEBUGMODE
                    case Terrain.AreaType.DebugCity:
                        numAreas = 1;
                        break;

//#endif
                    case Terrain.AreaType.ImError:
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.FarmOutpost:
                        numAreas = 4;
                        break;
                    case Terrain.AreaType.Lumberjack:
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.WiseLady:
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.Miner:
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.SalesmanOutpost:
                        numAreas = 4;
                        break;
                    case Terrain.AreaType.SoldierOutpost:
                        numAreas = 4;
                        break;
                    case Terrain.AreaType.TravelOutpost:
                        numAreas = 4;
                        break;
                    case Terrain.AreaType.Castle:
                        numAreas = LootfestLib.BossCount;
                        break;
                    case Terrain.AreaType.City:
                        useOnlyStartArea = 1;

                        numAreas = LootfestLib.NumCities;

                        break;
                    case Terrain.AreaType.Village:
                        useOnlyStartArea = 1;

                        numAreas = LootfestLib.NumVillages;

                        break;
                    case Terrain.AreaType.HomeBase:
                        useOnlyStartArea = int.MaxValue;
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.FreeBuild:
                        useOnlyStartArea = int.MaxValue;
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.EnemySpawn:
                        numAreas = 4;
                        
                        break;
                    case Terrain.AreaType.EndTomb:
                        numAreas = 1;
                        break;
                    case Terrain.AreaType.Fort:
                        numAreas = 5;
                        break;
                    case Terrain.AreaType.PrivateHome:
                        numAreas = Ref.netSession.maxGamers;
                        break;
                }
                areas.Add(atype, new Terrain.Area.AbsArea[numAreas]);
                AreasInfo.Add(atype, new Terrain.AreaInfo[numAreas]);

                for (byte areaLevel = 0; areaLevel < numAreas; areaLevel++)
                {
                    Rectangle useArea;
                    if (useOnlyStartArea > 0)
                    {
                        useArea = startArea;
                        useOnlyStartArea--;
                    }
                    else
                    {
                        useArea = Rectangle.Empty;
                        useArea.Width = AreasDivitions;
                        useArea.Height = AreasDivitions;

                    }
                    
                    
                    int numLoops = 0;
                    do
                    {

                        pos.X = useArea.X + Data.RandomSeed.Instance.Next(useArea.Width);
                        pos.Y = useArea.Y + Data.RandomSeed.Instance.Next(useArea.Height);
#if WINDOWS
                        numLoops++;
                        if (numLoops > 400)
                        {
                            throw new Debug.EndlessLoopException("Set area type");
                        }
#endif
                    } while (usedArea[pos.X, pos.Y]);

                    usedArea[pos.X, pos.Y] = true;
                    //ska lägga lite slump på positionen här
                    pos.X = ((pos.X * PublicConstants.Twice + 1) * AreaRadiusX) + WorldChunkEdge;
                    pos.Y = ((pos.Y * PublicConstants.Twice + 1) * AreaRadiusY) + WorldChunkEdge;

                    IntVector2 areaSize = IntVector2.One;

                    Terrain.Area.AbsArea area = null;
                    Data.RandomSeed.Instance.SetSeedPosition(pos);
                    switch (atype)
                    {
                        case Terrain.AreaType.TravelOutpost:
                            area = new Terrain.Area.Travel(pos, areaLevel);
                            break;
                        case Terrain.AreaType.SoldierOutpost:
                            area = new Terrain.Area.SoldierPost(pos, areaLevel);
                            break;
                        case Terrain.AreaType.ImError:
                            area = new Terrain.Area.ImError(pos, areaLevel);
                            break;
                        case Terrain.AreaType.Lumberjack:
                            area = new Terrain.Area.Salesman(pos, areaLevel, GameObjects.EnvironmentObj.MapChunkObjectType.Lumberjack);
                            break;
                        case Terrain.AreaType.WiseLady:
                            area = new Terrain.Area.Wiselady(pos, areaLevel);
                            break;

                        case Terrain.AreaType.Miner:
                            area = new Terrain.Area.Salesman(pos, areaLevel, GameObjects.EnvironmentObj.MapChunkObjectType.Miner);
                            break;
                        case Terrain.AreaType.SalesmanOutpost:
                            area = new Terrain.Area.Salesman(pos, areaLevel, GameObjects.EnvironmentObj.MapChunkObjectType.Salesman);
                            break;
                        case Terrain.AreaType.FarmOutpost:
                            area = new Terrain.Area.Farm(pos, areaLevel);
                            break;
                        case Terrain.AreaType.Castle:
                            bosses[areaLevel] = new Data.Characters.MagicianData(areaLevel);
                            area = new Terrain.Area.Castle(pos, areaLevel);
                            break;
                        case Terrain.AreaType.City:
                            area = new Terrain.Area.City(pos, areaLevel, tips, allCitiesWorkerDistribution);
                            break;
                        case Terrain.AreaType.Village:
                            area = new Terrain.Area.Village(pos, areaLevel, tips, allCitiesWorkerDistribution);
                            break;

                        case Terrain.AreaType.EnemySpawn:
                            area = new Terrain.Area.MonsterSpawn(pos, areaLevel);
                            break;

                        case Terrain.AreaType.HomeBase:
                            StartChunk = pos;
                            area = new Terrain.Area.HomeBase(pos, areaLevel);

                            Rectangle2 grassArea = new Rectangle2(StartChunk, 5);
                            ForXYLoop loop = new ForXYLoop(grassArea);
                            while (loop.Next())
                            {
                                if (!loop.AtEdge || Data.RandomSeed.Instance.BytePercent(150))
                                {
                                    
                                    ChunkData cd = chunks[loop.Position.X, loop.Position.Y];
                                    cd.Environment = Terrain.EnvironmentType.Grassfield;
                                    chunks[loop.Position.X, loop.Position.Y] = cd;
                                }
                            }
                            break;
                        case Terrain.AreaType.FreeBuild:
                            StartChunk = pos;
                            area = new Terrain.Area.FreeBuild(pos);
                            break;
                        case Terrain.AreaType.EndTomb:
                            area = new Terrain.Area.EndTomb(pos);
                            break;

                        case Terrain.AreaType.DebugCity:
                            if (PlatformSettings.DevBuild)//PlatformSettings.Debug == BuildDebugLevel.DeveloperDebug_1)
                            {
                                area = new Terrain.Area.DebugCity(pos);
                            }
                            break;
                        case Terrain.AreaType.Fort:
                            area = new Terrain.Area.Fort(pos, areaLevel);
                            break;
                        case Terrain.AreaType.PrivateHome:
                            area = new Terrain.Area.PrivateHome(pos, areaLevel);

                            break;
                    }
                    if (area != null)
                    {
                        areaSize = area.ChunkSize;
                        areas[atype][areaLevel] = area;
                        AreasInfo[atype][areaLevel] = new Terrain.AreaInfo(area.AreaChunkCenter, area.ToString());

                        //if (area.MonsterFree)
                        //{
                            const int MonsterFreeRadius = 1;
                            Rectangle2 areaRect = new Rectangle2(pos,areaSize);
                            for (int r = 0; r < MonsterFreeRadius; ++r)
                            {
                                areaRect.AddRadius(1);
                                ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(areaRect);
                                
                                while (edgeLoop.Next())
                                {
                                    
                                    ChunkData data = chunks[edgeLoop.Position.X, edgeLoop.Position.Y];
                                    data.AreaType = Terrain.AreaType.FlatEmptyAndMonsterFree;
                                    chunks[edgeLoop.Position.X, edgeLoop.Position.Y] = data;
                                }
                                
                            }
                            lib.DoNothing();
                            ChunkData d = chunks[67, 103];
                            

                        //}
                    }

                    for (int y = 0; y < areaSize.Y; y++)
                    {
                        for (int x = 0; x < areaSize.X; x++)
                        {
                            ChunkData cd = chunks[pos.X + x, pos.Y + y];
                            cd.AreaType = atype;
                            cd.AreaLevel = areaLevel;
                            chunks[pos.X + x, pos.Y + y] = cd;
                        }
                    }
                    
                }
            }

            lib.DoNothing();
            ChunkData d2 = chunks[67, 103];
        }

        public void FreeMemory()
        {
            areas = null;
        }

        public void DebugUnlockAll()
        {
            for (int y = 0; y < Map.WorldPosition.WorldChunksY; y += MiniMap.ChunksDivide)
            {
                for (int x = 0; x < Map.WorldPosition.WorldChunksX; x += MiniMap.ChunksDivide)
                {
                    LfRef.gamestate.Progress.SetVisitedArea(new IntVector2(x, y), null, false);
                }
            }
        }

        public void NetworkSend(Network.SendPacketToOptions toGamer)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(
                    Network.PacketType.LF2_WorldOverview, toGamer, 
                    Network.PacketReliability.Reliable, LootfestLib.LocalHostIx);
            
            bool combining = false;
            short numCombined = 0;
            IntVector2 combStart = IntVector2.Zero;

            IntVector2 pos = IntVector2.Zero;
            
            for (pos.Y = 0; pos.Y < Map.WorldPosition.WorldChunksY; pos.Y++)
            { 
                for (pos.X = 0; pos.X < Map.WorldPosition.WorldChunksX; pos.X++)
                {
                    if (combining)
                    {
                        if (chunks[pos.X, pos.Y].changed)
                        {
                            numCombined++;
                        }
                        else
                        {
                            w.Write(numCombined);
                            Map.WorldPosition.WriteChunkGrindex_Static(combStart, w);//combStart.WriteStream(System.IO.BinaryWriter);
                            combining = false;
                        }
                    }
                    else
                    {
                        if (chunks[pos.X, pos.Y].changed)
                        {
                            combining = true;
                            numCombined = 1;
                            combStart = pos;
                        }
                    }
                }
            }

           w.Write(short.MinValue);
        }
        public void NetworkReceive(System.IO.BinaryReader r)
        {
            while (true)
            {
                short numCombined = r.ReadInt16();
                if (numCombined == short.MinValue)
                    return;

                //ShortVector2 pos = ShortVector2.FromStream(r);
                IntVector2 pos = Map.WorldPosition.ReadChunkGrindex_Static(r);
                while (numCombined > 0)
                {
                    ChunkData data = chunks[pos.X, pos.Y];
                    data.changed = true;
                    chunks[pos.X, pos.Y] = data;

                    pos.X++;
                    if (pos.X >= Map.WorldPosition.WorldChunksX)
                    {
                        pos.X = 0; pos.Y++;
                    }
                    numCombined--;
                }
            }
        }


        public void Load(string dir)
        {
           // WorldDirName = dir;
            List<string> savedChunks = DataLib.SaveLoad.FilesInStorageDir(
                Data.WorldsSummaryColl.CurrentWorld.FolderPath, "*" + Chunk.SaveFileEnding);
            foreach (string s in savedChunks)
            {
                //måste ta bort worldX//
                IntVector2 index = lib.StringToIntV2(s);
                ChunkData data = chunks[index.X, index.Y];
                data.changed = true;
                chunks[index.X, index.Y] = data;
            }
        }

        public ChunkData GetChunkData(IntVector2 index)
        {
            return chunks[index.X, index.Y];
        }
        

        public void Test()
        {
            ForXYLoop loop = new ForXYLoop(new IntVector2(chunks.GetLength(0), chunks.GetLength(1)));
            while (loop.Next())
            {
                if (chunks[loop.Position.X, loop.Position.Y].AreaType == Terrain.AreaType.Miner)
                {
                    lib.DoNothing();
                }
            }
        }

        public Terrain.Area.AbsArea GetAreaObject(ChunkData type)
        {
            if (areas.ContainsKey(type.AreaType))
            {
                return areas[type.AreaType][type.AreaLevel];
            }
            throw new Exception("Missing area:" + type.ToString());
        }
        public void UnChangedChunk(IntVector2 index)
        {
            ChunkData chunk = chunks[index.X, index.Y];
            chunk.changed = false;
            chunks[index.X, index.Y] = chunk;
        }
        public void ChangedChunk(IntVector2 index)
        {
            Chunk screen = LfRef.chunks.GetScreenUnsafe(index);
           
            ChunkData chunk = chunks[index.X, index.Y];
            chunk.changed = true;
            chunks[index.X, index.Y] = chunk;

            

            if (screen != null && !screen.UnSavedChanges)
                screen.ChangedData();
        }
        public void WriteProtectedChunk(IntVector2 index)
        {
            ChunkData chunk = chunks[index.X, index.Y];
            chunk.changed = false;
            chunks[index.X, index.Y] = chunk;
        }

        //public void ListChangedChunks(HUD.File file)
        //{
        //    for (int y = 0; y < WorldPosition.WorldChunksY; y++)
        //    {
        //        for (int x = 0; x < WorldPosition.WorldChunksX; x++)
        //        {
        //            if (chunks[x, y].changed)
        //            {
        //                file.AddTextLink(new IntVector2(x,y).ToString());
        //            }
        //        }
        //    }

        //}

        public int SetPrivateHomeOwner(Players.AbsPlayer owner)
        {
            Terrain.Area.AbsArea[] homes = areas[Terrain.AreaType.PrivateHome];
            for (int i = 0; i < homes.Length; ++i)
            {
                if (((Terrain.Area.PrivateHome)homes[i]).TrySetOwner(owner))
                {
                    return i;
                }
            }

            if (PlatformSettings.ViewErrorWarnings)
                throw new ArgumentOutOfRangeException("set private home owner to: " + owner.ToString());
            else
                return 0;
        }

        public void RecievedPrivateHomeOwner(Players.AbsPlayer owner)
        {
            Terrain.Area.AbsArea[] homes = areas[Terrain.AreaType.PrivateHome];
            ((Terrain.Area.PrivateHome)homes[owner.PrivateHomeIndex]).Owner = owner;
        }

        public void PlayerLeftEvent()
        {
            Terrain.Area.AbsArea[] homes = areas[Terrain.AreaType.PrivateHome];
            for (int i = 0; i < homes.Length; ++i)
            {
                ((Terrain.Area.PrivateHome)homes[i]).PlayerLeftEvent();
            }
        }

        public bool CanEdit(IntVector2 chunk, Players.Player player)
        {
            ChunkData data = chunks[chunk.X, chunk.Y];
            if (data.AreaType == Terrain.AreaType.FreeBuild)
                return true;
            if (data.AreaType == Terrain.AreaType.PrivateHome)
                return ChunkHasOwner(chunk) == player;

            return false;
        }

        public Players.AbsPlayer ChunkHasOwner(IntVector2 chunk)
        {
            ChunkData data = chunks[chunk.X, chunk.Y];
            if (data.AreaType == Terrain.AreaType.PrivateHome)
            {
                Terrain.Area.AbsArea[] homes = areas[Terrain.AreaType.PrivateHome];
                Terrain.Area.PrivateHome home = ((Terrain.Area.PrivateHome)homes[data.AreaLevel]);
                return home.Owner;
            }
            return null;
        }

        //public void SaveChunkToOwner(bool save, DataLib.ISaveTostorageCallback callBack, LF2.Map.Chunk chunk)
        //{
        //    ChunkData data = chunks[chunk.Index.X, chunk.Index.Y];
        //    if (data.AreaType == Terrain.AreaType.PrivateHome)
        //    {
        //        Terrain.Area.AbsArea[] homes = areas[Terrain.AreaType.PrivateHome];
        //        Terrain.Area.PrivateHome home = ((Terrain.Area.PrivateHome)homes[data.AreaLevel]);

        //        if (home.Owner != null && home.Owner.Local)
        //        {
        //            IntVector2 pos = home.ToLocalPos(chunk.Index);

        //            if (save)
        //            {
        //                DataStream.MemoryStreamHandler stream = new DataStream.MemoryStreamHandler();
        //                chunk.WriteChunk(stream.GetWriter());
        //                home.Owner.Settings.PrivateAreaData[pos.X, pos.Y] = stream;
        //                ((Players.Player)home.Owner).SettingsChanged();
        //                if (callBack != null) callBack.SaveComplete(save, int.NUM_NON, null, false);
        //            }
        //            else
        //            {
        //                bool failed = true;
        //                if (home.Owner.Settings.PrivateAreaData[pos.X, pos.Y] != null && home.Owner.Settings.PrivateAreaData[pos.X, pos.Y].HasData)
        //                {
        //                    failed = false;
        //                    chunk.BeginReadChunk(home.Owner.Settings.PrivateAreaData[pos.X, pos.Y].GetReader());
        //                }
        //                if (callBack != null) callBack.SaveComplete(save, int.NUM_NON, null, failed);
        //            }
        //        }
        //    }
        //}

        public Terrain.Area.AbsArea GetArea(IntVector2 grindex)
        {
            ChunkData data = chunks[grindex.X, grindex.Y];
            Terrain.Area.AbsArea[] areaLevels;
            if (areas.TryGetValue(data.AreaType, out areaLevels))
            {
                return areaLevels[data.AreaLevel];
            }
            return null;
        }
    }
    
    /// <summary>
    /// Data that always will be loaded
    /// </summary>
    struct ChunkData : IUpdateableStructInArray
    {
        public Terrain.EnvironmentType Environment;
        Terrain.AreaType areaType;
        public Terrain.AreaType AreaType
        {
            get { return areaType; }
            set { areaType = value;
            if (value == Terrain.AreaType.Miner)
            {
                lib.DoNothing();
            }
            }
        }
        public byte AreaLevel;
        public bool changed;


        public ChunkData(Terrain.EnvironmentType Environment)
            :this()
        {
            this.Environment = Environment;
            AreaType = Terrain.AreaType.Empty;
            AreaLevel = 0;
            changed = false;
        }
        public ChunkData(Terrain.AreaType area, byte level)
            : this()
        {
            AreaType = area; 
            AreaLevel = level;
            changed = false;
            //Environment = (byte)Terrain.EnvironmentType.Grassfield;
            Environment = Terrain.EnvironmentType.NUM_NON;
        }

        public void ChangeStructArrayValue(object overridingValues)
        {
            ChunkData o = (ChunkData)overridingValues;

            lib.SetValueIfNotEqualTo(ref areaType, o.areaType, Terrain.AreaType.NUM_NON);
            lib.SetValueIfNotEqualTo(ref Environment, o.Environment, Terrain.EnvironmentType.NUM_NON);
            lib.SetValueIfNotEqualTo(ref AreaLevel, o.AreaLevel, byte.MinValue);
            changed = changed || o.changed;
        }

        public override string ToString()
        {
            return "Area:" + AreaType.ToString() + " lvl" + AreaLevel.ToString() + ", env:" + Environment.ToString();
        }
    }

    
}

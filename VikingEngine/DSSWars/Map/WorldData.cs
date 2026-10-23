using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.Core.DSSWars.Map.MapProcess;
using VikingEngine.DataStream;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapModels;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.DSSWars.Map.Path;
using VikingEngine.DSSWars.Map.Path3;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.LootFest.Map;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.DSSWars
{   

    partial class WorldData
    {  
        public static WorldData LoadingWorld = null;

        //public const int TileSubDivitions = 8;
        //public const int HalfTileSubDivitions = TileSubDivitions / 2;
        //public const int TileSubDivitions_MaxIndex = TileSubDivitions-1;

        public static readonly Color WaterCol = new Color(14, 155, 246);
        public static readonly Color WaterCol2 = ColorExt.Multiply(WaterCol, 0.9f);
        public static readonly Color WaterEdgeColor = new Color(62, 175, 233);
        public static readonly Color WaterEdgeColorBright = ColorExt.Multiply(WaterEdgeColor, 1.1f);
        public static readonly Color WaterDarkCol = new Color(0.043f, 0.486f, 0.773f);

        public static readonly Color WaterDarkCol1 = ColorExt.Mix(WaterDarkCol, WaterEdgeColor, 0.6f);
        public static readonly Color WaterDarkCol2 = ColorExt.Multiply(WaterDarkCol1, 1.05f);
        
        public static readonly Color WaterVeryDarkCol1 = WaterDarkCol;
        public static readonly Color WaterVeryDarkCol2 = ColorExt.Multiply(WaterVeryDarkCol1, 1.05f);

        public static readonly float TileHalfWidth = 0.5f;
        

        public WorldMetaData metaData;

        public Rectangle2 maptileBounds;
        public Rectangle2 chunkBounds;
        public Rectangle2 maptileBoundsSubOne;
        public VectorRect unitBounds;
        public IntVector2 Size;

        public Vector2 unitSize;
        public Vector2 unitSizeInv;
        public IntVector2 HalfSize;
        
        public UnitCollAreaGrid unitCollAreaGrid;

        public int claimedCityCount = int.MaxValue;
        public SpottedArray<City> cities = new SpottedArray<City>(256); 
        public SpottedArray<Faction> factions;

        public bool BordersUpdated = true;
       
        public PcgRandom rnd;
       
        public int areaTileCount;
        public bool abortLoad = false;

        public List<FactionType> availableGenericAiTypes = new List<FactionType>();

        //public GenerateMapPass generatePassCompleted = GenerateMapPass.Clear;

        public List<PFaction> quickMatchFactions = null;

        public Grid2D_L<MapChunkData8_8> chunkGrid;
        public Grid2D_L<SumTile4_4> tileGrid;
        public Grid2D_L<MapTile1_1> subTileGrid;

         public float storyPlacementScale = 1f;

        public WorldData()
        {
            factions = new SpottedArray<Faction>();
            initFlagTexture();
        }

        public void DestroyCity(IntVector2 mapPos, bool buildings)
        {
            if (DssRef.world.tileGrid.TryGet(WP.MaptileToSumTile(mapPos), out var tile) && tile.pcity.HasValue())
            {
                City city = tile.pcity.City();

                ForXYLoop loop = new ForXYLoop(city.citySumTileArea);
                while (loop.Next())
                {
                    if (DssRef.world.tileGrid.TryGet(loop.Position, out var citytile))
                    {
                        if (buildings)
                        {
                            IntVector2 mapTileStart = loop.Position * SumTile4_4.TileWidth;
                            IntVector2 mapTileEnd = mapTileStart + SumTile4_4.TileWidth;
                            IntVector2 pos = IntVector2.Zero;
                            for (pos.Y = mapTileStart.Y; pos.Y < mapTileEnd.Y; pos.Y++)
                            {
                                for (pos.X = mapTileStart.X; pos.X < mapTileEnd.X; pos.X++)
                                {
                                    //if(SelectedSubTile.MayDemolish(pos, null, out _))
                                    //{                                        
                                    //    BuildLib.Demolish(city, pos, true);
                                    //    new GodBuild(pos);                                        
                                    //}
                                    destroyTile(pos);
                                }
                            }
                        }
                        
                        citytile.pcity = PCity.Empty;
                        citytile.IsBorderTile = false;
                        DssRef.world.tileGrid.Set(loop.Position, citytile);
                    }
                }

                if (!buildings)
                {
                    destroyTile(city.maptilePos);
                    destroyTile(city.citySquareSubtilePos);
                }

                cities.RemoveAt(city.myIndex);

            }

            void destroyTile(IntVector2 tilePos)
            {
                var subTile = DssRef.world.subTileGrid.Get(tilePos);
                switch (subTile.mainTerrain)
                {
                    case TerrainMainType.Building:
                    case TerrainMainType.Decor:
                    case TerrainMainType.Road:
                    case TerrainMainType.Wall:
                        subTile.mainTerrain = TerrainMainType.DefaultLand;
                        subTile.subTerrain = 0;
                        EditSubTile edit = new EditSubTile(true, true, tilePos, subTile, true, true, true);
                        edit.Submit();
                        new GodBuild(tilePos);
                        break;
            }
            }
        }

        public void AddCity(IntVector2 mapPos, CityType cityType, bool runTime, bool buildings)
        {
            City city = new City(cities.Count, mapPos, cityType, this);
            cities.Add(city);

            city.NewCitySetup(this, null);
            //if (CurrentCityComponentCount() <= cities.Count)
            //{
            //    ExpandCityComponents(cities.Count * 2);
            //}

            //unitCollAreaGrid.add(city);

            if (runTime)
            {
                if (buildings)
                {
                    CityTemplateCollection templateCollection = new CityTemplateCollection();
                    city.createBuildingSubtiles(this, templateCollection);

                    Rectangle2 refreshArea = Rectangle2.FromCenterTileAndRadius(WP.MaptileToChunk(mapPos), 1);
                    ForXYLoop loop = new ForXYLoop(refreshArea);
                    while(loop.Next())
                    { 
                        DssRef.world.chunkGrid.GetRef(loop.Position).subtileVisualEdits++;
                    }                    
                }
                else
                {
                    city.createHallSubtiles(this);
                }

                var faction = new Faction(this, FactionType.DefaultAi);
                faction.AddCity(city, true);
                new Players.AiPlayer(faction, true);
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        CityMapClaim2.CityClaim(city);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                });
                city.onEditorStart();
            }
        }

        public bool CanPlaceCity(IntVector2 mapPos)
        {
            //const int MinEdgeDistance = 3;

            IntVector2 sumTilePos = WP.MaptileToSumTile(mapPos);

            if (DssRef.world.tileGrid.TryGet(sumTilePos, out var tile))
            {
                if (tile.pcity.IsEmpty())
                {
                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        if (!DssRef.world.tileGrid.TryGet(sumTilePos+dir, out var edgetile) ||
                            edgetile.pcity.HasValue())
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public CombinedTile GetCombinedTile(IntVector2 mapTilePos)
        {
            return new CombinedTile()
            {
                mapTile = subTileGrid.Get(mapTilePos),
                sumTile = tileGrid.Get(WP.MaptileToSumTile(mapTilePos))
            };
        }
        public CombinedTile GetCombinedTileAndLean(IntVector2 mapTilePos, out int leanY)
        {
            var result = new CombinedTile()
            {
                mapTile = subTileGrid.Get(mapTilePos),
                sumTile = tileGrid.Get(WP.MaptileToSumTile(mapTilePos))
            };

            mapTilePos.Y++;

            if (result.mapTile.heightValue >= MapHeight2.MountainStarHeight && mapTilePos.Y < subTileGrid.Size.Y)
            {
                leanY = result.mapTile.heightValue - subTileGrid.Get(mapTilePos).heightValue;
            }
            else
            { 
                leanY = 0;
            }

            return result;
        }

        public bool GetMapAndSumTile_Safe(IntVector2 mapTilePos, out MapTile1_1 mapTile, out SumTile4_4 sumTile)
        {
            if (DssRef.world.maptileBounds.IntersectTilePoint(mapTilePos))
            {
                mapTile = subTileGrid.Get(mapTilePos);
                sumTile = tileGrid.Get(WP.MaptileToSumTile(mapTilePos));
                return true;
            }
            mapTile = MapTile1_1.Empty;
            sumTile = SumTile4_4.Empty;
            return false;
        }

        public WorldData(WorldMetaData metaData, Map.Map2.Map2GenerateSettings generateSettings/*, bool customEditorMap*/)//ushort seed, MapSize size)
            : this()
        {
            this.metaData = metaData;
            //if (generateSettings.storage.customSeed)
            //{ 
            //    metaData.worldId.seed = generateSettings.storage.seed;
            //    metaData.worldId.objSeed = generateSettings.storage.seed;
            //    metaData.objRnd = new PcgRandom(generateSettings.storage.seed);
            //}

            //metaData.customEditorMap = customEditorMap;
            LoadingWorld = this;

            rnd = new PcgRandom(metaData.worldId.seed);

            refreshSize(metaData.mapScale.Size(false));
            //if (generateSettings.bCustomSize)
            //{
            //    metaData.mapSize = CustomMapSizeToSize(generateSettings.customMapSize);
            //    refreshSize(generateSettings.customMapSize);
            //}
            //else
            //{
            //    refreshSize(SizeDimentions(metaData.mapSize));
            //}
        }

        public Faction faction(int index)
        { 
            var result = factions.GetIndex_Safe(index);
            if (result != null && result.isAlive)
                return result;

            return null;
        }

        public Faction findOrCreate(FactionType factionType, int setIndex)
        {
            Faction faction;
            int firstEmpty = -1;


            if (setIndex >= 0 && factions.Array[setIndex] != null && factions.Array[setIndex].factiontype != factionType)
            {
                setIndex = -1;
            }

            if (setIndex >= 0)
            {
                faction = factions.Array[setIndex];
                if (faction != null)
                {
                    faction.isAlive = true;
                    return faction;                    
                }
                else
                {
                    firstEmpty = setIndex;
                }
            }
            else
            {
                for (int i = 0; i < factions.Array.Length; ++i)
                {
                    faction = factions.Array[i];
                    if (faction != null)
                    {
                        if (faction.factiontype == factionType)
                        {
                            faction.isAlive = true;
                            return faction;
                        }
                    }
                    else if (firstEmpty < 0)
                    {
                        firstEmpty = i;
                    }
                }
            }           
            var newFaction = new Faction(DssRef.world, factionType, firstEmpty);
            newFaction.initMidGameEnter();

            new Players.AiPlayer(newFaction, true);
            //newFaction.initDiplomacy(DssRef.world);
            return newFaction;            
        }


        

       

        public void refreshSize(IntVector2 sz)
        {
            Size = sz;
            HalfSize = Size / 2;
            areaTileCount = Size.X * Size.Y;
            maptileBounds = new Rectangle2(IntVector2.Zero, Size - 1);
            maptileBoundsSubOne = maptileBounds; 
            maptileBoundsSubOne.AddRadius(-1);

            unitSize = Size.Vec * MapTile1_1.ModelScale;
            unitSizeInv = Vector2.One / unitSize;
            unitBounds = new VectorRect(Vector2.Zero, unitSize);
            unitBounds.AddRadius(-1f);

            //create grid
            subTileGrid = new Grid2D_L<MapTile1_1>(Size);
            tileGrid = new Grid2D_L<SumTile4_4>(Size / SumTile4_4.TileWidth);
            chunkGrid = new Grid2D_L<MapChunkData8_8>(Size / MapChunkData8_8.TileWidth);

            unitCollAreaGrid = new UnitCollAreaGrid(Size);

            chunkBounds = chunkGrid.TileBound();

            path3Thread_test = new Path3Thread();

            if (Size.Area() > MapScale.SizeDimentions(MapSizeCategory.Medium).Area())
            {
                storyPlacementScale = (float)MapScale.SizeDimentions(MapSizeCategory.Medium).Area() / Size.Area();
            }
        }

        bool subTileHasRepeatValue(ref MapTile1_1 subtile)
        {
            return subtile.mainTerrain == TerrainMainType.DefaultSea;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            subTileGrid.LoopBegin();
            MapTile1_1 previuos = new MapTile1_1();

            while (subTileGrid.LoopNext())
            {
                var subtile = subTileGrid.LoopValueGet();
                subtile.write(w, ref previuos);

                previuos = subtile;

            }

            Debug.WriteCheck(w);

            var citiesC = cities.counter();
            while (citiesC.Next()) //foreach (City city in cities)
            {
                citiesC.sel.writeGameState(w);
            }

            Debug.WriteCheck(w);

            writeComponents(w);

            Debug.WriteCheck(w);

            w.Write((ushort)factions.Array.Length);
            foreach (var faction in factions.Array)
            {
                if (faction != null && (faction.isAlive || faction.quickMatchFaction))
                {
                    w.Write(true);
                    faction.writeGameState(w);
                    Debug.WriteCheck(w);
                }
                else
                { 
                    w.Write(false); 
                }
            }

            if (quickMatchFactions != null)
            {
                w.Write((byte)quickMatchFactions.Count);
                foreach (var m in quickMatchFactions)
                {
                    //w.Write((ushort)m);
                    m.write(w);
                }
            }
            else
            {
                w.Write(byte.MinValue);
            }

            Debug.WriteCheck(w);

            DssRef.world.diplomacy.writeRelations(w);
            
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            availableGenericAiTypes.Clear();
            subTileGrid.LoopBegin();
            MapTile1_1 previuos = new MapTile1_1();


            while (subTileGrid.LoopNext())
            {
                MapTile1_1 subtile = subTileGrid.LoopValueGet();
                subtile.read(r, ref previuos, subversion);
                subTileGrid.LoopValueSet(subtile);

                previuos = subtile;
            }
            
            Debug.ReadCheck(r);

            var citiesC = cities.counter();
            while (citiesC.Next()) //foreach (City city in cities)
            {
                citiesC.sel.readGameState(r, subversion, pointers);
            }

            Debug.ReadCheck(r);

            readComponents(r, subversion);

            Debug.ReadCheck(r);

            int factionLegth = factions.Array.Length;
            factionLegth = r.ReadUInt16();
            
            int darkLordCount = 0;

            for (int i = 0; i < factionLegth; i++)
            {
                var f = factions.Array[i];

                if (r.ReadBoolean())
                {
                    f.readGameState(r, subversion, pointers);
                    Debug.ReadCheck(r);

                    if (f.factiontype == FactionType.DarkLord)
                    {
                        darkLordCount++;
                    }
                }
                else
                {
                    if (f != null)
                    {
                        f.factiontype = FactionType.DefaultAi;
                        var emptyPlayer = new AiPlayer(f, false);
                        f.isAlive = false;
                    }
                }
            }

            
            int quickMatchFactionsCount = r.ReadByte();
            if (quickMatchFactionsCount > 0)
            {
                quickMatchFactions = new List<PFaction>(quickMatchFactionsCount);
                for (int i = 0; i < quickMatchFactionsCount; i++)
                {
                    //int fIx = r.ReadUInt16();
                    var pf = new PFaction(r);
                    if (pf.TryGetFaction(out var f))
                    {
                        f.quickMatchFaction = true;
                        //f.displayInFullOverview = true;
                        quickMatchFactions.Add(pf);
                    }
                }
            }
            

            Debug.ReadCheck(r);

            if (subversion >= 109)
            {
                DssRef.world.diplomacy.readRelations(r, subversion);
            }
        }

        public void writeNet(System.IO.BinaryWriter w)
        {
            Size.write(w);

            w.Write(cities.Count);
            w.Write(factions.Array.Length);
        }
        public void readNet(System.IO.BinaryReader r)
        {
            Size.read(r);
            refreshSize(Size);

            int cityCount = r.ReadInt32();
            cities = new SpottedArray<City>(cityCount);
            Init_CityComponents(cityCount);
            for (int cityIndex = 0; cityIndex < cityCount; ++cityIndex)
            {
                City c = new City(cityIndex, this);
                cities.Add(c);
                c.IsNetHosted = false;
            }

            int factionCount = r.ReadInt32();
            factions = new SpottedArray<Faction>(factionCount);
            
            for (int i = 0; i < factionCount; ++i)
            {
                var faction = new Faction(i);
                //faction.initClient(this);
                factions.Add(faction);
            }
            init_FactionComponents(factionCount);

        }

        public void writeNet_Tile(System.IO.BinaryWriter w, IntVector2 tilePos)
        {
            var remotePlayerC = DssRef.state.remotePlayers.counter();

            tilePos.writeUshort(w);

            var area = new Rectangle2(tilePos, new IntVector2(RemotePlayer.OverviewSendChunkSize));
            area.SetTileBounds(DssRef.world.maptileBounds);
            ForXYLoop loop = new ForXYLoop(area);

            SumTile4_4 previous = new SumTile4_4();
            while (loop.Next())
            {
                remotePlayerC.Reset();
                while (remotePlayerC.Next())
                {
                    if (remotePlayerC.sel.networkPeer.peer.mapLoadedAndReady)
                    {
                        var remoteTile = remotePlayerC.sel.remoteTileGrid.Get(loop.Position);
                        remoteTile.overview = true;
                        remotePlayerC.sel.remoteTileGrid.Set(loop.Position, remoteTile);
                    }
                }

                var tile = DssRef.world.tileGrid.Get(loop.Position);
                tile.writeMapFile(w, previous);

                previous = tile;

            }            
        }

        public void readNet_Tile(System.IO.BinaryReader r)
        {
            IntVector2 tilePos = IntVector2.FromReadUshort(r);

            var area = new Rectangle2(tilePos, new IntVector2(RemotePlayer.OverviewSendChunkSize));
            area.SetTileBounds(DssRef.world.maptileBounds);
            ForXYLoop loop = new ForXYLoop(area);
            SumTile4_4 previous = new SumTile4_4();
            while (loop.Next())
            {
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                tile.readMapFile(r, previous, int.MaxValue);
                DssRef.world.tileGrid.Set(loop.Position, tile); 

                previous = tile;
            }

            unitCollAreaGrid.netTilesRecieved(tilePos);
        }

        public void writeNet_SubTile(System.IO.BinaryWriter w, IntVector2 tilePos)
        {
            var remotePlayerC = DssRef.state.remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                if (remotePlayerC.sel.networkPeer.peer.mapLoadedAndReady)
                {
                    var remoteTile = remotePlayerC.sel.remoteTileGrid.Get(tilePos);
                    remoteTile.detail = true;
                    remoteTile.detailTimeStamp.setTimeFromNow(TimeExt.MinuteInSeconds * 10);
                    remotePlayerC.sel.remoteTileGrid.Set(tilePos, remoteTile);
                }
            }

            tilePos.writeUshort(w);

            var area = new Rectangle2(WP.ToSubTilePos_TopLeft(tilePos), new IntVector2(MapChunkData8_8.TileWidth));
            ForXYLoop loop = new ForXYLoop(area);
            MapTile1_1 previous = new MapTile1_1();

            while (loop.Next())
            {
                var tile = DssRef.world.subTileGrid.Get(loop.Position);
                tile.write(w, ref previous);

                previous = tile;
            }
        }

        public void readNet_SubTile(System.IO.BinaryReader r)
        {
            /*
            IntVector2 tilePos = IntVector2.FromReadUshort(r);

            var area = new Rectangle2(WP.ToSubTilePos_TopLeft(tilePos), new IntVector2(MapChunkData8_8.TileWidth));
            ForXYLoop loop = new ForXYLoop(area);
            MapTile1_1 previous = new MapTile1_1();

            while (loop.Next())
            {
                ref var subtile = ref DssRef.world.subTileGrid.GetRef(loop.Position);
                subtile.read(r, ref previous, int.MaxValue);

                previous = subtile;
            }
            ref var tile = ref DssRef.world.tileGrid.GetRef(tilePos);
            tile.subtileVisualEdits++;
            if (DssRef.world.cities[tile.CityIndex].pfaction.TryGetLocalPlayer(out var lp))
            {
               lp.orders.refreshYpos();
            }
            */
        }

        public void writeNet_Factions(System.IO.BinaryWriter w, PFaction faction)
        {
            
            //int faction = factions.First();
            //w.Write((ushort)faction);
            faction.write(w);
            this.factions.Array[faction.factionIndex].writeNet(w);
            Debug.WriteCheck(w);
            
            SteamP2PManager.CrashOnTooLargePacket(w);

            var remotePlayerC = DssRef.state.remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                remotePlayerC.sel.factionsRecieved[faction.factionIndex] = true;
            }
        }

        public void readNet_Factions(System.IO.BinaryReader r)
        {
             
            int faction = r.ReadUInt16();
            this.factions.Array[faction].readNet(r);
            Debug.ReadCheck(r);
        }

        public void writeNet_Cities(System.IO.BinaryWriter w, HashSet<int> CitiesInView)
        {
            var remotePlayerC = DssRef.state.remotePlayers.counter();

            int count = Math.Min(CitiesInView.Count, 8);

            w.Write((byte)count);
            foreach (int city in CitiesInView)
            {
                w.Write((ushort)city);
                this.cities[city].writeNet_map(w);
                Debug.WriteCheck(w);

                //Debug.WriteCheck(w);
                //
                remotePlayerC.Reset();
                while (remotePlayerC.Next())
                {
                    remotePlayerC.sel.citiesRecieved[city] = true;
                }

                --count;
                if (count <= 0)
                { 
                    break;
                }
            }

            SteamP2PManager.CrashOnTooLargePacket(w);            
            
        }

        public void readNet_Cities(System.IO.BinaryReader r)
        {
            int cityCount = r.ReadByte();
            for (int i = 0; i < cityCount; i++)
            {
                int city = r.ReadUInt16();
                this.cities[city].readNet_map(this, r);
                if (this.cities[city].IsNetHosted)
                {
                    lib.DoNothing();
                }
                Debug.ReadCheck(r);
            }

            DssRef.world.BordersUpdated = true;
        }

        public void writeMapFile(System.IO.BinaryWriter w)
        {
            //DebugWriteSize tilesSz = new DebugWriteSize();
            //DebugWriteSize citiesSz = new DebugWriteSize();
            //DebugWriteSize factionsSz = new DebugWriteSize();

            const int SaveMapVersion = 12;
            w.Write(SaveMapVersion);

            w.Write(metaData.worldId.seed);
            Size.write(w);

            if (abortLoad) return;
            
            //tilesSz.begin(w);
            ForXYLoop loop = new ForXYLoop(Size);
            SumTile4_4 previous = new SumTile4_4();
            while (loop.Next())
            {
                var tile = tileGrid.Get(loop.Position);
                tile.writeMapFile(w, previous);

                previous = tile;
            }
            
            if (abortLoad) return;

            Debug.WriteCheck(w);

            if (arraylib.HasMembers(cities))
            {
                w.Write(cities.Count);
                var citiesC = cities.counter();
                while (citiesC.Next()) //foreach (var m in cities)
                {
                    citiesC.sel.writeMapFile(w);
                }


                if (abortLoad) return;

                //factionsSz.begin(w);
                SaveGamestate.MainProgress++;

                Debug.WriteCheck(w);
                //var factionsCount = factions.counter();
                w.Write(factions.Array.Length);
                //SaveGamestate.LoopProgress = 0;

                for (int i = 0; i < factions.Array.Length; ++i)
                {
                    if (factions.Array[i] != null)
                    {
                        w.Write(true);
                        w.Write((byte)factions.Array[i].StoredFactionType());
                        factions.Array[i].writeMapFile(w);
                    }
                    else
                    {
                        w.Write(false);
                    }

                    SaveGamestate.LoopProgress++;
                    Debug.WriteCheck(w);
                }
            }
            else
            {
                w.Write(0);
            }

                //lib.DoNothing();
        }

        public void readMapFile(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            metaData.worldId.seed = r.ReadUInt16();

            Size.read(r);
            refreshSize(Size);
            ForXYLoop loop = new ForXYLoop(Size);
            SumTile4_4 previous = new SumTile4_4();
            while (loop.Next())
            {
                var tile = new SumTile4_4(r, previous, version);
                tileGrid.Set(loop.Position, tile);
                previous = tile;
            }

            Debug.ReadCheck(r);

            int cityCount = r.ReadInt32();

            if (cityCount > 0)
            {
                cities = new SpottedArray<City>(cityCount);
                Init_CityComponents(cityCount);
                for (int cityIndex = 0; cityIndex < cityCount; ++cityIndex)
                {
                    City c = new City(this, cityIndex, r, version);
                    cities.Add(c);
                    unitCollAreaGrid.add(c);
                }

                Debug.ReadCheck(r);

                if (version < 8)
                { //OLD
                    int factionCount = r.ReadInt32();
                    for (int factionIx = 0; factionIx < factionCount; ++factionIx)
                    {
                        FactionType factionType = (FactionType)r.ReadByte();
                        var faction = new Faction(this, factionType);
                        faction.readMapFile(r, version, this);
                    }
                    Debug.ReadCheck(r);
                }
                else
                {
                    int factionLength = r.ReadInt32();
                    factions = new SpottedArray<Faction>(factionLength);
                    init_FactionComponents(factionLength);

                    for (int i = 0; i < factionLength; ++i)
                    {
#if DEBUG
                        if (i == 100)
                        {
                            lib.DoNothing();
                        }
#endif
                        if (r.ReadBoolean())
                        {
                            FactionType factionType = (FactionType)r.ReadByte();
                            var faction = new Faction(this, factionType, i);
                            faction.readMapFile(r, version, this);
                        }
                        Debug.ReadCheck(r);
                    }
                }
            }
        }



        public static bool LoadingComplete { get { return DssRef.world != null; } }

        //public void Draw(int cameraIndex)
        //{
        //    //kan lägga in cam culling för dom olika områdena
        //    Map.MapDetailLayerManager drawView = Map.MapDetailLayerManager.CameraIndexToView[cameraIndex];
                
        //    if (drawView.current.DrawFullOverview)
        //    {
        //        factionsCounter.Reset();

        //        while(factionsCounter.Next())
        //        {
        //            factionsCounter.sel.Draw(cameraIndex, drawView);
        //        }
        //    }
           
        //}

        //public void loadModels()
        //{
        //    foreach (var m in cities)
        //    {
        //        m.enterGameState();
        //    }
        //}

        //public static BattleTerrain ToGeneralTerrain(TerrainType ground, bool shipBattle, bool city)
        //{
        //    if (city)
        //    {
        //        return BattleTerrain.City;
        //    }
        //    else if (shipBattle)
        //    {
        //        return BattleTerrain.Ship;
        //    }
        //    else
        //    {
        //        return BattleTerrain.Land;
        //    }
        //}

        //public AbsMapObject getUnit(System.IO.BinaryReader r)
        //{
        //    return getFaction(r).GetUnit(r);
        //}

        //public Faction getFaction(System.IO.BinaryReader r)
        //{
        //    byte factionId = r.ReadByte();
        //    return factions[factionId];
        //}


        //public SumTile4_4 tileFromSubTilePos(IntVector2 position)
        //{
        //    return tileGrid.Get(position.X / TileSubDivitions, position.Y / TileSubDivitions);
        //}  

        //public bool adjacentToLand(IntVector2 tile)
        //{
        //    SumTile4_4 t;
        //    //Check if it has a neighbor tile that is land
        //    foreach (IntVector2 dir in IntVector2.Dir8Array)
        //    {
        //        if (GetTileSafe(tile + dir, out t) && t.IsLand())
        //            return true;
        //    }
        //    return false;
        //}      

        public Faction ClosestFactionOverview(Vector3 position)
        {
            FindMinValuePointer<Faction> distances = new FindMinValuePointer<Faction>();

            var factionsCounter = factions.counter();
            while(factionsCounter.Next())
            {
                distances.Next((factionsCounter.sel.SelectionCenter - position).Length(), 
                    factionsCounter.sel);
            }
            return distances.minMember;
        }
        public const int HeadCityNeededFreeRadius = 14;
        public Rectangle2 CenterArea()
        {
            int radius = HeadCityNeededFreeRadius * (DssRef.difficulty.setting_QuickMatch_PlayerCount < 6? 4 : 5);
            radius = Bound.Max(radius, Size.Y / 2 - 5);
            Rectangle2 centerArea = Rectangle2.FromCenterTileAndRadius(Size / 2, radius);//new Rectangle2(IntVector2.Zero, world.Size);
            
            return centerArea;
        }

        //public Faction getPlayerAvailableFaction(bool firstPlayer, bool dropIn, List<Players.LocalPlayer> players)
        //{
        //    const int MultiPlayerDistance = GenerateMap.HeadCityNeededFreeRadius * 8;

        //    Rectangle2 centerArea = CenterArea();

        //    int loops = 0;
        //    while (true)
        //    {
        //        Faction result = factions.GetRandom(Ref.rnd);
                
        //        if (result.availableForPlayer && dropInCheck(result) && result.mainCity != null &&
        //            (centerArea.IntersectPoint(result.mainCity.tilePos) || loops >= 1000))
        //        {
        //            if (firstPlayer || loops >= 1000)
        //            {
        //                return result;
        //            }
        //            else if (!result.HasPlayerNeighbor() && 
        //                players[0].faction.mainCity.distanceTo(result.mainCity) <= MultiPlayerDistance)
        //            {
        //                return result;
        //            }
        //            ++loops;
        //        }

        //        if (++loops > 10000)
        //        {
        //            throw new EndlessLoopException("getPlayerAvailableFaction");
        //        }

        //        if (loops == 100 || loops == 200 || loops == 300 || loops == 400 || loops == 500)
        //        {
        //            centerArea.AddRadius(20);
        //        }
        //    }

        //    bool dropInCheck(Faction faction)
        //    {
        //        if (dropIn)
        //        {
        //            return faction.isAlive;
        //        }
        //        return true;
        //    }
        //}

        public Faction getPlayerAvailableFaction2(List<Players.LocalPlayer> players, bool firstPlayer, bool dropIn)
        {
            int MultiPlayerDistance = MathExt.MultiplyInt(HeadCityNeededFreeRadius, 1.75f * Ref.netsett.PlayerSpacing + 1.5f);

            List<Faction> sortedList = new List<Faction>();

            Rectangle2 centerArea = CenterArea();

            HashSet<PFaction> hadPlayerOwner = new HashSet<PFaction>();
            if (DssRef.state.PlayType() == GameState.PlayStateType.Play)
            {   
                foreach (var kv in DssRef.state.playstate().previousRemotePlayers)
                {
                    hadPlayerOwner.Add(kv.Value.pfaction);
                }
            }

            //Calculate scores
            foreach (var f in factions.Array)
            {
                if (f != null && f.player.IsBot() && f.cities.Count > 0)
                { 
                    sortedList.Add(f);

                    f.availableForPlayerScore = Ref.rnd.Int(100);

                    if (f.availableForPlayer)
                    {
                        f.availableForPlayerScore += 1000;
                    }

                    if (f.viewOnLargeMap)
                    {
                        f.availableForPlayerScore -= 1000;
                    }
                    else
                    {
                        var adj = f.adjacentFactions(true);
                        foreach (var nFaction in adj)
                        {
                            if (nFaction.viewOnLargeMap)
                            {
                                f.availableForPlayerScore -= 500;
                            }
                            if (nFaction.player.IsHumanPlayer())
                            {
                                f.availableForPlayerScore -= 200;
                            }
                        }
                    }

                    //if (f.cities.Count > 0)
                    //{
                    //    f.availableForPlayerScore += 1000;
                    //}

                    if (f.mainCity != null)
                    {
                        if (centerArea.IntersectPoint(f.mainCity.maptilePos))
                        {
                            f.availableForPlayerScore += 1000;
                        }
                        else
                        {
                            f.availableForPlayerScore += 500 - centerArea.LengthToClosestEdge(f.mainCity.maptilePos);
                        }

                        if (!firstPlayer)
                        {
                            float offsetToFirstPlayer = Math.Abs( players[0].pfaction.GetFaction().mainCity.distanceTo(f.mainCity) - MultiPlayerDistance);
                            f.availableForPlayerScore += 200 - Convert.ToInt32(offsetToFirstPlayer * 4);
                        }
                    }

                    if (!firstPlayer)
                    {
                        if (f.HasPlayerNeighbor() && Ref.netsett.PlayerSpacing > 0)
                        {
                            f.availableForPlayerScore -= 200;
                        }
                    }

                    if (dropIn)
                    {
                        int wars = f.CountWars(out int playerWars);
                        f.availableForPlayerScore -= wars * 2000 + playerWars * 8000;

                        if (hadPlayerOwner.Contains(f.pfaction))
                        {
                            f.availableForPlayerScore -= 500;
                        }
                    }
                }
            }

            sortedList.Sort((a, b) => a.availableForPlayerScore.CompareTo(b.availableForPlayerScore));

            return sortedList.LastOrDefault();
        }


        public City closestCity(IntVector2 pos, out float dist)
        {
            if (cities.Count == 0)
            {
                dist = float.MaxValue;
                return null;
            }

            FindMinValue closest = new FindMinValue(false);
            for (int i = 0; i < cities.Count; ++i)
            {
                var city = cities[i];
                if (city != null)
                {
                    closest.Next((pos.Vec - city.maptilePos.Vec).Length(), i);
                }
            }
            dist = closest.minValue;
            return cities[closest.minMemberIndex];
        }

        //public float SubTileHeight(Vector3 wp)
        //{
        //    return subTileGrid.Get(
        //        Convert.ToInt32(wp.X * TileSubDivitions + 3.5f), 
        //        Convert.ToInt32(wp.Z * TileSubDivitions + 3.5f)).groundY;                
        //}

        //public float SubTileHeight(Vector3 wp, out MapTile1_1 subTile)
        //{

        //    subTile = subTileGrid.Get(
        //        Convert.ToInt32(wp.X * TileSubDivitions + 3.5f),
        //        Convert.ToInt32(wp.Z * TileSubDivitions + 3.5f));
        //    return subTile.groundY;
        //}

        //public SumTile4_4 GetTile(Vector2 pos)
        //{
        //    return tileGrid.Get(WP.ToTilePos(pos));
        //}

        public SumTile4_4 GetTile(Vector3 pos)
        {
            return tileGrid.Get(WP.ToSumTilePos(pos));
        }



        public int TileSeed(int x, int y)
        {
            return metaData.worldId.seed + x * 11 + y * 13;
        }

        //public bool GetTileSafe(Vector3 pos, out Tile tile)
        //{
        //    IntVector2 tPos = WP.ToTilePos(pos);
        //    return GetTileSafe(tPos, out tile);
        //}

         
        //public float GetGroundHeight(Vector3 pos)
        //{
        //    const float InCenterRadius = 0.1f;

        //    IntVector2 gridPos = WP.ToTilePos(pos);

        //    SumTile4_4 center = tileGrid.Get(gridPos);
        //    float result = center.UnitGroundY();
            
        //    Vector2 centerDiff = new Vector2(pos.X - gridPos.X, pos.Z - gridPos.Y);

        //    int pointCount = 1;
        //    if (Math.Abs(centerDiff.X) > InCenterRadius)
        //    {
        //        IntVector2 nPos = gridPos;
        //        nPos.X += lib.ToLeftRight(centerDiff.X);

        //        SumTile4_4 nTile;
        //        if (GetTileSafe(nPos, out nTile))
        //        {
        //            result = lib.LargestValue(result, nTile.UnitGroundY());
        //            pointCount++;
        //        }
        //    }
        //    if (Math.Abs(centerDiff.Y) > InCenterRadius)
        //    {
        //        IntVector2 nPos = gridPos;
        //        nPos.Y += lib.ToLeftRight(centerDiff.Y);

        //        SumTile4_4 nTile;
        //        if (GetTileSafe(nPos, out nTile))
        //        {
        //            result = lib.LargestValue(result, nTile.UnitGroundY());
        //            pointCount++;
        //        }
        //    }

        //    if (pointCount == 3)
        //    {
        //        IntVector2 nPos = gridPos;
        //        nPos.X += lib.ToLeftRight(centerDiff.X);
        //        nPos.Y += lib.ToLeftRight(centerDiff.Y);
        //        SumTile4_4 nTile;
        //        if (GetTileSafe(nPos, out nTile))
        //        {
        //            result = lib.LargestValue(result, nTile.UnitGroundY());
        //            pointCount++;
        //        }
        //    }

        //    return Bound.Min(result, -0.1f);
        //}

        //public bool GetTileSafe(IntVector2 pos, out SumTile4_4 tile)
        //{
        //    if (tileBounds.IntersectTilePoint(pos))
        //    {
        //        tile = tileGrid.Get(pos);
        //        return true;
        //    }
        //    else
        //    {
        //        tile = new SumTile4_4();
        //        return false;
        //    }
        //}

        //public bool GetTileSafe_ref(IntVector2 pos, ref Tile tile)
        //{
        //    if (tileBounds.IntersectTilePoint(pos))
        //    {
        //        ref tile = ref tileGrid.array[pos.X, pos.Y];
        //        return true;
        //    }
        //    else
        //    {
        //        tile = new Tile();
        //        return false;
        //    }
        //}

        public IntVector2 GetFreeTile(IntVector2 center)
        {
            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = center + dir;
                MapTile1_1 t = subTileGrid.Get(pos);
                if (t.IsLand())
                {
                    return pos;
                }
            }
            Debug.LogError("GetFreeTile" + center.ToString());
            return center;
        }

        const float ExtraBoundRadius = 2f;
        public void WorldBound(ref float x, ref float z)
        {
            x = Bound.Set(x, ExtraBoundRadius, DssRef.world.unitSize.X - ExtraBoundRadius);
            z = Bound.Set(z, ExtraBoundRadius, DssRef.world.unitSize.Y - ExtraBoundRadius);
        }
        public static List<FactionType> NamedAiTypes()
        {
            return new List<FactionType>
            {
                FactionType.DarkLord,
                FactionType.DarkFollower,
                FactionType.Barbarians,
                FactionType.UnitedKingdom,
                FactionType.GreenWood,
                FactionType.EasternEmpire,
                FactionType.NordicRealm,
                FactionType.BearClaw,
                FactionType.NordicSpur,
                FactionType.IceRaven,
                FactionType.DragonSlayer,
                FactionType.SouthHara,
                FactionType.DyingMonger,
                FactionType.DyingHate,
                FactionType.DyingDestru,
            };
        }
        public static List<FactionType> AvailableGenericAiTypes()
        {
            var result = new List<FactionType>
            {
                FactionType.Starshield,//
                FactionType.Bluepeak,//
                FactionType.Hoft,//
                FactionType.RiverStallion,//
                FactionType.Sivo,//
                FactionType.AelthrenConclave,//
                FactionType.VrakasundEnclave,//
                FactionType.Tormürd,//
                FactionType.ElderysFyrd,//
                FactionType.Hólmgar,//
                FactionType.RûnothalOrder,//
            
                FactionType.GrimwardEotain,//
                FactionType.SkaeldraHaim,//
                FactionType.MordwynnCompact,//
                FactionType.AethmireSovren,//
                FactionType.ThurlanKin,//
                FactionType.ValestennOrder,//
                FactionType.Mournfold,//
                FactionType.OrentharTribes,//
                FactionType.SkarnVael,//
                FactionType.Glimmerfell,//

                FactionType.BleakwaterFold,//
                FactionType.Oathmaeren,//
                FactionType.Elderforge,//
                FactionType.MarhollowCartel,//
                FactionType.TharvaniDominion,//
                FactionType.KystraAscendancy,//
                FactionType.GildenmarkUnion,//
                FactionType.AurecanEmpire,//
                FactionType.BronzeReach,//
                FactionType.ElbrethGuild,//
                FactionType.ValosianSenate,//
                FactionType.IronmarchCompact,//
                FactionType.KaranthCollective,//
                FactionType.VerdicAlliance,//

                FactionType.OrokhCircles,//
                FactionType.TannagHorde,//
                FactionType.BraghkRaiders,//
                FactionType.ThurvanniStonekeepers,//
                FactionType.KolvrenHunters,//
                FactionType.JorathBloodbound,//
                FactionType.UlrethSkycallers,//
                FactionType.GharjaRavagers,//
                FactionType.RavkanShield,//
                FactionType.FenskaarTidewalkers,//

                FactionType.HroldaniStormguard,
                FactionType.SkirnirWolfkin,
                FactionType.ThalgarBearclaw,
                FactionType.VarnokRimeguard,
                FactionType.KorrakFirehand,
                FactionType.MoongladeGat,
                FactionType.DraskarSons,
                FactionType.YrdenFlamekeepers,
                FactionType.BrundirWarhorns,
                FactionType.OltunBonecarvers,

                FactionType.HaskariEmber,
                FactionType.ZalfrikThunderborn,
                FactionType.BjorunStonetender,
                FactionType.MyrdarrIcewalkers,
                FactionType.SkelvikSpear,
                FactionType.VaragThroatcallers,
                FactionType.Durakai,
                FactionType.FjornfellWarhowl,
                FactionType.AshgroveWard,
                FactionType.HragmarHorncarvers,

                FactionType.SylvaranGlade,
                FactionType.DrelmirePact,
                FactionType.KhazrunForgeclan,
                FactionType.VeylanHorselords,
                FactionType.ThalosCovenant,
                FactionType.NerathianTideguard,
                FactionType.SkaruunExiles,
                FactionType.DraktharDominion,
                FactionType.MalrekIronbound,

                FactionType.BranthollowBarony,   //
                FactionType.DunwadeHold,         //
                FactionType.CaerwynMarches,      //
                FactionType.StonevaleFreehold,   //
                FactionType.GlenmereLordship,    //
                FactionType.ArveldonPrincipality,//
                FactionType.WestmereReaches,     //
                FactionType.ThornwickWardens,    //
                FactionType.EvermereFief,        //
                FactionType.BryndralHollow,       //

                FactionType.Mendog,
                FactionType.Minde,
                FactionType.FloKingdom,
                FactionType.CarolusKeksenmark,
                FactionType.Etheleorthe,

                FactionType.DragonGem,
                FactionType.Hælfolc,
                FactionType.AerimAngren,

                FactionType.Ellium,
                FactionType.GrakPushdug,
                FactionType.Draugost,
            };


            return result;
        }

    }


}

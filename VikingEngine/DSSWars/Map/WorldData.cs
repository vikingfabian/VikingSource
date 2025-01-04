using System;
using System.Collections.Generic;

using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.GameObject;
using Microsoft.Xna.Framework;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Data;
using System.Xml.Linq;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.Network;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.SteamWrapping;
using System.Linq;

namespace VikingEngine.DSSWars
{   

    class WorldData
    {  
        public static WorldData LoadingWorld = null;

        //Måste vara delbart med UnitGridSquareWidth
        //Must be in sizes of 8
        public const int TinyMapWidth = 200;
        public const int TinyMapHeigth = 176;
        public const int SmallMapWidth = 320;
        public const int SmallMapHeigth = 232;
        public const int MediumMapWidth = 512;
        public const int MediumMapHeigth = 432;
        public const int LargeMapWidth = 720;
        public const int LargeMapHeigth = 624;
        public const int HugeMapWidth = 912;
        public const int HugeMapHeigth = 832;
        public const int EpicMapWidth = 1184;
        public const int EpicMapHeigth = 1024;
        public const double TileWidthInKm = 0.064;

        public const int TileSubDivitions = 8;
        public const int HalfTileSubDivitions = TileSubDivitions / 2;
        public const int TileSubDivitions_MaxIndex = TileSubDivitions-1;

        public static readonly Color WaterCol = new Color(14, 155, 246);
        public static readonly Color WaterCol2 = ColorExt.Multiply(WaterCol, 0.9f);
        public static readonly Color WaterDarkCol = new Color(0.043f, 0.486f,0.773f);
        public static readonly Color WaterDarkCol2 = ColorExt.Multiply(WaterDarkCol, 1.1f);

        public static readonly float TileHalfWidth = 0.5f;
        public static readonly float SubTileWidth = 1f / TileSubDivitions;
        public static readonly Vector2 SubTileWidthV2 = new Vector2(SubTileWidth);
        public static readonly float SubTileHalfWidth = SubTileWidth * 0.5f;

        public WorldMetaData metaData;

        public Rectangle2 tileBounds;
        public VectorRect unitBounds;
        public IntVector2 Size;
        public IntVector2 HalfSize;
        public Grid2D<Tile> tileGrid;
        public Grid2D<SubTile> subTileGrid;
       
        public UnitCollAreaGrid unitCollAreaGrid;

        public List<City> cities; 
        public SpottedArray<Faction> factions;
        //public SpottedArrayCounter<Faction> factionsCounter;

        public bool BordersUpdated = true;
        
       
        public PcgRandom rnd;
        
       
        public int areaTileCount;
        //public int evilFactionIndex=-1;
        public bool abortLoad = false;

        public List<FactionType> availableGenericAiTypes = new List<FactionType>();// AvailableGenericAiTypes();

        public WorldData()
        {
            factions = new SpottedArray<Faction>();
            //factionsCounter = new SpottedArrayCounter<Faction>(factions);
        }

        public WorldData(WorldMetaData metaData)//ushort seed, MapSize size)
            :this ()
        {
            this.metaData = metaData;
            LoadingWorld = this;
            //LoadStatus = 0;
            //mapSize = size;

            //size
            Size = SizeDimentions(metaData.mapSize);
            HalfSize = Size / 2;
            rnd = new PcgRandom(metaData.seed);

            refreshSize();
           
        }

        public static IntVector2 SizeDimentions(MapSize mapSize)
        {
            IntVector2 result;
            switch (mapSize)
            {
                case MapSize.Tiny:
                    result = new IntVector2(TinyMapWidth, TinyMapHeigth);
                    break;
                case MapSize.Small:
                    result = new IntVector2(SmallMapWidth, SmallMapHeigth);
                    break;
                case MapSize.Medium:
                    result = new IntVector2(MediumMapWidth, MediumMapHeigth);
                    break;
                case MapSize.Large:
                    result = new IntVector2(LargeMapWidth, LargeMapHeigth);
                    break;
                case MapSize.Huge:
                    result = new IntVector2(HugeMapWidth, HugeMapHeigth);
                    break;
                case MapSize.Epic:
                    result = new IntVector2(EpicMapWidth, EpicMapHeigth);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        public static string SizeString(MapSize mapSize)
        {
            string name = null;
            switch (mapSize)
            {
                case MapSize.Tiny: name = DssRef.lang.Lobby_MapSizeOptTiny; break;
                case MapSize.Small: name = DssRef.lang.Lobby_MapSizeOptSmall; break;
                case MapSize.Medium: name = DssRef.lang.Lobby_MapSizeOptMedium; break;
                case MapSize.Large: name = DssRef.lang.Lobby_MapSizeOptLarge; break;
                case MapSize.Huge: name = DssRef.lang.Lobby_MapSizeOptHuge; break;
                case MapSize.Epic: name = DssRef.lang.Lobby_MapSizeOptEpic; break;
            }

            var dim = SizeDimentions(mapSize);
            name += " " +
                string.Format(DssRef.lang.Lobby_MapSizeDesc,
                    Math.Round(dim.X * WorldData.TileWidthInKm),
                    Math.Round(dim.Y * WorldData.TileWidthInKm));

            return name;
        }

        void refreshSize()
        {
            areaTileCount = Size.X * Size.Y;
            tileBounds = new Rectangle2(IntVector2.Zero, Size - 1);
            unitBounds = new VectorRect(Vector2.Zero, Size.Vec);
            unitBounds.AddRadius(-1f);

            //create grid
            tileGrid = new Grid2D<Tile>(Size);

            unitCollAreaGrid = new UnitCollAreaGrid(Size);

            subTileGrid = new Grid2D<SubTile>(Size * TileSubDivitions);
        }

        //public Faction initEvilFactions()
        //{
        //    //evilFactionIndex = factions.Count;
        //    var faction = new Faction(this, FactionType.DarkLord);

        //    return faction;
        //}

        bool subTileHasRepeatValue(ref SubTile subtile)
        {
            return subtile.mainTerrain == TerrainMainType.DefaultSea;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            subTileGrid.LoopBegin();
            SubTile previuos = new SubTile();

            while (subTileGrid.LoopNext())
            {
                var subtile = subTileGrid.LoopValueGet();
                subtile.write(w, ref previuos);

                previuos = subtile;

                //if (subTileHasRepeatValue(ref subtile))
                //{
                //    //Find repeats
                //    int repeating = 0;

                //    while (true)
                //    {
                //        if (subTileGrid.LoopNext())
                //        {
                //            var nexttile = subTileGrid.LoopValueGet();
                //            if (subtile.EqualSaveData(ref nexttile))
                //            {
                //                ++repeating;
                //            }
                //            else
                //            {
                //                subTileGrid.LoopUndoToPrev();
                //                //end loop
                //                DataStreamLib.WriteGrowingBitShiftValue(w, repeating);
                //                break;
                //            }
                //        }
                //        else
                //        {
                //            //end loop, and final position on map
                //            DataStreamLib.WriteGrowingBitShiftValue(w, repeating);
                //            break;
                //        }
                //    }                    
                //}

            }

            Debug.WriteCheck(w);

            foreach (City city in cities)
            {
                city.writeGameState(w);
            }

            Debug.WriteCheck(w);

            foreach (var faction in factions.Array)
            {
                if (faction != null && faction.isAlive)
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

            Debug.WriteCheck(w);
            
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            //if (subversion >= 22  && subversion != SaveGamestate.MergeVersion)
            //{
            availableGenericAiTypes.Clear();
            subTileGrid.LoopBegin();
            SubTile previuos = new SubTile();


            while (subTileGrid.LoopNext())
            {
                SubTile subtile = subTileGrid.LoopValueGet();
                subtile.read(r, ref previuos, subversion);
                subTileGrid.LoopValueSet(subtile);

                previuos = subtile;
                //if (subTileHasRepeatValue(ref subtile))
                //{
                //    int repeating = DataStreamLib.ReadGrowingBitShiftValue(r);
                //    for (int i = 0; i < repeating; i++)
                //    {
                //        subTileGrid.LoopNext();
                //        SubTile nexttile = subTileGrid.LoopValueGet();
                //        nexttile.copySaveDataFrom(ref subtile);
                //        subTileGrid.LoopValueSet(nexttile);
                //    }
                //}   
            }
            //}
            //else
            //{
            //    subTileGrid.LoopBegin();
            //    while (subTileGrid.LoopNext())
            //    {
            //        SubTile st = subTileGrid.LoopValueGet();
            //        st.read(r, subversion);
            //        subTileGrid.LoopValueSet(st);
            //    }
            //}
            Debug.ReadCheck(r);

            foreach (City city in cities)
            {
                city.readGameState(r, subversion, pointers);
            }

            Debug.ReadCheck(r);

            for (int i = 0; i < factions.Array.Length; i++)
            {
                if (r.ReadBoolean())
                {
                    factions.Array[i].readGameState(r, subversion, pointers);
                    Debug.ReadCheck(r);
                }
                //else
                //{
                //    factions.Array[i] = null;
                //}
            }

            Debug.ReadCheck(r);

            
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
            refreshSize();

            int cityCount = r.ReadInt32();
            cities = new List<City>(cityCount);
            for (int cityIndex = 0; cityIndex < cityCount; ++cityIndex)
            {
                City c = new City(cityIndex);
                cities.Add(c);
            }

            int factionCount = r.ReadInt32();
            factions = new SpottedArray<Faction>(factionCount);
            for (int i = 0; i < factionCount; ++i)
            {
                factions.Add(new Faction(i));
            }
        }

        public void writeNet_Tile(System.IO.BinaryWriter w, IntVector2 tilePos)
        {
            var remotePlayerC = DssRef.state.remotePlayers.counter();

            tilePos.writeUshort(w);

            var area = new Rectangle2(tilePos, new IntVector2(RemotePlayer.OverviewSendChunkSize));
            area.SetTileBounds(DssRef.world.tileBounds);
            ForXYLoop loop = new ForXYLoop(area);

            Tile previous = new Tile();
            while (loop.Next())
            {
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                tile.writeMapFile(w, previous);

                previous = tile;

                remotePlayerC.Reset();
                while (remotePlayerC.Next())
                { 
                    var remoteTile = remotePlayerC.sel.remoteTileGrid.Get(loop.Position);
                    remoteTile.overview = true;
                    remotePlayerC.sel.remoteTileGrid.Set(loop.Position, remoteTile);
                }
            }            
        }

        public void readNet_Tile(System.IO.BinaryReader r)
        {
            IntVector2 tilePos = IntVector2.FromReadUshort(r);

            var area = new Rectangle2(tilePos, new IntVector2(RemotePlayer.OverviewSendChunkSize));
            area.SetTileBounds(DssRef.world.tileBounds);
            ForXYLoop loop = new ForXYLoop(area);
            Tile previous = new Tile();
            while (loop.Next())
            {
                var tile = DssRef.world.tileGrid.Get(loop.Position);
                tile.readMapFile(r, previous, int.MaxValue);
                DssRef.world.tileGrid.Set(loop.Position, tile); 

                previous = tile;
            }
        }

        public void writeNet_SubTile(System.IO.BinaryWriter w, IntVector2 tilePos)
        {
            tilePos.writeUshort(w);

            var area = new Rectangle2(WP.ToSubTilePos_TopLeft(tilePos), new IntVector2(WorldData.TileSubDivitions));
            ForXYLoop loop = new ForXYLoop(area);
            SubTile previous = new SubTile();

            while (loop.Next())
            {
                var tile = DssRef.world.subTileGrid.Get(loop.Position);
                tile.write(w, ref previous);

                previous = tile;
            }

            var remotePlayerC = DssRef.state.remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                var remoteTile = remotePlayerC.sel.remoteTileGrid.Get(tilePos);
                remoteTile.detail = true;
                remotePlayerC.sel.remoteTileGrid.Set(tilePos, remoteTile);
            }
        }

        public void readNet_SubTile(System.IO.BinaryReader r)
        {
            IntVector2 tilePos = IntVector2.FromReadUshort(r);

            var area = new Rectangle2(WP.ToSubTilePos_TopLeft(tilePos), new IntVector2(WorldData.TileSubDivitions));
            ForXYLoop loop = new ForXYLoop(area);
            SubTile previous = new SubTile();

            while (loop.Next())
            {
                var tile = DssRef.world.subTileGrid.Get(loop.Position);
                tile.read(r, ref previous, int.MaxValue);
                DssRef.world.subTileGrid.Set(loop.Position, tile);

                previous = tile;
            }
        }

        public void writeNet_Factions(System.IO.BinaryWriter w, HashSet<int> factions)
        {
            //int count = 

            //w.Write((byte)factions.Count);
            //foreach (int faction in factions) 
            //{
                int faction = factions.First();
                w.Write((ushort)faction);
                this.factions.Array[faction].writeNet(w);
                Debug.WriteCheck(w);
            //}

            SteamP2PManager.CrashOnTooLargePacket(w);

            var remotePlayerC = DssRef.state.remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                remotePlayerC.sel.factionsRecieved[faction] = true;
            }
        }

        public void readNet_Factions(System.IO.BinaryReader r)
        {
            //int factionCount = r.ReadByte();
            //for (int i = 0; i < factionCount; i++)
            //{ 
                int faction = r.ReadUInt16();
                this.factions.Array[faction].readNet(r);
                Debug.ReadCheck(r);
            //}
        }

        public void writeNet_Cities(System.IO.BinaryWriter w, HashSet<int> CitiesInView)
        {
            var remotePlayerC = DssRef.state.remotePlayers.counter();

            w.Write((byte)CitiesInView.Count);
            foreach (int city in CitiesInView)
            {
                w.Write((ushort)city);
                this.cities[city].writeNet(w);
                Debug.WriteCheck(w);

                //
                remotePlayerC.Reset();
                while (remotePlayerC.Next())
                {
                    remotePlayerC.sel.citiesRecieved[city] = true;
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
                this.cities[city].readNet(r);
                Debug.ReadCheck(r);
            }
        }

        public void writeMapFile(System.IO.BinaryWriter w)
        {
            //DebugWriteSize tilesSz = new DebugWriteSize();
            //DebugWriteSize citiesSz = new DebugWriteSize();
            //DebugWriteSize factionsSz = new DebugWriteSize();

            const int SaveMapVersion = 7;
            w.Write(SaveMapVersion);

            w.Write(metaData.seed);
            Size.write(w);

            if (abortLoad) return;
            
            //tilesSz.begin(w);
            ForXYLoop loop = new ForXYLoop(Size);
            Tile previous = new Tile();
            while (loop.Next())
            {
                var tile = tileGrid.Get(loop.Position);
                tile.writeMapFile(w, previous);

                previous = tile;
            }
            //tilesSz.end(w);

            if (abortLoad) return;

            Debug.WriteCheck(w);

            //citiesSz.begin(w);
            w.Write(cities.Count);
            foreach (var m in cities)
            {
                m.writeMapFile(w);
            }
            //citiesSz.end(w);

            if (abortLoad) return;

            Debug.WriteCheck(w);

            //factionsSz.begin(w);
            var factionsCount = factions.counter();
            w.Write(factions.Count);
            while (factionsCount.Next())
            {
                w.Write((byte)factionsCount.sel.factiontype);
                factionsCount.sel.writeMapFile(w);
            }
            //factionsSz.end(w);

            Debug.WriteCheck(w);

            //subtileSz.begin(w);
            //subTileGrid.LoopBegin();
            //while (subTileGrid.LoopNext())
            //{
            //    subTileGrid.LoopValueGet().write(w);
            //}
            //subtileSz.end(w);  
            
            lib.DoNothing();
        }

        public void readMapFile(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            metaData.seed = r.ReadUInt16();
            //rnd = new PcgRandom(metaData.seed);

            Size.read(r);
            refreshSize();
            ForXYLoop loop = new ForXYLoop(Size);
            Tile previous = new Tile();
            while (loop.Next())
            {
                var tile = new Tile(r, previous, version);
                tileGrid.Set(loop.Position, tile);
                previous = tile;
            }

            Debug.ReadCheck(r);

            int cityCount = r.ReadInt32();
            cities = new List<City>(cityCount);
            for (int cityIndex = 0; cityIndex < cityCount; ++cityIndex)
            {
                City c = new City(cityIndex, r, version);
                cities.Add(c);
                unitCollAreaGrid.add(c);
                
            }

            Debug.ReadCheck(r);

            int factionCount = r.ReadInt32();
            for (int factionIx = 0; factionIx < factionCount; ++factionIx)
            {
                FactionType factionType = (FactionType)r.ReadByte();
                var faction = new Faction(this, factionType);
                faction.readMapFile(r, version, this);
               
            }

            Debug.ReadCheck(r);

            //subTileGrid.LoopBegin();
            //while (subTileGrid.LoopNext())
            //{
            //    SubTile st = new SubTile();
            //    st.read(r, version);
            //    subTileGrid.LoopValueSet(st);
            //    //subTileGrid.LoopValueSet(new SubTile(r, version));
            //}

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

        public static BattleTerrain ToGeneralTerrain(TerrainType ground, bool shipBattle, bool city)
        {
            if (city)
            {
                return BattleTerrain.City;
            }
            else if (shipBattle)
            {
                return BattleTerrain.Ship;
            }
            else
            {
                return BattleTerrain.Land;
            }
        }

        public AbsMapObject getUnit(System.IO.BinaryReader r)
        {
            return getFaction(r).GetUnit(r);
        }

        public Faction getFaction(System.IO.BinaryReader r)
        {
            byte factionId = r.ReadByte();
            return factions[factionId];
        }


        public Tile tileFromSubTilePos(IntVector2 position)
        {
            return tileGrid.Get(position.X / TileSubDivitions, position.Y / TileSubDivitions);
        }  

        public bool adjacentToLand(IntVector2 tile)
        {
            Tile t;
            //Check if it has a neighbor tile that is land
            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                if (GetTileSafe(tile + dir, out t) && t.IsLand())
                    return true;
            }
            return false;
        }      

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

        

        public Faction getPlayerAvailableFaction(bool firstPlayer, List<Players.LocalPlayer> players)
        {
            const int MultiPlayerDistance = GenerateMap.HeadCityNeededFreeRadius * 8;

            Rectangle2 centerArea = new Rectangle2(IntVector2.Zero, Size);
            /// centerArea.
            centerArea.AddWidthRadius(-Size.X / 4);
            centerArea.AddHeightRadius(-Size.Y / 4);

            int loops = 0;
            while (true)
            {
                Faction result = factions.GetRandom(Ref.rnd);
                
                if (result.availableForPlayer &&
                    (centerArea.IntersectPoint(result.mainCity.tilePos) || loops >= 100))
                {
                    if (firstPlayer || loops >= 100)
                    {
                        return result;
                    }
                    else if (!result.HasPlayerNeighbor() && 
                        players[0].faction.mainCity.distanceTo(result.mainCity) <= MultiPlayerDistance)
                    {
                        return result;
                    }
                    ++loops;
                }

            }

            //return null;
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
                closest.Next((pos.Vec - cities[i].tilePos.Vec).Length(), i);
            }
            dist = closest.minValue;
            return cities[closest.minMemberIndex];
        }

        public float SubTileHeight(Vector3 wp)
        {
            return subTileGrid.array[
                Convert.ToInt32(wp.X * TileSubDivitions + 3.5f), 
                Convert.ToInt32(wp.Z * TileSubDivitions + 3.5f)].groundY;                
        }

        
        public Tile GetTile(Vector2 pos)
        {
            return tileGrid.Get(WP.ToTilePos(pos));
        }

        public Tile GetTile(Vector3 pos)
        {
            return tileGrid.Get(WP.ToTilePos(pos));
        }



        public int TileSeed(int x, int y)
        {
            return metaData.seed + x * 11 + y * 13;
        }

        //public bool GetTileSafe(Vector3 pos, out Tile tile)
        //{
        //    IntVector2 tPos = WP.ToTilePos(pos);
        //    return GetTileSafe(tPos, out tile);
        //}

         
        public float GetGroundHeight(Vector3 pos)
        {
            const float InCenterRadius = 0.1f;

            IntVector2 gridPos = WP.ToTilePos(pos);

            Tile center = tileGrid.Get(gridPos);
            float result = center.UnitGroundY();
            
            Vector2 centerDiff = new Vector2(pos.X - gridPos.X, pos.Z - gridPos.Y);

            int pointCount = 1;
            if (Math.Abs(centerDiff.X) > InCenterRadius)
            {
                IntVector2 nPos = gridPos;
                nPos.X += lib.ToLeftRight(centerDiff.X);

                Tile nTile;
                if (GetTileSafe(nPos, out nTile))
                {
                    result = lib.LargestValue(result, nTile.UnitGroundY());
                    pointCount++;
                }
            }
            if (Math.Abs(centerDiff.Y) > InCenterRadius)
            {
                IntVector2 nPos = gridPos;
                nPos.Y += lib.ToLeftRight(centerDiff.Y);

                Tile nTile;
                if (GetTileSafe(nPos, out nTile))
                {
                    result = lib.LargestValue(result, nTile.UnitGroundY());
                    pointCount++;
                }
            }

            if (pointCount == 3)
            {
                IntVector2 nPos = gridPos;
                nPos.X += lib.ToLeftRight(centerDiff.X);
                nPos.Y += lib.ToLeftRight(centerDiff.Y);
                Tile nTile;
                if (GetTileSafe(nPos, out nTile))
                {
                    result = lib.LargestValue(result, nTile.UnitGroundY());
                    pointCount++;
                }
            }

            return Bound.Min(result, -0.1f);
        }

        public bool GetTileSafe(IntVector2 pos, out Tile tile)
        {
            if (tileBounds.IntersectTilePoint(pos))
            {
                tile = tileGrid.array[pos.X, pos.Y];
                return true;
            }
            else
            {
                tile = new Tile();
                return false;
            }
        }

        public IntVector2 GetFreeTile(IntVector2 center)
        {
            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = center + dir;
                Tile t = tileGrid.Get(pos);
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
            x = Bound.Set(x, ExtraBoundRadius, DssRef.world.Size.X - ExtraBoundRadius);
            z = Bound.Set(z, ExtraBoundRadius, DssRef.world.Size.Y - ExtraBoundRadius);
        }
        public static List<FactionType> NamedAiTypes()
        {
            return new List<FactionType>
            {
                FactionType.DarkLord,
                FactionType.DarkFollower,
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
            return new List<FactionType>
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

            };
        }

    }


}

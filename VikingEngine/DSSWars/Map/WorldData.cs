using System;
using System.Collections.Generic;

using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.GameObject;
using Microsoft.Xna.Framework;
using VikingEngine.DataStream;

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

        public const int SubTileWidth = 8;

        public static readonly Color WaterCol = new Color(14, 155, 246);
        public static readonly Color WaterCol2 = ColorExt.Multiply(WaterCol, 0.9f);
        public static readonly Color WaterDarkCol = new Color(0.043f, 0.486f,0.773f);
        public static readonly Color WaterDarkCol2 = ColorExt.Multiply(WaterDarkCol, 1.1f);

        public static readonly Vector2 SubTileSz = new Vector2(1f / SubTileWidth);
        public Rectangle2 tileBounds;
        public VectorRect unitBounds;
        public IntVector2 Size;
        public Grid2D<Tile> tileGrid;
        public Grid2D<SubTile> subTileGrid;
       
        public UnitCollAreaGrid unitCollAreaGrid;

        public List<City> cities; 
        public SpottedArray<Faction> factions;
        public SpottedArrayCounter<Faction> factionsCounter;

        public bool BordersUpdated = true;
        
        public ushort seed;
        public PcgRandom rnd;
        public int saveIndex = -1;
        public MapSize mapSize;
        public int areaTileCount;
        //public int evilFactionIndex=-1;
        public bool abortLoad = false;

        public WorldData()
        {
            factions = new SpottedArray<Faction>();
            factionsCounter = new SpottedArrayCounter<Faction>(factions);
        }

        public WorldData(ushort seed, MapSize size)
            :this()
        {
            this.seed = seed;
            LoadingWorld = this;
            //LoadStatus = 0;
            mapSize = size;

            //size
            Size = SizeDimentions(mapSize);

            rnd = new PcgRandom(seed);

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

        void refreshSize()
        {
            areaTileCount = Size.X * Size.Y;
            tileBounds = new Rectangle2(IntVector2.Zero, Size - 1);
            unitBounds = new VectorRect(Vector2.Zero, Size.Vec);
            unitBounds.AddRadius(-1f);

            //create grid
            tileGrid = new Grid2D<Tile>(Size);

            unitCollAreaGrid = new UnitCollAreaGrid(Size);

            subTileGrid = new Grid2D<SubTile>(Size * SubTileWidth);
        }

        //public Faction initEvilFactions()
        //{
        //    //evilFactionIndex = factions.Count;
        //    var faction = new Faction(this, FactionType.DarkLord);
            
        //    return faction;
        //}

        public void write(System.IO.BinaryWriter w)
        {
            DebugWriteSize tilesSz = new DebugWriteSize();
            DebugWriteSize citiesSz = new DebugWriteSize();
            DebugWriteSize factionsSz = new DebugWriteSize();

            const int SaveMapVersion = 4;
            w.Write(SaveMapVersion);

            w.Write(seed);
            Size.write(w);

            if (abortLoad) return;
            
            tilesSz.begin(w);
            ForXYLoop loop = new ForXYLoop(Size);
            while (loop.Next())
            {
                var tile = tileGrid.Get(loop.Position);
                tile.write(w);
            }
            tilesSz.end(w);

            if (abortLoad) return;

            Debug.WriteCheck(w);

            citiesSz.begin(w);
            w.Write(cities.Count);
            foreach (var m in cities)
            {
                m.write(w);
            }
            citiesSz.end(w);

            if (abortLoad) return;

            Debug.WriteCheck(w);

            factionsSz.begin(w);
            var factionsCount = factionsCounter.Clone();
            w.Write(factions.Count);
            while (factionsCount.Next())
            {
                w.Write((byte)factionsCount.sel.factiontype);
                factionsCount.sel.write(w);
            }
            factionsSz.end(w);

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

        public void read(System.IO.BinaryReader r)
        {
           int version = r.ReadInt32();

            seed=r.ReadUInt16();
            Size.read(r);
            refreshSize();
            ForXYLoop loop = new ForXYLoop(Size);
            while (loop.Next())
            {
                tileGrid.Set(loop.Position, new Tile(r, version));
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
                faction.read(r, version, this);
               
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
            return tileGrid.Get(position.X / SubTileWidth, position.Y / SubTileWidth);
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
           
            factionsCounter.Reset();
            while(factionsCounter.Next())
            {
                distances.Next((factionsCounter.sel.SelectionCenter - position).Length(), 
                    factionsCounter.sel);
            }
            return distances.minMember;
        }

        

        public Faction getNextFreeFaction(bool firstPlayer)
        {
            int loops = 0;
            while (true)
            {
                //bool canTakeFaction = false;
                Faction result = factions.GetRandom(Ref.rnd);
                Rectangle2 centerArea = new Rectangle2(IntVector2.Zero, Size);
                /// centerArea.
                centerArea.AddWidthRadius(-Size.X / 4);
                centerArea.AddHeightRadius(-Size.Y / 4);
                if (result.Owner is Players.AiPlayer && result.cities.Count >= 2)
                {
                    bool pick = false;
                    if (firstPlayer)
                    {//Make sure the player ends up in the middle of the map
                        if (centerArea.IntersectPoint(result.mainCity.tilePos))
                        {
                            pick = true;
                        }
                    }
                    else
                    {//Set close to the other players, with an AI faction in between
                        if (result.HasPlayerNeighbor())
                        {
                            pick = true;
                        }
                    }

                    if (pick || loops >= 100)
                    {
                        if (result.availableForPlayer)
                        {
                            //allPlayers.Remove(result.Owner);
                            return result;
                        }
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
                Convert.ToInt32(wp.X * SubTileWidth + 3.5f), 
                Convert.ToInt32(wp.Z * SubTileWidth + 3.5f)].groundY;
                
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
            return seed + x * 11 + y * 13;
        }

        public bool GetTileSafe(Vector3 pos, out Tile tile)
        {
            IntVector2 tPos = WP.ToTilePos(pos);
            return GetTileSafe(tPos, out tile);
        }

         
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
                tile = null;
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

    }


}

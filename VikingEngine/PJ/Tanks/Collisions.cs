using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class Collisions
    {
        SafeCollectAsynchList<Tile> tiles;
        SafeCollectAsynchList<AbsGameObject> objects;
        SafeCollectAsynchList<Tank> tanks;

        public Collisions()
        {
            tiles = new SafeCollectAsynchList<Tile>(8);
            objects = new SafeCollectAsynchList<AbsGameObject>(4);
            tanks = new SafeCollectAsynchList<Tank>(2);
        }

        public void CollCheck(AbsGameObject obj, Physics.AbsBound2D bound, 
            bool bTiles, bool bObjects, bool bTanks)
        {
            Physics.Collision2D coll;

            if (bTiles)
            {
                tiles.checkForUpdatedList();

                for (int i = 0; i < tiles.list.Count; ++i)
                {
                    coll = bound.Intersect2(tiles.list[i].bound);
                    if (coll.IsCollision)
                    {
                        if (adjustTileColl(obj, coll, tiles.list[i]))
                        {
                            break;
                        }
                        //obj.onTileCollision(coll, tiles.list[i]);
                    }
                }
            }

            if (bObjects)
            {
                objects.checkForUpdatedList();

                for (int i = 0; i < objects.list.Count; ++i)
                {
                    coll = bound.Intersect2(objects.list[i].bound);
                    if (coll.IsCollision)
                    {
                        obj.onObjectCollision(coll, objects.list[i]);
                        break;
                    }
                }
            }

            if (bTanks)
            {
                tanks.checkForUpdatedList();

                for (int i = 0; i < tanks.list.Count; ++i)
                {
                    coll = bound.Intersect2(tanks.list[i].bound);
                    if (coll.IsCollision)
                    {
                        obj.onTankCollision(coll, tanks.list[i]);
                        break;
                    }
                }
            }
        }

        bool adjustTileColl(AbsGameObject obj, Physics.Collision2D coll, Tile tile)
        {
            if (coll.surfaceNormal.X > 0)
            {
                if (tile.neighborE)
                {
                    coll.surfaceNormal.X = 0;
                }
            }
            else if (coll.surfaceNormal.X < 0)
            {
                if (tile.neighborW)
                {
                    coll.surfaceNormal.X = 0;
                }
            }

            if (coll.surfaceNormal.Y > 0)
            {
                if (tile.neighborS)
                {
                    coll.surfaceNormal.Y = 0;
                }
            }
            else if (coll.surfaceNormal.Y < 0)
            {
                if (tile.neighborN)
                {
                    coll.surfaceNormal.Y = 0;
                }
            }

            if (VectorExt.HasValue(coll.surfaceNormal))
            {
                coll.surfaceNormal.Normalize();
                obj.onTileCollision(coll, tile);
                return true;
            }
            return false;
        }

        public void Collect_asynch(Physics.AbsBound2D bound)
        {
            {//TILES
                if (tiles.ReadyForAsynchProcessing())
                {
                    tiles.processList.Clear();

                    IntVector2 center = tankRef.level.wpToTilePos(bound.Center);

                    if (tankRef.level.grid.InBounds(center))
                    {
                        var area = Rectangle2.FromCenterTileAndRadius(center, 2);
                        area.SetBounds(tankRef.level.grid.Area);

                        ForXYLoop loop = new ForXYLoop(area);
                        while (loop.Next())
                        {
                            var t = tankRef.level.grid.Get(loop.Position);
                            if (t != null)
                            {
                                tiles.processList.Add(t);
                            }
                        }
                    }  
                    tiles.onAsynchProcessComplete();
                }
            }

            {//GAME OBJECTS
                if (objects.ReadyForAsynchProcessing())
                {
                    objects.processList.Clear();

                    var counter = tankRef.objects.active.counter();
                    while (counter.Next())
                    {
                        if (bound != counter.sel.bound &&
                            bound.AsynchCollect(counter.sel.bound))
                        {
                            objects.processList.Add(counter.sel);
                        }
                    }

                    objects.onAsynchProcessComplete();
                }
            }

            {//TANKS
                if (tanks.ReadyForAsynchProcessing())
                {
                    tanks.processList.Clear();

                    foreach (var g in tankRef.state.gamers)
                    {
                        if (bound != g.tank.bound &&
                            g.HasActiveVehicle() &&
                            bound.AsynchCollect(g.tank.bound))
                        {
                            tanks.processList.Add(g.tank);
                        }
                    }

                    tanks.onAsynchProcessComplete();
                }
            }
        }
    }
}

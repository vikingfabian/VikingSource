using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.PJ.SmashBirds
{
    class Collisions
    {
        SafeCollectAsynchList<Tile> mapBounds = new SafeCollectAsynchList<Tile>();

        public void terrainCollsionCheck(Gamer gamer, Physics.AbsBound2D bound)
        {
            Physics.Collision2D collision;

            mapBounds.checkForUpdatedList();
            foreach (var block in mapBounds.list)
            {

                collision = bound.Intersect2(block.bound);
                if (collision.IsCollision)
                {

                    if (block.bound.Type == Physics.Bound2DType.Rectangle)
                    {
                        //Forbid the collsion to push towards adjacent blocks
                        if (collision.direction.X != 0)
                        {                            
                            IntVector2 adj = block.position;
                            adj.X += lib.ToLeftRight(collision.direction.X);
                            Tile adjTile;
                            SmashRef.map.tileGrid.TryGet(adj, out adjTile);

                            if (adjTile != null)
                            {
                                collision.direction.X = 0;
                            }
                        }
                        if (collision.direction.Y != 0)
                        {
                            if (collision.direction.Y < 0) //roof
                            {
                                if (block.JumpThroughCollision)
                                    collision.direction.Y = 0;
                            }
                        }

                        if (collision.direction.X != 0 || collision.direction.Y != 0)
                        {
                            gamer.onTerrainCollision(block, collision);
                        }
                    }
                    else
                    {
                        gamer.onTerrainCollision(block, collision);
                    }
                }
            }
        }

        public bool terrainCollsionCheck(Physics.AbsBound2D bound, out Physics.Collision2D collision)
        {
            mapBounds.checkForUpdatedList();
            foreach (var block in mapBounds.list)
            {
                collision = bound.Intersect2(block.bound);
                if (collision.IsCollision)
                {
                    return true;
                }
            }

            collision = Physics.Collision2D.NoCollision;
            return false;
        }

        public void update_asynch(Vector2 center)
        {
            if (mapBounds.ReadyForAsynchProcessing())
            {
                const int SquareRadiusCheck = 2;

                mapBounds.processList.Clear();

                IntVector2 tilePos = SmashRef.map.wpToTilePos(center);

                ForXYLoop loop = new ForXYLoop(Rectangle2.FromCenterTileAndRadius(tilePos, SquareRadiusCheck));

                Tile tile;
                while (loop.Next())
                {
                    if (SmashRef.map.tileGrid.TryGet(loop.Position, out tile))
                    {
                        if (tile != null)
                        {
                            mapBounds.processList.Add(tile);
                        }
                    }
                }

                mapBounds.onAsynchProcessComplete();
            }
        }

        public void gamerToGamer(Gamer gamer1, Gamer gamer2)
        {
            if (gamer1.Alive && gamer2.Alive)
            {
                if (gamer1.bodyBound.Intersect(gamer2.bodyBound))
                {
                    DamageType damage1 = gamerCollisionType(gamer1, gamer2);
                    DamageType damage2 = gamerCollisionType(gamer2, gamer1);

                    if (damage1 == DamageType.PlayerAttack &&
                        damage2 == DamageType.PlayerAttack)
                    {
                        damage1 = DamageType.DoubleStunn;
                        damage2 = DamageType.DoubleStunn;
                    }

                    if (damage1 == DamageType.None &&
                        damage2 == DamageType.None)
                    {
                        LeftRight pushDir;
                        float xdiff = gamer1.Center.X - gamer2.Center.X;
                        if (xdiff == 0)
                        {
                            pushDir = LeftRight.Random();
                        }
                        else
                        {
                            pushDir = new LeftRight(xdiff);
                        }

                        gamer1.onPlayerBounce(pushDir);
                        pushDir.Invert();
                        gamer2.onPlayerBounce(pushDir);
                    }
                    else
                    {
                        removeDamageStunnConflict(ref damage1, ref damage2);
                        removeDamageStunnConflict(ref damage2, ref damage1);
                        
                        handleDamage(gamer1, gamer2, damage1);
                        handleDamage(gamer2, gamer1, damage2);
                    }
                }
            }

            //--
            void removeDamageStunnConflict(ref DamageType send, ref DamageType recieve)
            {
                if (recieve == DamageType.HeadStomp && send == DamageType.PlayerAttack)
                {
                    recieve = DamageType.None;
                }
            }
            
            DamageType gamerCollisionType(Gamer sender, Gamer reciever)
            {
                var damageBound = sender.damageBound();
                if (damageBound != null)
                {
                    if (damageBound.Intersect(reciever.bodyBound))
                    {
                        return DamageType.PlayerAttack;
                    }
                }

                if (sender.Center.Y < reciever.HeadStartY)
                {
                    return DamageType.HeadStomp;
                }

                return DamageType.None;
            }

            void handleDamage(Gamer sender, Gamer reciever, DamageType damage)
            {
                if (damage != DamageType.None)
                {
                    reciever.takeDamage(damage);
                    sender.onGiveDamage(damage);
                }
            }
        }

        
    }
}

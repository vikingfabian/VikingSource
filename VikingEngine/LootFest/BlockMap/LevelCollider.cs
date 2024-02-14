using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.BlockMap.Level;

namespace VikingEngine.LootFest.BlockMap
{
    abstract class AbsLevelCollider
    {
        public AbsLevel level;
        virtual public void updateCollisions() { }

        virtual public void SetLockedToArea()
        { }

        virtual public void setLevel(AbsLevel level)
        {
            this.level = level;
        }

        virtual public void refreshMovementFreedom()
        { }
        virtual public bool isBlocked(Map.WorldPosition wp) { return false; }
    }

    class LevelPointer : AbsLevelCollider
    {
        public LevelPointer(AbsLevel level)
        {
            this.level = level;
        }
        public LevelPointer(GO.AbsUpdateObj parent)
        {
            level = LfRef.levels2.GetLevelUnsafe(parent.WorldPos.ChunkGrindex);
        }
    }

    class LevelCollider : AbsLevelCollider
    {
        Vector3 prevPos;
        public GO.AbsUpdateObj parent;
        LevelCollisionType collType;
        byte lockedToArea;

        public LevelCollider(GO.AbsUpdateObj parent)
        {
            this.parent = parent;
            if (parent is VikingEngine.LootFest.GO.PlayerCharacter.AbsHero)
            {
                collType = LevelCollisionType.Hero;
            }
            else
            {
                collType = LevelCollisionType.LockedToAreaUntilHeroEnters;
            }
        }

        override public void refreshMovementFreedom()
        {
            //When a hero unlocks the area to monsters, they will get the same move areas as the hero
            if (collType == LevelCollisionType.LockedToAreaUntilHeroEnters)
            {
                if (level.unlockedAreas.Contains(lockedToArea))
                {
                    collType = LevelCollisionType.Hero;
                }
            }
        }

        override public void updateCollisions()
        {
            if (level == null)
            {
                setLevel( LfRef.levels2.GetLevelUnsafe(parent.WorldPos.ChunkGrindex));
                
            }
            else
            {
                //if (parent is VikingEngine.LootFest.GO.Characters.Boss.StatueBoss)
                //{
                //    lib.DoNothing();
                //}

                if (isBlocked(parent.WorldPos))// && prevPos != Vector3.Zero)
                {
                    parent.Position += collisionDir(parent.WorldPos);//= new Vector3(prevPos.X, parent.Position.Y, prevPos.Z);
                    parent.HandleObsticle(true, null);
                }
            }

            prevPos = parent.Position;
        }

        

        override public void setLevel(AbsLevel level)
        {
            this.level = level;

            if (collType != LevelCollisionType.Hero && level != null)
            {
                BlockMap.BlockMapSquare sq;
                if (level.getSquare(parent.WorldPos, out sq))
                {
                    lockedToArea = sq.lockId;
                    refreshMovementFreedom();
                }
            }
        }

        override public bool isBlocked(Map.WorldPosition wp)
        {
            if (level == null)
            {
                return false;
            }

            BlockMapSquare sq;
            if (level.getSquare(parent.WorldPos, out sq))
            {
                return isBlocked(sq);
            }
            else
            {
                level = null;
                return false;
            }
        }

        Vector3 collisionDir(Map.WorldPosition wp)
        {
            IntVector2 myXZ = wp.WorldXZ;
            IntVector2 squarePos = level.toSquarePos(myXZ);

            FindMinValue closestOpening = new FindMinValue();
            closestOpening.minMemberIndex = -1;

            for (int i = 0; i < IntVector2.Dir8Array.Length; ++i)//foreach (var dir in IntVector2.Dir8Array)
            {
                IntVector2 npos = squarePos + IntVector2.Dir8Array[i];
                if (!isBlocked(level.squares.Get(npos)))
                {
                    int l = (level.toCenterWorldXZ(npos) - myXZ).SideLength();
                    closestOpening.Next(l, i);
                }
            }

            if (closestOpening.minMemberIndex >= 0)
            {
                return VectorExt.V2toV3XZ(IntVector2.Dir8Array[closestOpening.minMemberIndex].Vec * 
                    Bound.Min((closestOpening.minValue - BlockMapLib.SquareHalfWidth) * 0.2f, 0.1f));
            }
            else
            {
                return Vector3.Zero;
            }
        }

        override public void SetLockedToArea()
        {
            collType = LevelCollisionType.LockedToArea;
        }

        bool isBlocked(BlockMapSquare sq)
        {
            if (sq.type == MapBlockType.Occupied || 
                sq.type == MapBlockType.Wall || 
                sq.type == MapBlockType.Water)
            {
                return true;
            }
            else if (collType == LevelCollisionType.LockedToArea)
            {
                return sq.lockId != lockedToArea;
            }
            else if (sq.lockId != AbsLevel.OpenAreaLockId)
            {
                return !level.unlockedAreas.Contains(sq.lockId);
            }
            return false;
        }

        enum LevelCollisionType
        {
            Hero,
            LockedToAreaUntilHeroEnters,
            LockedToArea,
        }
    }


}

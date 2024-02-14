using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest
{
    class SpawnPortalMonsterMoverArgument : AbsSpawnArgument
    {
        bool appliedMapY = false;
        Map.WorldPosition from, to;

        public SpawnPortalMonsterMoverArgument(Map.WorldPosition from, Map.WorldPosition to)
        {
            this.from = from;
            this.to = to;
        }

        override public void ApplyTo(GO.AbsUpdateObj obj)
        {
            if (!appliedMapY)
            {
                appliedMapY = true;
                //from.AddHeightMapHeight();
                //to.AddHeightMapHeight();
            }
            new SpawnPortalMonsterMover(obj, from, to);
        }
    }

    class SpawnPortalMonsterMover : AbsUpdateable
    {
        const float MoveLengthPerSec = 14;

        GO.AbsUpdateObj obj;
        Time moveTime;
        Vector3 pos;
        Vector3 move;

        public SpawnPortalMonsterMover(GO.AbsUpdateObj obj, Map.WorldPosition from, Map.WorldPosition to)
            :base(true)
        {
            this.obj = obj;
            pos = from.PositionV3;
            move = to.PositionV3 - pos;
            
            float l = move.Length();
            move = VectorExt.SafeNormalizeV3(move);
            moveTime.Seconds = (l / MoveLengthPerSec) * 1.2f;
        }

        public override void Time_Update(float time)
        {
            pos += MoveLengthPerSec * Ref.DeltaTimeSec * move;
            obj.Velocity.Value = move;
            obj.setImageDirFromSpeed();
            obj.Velocity.SetZero();

            obj.Position = pos;
            if (moveTime.CountDown())
            {
                
                DeleteMe();
            }
        }
    }
}

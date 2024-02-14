using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Effects
{
    class SleepingZZZ : AbsInGameUpdateable
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(
           // Color.DarkGray, new Vector3(0.2f, 0.3f, 0.05f));

        Time stateTime;

        IntervalF raiseTime = new IntervalF(2000, 2200);
        IntervalF hideTime = new IntervalF(200, 400);

        Graphics.AbsVoxelObj image;
        //GO.Characters.HumanoidEnemyBase parent;
        Vector3 sideDir;
        GO.AbsUpdateObj go;
        //Vector3 headOffset;

        public SleepingZZZ(GO.AbsUpdateObj go)//, Rotation1D dir, RotationQuarterion modelDir)
            :base(true)
        {
            this.go = go;
            //this.parent = parent;
            //this.headOffset = headOffset;
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.ZZZ, 0.1f, 0, false);

            //runDuringPause = false;
            resetPos();
        }

        public override void Time_Update(float time)
        {
            if (image.Visible)
            {
                image.position.Y += time * 0.0016f;
                //image.Scale1D += time * 0.00002f;
                image.Scale1D += time * 0.000025f;

                //Create a wave motion, side to side
                //image.Position += sideDir * time * 0.001f * (float)Math.Cos(Ref.TotalGameTimeSec * 0.5f);
                image.position += sideDir * time * 0.0014f * (float)Math.Cos(Ref.TotalTimeSec * 4f);
                //image.Position += sideDir * time * 0.001f * (float)Math.Cos(Ref.TotalTimeSec * 0.5f);

                if (stateTime.CountDown())
                {
                    image.Visible = false;
                    stateTime.MilliSeconds = hideTime.GetRandom();
                }
            }
            else
            {
                if (stateTime.CountDown())
                {
                    resetPos();
                }
            }
        }

        void resetPos()
        {
            image.position = go.expressionEffectPos();//headOffset + go.Position;

            image.Rotation = go.RotationQuarterion;

            Vector2 forward = go.Rotation.Direction(1f);
            sideDir.X = forward.Y;
            sideDir.Z = -forward.X;

            image.Scale1D = 0.1f;
            stateTime.MilliSeconds = raiseTime.GetRandom();
            image.Visible = true;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}

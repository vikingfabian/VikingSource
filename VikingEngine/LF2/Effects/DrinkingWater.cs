using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Effects
{
    /// <summary>
    /// Rotate a water symbol above his head
    /// </summary>
    class DrinkingWater : AbsUpdateable
    {
        
        Graphics.AbsVoxelObj model;
        GameObjects.AbsVoxelObj parent;
        Time timer;

        public DrinkingWater(GameObjects.AbsVoxelObj parent, float time)
            : base(true)
        {
            this.parent = parent;
            model = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.magicup_effect, Effects.HealUp.WaterDropTempImage, 0.7f, 0);
            timer = new Time(time);
        }

        void updatePosition()
        {
            Vector3 pos = parent.Position;
            pos.Y += 3.4f;

            model.position = pos;
            
        }

        public override void Time_Update(float time)
        {
            updatePosition();
            model.Rotation.RotateWorldX(0.004f * time);

            if (timer.CountDown())
            {
                DeleteMe();
            }
        }
        public override void DeleteMe()
        {
            model.DeleteMe();
            base.DeleteMe();
        }
    }
}

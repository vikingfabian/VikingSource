using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Effects
{
    class EnemyAttention : AbsUpdateable
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.WhiteSmoke, new Vector3(0.2f));
        Time viewTime;
        Graphics.AbsVoxelObj parentImage;
        Graphics.AbsVoxelObj image;
        Vector3 posDiff;
        float maxScale;

        public EnemyAttention(Time viewTime,
            Graphics.AbsVoxelObj parentImage,
            Vector3 posDiff, float maxScale)
            :base(true)
        {
            this.viewTime = viewTime;
            this.parentImage = parentImage;
            this.posDiff = posDiff;
            this.maxScale = maxScale;

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.enemyattention, TempImage, 0.01f, 0);
        }

        public override void Time_Update(float time)
        {
            if (image.scale.X < maxScale)//0.36f)
            {
                image.scale += 0.0015f * time * Vector3.One;
                posDiff.Y += 0.0015f * time;
            }
            image.position = parentImage.Rotation.TranslateAlongAxis(posDiff, parentImage.position);
            image.Rotation = parentImage.Rotation;

            if (viewTime.CountDown())
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            image.DeleteMe();
        }
    }
}

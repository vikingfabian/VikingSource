using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class EnemyAttention : AbsInGameUpdateable
    {
        ////static readonly Data.TempBlockReplacementSett PreAttackTempImage = new Data.TempBlockReplacementSett(Color.WhiteSmoke, new Vector3(0.2f));
        ////static readonly Data.TempBlockReplacementSett ExpressionTempImage = new Data.TempBlockReplacementSett(Color.Yellow, new Vector3(0.2f, 1f,0.2f));
        Time viewTime;
        Graphics.AbsVoxelObj parentImage;
        Graphics.AbsVoxelObj image;
        Vector3 posDiff;
        float maxScale;

        public EnemyAttention(Time viewTime,
            Graphics.AbsVoxelObj parentImage,
            Vector3 posDiff, float maxScale, EnemyAttentionType type)
            :base(true)
        {
            this.viewTime = viewTime;
            this.parentImage = parentImage;
            this.posDiff = posDiff;
            this.maxScale = maxScale;

            switch (type)
            {
                case EnemyAttentionType.Expression:
                    image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.enemy_expression, 0.01f, 0, false);
                    break;
                case EnemyAttentionType.PreAttack:
                    image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.enemyattention, 0.01f, 0, false);
                    break;

            }
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

    enum EnemyAttentionType
    {
        Expression,
        PreAttack,
        Cofused,
    }
}

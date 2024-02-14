using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Effects
{
    class HealUp : AbsUpdateable
    {
        public static readonly Data.TempBlockReplacementSett WaterDropTempImage = new Data.TempBlockReplacementSett(Color.CornflowerBlue, new Vector3(0.5f, 1f, 0.5f));
        public static readonly Data.TempBlockReplacementSett HeartTempImage = new Data.TempBlockReplacementSett(Color.Red, new Vector3(0.6f, 0.6f, 0.2f));

        float LifeTime = 2000;
        const float FadeTime = 500;
        const int NumIcons = 3;
        static readonly IntervalF ScaleRange = new IntervalF(0.4f, 0.6f);
        
        Graphics.AbsVoxelObj[] symbols;


        public HealUp(GameObjects.AbsVoxelObj obj, bool heal_notMagic, bool smallBoost)
            :base(true)
        {
            int num;// = smallBoost ? 1 : NumIcons;
            VoxelModelName model = heal_notMagic ? VoxelModelName.healup_effect : VoxelModelName.magicup_effect;
            Data.IReplacementImage replacementImg = heal_notMagic ? HeartTempImage : WaterDropTempImage;
            if (smallBoost)
            {
                num = 1;
            }
            else
            {   
                num = NumIcons;
            }

            symbols = new Graphics.VoxelModelInstance[num];
            Vector3 center =  obj.Position;
            center.Y += 4;

            
            
            for (int i = 0;  i < num; i++)
            {
                symbols[i] = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(model, replacementImg, ScaleRange.GetRandom(), 0); 
                symbols[i].position = lib.RandomV3(center, 1.2f);
                symbols[i].Rotation.RotateWorld(Vector3.UnitX * lib.RandomRotation()); 
            }
        }

        public override void Time_Update(float time)
        {
            LifeTime -= time;
            if (LifeTime <= 0)
            {
                for (int i = 0; i < symbols.Length; i++)
                {
                    symbols[i].DeleteMe();
                }
                this.DeleteMe();
            }

            const float MoveSpeed = 0.02f;
            const float MoveSpeedAdd = 0.005f;
            const float RotateSpeed = 0.12f;


            Vector3 rotate = Vector3.UnitX * RotateSpeed;
            for (int i = 0; i < symbols.Length; i++)
            {
                symbols[i].position.Y += MoveSpeed + MoveSpeedAdd * i;
                symbols[i].Rotation.RotateWorld(rotate);
            }
        }

    }
}

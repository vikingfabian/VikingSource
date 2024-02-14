using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Effects
{
    class BouncingBlock2Dummie : AbsUpdateable
    {

        protected Graphics.Mesh block;
        protected Vector3 speed;
        //protected const float Scale = 0.4f;
        protected Vector3 rotationSpeed;
        static readonly IntervalF rotationStartSpeed = new IntervalF(0.3f);
        protected static readonly IntervalF rotationBounceSpeed = new IntervalF(0.6f);

        public BouncingBlock2Dummie(Vector3 startPos, Data.MaterialType material, float scale)
            : this(startPos, material, scale, Rotation1D.Random)
        { }

        public BouncingBlock2Dummie(Vector3 startPos, Data.MaterialType material, float scale, Rotation1D dir)
            :base(false)
        {
            rotationSpeed = new Vector3(rotationStartSpeed.GetRandom(), rotationStartSpeed.GetRandom(), rotationStartSpeed.GetRandom());
            block = new Graphics.Mesh(LoadedMesh.cube_repeating, startPos,
                new Graphics.TextureEffect(TextureEffectType.LambertFixed, Data.MaterialBuilder.MaterialTile(material)),
                scale);
            block.Rotation = RotationQuarterion.Random;
            //Rotation1D dir = Rotation1D.Random();
            dir.Add(Ref.rnd.Plus_MinusF(0.4f));
            speed = Map.WorldPosition.V2toV3(dir.Direction(EffectLib.StandardStartSideSpeed.GetRandom()));
            speed.Y = EffectLib.StandardStartUpSpeed.GetRandom();

            AddToUpdateList(true);
        }
        public override void Time_Update(float time)
        {

            speed.Y += EffectLib.Gravity * time;
            block.Position += speed * time;
            block.Rotation.RotateAxis(rotationSpeed);
            if (block.Position.Y <= 1)
            {
                DeleteMe();
            }
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            block.DeleteMe();
        }
    }
    struct BouncingBlockColors
    {
        Data.MaterialType M1;
        Data.MaterialType M2;
        Data.MaterialType M3;

        public BouncingBlockColors(Data.MaterialType m1, Data.MaterialType m2, Data.MaterialType m3)
        {
            M1 = m1;
            M2 = m2;
            M3 = m3;
        }
        public Data.MaterialType GetRandom()
        {
            int rndPercent = Ref.rnd.Int(100);
            if (rndPercent < 50)
            {
                return M1;
            }
            else if (rndPercent < 80)
            {
                return M2;
            }
            else
            {
                return M3;
            }
            
        }
    }

}

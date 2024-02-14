using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Effects
{
    class ExplodingTerrainBlock: AbsUpdateable
    {

        protected Graphics.Mesh block;
        protected Vector3 speed;
        //protected const float Scale = 0.4f;
        protected Vector3 rotationSpeed;
        static readonly IntervalF rotationStartSpeed = new IntervalF(0.3f);
        protected static readonly IntervalF rotationBounceSpeed = new IntervalF(0.6f);


        public ExplodingTerrainBlock(Vector3 startPos, Data.MaterialType material)
            :base(false)
        {
            rotationSpeed = new Vector3(rotationStartSpeed.GetRandom(), rotationStartSpeed.GetRandom(), rotationStartSpeed.GetRandom());
            block = new Graphics.Mesh(LoadedMesh.cube_repeating, startPos,
                new Graphics.TextureEffect(TextureEffectType.LambertFixed, Data.MaterialBuilder.MaterialTile(material)),
                1);
            block.Rotation = RotationQuarterion.Random;
            //Rotation1D dir = Rotation1D.Random();
            //dir = Rotation1D
            speed = Map.WorldPosition.V2toV3(Rotation1D.Random.Direction(EffectLib.StandardStartSideSpeed.GetRandom()));
            speed.Y = EffectLib.StandardStartUpSpeed.GetRandom();

            AddToUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            const float Gravity = -0.00004f;
            speed.Y += Gravity * time;
            block.Position += speed * time;
            block.Rotation.RotateAxis(rotationSpeed);

            block.Transparentsy -= 0.0012f * time;
            block.Scale = Vector3.One * block.Transparentsy;

            if (block.Transparentsy <= 0)
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
}

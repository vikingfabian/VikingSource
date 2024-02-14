using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Effects
{
    class ScaleSpringY : VikingEngine.LootFest.GO.AbsChildObject
    {
        /* Properties */
        //public override UpdateType UpdateType { get { return VikingEngine.UpdateType.Full; } }

        /* Fields */
        float startScale;
        float time;
        float scaleAdd;
        float minScaleAdd;
        //float springConstant;
        AbsVoxelObj image;

        /* Constructors */
        public ScaleSpringY(AbsVoxelObj image)
        {
            startScale = image.scale.Y;
            time = MathHelper.Pi;
            restart();
            minScaleAdd = 0.01f * startScale;

            this.image = image;
           // this.springConstant = springConstant;
        }

        public void restart()
        {
            scaleAdd = 0.5f * startScale;
        }


        /* Methods */
        public override bool ChildObject_Update(GO.Characters.AbsCharacter parent)
        {
           
            if (scaleAdd < minScaleAdd)
            {
                image.scale.Y = startScale;
                return true;
            }
            else
            {
                time += Ref.DeltaTimeMs * 0.001f;

                image.scale.Y = scaleAdd * MathExt.Sinf(time * 17) + startScale;
                if (Ref.TimePassed16ms)
                {
                    scaleAdd *= 0.98f;
                }
                return false;
            }

        }

        public override void ChildObject_OnParentRemoval(GO.Characters.AbsCharacter parent)
        {
            //do nothing
        }
    }
}

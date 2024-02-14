using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba
{
    class MeleeEffect : Graphics.AbsUpdateableImage
    {
        Vector2 speed;
        Time lifeTime = new Time(200);

        public MeleeEffect(GO.AbsUnit from, GO.AbsUnit to)
            : base(SpriteName.cmdCCAttack, from.image.Position, new Vector2(MobaLib.UnitScale * 0.8f), ImageLayers.AbsoluteBottomLayer,
                 true, true, true)
        {
            LayerAbove(from.image);
            speed = VectorExt.SetLength((to.image.Position - from.image.Position), 5f * MobaLib.UnitScale);
            rotation = Rotation1D.FromDirection(speed).radians;
        }

        public override void Time_Update(float time_ms)
        {
            position += speed * Ref.DeltaTimeSec;
            if (lifeTime.CountDown())
            {
                DeleteMe();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba
{
    class Wall : Graphics.Image
    {
        Vector2 speed;
        Time maxMoveTime = new Time(2f, TimeUnit.Seconds);

        public Wall(GO.Hero hero)
            : base(SpriteName.WhiteArea, hero.image.Position,
                 new Vector2(2, 0.4f) * MobaLib.UnitScale, ImageLayers.AbsoluteBottomLayer, true, true)
        {
            hero.attackAnimation = maxMoveTime;
            Color = Color.Gray;
            LayerAbove(hero.image);

            speed = VectorExt.SetLength(hero.walkingTowardsGoal - position, MobaLib.UnitScale * 6f);
            rotation = Rotation1D.FromDirection(speed).radians;

        }

        public bool update(Input.IButtonMap button)
        {
            position += speed * Ref.DeltaTimeSec;

            if (maxMoveTime.CountDown() ||
                button.DownEvent)
            {
                new Timer.Terminator(2000, this);
                return true;
            }

            return false;
        }
    }
}

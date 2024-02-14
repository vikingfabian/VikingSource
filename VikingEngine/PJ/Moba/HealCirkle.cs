using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.PJ.Moba.GO;

namespace VikingEngine.PJ.Moba
{
    class HealCirkle : Graphics.AbsUpdateableImage
    {
        GO.Hero hero;
        Physics.CircleBound bound;
        List<AbsUnit> unitsInRange;

        public HealCirkle(GO.Hero hero)
            : base(SpriteName.ClickCirkleEffect, hero.image.Position, hero.image.Size * 0.1f, ImageLayers.AbsoluteBottomLayer,
                 true, true, true)
        {
            Color = Color.LightGreen;
            this.hero = hero;
            hero.attackAnimation.MilliSeconds = 500;
            unitsInRange = new List<AbsUnit>(hero.unitsInRange.Count);

            foreach (var m in hero.unitsInRange)
            {
                if (hero.isFriend(m))
                {
                    unitsInRange.Add(m);
                }
            }

            LayerBelow(hero.image);
            bound = new Physics.CircleBound(position, 0f);
        }

        public override void Time_Update(float time_ms)
        {
            Size1D += Ref.DeltaTimeSec * MobaLib.UnitScale * 15f;

            bound.radius = Size1D * 0.5f;
            for (int i = unitsInRange.Count - 1; i >= 0; --i)//each (var m in unitsInRange)
            {
                if (bound.Intersect2(unitsInRange[i].bound).IsCollision)
                {
                    unitsInRange[i].heal();
                    //unitsInRange[i].takeDamage(10, hero);
                    unitsInRange.RemoveAt(i);
                }
            }

            if (Size1D >= MobaLib.UnitScale * 5f)
            {
                DeleteMe();
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class StaminaUsageTooltip : ToggEngine.Display2D.AbsToolTip
    {
        Vector2 areaSz;

        public StaminaUsageTooltip(MapControls mapcontrols, Unit unit)
            : base(mapcontrols)
        {
            refresh(unit, 0);
            completeSetup(areaSz);
        }

        public void refresh(Unit unit, int useCount)
        {
            DeleteMe();

            Vector2 pos = Vector2.Zero;
            Vector2 sz = Engine.Screen.SmallIconSizeV2;

            Graphics.Image arrow = new Graphics.Image(SpriteName.cmdMoveLengthHudIcon,
                pos, sz, Layer);
            Add(arrow);
            pos.X += sz.X;

            float spacing = sz.X * 0.94f;

            for (int i = 0; i < unit.data.hero.stamina.maxValue; ++i)
            {
                SpriteName tn = i < unit.data.hero.stamina.Value ? SpriteName.cmdStamina : SpriteName.cmdStaminaGrayed;

                Graphics.Image icon = new Graphics.Image(tn, pos, sz, Layer);
                Add(icon);

                if (i >= (unit.data.hero.stamina.Value - useCount) && i < unit.data.hero.stamina.Value)
                {
                    Graphics.Image outline = new Graphics.Image(SpriteName.cmdStaminaStep, icon.Center, icon.Size * 1.3f, ImageLayers.AbsoluteBottomLayer, true);
                    outline.LayerBelow(icon);

                    Add(outline);
                }

                pos.X += spacing;
            }

            areaSz = new Vector2(pos.X, sz.Y);
            area = VectorRect.FromTwoPoints(Vector2.Zero, areaSz);

            createFrame(area);

            update();
        }
     }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.HeroStrategy;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    class BattleModifierLabel
    {
        Graphics.ImageGroup images;
        public RichBoxContent content = new RichBoxContent();

        public BattleModifierLabel()
        {
        }

        public void CreateLabel(ref Vector2 pos, float totalWidth, Graphics.ImageGroup images)
        {
            float edge = Engine.Screen.BorderWidth * 2;
            Vector2 contentPos = pos + new Vector2(edge, edge / 2);
            totalWidth -= edge * 2;

            RichBoxGroup richBox = new RichBoxGroup(contentPos, totalWidth, HudLib.AttackWheelLayer,
                HudLib.MouseTipRichBoxSett, content, true, true, false);
            images.Add(richBox);

            VectorRect area = richBox.area;
            area.AddRadius(edge);
            
            var border = HudLib.ThinBorder(area, HudLib.BackpackLayer + 2);
            images.Add(border);
            pos.Y = area.Bottom;
        }

        public void beginMod()
        {
            if (content.Count > 0)
            {
                content.Add(new RbSeperationLine());
            }
        }

        public void modSource(AbsProperty source)
        {
            if (source.Icon != SpriteName.MissingImage)
            {
                content.Add(new RbImage(source.Icon));
            }
            content.Add(new RbText(source.Name));
            sourceArrow();
        }

        public void modSource(AbsHeroStrategy source)
        {   
            content.Add(new RbImage(source.cardSprite));
            sourceArrow();
        }

        void sourceArrow()
        {
            content.Add(new RbImage(SpriteName.cmdConvertArrow, 0.6f));
        }

        public void attackModifier(int diceModifier)
        {
            content.Add(new RbText(TextLib.ValuePlusMinus(diceModifier)));
            content.Add(new RbImage(SpriteName.cmdDiceAttack));
        }

    }

}

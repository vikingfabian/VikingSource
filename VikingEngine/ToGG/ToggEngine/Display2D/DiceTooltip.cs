using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class DiceTooltip : Graphics.ImageGroup
    {
        public DiceTooltip(VectorRect diceArea, BattleDice dice)
            :base()
        {
            Vector2 arrowSz;
            float arrowOffset;
            HudLib.TooltipBorderArrowSize(out arrowSz, out arrowOffset);

            float width = Engine.Screen.IconSize * 7f;

            Vector2 startPos = diceArea.CenterBottom;
            startPos.Y += Engine.Screen.IconSize * 0.5f + arrowOffset;
            startPos.X -= width * 0.67f;

            Vector2 contentPos = startPos + new Vector2(HudLib.TooltipBorderEdgeSize);
            float contentW = width - HudLib.TooltipBorderEdgeSize * 2f;
            Graphics.TextG title = new Graphics.TextG(LoadedFont.Bold, VectorExt.AddX(contentPos, contentW * 0.5f),
                Engine.Screen.TextTitleScale, Graphics.Align.CenterWidth, BattleDice.DiceTypeName(dice.type) + " dice",
                HudLib.TitleTextBronze, HudLib.TooltipLayer);
            Add(title);

            contentPos.Y = title.MeasureBottomPos();

            float descW = width - Engine.Screen.SmallIconSize * 2f;

            foreach (var m in dice.sides)
            {
                Graphics.Image icon = new Graphics.Image(BattleDice.ResultIcon(m.result),
                    contentPos, Engine.Screen.SmallIconSizeV2, HudLib.TooltipLayer);

                float chance = m.result == BattleDiceResult.Empty ? dice.noneChance() : m.chance;

                Graphics.TextBoxSimple desc = new Graphics.TextBoxSimple(LoadedFont.Regular, 
                    VectorExt.AddX(icon.RightCenter, Engine.Screen.BorderWidth),
                    Engine.Screen.TextBreadScale, Graphics.Align.CenterHeight,
                    BattleDice.ResultDesc(m.result) + " (" + TextLib.PercentText(chance) + ")", Color.White, 
                    HudLib.TooltipLayer, descW);

                Add(icon); Add(desc);
                contentPos.Y += Math.Max(Engine.Screen.SmallIconSize, desc.MeasureText().Y) + Engine.Screen.BorderWidth * 0.5f;
            }

            Vector2 end = new Vector2(startPos.X + width, contentPos.Y + HudLib.TooltipBorderEdgeSize);

            VectorRect tiparea = VectorRect.FromTwoPoints(startPos, end);
            var bg = HudLib.TooltipBorder(tiparea, HudLib.TooltipBgLayer);
            var arrow = HudLib.TooltipBorderArrow(tiparea, diceArea.Center, Dir4.N, HudLib.TooltipLayer);

            images.AddRange(bg.images);
            Add(arrow);
        }
    }
}

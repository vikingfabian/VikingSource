using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.BattleEngine
{
    interface IBattleSetup
    { 
        bool BattleAttackIsMelee { get; }
        int BattleAttackBaseStrength { get; }
        string BattleAttackType { get; }
    }

    class AttackRollDiceDisplay
    {
        public Graphics.ImageGroup2D images;
        public AttackRollDiceGroup dice;
        public VectorRect bgArea;

        public DiceResultLabel resultLabel1 = null, resultLabel2 = null;

        public AttackRollDiceDisplay(bool attacker, AbsBattleSetup attacks, List<AbsUnit> units,
            ref Vector2 pos, BattleDice[] diceData, int wheelsWidthCount, float hudWidth)
        {
            images = new Graphics.ImageGroup2D();
            Vector2 start = pos;
            float titleH = Engine.Screen.IconSize;
            float titleTextH = titleH * 0.8f;

            VectorRect titleArea = new VectorRect(pos.X, pos.Y, hudWidth, titleH);
            titleArea.AddRadius(-HudLib.ThickBorderEdgeSize);
            Vector2 titlePos = titleArea.Position;

            foreach (var u in units)
            {
                Graphics.ImageAdvanced unitPortrait = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                    titlePos, new Vector2(titleH), HudLib.AttackWheelLayer, false);
                u.Data.modelSettings.IconSource(unitPortrait, true);
                titlePos.X += unitPortrait.Width;
                images.Add(unitPortrait);

                Graphics.Image portraitBg = new Graphics.Image(SpriteName.WhiteArea, unitPortrait.Position, unitPortrait.Size, ImageLayers.AbsoluteBottomLayer);
                portraitBg.Color = Color.Black;
                portraitBg.Opacity = 0.4f;
                portraitBg.LayerBelow(unitPortrait);
                images.Add(portraitBg);
            }           

            if (attacker)
            {
                //Graphics.Image attackTypeIcon = new Graphics.Image(attacks.targets.IsMelee ? SpriteName.cmdStatsMelee : SpriteName.cmdStatsRanged,
                //   titlePos + new Vector2(-unitPortrait.Width * 0.2f, unitPortrait.Height * 0.3f), unitPortrait.Size * 0.7f, HudLib.AttackWheelLayer);
                
                //Graphics.Image attackStrengthBg = new Graphics.Image(SpriteName.WhiteCirkle, attackTypeIcon.Area.PercentToPosition(new Vector2(0.8f, 0f)),
                //    attackTypeIcon.Size * 0.9f, ImageLayers.AbsoluteBottomLayer);
                //attackStrengthBg.LayerBelow(attackTypeIcon);
                //attackStrengthBg.Color = Color.Black;

                //Graphics.TextG attackStrengthText = new Graphics.TextG(LoadedFont.Regular,
                //    attackStrengthBg.Center, Vector2.One, Graphics.Align.CenterAll, attacks.AttackerSetup.baseAttackStrength.ToString(),
                //    Color.White, HudLib.AttackWheelLayer);
                //attackStrengthText.LayerAbove(attackStrengthBg);

                //attackStrengthText.SetHeight(attackStrengthBg.Height * 0.9f);

                //images.Add(attackTypeIcon); images.Add(attackStrengthBg); images.Add(attackStrengthText);
                //titlePos.X = attackStrengthBg.Right;
            }

            titlePos.X += Engine.Screen.BorderWidth * 2f;
            Graphics.TextG titleText = new Graphics.TextG(LoadedFont.Bold, titlePos, Engine.Screen.TextIconFitSize, Graphics.Align.Zero,
                attacker ? attacks.targets.AttackType.ToString() : "Defence",
                HudLib.TitleTextBronze, HudLib.AttackWheelLayer);

            images.Add(titleText);

            pos.Y += titleH + Engine.Screen.BorderWidth * 2f;

            dice = new AttackRollDiceGroup(this, ref pos, diceData, wheelsWidthCount);

            Vector2 end = pos;
            end.X += hudWidth;

            bgArea = VectorRect.FromTwoPoints(start, end);

            var bg = HudLib.ThickBorder(bgArea, HudLib.AttackWheelLayer + 1);
            images.images.AddRange(bg.images);
        }

        public void DeleteMe()
        {
            images.DeleteAll();
            dice.DeleteMe();

            if (resultLabel1 != null)
            {
                resultLabel1.DeleteMe();
            }

            if (resultLabel2 != null)
            {
                resultLabel2.DeleteMe();
            }
        }
    }
}

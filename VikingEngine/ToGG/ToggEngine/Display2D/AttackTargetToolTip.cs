using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Commander.Battle;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    /*
     * Visa counter attack
     * Visa block count
     * Visa ignore retreat
     * 
     * Visa supporters positioner
     */

    class AttackTargetToolTip : AbsToolTip
    {
        List<Graphics.Mesh> supportGui;

        public AttackTargetToolTip(AbsUnit attacker, AbsUnit target, AttackType attackType, 
            MapControls mapControls, CommandType strategy)//Players.LocalPlayer player)
            : base(mapControls)
        {
            var bgSz = createBar(Vector2.Zero, attacker, target, attackType, strategy);
            if (Commander.BattleLib.DefenderCanCounter(false, attackType, target))
            {
                createBar(VectorExt.V2FromY(bgSz.Y + Engine.Screen.IconSize * 0.2f), target, attacker, 
                    AttackType.CounterAttack, CommandType.NUM_NONE);
            }
            completeSetup(bgSz);
        }

        Vector2 createBar(Vector2 startPos, AbsUnit attacker, AbsUnit target, AttackType attackType, CommandType command)
        {
            BattleSetup attacks = new BattleSetup(
                new List<AbsUnit> { attacker }, new AttackTarget( target, attackType), command);

            if (attackType.IsNot(AttackUnderType.CounterAttack))// .underType != Battle.AttackType.CounterAtta)
            {
                supportGui = new List<Graphics.Mesh>();
                foreach (var m in attacks.supportingUnits.units)
                {
                    AbsAttackAnimation.SupportGui(m.unit, m.support, supportGui);
                }
            }

            //bool closeCombat = attackType != Battle.AttackType.Ranged;

            float edge = Engine.Screen.IconSize * 0.1f;
            float spacing = Engine.Screen.IconSize * 0.3f;
            Vector2 iconSz = Engine.Screen.SmallIconSizeV2;
            
            
            if (attackType.IsNot(AttackUnderType.CounterAttack))// != Battle.AttackType.CounterAttack)
            {
                iconSz *= 1.3f;
            }

            VectorRect area = new VectorRect(startPos, new Vector2(iconSz.Y));
            //Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea,
            //   startPos,
            //   new Vector2(iconSz.Y + edge * 2f), Layer);
            //if (attackType.IsNot(Battle.AttackUnderType.CounterAttack))//attackType != Battle.AttackType.CounterAttack)
            //{
            //    bg.Color = Color.Black;
            //}
            //else
            //{
            //    bg.Color = new Color(68, 0, 0);
            //}
            //bg.Opacity = 0.7f;

            Vector2 attIconPos = startPos + new Vector2(edge);
            if (attackType.Is(AttackUnderType.CounterAttack))// == Battle.AttackType.CounterAttack)
            {
                VectorRect unitIconArea = new VectorRect(attIconPos, iconSz);
                attIconPos.X = unitIconArea.Right;

                unitIconArea.AddPercentRadius(0.3f);
                Graphics.Image unitIcon = new Graphics.Image(attacker.Data.modelSettings.image,
                    unitIconArea.Position, unitIconArea.Size, Layer);
                
                Add(unitIcon);

                Graphics.Image counterIcon = new Graphics.Image(SpriteName.MenuIconResume,
                    attIconPos, iconSz, Layer);
                //counterIcon.LayerAbove(bg);
                Add(counterIcon);

                attIconPos.X = counterIcon.Right;
            } 

            Graphics.Image attIcon = new Graphics.Image(
                attackType.IsMelee ? SpriteName.cmdUnitMeleeGui : SpriteName.cmdUnitRangedGui,
                 attIconPos,
                 iconSz, ImageLayers.Lay2);
            Graphics.TextG attCount = new Graphics.TextG(LoadedFont.Regular, attIcon.RightCenter,
                new Vector2(Engine.Screen.TextSize * 0.8f), Graphics.Align.CenterHeight,
                attacks.attackerSetup.baseAttackStrength.ToString() + ":", Color.White, ImageLayers.Lay1);
            attCount.SetHeight(iconSz.Y * 1f);

            Graphics.Image hitSymb = new Graphics.Image(SpriteName.cmdHitFlatSymbol,
                new Vector2(attCount.MeasureRightPos() + spacing, attIcon.Ypos),
                iconSz, ImageLayers.Lay1);
            Graphics.TextG hitChanceTxt = new Graphics.TextG(LoadedFont.Regular, hitSymb.RightCenter, attCount.Size,
                Graphics.Align.CenterHeight, TextLib.PercentText(attacks.hitChance), Color.White, ImageLayers.Lay1);
            hitChanceTxt.SetHeight(hitSymb.Height * 0.84f);

            Graphics.Image retreatSymb = new Graphics.Image(SpriteName.cmdRetreatFlatSymbol,
              new Vector2(hitChanceTxt.MeasureRightPos() + spacing, hitSymb.Ypos),
                iconSz, ImageLayers.Lay1);
            Graphics.TextG retreatChanceTxt = new Graphics.TextG(LoadedFont.Regular, retreatSymb.RightCenter, hitChanceTxt.Size,
                Graphics.Align.CenterHeight, TextLib.PercentText(attacks.retreatChance), Color.White, ImageLayers.Lay1);

            Add(attIcon);
            Add(attCount);
            Add(hitSymb);
            Add(hitChanceTxt);
            Add(retreatSymb);
            Add(retreatChanceTxt);
            //Add(bg);

            float mostRightPos;

            if (attacks.supportingUnits.total > 0)
            {
                Graphics.Image supportSymb = new Graphics.Image(SpriteName.cmdSupporterIcon1,
                    new Vector2(retreatChanceTxt.MeasureRightPos() + spacing, retreatSymb.Ypos),
                    hitSymb.Size, ImageLayers.Lay1);
                Graphics.TextG supportersTxt = new Graphics.TextG(LoadedFont.Regular, supportSymb.RightCenter, hitChanceTxt.Size,
                    Graphics.Align.CenterHeight, attacks.supportingUnits.total.ToString(), Color.White, ImageLayers.Lay1);

                Add(supportSymb, supportersTxt);

                mostRightPos = supportersTxt.MeasureRightPos();
            }
            else
            {
                mostRightPos = retreatChanceTxt.MeasureRightPos();
            }

            if ((attacks.hitBlocks + attacks.retreatIgnores) > 0)
            {
                mostRightPos += spacing;
                Graphics.Image arrow = new Graphics.Image(SpriteName.LfNpcSpeechArrow,
                    new Vector2(mostRightPos, area.Center.Y), iconSz * 0.4f, Layer, false);
                //arrow.LayerAbove(bg);
                arrow.origo = VectorExt.V2HalfY;
                Add(arrow);

                mostRightPos = arrow.Right;

                for (int i = 0; i < attacks.hitBlocks; ++i)
                {
                    Graphics.Image blockIcon = new Graphics.Image(SpriteName.cmdArmorResult, new Vector2(mostRightPos, attIcon.Ypos),
                        iconSz, Layer);
                    //blockIcon.LayerAbove(bg);
                    Add(blockIcon);

                    mostRightPos = blockIcon.Right;
                }

                for (int i = 0; i < attacks.retreatIgnores; ++i)
                {
                    Graphics.Image retreatIgnoreIcon = new Graphics.Image(SpriteName.cmdRetreatFlatSymbol, new Vector2(mostRightPos, attIcon.Ypos),
                        iconSz, Layer);
                    //retreatIgnoreIcon.LayerAbove(bg);
                    Add(retreatIgnoreIcon);

                    VectorRect iconArea = retreatIgnoreIcon.Area;
                    iconArea.AddRadius(-0.1f * iconArea.Width);
                    Graphics.Image cross = new Graphics.Image(SpriteName.LfCheckNo,
                        iconArea.Position, iconArea.Size, ImageLayers.AbsoluteTopLayer);
                    cross.LayerAbove(retreatIgnoreIcon);
                    cross.Color = Color.Red;
                    Add(cross);

                    mostRightPos = retreatIgnoreIcon.Right;
                }
            }

            area.Width = mostRightPos - area.X;
            //bg.Width = mostRightPos - bg.Xpos + edge;
            createFrame(area);

            return area.Size;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            foreach (var m in supportGui)
            {
                m.DeleteMe();
            }
        }
    }
}

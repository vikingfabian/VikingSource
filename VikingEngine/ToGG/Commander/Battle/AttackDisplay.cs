using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.Battle
{
    
    class AttackDisplay
    {
        public AttackRoll2 attackRoll;
        CatapultAttackAnimation catapultAttackAnimation = null;
        Display3D.CatapultTargetGUI catapultTargetGUI = null;
        Players.AbsLocalPlayer player;
        Graphics.ImageGroup images = new Graphics.ImageGroup();
        Time finalViewTime;
        int state_1Attack_2end = 0;

        public AttackDisplay(Players.AbsLocalPlayer player, AbsUnit movingUnit) //Move backstabs
        {
            var stabbers = movingUnit.movelines.getBackstabbers();
            List<AbsUnit> stabberunits = new List<AbsUnit>();
            foreach (var m in stabbers)
            {
                stabberunits.AddRange(m.backstabberUnits);
            }

            setup(player, stabberunits, 
                new AttackTarget(movingUnit, new AttackType(AttackMainType.Melee, AttackUnderType.BackStab)), 
                CommandCard.CommandType.NUM_NONE);
        }

        public AttackDisplay(Players.AbsLocalPlayer player, 
            AbsUnit attacker, AttackTarget target, CommandCard.CommandType SelectedCommand)
        {
            setup(player, new List<AbsUnit> { attacker }, target, SelectedCommand);
        }

        void setup(Players.AbsLocalPlayer player,
            List<AbsUnit> attacker, AttackTarget target, CommandCard.CommandType SelectedCommand)
        {
            this.player = player;
            //player.mapControls?.removeToolTip();

            if (attacker[0].CatapultAttackType)
            {
                catapultAttackAnimation = new CatapultAttackAnimation(player,
                    attacker[0], target.position, catapultTargetGUI);
            }
            else
            {
                int wheelsWidthCount;
                float width;
                AbsAttackAnimation.AttackWheelsSize(out wheelsWidthCount, out width);
                float topBottomEdge = Engine.Screen.SafeArea.Height * 0.1f;

                var leftArea = new VectorRect(
                   Engine.Screen.Width * 0.5f - width, Engine.Screen.SafeArea.Y + topBottomEdge,
                  width, Engine.Screen.SafeArea.Height - topBottomEdge * 2f);

                var rightArea = leftArea;
                rightArea.SetRight(Engine.Screen.SafeArea.Right, false);

                VectorRect area = Input.Mouse.Position.X > Engine.Screen.Width * 0.6f ? leftArea : rightArea;


                attackRoll = new AttackRoll2(attacker, target.unit, target.attackType,
                    SelectedCommand, area);


                var diceLabelAr = diceLabel(attackRoll.attackDice.bgArea);
                battleModifierLabel(diceLabelAr);

                finalViewTime = new Time(toggRef.gamestate.attackRollSpeed.cmdFinalViewTime());
            }
        }


        VectorRect diceLabel(VectorRect prevArea)
        {
            prevArea.nextAreaY(1, Engine.Screen.BorderWidth);

            HUD.RichBox.RichBoxContent diceContent = new HUD.RichBox.RichBoxContent();
            diceContent.h2(attackRoll.setup.dice.name + " dice");
            foreach (var m in attackRoll.setup.dice.sides)
            {
                float chance = m.result == BattleDiceResult.Empty ? attackRoll.setup.dice.noneChance() : m.chance;
                diceContent.add(BattleDice.ResultIcon(m.result), BattleDice.ResultDesc(m.result) + " (" + TextLib.PercentText(chance) + ")");
            }

            var contentArea = prevArea;
            contentArea.AddRadius(-HudLib.ThickBorderEdgeSize);
            HUD.RichBox.RichBoxGroup richBox = new HUD.RichBox.RichBoxGroup(contentArea.Position, contentArea.Width,
                HudLib.AttackWheelLayer, HudLib.MouseTipRichBoxSett, diceContent, true, true, false);
            images.Add(richBox);

            var bgArea = prevArea;
            bgArea.Height = richBox.area.Height + HudLib.ThickBorderEdgeSize * 2f;

            var bg = HudLib.ThinBorder(bgArea, HudLib.AttackWheelLayer + 1);
            images.Add(bg.images);


            return bgArea;
        }

        void battleModifierLabel(VectorRect prevArea)
        {
            if (attackRoll.setup.battleModifierAttack.HasContent ||
                attackRoll.setup.battleModifierDefence.HasContent)
            {
                prevArea.nextAreaY(1, Engine.Screen.BorderWidth);
                var contentArea = prevArea;                

                HUD.RichBox.RichBoxContent content = new HUD.RichBox.RichBoxContent();
                if (attackRoll.setup.battleModifierAttack.HasContent)
                {
                    content.h2("Attack modifiers");
                    content.newLine();
                    content.AddRange(attackRoll.setup.battleModifierAttack.content);
                }

                content.newLine();

                if (attackRoll.setup.battleModifierDefence.HasContent)
                {
                    content.h2("Defence modifiers");
                    content.newLine();
                    content.AddRange(attackRoll.setup.battleModifierDefence.content);
                }

                contentArea.AddRadius(-HudLib.ThickBorderEdgeSize);
                HUD.RichBox.RichBoxGroup richBox = new HUD.RichBox.RichBoxGroup(contentArea.Position, contentArea.Width,
                    HudLib.AttackWheelLayer, HudLib.MouseTipRichBoxSett, content, true, true, false);
                images.Add(richBox);

                var bgArea = prevArea;
                bgArea.Height = richBox.area.Height + HudLib.ThickBorderEdgeSize * 2f;

                var bg = HudLib.ThinBorder(bgArea, HudLib.AttackWheelLayer + 1);
                images.Add(bg.images);
            }

        }



         public void beginAttack()
        {
            player.mapControls?.removeAvailableTiles();
            state_1Attack_2end = 1;
            

            attackRoll.beginAttack();
        }

        public bool Update(ref PhaseUpdateArgs args)
        {
            args.blockTooltip = true;

            switch (state_1Attack_2end)
            {
                default:
                    attackRoll.idleUpdate();
                    break;
                case 1:
                    if (attackRoll.Update())
                    {
                        state_1Attack_2end++;
                    }
                    break;
                case 2:
                    attackRoll.postUpdate();
                    if (finalViewTime.CountDown())
                    {
                        attackRoll.attacker[0].OnEvent(Data.EventType.AttackComplete, true, null);

                        if (attackRoll.defender.Dead)
                        {
                            //Killed unit
                            attackRoll.attacker[0].OnEvent(Data.EventType.DestroyedTarget, true, null);

                            player.onDestroyedUnit(attackRoll.attacker[0], attackRoll.defender);
                        }
                        return true;
                    }
                    break;
            }

            return false;
        }

        public void DeleteMe()
        {
            attackRoll.DeleteMe();

            images.DeleteAll();
        }

        public bool AbleToFollowUp 
        {
            get
            {
                return attackRoll.setup.targets.IsMelee &&
                    (attackRoll.completedRetreats > 0 || attackRoll.defender.Dead); 
            }
        }

        public bool IsAttacking => state_1Attack_2end > 0;

    }


}

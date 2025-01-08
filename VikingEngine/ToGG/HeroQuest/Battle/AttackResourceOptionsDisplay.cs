using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest
{
    class AttackResourceOptionsDisplay
    {
        //Color selCol = Color.LightGray;
        //Color unselCol = Color.DarkGray;
        
        Display.Button spendStaminaButton;
        VectorRect spendStaminaButtonArea;
        //Graphics.ImageGroup images;
        AttackDisplay display;
        public AbsItem specialArrow = null;

        OptionsWindow<AbsSurgeOption> convertSurgesOptions;
        OptionsWindow<AbsItem> arrowOptions = null;

        public AttackResourceOptionsDisplay(VectorRect area, AttackDisplay display)
        {
            //images = new Graphics.ImageGroup();
            this.display = display;
            
            Vector2 position = area.Position;

            {//Convert Surges
                convertSurgesOptions = new OptionsWindow<AbsSurgeOption>("Convert surges", 
                    position, area.Width, display.localplayer.onSurgeOption);
                convertSurgesOptions.hoverEvent = onSurgeOptHover;
                //convertSurgesOptions.clickEvent = display.localplayer.onSurgeOption;

                List<AbsSurgeOption> surgeOptions = new List<AbsSurgeOption>(4);
                var weapon = display.attackRoll.targets.attackSettings.weapon;

                if (weapon != null && weapon.surgeOptions != null)
                {
                    surgeOptions.AddRange(weapon.surgeOptions);
                }

                surgeOptions.Add(new StaminaSurgeOption(2, 1));
                surgeOptions.Add(new RageSurgeOption(2, 1));

                int previousOption = -1;
                for (int i = 0; i < surgeOptions.Count; ++i)//each (var opt in surgeOptions)
                {
                    var opt = surgeOptions[i];
                    List<HUD.RichBox.AbsRichBoxMember> label = new List<HUD.RichBox.AbsRichBoxMember>(8);
                    opt.addConvertIcons(label);
                    label.Add(new HUD.RichBox.RbText("to " + opt.Name));

                    convertSurgesOptions.addOption(opt, label, null);

                    if (display.localplayer.surgeOption != null &&
                        display.localplayer.surgeOption.SurgeOptionType == opt.SurgeOptionType)
                    {
                        previousOption = i;
                    }
                }

                position = convertSurgesOptions.completeWindow(display.localplayer.surgeOption);

                if (previousOption >= 0)
                {
                    convertSurgesOptions.setOption(previousOption);
                }
            }
            
            position.Y += AttackDisplay.WindowsSpacing();

            {//Spend stamina
                spendStaminaButtonArea = new VectorRect(position.X, position.Y, area.Width, Engine.Screen.IconSize * 1.2f);
                
                refreshSpendStaminaButton();

                position.Y = spendStaminaButtonArea.Bottom + AttackDisplay.WindowsSpacing();
            }

            {//Arrow type
                if (display.attackRoll.targets.AttackType.IsRanged)
                {
                    var arrows = display.player.specialArrows();
                    if (arrows.Count > 0)
                    {
                        arrows.Insert(0, new Gadgets.SpecialArrow(Gadgets.ArrowType.Arrow, Gadgets.ArrowSpecialType.Normal, 99));

                        arrowOptions = new OptionsWindow<AbsItem>("Arrow type",
                            position, area.Width, selectSpecialArrow);

                        arrows.loopBegin();
                        while(arrows.loopNext())
                        {
                            if (arrows.IsFirst)
                            {
                                arrowOptions.addOption(null, new List<HUD.RichBox.AbsRichBoxMember>
                                {
                                    new HUD.RichBox.RbImage(arrows.sel.Icon),
                                    new HUD.RichBox.RbText(arrows.sel.Name),
                                }, null);
                            }
                            else
                            {
                                List<HUD.RichBox.AbsRichBoxMember> members = new List<HUD.RichBox.AbsRichBoxMember>();
                                members.Add(new HUD.RichBox.RbImage(arrows.sel.Icon));
                                members.Add(new HUD.RichBox.RbImage(SpriteName.cmdConvertArrow, HudLib.ConvertArrowScale));

                                members.AddRange(arrows.sel.DescriptionIcons());
                                members.Add(new HUD.RichBox.RbText(
                                    arrows.sel.Name + TextLib.Parentheses(arrows.sel.count.ToString(), true)));
                                
                                arrowOptions.addOption(arrows.sel, members,
                                    HUD.RichBox.AbsRichBoxMember.FromText(arrows.sel.Description));
                            }                            
                        }

                        arrowOptions.completeWindow(null);

                    }
                }
            }

            refreshAttackerStats();
        }

        public void update()
        {
            convertSurgesOptions.update();

            arrowOptions?.update();

            if (spendStaminaButton.update())
            {
                if (display.staminaAttackBoost.AddAttack((Unit)display.attackRoll.attacker))
                {
                    refreshAttackerStats();
                    refreshSpendStaminaButton();
                    display.refreshAttackCount();
                }
            }
            //else if (spendStaminaButton.HasMouseEnterLeaveEvent)
            //{
            //    spendStaminaButton.tooltip
            //}
        }

        void onSurgeOptHover(AbsSurgeOption option)
        {
            List<HUD.RichBox.AbsRichBoxMember> richBox = null;

            if (option is StaminaSurgeOption)
            {
                HUD.RichBox.AbsRichBoxMember.PrepareNewRow(ref richBox);
                display.attackRoll.attacker.staminaToRichbox(richBox);
            }
            if (option is RageSurgeOption)
            {
                HUD.RichBox.AbsRichBoxMember.PrepareNewRow(ref richBox);
                display.attackRoll.attacker.bloodrageToRichbox(richBox);
            }

            if (richBox != null)
            {
                convertSurgesOptions.AddHoverToolTip(richBox);
            }
        }

        //void setSurgeOption(AbsSurgeOption opt)
        //{
        //    display.localplayer.surgeOption = opt;
        //}

        void selectSpecialArrow(AbsItem arrow)
        {
            specialArrow = arrow;
        }

        void refreshSpendStaminaButton()
        {
            if (spendStaminaButton != null)
            {
                spendStaminaButton.DeleteMe();
            }

            if (display.staminaAttackBoost.boostCount < StaminaAttackBoost.MaxBoostCount)
            {                
                spendStaminaButton = new HeroQuest.Display.Button(
                    spendStaminaButtonArea, HudLib.AttackWheelLayer, Display.ButtonTextureStyle.Standard);
                spendStaminaButton.grayImagesOnDisable = true;
                spendStaminaButton.createTooltipAction = spendStaminaTooltip;

                List<Graphics.AbsDraw2D> imageStrip = new List<Graphics.AbsDraw2D>();

                Vector2 sz = Engine.Screen.SmallIconSizeV2;
                ImageLayers lay = HudLib.AttackWheelLayer - 1;
                int cost = display.staminaAttackBoost.addCost();
                for (int i = 0; i < cost; ++i)
                {
                    Graphics.Image stamina = new Graphics.Image(SpriteName.cmdStamina, Vector2.Zero, sz, lay);
                    imageStrip.Add(stamina);
                }

                Graphics.Image arrow = new Graphics.Image(SpriteName.cmdConvertArrow, Vector2.Zero, sz, lay);

                Graphics.Image plus = new Graphics.Image(SpriteName.cmdPlus, Vector2.Zero, sz, lay);
                Graphics.Image one = new Graphics.Image(SpriteName.cmd1, Vector2.Zero, sz, lay);
                Graphics.Image attackDie = new Graphics.Image(SpriteName.cmdDiceAttack, Vector2.Zero, sz, lay);

                imageStrip.Add(arrow); imageStrip.Add(plus); imageStrip.Add(one); imageStrip.Add(attackDie);

                spendStaminaButton.addImageStrip(imageStrip);

                if (display.attackRoll.attacker.data.hero.stamina.Value < cost)
                {
                    spendStaminaButton.Enabled = false;
                }
            }
        }

        void spendStaminaTooltip(Graphics.ImageGroup tooltip)
        {
            List<HUD.RichBox.AbsRichBoxMember> richBox = new List<HUD.RichBox.AbsRichBoxMember>();

            richBox.Add(new HUD.RichBox.RbText(
                "Spend " + display.staminaAttackBoost.addCost().ToString() + 
                " stamina to add 1 attack dice"));
            richBox.Add(new HUD.RichBox.RbNewLine(true));

            display.attackRoll.attacker.staminaToRichbox(richBox);
           
            spendStaminaButton.addTooltipText(richBox, Dir4.S);
        }

        public void refreshAttackerStats()
        {
            display.localplayer.hud.addInfoCardDisplay(
                new ToggEngine.Display2D.UnitCardDisplay(display.localplayer.HeroUnit,
                 new ToggEngine.Display2D.UnitDisplaySettings(false, true, true, false, false, false), display.localplayer.hud));
        }

        public void DeleteMe()
        {  
            convertSurgesOptions.DeleteMe();

            arrowOptions?.DeleteMe();

            spendStaminaButton.DeleteMe();
        }

    }
}

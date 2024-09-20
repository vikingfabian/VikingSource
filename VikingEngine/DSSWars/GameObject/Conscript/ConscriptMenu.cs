using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Conscript
{
    class ConscriptMenu
    {
        City city;
        LocalPlayer player;
        ProgressQue que = new ProgressQue();

        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;


            if (arraylib.InBound(city.barracks, city.selectedConscript))
            {
                BarracksStatus currentStatus = get();

                content.Add(new RichBoxBeginTitle(1));


                content.Add(new RichBoxText(DssRef.todoLang.Conscription_Title + " " + currentStatus.idAndPosition.ToString()));
                content.space();
                content.Add(new RichboxButton(new List<AbsRichBoxMember>
                    { new RichBoxSpace(), new RichBoxText(DssRef.todoLang.Hud_EndSessionIcon),new RichBoxSpace(), },
                    new RbAction(() => { city.selectedConscript = -1; })));

                content.newParagraph();

                HudLib.Label(content, DssRef.todoLang.Conscript_WeaponTitle);
                content.newLine();
                for (MainWeapon weapon = 0; weapon < MainWeapon.NUM; weapon++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Weapon(weapon))
                },
                new RbAction1Arg<MainWeapon>(weaponClick, weapon),
                new RbAction1Arg<MainWeapon>(weaponTooltip, weapon)
                );
                    button.setGroupSelectionColor(HudLib.RbSettings, weapon == currentStatus.profile.weapon);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.todoLang.Conscript_ArmorTitle);
                content.newLine();
                for (ArmorLevel armorLvl = 0; armorLvl < ArmorLevel.NUM; armorLvl++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( LangLib.Armor(armorLvl))
                    }, new RbAction1Arg<ArmorLevel>(armorClick, armorLvl),
                    new RbAction1Arg<ArmorLevel>(armorTooltip, armorLvl));
                    button.setGroupSelectionColor(HudLib.RbSettings, armorLvl == currentStatus.profile.armorLevel);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.todoLang.Conscript_TrainingTitle);
                content.newLine();
                TrainingLevel maxLevel = TrainingLevel.Professional;
                if (city.Culture == CityCulture.CrabMentality)
                {
                    maxLevel = TrainingLevel.Basic;
                }
                for (TrainingLevel training = 0; training <= maxLevel; training++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Training(training))
                }, new RbAction1Arg<TrainingLevel>(trainingClick, training),
                    new RbAction1Arg<TrainingLevel>(trainingTooltip, training));
                    button.setGroupSelectionColor(HudLib.RbSettings, training == currentStatus.profile.training);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                que.toHud(player, content, queClick, currentStatus.que);

                if (currentStatus.active != ConscriptActiveStatus.Idle)
                {
                    content.newParagraph();
                    content.Add(new RichBoxSeperationLine());
                    {
                        content.newLine();
                        content.BulletPoint();
                        var text = new RichBoxText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingEquipment));
                        text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        content.BulletPoint();
                        var text = new RichBoxText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingMen));
                        text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingMen ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        content.BulletPoint();
                        content.Add(new RichBoxText(currentStatus.longTimeProgress()));
                    }
                }
            }
            else
            {

                content.h2(DssRef.todoLang.Conscript_SelectBuilding);
                if (city.barracks.Count == 0)
                {
                    content.text(DssRef.todoLang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                }

                for (int i = 0; i < city.barracks.Count; ++i)
                {
                    content.newLine();

                    BarracksStatus currentProfile = city.barracks[i];
                    var caption = new RichBoxText(
                            LangLib.Weapon(currentProfile.profile.weapon) + ", " +
                            LangLib.Armor(currentProfile.profile.armorLevel) + ", " +
                            LangLib.Training(currentProfile.profile.training)
                        );
                    caption.overrideColor = HudLib.TitleColor_Name;

                    content.Add(new RichboxButton(new List<AbsRichBoxMember>(){
                        new RichBoxBeginTitle(2),
                        caption,
                        new RichBoxNewLine(),
                        new RichBoxText(currentProfile.shortActiveString())
                    }, new RbAction1Arg<int>(selectClick, i)));

                }
            }
        }

        void weaponClick(MainWeapon weapon)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.weapon = weapon;
            set(currentProfile);
        }

        void weaponTooltip(MainWeapon weapon)
        {

            RichBoxContent content = new RichBoxContent();
            content.Add(new RichBoxText(string.Format(DssRef.todoLang.Conscript_WeaponDamage, ConscriptProfile.WeaponDamage(weapon))));

            player.hud.tooltip.create(player, content, true);
        }
        void armorClick(ArmorLevel armor)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.armorLevel = armor;
            set(currentProfile);
        }
        void armorTooltip(ArmorLevel armor)
        {

            RichBoxContent content = new RichBoxContent
            {
                new RichBoxText(string.Format(DssRef.todoLang.Conscript_ArmorHealth, ConscriptProfile.ArmorHealth(armor)))
            };

            player.hud.tooltip.create(player, content, true);
        }

        void trainingClick(TrainingLevel training)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.training = training;

            set(currentProfile);
        }
        void trainingTooltip(TrainingLevel training)
        {

            RichBoxContent content = new RichBoxContent();
            content.text(string.Format(DssRef.todoLang.Conscript_TrainingTime, new TimeLength(ConscriptProfile.TrainingTime(training)).LongString()));
            content.text(string.Format(DssRef.todoLang.Conscript_TrainingSpeed, TextLib.OneDecimal(ConscriptProfile.TrainingAttackSpeed(training))));

            player.hud.tooltip.create(player, content, true);
        }
        void queClick(int length)
        {
            BarracksStatus currentStatus = get();
            currentStatus.que = length;
            set(currentStatus);
        }

        void selectClick(int index)
        {
            city.selectedConscript = index;
        }

        BarracksStatus get()
        {
            return city.barracks[city.selectedConscript];
        }

        void set(BarracksStatus profile)
        {
            city.barracks[city.selectedConscript] = profile;
        }


    }
}

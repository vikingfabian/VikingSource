using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Delivery;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Conscript
{
    class ConscriptMenu
    {
        static readonly MainWeapon[] DefaultWeapons = {
            MainWeapon.SharpStick,
            MainWeapon.Sword,
            MainWeapon.Bow,
            MainWeapon.Longbow,
            MainWeapon.Ballista,
        };

        static readonly MainWeapon[] NobelWeapons = {
            MainWeapon.TwoHandSword,
            MainWeapon.KnightsLance,
        };

        City city;
        LocalPlayer player;
        ProgressQue que = new ProgressQue();

        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;


            if (arraylib.InBound(city.conscriptBuildings, city.selectedConscript))
            {
                BarracksStatus currentStatus = get();

                content.Add(new RichBoxImage(
                            new SoldierConscriptProfile() { conscript = currentStatus.profile }.Icon()
                            ));
                content.space();
                content.Add(new RichBoxBeginTitle(1));

                string typeName = currentStatus.nobelmen? DssRef.lang.Building_NobleHouse : DssRef.lang.BuildingType_Barracks;
                var title = new RichBoxText(typeName + " " + currentStatus.idAndPosition.ToString());
                title.overrideColor = HudLib.TitleColor_TypeName;
                content.Add(title);
                //content.Add(new RichBoxText(typeName + " " + currentStatus.idAndPosition.ToString()));
                content.space();
                 HudLib.CloseButton(content,new RbAction(() => { city.selectedConscript = -1; }, SoundLib.menuBack));

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_WeaponTitle);
                content.newLine();
                MainWeapon[] weapons = currentStatus.nobelmen? NobelWeapons : DefaultWeapons;
                //for (MainWeapon weapon = 0; weapon < MainWeapon.NUM; weapon++)
                foreach (var weapon in weapons)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( LangLib.Weapon(weapon))
                    },
                    new RbAction1Arg<MainWeapon>(weaponClick, weapon, SoundLib.menu),
                    new RbAction1Arg<MainWeapon>(weaponTooltip, weapon)
                    );
                    button.setGroupSelectionColor(HudLib.RbSettings, weapon == currentStatus.profile.weapon);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_ArmorTitle);
                content.newLine();
                for (ArmorLevel armorLvl = 0; armorLvl < ArmorLevel.NUM; armorLvl++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( LangLib.Armor(armorLvl))
                    }, new RbAction1Arg<ArmorLevel>(armorClick, armorLvl, SoundLib.menu),
                    new RbAction1Arg<ArmorLevel>(armorTooltip, armorLvl));
                    button.setGroupSelectionColor(HudLib.RbSettings, armorLvl == currentStatus.profile.armorLevel);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_TrainingTitle);
                content.newLine();
                TrainingLevel minLevel = currentStatus.nobelmen? TrainingLevel.Basic : TrainingLevel.Minimal;
                
                TrainingLevel maxLevel = TrainingLevel.Professional;
                if (city.Culture == CityCulture.CrabMentality)
                {
                    maxLevel = TrainingLevel.Basic;
                }
                for (TrainingLevel training = minLevel; training <= maxLevel; training++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                        new RichBoxText( LangLib.Training(training))
                    }, new RbAction1Arg<TrainingLevel>(trainingClick, training, SoundLib.menu),
                    new RbAction2Arg<TrainingLevel, bool>(trainingTooltip, training, currentStatus.nobelmen));
                    button.setGroupSelectionColor(HudLib.RbSettings, training == currentStatus.profile.training);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.lang.Conscript_SpecializationTitle);
                content.space();
                HudLib.InfoButton(content, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    content.text(string.Format(DssRef.lang.Conscript_SpecializationDescription, TextLib.PercentText(DssConst.Conscript_SpecializePercentage)));
                    player.hud.tooltip.create(player, content, true);
                }));
                content.newLine();

                SpecializationType[] specializationTypes = currentStatus.profile.avaialableSpecializations();
                

                foreach (var specialization in specializationTypes)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( LangLib.SpecializationTypeName(specialization))
                    }, new RbAction1Arg<SpecializationType>(specializationClick, specialization, SoundLib.menu));
                    button.setGroupSelectionColor(HudLib.RbSettings, specialization == currentStatus.profile.specialization);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();


                que.toHud(player, content, queClick, currentStatus.que);


                content.newParagraph();
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(DssRef.lang.Hud_CopySetup) },
                    new RbAction(() =>
                    {
                        if (currentStatus.nobelmen)
                        {
                            player.knightConscriptCopy = currentStatus.inProgress;
                        }
                        else
                        {
                            player.soldierConscriptCopy = currentStatus.inProgress;
                        }
                    }, SoundLib.menu)));
                content.space();
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(DssRef.lang.Hud_Paste) },
                    new RbAction(() =>
                    {
                        BarracksStatus currentStatus = get();

                        if (currentStatus.nobelmen)
                        {
                            currentStatus.profile = player.knightConscriptCopy;
                        }
                        else
                        {
                            currentStatus.profile = player.soldierConscriptCopy;
                        }

                        set(currentStatus);

                    }, SoundLib.menu)));

                if (currentStatus.active != ConscriptActiveStatus.Idle)
                {
                    content.newParagraph();
                    content.Add(new RichBoxSeperationLine());
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RichBoxText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingEquipment));
                        text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        var text = new RichBoxText(currentStatus.activeStringOf(ConscriptActiveStatus.CollectingMen));
                        text.overrideColor = currentStatus.active > ConscriptActiveStatus.CollectingMen ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }

                    if (currentStatus.active == ConscriptActiveStatus.Training)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxText(currentStatus.longTimeProgress()));
                    }
                }
            }
            else
            {

                content.h2(DssRef.lang.Conscript_SelectBuilding).overrideColor = HudLib.TitleColor_Action;
                if (city.conscriptBuildings.Count == 0)
                {
                    //EMPTY
                    content.text(DssRef.lang.Hud_EmptyList).overrideColor = HudLib.InfoYellow_Light;
                    content.newParagraph();
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.Add( new RichBoxImage(SpriteName.WarsBuild_Barracks));
                    content.space();
                    content.Add(new RichBoxText(DssRef.lang.BuildingType_Barracks));
                    content.newLine();
                    content.text(DssRef.lang.Hud_RequirementOr);
                    content.newLine();
                    content.Add(new RichBoxImage(SpriteName.WarsBuild_Nobelhouse));
                    content.space();
                    content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse));
                }
                else
                {
                    for (int i = 0; i < city.conscriptBuildings.Count; ++i)
                    {
                        content.newLine();

                        BarracksStatus currentProfile = city.conscriptBuildings[i];
                        var caption = new RichBoxText(
                                LangLib.Weapon(currentProfile.profile.weapon) + ", " +
                                LangLib.Armor(currentProfile.profile.armorLevel)
                            );
                        caption.overrideColor = HudLib.TitleColor_Name;

                        var info = new RichBoxText(
                                currentProfile.shortActiveString()
                            );
                        info.overrideColor = HudLib.InfoYellow_Light;

                        content.Add(new RichboxButton(new List<AbsRichBoxMember>(){
                        new RichBoxImage(
                            new SoldierConscriptProfile(){ conscript = currentProfile.profile }.Icon()
                            ),
                        new RichBoxSpace(),
                        caption,
                        new RichBoxNewLine(),
                        info,
                    }, new RbAction1Arg<int>(selectClick, i, SoundLib.menu)));

                        //content.text(currentProfile.shortActiveString()).overrideColor = HudLib.InfoYellow_Light;

                    }
                }
            }
        }

        void specializationClick(SpecializationType specialization)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.specialization = specialization;
            set(currentProfile);

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
            content.Add(new RichBoxText(string.Format(DssRef.lang.Conscript_WeaponDamage, ConscriptProfile.WeaponDamage(weapon))));
            content.newParagraph();
            content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;
            var item = ConscriptProfile.WeaponItem(weapon);

            bool reachedBuffer = false;
            city.GetGroupedResource(item).toMenu(content, item, false, ref reachedBuffer);

            //if (reachedBuffer)
            //{
            //    GroupedResource.BufferIconInfo(content);
            //}
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
                new RichBoxText(string.Format(DssRef.lang.Conscript_ArmorHealth, ConscriptProfile.ArmorHealth(armor)))
            };

            if (armor != ArmorLevel.None)
            {
                content.newParagraph();
                content.h2(DssRef.lang.Hud_Available).overrideColor = HudLib.TitleColor_Label;
                var item = ConscriptProfile.ArmorItem(armor);

                bool reachedBuffer = false;
                city.GetGroupedResource(item).toMenu(content, item, false, ref reachedBuffer);
                //if (reachedBuffer)
                //{
                //    GroupedResource.BufferIconInfo(content);
                //}
            }

            player.hud.tooltip.create(player, content, true);
        }

        void trainingClick(TrainingLevel training)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.training = training;

            set(currentProfile);
        }
        void trainingTooltip(TrainingLevel training, bool nobel)
        {

            RichBoxContent content = new RichBoxContent();
            content.text(string.Format(DssRef.lang.Conscript_TrainingTime, new TimeLength(ConscriptProfile.TrainingTime(training, nobel)).LongString()));
            content.text(string.Format(DssRef.lang.Conscript_TrainingSpeed, TextLib.OneDecimal(ConscriptProfile.TrainingAttackSpeed(training))));

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
            return city.conscriptBuildings[city.selectedConscript];
        }

        void set(BarracksStatus profile)
        {
            var spec = profile.profile.avaialableSpecializations();
            if (!spec.Contains(profile.profile.specialization))
            {
                profile.profile.specialization = spec[0];
            }

            city.conscriptBuildings[city.selectedConscript] = profile;

            city.onConscriptChange();
        }


    }
}

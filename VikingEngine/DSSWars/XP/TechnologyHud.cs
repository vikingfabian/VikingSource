﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.XP
{
    class TechnologyHud
    {
        public void technologyOverviewHud(RichBoxContent content, LocalPlayer player, City city, Faction faction)
        {
            TechnologyTemplate technology;
            if (city != null)
            {
                technology = city.technology;
            }
            else
            {
                technology = faction.technology;
            }

            content.newLine();
            content.Add(new RbImage(SpriteName.WarsTechnology_Unlocked));

            tech(technology.advancedBuilding, SpriteName.WarsBuild_Nobelhouse, DssRef.todoLang.Technology_AdvancedBuildings);

            tech(technology.advancedFarming, SpriteName.WarsWorkFarm, DssRef.todoLang.Technology_AdvancedFarming);

            tech(technology.advancedCasting, SpriteName.WarsResource_IronManCannon, DssRef.todoLang.Technology_AdvancedCasting);

            tech(technology.iron, SpriteName.WarsResource_Iron, DssRef.lang.Resource_TypeName_Iron);

            tech(technology.steel, SpriteName.WarsResource_Steel, DssRef.todoLang.Resource_TypeName_Steel);

            tech(technology.catapult, SpriteName.WarsResource_Catapult, DssRef.todoLang.Resource_TypeName_Catapult);

            tech(technology.blackPowder, SpriteName.WarsResource_BronzeRifle, DssRef.todoLang.Resource_TypeName_BlackPowder);

            tech(technology.gunPowder, SpriteName.WarsResource_IronRifle, DssRef.todoLang.Resource_TypeName_GunPowder);

            void tech(int value, SpriteName icon, string caption)
            {
                bool unlocked = value >= TechnologyTemplate.Unlocked;

                if (unlocked)
                {
                    var infoContent = new RichBoxContent();

                    infoContent.Add(new RbImage(icon));

                    var infoButton = new RbButton(infoContent, null, new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();

                        content.h2(DssRef.todoLang.Technology_Title).overrideColor = HudLib.TitleColor_Label;
                        content.newLine();
                        content.Add(new RbImage(icon));
                        content.space();
                        content.Add(new RbText(caption));


                        player.hud.tooltip.create(player, content, true);
                    }));

                    infoButton.overrideBgColor = HudLib.InfoYellow_BG;
                    content.Add(infoButton);
                    content.space();
                }
            }
        }

        public void technologyHud(RichBoxContent content, LocalPlayer player, City city, Faction faction)
        {
            bool cityView;
            TechnologyTemplate technology;
            int unlockValue; //cityView ? TechnologyTemplate.Unlocked : 1;
            if (city != null)
            {
                technology = city.technology;
                cityView = true;
                unlockValue = TechnologyTemplate.Unlocked;
            }
            else
            {
                technology = faction.technology;
                cityView = false;
                unlockValue = 1;
            }

            var advBuildingFields = new List<WorkExperienceType>
            {
                WorkExperienceType.HouseBuilding,
                WorkExperienceType.StoneCutter,
            };
            var advFarmingFields = new List<WorkExperienceType>
            {
                WorkExperienceType.Farm,
                WorkExperienceType.AnimalCare,
            };
            var advCastingFields = new List<WorkExperienceType>
            {
                WorkExperienceType.Smelting,
                WorkExperienceType.CastMetal,
            };
            var ironSteelFields = new List<WorkExperienceType>
            {
                WorkExperienceType.Mining,
                WorkExperienceType.CraftMetal,
            };
            var catapultFields = new List<WorkExperienceType>
            {
                WorkExperienceType.WoodWork,
                WorkExperienceType.Fletcher,
            };

            var gunPowderFields = new List<WorkExperienceType>
            {
                WorkExperienceType.CraftFuel,
                WorkExperienceType.Chemistry,
            };

            

            Unlocks advBuildingUnlock = new Unlocks(); advBuildingUnlock.UnlockAdvancedBuilding();
            tech(technology.advancedBuilding, TechnologyTemplate.Start.advancedBuilding, SpriteName.WarsBuild_Nobelhouse, DssRef.todoLang.Technology_AdvancedBuildings, advBuildingUnlock, advBuildingFields);

            content.newParagraph();
            Unlocks advFarmUnlock = new Unlocks(); advFarmUnlock.UnlockAdvancedFarming();
            tech(technology.advancedFarming, TechnologyTemplate.Start.advancedFarming, SpriteName.WarsWorkFarm, DssRef.todoLang.Technology_AdvancedFarming, advFarmUnlock, advFarmingFields);

            content.newParagraph();
            Unlocks advCastingUnlock = new Unlocks(); advCastingUnlock.UnlockAdvancedCasting();
            tech(technology.advancedCasting, TechnologyTemplate.Start.advancedCasting, SpriteName.WarsResource_IronManCannon, DssRef.todoLang.Technology_AdvancedCasting, advCastingUnlock, advCastingFields);

            content.newParagraph();
            Unlocks ironUnlock = new Unlocks(); ironUnlock.UnlockIron();
            tech(technology.iron, TechnologyTemplate.Start.iron, SpriteName.WarsResource_Iron, DssRef.lang.Resource_TypeName_Iron, ironUnlock, ironSteelFields);

            Unlocks steelUnlock = new Unlocks(); steelUnlock.UnlockSteel();
            tech(technology.steel, TechnologyTemplate.Start.steel, SpriteName.WarsResource_Steel, DssRef.todoLang.Resource_TypeName_Steel, steelUnlock, ironSteelFields);

            content.newParagraph();

            Unlocks catapultUnlock = new Unlocks(); catapultUnlock.UnlockCatapult();
            tech(technology.catapult, TechnologyTemplate.Start.catapult, SpriteName.WarsResource_Catapult, DssRef.todoLang.Resource_TypeName_Catapult, catapultUnlock, catapultFields);

            content.newParagraph();

            Unlocks blackpowUnlock = new Unlocks(); blackpowUnlock.UnlockBlackPowder();
            tech(technology.blackPowder, TechnologyTemplate.Start.blackPowder, SpriteName.WarsResource_BronzeRifle, DssRef.todoLang.Resource_TypeName_BlackPowder, blackpowUnlock, gunPowderFields);

            Unlocks gunpowUnlock = new Unlocks(); gunpowUnlock.UnlockGunPowder();
            tech(technology.gunPowder, TechnologyTemplate.Start.gunPowder, SpriteName.WarsResource_IronRifle, DssRef.todoLang.Resource_TypeName_GunPowder, gunpowUnlock, gunPowderFields);


            content.newParagraph();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsRelationGood));
                content.Add(new RbText($"{DssRef.lang.Diplomacy_RelationType_Good}: {string.Format(DssRef.todoLang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_GoodRelation_PerMin))}"));
                content.space();

                HudLib.InfoButton(content, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    var info = new RbText(string.Format(DssRef.todoLang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Good,
                        string.Format(DssRef.todoLang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_GoodRelation_PerMin))));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);

                    player.hud.tooltip.create(player, content, true);
                }));
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsRelationAlly));
                content.Add(new RbText($"{DssRef.lang.Diplomacy_RelationType_Ally}: {string.Format(DssRef.todoLang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_AllyRelation_PerMin))}"));
                content.space();

                HudLib.InfoButton(content, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    var info = new RbText(string.Format(DssRef.todoLang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Ally,
                    string.Format(DssRef.todoLang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_AllyRelation_PerMin))));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);

                    player.hud.tooltip.create(player, content, true);
                }));
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsCityHall));
                content.Add(new RbText($"{DssRef.lang.UnitType_City}: {string.Format(DssRef.todoLang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_CitySpread))}"));
                content.space();

                HudLib.InfoButton(content, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    var info = new RbText(string.Format(DssRef.todoLang.Technology_CitySpread,
                    string.Format(DssRef.todoLang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_CitySpread))));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);

                    player.hud.tooltip.create(player, content, true);
                }));
            }

            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(LangLib.ExperienceLevelIcon(ExperienceLevel.Master_4)));
                content.Add(new RbText($"{DssRef.todoLang.ExperienceLevel_4}: {TextLib.PlusMinus(DssConst.TechnologyGain_Master)}"));
                content.space();

                HudLib.InfoButton(content, new RbAction(() =>
                {
                    RichBoxContent content = new RichBoxContent();
                    var info = new RbText(string.Format(DssRef.todoLang.Technology_ForEachMaster, DssRef.lang.ResourceType_Workers, DssRef.todoLang.ExperienceLevel_4, TextLib.PlusMinus(DssConst.TechnologyGain_Master)));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);

                    player.hud.tooltip.create(player, content, true);
                }));
            }

            content.newLine();
            {
                HudLib.BulletPoint(content);
                var info = new RbText(DssRef.todoLang.Technology_CityCapture);
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }

            void tech(int value, int startValue, SpriteName icon, string caption, Unlocks unlocks, List<WorkExperienceType> experienceField)
            {
                content.newLine();

                
                bool unlocked = value >= unlockValue;

                if (!cityView)
                {
                    if (value >= faction.cities.Count)
                    {
                        caption += $" ({DssRef.todoLang.Hud_AllCities})";
                    }
                    else
                    {
                        caption += $" ({value}/{faction.cities.Count})";
                    }
                }

                var infoContent = new RichBoxContent();

                infoContent.Add(new RbImage(unlocked ? SpriteName.WarsTechnology_Unlocked : SpriteName.WarsTechnology_Locked));
                infoContent.Add(new RbImage(icon));
                infoContent.space();
               
                var captionText = new RbText(caption);
                captionText.overrideColor = unlocked ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                infoContent.Add(captionText);

                var infoButton = new RbButton(infoContent, null, new RbAction(() =>
                {
                    var items = unlocks.ListItems();
                    var buildings = unlocks.ListBuildings();
                    RichBoxContent content = new RichBoxContent();

                    content.h2(DssRef.todoLang.Hud_Unlock).overrideColor = HudLib.TitleColor_Label;
                    foreach (var item in items)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(Resource.ResourceLib.Icon(item)));
                        content.space();
                        content.Add(new RbText(Display.Translation.LangLib.Item(item)));
                    }

                    foreach (var item in buildings)
                    {
                        var opt = Build.BuildLib.BuildOptions[(int)item];

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(opt.sprite));
                        content.space();
                        content.Add(new RbText(opt.Label()));
                    }

                    content.newParagraph();

                    content.h2(DssRef.todoLang.Technology_ShareField).overrideColor = HudLib.TitleColor_Label;
                    foreach (var xpType in experienceField)
                    {
                        LangLib.ExperienceType(xpType, out string name, out SpriteName icon);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(icon));
                        content.space();
                        content.Add(new RbText(name));
                    }

                    player.hud.tooltip.create(player, content, true);
                }));

                infoButton.overrideBgColor = HudLib.InfoYellow_BG;
                content.Add(infoButton);

                if (cityView && !unlocked)
                {
                    content.space(2f);
                    content.Add(new RbText($"({value - startValue} / {TechnologyTemplate.Unlocked - startValue})"));
                }
            }
        }

    }
}

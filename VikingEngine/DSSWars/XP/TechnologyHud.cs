using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface.HudPinUi;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.XP
{
    class TechnologyHud
    {
        LocalPlayer player; City city;
        public TechnologyHud(LocalPlayer player, City city)
        {
            this.player = player;
            this.city = city;
        }


        public void technologyOverviewHud(RichBoxContent content, Faction faction)
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
            content.space();

            tech(technology.advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock, SpriteName.WarsBuild_Nobelhouse, DssRef.lang.Technology_AdvancedBuildings);
            tech(technology.advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock, SpriteName.WarsWorkFarm, DssRef.lang.Technology_AdvancedFarming);
            tech(technology.advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock, SpriteName.WarsResource_IronManCannon, DssRef.lang.Technology_AdvancedCasting);

            tech(technology.iron.points, XpLib.Unlock.IronUnlock, SpriteName.WarsResource_Iron, TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Iron));
            tech(technology.steel.points, XpLib.Unlock.SteelUnlock, SpriteName.WarsResource_Steel, DssRef.lang.Resource_TypeName_Steel);
            tech(technology.catapult.points, XpLib.Unlock.CatapultUnlock, SpriteName.WarsResource_Catapult, DssRef.lang.Resource_TypeName_Catapult);
            tech(technology.blackPowder.points, XpLib.Unlock.BlackPowderUnlock, SpriteName.WarsResource_BronzeRifle, XpLib.TechnologyName_BlackPowder());
            tech(technology.gunPowder.points, XpLib.Unlock.GunPowderUnlock, SpriteName.WarsResource_IronRifle, DssRef.lang.Resource_TypeName_GunPowder);


            void tech(int value, int unlock, SpriteName icon, string caption)
            {
                bool unlocked = value >= unlock;

                if (unlocked)
                {
                    var infoContent = new RichBoxContent();

                    infoContent.Add(new RbImage(icon));

                    var infoButton = new ArtButton( RbButtonStyle.HoverArea, infoContent, null, 
                        new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        //RichBoxContent content = new RichBoxContent();

                        content.h2(DssRef.lang.Technology_Title).overrideColor = HudLib.TitleColor_Label;
                        content.newLine();
                        content.Add(new RbImage(icon));
                        content.space();
                        content.Add(new RbText(caption));

                    }));
                                        
                    content.Add(infoButton);
                }
            }
        }

        public void technologyHud(RichBoxContent content, Faction faction)
        {
            bool cityView;
            TechnologyTemplate technology;
            //int unlockValue; //cityView ? TechnologyTemplate.Unlocked : 1;
            if (city != null)
            {
                technology = city.technology;
                cityView = true;
                //unlockValue = TechnologyTemplate.Unlocked;
            }
            else
            {
                if (faction == null)
                {
                    return;
                }
                technology = faction.technology;
                cityView = false;
                //unlockValue = 1;
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
            tech( TechnologyTreeType.advancedBuilding,technology.advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock, SpriteName.WarsBuild_Nobelhouse, DssRef.lang.Technology_AdvancedBuildings, advBuildingUnlock, advBuildingFields);

            content.newParagraph();
            Unlocks advFarmUnlock = new Unlocks(); advFarmUnlock.UnlockAdvancedFarming();
            tech( TechnologyTreeType.advancedFarming,technology.advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock, SpriteName.WarsWorkFarm, DssRef.lang.Technology_AdvancedFarming, advFarmUnlock, advFarmingFields);

            content.newParagraph();
            Unlocks advCastingUnlock = new Unlocks(); advCastingUnlock.UnlockAdvancedCasting();
            tech( TechnologyTreeType.advancedCasting,technology.advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock, SpriteName.WarsResource_IronManCannon, DssRef.lang.Technology_AdvancedCasting, advCastingUnlock, advCastingFields);

            content.newParagraph();
            Unlocks ironUnlock = new Unlocks(); ironUnlock.UnlockIron();
            tech( TechnologyTreeType.iron ,technology.iron.points, XpLib.Unlock.IronUnlock, SpriteName.WarsResource_Iron, TextLib.LargeFirstLetter( DssRef.lang.Resource_TypeName_Iron), ironUnlock, ironSteelFields);

            Unlocks steelUnlock = new Unlocks(); steelUnlock.UnlockSteel();
            tech(TechnologyTreeType.steel, technology.steel.points, XpLib.Unlock.SteelUnlock, SpriteName.WarsResource_Steel, DssRef.lang.Resource_TypeName_Steel, steelUnlock, ironSteelFields);

            content.newParagraph();
            Unlocks catapultUnlock = new Unlocks(); catapultUnlock.UnlockCatapult();
            tech(TechnologyTreeType.catapult, technology.catapult.points, XpLib.Unlock.CatapultUnlock, SpriteName.WarsResource_Catapult, DssRef.lang.Resource_TypeName_Catapult, catapultUnlock, catapultFields);

            content.newParagraph();
            Unlocks blackpowUnlock = new Unlocks(); blackpowUnlock.UnlockBlackPowder();
            tech(TechnologyTreeType.blackPowder, technology.blackPowder.points, XpLib.Unlock.BlackPowderUnlock, SpriteName.WarsResource_BronzeRifle, DssRef.lang.Resource_TypeName_BlackPowder, blackpowUnlock, gunPowderFields);

            Unlocks gunpowUnlock = new Unlocks(); gunpowUnlock.UnlockGunPowder();
            tech(TechnologyTreeType.gunPowder, technology.gunPowder.points, XpLib.Unlock.GunPowderUnlock, SpriteName.WarsResource_IronRifle, DssRef.lang.Resource_TypeName_GunPowder, gunpowUnlock, gunPowderFields);


            content.newParagraph();
            content.h2(DssRef.lang.Technology_GainTitle, HudLib.TitleColor_Label);
            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsRelationGood));
                content.Add(new RbText($"{DssRef.lang.Diplomacy_RelationType_Good}: {string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_GoodRelation_PerMin))}"));
                content.space();

                HudLib.InfoButton(content, new RbTooltip(tip)); //new RbAction(() =>
                //{
                //    RichBoxContent content = new RichBoxContent();
                //    var info = new RbText(string.Format(DssRef.lang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Good,
                //        string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_GoodRelation_PerMin))));
                //    info.overrideColor = HudLib.InfoYellow_Light;
                //    content.Add(info);

                //    player.hud.tooltip.create(player, content, true);
                //}));

                void tip(RichBoxContent content, object tag)
                {
                    var info = new RbText(string.Format(DssRef.lang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Good,
                       string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_GoodRelation_PerMin))));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);
                }
            }

            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsRelationAlly));
                content.Add(new RbText($"{DssRef.lang.Diplomacy_RelationType_Ally}: {string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_AllyRelation_PerMin))}"));
                content.space();

                HudLib.InfoButton(content, new RbTooltip(tip));

                void tip(RichBoxContent content, object tag)
                {
                    var info = new RbText(string.Format(DssRef.lang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Ally,
                     string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_AllyRelation_PerMin))));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);
                }
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsCityHall));
                content.Add(new RbText($"{DssRef.lang.UnitType_City}: {string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_CitySpread))}"));
                content.space();

                HudLib.InfoButton(content, new RbTooltip(tip));

                void tip(RichBoxContent content, object tag)
                {
                    var info = new RbText(string.Format(DssRef.lang.Technology_CitySpread,
                    string.Format(DssRef.lang.Hud_PointsPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_CitySpread))));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);
                }
            }

            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(LangLib.ExperienceLevelIcon(ExperienceLevel.Practitioner_2)));
                content.Add(new RbText($"{DssRef.lang.Technology_LevelUp}: {TextLib.PlusMinus(DssConst.TechnologyGain_AnyLevelUp)}"));
                content.space();
                
                HudLib.InfoButton(content, new RbTooltip(tip));

                void tip(RichBoxContent content, object tag)
                {
                    var info = new RbText(string.Format(DssRef.lang.Technology_ForEachLevelUp, TextLib.PlusMinus(DssConst.TechnologyGain_AnyLevelUp)));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);
                }
            }

            content.newLine();
            {
                HudLib.BulletPoint(content);
                content.Add(new RbImage(LangLib.ExperienceLevelIcon(ExperienceLevel.Master_4)));
                content.Add(new RbText($"{DssRef.lang.ExperienceLevel_4}: {TextLib.PlusMinus(DssConst.TechnologyGain_MasterLevelUp)}"));
                content.space();

                HudLib.InfoButton(content, new RbTooltip(tip));

                void tip(RichBoxContent content, object tag)
                {
                    var info = new RbText(string.Format(DssRef.lang.Technology_ForEachMaster, DssRef.lang.ResourceType_Workers, DssRef.lang.ExperienceLevel_4, TextLib.PlusMinus(DssConst.TechnologyGain_MasterLevelUp)));
                    info.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info);
                }
            }

            content.newLine();
            {
                HudLib.BulletPoint(content);
                var info = new RbText(DssRef.lang.Technology_CityCapture);
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }

            void tech(TechnologyTreeType type, int value, int unlock, SpriteName icon, string caption, Unlocks unlocks, List<WorkExperienceType> experienceField)
            {
                content.newLine();


                bool unlocked = value >= unlock;

                if (!cityView)
                {
                    if (value >= faction.cities.Count)
                    {
                        caption += $" ({DssRef.lang.Hud_AllCities})";
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

                var infoButton = new ArtButton(RbButtonStyle.HoverArea, infoContent, null,
                    new RbTooltip(techTip, new TechTipArgs() { experienceField = experienceField, unlocks = unlocks }));
                
                content.Add(infoButton);

                if (cityView && !unlocked)
                {
                    content.space(0.5f);
                    content.Add(new RbText($"({value} / {unlock})"));
                }

                if (DssRef.difficulty.GodPowers())
                {
                    content.space();
                    content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsTechnology_Locked) },
                     new RbAction1Arg<TechnologyTreeType>(godPowerLockTech, type)));
                    content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsTechnology_Unlocked) },
                     new RbAction1Arg<TechnologyTreeType>(godPowerUnLockTech, type)));
                }

                if (city != null)
                {
                    content.Add(new RbTab(0.7f));
                    CityHudPinId pinId = new CityHudPinId(city.myIndex, new HudPin(type));
                    player.hud.pins.toggleButton(content, pinId);
                }
                //bool onHud = player.hud.pins.isPinnedProperty(pinId, false, false);
                //content.Add(new ArtToggle(onHud, new List<AbsRichBoxMember> {
                //    new RbImage(SpriteName.HudPinIcon, 1f, onHud? Color.White : Color.Gray) }, new RbAction(() => { player.hud.pins.Set(pinId, !onHud); }),
                //    new RbTooltip_Text( DssRef.lang.HudPins)));
            }

            void godPowerLockTech(TechnologyTreeType type)
            {
                ref var progress = ref TechnologyTemplate.GetResearchProgressRef(ref city.technology, type);
                progress.points = 0;
            }
            void godPowerUnLockTech(TechnologyTreeType type)
            {
                ref var progress = ref TechnologyTemplate.GetResearchProgressRef(ref city.technology, type);
                progress.points = 100000;
            }
        }

        class TechTipArgs
        {
            public bool cityView;
            public int value;
            public Unlocks unlocks;
            public List<WorkExperienceType> experienceField;
        }
        void techTip(RichBoxContent content, object tag)
        {
            TechTipArgs args = (TechTipArgs)tag;

            var items = args.unlocks.ListItems();
            var buildings = args.unlocks.ListBuildings();
            //RichBoxContent content = new RichBoxContent();

            content.h2(DssRef.lang.Hud_Unlock).overrideColor = HudLib.TitleColor_Label;
            foreach (var item in items)
            {
                IconName.Item(item, out SpriteName itemIcon, out string itemName);
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(itemIcon));
                content.space();
                content.Add(new RbText(itemName));
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

            content.h2(DssRef.lang.Technology_ShareField).overrideColor = HudLib.TitleColor_Label;
            foreach (var xpType in args.experienceField)
            {
                LangLib.ExperienceType(xpType, out string name, out SpriteName icon);
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(icon));
                content.space();
                content.Add(new RbText(name));
            }
        }

        

    }
}

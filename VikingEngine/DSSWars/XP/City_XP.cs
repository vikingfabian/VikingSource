using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.XP;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;
using VikingEngine.DSSWars.Display.Translation;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public TechnologyTemplate technology = new TechnologyTemplate();

        public ExperienceLevel topskill_Farm = 0;
        public ExperienceLevel topskill_AnimalCare = 0;
        public ExperienceLevel topskill_HouseBuilding = 0;
        public ExperienceLevel topskill_WoodCutter = 0;
        public ExperienceLevel topskill_StoneCutter = 0;
        public ExperienceLevel topskill_Mining = 0;
        public ExperienceLevel topskill_Transport = 0;
        public ExperienceLevel topskill_Cook = 0;
        public ExperienceLevel topskill_Fletcher = 0;
        public ExperienceLevel topskill_Smelting = 0;
        public ExperienceLevel topskill_Casting = 0;
        public ExperienceLevel topskill_CraftMetal = 0;
        public ExperienceLevel topskill_CraftArmor = 0;
        public ExperienceLevel topskill_CraftWeapon = 0;
        public ExperienceLevel topskill_CraftFuel = 0;
        public ExperienceLevel topskill_Chemistry = 0;

        public ExperienceOrDistancePrio experenceOrDistance = ExperienceOrDistancePrio.Mix;

        

        public void technologyHud(RichBoxContent content, LocalPlayer player)
        {

            var advBuildingFields = new List<WorkExperienceType>
            {   
                WorkExperienceType.HouseBuilding,
                WorkExperienceType.StoneCutter,
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
                WorkExperienceType.WoodCutter,
                WorkExperienceType.Fletcher,
            };

            var gunPowderFields = new List<WorkExperienceType>
            {
                WorkExperienceType.CraftFuel,
                WorkExperienceType.Chemistry,
            };

            Unlocks advBuildingUnlock = new Unlocks(); advBuildingUnlock.UnlockAdvancedBuilding();
            tech(technology.advancedBuilding, SpriteName.WarsBuild_Nobelhouse, DssRef.todoLang.Technology_AdvancedBuildings, advBuildingUnlock, advBuildingFields);

            content.newParagraph();
            Unlocks advCastingUnlock = new Unlocks(); advCastingUnlock.UnlockAdvancedCasting();
            tech(technology.advancedBuilding, SpriteName.WarsResource_IronManCannon, DssRef.todoLang.Technology_AdvancedCasting, advCastingUnlock, advCastingFields);

            content.newParagraph();
            Unlocks ironUnlock = new Unlocks(); ironUnlock.UnlockIron();
            tech(technology.iron, SpriteName.WarsResource_Iron, DssRef.lang.Resource_TypeName_Iron, ironUnlock, ironSteelFields);

            Unlocks steelUnlock = new Unlocks(); steelUnlock.UnlockSteel();
            tech(technology.steel, SpriteName.WarsResource_Steel, DssRef.todoLang.Resource_TypeName_Steel, steelUnlock, ironSteelFields);

            content.newParagraph();

            Unlocks catapultUnlock = new Unlocks(); catapultUnlock.UnlockCatapult();
            tech(technology.catapult, SpriteName.WarsResource_Catapult, DssRef.todoLang.Resource_TypeName_Catapult, catapultUnlock, catapultFields);

            content.newParagraph();

            Unlocks blackpowUnlock = new Unlocks(); blackpowUnlock.UnlockBlackPowder();
            tech(technology.iron, SpriteName.WarsResource_BronzeRifle, DssRef.todoLang.Resource_TypeName_BlackPowder, blackpowUnlock, gunPowderFields);
            
            Unlocks gunpowUnlock = new Unlocks(); gunpowUnlock.UnlockGunPowder();
            tech(technology.iron, SpriteName.WarsResource_IronRifle, DssRef.todoLang.Resource_TypeName_GunPowder, gunpowUnlock, gunPowderFields);


            content.newParagraph();
            {
                HudLib.BulletPoint(content);
                var info = new RichBoxText(string.Format(DssRef.todoLang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Good,
                    string.Format(DssRef.todoLang.Hud_PercentPerMinute, TextLib.PlusMinus( DssConst.TechnologyGain_GoodRelation_PerMin))));
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                var info = new RichBoxText(string.Format(DssRef.todoLang.Technology_GainByNeigborRelation, DssRef.lang.Diplomacy_RelationType_Ally,
                    string.Format(DssRef.todoLang.Hud_PercentPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_AllyRelation_PerMin))));
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                var info = new RichBoxText(string.Format(DssRef.todoLang.Technology_CitySpread,
                    string.Format(DssRef.todoLang.Hud_PercentPerMinute, TextLib.PlusMinus(DssConst.TechnologyGain_CitySpread))));
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                var info = new RichBoxText(DssRef.todoLang.Technology_CityCapture);
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }
            content.newLine();
            {
                HudLib.BulletPoint(content);
                var info = new RichBoxText(string.Format(DssRef.todoLang.Technology_ForEachMaster, DssRef.lang.ResourceType_Workers, DssRef.todoLang.ExperienceLevel_4, TextLib.PlusMinus(DssConst.TechnologyGain_Master)));
                info.overrideColor = HudLib.InfoYellow_Light;
                content.Add(info);
            }


            void tech(int value, SpriteName icon, string caption, Unlocks unlocks, List<WorkExperienceType> experienceField)
            {
                content.newLine();

                bool unlocked = value >= TechnologyTemplate.Unlocked;

                var infoContent = new RichBoxContent();

                infoContent.Add(new RichBoxImage(unlocked? SpriteName.WarsTechnology_Unlocked: SpriteName.WarsTechnology_Locked));
                infoContent.Add(new RichBoxImage(icon));
                infoContent.space();
                var captionText = new RichBoxText(caption);
                captionText.overrideColor = unlocked? HudLib.AvailableColor : HudLib.NotAvailableColor;
                infoContent.Add(captionText);

                var infoButton = new RichboxButton(infoContent, null, new RbAction(() =>
                {
                    var items = unlocks.ListItems();
                    var buildings = unlocks.ListBuildings();
                    RichBoxContent content = new RichBoxContent();

                    content.h2(DssRef.todoLang.Hud_Unlock).overrideColor = HudLib.TitleColor_Label;
                    foreach (var item in items)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxImage(Resource.ResourceLib.Icon(item)));
                        content.space();
                        content.Add(new RichBoxText(Display.Translation.LangLib.Item(item)));
                    }

                    foreach (var item in buildings)
                    {
                        var opt = Build.BuildLib.BuildOptions[(int)item];

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxImage(opt.sprite));
                        content.space();
                        content.Add(new RichBoxText(opt.Label()));
                    }

                    content.newParagraph();

                    content.h2(DssRef.todoLang.Technology_ShareField).overrideColor = HudLib.TitleColor_Label;
                    foreach (var xpType in experienceField)
                    {
                        LangLib.ExperienceType(xpType, out string name, out SpriteName icon);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxImage(icon));
                        content.space();
                        content.Add(new RichBoxText(name));
                    }

                    player.hud.tooltip.create(player, content, true);
                }));

                infoButton.overrideBgColor = HudLib.InfoYellow_BG;
                content.Add(infoButton);

                if (!unlocked)
                {
                    content.space(2f);
                    content.Add(new RichBoxText($"({TechnologyTemplate.PercentProgress(value)}%)"));
                }
            }
        }
    }
}

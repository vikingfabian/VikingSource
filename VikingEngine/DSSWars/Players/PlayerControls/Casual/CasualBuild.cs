using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Players.PlayerControls.Casual
{
    enum CasualBuildCategory
    { 
        Build,
        UpgradeBuilding,
        Technology,
    }

    enum CasualBuildType
    {
        Tent,
        WorkerHut,
        Barracks,
        GuardTower_Wood,
        GuardTower_Stone,
        StartUpBarracks,
        Logistics,
        ResearchCenter,
        
        UnlockIronArmor,
        UnlockSteelArmor,
        UnlockSword,
        UnlockSteelSword,
        UnlockCatapult,
        UnlockBlackPower,
        UnlockGunPower,
        UnlockFarming2,
        UnlockFarming3,
        Embassy,
        NUM
    }

    struct CasualBuildPurchase
    {
        public CasualBuildType buildType;
        public int count;
    }

    class CasualBuildOption
    { 
        public CasualBuildCategory category;
        public CasualBuildType Type;
        public string Name;
        public bool upgradeIcon;
        public SpriteName icon;
        public int price;
        public int buildtime_sec;
    }

    struct CasualBuildQueueItem
    {
        public CasualBuildType build;
        public int count;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)build);
            w.Write((ushort)count);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            build = (CasualBuildType)r.ReadByte();
            count = r.ReadUInt16();
        }
    }


    static class CasualBuild
    {
        public static CasualBuildOption[] CasualBuildOptionList;

        public static void Init()
        {
            CasualBuildOptionList = new CasualBuildOption[(int)CasualBuildType.NUM];

            add(new CasualBuildOption 
            {
                category = CasualBuildCategory.Build,
                Type = CasualBuildType.WorkerHut,
                Name = DssRef.lang.BuildingType_WorkerHut,
                icon = SpriteName.WarsBuild_WorkerHuts,
                price = 400,
                buildtime_sec = (int)DssConst.WorkTime_Building_Default,
            });
            add(new CasualBuildOption 
            {
                category = CasualBuildCategory.Build,
                Type = CasualBuildType.Barracks,
                Name = DssRef.lang.BuildingType_Barracks,
                icon = SpriteName.WarsBuild_Barracks,
                price = 600,
                buildtime_sec = (int)DssConst.WorkTime_Building_Default * 2,
            });
            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Build,
                Type = CasualBuildType.GuardTower_Wood,
                Name = DssRef.lang.BuildingType_WoodTower,
                icon = SpriteName.WarsBuild_WoodTower,
                price = 200,
                buildtime_sec = (int)DssConst.WorkTime_Building_Small,
                //allowMultiBuild = true
            });
            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Build,
                Type = CasualBuildType.GuardTower_Stone,
                Name = DssRef.lang.BuildingType_StoneTower,
                icon = SpriteName.WarsBuild_StoneTower,
                price = 300,
                buildtime_sec = (int)DssConst.WorkTime_Building_Default,
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.UpgradeBuilding,
                Type = CasualBuildType.Logistics,
                Name = DssRef.lang.BuildingType_Logistics,
                icon = SpriteName.WarsBuild_Logistics,
                price = 1000,
                buildtime_sec = (int)DssConst.WorkTime_Building_Large,
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.UpgradeBuilding,
                Type = CasualBuildType.Embassy,
                Name = DssRef.lang.BuildingType_Embassy,
                icon = SpriteName.WarsBuild_Embassy,
                price = 2000,
                buildtime_sec = (int)DssConst.WorkTime_Building_Large,
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.UpgradeBuilding,
                Type = CasualBuildType.ResearchCenter,
                Name = DssRef.lang.BuildingType_ReseachCenter,
                icon = SpriteName.WarsBuild_ResearchCenter,
                price = 1000,
                buildtime_sec = (int)DssConst.WorkTime_Building_Large,
                //allowMultiBuild = false
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockIronArmor,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                    DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_IronArmor),
                icon = SpriteName.WarsResource_IronArmor,
                price = 2000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level2_Minutes * TimeExt.MinuteInSeconds),
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockSteelArmor,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                                DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_FullPlateArmor),
                icon = SpriteName.WarsResource_FullPlateArmor,
                price = 3000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level3_Minutes * TimeExt.MinuteInSeconds),
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockSword,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                   DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_Sword),
                icon = SpriteName.WarsResource_Sword,
                price = 2000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level2_Minutes * TimeExt.MinuteInSeconds),
            });
            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockSteelSword,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                                DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_LongSword),
                icon = SpriteName.WarsResource_Longsword,
                price = 3000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level3_Minutes * TimeExt.MinuteInSeconds),
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockCatapult,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                    DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_Catapult),
                icon = SpriteName.WarsResource_Catapult,
                price = 3000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level2_Minutes * TimeExt.MinuteInSeconds),
            });
            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockBlackPower,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                                DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_BlackPowder),
                icon = SpriteName.WarsResource_BlackPowder,
                price = 4000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level3_Minutes * TimeExt.MinuteInSeconds),
            });
            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockGunPower,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                                DssRef.lang.Hud_Unlock, DssRef.lang.Resource_TypeName_GunPowder),
                icon = SpriteName.WarsResource_GunPowder,
                price = 6000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level4_Minutes * TimeExt.MinuteInSeconds),
            });

            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockFarming2,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                    DssRef.lang.Hud_Unlock, DssRef.lang.Technology_AdvancedFarming),
                icon = SpriteName.WarsResource_Toolkit,
                price = 2000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level2_Minutes * TimeExt.MinuteInSeconds),
            });
            add(new CasualBuildOption
            {
                category = CasualBuildCategory.Technology,
                Type = CasualBuildType.UnlockFarming3,
                Name = string.Format(DssRef.lang.Language_ItemCount_Colon,
                                DssRef.lang.Hud_Unlock, DssRef.lang.Technology_ModernFarming),
                icon = SpriteName.WarsResource_Wagon4Wheel,
                price = 3000,
                buildtime_sec = (int)(DssConst.WorkTime_CasualResearch_Level3_Minutes * TimeExt.MinuteInSeconds),
            });

            void add(CasualBuildOption buildOption)
            {
                CasualBuildOptionList[(int)buildOption.Type] = buildOption;
            }
        }

        public static CasualBuildOption Get(CasualBuildType type)
        { 
            return CasualBuildOptionList[(int)type];
        }

        //static readonly int[] BuildCountOptions = [4, 8, 20];



        public static void ToHud(LocalPlayer player, RichBoxContent content, City city)
        {
            var profile = city.casualCityProfile;
            var progress = city.GetCasualProgress();
            profile.availableBuildings(city, out List<CasualBuildType> available, out List<CasualBuildType> complete);

            foreach (var buildType in available)
            {
                CasualBuildOption option = CasualBuildOptionList[(int)buildType];
                if (option != null)
                {
                    AddBuildButton(option, false);
                }
            }

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember>
                    {
                        new RbImage( SpriteName.AutomationGearIcon),
                        new RbSpace(0.5f),
                        new RbImage( SpriteName.WarsConstructBuildingIcon),
                        new RbSpace(),
                        new RbText(DssRef.lang.Hud_Purchase_AllBuildings) },
                        new RbAction(queueAllCasualBuildings), null));
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember>
                    {
                        new RbImage( SpriteName.AutomationGearIcon),
                        new RbSpace(0.5f),
                        new RbImage( SpriteName.WarsTechnology_Unlocked),
                        new RbSpace(),
                        new RbText(DssRef.lang.Hud_Purchase_AllTech) },
                        new RbAction(queueAllCasualProgress), null));

            //CURRENT PROGRESS
            city.GetCasualProgress().BuildToHud(player, city, content);

            if (complete.Count > 0)
            {
                content.newParagraph();
                content.Add(new RbSeperationLine());
                content.h2(DssRef.lang.Hud_Available, HudLib.TitleColor_Label);
                foreach (var buildType in complete)
                {
                    CasualBuildOption option = CasualBuildOptionList[(int)buildType];
                    if (option != null)
                    {
                        AddBuildButton(option, true);
                    }
                }
            }

            void AddBuildButton(CasualBuildOption option, bool complete)
            {
                content.newLine();
                
                int count = city.getCount(option.Type);
                int maxCount = city.getMaxCount(option.Type);
                bool mayQueue = count < maxCount;

                switch (option.category)
                {
                    default:
                        content.Add(new RbText(count.ToString()));
                        break;
                    case CasualBuildCategory.Technology:
                        content.Add(new RbImage(count > 0? SpriteName.WarsTechnology_Unlocked : SpriteName.WarsTechnology_Locked));
                        break;
                }
                content.Add(new RbTab(0.06f));

                {
                    bool canAfford = player.faction.hasGold(option.price, city);

                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
                    {
                        new RbImage(option.icon),
                        new RbSpace(),
                        new RbText(option.Name),
                        new RbSpace(2),
                        new RbImage(SpriteName.rtsMoney),
                        new RbText(option.price.ToString(), canAfford ? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark)
                    }, new RbAction2Arg<CasualBuildType, int>(city.CasualBuild, option.Type, 1),
                    new RbTooltip(buildTooltip, new CasualBuildPurchase() { buildType = option.Type, count = 1 }), !complete && mayQueue));
                }

                if (option.category == CasualBuildCategory.Build)
                {
                    int maxBuild = maxCount - count;
                    if (maxBuild >= 4)
                    {
                        xButton(4);
                        if (maxBuild >= 6)
                        {
                            if (maxBuild <= 10)
                            {
                                xButton(maxBuild);
                            }
                            else
                            {
                                xButton(8);
                                xButton(maxBuild);
                            }
                        }
                    }

                    void xButton(int buildcount)
                    {
                        if (buildcount <= maxCount)
                        {
                            bool canAfford = player.faction.hasGold(option.price * buildcount, city);

                            if (count + buildcount <= maxCount)
                            {
                                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>
                                {
                                    new RbText(string.Format( DssRef.lang.Hud_XTimes, buildcount), canAfford ? HudLib.AvailableColor_Dark : HudLib.NotAvailableColor_Dark)
                                }, new RbAction2Arg<CasualBuildType, int>(city.CasualBuild, option.Type, buildcount),
                                new RbTooltip(buildTooltip, new CasualBuildPurchase() { buildType = option.Type, count = buildcount }), !complete && mayQueue));
                            }
                        }
                    }
                }
            }

            void queueAllCasualProgress()
            {
                /*
                Tent,
                WorkerHut,
                Barracks,
                GuardTower_Wood,
                GuardTower_Stone,
                StartUpBarracks,
                Logistics,
                ResearchCenter,
        
                UnlockIronArmor,
                UnlockSteelArmor,
                UnlockSword,
                UnlockSteelSword,
                UnlockCatapult,
                UnlockBlackPower,
                UnlockGunPower,
                UnlockFarming2,
                UnlockFarming3,
                Embassy,
                NUM
                 */
                List<CasualBuildType> allProgress = [
                    CasualBuildType.Logistics,
                    CasualBuildType.ResearchCenter,
                    CasualBuildType.UnlockIronArmor,
                    CasualBuildType.UnlockSword,
                    CasualBuildType.UnlockCatapult,
                    CasualBuildType.UnlockFarming2,
                    CasualBuildType.UnlockSteelArmor,
                    CasualBuildType.UnlockSteelSword,
                    CasualBuildType.UnlockBlackPower,
                    CasualBuildType.UnlockGunPower,
                    CasualBuildType.UnlockFarming3,
                ];

                city.CasualBuild(allProgress);
            }
            void queueAllCasualBuildings()
            {
                List< CasualBuildType> allBuildings = new List< CasualBuildType >(64);

                allBuildings.Add(CasualBuildType.Logistics);
                for (int i = 0; i < 8; i++)
                {
                    allBuildings.Add(CasualBuildType.WorkerHut);
                    allBuildings.Add(CasualBuildType.WorkerHut);
                    allBuildings.Add(CasualBuildType.Barracks);
                    //allBuildings.Add(CasualBuildType.Tent);
                    allBuildings.Add(CasualBuildType.GuardTower_Stone);
                    if (i == 2)
                    {
                        allBuildings.Add(CasualBuildType.Embassy);
                    }
                }
                city.CasualBuild(allBuildings);
            }

            

            void buildTooltip(RichBoxContent content, object tag)
            {
                var buildPurchase = (CasualBuildPurchase)tag;
                CasualBuildOption option = CasualBuildOptionList[(int)buildPurchase.buildType];

                content.h1(option.Name, HudLib.TitleColor_Head);
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);

                content.newLine();
                HudLib.BulletPoint(content);
                HudLib.ResourceCost(content, ResourceType.Gold, option.price * buildPurchase.count, (int)player.faction.GetGold(city));

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.IconSandGlass));
                content.space();
                content.Add(new RbText(DssRef.lang.BuildHud_BuildTime + ": " + new TimeLength(option.buildtime_sec).LongString()));
                               
                content.newParagraph();
                
                switch (buildPurchase.buildType)
                {
                    case CasualBuildType.Tent:
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(SpriteName.WarsUnitIcon_Immigrant));
                        content.space();
                        content.Add(new RbText(string.Format(DssRef.lang.BuildingType_ImmigrationTent_Description, DssConst.ImmigrantionTent_Capacity)));
                        break;

                    case CasualBuildType.WorkerHut:
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(SpriteName.WarsWorkerAdd));
                        content.space();
                        content.Add(new RbText(string.Format(DssRef.lang.CityOption_ExpandWorkForce_IncreaseMax, DssConst.HousingCount_WorkerHut)));
                        break;

                    case CasualBuildType.Barracks:
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.BuildingType_CasualBarracks_Description));
                        break;

                    case CasualBuildType.GuardTower_Wood:
                    case CasualBuildType.GuardTower_Stone:
                        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(SpriteName.WarsGuardPostIcon));
                        content.space();
                        content.Add(new RbText(DssRef.lang.Defence_GuardPost));
                        break;

                    case CasualBuildType.Logistics:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding));
                        content.Add(new RbImage(SpriteName.WarsBuild_Tent));
                        content.Add(new RbText(DssRef.lang.BuildingType_ImmigrationTent));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding));
                        content.Add(new RbImage(SpriteName.WarsBuild_StoneWall));
                        content.Add(new RbText(DssRef.lang.BuildingType_StoneWall));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding));
                        content.Add(new RbImage(SpriteName.WarsBuild_ResearchCenter));
                        content.Add(new RbText(DssRef.lang.BuildingType_ReseachCenter));
                        break;

                    case CasualBuildType.Embassy:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        content.newLine();
                        Build.BuildControls.EmbassyDescription(content);
                        break;

                    case CasualBuildType.ResearchCenter:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding));
                        content.Add(new RbImage(SpriteName.WarsTechnology_Unlocked));
                        content.Add(new RbText(DssRef.lang.Technology_Title));
                        break;

                    case CasualBuildType.UnlockIronArmor:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.IronArmor);
                        break;
                    case CasualBuildType.UnlockSteelArmor:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.FullPlateArmor);
                        break;

                    case CasualBuildType.UnlockSword:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.Sword);
                        break;
                    case CasualBuildType.UnlockSteelSword:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.LongSword);
                        break;

                    case CasualBuildType.UnlockCatapult:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.Catapult);
                        unlockItemHud(ItemResourceType.Crossbow);
                        break;
                    case CasualBuildType.UnlockBlackPower:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.ManCannonBronze);
                        unlockItemHud(ItemResourceType.HandCulverin);
                        break;
                    case CasualBuildType.UnlockGunPower:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        unlockItemHud(ItemResourceType.ManCannonIron);
                        unlockItemHud(ItemResourceType.Rifle);
                        break;

                    case CasualBuildType.UnlockFarming2:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(SpriteName.rtsIncomeTime));
                        content.space();
                        content.Add(new RbText(string.Format(DssRef.lang.Economy_TaxIncome, "+" + TextLib.TwoDecimal(Money.CopperToGold * DssConst.Casual_Farm2TaxIncreasePercUnits_copp))));
                        break;

                    case CasualBuildType.UnlockFarming3:
                        content.h2(DssRef.lang.Hud_Unlock, HudLib.TitleColor_Label);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbImage(SpriteName.rtsIncomeTime));
                        content.space();
                        content.Add(new RbText(string.Format(DssRef.lang.Economy_TaxIncome, "+" + TextLib.TwoDecimal(Money.CopperToGold * DssConst.Casual_Farm3TaxIncreasePercUnits_copp))));
                        break;

                }

                content.newParagraph();
                content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn, HudLib.TitleColor_Label);
                content.newLine();
                int count = city.getCount(option.Type);
                content.Add(new RbText(string.Format(DssRef.lang.Language_XCountIsY, option.Name, count)));

                if (option.category == CasualBuildCategory.Build)
                {                    
                    int maxCount = city.getMaxCount(option.Type);
                    bool mayQueue = count < maxCount;
                    
                    content.newLine();
                    content.Add(new RbImage(mayQueue ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
                    content.hspace();
                    content.Add(new RbText(string.Format(DssRef.lang.Resource_MaxAmount, maxCount), mayQueue? HudLib.AvailableColor : HudLib.NotAvailableColor));
                }
            
                void unlockItemHud(ItemResourceType item)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.birdUnLock));
                    IconName.Item(item, out SpriteName itemIcon, out string itemName);
                    content.Add(new RbImage(itemIcon));
                    content.Add(new RbText(itemName));
                }
            
            }
        }



    }
}

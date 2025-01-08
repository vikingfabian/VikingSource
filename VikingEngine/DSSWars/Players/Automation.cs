using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players
{
    class Automation
    {
        //static readonly Build.BuildAndExpandType[] AutoBuildOptions =
        //    {
        //        Build.BuildAndExpandType.WorkerHuts,
        //        Build.BuildAndExpandType.WheatFarms,
        //        Build.BuildAndExpandType.LinnenFarms,
        //        Build.BuildAndExpandType.PigPen,
        //        Build.BuildAndExpandType.HenPen,
        //    };

        Players.LocalPlayer player;
        //bool autoRecruit= false;
        bool autoBuild = false;
        bool autoBuild_intelligent = true;
        bool autoExpandGuard = false;
        //bool autoNobelhouse = false;
        bool autoRepair = false;
        bool autoUpgradeLogistics = false;
        //int[] recruitAmount = new int[DssLib.AvailableUnitTypes.Length];

        AutomationAction automationAction = AutomationAction.WaitForUpdate;
        City cityAction = null;
        UnitType recruitType = UnitType.NULL;
        int recruitCount = 0;
        IntVector2 subtilePos;

        Build.BuildAndExpandType autoBuildType = Build.BuildAndExpandType.WorkerHuts;

        public Automation(Players.LocalPlayer player)
        {
            this.player = player;
        }

        public void writeGameState(BinaryWriter w)
        {
            //w.Write(autoRecruit);
            w.Write(autoRepair);
            w.Write(autoBuild);
            w.Write(autoBuild_intelligent);
            w.Write((byte)autoBuildType);
            w.Write(autoExpandGuard);
            w.Write(autoUpgradeLogistics);

            //foreach (var recruit in recruitAmount)
            //{
            //    w.Write((byte)recruit);
            //}
        }

        public void readGameState(BinaryReader r, int subVersion)
        {
            //autoRecruit = r.ReadBoolean();

            autoRepair = r.ReadBoolean();
            autoBuild = r.ReadBoolean();
            autoBuild_intelligent = r.ReadBoolean();
            autoBuildType = (Build.BuildAndExpandType)r.ReadByte();
            autoExpandGuard = r.ReadBoolean();

            if (subVersion >= 30)
            {
                autoUpgradeLogistics = r.ReadBoolean();
            }
            //for (int i =0; i< recruitAmount.Length;++i)
            //{
            //    recruitAmount[i] = r.ReadByte();
            //}
        }

        //bool AutoRecruitProperty(int index, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        autoRecruit = value;
        //    }
        //    return autoRecruit;
        //}

        bool AutoBuildProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoBuild = value;
            }
            return autoBuild;
        }
        bool AutoBuildIntelligentProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoBuild_intelligent = value;
            }
            return autoBuild_intelligent;
        }

        bool AutoExpandGuardProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoExpandGuard = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return autoExpandGuard;
        }

        bool AutoRepairCityProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoRepair = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return autoRepair;
        }

        bool AutoUpgradeLogisticsProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoUpgradeLogistics = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return autoUpgradeLogistics;
        }

        //bool AutoNobelHouseProperty(int index, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        autoNobelhouse = value;
        //    }
        //    return autoNobelhouse;
        //}

        //int RecruitAmountProperty(int index, bool set, int value)
        //{
        //    if (set)
        //    {
        //        recruitAmount[index] = Bound.Set(value, 0, 100);
        //    }
        //    return recruitAmount[index];
        //}

        public void toMenu(RichBoxContent content, bool fullDisplay)
        {
            //content.h1(DssRef.lang.Automation_Title);
            content.newParagraph();
            //content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
            //    {
            //        new RichBoxText( DssRef.lang.UnitType_Recruit),
            //    }, AutoRecruitProperty));

            //for (int i = 0; i < DssLib.AvailableUnitTypes.Length; i++)
            //{
            //    content.PlusMinusInt(SpriteName.WarsGroupIcon, DssLib.AvailableUnitTypes[i].ToString(), RecruitAmountProperty, i);
            //}

            //content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
            //    {
            //        new RichBoxText( DssRef.lang.CityOption_AutoBuild),
            //    }, AutoBuildProperty));
            //content.newLine();

            //if (autoBuild)
            //{
            //    content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
            //    {
            //        new RichBoxText(DssRef.lang.CityOption_AutoBuild_Intelligent),
            //    }, AutoBuildIntelligentProperty));

            //    content.newLine();
            //    foreach (var opt in AutoBuildOptions)
            //    {
            //        var optButton = new RichboxButton(new List<AbsRichBoxMember> {
            //        new RichBoxText(BuildLib.BuildOptions[(int)opt].Label())
            //        }, new RbAction1Arg<Build.BuildAndExpandType>(selectBuildOption, opt));
            //            optButton.setGroupSelectionColor(HudLib.RbSettings, opt == autoBuildType);
            //        content.Add(optButton);
            //        content.space();
            //    }
            //}
            //content.newParagraph();

            content.Add(new RbCheckbox(new List<AbsRichBoxMember>
                {
                    new RbText( DssRef.lang.CityOption_Repair),
                }, AutoRepairCityProperty));

            content.newLine();

            content.Add(new RbCheckbox(new List<AbsRichBoxMember>
                {
                    new RbText(string.Format(DssRef.lang.XP_UpgradeBuildingX, DssRef.lang.BuildingType_Logistics)),
                }, AutoUpgradeLogisticsProperty));

            content.newLine();
            //content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
            //    {
            //        new RichBoxText(string.Format( DssRef.lang.HudAction_BuyItem, DssRef.lang.Building_NobleHouse)),
            //    }, AutoNobelHouseProperty));

            //content.newLine();

            content.Add(new RbCheckbox(new List<AbsRichBoxMember>
                {
                    new RbText( DssRef.lang.CityOption_ExpandGuardSize),
                }, AutoExpandGuardProperty));

            content.newLine();

            List<string> listinfo = new List<string> 
            {
                DssRef.lang.Automation_InfoLine_MaxWorkforce,
                DssRef.lang.Automation_InfoLine_NegativeIncome,
                DssRef.lang.Automation_InfoLine_Priority,
                DssRef.lang.Automation_InfoLine_PurchaseSpeed,
            };

            foreach (var m in listinfo)
            {
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(m));
            }

        }

        public void asyncUpdate()
        {
            if (automationAction == AutomationAction.ProcessReady)
            {
                automationAction = AutomationAction.WaitForUpdate;

                if (player.faction.MoneySecDiff() > 0)
                {
                    var citiesC = player.faction.cities.counter();

                    for (CityType type = CityType.Factory; type >= CityType.Small; type--)
                    {
                        citiesC.Reset();
                        while (citiesC.Next())
                        {
                            if (citiesC.sel.CityType == type &&
                                citiesC.sel.isMaxHomeUsers() )
                                //&&
                                //citiesC.sel.battleGroup == null)
                            {
                                if (autoRepair && citiesC.sel.damages.HasValue())
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.Repair;
                                    return;
                                }

                                if (autoUpgradeLogistics && citiesC.sel.autoUpgradeLogistics(IntVector2.Zero, false))
                                {   
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.UpgradeLogistics;
                                    CityStructure.AutomationInstance.update(citiesC.sel, 0, 4);
                                    subtilePos = CityStructure.AutomationInstance.EmptyLand.Last();
                                    return;
                                }

                                if (autoExpandGuard && citiesC.sel.canIncreaseGuardSize(1, true))
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.GuardSize;
                                    return;
                                }

                            }
                        }
                    }
                }
            }
        }

        void selectBuildOption(Build.BuildAndExpandType opt)
        {
            autoBuildType = opt;
        }

        public void oneSecondUpdate()
        {
            switch (automationAction)
            {
                case AutomationAction.UpgradeLogistics:
                    cityAction.autoUpgradeLogistics(subtilePos, true);
                    break;

                case AutomationAction.Repair:
                    cityAction.buyRepair(true, true);
                    break;

                case AutomationAction.GuardSize:
                    cityAction.buyCityGuards(true, 1);
                    break;
            }

            cityAction = null;
            automationAction = AutomationAction.ProcessReady;

        }

        //public Build.BuildAndExpandType AutoExpandType(out bool intelligent)
        //{
        //    intelligent = autoBuild_intelligent;
        //    if (autoBuild)
        //    {
        //        return autoBuildType;
        //    }
        //    else
        //    { 
        //        return Build.BuildAndExpandType.NUM_NONE;
        //    }
        //}
    }

    enum AutomationAction
    { 
        WaitForUpdate,
        ProcessReady,

        Recruit,
        Repair,
        ExpandWorkforce,
        /*NobelHouse*/
        GuardSize,
        UpgradeLogistics,
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
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
        bool autoRecruit= false;
        bool autoBuild = false;
        bool autoBuild_intelligent = true;
        bool autoExpandGuard = false;
        bool autoNobelhouse = false;
        bool autoRepair = false;
        int[] recruitAmount = new int[DssLib.AvailableUnitTypes.Length];

        AutomationAction automationAction = AutomationAction.WaitForUpdate;
        City cityAction = null;
        UnitType recruitType = UnitType.NULL;
        int recruitCount = 0;

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
            w.Write(autoNobelhouse);

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
            autoNobelhouse = r.ReadBoolean();

            //for (int i =0; i< recruitAmount.Length;++i)
            //{
            //    recruitAmount[i] = r.ReadByte();
            //}
        }

        bool AutoRecruitProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoRecruit = value;
            }
            return autoRecruit;
        }

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
            }
            return autoExpandGuard;
        }

        bool AutoRepairCityProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoRepair = value;
            }
            return autoRepair;
        }

        bool AutoNobelHouseProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoNobelhouse = value;
            }
            return autoNobelhouse;
        }

        int RecruitAmountProperty(int index, bool set, int value)
        {
            if (set)
            {
                recruitAmount[index] = Bound.Set(value, 0, 100);
            }
            return recruitAmount[index];
        }

        public void toMenu(RichBoxContent content, bool fullDisplay)
        {
            content.h1(DssRef.lang.Automation_Title);
            content.newLine();
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
            //        new RichBoxText( DssRef.todoLang.CityOption_AutoBuild),
            //    }, AutoBuildProperty));
            //content.newLine();

            //if (autoBuild)
            //{
            //    content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
            //    {
            //        new RichBoxText(DssRef.todoLang.CityOption_AutoBuild_Intelligent),
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

            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_Repair),
                }, AutoRepairCityProperty));

            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText(string.Format( DssRef.lang.HudAction_BuyItem, DssRef.lang.Building_NobleHouse)),
                }, AutoNobelHouseProperty));

            content.newLine();
            
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_ExpandGuardSize),
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
                content.BulletPoint();
                content.Add(new RichBoxText(m));
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
                                citiesC.sel.isMaxHomeUsers() &&
                                citiesC.sel.battleGroup == null)
                            {
                                if (autoRepair && citiesC.sel.damages.HasValue())
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.Repair;
                                    return;
                                }

                                if (autoExpandGuard && citiesC.sel.canIncreaseGuardSize(1))
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.GuardSize;
                                    return;
                                }

                                if (autoNobelhouse && citiesC.sel.canBuyNobelHouse())
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.NobelHouse;
                                    return;
                                }

                                //if (autoBuild && citiesC.sel.canExpandWorkForce(1))
                                //{
                                //    cityAction = citiesC.sel;
                                //    automationAction = AutomationAction.ExpandWorkforce;
                                //    return;
                                //}

                                if (autoRecruit)
                                {
                                    const int RecruitChunk = 5;
                                    var army = citiesC.sel.recruitToClosestArmy();
                                    Dictionary<UnitType, int> typeCount = null;
                                    if (army != null)
                                    {
                                        typeCount = army.Status().getTypeCounts();
                                    }

                                    for (int maxCount = RecruitChunk; maxCount <= 100; maxCount += RecruitChunk)
                                    {
                                        for (int i = 0; i < DssLib.AvailableUnitTypes.Length; i++)
                                        {
                                            recruitCount = Bound.Max(recruitAmount[i], maxCount);

                                            var unitType = DssLib.AvailableUnitTypes[i];

                                            if (recruitCount > 0 && 
                                                citiesC.sel.HasUnitPurchaseOption(unitType))
                                            {
                                                
                                                int current = 0;
                                                if (typeCount != null)
                                                {
                                                    typeCount.TryGetValue(unitType, out current);
                                                }

                                                if (current < recruitCount)
                                                {
                                                    cityAction = citiesC.sel;
                                                    recruitType = unitType;
                                                    recruitCount = Bound.Max(recruitAmount[i] - current, RecruitChunk);
                                                    automationAction = AutomationAction.Recruit;
                                                    return;
                                                }
                                            }
                                        }
                                    }
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
                case AutomationAction.NobelHouse:
                    cityAction.buyNobelHouseAction();
                    break;
                case AutomationAction.Repair:
                    cityAction.buyRepair(true, true);
                    break;
                //case AutomationAction.ExpandWorkforce:
                //    cityAction.buyWorkforce(true, 1);
                //    break;
                //case AutomationAction.Recruit:
                //    cityAction.buySoldiersAction(recruitType, recruitCount, null);
                //    break;
                case AutomationAction.GuardSize:
                    cityAction.buyCityGuards(true, 1);
                    break;
            }

            cityAction = null;
            automationAction = AutomationAction.ProcessReady;

        }

        public Build.BuildAndExpandType AutoExpandType(out bool intelligent)
        {
            intelligent = autoBuild_intelligent;
            if (autoBuild)
            {
                return autoBuildType;
            }
            else
            { 
                return Build.BuildAndExpandType.NUM_NONE;
            }
        }
    }

    enum AutomationAction
    { 
        WaitForUpdate,
        ProcessReady,

        Recruit,
        Repair,
        ExpandWorkforce,
        NobelHouse,
        GuardSize,
    }
}

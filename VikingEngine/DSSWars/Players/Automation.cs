using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players
{
    class Automation
    {
        Players.LocalPlayer player;
        bool autoRecruit= false;
        bool autoExpandCity = false;
        bool autoNobelhouse = false;
        bool autoRepair = false;
        int[] recruitAmount = new int[DssLib.AvailableUnitTypes.Length];

        AutomationAction automationAction = AutomationAction.WaitForUpdate;
        City cityAction = null;
        UnitType recruitType = UnitType.NULL;
        int recruitCount = 0;

        public Automation(Players.LocalPlayer player)
        {
            this.player = player;
        }

        public void writeGameState(BinaryWriter w)
        {
            w.Write(autoRecruit);
            w.Write(autoRepair);
            w.Write(autoExpandCity);
            w.Write(autoNobelhouse);

            foreach (var recruit in recruitAmount)
            {
                w.Write((byte)recruit);
            }
        }

        public void readGameState(BinaryReader r, int subVersion)
        {
            autoRecruit = r.ReadBoolean();

            if (subVersion >= 4)
            {
                autoRepair = r.ReadBoolean();
            }

            autoExpandCity = r.ReadBoolean();
            autoNobelhouse= r.ReadBoolean();

            for (int i =0; i< recruitAmount.Length;++i)
            {
                recruitAmount[i] = r.ReadByte();
            }
        }

        bool AutoRecruitProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoRecruit = value;
            }
            return autoRecruit;
        }

        bool AutoExpandCityProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoExpandCity = value;
            }
            return autoExpandCity;
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
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_Recruit),
                }, AutoRecruitProperty));
                        
            for (int i = 0; i < DssLib.AvailableUnitTypes.Length; i++)
            {
                content.PlusMinusInt(SpriteName.WarsGroupIcon, DssLib.AvailableUnitTypes[i].ToString(), RecruitAmountProperty, i);
            }

            content.newParagraph();

            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_Repair),
                }, AutoRepairCityProperty));

            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_ExpandWorkForce),
                }, AutoExpandCityProperty));

            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText(string.Format( DssRef.lang.HudAction_BuyItem, DssRef.lang.Building_NobleHouse)),
                }, AutoNobelHouseProperty));

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
                content.ListDot();
                content.Add(new RichBoxText(m));
            }

        }

        public void asyncUpdate()
        {
            if (automationAction == AutomationAction.ProcessReady)
            {
                automationAction = AutomationAction.WaitForUpdate;

                if (player.faction.NetIncome() > 0)
                {
                    var citiesC = player.faction.cities.counter();

                    for (CityType type = CityType.Factory; type >= CityType.Small; type--)
                    {
                        citiesC.Reset();
                        while (citiesC.Next())
                        {
                            if (citiesC.sel.CityType == type &&
                                citiesC.sel.isMaxWorkForce())
                            {
                                if (autoRepair && citiesC.sel.damages.HasValue())
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.Repair;
                                    return;
                                }

                                if (autoNobelhouse && citiesC.sel.canBuyNobelHouse())
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.NobelHouse;
                                    return;
                                }

                                if (autoExpandCity && citiesC.sel.canExpandWorkForce(1))
                                {
                                    cityAction = citiesC.sel;
                                    automationAction = AutomationAction.ExpandWorkforce;
                                    return;
                                }

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
                case AutomationAction.ExpandWorkforce:
                    cityAction.buyWorkforce(true, 1);
                    break;
                case AutomationAction.Recruit:
                    cityAction.buySoldiersAction(recruitType, recruitCount);
                    break;
            }

            cityAction = null;
            automationAction = AutomationAction.ProcessReady;

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
    }
}

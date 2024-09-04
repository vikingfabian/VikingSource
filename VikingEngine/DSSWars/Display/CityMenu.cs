using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.PJ;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class CityMenu
    {
        static readonly MenuTab[] Tabs = { MenuTab.Recruit, MenuTab.Resources, MenuTab.Work, MenuTab.Trade };
        Players.LocalPlayer player;
        City city;

        public const string ResourcesMenuState = "resource";
        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

            //if (this.player.cityTab == CityTab.Resources)
            //{
            //    player.hud.displays.SetMenuState(ResourcesMenuState);
            //    this.player.cityTab = CityTab.Recruit;
            //}

            content.newLine();
            var tabs = new List<RichboxTabMember>((int)MenuTab.NUM);
            foreach (var tab in Tabs)//for (MenuTab tab = 0; tab < MenuTab.NUM; tab++)
            {
                tabs.Add(new RichboxTabMember(new List<AbsRichBoxMember>
                {
                    new RichBoxText(LangLib.Tab(tab))
                }));
            }

            content.Add(new RichboxTabgroup(tabs, (int)this.player.cityTab, player.cityTabClick, null, null));

            switch (player.cityTab)
            { 
                case MenuTab.Work:
                    city.workTab(content);
                    break;

                case MenuTab.Recruit:
                    recruitTab(content);
                    break;

                case MenuTab.Resources:
                    city.resourcesToMenu(content);
                    break;

                case MenuTab.Trade:
                    tradeTab(content);
                    break;
            }
            //content.h2(DssRef.lang.UnitType_Recruit);
            //foreach (var opt in city.cityPurchaseOptions)
            //switch (player.hud.displays.CurrentMenuState)
            //{

            //    default:
                    
            //        break;

            //    case ResourcesMenuState:
            //        {
            //            city.resourcesToMenu(content);
            //        }
            //        break;
            //}
        }

        void recruitTab(RichBoxContent content)
        {
            
                content.newLine();

                //var resourcesButton = new HUD.RichBox.RichboxButton(
                //new List<AbsRichBoxMember>
                //{
                //    new HUD.RichBox.RichBoxText("Resources"),
                //},
                //new RbAction1Arg<string>(player.hud.displays.SetMenuState, ResourcesMenuState, SoundLib.menu),
                //null);
                //content.Add(resourcesButton);


                ArmyStatus status;
                Army recruitArmy = city.recruitToClosestArmy();
                if (recruitArmy != null)
                {
                    status = recruitArmy.Status();
                }
                else
                {
                    status = new ArmyStatus();
                }

                content.h2(DssRef.lang.UnitType_Recruit);
                foreach (var opt in city.cityPurchaseOptions)
                {
                    if (opt.available)
                    {
                        content.newLine();

                        string recruitText = string.Format(DssRef.lang.CityOption_RecruitType, DssRef.unitsdata.Name(opt.unitType));
                        string count = status.typeCount[(int)opt.unitType].ToString();
                        AbsSoldierData typeData = DssRef.unitsdata.Get(opt.unitType);

                        content.Add(new RichBoxText(count));
                        content.Add(new RichBoxImage(typeData.icon));

                        content.Add(new RichBoxSpace());

                        content.Add(new RichboxButton(
                            new List<AbsRichBoxMember>
                            {
                            new RichBoxText(recruitText),
                            },
                            new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, 1, player, SoundLib.menuBuy),
                            new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 1),
                            canBuySoldiers(opt.unitType, 1)));

                        content.space();
                        multiBuy(5);

                        content.space();

                        multiBuy(25);

                        void multiBuy(int multiCount)
                        {
                            content.Button(string.Format(DssRef.lang.Hud_XTimes, multiCount),
                                new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, multiCount, player, SoundLib.menuBuy),
                                new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, multiCount),
                                canBuySoldiers(opt.unitType, multiCount));
                        }
                    }
                }

                if (!player.inTutorialMode)
                {
                    content.newLine();

                    content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.CityOption_XMercenaries, TextLib.LargeNumber(city.mercenaries)));

                    content.newLine();

                    //string importMecenariesText = "Import {0} mercenaries";

                    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                    new RichBoxImage(SpriteName.WarsSoldierIcon),
                    new RichBoxText( string.Format(DssRef.lang.CityOption_BuyXMercenaries, DssLib.MercenaryPurchaseCount)),
                },
                       new RbAction1Arg<int>(buyMercenaryAction, 1, SoundLib.menuBuy),
                       new RbAction1Arg<int>(buyMercenaryToolTip, 1),
                       city.buyMercenary(false, 1)));

                    content.Add(new RichBoxSpace());

                    content.Button((DssLib.MercenaryPurchaseCount * 5).ToString(),
                       new RbAction1Arg<int>(buyMercenaryAction, 5, SoundLib.menuBuy),
                       new RbAction1Arg<int>(buyMercenaryToolTip, 5),
                       city.buyMercenary(false, 5));

                    content.Add(new RichBoxNewLine(true));


                    if (city.damages.HasValue())
                    {
                        content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                                        new RichBoxImage(SpriteName.unitEmoteLove),
                                        new RichBoxText(DssRef.lang.CityOption_Repair),
                                    },
                            new RbAction1Arg<bool>(buyRepairAction, true, SoundLib.menuBuy),
                            new RbAction1Arg<bool>(buyRepairToolTip, true),
                            city.buyRepair(false, true)));
                    }
                    //else
                    //{
                    //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                    //            new RichBoxImage(SpriteName.WarsWorkerAdd),
                    //            new RichBoxText(DssRef.lang.CityOption_ExpandWorkForce),
                    //        },
                    //        new RbAction1Arg<int>(buyWorkforceAction, 1, SoundLib.menuBuy),
                    //        new RbAction1Arg<int>(buyWorkforceToolTip, 1),
                    //        city.buyWorkforce(false, 1)));
                    //}


                    content.newLine();

                    //if (city.battleGroup == null)
                    //{
                    //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                    //            new RichBoxImage(SpriteName.birdFireball),
                    //            new RichBoxText(DssRef.lang.CityOption_BurnItDown),
                    //        },
                    //        new RbAction(city.burnItDown, SoundLib.menu),
                    //        new RbAction(burnToolTip),
                    //         city.damages.value < city.MaxDamages()));

                    //    content.newLine();
                    //}

                    {
                        int count = 1;
                        content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                                        new RichBoxImage(SpriteName.WarsGuardAdd),
                                        new RichBoxText( DssRef.lang.CityOption_ExpandGuardSize),
                                    },
                            new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
                            new RbAction1Arg<int>(buyGuardSizeToolTip, count),
                            city.buyCityGuards(false, count)));
                    }
                    content.Add(new RichBoxSpace());
                    {
                        int count = 5;
                        content.Button(string.Format(DssRef.lang.Hud_XTimes, count),
                        new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
                        new RbAction1Arg<int>(buyGuardSizeToolTip, count),
                        city.buyCityGuards(false, count));
                    }

                    content.newLine();

                    if (!city.nobelHouse && city.canEverGetNobelHouse())
                    {
                        content.Button(DssRef.lang.Building_NobleHouse,
                             new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
                             new RbAction(buyNobelhouseTooltip),
                             city.canBuyNobelHouse());
                    }

                    content.newLine();
                }
            
        }

        void tradeTab(RichBoxContent content)
        {
            city.tradeTemplate.toHud(content, city.faction, city);
        }

        //void tabClick(int tab)
        //{
        //    this.player.cityTab = (MenuTab)tab;
        //}

        bool canBuySoldiers(UnitType unitType, int count)
        {
            Army army;
            return city.buySoldiers(unitType, count, false, out army);
        }
       
        void buyNobelhouseTooltip()
        {
            RichBoxContent content = new RichBoxContent(); 

            if (city.nobelHouse)
            {
                content.h2(DssRef.lang.Building_IsBuilt);
            }
            else
            {
                content.h2(DssRef.lang.Building_BuildAction);
                content.newLine();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement);
                content.newLine();
                HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Worker, DssLib.NobelHouseWorkForceReqiurement, city.workForce.Int());
                content.newLine();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                content.newLine();
                HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, DssLib.NobleHouseCost, player.faction.gold);
                HudLib.Upkeep(content, Convert.ToInt32(DssLib.NobleHouseUpkeep));
                content.newLine();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
                
            }

            content.newLine();

            //string addDiplomacy = "1 diplomacy point per {0} seconds";
            int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.NobelHouseAddDiplomacy * 3600);
            //string addDiplomacyMax = "+{0} to diplomacy point max limit";
            //string addCommand = "1 command point per {0} seconds";
            //int commandSec = Convert.ToInt32(DssLib.NobelHouseAddCommand * 3600);
            //string upkeep = "upkeep +{0}";


            content.ListDot();
            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticAddTime));
            content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
            content.newLine();

            content.ListDot();
            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticPoint));
            content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.NobelHouseAddMaxDiplomacy)));
            content.newLine();

            //content.ListDot();
            //content.Add(new RichBoxImage(SpriteName.WarsCommandAddTime));
            //content.Add(new RichBoxText(string.Format(addCommand, commandSec)));
            //content.newLine();

            content.ListDot();
            content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
            content.newLine();

            //content.ListDot();
            //content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
            //HudLib.Upkeep(content, Convert.ToInt32(DssLib.NobelHouseUpkeep), true);
            //content.Add(new RichBoxText(string.Format(upkeep, DssLib.NobelHouseUpkeep)));
            content.newLine();

            player.hud.tooltip.create(player, content, true);
        
        
        }


        void buyMercenaryAction(int count)
        {
            city.buyMercenary(true, count);
        }

        public void buyMercenaryToolTip(int count)
        {
            RichBoxContent content = new RichBoxContent();

            int cost = city.buyMercenaryCost(count);

            content.text(TextLib.Quote(DssRef.lang.CityOption_Mercenaries_Description));
            content.newLine();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
            content.newLine();
            HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, cost, player.faction.gold);
            content.text(string.Format(DssRef.lang.Hud_Purchase_CostWillIncreaseByX, DssRef.difficulty.MercenaryPurchaseCost_Add * count));
            content.newLine();
            HudLib.ResourceCost(content, GameObject.Resource.ResourceType.MercenaryOnMarket, DssLib.MercenaryPurchaseCount * count, player.mercenaryMarket.Int());

            content.newParagraph();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            content.newLine();
            content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.CityOption_XMercenaries, DssLib.MercenaryPurchaseCount * count));
            
            player.hud.tooltip.create(player, content, true);
        }


        void buyWorkforceAction(int count)
        {
            city.buyWorkforce(true, count);
        }

        void buyRepairAction(bool all)
        {
            city.buyRepair(true, all);
        }

        public void buyWorkforceToolTip(int count)
        {
            RichBoxContent content = new RichBoxContent();
            if (city.canExpandWorkForce(count))
            {
                content.text(TextLib.Quote(DssRef.lang.ResourceType_Workers_Description));
                content.newLine();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                content.newLine();
                HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, city.expandWorkForceCost()* count, player.faction.gold);
                content.newLine();
                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
                content.newLine();
                content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.CityOption_ExpandWorkForce_IncreaseMax, City.ExpandWorkForce * count));
            }
            else 
            {
                content.Add(new RichBoxText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
            }
            player.hud.tooltip.create(player, content, true);
        }

        public void burnToolTip()
        {
            RichBoxContent content = new RichBoxContent();

            content.text(DssRef.lang.CityOption_BurnItDown_Description);

            player.hud.tooltip.create(player, content, true);
        }

            public void buyRepairToolTip(bool all)
        {
            RichBoxContent content = new RichBoxContent();
            int count, cost;
            city.repairCountAndCost( all, out count, out cost);

            content.text(TextLib.Quote(DssRef.lang.CityOption_Repair_Description));
            content.newLine();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
            content.newLine();
            HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, cost, player.faction.gold);
            content.newLine();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            content.newLine();
            content.icontext(SpriteName.unitEmoteLove, string.Format(DssRef.lang.CityOption_RepairGain, city.damages.Int()));
            
            player.hud.tooltip.create(player, content, true);
        }

        void buyCityGuardsAction(int count)
        {
            city.buyCityGuards(true, count);
        }
        public void buyGuardSizeToolTip(int count)
        {
            RichBoxContent content = new RichBoxContent();

            if (city.canIncreaseGuardSize(count))
            {
                content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                content.newLine();
                HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, City.ExpandGuardSizeCost * count, player.faction.gold);
                content.newLine();
                //content.icontext(SpriteName.rtsUpkeepTime, "Upkeep +" + city.GuardUpkeep(City.ExpandGuardSize * count).ToString());
                HudLib.Upkeep(content, city.GuardUpkeep(City.ExpandGuardSize * count));

                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
                
                content.icontext(SpriteName.WarsGuardAdd, string.Format(DssRef.lang.Hud_IncreaseMaxGuardCount, City.ExpandGuardSize * count));
            }
            else 
            {
                content.Add(new RichBoxText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
                content.newLine();
                content.Add(new RichBoxText(DssRef.lang.Hud_GuardCount_MustExpandCityMessage, Color.Red));
            }

            player.hud.tooltip.create(player, content, true);
        }

        public void buySoldiersTip(CityPurchaseOption opt, int count)
        {
            var typeData = DssRef.unitsdata.Get(opt.unitType);
            var soldierData = DssRef.unitsdata.Get(UnitType.Soldier);
            int dpsSoldier = soldierData.DPS_land();
            RichBoxContent content = new RichBoxContent();
            content.text(TextLib.Quote(typeData.description));
            content.newLine();
            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
            content.newLine();
            HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, opt.goldCost * count, player.faction.gold);
            content.newLine();
            HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Worker, typeData.workForceCount() * count, city.workForce.Int());
            content.newLine();
            content.newLine();

            content.text(string.Format("Food energy upkeep {0}", typeData.energyPerSoldier));
            //HudLib.Upkeep(content, typeData.Upkeep() * count);
            //content.icontext(SpriteName.rtsUpkeep, DssRef.lang.Hud_Upkeep + ": " + (typeData.Upkeep() * count).ToString());
            content.newParagraph();

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
            int unitCount = typeData.rowWidth * typeData.columnsDepth;
            //string countText = "{0} groups, a total of {1} units";
            content.text(string.Format(DssRef.lang.SoldierStats_GroupCountAndSoldierCount, count, unitCount * count));
            content.newParagraph();

            content.h2(DssRef.lang.SoldierStats_Title);
            content.text(DssRef.lang.Hud_PurchaseTitle_Cost + ": " + TextLib.OneDecimal(opt.goldCost / (double)unitCount));
            //content.text(DssRef.lang.Hud_Upkeep + ": " + string.Format(HudLib.OneDecimalFormat, typeData.Upkeep() / (double)unitCount));
            //HudLib.Upkeep(content, typeData.Upkeep() / (double)unitCount);

            
            content.text(string.Format(DssRef.lang.SoldierStats_AttackStrengthLandSeaCity, dpsCompared(typeData.DPS_land(), dpsSoldier), dpsCompared(typeData.DPS_sea(), dpsSoldier), dpsCompared(typeData.DPS_structure(), dpsSoldier)));
            content.text(string.Format( DssRef.lang.SoldierStats_Health, typeData.basehealth));
            content.text(string.Format(DssRef.lang.SoldierStats_RecruitTrainingTimeMinutes, TextLib.OneDecimal(typeData.recruitTrainingTimeSec / 60.0)));

            speedBonus(true, typeData.ArmySpeedBonusLand);
            speedBonus(false, typeData.ArmySpeedBonusSea);


            player.hud.tooltip.create(player, content, true);

            void speedBonus(bool land, double bonus)
            {
                if (bonus != 0)
                {                    
                    string bonusText = land? DssRef.lang.SoldierStats_SpeedBonusLand : DssRef.lang.SoldierStats_SpeedBonusSea;
                    content.text(string.Format(bonusText, TextLib.PercentAddText((float)bonus)));
                }
            }
        }

        string dpsCompared(int dps, int dpsSoldier)
        {
           return TextLib.OneDecimal(dps / (double)dpsSoldier);
        }

        
    }

    enum MenuTab
    {         
        Info,
        Recruit,
        Resources,
        Work,
        Trade,
        Build,
        NUM
    }
}

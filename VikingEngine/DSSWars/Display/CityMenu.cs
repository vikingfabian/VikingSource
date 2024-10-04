﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using Valve.Steamworks;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Delivery;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class CityMenu
    {
        public static readonly MenuTab[] Tabs = { MenuTab.Info, MenuTab.Conscript, MenuTab.Resources, MenuTab.BlackMarket, MenuTab.Work, MenuTab.Delivery, MenuTab.Build };
        Players.LocalPlayer player;
        City city;

        public const string ResourcesMenuState = "resource";
        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

            content.newLine();
            
            int tabSel = 0;

            var tabs = new List<RichboxTabMember>((int)MenuTab.NUM);
            for (int i = 0; i < Tabs.Length; ++i)
            {
                var text = new RichBoxText(LangLib.Tab(Tabs[i]));
                text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                tabs.Add(new RichboxTabMember(new List<AbsRichBoxMember>
                    {
                        text
                    }));

                if (Tabs[i] == player.cityTab)
                {
                    tabSel = i;
                }
            }

            content.Add(new RichboxTabgroup(tabs, tabSel, player.cityTabClick, null, null));

            switch (player.cityTab)
            { 
                case MenuTab.Info:
                    city.CityDetailsHud(false, player, content);
                    purchaseOptions(content);
                    break;

                case MenuTab.Work:
                    city.workTab(player, content);
                    break;

                case MenuTab.Conscript:
                    conscriptTab(content);
                    break;

                //case MenuTab.Recruit:
                //    recruitTab(content);
                //    break;

                case MenuTab.BlackMarket:
                    BlackMarketResources.ToHud(player, content, city);
                    break;

                case MenuTab.Delivery:
                    deliveryTab(content);
                    break;

                case MenuTab.Resources:
                    resourcesToMenu(content);
                    break;

                case MenuTab.Trade:
                    tradeTab(content);
                    break;

                case MenuTab.Build:
                    player.BuildControls.toHud(player, content, city);
                    break;
            }
        }
        public void resourcesToMenu(RichBoxContent content)
        {
            //content.Add(new RichBoxSeperationLine());

            content.h1(DssRef.todoLang.MenuTab_Resources);
            content.newLine();

            // content.text("Water: " + water.ToString());
            //city.water.toMenu(content, DssRef.todoLang.Resource_TypeName_Water);
            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
            content.Add(new RichBoxText( DssRef.todoLang.Resource_TypeName_Water + ": " + string.Format(DssRef.todoLang.Language_CollectProgress, city.res_water.amount, city.maxWater)));
            content.Add(new RichBoxTab(0.4f));
            content.Add(new RichBoxImage(SpriteName.WarsResource_WaterAdd));
            content.Add(new RichBoxText(TextLib.OneDecimal(city.waterAddPerSec)));

            city.res_beer.toMenu(content, ItemResourceType.Beer);
            city.res_wood.toMenu(content,  ItemResourceType.Wood_Group);
            city.res_fuel.toMenu(content, ItemResourceType.Fuel_G);
            city.res_stone.toMenu(content,  ItemResourceType.Stone_G);
            city.res_rawFood.toMenu(content, ItemResourceType.RawFood_Group);
            city.res_skinLinnen.toMenu(content,  ItemResourceType.SkinLinen_Group);
            city.res_ore.toMenu(content, ItemResourceType.IronOre_G);

            content.newParagraph();

            city.res_food.toMenu(content,  ItemResourceType.Food_G);
            blueprintButton(player, content, ResourceLib.CraftFood1, ResourceLib.CraftFood2);

            city.res_iron.toMenu(content, ItemResourceType.Iron_G);
            blueprintButton(player, content, ResourceLib.CraftIron);

            city.res_sharpstick.toMenu(content, ItemResourceType.SharpStick);
            blueprintButton(player, content, ResourceLib.CraftSharpStick);

            city.res_sword.toMenu(content, ItemResourceType.Sword);
            blueprintButton(player, content, ResourceLib.CraftSword);

            city.res_twohandsword.toMenu(content, ItemResourceType.TwoHandSword);
            blueprintButton(player, content, ResourceLib.CraftTwoHandSword);

            city.res_knightslance.toMenu(content, ItemResourceType.KnightsLance);
            blueprintButton(player, content, ResourceLib.CraftKnightsLance);

            city.res_bow.toMenu(content, ItemResourceType.Bow);
            blueprintButton(player, content, ResourceLib.CraftBow);

            city.res_ballista.toMenu(content, ItemResourceType.Ballista);
            blueprintButton(player, content, ResourceLib.CraftBallista);

            city.res_lightArmor.toMenu(content, ItemResourceType.LightArmor);
            blueprintButton(player, content, ResourceLib.CraftLightArmor);

            city.res_mediumArmor.toMenu(content, ItemResourceType.MediumArmor);
            blueprintButton(player, content, ResourceLib.CraftMediumArmor);

            city.res_heavyArmor.toMenu(content, ItemResourceType.HeavyArmor);
            blueprintButton(player, content, ResourceLib.CraftHeavyArmor);

            content.newParagraph();
            
            content.text("1 gold ore => " + DssConst.GoldOreSellValue.ToString() + "gold");
            content.text("1 food => " + DssConst.FoodEnergy + " energy (seconds of work)");
        }

        void purchaseOptions(RichBoxContent content)
        {
            if (city.battleGroup == null)
            {
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

                //if (!city.nobelHouse && city.canEverGetNobelHouse())
                //{
                //    content.Button(DssRef.lang.Building_NobleHouse,
                //            new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
                //            new RbAction(buyNobelhouseTooltip),
                //            city.canBuyNobelHouse());
                //}
            }
        }

        void blueprintButton(LocalPlayer player, RichBoxContent content, CraftBlueprint blueprint, CraftBlueprint optionalBp = null)
        {
            content.Add(new RichBoxTab(0.8f));

            //RichBoxText B = new RichBoxText(DssRef.todoLang.Blueprint_LetterSymbol);
            //B.overrideColor = Color.Blue;

            content.Add(new RichboxButton(new List<AbsRichBoxMember> { 
                new RichBoxImage(SpriteName.WarsBluePrint)
            },
               null, new RbAction2Arg<CraftBlueprint, CraftBlueprint>(blueprintTooltip, blueprint, optionalBp)));

        }

        void blueprintTooltip(CraftBlueprint blueprint, CraftBlueprint optionalBp)
        {
            //hover
            RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.todoLang.Blueprint_Title).overrideColor = HudLib.TitleColor_TypeName;
            blueprint.toMenu(content, city);
            if (optionalBp != null)
            { 
                content.newLine();
                optionalBp.toMenu(content, city);
            }

            if (blueprint.requirement != CraftRequirement.None)
            {
                content.newLine();
                HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                content.newLine();
                content.BulletPoint();
                RichBoxText requirement1 = new RichBoxText(DssRef.todoLang.BuildingType_Carpenter);
                requirement1.overrideColor = city.hasBuilding_carpenter? HudLib.AvailableColor : HudLib.NotAvailableColor;
                content.Add(requirement1);
            }

            player.hud.tooltip.create(player, content, true);
        }

        void conscriptTab(RichBoxContent content)
        {
            new ConscriptMenu().ToHud(city, player, content);
        }

        void deliveryTab(RichBoxContent content)
        {
            new DeliveryMenu().ToHud(city, player, content);
        }

        //void recruitTab(RichBoxContent content)
        //{            
        //    content.newLine();

        //    ArmyStatus status;
        //    Army recruitArmy = city.recruitToClosestArmy();
        //    if (recruitArmy != null)
        //    {
        //        status = recruitArmy.Status();
        //    }
        //    else
        //    {
        //        status = new ArmyStatus();
        //    }

        //    content.h2(DssRef.lang.UnitType_Recruit);
        //    foreach (var opt in city.cityPurchaseOptions)
        //    {
        //        if (opt.available)
        //        {
        //            content.newLine();

        //            string recruitText = string.Format(DssRef.lang.CityOption_RecruitType, DssRef.profile.Name(opt.unitType));
        //            string count = status.typeCount[(int)opt.unitType].ToString();
        //            AbsSoldierProfile typeData = DssRef.profile.Get(opt.unitType);

        //            content.Add(new RichBoxText(count));
        //            content.Add(new RichBoxImage(typeData.icon));

        //            content.Add(new RichBoxSpace());

        //            content.Add(new RichboxButton(
        //                new List<AbsRichBoxMember>
        //                {
        //                new RichBoxText(recruitText),
        //                },
        //                new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, 1, player, SoundLib.menuBuy),
        //                new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 1),
        //                canBuySoldiers(opt.unitType, 1)));

        //            content.space();
        //            multiBuy(5);

        //            content.space();

        //            multiBuy(25);

        //            void multiBuy(int multiCount)
        //            {
        //                content.Button(string.Format(DssRef.lang.Hud_XTimes, multiCount),
        //                    new RbAction3Arg<UnitType, int, LocalPlayer>(city.buySoldiersAction, opt.unitType, multiCount, player, SoundLib.menuBuy),
        //                    new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, multiCount),
        //                    canBuySoldiers(opt.unitType, multiCount));
        //            }
        //        }
        //    }

        //    if (!player.inTutorialMode)
        //    {
        //        content.newLine();

        //        content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.CityOption_XMercenaries, TextLib.LargeNumber(city.mercenaries)));

        //        content.newLine();

        //        //string importMecenariesText = "Import {0} mercenaries";

        //        content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        new RichBoxImage(SpriteName.WarsSoldierIcon),
        //        new RichBoxText( string.Format(DssRef.lang.CityOption_BuyXMercenaries, DssLib.MercenaryPurchaseCount)),
        //    },
        //            new RbAction1Arg<int>(buyMercenaryAction, 1, SoundLib.menuBuy),
        //            new RbAction1Arg<int>(buyMercenaryToolTip, 1),
        //            city.buyMercenary(false, 1)));

        //        content.Add(new RichBoxSpace());

        //        content.Button((DssLib.MercenaryPurchaseCount * 5).ToString(),
        //            new RbAction1Arg<int>(buyMercenaryAction, 5, SoundLib.menuBuy),
        //            new RbAction1Arg<int>(buyMercenaryToolTip, 5),
        //            city.buyMercenary(false, 5));

        //        content.Add(new RichBoxNewLine(true));


        //        if (city.damages.HasValue())
        //        {
        //            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //                            new RichBoxImage(SpriteName.unitEmoteLove),
        //                            new RichBoxText(DssRef.lang.CityOption_Repair),
        //                        },
        //                new RbAction1Arg<bool>(buyRepairAction, true, SoundLib.menuBuy),
        //                new RbAction1Arg<bool>(buyRepairToolTip, true),
        //                city.buyRepair(false, true)));
        //        }
        //        //else
        //        //{
        //        //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        //            new RichBoxImage(SpriteName.WarsWorkerAdd),
        //        //            new RichBoxText(DssRef.lang.CityOption_ExpandWorkForce),
        //        //        },
        //        //        new RbAction1Arg<int>(buyWorkforceAction, 1, SoundLib.menuBuy),
        //        //        new RbAction1Arg<int>(buyWorkforceToolTip, 1),
        //        //        city.buyWorkforce(false, 1)));
        //        //}


        //        content.newLine();

        //        //if (city.battleGroup == null)
        //        //{
        //        //    content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //        //            new RichBoxImage(SpriteName.birdFireball),
        //        //            new RichBoxText(DssRef.lang.CityOption_BurnItDown),
        //        //        },
        //        //        new RbAction(city.burnItDown, SoundLib.menu),
        //        //        new RbAction(burnToolTip),
        //        //         city.damages.value < city.MaxDamages()));

        //        //    content.newLine();
        //        //}

        //        {
        //            int count = 1;
        //            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
        //                            new RichBoxImage(SpriteName.WarsGuardAdd),
        //                            new RichBoxText( DssRef.lang.CityOption_ExpandGuardSize),
        //                        },
        //                new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
        //                new RbAction1Arg<int>(buyGuardSizeToolTip, count),
        //                city.buyCityGuards(false, count)));
        //        }
        //        content.Add(new RichBoxSpace());
        //        {
        //            int count = 5;
        //            content.Button(string.Format(DssRef.lang.Hud_XTimes, count),
        //            new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
        //            new RbAction1Arg<int>(buyGuardSizeToolTip, count),
        //            city.buyCityGuards(false, count));
        //        }

        //        content.newLine();

        //        if (!city.nobelHouse && city.canEverGetNobelHouse())
        //        {
        //            content.Button(DssRef.lang.Building_NobleHouse,
        //                    new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
        //                    new RbAction(buyNobelhouseTooltip),
        //                    city.canBuyNobelHouse());
        //        }

        //        content.newLine();
        //    }
            
        //}

        void tradeTab(RichBoxContent content)
        {
            city.tradeTemplate.toHud(player,content, city.faction, city);
        }

        //void tabClick(int tab)
        //{
        //    this.player.cityTab = (MenuTab)tab;
        //}

        //bool canBuySoldiers(UnitType unitType, int count)
        //{
        //    Army army;
        //    return city.buySoldiers(unitType, count, false, out army);
        //}
       
        //void buyNobelhouseTooltip()
        //{
        //    RichBoxContent content = new RichBoxContent(); 

        //    if (city.nobelHouse)
        //    {
        //        content.h2(DssRef.lang.Building_IsBuilt);
        //    }
        //    else
        //    {
        //        content.h2(DssRef.lang.Building_BuildAction);
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Requirement);
        //        content.newLine();
        //        HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Worker, DssLib.NobelHouseWorkForceReqiurement, city.workForce);
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //        content.newLine();
        //        HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, DssLib.NobleHouseCost, player.faction.gold);
        //        HudLib.Upkeep(content, Convert.ToInt32(DssLib.NobleHouseUpkeep));
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
                
        //    }

        //    content.newLine();

        //    //string addDiplomacy = "1 diplomacy point per {0} seconds";
        //    int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.NobelHouseAddDiplomacy * 3600);
        //    //string addDiplomacyMax = "+{0} to diplomacy point max limit";
        //    //string addCommand = "1 command point per {0} seconds";
        //    //int commandSec = Convert.ToInt32(DssLib.NobelHouseAddCommand * 3600);
        //    //string upkeep = "upkeep +{0}";


        //    content.BulletPoint();
        //    content.Add(new RichBoxImage(SpriteName.WarsDiplomaticAddTime));
        //    content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
        //    content.newLine();

        //    content.BulletPoint();
        //    content.Add(new RichBoxImage(SpriteName.WarsDiplomaticPoint));
        //    content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.NobelHouseAddMaxDiplomacy)));
        //    content.newLine();

        //    //content.ListDot();
        //    //content.Add(new RichBoxImage(SpriteName.WarsCommandAddTime));
        //    //content.Add(new RichBoxText(string.Format(addCommand, commandSec)));
        //    //content.newLine();

        //    content.BulletPoint();
        //    content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
        //    content.newLine();

        //    //content.ListDot();
        //    //content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
        //    //HudLib.Upkeep(content, Convert.ToInt32(DssLib.NobelHouseUpkeep), true);
        //    //content.Add(new RichBoxText(string.Format(upkeep, DssLib.NobelHouseUpkeep)));
        //    content.newLine();

        //    player.hud.tooltip.create(player, content, true);
        
        
        //}


        //void buyMercenaryAction(int count)
        //{
        //    city.buyMercenary(true, count);
        //}

        //public void buyMercenaryToolTip(int count)
        //{
        //    RichBoxContent content = new RichBoxContent();

        //    int cost = city.buyMercenaryCost(count);

        //    content.text(TextLib.Quote(DssRef.lang.CityOption_Mercenaries_Description));
        //    content.newLine();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, cost, player.faction.gold);
        //    content.text(string.Format(DssRef.lang.Hud_Purchase_CostWillIncreaseByX, DssRef.difficulty.MercenaryPurchaseCost_Add * count));
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.MercenaryOnMarket, DssLib.MercenaryPurchaseCount * count, player.mercenaryMarket.Int());

        //    content.newParagraph();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //    content.newLine();
        //    content.icontext(SpriteName.WarsSoldierIcon, string.Format(DssRef.lang.CityOption_XMercenaries, DssLib.MercenaryPurchaseCount * count));
            
        //    player.hud.tooltip.create(player, content, true);
        //}


        //void buyWorkforceAction(int count)
        //{
        //    city.buyWorkforce(true, count);
        //}

        void buyRepairAction(bool all)
        {
            city.buyRepair(true, all);
        }

        //public void buyWorkforceToolTip(int count)
        //{
        //    RichBoxContent content = new RichBoxContent();
        //    if (city.canExpandWorkForce(count))
        //    {
        //        content.text(TextLib.Quote(DssRef.lang.ResourceType_Workers_Description));
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //        content.newLine();
        //        HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, city.expandWorkForceCost()* count, player.faction.gold);
        //        content.newLine();
        //        content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //        content.newLine();
        //        content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.CityOption_ExpandWorkForce_IncreaseMax, DssConst.ExpandWorkForce * count));
        //    }
        //    else 
        //    {
        //        content.Add(new RichBoxText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
        //    }
        //    player.hud.tooltip.create(player, content, true);
        //}

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
                HudLib.Upkeep(content, city.GuardUpkeep(DssConst.ExpandGuardSize * count));

                content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
                
                content.icontext(SpriteName.WarsGuardAdd, string.Format(DssRef.lang.Hud_IncreaseMaxGuardCount, DssConst.ExpandGuardSize * count));
            }
            else 
            {
                content.Add(new RichBoxText(DssRef.lang.Hud_Purchase_MaxCapacity, Color.Red));
                content.newLine();
                content.Add(new RichBoxText(DssRef.lang.Hud_GuardCount_MustExpandCityMessage, Color.Red));
            }

            player.hud.tooltip.create(player, content, true);
        }

        //public void buySoldiersTip(CityPurchaseOption opt, int count)
        //{
        //    var typeData = DssRef.profile.Get(opt.unitType);
        //    var soldierData = DssRef.profile.Get(UnitType.Soldier);
        //    int dpsSoldier = soldierData.DPS_land();
        //    RichBoxContent content = new RichBoxContent();
        //    HudLib.Description(content, typeData.description);//content.text(TextLib.Quote(typeData.description));
        //    content.newLine();
        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Gold, opt.goldCost * count, player.faction.gold);
        //    content.newLine();
        //    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.Worker, typeData.workForceCount() * count, city.workForce);
        //    content.newLine();
        //    content.newLine();

        //    content.text(string.Format(DssRef.todoLang.Hud_EnergyUpkeepX, typeData.energyPerSoldier));
        //    //HudLib.Upkeep(content, typeData.Upkeep() * count);
        //    //content.icontext(SpriteName.rtsUpkeep, DssRef.lang.Hud_Upkeep + ": " + (typeData.Upkeep() * count).ToString());
        //    content.newParagraph();

        //    content.h2(DssRef.lang.Hud_PurchaseTitle_Gain);
        //    int unitCount = typeData.rowWidth * typeData.columnsDepth;
        //    //string countText = "{0} groups, a total of {1} units";
        //    content.text(string.Format(DssRef.lang.SoldierStats_GroupCountAndSoldierCount, count, unitCount * count));
        //    content.newParagraph();

        //    content.h2(DssRef.lang.SoldierStats_Title);
        //    content.text(DssRef.lang.Hud_PurchaseTitle_Cost + ": " + TextLib.OneDecimal(opt.goldCost / (double)unitCount));
        //    //content.text(DssRef.lang.Hud_Upkeep + ": " + string.Format(HudLib.OneDecimalFormat, typeData.Upkeep() / (double)unitCount));
        //    //HudLib.Upkeep(content, typeData.Upkeep() / (double)unitCount);

            
        //    content.text(string.Format(DssRef.lang.SoldierStats_AttackStrengthLandSeaCity, dpsCompared(typeData.DPS_land(), dpsSoldier), dpsCompared(typeData.DPS_sea(), dpsSoldier), dpsCompared(typeData.DPS_structure(), dpsSoldier)));
        //    content.text(string.Format( DssRef.lang.SoldierStats_Health, typeData.basehealth));
        //    content.text(string.Format(DssRef.lang.SoldierStats_RecruitTrainingTimeMinutes, TextLib.OneDecimal(typeData.recruitTrainingTimeSec / 60.0)));

        //    speedBonus(true, typeData.ArmySpeedBonusLand);
        //    speedBonus(false, typeData.ArmySpeedBonusSea);


        //    player.hud.tooltip.create(player, content, true);

        //    void speedBonus(bool land, double bonus)
        //    {
        //        if (bonus != 0)
        //        {                    
        //            string bonusText = land? DssRef.lang.SoldierStats_SpeedBonusLand : DssRef.lang.SoldierStats_SpeedBonusSea;
        //            content.text(string.Format(bonusText, TextLib.PercentAddText((float)bonus)));
        //        }
        //    }
        //}

        string dpsCompared(int dps, int dpsSoldier)
        {
           return TextLib.OneDecimal(dps / (double)dpsSoldier);
        }

        
    }

    enum MenuTab
    {         
        Info,
        Conscript,
        Recruit,
        Economy,
        Resources,
        Work,
        Trade,
        BlackMarket,
        Delivery,
        Build,
        Automation,
        NUM
    }
}

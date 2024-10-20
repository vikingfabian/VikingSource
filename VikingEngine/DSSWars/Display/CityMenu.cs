using Microsoft.Xna.Framework;
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
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class CityMenu
    {
        public static readonly List<MenuTab> Tabs = new List<MenuTab>() { 
            MenuTab.Info, MenuTab.Resources, MenuTab.BlackMarket, MenuTab.Work, MenuTab.Build, MenuTab.Delivery, MenuTab.Conscript };
        Players.LocalPlayer player;
        City city;
        static readonly int[] StockPileControls = { 100, 1000 };

        //public const string ResourcesMenuState = "resource";
        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

            content.newLine();
            
            int tabSel = 0;

            var tabs = new List<RichboxTabMember>((int)MenuTab.NUM);

            List<MenuTab> availableTabs =player.AvailableCityTabs();//player.tutorial != null ? player.tutorial.cityTabs : Tabs;
            for (int i = 0; i < availableTabs.Count; ++i)
            {
                var text = new RichBoxText(LangLib.Tab(availableTabs[i], out string description));
                text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                AbsRbAction enter = null;
                if (description != null)
                {
                    enter = new RbAction(() =>
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.text(description).overrideColor = HudLib.InfoYellow_Light;

                        player.hud.tooltip.create(player, content, true);
                    });
                }

                tabs.Add(new RichboxTabMember(new List<AbsRichBoxMember>
                    {
                        text
                    }, enter));

                if (availableTabs[i] == player.cityTab)
                {
                    tabSel = i;
                }
            }

            content.Add(new RichboxTabgroup(tabs, tabSel, player.cityTabClick, null, null));

            content.newLine();

            switch (player.cityTab)
            { 
                case MenuTab.Info:
                    city.CityDetailsHud(false, player, content);
                    purchaseOptions(content);
                    break;

                case MenuTab.Work:
                    city.workTemplate.toHud(player, content, city.faction, city);
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
            if (player.tutorial == null || player.tutorial.DisplayStockpile())
            {
                for (ResourcesSubTab resourcesSubTab = 0; resourcesSubTab < ResourcesSubTab.NUM; ++resourcesSubTab)
                {
                    string text = null;
                    switch (resourcesSubTab)
                    {
                        case ResourcesSubTab.Overview:
                            text = DssRef.lang.Resource_Tab_Overview;
                            break;
                        case ResourcesSubTab.Stockpile:
                            text = DssRef.lang.Resource_Tab_Stockpile;
                            break;
                    }
                    var subTab = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(text) },
                        new RbAction1Arg<ResourcesSubTab>((ResourcesSubTab resourcesSubTab) =>
                        {
                            player.resourcesSubTab = resourcesSubTab;
                        }, resourcesSubTab, SoundLib.menu));
                    subTab.setGroupSelectionColor(HudLib.RbSettings, player.resourcesSubTab == resourcesSubTab);
                    content.Add(subTab);
                    content.space();
                }
                content.newParagraph();
            }

            switch (player.resourcesSubTab)
            {
                case ResourcesSubTab.Overview:
                    content.h1(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();

                    content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                    content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water + ": " + string.Format(DssRef.lang.Language_CollectProgress, city.res_water.amount, city.maxWater)));
                    content.Add(new RichBoxTab(0.4f));
                    content.Add(new RichBoxImage(SpriteName.WarsResource_WaterAdd));
                    content.Add(new RichBoxText(TextLib.OneDecimal(city.waterAddPerSec)));

                    bool reachedBuffer = false;

                    city.res_wood.toMenu(content, ItemResourceType.Wood_Group, ref reachedBuffer);
                    city.res_stone.toMenu(content, ItemResourceType.Stone_G, ref reachedBuffer);
                    city.res_rawFood.toMenu(content, ItemResourceType.RawFood_Group, ref reachedBuffer);
                    city.res_skinLinnen.toMenu(content, ItemResourceType.SkinLinen_Group, ref reachedBuffer);
                    city.res_ironore.toMenu(content, ItemResourceType.IronOre_G, ref reachedBuffer);

                    content.Add(new RichBoxSeperationLine());

                    city.res_food.toMenu(content, ItemResourceType.Food_G, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftFood1, ResourceLib.CraftFood2);

                    city.res_fuel.toMenu(content, ItemResourceType.Fuel_G, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftFuel1, null, true);
                    content.space();
                    blueprintButton(player, content, ResourceLib.CraftCharcoal);

                    city.res_beer.toMenu(content, ItemResourceType.Beer, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftBeer);

                    city.res_iron.toMenu(content, ItemResourceType.Iron_G, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftIron);

                    content.Add(new RichBoxSeperationLine());

                    city.res_sharpstick.toMenu(content, ItemResourceType.SharpStick, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftSharpStick);

                    city.res_sword.toMenu(content, ItemResourceType.Sword, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftSword);

                    city.res_twohandsword.toMenu(content, ItemResourceType.TwoHandSword, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftTwoHandSword);

                    city.res_knightslance.toMenu(content, ItemResourceType.KnightsLance, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftKnightsLance);

                    city.res_bow.toMenu(content, ItemResourceType.Bow, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftBow);

                    city.res_longbow.toMenu(content, ItemResourceType.LongBow, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftLongBow);

                    city.res_ballista.toMenu(content, ItemResourceType.Ballista, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftBallista);

                    content.Add(new RichBoxSeperationLine());

                    city.res_lightArmor.toMenu(content, ItemResourceType.LightArmor, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftLightArmor);

                    city.res_mediumArmor.toMenu(content, ItemResourceType.MediumArmor, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftMediumArmor);

                    city.res_heavyArmor.toMenu(content, ItemResourceType.HeavyArmor, ref reachedBuffer);
                    blueprintButton(player, content, ResourceLib.CraftHeavyArmor);

                    //if (reachedBuffer)
                    //{
                    //    GroupedResource.BufferIconInfo(content);
                    //}
                    content.Add(new RichBoxSeperationLine());
                    GroupedResource.BufferIconInfo(content);
                    ResourceLib.ConvertGoldOre.toMenu(content, city);
                    //content.text("1 gold ore => " + DssConst.GoldOreSellValue.ToString() + "gold");
                    {
                        //content.text("1 food => " + DssConst.FoodEnergy + " energy (seconds of work)");
                        content.Add(new RichBoxText(1.ToString()));
                        content.Add(new RichBoxImage(ResourceLib.Icon(ItemResourceType.Food_G)));
                        content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Food));
                        var arrow = new RichBoxImage(SpriteName.pjNumArrowR);
                        arrow.color = Color.CornflowerBlue;
                        content.Add(arrow);
                        content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_EnergyAmount, DssConst.FoodEnergy)));
                    }
                    break;

                case ResourcesSubTab.Stockpile:
                    content.h1(DssRef.lang.Resource_Tab_Stockpile).overrideColor = HudLib.TitleColor_Label;

                    stockpile(ItemResourceType.Wood_Group);
                    stockpile(ItemResourceType.Stone_G);
                    stockpile(ItemResourceType.RawFood_Group);
                    stockpile(ItemResourceType.SkinLinen_Group);
                    stockpile(ItemResourceType.IronOre_G);
                    content.Add(new RichBoxSeperationLine());
                    stockpile(ItemResourceType.Food_G);
                    stockpile(ItemResourceType.Fuel_G);
                    stockpile(ItemResourceType.Beer);
                    stockpile(ItemResourceType.Iron_G);
                    content.Add(new RichBoxSeperationLine());
                    stockpile(ItemResourceType.Sword);
                    stockpile(ItemResourceType.TwoHandSword);
                    stockpile(ItemResourceType.KnightsLance);
                    stockpile(ItemResourceType.Bow);
                    stockpile(ItemResourceType.LongBow);
                    stockpile(ItemResourceType.Ballista);
                    content.Add(new RichBoxSeperationLine());
                    stockpile(ItemResourceType.LightArmor);
                    stockpile(ItemResourceType.MediumArmor);
                    stockpile(ItemResourceType.HeavyArmor);

                    HudLib.Description(content, DssRef.lang.Resource_StockPile_Info);
                    GroupedResource.BufferIconInfo(content);
                    break;
            }

            void stockpile(ItemResourceType item)
            {
                const int MinBound = 0;
                const int MaxBound = 20000;


                var res = city.GetGroupedResource(item);

                content.newLine();
                var icon = new RichBoxImage(res.amount >= res.goalBuffer ? SpriteName.WarsStockpileStop : SpriteName.WarsStockpileAdd);
                //if (res.amount >= res.goalBuffer)
                //{
                //    icon.color = Color.OrangeRed;
                //}
                content.Add(icon);
                content.Add(new RichBoxImage(ResourceLib.Icon(item)));
                content.space();
                //content.Add(new RichBoxText(LangLib.Item(item) + ": "));
                RbAction hover = new RbAction(() => {
                    RichBoxContent content = new RichBoxContent();
                    content.Add(new RichBoxText(LangLib.Item(item)));
                    player.hud.tooltip.create(player, content, true);
                });
                //content.newLine();
                for (int i = StockPileControls.Length - 1; i >= 0; i--)
                {
                    int change = -StockPileControls[i];
                    content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                        new RbAction1Arg<int>((int change) => {
                            var res = city.GetGroupedResource(item);
                            res.goalBuffer = Bound.Set(res.goalBuffer + change, MinBound, MaxBound);
                            city.SetGroupedResource(item, res);

                        }, change, SoundLib.menu), hover));

                    content.space();
                }

                content.Add(new RichBoxText(res.goalBuffer.ToString()));

                for (int i = 0; i < StockPileControls.Length; i++)
                {
                    content.space();

                    int change = StockPileControls[i];
                    content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                        new RbAction1Arg<int>((int change) => {
                            var res = city.GetGroupedResource(item);
                            res.goalBuffer = Bound.Set(res.goalBuffer + change, MinBound, MaxBound);
                            city.SetGroupedResource(item, res);

                        }, change, SoundLib.menu), hover));
                }
            }
        }

        void purchaseOptions(RichBoxContent content)
        {
            if (city.battleGroup == null)
            {
                if (city.damages.HasValue())
                {
                    content.newLine();
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

        void blueprintButton(LocalPlayer player, RichBoxContent content, CraftBlueprint blueprint, CraftBlueprint optionalBp = null, bool roomForAnotherButton = false)
        {
            
            content.Add(new RichBoxTab(0.65f));//roomForAnotherButton? 0.65f : 0.8f));

            content.Add(new RichboxButton(new List<AbsRichBoxMember> { 
                new RichBoxImage(SpriteName.WarsBluePrint)
            },
               null, new RbAction2Arg<CraftBlueprint, CraftBlueprint>(blueprintTooltip, blueprint, optionalBp)));

        }

        void blueprintTooltip(CraftBlueprint blueprint, CraftBlueprint optionalBp)
        {
            //hover
            RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.lang.Blueprint_Title).overrideColor = HudLib.TitleColor_TypeName;
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

                string reqText;
                bool available;
                switch (blueprint.requirement)
                {
                    case CraftRequirement.Carpenter:
                        reqText = DssRef.lang.BuildingType_Carpenter;
                        available = city.hasBuilding_carpenter;
                        break;
                    case CraftRequirement.Brewery:
                        reqText = DssRef.lang.BuildingType_Brewery;
                        available = city.hasBuilding_brewery;
                        break;
                    case CraftRequirement.Smith:
                        reqText = DssRef.lang.BuildingType_Smith;
                        available = city.hasBuilding_smith;
                        break;
                    case CraftRequirement.CoalPit:
                        reqText = DssRef.lang.BuildingType_CoalPit;
                        available = city.coalpit_buildingCount > 0;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                RichBoxText requirement1 = new RichBoxText(reqText);
                requirement1.overrideColor = available ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                content.Add(requirement1);
            }

            content.Add(new RichBoxSeperationLine());
            content.newParagraph();
            content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
            blueprint.listResources(content, city, optionalBp);

            player.hud.tooltip.create(player, content, true, blueprint.tooltipId);
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

        //    content.text(string.Format(DssRef.lang.Hud_EnergyUpkeepX, typeData.energyPerSoldier));
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

    enum ResourcesSubTab
    { 
        Overview,
        Stockpile,
        NUM
    }
}

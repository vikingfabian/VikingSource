using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Display
{
    class CityMenu
    {
        Players.LocalPlayer player;
        City city;

        public CityMenu(Players.LocalPlayer player, City city, RichBoxContent content)
        {
            this.player = player;
            this.city = city;

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

            content.h2("Recruit soldier groups");
            foreach (var opt in city.cityPurchaseOptions)
            {
                if (opt.available)
                {
                    content.newLine();

                    string recruitText = "Recruit " + opt.unitType.ToString();
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
                        new RbAction2Arg<UnitType, int>(city.buySoldiersAction, opt.unitType, 1, SoundLib.menuBuy),
                        new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 1),
                        canBuySoldiers(opt.unitType, 1)));

                    //content.Button(recruitText,
                    //    new RbAction2Arg<UnitType, int>(city.buySoldiersAction, opt.unitType, 1, SoundLib.menuBuy),
                    //    new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 1),
                    //    canBuySoldiers(opt.unitType, 1));

                    content.Add(new RichBoxSpace());

                    content.Button("x5",
                        new RbAction2Arg<UnitType, int>(city.buySoldiersAction, opt.unitType, 5, SoundLib.menuBuy),
                        new RbAction2Arg<CityPurchaseOption, int>(buySoldiersTip, opt, 5),
                        canBuySoldiers(opt.unitType, 5));

                }
            }

            content.newLine();

            content.icontext(SpriteName.WarsSoldierIcon, "Mercenaries: " + TextLib.LargeNumber(city.mercenaries));

            content.newLine();

            string importMecenariesText = "Import {0} mercenaries";

            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                    new RichBoxImage(SpriteName.WarsSoldierIcon),
                    new RichBoxText( string.Format(importMecenariesText, DssLib.MercenaryPurchaseCount)),
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

            content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                    new RichBoxImage(SpriteName.WarsWorkerAdd),
                    new RichBoxText( "Expand city"),
                },
                new RbAction1Arg<int>(buyWorkforceAction, 1, SoundLib.menuBuy),
                new RbAction1Arg<int>(buyWorkforceToolTip, 1),
                city.buyWorkforce(false, 1)));
            
            content.newLine();

            {
                int count = 1;
                content.Add(new RichboxButton(new List<AbsRichBoxMember>{
                        new RichBoxImage(SpriteName.WarsGuardAdd),
                        new RichBoxText( "Expand guard"),
                    },
                    new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
                    new RbAction1Arg<int>(buyGuardSizeToolTip, count),
                    city.buyCityGuards(false, count)));
            }
            content.Add(new RichBoxSpace());
            {
                int count = 5;
                content.Button("x5",
                new RbAction1Arg<int>(buyCityGuardsAction, count, SoundLib.menuBuy),
                new RbAction1Arg<int>(buyGuardSizeToolTip, count),
                city.buyCityGuards(false, count));
            }

            content.newLine();

            if (!city.nobelHouse && city.canEverGetNobelHouse())
            {
                content.Button("Nobel house",
                     new RbAction(city.buyNobelHouseAction, SoundLib.menuBuy),
                     new RbAction(buyNobelhouseTooltip),
                     city.canBuyNobelHouse());
            }

            content.newLine();
            
        }

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
                content.h2("Built");
            }
            else
            {
                content.h2("Build");
                content.newLine();
                content.h2("Requirement");
                content.newLine();
                HudLib.ResourceCost(content, SpriteName.WarsWorker,"Workers", DssLib.NobelHouseWorkForceReqiurement, city.workForce.Int());
                content.newLine();
                content.h2("Cost");
                content.newLine();
                HudLib.ResourceCost(content, SpriteName.rtsUpkeep, "Gold", DssLib.NobelHouseCost, player.faction.gold);
                content.newLine();
                content.h2("Gain");
            }

            content.newLine();

            string addDiplomacy = "1 diplomacy point per {0} seconds";
            int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.NobelHouseAddDiplomacy * 3600);
            string addDiplomacyMax = "+{0} to diplomacy point max limit";
            string addCommand = "1 command point per {0} seconds";
            int commandSec = Convert.ToInt32(DssLib.NobelHouseAddCommand * 3600);
            string upkeep = "upkeep +{0}";


            content.ListDot();
            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticAddTime));
            content.Add(new RichBoxText(string.Format(addDiplomacy, diplomacydSec)));
            content.newLine();

            content.ListDot();
            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticPoint));
            content.Add(new RichBoxText(string.Format(addDiplomacyMax, DssRef.diplomacy.NobelHouseAddMaxDiplomacy)));
            content.newLine();

            content.ListDot();
            content.Add(new RichBoxImage(SpriteName.WarsCommandAddTime));
            content.Add(new RichBoxText(string.Format(addCommand, commandSec)));
            content.newLine();

            content.ListDot();
            content.Add(new RichBoxText("Unlocks Knight unit"));
            content.newLine();

            content.ListDot();
            content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
            content.Add(new RichBoxText(string.Format(upkeep, DssLib.NobelHouseUpkeep)));
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

            content.text(TextLib.Quote("Soldiers will be drafted from mercenaries instead of your workforce"));
            content.newLine();
            content.h2("Cost");
            content.newLine();
            HudLib.ResourceCost(content, SpriteName.rtsUpkeep, "Gold", cost, player.faction.gold);
            content.text(string.Format("Cost will increase by {0}", DssRef.difficulty.MercenaryPurchaseCost_Add * count));

            content.newParagraph();
            content.h2("Gain");
            content.newLine();
            content.icontext(SpriteName.WarsSoldierIcon, string.Format("{0} mercenaries", DssLib.MercenaryPurchaseCount * count));
            
            player.hud.tooltip.create(player, content, true);
        }


        void buyWorkforceAction(int count)
        {
            city.buyWorkforce(true, count);
        }

        public void buyWorkforceToolTip(int count)
        {
            RichBoxContent content = new RichBoxContent();
            if (city.canExpandWorkForce(count))
            {
                content.text(TextLib.Quote("Workers provide income. And are drafted as soldiers for your armies"));
                content.newLine();
                content.h2("Cost");
                content.newLine();
                HudLib.ResourceCost(content, SpriteName.rtsUpkeep, "Gold", city.expandWorkForceCost()* count, player.faction.gold);
                content.newLine();
                content.h2("Gain");
                content.newLine();
                content.icontext(SpriteName.WarsWorkerAdd, "Max work force +" + (City.ExpandWorkForce*count).ToString());
            }
            else 
            {
                content.Add(new RichBoxText("Has reached maximum capacity", Color.Red));
            }
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
                content.h2("Cost");
                content.newLine();
                HudLib.ResourceCost(content, SpriteName.rtsUpkeep, "Gold", City.ExpandGuardSizeCost * count, player.faction.gold);
                content.newLine();
                content.icontext(SpriteName.rtsUpkeepTime, "Upkeep +" + city.GuardUpkeep(City.ExpandGuardSize * count).ToString());
                
                content.h2("Gain");
                
                content.icontext(SpriteName.WarsGuardAdd,"Max guard size +" + (City.ExpandGuardSize * count).ToString());
            }
            else 
            {
                content.Add(new RichBoxText("Has reached maximum capacity. You need to expand the city.", Color.Red));
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
            content.h2("Cost");
            content.newLine();
            HudLib.ResourceCost(content, SpriteName.rtsUpkeep,"Gold", opt.goldCost * count, player.faction.gold);
            content.newLine();
            HudLib.ResourceCost(content, SpriteName.WarsWorkerSub, "Workers", typeData.workForceCount() * count, city.workForce.Int());
            content.newLine();
            content.newLine();
            content.icontext(SpriteName.rtsUpkeep, "Upkeep: " + (typeData.Upkeep() * count).ToString());
            content.newParagraph();

            content.h2("Gain");
            int unitCount = typeData.rowWidth * typeData.columnsDepth;
            string countText = "{0} groups, a total of {1} units";
            content.text(string.Format(countText, count, unitCount * count));
            content.newParagraph();

            content.h2("Stats per unit");
            content.text("Cost: " + string.Format(HudLib.OneDecimalFormat, opt.goldCost / (double)unitCount));
            content.text("Upkeep: " + string.Format(HudLib.OneDecimalFormat, typeData.Upkeep() / (double)unitCount));

            content.text("Attack strength: ");

            string attackStrengthAreas = "Land {0} | Sea {1} | City {2}";
            content.text(string.Format(attackStrengthAreas, dpsCompared(typeData.DPS_land(), dpsSoldier), dpsCompared(typeData.DPS_sea(), dpsSoldier), dpsCompared(typeData.DPS_structure(), dpsSoldier)));

            //content.text("Attack on sea: " + dpsCompared(typeData.DPS_sea(), dpsSoldier));
            //content.text("Attack city: " + dpsCompared(typeData.DPS_structure(), dpsSoldier));
            content.text("Health: " + typeData.basehealth);


            speedBonus(true, typeData.ArmySpeedBonusLand);
            speedBonus(false, typeData.ArmySpeedBonusSea);


            player.hud.tooltip.create(player, content, true);

            void speedBonus(bool land, double bonus)
            {
                if (bonus != 0)
                {                    
                    string bonusText = land? "Army speed bonus on land: {0}" : "Army speed bonus on sea: {0}";
                    content.text(string.Format(bonusText, TextLib.PercentAddText((float)bonus)));
                }
            }
        }

        string dpsCompared(int dps, int dpsSoldier)
        {
           return string.Format(HudLib.OneDecimalFormat, dps/(double)dpsSoldier);
        }

        
    }
}

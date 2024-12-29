
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Work
{
    struct WorkTemplate
    {
        public const int NoPrio = 0;
        public const int MinPrio = 1;
        public const int MaxPrio = 5;
        public const int SafeGuardPrio = MaxPrio + 1;

        public WorkPriority move = new WorkPriority(3);
        public WorkPriority wood = new WorkPriority(2);
        public WorkPriority stone = new WorkPriority(2);
        public WorkPriority craft_fuel = new WorkPriority(4);
        public WorkPriority craft_food = new WorkPriority(4);
        public WorkPriority craft_beer = new WorkPriority(1);
        public WorkPriority craft_coolingfluid = new WorkPriority(0);

        public WorkPriority craft_iron = new WorkPriority(3);
        public WorkPriority craft_tin = new WorkPriority(0);
        public WorkPriority craft_cupper = new WorkPriority(0);
        public WorkPriority craft_lead = new WorkPriority(0);
        public WorkPriority craft_silver = new WorkPriority(0);

        public WorkPriority craft_bronze = new WorkPriority(0);
        public WorkPriority craft_castiron = new WorkPriority(0);
        public WorkPriority craft_bloomeryiron = new WorkPriority(0);
        public WorkPriority craft_steel = new WorkPriority(0);
        public WorkPriority craft_mithril = new WorkPriority(0);

        public WorkPriority craft_toolkit = new WorkPriority(0);
        public WorkPriority craft_wagonlight = new WorkPriority(0);
        public WorkPriority craft_wagonheavy = new WorkPriority(0);
        public WorkPriority craft_blackpowder = new WorkPriority(0);
        public WorkPriority craft_gunpowder = new WorkPriority(0);
        public WorkPriority craft_bullet = new WorkPriority(0);

        public WorkPriority craft_sharpstick = new WorkPriority(1);
        public WorkPriority craft_bronzesword = new WorkPriority(0);
        public WorkPriority craft_shortsword = new WorkPriority(0);
        public WorkPriority craft_sword = new WorkPriority(0);
        public WorkPriority craft_longsword = new WorkPriority(0);
        public WorkPriority craft_handspear = new WorkPriority(0);
        public WorkPriority craft_mithrilsword = new WorkPriority(0);
        public WorkPriority craft_warhammer = new WorkPriority(0);
        public WorkPriority craft_twohandsword = new WorkPriority(0);
        public WorkPriority craft_knightslance = new WorkPriority(0);

        public WorkPriority craft_slingshot = new WorkPriority(0);
        public WorkPriority craft_throwingspear = new WorkPriority(0);
        public WorkPriority craft_bow = new WorkPriority(0);
        public WorkPriority craft_longbow = new WorkPriority(0);
        public WorkPriority craft_crossbow = new WorkPriority(0);
        public WorkPriority craft_mithrilbow = new WorkPriority(0);

        public WorkPriority craft_handcannon = new WorkPriority(0);
        public WorkPriority craft_handculverin = new WorkPriority(0);
        public WorkPriority craft_rifle = new WorkPriority(0);
        public WorkPriority craft_blunderbus = new WorkPriority(0);

        public WorkPriority craft_ballista = new WorkPriority(0);
        public WorkPriority craft_manuballista = new WorkPriority(0);
        public WorkPriority craft_catapult = new WorkPriority(0);
        public WorkPriority craft_batteringram = new WorkPriority(0);

        public WorkPriority craft_siegecannonbronze = new WorkPriority(0);
        public WorkPriority craft_mancannonbronze = new WorkPriority(0);
        public WorkPriority craft_siegecannoniron = new WorkPriority(0);
        public WorkPriority craft_mancannoniron = new WorkPriority(0);

        public WorkPriority craft_paddedarmor = new WorkPriority(1);
        public WorkPriority craft_heavypaddedarmor = new WorkPriority(0);
        public WorkPriority craft_bronzearmor = new WorkPriority(0);
        public WorkPriority craft_mailarmor = new WorkPriority(0);
        public WorkPriority craft_heavymailarmor = new WorkPriority(0);
        public WorkPriority craft_platearmor = new WorkPriority(0);
        public WorkPriority craft_fullplatearmor = new WorkPriority(0);
        public WorkPriority craft_mithrilarmor = new WorkPriority(0);

        public WorkPriority farm_food = new WorkPriority(2);
        public WorkPriority farm_fuel = new WorkPriority(2);
        public WorkPriority farm_linen = new WorkPriority(1);
        public WorkPriority bogiron = new WorkPriority(1);
        public WorkPriority mining_iron = new WorkPriority(3);
        public WorkPriority mining_tin = new WorkPriority(0);
        public WorkPriority mining_cupper = new WorkPriority(0);
        public WorkPriority mining_lead = new WorkPriority(0);
        public WorkPriority mining_silver = new WorkPriority(0);
        public WorkPriority mining_gold = new WorkPriority(1);
        public WorkPriority mining_mithril = new WorkPriority(0);
        public WorkPriority mining_sulfur = new WorkPriority(0);
        public WorkPriority mining_coal = new WorkPriority(0);

        public WorkPriority trading = new WorkPriority(2);
        public WorkPriority autoBuild = new WorkPriority(1);

        public WorkPriority coinmaker_cupper = new WorkPriority(0);
        public WorkPriority coinmaker_bronze = new WorkPriority(0);
        public WorkPriority coinmaker_silver = new WorkPriority(0);
        public WorkPriority coinmaker_mithril = new WorkPriority(0);
        public void applyUnlock(Unlocks unlocks)
        {
            coinmaker_cupper.unlocked = unlocks.coinMaking;
            coinmaker_bronze.unlocked = unlocks.coinMaking;
            coinmaker_silver.unlocked = unlocks.coinMaking;
            coinmaker_mithril.unlocked = unlocks.coinMaking;

            craft_toolkit.unlocked = unlocks.item_tools;

            craft_castiron.unlocked = unlocks.item_castIron;
            craft_mithril.unlocked = unlocks.item_castMithril;

            craft_iron.unlocked = unlocks.item_Iron;
            craft_shortsword.unlocked = unlocks.item_Sword;
            craft_sword.unlocked = unlocks.item_Sword;
            craft_mailarmor.unlocked = unlocks.item_IronArmor;
            craft_heavymailarmor.unlocked = unlocks.item_IronArmor;
            craft_warhammer.unlocked = unlocks.item_Sword;
            craft_knightslance.unlocked = unlocks.item_Sword;

            craft_bloomeryiron.unlocked = unlocks.item_Steel;
            craft_steel.unlocked = unlocks.item_Steel;
            craft_longsword.unlocked = unlocks.item_LongSword;
            craft_twohandsword.unlocked = unlocks.item_LongSword;
            craft_platearmor.unlocked = unlocks.item_SteelArmor;
            craft_fullplatearmor.unlocked = unlocks.item_SteelArmor;

            craft_catapult.unlocked = unlocks.item_catapult;
            craft_manuballista.unlocked = unlocks.item_catapult;
            craft_crossbow.unlocked = unlocks.item_crossbow;

            craft_blackpowder.unlocked = unlocks.item_blackPowder;
            craft_bullet.unlocked = unlocks.item_blackPowder;
            craft_handcannon.unlocked = unlocks.item_blackPowder;
            craft_handculverin.unlocked = unlocks.item_blackPowder;
            
            craft_gunpowder.unlocked = unlocks.item_gunPowder;
            craft_rifle.unlocked = unlocks.item_gunPowder;
            craft_blunderbus.unlocked = unlocks.item_gunPowder;

            craft_siegecannonbronze.unlocked = unlocks.item_cannon;
            craft_mancannonbronze.unlocked = unlocks.item_cannon;
            craft_siegecannoniron.unlocked = unlocks.item_cannon;
            craft_mancannoniron.unlocked = unlocks.item_cannon;
        }

        public WorkTemplate()
        {
        }

        public void writeGameState(System.IO.BinaryWriter w, bool isCity)
        {
            move.writeGameState(w, isCity);
            wood.writeGameState(w, isCity);
            stone.writeGameState(w, isCity);
            craft_fuel.writeGameState(w, isCity);
            craft_food.writeGameState(w, isCity);
            craft_beer.writeGameState(w, isCity);
            craft_coolingfluid.writeGameState(w, isCity);

            craft_iron.writeGameState(w, isCity);
            craft_tin.writeGameState(w, isCity);
            craft_cupper.writeGameState(w, isCity);
            craft_lead.writeGameState(w, isCity);
            craft_silver.writeGameState(w, isCity);

            craft_bronze.writeGameState(w, isCity);
            craft_castiron.writeGameState(w, isCity);
            craft_bloomeryiron.writeGameState(w, isCity);
            craft_steel.writeGameState(w, isCity);
            craft_mithril.writeGameState(w, isCity);

            craft_toolkit.writeGameState(w, isCity);
            craft_wagonlight.writeGameState(w, isCity);
            craft_wagonheavy.writeGameState(w, isCity);
            craft_blackpowder.writeGameState(w, isCity);
            craft_gunpowder.writeGameState(w, isCity);
            craft_bullet.writeGameState(w, isCity);

            craft_sharpstick.writeGameState(w, isCity);
            craft_bronzesword.writeGameState(w, isCity);
            craft_shortsword.writeGameState(w, isCity);
            craft_sword.writeGameState(w, isCity);
            craft_longsword.writeGameState(w, isCity);
            craft_handspear.writeGameState(w, isCity);
            craft_mithrilsword.writeGameState(w, isCity);
            craft_warhammer.writeGameState(w, isCity);
            craft_twohandsword.writeGameState(w, isCity);
            craft_knightslance.writeGameState(w, isCity);

            craft_slingshot.writeGameState(w, isCity);
            craft_throwingspear.writeGameState(w, isCity);
            craft_bow.writeGameState(w, isCity);
            craft_longbow.writeGameState(w, isCity);
            craft_crossbow.writeGameState(w, isCity);
            craft_mithrilbow.writeGameState(w, isCity);

            craft_handcannon.writeGameState(w, isCity);
            craft_handculverin.writeGameState(w, isCity);
            craft_rifle.writeGameState(w, isCity);
            craft_blunderbus.writeGameState(w, isCity);

            craft_ballista.writeGameState(w, isCity);
            craft_manuballista.writeGameState(w, isCity);
            craft_catapult.writeGameState(w, isCity);
            craft_batteringram.writeGameState(w, isCity);

            craft_siegecannonbronze.writeGameState(w, isCity);
            craft_mancannonbronze.writeGameState(w, isCity);
            craft_siegecannoniron.writeGameState(w, isCity);
            craft_mancannoniron.writeGameState(w, isCity);

            craft_paddedarmor.writeGameState(w, isCity);
            craft_heavypaddedarmor.writeGameState(w, isCity);
            craft_bronzearmor.writeGameState(w, isCity);
            craft_mailarmor.writeGameState(w, isCity);
            craft_heavymailarmor.writeGameState(w, isCity);
            craft_platearmor.writeGameState(w, isCity);
            craft_fullplatearmor.writeGameState(w, isCity);
            craft_mithrilarmor.writeGameState(w, isCity);

            farm_food.writeGameState(w, isCity);
            farm_fuel.writeGameState(w, isCity);
            farm_linen.writeGameState(w, isCity);
            bogiron.writeGameState(w, isCity);
            mining_iron.writeGameState(w, isCity);
            mining_tin.writeGameState(w, isCity);
            mining_cupper.writeGameState(w, isCity);
            mining_lead.writeGameState(w, isCity);
            mining_silver.writeGameState(w, isCity);
            mining_gold.writeGameState(w, isCity);
            mining_mithril.writeGameState(w, isCity);
            mining_sulfur.writeGameState(w, isCity);
            mining_coal.writeGameState(w, isCity);

            trading.writeGameState(w, isCity);
            autoBuild.writeGameState(w, isCity);

            coinmaker_cupper.writeGameState(w, isCity);
            coinmaker_bronze.writeGameState(w, isCity);
            coinmaker_silver.writeGameState(w, isCity);
            coinmaker_mithril.writeGameState(w, isCity);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            move.readGameState(r, subversion, isCity);
            wood.readGameState(r, subversion, isCity);
            stone.readGameState(r, subversion, isCity);
            craft_fuel.readGameState(r, subversion, isCity);
            craft_food.readGameState(r, subversion, isCity);
            craft_beer.readGameState(r, subversion, isCity);
            craft_coolingfluid.readGameState(r, subversion, isCity);

            craft_iron.readGameState(r, subversion, isCity);
            craft_tin.readGameState(r, subversion, isCity);
            craft_cupper.readGameState(r, subversion, isCity);
            craft_lead.readGameState(r, subversion, isCity);
            craft_silver.readGameState(r, subversion, isCity);

            craft_bronze.readGameState(r, subversion, isCity);
            craft_castiron.readGameState(r, subversion, isCity);
            craft_bloomeryiron.readGameState(r, subversion, isCity);
            craft_steel.readGameState(r, subversion, isCity);
            craft_mithril.readGameState(r, subversion, isCity);

            craft_toolkit.readGameState(r, subversion, isCity);
            craft_wagonlight.readGameState(r, subversion, isCity);
            craft_wagonheavy.readGameState(r, subversion, isCity);
            craft_blackpowder.readGameState(r, subversion, isCity);
            craft_gunpowder.readGameState(r, subversion, isCity);
            craft_bullet.readGameState(r, subversion, isCity);

            craft_sharpstick.readGameState(r, subversion, isCity);
            craft_bronzesword.readGameState(r, subversion, isCity);
            craft_shortsword.readGameState(r, subversion, isCity);
            craft_sword.readGameState(r, subversion, isCity);
            craft_longsword.readGameState(r, subversion, isCity);
            craft_handspear.readGameState(r, subversion, isCity);
            craft_mithrilsword.readGameState(r, subversion, isCity);
            craft_warhammer.readGameState(r, subversion, isCity);
            craft_twohandsword.readGameState(r, subversion, isCity);
            craft_knightslance.readGameState(r, subversion, isCity);

            craft_slingshot.readGameState(r, subversion, isCity);
            craft_throwingspear.readGameState(r, subversion, isCity);
            craft_bow.readGameState(r, subversion, isCity);
            craft_longbow.readGameState(r, subversion, isCity);
            craft_crossbow.readGameState(r, subversion, isCity);
            craft_mithrilbow.readGameState(r, subversion, isCity);

            craft_handcannon.readGameState(r, subversion, isCity);
            craft_handculverin.readGameState(r, subversion, isCity);
            craft_rifle.readGameState(r, subversion, isCity);
            craft_blunderbus.readGameState(r, subversion, isCity);

            craft_ballista.readGameState(r, subversion, isCity);
            craft_manuballista.readGameState(r, subversion, isCity);
            craft_catapult.readGameState(r, subversion, isCity);
            craft_batteringram.readGameState(r, subversion, isCity);

            craft_siegecannonbronze.readGameState(r, subversion, isCity);
            craft_mancannonbronze.readGameState(r, subversion, isCity);
            craft_siegecannoniron.readGameState(r, subversion, isCity);
            craft_mancannoniron.readGameState(r, subversion, isCity);

            craft_paddedarmor.readGameState(r, subversion, isCity);
            craft_heavypaddedarmor.readGameState(r, subversion, isCity);
            craft_bronzearmor.readGameState(r, subversion, isCity);
            craft_mailarmor.readGameState(r, subversion, isCity);
            craft_heavymailarmor.readGameState(r, subversion, isCity);
            craft_platearmor.readGameState(r, subversion, isCity);
            craft_fullplatearmor.readGameState(r, subversion, isCity);
            craft_mithrilarmor.readGameState(r, subversion, isCity);

            farm_food.readGameState(r, subversion, isCity);
            farm_fuel.readGameState(r, subversion, isCity);
            farm_linen.readGameState(r, subversion, isCity);
            bogiron.readGameState(r, subversion, isCity);
            mining_iron.readGameState(r, subversion, isCity);
            mining_tin.readGameState(r, subversion, isCity);
            mining_cupper.readGameState(r, subversion, isCity);
            mining_lead.readGameState(r, subversion, isCity);
            mining_silver.readGameState(r, subversion, isCity);
            mining_gold.readGameState(r, subversion, isCity);
            mining_mithril.readGameState(r, subversion, isCity);
            mining_sulfur.readGameState(r, subversion, isCity);
            mining_coal.readGameState(r, subversion, isCity);

            trading.readGameState(r, subversion, isCity);
            autoBuild.readGameState(r, subversion, isCity);

            coinmaker_cupper.readGameState(r, subversion, isCity);
            coinmaker_bronze.readGameState(r, subversion, isCity);
            coinmaker_silver.readGameState(r, subversion, isCity);
            coinmaker_mithril.readGameState(r, subversion, isCity);
        }



        public void onFactionChange(WorkTemplate factionTemplate)
        {
            move.onFactionValueChange(factionTemplate.move);
            wood.onFactionValueChange(factionTemplate.wood);
            stone.onFactionValueChange(factionTemplate.stone);
            craft_fuel.onFactionValueChange(factionTemplate.craft_fuel);
            craft_food.onFactionValueChange(factionTemplate.craft_food);
            craft_beer.onFactionValueChange(factionTemplate.craft_beer);
            craft_coolingfluid.onFactionValueChange(factionTemplate.craft_coolingfluid);

            craft_iron.onFactionValueChange(factionTemplate.craft_iron);
            craft_tin.onFactionValueChange(factionTemplate.craft_tin);
            craft_cupper.onFactionValueChange(factionTemplate.craft_cupper);
            craft_lead.onFactionValueChange(factionTemplate.craft_lead);
            craft_silver.onFactionValueChange(factionTemplate.craft_silver);

            craft_bronze.onFactionValueChange(factionTemplate.craft_bronze);
            craft_castiron.onFactionValueChange(factionTemplate.craft_castiron);
            craft_bloomeryiron.onFactionValueChange(factionTemplate.craft_bloomeryiron);
            craft_steel.onFactionValueChange(factionTemplate.craft_steel);
            craft_mithril.onFactionValueChange(factionTemplate.craft_mithril);

            craft_toolkit.onFactionValueChange(factionTemplate.craft_toolkit);
            craft_wagonlight.onFactionValueChange(factionTemplate.craft_wagonlight);
            craft_wagonheavy.onFactionValueChange(factionTemplate.craft_wagonheavy);
            craft_blackpowder.onFactionValueChange(factionTemplate.craft_blackpowder);
            craft_gunpowder.onFactionValueChange(factionTemplate.craft_gunpowder);
            craft_bullet.onFactionValueChange(factionTemplate.craft_bullet);

            craft_sharpstick.onFactionValueChange(factionTemplate.craft_sharpstick);
            craft_bronzesword.onFactionValueChange(factionTemplate.craft_bronzesword);
            craft_shortsword.onFactionValueChange(factionTemplate.craft_shortsword);
            craft_sword.onFactionValueChange(factionTemplate.craft_sword);
            craft_longsword.onFactionValueChange(factionTemplate.craft_longsword);
            craft_handspear.onFactionValueChange(factionTemplate.craft_handspear);
            craft_mithrilsword.onFactionValueChange(factionTemplate.craft_mithrilsword);
            craft_warhammer.onFactionValueChange(factionTemplate.craft_warhammer);
            craft_twohandsword.onFactionValueChange(factionTemplate.craft_twohandsword);
            craft_knightslance.onFactionValueChange(factionTemplate.craft_knightslance);

            craft_slingshot.onFactionValueChange(factionTemplate.craft_slingshot);
            craft_throwingspear.onFactionValueChange(factionTemplate.craft_throwingspear);
            craft_bow.onFactionValueChange(factionTemplate.craft_bow);
            craft_longbow.onFactionValueChange(factionTemplate.craft_longbow);
            craft_crossbow.onFactionValueChange(factionTemplate.craft_crossbow);
            craft_mithrilbow.onFactionValueChange(factionTemplate.craft_mithrilbow);

            craft_handcannon.onFactionValueChange(factionTemplate.craft_handcannon);
            craft_handculverin.onFactionValueChange(factionTemplate.craft_handculverin);
            craft_rifle.onFactionValueChange(factionTemplate.craft_rifle);
            craft_blunderbus.onFactionValueChange(factionTemplate.craft_blunderbus);

            craft_ballista.onFactionValueChange(factionTemplate.craft_ballista);
            craft_manuballista.onFactionValueChange(factionTemplate.craft_manuballista);
            craft_catapult.onFactionValueChange(factionTemplate.craft_catapult);
            craft_batteringram.onFactionValueChange(factionTemplate.craft_batteringram);

            craft_siegecannonbronze.onFactionValueChange(factionTemplate.craft_siegecannonbronze);
            craft_mancannonbronze.onFactionValueChange(factionTemplate.craft_mancannonbronze);
            craft_siegecannoniron.onFactionValueChange(factionTemplate.craft_siegecannoniron);
            craft_mancannoniron.onFactionValueChange(factionTemplate.craft_mancannoniron);

            craft_paddedarmor.onFactionValueChange(factionTemplate.craft_paddedarmor);
            craft_heavypaddedarmor.onFactionValueChange(factionTemplate.craft_heavypaddedarmor);
            craft_bronzearmor.onFactionValueChange(factionTemplate.craft_bronzearmor);
            craft_mailarmor.onFactionValueChange(factionTemplate.craft_mailarmor);
            craft_heavymailarmor.onFactionValueChange(factionTemplate.craft_heavymailarmor);
            craft_platearmor.onFactionValueChange(factionTemplate.craft_platearmor);
            craft_fullplatearmor.onFactionValueChange(factionTemplate.craft_fullplatearmor);
            craft_mithrilarmor.onFactionValueChange(factionTemplate.craft_mithrilarmor);

            farm_food.onFactionValueChange(factionTemplate.farm_food);
            farm_fuel.onFactionValueChange(factionTemplate.farm_fuel);
            farm_linen.onFactionValueChange(factionTemplate.farm_linen);
            bogiron.onFactionValueChange(factionTemplate.bogiron);
            mining_iron.onFactionValueChange(factionTemplate.mining_iron);
            mining_tin.onFactionValueChange(factionTemplate.mining_tin);
            mining_cupper.onFactionValueChange(factionTemplate.mining_cupper);
            mining_lead.onFactionValueChange(factionTemplate.mining_lead);
            mining_silver.onFactionValueChange(factionTemplate.mining_silver);
            mining_gold.onFactionValueChange(factionTemplate.mining_gold);
            mining_mithril.onFactionValueChange(factionTemplate.mining_mithril);
            mining_sulfur.onFactionValueChange(factionTemplate.mining_sulfur);
            mining_coal.onFactionValueChange(factionTemplate.mining_sulfur);

            trading.onFactionValueChange(factionTemplate.trading);
            autoBuild.onFactionValueChange(factionTemplate.autoBuild);

            coinmaker_cupper.onFactionValueChange(factionTemplate.coinmaker_cupper);
            coinmaker_bronze.onFactionValueChange(factionTemplate.coinmaker_bronze);
            coinmaker_silver.onFactionValueChange(factionTemplate.coinmaker_silver);
            coinmaker_mithril.onFactionValueChange(factionTemplate.coinmaker_mithril);
        }

        public void setAllToFollowFaction()
        {
            move.followFaction = true;

            wood.followFaction = true;
            stone.followFaction = true;
            craft_fuel.followFaction = true;
            craft_food.followFaction = true;
            craft_beer.followFaction = true;
            craft_coolingfluid.followFaction = true;

            craft_iron.followFaction = true;
            craft_tin.followFaction = true;
            craft_cupper.followFaction = true;
            craft_lead.followFaction = true;
            craft_silver.followFaction = true;

            craft_bronze.followFaction = true;
            craft_castiron.followFaction = true;
            craft_bloomeryiron.followFaction = true;
            craft_steel.followFaction = true;
            craft_mithril.followFaction = true;

            craft_toolkit.followFaction = true;
            craft_wagonlight.followFaction = true;
            craft_wagonheavy.followFaction = true;
            craft_blackpowder.followFaction = true;
            craft_gunpowder.followFaction = true;
            craft_bullet.followFaction = true;

            craft_sharpstick.followFaction = true;
            craft_bronzesword.followFaction = true;
            craft_shortsword.followFaction = true;
            craft_sword.followFaction = true;
            craft_longsword.followFaction = true;
            craft_handspear.followFaction = true;
            craft_mithrilsword.followFaction = true;
            craft_warhammer.followFaction = true;
            craft_twohandsword.followFaction = true;
            craft_knightslance.followFaction = true;

            craft_slingshot.followFaction = true;
            craft_throwingspear.followFaction = true;
            craft_bow.followFaction = true;
            craft_longbow.followFaction = true;
            craft_crossbow.followFaction = true;
            craft_mithrilbow.followFaction = true;

            craft_handcannon.followFaction = true;
            craft_handculverin.followFaction = true;
            craft_rifle.followFaction = true;
            craft_blunderbus.followFaction = true;

            craft_ballista.followFaction = true;
            craft_manuballista.followFaction = true;
            craft_catapult.followFaction = true;
            craft_batteringram.followFaction = true;

            craft_siegecannonbronze.followFaction = true;
            craft_mancannonbronze.followFaction = true;
            craft_siegecannoniron.followFaction = true;
            craft_mancannoniron.followFaction = true;

            craft_paddedarmor.followFaction = true;
            craft_heavypaddedarmor.followFaction = true;
            craft_bronzearmor.followFaction = true;
            craft_mailarmor.followFaction = true;
            craft_heavymailarmor.followFaction = true;
            craft_platearmor.followFaction = true;
            craft_fullplatearmor.followFaction = true;
            craft_mithrilarmor.followFaction = true;

            farm_food.followFaction = true;
            farm_fuel.followFaction = true;
            farm_linen.followFaction = true;
            bogiron.followFaction = true;
            mining_iron.followFaction = true;
            mining_tin.followFaction = true;
            mining_cupper.followFaction = true;
            mining_lead.followFaction = true;
            mining_silver.followFaction = true;
            mining_gold.followFaction = true;
            mining_mithril.followFaction = true;
            mining_sulfur.followFaction = true;
            mining_coal.followFaction = true;

            trading.followFaction = true;
            autoBuild.followFaction = true;

            coinmaker_cupper.followFaction = true;
            coinmaker_bronze.followFaction = true;
            coinmaker_silver.followFaction = true;
            coinmaker_mithril.followFaction = true;
        }

        public void setWorkPrio(int set, WorkPriorityType priorityType)
        {
            var work = GetWorkPriority(priorityType);
            work.value = set;//Bound.Set(work.value + set, NoPrio, MaxPrio);
            work.followFaction = false;
            SetWorkPriority(priorityType, work);
        }
        //public void setWorkPrioSafeGuard(bool set, WorkPriorityType priorityType)
        //{
        //    var work = GetWorkPriority(priorityType);
        //    work.safeguard = set;//Bound.Set(work.value + set, NoPrio, MaxPrio);
        //    work.followFaction = false;
        //    SetWorkPriority(priorityType, work);
        //}
        public void followFactionClick(WorkPriorityType prioType, WorkTemplate factionTemplate)
        {
            var work = GetWorkPriority(prioType);
            work.followFaction = !work.followFaction;
            work.onFactionValueChange(factionTemplate.GetWorkPriority(prioType));
            SetWorkPriority(prioType, work);
        }
        public WorkPriority GetWorkPriority(ItemResourceType item)
        {
            switch (item)
            {
                case ItemResourceType.Fuel_G:
                    return craft_fuel;
                case ItemResourceType.Food_G:
                    return craft_food;
                case ItemResourceType.Beer:
                    return craft_beer;
                case ItemResourceType.CoolingFluid:
                    return craft_coolingfluid;

                case ItemResourceType.Iron_G:
                    return craft_iron;
                case ItemResourceType.Tin:
                    return craft_tin;
                case ItemResourceType.Copper:
                    return craft_cupper;
                case ItemResourceType.Lead:
                    return craft_lead;
                case ItemResourceType.Silver:
                    return craft_silver;

                case ItemResourceType.Bronze:
                    return craft_bronze;
                case ItemResourceType.CastIron:
                    return craft_castiron;
                case ItemResourceType.BloomeryIron:
                    return craft_bloomeryiron;
                case ItemResourceType.Steel:
                    return craft_steel;
                case ItemResourceType.Mithril:
                    return craft_mithril;

                case ItemResourceType.Toolkit:
                    return craft_toolkit;
                case ItemResourceType.Wagon2Wheel:
                    return craft_wagonlight;
                case ItemResourceType.Wagon4Wheel:
                    return craft_wagonheavy;
                case ItemResourceType.BlackPowder:
                    return craft_blackpowder;
                case ItemResourceType.GunPowder:
                    return craft_gunpowder;
                case ItemResourceType.LedBullet:
                    return craft_bullet;

                case ItemResourceType.PaddedArmor:
                    return craft_paddedarmor;
                case ItemResourceType.HeavyPaddedArmor:
                    return craft_heavypaddedarmor;
                case ItemResourceType.BronzeArmor:
                    return craft_bronzearmor;
                case ItemResourceType.IronArmor:
                    return craft_mailarmor;
                case ItemResourceType.HeavyIronArmor:
                    return craft_heavymailarmor;
                case ItemResourceType.LightPlateArmor:
                    return craft_platearmor;
                case ItemResourceType.FullPlateArmor:
                    return craft_fullplatearmor;
                case ItemResourceType.MithrilArmor:
                    return craft_mithrilarmor;

                case ItemResourceType.SharpStick:
                    return craft_sharpstick;
                case ItemResourceType.BronzeSword:
                    return craft_bronzesword;
                case ItemResourceType.ShortSword:
                    return craft_shortsword;
                case ItemResourceType.Sword:
                    return craft_sword;
                case ItemResourceType.LongSword:
                    return craft_longsword;
                case ItemResourceType.HandSpear:
                    return craft_handspear;
                case ItemResourceType.MithrilSword:
                    return craft_mithrilsword;
                case ItemResourceType.Warhammer:
                    return craft_warhammer;
                case ItemResourceType.TwoHandSword:
                    return craft_twohandsword;
                case ItemResourceType.KnightsLance:
                    return craft_knightslance;

                case ItemResourceType.SlingShot:
                    return craft_slingshot;
                case ItemResourceType.ThrowingSpear:
                    return craft_throwingspear;
                case ItemResourceType.Bow:
                    return craft_bow;
                case ItemResourceType.LongBow:
                    return craft_longbow;
                case ItemResourceType.Crossbow:
                    return craft_crossbow;
                case ItemResourceType.MithrilBow:
                    return craft_mithrilbow;

                case ItemResourceType.HandCannon:
                    return craft_handcannon;
                case ItemResourceType.HandCulverin:
                    return craft_handculverin;
                case ItemResourceType.Rifle:
                    return craft_rifle;
                case ItemResourceType.Blunderbus:
                    return craft_blunderbus;

                case ItemResourceType.Ballista:
                    return craft_ballista;
                case ItemResourceType.Manuballista:
                    return craft_manuballista;
                case ItemResourceType.Catapult:
                    return craft_catapult;
                case ItemResourceType.UN_BatteringRam:
                    return craft_batteringram;
                case ItemResourceType.SiegeCannonBronze:
                    return craft_siegecannonbronze;
                case ItemResourceType.ManCannonBronze:
                    return craft_mancannonbronze;
                case ItemResourceType.SiegeCannonIron:
                    return craft_siegecannoniron;
                case ItemResourceType.ManCannonIron:
                    return craft_mancannoniron;


                default:
                    throw new NotImplementedException();
            }
        }

        public WorkPriority GetWorkPriority(WorkPriorityType priorityType)
        {
            switch (priorityType)
            {
                case WorkPriorityType.move:
                    return move;
                case WorkPriorityType.wood:
                    return wood;
                case WorkPriorityType.stone:
                    return stone;
                case WorkPriorityType.craftFuel:
                    return craft_fuel;
                case WorkPriorityType.craftFood:
                    return craft_food;
                case WorkPriorityType.craftBeer:
                    return craft_beer;
                case WorkPriorityType.craftCoolingFluid:
                    return craft_coolingfluid;

                case WorkPriorityType.smeltIron:
                    return craft_iron;
                case WorkPriorityType.smeltTin:
                    return craft_tin;
                case WorkPriorityType.smeltCopper:
                    return craft_cupper;
                case WorkPriorityType.smeltLead:
                    return craft_lead;
                case WorkPriorityType.smeltSilver:
                    return craft_silver;
                case WorkPriorityType.craftBronze:
                    return craft_bronze;
                case WorkPriorityType.craftCastIron:
                    return craft_castiron;
                case WorkPriorityType.craftBloomeryIron:
                    return craft_bloomeryiron;
                case WorkPriorityType.craftSteel:
                    return craft_steel;
                case WorkPriorityType.craftMithril:
                    return craft_mithril;

                case WorkPriorityType.craftToolkit:
                    return craft_toolkit;
                case WorkPriorityType.craftWagonLight:
                    return craft_wagonlight;
                case WorkPriorityType.craftWagonHeavy:
                    return craft_wagonheavy;
                case WorkPriorityType.craftBlackPowder:
                    return craft_blackpowder;
                case WorkPriorityType.craftGunPowder:
                    return craft_gunpowder;
                case WorkPriorityType.craftBullet:
                    return craft_bullet;

                case WorkPriorityType.craftSharpStick:
                    return craft_sharpstick;
                case WorkPriorityType.craftBronzeSword:
                    return craft_bronzesword;
                case WorkPriorityType.craftShortSword:
                    return craft_shortsword;
                case WorkPriorityType.craftSword:
                    return craft_sword;
                case WorkPriorityType.craftLongSword:
                    return craft_longsword;
                case WorkPriorityType.craftHandSpear:
                    return craft_handspear;

                case WorkPriorityType.craftWarhammer:
                    return craft_warhammer;
                case WorkPriorityType.craftTwoHandSword:
                    return craft_twohandsword;
                case WorkPriorityType.craftKnightsLance:
                    return craft_knightslance;
                case WorkPriorityType.craftMithrilSword:
                    return craft_mithrilsword;
                case WorkPriorityType.craftMithrilbow:
                    return craft_mithrilbow;

                case WorkPriorityType.craftSlingshot:
                    return craft_slingshot;
                case WorkPriorityType.craftThrowingspear:
                    return craft_throwingspear;
                case WorkPriorityType.craftBow:
                    return craft_bow;
                case WorkPriorityType.craftLongbow:
                    return craft_longbow;
                case WorkPriorityType.craftCrossbow:
                    return craft_crossbow;

                case WorkPriorityType.craftHandCannon:
                    return craft_handcannon;
                case WorkPriorityType.craftHandCulverin:
                    return craft_handculverin;
                case WorkPriorityType.craftRifle:
                    return craft_rifle;
                case WorkPriorityType.craftBlunderbus:
                    return craft_blunderbus;

                case WorkPriorityType.craftBallista:
                    return craft_ballista;
                case WorkPriorityType.craftManuBallista:
                    return craft_manuballista;
                case WorkPriorityType.craftCatapult:
                    return craft_catapult;
                case WorkPriorityType.craftBatteringRam:
                    return craft_batteringram;

                case WorkPriorityType.craftSiegeCannonBronze:
                    return craft_siegecannonbronze;
                case WorkPriorityType.craftManCannonBronze:
                    return craft_mancannonbronze;
                case WorkPriorityType.craftSiegeCannonIron:
                    return craft_siegecannoniron;
                case WorkPriorityType.craftManCannonIron:
                    return craft_mancannoniron;

                case WorkPriorityType.craftPaddedArmor:
                    return craft_paddedarmor;
                case WorkPriorityType.craftHeavyPaddedArmor:
                    return craft_heavypaddedarmor;
                case WorkPriorityType.craftBronzeArmor:
                    return craft_bronzearmor;
                case WorkPriorityType.craftMailArmor:
                    return craft_mailarmor;
                case WorkPriorityType.craftHeavyMailArmor:
                    return craft_heavymailarmor;
                case WorkPriorityType.craftPlateArmor:
                    return craft_platearmor;
                case WorkPriorityType.craftFullPlateArmor:
                    return craft_fullplatearmor;
                case WorkPriorityType.craftMithrilArmor:
                    return craft_mithrilarmor;

                case WorkPriorityType.farmfood:
                    return farm_food;
                case WorkPriorityType.farmfuel:
                    return farm_fuel;
                case WorkPriorityType.farmlinen:
                    return farm_linen;

                case WorkPriorityType.bogiron:
                    return bogiron;

                case WorkPriorityType.miningIron:
                    return mining_iron;
                case WorkPriorityType.miningTin:
                    return mining_tin;
                case WorkPriorityType.miningCopper:
                    return mining_cupper;
                case WorkPriorityType.miningLead:
                    return mining_lead;
                case WorkPriorityType.miningSilver:
                    return mining_silver;
                case WorkPriorityType.miningGold:
                    return mining_gold;
                case WorkPriorityType.miningMithril:
                    return mining_mithril;
                case WorkPriorityType.miningSulfur:
                    return mining_sulfur;
                case WorkPriorityType.miningCoal:
                    return mining_coal;

                case WorkPriorityType.trading:
                    return trading;
                case WorkPriorityType.autoBuild:
                    return autoBuild;
                //case WorkPriorityType.expandFarms:
                //    return expandFarms;

                case WorkPriorityType.coinmaker_cupper:
                    return coinmaker_cupper;
                case WorkPriorityType.coinmaker_bronze:
                    return coinmaker_bronze;
                case WorkPriorityType.coinmaker_silver:
                    return coinmaker_silver;
                case WorkPriorityType.coinmaker_mithril:
                    return coinmaker_mithril;

                default:
                    throw new NotImplementedException();
            }
        }


        void SetWorkPriority(WorkPriorityType priorityType, WorkPriority value)
        {
            switch (priorityType)
            {
                case WorkPriorityType.move:
                    move = value;
                    break;
                case WorkPriorityType.wood:
                    wood = value;
                    break;
                case WorkPriorityType.stone:
                    stone = value;
                    break;
                case WorkPriorityType.craftFuel:
                    craft_fuel = value;
                    break;
                case WorkPriorityType.craftFood:
                    craft_food = value;
                    break;
                case WorkPriorityType.craftBeer:
                    craft_beer = value;
                    break;
                case WorkPriorityType.craftCoolingFluid:
                    craft_coolingfluid = value;
                    break;

                case WorkPriorityType.smeltIron:
                    craft_iron = value;
                    break;
                case WorkPriorityType.smeltTin:
                    craft_tin = value;
                    break;
                case WorkPriorityType.smeltCopper:
                    craft_cupper = value;
                    break;
                case WorkPriorityType.smeltLead:
                    craft_lead = value;
                    break;
                case WorkPriorityType.smeltSilver:
                    craft_silver = value;
                    break;

                case WorkPriorityType.craftBronze:
                    craft_bronze = value;
                    break;
                case WorkPriorityType.craftCastIron:
                    craft_castiron = value;
                    break;
                case WorkPriorityType.craftBloomeryIron:
                    craft_bloomeryiron = value;
                    break;
                case WorkPriorityType.craftSteel:
                    craft_steel = value;
                    break;
                case WorkPriorityType.craftMithril:
                    craft_mithril = value;
                    break;

                case WorkPriorityType.craftToolkit:
                    craft_toolkit = value;
                    break;
                case WorkPriorityType.craftWagonLight:
                    craft_wagonlight = value;
                    break;
                case WorkPriorityType.craftWagonHeavy:
                    craft_wagonheavy = value;
                    break;
                case WorkPriorityType.craftBlackPowder:
                    craft_blackpowder = value;
                    break;
                case WorkPriorityType.craftGunPowder:
                    craft_gunpowder = value;
                    break;
                case WorkPriorityType.craftBullet:
                    craft_bullet = value;
                    break;

                case WorkPriorityType.craftSharpStick:
                    craft_sharpstick = value;
                    break;
                case WorkPriorityType.craftBronzeSword:
                    craft_bronzesword = value;
                    break;
                case WorkPriorityType.craftShortSword:
                    craft_shortsword = value;
                    break;
                case WorkPriorityType.craftSword:
                    craft_sword = value;
                    break;
                case WorkPriorityType.craftLongSword:
                    craft_longsword = value;
                    break;
                case WorkPriorityType.craftHandSpear:
                    craft_handspear = value;
                    break;

                case WorkPriorityType.craftWarhammer:
                    craft_warhammer = value;
                    break;
                case WorkPriorityType.craftTwoHandSword:
                    craft_twohandsword = value;
                    break;
                case WorkPriorityType.craftKnightsLance:
                    craft_knightslance = value;
                    break;
                case WorkPriorityType.craftMithrilSword:
                    craft_mithrilsword = value;
                    break;

                case WorkPriorityType.craftSlingshot:
                    craft_slingshot = value;
                    break;
                case WorkPriorityType.craftThrowingspear:
                    craft_throwingspear = value;
                    break;
                case WorkPriorityType.craftBow:
                    craft_bow = value;
                    break;
                case WorkPriorityType.craftLongbow:
                    craft_longbow = value;
                    break;
                case WorkPriorityType.craftCrossbow:
                    craft_crossbow = value;
                    break;
                case WorkPriorityType.craftMithrilbow:
                    craft_mithrilbow = value;
                    break;

                case WorkPriorityType.craftHandCannon:
                    craft_handcannon = value;
                    break;
                case WorkPriorityType.craftHandCulverin:
                    craft_handculverin = value;
                    break;
                case WorkPriorityType.craftRifle:
                    craft_rifle = value;
                    break;
                case WorkPriorityType.craftBlunderbus:
                    craft_blunderbus = value;
                    break;

                case WorkPriorityType.craftBallista:
                    craft_ballista = value;
                    break;
                case WorkPriorityType.craftManuBallista:
                    craft_manuballista = value;
                    break;
                case WorkPriorityType.craftCatapult:
                    craft_catapult = value;
                    break;
                case WorkPriorityType.craftBatteringRam:
                    craft_batteringram = value;
                    break;

                case WorkPriorityType.craftSiegeCannonBronze:
                    craft_siegecannonbronze = value;
                    break;
                case WorkPriorityType.craftManCannonBronze:
                    craft_mancannonbronze = value;
                    break;
                case WorkPriorityType.craftSiegeCannonIron:
                    craft_siegecannoniron = value;
                    break;
                case WorkPriorityType.craftManCannonIron:
                    craft_mancannoniron = value;
                    break;

                case WorkPriorityType.craftPaddedArmor:
                    craft_paddedarmor = value;
                    break;
                case WorkPriorityType.craftHeavyPaddedArmor:
                    craft_heavypaddedarmor = value;
                    break;
                case WorkPriorityType.craftBronzeArmor:
                    craft_bronzearmor = value;
                    break;
                case WorkPriorityType.craftMailArmor:
                    craft_mailarmor = value;
                    break;
                case WorkPriorityType.craftHeavyMailArmor:
                    craft_heavymailarmor = value;
                    break;
                case WorkPriorityType.craftPlateArmor:
                    craft_platearmor = value;
                    break;
                case WorkPriorityType.craftFullPlateArmor:
                    craft_fullplatearmor = value;
                    break;
                case WorkPriorityType.craftMithrilArmor:
                    craft_mithrilarmor = value;
                    break;

                case WorkPriorityType.farmfood:
                    farm_food = value;
                    break;
                case WorkPriorityType.farmfuel:
                    farm_fuel = value;
                    break;
                case WorkPriorityType.farmlinen:
                    farm_linen = value;
                    break;

                case WorkPriorityType.bogiron:
                    bogiron = value;
                    break;
                case WorkPriorityType.miningIron:
                    mining_iron = value;
                    break;
                case WorkPriorityType.miningTin:
                    mining_tin = value;
                    break;
                case WorkPriorityType.miningCopper:
                    mining_cupper = value;
                    break;
                case WorkPriorityType.miningLead:
                    mining_lead = value;
                    break;
                case WorkPriorityType.miningSilver:
                    mining_silver = value;
                    break;
                case WorkPriorityType.miningGold:
                    mining_gold = value;
                    break;
                case WorkPriorityType.miningMithril:
                    mining_mithril = value;
                    break;
                case WorkPriorityType.miningSulfur:
                    mining_sulfur = value;
                    break;
                case WorkPriorityType.miningCoal:
                    mining_coal = value;
                    break;

                case WorkPriorityType.trading:
                    trading = value;
                    break;
                case WorkPriorityType.autoBuild:
                    autoBuild = value;
                    break;
                //case WorkPriorityType.expandFarms:
                //     = value;
                //    break;

                case WorkPriorityType.coinmaker_cupper:
                    coinmaker_cupper = value;
                    break;
                case WorkPriorityType.coinmaker_bronze:
                    coinmaker_bronze = value;
                    break;
                case WorkPriorityType.coinmaker_silver:
                    coinmaker_silver = value;
                    break;
                case WorkPriorityType.coinmaker_mithril:
                    coinmaker_mithril = value;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        


        public void toHud(Players.LocalPlayer player, RichBoxContent content, ResourcesSubTab tab, Faction faction, City city)
        {
            switch (tab)
            {
                case ResourcesSubTab.Work_Resources:
                    move.toHud(player, content, DssRef.lang.Work_Move, SpriteName.WarsWorkMove, SpriteName.WarsBuild_Storehouse, WorkPriorityType.move, faction, city);
                    wood.toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Wood), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Wood, WorkPriorityType.wood, faction, city);
                    stone.toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Stone), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Stone, WorkPriorityType.stone, faction, city);
                    craft_food.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Food), SpriteName.WarsHammer, SpriteName.WarsResource_Food, WorkPriorityType.craftFood, faction, city);
                    craft_fuel.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Fuel), SpriteName.WarsHammer, SpriteName.WarsResource_Fuel, WorkPriorityType.craftFuel, faction, city);
                    craft_beer.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Beer), SpriteName.WarsHammer, SpriteName.WarsResource_Beer, WorkPriorityType.craftBeer, faction, city);
                    craft_coolingfluid.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_CoolingFluid), SpriteName.WarsHammer, SpriteName.WarsResource_CoolingFluid, WorkPriorityType.craftCoolingFluid, faction, city);

                    craft_toolkit.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Toolkit), SpriteName.WarsHammer, SpriteName.WarsResource_Toolkit, WorkPriorityType.craftToolkit, faction, city);
                    craft_wagonlight.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Wagon2Wheel), SpriteName.WarsHammer, SpriteName.WarsResource_Wagon2Wheel, WorkPriorityType.craftWagonLight, faction, city);
                    craft_wagonheavy.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Wagon4Wheel), SpriteName.WarsHammer, SpriteName.WarsResource_Wagon4Wheel, WorkPriorityType.craftWagonHeavy, faction, city);
                    craft_blackpowder.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_BlackPowder), SpriteName.WarsHammer, SpriteName.WarsResource_BlackPowder, WorkPriorityType.craftBlackPowder, faction, city);
                    craft_gunpowder.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_GunPowder), SpriteName.WarsHammer, SpriteName.WarsResource_GunPowder, WorkPriorityType.craftGunPowder, faction, city);
                    craft_bullet.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_LedBullet), SpriteName.WarsHammer, SpriteName.WarsResource_Bullets, WorkPriorityType.craftBullet, faction, city);
                                        
                    farm_food.toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Food, SpriteName.WarsWorkFarm, SpriteName.WarsResource_RawFood, WorkPriorityType.farmfood, faction, city);
                    farm_fuel.toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Fuel, SpriteName.WarsWorkFarm, SpriteName.WarsResource_Fuel, WorkPriorityType.farmfuel, faction, city);
                    farm_linen.toHud(player, content, DssRef.lang.Work_Farming + ": " + DssRef.lang.Resource_TypeName_Linen, SpriteName.WarsWorkFarm, SpriteName.WarsResource_LinenCloth, WorkPriorityType.farmlinen, faction, city);
                                       
                    autoBuild.toHud(player, content, DssRef.lang.Work_AutoBuild, SpriteName.AutomationGearIcon, SpriteName.NO_IMAGE, WorkPriorityType.autoBuild, faction, city);
                   

                    HudLib.Description(content, string.Format(DssRef.lang.Work_OrderPrioDescription, MaxPrio));
                    break;

                case ResourcesSubTab.Work_Metals:
                    bogiron.toHud(player, content, DssRef.lang.Resource_TypeName_BogIron, SpriteName.WarsWorkCollect, SpriteName.WarsResource_IronOre, WorkPriorityType.bogiron, faction, city);
                    content.space();
                    HudLib.InfoButton(content, new RbAction(() => {
                        RichBoxContent content = new RichBoxContent();
                        content.text(DssRef.lang.Resource_BogIronDescription);
                        player.hud.tooltip.create(player, content, true);
                    }));

                    mining_iron.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.lang.Resource_TypeName_Iron), SpriteName.WarsWorkMine, SpriteName.WarsResource_Iron, WorkPriorityType.miningIron, faction, city);
                    mining_tin.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Tin), SpriteName.WarsWorkMine, SpriteName.WarsResource_Tin, WorkPriorityType.miningTin, faction, city);
                    mining_cupper.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Copper), SpriteName.WarsWorkMine, SpriteName.WarsResource_Copper, WorkPriorityType.miningCopper, faction, city);
                    mining_lead.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Lead), SpriteName.WarsWorkMine, SpriteName.WarsResource_Lead, WorkPriorityType.miningLead, faction, city);
                    mining_silver.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Silver), SpriteName.WarsWorkMine, SpriteName.WarsResource_Silver, WorkPriorityType.miningSilver, faction, city);
                    mining_gold.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.lang.ResourceType_Gold), SpriteName.WarsWorkMine, SpriteName.WarsResource_Gold, WorkPriorityType.miningGold, faction, city);
                    mining_mithril.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Mithril), SpriteName.WarsWorkMine, SpriteName.WarsResource_Mithril, WorkPriorityType.miningMithril, faction, city);
                    mining_sulfur.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.todoLang.Resource_TypeName_Sulfur), SpriteName.WarsWorkMine, SpriteName.WarsResource_Sulfur, WorkPriorityType.miningSulfur, faction, city);
                    mining_coal.toHud(player, content, string.Format(DssRef.todoLang.Work_MiningResource, DssRef.lang.Resource_TypeName_Coal), SpriteName.WarsWorkMine, SpriteName.WarsResource_Fuel, WorkPriorityType.miningCoal, faction, city);
                    content.newParagraph();
           
                    craft_iron.toHud(player, content, string.Format(DssRef.todoLang.Work_SmeltX, DssRef.lang.Resource_TypeName_Iron), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Iron, WorkPriorityType.smeltIron, faction, city);
                    craft_tin.toHud(player, content, string.Format(DssRef.todoLang.Work_SmeltX, DssRef.todoLang.Resource_TypeName_Tin), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Tin, WorkPriorityType.smeltTin, faction, city);
                    craft_cupper.toHud(player, content, string.Format(DssRef.todoLang.Work_SmeltX, DssRef.todoLang.Resource_TypeName_Copper), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Copper, WorkPriorityType.smeltCopper, faction, city);
                    craft_lead.toHud(player, content, string.Format(DssRef.todoLang.Work_SmeltX, DssRef.todoLang.Resource_TypeName_Lead), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Lead, WorkPriorityType.smeltLead, faction, city);
                    craft_silver.toHud(player, content, string.Format(DssRef.todoLang.Work_SmeltX, DssRef.todoLang.Resource_TypeName_Silver), SpriteName.WarsWorkSmelting, SpriteName.WarsResource_Silver, WorkPriorityType.smeltSilver, faction, city);
                    craft_bronze.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Bronze), SpriteName.WarsHammer, SpriteName.WarsResource_Bronze, WorkPriorityType.craftBronze, faction, city);
                    craft_castiron.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_CastIron), SpriteName.WarsHammer, SpriteName.WarsResource_CastIron, WorkPriorityType.craftCastIron, faction, city);
                    craft_bloomeryiron.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_BloomIron), SpriteName.WarsHammer, SpriteName.WarsResource_BloomeryIron, WorkPriorityType.craftBloomeryIron, faction, city);
                    craft_steel.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Steel), SpriteName.WarsHammer, SpriteName.WarsResource_Steel, WorkPriorityType.craftSteel, faction, city);
                    craft_mithril.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Mithril), SpriteName.WarsHammer, SpriteName.WarsResource_MithrilAlloy, WorkPriorityType.craftMithril, faction, city);

                    break;

                case ResourcesSubTab.Work_Weapons:
                    craft_sharpstick.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SharpStick), SpriteName.WarsHammer, SpriteName.WarsResource_Sharpstick, WorkPriorityType.craftSharpStick, faction, city);
                    craft_bronzesword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_BronzeSword), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeSword, WorkPriorityType.craftBronzeSword, faction, city);
                    craft_shortsword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_ShortSword), SpriteName.WarsHammer, SpriteName.WarsResource_ShortSword, WorkPriorityType.craftShortSword, faction, city);
                    craft_sword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Sword), SpriteName.WarsHammer, SpriteName.WarsResource_Sword, WorkPriorityType.craftSword, faction, city);
                    craft_longsword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_LongSword), SpriteName.WarsHammer, SpriteName.WarsResource_Longsword, WorkPriorityType.craftLongSword, faction, city);
                    craft_handspear.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HandSpear), SpriteName.WarsHammer, SpriteName.WarsResource_HandSpear, WorkPriorityType.craftHandSpear, faction, city);

                    craft_warhammer.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Warhammer), SpriteName.WarsHammer, SpriteName.WarsResource_Warhammer, WorkPriorityType.craftWarhammer, faction, city);
                    craft_twohandsword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_TwoHandSword), SpriteName.WarsHammer, SpriteName.WarsResource_TwoHandSword, WorkPriorityType.craftTwoHandSword, faction, city);
                    craft_knightslance.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_KnightsLance), SpriteName.WarsHammer, SpriteName.WarsResource_KnightsLance, WorkPriorityType.craftKnightsLance, faction, city);
                    craft_mithrilsword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_MithrilSword), SpriteName.WarsHammer, SpriteName.WarsResource_MithrilSword, WorkPriorityType.craftMithrilSword, faction, city);
                    craft_mithrilbow.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_MithrilBow), SpriteName.WarsHammer, SpriteName.WarsResource_Mithrilbow, WorkPriorityType.craftMithrilbow, faction, city);

                    break;

                case ResourcesSubTab.Work_Projectile:
                    craft_slingshot.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_SlingShot), SpriteName.WarsHammer, SpriteName.WarsResource_Slingshot, WorkPriorityType.craftSlingshot, faction, city);
                    craft_throwingspear.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_ThrowingSpear), SpriteName.WarsHammer, SpriteName.WarsResource_ThrowSpear, WorkPriorityType.craftThrowingspear, faction, city);
                    craft_bow.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bow), SpriteName.WarsHammer, SpriteName.WarsResource_Bow, WorkPriorityType.craftBow, faction, city);
                    craft_longbow.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Longbow), SpriteName.WarsHammer, SpriteName.WarsResource_Longbow, WorkPriorityType.craftLongbow, faction, city);
                    craft_crossbow.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Crossbow), SpriteName.WarsHammer, SpriteName.WarsResource_Crossbow, WorkPriorityType.craftCrossbow, faction, city);

                    craft_handcannon.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HandCannon), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeRifle, WorkPriorityType.craftHandCannon, faction, city);
                    craft_handculverin.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HandCulverin), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeShotgun, WorkPriorityType.craftHandCulverin, faction, city);
                    craft_rifle.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Rifle), SpriteName.WarsHammer, SpriteName.WarsResource_IronRifle, WorkPriorityType.craftRifle, faction, city);
                    craft_blunderbus.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Blunderbus), SpriteName.WarsHammer, SpriteName.WarsResource_IronShotgun, WorkPriorityType.craftBlunderbus, faction, city);

                    craft_ballista.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.UnitType_Ballista), SpriteName.WarsHammer, SpriteName.WarsResource_Ballista, WorkPriorityType.craftBallista, faction, city);
                    craft_manuballista.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Manuballista), SpriteName.WarsHammer, SpriteName.WarsResource_Manuballista, WorkPriorityType.craftManuBallista, faction, city);
                    craft_catapult.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Catapult), SpriteName.WarsHammer, SpriteName.WarsResource_Catapult, WorkPriorityType.craftCatapult, faction, city);

                    craft_siegecannonbronze.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_SiegeCannonBronze), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeSiegeCannon, WorkPriorityType.craftSiegeCannonBronze, faction, city);
                    craft_mancannonbronze.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_ManCannonBronze), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeManCannon, WorkPriorityType.craftManCannonBronze, faction, city);
                    craft_siegecannoniron.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_SiegeCannonIron), SpriteName.WarsHammer, SpriteName.WarsResource_IronSiegeCannon, WorkPriorityType.craftSiegeCannonIron, faction, city);
                    craft_mancannoniron.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_ManCannonIron), SpriteName.WarsHammer, SpriteName.WarsResource_IronManCannon, WorkPriorityType.craftManCannonIron, faction, city);

                    break;

                case ResourcesSubTab.Work_Armor:
                    craft_paddedarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_PaddedArmor), SpriteName.WarsHammer, SpriteName.WarsResource_PaddedArmor, WorkPriorityType.craftPaddedArmor, faction, city);
                    craft_heavypaddedarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HeavyPaddedArmor), SpriteName.WarsHammer, SpriteName.WarsResource_HeavyPaddedArmor, WorkPriorityType.craftHeavyPaddedArmor, faction, city);
                    craft_bronzearmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_BronzeArmor), SpriteName.WarsHammer, SpriteName.WarsResource_BronzeArmor, WorkPriorityType.craftBronzeArmor, faction, city);
                    craft_mailarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_IronArmor), SpriteName.WarsHammer, SpriteName.WarsResource_IronArmor, WorkPriorityType.craftMailArmor, faction, city);
                    craft_heavymailarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HeavyIronArmor), SpriteName.WarsHammer, SpriteName.WarsResource_HeavyIronArmor, WorkPriorityType.craftHeavyMailArmor, faction, city);
                    craft_platearmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_LightPlateArmor), SpriteName.WarsHammer, SpriteName.WarsResource_LightPlateArmor, WorkPriorityType.craftPlateArmor, faction, city);
                    craft_fullplatearmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_FullPlateArmor), SpriteName.WarsHammer, SpriteName.WarsResource_FullPlateArmor, WorkPriorityType.craftFullPlateArmor, faction, city);
                    craft_mithrilarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_MithrilArmor), SpriteName.WarsHammer, SpriteName.WarsResource_MithrilArmor, WorkPriorityType.craftMithrilArmor, faction, city);

                    break;
            } 
        
        }
    }

    struct WorkPriority
    {
        public static readonly WorkPriority Empty = new WorkPriority();

        public int value;
        public bool followFaction;
        public bool unlocked;
        //public bool safeguard;

        public WorkPriority(int defaultVal)//, bool safeguard)
        {
            followFaction = true;
            unlocked = true;
            value = defaultVal;
        }


        public void set(int value)
        { 
            this.value = value;
            followFaction = false;
        }

        public void onFactionValueChange(WorkPriority factionTemplate)
        {
            if (followFaction && unlocked)
            {
                value = factionTemplate.value;
            }
        }

        public void toHud(Players.LocalPlayer player, RichBoxContent content, string name, SpriteName sprite1, SpriteName sprite2, WorkPriorityType priorityType, Faction faction, City city)//, bool allowSafeGuard)
        {
            content.newLine();
            var infoContent = new List<AbsRichBoxMember>(2);
            infoContent.Add(new RichBoxImage(sprite1));
            if (sprite2 != SpriteName.NO_IMAGE)
            {
                infoContent.Add(new RichBoxImage(sprite2));
            }
            var infoButton = new RichboxButton(infoContent, null, new RbAction(() =>
            {
                RichBoxContent content = new RichBoxContent();
                content.Add(new RichBoxText(name));
                player.hud.tooltip.create(player, content, true);
            }));
            infoButton.overrideBgColor = HudLib.InfoYellow_BG;

            content.Add(infoButton);
            content.Add(new RichBoxTab(0.2f));

            if (unlocked)
            {
                if (city != null)
                {
                    HudLib.FollowFactionButton(followFaction,
                        faction.workTemplate.GetWorkPriority(priorityType).value,
                        new RbAction2Arg<WorkPriorityType, City>(faction.workFollowFactionClick, priorityType, city, followFaction ? SoundLib.menuBack : SoundLib.menu),
                        player, content);
                }

                for (int prio = 0; prio <= WorkTemplate.MaxPrio; prio++)
                {
                    content.space();

                    string prioText = null;
                    switch (prio)
                    {
                        case WorkTemplate.NoPrio:
                            prioText = DssRef.lang.Work_OrderPrio_No;
                            break;

                        case WorkTemplate.MinPrio:
                            prioText = DssRef.lang.Work_OrderPrio_Min;
                            break;

                        case WorkTemplate.MaxPrio:
                            prioText = DssRef.lang.Work_OrderPrio_Max;
                            break;
                    }

                    AbsRbAction hover = null;
                    if (prioText != null)
                    {
                        hover = new RbAction(() =>
                        {
                            RichBoxContent content = new RichBoxContent();
                            content.text(prioText);

                            player.hud.tooltip.create(player, content, true);
                        });
                    }

                    var button = new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxText(prio.ToString())
                },
                    new RbAction3Arg<int, WorkPriorityType, City>(faction.setWorkPrio, prio, priorityType, city, SoundLib.menu),
                    hover);
                    button.setGroupSelectionColor(HudLib.RbSettings, prio == value);
                    content.Add(button);

                }
            }
            else
            {
                content.Add(new RichBoxImage(SpriteName.birdLock));
            }
        }
        public void writeGameState(System.IO.BinaryWriter w, bool isCity)
        {

            if (isCity)
            {
                EightBit eightBit = new EightBit(followFaction, false);
                eightBit.write(w);

                if (!followFaction)
                {
                    w.Write((byte)value);
                }
            }
            else
            {
                w.Write((byte)value);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            
            if (isCity)
            {
                EightBit eightBit = new EightBit(r);
                followFaction = eightBit.Get(0);

                if (!followFaction)
                {
                    value = r.ReadByte();
                }
            }
            else
            {
                value = r.ReadByte();
            }
          
        }

        public void addPrio(int add)
        {
            followFaction = false;
            value = Bound.Set(value + add, 0, WorkTemplate.MaxPrio);
        }

        public void addPrio_belowMax(int add)
        {
            followFaction = false;
            value = Bound.Set(value + add, 0, WorkTemplate.MaxPrio - 1);
        }

        public bool HasPrio()
        {
            return value > WorkTemplate.NoPrio;
        }
    }

    enum WorkPriorityType
    {
        move,
        wood,
        stone,
        craftFuel,
        craftFood,
        craftBeer,
        craftCoolingFluid,

        smeltIron,
        smeltTin,
        smeltCopper,
        smeltLead,
        smeltSilver,
        craftBronze,
        craftCastIron,
        craftBloomeryIron,
        craftSteel,
        craftMithril,
        craftToolkit,
        craftWagonLight,
        craftWagonHeavy,
        craftBlackPowder,
        craftGunPowder,
        craftBullet,

        craftSharpStick,
        craftBronzeSword,
        craftShortSword,
        craftSword,
        craftLongSword,
        craftHandSpear,

        craftWarhammer,
        craftTwoHandSword,
        craftKnightsLance,
        craftMithrilSword,

        craftSlingshot,
        craftThrowingspear,
        craftBow,
        craftLongbow,
        craftCrossbow,
        craftMithrilbow,

        craftHandCannon,
        craftHandCulverin,
        craftRifle,
        craftBlunderbus,

        craftBallista,
        craftManuBallista,
        craftCatapult,
        craftBatteringRam,
        craftSiegeCannonBronze,
        craftManCannonBronze,
        craftSiegeCannonIron,
        craftManCannonIron,

        craftPaddedArmor,
        craftHeavyPaddedArmor,
        craftBronzeArmor,
        craftMailArmor,
        craftHeavyMailArmor,
        craftPlateArmor,
        craftFullPlateArmor,
        craftMithrilArmor,

        farmfood,
        farmfuel,
        farmlinen,
        bogiron,
        miningIron,
        miningTin,
        miningCopper,
        miningLead,
        miningSilver,
        miningGold,
        miningMithril,
        miningSulfur,
        miningCoal,

        trading,
        autoBuild,
        //expandFarms,

        coinmaker_cupper,
        coinmaker_bronze,
        coinmaker_silver,
        coinmaker_mithril,

        NUM_NONE
    }

}

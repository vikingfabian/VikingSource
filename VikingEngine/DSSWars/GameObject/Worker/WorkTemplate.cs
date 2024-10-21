
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Security.AccessControl;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    struct WorkTemplate
    {
        public const int NoPrio = 0;
        public const int MinPrio = 1;
        public const int MaxPrio = 5;
        public const int SafeGuardPrio = MaxPrio + 1;

        public WorkPriority move = new WorkPriority(3, false);
        public WorkPriority wood = new WorkPriority(2, true);
        public WorkPriority stone = new WorkPriority(2, false);
        public WorkPriority craft_fuel = new WorkPriority(4, true);
        public WorkPriority craft_food = new WorkPriority(4, true);
        public WorkPriority craft_beer = new WorkPriority(1, false);
        public WorkPriority craft_iron = new WorkPriority(3, false);
        public WorkPriority craft_sharpstick = new WorkPriority(1, false);
        public WorkPriority craft_sword = new WorkPriority(0, false);
        public WorkPriority craft_twohandsword = new WorkPriority(0, false);
        public WorkPriority craft_knightslance = new WorkPriority(0, false);
        public WorkPriority craft_bow = new WorkPriority(0, false);
        public WorkPriority craft_longbow = new WorkPriority(0, false);
        public WorkPriority craft_ballista= new WorkPriority(0, false);
        public WorkPriority craft_lightarmor = new WorkPriority(1, false);
        public WorkPriority craft_mediumarmor = new WorkPriority(0, false);
        public WorkPriority craft_heavyarmor = new WorkPriority(0, false);
        public WorkPriority farming = new WorkPriority(2, true);
        public WorkPriority bogiron = new WorkPriority(1, false);
        public WorkPriority mining = new WorkPriority(3, false);
        public WorkPriority trading = new WorkPriority(2, false);
        public WorkPriority autoBuild = new WorkPriority(1, false);

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
            craft_iron.writeGameState(w, isCity);
            craft_sharpstick.writeGameState(w, isCity);
            craft_sword.writeGameState(w, isCity);
            craft_bow.writeGameState(w, isCity);

            craft_twohandsword.writeGameState(w, isCity);
            craft_knightslance.writeGameState(w, isCity);            
            craft_ballista.writeGameState(w, isCity);

            craft_lightarmor.writeGameState(w, isCity);
            craft_mediumarmor.writeGameState(w, isCity);
            craft_heavyarmor.writeGameState(w, isCity);
            farming.writeGameState(w, isCity);
            mining.writeGameState(w, isCity);
            trading.writeGameState(w, isCity);
            autoBuild.writeGameState(w, isCity);

            bogiron.writeGameState(w, isCity);
            craft_longbow.writeGameState(w, isCity);

        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            move.readGameState(r, subversion, isCity);
            wood.readGameState(r, subversion, isCity);
            stone.readGameState(r, subversion, isCity);
            craft_fuel.readGameState(r, subversion, isCity);
            craft_food.readGameState(r, subversion, isCity);
            craft_beer.readGameState(r, subversion, isCity);
            craft_iron.readGameState(r, subversion, isCity);
            craft_sharpstick.readGameState(r, subversion, isCity);
            craft_sword.readGameState(r, subversion, isCity);
            craft_bow.readGameState(r, subversion, isCity);
            
            if (subversion >= 13)
            {
                craft_twohandsword.readGameState(r, subversion, isCity);
                craft_knightslance.readGameState(r, subversion, isCity);
                craft_ballista.readGameState(r, subversion, isCity);
            }
            craft_lightarmor.readGameState(r, subversion, isCity);
            craft_mediumarmor.readGameState(r, subversion, isCity);
            craft_heavyarmor.readGameState(r, subversion, isCity);
            farming.readGameState(r, subversion, isCity);
            mining.readGameState(r, subversion, isCity);
            trading.readGameState(r, subversion, isCity);
            autoBuild.readGameState(r, subversion, isCity);
            
            if (subversion >= 18)
            {
                bogiron.readGameState(r, subversion, isCity);
                craft_longbow.readGameState(r, subversion, isCity);
            }

        }

        public void onFactionChange(WorkTemplate factionTemplate)
        {
            move.onFactionValueChange(factionTemplate.move);
            wood.onFactionValueChange(factionTemplate.wood);
            stone.onFactionValueChange(factionTemplate.stone);
            craft_fuel.onFactionValueChange(factionTemplate.craft_fuel);
            craft_food.onFactionValueChange(factionTemplate.craft_food);
            craft_beer.onFactionValueChange(factionTemplate.craft_beer);
            craft_iron.onFactionValueChange(factionTemplate.craft_iron);

            craft_sharpstick.onFactionValueChange(factionTemplate.craft_sharpstick);
            craft_sword.onFactionValueChange(factionTemplate.craft_sword);
            craft_twohandsword.onFactionValueChange(factionTemplate.craft_twohandsword);
            craft_knightslance.onFactionValueChange(factionTemplate.craft_knightslance);
            craft_bow.onFactionValueChange(factionTemplate.craft_bow);
            craft_longbow.onFactionValueChange(factionTemplate.craft_longbow);
            craft_ballista.onFactionValueChange(factionTemplate.craft_ballista);

            craft_lightarmor.onFactionValueChange(factionTemplate.craft_lightarmor);
            craft_mediumarmor.onFactionValueChange(factionTemplate.craft_mediumarmor);
            craft_heavyarmor.onFactionValueChange(factionTemplate.craft_heavyarmor);

            farming.onFactionValueChange(factionTemplate.farming);
            bogiron.onFactionValueChange(factionTemplate.bogiron);
            mining.onFactionValueChange(factionTemplate.mining);
            trading.onFactionValueChange(factionTemplate.trading);
            autoBuild.onFactionValueChange(factionTemplate.autoBuild);
        }

        public void setAllToFollowFaction()
        {
            move.followFaction = true;

            wood.followFaction = true;
            stone.followFaction = true;
            craft_fuel.followFaction = true;
            craft_food.followFaction = true;
            craft_beer.followFaction = true;
            craft_iron.followFaction = true;

            craft_sharpstick.followFaction = true;
            craft_sword.followFaction = true;
            craft_twohandsword.followFaction = true;
            craft_knightslance.followFaction = true;
            craft_bow.followFaction = true;
            craft_longbow.followFaction = true;
            craft_ballista.followFaction = true;

            craft_lightarmor.followFaction = true;
            craft_mediumarmor.followFaction = true;
            craft_heavyarmor.followFaction = true;

            farming.followFaction = true;
            bogiron.followFaction = true;
            mining.followFaction = true;
            trading.followFaction = true;
            autoBuild.followFaction = true;
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
                case ItemResourceType.Iron_G:
                    return craft_iron;

                case ItemResourceType.LightArmor: return craft_lightarmor;
                case ItemResourceType.MediumArmor: return craft_mediumarmor;
                case ItemResourceType.HeavyArmor: return craft_heavyarmor;

                case ItemResourceType.SharpStick: return craft_sharpstick;
                case ItemResourceType.Sword: return craft_sword;
                case ItemResourceType.TwoHandSword: return craft_twohandsword;
                case ItemResourceType.KnightsLance: return craft_knightslance;
                case ItemResourceType.Bow: return craft_bow;
                case ItemResourceType.LongBow: return craft_longbow;
                case ItemResourceType.Ballista: return craft_ballista;

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
                case WorkPriorityType.craftIron:
                    return craft_iron;
                case WorkPriorityType.craftSharpStick:
                    return craft_sharpstick;
                case WorkPriorityType.craftSword:
                    return craft_sword;
                case WorkPriorityType.craftTwoHandSword:
                    return craft_twohandsword;
                case WorkPriorityType.craftKnightsLance:
                    return craft_knightslance;
                case WorkPriorityType.craftBow:
                    return craft_bow;
                case WorkPriorityType.craftLongbow:
                    return craft_longbow;
                case WorkPriorityType.craftBallista:
                    return craft_ballista;
                case WorkPriorityType.craftLightArmor:
                    return craft_lightarmor;
                case WorkPriorityType.craftMediumArmor:
                    return craft_mediumarmor;
                case WorkPriorityType.craftHeavyArmor:
                    return craft_heavyarmor;
                case WorkPriorityType.farming:
                    return farming;
                case WorkPriorityType.bogiron:
                    return bogiron;
                case WorkPriorityType.mining:
                   return mining;
                case WorkPriorityType.trading:
                    return trading;
                case WorkPriorityType.autoBuild:
                    return autoBuild;
                //case WorkPriorityType.expandFarms:
                //    return expand_farms;

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
                case WorkPriorityType.craftIron:
                    craft_iron = value;
                    break;
                case WorkPriorityType.craftSharpStick:
                    craft_sharpstick = value;
                    break;
                case WorkPriorityType.craftSword:
                    craft_sword = value;
                    break;
                case WorkPriorityType.craftTwoHandSword:
                    craft_twohandsword = value;
                    break;
                case WorkPriorityType.craftKnightsLance:
                    craft_knightslance = value;
                    break;
                case WorkPriorityType.craftBow:
                    craft_bow = value;
                    break;
                case WorkPriorityType.craftLongbow:
                    craft_longbow = value;
                    break;
                case WorkPriorityType.craftBallista:
                    craft_ballista = value;
                    break;
                case WorkPriorityType.craftLightArmor:
                    craft_lightarmor = value;
                    break;
                case WorkPriorityType.craftMediumArmor:
                    craft_mediumarmor = value;
                    break;
                case WorkPriorityType.craftHeavyArmor:
                    craft_heavyarmor = value;
                    break;
                case WorkPriorityType.farming:
                    farming = value;
                    break;
                case WorkPriorityType.mining:
                    mining = value;
                    break;
                case WorkPriorityType.bogiron:
                    bogiron = value;
                    break;
                case WorkPriorityType.trading:
                    trading = value;
                    break;
                case WorkPriorityType.autoBuild:
                    autoBuild = value;
                    break;


                default:
                    throw new NotImplementedException();
            }
        }

        public void toHud(Players.LocalPlayer player, RichBoxContent content, Faction faction, City city)
        {
            content.h2(DssRef.lang.Work_OrderPrioTitle).overrideColor = HudLib.TitleColor_Label;
            
            move.toHud(player, content, DssRef.lang.Work_Move, SpriteName.WarsWorkMove, SpriteName.WarsBuild_Storehouse, WorkPriorityType.move, faction, city);
            wood.toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Wood), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Wood, WorkPriorityType.wood, faction, city);
            stone.toHud(player, content, string.Format(DssRef.lang.Work_GatherXResource, DssRef.lang.Resource_TypeName_Stone), SpriteName.WarsWorkCollect, SpriteName.WarsResource_Stone, WorkPriorityType.stone, faction, city);
            craft_food.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Food), SpriteName.WarsHammer, SpriteName.WarsResource_Food, WorkPriorityType.craftFood, faction, city);
            craft_fuel.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Fuel), SpriteName.WarsHammer, SpriteName.WarsResource_Fuel, WorkPriorityType.craftFuel, faction, city);
            craft_beer.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Beer), SpriteName.WarsHammer, SpriteName.WarsResource_Beer, WorkPriorityType.craftBeer, faction, city);
            craft_iron.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Iron), SpriteName.WarsHammer, SpriteName.WarsResource_Iron, WorkPriorityType.craftIron, faction, city);

            craft_sharpstick.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_SharpStick), SpriteName.WarsHammer, SpriteName.WarsResource_Sharpstick, WorkPriorityType.craftSharpStick, faction, city);
            craft_sword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Sword), SpriteName.WarsHammer, SpriteName.WarsResource_Sword, WorkPriorityType.craftSword, faction, city);
            craft_twohandsword.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_TwoHandSword), SpriteName.WarsHammer, SpriteName.WarsResource_TwoHandSword, WorkPriorityType.craftTwoHandSword, faction, city);
            craft_knightslance.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_KnightsLance), SpriteName.WarsHammer, SpriteName.WarsResource_KnightsLance, WorkPriorityType.craftKnightsLance, faction, city);
            craft_bow.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_Bow), SpriteName.WarsHammer, SpriteName.WarsResource_Bow, WorkPriorityType.craftBow, faction, city);
            craft_longbow.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Longbow), SpriteName.WarsHammer, SpriteName.WarsResource_Longbow, WorkPriorityType.craftBow, faction, city);
            craft_ballista.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.UnitType_Ballista), SpriteName.WarsHammer, SpriteName.WarsResource_Ballista, WorkPriorityType.craftBallista, faction, city);

            craft_lightarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_LightArmor), SpriteName.WarsHammer, SpriteName.WarsResource_LightArmor, WorkPriorityType.craftLightArmor, faction, city);
            craft_mediumarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_MediumArmor), SpriteName.WarsHammer, SpriteName.WarsResource_MediumArmor, WorkPriorityType.craftMediumArmor, faction, city);
            craft_heavyarmor.toHud(player, content, string.Format(DssRef.lang.Work_CraftX, DssRef.lang.Resource_TypeName_HeavyArmor), SpriteName.WarsHammer, SpriteName.WarsResource_HeavyArmor, WorkPriorityType.craftHeavyArmor, faction, city);

            farming.toHud(player, content, DssRef.lang.Work_Farming, SpriteName.WarsWorkFarm, SpriteName.WarsResource_RawFood, WorkPriorityType.farming, faction, city);
            bogiron.toHud(player, content, DssRef.lang.Resource_TypeName_BogIron, SpriteName.WarsWorkCollect, SpriteName.WarsResource_IronOre, WorkPriorityType.bogiron, faction, city);
            mining.toHud(player, content, DssRef.lang.Work_Mining, SpriteName.WarsWorkMine, SpriteName.NO_IMAGE, WorkPriorityType.mining, faction, city);
            autoBuild.toHud(player, content, DssRef.lang.Work_AutoBuild, SpriteName.MenuPixelIconSettings, SpriteName.NO_IMAGE, WorkPriorityType.autoBuild, faction, city);
            //SpriteName.MenuPixelIconSettings

            HudLib.Description( content, string.Format(DssRef.lang.Work_OrderPrioDescription, MaxPrio));
        }

        
    }

    struct WorkPriority
    {
        public static readonly WorkPriority Empty = new WorkPriority();

        public int value;
        public bool followFaction;
        //public bool safeguard;

        public WorkPriority(int defaultVal)//, bool safeguard)
        {
            followFaction = true;
            //this.safeguard = safeguard;
            value = defaultVal; 
        }

        public void onFactionValueChange(WorkPriority factionTemplate)
        {
            if (followFaction)
            {
                value = factionTemplate.value;
              //  safeguard = factionTemplate.safeguard;
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

            if (city != null)
            {
                HudLib.FollowFactionButton(followFaction,
                    faction.workTemplate.GetWorkPriority(priorityType).value,
                    new RbAction2Arg<WorkPriorityType, City>(faction.workFollowFactionClick, priorityType, city),
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
                new RbAction3Arg<int, WorkPriorityType, City>(faction.setWorkPrio, prio, priorityType, city),
                hover);
                button.setGroupSelectionColor(HudLib.RbSettings, prio == value);
                content.Add(button);

            }

            //if (allowSafeGuard)
            //{
            //    content.space();
            //    content.Add(new RichboxButton(new List<AbsRichBoxMember> {
            //        new RichBoxImage(safeguard? SpriteName.WarsProtectedStockpileOn : SpriteName.WarsProtectedStockpileOff),
            //    },
            //    new RbAction3Arg<bool, WorkPriorityType, City>(faction.setWorkPrioSafeGuard, !safeguard, priorityType, city),
            //    player.WorkSafeguardTooltip));

            //}
        }
        public void writeGameState(System.IO.BinaryWriter w, bool isCity)
        {
            w.Write((byte)value);
            if (isCity)
            {
                EightBit eightBit = new EightBit(followFaction, false);
                eightBit.write(w);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            value = r.ReadByte();
            if (isCity)
            {
                if (subversion < 20)
                {//old
                    followFaction = r.ReadBoolean();
                }
                else
                {
                    EightBit eightBit = new EightBit(r);
                    followFaction = eightBit.Get(0);
                }
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
            value = Bound.Set(value + add, 0, WorkTemplate.MaxPrio -1);
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

        craftIron,

        craftSharpStick,
        craftSword,
        craftBow,
        craftLongbow,
        craftBallista,

        craftLightArmor,
        craftMediumArmor,
        craftHeavyArmor,

        farming,
        bogiron,
        mining,
        trading,
        autoBuild,
        expandFarms,
        craftTwoHandSword,
        craftKnightsLance,
        NUM
    }
}

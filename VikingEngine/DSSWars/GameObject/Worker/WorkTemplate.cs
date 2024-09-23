
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.AccessControl;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    struct WorkTemplate
    {
        public const int NoPrio = 0;
        const int MinPrio = 1;
        public const int MaxPrio = 5;

        public WorkPriority move = new WorkPriority(3);
        public WorkPriority wood = new WorkPriority(2);
        public WorkPriority stone = new WorkPriority(2);
        public WorkPriority craft_food = new WorkPriority(4);
        public WorkPriority craft_beer = new WorkPriority(1);
        public WorkPriority craft_iron = new WorkPriority(3);
        public WorkPriority craft_sharpstick = new WorkPriority(1);
        public WorkPriority craft_sword = new WorkPriority(0);
        public WorkPriority craft_bow = new WorkPriority(0);
        public WorkPriority craft_lightarmor = new WorkPriority(1);
        public WorkPriority craft_mediumarmor = new WorkPriority(0);
        public WorkPriority craft_heavyarmor = new WorkPriority(0);
        public WorkPriority farming = new WorkPriority(2);
        public WorkPriority mining = new WorkPriority(2);
        public WorkPriority trading = new WorkPriority(2);
        public WorkPriority autoBuild = new WorkPriority(1);

        public WorkTemplate()
        {
        }

        public void writeGameState(System.IO.BinaryWriter w, bool isCity)
        {
            move.writeGameState(w, isCity);
            wood.writeGameState(w, isCity);
            stone.writeGameState(w, isCity);
            craft_food.writeGameState(w, isCity);
            craft_beer.writeGameState(w, isCity);
            craft_iron.writeGameState(w, isCity);
            craft_sharpstick.writeGameState(w, isCity);
            craft_sword.writeGameState(w, isCity);
            craft_bow.writeGameState(w, isCity);
            craft_lightarmor.writeGameState(w, isCity);
            craft_mediumarmor.writeGameState(w, isCity);
            craft_heavyarmor.writeGameState(w, isCity);
            farming.writeGameState(w, isCity);
            mining.writeGameState(w, isCity);
            trading.writeGameState(w, isCity);
            autoBuild.writeGameState(w, isCity);

        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            move.readGameState(r, subversion, isCity);
            wood.readGameState(r, subversion, isCity);
            stone.readGameState(r, subversion, isCity);
            craft_food.readGameState(r, subversion, isCity);
            craft_beer.readGameState(r, subversion, isCity);
            craft_iron.readGameState(r, subversion, isCity);
            craft_sharpstick.readGameState(r, subversion, isCity);
            craft_sword.readGameState(r, subversion, isCity);
            craft_bow.readGameState(r, subversion, isCity);
            craft_lightarmor.readGameState(r, subversion, isCity);
            craft_mediumarmor.readGameState(r, subversion, isCity);
            craft_heavyarmor.readGameState(r, subversion, isCity);
            farming.readGameState(r, subversion, isCity);
            mining.readGameState(r, subversion, isCity);
            trading.readGameState(r, subversion, isCity);
            autoBuild.readGameState(r, subversion, isCity);
        }

        public void onFactionChange(WorkTemplate factionTemplate)
        {
            move.onFactionValueChange(factionTemplate.move);
            wood.onFactionValueChange(factionTemplate.wood);
            stone.onFactionValueChange(factionTemplate.stone);
            craft_food.onFactionValueChange(factionTemplate.craft_food);
            craft_beer.onFactionValueChange(factionTemplate.craft_food);
            craft_iron.onFactionValueChange(factionTemplate.craft_iron);

            craft_sharpstick.onFactionValueChange(factionTemplate.craft_sharpstick);
            craft_sword.onFactionValueChange(factionTemplate.craft_sword);
            craft_bow.onFactionValueChange(factionTemplate.craft_bow);

            craft_lightarmor.onFactionValueChange(factionTemplate.craft_lightarmor);
            craft_mediumarmor.onFactionValueChange(factionTemplate.craft_mediumarmor);
            craft_heavyarmor.onFactionValueChange(factionTemplate.craft_heavyarmor);

            farming.onFactionValueChange(factionTemplate.farming);
            mining.onFactionValueChange(factionTemplate.mining);
            trading.onFactionValueChange(factionTemplate.trading);
            autoBuild.onFactionValueChange(factionTemplate.autoBuild);
        }

        public void setWorkPrio(int set, WorkPriorityType priorityType)
        {
            var work = GetWorkPriority(priorityType);
            work.value = set;//Bound.Set(work.value + set, NoPrio, MaxPrio);
            work.followFaction = false;
            SetWorkPriority(priorityType, work);
        }

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
                case ItemResourceType.Food_G:
                    return craft_food;
                case ItemResourceType.Iron_G:
                    return craft_iron;

                case ItemResourceType.LightArmor: return craft_lightarmor;
                case ItemResourceType.MediumArmor: return craft_mediumarmor;
                case ItemResourceType.HeavyArmor: return craft_heavyarmor;

                case ItemResourceType.SharpStick: return craft_sharpstick;
                case ItemResourceType.Sword: return craft_sword;
                case ItemResourceType.Bow: return craft_bow;

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
                case WorkPriorityType.craftBow:
                    return craft_bow;
                case WorkPriorityType.craftLightArmor:
                    return craft_lightarmor;
                case WorkPriorityType.craftMediumArmor:
                    return craft_mediumarmor;
                case WorkPriorityType.craftHeavyArmor:
                    return craft_heavyarmor;
                case WorkPriorityType.farming:
                    return farming;
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
                case WorkPriorityType.craftBow:
                    craft_bow = value;
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
            content.h2(DssRef.todoLang.Work_OrderPrioTitle);
            
            move.toHud(player, content, DssRef.todoLang.Work_Move, WorkPriorityType.move, faction, city);
            wood.toHud(player, content, string.Format(DssRef.todoLang.Work_GatherXResource, DssRef.todoLang.Resource_TypeName_Wood), WorkPriorityType.wood, faction, city);
            stone.toHud(player, content, string.Format(DssRef.todoLang.Work_GatherXResource, DssRef.todoLang.Resource_TypeName_Stone), WorkPriorityType.stone, faction, city);
            craft_food.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Food), WorkPriorityType.craftFood, faction, city);
            craft_beer.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Beer), WorkPriorityType.craftBeer, faction, city);
            craft_iron.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Iron), WorkPriorityType.craftIron, faction, city);

            craft_sharpstick.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_SharpStick), WorkPriorityType.craftSharpStick, faction, city);
            craft_sword.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Sword), WorkPriorityType.craftSword, faction, city);
            craft_bow.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Bow), WorkPriorityType.craftBow, faction, city);
            
            craft_lightarmor.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_LightArmor), WorkPriorityType.craftLightArmor, faction, city);
            craft_mediumarmor.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_MediumArmor), WorkPriorityType.craftMediumArmor, faction, city);
            craft_heavyarmor.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HeavyArmor), WorkPriorityType.craftHeavyArmor, faction, city);

            farming.toHud(player, content, DssRef.todoLang.Work_Farming, WorkPriorityType.farming, faction, city);
            mining.toHud(player, content, DssRef.todoLang.Work_Mining, WorkPriorityType.mining, faction, city);
            //trading.toHud(player, content, DssRef.todoLang.Work_Trading, WorkPriorityType.trading, faction, city);
            autoBuild.toHud(player, content, DssRef.todoLang.Work_AutoBuild, WorkPriorityType.autoBuild, faction, city);
            //expand_farms.toHud(player, content, DssRef.todoLang.Work_ExpandFarms, WorkPriorityType.expandFarms, faction, city);

            HudLib.Description( content, string.Format(DssRef.todoLang.Work_OrderPrioDescription, MaxPrio));
        }

        
    }

    struct WorkPriority
    {
        public static readonly WorkPriority Empty = new WorkPriority();

        public int value;
        public bool followFaction;

        public WorkPriority(int defaultVal)
        {
            followFaction = true;
            value = defaultVal; 
        }

        public void onFactionValueChange(WorkPriority factionTemplate)
        {
            if (followFaction)
            {
                value = factionTemplate.value;
            }
        }

        public void toHud(Players.LocalPlayer player, RichBoxContent content, string name, WorkPriorityType priorityType, Faction faction, City city)
        {
            content.newLine();
            content.Add(new RichBoxText(name));
            content.Add(new RichBoxTab(0.4f));
            

            if (city != null)
            {
                //var followFactionButton = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(followFaction ? "=F" : "!F") },
                //        new RbAction2Arg<WorkPriorityType, City>(faction.workFollowFactionClick, priorityType, city));
                //if (!followFaction)
                //{
                //    followFactionButton.overrideBgColor = Color.DarkOrange;
                //}
                //content.Add(followFactionButton);
                HudLib.FollowFactionButton(followFaction,
                    faction.workTemplate.GetWorkPriority(priorityType).value,
                    new RbAction2Arg<WorkPriorityType, City>(faction.workFollowFactionClick, priorityType, city),
                    player,content);

                //content.space();
            }

            for (int prio = 0; prio <= WorkTemplate.MaxPrio; prio++)
            {
                content.space();
                var button =  new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(prio.ToString()) }, new RbAction3Arg<int, WorkPriorityType, City>(faction.setWorkPrio, prio, priorityType, city));
                button.setGroupSelectionColor(HudLib.RbSettings, prio == value);
                content.Add(button);

            }

            //{
            //    int change = -1;
            //    content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
            //            new RbAction3Arg<int, WorkPriorityType, City>(faction.changeWorkPrio, change, priorityType, city)));

            //    content.space();
            //}

            //content.Add(new RichBoxText(value.ToString()));

            //content.space();

            //{
            //    int change = 1;
            //    content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
            //            new RbAction3Arg<int, WorkPriorityType, City>(faction.changeWorkPrio, change, priorityType, city)));

            //    content.space();
            //}
        }

        public void writeGameState(System.IO.BinaryWriter w, bool isCity)
        {
            w.Write((byte)value);
            if (isCity)
            {
                w.Write(followFaction);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, bool isCity)
        {
            value = r.ReadByte();
            if (isCity)
            {
                followFaction = r.ReadBoolean();
            }
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
        craftFood,
        craftBeer,

        craftIron,

        craftSharpStick,
        craftSword,
        craftBow,

        craftLightArmor,
        craftMediumArmor,
        craftHeavyArmor,

        farming,
        mining,
        trading,
        autoBuild,
        expandFarms,
        NUM
    }
}

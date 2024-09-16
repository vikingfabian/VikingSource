
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    struct WorkTemplate
    {
        public const int NoPrio = 0;
        const int MinPrio = 1;
        const int MaxPrio = 10;

        public WorkPriority move = new WorkPriority(6);
        public WorkPriority wood = new WorkPriority(4);
        public WorkPriority stone = new WorkPriority(4);
        public WorkPriority craft_food = new WorkPriority(8);
        public WorkPriority craft_iron = new WorkPriority(6);
        public WorkPriority craft_sharpstick = new WorkPriority(1);
        public WorkPriority craft_sword = new WorkPriority(0);
        public WorkPriority craft_bow = new WorkPriority(0);
        public WorkPriority craft_lightarmor = new WorkPriority(1);
        public WorkPriority craft_mediumarmor = new WorkPriority(0);
        public WorkPriority craft_heavyarmor = new WorkPriority(0);
        public WorkPriority farming = new WorkPriority(5);
        public WorkPriority mining = new WorkPriority(5);
        public WorkPriority trading = new WorkPriority(5);
        public WorkPriority expand_housing = new WorkPriority(3);
        public WorkPriority expand_farms = new WorkPriority(3);

        public WorkTemplate()
        {
        }

        public void onFactionChange(WorkTemplate factionTemplate)
        {
            move.onFactionValueChange(factionTemplate.move);
            wood.onFactionValueChange(factionTemplate.wood);
            stone.onFactionValueChange(factionTemplate.stone);
            craft_food.onFactionValueChange(factionTemplate.craft_food);
            craft_iron.onFactionValueChange(factionTemplate.craft_iron);

            craft_sharpstick.onFactionValueChange(factionTemplate.craft_food);
            craft_sword.onFactionValueChange(factionTemplate.craft_food);
            craft_bow.onFactionValueChange(factionTemplate.craft_food);

            craft_lightarmor.onFactionValueChange(factionTemplate.craft_food);
            craft_mediumarmor.onFactionValueChange(factionTemplate.craft_food);
            craft_heavyarmor.onFactionValueChange(factionTemplate.craft_food);

            farming.onFactionValueChange(factionTemplate.farming);
            mining.onFactionValueChange(factionTemplate.mining);
            trading.onFactionValueChange(factionTemplate.trading);
            expand_housing.onFactionValueChange(factionTemplate.expand_housing);
            expand_farms.onFactionValueChange(factionTemplate.expand_farms);
        }

        public void changeWorkPrio(int change, WorkPriorityType priorityType)
        {
            var work = GetWorkPriority(priorityType);
            work.value = Bound.Set(work.value + change, NoPrio, MaxPrio);
            work.followFaction = false;
            SetWorkPriority(priorityType, work);
            //switch (priorityType)
            //{
            //    case WorkPriorityType.move:
            //        move.value = Bound.Set(move.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.wood:
            //        wood.value = Bound.Set(wood.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.stone:
            //        stone.value = Bound.Set(stone.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.craftFood:
            //        craft_food.value = Bound.Set(craft_food.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.craftIron:
            //        craft_iron.value = Bound.Set(craft_iron.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.farming:
            //        farming.value = Bound.Set(farming.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.mining:
            //        mining.value = Bound.Set(mining.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.trading:
            //        trading.value = Bound.Set(trading.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.expandHousing:
            //        expand_housing.value = Bound.Set(expand_housing.value + change, MinPrio, MaxPrio);
            //        break;
            //    case WorkPriorityType.expandFarms:
            //        expand_farms.value = Bound.Set(expand_farms.value + change, MinPrio, MaxPrio);
            //        break;

            //}
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
                case WorkPriorityType.craftIron:
                    return craft_iron;
                case WorkPriorityType.farming:
                    return farming;
                case WorkPriorityType.mining:
                   return mining;
                case WorkPriorityType.trading:
                    return trading;
                case WorkPriorityType.expandHousing:
                    return expand_housing;
                case WorkPriorityType.expandFarms:
                    return expand_farms;

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
                case WorkPriorityType.craftIron:
                    craft_iron = value;
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
                case WorkPriorityType.expandHousing:
                    expand_housing = value;
                    break;
                case WorkPriorityType.expandFarms:
                    expand_farms = value;
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
            craft_iron.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Iron), WorkPriorityType.craftIron, faction, city);

            craft_sharpstick.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_SharpStick), WorkPriorityType.craftSharpStick, faction, city);
            craft_sword.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Sword), WorkPriorityType.craftSword, faction, city);
            craft_bow.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Bow), WorkPriorityType.craftBow, faction, city);
            
            craft_lightarmor.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_LightArmor), WorkPriorityType.craftLightArmor, faction, city);
            craft_mediumarmor.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_MediumArmor), WorkPriorityType.craftMediumArmor, faction, city);
            craft_heavyarmor.toHud(player, content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_HeavyArmor), WorkPriorityType.craftHeavyArmor, faction, city);

            farming.toHud(player, content, DssRef.todoLang.Work_Farming, WorkPriorityType.farming, faction, city);
            mining.toHud(player, content, DssRef.todoLang.Work_Mining, WorkPriorityType.mining, faction, city);
            trading.toHud(player, content, DssRef.todoLang.Work_Trading, WorkPriorityType.trading, faction, city);
            expand_housing.toHud(player, content, DssRef.todoLang.Work_ExpandHousing, WorkPriorityType.expandHousing, faction, city);
            expand_farms.toHud(player, content, DssRef.todoLang.Work_ExpandFarms, WorkPriorityType.expandFarms, faction, city);

            content.text(DssRef.todoLang.Work_OrderPrioDescription).overrideColor = Color.DarkGray;
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
            content.Add(new RichBoxTab(0.5f));

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

            {
                int change = -1;
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                        new RbAction3Arg<int, WorkPriorityType, City>(faction.changeWorkPrio, change, priorityType, city)));

                content.space();
            }

            content.Add(new RichBoxText(value.ToString()));

            content.space();

            {
                int change = 1;
                content.Add(new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(TextLib.PlusMinus(change)) },
                        new RbAction3Arg<int, WorkPriorityType, City>(faction.changeWorkPrio, change, priorityType, city)));

                content.space();
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
        expandHousing,
        expandFarms,
        NUM
    }
}

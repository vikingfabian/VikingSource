
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    struct WorkTemplate
    {
        const int MinPrio = 1;
        const int MaxPrio = 10;

        public WorkPriority move = new WorkPriority(6);
        public WorkPriority wood = new WorkPriority(4);
        public WorkPriority stone = new WorkPriority(4);
        public WorkPriority craft_food = new WorkPriority(8);
        public WorkPriority craft_iron = new WorkPriority(6);
        public WorkPriority farming = new WorkPriority(5);
        public WorkPriority mining = new WorkPriority(5);
        public WorkPriority trading = new WorkPriority(5);
        public WorkPriority expand_housing = new WorkPriority(3);
        public WorkPriority expand_farms = new WorkPriority(3);

        public WorkTemplate() 
        { 
        }
        public void changeWorkPrio(int change, WorkPriorityType priorityType)
        {
            switch (priorityType)
            {
                case WorkPriorityType.move:
                    move.value = Bound.Set(move.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.wood:
                    wood.value = Bound.Set(wood.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.stone:
                    stone.value = Bound.Set(stone.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.craftFood:
                    craft_food.value = Bound.Set(craft_food.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.craftIron:
                    craft_iron.value = Bound.Set(craft_iron.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.farming:
                    farming.value = Bound.Set(farming.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.mining:
                    mining.value = Bound.Set(mining.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.trading:
                    trading.value = Bound.Set(trading.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.expandHousing:
                    expand_housing.value = Bound.Set(expand_housing.value + change, MinPrio, MaxPrio);
                    break;
                case WorkPriorityType.expandFarms:
                    expand_farms.value = Bound.Set(expand_farms.value + change, MinPrio, MaxPrio);
                    break;

            }
        }

        public void toHud(RichBoxContent content, Faction faction, City city)
        {
            content.h2(DssRef.todoLang.Work_OrderPrioTitle);
            
            move.toHud(content, DssRef.todoLang.Work_Move, WorkPriorityType.move, faction, city);
            wood.toHud(content, string.Format(DssRef.todoLang.Work_GatherXResource, DssRef.todoLang.Resource_TypeName_Wood), WorkPriorityType.wood, faction, city);
            stone.toHud(content, string.Format(DssRef.todoLang.Work_GatherXResource, DssRef.todoLang.Resource_TypeName_Stone), WorkPriorityType.stone, faction, city);
            craft_food.toHud(content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Food), WorkPriorityType.craftFood, faction, city);
            craft_iron.toHud(content, string.Format(DssRef.todoLang.Work_CraftX, DssRef.todoLang.Resource_TypeName_Iron), WorkPriorityType.craftIron, faction, city);
            farming.toHud(content, DssRef.todoLang.Work_Farming, WorkPriorityType.farming, faction, city);
            mining.toHud(content, DssRef.todoLang.Work_Mining, WorkPriorityType.mining, faction, city);
            trading.toHud(content, DssRef.todoLang.Work_Trading, WorkPriorityType.trading, faction, city);
            expand_housing.toHud(content, DssRef.todoLang.Work_ExpandHousing, WorkPriorityType.expandHousing, faction, city);
            expand_farms.toHud(content, DssRef.todoLang.Work_ExpandFarms, WorkPriorityType.expandFarms, faction, city);

            content.text(DssRef.todoLang.Work_OrderPrioDescription).overrideColor = Color.DarkGray;
        }

        
    }

    struct WorkPriority
    {
        public int value;

        public WorkPriority(int defaultVal)
        { value = defaultVal; }

        public void toHud(RichBoxContent content, string name, WorkPriorityType priorityType, Faction faction, City city)
        {
            content.newLine();
            content.Add(new RichBoxText(name));
            content.Add(new RichBoxTab(0.5f));

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
    }

    enum WorkPriorityType
    {
        move,
        wood,
        stone,
        craftFood,
        craftIron,
        farming,
        mining,
        trading,
        expandHousing,
        expandFarms,

    }
}

using MonoGame.Framework.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Interface.MapObjMenu
{
    class MovingGroupsCollection
    {   
        public MovingGroups mainArmy;
        public List<MovingGroups> otherArmies;

        public MovingGroupsCollection(AbsArmy fromArmy)
        {
            mainArmy = new MovingGroups(fromArmy);
            otherArmies = new List<MovingGroups> { new MovingGroups(null) };
        }
    }

    class MovingGroups
    {
        /// <summary>
        /// Null if new army
        /// </summary>
        public AbsArmy fromArmy;
        public HashSet<SoldierGroup> groups;

        public MovingGroups(AbsArmy fromArmy)
        {
            this.fromArmy = fromArmy;
            if (fromArmy != null)
            {
                this.groups = new HashSet<SoldierGroup>(16);
            }
        }

        public void AddGroup(SoldierGroupAndCount group, bool moveAll)
        {
            if (moveAll)
            {
                var groupsC = fromArmy.groups.counter();
                while (groupsC.Next())
                {
                    if (groupsC.sel.soldierConscript.conscript.SortOrderValue() == group.sortId)
                    { 
                        groups.Add(groupsC.sel);
                    }
                }
            }
            else
            { 
                groups.Add(group.group);
            }
        }
    }
    class SoldierGroupAndCount
    {
        public int sortId;
        public SoldierGroup group;
        public int count;

        public SoldierGroupAndCount(int sortId, SoldierGroup group)
        {
            this.group = group;
            count = 1;
            this.sortId = sortId;
        }
    }

    partial class MapObjMenu //REASSIGN MENU
    {
        

        protected void ResassignTab(RichBoxContent content, out RichBoxContent content2)
        {
            var army = mapObj.GetAbsArmy();

            if (player.movingGroupsCollection == null || player.movingGroupsCollection.mainArmy.fromArmy != army)
            {
                player.movingGroupsCollection = new MovingGroupsCollection(army);
            }

            listUnits(content, army, player.movingGroupsCollection.mainArmy, out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter);

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.HUD_Filter);
            content.newLine();
            itemsUsed.Remove(ItemResourceType.NONE);
            foreach (ItemResourceType itemResourceType in itemsUsed)
            {
                IconName.Item(itemResourceType, out var itemIcon, out string itemName);
                content.Add(new ArtToggle(player.armyFilterItems.Contains(itemResourceType),
                    new List<AbsRichBoxMember> { new RbImage(itemIcon) },
                    new RbAction1Arg<ItemResourceType>((ItemResourceType selected) =>
                    {
                        if (player.armyFilterItems.Contains(selected))
                        {
                            player.armyFilterItems.Remove(selected);
                        }
                        else
                        {
                            player.armyFilterItems.Add(selected);
                        }
                    }, itemResourceType),
                    new RbTooltip_Text(itemName)));
            }
            content.newLine();
            for (UnitFilterType filterType = 0; filterType < UnitFilterType.NUM; filterType++)
            {
                if (unitFilterUsed.Contains(filterType))
                {
                    content.Add(new ArtToggle(player.armyFilterClasses.Contains(filterType),
                    new List<AbsRichBoxMember> { new RbText(filterType.ToString()) },
                    new RbAction1Arg<UnitFilterType>((UnitFilterType selected) =>
                    {
                        if (player.armyFilterClasses.Contains(selected))
                        {
                            player.armyFilterClasses.Remove(selected);
                        }
                        else
                        {
                            player.armyFilterClasses.Add(selected);
                        }
                    }, filterType)));
                }
            }

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.FlagEditor_ClearAll) },
                new RbAction(() =>
                {
                    player.armyFilterItems.Clear();
                    player.armyFilterClasses = new UnitFilter();
                }), null, !noFilter));


            void tooltip(RichBoxContent content, object tag)
            {
                SoldierGroup group = (SoldierGroup)tag;

                group.toTooltip(new ObjectHudArgs() { content = content, player = player });
            }



            content2 = reassignToMenu();

            void listUnits(RichBoxContent content, AbsArmy army, MovingGroups movedAway, out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter)
            {
                List<SoldierGroup> groups = army.groups.toList();
                Dictionary<int, SoldierGroupAndCount> groupCountDic = new Dictionary<int, SoldierGroupAndCount>(groups.Count);
                
                foreach (var group in groups)
                {
                    if (!movedAway.groups.Contains(group))
                    {
                        int sortId = group.soldierConscript.conscript.SortOrderValue();
                        if (groupCountDic.TryGetValue(sortId, out var groupAndCount))
                        {
                            groupAndCount.count++;
                        }
                        else
                        {
                            groupCountDic.Add(sortId, new SoldierGroupAndCount(sortId, group));
                        }
                    }
                }
                List<SoldierGroupAndCount> groupAndCounts = groupCountDic.Values.ToList();
                groupAndCounts.Sort((a, b) => a.sortId.CompareTo(b.sortId));

                itemsUsed = new HashSet<ItemResourceType>();
                unitFilterUsed = new UnitFilter();
                noFilter = player.armyFilterItems.Count == 0 && player.armyFilterClasses.value.IsEmpty();
                foreach (SoldierGroupAndCount groupcount in groupAndCounts)
                {
                    var group = groupcount.group;
                    itemsUsed.Add(group.soldierConscript.conscript.man);
                    itemsUsed.Add(group.soldierConscript.conscript.weapon);
                    itemsUsed.Add(group.soldierConscript.conscript.shield);
                    itemsUsed.Add(group.soldierConscript.conscript.animal);
                    itemsUsed.Add(group.soldierConscript.conscript.armorLevel);
                    itemsUsed.Add(group.soldierConscript.conscript.vehicle);

                    bool inFilter = noFilter ||
                        player.armyFilterClasses.value.InFilter(group.soldierData.unitFilter.value) ||
                        player.armyFilterItems.Contains(group.soldierConscript.conscript.man) ||
                        player.armyFilterItems.Contains(group.soldierConscript.conscript.weapon) ||
                        player.armyFilterItems.Contains(group.soldierConscript.conscript.shield) ||
                        player.armyFilterItems.Contains(group.soldierConscript.conscript.animal) ||
                        player.armyFilterItems.Contains(group.soldierConscript.conscript.armorLevel) ||
                        player.armyFilterItems.Contains(group.soldierConscript.conscript.vehicle);

                    unitFilterUsed.value.Combine(group.soldierData.unitFilter.value);

                    RichBoxContent buttonContent = new RichBoxContent();
                    group.TypeIcon(buttonContent);
                    group.soldierConscript.conscript.toHud(buttonContent, true);
                    content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent, new RbAction2Arg<SoldierGroupAndCount, bool>(movedAway.AddGroup, groupcount, false), new RbTooltip(tooltip, group), inFilter));

                    if (groupcount.count > 1)
                    {
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("×" + groupcount.count.ToString()) },
                            new RbAction2Arg<SoldierGroupAndCount, bool>(movedAway.AddGroup, groupcount, true), null, inFilter));
                    }

                    content.space();
                }
            }
        }

        RichBoxContent reassignToMenu()
        {
            RichBoxContent content = new RichBoxContent();

            return content;
        }
    }
}

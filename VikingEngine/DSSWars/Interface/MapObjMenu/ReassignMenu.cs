using MonoGame.Framework.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Interface.MapObjMenu
{
    class MovingGroupsCollection
    {   
        public MovingGroups mainArmy;
        public ListWithSelection<MovingGroups> otherArmies;

        public MovingGroupsCollection(AbsArmy fromArmy)
        {
            mainArmy = new MovingGroups(fromArmy);
            otherArmies = new ListWithSelection<MovingGroups>(4);// { new MovingGroups(null) };
            otherArmies.Add(new MovingGroups(null), true);
        }

        public bool hasMoved()
        {
            if (mainArmy.moveGroups.Count > 0)
            {
                return true;
            }
            foreach (var m in otherArmies.list)
            {
                if (m.moveGroups.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public void cancel()
        {
            mainArmy.cancel();
            foreach (var m in otherArmies.list)
            {
                m.cancel();
            }
        }
    }

    class MovingGroups
    {
        /// <summary>
        /// Null if new army
        /// </summary>
        public AbsArmy army;
        public HashSet<SoldierGroup> moveGroups;
        public List<SoldierGroup> recieveGroups;

        public MovingGroups(AbsArmy fromArmy)
        {
            this.army = fromArmy;
            //if (fromArmy != null)
            //{
                this.moveGroups = new HashSet<SoldierGroup>(16);//new HashSet<SoldierGroup>(16);
            //}
            recieveGroups = new List<SoldierGroup>(32);
        }

        public void cancel()
        {
            moveGroups.Clear();
            recieveGroups.Clear();
        }

        public void AddGroup(SoldierGroupAndCount group, MovingGroups toArmy, bool add, bool moveAll)
        {
            if (add)
            {
                if (moveAll)
                {
                    var groupsC = army.groups.counter();
                    while (groupsC.Next())
                    {
                        if (groupsC.sel.soldierConscript.conscript.SortOrderValue() == group.sortId)
                        {
                            moveGroups.Add(groupsC.sel);
                            toArmy.recieveGroups.Add(groupsC.sel);
                        }
                    }
                }
                else
                {
                    moveGroups.Add(group.group);
                    toArmy.recieveGroups.Add(group.group);
                }
            }
            else
            {
                if (moveAll)
                {
                    var groupsC = army.groups.counter();
                    while (groupsC.Next())
                    {
                        if (groupsC.sel.soldierConscript.conscript.SortOrderValue() == group.sortId)
                        {
                            moveGroups.Remove(groupsC.sel);
                            toArmy.recieveGroups.Remove(groupsC.sel);
                        }
                    }
                }
                else
                {
                    moveGroups.Remove(group.group);
                    toArmy.recieveGroups.Remove(group.group);
                }
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

            if (player.movingGroupsCollection == null || player.movingGroupsCollection.mainArmy.army != army)
            {
                player.movingGroupsCollection = new MovingGroupsCollection(army);
            }

            listUnits(content, player.movingGroupsCollection.mainArmy, player.movingGroupsCollection.otherArmies.Selected(), out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter);

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


            



            content2 = reassignToMenu();           
        }

        RichBoxContent reassignToMenu()
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(string.Format( DssRef.lang.ArmyOption_SendToX, string.Empty), HudLib.TitleColor_Head);

            content.newParagraph();
            //TABS
            var tabs = new List<ArtTabMember>(player.movingGroupsCollection.otherArmies.Count);
            
            for (int index = 0; index < player.movingGroupsCollection.otherArmies.Count; index++)
            {
                RichBoxContent buttonContent = new RichBoxContent();
                var army = player.movingGroupsCollection.otherArmies.list[index].army;

                if (army == null)
                {
                    buttonContent.Add(new RbText(DssRef.lang.ArmyOption_NewArmy));
                }
                else
                {
                    army.toButtonContent(buttonContent, true);
                }

                tabs.Add(new ArtTabMember(buttonContent));
            }

            var tabGroup = new ArtTabgroup(tabs, player.movingGroupsCollection.otherArmies.selectedIndex, (int select) =>
            {
                player.movingGroupsCollection.otherArmies.selectedIndex = select;
            });
            content.Add(tabGroup);

            content.newLine();

            //TAB CONTENT
            listUnits(content, player.movingGroupsCollection.otherArmies.Selected(), player.movingGroupsCollection.mainArmy, out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter);

            content.newParagraph();
            bool hasMoveChanges = player.movingGroupsCollection.hasMoved();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Apply) }, null, null, hasMoveChanges));
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Cancel) }, new RbAction(player.movingGroupsCollection.cancel), null, hasMoveChanges));
            

            return content;
        }


        void listUnits(RichBoxContent content, MovingGroups sending, MovingGroups recieving, out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter)
        {
            Dictionary<int, SoldierGroupAndCount> groupCountDic = new Dictionary<int, SoldierGroupAndCount>(16);
            List<SoldierGroup> groups = new List<SoldierGroup>(64);
            if (sending.army != null)
            {
                var groupsC = sending.army.groups.counter();
                while (groupsC.Next())
                {
                    if (!sending.moveGroups.Contains(groupsC.sel))
                    { 
                        groups.Add(groupsC.sel);
                    }
                }
            }

            groups.AddRange(sending.recieveGroups);

            foreach (var group in groups)
            {
                if (!sending.moveGroups.Contains(group))
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
                content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent,
                    new RbAction4Arg<SoldierGroupAndCount, MovingGroups, MovingGroups, bool>(moveGroup,
                        groupcount, player.movingGroupsCollection.mainArmy, player.movingGroupsCollection.otherArmies.Selected(), false),
                    new RbTooltip(tooltip, group), inFilter));

                if (groupcount.count > 1)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("×" + groupcount.count.ToString()) },
                        new RbAction4Arg<SoldierGroupAndCount, MovingGroups, MovingGroups, bool>(moveGroup,
                        groupcount, player.movingGroupsCollection.mainArmy, player.movingGroupsCollection.otherArmies.Selected(), true),
                        null, inFilter));
                }

                content.space();
            }

            void moveGroup(SoldierGroupAndCount group, MovingGroups sending, MovingGroups recieving, bool moveAll)
            {
                if (group.group.army.TryGetTarget(out var groupOwner))
                {
                    bool leftSide;
                    if (groupOwner == sending.army)
                    {
                        leftSide = !sending.moveGroups.Contains(group.group);
                    }
                    else
                    {
                        leftSide = recieving.moveGroups.Contains(group.group);
                    }

                    sending.AddGroup(group, recieving, leftSide, moveAll);
                 
                }

            }
        }
        void tooltip(RichBoxContent content, object tag)
        {
            SoldierGroup group = (SoldierGroup)tag;

            group.toTooltip(new ObjectHudArgs() { content = content, player = player });
        }
    }
}

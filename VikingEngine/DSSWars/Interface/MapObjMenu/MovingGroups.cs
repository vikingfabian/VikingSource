using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
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
            mainArmy = new MovingGroups(fromArmy, true);
            otherArmies = new ListWithSelection<MovingGroups>(4);
            otherArmies.Add(new MovingGroups(null, false), true);
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
        public bool isMainArmy;
        /// <summary>
        /// Null if new army
        /// </summary>
        public AbsArmy army;
        public HashSet<SoldierGroup> moveGroups;
        public List<SoldierGroup> recieveGroups;

        public MovingGroups(AbsArmy fromArmy, bool isMainArmy)
        {
            this.army = fromArmy;
            this.moveGroups = new HashSet<SoldierGroup>(16);
            recieveGroups = new List<SoldierGroup>(32);
            this.isMainArmy = isMainArmy;
        }

        public void cancel()
        {
            moveGroups.Clear();
            recieveGroups.Clear();
        }
        public void SendAllGroups(LocalPlayer player, MovingGroups toArmy, bool bHalf)
        {
            List<SoldierGroupAndCount> allGroups = ListUnits(player, null, this, toArmy, out _, out _, out _);

            bool halfToggle = true;
            foreach (var group in allGroups)
            {
                if (group.inFilter && group.group.army.TryGetTarget(out var tArmy))
                {
                    if (tArmy == army)
                    {
                        MoveGroup(group, toArmy, true, true, bHalf, ref halfToggle);
                    }
                    else
                    {
                        toArmy.MoveGroup(group, this, false, true, bHalf, ref halfToggle);
                    }
                }
            }
        }
        public void AddGroup(SoldierGroupAndCount group, MovingGroups toArmy, bool add, bool moveAll)
        {
            bool non = false;
            MoveGroup(group, toArmy, add, moveAll, false, ref non);
        }
        public void MoveGroup(SoldierGroupAndCount group, MovingGroups toArmy, bool add, bool moveAll, bool moveHalf, ref bool halfToggler)
        {
            if (add)
            {
                if (moveAll)
                {

                    var groupsC = army.groups.counter();
                    while (groupsC.Next())
                    {
                        if (!moveGroups.Contains(groupsC.sel) &&
                            groupsC.sel.soldierConscript.conscript.SortOrderValue() == group.sortId)
                        {
                            if (!moveHalf || halfToggler)
                            {
                                moveGroups.Add(groupsC.sel);
                                toArmy.recieveGroups.Add(groupsC.sel);
                            }
                            halfToggler = !halfToggler;
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
                            if (!moveHalf || halfToggler)
                            {
                                moveGroups.Remove(groupsC.sel);
                                toArmy.recieveGroups.Remove(groupsC.sel);
                            }
                            halfToggler = !halfToggler;
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

        public static List<SoldierGroupAndCount> ListUnits(LocalPlayer player, RichBoxContent content, MovingGroups sending, MovingGroups recieving, 
            out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter)
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

                groupcount.inFilter = noFilter ||
                    player.armyFilterClasses.value.InFilter(group.soldierData.unitFilter.value) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.man) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.weapon) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.shield) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.animal) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.armorLevel) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.vehicle);

                unitFilterUsed.value.Combine(group.soldierData.unitFilter.value);

                if (content != null)
                {
                    RichBoxContent buttonContent = new RichBoxContent();
                    group.TypeIcon(buttonContent);
                    group.soldierConscript.conscript.toHud(buttonContent, true);
                    content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent,
                        new RbAction4Arg<SoldierGroupAndCount, MovingGroups, MovingGroups, bool>(moveGroup,
                            groupcount, player.movingGroupsCollection.mainArmy, player.movingGroupsCollection.otherArmies.Selected(), false),
                        new RbTooltip(tooltip, group), groupcount.inFilter));

                    if (groupcount.count > 1)
                    {
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("×" + groupcount.count.ToString()) },
                            new RbAction4Arg<SoldierGroupAndCount, MovingGroups, MovingGroups, bool>(moveGroup,
                            groupcount, player.movingGroupsCollection.mainArmy, player.movingGroupsCollection.otherArmies.Selected(), true),
                            null, groupcount.inFilter));
                    }

                    content.space();

                    //--
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
            }

            if (content != null)
            {
                content.newParagraph();
                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.ArmyOption_SendAll) },
                    new RbAction3Arg<MovingGroups, MovingGroups, bool>(moveAll,
                            sending, recieving, false), null, groupAndCounts.Count > 0));

                content.Add(new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.ArmyOption_DivideHalf) },
                    new RbAction3Arg<MovingGroups, MovingGroups, bool>(moveAll,
                            sending, recieving, true), null, groupAndCounts.Count > 0));

                void moveAll(MovingGroups sending, MovingGroups recieving, bool moveHalf)
                {
                    sending.SendAllGroups(player, recieving, moveHalf);
                }
            }

            return groupAndCounts;

            static void tooltip(RichBoxContent content, object tag)
            {
                SoldierGroup group = (SoldierGroup)tag;

                group.toTooltip(new ObjectHudArgs() { content = content, player = group.pfaction.GetPlayer()?.GetLocalPlayer() });
            }
        }
        

    }
    class SoldierGroupAndCount
    {
        public int sortId;
        public SoldierGroup group;
        public int count;
        public bool inFilter;

        public SoldierGroupAndCount(int sortId, SoldierGroup group)
        {
            this.group = group;
            count = 1;
            this.sortId = sortId;
        }
    }
}

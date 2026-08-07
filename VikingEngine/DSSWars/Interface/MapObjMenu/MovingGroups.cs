using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
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

        public bool Contains(AbsArmy army)
        {
            foreach (var oa in otherArmies.list)
            {
                if (oa.army == army)
                {
                    return true;
                }
            }

            return false;
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
        public void disband()
        {
            mainArmy.Disband(otherArmies.First);
        }
        public void apply()
        {   
            foreach (var m in otherArmies.list)
            {
                mainArmy.Apply(m);

                m.Apply(mainArmy);
            }
        }
        public void cancel()
        {
            mainArmy.cancel();
            foreach (var m in otherArmies.list)
            {
                mainArmy.cancel();
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
            HashSet<ItemResourceType> itemsUsed = new HashSet<ItemResourceType>();
            UnitFilter unitFilterUsed = new UnitFilter();
            List<SoldierGroupAndCount> allGroups = ListUnits(player, null, this, toArmy, itemsUsed, ref unitFilterUsed, out _);

            bool halfToggle = true;
            foreach (var group in allGroups)
            {
                if (group.inFilter && group.displayGroup.army.TryGetTarget(out var tArmy))
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
        public void AddGroup(SoldierGroupAndCount group, MovingGroups toArmy, bool moveAway, bool moveAll)
        {
            bool non = false;
            MoveGroup(group, toArmy, moveAway, moveAll, false, ref non);
        }
        public void MoveGroup(SoldierGroupAndCount group, MovingGroups toArmy, bool moveAway, bool moveAll, bool moveHalf, ref bool halfToggler)
        {
            if (moveAway)
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
                    moveGroups.Add(group.displayGroup);
                    toArmy.recieveGroups.Add(group.displayGroup);
                }
            }
            else
            {
                if (moveAll)
                {
                    var groupsC = toArmy.army.groups.counter();
                    while (groupsC.Next())
                    {
                        if (groupsC.sel.soldierConscript.conscript.SortOrderValue() == group.sortId)
                        {
                            if (!moveHalf || halfToggler)
                            {
                                toArmy.moveGroups.Remove(groupsC.sel);
                                recieveGroups.Remove(groupsC.sel);
                            }
                            halfToggler = !halfToggler;
                        }
                    }
                }
                else
                {
                    toArmy.moveGroups.Remove(group.displayGroup);
                    recieveGroups.Remove(group.displayGroup);
                }
            }
        }

        public void Disband(MovingGroups toArmy)
        {            
            foreach (var group in toArmy.recieveGroups)
            {
                group.DeleteMe(DeleteReason.Disband, true);
            }

            if (army.groups.Count > 0 && army.IsArmy())
            {
                army.GetArmy().refreshPositions(false);
            }
        }

        public void Apply(MovingGroups toArmy)
        {
            int moveCount = 0;

            if (moveGroups.Count > 0)
            {

                if (toArmy.army == null)
                {
                    IntVector2 onTile = DssRef.world.GetFreeTile(army.tilePos);
                    toArmy.army = army.pfaction.GetFaction().NewArmy(onTile);
                }

                foreach (var group in toArmy.recieveGroups)
                {
                    if (group.isDeleted)
                    { continue; }

                    army?.groups.RemoveAt(group.myIndex);
                    toArmy.army.AddSoldierGroup(group);
                    moveCount++;
                }

                if (army != null && army.IsArmy())
                {
                    //Move gold
                    var myArmy = army.GetArmy();
                    float startGroupCount = army.groups.Count;
                    Money transportGold;
                    float food = 0;
                    float conservedFood = 0;

                    if (army.groups.Count <= 0)
                    {
                        transportGold = army.money;
                        food = myArmy.food;
                        conservedFood = myArmy.conservedFood;
                        army.DeleteMe(DeleteReason.EmptyGroup, true);
                    }
                    else
                    {
                        float percMove = (startGroupCount - army.groups.Count) / startGroupCount;
                        transportGold = new Money(army.money.copper * percMove);
                        food = myArmy.food * percMove;
                        conservedFood = myArmy.conservedFood * percMove;
                        myArmy.refreshPositions(false);
                    }

                    myArmy.money -= transportGold;
                    myArmy.food -= food;
                    myArmy.conservedFood -= conservedFood;

                    if (toArmy.army != null)
                    {
                        var otherArmy = toArmy.army.GetArmy();
                        otherArmy.money += transportGold;
                        otherArmy.food += food;
                        otherArmy.conservedFood += conservedFood;

                        otherArmy.refreshPositions(false);
                        otherArmy.onArmyMerge();
                    }
                }
            }
        }

        public static List<SoldierGroupAndCount> ListUnits(LocalPlayer player, RichBoxContent content, MovingGroups sending, MovingGroups recieving, 
            HashSet<ItemResourceType> itemsUsed, ref UnitFilter unitFilterUsed, out bool noFilter)
        {
            Dictionary<int, SoldierGroupAndCount> groupCountDic = new Dictionary<int, SoldierGroupAndCount>(16);
            
            if (sending.army != null)
            {
                var groupsC = sending.army.groups.counter();
                while (groupsC.Next())
                {
                    if (!sending.moveGroups.Contains(groupsC.sel))
                    {
                        addGroup(groupsC.sel, true);
                    }
                }
            }

            foreach (SoldierGroup group in sending.recieveGroups)
            {
                addGroup(group, false);
            }

            void addGroup(SoldierGroup group, bool sending)
            {
                int sortId = group.soldierConscript.conscript.SortOrderValue();
                if (groupCountDic.TryGetValue(sortId, out var groupAndCount))
                {
                    groupAndCount.AddOne(group, sending);//.count++;
                }
                else
                {
                    groupCountDic.Add(sortId, new SoldierGroupAndCount(sortId, group, sending));
                }
            }

            List<SoldierGroupAndCount> groupAndCounts = groupCountDic.Values.ToList();
            groupAndCounts.Sort((a, b) => a.sortId.CompareTo(b.sortId));

            //itemsUsed = new HashSet<ItemResourceType>();
            //unitFilterUsed = new UnitFilter();
            noFilter = player.armyFilterItems.Count == 0 && player.armyFilterClasses.value.IsEmpty();
            foreach (SoldierGroupAndCount groupcount in groupAndCounts)
            {
                var group = groupcount.displayGroup;
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
                            groupcount, sending, recieving, false),
                        new RbTooltip(tooltip, group), groupcount.inFilter));

                    if (groupcount.count > 1)
                    {
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("×" + groupcount.count.ToString()) },
                            new RbAction4Arg<SoldierGroupAndCount, MovingGroups, MovingGroups, bool>(moveGroup,
                            groupcount, sending, recieving, true),
                            null, groupcount.inFilter));
                    }

                    content.space();

                    //--
                    void moveGroup(SoldierGroupAndCount group, MovingGroups sending, MovingGroups recieving, bool moveAll)
                    {
                        if (group.sending != null)
                        {
                            sending.AddGroup(group, recieving, true, moveAll);
                            if (!moveAll)
                            {
                                return;
                            }
                        }

                        if (group.recieving != null)
                        {
                            sending.AddGroup(group, recieving, false, moveAll);
                        }
                    }

                    
                }
            }

            if (content != null)
            {
                content.newParagraph();
                content.Add(new ArtButton(RbButtonStyle.Secondary, MoveButtonContent(DssRef.lang.ArmyOption_SendAll),
                    new RbAction3Arg<MovingGroups, MovingGroups, bool>(moveAll,
                            sending, recieving, false), null, groupAndCounts.Count > 0));

                content.Add(new ArtButton(RbButtonStyle.Secondary, MoveButtonContent(DssRef.todoLang.ArmyOption_SendHalf),
                    new RbAction3Arg<MovingGroups, MovingGroups, bool>(moveAll,
                            sending, recieving, true), null, groupAndCounts.Count > 0));

                List<AbsRichBoxMember> MoveButtonContent(string caption)
                {
                    var buttonContent = new List<AbsRichBoxMember> { new RbText(caption) };
                    if (sending.isMainArmy)
                    {
                        buttonContent.Add(new RbSpace());
                        buttonContent.Add(new RbImage(SpriteName.VoxelEditorFrameNext));
                    }
                    else
                    {
                        buttonContent.Insert(0,new RbImage(SpriteName.VoxelEditorFramePrevious));
                        buttonContent.Insert(1,new RbSpace());
                        
                    }
                    return buttonContent;
                }

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
        public SoldierGroup displayGroup;
        public SoldierGroup sending;
        public SoldierGroup recieving;
        public int count;
        public bool inFilter;

        public SoldierGroupAndCount(int sortId, SoldierGroup group, bool sendingSide)
        {
            this.displayGroup = group;
            if (sendingSide)
            {
                sending = group;
            }
            else
            {
                recieving = group;
            }
            count = 1;
            this.sortId = sortId;
        }

        public void AddOne(SoldierGroup group, bool sendingSide)
        {
            if (sendingSide)
            {
                if (sending == null)
                {
                    sending = group;
                }
            }
            else
            {
                if (recieving == null)
                {
                    recieving = group;
                }
            }
            count++;
        }
    }
}

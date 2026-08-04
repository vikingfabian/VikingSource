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
    partial class MapObjMenu //REASSIGN MENU
    {
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
                //sortId = group.soldierConscript.conscript.SortOrderValue();
            }
        }

        protected void ResassignTab(RichBoxContent content)
        {
            var army = mapObj.GetAbsArmy();

            List<SoldierGroup> groups = army.groups.toList();
            Dictionary<int, SoldierGroupAndCount> groupCountDic = new Dictionary<int, SoldierGroupAndCount>(groups.Count);
            //List<SoldierGroupAndCount> groupAndCounts = new List<SoldierGroupAndCount>(groups.Count);
            foreach (var group in groups)
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
            List<SoldierGroupAndCount> groupAndCounts = groupCountDic.Values.ToList();
            groupAndCounts.Sort((a, b) => a.sortId.CompareTo(b.sortId));

            HashSet<ItemResourceType> itemsUsed = new HashSet<ItemResourceType>();
            UnitFilter unitFilterUsed = new UnitFilter();

            bool noFilter = player.armyFilterItems.Count == 0 && player.armyFilterClasses.value.IsEmpty();

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
                content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent, null, new RbTooltip(tooltip, group), inFilter));

                if (groupcount.count > 1)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("×" + groupcount.count.ToString()) },
                        null, null, inFilter));
                }

                content.space();
            }

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.HUD_Filter);
            content.newLine();
            itemsUsed.Remove(ItemResourceType.NONE);
            foreach (ItemResourceType itemResourceType in itemsUsed)
            {
                IconName.Item(itemResourceType, out var itemIcon, out string itemName);
                content.Add(new ArtToggle(player.armyFilterItems.Contains(itemResourceType), 
                    new List<AbsRichBoxMember> { new RbImage(itemIcon) }, 
                    new RbAction1Arg<ItemResourceType>((ItemResourceType selected)=> {
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
                    new RbAction1Arg<UnitFilterType>((UnitFilterType selected) => {
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
                new RbAction(() => { 
                    player.armyFilterItems.Clear();
                    player.armyFilterClasses = new UnitFilter();
                }),null, !noFilter));


            void tooltip(RichBoxContent content, object tag)
            {
                SoldierGroup group = (SoldierGroup)tag;

                group.toTooltip(new ObjectHudArgs() { content = content, player = player });
            }

        }
    }
}

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
        

        protected void ResassignTab(RichBoxContent content)
        {
            var army = mapObj.GetAbsArmy();

            List<SoldierGroup> groups = army.groups.toList();
            groups.Sort((a, b) => b.soldierConscript.conscript.SortOrderValue().CompareTo(a.soldierConscript.conscript.SortOrderValue()));

            HashSet<ItemResourceType> itemsUsed = new HashSet<ItemResourceType>();
            UnitFilter unitFilter = new UnitFilter();

            foreach (SoldierGroup group in groups)
            {
                itemsUsed.Add(group.soldierConscript.conscript.man);
                itemsUsed.Add(group.soldierConscript.conscript.weapon);
                itemsUsed.Add(group.soldierConscript.conscript.shield);
                itemsUsed.Add(group.soldierConscript.conscript.animal);
                itemsUsed.Add(group.soldierConscript.conscript.armorLevel);
                itemsUsed.Add(group.soldierConscript.conscript.vehicle);

                bool inFilter = player.armyFilterItems.Count == 0 ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.man) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.weapon) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.shield) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.animal) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.armorLevel) ||
                    player.armyFilterItems.Contains(group.soldierConscript.conscript.vehicle);

                unitFilter.value.Combine(group.soldierData.unitFilter.value);

                RichBoxContent buttonContent = new RichBoxContent();
                group.TypeIcon(buttonContent);
                group.soldierConscript.conscript.toHud(buttonContent, true);
                content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent, null, new RbTooltip( tooltip, group), inFilter));
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
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.FlagEditor_ClearAll) },
                new RbAction(() => { 
                    player.armyFilterItems.Clear();
                    player.armyFilterClasses = new UnitFilter();
                })));


            void tooltip(RichBoxContent content, object tag)
            {
                SoldierGroup group = (SoldierGroup)tag;

                group.toTooltip(new ObjectHudArgs() { content = content, player = player });
            }

        }
    }
}

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
   

    partial class MapObjMenu //REASSIGN MENU
    {

        protected void ResassignTab(RichBoxContent content, out RichBoxContent content2)
        {
            var army = mapObj.GetAbsArmy();

            if (player.movingGroupsCollection == null || player.movingGroupsCollection.mainArmy.army != army)
            {
                player.movingGroupsCollection = new MovingGroupsCollection(army);               
            }

            var armyArmy = army.GetArmy();
            if (armyArmy != null)
            {
                List<AbsArmy> tradeAbleArmies = new List<AbsArmy>();
                DssRef.world.unitCollAreaGrid.collectArmies(player.pfaction, army.tilePos, 1,
                    tradeAbleArmies);

                FilterTradeAbleArmies(armyArmy, tradeAbleArmies);

                foreach (var ta in tradeAbleArmies)
                {
                    if (!player.movingGroupsCollection.Contains(ta))
                    {
                        player.movingGroupsCollection.otherArmies.Add(new MovingGroups(ta, false));
                    }
                }
            }

            HashSet<ItemResourceType> itemsUsed = new HashSet<ItemResourceType>();
            UnitFilter unitFilterUsed = new UnitFilter();
            MovingGroups.ListUnits(player, content, player.movingGroupsCollection.mainArmy, player.movingGroupsCollection.otherArmies.Selected(), 
                itemsUsed, ref unitFilterUsed, out bool noFilter);
            
            RichBoxContent otherUnitList = new RichBoxContent();
            if (player.gameControls.map.selection.obj != null)
            {
                MovingGroups.ListUnits(player, otherUnitList, player.movingGroupsCollection.otherArmies.Selected(), player.movingGroupsCollection.mainArmy,
                itemsUsed, ref unitFilterUsed, out noFilter);
            }

            content.newParagraph();
            HudLib.Label(content, DssRef.lang.HUD_Filter);
            content.newLine();

            itemsUsed.UnionWith(player.armyFilterItems);
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
                bool inUse = player.armyFilterClasses.Contains(filterType);
                if (inUse || unitFilterUsed.Contains(filterType))
                {
                    content.Add(new ArtToggle(inUse,
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

            if (player.gameControls.map.selection.obj != null)
            {
                content2 = reassignToMenu(otherUnitList);
            }
            else
            {
                content2 = null;
            }
        }

        RichBoxContent reassignToMenu(RichBoxContent unitList)
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
                    army.toTabContent(buttonContent, true);
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
            if (player.movingGroupsCollection.otherArmies.Selected().army != null)
            {
                player.movingGroupsCollection.otherArmies.Selected().army.toButtonContent(content, false);
                content.Add(new RbSeperationLine());
                content.newParagraph();
            }
            //MovingGroups.ListUnits(player, content, player.movingGroupsCollection.otherArmies.Selected(), player.movingGroupsCollection.mainArmy, 
            //    out HashSet<ItemResourceType> itemsUsed, out UnitFilter unitFilterUsed, out bool noFilter);
            content.AddRange(unitList);

            content.newParagraph();
            bool hasMoveChanges = player.movingGroupsCollection.hasMoved();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Apply) }, new RbAction(()=>
                {
                    player.movingGroupsCollection.apply();
                    player.movingGroupsCollection = null;
                }), 
                null, hasMoveChanges) { fillWidth = true });
            if (player.movingGroupsCollection.otherArmies.Selected().army == null)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.ArmyOption_Disband) }, new RbAction(() =>
                {
                    player.movingGroupsCollection.disband();
                    player.movingGroupsCollection = null;
                }), null, hasMoveChanges)
                { fillWidth = true });
            }
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Cancel) }, new RbAction(() =>
            {
                player.movingGroupsCollection = null;
            }), null, hasMoveChanges) { fillWidth = true });
            

            return content;
        }
    }
}

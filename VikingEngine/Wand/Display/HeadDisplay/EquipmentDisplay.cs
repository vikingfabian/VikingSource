using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Player;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.Core.Wand.Display.HeadDisplay
{
    class EquipmentDisplay
    {
        RichMenu menu;
        EquipmentSetup equipmentSetup;

        public EquipmentDisplay(EquipmentSetup equipmentSetup)
        {
            this.equipmentSetup = equipmentSetup;
            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top2, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top2_Back);

            menu.needRefresh = true;
            //refreshMenu();
        }

        public float Right()
        {
            return menu.backgroundArea.Right;
        }

        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

        public void NeedRefresh()
        {
            menu.needRefresh = true;
        }

        void refreshMenu()
        {
            equipmentMenu();
        }

        void equipmentMenu()
        {            

            RichBoxContent content = new RichBoxContent();
            content.h1("Equipment", HudLib.TitleColor_Head);
            content.newParagraph();

            content.h2("Hand weapon", HudLib.TitleColor_Label);
            content.newLine();
            content.Add(new RbText(equipmentSetup.handWeapon.Name));
            content.space();
            content.Add(new RbText(equipmentSetup.handWeapon.attack.ToString("-"), Color.Yellow));

            content.Add(new RbSeperationLine());
            content.newParagraph();
            content.h2("Wands", HudLib.TitleColor_Label);
            content.newLine();


            bool altar = WandRef.world.map.tileGrid.Get(WandRef.world.hero.currentPos.tilePos).item.itemType == GO.ObjectPointer.TileItemType.Altar;
            for (int i = 0; i < EquipmentSetup.MaxWandCount; ++i)
            {
                if (i < equipmentSetup.wands.Count)
                {
                    bool selected = equipmentSetup.selectedWand.HasValue && equipmentSetup.selectedWand.Value == i;
                    
                    equipmentSetup.wands.Array[i].ToHud(content, i, selected, altar);
                }
                content.Add(new RbSeperationLine());
                content.newParagraph();
            }
            content.newParagraph();
            content.h1("Tutorial", HudLib.TitleColor_Label);
            content.text("Use arrows to move and attack.");
            content.text("Select wand and press arrow to use magic.");
            content.text("All spells are hidden until they are triggered.");
            content.text("Wands must be recharged on altars.");
            content.text("ESC: quick reboot");
#if DEBUG 
            content.newParagraph();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText( "Recharge") }, new RbAction(WandRef.PlayScene.player.rechargeWands)));
#endif

            menu.Refresh(content);
        }
    }
}

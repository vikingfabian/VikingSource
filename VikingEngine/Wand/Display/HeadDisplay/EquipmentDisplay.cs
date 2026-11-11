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

            for (int i = 0; i < EquipmentSetup.MaxWandCount; ++i)
            {
                if (i < equipmentSetup.wands.Count)
                {
                    if (equipmentSetup.selectedWand.HasValue && equipmentSetup.selectedWand.Value == i)
                    {
                        HudLib.BulletPoint(content);
                    }
                    equipmentSetup.wands.Array[i].ToHud(content, i);
                }
                content.Add(new RbSeperationLine());
                content.newParagraph();
            }

            menu.Refresh(content);
        }
    }
}

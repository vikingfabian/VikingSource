using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Interface.MapObjMenu;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    class MapEditorPlayer : Players.LocalPlayer
    {
        public static List<MenuTab> EditorCityTabs = new List<MenuTab> { MenuTab.Info, MenuTab.Build };
        DetailEditorDisplay display;
        InfoDisplay infoDisplay;
        public MapEditorPlayer(Faction faction)
            : base(faction, true)
        {
            display = new DetailEditorDisplay();
            infoDisplay = new InfoDisplay();
        }

        public override bool updateObjectDisplay()
        {
            if (hud.maximizedHud)
            {
                hud.objMenu.createMenu(true, this);
                RichBoxContent content = new RichBoxContent();

                display.refresh(content);


                hud.objMenu.refresh(this, content);
                return true;
            }
            return false;
        }

        
        //public override void Update()
        //{
        //    base.Update();
        //    infoDisplay.update();
        //}
        public override void userUpdate(bool cityUpdate)
        {
            base.userUpdate(cityUpdate);
            infoDisplay.update();
        }
        

        public override List<MenuTab> AvailableCityTabs()
        {
            return EditorCityTabs;
        }
    }
}

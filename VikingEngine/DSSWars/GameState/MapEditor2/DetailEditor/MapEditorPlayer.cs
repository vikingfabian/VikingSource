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
        public DetailEditorDisplay display;
        
        public MapEditorPlayer(Faction faction)
            : base(faction, true)
        {
            display = new DetailEditorDisplay();

            if (infoDisplay == null)
            {
                infoDisplay = new InfoDisplay(Engine.Screen.SafeArea.Position);
            }
        }

        public void onEditorStart()
        { 
             gameControls.build.editorTool = new DetailEditorTool((MapEditorPlayState)DssRef.state);
        }

        public override void onGameStart(bool newGame)
        {
            base.onGameStart(newGame);
           
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
            
        }
        

        public override List<MenuTab> AvailableCityTabs()
        {
            return EditorCityTabs;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    class MapEditorPlayer : Players.LocalPlayer
    {
        public MapEditorPlayer(Faction faction)
            : base(faction, true)
        {
            
        }

        public override bool updateObjectDisplay()
        {
            if (hud.maximizedHud)
            {
                hud.objMenu.createMenu(true, this);
                RichBoxContent content = new RichBoxContent();
                content.h1("Map editor - detail", HudLib.TitleColor_Head);
                hud.objMenu.refresh(this, content);

                return true;
            }
            return false;
        }


    }
}

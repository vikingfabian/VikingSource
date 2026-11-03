using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.Interface
{
    struct ObjectHudArgs
    {
        public RichMenu menu;
        public HUD.RichBox.RichBoxContent content;
        public Players.LocalPlayer player;
        public bool selected;
        //public HUD.RichBox.RichboxGui gui;
        public ObjectHudArgs(RichMenu menu, HUD.RichBox.RichBoxContent content, Players.LocalPlayer player, bool selected)
        {
            //this.gui = gui; 
            this.menu = menu;
            this.content = content;
            this.player = player;
            this.selected = selected;
        }

        public ObjectHudArgs(HUD.RichBox.RichBoxContent content)
        {
            //this.gui = gui; 
            this.content = content;
            this.player = null;
            this.selected = false;
        }

        public bool ShowFull => player.hud.maximizedHud;
    }
}

using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display
{
    class ObjectDisplay : RichboxGuiPart
    {
        public ObjectDisplay(RichboxGui gui)
            : base(gui)
        { }

        public void refresh(Players.LocalPlayer player, GameObject.AbsWorldObject obj, bool selected, Vector2 pos)
        {
            interaction?.DeleteMe();
            interaction = null;

            setVisible(obj != null);

            if (obj != null)
            {
                beginRefresh();
                obj.toHud(new ObjectHudArgs(gui, content, player, selected));
                if (gui.menuState.Count > 0) 
                {
                    content.newLine();
                    content.Button(DssRef.lang.Hud_Back, new RbAction(gui.menuBack, SoundLib.menuBack), 
                        null, true);
                }
                endRefresh(pos, selected);
                viewOutLine(player.mapControls.focusedObjectMenuState());
            }
        }

    }

    struct ObjectHudArgs
    {
        public HUD.RichBox.RichBoxContent content;
        public Players.LocalPlayer player;
        public bool selected;
        public HUD.RichBox.RichboxGui gui;
        public ObjectHudArgs(HUD.RichBox.RichboxGui gui, HUD.RichBox.RichBoxContent content, Players.LocalPlayer player, bool selected)
        {
            this.gui = gui; 
            this.content = content;
            this.player = player;
            this.selected = selected;
        }
    }
}

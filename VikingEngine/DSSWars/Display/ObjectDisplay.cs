using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display
{
    class ObjectDisplay : RichboxGuiPart
    {
        public ObjectDisplay(RichboxGui gui)
            : base(gui)
        { }

        public void refresh(Players.LocalPlayer player, GameObject.AbsGameObject obj, bool selected, Vector2 pos)
        {
            interaction?.DeleteMe();
            interaction = null;

            setVisible(obj != null);

            if (obj != null)
            {
                beginRefresh();
                if (obj.CanMenuFocus() && player.input.inputSource.IsController)
                {
                    content.Add(new HUD.RichBox.RichBoxImage(player.input.ControllerFocus.Icon));
                    content.Add(new HUD.RichBox.RichBoxText(":"));
                    content.newLine();
                }

                obj.toHud(new ObjectHudArgs(gui, content, player, selected));
                if (gui.menuState.Count > 0) 
                {
                    content.newLine();
                    content.Button(Ref.langOpt.Hud_Back, new RbAction(gui.menuBack, SoundLib.menuBack), 
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

        public bool ShowFull => player.hud.detailLevel == HudDetailLevel.Normal;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

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
                
                content.newParagraph();

                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Revert to Icon") },
                    new RbAction(() => {
                        new StartIconEditor(DssRef.world);
                    }), null));



                hud.objMenu.refresh(this, content);
                return true;
            }
            return false;
        }


    }
}

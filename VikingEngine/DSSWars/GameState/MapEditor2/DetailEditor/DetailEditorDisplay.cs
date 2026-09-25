using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    class DetailEditorDisplay
    {
        public DetailEditorDisplay()
        { 
            
        }

        public void refresh(RichBoxContent content)
        {
            content.h1("Map editor - detail", HudLib.TitleColor_Head);

            content.newParagraph();

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Revert to Icon") },
                new RbAction(() => {
                    new StartIconEditor(DssRef.world);
                }), null));
        }
    }
}

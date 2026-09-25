using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.GameState.MapEditor2.IconEditor;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    enum DetailEditorTab
    {
        File,
        //Setup,
        
        Tiles,
        
        Cities,


        NUM
    }

    class DetailEditorDisplay
    {
        public DetailEditorTab tab = 0;

        public DetailEditorDisplay()
        { 
            
        }

        public void refresh(RichBoxContent content)
        {
            content.h1("Map editor - detail", HudLib.TitleColor_Head);

            content.newParagraph();

            var tabs = new List<ArtTabMember>();
            {
                for (DetailEditorTab tabType = 0; tabType < DetailEditorTab.NUM; tabType++)
                {
                    tabs.Add(new ArtTabMember(new List<AbsRichBoxMember> { new RbText(tabType.ToString()) }));
                }

                var tabGroup = new ArtTabgroup(tabs, (int)tab, (int ix) =>
                {
                    tab = (DetailEditorTab)ix;

                    switch (tab)
                    {
                        case DetailEditorTab.Cities:
                        case DetailEditorTab.Tiles:
                            DssRef.state.LocalHost().gameControls.build.buildMode = Players.SelectTileResult.EditorBuild;
                            break;
                    }

                }, null);

                content.Add(tabGroup);
            }            
            content.newLine();

            switch (tab)
            {
                case DetailEditorTab.File:
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Revert to Icon") },
                        new RbAction(() => {
                            new StartIconEditor(DssRef.world);
                        }), null));
                    break;
            }


            
        }
    }
}

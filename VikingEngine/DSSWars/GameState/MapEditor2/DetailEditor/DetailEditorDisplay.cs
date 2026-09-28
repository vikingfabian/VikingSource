using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.GameState.MapEditor2.IconEditor;
using VikingEngine.DSSWars.Map.MapLib;
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

                    DssRef.state.LocalHost().gameControls.editorBuildMode = false;

                    switch (tab)
                    {
                        case DetailEditorTab.Cities:
                        case DetailEditorTab.Tiles:
                            DssRef.state.LocalHost().gameControls.build.buildMode = Players.SelectTileResult.EditorBuild;
                            DssRef.state.LocalHost().gameControls.editorBuildMode = true;
                            break;
                    }

                }, null);

                
                content.Add(tabGroup);
            }
            
            content.newLine();
            var tool = DssRef.state.LocalHost().gameControls.build.editorTool;
            switch (tab)
            {
                case DetailEditorTab.File:
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Revert to Icon") },
                        new RbAction(() => {
                            new StartIconEditor(DssRef.world);
                        }), null));
                    break;
                case DetailEditorTab.Tiles:
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Edit height") }, tool.editHeightProperty));
                    if (tool.editHeight)
                    {
                        content.newLine();
                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Set") }, tool.setHeightProperty));
                        content.newLine();
                        HudLib.Label(content, "Height");
                        content.Add(new RbTab(0.25f));
                        if (tool.setHeight)
                        {
                            RbDragButton.RbDragButtonGroup(content, new List<float> { 8, 32 }, new DragButtonSettings(0, byte.MaxValue, 1), tool.setHeightValueProperty, false);
                        }
                        else
                        {
                            RbDragButton.RbDragButtonGroup(content, new List<float> { 8, 32 }, new DragButtonSettings(-200, 200, 1), tool.addHeightValueProperty, false);
                        }
                    }
                    content.Add(new RbSeperationLine());
                    content.newParagraph();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Edit ground type") }, tool.editGroundTypeProperty));

                    if (tool.editGroundType)
                    {
                        content.newLine();
                        for (GroundType type = 0; type < GroundType.NUM; type++)
                        {
                            content.Add(new ArtOption(type == tool.groundType, new List<AbsRichBoxMember> { new RbText(type.ToString()) },
                                new RbAction1Arg<GroundType>((GroundType selected) =>
                                {
                                    tool.groundType = selected;
                                }, type)));
                        }
                    }
                    break;
            }


            
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.PJ.MiniGolf;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.Core.BlackBolts.Interface
{
    class ToolMenu
    {
        Player.Player player;
        RichMenu menu;

        public ToolMenu(Player.Player player)
        {
            this.player = player;
            HudLib.Init();

            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top2, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top2_Back);

            refreshMenu();
        }

        public void NeedRefresh()
        { 
            menu.needRefresh = true;
        }

        public void refreshMenu()
        {
            iconMenu();
        }

        void iconMenu()
        {
            RichBoxContent content = new RichBoxContent();           
            content.h1("Black Bolt Industries", HudLib.TitleColor_Head);
            content.newParagraph();

            if (player.editMode)
            {
                var mission = BlackRef.playScene.missionSetup;

                HudLib.Label(content, "Component");
                content.newLine();
                //content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Click me") }, null));
                foreach (var comp in mission.componentList)//for (FactoryObjectType objectType = 0; objectType < FactoryObjectType.NUM_NONE; objectType++)
                {
                    content.Add(new ArtOption(comp == player.toolShop.placementData.component, new List<AbsRichBoxMember> { new RbText(comp.objectType.ToString()) },
                        new RbAction1Arg<ToolSetupComponent>((ToolSetupComponent selected) =>
                        {
                            player.toolShop.selectTool(selected);
                            player.OnToolRefresh();
                        }, comp), null));
                }

                var toolProp = player.toolShop.placementData.component.properties();

                if (toolProp.holdResourceType != HoldResourceType.NoResource)
                {
                    content.newParagraph();
                    if (toolProp.includeResource == IncludeType.Optional)
                    {
                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Include item") },
                            player.toolShop.IncludeItemProperty));
                    }

                    if (toolProp.includeResource == IncludeType.Required ||
                        player.toolShop.placementData.includeItem)
                    {
                        HudLib.Label(content, "Resource");
                        content.newLine();
                        for (ResourceType resource = 0; resource < ResourceType.NUM_NONE; resource++)
                        {
                            var resProp = ResourceLib.Get(resource);
                            if (resProp.debugLevel > ObjectDebugLevel.Incomplete && resProp.isSolid)
                            {
                                content.Add(new ArtOption(resource == player.toolShop.placementData.resourceType, new List<AbsRichBoxMember> { new RbText(resource.ToString()) },
                                    new RbAction1Arg<ResourceType>((ResourceType selected) =>
                                    {
                                        player.toolShop.selectResource(selected);
                                    }, resource), null));
                            }
                        }
                    }
                }

                content.newParagraph();
                HudLib.Label(content, "Machine");
                
                foreach (var io in BlackRef.playScene.missionSetup.ioUnits)
                {
                    content.newLine();
                    content.Add(new ArtOption(io.id == player.toolShop.placementData.machineId, 
                        new List<AbsRichBoxMember> { new RbText(io.name) }, new RbAction1Arg<MachineId>((MachineId selected)=>
                        {
                            player.toolShop.placementData.component = new ToolSetupComponent(FactoryObjectType.IOunit);
                            player.toolShop.placementData.machineId = io.id;
                        }, io.id)));
                }

                content.newParagraph();

                if (player.toolShop.placementData.component.properties().rotationType != RotationType.None)
                {
                    player.inputMap.rotate.ToRichContent(content);
                    content.hspace();
                    content.Add(new ArtButton(RbButtonStyle.Primary,
                        new List<AbsRichBoxMember> { new RbImage(SpriteName.RotateCW), new RbSpace(),
                new RbText("Rotate")}, new RbAction(player.rotateToolAction)));
                    content.newLine();
                }
                player.inputMap.toggleEditMode.ToRichContent(content);
                content.hspace();
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarPlayIcon), new RbSpace(),
                new RbText("Run production")}, new RbAction(player.toggleRunSimulation)));
            }
            else
            {
                player.inputMap.toggleEditMode.ToRichContent(content);
                content.hspace();
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarPauseIcon), new RbSpace(),
                new RbText("Stop")}, new RbAction(player.toggleRunSimulation)));
                content.text("Running...", Color.Gray);
            }

            content.newParagraph();


            content.Add(new RbSeperationLine());
            content.newParagraph();
            Ref.gamesett.fullScreenOptions(content);
            content.Add(new RbSeperationLine());
            content.newParagraph();

            content.Add(new ArtButton( RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Reset") }, new RbAction(() =>
            {
                new BlackPlayScene(BlackRef.playScene.missionSetup);    
            }), null){fillWidth=true});

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Exit") }, 
                new RbAction(Ref.update.Exit), null)
            { fillWidth = true });


            content.newParagraph();
            content.text(string.Format(HudLib.EngineVersionString, Engine.LoadContent.EngineVersion), Color.DarkGray);

            menu.Refresh(content);
        }

        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

    }
}

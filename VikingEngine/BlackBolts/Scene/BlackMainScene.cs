using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Mission;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.Core.BlackBolts.Scene
{
    class BlackMainScene : Engine.GameState
    {
        public static Texture2D bgTex;
        Graphics.ImageAdvanced bgImage = null;

        RichMenu menu;
        protected ImageLayers layer = ImageLayers.Foreground7;

        public BlackMainScene()
            : base()
        {
            createBackground();

            if (Ref.steam.isInitialized)
            {
                SteamTimeline.SetTimelineGameMode(ETimelineGameMode.k_ETimelineGameMode_Menus);
            }

            VectorRect menuArea = Engine.Screen.SafeArea;

            menuArea.Width = Engine.Screen.IconSize * 9;
            menuArea.X = Engine.Screen.CenterScreen.X - menuArea.Width / 2;
            menuArea.AddYRadius(-Engine.Screen.IconSize);
            menu = new RichMenu(HudLib.RbSettings, menuArea, new Vector2(8), RichMenu.DefaultRenderEdge, layer, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, layer + 4);
            refreshMenu();
        }

        void refreshMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Black Bolt Industries", Color.OrangeRed);

            missionButton(MissionType.Tutorial);
            missionButton(MissionType.WhiteKnight);
            //content.Add(new RbSeperationLine() { thick = true });
            missionButton(MissionType.Sandbox);
            

            content.newParagraph();
            Ref.gamesett.fullScreenOptions(content);
            content.Add(new RbSeperationLine());
            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Exit") },
                new RbAction(Ref.update.Exit), null)
            { fillWidth = true });

            content.newParagraph();
            content.h2("Developer notes:", Color.Orange);
            content.text("Prototype; the game is a proof of concept test, to see if it worth being worked on at all.");
            content.text("- No sound");
            content.text("- Use sandbox to learn features");
            content.text("- Please let me know what you think!");



            content.newParagraph();
            content.text(string.Format(HudLib.EngineVersionString, Engine.LoadContent.EngineVersion), Color.DarkGray);

            menu.Refresh(content);

            void missionButton(MissionType mission)
            {
                string caption, description;

                switch (mission)
                {
                    default:
                        caption = "Contract: Practice";
                        description = "Learn the value of success, and to make your bosses richer";
                        break;
                    case MissionType.WhiteKnight:
                        caption = "Contract: White knights";
                        description = "Girls are supposed to like bad boys, get rid of those knights!";
                        break;
                    case MissionType.Sandbox:
                        caption = "Sandbox";
                        description = "Freely play around with all the tools, like the baby you are!";
                        break;
                }

                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember> { new RbText(caption) },
                    new RbAction(()=> {
                        new Scene.LoadMissionScene(mission);
                    }), new RbTooltip_Text(description))
                { fillWidth = true });
            }
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            bool mouseOver = false;
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

        void createBackground()
        {
          
            var area = Engine.Screen.Area;
          
            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                area.Position, area.Size, ImageLayers.Background5, false);
            bgImage.Texture = bgTex;
            bgImage.SetFullTextureSource();
            bgImage.Opacity = 1;//0.8f;

        }
    }
}

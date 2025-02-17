using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;
using VikingEngine.HUD;
using VikingEngine.DSSWars.Players;
using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.Display
{
    class PlayerHud_HeadOptions
    {
        public Vector2 MessageStart;
        RichMenu menu;
        LocalPlayer player;

        public PlayerHud_HeadOptions(LocalPlayer player)
        {
            this.player = player;
            //
            var optionsDisplayAr = player.playerData.view.safeScreenArea;
            optionsDisplayAr.X = 0;
            menu = new RichMenu(HudLib.RbSettings_HeadOptions, optionsDisplayAr, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            {
                refreshUpdate();
                menu.updateWidthFromContent();
                menu.move(VectorExt.V2FromX(player.playerData.view.safeScreenArea.Right - menu.backgroundArea.Width));
                menu.updateHeightFromContent(false);

                NineSplitAreaTexture bg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudHeadBarSecondaryBg, 1, 11, 1f, true, true), menu.backgroundArea, HudLib.GUILayer + 4);
            }

            MessageStart = new Vector2(player.playerData.view.safeScreenArea.Right - (RichMenu.DefaultRenderEdge.X + HudLib.MessageDisplayWidth),
                menu.backgroundArea.Bottom + Engine.Screen.IconSize * 0.5f);
        }

        public void refreshUpdate()
        {
            RichBoxContent content = new RichBoxContent();
            headOptionsMenu(content);
            menu.Refresh(content);
        }

        public void headOptionsMenu(RichBoxContent content)
        {
            //content.Add(new RichBoxScale(1.6f));

            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbImage(Ref.isPaused ? SpriteName.WarsHudHeadBarPauseIcon : SpriteName.WarsHudHeadBarPlayIcon) },
                new RbAction(pauseAction), new RbTooltip_Text(DssRef.lang.Input_Pause)));

            for (int i = 0; i < player.GameSpeedOptions.Length; i++)
            {
                int speed = player.GameSpeedOptions[i];
                content.Add(new ArtOption(Ref.TargetGameTimeSpeed == speed,
                    new List<AbsRichBoxMember> { new RbText(speed.ToString()) },
                    new RbAction1Arg<int>(gameSpeedClick, speed),
                    new RbTooltip_Text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Input_GameSpeed, string.Format(DssRef.lang.Hud_XTimes, speed)))));
               
            }

            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarMenuIcon) },
                new RbAction(DssRef.state.menuSystem.pauseMenu), new RbTooltip_Text(DssRef.lang.GameMenu_Title)));

            if (DssRef.state.PlayType() == GameState.PlayStateType.BattleLab)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    { new RbText("Step 1 frame", Color.White) }, new RbAction1Arg<int>(DssRef.state.stepFrames, 1)));
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    { new RbText("4", Color.White) }, new RbAction1Arg<int>(DssRef.state.stepFrames, 4)));
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    { new RbText("10", Color.White) }, new RbAction1Arg<int>(DssRef.state.stepFrames, 10)));

            }
        }

        public void pauseAction()
        {
            Ref.SetPause(!Ref.isPaused);
        }

        void gameSpeedClick(int toSpeed)
        {
            Ref.SetPause(false);
            Ref.SetGameSpeed(toSpeed);
        }

        /// <returns>need refresh</returns>
        public bool updateMouseInput(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            return menu.needRefresh;
        }
    }
}

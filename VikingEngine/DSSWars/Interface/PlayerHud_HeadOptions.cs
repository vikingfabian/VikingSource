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

namespace VikingEngine.DSSWars.Interface
{
    class PlayerHud_HeadOptions
    {
        //public Vector2 MessageStart;
        RichMenu menu;
        LocalPlayer player;
        public float Left;
        public PlayerHud_HeadOptions(LocalPlayer player)
        {
            this.player = player;
            //
            var optionsDisplayAr = player.playerData.view.safeScreenArea;
            optionsDisplayAr.X = 0;
            menu = new RichMenu(HudLib.RbSettings_HeadOptions, optionsDisplayAr, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, player.playerData);
            {
                refreshUpdate();
                menu.updateWidthFromContent(true);
                float toX = lib.SmallestValue(player.playerData.view.DrawAreaF.Right - 8,  player.playerData.view.safeScreenArea.Right) - menu.backgroundArea.Width;
                
                menu.move(VectorExt.V2FromX(toX));
                menu.updateHeightFromContent(false);
                Left = menu.backgroundArea.X;
                NineSplitAreaTexture bg = new NineSplitAreaTexture(new NineSplitSettings(SpriteName.WarsHudHeadBarSecondaryBg, 1, 11, 1f, true, true), menu.backgroundArea, HudLib.GUILayer + 4);
            }

            player.hud.MessageStart.Y =/* new Vector2(player.playerData.view.safeScreenArea.Right - (RichMenu.DefaultRenderEdge.X + HudLib.MessageDisplayWidth),*/
                menu.backgroundArea.Bottom + Engine.Screen.IconSize * 0.5f;
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

            if (DssRef.state.IsSinglePlayer_LocalAndOnline())
            {

                bool viewControllerTabs = player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.Pause_GamePlay, out Color focusColor);
                if (viewControllerTabs)
                {
                    content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                    content.space(0.5f);
                }

                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember> { new RbImage(Ref.isPaused ? SpriteName.WarsHudHeadBarPauseIcon : SpriteName.WarsHudHeadBarPlayIcon) },
                    new RbAction(Ref.TogglePause), new RbTooltip((RichBoxContent content, object tag) =>
                    {
                        content.Add(new RbImage(player.gameControls.input.PauseGame.Icon));
                        content.Add(new RbSpace(0.5f));
                        content.Add(new RbText(DssRef.lang.Input_Pause));
                    })));


                if (viewControllerTabs)
                {
                    content.Add(new RbImage(player.gameControls.input.Controller_TabRight.Icon) { color = focusColor });
                    content.space(0.5f);
                }
                for (int i = 0; i < player.gameControls.GameSpeedOptions.Length; i++)
                {
                    int speed = player.gameControls.GameSpeedOptions[i];
                    content.Add(new ArtOption(Ref.TargetGameTimeSpeed == speed,
                        new List<AbsRichBoxMember> { new RbText(speed.ToString()) },
                        new RbAction1Arg<int>(gameSpeedClick, speed),
                        new RbTooltip((RichBoxContent content, object tag) =>
                        {
                            content.Add(new RbImage(player.gameControls.input.GameSpeed.Icon));
                            content.Add(new RbSpace(0.5f));
                            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Input_GameSpeed, string.Format(DssRef.lang.Hud_XTimes, speed))));
                        })));

                }
                content.space();
            }

            
            if (player.gameControls.input.inputSource.IsController)
            {
                content.Add(new RbImage(player.gameControls.input.Menu.Icon));
                content.space(0.5f);
            }
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudHeadBarMenuIcon) },
                new RbAction(DssRef.state.menuSystem.pauseMenu),
                new RbTooltip((RichBoxContent content, object tag) => {
                    content.Add(new RbImage(player.gameControls.input.menuInput.OpenCloseKeyBoard.Icon));
                    content.Add(new RbSpace(0.5f));
                    content.Add(new RbText(DssRef.lang.GameMenu_Title));
                })
                ));

            if (DssRef.state.PlayType() == GameState.PlayStateType.BattleLab)
            {
                content.newLine();
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    { new RbText(DssRef.lang.Input_StepOneFrame, Color.White) }, new RbAction1Arg<int>(DssRef.state.stepFrames, 1)));
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    { new RbText("4", Color.White) }, new RbAction1Arg<int>(DssRef.state.stepFrames, 4)));
                content.Add(new ArtButton(RbButtonStyle.Primary,
                    new List<AbsRichBoxMember>
                    { new RbText("10", Color.White) }, new RbAction1Arg<int>(DssRef.state.stepFrames, 10)));

            }
        }

        //public void pauseAction()
        //{
        //    Ref.SetPause(!Ref.isPaused);
        //}

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

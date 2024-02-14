using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander.Players;

namespace VikingEngine.ToGG
{
    class WinnerState : AbsToggState
    {
        Time inputLockTime = new Time(400);

        public WinnerState(AbsCmdPlayer winner)
        {
            draw.ClrColor = Color.Black;
            Graphics.TextG winnerText = new Graphics.TextG( LoadedFont.Regular, Engine.Screen.CenterScreen,
                new Vector2(Engine.Screen.TextSize * 1.6f), Graphics.Align.CenterWidth,
                winner.pData.PublicName(LoadedFont.Regular) + " won!", Color.White, ImageLayers.Lay3);

            AbsCmdPlayer vsPlayer = Commander.cmdRef.players.Player(winner.pData.globalPlayerIndex == 0? 1 : 0);

            Graphics.TextG scoreText = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.CenterScreen,
                new Vector2(Engine.Screen.TextSize * 1.0f), Graphics.Align.CenterWidth,
                winner.VictoryPoints.ToString() + "VP - " + vsPlayer.VictoryPoints.ToString() + "VP" , Color.White, ImageLayers.Lay3);
            scoreText.Ypos += Engine.Screen.Height * 0.1f;

            Graphics.TextG turnText = new Graphics.TextG(LoadedFont.Regular, scoreText.Position,
                new Vector2(Engine.Screen.TextSize * 1.0f), Graphics.Align.CenterWidth,
                winner.TurnsCount.ToString() + " turns", Color.White, ImageLayers.Lay3);
            turnText.Ypos += Engine.Screen.Height * 0.1f;

            if (winner is LocalPlayer)
            {
                toggRef.storage.wonQuickPlay++;
            }
            else
            {
                toggRef.storage.lostQuickPlay++;
            }
            toggRef.storage.saveLoad(true);
        }

        public override void Time_Update(float time)
        {

            base.Time_Update(time);

            if (inputLockTime.CountDown())
            {

                if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape) ||
                    Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Enter) ||
                    Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    new GameState.MainMenuState();
                }
            }
        }
        protected override bool DefaultMouseLock
        {
            get { return false; }
        }
    }
}

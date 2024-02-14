using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class PlayerTurnPresentation 
    {
        Graphics.ImageGroup2D presentation;

        public Time cameraMoveTime = new Time(300);
        Time startFade = new Time(2f, TimeUnit.Seconds);

        float opacity = 1f;

        public PlayerTurnPresentation(AbsGenericPlayer player, bool turnCountDisplay) 
        {
            const ImageLayers Layer = ImageLayers.Top5;

            VectorRect borderArea = new VectorRect(0, 0, Engine.Screen.Width, Engine.Screen.IconSize * 2f);
            borderArea.AddRadius(1f);
            
            string textstring = player.pData.PublicName(LoadedFont.Regular); ;
            if (turnCountDisplay)
            {
                textstring += " : Turn " + (player.TurnsCount + 1).ToString();
            }

            Graphics.Image textBg = new Graphics.Image(SpriteName.WhiteArea, 
                Engine.Screen.CenterScreen, Engine.Screen.IconSizeV2 * 2f, Layer, true);
            textBg.Color = Color.Black;

            Graphics.TextG textSprite = new Graphics.TextG(LoadedFont.Regular, textBg.Position,
                Vector2.One, Graphics.Align.CenterAll, textstring, Color.White, ImageLayers.AbsoluteTopLayer);
            textSprite.LayerAbove(textBg);
            textSprite.SetHeight(textBg.Height * 0.6f);

            float textSpriteW = textSprite.MeasureText().X;
            textBg.Width = textSpriteW + Engine.Screen.IconSize * 2f;

            presentation = new Graphics.ImageGroup2D(new List<AbsDraw2D>
            {
                textBg, textSprite, 
            });

            if (toggRef.mode == GameMode.Commander && turnCountDisplay && player.IsScenarioFriendly)
            {
                Graphics.TextG turnLimitSprite = new Graphics.TextG(LoadedFont.Regular,
                    new Vector2(textSprite.Xpos + textSpriteW * 0.5f, textBg.RealTop + textBg.Height * 0.35f),
                    Vector2.One, Graphics.Align.Zero, "/" + toggRef.gamestate.gameSetup.WinningConditions.timeLimit.ToString(), 
                    Color.White, ImageLayers.AbsoluteTopLayer);
                turnLimitSprite.LayerAbove(textBg);
                turnLimitSprite.SetHeight(textBg.Height * 0.4f);

                textBg.Width += turnLimitSprite.MeasureText().X;

                presentation.Add(turnLimitSprite);
            }            

            if (!turnCountDisplay)
            {
                Graphics.TextG title = new Graphics.TextG(LoadedFont.Regular,
                    textBg.RealTopLeft, Engine.Screen.TextSizeV2, new Graphics.Align(Vector2.UnitY),
                    "Player Turn", Color.White, Layer);

                presentation.Add(title);
            }

            //if (toggRef.mode == GameMode.HeroQuest && 
            //    player.pData.teamIndex == HeroQuest.Players.PlayerCollection.HeroTeam)// &&
            //    //HeroQuest.hqRef.players.localHost.HeroUnit.Dead)
            //{
            //    cameraMoveTime.MilliSeconds = 60;
            //}
        }

        public bool update()
        {
            if (startFade.CountDown())
            {
                opacity -= Ref.DeltaTimeSec * 2f;

                presentation.SetOpacity(opacity);

                if (opacity <= 0)
                {
                    //DeleteMe();
                    return true;
                }
            }

            return false;
        }

        public void DeleteMe()
        {
            
                presentation.DeleteAll();
            
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class HistoryDisplay : HUD.AbsButtonGui
    {
        const int MaxCount = 32;

        Graphics.Text2 lastMessage;
        Graphics.Text2 messageQue = null;

        List2<string> messages = new List2<string>(MaxCount);

        public HistoryDisplay()
            : base(new HUD.ButtonGuiSettings(
                Color.CornflowerBlue, HudLib.SelectionOutlineThickness, 
                Color.White, Color.Black))
        {
            Vector2 pos = new Vector2(
                Engine.Screen.SafeArea.X,
                PlayerNameDisplay.Bottom());

            this.area = new VectorRect(pos, Engine.Screen.SmallIconSizeV2);

            baseImage = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, 
                HudLib.ContentLayer, false);
            baseImage.Color = Color.DarkBlue;

            Graphics.Image icon = new Graphics.Image(SpriteName.MenuPixelIconManual, 
                area.Position, area.Size,
                HudLib.ContentLayer, false);
            icon.LayerAbove(baseImage);

            lastMessage = new Graphics.Text2("", LoadedFont.Regular,
                VectorExt.AddX(area.RightCenter, Engine.Screen.BorderWidth), Engine.Screen.TextBreadHeight, Color.White, HudLib.ContentLayer);
            lastMessage.OrigoAtCenterHeight();

            add("Network debug display");
        }

        public void add(string text)
        {
            messages.SetMaxCount(MaxCount - 1);
            messages.AddFirst(text);

            lastMessage.TextString = text;
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            if (enter)
            {
                if (messageQue == null)
                {
                    StringBuilder text = new StringBuilder();
                    foreach (var m in messages)
                    {
                        text.Append("> ");
                        text.Append(m);
                        text.Append(Environment.NewLine);
                    }
                    messageQue = new Graphics.Text2(text.ToString(), LoadedFont.Regular,
                        area.LeftBottom, Engine.Screen.TextBreadHeight,
                        Color.Yellow, ImageLayers.AbsoluteTopLayer,
                        Engine.Screen.Height * 0.5f);
                    messageQue.outline = Graphics.TextOutlineType.ShadowSouthEast;
                    messageQue.outlineThickness = 1f;
                    messageQue.outlineColor = Color.Black;
                }

                lastMessage.Visible = false;
            }
            else
            {
                messageQue?.DeleteMe();
                messageQue = null;

                lastMessage.Visible = true;
            }
        }
    }
}

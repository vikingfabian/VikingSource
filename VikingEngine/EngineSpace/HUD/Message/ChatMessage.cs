using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    abstract class AbsChatMessage : AbsMessage
    {
        string message;
        int letter = 0;

        public AbsChatMessage()
        {
            images = new List<AbsDraw2D>();
        }

        protected void createTextAndBackground(Vector2 pos, Vector2 adjTextPos, float width, string text, 
            TextFormat textFormat, Color bgCol, float titleWidth, ImageLayers layer)
        {
            Image background;

            this.textBox = new TextBoxSimple(textFormat.Font, pos, textFormat.Scale, Align.Zero, text,
                textFormat.Color, layer - 1, width);
            message = this.textBox.TextString;

            this.textBox.Position += adjTextPos;

            Vector2 bgSize = textBox.MeasureText();
            bgSize.X = lib.LargestValue(bgSize.X, titleWidth) + 4;
            bgSize.Y += 17;
            background = new Image(SpriteName.WhiteArea, pos, bgSize, layer);
            background.Color = bgCol;
            //background.Color = Color.Black;
            //background.Opacity = 0.5f;
            area = new VectorRect(pos, textBox.MeasureText() + new Vector2(0, adjTextPos.Y));
            images.Add(this.textBox);
            images.Add(background);

            this.textBox.TextString = TextLib.EmptyString;
        }


        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (letter < message.Length)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (letter < message.Length)
                    {
                        this.textBox.AddChar(message[letter]);
                        letter++;
                    }
                }
            }
        }
    }

    class ChatMessage : AbsChatMessage
    {
        static readonly Vector2 AdjTextPos = new Vector2(0, 20);

        public ChatMessage(Vector2 pos, float width, string text, string senderName, 
            TextFormat titleFormat, TextFormat textFormat, Color bgCol, ImageLayers layer)
            :base()
        {

            TextG sender;
            sender = new TextG(titleFormat.Font, pos, titleFormat.Scale, new Align(new Vector2(0f, 0.10f)), senderName, titleFormat.Color, layer - 1);
            pos.Y += Engine.Screen.BorderWidth;//titleFormat.LineHeightAdjust;
            images.Add(sender);

            createTextAndBackground(pos, AdjTextPos, width, text, textFormat, bgCol, sender.MeasureText().X, layer);
        }
    }
}

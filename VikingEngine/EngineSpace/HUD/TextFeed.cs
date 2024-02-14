using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.HUD
{
    class TextFeed
    {
        VectorRect area;
        TextSettings defaultSettings;
        public float lineSpacing = Engine.Screen.BorderWidth;

        List<TextFeedMember> members = new List<TextFeedMember>(8);

        public TextFeed(VectorRect area, float textScale, LoadedFont font, Color defaultCol, ImageLayers layer)
        {
            this.area = area;
            defaultSettings = new TextSettings();

            defaultSettings.textScale = textScale;
            defaultSettings.font = font;
            defaultSettings.color = defaultCol;
            defaultSettings.layer = layer;
        }

        public void print(string text)
        {
            TextFeedMember m = new TextFeedMember(area, defaultSettings, text);
            members.Insert(0, m);

            //Refresh positions
            Vector2 pos = area.Position;
            for (int i = 0; i < members.Count; ++i)
            {
                members[i].refreshPostion(ref pos);
                pos.Y += lineSpacing;

                if (pos.Y >= area.Bottom)
                {
                    //Remove all from index
                    for (int removeIx = members.Count - 1; removeIx > i; --removeIx)
                    {
                        members[removeIx].DeleteMe();
                        members.RemoveAt(removeIx);
                    }

                    return;
                }
            }
        }

        public void DeleteMe()
        {
            for (int i = 0; i < members.Count; ++i)
            {
                members[i].DeleteMe();
            }
            members.Clear();
        }

        class TextFeedMember
        {
            Graphics.TextBoxSimple text;
            float height;

            public TextFeedMember(VectorRect area, TextSettings settings, string textstring)
            {
                text = new Graphics.TextBoxSimple(settings.font, area.Position, new Vector2(settings.textScale), Graphics.Align.Zero,
                    textstring, settings.color, settings.layer, area.Width);
                height = text.MeasureText().Y;
            }

            public void refreshPostion(ref Vector2 pos)
            {
                text.Position = pos;
                pos.Y += height;
            }

            public void DeleteMe()
            {
                text.DeleteMe();
            }
        }

        struct TextSettings
        {
            public float textScale;
            public LoadedFont font;
            public Color color;
            public ImageLayers layer;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Display
{
    class MessageGroup
    {
        Vector2 position;
        public RichboxGuiSettings settings;

        List<Message> messages = new List<Message>();

        public MessageGroup(RichboxGuiSettings settings)
        {
            this.settings = settings;
        }

        public void Add(string title, string text)
        {
            RichBoxContent content = new RichBoxContent();
            content.Add(new RichBoxBeginTitle(2));
            content.Add(new RichBoxImage(SpriteName.cmdWarningTriangle));
            content.space();
            content.Add(new RichBoxText(title, Color.Yellow));
            content.newLine();
            content.text(text);

            Add(content);
        }

        public void Add(RichBoxContent content)
        {
            messages.Insert(0, new Message(content, settings));
            UpdatePositions();
        }

        public void Update(Vector2 position)
        {
            if (this.position != position)
            { 
                this.position = position;
                UpdatePositions();
            }

            if (messages.Count > 0)
            {
                if (messages.Last().time.secPassed(20))
                {
                    arraylib.PullLastMember(messages).DeleteMe();
                } 
            }
        }

        void UpdatePositions()
        {
            Vector2 currentPos = position;
            if (messages.Count > 0)
            {                
                foreach (var message in messages)
                {
                    currentPos.Y += settings.edgeWidth * 2f;
                    currentPos = message.Update(currentPos);                    
                }
            }
        }
    }

    class Message
    {
        public RichBoxGroup richBox;
        protected RichBoxContent content = new RichBoxContent();
        protected Graphics.Image bg;
        public VectorRect area;
        public TimeStamp time;
        Vector2 contentOffset;

        public Message(RichBoxContent content, RichboxGuiSettings settings)
        {
            richBox = new RichBoxGroup(Vector2.Zero,
            settings.width, settings.contentLayer, settings.RbSettings, content, true, true, false);
            area = richBox.area;

            bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, settings.bglayer);
            bg.ColorAndAlpha(settings.bgCol, settings.bgAlpha);

            contentOffset = new Vector2(settings.edgeWidth);

            area.AddRadius(settings.edgeWidth);

            time = TimeStamp.Now();
        }

        public Vector2 Update(Vector2 position)
        {
            area.Position = position;
            bg.Area = area;
            richBox.SetOffset(area.Position + contentOffset);
            return area.LeftBottom;
        }

        public void DeleteMe()
        {
            bg.DeleteMe();
            richBox.DeleteAll();
        }
    }
}

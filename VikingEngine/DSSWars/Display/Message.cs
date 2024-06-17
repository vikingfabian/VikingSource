using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Display
{
    class MessageGroup
    {
        Vector2 position;
        public RichboxGuiSettings settings;

        List<Message> messages = new List<Message>();
        LocalPlayer player;
        public MessageGroup(LocalPlayer player, RichboxGuiSettings settings)
        {
            this.player = player;   
            this.settings = settings;
        }

        public void Title(RichBoxContent content, string title)
        {
            content.Add(new RichBoxBeginTitle(2));
            content.Add(new RichBoxImage(SpriteName.cmdWarningTriangle));
            content.space();
            content.Add(new RichBoxText(title, Color.Yellow));
            content.newLine();
        }

        public void Add(string title, string text)
        {
            RichBoxContent content = new RichBoxContent();
            //content.Add(new RichBoxBeginTitle(2));
            //content.Add(new RichBoxImage(SpriteName.cmdWarningTriangle));
            //content.space();
            //content.Add(new RichBoxText(title, Color.Yellow));
            //content.newLine();
            Title(content, title);
            content.text(text);

            Add(content);
        }

        public void Add(RichBoxContent content)
        {
            SoundLib.message.Play(Pan.Left);
            messages.Insert(0, new Message(player, content, settings));
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
                foreach (var message in messages)
                {
                    message.update();
                }

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
                    currentPos = message.UpdatePoisitions(currentPos);                    
                }
            }
        }

        public bool mouseOver()
        {
            foreach (var p in messages)
            {
                if (p.mouseOver())
                {
                    return true;
                }
            }

            return false;
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
        RbInteraction interaction;

        public Message(LocalPlayer player, RichBoxContent content, RichboxGuiSettings settings)
        {
            richBox = new RichBoxGroup(Vector2.Zero,
            settings.width, settings.contentLayer, settings.RbSettings, content, true, true, false);
            area = richBox.area;

            bg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.Zero, settings.bglayer);
            bg.ColorAndAlpha(settings.bgCol, settings.bgAlpha);

            contentOffset = new Vector2(settings.edgeWidth);

            area.AddRadius(settings.edgeWidth);
            time = TimeStamp.Now();
            interaction = new RbInteraction(content, settings.contentLayer,
                    player.input.RichboxGuiSelect);
        }

        public Vector2 UpdatePoisitions(Vector2 position)
        {
            area.Position = position;
            bg.Area = area;
            richBox.SetOffset(area.Position + contentOffset);
                       

            return area.LeftBottom;
        }

        public void update()
        { 
            interaction.update();
        }

        public bool mouseOver()
        {
            return bg.Area.IntersectPoint(Input.Mouse.Position);
        }

        public void DeleteMe()
        {
            interaction.DeleteMe();
            bg.DeleteMe();
            richBox.DeleteAll();
        }
    }
}

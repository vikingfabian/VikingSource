using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    interface IMessageHandlerParent
    {
        bool LockMessageHandler { get; }
    }

    class MessageHandler : AbsUpdateable
    {
        public static readonly LoadedFont StandardFont = LoadedFont.Regular;
        public const float LINE_WIDTH = 400;        
        
        IMessageHandlerParent parent;

        protected SpriteName backgroundImage;
        public Color BackgroundCol = Color.LightGray;
        protected TextFormat messageFormat = new TextFormat(StandardFont, 0.9f, Color.Black, ColorExt.Empty);
        public TextFormat ChatTitleFormat = new TextFormat(StandardFont, 0.65f, new Color(0, 93, 113), ColorExt.Empty);
       
        public TextFormat ChatTextFormat = new TextFormat(StandardFont, 0.8f, Color.Black, ColorExt.Empty);

        Color localChatBgCol = new Color(226,253,255);
        Color remoteChatBgCol = Color.White;

        public ImageLayers MessageLayer = ImageLayers.Background4;

        List<string> stored = new List<string>();
        List<AbsMessage> messages = new List<AbsMessage>();
        Vector2 startPos;
        public Vector2 Position
        {
            set { startPos = value; }
        }
        protected int maxlines;
        float messageLifeTime;
        float currentLifeTime = 0;
        public int KeepMessagesAlive = 0;
        protected float MessagesSpacing = 4;

        public MessageHandler(int maxlines, SpriteName background, float messageLifeTime)
            : this(maxlines, background, messageLifeTime, Engine.Screen.SafeArea.Position, null)
        { }

        public MessageHandler(int maxlines, SpriteName background, float messageLifeTime, Vector2 startPos, IMessageHandlerParent parent)
            :base(true)
        {
            this.parent = parent;
            this.startPos = startPos;
            this.backgroundImage = background;
            this.maxlines = maxlines;

            this.messageLifeTime = messageLifeTime;
        }

        public void PrintChat(ChatMessageData message, float maxWidth, bool localMessage = true)
        {
            addMessage(new ChatMessage(NextPosition(), maxWidth, message.Text, message.Sender + ":", ChatTitleFormat, ChatTextFormat, 
                localMessage? localChatBgCol : remoteChatBgCol, MessageLayer));
        }
        public void Print(string text)
        {
            addMessage(new MessageBlock(NextPosition(), LINE_WIDTH, backgroundImage, BackgroundCol, text, messageFormat, MessageLayer));
        }
        public void Print(string text, SpriteName icon)
        {
            addMessage(new MessageBlock(NextPosition(), LINE_WIDTH, backgroundImage, BackgroundCol, text, messageFormat, icon, MessageLayer));
        }

        public Vector2 NextPosition()
        {
            Vector2 pos = startPos;
            if (messages.Count > 0)
            {
                pos.Y = messages[messages.Count - 1].Bottom + MessagesSpacing;
            }
            return pos;
        }

        public void addMessage(AbsMessage message)
        {
            if (this.IsDeleted)
            { //to aviod messages pop up after removal
                message.DeleteMe();
                return;
            }
            bool bUpdatePos = false;
            if (messages.Count >= maxlines)
            {
                RemoveOldest();
                bUpdatePos = true;
            }
            if (messages.Count == 0)
            {
                currentLifeTime = 0;
            }

            messages.Add(message);
            if (!visible)
                message.Visible = visible;
            if (bUpdatePos)
            {
                updatePos();
            }
        }

       
        public override void Time_Update(float time)
        {
            if (parent == null || !parent.LockMessageHandler)
            {

                if (messages.Count > KeepMessagesAlive)
                {
                    currentLifeTime += time;
                    if (currentLifeTime >= messageLifeTime)
                    {
                        RemoveOldest();
                        updatePos();
                        currentLifeTime = 0;
                    }
                }
            }
        }
        void updatePos()
        {
            if (messages.Count > 0)
            {
                if (messages[0].Y > startPos.Y)
                {
                    float goal = startPos.Y;
                    foreach (AbsMessage m in messages)
                    {
                        m.GoalPos(goal);
                        goal += m.Height + MessagesSpacing;
                    }
                }
            }
        }

        void RemoveOldest()
        {
            if (messages.Count > 0)
            {
                messages[0].GoAway();
                float height = messages[0].Height;
                messages.Remove(messages[0]);
                currentLifeTime = 0;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            foreach (AbsMessage m in messages)
            {
                m.DeleteMe();
            }
        }

        bool visible = true;
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (value != visible)
                {
                    visible = value;
                    foreach (AbsMessage m in messages)
                    {
                        m.Visible = value;
                    }
                }
            }
        }
    }


    class ChatMessageData
    {
        public string Text;
        public string Sender;

        public ChatMessageData(string text, string sender)
        {
            Text = text;
            Sender = sender;
        }

        public ChatMessageData(System.IO.BinaryReader r)
        {
            ReadStream(r);
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            SaveLib.WriteString(w, Text);
            SaveLib.WriteString(w, Sender);

        }
        public void ReadStream(System.IO.BinaryReader r)
        {
            Text = SaveLib.ReadString_safe(r);
            Sender = SaveLib.ReadString_safe(r);
        }


    }

}

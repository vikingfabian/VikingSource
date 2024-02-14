using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.LF2 
{
    class QuestDialogue : LazyUpdate
    {
        Timer.Basic messageRate = new Timer.Basic(0);
 
        List<IQuestDialoguePart> dialogue;
        Players.Player p;
        object speaker;
        int lineIx = 0;
        public QuestDialogue(List<IQuestDialoguePart> dialogue, object speaker, Players.Player p)
            : base(true)
        {
            this.speaker = speaker;
            this.dialogue = dialogue;
            this.p = p;

            if (p.QuestDialogue != null)
            {
                p.QuestDialogue.QuickExit();
            }
            p.hero.RemoveActionPrompt();
            p.QuestDialogue = this;

            //NextMessage();
        }

        public override void Time_Update(float time)
        {
            if (messageRate.Update(time))
            {
                nextMessage();
            }
        }

        void nextMessage()
        {
            //p.exitCurrentMode();

            dialogue[lineIx].RunQuestDialoguePart(speaker, p);
            messageRate.Set(dialogue[lineIx].ViewTime);
            lineIx++;
            if (lineIx >= dialogue.Count)
            {
                DeleteMe();
            }
            p.hero.GotChatMessage();
        }

        //do all the high prio stuff at once
        public void QuickExit()
        {
            for (int i = lineIx; i < dialogue.Count; i++)
            {
                if (dialogue[i].highPrio)
                {
                    dialogue[i].RunQuestDialoguePart(speaker, p);
                }
            }
            DeleteMe();
        }
        public override void DeleteMe()
        {
            p.QuestDialogue = null;
            base.DeleteMe();
        }
    }

    interface IQuestDialoguePart
    {
        void RunQuestDialoguePart(object speaker, Players.Player p);
        float ViewTime { get; }
        bool highPrio { get; }
    }
    struct QuestDialogueSpeach : IQuestDialoguePart
    {
        string text;
        string overridingSpeakerName;
        LoadedSound sound;
        public QuestDialogueSpeach(string text, string speakerName, LoadedSound sound)
        { 
            this.text = text; overridingSpeakerName = speakerName;
            this.sound = sound;
        }
        public QuestDialogueSpeach(string text, LoadedSound sound)
        { 
            this.text = text; overridingSpeakerName = null;
            this.sound = sound;
        }

        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {
            ChatMessageData m = new ChatMessageData(text, overridingSpeakerName==null? speaker.ToString() : overridingSpeakerName);
            //string speakerName = ;
            p.PrintChat(m, sound);
            LfRef.gamestate.AddChat(m, true);
        }

        public float ViewTime { get { return text.Length * 15 + 1000; } }
        public bool highPrio { get { return false; } }
    }

    struct QuestDialogueMapLocationMessage : IQuestDialoguePart
    {
        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {
            p.MapLocationMessage();
        }
        public float ViewTime { get { return 800; } }
        public bool highPrio { get { return true; } }
    }

    struct QuestDialogueQuestMapLocation : IQuestDialoguePart
    {
        public QuestDialogueQuestMapLocation(Data.GeneralProgress progress)
        {
            LfRef.gamestate.Progress.GeneralProgress = progress;
        }

        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {

            LF2.Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
            LfRef.gamestate.Progress.SetQuestGoal(p.hero);  
        }
        public float ViewTime { get { return 800; } }
        public bool highPrio { get { return true; } }
    }
    struct QuestDialogueItemGift : IQuestDialoguePart
    {
        GameObjects.Gadgets.IGadget item;
        public QuestDialogueItemGift(GameObjects.Gadgets.IGadget item)
        {
            this.item = item;
        }

        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {
            LF2.Music.SoundManager.PlayFlatSound(LoadedSound.PickUp);
            p.AddItem(item, true);
        }
        public float ViewTime { get { return 800; } }
        public bool highPrio { get { return true; } }
    }
    struct QuestDialogueButtonTutorial : IQuestDialoguePart
    {
        Input.IDirectionalMap stick;
        Input.IButtonMap button;
        //bool buttonOrStick;
        string text;
        //SpriteName tile;
        public QuestDialogueButtonTutorial(Input.IButtonMap button, Input.IDirectionalMap stick, string text)
        {
            
            this.stick = stick;
            this.button = button;
            this.text = text;
            //this.tile = tile;
        }
        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {
            p.beginButtonTutorial(new ButtonTutorialArgs(button, stick, text, p.SafeScreenArea));
        }
        public float ViewTime { get { return 2000; } }
        public bool highPrio { get { return false; } }
    }
    struct QuestDialoguePause : IQuestDialoguePart
    {
        float pause;
        public QuestDialoguePause(float time)
        {
            this.pause = time;
        }
        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {
            //do nothing
        }
        public float ViewTime { get { return pause; } }
        public bool highPrio { get { return false; } }
    }

    struct QuestDialogueLargeCompassPulse : IQuestDialoguePart
    {
        //public QuestDialogueCompassPulse()
        //{
        //}
        public void RunQuestDialoguePart(object speaker, Players.Player p)
        {
            new CompassPulse(p);
        }
        public float ViewTime { get { return 200; } }
        public bool highPrio { get { return true; } }
    }
    //struct QuestDialogueSmallCompassPulse : IQuestDialoguePart
    //{
    //    //public QuestDialogueCompassPulse()
    //    //{
    //    //}
    //    public void RunQuestDialoguePart(object speaker, Players.Player p)
    //    {
    //        //Make the compass '!' pulse once
    //        const float LifeTime = 800;
    //        Graphics.Image expressPulse = new Graphics.Image(SpriteName.IconMapQuest, p.CompassQuestMarkPos, Vector2.One * Players.Player.CompassIconSz, ImageLayers.Lay9, true);
    //        new Graphics.Effects.Motion2d(Graphics.MotionType.SCALE, expressPulse, Vector2.One * 100, Graphics.MotionRepeate.NO_REPEATE, LifeTime, true);
    //        new Graphics.Effects.Motion2d(Graphics.MotionType.TRANSPARENSY, expressPulse, new Vector2NegativeOne, Graphics.MotionRepeate.NO_REPEATE, LifeTime, true);
    //        new Timer.Terminator(LifeTime, expressPulse);
    //    }
    //    public float ViewTime { get { return 200; } }
    //    public bool highPrio { get { return false; } }
    //}

}

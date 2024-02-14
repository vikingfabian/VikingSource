using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class Healer :AbsNPC
    {
        public Healer(Map.WorldPosition startPos,Data.Characters.NPCdata data)
            : base(startPos, data)
        {
            aggresive = Aggressive.Flee;
            Health = float.MaxValue;
            NetworkShareObject();
        }
        public Healer(System.IO.BinaryReader r, GameObjects.EnvironmentObj.MapChunkObjectType npcType)
            : base(r, npcType)
        {
            loadImage();
        }


        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            string say = null;

            switch (Ref.rnd.Int(4))
            {
                case 0:
                    say = "I will make you feel much better";
                    break;
                case 1:
                    say = "You need to undress and lie down";
                    break;
                case 2:
                    say = "Don't be shy, I've seen naked boys before";
                    break;
                case 3:
                    say = "Let me take a look at you";
                    break;
                case 4:
                    say = "Should I blow where it hurts?";//"Should I blow on your wound?";
                    break;

            }

            return new HUD.DialogueData(this, say);
        }
        override public File Interact_TalkMenu(Characters.Hero hero)
        {
            
            
            string say;
            if (hero.FullHealth)
            {
                switch (Ref.rnd.Int(2))
                {
                    default:
                        say = "Come back if you are hurt";
                        break;
                    case 1:
                        say = "You seem to be at full health";
                        break;
                 }
            }
            else
            {
                switch (Ref.rnd.Int(4))
                {
                    default:
                        say = "I can restore your life";
                        break;
                    case 1:
                        say = "Feeling better now?";
                        break;
                    case 2:
                        say = "Be more careful from now on!";
                        break;
                    case 3:
                        say = "There you go!";
                        break;

                 }
                hero.addHealth(float.MaxValue);
            }

            new QuestDialogue(
                new List<IQuestDialoguePart> { new QuestDialogueSpeach(say, LoadedSound.Dialogue_Neutral) }, this, hero.Player);

            return null;
            
        }
        protected override void mainTalkMenu(ref File file, Characters.Hero hero)
        {
            file = null;
        }

        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Healer; }
        }

        public override string ToString()
        {
            return "Healer";
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 20;
            }
        }
        protected override void loadImage()
        {
            new Process.LoadImage(this, VoxelModelName.healer, BasicPositionAdjust);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.light_gray, Data.MaterialType.white, Data.MaterialType.skin);
        }
        protected override bool hasQuestExpression
        {
            get { return false; }
        }
        protected override bool willTalk
        {
            get { return true; }
        }
        public const SpriteName HealerCompassIcon = SpriteName.LFHealIcon;
        public override SpriteName CompassIcon
        {
            get { return HealerCompassIcon; }
        }

        protected override bool Immortal
        {
            get { return true; }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            //System.Diagnostics.Debug.WriteLine("Healer pos: " + image.Position.ToString());
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }
    }
}

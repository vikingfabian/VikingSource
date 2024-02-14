using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class Mother: AbsNPC
    {
        public Mother(Map.WorldPosition position, Data.Characters.NPCdata chunkData)
            :base(position, chunkData)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Defending;
            imWho = chunkData.MapChunkObjectType;
            NetworkShareObject();
        }

        public Mother(System.IO.BinaryReader r, EnvironmentObj.MapChunkObjectType imWho)
            : base(r, imWho)
        {
            this.imWho = imWho;
            loadImage();
        }

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return null;
        }

       
        protected override void loadImage()
        {
            Graphics.AnimationsSettings anim = new Graphics.AnimationsSettings(7, 1.1f, 1);
            
            image = LF2.Data.ImageAutoLoad.AutoLoadImgReplacement(image, VoxelModelName.mother, TempCharacterImage, 1, anim);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.skin, Data.MaterialType.white, Data.MaterialType.black);
        }

        override public File Interact_TalkMenu(Characters.Hero hero)
        {
            string say;

            if (hero.FullHealth)
            {
                switch (Ref.rnd.Int(4))
                {
                    default:
                        say = "Dress warm before you travel, dear";
                        break;
                    case 1:
                        say = "Your father only means well";
                        break;
                    case 2:
                        say = "Could you buy some milk on your way home from this quest thing?";
                        break;
                    case 3:
                        say = "You boys only think about playing with swords and adventure";
                        break;
                }

            }
            else
            {
                switch (Ref.rnd.Int(4))
                {
                    default:
                        say = "What have you gotten yourself into now?";
                        break;
                    case 1:
                        say = "You look like a mess, wash off immedietly!";
                        break;
                    case 2:
                        say = "Oh no! I just bought those clothes.";
                        break;
                    case 3:
                        say = "It's about time that you start taking care of yourself!";
                        break;

                }
                hero.addHealth(float.MaxValue);
            }
            List<IQuestDialoguePart> mission1 = new List<IQuestDialoguePart>
            {
                 new QuestDialogueSpeach(say, LoadedSound.Dialogue_Neutral),

            };

            new QuestDialogue(
                mission1, this, hero.Player);

            return null;
        }

        protected override bool willTalk
        {
            get { return true; }
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Mother; }
        }
        public override string ToString()
        {
            return "Mother";
        }
        protected override bool hasQuestExpression
        {
            get { return false; }
        }

        protected override float scale
        {
            get
            {
                return 0.15f;
            }
        }

        public override SpriteName CompassIcon
        {
            get { return SpriteName.NO_IMAGE; }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }
}

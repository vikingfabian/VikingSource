using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class ImError : AbsNPC
    {
        string name;

        public ImError(Map.WorldPosition position, Data.Characters.NPCdata chunkData)
            :base(position, chunkData)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Defending;
            imWho = chunkData.MapChunkObjectType;
            setName();
            NetworkShareObject();
        }

        public ImError(System.IO.BinaryReader r, EnvironmentObj.MapChunkObjectType imWho)
            : base(r, imWho)
        {
            this.imWho = imWho;
            setName(); 
            loadImage();
        }

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return null;
        }

        void setName()
        {
            switch (imWho)
            {
                default:
                    name = "Error";
                    break;
                case EnvironmentObj.MapChunkObjectType.ImGlitch:
                    name = "Glitch";
                    break;
                case EnvironmentObj.MapChunkObjectType.ImBug:
                    name = "Bug";
                    break;

            }
        }

        protected override void loadImage()
        {
            Graphics.AnimationsSettings anim = new Graphics.AnimationsSettings(7, 1.1f, 1);
            VoxelModelName img;
            switch (imWho)
            {
                default:
                    img = VoxelModelName.im_error;
                    break;
                case EnvironmentObj.MapChunkObjectType.ImGlitch:
                    img = VoxelModelName.im_glitch;
                    break;
                case EnvironmentObj.MapChunkObjectType.ImBug:
                    img = VoxelModelName.im_bug;
                    anim.NumFramesPlusIdle = 4;
                    break;
            }
            image = LF2.Data.ImageAutoLoad.AutoLoadImgReplacement(image, img, TempCharacterImage, 1, anim);

            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.white, Data.MaterialType.black);
        }

        override public File Interact_TalkMenu(Characters.Hero hero)
        {
            
            List<IQuestDialoguePart> mission1 = new List<IQuestDialoguePart>
            {
                 new QuestDialogueSpeach("I am " + name, LoadedSound.Dialogue_Neutral),

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
            get { return imWho; }
        }
       
        public override string ToString()
        {
            return name;
        }
        public override string InteractionText
        {
            get
            {
                return name;
            }
        }
        protected override bool hasQuestExpression
        {
            get { return false; }
        }

        public override SpriteName CompassIcon
        {
            get { return SpriteName.NO_IMAGE; }
        }
        protected override bool Immortal
        {
            get { return false; }
        }
    }
}

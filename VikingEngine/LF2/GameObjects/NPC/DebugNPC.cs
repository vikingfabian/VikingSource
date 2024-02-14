using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.GameObjects.NPC
{
    class DebugNPC: AbsNPC
    {
        
        public DebugNPC(Map.WorldPosition position, Data.Characters.NPCdata chunkData)
            :base(position, chunkData)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Defending;
            imWho = chunkData.MapChunkObjectType;
           
            NetworkShareObject();
            Health = float.MaxValue;
        }

        public DebugNPC(System.IO.BinaryReader r, EnvironmentObj.MapChunkObjectType imWho)
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
            VoxelModelName img  = VoxelModelName.im_error;

            image = LF2.Data.ImageAutoLoad.AutoLoadImgReplacement(image, img, TempCharacterImage, 1, anim);

            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.red, Data.MaterialType.white, Data.MaterialType.black);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            if (image.position.Y < Map.WorldPosition.ChunkStandardHeight - 1)
            {
                image.position.Y = Map.WorldPosition.ChunkStandardHeight;
            }
        }

        protected override float walkingSpeed
        {
            get
            {
                return 0;
            }
        }

        override public File Interact_TalkMenu(Characters.Hero hero)
        {
            
            List<IQuestDialoguePart> mission1 = new List<IQuestDialoguePart>
            {
                 new QuestDialogueSpeach("Chunk" + WorldPosition.ChunkGrindex.ToString() + ", blockX:"  + WorldPosition.LocalBlockX.ToString(), LoadedSound.Dialogue_Neutral),

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
            get { return EnvironmentObj.MapChunkObjectType.ImError; }
        }
        public override string ToString()
        {
            return "Debug npc";
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

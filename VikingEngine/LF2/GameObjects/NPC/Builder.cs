using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.LF2.GameObjects.NPC
{
    //Talk to the builder to edit your private home
    class Builder: AbsNPC
    {
        public Builder(Map.WorldPosition position, Data.Characters.NPCdata chunkData)
            :base(position, chunkData)
        {
            socialLevel = SocialLevel.Interested;
            aggresive = Aggressive.Defending;
            this.imWho = EnvironmentObj.MapChunkObjectType.Builder;
            Health = float.MaxValue;
            NetworkShareObject();
        }

        public Builder(System.IO.BinaryReader r)
            : base(r, EnvironmentObj.MapChunkObjectType.Builder)
        {
            //this.imWho = EnvironmentObj.MapChunkObjectType.Builder;
            loadImage();
        }

        public override HUD.DialogueData Interact_OpeningPhrase(Characters.Hero hero)
        {
            return null;
        }

       
        protected override void loadImage()
        {
            Graphics.AnimationsSettings anim = new Graphics.AnimationsSettings(7, 1.1f, 2);

            image = LF2.Data.ImageAutoLoad.AutoLoadImgReplacement(image, VoxelModelName.housebuilder, TempCharacterImage, 1, anim);
            damageColors = new Effects.BouncingBlockColors(Data.MaterialType.skin, Data.MaterialType.orange, Data.MaterialType.brown);
        }

        bool haveSaidBuild, haveSaidTutorial;
        //override public File Interact_TalkMenu(Characters.Hero hero)
        //{
        //    File file = new File();
        //    file.AddIconTextLink(SpriteName.IconInfo, "Build", "How do you start building", new MenuLink(hero.Player, startHelp, 0),
        //        haveSaidBuild? SpriteName.LFMenuRectangleGray : SpriteName.NO_IMAGE);
        //    file.AddIconTextLink(SpriteName.IconInfo, "Tutorial", "Tips and tricks when building", new MenuLink(hero.Player, tutorial, 0),
        //        haveSaidTutorial ? SpriteName.LFMenuRectangleGray : SpriteName.NO_IMAGE);
            
        //    return file;
        //}

        void startHelp(Players.Player p, int non)
        {
            List<IQuestDialoguePart> say; //= new List<IQuestDialoguePart>();
            if (publicArea())
            {
                say = new List<IQuestDialoguePart>
                {
                    new QuestDialogueSpeach("Welcome to the public building area", LoadedSound.Dialogue_DidYouKnow),
                    new QuestDialogueSpeach("You and your friends, are all welcome to make your creations here", LoadedSound.Dialogue_Neutral),
                    new QuestDialogueSpeach("All changes will be saved to this world", LoadedSound.Dialogue_DidYouKnow),
                };
            }
            else
            {
                say = new List<IQuestDialoguePart>
                {
                    new QuestDialogueSpeach("This is a private home", LoadedSound.Dialogue_DidYouKnow),
                    new QuestDialogueSpeach("The owner will be able to build here", LoadedSound.Dialogue_Neutral),
                    new QuestDialogueSpeach("The home will look the same in all worlds", LoadedSound.Dialogue_DidYouKnow),
                };
            }

            say.Add(new QuestDialogueButtonTutorial(SpriteName.ButtonRB, numBUTTON.RB, null, "Use your build hammer"));
            //p.Settings.UnlockedBuildHammer = true;
            //p.SettingsChanged();
           // if (p.Settings.UnlockedBuildHammer)

            new QuestDialogue(
                say, this.ToString(), p);

            p.UnlockBuildHammer();
        }
        void tutorial(Players.Player p, int non)
        {
            p.CloseMenu();
            LfRef.gamestate.StartTutorial();
        }

        bool publicArea()
        {
            UpdateWorldPos();
            return LfRef.chunks.GetScreen(WorldPosition).chunkData.AreaType == Map.Terrain.AreaType.FreeBuild;
        }

        protected override bool willTalk
        {
            get { return true; }
        }
        protected override EnvironmentObj.MapChunkObjectType dataType
        {
            get { return EnvironmentObj.MapChunkObjectType.Builder; }
        }
        public override string ToString()
        {
            return "Builder";
        }
        protected override bool hasQuestExpression
        {
            get { return false; }
        }

        //public const SpriteName BuilderCompassIcon = SpriteName.LFIconCreativeMode;
        //public override SpriteName CompassIcon
        //{
        //    get { return BuilderCompassIcon; }
        //}

        protected override float maxWalkingLength
        {
            get
            {
                return 8;
            }
        }

        const float WalkingSpeed = StandardWalkingSpeed * 0.6f;
        protected override float walkingSpeed
        {
            get
            {
                return WalkingSpeed;
            }
        }
        protected override bool Immortal
        {
            get { return true; }
        }
    }
}

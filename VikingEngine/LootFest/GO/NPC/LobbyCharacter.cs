using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.NPC
{
    class LobbyCharacter : AbsNPC
    {
        VikingEngine.Graphics.Text3DBillboard namePlane;
        Vector3 namePlaneOffset;

        public AbsAvailableSession lobby;
        Time lifeTime = new Time(1, TimeUnit.Minutes);

        public LobbyCharacter(GoArgs args, AbsAvailableSession lobby)
            : base(args)
        {
            this.lobby = lobby;

            VoxelModelName modelName;
            if (lobby != null && lobby.friend)
            {
                //damageColors  
                modelName = VoxelModelName.lobby_character_friend;
                
                damageColors = new Effects.BouncingBlockColors(
                    Data.MaterialType.pastel_magenta, 
                    Data.MaterialType.gray_25, 
                    Data.MaterialType.pale_cool_brown);
            }
            else
            {
                modelName = VoxelModelName.lobby_character;

                damageColors = new Effects.BouncingBlockColors(
                    Data.MaterialType.dark_cyan_blue,
                    Data.MaterialType.gray_85,
                    Data.MaterialType.pale_cool_brown);
            }

            modelScale =  scale * 26;
            image = LfRef.modelLoad.AutoLoadModelInstance(modelName, modelScale);
            //postImageSetup();
            animSettings.TimePerFrameAndSpeed = scale * 24f;
            animSettings.NumFramesPlusIdle = 7;
            animSettings.NumIdleFrames = 1;
            image.position = startPos;

            SetAsManaged();
            Health = float.MaxValue;

            socialLevel = SocialLevel.Follower;
            aggresive = Aggressive.Flee;

            string text;
            if (lobby != null)
            {
                text = TextLib.FirstLettersDotDotDot(Engine.LoadContent.CheckCharsSafety(lobby.hostName, LoadedFont.Regular), 13);
            }
            else
            {
                text = "name name nam";
            }

            namePlane = new Graphics.Text3DBillboard(LoadedFont.Regular, text, Color.Black, null,
                Vector3.Zero, scale * 3.5f, 1f, true);
            namePlane.FaceCamera = false;
            namePlaneOffset = new Vector3(0f, modelScale * 0.4f, modelScale * 0.3f);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            namePlaneOffset = new Vector3(0f, modelScale * 0.27f, modelScale * 0.18f);
            namePlane.Rotation.RotationV3 = new Vector3(MathHelper.Pi - rotation.Radians, MathHelper.PiOver2, 0);
            //namePlane.Rotation.RotateWorldZ(MathHelper.PiOver2);
            namePlane.Position = image.Rotation.TranslateAlongAxis(namePlaneOffset, image.position);

            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
            }

            if (lifeTime.CountDown())
            {
                remove();
            }
        }

        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfChatBobbleIcon;
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            //start speech dialogue
            if (start)
            {
                if (lobby != null)
                {
                    LfRef.net.joinSessionLink(lobby);
                }
                    //startSpeechDialogue(hero);
                remove();
            }
        }

        public void remove()
        {
            Effects.EffectLib.DamageBlocks(16, image, damageColors);
            DeleteMe();
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            namePlane.DeleteMe();
        }

        protected override float maxWalkingLength
        {
            get
            {
                return 12;
            }
        }

        protected override float scale
        {
            get
            {
                return 0.86f;
            }
        }

        protected override float walkingSpeed
        {
            get
            {
                return StandardWalkingSpeed * 0.2f;
            }
        }

        protected override bool Immortal
        {
            get { return true; }
        }

        public override GameObjectType Type
        {
            get { throw new NotImplementedException(); }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return NetworkShare.None;
            }
        }

        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(5000, 8000);

        override protected float walkingModeTime
        {
            get { return WalkingModeTime.GetRandom(); }
        }
        override protected float waitingModeTime
        {
            get { return WaitingModeTime.GetRandom(); }
        }

        protected override bool waitingNpcWillLookAtHero()
        {
            return true;
        }
    }
}

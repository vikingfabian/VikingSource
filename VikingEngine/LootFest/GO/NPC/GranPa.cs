using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    class AmmoGranpa : AbsGranPa
    {
        GO.PickUp.SpecialAmmoAdd1 currentAmmoDispose = null;

        public AmmoGranpa(GoArgs args)
            :base(args)
        { 
        
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            if (args.halfUpdate == halfUpdateRandomBool)
            {
                Interact2_SearchPlayer(false);
            }

            updateSpeechDialogue();
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
                startSpeechDialogue(hero);
            }
        }

        protected override void startSpeechDialogue(PlayerCharacter.AbsHero hero)
        {
            base.startSpeechDialogue(hero);
            //speechbobble.attackTutorial();
            speechbobble.specialAttackTutorial();
            //speechbobble.jumpTutorial();
        }

        protected override void updateGranpaInteract(PlayerCharacter.AbsHero closestHero)
        {
            if (currentAmmoDispose == null)
            {
                if (closestHero != null && distanceToObject(closestHero) < 10f && closestHero.player.Gear.SpecialAttackAmmo < closestHero.player.Gear.suit.MaxSpecialAmmo)
                {
                    currentAmmoDispose = new GO.PickUp.SpecialAmmoAdd1(new GoArgs( image.position + VectorExt.V2toV3XZ(rotation.Direction(1f), 1.6f)));
                    const float AmmoStartVel = 0.005f;
                    currentAmmoDispose.Velocity.Set(rotation, AmmoStartVel);
                    currentAmmoDispose.Velocity.Y = AmmoStartVel;
                }
            }
            else if (currentAmmoDispose.IsDeleted)
            {
                currentAmmoDispose = null;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.AmmoGranPa; }
        }
    }

    class SuitGranpa : AbsGranPa
    {
        GO.PickUp.SuitBox currentSuitDispose = null;

        public SuitGranpa(GoArgs args)
            : base(args)
        {
            if (LfRef.progress.GotUnlock(ClosestHero(image.position, true).player.Storage, VikingEngine.LootFest.Players.UnlockType.Cards))
            {
                //var wp = args.startWp;
                args.startWp.X += 4;
                new GO.PickUp.ItemForSale(args, GO.PickUp.ForSaleType.Cards).managedGameObject = false;
            }
        
        }

        protected override void updateGranpaInteract(PlayerCharacter.AbsHero closestHero)
        {
            if (currentSuitDispose == null)
            {
                if (closestHero != null && distanceToObject(closestHero) < 10f && closestHero.player.Gear.suit.Type == SuitType.Basic)
                {
                    currentSuitDispose =  new GO.PickUp.SuitBox(new GoArgs( image.position + VectorExt.V2toV3XZ(rotation.Direction(1f), 1.6f)), Ref.rnd.Bool()? SuitType.Swordsman : SuitType.Archer);
                    const float AmmoStartVel = 0.005f;
                    currentSuitDispose.Velocity.Set(rotation, AmmoStartVel);
                    currentSuitDispose.Velocity.Y = AmmoStartVel;
                }
            }
            else if (currentSuitDispose.IsDeleted)
            {
                currentSuitDispose = null;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.SuitGranpa; }
        }
    }

    class TrollTutorialGranpa : AbsGranPa
    {
        public TrollTutorialGranpa(GoArgs args)
            : base(args)
        { }

        protected override void updateGranpaInteract(PlayerCharacter.AbsHero closestHero)
        {
            if (Interact2_HeroCollision(closestHero))
            {
                Interact2_PromptHero(closestHero);
            }
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            //start speech dialogue
            if (start)
            {
                targetHero = hero;
                SoundLib.NpcChatSound.PlayFlat();
                speechbobble = new Display.NpcSpeechBobble(hero.player);
                speechbobble.killTrollTutorial();
               
            }
        }
        public override SpriteName InteractVersion2_interactIcon
        {
            get
            {
                return SpriteName.LfChatBobbleIcon;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.TrollTutorialGranpa; }
        }
    }

    abstract class AbsGranPa: AbsNPC
    {
        public AbsGranPa(GoArgs args)
            :base(args)
        {
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.granpa2, 0f, 1f);
            //loadImage();
            postImageSetup();

            if (args.LocalMember)
            {
                socialLevel = SocialLevel.Follower;
                aggresive = Aggressive.Defending;
                NetworkShareObject();
            }
            animSettings = new Graphics.AnimationsSettings(3, 1.1f, 1);
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            var h = GetClosestHero(true);
            updateGranpaInteract(h);
        }

        abstract protected void updateGranpaInteract(PlayerCharacter.AbsHero closestHero);
        


        //protected override void loadImage()
        //{
        //    new Process.LoadImage(this, VoxelModelName.granpa2, BasicPositionAdjust);
        //}

        public override string ToString()
        {
            return "GranPa";
        }

        override protected float scale
        {
            get
            {
                return 0.14f;
            }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 2;
            }
        }

        const float WalkingSpeed = 0.0005f;
        override protected float walkingSpeed
        { get { return WalkingSpeed; } }


        //protected static readonly Graphics.AnimationsSettings AnimationSett = new Graphics.AnimationsSettings(3, 1.1f, 1);
        //protected override Graphics.AnimationsSettings AnimationsSettings
        //{
        //    get
        //    {
        //        return AnimationSett;
        //    }
        //}

        protected override bool Immortal
        {
            get { return true; }
        }

        static readonly IntervalF WalkingModeTime = new IntervalF(400, 600);
        static readonly IntervalF WaitingModeTime = new IntervalF(3000, 8000);

        override protected float walkingModeTime
        {
            get { return WalkingModeTime.GetRandom(); }
        }
        override protected float waitingModeTime
        {
            get { return WaitingModeTime.GetRandom(); }
        }

        
    }
}

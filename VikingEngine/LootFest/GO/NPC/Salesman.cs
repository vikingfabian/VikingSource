using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.NPC
{
    class Salesman : AbsNPC
    {
        public Salesman(GoArgs args)
            :base(args)
        {
            loadImage();
            postImageSetup();

            SetAsManaged();

            if (args.LocalMember)
            {
                socialLevel = SocialLevel.Follower;
                aggresive = Aggressive.Defending;

                GoArgs guardCopy = args;
                guardCopy.startWp.X += 6;
                new GO.NPC.Guard(guardCopy);
                guardCopy.startWp.X -= 12;
                new GO.NPC.Guard(guardCopy);
            }

            initSales();

            damageColors = new Effects.BouncingBlockColors(
                Data.MaterialType.pastel_red_orange,
                Data.MaterialType.pure_blue_violet,
                Data.MaterialType.gray_75);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }

            animSettings = StandardCharacterAnimations;
        }

        virtual protected void initSales()
        {
            GoArgs itemArgs = new GoArgs(WorldPos);
            
            itemArgs.startWp.WorldGrindex.Z += 3;

            itemArgs.startWp.WorldGrindex.X -= 4;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.HealthRefill);
            itemArgs.startWp.WorldGrindex.X += 3;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.SpecialAmmoRefill);
            itemArgs.startWp.WorldGrindex.X += 3;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.ItemApple);
            itemArgs.startWp.WorldGrindex.X += 3;
            itemArgs.startWp.SetAtClosestFreeY(1);
            itemArgs.updatePosV3();
            if (Ref.rnd.Chance(0.6f) && LfRef.progress.GotUnlock(ClosestHero(image.position, true).player.Storage, VikingEngine.LootFest.Players.UnlockType.Cards))
            {
                new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.Cards);
            }
            else
            {
                new GO.PickUp.ItemForSale(itemArgs, GO.PickUp.ForSaleType.ItemPie);
            }
            
        }

        protected override void loadImage()
        {
            image = new Graphics.VoxelModelInstance(
                LfRef.Images.StandardModel_Character);
            new Process.LoadImage(this, VoxelModelName.Salesman, BasicPositionAdjust);
        }

        public override string ToString()
        {
            return "Salesman";
        }

        override protected float scale
        {
            get
            {
                return 0.16f;
            }
        }
        protected override float maxWalkingLength
        {
            get
            {
                return 2;
            }
        }

        const float WalkingSpeed = 0.0012f;
        override protected float walkingSpeed
        { get { return WalkingSpeed; } }


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
        public override GameObjectType Type
        {
            get { return GameObjectType.Salesman; }
        }
    }
}

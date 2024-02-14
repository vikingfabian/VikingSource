using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.PickUp
{
    class Baby : AbsHeroPickUp
    {
        const float Scale = 2f;

        public VikingEngine.LootFest.Players.BabyLocation location;
       // Map.AbsWorldLevel lvl;
        //int lvlIteration;

        public Baby(GoArgs args)
            : base(args)
        {
            location = (Players.BabyLocation)args.characterLevel;

            WorldPos = args.startWp;
            WorldPos.SetAtClosestFreeY(2);
            
            image.position.Y = WorldPos.Y;
            Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.GoldenSparkle, image.position, 1, 120, 3);
            RotationSpeed = 0.001f;

            if (args.LocalMember)
            {
                physics.Gravity *= 0.4f;
                physics.WakeUp();
                physics.SpeedY = 0.01f;
                physics.MaxBounces = 4;

                NetworkShareObject();
            }
        }

        //public override void netWriteGameObject(System.IO.BinaryWriter w)
        //{
        //    base.netWriteGameObject(w);
        //    w.Write((byte)lvl.LevelEnum);
        //}


        protected override bool heroPickUp(PlayerCharacter.AbsHero hero)
        {
            hero.foundItemAnimation(VoxelModelName.baby, Scale, true);
            new Effects.BossDefeatedAnimation(WorldPos, location);
            return true;
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            //if (lvl.LevelIteration != lvlIteration)
            //{
            //    DeleteMe();
            //}
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        //public //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255, 215, 171), new Vector3(1.4f, 1.4f, 3f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}
        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.baby; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Baby; }
        }

        protected override bool giveStartSpeed
        {
            get
            {
                return false;
            }
        }

        protected override bool timedRemoval
        {
            get
            {
                return false;
            }
        }
        protected override bool autoMoveTowardsHero
        {
            get
            {
                return false;
            }
        }

        protected override bool rotating
        {
            get
            {
                return base.rotating;
            }
        }
        public override void Force(Vector3 center, float force)
        {
            //do nothing
        }

        protected override void checkActiveUpdate()
        {
            //base.checkActiveUpdate();
        }
        
        override protected float imageScale { get { return Scale; } }

        override public bool HelpfulLooterTarget { get { return true; } }
    }
}

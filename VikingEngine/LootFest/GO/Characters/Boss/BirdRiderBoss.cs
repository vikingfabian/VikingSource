using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class BirdRiderBoss: GoblinBerserk
    {
        public BirdRiderBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
           // this.subLevel = subLevel;
            alwaysFullAttension();

            var mount = new BirdRiderMount(args, level);
            MountAnimal(mount);

            if (level != null)
            {
                var manager = new Director.BossManager(this, level, Players.BabyLocation.NUM);
                manager.addBossObject(mount, false);
            }
        }

       

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            if (isMounted)
            {
                return false;
            }
            return base.willReceiveDamage(damage);
        }
        override public CardType CardCaptureType
        {
            get { return CardType.BirdRiderBoss; }
        }
        
        //public override void Time_Update(UpdateArgs args)
        //{
        //    base.Time_Update(args);
        //    //if (subLevel != null)
        //    //{ 
        //    //    subLevel.CheckLevelBounds(this);
        //    //}
        //}
    }

    class BirdRiderMount : FatBird
    {
        Vector3 riderOffset;
        int extraEggs = 8;

        public BirdRiderMount(GoArgs args, BlockMap.AbsLevel level)
            : base(new GoArgs(GameObjectType.NUM_NON, args.startWp, 1))
        {
            //this.subLevel = subLevel;
            alwaysFullAttension();

            
        }

        public override MountType MountType
        {
            get
            {
                return  GO.MountType.Mount;
            }
        }
        protected override Vector3 mountSaddlePos()
        {
            riderOffset = new Vector3(0, 0.5f, -0.26f) * modelScale;

            Vector3 result = image.Rotation.TranslateAlongAxis(riderOffset, image.position);
            return result;
            //return base.mountSaddlePos();
        }

        public override void onGroundPounce(float fallSpeed)
        {
            base.onGroundPounce(fallSpeed);
            if (extraEggs-- > 0)
            {
                eggCount = 1;
            }
        }

        override public CardType CardCaptureType
        {
            get { return CardType.BirdRiderBoss; }
        }
        //public override void Time_Update(UpdateArgs args)
        //{
        //    base.Time_Update(args);
        //    if (subLevel != null)
        //    { subLevel.CheckLevelBounds(this); }
        //}
    }
}

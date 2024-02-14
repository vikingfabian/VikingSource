using VikingEngine.LootFest.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters
{
    class GoblinWolfRiderBoss : GoblinBerserk
    {
        public GoblinWolfRiderBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            if (localMember)
            {
                alwaysFullAttension();

                var mount = new WolfRiderMount(args, level);
                MountAnimal(mount);

                var manager = new Director.BossManager(this, level, Players.BabyLocation.Horse);
                manager.addBossObject(mount, false);

                if (args.characterLevel == 1)
                {
                    args.startWp.X -= 4;
                    new GreatWolf(args).alwaysFullAttension();

                    args.startWp.X += 8;
                    new GreatWolf(args).alwaysFullAttension();
                }

                //NetworkShareObject();
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
    }

    class OrcWolfRiderBoss : OrcKnight
    {
        public OrcWolfRiderBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            if (localMember)
            {
                //this.subLevel = subLevel;
                alwaysFullAttension();

                var mount = new WolfRiderMount(args, level);
                MountAnimal(mount);

                var manager = new Director.BossManager(this, level, Players.BabyLocation.Horse);
                manager.addBossObject(mount, false);

                //NetworkShareObject();
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
    }


    class WolfRiderMount : GreatWolf
    {
        public WolfRiderMount(GoArgs args, BlockMap.AbsLevel level)
            :base(args)
        {
             //this.subLevel = subLevel;
             alwaysFullAttension();
        }

        //public WolfRiderMount(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}
       
        //public override void Time_Update(UpdateArgs args)
        //{
        //    base.Time_Update(args);
        //    if (subLevel != null)
        //    { subLevel.CheckLevelBounds(this); }
        //}
    }
}

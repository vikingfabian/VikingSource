using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.GO.Characters.Monster3;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class BeetleRiderBoss : GoblinBerserk
    {
        public BeetleRiderBoss(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            if (localMember)
            {
                attentionStatus = HumanoidEnemyAttentionStatus.FoundHero_5;

                var mount = new Beetle(new GoArgs( args.startPos, 1));
                MountAnimal(mount);

                var manager = new Director.BossManager(this, level, Players.BabyLocation.NUM);
                manager.addBossObject(mount, false);

                
                GoArgs minionArgs = new GoArgs(args.startWp);
                minionArgs.startWp.X -= 8;
                for (int i = 0; i < 4; ++i)
                {
                    minionArgs.updatePosV3();
                    new Beetle(args);

                    minionArgs.startWp.X += 2;
                }
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
}

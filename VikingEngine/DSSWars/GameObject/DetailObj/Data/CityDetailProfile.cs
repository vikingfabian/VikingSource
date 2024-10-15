using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class CityDetailProfile : AbsDetailUnitProfile
    {
        public const float ShortRangeAttack = 1.5f;
        public const float LongRangeAttack = 3.01f;
        public CityDetailProfile()
        {
            data.mainAttack = AttackType.Arrow;
            data.secondaryAttack = AttackType.Ballista;
            data.attackDamage = DssConst.Soldier_DefaultHealth;
            data.attackDamageSea = data.attackDamage;
            data.secondaryAttackDamage = data.attackDamage / 2;
            data.attackRange = LongRangeAttack;
            targetSpotRange = StandardTargetSpotRange;
        }

        public override AbsSoldierUnit CreateUnit()
        {
            throw new NotImplementedException();
        }
    }

}

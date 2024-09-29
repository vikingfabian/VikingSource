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
            mainAttack = AttackType.Arrow;
            secondaryAttack = AttackType.Ballista;
            attackDamage = DssConst.Soldier_DefaultHealth;
            attackDamageSea = attackDamage;
            secondaryAttackDamage = attackDamage / 2;
            attackRange = LongRangeAttack;
            targetSpotRange = StandardTargetSpotRange;
        }

        public override AbsDetailUnit CreateUnit()
        {
            throw new NotImplementedException();
        }
    }

}

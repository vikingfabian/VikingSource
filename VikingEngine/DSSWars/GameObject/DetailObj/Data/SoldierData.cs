using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Data
{
    struct SoldierData
    {
        public int basehealth=0;
        public AttackType mainAttack=0;
        public AttackType secondaryAttack = 0;
        public int bonusProjectiles = 0;
        public int attackDamage = 0, attackDamageSea = 0, attackDamageStructure = 0;
        public int secondaryAttackDamage = 0;
        public float attackTimePlusCoolDown = 0;

        public float attackRange = 0;
        public float secondaryAttackRange = 0;

        public LootFest.VoxelModelName modelName = 0;
        public int modelVariationCount = 1;
        public SpriteName icon = SpriteName.MissingImage;

        public ArmyPlacement ArmyFrontToBackPlacement = 0;
        public float energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep;
        public float walkingSpeed = DssConst.Men_StandardWalkingSpeed;
        public bool canAttackCharacters = true;
        public bool canAttackStructure = true;

        public SoldierData()
        { }

        public LootFest.VoxelModelName RandomModelName()
        {
            if (modelVariationCount == 1)
            {
                return modelName;
            }

            double rnd = Ref.rnd.Double();
            if (modelVariationCount >= 3 && rnd < 0.1)
            {
                return modelName + 2;
            }
            else if (rnd < 0.35)
            {
                return modelName + 1;
            }
            else
            {
                return modelName;
            }
        }

        public int DPS_land()
        {
            return Convert.ToInt32(attackDamage / (attackTimePlusCoolDown / 1000.0));
        }
        public int DPS_sea()
        {
            return Convert.ToInt32(attackDamageSea / (attackTimePlusCoolDown / 1000.0));
        }
        public int DPS_structure()
        {
            return Convert.ToInt32(attackDamageStructure / (attackTimePlusCoolDown / 1000.0));
        }
    }
}

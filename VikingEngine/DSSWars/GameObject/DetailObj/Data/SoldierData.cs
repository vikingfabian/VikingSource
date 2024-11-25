using Microsoft.Xna.Framework;
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
        public bool arrowWeakness = false;
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
        public float modelScale = DssConst.Men_StandardModelScale;

        public float upkeepPerSoldier = DssLib.SoldierDefaultUpkeep;
        public int workForcePerUnit = 1;
        public float groupSpacing = DssVar.DefaultGroupSpacing;
        public float groupSpacingRndOffset = DssVar.StandardBoundRadius * 0.3f;
        public int rowWidth = DssConst.SoldierGroup_RowWidth;
        public int columnsDepth = DssConst.SoldierGroup_ColumnsDepth;
        public Vector3 modelToShadowScale = new Vector3(0.4f, 1f, 0.32f);

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

        public int workForceCount()
        {
            return rowWidth * columnsDepth * workForcePerUnit;
        }

        public int Upkeep()
        {
            return Convert.ToInt32(rowWidth * columnsDepth * upkeepPerSoldier);
        }

        public Vector3 ShadowModelScale()
        {
            return modelToShadowScale * DssConst.Men_StandardModelScale;
        }
    }
}

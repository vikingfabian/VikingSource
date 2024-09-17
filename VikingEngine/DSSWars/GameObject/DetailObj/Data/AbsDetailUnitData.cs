using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsDetailUnitData
    {
        
        


        protected const float StandardTargetSpotRange = 3;
        
        public const float SpotEnemyLengh = 1.5f;

        public LootFest.VoxelModelName modelName;
        public int modelVariationCount = 1;
        public float modelScale;
        public float modelAdjY = 0;
        public float boundRadius;       

        public float attackRange;
        public float secondaryAttackRange;
        public int basehealth;
        //public int shieldDamageReduction = 0;
        public bool turnTowardsDamage = false;

        public AttackType mainAttack;
        public AttackType secondaryAttack;
        public int bonusProjectiles = 0;
        public int attackDamage, attackDamageSea, attackDamageStructure;// = 5;
        public int splashDamageCount = 0;
        public float percSplashDamage = 0.6f;
        public int secondaryAttackDamage;
        public float attackTimePlusCoolDown;
        //public int attackSetCount = 0;

        public float attackSetCoolDown;
        //public int splashDamage = 0;
        //public float splashRadius;

        public float targetSpotRange;// = 5f;
        public bool canBeAttackTarget = true;
        public bool scoutMovement = false;

        public float walkingSpeed;
        public float shipSpeed = DssConst.Men_StandardShipSpeed;

        public int idleFrame = 0, idleBlinkFrame = 1, attackFrame = 2;
        public float attackFrameTime = 400;

        public bool restrictAngle = false; 
        public float angle;

        abstract public AbsDetailUnit CreateUnit();

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
    }

   
}

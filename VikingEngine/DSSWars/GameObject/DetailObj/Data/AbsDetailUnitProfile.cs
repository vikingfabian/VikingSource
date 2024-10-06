using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsDetailUnitProfile
    {
        public SoldierData data = new SoldierData();

        protected const float StandardTargetSpotRange = 3;
        
        public const float SpotEnemyLengh = 1.5f;

        
        //public float modelScale;
        public float modelAdjY = 0;
        public float boundRadius;       

        
        
        //public int shieldDamageReduction = 0;
        //public bool turnTowardsDamage = false;

       
        //public int attackSetCount = 0;

        public float attackSetCoolDown;
        //public int splashDamage = 0;
        //public float splashRadius;

        public float targetSpotRange;// = 5f;
        public bool canBeAttackTarget = true;
        //public bool scoutMovement = false;

        
        //public float shipSpeed = DssConst.Men_StandardShipSpeed;

        public int idleFrame = 0, idleBlinkFrame = 1, attackFrame = 2;
        public float attackFrameTime = 400;

        public bool restrictTargetAngle = false; 
        public float targetAngle;

        abstract public AbsSoldierUnit CreateUnit();

        
    }

   
}

using Microsoft.Xna.Framework;
using System;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsSoldierData : AbsDetailUnitData
    {

        public const int RowWidth = 6;
        public const int ColumnsDepth = 5;
        public const int GroupDefaultCount = RowWidth * ColumnsDepth;

        public UnitType convertSoldierShipType;
        public int rowWidth = RowWidth;
        public int columnsDepth = ColumnsDepth;
        public ArmyPlacement ArmyFrontToBackPlacement = 0;
        protected int workForcePerUnit = 1;
        public int goldCost = DssLib.GroupDefaultCost;

        public float upkeepPerSoldier = DssLib.SoldierDefaultUpkeep;
        public float energyPerSoldier =  DssLib.SoldierDefaultEnergyUpkeep;

        public int recruitTrainingTimeSec = DssLib.DefalutRecruitTrainingTimeSec;

        protected const float StandardRotatingSpeed = 6.5f;

        public const float StandardBoundRadius = 0.3f * StandardModelScale;
        public const int DefaultHealth = 200;
        public const float DefaultGroupSpacing = StandardBoundRadius * 3f;

        public float groupSpacing = DefaultGroupSpacing;
        public float groupSpacingRndOffset = StandardBoundRadius * 0.3f;
        public float rotationSpeed;
        public float walkingWaggleAngle = 0.16f;
        public float ShipBuildTimeMultiplier = 1;

        public float maxAttackAngle = 0.15f;
        
        public Vector3 modelToShadowScale = new Vector3(0.4f, 1f, 0.32f);

        public bool canAttackCharacters = true;
        public bool canAttackStructure = true;
        public bool hasBannerMan = true;
        public string description;

        public UnitType unitType;

        public bool Command_Javelin=false;

        /// <summary>
        /// Add to basic speed, +1 is double, -1 is half (percentage is halved)
        /// </summary>
        public double ArmySpeedBonusLand = 0;
        public double ArmySpeedBonusSea = 0;

        public Vector3 captainPosDiff;
        public Vector3 leftCrewPosDiff;
        public int factionUniqueType = -1;

        public SpriteName icon = SpriteName.MissingImage;

        public Vector3 ShadowModelScale()
        {
            return modelToShadowScale * modelScale;
        }

        public void setupJavelinCommand()
        {
            Command_Javelin = true;
            secondaryAttack = AttackType.Javelin;
            secondaryAttackDamage = 100;
            secondaryAttackRange = 1f;
        }

        public override AbsDetailUnit CreateUnit()
        {
            
            return new BaseSoldier();
        }

        public int workForceCount()
        {
            return rowWidth * columnsDepth * workForcePerUnit;
        }

        public int Upkeep()
        {
            return Convert.ToInt32(rowWidth * columnsDepth * upkeepPerSoldier);
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

        virtual public bool IsShip()
        {
            return false;
        }

    }
}

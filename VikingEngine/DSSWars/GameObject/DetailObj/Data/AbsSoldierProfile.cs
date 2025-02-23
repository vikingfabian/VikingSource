﻿using Microsoft.Xna.Framework;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Conscript;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsSoldierProfile : AbsDetailUnitProfile
    {        
        //public UnitType /*convertSoldierShipType*/;
        public int rowWidth =DssConst.SoldierGroup_RowWidth;
        public int columnsDepth = DssConst.SoldierGroup_ColumnsDepth;

        protected int workForcePerUnit = 1;
        public int goldCost = DssLib.GroupDefaultCost;

        public float upkeepPerSoldier = DssLib.SoldierDefaultUpkeep;
        
        //public int recruitTrainingTimeSec = DssLib.DefalutRecruitTrainingTimeSec;

        protected const float StandardRotatingSpeed = 6.5f;

        public float groupSpacing = DssVar.DefaultGroupSpacing;
        public float groupSpacingRndOffset = DssVar.StandardBoundRadius * 0.3f;

        public float rotationSpeed;
        public float walkingWaggleAngle = 0.16f;
        public float ShipBuildTimeMultiplier = 1;

        public float maxAttackAngle = 0.15f;
        
        public Vector3 modelToShadowScale = new Vector3(0.4f, 1f, 0.32f);

        
        public bool hasBannerMan = true;
        public string description;

        //public SoldierProfile profile;
        public UnitType unitType;

        public bool Command_Javelin=false;

        /// <summary>
        /// Add to basic speed, +1 is double, -1 is half (percentage is halved)
        /// </summary>
        //public double ArmySpeedBonusLand = 0;
        //public double ArmySpeedBonusSea = 0;

        //public Vector3 captainPosDiff;
        //public Vector3 leftCrewPosDiff;
        public int factionUniqueType = -1;

        

        public Vector3 ShadowModelScale()
        {
            return modelToShadowScale * DssConst.Men_StandardModelScale;
        }

        //public void setupJavelinCommand()
        //{
        //    Command_Javelin = true;
        //    secondaryAttack = AttackType.Javelin;
        //    secondaryAttackDamage = 100;
        //    secondaryAttackRange = 1f;
        //}

        public override AbsSoldierUnit CreateUnit()
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

        virtual public UnitType ShipType()
        {
            return UnitType.ConscriptWarship;
        }


        virtual public bool IsShip()
        {
            return false;
        }

        virtual public void writeGameState(System.IO.BinaryWriter w)
        {
           throw new NotImplementedException();
        }
        virtual public void readGameState(System.IO.BinaryReader r)
        {
           throw new NotImplementedException();
        }

    }
}

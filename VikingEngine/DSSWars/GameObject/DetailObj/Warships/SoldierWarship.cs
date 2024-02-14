using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.MoonFall.GO;

namespace VikingEngine.DSSWars.GameObject
{
    //class RecruitWarshipData : AbsWarShipData
    //{
    //    public RecruitWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
    //        :base(shipUnitType, soldierData)
    //    {
            
    //        modelName = LootFest.VoxelModelName.wars_soldier_ship;
    //        captainPosDiff = new Vector3(0, 0.28f, -.3f);
    //        leftCrewPosDiff = new Vector3(-0.076f, 0.14f, 0.07f);
    //    }
    //}

    class SoldierWarshipData: AbsWarShipData
    {
        public SoldierWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
            :base(shipUnitType, soldierData)
        {
            
            modelName = LootFest.VoxelModelName.wars_soldier_ship;
            captainPosDiff = new Vector3(0, 0.28f, -.3f);
            leftCrewPosDiff = new Vector3(-0.076f, 0.14f, 0.07f);
        }
    }

    class ArcherWarshipData : AbsWarShipData
    {
        public ArcherWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
            :base(shipUnitType, soldierData)
        {
            modelName = LootFest.VoxelModelName.wars_archer_ship;
            captainPosDiff = new Vector3(0, 0.28f, -.3f);
            leftCrewPosDiff = new Vector3(-0.076f, 0.14f, 0.07f);
        }
    }

    class BallistaWarshipData : AbsWarShipData
    {
        public BallistaWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
            : base(shipUnitType, soldierData)
        {
            modelName = LootFest.VoxelModelName.wars_ballista_ship;
            captainPosDiff = new Vector3(0, 0.14f, -0.05f);
            leftCrewPosDiff = new Vector3(-0.076f, 0.14f, 0.00f);
        }
    }

    class VikingWarshipData : AbsWarShipData
    {
        public VikingWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
            : base(shipUnitType, soldierData)
        {
            modelName = LootFest.VoxelModelName.wars_viking_ship;
            captainPosDiff = new Vector3(0.05f, 0.12f, -0.34f);
            leftCrewPosDiff = new Vector3(-0.076f, 0.12f, -0.05f);

            mainAttack = AttackType.Javelin;
            attackTimePlusCoolDown = StandardAttackAndCoolDownTime * 2.5f;
            attackRange = 1f;

            walkingSpeed *= 1.5f;
        }
    }

    class KnightWarshipData : AbsWarShipData
    {
        public KnightWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
            : base(shipUnitType, soldierData)
        {
            modelName = LootFest.VoxelModelName.wars_knight_ship;
            captainPosDiff = new Vector3(0, 0.34f, -.3f);
            leftCrewPosDiff = new Vector3(-0.076f, 0.17f, 0.06f);
            
        }
    }

    class FolkWarshipData : AbsWarShipData
    {
        public FolkWarshipData(UnitType shipUnitType, AbsSoldierData soldierData)
            : base(shipUnitType, soldierData)
        {
            modelName = LootFest.VoxelModelName.wars_folk_ship;
            captainPosDiff = new Vector3(-0.05f, 0.18f, -.27f);
            leftCrewPosDiff = new Vector3(-0.076f, 0.12f, 0.065f);
        }
    }
}

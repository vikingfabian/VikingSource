using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Conscript;

namespace VikingEngine.DSSWars.GameObject
{
    class ConscriptedWarshipData : AbsWarShipData
    {
        public ConscriptedWarshipData(AbsSoldierData soldierData, SoldierProfile profile)
            : base(UnitType.ConscriptWarship, soldierData)
        {
            if (profile.conscript.specialization == SpecializationType.Sea)
            {
                if (!profile.RangedUnit())
                {
                    modelName = LootFest.VoxelModelName.wars_viking_ship;
                    captainPosDiff = new Vector3(0.05f, 0.12f, -0.34f);
                    leftCrewPosDiff = new Vector3(-0.076f, 0.12f, -0.05f);

                    mainAttack = AttackType.Javelin;
                    attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                    attackRange = 1f;
                }
                walkingSpeed *= 1.5f;
            }

            if (modelName != LootFest.VoxelModelName.wars_viking_ship)
            {
                switch (profile.conscript.weapon)
                {
                    case MainWeapon.SharpStick:
                        modelName = LootFest.VoxelModelName.wars_folk_ship;
                        captainPosDiff = new Vector3(-0.05f, 0.18f, -.27f);
                        leftCrewPosDiff = new Vector3(-0.076f, 0.12f, 0.065f);
                        break;
                    case MainWeapon.Sword:
                        modelName = LootFest.VoxelModelName.wars_soldier_ship;
                        captainPosDiff = new Vector3(0, 0.28f, -.3f);
                        leftCrewPosDiff = new Vector3(-0.076f, 0.14f, 0.07f);
                        break;
                    case MainWeapon.Bow:
                        modelName = LootFest.VoxelModelName.wars_archer_ship;
                        captainPosDiff = new Vector3(0, 0.28f, -.3f);
                        leftCrewPosDiff = new Vector3(-0.076f, 0.14f, 0.07f);
                        break;
                }
            }

        }

        
    }
}

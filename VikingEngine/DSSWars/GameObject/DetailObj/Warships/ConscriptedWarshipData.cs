using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Conscript;

namespace VikingEngine.DSSWars.GameObject
{
    class ConscriptedWarshipData : AbsWarShipProfile
    {
        public ConscriptedWarshipData()
            : base(UnitType.ConscriptWarship)
        {
            //if (profile.conscript.specialization == SpecializationType.Sea)
            //{
            //    if (!profile.RangedUnit())
            //    {
            //        modelName = LootFest.VoxelModelName.wars_viking_ship;
                    

            //        mainAttack = AttackType.Javelin;
            //        attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
            //        attackRange = 1f;
            //    }
            //    walkingSpeed *= 1.5f;
            //}

            //if (modelName != LootFest.VoxelModelName.wars_viking_ship)
            //{
            //    switch (profile.conscript.weapon)
            //    {
            //        case MainWeapon.SharpStick:
            //            modelName = LootFest.VoxelModelName.wars_folk_ship;
                        
            //            break;
            //        case MainWeapon.Sword:
            //            modelName = LootFest.VoxelModelName.wars_soldier_ship;
                        
            //            break;
            //        case MainWeapon.Bow:
            //            modelName = LootFest.VoxelModelName.wars_archer_ship;
                        
            //            break;
            //    }
            //}

        }

        public override AbsSoldierUnit CreateUnit()
        {

            AbsSoldierUnit result = base.CreateUnit();
            //if (result.group.soldierConscript.conscript.specialization == SpecializationType.DarkLord)
            //{
            //    DssRef.settings.darkLordPlayer.darkLordUnit = result;
            //}
            return result;
        }


    }
}

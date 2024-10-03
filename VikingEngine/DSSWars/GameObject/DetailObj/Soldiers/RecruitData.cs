using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;

namespace VikingEngine.DSSWars.GameObject
{
    //class RecruitData : AbsSoldierProfile
    //{
    //    public UnitType recruitForType;
    //    public RecruitData(AbsSoldierProfile recruitFor)
    //    {
    //        unitType = UnitType.Recruit;
    //        recruitForType = recruitFor.unitType;

    //        modelScale = DssConst.Men_StandardModelScale * 0.9f;
    //        boundRadius = DssVar.StandardBoundRadius * 0.9f;

    //        walkingSpeed = DssConst.Men_StandardWalkingSpeed;
    //        rotationSpeed = StandardRotatingSpeed;
    //        targetSpotRange = StandardTargetSpotRange;
    //        attackRange = 0.04f;
    //        basehealth = MathExt.MultiplyInt(0.25, DssConst.Soldier_DefaultHealth);
    //        mainAttack = AttackType.Melee;
    //        attackDamage = 5;
    //        attackDamageStructure = attackDamage;
    //        attackDamageSea = attackDamage;
    //        attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2f;

    //        groupSpacing = recruitFor.groupSpacing;
    //        ArmyFrontToBackPlacement = recruitFor.ArmyFrontToBackPlacement;

    //        modelName = LootFest.VoxelModelName.war_recruit;
    //        icon = recruitFor.icon;
    //    }

    //    public override AbsDetailUnit CreateUnit()
    //    {
    //        return new Recruit();
    ////    }
    //}

    //class Recruit : BaseSoldier
    //{
    //    public Recruit()
    //        : base()
    //    {

    //    }

    //    public override void DeleteMe(DeleteReason reason, bool removeFromParent)
    //    {
    //        if (reason == DeleteReason.Death &&
    //            group.soldiers.Array[parentArrayIndex] != this)
    //        {
    //            //Is already transformed and deleted
    //            return;
    //        }
    //        base.DeleteMe(reason, removeFromParent);
    //    }

    //}
}

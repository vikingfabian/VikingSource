using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class HoundBuilder : ConscriptedSoldierBuilder
    {
        public HoundBuilder() : base() 
        { unitBuildType = UnitBuildType.ConscriptHound; }

        public override AbsSoldierUnit CreateUnit(bool bannerman)
        {
            return new Hound();
        }
    }

    class Hound : BaseSoldier
    {
        public Hound() : base()
        {

        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            updateGroudY(true);
            
            return new HoundModel(this);            
        }
    }

    class HoundModel : AbsDetailUnitAdvancedModel
    {
        protected WalkingAnimation walkingAnimation;
        //Graphics.VoxelModelInstance animalmodel;
        //float riderY;
        public HoundModel(AbsSoldierUnit soldier)
           //: base(soldier)
        {

            switch (soldier.group.soldierConscript.conscript.animal)
            {
                default:
                    model = DssRef.models.ModelInstance_drawbatch(VoxelModelName.dog1, DssConst.Men_StandardModelScale * 0.6f);
                    walkingAnimation = new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1.1f);
                    //riderY = 0.018f;
                    break;

            }

            var soldierProfile = soldier.Profile();
            walkingAnimation.attackframe = 0;
            walkingAnimation.idleframe = 0;
            walkingAnimation.idleblinkframe = 0;

            createShadow(soldier);
        }

        public override void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);
            updateAnimation(soldier);
            //model.position.Y += riderY;
        }

        protected void updateAnimation(AbsSoldierUnit soldier)
        {
            

            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                walkingAnimation.update(move, model);
            }
            else
            {
                if (soldier.inAttackAnimation())
                {
                    model.Frame = walkingAnimation.attackframe;
                }
                else
                {
                    model.Frame = walkingAnimation.idleframe;
                }
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);
        }

        //public override void DeleteMe()
        //{
        //    base.DeleteMe();
            
        //}
    }
}

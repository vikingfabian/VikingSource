using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
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
        //public static readonly float DogScale = DssConst.Men_StandardModelScale * 0.6f;
        //public static readonly WalkingAnimation DogAnimation = new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 1.1f);

        //public static readonly float PigScale = DssConst.Men_StandardModelScale * 0.5f;
        //public static readonly WalkingAnimation PigAnimation= new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames);

        protected WalkingAnimation walkingAnimation;
        
        public HoundModel(AbsSoldierUnit soldier)
        {
            AnimalProfile modelData;
            switch (soldier.group.soldierConscript.conscript.animal)
            {
                default:
                    modelData = DssVar.dogModel;
                    break;
                case Resource.ItemResourceType.Hound:
                    modelData = DssVar.houndModel;
                    break;
                case Resource.ItemResourceType.Pig:
                    modelData = DssVar.pigModel;
                    break;
            }
            model = DssRef.models.ModelInstance_drawbatch(modelData.modelName, modelData.scale);
            walkingAnimation = modelData.animation;
            animalNoiseType = modelData.noiseType;

            var soldierProfile = soldier.Profile();
            walkingAnimation.attackframe = 0;
            walkingAnimation.idleframe = 0;
            walkingAnimation.idleblinkframe = 0;
            
            resetAnimalNoise();
            createShadow(soldier);
        }

        public override void update(AbsSoldierUnit soldier)
        {
            base.update(soldier);
            updateAnimation(soldier);
        }

        protected void updateAnimation(AbsSoldierUnit soldier)
        {  
            if (soldier.state.walking)
            {
                float move = soldier.walkingSpeedWithModifiers(Ref.DeltaGameTimeMs);

                walkingAnimation.update(move, model, out bool enterEvenFrame);
                if (enterEvenFrame && Ref.peRnd.ChanceF_Low(0.04f))
                {
                    SoundLib.footstep.Play(model.position);
                }
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
            updateAnimalNoise();
        }
    }
}

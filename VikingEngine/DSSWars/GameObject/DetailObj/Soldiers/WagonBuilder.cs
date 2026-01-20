using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Soldiers
{
    class WagonBuilder : ConscriptedSoldierBuilder
    {
        public WagonBuilder() 
            :base()
        { 
            unitBuildType = UnitBuildType.ConscriptWagon;
            
        }
    }

    class WagonSoldier : BaseSoldier
    {
        public WagonSoldier()
            : base()
        {

        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            updateGroudY(true);

            return new WagonRiderModel(this);
        }
    }

    class WagonRiderModel : AbsDetailUnitAdvancedModel
    {
        Graphics.VoxelModelInstance animalmodel_left, animalmodel_right;
        WalkingAnimation horseWalkingAnimation;
        WalkingAnimation wagonRollAnimation;
        public WagonRiderModel(AbsSoldierUnit soldier)
        {
            CavalryModel.AnimalModel(soldier.group.soldierConscript.conscript.animal, out VoxelModelName modelName, out float modelScale, out horseWalkingAnimation, out float riderY);
            animalmodel_left = DssRef.models.ModelInstance_drawbatch(modelName, modelScale);
            animalmodel_right = DssRef.models.ModelInstance_drawbatch(modelName, modelScale);

            model = DssRef.models.ModelInstance_drawbatch( LootFest.VoxelModelName.wagon_light, DssConst.Men_StandardModelScale * 1.1f);
        }
    }
}

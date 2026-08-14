using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.DetailObj.Soldiers;
using VikingEngine.DSSWars.Map;
using VikingEngine.Engine;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars.GameObject.Animal
{
    abstract class AbsLivestock : AbsUpdateable
    {        
        VectorRect area;
        protected Graphics.VoxelModelInstance model;
        protected AnimalProfile modelData;
        IntVector2 tilepos;
        Time stateTime;
        Vector3 walkDir;
        bool walkState = true;
        SoundContainerBase soundFile;
        float soundPitch;


        public AbsLivestock(IntVector2 tilepos, Vector3 topCenterWp, AnimalProfile modelData, SoundContainerBase soundFile, float soundPitch)
            :base(true)
        {
            this.modelData = modelData;
            this.soundFile = soundFile;
            this.soundPitch = soundPitch;
            this.tilepos = tilepos;
            model = createModel();

            stateTime = new Time(Ref.peRnd.Float(10, 2000));
            area = VectorRect.FromCenterSize(VectorExt.PlaneXZVec(topCenterWp), WorldData.SubTileWidthV2 * 0.8f);
            model.position = VectorExt.V3FromXZ( area.RandomPos(), topCenterWp.Y);
            WP.Rotation1DToQuaterion(model, Ref.peRnd.Rotation());
        }

        protected Graphics.VoxelModelInstance createModel()
        {
            return DssRef.models.ModelInstance_drawbatch(modelData.modelName,
                modelData.scale);
        }

        void randomWalkDir()
        {
            float dir = Ref.peRnd.Rotation();
            WP.Rotation1DToQuaterion(model, dir);
            walkDir = VectorExt.V2toV3XZ(lib.AngleToV2(dir, 1f), 0);
        }

        public override void Time_Update(float time_ms)
        {
            if (stateTime.CountDownGameTime())
            {
                walkState = !walkState;
                stateTime = new Time(Ref.peRnd.Float(500, 5000));

                sound();

                if (walkState)
                {
                    randomWalkDir();
                }
                else
                {
                    model.Frame = 0;
                }
            }

            if (walkState)
            {
                float speed =DssConst.Livestock_WalkingSpeed * Ref.DeltaGameTimeMs;
                model.position += walkDir * speed;
                modelData.animation.update(speed, model, out _);

                if (!area.IntersectX(model.position.X) ||
                    !area.IntersectY(model.position.Z))
                {
                    model.position = VectorExt.V2toV3XZ(area.KeepPointInsideBound_Position(VectorExt.V3XZtoV2(model.position)), model.PositionY);
                    randomWalkDir();
                }
            }

            var tile = DssRef.world.tileGrid.Get(tilepos);
            if (!tile.hasTileInRender && tile.OutOfRenderTimeOut())
            {
                DeleteMe();
            }
        }

        protected void sound()
        {
            if (Ref.peRnd.Chance(0.03) && soundFile != null && SoundStackManager.RareAvailable())
            {
                soundFile.Play(model.position);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.preRemoveFromDrawBatch();
            
            //model.DeleteMe();
        }
    }

    class Livestock : AbsLivestock
    {
        
        public Livestock(IntVector2 tilepos, Vector3 topCenterWp, AnimalProfile modelData, SoundContainerBase soundFile, float soundPitch)
            : base(tilepos, topCenterWp, modelData, soundFile, soundPitch)
        {
        }
        
       
    }

    class Pig : AbsLivestock
    {
        public Pig(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp, DssVar.pigModel, SoundLib.pig, 0)
        { }
        //protected override Graphics.VoxelModelInstance createModel()
        //{
        //    return DssRef.models.ModelInstance_drawbatch(VoxelModelName.Pig,
        //        DssConst.Men_StandardModelScale * 0.5f);
        //}

        //protected override void sound()
        //{
        //    if (Ref.peRnd.Chance(0.03) && SoundStackManager.RareAvailable())
        //    {
        //        SoundLib.pig.Play(model.position);
        //    }
        //}
    }
    

    class Hen : AbsLivestock
    {
        public Hen(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp, DssVar.henModel, SoundLib.hen, 0)
        { }
        //protected override Graphics.VoxelModelInstance createModel()
        //{
        //    return DssRef.models.ModelInstance_drawbatch(VoxelModelName.Hen,
        //        DssConst.Men_StandardModelScale * 0.3f);
        //}

        //protected override void sound()
        //{
        //    if (Ref.peRnd.Chance(0.03) && SoundStackManager.RareAvailable())
        //    {
        //        SoundLib.hen.Play(model.position);
        //    }
        //}
    }

    class TempAnimal : AbsLivestock
    {
        public TempAnimal(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp, DssVar.emptyAnimalModel, null, 0)
        { }
        //protected override Graphics.VoxelModelInstance createModel()
        //{
        //    return DssRef.models.ModelInstance_drawbatch(VoxelModelName.ErrorCube,
        //        DssConst.Men_StandardModelScale * 0.5f);
        //}

        //protected override void sound()
        //{
        //    if (Ref.peRnd.Chance(0.03) && SoundStackManager.RareAvailable())
        //    {
        //        SoundLib.pig.Play(model.position);
        //    }
        //}
    }

    class Pheasant : AbsLivestock
    {
        public Pheasant(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp, DssVar.pheasantModel, null, 0)
        { }
        //protected override Graphics.VoxelModelInstance createModel()
        //{
        //    return DssRef.models.ModelInstance_drawbatch(VoxelModelName.Pheasant,
        //        DssConst.Men_StandardModelScale * 0.6f);
        //}

        //protected override void sound()
        //{
        //    //if (Ref.rnd.Chance(0.02))
        //    //{
        //    //    SoundLib.hen.Play(model.position);
        //    //}
        //}
    }

    //class Horse : AbsLivestock
    //{
    //    public Horse(IntVector2 tilepos, Vector3 topCenterWp)
    //        : base(tilepos, topCenterWp)
    //    { }
    //    protected override Graphics.VoxelModelInstance createModel()
    //    {
    //        walkingAnimation = CavalryModel.HorseAnimation;

    //        return DssRef.models.ModelInstance_drawbatch(VoxelModelName.horse_brown,
    //            CavalryModel.HorseScale);
    //    }

    //    protected override void sound()
    //    {
    //    }
    //}

    //class Dog : AbsLivestock
    //{
    //    public Dog(IntVector2 tilepos, Vector3 topCenterWp)
    //        : base(tilepos, topCenterWp, DssVar.dogModel)
    //    { }
    //    //protected override Graphics.VoxelModelInstance createModel()
    //    //{
    //    //    return DssRef.models.ModelInstance_drawbatch(DssVar.dogModel.modelName,
    //    //        DssVar.dogModel.scale);
    //    //}

    //    //protected override void sound()
    //    //{
    //    //}
    //}

    //class Hog : AbsLivestock
    //{
    //    public Hog(IntVector2 tilepos, Vector3 topCenterWp)
    //        : base(tilepos, topCenterWp)
    //    { }
    //    protected override Graphics.VoxelModelInstance createModel()
    //    {
    //        walkingAnimation = CavalryModel.HogAnimation;

    //        return DssRef.models.ModelInstance_drawbatch(VoxelModelName.hog1,
    //            CavalryModel.HogScale);
    //    }

    //    protected override void sound()
    //    {
    //    }
    //}
    //class Lion : AbsLivestock
    //{
    //    public Lion(IntVector2 tilepos, Vector3 topCenterWp)
    //        : base(tilepos, topCenterWp)
    //    { }
    //    protected override Graphics.VoxelModelInstance createModel()
    //    {
    //        walkingAnimation = CavalryModel.LionAnimation;

    //        return DssRef.models.ModelInstance_drawbatch(VoxelModelName.lion1,
    //            CavalryModel.LionScale);
    //    }

    //    protected override void sound()
    //    {
    //    }
    //}
    //class Wolf : AbsLivestock
    //{
    //    public Wolf(IntVector2 tilepos, Vector3 topCenterWp)
    //        : base(tilepos, topCenterWp)
    //    { }
    //    protected override Graphics.VoxelModelInstance createModel()
    //    {
    //        walkingAnimation = CavalryModel.WolfAnimation;

    //        return DssRef.models.ModelInstance_drawbatch(VoxelModelName.wolf1,
    //            CavalryModel.WolfScale);
    //    }

    //    protected override void sound()
    //    {
    //    }
    //}

    //class Elephant : AbsLivestock
    //{
    //    public Elephant(IntVector2 tilepos, Vector3 topCenterWp)
    //        : base(tilepos, topCenterWp)
    //    { }
    //    protected override Graphics.VoxelModelInstance createModel()
    //    {
    //        walkingAnimation = CavalryModel.ElephantAnimation;

    //        return DssRef.models.ModelInstance_drawbatch(VoxelModelName.Elephant_default,
    //            CavalryModel.ElephantScale);
    //    }

    //    protected override void sound()
    //    {
    //    }
    //}



    //enum AnimalType
    //{ 
    //    Pig,
    //    Hen,
    //    Pheasant,
    //}
}

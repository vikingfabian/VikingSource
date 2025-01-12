using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;

namespace VikingEngine.DSSWars.GameObject.Animal
{
    abstract class AbsLivestock : AbsUpdateable
    {        
        VectorRect area;
        protected WalkingAnimation walkingAnimation;
        protected Graphics.VoxelModelInstance model;
        IntVector2 tilepos;
        Time stateTime;
        Vector3 walkDir;
        bool walkState = true;
        public AbsLivestock(IntVector2 tilepos, Vector3 topCenterWp)
            :base(true)
        {
            this.tilepos = tilepos;
            model = createModel();
            //model.AddToRender(DrawGame.UnitDetailLayer);
            

            stateTime = new Time(Ref.rnd.Float(10, 2000));
            area = VectorRect.FromCenterSize(VectorExt.PlaneXZVec(topCenterWp), WorldData.SubTileWidthV2 * 0.8f);
            model.position = VectorExt.V3FromXZ( area.RandomPos(), topCenterWp.Y);
            WP.Rotation1DToQuaterion(model, Ref.rnd.Rotation());
        }

        abstract protected Graphics.VoxelModelInstance createModel();

        void randomWalkDir()
        {
            float dir = Ref.rnd.Rotation();
            WP.Rotation1DToQuaterion(model, dir);
            walkDir = VectorExt.V2toV3XZ(lib.AngleToV2(dir, 1f), 0);
        }

        public override void Time_Update(float time_ms)
        {
            if (stateTime.CountDownGameTime())
            {
                walkState = !walkState;
                stateTime = new Time(Ref.rnd.Float(500, 5000));

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
                walkingAnimation.update(speed, model);

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

        abstract protected void sound();

        public override void DeleteMe()
        {
            base.DeleteMe();
            DssRef.models.recycle(model, true);
            model = null;
            //model.DeleteMe();
        }
    }

    class Pig : AbsLivestock
    {
        public Pig(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp)
        { }
        protected override Graphics.VoxelModelInstance createModel()
        {
            walkingAnimation = new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames);

            return DssRef.models.ModelInstance(VoxelModelName.Pig, true,
                DssConst.Men_StandardModelScale * 0.5f, true);
        }

        protected override void sound()
        {
            if (Ref.rnd.Chance(0.03))
            {
                SoundLib.pig.Play(model.position);
            }
        }
    }
    class Hen : AbsLivestock
    {
        public Hen(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp)
        { }
        protected override Graphics.VoxelModelInstance createModel()
        {
            walkingAnimation = new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 0.25f);

            return DssRef.models.ModelInstance(VoxelModelName.Hen, true,
                DssConst.Men_StandardModelScale * 0.3f, true);
        }

        protected override void sound()
        {
            if (Ref.rnd.Chance(0.02))
            {
                SoundLib.hen.Play(model.position);
            }
        }
    }

    class Pheasant : AbsLivestock
    {
        public Pheasant(IntVector2 tilepos, Vector3 topCenterWp)
            : base(tilepos, topCenterWp)
        { }
        protected override Graphics.VoxelModelInstance createModel()
        {
            walkingAnimation = new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 0.25f);

            return DssRef.models.ModelInstance(VoxelModelName.Pheasant, true,
                DssConst.Men_StandardModelScale * 0.6f, true);
        }

        protected override void sound()
        {
            //if (Ref.rnd.Chance(0.02))
            //{
            //    SoundLib.hen.Play(model.position);
            //}
        }
    }

    enum AnimalType
    { 
        Pig,
        Hen,
        Pheasant,
    }
}

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
        const float WalkingSpeed = AbsDetailUnitData.StandardWalkingSpeed * 0.2f;
        VectorRect area;
        protected WalkingAnimation walkingAnimation;
        protected Graphics.AbsVoxelObj model;
        Tile tile;
        Time stateTime;
        Vector3 walkDir;
        bool walkState = true;
        public AbsLivestock(Tile tile, Vector3 topCenterWp)
            :base(true)
        {
            this.tile = tile;
            model = createModel();
            model.AddToRender(DrawGame.UnitDetailLayer);
            

            stateTime = new Time(Ref.rnd.Float(10, 2000));
            area = VectorRect.FromCenterSize(VectorExt.PlaneXZVec(topCenterWp), WorldData.SubTileWidthV2 * 0.8f);
            model.position = VectorExt.V3FromXZ( area.RandomPos(), topCenterWp.Y);
            WP.Rotation1DToQuaterion(model, Ref.rnd.Rotation());
        }

        abstract protected Graphics.AbsVoxelObj createModel();

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
                float speed = WalkingSpeed * Ref.DeltaGameTimeMs;
                model.position += walkDir * speed;
                walkingAnimation.update(speed, model);

                if (!area.IntersectX(model.position.X) ||
                    !area.IntersectY(model.position.Z))
                {
                    model.position = VectorExt.V2toV3XZ(area.KeepPointInsideBound_Position(VectorExt.V3XZtoV2(model.position)), model.PositionY);
                    randomWalkDir();
                }
            }

            if (!tile.hasTileInRender && tile.OutOfRenderTimeOut())
            {
                DeleteMe();
            }
        }

        abstract protected void sound();

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }

    class Pig : AbsLivestock
    {
        public Pig(Tile tile, Vector3 topCenterWp)
            : base(tile, topCenterWp)
        { }
        protected override Graphics.AbsVoxelObj createModel()
        {
            walkingAnimation = new WalkingAnimation(1, 2, WalkingAnimation.StandardMoveFrames);

            return DssRef.models.ModelInstance(VoxelModelName.Pig, 
                AbsDetailUnitData.StandardModelScale * 0.5f, false);
        }

        protected override void sound()
        {
            if (Ref.rnd.Chance(0.3))
            {
                SoundLib.pig.Play(model.position);
            }
        }
    }
    class Hen : AbsLivestock
    {
        public Hen(Tile tile, Vector3 topCenterWp)
            : base(tile, topCenterWp)
        { }
        protected override Graphics.AbsVoxelObj createModel()
        {
            walkingAnimation = new WalkingAnimation(1, 4, WalkingAnimation.StandardMoveFrames * 0.25f);

            return DssRef.models.ModelInstance(VoxelModelName.Hen,
                AbsDetailUnitData.StandardModelScale * 0.3f, false);
        }

        protected override void sound()
        {
            if (Ref.rnd.Chance(0.1))
            {
                SoundLib.hen.Play(model.position);
            }
        }
    }

    enum AnimalType
    { 
        Pig,
        Hen,
    }
}

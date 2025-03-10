using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsSoldierStateTransform : AbsInGameUpdateable
    {
        protected SoldierGroup group;
        protected bool lookingForTerrain = true;
        
        protected Time transformTimer;
        VoxelModelInstance transformModel, loadingModel;
        bool transformEffect = false;

        public AbsSoldierStateTransform(SoldierGroup group, bool immediet)
            : base(false)
        {
            this.group = group;
            group.inShipOrGuardTransform = true;

            init(out float timeSec);
            transformTimer.Seconds = timeSec;
            //toShip = !group.isShip;
            //transformTimer.Seconds = (toShip ? DssLib.ShipBuildTimeSec : DssLib.ShipExitTimeSec) * group.typeCurrentData.ShipBuildTimeMultiplier;

            AddToUpdateList();

            if (immediet)
            { begin(); }
        }

        abstract protected void init(out float timeSec);
        

        public override void Time_Update(float time_ms)
        {
            

            updateEffect();
        }

        protected void begin()
        {
            transformEffect = true;
            lookingForTerrain = false;
        }

        void updateEffect()
        {
            if (group.isDeleted)
            {
                DeleteMe();
            }
            else if (transformEffect && group.army.inRender_detailLayer)
            {
                if (transformModel == null)
                {
                    transformModel = DssRef.models.ModelInstance(LootFest.VoxelModelName.wars_shipbuild, true, DssConst.Men_StandardModelScale * 2f, true);

                    loadingModel = DssRef.models.ModelInstance(LootFest.VoxelModelName.wars_loading_anim, true, DssConst.Men_StandardModelScale * 2f, true);
                    transformModel.Frame = modelFrame();


                    loadingModel.position = group.position;
                    loadingModel.position.Y += 0.15f;

                    transformModel.position = loadingModel.position;
                    transformModel.position.Y += 0.04f;

                }

                loadingModel.Rotation.RotateWorldX(MathExt.Tau * Ref.DeltaTimeSec * -0.25f);
            }
        }


        public override void DeleteMe()
        {
            base.DeleteMe();

            DssRef.models.recycle(ref transformModel, true);
            DssRef.models.recycle(ref loadingModel, true);

            completeTransform();
        }
        abstract protected int modelFrame();
        abstract protected void completeTransform();
    }

    class ShipTransform : AbsSoldierStateTransform
    {
        bool toShip;

        public ShipTransform(SoldierGroup group, bool immediet)
            :base(group, immediet)
        { }

        protected override void init(out float timeSec)
        {
            toShip = !group.isShip;
            timeSec = (toShip ? DssConst.ShipBuildTimeSec : DssConst.ShipExitTimeSec) * group.typeCurrentData.ShipBuildTimeMultiplier;

        }

        public override void Time_Update(float time_ms)
        {
            if (lookingForTerrain)
            {
                if (DssRef.world.tileGrid.TryGet(group.tilePos, out Tile tile) &&
                    tile.IsWater() == toShip)
                {
                    begin();
                }
            }
            else
            {
                if (transformTimer.CountDownGameTime())
                {
                    DeleteMe();
                    return;
                }
            }

            base.Time_Update(time_ms);
        }

        override protected int modelFrame()
        { 
            return toShip ? 0 : 1;
        }

        protected override void completeTransform()
        {
            group.completeTransform(toShip ? SoldierTransformType.ToShip : SoldierTransformType.FromShip, -1);
        }
    }

    class GuardPostTransform : AbsSoldierStateTransform
    {
        bool toGuard;
        int postIdAndPosition;
        public GuardPostTransform(SoldierGroup group, int postIdAndPosition, bool immediet)
            : base(group, immediet)
        {
            this.postIdAndPosition = postIdAndPosition;
        }

        protected override void init(out float timeSec)
        {
            toGuard = !group.InGuardPost();
            timeSec = toGuard ? DssConst.GuardPostEnter_TimeSec : DssConst.GuardPostExit_TimeSec;


        }

        public override void Time_Update(float time_ms)
        {
            if (lookingForTerrain)
            {
                //if (DssRef.world.tileGrid.TryGet(group.tilePos, out Tile tile) &&
                //    tile.IsWater() == toGuard)
                //{
                    begin();
                //}
            }
            else
            {
                if (transformTimer.CountDownGameTime())
                {
                    DeleteMe();
                    return;
                }
            }

            base.Time_Update(time_ms);
        }

        override protected int modelFrame()
        {
            return toGuard ? 2 : 3;
        }

        protected override void completeTransform()
        {
            group.completeTransform(toGuard ? SoldierTransformType.EnterGuard : SoldierTransformType.ExitGuard, postIdAndPosition);
        }
    }
}

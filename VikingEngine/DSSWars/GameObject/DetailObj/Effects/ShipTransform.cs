﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.GameObject
{
    class ShipTransform : AbsInGameUpdateable
    {
        SoldierGroup group;
        bool lookingForTerrain = true;
        bool toShip;
        Time transformTimer;
        //Graphics.Mesh transformIcon;
        VoxelModelInstance transformModel, loadingModel;
        bool transformEffect = false;

        public ShipTransform(SoldierGroup group, bool immediet)
            :base(false)
        {

            this.group = group;
            
            toShip = !group.isShip;

            transformTimer.Seconds = (toShip ? DssLib.ShipBuildTimeSec : DssLib.ShipExitTimeSec) * group.typeCurrentData.ShipBuildTimeMultiplier;

            AddToUpdateList();

            if (immediet)
            { begin(); }
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

            updateEffect();
        }

        void begin()
        { 
            transformEffect = true;                    
            lookingForTerrain = false;
            //group.lockMovement = true;
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

                    //transformIcon = new Graphics.Mesh(LoadedMesh.cube_repeating, group.position,
                    //    new Vector3(AbsDetailUnitData.StandardModelScale * 2f), Graphics.TextureEffectType.Flat,
                    //        SpriteName.WhiteArea, Color.Brown, false);
                    

                    loadingModel = DssRef.models.ModelInstance( LootFest.VoxelModelName.wars_loading_anim,true, DssConst.Men_StandardModelScale * 2f, true);
                    transformModel.Frame = toShip? 0 : 1;
                    

                    loadingModel.position = group.position;
                    loadingModel.position.Y += 0.15f;

                    transformModel.position = loadingModel.position;
                    transformModel.position.Y += 0.04f;

                    //transformModel.AddToRender(DrawGame.UnitDetailLayer);
                    //loadingModel.AddToRender(DrawGame.UnitDetailLayer);
                    //new Graphics.Motion3d(Graphics.MotionType.ROTATE, loadingModel,
                    //    new Vector3(MathExt.Tau, 0, 0), Graphics.MotionRepeate.Loop, 1000, true);
                }

                loadingModel.Rotation.RotateWorldX(MathExt.Tau * Ref.DeltaTimeSec * -0.25f);
            }
        }


        public override void DeleteMe()
        {
            base.DeleteMe();

            DssRef.models.recycle(transformModel, true);
            DssRef.models.recycle(loadingModel, true);
            //if (transformModel != null)
            //{
            //    transformModel?.DeleteMe();
            //}
            //loadingModel?.DeleteMe();
            //group.inShipTransform = null;
            //group.lockMovement = false;
            group.completeTransform(toShip? SoldierTransformType.ToShip : SoldierTransformType.FromShip);
        }
    }
}

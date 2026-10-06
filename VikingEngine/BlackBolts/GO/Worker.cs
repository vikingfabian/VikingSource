using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class Worker: AbsGameObject
    {
        public MapPlacement beltPos;
        public Fluid stompSmear = Fluid.Empty;
        //MapPlacement spawnPos;

        Animation animation;

        public Worker(PlaceObjectData placementData) 
            :base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            nextPos = currentPos;
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.goblin_worker], true);
            //model.Color = Color.Green;
            model.scale = new Vector3(1.15f * model.SizeToScale);

            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);

            RefreshResourcePos();
        }
        public override void OnResourceChanged()
        {
            base.OnResourceChanged();
            refreshAnimation();
        }

        public void refreshAnimation()
        {
            animation = new Animation(1, 5, 120);
            if (pResource.hasValue)
            {
                animation.Add(5);
            }
        }

        public override void TweenUpdate(bool beltMove, float tween)
        {
            base.TweenUpdate(beltMove, tween);

            if (!beltMove)
            {
                animation.update(Ref.DeltaGameTimeMs, model, out _);
            }

            RefreshResourcePos();
        }

        public void FinalizeMove()
        {
            bool didMove = currentPos.tilePos != nextPos.tilePos;
            currentPos = nextPos;
            var tile = BlackRef.mapData.tileGrid.Get(currentPos.tilePos);
            tile.pCreature = pointer;
            refreshPos();

            if (didMove)
            {
                //Check stomp
                if (stompSmear.HasValue)
                {
                    tile.fluid = stompSmear;
                    BlackRef.playScene.mapmodel.decalsNeedsUpdate = true;
                    stompSmear.amount--;
                }
                else if (tile.fluid.HasValue)
                {
                    stompSmear = tile.fluid.GetOne();
                }

                if (tile.pResource.hasValue)
                {
                    var resource = tile.pResource.GetSolidResource();
                    if (ResourceLib.Get(resource.placementData.resourceType).stompEffect)
                    {
                        resource.DeleteMe();
                        tile.pResource.hasValue = false;
                        tile.fluid = new Fluid(resource.placementData.resourceType);
                        stompSmear = tile.fluid;
                        BlackRef.playScene.mapmodel.decalsNeedsUpdate = true;
                    }
                }
            }
        }

        public bool NoMovement()
        {
            return currentPos.tilePos == nextPos.tilePos;
        }

        public override void RefreshResourcePos()
        {
            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                resource.model.Rotation = model.Rotation;
                resource.model.position = model.Rotation.TranslateAlongAxis(
                    new Vector3(0, 0.25f, 0.4f), model.position);
            }
        }

        static Vector3 diff = new Vector3(0, 0.4f, 0.4f);
        public override Vector3 ResourceOffset()
        {
            return diff;
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            BlackRef.mapData.creatureList.RemoveAt(pointer.objIndex);
        }
        public override FactoryObjectType GameObjectType =>  FactoryObjectType.GoblinWorker;
    }
}

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
        protected override void OnResourceChanged()
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
            currentPos = nextPos;
            BlackRef.mapData.tileGrid.Get(currentPos.tilePos).pCreature = pointer;
            refreshPos();
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
        public override GameObjectType GameObjectType =>  GameObjectType.Worker;
    }
}

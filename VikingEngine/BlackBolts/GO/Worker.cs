using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class Worker: AbsGameObject
    {
        //public IntVector2 tilePos;
        
        public MapPlacement nextPos;

        public bool hasBeltMove = false;
        public MapPlacement beltPos;

        MapPlacement spawnPos;
        

        public Worker(MapPlacement placement) 
        {
            this.currentPos = placement;
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

        public void TweenUpdate(bool beltMove, float tween)
        {
            //if (NoMovement())
            if (!beltMove || hasBeltMove)
            {
                {
                    float from = WP.DirToAngle(currentPos.direction);
                    float to = WP.DirToAngle(nextPos.direction);
                    WP.Rotation1DToQuaterion(model, from * (1 - tween) + to * tween);
                }
                //else
                {
                    Vector3 from = WP.TileToWp(currentPos.tilePos);
                    Vector3 to = WP.TileToWp(nextPos.tilePos);
                    model.position = from * (1 - tween) + to * tween;
                }

                RefreshResourcePos();
            }
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

        public override GameObjectType GameObjectType =>  GameObjectType.Worker;
    }
}

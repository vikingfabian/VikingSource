using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class Worker
    {
        //public IntVector2 tilePos;
        public MapPlacement currentPos;
        public MapPlacement nextPos;

        MapPlacement spawnPos;
        VoxelModelInstance model;
        public ObjectPointer pointer;

        public Worker(MapPlacement placement) 
        {
            this.currentPos = placement;
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.war_worker], true);
            model.Color = Color.Green;
            model.scale = new Vector3(1.7f * model.SizeToScale);

            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);
        }

        public void TweenUpdate(float tween)
        {
            if (NoMovement())
            {
                float from = WP.DirToAngle(currentPos.direction);
                float to = WP.DirToAngle(nextPos.direction);
                WP.Rotation1DToQuaterion(model, from * (1 - tween) + to * tween);
            }
            else
            {
                Vector3 from = WP.TileToWp(currentPos.tilePos);
                Vector3 to = WP.TileToWp(nextPos.tilePos);
                model.position = from * (1 - tween) + to * tween;
            }
        }

        public void FinalizeMove()
        {
            currentPos = nextPos;
            BlackRef.mapData.tileGrid.Get(currentPos.tilePos).gameobject = pointer;
            refreshPos();
        }

        public bool NoMovement()
        {
            return currentPos.tilePos == nextPos.tilePos;
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    //abstract class AbsResource : AbsGameObject
    //{

    //}

    class SolidResource : AbsGameObject
    {
        //ResourceType resourceType;
        public bool onFloor = false;

        public SolidResource(PlaceObjectData placementData)
            :base(placementData)
        {
            //this.resourceType = resourceType;
            //this.currentPos = placement;

            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_item], true);            

            float scale = 0.95f;
            int frame = 0;
            switch (placementData.resourceType)
            {
                case ResourceType.Box:
                    scale = 0.95f;
                    frame = 3;
                    break;
                case ResourceType.Flesh:
                    frame = 4;
                    break;
                case ResourceType.Bone:
                    frame = 2;
                    break;
                case ResourceType.Grilled_meat:
                    frame = 1;
                    break;
            }
            model.scale = new Vector3(scale * model.SizeToScale);
            model.Frame = frame;
            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
        }
        public void placeResourceOnFloor(IntVector2 toPos)
        {
            BlackRef.mapData.tileGrid.Get(toPos).pResource = pointer;
            currentPos.tilePos = toPos;
            currentPos.refreshGroundY();

            model.position = WP.TileToWp(toPos);
            model.position.Y = currentPos.groundY;

            nextPos = currentPos;
            onFloor = true;

            checkFloorTransformation();
        }
        public void checkFloorTransformation()
        {
            if (onFloor)
            {
                var tile = BlackRef.mapData.tileGrid.Get(currentPos.tilePos);
                var pM = tile.pMachine;
                if (pM.hasValue)
                {
                    var m = pM.GetMachine();
                    if (m.GameObjectType == GameObjectType.Table)
                    {
                        onFloor = false;
                        m.pResource = pointer;
                        tile.pResource.hasValue = false;
                    }
                }
            }
        }
        
        public override GameObjectType GameObjectType => GameObjectType.NUM_NONE;
    }
}

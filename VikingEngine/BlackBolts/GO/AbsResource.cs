using Microsoft.Xna.Framework;
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
        
        public override GameObjectType GameObjectType => GameObjectType.NUM_NONE;
    }
}

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

    class SolidResource : AbsGameObject
    {
        public bool onFloor = false;

        public SolidResource(PlaceObjectData placementData)
            :base(placementData)
        {
            ResourceModel(placementData.resourceType, out LootFest.VoxelModelName modelName, out int frame, out float scale);
            model = new VoxelModelInstance(BlackRef.models.voxelModels[modelName], true);            

            model.scale = new Vector3(scale * model.SizeToScale);
            model.Frame = frame;
            refreshPos();
        }

        public static void ResourceModel(ResourceType resourceType, out LootFest.VoxelModelName modelName,out int frame,out float scale)
        {
            modelName = LootFest.VoxelModelName.bb_item;
            scale = 0.95f;
            frame = 0;
            switch (resourceType)
            {
                case ResourceType.Box:
                    scale = 0.95f;
                    frame = 3;
                    break;
                case ResourceType.Flesh:
                    frame = 0;
                    break;
                case ResourceType.Bone:
                    frame = 2;
                    break;
                case ResourceType.Grilled_meat:
                    frame = 1;
                    break;
                case ResourceType.Dragon_egg:
                    frame = 4;
                    break;
                case ResourceType.Void_egg:
                    frame = 5;
                    break;
                case ResourceType.Magic_crystal:
                    frame = 6;
                    break;
                case ResourceType.Fire_crystal:
                    frame = 7;
                    break;
                case ResourceType.Void_cube:
                    frame = 8;
                    break;
                case ResourceType.Poop:
                    frame = 9;
                    break;
                case ResourceType.Burned_shit:
                    frame = 10;
                    break;
                case ResourceType.Old_shoe:
                    frame = 11;
                    break;
                case ResourceType.Chicken:
                    modelName = LootFest.VoxelModelName.Hen;
                    frame = 1;
                    break;
                case ResourceType.Chicken_egg:
                    frame = 12;
                    break;
                case ResourceType.Feather:
                    frame = 13;
                    break;
                case ResourceType.Rubble:
                    frame = 14;
                    break;
                case ResourceType.Job_knight:
                    frame = 15;
                    break;
            }
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
                    if (m.GameObjectType == FactoryObjectType.Table)
                    {
                        onFloor = false;
                        m.pResource = pointer;
                        tile.pResource.hasValue = false;
                    }
                }
            }
        }
        
        public override FactoryObjectType GameObjectType => FactoryObjectType.NUM_NONE;
    }
}

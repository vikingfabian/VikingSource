using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsItemTable : AbsMachine
    {
        public AbsItemTable(Map.MapPlacement placement,
            LootFest.VoxelModelName modelName, int frame)
        {
            this.currentPos = placement;

            model = new VoxelModelInstance(BlackRef.models.voxelModels[modelName], true);
            model.scale = new Vector3(1.3f * model.SizeToScale);
            model.Frame = frame;
            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
        }
        override public void AnimateUpdate()
        {
        }
        public override bool WalkableTile()
        {
            return false;
        }
    }
    class ItemTable: AbsItemTable
    {
        public ItemTable(Map.MapPlacement placement)
            : base(placement, LootFest.VoxelModelName.bb_onetile, 0)
        { }
        public override GameObjectType GameObjectType => GameObjectType.Table;
    }

    class Dispencer: AbsItemTable
    {
        
        ResourceType resourceType;
        public Dispencer(Map.MapPlacement placement, ResourceType resourceType)
           : base(placement, LootFest.VoxelModelName.bb_onetile, 1)
        { 
            this.resourceType = resourceType;
            generateResource();
        }
        public override void OnCykleEnd()
        {
            if (!pResource.hasValue)
            {
                 generateResource();
            }
        }

        void generateResource()
        {
            var resource = BlackRef.mapData.SpawnResource(resourceType);
            pResource = resource.pointer;
            resource.model.position = model.position + ResourcePos;
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.5f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }
        public override GameObjectType GameObjectType => GameObjectType.Dispencer;
    }

    class DeliveryPoint: AbsItemTable
    {
        public DeliveryPoint(Map.MapPlacement placement)
           : base(placement, LootFest.VoxelModelName.bb_onetile, 2)
        {
            WP.DirToQuaterion(model, Dir4.W);
        }
        public override GameObjectType GameObjectType => GameObjectType.Delivery_point;
    }
}

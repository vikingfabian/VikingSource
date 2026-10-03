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
        //public override void RefreshResourcePos()
        //{
        //    if (pResource.hasValue)
        //    {
        //        var resource = pResource.GetSolidResource();
        //        resource.model.position = model.position + ResourceOffset();
        //    }
        //}
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

        public override void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayDrop = true;
            mayPick = true;
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.25f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }
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
            RefreshResourcePos();
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.5f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }

        public override void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayDrop = false;
            mayPick = true;
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

        public override void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayDrop = true;
            mayPick = false;
        }
        public override void OnCykleEnd()
        {
            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                Engine.ParticleHandler.AddExpandingParticleArea(ParticleSystemType.Dust, 
                    VectorExt.AddY( resource.model.position, 0.5f), 0.2f, 40, 0.5f);
                resource.DeleteMe();
                BlackRef.mapData.resourceList.RemoveAt(pResource.objIndex);
                pResource = ObjectPointer.Empty;
            }
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.1f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }
        public override GameObjectType GameObjectType => GameObjectType.Delivery_point;
    }
}

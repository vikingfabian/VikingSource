using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Interface;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsItemTable : AbsMachine
    {
        public AbsItemTable(PlaceObjectData placementData,
            LootFest.VoxelModelName modelName, int frame)
            :base(placementData)
        {
            this.currentPos = placementData.mapPlacement;

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
        public ItemTable(PlaceObjectData placementData)
            : base(placementData, LootFest.VoxelModelName.bb_onetile, 0)
        { }
        public override FactoryObjectType GameObjectType => FactoryObjectType.Table;

        public override void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayDrop = true;
            mayPick = true;
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.3f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }
    }

    class Dispencer: AbsItemTable
    {
        
        //ResourceType resourceType;
        public Dispencer(PlaceObjectData placementData)
           : base(placementData, LootFest.VoxelModelName.bb_onetile, 1)
        { 
            //this.resourceType = placementData.resourceType;
            //this.placementData = placementData;
            generateResource();
        }
        public override bool RefreshUiDisplay(IOdisplay display)
        {
            display.AddInput(new IO_port(false, placementData.mapPlacement, placementData.resourceType, 1)
                { isTabledispence = true } );
            return true;
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
            var resource = BlackRef.mapData.SpawnResource(placementData.resourceType);
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
        public override FactoryObjectType GameObjectType => FactoryObjectType.Dispencer;
    }

    class DeliveryPoint: AbsItemTable
    {
        public DeliveryPoint(PlaceObjectData placementData)
           : base(placementData, LootFest.VoxelModelName.bb_onetile, 2)
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
        public override FactoryObjectType GameObjectType => FactoryObjectType.Delivery_point;
    }

    class GarbageDisposal: AbsItemTable
    { 
        public GarbageDisposal(PlaceObjectData placementData)
           : base(placementData, LootFest.VoxelModelName.bb_onetile, 8)
        {
            WP.DirToQuaterion(model, Dir4.S);
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
                    VectorExt.AddY(resource.model.position, 0.5f), 0.2f, 40, 0.5f);
                resource.DeleteMe();
                BlackRef.mapData.resourceList.RemoveAt(pResource.objIndex);
                pResource = ObjectPointer.Empty;
            }
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.13f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }
        public override FactoryObjectType GameObjectType => FactoryObjectType.Garbage_disposal;
    }
}


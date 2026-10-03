using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsGameObject
    {
        public PlaceObjectData placementData;

        public MapPlacement currentPos;
        public VoxelModelInstance model;
        public ObjectPointer pointer;

        public bool lockItem = false;
        public ObjectPointer pResource = ObjectPointer.Empty;

        abstract public GameObjectType GameObjectType { get; }

        virtual public Vector3 ResourceOffset() { return Vector3.Zero; }

        public AbsGameObject(PlaceObjectData placementData)
        { 
            this.placementData = placementData;
        }

        virtual public void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayPick = false;
            mayDrop = false;
        }

        public ObjectPointer HandoverItem(ObjectPointer newResource)
        {
            if (newResource.hasValue || pResource.hasValue)
            {
                ObjectPointer returnItem = pResource;
                pResource = newResource;
                RefreshResourcePos();
                OnResourceChanged();
                return returnItem;
            }
            return ObjectPointer.Empty;
        }

        virtual protected void OnResourceChanged()
        { }

        virtual public void RefreshResourcePos()
        {
            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                resource.model.position = model.position + ResourceOffset();
            }
        }

        virtual public void DeleteMe()
        {
            model.DeleteMe();

            if (pResource.hasValue)
            {
                var resource = pResource.GetSolidResource();
                resource.DeleteMe();
            }
        }
    }


}

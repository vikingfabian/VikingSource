using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    abstract class AbsGameObject
    {
        public MapPlacement currentPos;
        public VoxelModelInstance model;
        public ObjectPointer pointer;

        public bool lockItem = false;
        public ObjectPointer pResource = ObjectPointer.Empty;

        abstract public GameObjectType GameObjectType { get; }

        virtual public Vector3 ResourceOffset() { return Vector3.Zero; }

        virtual public void ItemHandle(out bool mayPick, out bool mayDrop)
        {
            mayPick = false;
            mayDrop = false;
        }

        public ObjectPointer HandoverItem(ObjectPointer newResource)
        {
            ObjectPointer returnItem = pResource;
            pResource = newResource;
            RefreshResourcePos();
            return returnItem;
        }

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
        }
    }


}

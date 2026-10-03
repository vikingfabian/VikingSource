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
        public ObjectPointer pResource = ObjectPointer.Empty;

        abstract public GameObjectType GameObjectType { get; }

        virtual public Vector3 ResourceOffset() { return Vector3.Zero; }
    }


}

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
        protected VoxelModelInstance model;
        public ObjectPointer pointer;

        abstract public GameObjectType GameObjectType { get; }
    }


}

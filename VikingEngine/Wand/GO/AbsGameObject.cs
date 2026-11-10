using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO
{
    abstract class AbsGameObject
    {
        public MapPlacement currentPos;
        public VoxelModelInstance model;
    }
}

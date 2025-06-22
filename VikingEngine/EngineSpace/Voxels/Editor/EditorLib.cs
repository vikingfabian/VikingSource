using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Map.HDvoxel;
using VikingEngine.LootFest.Map;

namespace VikingEngine.Voxels
{
    class NetworkDraw : IVoxelDesigner
    {
        WorldPosition worldPos;

        public NetworkDraw(WorldPosition worldPos)
        {
            this.worldPos = worldPos;
        }

        public void SetVoxel(IntVector3 drawPoint, ushort material)
        {
            WorldPosition pos = worldPos;
            pos.WorldGrindex.Add(drawPoint);
            pos.SetBlock(material);
        }
        public ushort GetVoxel(IntVector3 drawPoint)
        {
            
            WorldPosition pos = worldPos;
            pos.WorldGrindex.Add(drawPoint);
            return pos.GetBlock();
        }
    }

    struct LetterRows
    {
        public IntervalIntV3 selectionArea;
        public List<int> lengths;
    }
    
    enum ThreadedLoad
    {
        StartUp,
        ListTemplates,
        ListTemplatesCategory,
    }
    enum MainMenuScene
    {
        bosslock,
        coverlike_stand,
        NUM
    }
}

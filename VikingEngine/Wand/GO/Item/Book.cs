using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Item
{
    class Book : AbsItem
    {
        public Book(MapPlacement placementData)
            :base( ObjectPointer.TileItemType.Book, placementData)
        {
            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.ErrorCube], true);
            model.scale = new Vector3(0.8f * model.SizeToScale);
            model.scale.Y *= 0.2f;
            model.Color = Color.Blue;

            refreshPos();
        }
    }
}

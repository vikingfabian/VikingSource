using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Item
{
    class Altar : AbsItem
    {
        public Altar(MapPlacement placementData)
            : base(ObjectPointer.TileItemType.Altar, placementData)
        {
            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.bb_onetile], true);
            model.scale = new Vector3(1.2f * model.SizeToScale);
            model.scale.Y *= 0.5f;
            model.Color = Color.Blue;

            refreshPos();
        }
    }
}

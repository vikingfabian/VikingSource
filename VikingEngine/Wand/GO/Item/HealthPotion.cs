using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Item
{
    class HealthPotion : AbsItem
    {
        public HealthPotion(MapPlacement placementData)
            : base(ObjectPointer.TileItemType.Health_potion, placementData)
        {
            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.ErrorCube], true);
            model.scale = new Vector3(0.5f * model.SizeToScale);
            model.Color = Color.Red;

            refreshPos();
        }
    }
}

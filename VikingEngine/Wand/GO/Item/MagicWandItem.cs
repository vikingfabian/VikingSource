using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Item
{
    internal class MagicWandItem : AbsItem
    {
        public Equipment.MagicWand wand;

        public MagicWandItem(MapPlacement placementData)
            : base(ObjectPointer.TileItemType.MagicWand, placementData)
        {
            wand = new Equipment.MagicWand(3);

            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.ErrorCube], true);
            model.scale = new Vector3(0.1f * model.SizeToScale);
            model.scale.X *= 4f;
            model.Color = Color.Brown;

            refreshPos();
        }

        public override bool LevelUp(int tier)
        {
            wand.LevelUp(tier);
            return base.LevelUp(tier);
        }
    }
}

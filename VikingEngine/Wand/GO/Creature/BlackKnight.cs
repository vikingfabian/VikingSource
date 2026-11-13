using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{
    class BlackKnight : AbsCreature
    {
        public BlackKnight(MapPlacement placementData)
            : base(ObjectPointer.CreatureType.BlackKnight, placementData, Ref.rnd.Int(4, 6))
        {
            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.bb_blackknight], true);
            model.scale = new Vector3(1.5f * model.SizeToScale);

            refreshPos();
        }
        public override Range AttackStrength()
        {
            return new Range(3, 4);
        }
    }
}

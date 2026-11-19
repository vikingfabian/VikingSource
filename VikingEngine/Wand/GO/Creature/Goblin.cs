using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{
    class Goblin : AbsCreature
    {
        public Goblin(MapPlacement placementData)
            :base(ObjectPointer.CreatureType.Goblin, placementData, Ref.rnd.Int(1, 3))
        {
            
            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.goblin_worker], true);
            model.scale = new Vector3(0.8f * model.SizeToScale);

            refreshPos();

        }
        public override Range AttackStrength()
        {
            return new Range(1, 2);
        }
    }
}

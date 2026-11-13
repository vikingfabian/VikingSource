using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts;
using VikingEngine.Core.Wand.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.Wand.GO.Creature
{
    class Orc : AbsCreature
    {
        public Orc(MapPlacement placementData)
            : base(ObjectPointer.CreatureType.Orc, placementData, Ref.rnd.Int(2, 4))
        {

            model = new VoxelModelInstance(WandRef.models.voxelModels[LootFest.VoxelModelName.goblin_worker], true);
            model.scale = new Vector3(1f * model.SizeToScale);
            model.Color = Color.DarkGray;

            refreshPos();

        }
        public override Range AttackStrength()
        {
            return new Range(2, 3);
        }
    }
}

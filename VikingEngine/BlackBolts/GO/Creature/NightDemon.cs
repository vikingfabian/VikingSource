using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class NightDemon : AbsCreature
    {
        public NightDemon(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.goblin_worker], true);
            //model.Color = Color.Green;
            model.scale = new Vector3(1.2f * model.SizeToScale);
            model.Color = Color.DarkMagenta;

            refreshPos();
        }
        public override bool WillMoveItems()
        {
            return false;
        }
        public override FactoryObjectType GameObjectType => FactoryObjectType.NightDemon;
    }
}

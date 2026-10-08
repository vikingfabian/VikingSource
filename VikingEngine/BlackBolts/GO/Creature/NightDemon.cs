using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class NightDemon : AbsKnight
    {
        public NightDemon(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_nightdemon], true);
           
            model.scale = new Vector3(1.4f * model.SizeToScale);

            refreshPos();
        }
        public override bool WillMoveItems()
        {
            return false;
        }
        protected override DestroyType DestroyType => DestroyType.Void;
        protected override int AttackFrame => 0;
        protected override bool IsGoodSide => false;
        public override FactoryObjectType FactoryObjectType => FactoryObjectType.NightDemon;
    }
}

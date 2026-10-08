using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class BlackKnight : AbsKnight
    {
        public BlackKnight(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_blackknight], true);
            //model.Color = Color.Green;
            model.scale = new Vector3(2.5f * model.SizeToScale);
            //model.Color = Color.Black;

            refreshPos();
        }
        protected override bool IsGoodSide => false;
        public override FactoryObjectType FactoryObjectType => FactoryObjectType.BlackKnight;
    }
}

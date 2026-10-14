using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO.Creature
{
    class WhiteKnight : AbsKnight
    {
        public WhiteKnight(PlaceObjectData placementData)
            : base(placementData)
        {
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_whiteknight], true);
            //model.Color = Color.Green;
            model.scale = new Vector3(2.5f * model.SizeToScale);
           // model.Color = Color.Yellow;

            refreshPos();
        }
        protected override bool IsGoodSide => true;
        public override FactoryObjectType FactoryObjectType => FactoryObjectType.WhiteKnight;
    }
}

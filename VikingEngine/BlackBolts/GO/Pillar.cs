using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class Pillar : AbsMachine
    {
        public Pillar(PlaceObjectData placementData)
            : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;

            model = new VoxelModelInstance(BlackRef.models.voxelModels[ LootFest.VoxelModelName.bb_onetile], true);
            model.scale = new Vector3(1.5f * model.SizeToScale);
            model.Frame = 6;
            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
        }

        public override void AnimateUpdate()
        {
        }
        public override bool WalkableTile()
        {
            return false;
        }

        public override GameObjectType GameObjectType => GameObjectType.Stone_pillar;

    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class Belt : AbsMachine
    {
        Animation animation;
        public Belt(PlaceObjectData placementData)
            : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_belt], true);
            model.scale = new Vector3(1.3f * model.SizeToScale);

            animation = new Animation(0, 3, 45);

            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.DirToQuaterion(model, currentPos.direction);
        }

        override public void AnimateUpdate()
        {
            animation.update(Ref.DeltaGameTimeMs, model, out _);
        }

        static readonly Vector3 ResourcePos = new Vector3(0, 0.22f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }

        public override FactoryObjectType FactoryObjectType => FactoryObjectType.Belt;
    }
}

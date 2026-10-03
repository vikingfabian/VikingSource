using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class Belt : AbsGameObject
    {
        Animation animation;
        public Belt(Map.MapPlacement placement)
        {
            this.currentPos = placement;
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

        public void AnimateUpdate()
        {
            animation.update(Ref.DeltaGameTimeMs, model, out _);
        }
    }
}

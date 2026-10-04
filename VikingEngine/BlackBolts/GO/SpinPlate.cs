using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Core.BlackBolts.Render;
using VikingEngine.Graphics;

namespace VikingEngine.Core.BlackBolts.GO
{
    class SpinPlate : AbsMachine
    {
        const float AngleSpeed = MathExt.TauOver4 / RunExecuter.MoveTime;
        Rotation1D angle = Rotation1D.D0;
        public int rotateDir;
        public SpinPlate(PlaceObjectData placementData)
            : base(placementData)
        {
            this.currentPos = placementData.mapPlacement;
            model = new VoxelModelInstance(BlackRef.models.voxelModels[LootFest.VoxelModelName.bb_rotate], true);
            model.scale = new Vector3(1.3f * model.SizeToScale);

            bool clockwise = placementData.mapPlacement.direction == Dir4.E;
            rotateDir = lib.BoolToLeftRight(clockwise);
            if (!clockwise)
            {
                model.Frame = 1;
            }

            refreshPos();
        }

        void refreshPos()
        {
            model.position = WP.TileToWp(currentPos.tilePos);
            WP.Rotation1DToQuaterion(model, angle.radians);
        }

        override public void AnimateUpdate()
        {
            angle.Add(rotateDir * AngleSpeed * Ref.DeltaGameTimeMs);
            WP.Rotation1DToQuaterion(model, angle.radians);
        }
        static readonly Vector3 ResourcePos = new Vector3(0, 0.2f, 0);
        public override Vector3 ResourceOffset()
        {
            return ResourcePos;
        }
        public override GameObjectType GameObjectType => GameObjectType.Spin_plate;
    }
}

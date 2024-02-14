using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO
{
    class DebugButterfly : Characters.AbsCharacter
    {
        public DebugButterfly(GoArgs args)
            : base(args)
        {
            args.startWp.SetAtClosestFreeY(6);
            WorldPos = args.startWp;

            animSettings = new Graphics.AnimationsSettings(2, 100f, 0);
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.butterfly,
                0.8f, 1, false);
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(0.8f);
            image.position = WorldPos.PositionV3;
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (checkOutsideUpdateArea_ActiveChunk())
            {
                DeleteMe();
            }
            base.Time_Update(args);
        }

        protected override bool animationUseMoveVelocity
        {
            get
            {
                return false;
            }
        }

        protected override void moveImage(Velocity speed, float time)
        {
            //base.moveImage(speed, time);
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }
        public override GameObjectType Type
        {
            get { throw new NotImplementedException(); }
        }
    }
}

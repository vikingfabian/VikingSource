using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class FlyingPig : Characters.AbsCharacter
    {
        const float WalkingSpeed = 0.006f;

        GO.PlayerCharacter.AbsHero follow = null;
        AiPhysicsLf3 aiPhys;
        new const float Scale = 1.6f;

        public FlyingPig(GoArgs args)
            : base(args)
        {
            WorldPos = args.startWp;
            chickInit();
            aiPhys = (AiPhysicsLf3)physics;
        }

        void chickInit()
        {
            animSettings =  new Graphics.AnimationsSettings(4, 0.8f, false);

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.pet_pig,
                Scale, 1, false);
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(Scale);

            aiPhys = new AiPhysicsLf3(this, true);
            physics = aiPhys;
            physType = ObjPhysicsType.FlyingAi;

            image.position = WorldPos.PositionV3;
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                    return;
                }
            }

            if (follow == null)
            {
                follow = GetClosestHero(false);
            }
            aiPhys.MovUpdate_MoveTowards(follow.Position + Vector3.Up * 4f, 2, WalkingSpeed);

            base.Time_Update(args);
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            if (localMember)
            {
                aiPhys.AsynchUpdate(args.time);
            }
        }

        ObjPhysicsType physType = ObjPhysicsType.NO_PHYSICS;
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return physType;
            }
        }
       

        public override GameObjectType Type
        {
            get { return GameObjectType.FlyingPig; }
        }
    }
}

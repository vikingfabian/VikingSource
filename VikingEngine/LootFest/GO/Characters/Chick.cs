using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters
{
    class Chick : Characters.AbsCharacter
    {
        const float WalkingSpeed = 0.006f;

        GO.PlayerCharacter.AbsHero follow = null;
        AiPhysicsLf3 aiPhys; 
 
        public Chick(GoArgs args)
            :base(args)
        {
            WorldPos = args.startWp;
            chickInit();
            aiPhys = (AiPhysicsLf3)physics;
        }

        void chickInit()
        {
            animSettings =  new Graphics.AnimationsSettings(5, 0.2f, 1);
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.chick,
                1.2f, 1, false);
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(0.8f);
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
            aiPhys.MovUpdate_MoveTowards(follow, 2, WalkingSpeed);
            
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

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.GroundAi;
            }
        }

        public override GameObjectType Type
        {
            get { throw new NotImplementedException(); }
        }
    }

    
}

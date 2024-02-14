using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters
{
    class Moth3 : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(1f, 1.2f);

        Vector3 targetOffset;
        Time targetOffsetTime;

        public Moth3(Vector3 startPos, GO.Characters.AbsCharacter target)//, System.IO.BinaryReader r)
            :base(new GoArgs(startPos, 0))
        {
            modelScale = ScaleRange.GetRandom();
            createImage(VoxelModelName.moth, modelScale, new Graphics.AnimationsSettings(2, 0.8f, 0));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(
                new Vector3(0.3f * modelScale, 0.3f * modelScale, modelScale * 0.4f));

            //if (r == null)
            //{
                this.target = target;
                targetOffset = Ref.rnd.Vector3_Sq(Vector3.Zero, 4);
                targetOffsetTime.MilliSeconds = Ref.rnd.Int(200, 400);
                image.position = startPos;
                TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(modelScale * 0.4f, modelScale * 0.5f);
                createAiPhys();
                aiPhys.accelerateTowardsFlyPathPerc = 0.05f;
            //}
        }

        //public Moth3(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (checkOutsideUpdateArea_ActiveChunk())
                {
                    DeleteMe();
                    return;
                }
                Vector3 targetPos = target.Position;
                targetPos.Y += 1f;

                if (!targetOffsetTime.CountDown())
                {
                    targetPos += targetOffset;
                }

                aiPhys.MovUpdate_MoveTowards(targetPos, 0, walkingSpeed);
            }
            updateAnimation();
            UpdateBound();
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            //base.AsynchGOUpdate(args);
            UpdateWorldPos();
            aiPhys.AsynchUpdate(args.time);
            checkCharacterCollision(args.localMembersCounter, false);

            if (WorldPos.BlockHasColllision())
            {
                UnthreadDeleteMe();
            }
        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            DeleteMe();
            //base.HandleObsticle(wallNotPit, ObjCollision);
        }

        protected override void onHitCharacter(AbsUpdateObj character)
        {
            UnthreadDeleteMe();
            //DeleteMe();
            //base.HitCharacter(character);
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
        }
        protected override void lootDrop()
        {
            //base.lootDrop();
        }

        public override void BlockSplatter()
        {
            //base.BlockSplatter();
        }
       // override

        public override GameObjectType Type
        {
            get { return GameObjectType.Moth; }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.FlyingAi;
            }
        }

        protected override bool animationUseMoveVelocity
        {
            get
            {
                return false;
            }
        }

        public override bool SolidBody
        {
            get
            {
                return false;
            }
        }

        public override float LightSourceRadius
        {
            get
            {
                return image.scale.X * 8;
            }
        }

        const float WalkingSpeed = 0.022f;
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
    }
}

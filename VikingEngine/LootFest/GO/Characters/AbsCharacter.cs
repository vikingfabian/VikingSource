using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna

namespace VikingEngine.LootFest.GO.Characters
{
    abstract class AbsCharacter : AbsGameObject
    {
        public AiState aiState = AiState.Waiting;
        protected Time aiStateTimer = 0;
        protected IntVector2 spawnChunk;
        
        public float StunnedSpeedModifier = 1;

        public AbsCharacter(GoArgs args)
            :base(args)
        {

        }

        virtual public void UpdateMissioni() { }
        protected AbsCharacter getClosestCharacter(float maxDistance, ISpottedArrayCounter<AbsUpdateObj> objects, WeaponAttack.WeaponUserType friendOrFoe)
        {
            FindMinValuePointer<AbsCharacter> lowestDist = new FindMinValuePointer<AbsCharacter>(true);
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetSelection is GO.Characters.AbsCharacter && (WeaponAttack.WeaponLib.IsFoeTarget(friendOrFoe, objects.GetSelection.WeaponTargetType, true)))
                {
                    float dist = distanceToObject(objects.GetSelection);
                    if (dist <= maxDistance)
                    {
                        lowestDist.Next(dist, objects.GetSelection as AbsCharacter);
                    }
                }
            }

            if (!lowestDist.hasValue)
                return null;

            return lowestDist.minMember;            
        }

        protected AbsCharacter getRndCharacter(float maxDistance, ISpottedArrayCounter<GO.AbsUpdateObj> counter, WeaponAttack.WeaponUserType ofType)
        {
            List<AbsUpdateObj> inView = null;
            counter.Reset();
            while (counter.Next())
            {
                if (counter.GetSelection is GO.Characters.AbsCharacter &&
                    counter.GetSelection.WeaponTargetType == ofType &&
                    distanceToObject(counter.GetSelection) <= maxDistance)
                {
                    if (inView == null)
                    {
                        inView = new List<AbsUpdateObj> { counter.GetSelection };
                    }
                    else
                    {
                        inView.Add(counter.GetSelection);
                    }
                }
            }

            if (inView == null)
            {
                return null;
            }
            else
            {
                return (AbsCharacter)arraylib.RandomListMember(inView);
            }
        }

        protected AbsCharacter getRndCharacterWithinView(float maxDistance, float maxAngle, ISpottedArrayCounter<GO.AbsUpdateObj> objects, WeaponAttack.WeaponUserType friendOrFoe)
        {
            List<AbsUpdateObj> inView = null;
            
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetSelection is GO.Characters.AbsCharacter &&
                    WeaponAttack.WeaponLib.IsFoeTarget(friendOrFoe, objects.GetSelection.WeaponTargetType, true) &&
                    distanceToObject(objects.GetSelection) <= maxDistance &&
                    LookingTowardObject(objects.GetSelection, maxAngle)
                    )
                {
                    if (inView == null)
                    {
                        inView = new List<AbsUpdateObj> { objects.GetSelection };
                    }
                    else
                    {
                        inView.Add(objects.GetSelection);
                    }
                }
            }

            if (inView == null)
            {
                return null;
            }
            else
            {
                return (AbsCharacter)arraylib.RandomListMember(inView);
            }
        }

        virtual protected void activeCheckUpdate()
        {
            if (localMember)
            {
                if (managedGameObject)
                {
                    checkSleepingState();
                }
                else
                {
                    if (checkOutsideUpdateArea_ActiveChunk())
                    {
                        DeleteMe();
                        return;
                    }
                }
            }
        }
        
        override public NetworkShare NetworkShareSettings
        {
            get
            {
                if (localMember)
                {
                    NetworkShare NetShare = GO.NetworkShare.Full;
                    NetShare.Update = !Velocity.ZeroPlaneSpeed || updatePositionToNewbie > 0 || Ref.rnd.Chance(20);
                    return NetShare;
                }
                else
                {
                    return  GO.NetworkShare.Full;
                }
            }
        }

       
        
        virtual protected Color damageTextCol { get { return Color.White; } }

        protected void characterCritiqalUpdate(bool outsideCheck)
        {
            immortalityTime.CountDown();
            UpdateBound();

            if (outsideCheck && localMember && checkOutsideUpdateArea_ActiveChunk())
            {
                DeleteMe();
            }
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
            

            if (damage.Damage > LfLib.MinVisualDamage || Dead)
            {
                //blood
                BlockSplatter();
            }
        }
        
        public override void Time_Update(UpdateArgs args)
        {
            if ((localMember || !NetworkShareSettings.Update) && 
                PhysicsType != ObjPhysicsType.GroundAi && PhysicsType != ObjPhysicsType.FlyingAi)
            {
                moveImage(Velocity, args.time);
            }

            updateAnimation();
            base.Time_Update(args);
        }
        public override void AsynchGOUpdate(UpdateArgs args)
        {
            UpdateWorldPos();
            base.AsynchGOUpdate(args);
        }

        virtual protected bool animationUseMoveVelocity { get { return true; } }

        virtual protected void updateAnimation()
        {
            float animSpeed;
            if (animationUseMoveVelocity)
            { animSpeed = localMember ? Velocity.PlaneLength() : clientSpeedLength; }
            else
            { animSpeed = 1f; }

            if (animSpeed <= 0f)
            {
                image.Frame = 0;
            }
            else
            {
                animSettings.UpdateAnimation(image, animSpeed, Ref.DeltaTimeMs);
            }
        }
        
        public override void Time_LasyUpdate(ref float time)
        {
            base.Time_LasyUpdate(ref time);
            
            updatePositionToNewbie--;
        }
        protected override void moveImage(Velocity speed, float time)
        {
            if (localMember)
            {
                speed.SetValue_Safe( speed.Value * StunnedSpeedModifier);
                image.position = physics.UpdateMovement();
            }
            else
                base.moveImage(speed, time);
        }
        protected override bool autoMoveImage
        {
            get
            {
                return false;
            }
        }

        virtual public void HandleCastleRoomCollision()
        { }

        
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return GO.ObjPhysicsType.CharacterSimple;
            }
        }
        public override ObjLevelCollType LevelCollType
        {
            get
            {
                return ObjLevelCollType.Standard;
            }
        }

        public Vector3 Scale
        {
            get { return image.scale; }
        }

        override public bool SolidBody
        {
            get { return true; }
        }
        public override bool IsWeaponTarget
        { get { return true; } }

        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.ReceiveDamage; }
        }

        public override void onGroundPounce(float fallSpeed)
        {
            if (fallSpeed > 0.03f)
            {
                int smokeCount;
                float smokeSpeed;

                if (fallSpeed < 0.06f)
                {
                    smokeCount = 12;
                    smokeSpeed = 1.6f;
                }
                else //high speed fall
                {
                    smokeCount =24;
                    smokeSpeed = 3f;
                }


                float radiansStep = MathHelper.TwoPi / smokeCount;
                Rotation1D dir = Rotation1D.Random();
                var particles =  new List<Graphics.ParticleInitData>(smokeCount);
                Vector3 particleCenter = image.position;
                particleCenter.Y -= 0.2f;

                for (int i = 0; i < smokeCount; ++i)
                {
                    Vector2 planeDir = dir.Direction(1f);
                    Vector3 start = particleCenter + VectorExt.V2toV3XZ(planeDir * 0.6f);
                    particles.Add(new Graphics.ParticleInitData(start, VectorExt.V2toV3XZ(planeDir * smokeSpeed)));
                    dir.Radians += radiansStep;
                }

                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.RunningSmoke, particles);
            }
        }

        public void UpdateAllChildObjects()
        {
            if (childObjects != null)
            {
                ChildObjectsCounter count = new ChildObjectsCounter(childObjects);
                while (count.Next())
                {
                    if (count.currentChild.ChildObject_Update(this))
                    {
                        count.RemoveCurrent(ref childObjects);
                    }
                }
            }
        }       
        
        protected void addStunnEffect()
        {
            if (aiState == AiState.IsStunned)
            {
                ChildObjectsCounter count = new ChildObjectsCounter(childObjects);
                while (count.Next())
                {
                    if (count.currentChild is Effects.StunEffect)
                    {
                        Effects.StunEffect eff = (Effects.StunEffect)count.currentChild;
                        eff.isDeleted = false;
                        eff.stunTimer = aiStateTimer;
                        return;
                    }
                }
            }
            aiState = AiState.IsStunned;
            new Effects.StunEffect(expressionEffectPosOffset, 2f, aiStateTimer, this);
        }

        protected void createNpcBounds(float scale)
        {
            this.TerrainInteractBound = LootFest.ObjSingleBound.QuickBoundingBoxFromFeetPos(new Vector3(scale * 5), 0f);
            this.CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(scale * 4.4f, scale * 8, 0f);
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (childObjects != null)
            {
                ChildObjectsCounter count = new ChildObjectsCounter(childObjects);
                while (count.Next())
                {
                    count.currentChild.ChildObject_OnParentRemoval(this);
                }
            }
        }

        protected void netWriteAiState()
        {
            NetworkWriteObjectState(this.aiState);
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            this.aiState = state;
        }

        public override void Force(Vector3 center, float force)
        {
            if (this.physics != null && !(this.physics is NoPhysics))
            {
                new PushForce(force, VectorExt.V3XZtoV2(image.position - center), this.physics);
            }
        }
        public void Jump(float force)
        {
            physics.Jump(force, image);
        }
       
        override public LightParticleType LightSourceType { get { return Graphics.LightParticleType.Shadow; } }

        virtual public void onAiJumpOverObsticle() { }

        override public System.IO.FileShare BoundSaveAccess
        {
            get { return System.IO.FileShare.Read; }
        }
    }
    
}

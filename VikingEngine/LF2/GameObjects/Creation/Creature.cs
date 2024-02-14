using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GameObjects;
using VikingEngine.LootFest.GameObjects.Characters;

namespace VikingEngine.LootFest.Creation
{
    class Creature : GameObjects.Characters.AbsEnemy
    {
        const float WalkingSpeed = 0.01f;
        bool walkingMode = false;
        float modeTimeLeft = 0;
        CreatureScale scale;
        AnimationSpeed animationSpeed;
        CritterAI ai;
        GameObjects.Characters.Hero target;
        float goalY;

        //floating
        float wantedDistance; //= 5;
        float wantedHeightAboveHero = 4;
        Rotation1D goalDir = Rotation1D.Random;
        int moveDirAddDir = lib.RandomDirection();

        public Creature(Map.WorldPosition pos, Graphics.AbsVoxelObjMaster master, 
            CreatureScale scale, AnimationSpeed animationSpeed, CritterAI ai, GameObjects.Characters.Hero target)
            : base(0)
        {

            this.ai = ai;
            if (ai == CritterAI.Floating || ai == CritterAI.Flying)
            {
                physType = ObjPhysicsType.Projectile;
                wantedDistance = 5 + lib.RandomFloat(3);
                if (ai == CritterAI.Flying)
                {
                    goalDir = Rotation1D.Random;
                    const float FlyingMoveSpeed = 0.02f;
                    Velocity = new Velocity( goalDir, FlyingMoveSpeed);
                }
            }
            else
            {
                physType = ObjPhysicsType.Character;
            }
            addPhysics();

            this.target = target;
            this.scale = scale;
            WorldPosition = pos;
            this.animationSpeed = animationSpeed;
            basicInit(master);
            NetworkShareObject();
            Health = 5;
        }

        public Creature(System.IO.BinaryReader packetReader, Graphics.AbsVoxelObjMaster master)
            : base(packetReader)
        {
             this.scale = (CreatureScale)packetReader.ReadByte();
             this.animationSpeed = (AnimationSpeed)packetReader.ReadByte();
             basicInit(master);
        }

        

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)scale);
            writer.Write((byte)animationSpeed);
        }

        void basicInit(Graphics.AbsVoxelObjMaster master)
        {
            image = new Graphics.VoxelModelInstance(master,
                new Graphics.AnimationsSettings(master.NumFrames, animSpeedPerFrame()));
            image.Position = WorldPosition.ToV3();
            image.Position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition) + 2;


            float sz = critterSz();
            image.Scale = Vector3.One * sz * master.OneScale;

            CollisionBound = LootFest.ObjSingleBound.QuickBoundingBox(0.24f * sz);
            LfRef.gamestate.NumEnemies++;
        }

        float animSpeedPerFrame()
        {
            switch (animationSpeed)
            {
                case AnimationSpeed.VerySlow:
                    return 2f;
                case AnimationSpeed.Slow:
                    return 1.6f;
                case AnimationSpeed.Normal:
                    return 1.1f;
                case AnimationSpeed.Fast:
                    return 0.7f;
                case AnimationSpeed.VeryFast:
                    return 0f;

            }
            return 0;
        }


        float critterSz()
        {
            float sz = 1;
            switch (scale)
            {
                case CreatureScale.Horse:
                    sz = 4.8f;
                    break;
                case CreatureScale.Human:
                    sz = 3;
                    break;
                case CreatureScale.Dog:
                    sz = 2.2f;
                    break;
                case CreatureScale.Cat:
                    sz = 1.5f;
                    break;
                case CreatureScale.Mouse:
                    sz = 1.1f;
                    break;

            }
            return sz;
        }

        static readonly IntervalF WalkingModeTime = new IntervalF(400, 1000);
        static readonly IntervalF WaitingModeTime = new IntervalF(400, 3000);

        public override void Time_Update(UpdateArgs args)
        {
            
            if (physType == ObjPhysicsType.Character)
            {
                base.Time_Update(args);
            }
            else
            {
                image.UpdateAnimation(Velocity.PlaneLength(), args.time);
                if (!Velocity.ZeroPlaneSpeed)//Speed != Vector2.Zero)
                {
                    oldVelocity = Velocity;
                    //image.Position.X += Speed.X * args.time;
                    //image.Position.Z += Speed.Y * args.time;

                    Velocity.PlaneUpdate(args.time, image);
                    //Speed.SetZeroPlaneSpeed();
                }

                float ydiff = goalY - image.Position.Y;
                image.Position.Y += ydiff * 0.05f;
            }


            if (ai == CritterAI.Floating)
            {
                //speed decline
                Velocity *= 0.84f;

                goalDir += lib.RandomFloat(0.06f) * moveDirAddDir;
                if (lib.PercentChance(1))
                {
                    moveDirAddDir = -moveDirAddDir;
                    wantedHeightAboveHero = 3 + lib.RandomFloat(3);
                }
                Velocity.Add( goalDir, 0.001f);
                
            }
            else if (ai == CritterAI.Flying)
            {
               float aDiff =  rotation.AngleDifference(goalDir.Radians);
               rotateTowardsGoalDir(goalDir.Radians, 0.001f, args.time);
               setImageDirFromRotation();
               if (lib.PercentChance(0.008f))
               {
                   goalDir = Rotation1D.Random;
                   wantedHeightAboveHero = 4 + lib.RandomFloat(8);
               }
            }

            setImageDirFromSpeed();
            
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
           
            base.AIupdate(args);

            //if (checkOutsideUpdateArea_ClosestHero())
            //    UnthreadedDeleteMe();
            if (localMember)
            {
                switch (ai)
                {
                    default:
                        modeTimeLeft -= args.time;
                        if (modeTimeLeft <= 0)
                        {
                            walkingMode = !walkingMode;
                            bool follow = false;
                            if (ai != CritterAI.Critter && distanceToObject(target) >= 8)
                            {
                                follow = ai == CritterAI.Follower || lib.PercentChance(20);
                            }

                            if (follow)
                            {
                                moveTowardsObject(target, 0, WalkingSpeed, 0.5f);
                                modeTimeLeft = WalkingModeTime.GetRandom();
                            }
                            else
                            {
                                if (walkingMode)
                                {
                                    Rotation = Rotation1D.Random;
                                    Velocity.Set(rotation, WalkingSpeed);
                                    modeTimeLeft = WalkingModeTime.GetRandom();
                                    if (ai == CritterAI.Follower)
                                    {
                                        modeTimeLeft *= 0.4f;
                                    }
                                }
                                else
                                {
                                    Velocity.SetZeroPlaneSpeed();
                                    modeTimeLeft = WaitingModeTime.GetRandom();
                                }
                            }
                        }
                        break;
                    case CritterAI.Floating:
                        SolidBodyCheck(args.allMembersCounter);
                        goalY = target.Position.Y + wantedHeightAboveHero;
                        Vector2 diff = PositionDiff(target);
                        if (diff.Length() > wantedDistance)
                        {
                            //boost towards player
                            diff.Normalize();
                            Velocity.Add(diff * 0.002f);
                            goalDir = Rotation1D.FromDirection(diff);
                        }
                        break;
                    case CritterAI.Flying:
                        goalY = target.Position.Y + wantedHeightAboveHero;
                        const float MaxDistance = Map.WorldPosition.ChunkWidth * 3;

                        if (distanceToObject(target) >= MaxDistance)
                        {
                            goalDir.Radians = AngleDirToObject(target);
                        }
                        break;
                }
            }
            
        }

        public void UpdateMasterImg(Graphics.AbsVoxelObjMaster master)
        {
            Vector3 pos = image.Position;
            image.DeleteMe();
            image = new Graphics.VoxelModelInstance(master,
                new Graphics.AnimationsSettings(master.NumFrames, animSpeedPerFrame()));
            image.Scale = image.Scale = Vector3.One * critterSz() * master.OneScale;
            image.Position = pos;
        }

        

        protected override void clientSpeed(float speed)
        {
            image.UpdateAnimation(speed, Ref.DeltaTimeMs);
        }
        
        protected override LoadedSound HurtSound
        {
            get
            {
                return LoadedSound.MonsterHit1;
            }
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, LootFest.ObjBoundCollData collisionData)
        {
            return true;//do nothing
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, AbsUpdateObj ObjCollision)
        {
            if (ai == CritterAI.Floating)
            {
                //collData.planePosDiff.Normalize();
                //speed -= collData.planePosDiff * 0.001f;
                //goalDir = Rotation1D.FromDirection(-collData.planePosDiff);
                Vector3 depth = CollisionBound.IntersectionDepthAndDir(collData);
                goalDir = Rotation1D.FromDirection(-Map.WorldPosition.V3toV2(depth));
            }
            else if (ai == CritterAI.Flying)
            {
                if (ObjCollision != null)
                {
                    base.HandleColl3D(collData, ObjCollision);
                    if (lib.PercentChance(50))
                    {
                        Velocity.SetZeroPlaneSpeed();
                    }
                    else
                    {
                        //collData.planePosDiff.Normalize();
                        Vector3 depth = CollisionBound.IntersectionDepthAndDir(collData);
                        Vector2 diff = Map.WorldPosition.V3toV2(depth);
                        diff.Normalize();
                        Velocity.PlaneValue = -diff * WalkingSpeed;
                    }
                }
            }
            else
            {
                base.HandleColl3D(collData, ObjCollision);
                Velocity.SetZeroPlaneSpeed();
                modeTimeLeft = 0;
            }
        }
        
        protected override void DeathEvent(bool local, VikingEngine.LootFest.GameObjects.WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
#if !CMODE
            //new PickUp.DeathLoot(image.Position, Data.Characters.WorldCharacters.CritterLoot(critterType));
#endif
        }

        ObjPhysicsType physType = ObjPhysicsType.NO_PHYSICS;
        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return physType;
            }
        }

        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Craeture; }
        }
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }

    }

    enum CreatureScale
    {
        Horse,
        Human,
        Dog,
        Cat,
        Mouse,
        NUM
    }
    enum CritterAI
    {
        Critter,
        Social,
        Follower,
        Floating,
        Flying,
        NUM
    }
    enum AnimationSpeed
    {
        VerySlow,
        Slow,
        Normal,
        Fast,
        VeryFast,
        NUM
    }
}

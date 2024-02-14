using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters
{
    class Ogre : AbsEnemy
    {
        Map.WalkingPath walkingPath;
        Rotation1D walkingCykle = Rotation1D.Random();
        const float WalkingSpeed = 0.008f;
        float stateTime = 0;
        OgreState state = OgreState.Waiting;

        new const float Scale = 0.5f;
        //Limb RLeg, LLeg, RArm, LArm;

        AddFloatPerTime rotate = new AddFloatPerTime();
        float destructionDelay = 6000;


        public Ogre(GoArgs args)
            :base(args)
        {
            bosslevel = 1;
            //startPos.Y = LfRef.chunks.GetScreen(startPos).GetGroundY(startPos) + 1;
            WorldPos = args.startWp;

            Health = 100;
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(7f * Scale);
            //image = LootfestLib.Images.StandardObjInstance(VoxelObjName.ogre_body);
            image.scale = Vector3.One * Scale;

            image.position = WorldPos.PositionV3;

            // Unused vars
            //const float ArmScale = Scale;
            //const float LegScale = Scale;

            //Vector3 legRelPos = new Vector3(5, 10, 0) * Scale;
            //RLeg = new Limb(false, legRelPos, LootfestLib.Images.OgreLeg1, LegScale);
            //LLeg = new Limb(true, legRelPos, LootfestLib.Images.OgreLeg2, LegScale);
            //Vector3 armRelPos = new Vector3(11, 26, 2) * Scale;
             //RArm = new Limb(false, armRelPos, LootfestLib.Images.OgreArm1, ArmScale);
            //LArm = new Limb(true, armRelPos, LootfestLib.Images.OgreArm2, ArmScale);

        }

        static readonly Vector2 LegMove = new Vector2(3, 3) * Scale;
        
        
        public override void Time_Update(UpdateArgs args)
        {

            Velocity.SetZeroPlaneSpeed();
            if (state == OgreState.Rotating)
            {
                rotation.Add(rotate.NextStep(args.time));
                setImageDirFromRotation();
            }
            else if (walkingPath == null)
            {

            }
            else
            {
                Velocity.PlaneValue = walkingPath.WalkTowardsNode(image.position, rotation, 0.0001f * args.time, WalkingSpeed);
                if (Velocity.ZeroPlaneSpeed)
                {
                    walkingPath = null;
                }
                else
                {
                    setImageDirFromSpeed();
                    oldVelocity = Velocity;
                    moveImage(Velocity, args.time);
                }
            }
            
            const float MinY = Map.WorldPosition.ChunkStandardHeight - 2;
            image.position.Y = Bound.Min(image.position.Y, MinY);
            destructionDelay -= args.time;
            //SET limbs pos
            //RLeg.Update(image);
            //LLeg.Update(image);
            //RArm.Update(image);
            //LArm.Update(image);

            if (state == OgreState.Rotating || walkingPath != null)
            {


                walkingCykle.Add(0.4f * args.time * WalkingSpeed);
                Rotation1D walkingCykleInv = walkingCykle;
                walkingCykleInv.Radians += MathHelper.Pi;

                Vector2 cyclePos1 = walkingCykle.Direction(1);
                Vector2 cyclePos2 = walkingCykleInv.Direction(1);


                //const float ArmRotationLength = 0.2f;
                //RArm.RelRotation.Y = cyclePos1.X * ArmRotationLength;
                //LArm.RelRotation.Y = cyclePos2.X * ArmRotationLength;

                //const float LegRotationLength = 0.28f;
                //LLeg.RelRotation.Y = cyclePos1.X * LegRotationLength;
                //RLeg.RelRotation.Y = cyclePos2.X * LegRotationLength;

                //  image.Rotation.Zradians = cyclePos1.X * 0.05f;

                Vector2 leg1Pos = cyclePos1 * LegMove;
                Vector2 leg2Pos = cyclePos2 * LegMove;

                //RLeg.Position.Z = leg1Pos.X;
                //RLeg.Position.Y = Bound.SetMinVal(leg2Pos.Y, 0);


                //LLeg.Position.Z = leg2Pos.X;
                //LLeg.Position.Y = Bound.SetMinVal(leg1Pos.Y, 0);

                if (destructionDelay < 0)
                {
                    //Pick a random pos and see if it collides with terrain
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 collCheckPos = new Vector3(Ref.rnd.Plus_MinusF(Scale * 14), (12 + Ref.rnd.Float(36)) * Scale, Ref.rnd.Plus_MinusF(Scale * 8));
                        collCheckPos = image.Rotation.TranslateAlongAxis(collCheckPos, image.position);
                        Map.WorldPosition wp = new Map.WorldPosition(collCheckPos);

                        ushort m = wp.GetBlock();//LfRef.chunks.Get(wp);
                        if (m != VikingEngine.LootFest.Map.HDvoxel.BlockHD.EmptyBlock)
                        {
                           // Music.SoundManager.PlaySound(LoadedSound.terrain_destruct1, collCheckPos);
                            LfRef.chunks.TerrainDestruction(collCheckPos, 5);
                            new LootFest.Effects.BouncingBlock2(collCheckPos, (Data.MaterialType)m, 1);
                            new LootFest.Effects.BouncingBlock2Dummie(collCheckPos, (Data.MaterialType)m, 1);
                        }
                    }
                }
                //setImageDirFromRotation();
            }
        }
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            if (localMember)
            {
                stateTime -= args.time;
                if (stateTime <= 0)
                {
                    //change state
                    if (state == OgreState.Waiting)
                    {
                        if (Ref.rnd.Chance(10))
                        {
                            rotatingState();
                        }
                        else
                        {
                            //float moveSpeed = WalkingSpeed;
                            target = getClosestCharacter(32, args.allMembersCounter, this.WeaponTargetType);
                            //setGoalDir(AngleDirToObject(target));
                            if (target != null)
                            {//attack
                                //state = OgreState.Attacking;
                                walkingPath = walkingPathTowardsObject(target);
                            }
                            else //walk around
                            {
                                state = OgreState.Walking;
                                //setGoalDir(Ref.rnd.Rotation());
                                walkingPath = LfRef.chunks.PathFindRandomDir(image.position);
                            }
                            //Speed.Set(rotation, moveSpeed);
                            stateTime = 3000;
                        }
                    }
                    else
                    {
                        state = OgreState.Waiting;
                        walkingPath = null;
                        //Speed.SetZeroPlaneSpeed();
                        stateTime = 600 + Ref.rnd.Int(1600);

                        //LLeg.RelRotation = Vector3.Zero;
                        //LLeg.Position = Vector3.Zero;

                        //RLeg.RelRotation = Vector3.Zero;
                        //RLeg.Position = Vector3.Zero;

                    }


                }
            }
            base.AsynchGOUpdate(args);
            SolidBodyCheck(args.localMembersCounter);
            //if (checkOutsideUpdateArea_ClosestHero())
            //    UnthreadedDeleteMe();
        }

        //protected override void handleObsticleColl2D(Physics.CollisionIntersection2D collData)
        //{
        //    base.handleObsticleColl2D(collData);
            
        //}
        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //if (ObjCollision == null)
            //{
            //    base.HandleColl3D(collData, ObjCollision);
            //    if (ObjCollision == null && Ref.rnd.RandomChance(5))
            //    {
            //        rotatingState();
            //    }
            //}
        }

        void rotatingState()
        {
            //LfRef.gamestate.LocalHostingPlayerPrint("Ogre rot", SpriteName.ArmourGold);
            state = OgreState.Rotating;
            Velocity.SetZeroPlaneSpeed();
            setGoalDir(rotation.Radians + MathHelper.Pi * Ref.rnd.LeftRight());
            stateTime = 1200 + Ref.rnd.Int(2000);
        }

        void setGoalDir(float dir)
        {
            const float RotationSpeed = 0.25f;
            const float RotationSpeedStandingStill = 0.9f;
            rotate = new AddFloatPerTime(rotation.AngleDifference(dir),state == OgreState.Rotating? RotationSpeedStandingStill : RotationSpeed);
        }


        //public override Data.Characters.EnemyType EnemyType
        //{
        //    get { return Data.Characters.EnemyType.Giant; }
        //}

        protected override LoadedSound HurtSound
        {
            get
            {
                //if (Ref.rnd.RandomChance(20))
                //{
                //    return LoadedSound.ogre_hurt1;
                //}
                return LoadedSound.NON;
            }
        }
        protected override bool pushable
        {
            get
            {
                return false;
            }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Ogre; }
        }
        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            //for (int i = 0; i < 3; i++)
            //{
            //    new PickUp.HumanoidLoot(image.Position, EnemyValueLevel);
            //}
        }
        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            //RLeg.DeleteMe();
            //LLeg.DeleteMe();
            //RArm.DeleteMe();
            //LArm.DeleteMe();
        }
    }

    enum OgreState
    {
        Waiting,
        Rotating,
        Walking,
        Attacking,
        NUM
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest
{
    //Bör innehålla ett smidigt sätt att sätta mål positioner, ska ta sig fram genom hinder
    //Kunna hantera rörelser som mjukt vända sig om
    class AiPhysicsLf3 : AbsPhysics
    {
        const float PushForceTime = 200;
        const float PushForceFadeTime = 80;
        const float PushForceTotalTime = PushForceTime + PushForceFadeTime;
        Vector2 forceVector;
        Time pushForceTimer = 0;
        const float PushForceMultiplier = 0.025f;

        public float SlideUpLengthPerSec = 2f;

        public SimplePathFinder path;
        public SimpleFlyPath flyPath;
        Vector3 flyDir = Vector3.Zero;
        public float accelerateTowardsFlyPathPerc = 0.15f;

        protected bool physicsStatusFalling = true;
        override public bool PhysicsStatusFalling
        { get { return physicsStatusFalling; } }

        float groundY;
        public float yAdj = 0.0f;

        bool flyingState;

        bool prevStandStillAndRotating = false;
        public bool movementHasRotateLimit = true;
        public const float StandardRotateSpeedMoving =  0.003f;
        public float rotateSpeedMoving = StandardRotateSpeedMoving;
        public float rotateSpeedStanding = StandardRotateSpeedMoving * 2f;
        public float maxWalkAndRotateAngle = MathHelper.PiOver2;


        public AiPhysicsLf3(GO.Characters.AbsCharacter parent, bool flyingState)
            : base(parent)
        {
            FlyingState = flyingState;

            groundY = GetGroundY().slopeY + yAdj;
        }

        public void AddPushForce(VikingEngine.LootFest.GO.WeaponAttack.DamageData damage)
        {
            AddPushForce(damage.PushDir.Direction(damage.Damage));
        }

        public void AddPushForce(Vector2 forceVector)//float power, Vector2 dir)
        {
            pushForceTimer.MilliSeconds = PushForceTotalTime;
            this.forceVector = forceVector * PushForceMultiplier;
        }

        public bool FlyingState
        {
            get { return flyingState; }
            set
            {
                //if (flyingState != value)
                //{
                    if (value)
                    {
                        if (flyPath == null)
                        { flyPath = new SimpleFlyPath((GO.Characters.AbsCharacter)parent); }
                    }
                    else
                    {
                        if (path == null)
                        { path = new SimplePathFinder((GO.Characters.AbsCharacter)parent); }
                    }

                    flyingState = value;
                //}
            }
        }

        override public GroundWithSlopesData GetGroundY()
        {
            Vector3 pos = parent.Position;
            pos.Y -= yAdj;
            //GroundWithSlopesData data = groundYwithSlopes2(pos);
            Map.WorldPosition wp = new Map.WorldPosition(pos);
            GroundWithSlopesData data = new GroundWithSlopesData(wp.Screen.GetClosestFreeY(wp));

            physicsStatusFalling = data.slopeY < pos.Y || parent.Velocity.Y > 0.000001f;

            return data;
        }

        public void MovUpdate_FleeFrom(AbsUpdateObj target, float speed, float goalY)
        {
            if (target != null)
            {
                Vector3 diff = parent.Position - target.Position;
                diff.Y = 0;
                diff.Normalize();
                Vector3 goal = parent.Position + diff * 32;
                if (flyingState)
                {
                    goal.Y = goalY;
                }
                MovUpdate_MoveTowards(goal, 0, speed);
            }
        }

        //move towards
        public bool MovUpdate_MoveTowards(AbsUpdateObj target, float minDistance, float speed)
        {
            if (target == null)
            {
                return false;
            }

            return MovUpdate_MoveTowards(target.Position, minDistance, speed);
        }

        /// <returns>Is moving</returns>
        public bool MovUpdate_MoveTowards(Vector3 position, float minDistance, float speed)
        {
            //Två steg, förbered path finder, och jobba med det man har
            bool isMoving = true;

            if (flyingState)
            {
                flyPath.goalPos = position;
                if (flyPath.lengthToGoal > minDistance)
                {
                    updateFlyDir(flyPath.pathDir);
                    parent.Velocity.Value = flyDir * speed;
                    parent.setImageDirFromSpeed();
                }
                updateMovement();
            }
            else
            {
                path.goalPos = position;

                if (!physicsStatusFalling)
                {
                    if (path.lengthToGoal > minDistance)
                    {
                        if (rotateTowardsGoal(lib.V2ToAngle(path.pathDir), ref isMoving, speed))
                        {
                            //Rotating
                        }
                        else
                        {
                            //No rotation
                            parent.Velocity.PlaneValue = path.pathDir * speed;
                            parent.setImageDirFromSpeed();
                        }
                        if (path.jump)
                        {
                            if (!physicsStatusFalling)
                            {
                                SpeedY = 0.012f;
                                physicsStatusFalling = true;
                                ((GO.Characters.AbsCharacter)parent).onAiJumpOverObsticle();
                            }
                            path.jump = false;
                        }

                    }
                    else
                    {
                        parent.Velocity.SetZeroPlaneSpeed();
                        isMoving = false;
                    }
                }
                updateMovement();
            }

            return isMoving;
        }

        bool rotateTowardsGoal(float angleToTarget, ref bool isMoving, float speed)
        {
            if (movementHasRotateLimit)
            {
               // float angleToTarget = lib.V2ToAngle(path.pathDir);//parent.AngleDirToObject(position);
                float angleDiff = parent.Rotation.AngleDifference(angleToTarget);

                if (Math.Abs(angleDiff) > rotateSpeedMoving * Ref.DeltaTimeMs)
                {
                    //prevStandStillAndRotating: fortsätter att rotera en liten stund annars blir mostret väldigt ryckig
                    if (Math.Abs(angleDiff) > (prevStandStillAndRotating ? maxWalkAndRotateAngle * 0.6f : maxWalkAndRotateAngle))
                    {
                        parent.rotateTowardsGoalDir(angleToTarget, angleDiff, rotateSpeedStanding, 0f);
                        parent.setImageDirFromRotation();
                        //Standing still and rotating
                        parent.Velocity.SetZeroPlaneSpeed();
                        isMoving = false;
                        prevStandStillAndRotating = true;
                    }
                    else
                    {
                        parent.rotateTowardsGoalDir(angleToTarget, rotateSpeedMoving, speed);
                        //parent.Velocity.Set(parent.Rotation.Radians, speed);
                        parent.setImageDirFromRotation();
                        prevStandStillAndRotating = false;
                    }
                    return true;
                }
            }
            return false;
        }

        void updateFlyDir(Vector3 dir)
        {
            if (flyDir == Vector3.Zero)
            {
                flyDir = dir;
            }
            else if (Ref.TimePassed16ms)
            {
                flyDir = flyDir * (1f - accelerateTowardsFlyPathPerc) + flyPath.pathDir * accelerateTowardsFlyPathPerc;
                flyDir.Normalize();
            }
            
        }

        public void MovUpdate_MoveForward(Rotation1D dir, float speed)
        {
            MovUpdate_MoveForward(dir, speed, parent.Y);
        }

        public void MovUpdate_MoveForward(Rotation1D dir, float speed, float goalHeight)
        {
            if (flyingState)
            {
                if (Ref.rnd.Chance(5))
                {
                    Vector3 goal = parent.Position;
                    Vector2 planeDir = dir.Direction(64);
                    goal.X += planeDir.X;
                    goal.Y = goalHeight;
                    goal.Z += planeDir.Y;
                    flyPath.goalPos = goal;
                }
                updateFlyDir(flyPath.pathDir);
                parent.Velocity.Value = flyDir * speed;
                parent.setImageDirFromSpeed();
            }
            else
            {
                if (!physicsStatusFalling)
                {
                    bool isMoving = true;
                    if (rotateTowardsGoal(dir.Radians, ref isMoving, speed))
                    {
                        //Rotating
                    }
                    else
                    {
                        //No rotation
                        parent.Velocity.Set(dir, speed);
                    }

                    if (!isMoving)
                    {
                        parent.Velocity.SetZeroPlaneSpeed();
                    }
                    else if (path.PathCollisionCheck(parent.Position, parent.Velocity))
                    {
                        parent.Velocity.SetZeroPlaneSpeed();
                        parent.HandleObsticle(true, null);
                    }
                    
                    parent.setImageDirFromRotation();
                }
            }
            updateMovement();
        }

        public override void PushForce(Vector2 force)
        {
            throw new Exception("Use the new AddPushForce instead");
        }

        public override void Jump(float force, Graphics.AbsVoxelObj image)
        {
            parent.Velocity.Y += 0.012f * force;
            physicsStatusFalling = true;
            path.jump = false;
        }

        public void MovUpdate_StandStill()
        {
            if (!flyingState)
            {
                if (!physicsStatusFalling)
                { parent.Velocity.SetZeroPlaneSpeed(); }
                updateMovement();
            }
        }

        public void MovUpdate_RotateTowards(AbsUpdateObj target, float rotateSpeed)
        {
            MovUpdate_StandStill();
            parent.rotateTowardsObject(target, rotateSpeed);
            parent.setImageDirFromRotation();
        }

        public bool MovUpdate_FallToGround(float gravity)
        {
            if (Ref.TimePassed16ms)
            {
                parent.Velocity.Y += gravity;
            }

            Vector3 pos = parent.Position;
            pos += parent.Velocity.Value * Ref.DeltaTimeMs;

            bool hitGround = pos.Y <= groundY;
            
            if (hitGround)
            {
                pos.Y = groundY;
                parent.Velocity.Value = Vector3.Zero;
            }

            parent.Position = pos;
            return hitGround;
        }


        public void updateMovement()
        {
            Velocity v = parent.Velocity;
            if (!pushForceTimer.CountDown())
            {
                if (Ref.TimePassed16ms && pushForceTimer.MilliSeconds < PushForceFadeTime)
                {
                    forceVector *= 0.8f;
                }
                v.Add(forceVector);
            }
            

            Vector3 pos = parent.Position;
            pos += v.Value * Ref.DeltaTimeMs;

            if (!flyingState)
            {
                if (physicsStatusFalling)
                {
                    if (Ref.TimePassed16ms)
                    {
                        parent.Velocity.Y += Gravity;
                    }
                    bool hitGround = pos.Y <= groundY;

                    if (hitGround)
                    {
                        pos.Y = groundY;
                        parent.Velocity.Value.Y = 0;
                        physicsStatusFalling = false;
                    }
                }
                else
                {
                    
                    float groundDiff = groundY - pos.Y;
                    float maxSlideY = SlideUpLengthPerSec * Ref.DeltaTimeSec;
                    if (Math.Abs(groundDiff) > maxSlideY)
                    {
                        pos.Y += maxSlideY * lib.ToLeftRight(groundDiff);
                    }
                    else
                    {
                        pos.Y = groundY;
                    }
                    parent.Velocity.Y = 0;
                }
            }
            parent.Position = pos;
        }

        public override void AsynchUpdate(float time)
        {
            groundY = GetGroundY().slopeY + yAdj;

            if (flyingState)
            { flyPath.AsynchUpdate(time); }
            else
            { path.AsynchUpdate(time); }
        }

        public void hitObsticle()
        {
            if (flyingState)
            { flyPath.hitObsticle(); }
            else
            { path.hitObsticle(); }
        }
    }

   
}

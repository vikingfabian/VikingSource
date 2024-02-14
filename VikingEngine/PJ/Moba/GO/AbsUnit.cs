using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Moba.GO
{    
    abstract class AbsUnit : AbsGameObject
    {   
        protected bool positiveDir = true;
        Vector2 position;
        Rotation1D moveBobb = Rotation1D.D0;
        float moveBobbSpeed = 0.3f;
        float moveBobbHeight;

        int asynchCollectState_0BuildNew_1NewReady = 0;
        public List<AbsUnit> unitsInRange = new List<AbsUnit>(8);
        List<AbsUnit> unitsInRange_building = new List<AbsUnit>(8);

        AbsUnit attackTarget = null;
        public Time attackAnimation = Time.Zero;
        Timer.Basic attackCooldownTime;

        public Physics.CircleBound bound;
        float attackRange;
        protected float walkingSpeed = 1f;

        protected ValueBar health;
        public Vector2 walkingTowardsGoal;

        protected void initUnit(SpriteName sprite, Vector2 pos, float scale, float boundScale, float attackFrequency)
        {
            this.position = pos;
            initImage(sprite, pos, scale);
            MobaRef.objects.units.Add(this);

            moveBobbHeight = image.Height * 0.02f;

            bound = new Physics.CircleBound(pos, MobaLib.UnitScale * boundScale * 0.5f);
            attackRange = MobaLib.UnitScale * 0.2f;

            attackCooldownTime = new Timer.Basic(attackFrequency, false);
            attackCooldownTime.SetZeroTimeLeft();
        }

        override public void Update()
        {
            if (asynchCollectState_0BuildNew_1NewReady == 1)
            {
                lib.SwitchPointers(ref unitsInRange, ref unitsInRange_building);
                asynchCollectState_0BuildNew_1NewReady = 0;
            }
            bool standStill;
            updateAttack(out standStill);

            if (!standStill)
            {
                
                if (attackTarget != null && this is Minion)
                {
                    walkingTowardsGoal = attackTarget.position;
                }
                else
                {
                    //Follow lane
                    walkingTowardsGoal = positiveDir ? MobaRef.map.line.P2 : MobaRef.map.line.P1;
                }
                

                Vector2 moveSpeed;
                if (tryWalkTowards(walkingTowardsGoal, walkingSpeed, out moveSpeed))
                {
                    updateBobbing(moveSpeed.Length());
                }
                updateDepth();

                image.Position = position;
                bound.Center = position;
                image.Ypos += VikingEngine.Bound.Max(MathExt.Sinf(moveBobb.radians), 0f) * moveBobbHeight;
            }
        }

        
        bool tryWalkTowards(Vector2 goal, float speed, out Vector2 moveSpeed)
        {
            const float MaxWalkAngle = 1.7f;
            const float AngleStep = MaxWalkAngle * 0.2f;


            moveSpeed = Vector2.Zero;
            Vector2 diff = goal - position;

            image.spriteEffects = diff.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            

            if (diff.Length() > MobaLib.UnitScale * 0.1f)
            {
                Rotation1D angle = Rotation1D.FromDirection(diff);
                float speedLength = MobaLib.UnitScale * speed * Ref.DeltaTimeSec;

                float angleAdd = 0;
                bool ccDir = Ref.rnd.Bool();
                int angleDirChecks = 1000;

                while (angleAdd < MaxWalkAngle)
                {
                    Rotation1D tryAngle = angle;
                    tryAngle.Add(lib.BoolToLeftRight(ccDir) * angleAdd); 
                    moveSpeed = tryAngle.Direction(speedLength);

                    Vector2 tryPos = position + moveSpeed;
                    bound.Center = tryPos;

                    if (inCollision() == false)
                    {
                        position = tryPos;
                        return true;
                    }
                    else
                    {
                        lib.DoNothing();
                    }

                    if (angleDirChecks == 0)
                    {
                        angleDirChecks++;
                    }
                    else
                    {
                        angleDirChecks = 0;
                        angleAdd += AngleStep;
                    }
                    lib.Invert(ref ccDir);
                }

                ccDir = true;
                angle.Add(lib.BoolToLeftRight(ccDir) * angleAdd);
                moveSpeed = angle.Direction(speedLength);

                position += moveSpeed;
                bound.Center = position;
                pushCollisions();
            }

            return false;
        }

        void updateAttack(out bool standStill)
        {
            if (attackAnimation.HasTime)
            {
                attackAnimation.CountDown();
                standStill = true;
            }
            else if (attackCooldownTime.Active)
            {
                attackCooldownTime.Update();
                standStill = this is Minion;
            }
            else
            {
                //Ready to attack again
                float targetDistance = float.MaxValue;
                if (attackTarget == null)
                {
                    attackTarget = closestTarget(out targetDistance);
                }
                else if (attackTarget.Dead)
                {
                    attackTarget = null;
                }
                else
                {
                    targetDistance = boundDistanceTo(attackTarget);
                }

                if (attackTarget != null && targetDistance <= attackRange)
                {
                    standStill = true;
                    attack(attackTarget);
                }
                else
                {
                    standStill = false;
                }
            }
        }

        virtual protected void attack(AbsUnit target)
        {
            attackCooldownTime.Reset();
            attackAnimation.MilliSeconds = 300;

            new MeleeEffect(this, target);
            target.takeDamage(meleeDamage(), this);
        }

        abstract protected int meleeDamage();

        virtual public void takeDamage(int damage, AbsUnit fromAttacker)
        {
            this.attackTarget = fromAttacker;

            health.add(-damage);
            if (Dead)
            {
                DeleteMe();
            }
        }

        AbsUnit closestTarget(out float distance)
        {  
            FindMinValuePointer<AbsUnit> closest = new FindMinValuePointer<AbsUnit>(false);
            foreach (var m in unitsInRange)
            {
                if (isOpponent(m))
                {
                    var dist = boundDistanceTo(m);
                    //if (dist < attackRange)
                    {
                        closest.Next(dist, m);
                    }
                }
            }

            distance = closest.minValue;
            return closest.minMember;
        }

        bool inCollision()
        {
            foreach (var m in unitsInRange)
            {
                if (bound.Intersect2(m.bound).IsCollision)
                {
                    return true;
                }
            }
            return false;
        }

        void pushCollisions()
        {
            Physics.Collision2D coll;

            foreach (var m in unitsInRange)
            {
                coll = bound.Intersect2(m.bound);
                if (coll.IsCollision)
                {
                    Vector2 push = -coll.surfaceNormal;
                    m.position += push;
                    this.position -= push;
                }
            }
        }

        public void UpdateAsynch()
        {
            if (asynchCollectState_0BuildNew_1NewReady == 0)
            {
                unitsInRange_building.Clear();
                float range = MobaLib.UnitScale * 5f;

                var counter = MobaRef.objects.unitCounter.IClone();
                while (counter.Next())
                {
                    if (counter.GetSelection != this && distanceTo(counter.GetSelection) <= range)
                    {
                        unitsInRange_building.Add(counter.GetSelection);
                    }
                }
            }
            asynchCollectState_0BuildNew_1NewReady = 1;
            //lib.SwitchPointers(ref unitsInRange, ref unitsInRange_building);
        }

        float distanceTo(AbsUnit otherUnit)
        {
            return (this.position - otherUnit.position).Length();
        }

        float boundDistanceTo(AbsUnit otherUnit)
        {
            return VikingEngine.Bound.Min( (this.position - otherUnit.position).Length() - (this.bound.radius + otherUnit.bound.radius), 0f);
        }

        public bool isOpponent(AbsUnit otherUnit)
        {
            return this.blueTeam != otherUnit.blueTeam;
        }

        public bool isFriend(AbsUnit otherUnit)
        {
            return this.blueTeam == otherUnit.blueTeam;
        }

        void updateBobbing(float moveSpeed)
        {
            moveBobb.Add(moveSpeed * moveBobbSpeed);
        }

        virtual public void heal()
        {
            health.setMax();

            //heal effect
            Graphics.Image heart = new Graphics.Image(SpriteName.birdHeart, image.RealCenter, new Vector2(MobaLib.UnitScale * 0.2f), ImageLayers.AbsoluteBottomLayer, true);
            heart.LayerAbove(image);

            const float ViewTime = 600;
            new Graphics.Motion2d(Graphics.MotionType.MOVE, heart, new Vector2(0, -MobaLib.UnitScale), Graphics.MotionRepeate.NO_REPEAT,
                ViewTime, true);
            new Timer.Terminator(ViewTime, heart);
        }

        public void flipDir()
        {
            lib.Invert(ref positiveDir);
        }

        protected override void DeleteMe()
        {
            base.DeleteMe();
            MobaRef.objects.units.Remove(this);
        }

        public bool Alive { get { return health.HasValue; } }
        public bool Dead { get { return health.IsZero; } }

    }
}

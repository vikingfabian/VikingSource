using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.Bounds;
using VikingEngine.LootFest.Data;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.GO.Characters.Boss
{
    class ScorpionBot : AbsMonster3
    {
        public const float BodySize = 15f;
        
        ScorpionbotGoblin goblin;
        ScorpionbotNosePoint nosePoint;
        ScorpionbotKnife leftKnife, rightKnife;
        bool bLeftArm;

        int bulletsLeft;
        bool leftSideMissile = true;
        AbsBound[] storedCollisionBounds;

        public ScorpionBot(GoArgs args, BlockMap.AbsLevel level)
            : base(args)
        {
            nextRangedAttackTimer = new Time(4, TimeUnit.Seconds);
            modelScale = BodySize;
            createImage(VoxelModelName.scorpionbot, modelScale, 0, new Graphics.AnimationsSettings(5, 0.8f, 1));
            Health = 1f;
            loadBounds();

            foreach (var b in CollisionAndDefaultBound.Bounds)
            {
                b.ignoresDamage = true;
            }
            storedCollisionBounds = CollisionAndDefaultBound.Bounds;
            goblin = new ScorpionbotGoblin(image); AddChildObject(goblin);

            AbsBound[]  damageBounds = new AbsBound[2];
            leftKnife = new ScorpionbotKnife(true, this, damageBounds);
            rightKnife = new ScorpionbotKnife(false, this, damageBounds);

            DamageBound = new ObjectBound(damageBounds);
            DamageBound.DebugBoundColor(Color.Red);

            alwaysFullAttension();

            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.path.maxJumpUp = 6;
                aiPhys.path.maxJumpDown = 12;
                aiPhys.Gravity *= 0.86f;
                targetSearchDistanceIdle = 15; targetSearchDistanceTaunted = 24;

                NetworkShareObject();

                if (level != null)
                {
                    var manager = new Director.BossManager(this, level, Players.BabyLocation.NUM);
                }
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            updateMissileAttacks();
            
            if (leftKnife != null) leftKnife.Update(image);
            if (rightKnife != null) rightKnife.Update(image);

            UpdateAllChildObjects();
            DamageBound.updateVisualBounds();
            CollisionAndDefaultBound.updateVisualBounds();
        }


        override protected void updateAiMovement()
        {
            if (aiStateTimer.CountDown())
            {
                if (hasTarget())
                {
                    /*
                     * Follow
                     * Stand still
                     * Arm attack
                     */
                    if (aiState == AiState.Following)
                    {
                        aiState = AiState.RotatingTowardsGoal;
                        aiStateTimer.Seconds = Ref.rnd.Float(0.6f, 1f);
                    }
                    else if (aiState == AiState.RotatingTowardsGoal)
                    {
                        if (leftKnife == null && rightKnife == null)
                        {
                            aiState = AiState.Waiting;
                            aiStateTimer.Seconds = 1f;
                        }
                        else
                        {
                            Vector3 searchPos = image.position + VectorExt.V2toV3XZ(rotation.Direction(BodySize));
                            var hero = ClosestHero(searchPos, false);
                            if (hero != null && distanceToObject(hero) < 32)
                            {
                                storedTarget = hero;
                                bLeftArm = AngleDirToObject(hero) < 0f;

                                if (bLeftArm)
                                {
                                    if (leftKnife == null)
                                    { bLeftArm = false; }
                                }
                                else
                                {
                                    if (rightKnife == null)
                                    { bLeftArm = true; }
                                }

                                ScorpionbotKnife arm = bLeftArm ? leftKnife : rightKnife;
                                Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.ExplosionFire, arm.model.position, 2f, 40);
                                Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.Smoke, arm.model.position, 2f, 40);

                                aiState = AiState.PreAttack;
                                aiStateTimer.Seconds = 0.4f;
                            }
                            else
                            {
                                aiState = AiState.Waiting;
                                aiStateTimer.Seconds = 1f;
                            }
                        }
                    }
                    else if (aiState == AiState.PreAttack)
                    {
                        aiStateTimer.Seconds = 30f;
                        aiState = AiState.Attacking;
                    }
                    else if (aiState == AiState.Attacking)
                    {
                        
                    }
                    else
                    {
                        aiState = AiState.Following;
                        aiStateTimer.Seconds = Ref.rnd.Float(2f, 4f);
                    }
                }
                else
                {
                    aiState = AiState.Waiting;
                }
               
            }

          
            if (aiState == AiState.Following)
            {
                aiPhys.MovUpdate_MoveTowards(storedTarget, 1f, WalkingSpeed);
            }
            else if (aiState == AiState.RotatingTowardsGoal)
            {
                aiPhys.MovUpdate_RotateTowards(storedTarget, 0.0004f);
            }
            else if (aiState == AiState.Attacking)
            {
                ScorpionbotKnife arm = bLeftArm ? leftKnife : rightKnife;

                if (arm == null)
                {
                    aiState = AiState.Waiting;
                    aiStateTimer.Seconds = 1f;
                }
                else
                {
                    if (arm.extendArm(storedTarget))
                    {
                        aiState = AiState.Waiting;
                        aiStateTimer.Seconds = 1f;
                    }
                }

                aiPhys.MovUpdate_StandStill();
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }


        Timer.Basic preAttackTimer = new Timer.Basic(66, true);

        
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, modelScale * 0.3f, 0);
            }
        }

        void updateMissileAttacks()
        {
            if (hasTarget() && nextRangedAttackTimer.CountDown())
            {
                if (bulletsLeft-- <= 0)
                {
                    nextRangedAttackTimer.Seconds = 5f;
                    bulletsLeft = 4;
                }
                else
                {
                    nextRangedAttackTimer.Seconds = 1f;
                }

                float heightAdd = modelScale * 0.56f;
              
                Vector3 firePos = image.Rotation.TranslateAlongAxis(new Vector3(
                    modelScale * 0.06f * lib.BoolToLeftRight(leftSideMissile),
                    heightAdd,
                    -modelScale * 0.1f), image.position);

                Vector3 targetPos = firePos + VectorExt.V2toV3XZ(rotation.Direction(10), Bound.MaxAbs(storedTarget.HeadPosition.Y - this.image.position.Y, 4) - heightAdd + 0.5f);//.Position.Y

                new VikingEngine.LootFest.GO.WeaponAttack.Boss.ScorpionBotRocket(new GoArgs(firePos), storedTarget, rotation);

                leftSideMissile = !leftSideMissile;
            }
        }

        public void viewArmPiece(ScorpionbotArmPiece armPiece, bool view)
        {
            armPiece.model.Visible = view;

            if (view)
            {
                List<AbsBound> bounds = new List<AbsBound> { armPiece.collisionBound };
                bounds.AddRange(storedCollisionBounds);
                CollisionAndDefaultBound.Bounds = bounds.ToArray();
            }
            else
            {
                CollisionAndDefaultBound.Bounds = storedCollisionBounds;
            }

            CollisionAndDefaultBound.refreshVisualBounds();
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            
            if (leftKnife == null && rightKnife == null)
            {
                //Final blow
                base.handleDamage(damage, local);
            }
            else
            {
                if (bLeftArm)
                {
                    if (leftKnife != null)
                    {
                        leftKnife.explode();
                        leftKnife = null;
                    }
                }
                else
                {
                    if (rightKnife != null)
                    {
                        rightKnife.explode();
                        rightKnife = null;
                    }
                }
                

                if (leftKnife == null && rightKnife == null)
                {
                    //View final-blow-bound
                    storedCollisionBounds[0].ignoresDamage = false;
                    CollisionAndDefaultBound.Bounds = storedCollisionBounds;
                    nosePoint = new ScorpionbotNosePoint(image); AddChildObject(nosePoint);
                }
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.ScorpionBot; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsWeakPoint = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_yellow,
            Data.MaterialType.pure_red_orange);

        public static readonly Effects.BouncingBlockColors DamageColorsBody = new Effects.BouncingBlockColors(
           Data.MaterialType.light_blue, 
            Data.MaterialType.gray_75, 
            Data.MaterialType.dark_blue);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsBody;
            }
        }

        public override bool canBeStunned
        {
            get
            {
                return false;
            }
        }

        protected override bool pushable
        {
            get
            {
                return false;
            }
        }

        public override void Force(Vector3 center, float force)
        {
            base.Force(center, force);
        }

        protected const float WalkingSpeed = 0.009f;
        protected const float CasualWalkSpeed = StandardWalkingSpeed * 0.7f;


        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
            if (leftKnife != null) leftKnife.DeleteMe();
            if (rightKnife != null) rightKnife.DeleteMe();
        }
    }

    class ScorpionbotArmPiece : CharacterLimb
    {
        
        public AbsBound collisionBound;
        ScorpionbotKnife knife;

        public ScorpionbotArmPiece(bool left, ScorpionBot parent, ScorpionbotKnife knife)
        {
            this.knife = knife;
            float modelScale = ScorpionBot.BodySize * 0.15f;

            model = LfRef.modelLoad.AutoLoadModelInstance(
                VoxelModelName.scorpionbot_arm_yellow,
               modelScale, 0f, true);
            model.Visible = false;

            collisionBound = AbsBound.CreateBound(BoundShape.Cylinder, Vector3.Zero, new Vector3(modelScale * 0.6f), Vector3.Zero);
           
        }

        public override void Update(Graphics.AbsVoxelObj parent)
        {
            relpos = knife.relpos * 0.6f;
            base.Update(parent);

            collisionBound.updatePosition(model.position, 0f);
            
        }
    }

    class ScorpionbotKnife : CharacterLimb
    {
        public ScorpionbotArmPiece armPiece;
        ScorpionBot parent;
        AbsBound damageBound;

        int armState_0None_1Out_2Hold_3In = 0;
        Time armHoldTimer;

        float maxMoveLength;
        float moveSpeed;
        float moveLength = 0;

        float maxSideLength;
        float sideLength = 0;
        float sideSpeed;

        bool movingSideWays = false;

        Vector3 storedRelPos;

        public ScorpionbotKnife(bool left, ScorpionBot parent, AbsBound[] damageBounds)
        {
            this.parent = parent;
            float modelScale =  ScorpionBot.BodySize * 0.8f;
            model = LfRef.modelLoad.AutoLoadModelInstance(
                left? VoxelModelName.scorpionbot_arm_knife_l : VoxelModelName.scorpionbot_arm_knife_r, 
                modelScale, 0f, true);

            damageBound = AbsBound.CreateBound(BoundShape.Box1axisRotation, Vector3.Zero, 
                new Vector3(modelScale * 0.16f, modelScale * 0.06f, modelScale * 0.16f), //scale
                new Vector3(0, 0, modelScale * 0.3f)); //offset
            storedRelPos = new Vector3(lib.BoolToLeftRight(left) * ScorpionBot.BodySize * 0.23f, ScorpionBot.BodySize * 0.11f, ScorpionBot.BodySize * 0.25f);

            damageBounds[left ? 0 : 1] = damageBound;

            maxMoveLength = ScorpionBot.BodySize * 0.5f;
            moveSpeed = maxMoveLength / 200f;

            sideSpeed = moveSpeed * 0.2f;

            armPiece = new ScorpionbotArmPiece(left, parent, this);
        }

        public bool extendArm(AbsCharacter target)
        {
            if (armPiece == null)
            {
                //parent.viewArmPiece(armPiece, false);
                return true;
            }

            //SIDE MOVE
            maxSideLength = moveLength * 0.4f;

            Vector3 posDiff = target.Position - model.position;

            float chaseLength = movingSideWays ? 2f : 5f;
            movingSideWays = false;

            if (posDiff.Length() > chaseLength)
            {
                Rotation1D dir = Rotation1D.FromDirection(VectorExt.V3XZtoV2(posDiff));
                float angleToTarget = parent.Rotation.AngleDifference(dir.Radians);

                if (Math.Abs(angleToTarget) > 0.5f)
                {
                    sideLength += sideSpeed * lib.BoolToLeftRight(angleToTarget > 0) * Ref.DeltaTimeMs;
                    movingSideWays = true;
                }
            }
            sideLength = Bound.MaxAbs(sideLength, maxSideLength);

            //LENGTH EXTEND
            switch (armState_0None_1Out_2Hold_3In)
            {
                case 0:
                    parent.viewArmPiece(armPiece, true);
                    armState_0None_1Out_2Hold_3In++;
                    break;
                case 1:
                    moveLength += moveSpeed * Ref.DeltaTimeMs;
                    if (moveLength >= maxMoveLength)
                    {
                        moveLength = maxMoveLength;
                        armHoldTimer.Seconds = 2f;
                        armState_0None_1Out_2Hold_3In++;
                    }
                    break;
                case 2:
                    if (armHoldTimer.CountDown())
                    {
                        armState_0None_1Out_2Hold_3In++;
                    }
                    break;
                case 3:
                    moveLength -= moveSpeed * Ref.DeltaTimeMs;
                    if (moveLength <= 0)
                    {
                        moveLength = 0f;
                        armState_0None_1Out_2Hold_3In = 0;
                        parent.viewArmPiece(armPiece, false);
                        return true;
                    }
                    break;
            }
            return false;
        }

        public override void Update(Graphics.AbsVoxelObj parent)
        {
            relpos = storedRelPos;
            relpos.Z += moveLength;
            relpos.X += sideLength;
            base.Update(parent);
            damageBound.updatePosition(model.Rotation.TranslateAlongAxis(damageBound.offset, model.position), this.parent.Rotation.Radians);

            if (armState_0None_1Out_2Hold_3In != 0)
            {
                armPiece.Update(this.parent.image);
            }
        }

        public static readonly Effects.BouncingBlockColors DamageColors = new Effects.BouncingBlockColors(
           Data.MaterialType.light_blue,
            Data.MaterialType.gray_75,
            Data.MaterialType.dark_blue);

        public void explode()
        {
            parent.viewArmPiece(armPiece, false);
            Music.SoundManager.PlaySound(LoadedSound.deathpop, model.position);
            Effects.EffectLib.DamageBlocks(20, model, DamageColors);
            DeleteMe();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            armPiece.DeleteMe();
            armPiece = null;
        }
    }

    class ScorpionbotGoblin : VikingEngine.LootFest.GO.AbsChildModel
    {
        public ScorpionbotGoblin(Graphics.AbsVoxelObj parentModel)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.spiderbot_goblin,
                parentModel.Size1D * 0.18f, 0, false);
            posOffset = new Vector3(0, 4.7f, 0f);
        }
    }

    class ScorpionbotNosePoint : VikingEngine.LootFest.GO.AbsChildModel
    {
        public ScorpionbotNosePoint(Graphics.AbsVoxelObj parentModel)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.scorpionbot_arm_yellow,
                parentModel.Size1D * 0.1f, 0, true);
            posOffset = new Vector3(0, 2f, 5.5f);
        }
    }
}

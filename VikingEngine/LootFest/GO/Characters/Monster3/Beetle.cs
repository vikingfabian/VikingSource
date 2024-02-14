using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters.Monster3
{
    class Beetle : AbsMonster3
    {
        const float FlyHeight = 8;
        float goalY = Map.WorldPosition.ChunkStandardHeight + FlyHeight;
        float fallDownForAttackDist;
        bool animateFlyingState = true;

        public Beetle(GoArgs args)
            : base(args)
        {
            VoxelModelName model;
            if (args.characterLevel == 0)
            {
                model = VoxelModelName.beetle1;
                modelScale = Ref.rnd.Float(2f, 2.5f);
                fallDownForAttackDist = 10;
            }
            else
            {
                model = VoxelModelName.beetle2;
                modelScale = 8f;//Ref.rnd.Float(2f, 2.5f);
                fallDownForAttackDist = 15;
                Health = 2;
            }

            createImage(model, modelScale, -0.6f, new Graphics.AnimationsSettings(8, 0.4f, 3));

            //characterLevel = args.characterLevel;
            loadBounds();

            if (args.characterLevel == 1)
            {
                CollisionAndDefaultBound.Bounds[0].ignoresDamage = true;
            }

            alwaysFullAttension();
            if (args.LocalMember)
            {

                createAiPhys();
                if (args.characterLevel == 1)
                {
                    aiPhys.rotateSpeedMoving = AiPhysicsLf3.StandardRotateSpeedMoving * 0.5f;
                    aiPhys.rotateSpeedStanding = AiPhysicsLf3.StandardRotateSpeedMoving * 0.9f;
                }
                NetworkShareObject();
            }
        }

        public override void NetWriteUpdate(System.IO.BinaryWriter w)
        {
            base.NetWriteUpdate(w);
            w.Write(aiPhys.FlyingState);
        }
        public override void NetReadUpdate(System.IO.BinaryReader r)
        {
            base.NetReadUpdate(r);
            animateFlyingState = r.ReadBoolean();
        }


        protected override void updateAiMovement()
        {

            if (aiStateTimer.CountDown())
            {
                if (aiPhys.FlyingState)
                {
                    if (hasTarget() && Ref.rnd.Chance(0.6f))
                    {
                        float dist = distanceToObject(storedTarget);
                        if (dist < fallDownForAttackDist && !storedTarget.LookingTowardObject(this, MathHelper.PiOver4))
                        {
                            //Fall down for an attack
                            aiState = AiState.Falling;
                            aiStateTimer.MilliSeconds = 10000;
                        }
                        else
                        {
                            aiStateTimer.Seconds = Ref.rnd.Float(0.8f, 1.5f);
                            aiState = AiState.Follow;
                        }
                    }
                    else
                    {
                        aiStateTimer.Seconds = Ref.rnd.Float(0.4f, 0.8f);
                        rotation.Add(Ref.rnd.Plus_MinusF(MathHelper.PiOver2));
                        
                        aiState = AiState.Walking;
                    }
                }
                else
                {
                    if (Ref.rnd.Chance(90) && hasTarget() && storedTarget.LookingTowardObject(this, MathHelper.PiOver4))
                    {
                        //Flee flying
                        aiPhys.FlyingState = true;
                        aiState = AiState.Flee;
                        aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);
                    }
                    else if (aiState == AiState.Waiting)
                    {
                        aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);
                        rotation = Rotation1D.Random();
                        aiState = AiState.Walking;
                    }
                    else if (aiState == AiState.Walking && hasTarget())
                    {
                        aiStateTimer.Seconds = Ref.rnd.Float(4f, 5f);
                        aiState = AiState.Follow;
                    }
                    else
                    {
                        aiStateTimer.Seconds = Ref.rnd.Float(0.4f, 0.6f);

                        aiState = AiState.Waiting;
                    }
                }
            }

            switch (aiState)
            {
                case AiState.Follow:
                    if (aiPhys.FlyingState)
                    {
                        Vector3 target = storedTarget.Position;
                        target.Y += FlyHeight;
                        aiPhys.MovUpdate_MoveTowards(target, 3f, walkingSpeed);
                    }
                    else
                    {
                        aiPhys.MovUpdate_MoveTowards(storedTarget, 1f, walkingSpeed);
                    }
                    break;
                case AiState.Flee:
                    aiPhys.FlyingState = true;
                    aiPhys.MovUpdate_FleeFrom(storedTarget, walkingSpeed, goalY);
                    break;
                case AiState.Walking:
                    aiPhys.MovUpdate_MoveForward(rotation, casualWalkSpeed, goalY);
                    break;
                case AiState.Waiting:
                    aiPhys.MovUpdate_StandStill();
                    break;

                case AiState.IsStunned:
                case AiState.Falling:
                    aiPhys.FlyingState = false;

                    if (aiPhys.MovUpdate_FallToGround(physics.Gravity * 1f))
                    {
                        if (hasTarget())
                        {
                            aiState = AiState.Follow;
                        }
                        else
                        {
                            aiState = AiState.Walking;
                        }
                    }
                    break;
                default:
                    aiPhys.MovUpdate_StandStill();
                    break;

            }

        }

        public override bool CharacterLevelsHasDifferentBounds
        {
            get
            {
                return true;
            }
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            if (localMember)
            {
                if (aiPhys.FlyingState)
                {
                    goalY = WorldPos.GetHeightMapHeight() + FlyHeight;
                }
            }
        }

        public override void stunForce(float power, float takeDamage, bool headStomp, bool local)
        {
            base.stunForce(power, takeDamage, headStomp, local);

            if (localMember)
            { aiPhys.FlyingState = false; } 
        }
        protected override void onHitCharacter(AbsUpdateObj character)
        {
            base.onHitCharacter(character);
            aiPhys.FlyingState = true;
            aiState = AiState.Flee;
            aiStateTimer.Seconds = Ref.rnd.Float(1f, 2f);
        }

        public override void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            base.HandleObsticle(wallNotPit, ObjCollision);
        }
        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.FlyingAi;
            }
        }

        Timer.Basic flyAnimTimer = new Timer.Basic(99, true);
        protected override void updateAnimation()
        {
            if (localMember)
            {
                animateFlyingState = aiPhys.FlyingState;
            }

            if (animateFlyingState)
            {
                if (flyAnimTimer.Update())
                {
                    image.Frame = image.Frame == 1 ? 2 : 1;
                }
            }
            else
            {
                base.updateAnimation();
            }
        }

        const float BeetleFlyingSpeedLvl1 = 0.010f;
        const float BeetleWalkingSpeedLvl1 = 0.014f;
        const float BeetleFlyingSpeedLvl2 = 0.008f;
        const float BeetleWalkingSpeedLvl2 = 0.009f;

        const float CasualWalkSpeed = BeetleWalkingSpeedLvl1 * 0.5f;

        override protected float casualWalkSpeed
        {
            get { return aiPhys.FlyingState? BeetleFlyingSpeedLvl1 : CasualWalkSpeed ; }
        }
        protected override float walkingSpeed
        {
            get 
            {
                if (aiPhys.FlyingState)
                {
                    if (characterLevel == 0) return BeetleFlyingSpeedLvl1;
                    else return BeetleFlyingSpeedLvl2;
                }
                else
                {
                    if (characterLevel == 0) return BeetleWalkingSpeedLvl1;
                    else return BeetleWalkingSpeedLvl2;
                }
             }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
           Data.MaterialType.pure_blue,
             Data.MaterialType.dark_cyan_blue,
             Data.MaterialType.pastel_cyan_blue);
        public static readonly Effects.BouncingBlockColors DamageColorsLvl2 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_green,
            Data.MaterialType.darker_green,
            Data.MaterialType.pastel_pea_green);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return characterLevel == 0? DamageColorsLvl1 : DamageColorsLvl2;
            }
        }

        override public CardType CardCaptureType
        {
            get { return CardType.UnderConstruction; }
        }
        static readonly Vector3 ExpressionEffectScaleToOffset = new Vector3(0, 0.55f, 0.3f);
        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return ExpressionEffectScaleToOffset * modelScale;
            }
        }

        protected override Vector3 mountSaddlePos()
        {
            return calcSaddlePos(0.6f, 3f, 1.2f);
        }

        public override bool canBeStunned
        {
            get
            {
                return characterLevel == 0;
            }
        }
       

        public override GameObjectType Type
        {
            get { return GameObjectType.Beetle1; }
        }
    }
}

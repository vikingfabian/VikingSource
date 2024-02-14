using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.LootFest.GO.Characters
{
    class Slime : AbsMonster3
    {
        float shadowRadius;

        public Slime(GoArgs args)
            : base(args)
        {
            VoxelModelName modelName;
            bool bigModelBound;
            switch (characterLevel)
            {
                default:// case 0:
                    Health = LfLib.SuperWeakEnemyHealth;
                    modelScale = Ref.rnd.Float(0.6f, 0.7f);
                    modelName = VoxelModelName.green_slime_small;
                    attackRate.Seconds = 1f;
                    bigModelBound = false;
                    break;
                case 1:
                    modelScale = Ref.rnd.Float(2, 2.2f);
                    modelName = VoxelModelName.green_slime;
                    attackRate.Seconds = 1.6f;
                    bigModelBound = true;
                    break;
                case 2:
                    modelScale = Ref.rnd.Float(4, 4.2f);
                    modelName = VoxelModelName.green_slime;
                    attackRate.Seconds = 3f;
                    bigModelBound = true;
                    break;
            }

            createImage(modelName, modelScale, new Graphics.AnimationsSettings(2, 0.8f, 1));
            if (bigModelBound)
            {
                shadowRadius = modelScale * 0.75f;
                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(modelScale * 0.27f, modelScale * 0.15f, modelScale * 0.2f);
            }
            else
            {
                shadowRadius = modelScale * 1.6f;
                CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.5f) * modelScale, modelScale * 0.1f);
            }

            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.yAdj = 0;
                alwaysFullAttension();

                NetworkShareObject();
            }
        }

        
        public void StartJump(Rotation1D dir, float damage)
        {
            rotation = dir;
            slimeMovement(false);
            immortalityTime.MilliSeconds = 600;
            lastDamageLevel = damage;
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);
            if (characterLevel > 0)
            {
                Rotation1D dir = damage.PushDir;
                dir.Add(MathHelper.PiOver2);

                for (int i = 0; i < 2; ++i)
                {
                    var child = new Slime(new GoArgs(image.position, characterLevel - 1));
                    child.StartJump(dir, damage.Damage);
                    dir.Add(MathHelper.Pi);
                }
            }
        }

        protected override void updateAiMovement_Attacking()
        {
            slimeMovement(true);
        }
        protected override void updateAiMovement_Idle()
        {
            slimeMovement(true);
        }

        void slimeMovement(bool randomIdle)
        {
            if (nextCCAttackTimer.CountDown() &&
                !aiPhys.PhysicsStatusFalling)
            {
                //aiPhys.MovUpdate_MoveForward(rotation, rushSpeed);
                nextCCAttackTimer = attackRate;
                if (hasTarget() && distanceToObject(target) < 20)
                {
                    aiPhys.MovUpdate_MoveTowards(target, 0, walkingSpeed);
                }
                else
                {
                    if (randomIdle)
                    { rotation = Rotation1D.Random(); }
                    aiPhys.MovUpdate_MoveForward(rotation, walkingSpeed);
                }
                physics.Jump(0.6f, image);
                aiState = AiState.LockedInAttack;
                nextCCAttackTimer = attackRate;
            }
            else
            {
                aiPhys.MovUpdate_StandStill();
            }
        }

        protected override void updateAnimation()
        {
            if (Velocity.Value == Vector3.Zero)
            {
                image.Frame = 0;
            }
            else
            {
                image.Frame = 1;
            }
            //base.updateAnimation();
        }

        const float WalkingSpeed = 0.012f;
        override protected float walkingSpeed
        {
            get { return WalkingSpeed; }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.GreenSlime; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.Slime; }
        }
        public override bool canBeCardCaptured
        {
            get
            {
                return characterLevel == 0;
            }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.RGB_green);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        protected override int MaxLevel
        {
            get
            {
                return 2;
            }
        }

        public override float LightSourceRadius
        {
            get
            {
                return shadowRadius;
            }
        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, modelScale, 0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class Mummy3 : AbsMonster3
    {
        static readonly IntervalF ScaleRange = new IntervalF(5, 5.2f);
        const float MothFireRate = 100;
        int mothAttackCount = 0;

        public Mummy3(GoArgs args)
            : base(args)
        {
            projectileRate.Seconds = 4;
            targetSearchDistanceTaunted = 32;
            Health = LfLib.LargeEnemyHealth;
            modelScale = ScaleRange.GetRandom();
            createImage(VoxelModelName.mommy, modelScale, new Graphics.AnimationsSettings(7, 0.5f, 4));

            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.24f * modelScale, 0.43f * modelScale, modelScale * 0.29f));

            if (args.LocalMember)
            {
                TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(modelScale * 0.26f, modelScale * 0.4f);

                createAiPhys();
                aiPhys.yAdj = 0f;

                NetworkShareObject();
            }
            foundHeroEffect = true;
        }
       
        override protected void updateAiMovement_Attacking()
        {
            if (mothAttackCount > 0)
            {
                aiPhys.MovUpdate_StandStill();
                if (aiStateTimer.CountDown())
                {
                    Vector3 headPos = image.position;
                    headPos.Y += modelScale * 0.4f;
                    new Moth3(headPos, target);
                    mothAttackCount--;
                    aiStateTimer.MilliSeconds = MothFireRate;
                }
            }
            else
            {
                float distanceToTarget = distanceToObject(target);
                bool lookingAt = LookingTowardObject(target, 1f);

                if (nextRangedAttackTimer.CountDown() && distanceToTarget < 15 && lookingAt)
                {
                    //begin attack
                    mothAttackCount = Ref.rnd.Int(4, 5);
                    aiStateTimer.MilliSeconds = 600;
                    nextRangedAttackTimer = projectileRate;
                }

                if (distanceToTarget > 10f)
                {
                    aiPhys.MovUpdate_MoveTowards(target, 6f, walkingSpeed);
                }
                else if (lookingAt)
                {
                    aiPhys.MovUpdate_StandStill();
                }
                else
                {
                    aiPhys.MovUpdate_RotateTowards(target, 0.01f);
                }
            }
        }

        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            base.HandleColl3D(collData, ObjCollision);
        }

        protected override void updateAnimation()
        {
            if (mothAttackCount > 0)
            {
                image.Frame = 1;
            }
            else
            {
                base.updateAnimation();
            }
        }

        public override bool canBeStunned
        {
            get
            {
                return false;
            }
        }
        public override bool canBeCardCaptured
        {
            get
            {
                return base.canBeCardCaptured;
            }
        }

        protected override Vector3 expressionEffectPosOffset
        {
            get
            {
                return new Vector3(0, modelScale * 20, 0);
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Mummy; }
        }

        override public CardType CardCaptureType
        {
            get { return CardType.Mummy; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.gray_30,
            Data.MaterialType.light_yellow_orange,
            Data.MaterialType.gray_45);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }
        
    }
}

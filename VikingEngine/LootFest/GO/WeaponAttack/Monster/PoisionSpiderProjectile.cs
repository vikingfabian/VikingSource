using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack.Monster
{
    class PoisionSpiderProjectile : AbsGravityProjectile
    {
        const float ProjectileSpeed = 0.020f;
        const float Scale = 1.2f;
        const float scaleToBoundSize = 0.3f;

        int state_fly0_stuckGrowing1_idle2_fadeout3 = 0;
        Time idleTime = new Time(2f, TimeUnit.Seconds);


        public PoisionSpiderProjectile(GoArgs args, Vector3 direction)
            : base(args, new DamageData(1f), args.startPos, direction * ProjectileSpeed,
            new GO.Bounds.ObjectBound(Bounds.BoundShape.BoundingBox, args.startPos, new Vector3(Scale * scaleToBoundSize), Vector3.Zero))
        {
            rotationFollowSpeed = false;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            bool damageSearch = false;

            switch (state_fly0_stuckGrowing1_idle2_fadeout3)
            {
                case 0:
                    base.Time_Update(args);
                    break;
                case 1:
                    damageSearch = true;

                    image.Size1D += 1f * Ref.DeltaTimeSec;
                    CollisionAndDefaultBound.MainBound.halfSize = new Vector3(image.Size1D * scaleToBoundSize);
                    CollisionAndDefaultBound.MainBound.refreshBound();
                    CollisionAndDefaultBound.updateVisualBounds();

                    if (image.Size1D >= 4f)
                    {
                        state_fly0_stuckGrowing1_idle2_fadeout3++;
                    }
                    break;
                case 2:
                    damageSearch = true;

                    if (idleTime.CountDown())
                    {
                        state_fly0_stuckGrowing1_idle2_fadeout3++;
                    }
                    break;
                case 3:
                    image.Size1D -= 2f * Ref.DeltaTimeSec;

                    if (image.Size1D <= 0.1f)
                    {
                        DeleteMe();
                    }
                    break;

            }

            if (damageSearch)
            {
                characterCollCheck(args.localMembersCounter);
            }
            
        }

        override protected void hitShield(Vector3 shieldPos, Rotation1D heroDir)
        {
            for (int i = 0; i < 5; i++)
            {
                new Effects.BouncingBlock2(shieldPos, DamageColors.GetRandom(), 0.16f, heroDir);
            }
        }

        protected override bool canBeBlockedByShield
        {
            get
            {
                return state_fly0_stuckGrowing1_idle2_fadeout3 == 0;
            }
        }

        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            //Stick to the surface for a while
            state_fly0_stuckGrowing1_idle2_fadeout3 = 1;
            Vector3 moveback = Velocity.Value;
            moveback.Normalize();
            image.position -= moveback * 0.4f + Vector3.Down * 1f;
            Velocity.SetZero();
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (state_fly0_stuckGrowing1_idle2_fadeout3 > 0)
            {
                if (character.IsHero())
                {
                    ((VikingEngine.LootFest.GO.PlayerCharacter.AbsHero)character).slowedByEnemyTrap = 0.4f;
                }
            }

            if (base.handleCharacterColl(character, collisionData, usesMyDamageBound))
            {
                if (state_fly0_stuckGrowing1_idle2_fadeout3 == 0)
                {
                    DeleteMe();
                }
                
                return true;
            }

            return false;
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return EnemyProjGreenDamageCols;
            }
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponUserType.Enemy;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.PoisionSpiderProjectile; }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.enemy_projectile_green; }
        }
        protected override float ImageScale
        {
            get { return Scale; }
        }
        protected override bool LocalTargetsCheck
        {
            get
            {
                return true;
            }
        }

        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.ReceiveDamage;
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.Full;
            }
        }
    }
}

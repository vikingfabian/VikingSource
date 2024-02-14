using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack.Boss
{
    abstract class AbsHomingProjectile : AbsWeapon
    {
        Characters.AbsCharacter target;
        public float speed, homingSpeed;
        protected bool hitGroundState = false;

        public AbsHomingProjectile(GoArgs args, Characters.AbsCharacter target, Rotation1D startDir, float speed, float homingSpeed)
            : base(args)
        {
            this.target = target;
            this.speed = speed;
            this.homingSpeed = homingSpeed;

            rotation = startDir;

            Velocity.Set(startDir, speed);
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return GO.ObjPhysicsType.Projectile;
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            physics.Update(Ref.DeltaTimeMs);

            if (localMember || LocalTargetsCheck)
            {
                characterCollCheck(LocalTargetsCheck ? args.localMembersCounter : args.allMembersCounter);
            }

            rotateTowardsObject(target, 0.002f * homingSpeed);
            Velocity.Set(rotation, speed);

            if (Ref.TimePassed16ms)
            {
                const float YspeedMulti = 2f;

                float targetSpeedY;
                float ydiff = target.HeadPosition.Y - image.position.Y;

                if (hitGroundState)
                {
                    targetSpeedY = -speed * YspeedMulti;
                }
                else
                {
                    if (Math.Abs(ydiff) < 0.6f)
                    {
                        targetSpeedY = 0;
                    }
                    else if (ydiff < 0)
                    {
                        targetSpeedY = -speed * YspeedMulti;
                    }
                    else
                    {
                        targetSpeedY = speed * YspeedMulti;
                    }
                }

                Velocity.Y = (targetSpeedY - Velocity.Y) * 0.25f;
            }

            image.Rotation.PointAlongVector(Velocity.Value);
        }
    }

    class ScorpionBotRocket : AbsHomingProjectile
    {
        const float Scale = 4f;
        const float StartSpeed = 0.01f;
        const float GoalSpeed = StartSpeed * 3f;
        const float HomingSpeed = 1f;

        Time lifeTime = new Time(Ref.rnd.Float(1.6f, 2.4f), TimeUnit.Seconds);

        //public //static readonly Data.TempBlockReplacementSett TempImage = 
         //   new Data.TempBlockReplacementSett(Color.Red, new Vector3(0.1f, 0.1f, 1.5f));

        public ScorpionBotRocket(GoArgs args, Characters.AbsCharacter target, Rotation1D startDir)
            : base(args, target, startDir, StartSpeed, HomingSpeed)
        {

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.bot_rocket, Scale, 0, true);
            image.position = args.startPos;

            //givesDamage = ;

            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.Box1axisRotation, Vector3.Zero,
                new Vector3(0.12f * Scale, 0.12f * Scale, Scale * 0.4f), Vector3.Zero);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);

            hitGroundState = lifeTime.CountDown();

            if (speed < GoalSpeed)
            {
                speed += 0.04f * Ref.DeltaTimeSec;
            }
            
            if (Ref.gamesett.DetailLevel > 0 && Ref.TimePassed16ms)
            {
                Vector3 offset = image.Rotation.TranslateAlongAxis(-Vector3.UnitZ, Vector3.Zero);

                int numParticles = Ref.gamesett.DetailLevel == 2 ? 4 : 16;

                for (int i = 0; i < numParticles; i++)
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire,
                        new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(Scale * 0.5f * offset + image.position, Scale * 0.2f), offset));
                }

                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke,
                    new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(Scale * 0.7f * offset + image.position, Scale * 0.1f), offset * 0.2f));

            }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            explode();
            return false;
        }

        public override void HandleColl3D(Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            explode();
        }

        void explode()
        {
            new GO.WeaponAttack.Explosion(new GoArgs(image.position), LfRef.gamestate.GameObjCollection.localMembersUpdateCounter,
                new DamageData(1f, WeaponUserType.Enemy, NetworkId.Empty),
                3f, Data.MaterialType.gray_85, false, true, null);
            DeleteMe();
        }

        

        public override void BlockSplatter()
        {
            base.BlockSplatter();
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponUserType.Enemy;
            }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        protected override RecieveDamageType recieveDamageType
        {
            get
            {
                return RecieveDamageType.ReceiveDamage;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.BotRocket; }
        }
    }
}

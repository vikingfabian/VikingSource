using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Characters.CastleEnemy
{
    /// <summary>
    /// Walks trough walls, slides straight towards the player
    /// </summary>
    class Ghost : AbsMonster
    {
        public Ghost(GoArgs args)
            : base(args)
        {
            aiState = AiState.Waiting;
            rotation = Rotation1D.Random();
            //image.Position.Y = CastleEnemy.AbsCastleMonster.CastleFloorY;
            Health = LfLib.LargeEnemyHealth;// *(1 + LootfestLib.CastleMonsterHealthLvlMulti * characterLevel);

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }
        // public Ghost(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //}
         override protected void createBound(float imageScale)
         {
             CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.24f * imageScale, 0.36f * imageScale, imageScale * 0.31f));
             TerrainInteractBound = LootFest.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
         }
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            base.AsynchGOUpdate(args);
            //characterCollCheck(active);
        }


        static readonly IntervalF AttackUpdate = new IntervalF(400, 600);
        static readonly IntervalF FleeUpdate = new IntervalF(1200, 1800);
        const float WaitUpdate = 1600;

        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (aiState == AiState.Attacking || aiState == AiState.Flee)
                {
                    int dir = aiState == AiState.Attacking ? 1 : -1;
                    moveTowardsObject(target, 1, walkingSpeed * dir);
                    Velocity.PlaneUpdate(args.time, image);

                    float goalY = lightSourceY + 1f;
                    float ydiff = goalY - image.position.Y;
                    if (Math.Abs(ydiff) > 0.4f)
                    {
                        image.position.Y += lib.ToLeftRight(ydiff) * walkingSpeed * args.time;
                    }

                    //image.Position += new Vector2toV3(Speed * args.time);
                    setImageDirFromSpeed();
                }

                aiStateTimer.MilliSeconds -= args.time;
                if (aiStateTimer.MilliSeconds <= 0)
                {
                    //stateTime = 600;
                    target = getClosestCharacter(40, args.allMembersCounter, WeaponAttack.WeaponUserType.Enemy);

                    if (target == null)
                    {
                        aiState = AiState.Waiting;
                        aiStateTimer.MilliSeconds = WaitUpdate;
                    }
                    else
                    {
                        float fromTargetToMeDir = target.AngleDirToObject(this);
                        if (distanceToObject(target) < 10 && target.LookingTowardObject(this, 0.6f))//Math.Abs( target.Rotation.AngleDifference(fromTargetToMeDir)) < 0.6f)
                        {
                            //the player is looking at the ghost
                            aiState = AiState.Flee;
                            aiStateTimer.MilliSeconds = FleeUpdate.GetRandom();
                        }
                        else
                        {
                            aiState = AiState.Attacking;
                            aiStateTimer.MilliSeconds = AttackUpdate.GetRandom();
                        }
                    }
                }
                //immortalityTime.CountDown();
                characterCritiqalUpdate(true);
            }
            else base.Time_Update(args);
            
        }

        static readonly IntervalF TeleportRange = new IntervalF(16, 24);
        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        { //Shosts dont like magic damage
            if (damage.Magic != Magic.MagicElement.NoMagic)
            {
                damage.Damage *= 3;
            }
            base.handleDamage(damage, local);
            if (Alive && damage.Push != WeaponAttack.WeaponPush.NON && localMember)
            {
                Vector3 oldPos = image.position;
                //teleport away
                damage.PushDir.Radians += Ref.rnd.Plus_MinusF(1.2f);
                PlanePos += damage.PushDir.Direction(TeleportRange.GetRandom());

                //line of particles
                teleportSmokeTrace(oldPos);

                System.IO.BinaryWriter w = NetworkWriteObjectState(0);
                AbsUpdateObj.WritePosition(oldPos, w);
                AbsUpdateObj.WritePosition(image.position, w);
            }
            aiStateTimer.MilliSeconds = 0;
        }

        protected override void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        {
            Vector3 oldPos = AbsUpdateObj.ReadPosition(r);
            image.position = AbsUpdateObj.ReadPosition(r);

            teleportSmokeTrace(oldPos);
        }

        void teleportSmokeTrace(Vector3 oldPos)
        {
            Vector3 diff = image.position - oldPos;
            const int NumParticles = 50;
            diff /= NumParticles;
            List<Graphics.ParticleInitData> particles = new List<Graphics.ParticleInitData>(NumParticles);

            for (int i = 0; i < NumParticles; i++)
            {
                particles.Add(new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(oldPos, 1)));
                oldPos += diff;
            }
            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, particles);
        }



        protected override bool pushable
        {
            get
            {
                return false;
            }
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }

        protected override VoxelModelName imageName
        {
            get { return VoxelModelName.ghost; }
        }
        protected override Graphics.AnimationsSettings animSettings
        {
            get { return new Graphics.AnimationsSettings(5, 3); }
        }

        const float WalkingSpeed = 0.012f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeed; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.Ghost; }
        }
        public override CardType CardCaptureType
        {
            get
            {
                return CardType.Ghost;
            }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.Ghost; }
        }
        static readonly IntervalF ScaleRange = new IntervalF(3f, 3.2f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.gray_40,
            Data.MaterialType.gray_15,
            Data.MaterialType.gray_60);
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
                return 8;
            }
        }
    }
}

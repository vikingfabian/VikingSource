using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters.CastleEnemy
{
    /// <summary>
    /// Walks trough walls, slides straight towards the player
    /// </summary>
    class Ghost : AbsMonster2
    {
        public Ghost(Map.WorldPosition startPosition, int level)
            :base(startPosition, level)
        {
            aiState = AiState.Waiting;
            rotation = Rotation1D.Random();
            image.position.Y = CastleEnemy.AbsCastleMonster.CastleFloorY;
            Health = LootfestLib.GhostHealth * (1 + LootfestLib.CastleMonsterHealthLvlMulti * areaLevel);
            NetworkShareObject();
        }
         public Ghost(System.IO.BinaryReader r)
            : base(r)
        {
        }
         override protected void createBound(float imageScale)
         {
             CollisionBound = LF2.ObjSingleBound.QuickRectangleRotated2(new Vector3(0.24f * imageScale, 0.36f * imageScale, imageScale * 0.31f));
             TerrainInteractBound = LF2.ObjSingleBound.QuickCylinderBound(imageScale * StandardScaleToTerrainBound, imageScale * 0.5f);
         }
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            base.AIupdate(args);
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
                    //image.Position += Map.WorldPosition.V2toV3(Speed * args.time);
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
                        if (distanceToObject(target) < 10 && target.LookingAtObject(this, 0.6f))//Math.Abs( target.Rotation.AngleDifference(fromTargetToMeDir)) < 0.6f)
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
                particles.Add(new Graphics.ParticleInitData(lib.RandomV3(oldPos, 1)));
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
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.Ghost; }
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

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(Data.MaterialType.black  , Data.MaterialType.dark_gray, Data.MaterialType.flat_black);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }

        static readonly Data.Characters.MonsterLootSelection LootSelection = new Data.Characters.MonsterLootSelection(
             Gadgets.GoodsType.Plastma, 60, Gadgets.GoodsType.Black_tooth, 100, Gadgets.GoodsType.NONE, 0);
        protected override Data.Characters.MonsterLootSelection lootSelection
        {
            get { return LootSelection; }
        }

        protected override int MaxLevel
        {
            get
            {
                return LootfestLib.BossCount;
            }
        }
    }
}

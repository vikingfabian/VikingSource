using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Net;

namespace VikingEngine.LootFest.Creation.Zombies
{
    class FatZombie : AbsZombie
    {
        bool exploding = false;
        const float ShakeTime = 90;
        const float ExpandSpeed = 0.001f;
        float expandTime = ShakeTime;
        int expandDir = 1;
        int numShakes = 4;

        public FatZombie()
            : base(0)
        { }
        public FatZombie(System.IO.BinaryReader packetReader)
            : base(packetReader)
        {}
        public override void Time_Update(GameObjects.UpdateArgs args)
        {
            base.Time_Update(args);

            basicUpdate(args);

        }
        
        void basicUpdate(GameObjects.UpdateArgs args)
        {
            if (exploding)
            {
                image.Scale = lib.V3(image.Scale.X + ExpandSpeed * args.time * expandDir);
                expandTime -= args.time;
                if (expandTime <= 0)
                {
                    numShakes--;
                    if (numShakes <= 0)
                    {
                        Vector3 pos = image.Position;
                        pos.Y += 4;
                        splatter(pos);

                        
                        explode(args.localMembersCounter, pos);
                        DeleteMe();
                    }
                    else
                    {
                        expandDir = -expandDir;
                        expandTime = ShakeTime;
                    }
                }
            }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.fatzombie; }
        }
        static readonly IntervalF Scale = CharacterScale * 3f;
        protected override IntervalF ScaleRange
        {
            get { return Scale; }
        }

        static readonly IntervalF MySpeedRange = new IntervalF(0.004f, 0.005f);
        override protected IntervalF SpeedRange { get { return MySpeedRange; } }

        static readonly IntervalF MyRunningSpeedRange = new IntervalF(0.005f, 0.006f);
        override protected IntervalF RunningSpeedRange { get { return MyRunningSpeedRange; } } 
        
        static readonly IntervalF MyBasicStateTime = new IntervalF(1000, 3000);
        override protected IntervalF BasicStateTime { get { return MyBasicStateTime; } } 
        
        const int MyChanceToMove = 30;
        override protected int ChanceToMove { get { return MyChanceToMove; } }
        
        const float TargetRndAngleAdd = MathHelper.Pi * 0.6f;
        override protected float WalkingToTargetRndAngleAdd { get { return TargetRndAngleAdd; } }

        protected override float StartHealth { get { return 4; } }

      

        static readonly IntervalF FireDist = new IntervalF(0, 9);
        override protected IntervalF attackDistance { get { return FireDist; } }
        override protected AttackType attackType { get { return AttackType.Projectile; } }
        static readonly Range FireReloadTime = new Range(200, 200);
        override protected Range attackReloadTimeInStates { get { return FireReloadTime; } }


        static readonly IntervalF ParticleSpeed = new IntervalF(12f, 20f);
        public void Fire()
        {
            exploding = true;
        }
        protected override void fireProjectile()
        {
            PacketWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.FatzombieBeginAttack, Network.PacketReliability.Reliable, LootfestLib.LocalHostIx);
            ObjOwnerAndId.WriteStream(w);
           //prepare explosion
            exploding = true;
        }
        static readonly GameObjects.WeaponAttack.DamageData ExplosionDamage = new GameObjects.WeaponAttack.DamageData(3, GameObjects.WeaponAttack.WeaponUserType.NON, ByteVector2.Zero);
        static readonly IntervalF ExplosionSize = new IntervalF(8, 10);
        void explode(ISpottedArrayCounter<GameObjects.AbsUpdateObj> active, Vector3 pos)
        {
            //Explode!
            float size =  ExplosionSize.GetRandom();
            if (localMember)
                new GameObjects.WeaponAttack.Explosion(active, pos, ExplosionDamage, size, Data.MaterialType.violet, true, true, this);
        }
        void splatter(Vector3 pos)
        {
#if CMODE
            Music.SoundManager.PlaySound(LoadedSound.fatty_explode, image.Position, 0);

            ParticleInitData data = new ParticleInitData(pos, Vector3.Zero);
            const int NumParticles = 64;
            List<ParticleInitData> particles = new List<ParticleInitData>();
            particles.Capacity = NumParticles;
            Rotation1D rot;

            for (int i = 0; i < NumParticles; i++)
            {
                rot = Rotation1D.Random;
                data.StartSpeed = Map.WorldPosition.V2toV3(rot.Direction(ParticleSpeed.GetRandom()), ParticleSpeed.GetRandom());
                particles.Add(data);
            }
            Engine.ParticleHandler.AddParticles(ParticleSystemType.ZombieSplatter, particles);

            health = 0;
            BlockSplatter();
#endif
        }

        protected override LoadedSound MoanSound
        {
            get
            {
                return LoadedSound.zombie_fatty_moan1;
            }
        }
        

        static readonly Graphics.AnimationsSettings AnimationsSettings = new Graphics.AnimationsSettings(5, 0.8f);
        override protected Graphics.AnimationsSettings animationsSettings { get { return AnimationsSettings; } }

        public override GameObjects.NetworkShare NetworkShareSettings
        {
            get
            {
                GameObjects.NetworkShare share = base.NetworkShareSettings;
                share.DeleteByHost = false;
                return share;
            }
        }

        public override GameObjects.Characters.CharacterUtype CharacterType
        {
            get { return GameObjects.Characters.CharacterUtype.FatZombie; }
        }
    }
}

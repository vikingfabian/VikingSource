#define USE_PARTICLES

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Engine
{
    static class ParticleHandler
    {
        static ParticleSystemData[] particleSystems;

        static List<ParticleSystemData> active = new List<ParticleSystemData>();

        public static void Init()
        {
#if USE_PARTICLES
            particleSystems = new ParticleSystemData[(int)Graphics.ParticleSystemType.NUM];

            if (PlatformSettings.RunProgram == StartProgram.LootFest3)
            {
                Graphics.ParticleSystemType[] use = new Graphics.ParticleSystemType[]
                {
                    Graphics.ParticleSystemType.BulletTrace,
                    Graphics.ParticleSystemType.Dust,
                    Graphics.ParticleSystemType.ExplosionFire,
                    Graphics.ParticleSystemType.Fire,
                    Graphics.ParticleSystemType.GoldenSparkle,
                    Graphics.ParticleSystemType.Poision,
                    Graphics.ParticleSystemType.RunningSmoke,
                    Graphics.ParticleSystemType.Smoke,
                    Graphics.ParticleSystemType.WeaponSparks,

                };
                foreach (Graphics.ParticleSystemType type in use)
                {
                    particleSystems[(int)type] = new ParticleSystemData(type);
                }
            }
            else if (PlatformSettings.RunProgram == StartProgram.ToGG)
            {
                Graphics.ParticleSystemType[] use = new Graphics.ParticleSystemType[]
               {
                    Graphics.ParticleSystemType.TorchFire,
                    Graphics.ParticleSystemType.CommanderDamage,
               };
                foreach (Graphics.ParticleSystemType type in use)
                {
                    particleSystems[(int)type] = new ParticleSystemData(type);
                }
            }
            else if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                Graphics.ParticleSystemType[] use = new Graphics.ParticleSystemType[]
                {
                    Graphics.ParticleSystemType.BulletTrace,
                    Graphics.ParticleSystemType.GoldenSparkle,
                    Graphics.ParticleSystemType.Fire,
                    Graphics.ParticleSystemType.Smoke,
                    Graphics.ParticleSystemType.DssDamage,
                    Graphics.ParticleSystemType.CommanderDamage,
                };
                foreach (Graphics.ParticleSystemType type in use)
                {
                    particleSystems[(int)type] = new ParticleSystemData(type);
                }
            }
            else
            {
                for (Graphics.ParticleSystemType type = (Graphics.ParticleSystemType)0;
                    type < Graphics.ParticleSystemType.NUM; type++)
                {
                    particleSystems[(int)type] = new ParticleSystemData(type);
                }
            }
#endif
        }

        public static void Draw()
        {
            //lock (active)
            //{
                foreach (ParticleSystemData system in active)
                {
                    system.Draw();
                }
            //}

        }

        public static void AddParticles(Graphics.ParticleSystemType type, List<Graphics.ParticleInitData> startValues)
        {
#if USE_PARTICLES
            addSystem(type);
            particleSystems[(int)type].AddParticles(startValues);
#endif
        }

        public static void AddParticles(Graphics.ParticleSystemType type, Vector3 startPos)
        {
            #if USE_PARTICLES
            AddParticles(type, new Graphics.ParticleInitData(startPos));
            #endif
        }

        public static void AddParticles(Graphics.ParticleSystemType type, Graphics.ParticleInitData startValue)
        {
            #if USE_PARTICLES
            addSystem(type);
            particleSystems[(int)type].AddParticles(startValue);
            #endif
        }

        public static void AddParticleArea(Graphics.ParticleSystemType type, Vector3 center, float radius, int numParticles)
        {
            #if USE_PARTICLES
            addSystem(type);
            ParticleSystemData sys = particleSystems[(int)type];
            for (int i = 0; i < numParticles; i++)
            {
                sys.AddParticles(new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(center, radius)));
            }
            #endif
        }

        public static void AddParticleSphere(Graphics.ParticleSystemType type, Vector3 center, float radius, int numParticles)
        {
#if USE_PARTICLES
            addSystem(type);
            ParticleSystemData sys = particleSystems[(int)type];
            for (int i = 0; i < numParticles; i++)
            {
                sys.AddParticles(new Graphics.ParticleInitData(center + Ref.rnd.Vector3_Sph() * radius));
            }
#endif
        }

        public static void AddExpandingParticleArea(Graphics.ParticleSystemType type, Vector3 center, float radius, int numParticles, float expandSpeed)
        {
            #if USE_PARTICLES
            addSystem(type);
            ParticleSystemData sys = particleSystems[(int)type];
            Graphics.ParticleInitData data = new Graphics.ParticleInitData();
            
            for (int i = 0; i < numParticles; i++)
            {
                data.Position = Ref.rnd.Vector3_Sq(center, radius);
                Vector3 diff = data.Position - center;
                data.StartSpeed = VectorExt.SafeNormalizeV3(diff) * expandSpeed;
                sys.AddParticles(data);
            }
#endif
        }

        static void addSystem(Graphics.ParticleSystemType type)
        {
#if USE_PARTICLES
            Debug.CrashIfThreaded();   
            if (!particleSystems[(int)type].InUpdate)
            {
                //lock (active)
                //{
                    active.Add(particleSystems[(int)type]);
                    particleSystems[(int)type].InUpdate = true;
                //}
            }
#endif
        }

        public static void Update(float time)
        {
#if USE_PARTICLES
            //lock (active)
            //{
                for (int i = 0; i < active.Count; i++)
                {
                    if (active[i].Update(time))
                    {
                        active.Remove(active[i]);
                    }
                }
            //}
#endif
        }
    }
    struct ParticleSystemData
    {
        public bool InUpdate;
        Graphics.ParticleSystem System;

        float Time;
        public ParticleSystemData(Graphics.ParticleSystemType type)
        {
            switch (type)
            {
                case Graphics.ParticleSystemType.DssDamage:
                    System = new Graphics.DssDamage();
                    break;
                case Graphics.ParticleSystemType.Smoke:
                    System = new Graphics.Smoke();
                    break;
                case Graphics.ParticleSystemType.Fire:
                    System = new Graphics.Fire();
                    break;

                case Graphics.ParticleSystemType.Poision:
                    System = new Graphics.Poision();
                    break;
                case Graphics.ParticleSystemType.LightSparks:
                    System = new Graphics.LightSparks();
                    break;
               case Graphics.ParticleSystemType.ExplosionFire:
                    System = new Graphics.ExplosionFire();
                    break;
               

               case Graphics.ParticleSystemType.GoldenSparkle:
                    System = new Graphics.GoldenSpark();
                    break;
               case Graphics.ParticleSystemType.BulletTrace:
                    System = new Graphics.BulletTrace();
                    break;
                case Graphics.ParticleSystemType.RunningSmoke:
                    System = new Graphics.RunningSmoke();
                    break;
                
                
                case Graphics.ParticleSystemType.Sparkle:
                    System = new Graphics.Sparkle();
                    break;
                case Graphics.ParticleSystemType.Dust:
                    System = new Graphics.Dust();
                    break;
                case Graphics.ParticleSystemType.WeaponSparks:
                    System = new Graphics.WeaponSparks();
                    break;
                case Graphics.ParticleSystemType.CommanderDamage:
                    System = new Graphics.CommanderUnitDamage();
                    break;
                case Graphics.ParticleSystemType.TorchFire:
                    System = new Graphics.TorchFire();
                    break;
                default:
                    System = null;
                    break;
            }
            Time = 1000;
            InUpdate = false;
        }
        const float AddParticlesTime = 5000;
        public void AddParticles(List<Graphics.ParticleInitData> startValues)
        {
            InUpdate = true;
            for (int i = 0; i < startValues.Count; i++)
            {
                System.AddParticle(startValues[i].Position, startValues[i].StartSpeed);
            }
            Time = AddParticlesTime;
        }
        public void AddParticles(Graphics.ParticleInitData startValue)
        {
            InUpdate = true;
            System.AddParticle(startValue.Position, startValue.StartSpeed);
            
            Time = AddParticlesTime;
        }
        /// <returns>If it is time to remove the system from update</returns>
        public bool Update(float time)
        {
            System.Time_Update(time);
            Time -= time;
            if (Time <= 0)
            {
                InUpdate = false;
                return true;
            }
            return false;
        }
        public void Draw()
        {
            System.SetCamera();
            System.Draw();
        }
    }



}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Work
{
    abstract class AbsWorkEffect
    {
        virtual public void update() { }
        virtual public void onSoundAnimation() { }
    }

    class CookingWorkEffect:AbsWorkEffect
    {
        Vector3 emitterPos;

        public CookingWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X -= 0.01f;
            emitterPos.Y += 0.005f;
            emitterPos.Z += 0.02f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)//
            {
                if (Ref.peRnd.Chance(0.5))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, emitterPos);
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                }
            }
        }
    }

    class BreweryWorkEffect : AbsWorkEffect
    {
        Vector3 emitterPos;

        public BreweryWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X += 0.025f;
            emitterPos.Y += 0.005f;
            emitterPos.Z += 0.02f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)//
            {
                if (Ref.peRnd.Chance(0.2))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, emitterPos);
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                }
            }
        }
    }

    class CoalPitWorkEffect : AbsWorkEffect
    {
        const float AreaSz = 0.03f;
        Vector3 emitterPos;

        public CoalPitWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X -= 0.01f;
            emitterPos.Y += 0.005f;
            emitterPos.Z += 0.00f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)//if (Ref.TimePassed16ms)
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, AreaSz));
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, AreaSz));
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, AreaSz));
            }
        }
    }

    class FoundryWorkEffect : AbsWorkEffect
    {
        Vector3 emitterPos;

        public FoundryWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X -= 0.01f;
            emitterPos.Y += 0.05f;
            emitterPos.Z -= 0.02f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)//
            {
                if (/*Ref.TimePassed16ms &&*/ Ref.peRnd.Chance(0.5))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, emitterPos);
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                }
            }
        }
    }

    class SmelterWorkEffect : AbsWorkEffect
    {
        Vector3 emitterPos;

        public SmelterWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X -= 0.01f;
            emitterPos.Y += 0.05f;
            emitterPos.Z += 0.00f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)//
            {
                if (Ref.peRnd.Chance(0.5))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, emitterPos);
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                }
            }
        }
    }

    class PotteryWorkEffect : AbsWorkEffect
    {
        Vector3 emitterPos;

        public PotteryWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X += 0.05f;
            emitterPos.Y += 0.05f;
            emitterPos.Z += 0.00f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            for (int i = 0; i < Ref.GameTimePassed16ms; ++i)//
            {
                if (Ref.peRnd.Chance(0.5))
                {
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, emitterPos);
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                    Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
                }
            }
        }
    }

    class SmithWorkEffect : AbsWorkEffect
    {
        Vector3 emitterPos;

        public SmithWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X -= 0.02f;
            emitterPos.Y += 0.005f;
            emitterPos.Z -= 0.01f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            if (/*Ref.TimePassed16ms && */Ref.peRnd.Chance(0.5 / Ref.UpdateTimes60FPS))
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, emitterPos);
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke, Ref.peRnd.Vector3_Sq(emitterPos, 0.01f));
            }
        }
    }

    class ButcherWorkEffect : AbsWorkEffect
    {
        Vector3 emitterPos;

        public ButcherWorkEffect(IntVector2 subTilePos)
        {
            emitterPos = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
            emitterPos.X -= 0.02f;
            emitterPos.Y += 0.005f;
            emitterPos.Z -= 0.01f;
        }

        public override void update()
        {
            //Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Fire, smokeEmitter);
            if (/*Ref.TimePassed16ms && */Ref.peRnd.Chance(0.1 / Ref.UpdateTimes60FPS))
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.DssDamage, emitterPos);
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.DssDamage, emitterPos);
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.DssDamage, emitterPos);

            }
        }
    }
}

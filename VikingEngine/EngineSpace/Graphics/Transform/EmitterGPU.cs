using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class EmitterGPU : AbsUpdateable
    {
        public IntervalVector3 Area;
        IntervalVector3 Velocity;
        ParticleSystemType type;
        public IntervalF addPerMs;
        float addParticles = 0;
        static List<ParticleInitData> startValues = new List<ParticleInitData>();

        public EmitterGPU(IntervalVector3 _area, IntervalVector3 _velocity,
            ParticleSystemType _type, IntervalF _addPerMs)
            :base(true)
        {
            Area = _area;
            Velocity = _velocity;
            type = _type;
            addPerMs = _addPerMs;
        }
        public EmitterGPU CloneMe(Vector3 posDiff)
        {
            EmitterGPU clone = new EmitterGPU(Area, Velocity, type, addPerMs);
            clone.Area.Min += posDiff;
            return clone;
        }
        public override void Time_Update(float time)
        {
            addParticles += addPerMs.GetRandom() * time;
            if (addParticles >= 1)
            {
                int add = (int)addParticles;
                addParticles -= add;
                GenerateParticles(add, Area, Velocity, type);
                //startValues.Clear();
                //for (int i = 0; i < add; i++)
                //{
                //    startValues.Add(new ParticleInitData(Area.GetRandom(), Velocity.GetRandom()));
                //}
                //Engine.ParticleHandler.AddParticles(type, startValues);
            }
        }
        public static void GenerateParticles(int numParticles, IntervalVector3 area, IntervalVector3 velocity, ParticleSystemType type)
        {
            startValues.Clear();
            for (int i = 0; i < numParticles; i++)
            {
                startValues.Add(new ParticleInitData(area.GetRandom(), velocity.GetRandom()));
            }
            Engine.ParticleHandler.AddParticles(type, startValues);
        }
    }
}

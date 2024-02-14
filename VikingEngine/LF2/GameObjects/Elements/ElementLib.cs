using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Elements
{
    static class ElementLib
    {

        static readonly Vector3 SmokeUpSpeed = new Vector3(0, 2f, 0);
        public static void SmokePuff(Vector3 center, float radius)
        {
            for (int i = 0; i < 8; i++)
            {
                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke,
                       new Graphics.ParticleInitData(lib.RandomV3(center, radius),  SmokeUpSpeed));
            }
        }
    }

    enum ElementType
    {
        Fire,
        Thunder,
        NUM
    }
}

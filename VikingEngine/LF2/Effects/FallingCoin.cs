using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Effects
{
    class FallingCoin : AbsUpdateable
    {
        static readonly IntervalF FallSpeed = new IntervalF(0.01f, 0.016f);
        float ySpeed;
        static readonly IntervalF Rotation = new IntervalF(-0.02f, -0.01f);
        float rotationSpeed = Rotation.GetRandom();
        static readonly RangeV3 startPos = RangeV3.FromRadius(new Vector3(0, 10, 3), 3);
        Graphics.VoxelModelInstance img;
        static readonly Vector3 Scale = lib.V3(0.08f);

        public FallingCoin(Graphics.VoxelObj originalMesh)
            :base(true)
        {
            img = new Graphics.VoxelModelInstance(originalMesh);
            img.scale = Scale;
            restart();
        }
        void restart()
        {
            ySpeed = -FallSpeed.GetRandom();
            img.position = startPos.GetRandom();
            img.Rotation.RotateWorld(new Vector3(lib.RandomRotation(), 0, 0));
        }

        public override void Time_Update(float time)
        {

            img.position.Y += ySpeed;
            if (img.position.Y <= -2)
            {
                restart();
            }
            img.Rotation.RotateWorld(new Vector3(rotationSpeed, 0, 0));
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class UnitDeathAnimation : AbsUpdateable
    {
        static readonly IntervalF RotationSpeedRange = new IntervalF(0.015f, 0.025f);
        Graphics.Mesh model;
        Rotation1D rotation = Rotation1D.D0;
        float rotationSpeed;
        Vector3 speed;

        public UnitDeathAnimation(AbsUnit unit)
            :base(true)
        {
            model = new Graphics.Mesh(LoadedMesh.plane, unit.soldierModel.Position, new Vector3(0.74f),
                Graphics.TextureEffectType.Flat, unit.Data.modelSettings.image, Color.White);
                //new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, unit.data.image), 0.74f);
            model.Y = 0.4f * model.ScaleY;
            
            rotationSpeed = RotationSpeedRange.GetRandom() * Ref.main.TargetElapsedTime.Milliseconds;

            speed.X = Ref.rnd.Plus_MinusF(0.002f);
            speed.Y = Ref.rnd.Float(0.004f, 0.008f);
        }

        public override void Time_Update(float time)
        {
            rotation.Add(rotationSpeed);
            model.Rotation = toggLib.PlaneTowardsCamWithRotation(rotation.radians);

            speed.Y += -0.000016f * time;

            model.Position += speed * time;


            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.CommanderDamage, new Graphics.ParticleInitData(model.Position, -speed));

            if (model.Y < -1)
            {
                this.DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}

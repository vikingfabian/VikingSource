using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using VikingEngine.Wars.GameObject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.DSSWars.GameObject
{
    static class GoreManager
    {
        //static readonly Graphics.TextureEffect HumanBloodCol = new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.DarkRed);
        //static readonly Graphics.TextureEffect OrcBloodCol = new Graphics.TextureEffect(Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, new Color(0, 50, 0));
        const float BloodRadius = AbsSoldierData.StandardModelScale * 0.1f;

        public static void ViewDamage(AbsDetailUnit reciever, int damageAmount, Rotation1D attackDir)
        {
            Vector3 startPos = reciever.position;
            startPos.Y += AbsSoldierData.StandardModelScale * 0.01f;
            int particleCount = reciever.Alive() ? damageAmount / 4 : damageAmount;
            // Graphics.TextureEffect col = reciever.faction == Faction.Human ? HumanBloodCol : OrcBloodCol;
            Vector3 pos = reciever.position;
            pos.Y += AbsSoldierData.StandardModelScale;
            Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.DssDamage, pos, BloodRadius, particleCount);
            //if (reciever.Alive())
            //{
            //    Rotation1D attackAngle = attackDir;

            //    //for (int i = 0; i < Bound.Min(damageAmount / 2, 1); ++i)
            //    //{
            //    //    //Rotation1D dir = attackAngle;
            //    //    //dir.Add(Ref.rnd.Plus_MinusF(1f));
            //    //    //new BloodBlock(startPos, reciever.position.Y, dir, false);

            //    //}
            //}
            //else
            //{
            //    //for (int i = 0; i < damageAmount; ++i)
            //    //{
            //    //    //new BloodBlock(startPos, reciever.position.Y, Rotation1D.Random(), true);
            //    //}
            //}
        }
    }

    class BloodBlock : AbsInGameUpdateable
    {
        const float FadeDownSpeed = AbsSoldierData.StandardModelScale * 0.01f;
        const float Gravity = AbsSoldierData.StandardModelScale * -0.006f;
        const float BlockScale = AbsSoldierData.StandardModelScale * 0.1f;
        const float MinSpeed = AbsSoldierData.StandardModelScale * 0.004f;
        //const float GroundY = BlockScale * 0.5f + 0.1f;
        const float RemovalY = -BlockScale;

        Graphics.Mesh model;
        Velocity velocity;
        int bounces = 0;
        float lifeTime = 0;
        float groundY;

        public BloodBlock(Vector3 pos, float groundY, Rotation1D dir, bool onDeath)
            :base(true)
        {
            this.groundY = groundY;
            model = new Graphics.Mesh(LoadedMesh.cube_repeating, pos, new Vector3(BlockScale), 
                Graphics.TextureEffectType.Flat, SpriteName.WhiteArea_LFtiles, Color.Red);
            float speed = Ref.rnd.Float(0.001f, 0.003f) * AbsSoldierData.StandardModelScale;
            if (onDeath)
            {
                speed *= 1.6f;
            }
            velocity.Set(dir, speed);
            velocity.Y = Ref.rnd.Plus_MinusF(0.01f) * AbsSoldierData.StandardModelScale;
        }

        public override void Time_Update(float time_ms)
        {
            lifeTime += time_ms;
            if (lifeTime > 2000)
            {
                DeleteMe();
            }
            else if (velocity.Value == Vector3.Zero)
            { //state 3: fade out
                model.Y -= FadeDownSpeed * time_ms;

                if (model.Y < RemovalY)
                {
                    DeleteMe();
                }
            }
            else
            {
                if (bounces <= 1)
                {
                    if (Ref.TimePassed16ms)
                    {
                        velocity.Y += Gravity;
                    }
                }
                else
                {
                    //State 2: sliding
                    velocity.Y = 0;

                    if (Ref.TimePassed16ms)
                    {
                        velocity.Value *= 0.9f;
                        if (velocity.PlaneLength() < MinSpeed)
                        {
                            velocity.Value = Vector3.Zero;
                        }
                    }
                }
                model.Position += velocity.Value * time_ms;

                if (model.Y < groundY)
                {
                    model.Y = groundY;

                    if (velocity.Y < 0)
                    {
                        bounces++;
                        if (bounces <= 1)
                        {
                            velocity.Y = -velocity.Y;
                        }
                        velocity.Value *= 0.6f;
                    }
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            model.DeleteMe();
        }
    }
}

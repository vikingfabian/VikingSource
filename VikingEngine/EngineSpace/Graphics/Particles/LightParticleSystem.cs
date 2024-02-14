#region File Description
//-----------------------------------------------------------------------------
// ParticleSystem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Collections.Generic;
#endregion

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Will render a sphere of light or darkness around it
    /// </summary>
    interface ILightSource
    {
        Vector3 LightSourcePosition { get; }
        float LightSourceRadius { get; }
        LightParticleType LightSourceType { get; }
        int LightSourceArrayIndex { get; set; }
        LightSourcePrio LightSourcePrio { get; }
        float LightSourceDistanceToGamer { get; set; }
    }
     


    class LightSystem : AbsLightParticleSystem
    {
        

        public LightSystem()
            : base()
        { }

        protected override void InitializeSettings(ParticleSettings settings)
        {
            //settings.TextureName = "fire";
            //settings.Texture = LoadedTexture.pfire;
            //settings.Texture = LoadedTexture.lightDepthMap;
            settings.MaxParticles = 64;

            settings.Duration = TimeSpan.FromSeconds(500f);

            settings.DurationRandomness = 1;

            const float Speed = 0.0f;
            settings.MinHorizontalVelocity = -Speed;
            settings.MaxHorizontalVelocity = Speed;

            settings.MinVerticalVelocity = 0;
            settings.MaxVerticalVelocity = Speed;

            // Set gravity upside down, so the flames will 'fall' upward.
            settings.Gravity = new Vector3(0, 0f, 0);

            settings.EndVelocity = 0f; //n

            const float Size = 20;
            settings.MinStartSize = Size;
            settings.MaxStartSize = Size;

            settings.MinEndSize = Size;
            settings.MaxEndSize = Size;

            const float Rotation = 0;
            settings.MinRotateSpeed = -Rotation;
            settings.MaxRotateSpeed = Rotation;
            // Use additive blending.
            settings.BlendState = BlendState.NonPremultiplied;
            //settings.SourceBlend = Blend.SourceAlpha;
            //settings.DestinationBlend = Blend.One;

            settings.MinColor = Color.White;
            settings.MaxColor = Color.White;
        }
    }

    
    abstract class AbsLightParticleSystem : AbsParticleSystem
    {
        List<ILightSource> lightsAndShadows = new List<ILightSource>();

        public AbsLightParticleSystem()
            : base()
        {

        }

        public void AddParticle(ILightSource p)
        {
            lightsAndShadows.Add(p);
            particleDataToBuffer(lightsAndShadows.Count - 1);
        }
        public void RemoveParticle(ILightSource p)
        {
            lightsAndShadows.Remove(p);
        }
        public void Clear()
        {
            lightsAndShadows.Clear();
        }

        void particleDataToBuffer(int index)
        {
            ILightSource p = lightsAndShadows[index];
            for (int indiceIx = 0; indiceIx < GraphicsLib.PolygonIndicesCount; indiceIx++)
            {
                int indiceBufferIx = index * (GraphicsLib.PolygonIndicesCount - 1) + indiceIx;
                particles_CPU[indiceIx].Position = p.LightSourcePosition;
                particles_CPU[indiceBufferIx].Random_Vcolor = new Color((byte)p.LightSourceRadius, (byte)p.LightSourceType, 0);
            }
        }


        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            for (int i = 0; i < lightsAndShadows.Count; ++i)
            {
                for (int indiceIx = 0; indiceIx < GraphicsLib.PolygonIndicesCount; indiceIx++)
                {
                    int indiceBufferIx = i * GraphicsLib.PolygonIndicesCount + indiceIx;
                    particles_CPU[indiceBufferIx].Position = lightsAndShadows[i].LightSourcePosition;
                }
            }
            updateVertexBuffer(0, lightsAndShadows.Count);
        }
        public override void Draw()
        {
            // Restore the vertex buffer contents if the graphics device was lost.
            if (vertexBuffer_GPU.IsContentLost)
            {
                vertexBuffer_GPU.SetData(particles_CPU);
            }
            drawParticleRange(0, lightsAndShadows.Count);
        }
        protected override LoadedEffect loadedEffect
        {
            get { return LoadedEffect.LightParticleEffect; }
        }

        public void PostRender(RenderTarget2D scene, RenderTarget2D depthMap)
        {
            particleEffect.Parameters["wvp"].SetValue(Ref.draw.wvpMatrix);
            //particleEffect.Parameters["SceneMap"].SetValue(scene);
            particleEffect.Parameters["DepthMap"].SetValue(depthMap);
            SetCamera();
            Draw();
        }
    }


    enum LightParticleType
    {
        Shadow,
        Fire,
        Lightning,
        MagicLight,
        NUM_NON,
    }
    enum LightSourcePrio
    {
        /// <summary>
        /// For the hero alone basically
        /// </summary>
        High,
        
        /// <summary>
        /// Characters in general
        /// </summary>
        Medium,
        
        /// <summary>
        /// Terrain light effects
        /// </summary>
        Low,

        /// <summary>
        /// For short lived effects
        /// </summary>
        VeryLow,
        NUM
    }

}
        
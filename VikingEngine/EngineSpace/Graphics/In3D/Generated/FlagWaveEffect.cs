using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class FlagWaveEffect : CustomEffect
    {
        public static FlagWaveEffect Singleton;
        // Some default wave settings
        public float Time { get; set; } = 0.0f;
        public float WaveSpeed { get; set; } = 4f;
        public float WaveFrequency { get; set; } = 250.0f;
        public float WaveAmplitude { get; set; } = 0.0015f;

        // Secondary “flutter” wave for amplitude modulation
        public float AmplitudeModFrequency { get; set; } = 5f;
        //public float AmplitudeModRange { get; set; } = 0.55f;

        /// <summary>
        /// Constructor that calls base with:
        ///     TechniqueName   = "FlagWaveTechnique"
        ///     usesWorldPos    = true (if your shader expects to set World, etc.)
        /// </summary>
        public FlagWaveEffect()
            : base("FlagWaveTechnique", usesWorldPos: true)
        {
            shader = Engine.Draw.effectFlag;
            shader.CurrentTechnique = shader.Techniques[TechniqueName];

            shader.Parameters["WaveSpeed"]?.SetValue(WaveSpeed);
            shader.Parameters["WaveFrequency"]?.SetValue(WaveFrequency);
            shader.Parameters["WaveAmplitude"]?.SetValue(WaveAmplitude);
            shader.Parameters["AmplitudeModFrequency"]?.SetValue(AmplitudeModFrequency);
            //shader.Parameters["AmplitudeModRange"]?.SetValue(AmplitudeModRange);
        }

        public override void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
        {
            //AmplitudeModFrequency = 5f;
            //WaveFrequency = 250;
            //AmplitudeModRange = 0.8f;
            //WaveAmplitude = 0.0015f;
            //WaveSpeed = 4f;
            ////AmplitudeModRange = 0f;
            //shader.Parameters["WaveSpeed"]?.SetValue(WaveSpeed);
            //shader.Parameters["WaveFrequency"]?.SetValue(WaveFrequency);
            //shader.Parameters["WaveAmplitude"]?.SetValue(WaveAmplitude);
            //shader.Parameters["AmplitudeModFrequency"]?.SetValue(AmplitudeModFrequency);
            //shader.Parameters["AmplitudeModRange"]?.SetValue(AmplitudeModRange);




            shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);
            base.DrawVB(frame, obj, VB);
        }

        /// <summary>
        /// Override the Draw method to set the wave parameters each frame.
        /// </summary>
        public override void Draw(Mesh obj)
        {
            // Choose the technique that does the flag waving
            shader.CurrentTechnique = shader.Techniques[TechniqueName];

            // Set the wave parameters in the shader
            shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);



            // If your .fx also has lighting parameters like LightDirection, LightColor, etc.,
            // you could set them here. For example:
            // shader.Parameters["LightDirection"] ?.SetValue(new Vector3(0, -1, 1));
            // shader.Parameters["AmbientIntensity"] ?.SetValue(0.2f);
            // ... etc.

            // Then let the base class handle the rest: 
            //   - setting texture (if you have one),
            //   - setting color,
            //   - binding the effect to the mesh,
            //   - drawing the mesh, etc.
            shader.CurrentTechnique = shader.Techniques[TechniqueName];
            obj.TextureSource.SetCustomShaderParameters(ref shader);
            
            var model = Engine.LoadContent.Models[(int)obj.LoadedMeshType]; //Engine.LoadContent.Mesh(obj.LoadedMeshType);

            //for (modelMeshIx = 0; modelMeshIx < model.Meshes.Count; modelMeshIx++)
            //{
            modelListMesh = model.Meshes[0];
            obj.CalcWorldMatrix(modelListMesh);

            //for (int meshPartIx = 0; meshPartIx < modelListMesh.MeshParts.Count; meshPartIx++)
            //{ 
            modelListMesh.MeshParts[0].Effect = shader;
            //}
            modelListMesh.Draw();
            //}
        }

        

        protected override void SetVertexBufferEffect(AbsVoxelObj obj)
        {
            base.shader.CurrentTechnique = base.shader.Techniques[TechniqueName];

            //base.shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(Engine.LoadContent.Texture(obj.texture));
            //base.shader.Parameters["SourcePos"].SetValue(Vector2.Zero);
            //base.shader.Parameters["SourceSize"].SetValue(Vector2.One);

            //Ref.draw.worldMatrix = Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position);//Matrix.CreateTranslation(obj.Position);
            ////if (usesWorldPos)
            ////{
            //    const string CameraPositionSetting = "CameraPosition";
            //    base.shader.Parameters[CameraPositionSetting].SetValue(Ref.draw.Camera.Position);
            //    base.shader.Parameters["world"].SetValue(Ref.draw.worldMatrix);
            //}
            //base.shader.Parameters["wvp"].SetValue(Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection);
            //base.shader.Parameters[CustomEffect.ColorArgument].SetValue(obj.colorAndAlpha);

            shader.Parameters["World"].SetValue(Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position));
            shader.Parameters["View"].SetValue(Ref.draw.Camera.ViewMatrix);
            shader.Parameters["Projection"].SetValue(Ref.draw.Camera.Projection);


            //basicEffect.World = Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position);
            //basicEffect.Projection = Ref.draw.Camera.Projection;
            //basicEffect.View = Ref.draw.Camera.ViewMatrix;
        }

        public static FlagWaveEffect GetSingletonSafe()
        {
            if (Singleton == null)
            {
                Singleton = new FlagWaveEffect();
                //Singleton.ObjectShader();
            }

            //bool on = !Input.Keyboard.Ctrl;
            //Singleton.basicEffect.LightingEnabled = on; // Enable lighting calculations
            //Singleton.basicEffect.DirectionalLight0.Enabled = on;
            //Singleton.basicEffect.DirectionalLight0.Direction = new Vector3(0.1f, -0.8f, -0.8f); 
            //Singleton.basicEffect.DirectionalLight0.DiffuseColor = new Vector3(0.3f); // White light

            //Singleton.basicEffect.DirectionalLight1.Enabled = on;
            //Singleton.basicEffect.DirectionalLight1.Direction = new Vector3(-0.1f, 0.8f, -0.8f);
            //Singleton.basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0.25f, 0.25f, 0); //theme color tint

            //Singleton.basicEffect.AmbientLightColor = new Vector3(0.7f);


            return Singleton;
        }
    }
}

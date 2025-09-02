using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;

namespace VikingEngine.Graphics
{
    class WaveXzEffect : CustomEffect
    {
        public static WaveXzEffect WaveSingleton;
        // Some default wave settings
        public float Time = 0.0f;
        
        public WaveXzEffect()
            : base("WaveXZTechnique", usesWorldPos: true)
        {
            shader = Engine.Draw.effectWaveXz;
            shader.CurrentTechnique = shader.Techniques[TechniqueName];


            float WaveSpeed = 4f;
            float WaveFrequency = 250.0f;
            float WaveAmplitude = 0.0015f;

            // Secondary “flutter” wave for amplitude modulation
            float AmplitudeModFrequency = 5f;
            
            shader.Parameters["WaveSpeed"]?.SetValue(WaveSpeed);
            shader.Parameters["WaveFrequency"]?.SetValue(WaveFrequency);
            shader.Parameters["WaveAmplitude"]?.SetValue(WaveAmplitude);
            shader.Parameters["AmplitudeModFrequency"]?.SetValue(AmplitudeModFrequency);

            shader.Parameters["NoiseScale"]?.SetValue(0.5f);
            shader.Parameters["NoiseSpeed"]?.SetValue(0.3f);
            shader.Parameters["NoiseStrength"]?.SetValue(0.6f);
            shader.Parameters["NoiseOctaves"]?.SetValue(4);
            shader.Parameters["NoiseGain"]?.SetValue(0.5f);
            shader.Parameters["NoiseLacunarity"]?.SetValue(2.0f);

            // Tint gradient (requires using Microsoft.Xna.Framework for Vector3)
            shader.Parameters["TintLo"]?.SetValue(new Vector3(0.9f, 0.9f, 1.0f));
            shader.Parameters["TintHi"]?.SetValue(new Vector3(1.0f, 0.6f, 0.8f));

        }

        public void beginDraw()
        {

            //base.shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(DssRef.state.detailMap.waterEdgeTex());
        }

        public override void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
        {
            shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);
            base.DrawVB(frame, obj, VB);
        }

        /// <summary>
        /// Override the Draw method to set the wave parameters each frame.
        /// </summary>
        public override void Draw(Mesh obj)
        {
            shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);

            shader.CurrentTechnique = shader.Techniques[TechniqueName];
            obj.TextureSource.SetCustomShaderParameters(ref shader);
            
            var model = Engine.LoadContent.Models[(int)obj.LoadedMeshType];

            modelListMesh = model.Meshes[0];
            obj.CalcWorldMatrix(modelListMesh);
 
            modelListMesh.MeshParts[0].Effect = shader;
           
            modelListMesh.Draw();
          
        }

        protected override void SetVertexBufferEffect(AbsVoxelObj obj)
        {
            base.shader.CurrentTechnique = base.shader.Techniques[TechniqueName];

            shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(Engine.LoadContent.Texture(obj.texture));
            //base.shader.Parameters["SourcePos"].SetValue(Vector2.Zero);
            //base.shader.Parameters["SourceSize"].SetValue(Vector2.One);

            shader.Parameters[ColorArgument]?.SetValue(obj.colorAndAlpha);
            shader.Parameters["World"].SetValue(Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position));
            shader.Parameters["View"].SetValue(Ref.draw.Camera.ViewMatrix);
            shader.Parameters["Projection"].SetValue(Ref.draw.Camera.Projection);

        }

        public static WaveXzEffect GetWaveSingletonSafe()
        {
            if (WaveSingleton    == null)
            {
                WaveSingleton = new WaveXzEffect();
            }

            return WaveSingleton;
        }
    }
}

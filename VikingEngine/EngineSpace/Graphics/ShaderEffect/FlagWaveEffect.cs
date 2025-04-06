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

        
        public FlagWaveEffect()
            : base("FlagWaveTechnique", usesWorldPos: true)
        {
            shader = Engine.Draw.effectFlag;
            shader.CurrentTechnique = shader.Techniques[TechniqueName];

            shader.Parameters["WaveSpeed"]?.SetValue(WaveSpeed);
            shader.Parameters["WaveFrequency"]?.SetValue(WaveFrequency);
            shader.Parameters["WaveAmplitude"]?.SetValue(WaveAmplitude);
            shader.Parameters["AmplitudeModFrequency"]?.SetValue(AmplitudeModFrequency);
            //shader.Parameters["LightColor"]?.SetValue(new Vector3(1f));
            //shader.Parameters["AmplitudeModRange"]?.SetValue(AmplitudeModRange);
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

            shader.Parameters["World"].SetValue(Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position));
            shader.Parameters["View"].SetValue(Ref.draw.Camera.ViewMatrix);
            shader.Parameters["Projection"].SetValue(Ref.draw.Camera.Projection);

        }

        public static FlagWaveEffect GetSingletonSafe()
        {
            if (Singleton == null)
            {
                Singleton = new FlagWaveEffect();
            }

            return Singleton;
        }
    }
}

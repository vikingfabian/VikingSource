using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;

namespace VikingEngine.Graphics
{
    class SeaNoiseEffect : CustomEffect
    {
        public static SeaNoiseEffect Singleton;

        // Noise animation time
        public float Time { get; set; } = 0.0f;

        public SeaNoiseEffect()
            : base("SeaNoiseTechnique", usesWorldPos: true)
        {
            shader = Engine.Draw.effectSeaNoise;
            shader.CurrentTechnique = shader.Techniques[TechniqueName];

            shader.Parameters["Time"]?.SetValue(Time);
        }

        public override void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
        {
            shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);
            base.DrawVB(frame, obj, VB);
        }

        public override void Draw(Mesh obj)
        {
            shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);

            shader.CurrentTechnique = shader.Techniques[TechniqueName];
            //obj.TextureSource.SetCustomShaderParameters(ref shader);

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

        public static SeaNoiseEffect GetSingletonSafe()
        {
            if (Singleton == null)
            {
                Singleton = new SeaNoiseEffect();
            }

            return Singleton;
        }
    }
}

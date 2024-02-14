using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class CustomEffect : AbsEffect
    {
        public const string ColorArgument = "ColorAndAlpha";

        static ModelMesh modelListMesh;
        static int modelMeshIx = 0;
        string TechniqueName;
        bool usesWorldPos;

        public CustomEffect(string TechniqueName, bool usesWorldPos)
        {
            shader = Engine.Draw.effectBR;
            this.usesWorldPos = usesWorldPos;
            this.TechniqueName = TechniqueName;
        }
        public override void Draw(Mesh obj)
        {
            shader.CurrentTechnique = shader.Techniques[TechniqueName];
            obj.TextureSource.SetCustomShaderParameters(ref shader);
            shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(obj.texture);
            shader.Parameters[ColorArgument].SetValue(obj.colorAndAlpha);

            var model = Engine.LoadContent.Mesh(obj.LoadedMeshType);

            for (modelMeshIx = 0; modelMeshIx < model.Meshes.Count; modelMeshIx++)
            {
                modelListMesh = model.Meshes[modelMeshIx];
                obj.CalcWorldMatrix(modelListMesh);

                for (int meshPartIx = 0; meshPartIx < modelListMesh.MeshParts.Count; meshPartIx++)
                { modelListMesh.MeshParts[meshPartIx].Effect = shader; }
                modelListMesh.Draw();
            }
        }

        protected override void SetVertexBufferEffect(AbsVoxelObj obj)
        {
            base.shader.CurrentTechnique = base.shader.Techniques[TechniqueName];

            base.shader.Parameters["ColorMap"].SetValue(Engine.LoadContent.Texture(obj.texture));
            base.shader.Parameters["SourcePos"].SetValue(Vector2.Zero);
            base.shader.Parameters["SourceSize"].SetValue(Vector2.One);

            Ref.draw.worldMatrix = Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position);//Matrix.CreateTranslation(obj.Position);
            if (usesWorldPos)
            {
                const string CameraPositionSetting = "CameraPosition";
                base.shader.Parameters[CameraPositionSetting].SetValue(Ref.draw.Camera.Position);               
                base.shader.Parameters["world"].SetValue(Ref.draw.worldMatrix);
            }
            base.shader.Parameters["wvp"].SetValue(Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection);
            base.shader.Parameters[CustomEffect.ColorArgument].SetValue(obj.colorAndAlpha);
        }        
    }    
}

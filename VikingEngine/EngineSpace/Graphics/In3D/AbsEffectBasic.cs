using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    abstract class AbsEffectBasic : AbsEffect
    {
        protected static Vector3 AmbientCol = VectorExt.V3(0.72f);
        public Microsoft.Xna.Framework.Graphics.BasicEffect basicEffect;
        // override protected void SetEffectVWP(AbsVoxelObj obj)

        public override void Draw(Mesh obj)
        {
            throw new NotImplementedException();
        }

        override protected void SetVertexBufferEffect(AbsVoxelObj obj)
        {
            //Model.World = Scale * Rotate * Translate //make sure this is the order
            basicEffect.World = Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position);
            basicEffect.Projection = Ref.draw.Camera.Projection;
            basicEffect.View = Ref.draw.Camera.ViewMatrix;
        }

        public override void SetColor(Vector3 col)
        {
            basicEffect.AmbientLightColor = col * AmbientCol;
        }
    }
    class EffectBasicNormal : AbsEffectBasic
    {
        public static EffectBasicNormal Singleton = new EffectBasicNormal(VikingEngine.LootFest.LfLib.BlockTexture);

        public static EffectBasicNormal GetSingletonSafe()
        {
            if (Singleton == null)
            {
                Singleton = new EffectBasicNormal(VikingEngine.LootFest.LfLib.BlockTexture);
            }
            return Singleton;
        }

        public EffectBasicNormal(LoadedTexture texture)
        {
            basicEffect = new Microsoft.Xna.Framework.Graphics.BasicEffect(Engine.Draw.graphicsDeviceManager.GraphicsDevice);
            basicEffect.TextureEnabled = true;

            //if (PlatformSettings.RunProgram == StartProgram.TableTop)
            //{
            //    basicEffect.VertexColorEnabled = true;
            //    basicEffect.Texture = Engine.LoadContent.Texture(LoadedTexture.LF3Tiles);
            //}
            //else
            //{
            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                AmbientCol = new Vector3(0.9f);
            }

            basicEffect.EnableDefaultLighting();
            basicEffect.DirectionalLight0.Enabled = true;
            Vector3 lightDir = new Vector3(-0.08f, 0.89f, 0.45f);
            lightDir.Normalize();

            Vector3 orangeLight = Color.Orange.ToVector3() * 0.4f;
            Vector3 blueLight = new Vector3(0, 0.1f, 0.5f) * 0.6f;

            basicEffect.DirectionalLight0.Direction = lightDir;
            basicEffect.DirectionalLight0.DiffuseColor = VectorExt.V3(0.4f);
            basicEffect.DirectionalLight0.SpecularColor = VectorExt.V3(0.12f);//Vector3.Zero;
            basicEffect.DirectionalLight1.Enabled = true;
            basicEffect.DirectionalLight1.DiffuseColor = orangeLight;
            basicEffect.DirectionalLight1.SpecularColor = Vector3.Zero; // orangeLight;
            basicEffect.DirectionalLight2.Enabled = true;
            basicEffect.DirectionalLight2.DiffuseColor = blueLight;
            basicEffect.DirectionalLight2.SpecularColor = Vector3.Zero; //blueLight;
            Vector3 blueSide = new Vector3(-0.88f, 0.4f, 0.24f);
            Vector3 orangeSide = new Vector3(0.68f, -0.0005f, -0.73f);
            blueSide.Normalize(); orangeSide.Normalize();
            basicEffect.DirectionalLight1.Direction = orangeSide;
            basicEffect.DirectionalLight2.Direction = blueSide;

            basicEffect.AmbientLightColor = AmbientCol;
            basicEffect.Texture = Engine.LoadContent.Texture(texture);
            //}
            shader = basicEffect;
        }
    }
    class EffectBasicColor : AbsEffectBasic
    {
        public static EffectBasicColor Singleton;

        public EffectBasicColor(LoadedTexture tex)
        {
            basicEffect = new Microsoft.Xna.Framework.Graphics.BasicEffect(Engine.Draw.graphicsDeviceManager.GraphicsDevice);
            
            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = true;
            basicEffect.Texture = Engine.LoadContent.Texture(tex);

            shader = basicEffect;
        }

        public static EffectBasicColor GetSingletonSafe()
        {
            if (Singleton == null)
            {
                Singleton = new EffectBasicColor(LoadedTexture.WhiteArea);
            }
            return Singleton;
        }
    }

    class EffectBasicVertexColor : AbsEffectBasic
    {
        public static EffectBasicVertexColor Singleton;

        public EffectBasicVertexColor()
        {
            basicEffect = new Microsoft.Xna.Framework.Graphics.BasicEffect(Engine.Draw.graphicsDeviceManager.GraphicsDevice);

            basicEffect.TextureEnabled = false;
            basicEffect.VertexColorEnabled = true;

            shader = basicEffect;
        }

        public static EffectBasicVertexColor GetSingletonSafe()
        {
            if (Singleton == null)
            {
                Singleton = new EffectBasicVertexColor();
            }
            return Singleton;
        }
    }
}

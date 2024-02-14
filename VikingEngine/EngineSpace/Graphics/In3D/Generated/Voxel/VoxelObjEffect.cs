//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.Graphics
//{
//    abstract class AbsVoxelObjEffect
//    {
//        protected Effect effect;
//        //abstract protected void SetEffectVWP(AbsVoxelObj obj);
//        abstract protected void SetVertexBufferEffect(AbsVoxelObj obj);

//        public void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
//        {
//        //    SetEffectVWP(obj);
//            SetVertexBufferEffect(obj);

//            VB.SetBuffer();

//            effect.CurrentTechnique.Passes[0].Apply();
//            VB.Draw(frame);
//        }

//        virtual public void SetColor(Vector3 col) {  throw new NotImplementedException(); }
//    }

//    abstract class AbsVoxelObjEffectBasic : AbsVoxelObjEffect
//    {
//        protected static Vector3 AmbientCol = VectorExt.V3(0.72f);
//        public BasicEffect basicEffect;
//       // override protected void SetEffectVWP(AbsVoxelObj obj)
//        override protected void SetVertexBufferEffect(AbsVoxelObj obj)
//        {
//            //Model.World = Scale * Rotate * Translate //make sure this is the order
//            basicEffect.World = Matrix.CreateScale(obj.Scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.Position);
//            basicEffect.Projection = Ref.draw.Camera.Projection;
//            basicEffect.View = Ref.draw.Camera.ViewMatrix;
//        }

//        public override void SetColor(Vector3 col)
//        {
//            basicEffect.AmbientLightColor = col * AmbientCol;
//        }
//    }

//    class VoxelObjEffectBasicNormal : AbsVoxelObjEffectBasic
//    {
//        public static VoxelObjEffectBasicNormal Singleton = new VoxelObjEffectBasicNormal();

//        public static VoxelObjEffectBasicNormal GetSingletonSafe()
//        {
//            while (Singleton == null)
//            {
//                Debug.LogError("VoxelObjEffectBasicNormal is null");
//                Singleton = new VoxelObjEffectBasicNormal();
//            }
//            return Singleton;
//        }

//        public VoxelObjEffectBasicNormal()
//        {
//            basicEffect = new Microsoft.Xna.Framework.Graphics.BasicEffect(Engine.Draw.graphicsDeviceManager.GraphicsDevice);
//            basicEffect.TextureEnabled = true;

//            //if (PlatformSettings.RunProgram == StartProgram.TableTop)
//            //{
//            //    basicEffect.VertexColorEnabled = true;
//            //    basicEffect.Texture = Engine.LoadContent.Texture(LoadedTexture.LF3Tiles);
//            //}
//            //else
//            //{
//               // if (PlatformSettings.RunProgram == StartProgram.Stupid)
//                if (PlatformSettings.RunProgram == StartProgram.Wars)
//                {
//                    AmbientCol = new Vector3(0.9f);
//                }

//                basicEffect.EnableDefaultLighting();
//                basicEffect.DirectionalLight0.Enabled = true;
//                Vector3 lightDir = new Vector3(-0.08f, 0.89f, 0.45f);
//                lightDir.Normalize();

//                Vector3 orangeLight = Color.Orange.ToVector3() * 0.4f;
//                Vector3 blueLight = new Vector3(0, 0.1f, 0.5f) * 0.6f;

//                basicEffect.DirectionalLight0.Direction = lightDir;
//                basicEffect.DirectionalLight0.DiffuseColor = VectorExt.V3(0.4f);
//                basicEffect.DirectionalLight0.SpecularColor = VectorExt.V3(0.12f);//Vector3.Zero;
//                basicEffect.DirectionalLight1.Enabled = true;
//                basicEffect.DirectionalLight1.DiffuseColor = orangeLight;
//                basicEffect.DirectionalLight1.SpecularColor = Vector3.Zero; // orangeLight;
//                basicEffect.DirectionalLight2.Enabled = true;
//                basicEffect.DirectionalLight2.DiffuseColor = blueLight;
//                basicEffect.DirectionalLight2.SpecularColor = Vector3.Zero; //blueLight;
//                Vector3 blueSide = new Vector3(-0.88f, 0.4f, 0.24f);
//                Vector3 orangeSide = new Vector3(0.68f, -0.0005f, -0.73f);
//                blueSide.Normalize(); orangeSide.Normalize();
//                basicEffect.DirectionalLight1.Direction = orangeSide;
//                basicEffect.DirectionalLight2.Direction = blueSide;

//                basicEffect.AmbientLightColor = AmbientCol;
//                basicEffect.Texture = Engine.LoadContent.Texture(VikingEngine.LootFest.LfLib.BlockTexture);
//            //}
//            effect = basicEffect;
//        }
//    }

//    class VoxelObjEffectBasicColor: AbsVoxelObjEffectBasic
//    {
//       public static VoxelObjEffectBasicColor Singleton;
       
//        public VoxelObjEffectBasicColor(LoadedTexture tex)
//        {
//            basicEffect = new Microsoft.Xna.Framework.Graphics.BasicEffect(Engine.Draw.graphicsDeviceManager.GraphicsDevice);
//            basicEffect.TextureEnabled = true;
//            basicEffect.VertexColorEnabled = true;
//            basicEffect.Texture = Engine.LoadContent.Texture(tex);

//            effect = basicEffect;
//        }

//        public static VoxelObjEffectBasicColor GetSingletonSafe()
//        {
//            while (Singleton == null)
//            {
//                Debug.LogError("VoxelObjEffectBasicNormal is null");
//                Singleton = new VoxelObjEffectBasicColor(LoadedTexture.WhiteArea);
//            }
//            return Singleton;
//        }
//    }

//    class VoxelObjEffectCustom : AbsVoxelObjEffect
//    {
//        int technique;
//        LoadedTexture tex;

//        public VoxelObjEffectCustom(LoadedTexture tex, TextureEffectType technique)
//        {
//            this.tex = tex;
//            this.technique = (int)technique;
//            effect = VikingEngine.Engine.LoadContent.LoadShader("Effect");
//        }

//        protected override void SetVertexBufferEffect(AbsVoxelObj obj)
//        {
//            effect.CurrentTechnique = effect.Techniques[Graphics.TextureEffectLib.TechniqueName[technique]];

//            effect.Parameters["ColorMap"].SetValue(Engine.LoadContent.Texture(tex));
//            effect.Parameters["SourcePos"].SetValue(Vector2.Zero);
//            effect.Parameters["SourceSize"].SetValue(Vector2.One);

//            const string CameraPositionSetting = "CameraPosition";
//            effect.Parameters[CameraPositionSetting].SetValue(Ref.draw.Camera.Position);

//            Ref.draw.worldMatrix = Matrix.CreateScale(obj.Scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.Position);//Matrix.CreateTranslation(obj.Position);
//            effect.Parameters["world"].SetValue(Ref.draw.worldMatrix);
//            effect.Parameters["wvp"].SetValue(Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection);

//            effect.Parameters[CustomEffect.ColorArgument].SetValue(obj.ColorAndAlpha);
//        }
//    }

//    //class VoxelObjEffectWater : AbsVoxelObjEffect
//    //{
//    //    public static readonly VoxelObjEffectWater Instance = new VoxelObjEffectWater();

//    //    public VoxelObjEffectWater()
//    //    {
//    //        effect = Engine.LoadContent.Effect(LoadedEffect.Water);

//    //        effect.CurrentTechnique = effect.Techniques["Water"];
//    //        effect.Parameters["ColorMap"].SetValue(Engine.LoadContent.Texture(LoadedTexture.waterheigth));
//    //    }

//    //    protected override void SetEffectVWP(AbsVoxelObj obj)
//    //    {
//    //        const string CameraPositionSetting = "CameraPosition";
//    //        effect.Parameters[CameraPositionSetting].SetValue(Ref.draw.Camera.Position);
//    //        effect.Parameters["WaterTime"].SetValue(Ref.update.TotalGameTime);

//    //        Ref.draw.worldMatrix = Matrix.CreateTranslation(obj.Position);
//    //        effect.Parameters["world"].SetValue(Ref.draw.worldMatrix);
//    //        effect.Parameters["wvp"].SetValue(Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection);
//    //    }
//    //}
//}

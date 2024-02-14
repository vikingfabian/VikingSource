using VikingEngine.Engine;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering
{
    class GaussianBlur
    {
        /* Properties */
        public float Amount { get { return amount; } set { amount = value; } }

        /* Fields */
        Effect effect;
        int radius;
        float amount;
        float[] kernel;
        Vector2[] offsetsHori;
        Vector2[] offsetsVerti;
        RenderTarget2D intermediateRenderTarget;

        /* Constructors */
        public GaussianBlur(float amount)
        {
            radius = 7; // match in shader:
            effect = LoadContent.LoadShader("DeferredRenderer\\SSAOGaussianBlur");
            effect.CurrentTechnique = effect.Techniques[0];
            this.amount = amount;
        }

        /* Novelty methods */
        public void ComputeKernel()
        {
            kernel = new float[radius * 2 + 1];
            float sigma = radius / amount;

            float twoSigmaSquare = 2.0f * sigma * sigma;
            float sigmaRoot = (float)Math.Sqrt(twoSigmaSquare * Math.PI);
            float total = 0.0f;
            float distance = 0.0f;
            int index = 0;

            for (int i = -radius; i <= radius; ++i)
            {
                distance = i * i;
                index = i + radius;
                kernel[index] = (float)Math.Exp(-distance / twoSigmaSquare) / sigmaRoot;
                total += kernel[index];
            }

            for (int i = 0; i < kernel.Length; ++i)
                kernel[i] /= total;
        }

        public void ComputeOffsets(float textureWidth, float textureHeight)
        {
            offsetsHori  = new Vector2[radius * 2 + 1];
            offsetsVerti = new Vector2[radius * 2 + 1];

            int index = 0;
            float xOffset = 1.0f / textureWidth;
            float yOffset = 1.0f / textureHeight;

            for (int i = -radius; i <= radius; ++i)
            {
                index = i + radius;
                offsetsHori[index] = new Vector2(i * xOffset, 0.0f);
                offsetsVerti[index] = new Vector2(0.0f, i * yOffset);
            }
        }

        public void PerformGaussianBlur(GraphicsDevice device, RenderTarget2D input, RenderTarget2D output, FullscreenQuad quad)
        {
            if (intermediateRenderTarget == null ||
                (intermediateRenderTarget.Width != Screen.Width ||
                intermediateRenderTarget.Height != Screen.Height))
            {
                ComputeOffsets(Screen.Width, Screen.Height);
                intermediateRenderTarget = new RenderTarget2D(device, Screen.Width, Screen.Height, false, SurfaceFormat.Color, DepthFormat.None);
            }
            ComputeKernel();

            // Perform horizontal Gaussian blur on intermediate.
            device.SetRenderTarget(intermediateRenderTarget);
            device.Clear(Color.White);
            device.Textures[3] = input;
            device.SamplerStates[3] = SamplerState.LinearClamp;
            effect.Parameters["weights"].SetValue(kernel);
            effect.Parameters["offsets"].SetValue(offsetsHori);
            effect.Parameters["TargetSize"].SetValue(Screen.ResolutionVec);

            effect.CurrentTechnique.Passes[0].Apply();
            quad.ReadyAndDraw(device);

            // Perform vertical Gaussian blur on output.
            device.SetRenderTarget(output);
            device.Clear(Color.White);
            device.Textures[3] = intermediateRenderTarget;
            effect.Parameters["offsets"].SetValue(offsetsVerti);

            effect.CurrentTechnique.Passes[0].Apply();
            quad.Draw(device);
        }
    }

    class SSAO
    {
        /* Constants */
        const int NUM_SAMPLES = 12;
        const int NOISETEX_SIDE = 8;

        /* Properties */
        public float SampleRadius { get { return sampleRadius; } set { sampleRadius = value; } }
        public float OcclusionPower { get { return occlusionPower; } set { occlusionPower = value; } }
        //public GaussianBlur Blur { get { return gaussianBlur; } }

        /* Fields */
        Effect ssao;
        Effect ssaoBlur;
        Effect composer;
        Texture2D randomNormals;
        float sampleRadius;
        float occlusionPower;
        RenderTarget2D ssaoTarget;
        RenderTarget2D intermediateTarget;
        RenderTarget2D blurTarget;
        FullscreenQuad fullscreenQuad;
        //GaussianBlur gaussianBlur;

        Vector3[] sampleKernel;

        /* Constructors */
        public SSAO(GraphicsDevice device, int width, int height)
        {
            ssao = LoadContent.LoadShader("DeferredRenderer\\SSAO");
            ssao.CurrentTechnique = ssao.Techniques[0];

            ssaoBlur = LoadContent.LoadShader("DeferredRenderer\\SSAOLinearBlur");
            ssaoBlur.CurrentTechnique = ssaoBlur.Techniques[0];

            composer = LoadContent.LoadShader("DeferredRenderer\\SSAOFinal");
            composer.CurrentTechnique = composer.Techniques[0];

            ssaoTarget = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            intermediateTarget = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            blurTarget = new RenderTarget2D(device, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            fullscreenQuad = new FullscreenQuad(device);

            randomNormals = LoadContent.LoadTexture("Texture\\ssao_random_normals");
            sampleRadius = 0.3f;
            occlusionPower = 3.1f;

            GenerateSampleKernel();
            //GenerateNoiseTexture(device);
        }

        /* Novelty methods */
        public void Debug(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, null, null);

            int width = Engine.Screen.Width / 4;
            int height = Engine.Screen.Height / 4;

            Rectangle rect = new Rectangle(width * 3, height * 3, width, height);

            spriteBatch.Draw((Texture2D)ssaoTarget, rect, Color.White);
            //rect.X += width;
            //spriteBatch.Draw((Texture2D)blurTarget, rect, Color.White);

            spriteBatch.End();
        }

        public void Draw(GraphicsDevice device, RenderTargetBinding[] gBuffer, AbsCamera camera, RenderTarget2D output)
        {
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            device.RasterizerState = RasterizerState.CullCounterClockwise;

            RenderSSAO(device, gBuffer, camera);
            RenderBlur(device, output);
            //RenderComposition(device, scene, output, true);
        }

        void GenerateSampleKernel()
        {
            sampleKernel = new Vector3[NUM_SAMPLES];
            for (int i = 0; i < sampleKernel.Length; ++i)
            {
                float phi = Ref.rnd.Float(MathHelper.TwoPi);
                float rho = Ref.rnd.Float(MathHelper.Pi);

                sampleKernel[i].X = (float)(Math.Cos(phi) * Math.Sin(rho));
                sampleKernel[i].Y = (float)(Math.Sin(phi) * Math.Sin(rho));
                sampleKernel[i].Z = (float)Math.Cos(rho);

                sampleKernel[i] *= Ref.rnd.Float(0.1f, 1.0f);
                float scale = (float)i / (float)NUM_SAMPLES;
                scale = MathExt.Lerp(0.1f, 1.0f, scale * scale);
                sampleKernel[i] *= scale;
            }
        }

        void GenerateNoiseTexture(GraphicsDevice device)
        {
            Texture2D tex = new Texture2D(device, NOISETEX_SIDE, NOISETEX_SIDE, false, SurfaceFormat.Vector4);

            Vector4[] noiseTexture;
            int N = NOISETEX_SIDE * NOISETEX_SIDE;
            noiseTexture = new Vector4[N];
            for (int i = 0; i < N; ++i)
            {
                float phi = Ref.rnd.Float(MathHelper.TwoPi);
                float rho = Ref.rnd.Float(MathHelper.Pi);

                Vector3 color = new Vector3((float)(Math.Cos(phi) * Math.Sin(rho)),
                                            (float)(Math.Sin(phi) * Math.Sin(rho)),
                                            (float)Math.Cos(rho));

                //Vector3 color = new Vector3(Ref.rnd.Float(-1.0f, 1.0f), Ref.rnd.Float(-1.0f, 1.0f), Ref.rnd.Float(-1, 1));
                //color.Normalize();
                noiseTexture[i].X = color.X * 0.5f + 0.5f;
                noiseTexture[i].Y = color.Y * 0.5f + 0.5f;
                noiseTexture[i].Z = color.Z * 0.5f + 0.5f;
                noiseTexture[i].W = 1.0f;
            }

            tex.SetData(noiseTexture);
            System.IO.FileStream stream = new System.IO.FileStream("noiseTex.png", System.IO.FileMode.Create);
            tex.SaveAsPng(stream, NOISETEX_SIDE, NOISETEX_SIDE);
            stream.Dispose();
        }

        void RenderSSAO(GraphicsDevice device, RenderTargetBinding[] gBuffer, AbsCamera camera)
        {
            device.SetRenderTarget(ssaoTarget);
            device.Clear(Color.White);
            device.Textures[1] = gBuffer[1].RenderTarget;
            device.SamplerStates[1] = SamplerState.LinearClamp;
            device.Textures[2] = gBuffer[2].RenderTarget;
            device.SamplerStates[2] = SamplerState.PointClamp;
            device.Textures[3] = randomNormals;
            device.SamplerStates[3] = SamplerState.LinearWrap;

            Vector3 cornerFrustum = Vector3.Zero;
            cornerFrustum.Y = (float)Math.Tan(MathHelper.ToRadians(camera.FieldOfView) * 0.5f);
            cornerFrustum.X = cornerFrustum.Y * camera.aspectRatio;
            cornerFrustum.Z = 1;

            ssao.Parameters["Projection"].SetValue(camera.Projection);
            ssao.Parameters["CornerFrustum"].SetValue(cornerFrustum);
            ssao.Parameters["SampleRadius"].SetValue(sampleRadius);
            ssao.Parameters["OcclusionPower"].SetValue(occlusionPower);
            ssao.Parameters["GBufferTextureSize"].SetValue(new Vector2(ssaoTarget.Width,
                                                                       ssaoTarget.Height));
            ssao.Parameters["sampleKernel"].SetValue(sampleKernel);
            ssao.CurrentTechnique.Passes[0].Apply();

            fullscreenQuad.ReadyAndDraw(device);
        }

        void RenderBlur(GraphicsDevice device, RenderTarget2D output)
        {
            device.SetRenderTarget(intermediateTarget);
            device.Textures[3] = ssaoTarget;
            device.SamplerStates[3] = SamplerState.LinearClamp;

            ssaoBlur.Parameters["TargetSize"].SetValue(new Vector2(blurTarget.Width, blurTarget.Height));
            ssaoBlur.Parameters["BlurDirection"].SetValue(new Vector2(1, 0));
            ssaoBlur.CurrentTechnique.Passes[0].Apply();
            fullscreenQuad.Draw(device);

            device.SetRenderTarget(output);
            device.Textures[3] = intermediateTarget;
            ssaoBlur.Parameters["BlurDirection"].SetValue(new Vector2(0, 1));
            ssaoBlur.CurrentTechnique.Passes[0].Apply();
            fullscreenQuad.Draw(device);

            //gaussianBlur.PerformGaussianBlur(device, ssaoTarget, blurTarget, fullscreenQuad);
        }

        /*
        void RenderComposition(GraphicsDevice device, RenderTarget2D scene, RenderTarget2D output, bool useBlurredSSAO)
        {
            device.SetRenderTarget(output);
            device.Clear(Color.White);
            device.Textures[0] = scene;
            device.SamplerStates[0] = SamplerState.LinearClamp;

            if (useBlurredSSAO)
                device.Textures[1] = blurTarget;
            else
                device.Textures[1] = ssaoTarget;

            device.SamplerStates[1] = SamplerState.LinearClamp;

            composer.Parameters["HalfPixel"].SetValue(new Vector2(0.5f / ssaoTarget.Width,
                                                                  0.5f / ssaoTarget.Height));
            composer.CurrentTechnique.Passes[0].Apply();
            fullscreenQuad.ReadyAndDraw(device);
        }
        */
    }
}

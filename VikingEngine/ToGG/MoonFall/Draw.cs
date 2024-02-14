using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace VikingEngine.ToGG.MoonFall
{
    class Draw : Engine.Draw
    {
        public const float ShadowOpacity = 0.2f;
        public const int MapLayer = (int)RenderLayer.Layer3;
        public const int ShadowObjLayer = (int)RenderLayer.Layer2;
        public const int HudLayer = (int)RenderLayer.Basic;

        RenderTarget2D shadowTarget;
        Graphics.ImageAdvanced shadowTargetImage_Color, shadowTargetImage_Shadow;

        public Draw()
        {
            shadowTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice,
                Engine.Screen.RenderingResolution.X, Engine.Screen.RenderingResolution.Y,
                false, SurfaceFormat.Color, DepthFormat.Depth24);

            shadowTargetImage_Color = new Graphics.ImageAdvanced(false);
            shadowTargetImage_Color.Texture = shadowTarget;
            shadowTargetImage_Color.SetFullTextureSource();

            shadowTargetImage_Color.Size = Engine.Screen.RenderingResolution.Vec;


        }

        public void initShadow()
        {
            shadowTargetImage_Shadow = shadowTargetImage_Color.Clone();
            shadowTargetImage_Shadow.Color = Color.Black;
            shadowTargetImage_Shadow.Opacity = ShadowOpacity;

            shadowTargetImage_Shadow.Position += new Vector2(MathExt.Round(moonRef.map.soldierHeight * 0.1f));//GolfRef.field.squareSize * 0.06f;

            shadowTargetImage_Color.Layer = ImageLayers.Bottom8_Front;
            shadowTargetImage_Shadow.Layer = ImageLayers.Bottom8;
        }

        protected override void drawEvent()
        {
            Ref.draw.AddToContainer = null;

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(shadowTarget);
            spriteBatch.GraphicsDevice.Clear(ColorExt.Empty);
            Draw2d(ShadowObjLayer);

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(MainRenderTarget);

            spriteBatch.GraphicsDevice.Clear(ClrColor);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);
            KeepDraw2D(MapLayer);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, null);
            shadowTargetImage_Shadow.Draw(0);
            shadowTargetImage_Color.Draw(0);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, null);

            KeepDraw2D(HudLayer);

            spriteBatch.End();
        }
    }
}

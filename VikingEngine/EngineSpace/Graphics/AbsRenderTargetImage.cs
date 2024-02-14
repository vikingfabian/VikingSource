using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsRenderTargetImage : AbsDraw2D
    {
        /* Properties */
        abstract protected BlendState blendState { get; }

        public Vector2 Transform2D
        {
            set
            {
                store2Dtrans = value;
                updateTransformMatrix();
            }
            get { return store2Dtrans; }
        }
        public float Scale2D
        {
            set
            {
                scale = value;
                updateTransformMatrix();
            }
            get { return scale; }
        }

        /* Fields */
        public Color ClearColor = ColorExt.Empty;
        public Graphics.AbsCamera Camera;

        public Rectangle drawSource;

        Matrix transformMatrix = Matrix.Identity;
        Vector2 store2Dtrans = Vector2.Zero;
        float scale = 1;

        /* Constructors */
        public AbsRenderTargetImage(Vector2 pos, Vector2 size, bool addToRender)
            : base(addToRender)
        {
            this.position = pos;
            this.size = size;
            drawSource = new VectorRect(Vector2.Zero, size).Rectangle;
        }
        public AbsRenderTargetImage(Vector2 pos, Vector2 size)
            : this(pos, size, true)
        { }

        /* Methods */
        abstract public void SetAsTarget(bool set, CubeMapFace face);

        public void SetAsTarget(bool set) { this.SetAsTarget(set, CubeMapFace.NegativeX); }
        public void ClearTarget()
        {
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.Clear(ClearColor);
        }

        
        /// <summary>
        /// Draws a list of images to the rendertarget
        /// </summary>
        /// <param name="clear">remove the current image contained</param>
        /// <remarks>Does only work with 2d images</remarks>
        public void DrawImagesToTarget(List<Graphics.AbsDraw> renderList2D, bool clear = true, CubeMapFace face = CubeMapFace.NegativeX)
        {
            this.DrawImagesToTarget(renderList2D, null, clear, face);
        }
        public void DrawImagesToTarget(List<Graphics.AbsDraw> renderList2D, List<Graphics.AbsDraw> renderList3D, 
            bool clear, CubeMapFace face)
        {
            
            if (!clear)
            {//must save its texture
                this.visible = true;
                Engine.Draw.RenderTargetImageBuffer.DrawImagesToTarget(new List<AbsDraw> { this }, true); //RenderTargetImageBuffer = null på xboxen
            }

            SetAsTarget(true, face);
            ClearTarget();
            

            if (renderList2D != null)
            {
                Ref.draw.spriteBatch.Begin(SpriteSortMode.BackToFront, this.blendState, null, null, null, null, transformMatrix);
                if (!clear)
                {//must draw itself on top
                    Engine.Draw.RenderTargetImageBuffer.Draw(0);
                }
                for (int i = 0; i < renderList2D.Count; i++)
                {
                    renderList2D[i].Draw(0);
                }
                Ref.draw.spriteBatch.End();
            }

            if (renderList3D != null)
            {
                AbsCamera storeCam = null;
                if (Camera != null)
                {
                    storeCam = Ref.draw.Camera;
                    Ref.draw.Camera = Camera;
                }

                for (int i = 0; i < renderList3D.Count; i++)
                {
                    if (renderList3D[i] != null)
                    {
                        renderList3D[i].Draw(0);
                    }
                }

                if (storeCam != null)
                { Ref.draw.Camera = storeCam; }
            }
            SetAsTarget(false);
        }

        private void updateTransformMatrix()
        {
            Vector3 translate = Vector3.Zero;
            translate.X = store2Dtrans.X;
            translate.Y = store2Dtrans.Y;
            transformMatrix = Matrix.CreateTranslation(translate) * Matrix.CreateScale(new Vector3(scale, scale, 1f));
        }
    }
}

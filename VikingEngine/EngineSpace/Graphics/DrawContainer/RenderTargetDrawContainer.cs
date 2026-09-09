using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Keeps a list of images and render them in a restricted area, with the help of a rendertarget
    /// </summary>
    class RenderTargetDrawContainer : RenderTargetImage, IDrawContainer, IRenderTargetContainer
    {
        public List<Graphics.AbsDraw> renderList;
        public bool alwaysRedraw = false;
        public bool isDirty = true;

        public RenderTargetDrawContainer(Vector2 pos, Vector2 size, ImageLayers layer, List<Graphics.AbsDraw> renderList2D, 
            bool addToDraw = true)
           : base(pos, size, layer)
        {
            this.renderList = renderList2D;

            if (addToDraw)
            {
                Ref.draw?.drawContainers.Add(this);
            }
        }

        public void AddImage(Graphics.AbsDraw image)
        {
            image.DeleteMe();
            renderList.Add(image);
            isDirty = true;
        }
        public void RemoveImage(Graphics.AbsDraw image)
        {
            renderList.Remove(image);
            isDirty = true;
        }

        public void ClearImageList()
        {
            renderList.Clear();
            isDirty = true;
        }       

        public override void DeleteMe()
        {
            base.DeleteMe();
            Ref.draw?.drawContainers.Remove(this);
        }

        public void DrawToTarget()
        {
            if (visible && (alwaysRedraw || isDirty))
            {
                DrawImagesToTarget(renderList, true);
                isDirty = false;
            }
        }

        public void Render()
        { 
            DrawToTarget();
        }

        public override float PaintLayer
        {
            get
            {
                return base.PaintLayer;
            }
            set
            {
                base.PaintLayer = value;
            }
        }
    }
}

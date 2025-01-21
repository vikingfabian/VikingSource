using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Keeps a list of images and render them in a restricted area, with the help of a rendertarget
    /// </summary>
    class RenderTargetDrawContainer : RenderTargetImage, IDrawContainer, IUpdateable
    {
        public List<Graphics.AbsDraw> renderList;

        public RenderTargetDrawContainer(Vector2 pos, Vector2 size, ImageLayers layer, List<Graphics.AbsDraw> renderList2D)
           : base(pos, size, layer)
        {
            this.renderList = renderList2D;
            Ref.update.AddToOrRemoveFromUpdate(this, true);
        }

        public void AddImage(Graphics.AbsDraw image)
        {
            image.DeleteMe();
            renderList.Add(image);
        }
        public void RemoveImage(Graphics.AbsDraw image)
        {
            renderList.Remove(image);
        }

        public void ClearImageList()
        {
            renderList.Clear();
        }       

        public override void DeleteMe()
        {
            base.DeleteMe();
            Ref.update.AddToOrRemoveFromUpdate(this, false);
        }

        public UpdateType UpdateType { get { return VikingEngine.UpdateType.Full; } }
        public void Time_Update(float time)
        {
            if (visible)
            {
                DrawImagesToTarget(renderList, true);
            }
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

        public bool RunDuringPause { get { return true; } }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    interface IDrawContainer
    {
        void AddImage(Graphics.AbsDraw image);
        void RemoveImage(Graphics.AbsDraw image);
    }

    abstract class AbsDrawContainer : AbsDraw, IDrawContainer
    {
        protected List<Graphics.AbsDraw> drawList = new List<AbsDraw>();

        public void AddImage(Graphics.AbsDraw image)
        {
            image.DeleteMe();
            drawList.Add(image);
        }
        public void RemoveImage(Graphics.AbsDraw image)
        {
            drawList.Remove(image);
        }

        public void ClearImageList()
        {
            drawList.Clear();
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            throw new NotImplementedException();
        }
        public override AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override Color Color
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override float Opacity
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override void UpdateCulling()
        {
            throw new NotImplementedException();
        }

    }
}

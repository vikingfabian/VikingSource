using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class ImageGroup2D
    {
        public List<AbsDraw2D> images;

        public ImageGroup2D()
        {
            images = new List<AbsDraw2D>();
        }
        public ImageGroup2D(int count)
        {
            images = new List<AbsDraw2D>(count);
        }

        public ImageGroup2D(List<AbsDraw2D> images)
        {
            this.images = images;
        }

        public ImageGroup2D(AbsDraw2D image1, AbsDraw2D image2)
        {
            images = new List<AbsDraw2D> { image1, image2 };
        }

        public void Add(AbsDraw2D image)
        {
            images.Add(image);
        }

        public void Add(Graphics.RectangleLines image)
        {
            images.AddRange(image.lines);
        }

        public void Remove(AbsDraw2D image)
        {
            images.Remove(image);
        }

        public void DeleteAll()
        {
            foreach (var img in images)
            {
                img.DeleteMe();
            }
            images.Clear();
        }

        public void Hide()
        {
            SetVisible(false);
        }
        public void Show()
        {
            SetVisible(true);
        }

        public void SetVisible(bool visible)
        {
            foreach (var img in images)
            {
                img.Visible = visible;
            }
        }

        public void SetOpacity(float transparentsy)
        {
            foreach (var img in images)
            {
                img.Opacity = transparentsy;
            }
        }

        public void SetColor(Color color)
        {
            foreach (var img in images)
            {
                img.Color = color;
            }
        }

        public void SetSpriteName(SpriteName sprite)
        {
            foreach (var img in images)
            {
                Graphics.Image conv = img as Graphics.Image;
                if (conv != null)
                {
                    conv.SetSpriteName(sprite);
                }
            }
        }
    }
}

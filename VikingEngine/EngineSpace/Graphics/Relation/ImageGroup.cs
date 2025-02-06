using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars;

namespace VikingEngine.Graphics
{
    class ImageGroup
    {
        public List<AbsDraw> images;
        protected Vector2 posOffset = Vector2.Zero;

        public ImageGroup()
        {
            images = new List<AbsDraw>();
        }
        public ImageGroup(int count)
        {
            images = new List<AbsDraw>(count);
        }
        public ImageGroup(AbsDraw image1)
        {
            images = new List<AbsDraw> { image1 };
        }
        public ImageGroup(AbsDraw image1, AbsDraw image2)
        {
            images = new List<AbsDraw> { image1, image2 };
        }
        public ImageGroup(AbsDraw image1, AbsDraw image2, AbsDraw image3)
        {
            images = new List<AbsDraw> { image1, image2, image3 };
        }
        public ImageGroup(AbsDraw image1, AbsDraw image2, AbsDraw image3, AbsDraw image4)
        {
            images = new List<AbsDraw> { image1, image2, image3, image4 };
        }
        public ImageGroup(AbsDraw image1, AbsDraw image2, AbsDraw image3, AbsDraw image4, AbsDraw image5)
        {
            images = new List<AbsDraw> { image1, image2, image3, image4, image5 };
        }
        public ImageGroup(List<AbsDraw> images)
        {
            this.images = images;
        }

        public void Add(AbsDraw image)
        {
            images.Add(image);
        }

        public void Add(RectangleLines rect)
        {
            images.AddRange(rect.lines);
        }

        public void Add(ImageGroup group)
        {
            images.AddRange(group.images);
        }

        public void Add(HUD.NineSplitAreaTexture image)
        {
            images.AddRange(image.images);
        }

        public void Add(AbsSpriteText spriteText)
        {
            images.AddRange(spriteText.letters);
        }

        public void Add(AbsDraw[] imageArray)
        {
            images.AddRange(imageArray);
        }

        public void Add(List<AbsDraw> imageArray)
        {
            images.AddRange(imageArray);
        }

        public void Add(List<Image> imageArray)
        {
            images.AddRange(imageArray);
        }

        public void Remove(AbsDraw image)
        {
            images.Remove(image);
        }

        public void AddToRender(int layer)
        {
            foreach (var img in images)
            {
                img.AddToRender(layer);
            }
        }

        virtual public void DeleteAll()
        {
            posOffset = Vector2.Zero;

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

        public void ColorAndAlpha(Color color, float alpha)
        {
            foreach (var img in images)
            {
                img.ColorAndAlpha(color, alpha);
            }
        }

        public bool IsEmpty
        {
            get { return images.Count == 0; }
        }

        public bool HasMembers
        {
            get { return images.Count > 0; }
        }

        public void Move(Vector2 move)
        {
            posOffset += move;

            foreach (var m in images)
            {
                m.AddXY(move);
            }
        }

        public bool HasOffset()
        {
            return posOffset != Vector2.Zero;
        }

        virtual public void SetOffset(Vector2 position)
        {


            Vector2 move = position - posOffset;

            if (VectorExt.SideLength(move) >= 1f)
            {
                posOffset = position;

                foreach (var m in images)
                {
                    m.AddXY(move);
                }
            }
        }

        public Vector2 GetOffset()
        {
            return posOffset;
        }
    }
}

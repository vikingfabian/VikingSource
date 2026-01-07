using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class ImageGroupParent2D
    {
        Vector2 parentPos;
        public Vector2 ParentPosition
        {
            get { return parentPos; }
            set 
            { 
                parentPos = value;
                updateChildPositions();
            }
        }

        private void updateChildPositions()
        {
            foreach (ImageGroup2DMember img in images)
            {
                img.UpdatePosition(parentPos);
            }
        }

        public float ParentX
        {
            get { return parentPos.X; }
            set
            {
                parentPos.X = value;
                updateChildPositions();
            }
        }
        public float ParentY
        {
            get { return parentPos.Y; }
            set
            {
                parentPos.Y = value;
                updateChildPositions();
            }
        }

        public List<ImageGroup2DMember> images;
        public bool groupVisible = true;
        public float groupOpacity = 1f;
        
        public ImageGroupParent2D()
        {
            images = new List<ImageGroup2DMember>();
        }
        public ImageGroupParent2D(int capacity)
        {
            images = new List<ImageGroup2DMember>(capacity);
        }

        public ImageGroupParent2D(AbsDraw2D image1)
        {
            images = new List<ImageGroup2DMember>{ new ImageGroup2DMember(image1) };
        }
        public ImageGroupParent2D(AbsDraw2D image1, AbsDraw2D image2)
        {
            images = new List<ImageGroup2DMember> { new ImageGroup2DMember(image1), new ImageGroup2DMember(image2) };
        }
        public ImageGroupParent2D(AbsDraw2D image1, AbsDraw2D image2, AbsDraw2D image3)
        {
            images = new List<ImageGroup2DMember> { new ImageGroup2DMember(image1), new ImageGroup2DMember(image2), new ImageGroup2DMember(image3) };
        }

        public ImageGroup2DMember Add(AbsDraw2D image)
        {
            ImageGroup2DMember member = new ImageGroup2DMember(image);
            images.Add(member);
            return member;
        }

        public void Add(List<Image> list)
        {
            foreach (AbsDraw2D img in list)
            {
                images.Add(new ImageGroup2DMember(img));
            }
        }

        public void Add(AbsDraw2D image1, AbsDraw2D image2)
        {
            images.Add(new ImageGroup2DMember(image1));
            images.Add(new ImageGroup2DMember(image2));
        }

        public void Add(Graphics.RectangleLines rect)
        {
            foreach (var m in rect.lines)
            {
                images.Add(new ImageGroup2DMember(m));
            }
        }

        public void Add(ImageGroup imageGroup)
        {
            foreach (var m in imageGroup.images)
            {
                var img = m as AbsDraw2D;
                if (img != null)
                {
                    images.Add(new ImageGroup2DMember(img));
                }
            }
        }

        public void AddAndUpdate(AbsDraw2D image)
        {
            ImageGroup2DMember child = new ImageGroup2DMember(image);
            images.Add(child);
            child.UpdatePosition(parentPos);
        }

        public void Remove(AbsDraw2D image)
        {
            for (int i = 0; i < images.Count; ++i)
            {
                if (images[i].image == image)
                {
                    images.RemoveAt(i);
                    return;
                }
            }
        }

        public virtual void DeleteMe()
        {
            foreach (ImageGroup2DMember img in images)
            {
                img.image.DeleteMe();
            }
            images.Clear();
        }

        public bool IsDeleted { get { return images.Count == 0; } }

        public void Clear()
        {
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
            groupVisible = visible;

            foreach (ImageGroup2DMember img in images)
            {
                img.image.Visible = groupVisible;
            }
        }

        public void SetVisible_Quick(bool visible)
        {
            if (groupVisible != visible)
            {
                groupVisible = visible;

                foreach (ImageGroup2DMember img in images)
                {
                    img.image.Visible = groupVisible;
                }
            }
        }

        public void SetOpacity(float opacity)
        {
            groupOpacity = opacity;

            foreach (ImageGroup2DMember img in images)
            {
                img.image.Opacity = groupOpacity;
            }
        }

        public void AddOpacity(float add)
        {
            groupOpacity = Bound.Set(groupOpacity + add, 0, 1f);

            foreach (ImageGroup2DMember img in images)
            {
                img.image.Opacity = groupOpacity;
            }
        }

        public Image GetImage(int index)
        {
            return images[index].image as Image;
        }
        public TextG GetTextG(int index)
        {
            return images[index].image as TextG;
        }
        public bool Empty { get { return images == null || images.Count == 0; } }
    }

    class ImageGroup2DMember
    {
        public AbsDraw2D image;
        public Vector2 relativePostion;
    
        public ImageGroup2DMember(AbsDraw2D image)
        {
            this.image = image;
            relativePostion = image.Position;
        }

        public void UpdatePosition(Vector2 parentPos)
        {
            image.Position = relativePostion + parentPos;
        }

        public void SetScreenPosition(Vector2 screenPos, ImageGroupParent2D parent)
        {
            image.Position = screenPos;
            relativePostion = screenPos - parent.ParentPosition;
        }
    }
}

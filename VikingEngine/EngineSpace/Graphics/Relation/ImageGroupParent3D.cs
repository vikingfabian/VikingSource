using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;

namespace VikingEngine.Graphics
{
    class ImageGroupParent3D
    {
        Vector3 parentPos;
        public Vector3 ParentPosition
        {
            get { return parentPos; }
            set 
            { 
                parentPos = value;
                foreach (ImageGroup3DMember img in images)
                {
                    img.UpdatePosition(parentPos);
                }
            }
        }
        protected List<ImageGroup3DMember> images;

        public ImageGroupParent3D()
        {
            images = new List<ImageGroup3DMember>();
        }

        public ImageGroupParent3D(int capacity)
        {
            images = new List<ImageGroup3DMember>(capacity);
        }

        public ImageGroupParent3D(Point3D image1)
        {
            images = new List<ImageGroup3DMember>{ new ImageGroup3DMember(image1) };
        }
        public ImageGroupParent3D(Point3D image1, Point3D image2)
        {
            images = new List<ImageGroup3DMember> { new ImageGroup3DMember(image1), new ImageGroup3DMember(image2) };
        }
        public ImageGroupParent3D(Point3D image1, Point3D image2, Point3D image3)
        {
            images = new List<ImageGroup3DMember> { new ImageGroup3DMember(image1), new ImageGroup3DMember(image2), new ImageGroup3DMember(image3) };
        }

        public void Add(Point3D image)
        {
            images.Add(new ImageGroup3DMember(image));
        }
        public void AddAndUpdate(Point3D image)
        {
            var m = new ImageGroup3DMember(image);
            images.Add(m);
            m.UpdatePosition(parentPos);
        }
        public void Remove(Point3D image)
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

        public void DeleteAll()
        {
            foreach (ImageGroup3DMember img in images)
            {
                img.image.DeleteMe();
            }
            images.Clear();
        }
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

        public void SetPositionXZ(float x, float z)
        {
            parentPos.X = x;
            parentPos.Z = z;
            foreach (ImageGroup3DMember img in images)
            {
                img.UpdatePosition(parentPos);
            }
        }

        public bool VisibleGroup { get; private set; }
        public void SetVisible(bool visible)
        {
            VisibleGroup = visible;
            foreach (ImageGroup3DMember img in images)
            {
                img.image.Visible = visible;
            }
        }

        public void SetTransparentsy(float transparentsy)
        {
            foreach (ImageGroup3DMember img in images)
            {
                img.image.Opacity = transparentsy;
            }
        }

        public void AddAllToDraw()
        {
            foreach (ImageGroup3DMember img in images)
            {
                Ref.draw.AddToRenderList(img.image);
            }
        }

        public Point3D GetChild(int index)
        {
            return images[index].image;
        }

        public Mesh GetChildMesh(int index)
        {
            return images[index].image as Mesh;
        }

        public int Count => images.Count;
    }

    class ImageGroup3DMember
    {
        public Point3D image;
        public Vector3 relativePostion;
    
        public ImageGroup3DMember(Point3D image)
        {
            this.image = image;
            relativePostion = image.Position;
        }

        public void UpdatePosition(Vector3 parentPos)
        {
            image.Position = relativePostion + parentPos;
        }
    }
}

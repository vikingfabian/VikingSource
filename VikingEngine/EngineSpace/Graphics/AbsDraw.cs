using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;

namespace VikingEngine.Graphics
{
    abstract class AbsDraw : IDeleteable, IPosition, ISpottedArrayMember
    {
        /* Properties */
        public abstract DrawObjType DrawType { get; }

        public Color pureColor = Color.White;
        public abstract Color Color { get; set; }
        public abstract float Opacity { get; set; }

        abstract public void ColorAndAlpha(Color color, float alpha);

        public virtual bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        public virtual bool InRenderList { get { return inRenderList; } }
        //public virtual float GetPositionX { get { throw new NotImplementedException(); } }
        //public virtual float GetPositionZ { get { throw new NotImplementedException(); } }
        public bool IsDeleted { get { return !inRenderList; } }
        public int SpottedArrayMemberIndex
        {
            get { return spottedArrayMemberIndex; }
            set { spottedArrayMemberIndex = value; ++inSpottedArray; }
        }
        public virtual bool SpottedArrayUseIndex { get { return inSpottedArray <= 1; } }

        protected virtual bool drawable { get { return true; } }

        /* Fields */
        protected bool visible = true;
        protected bool inRenderList = false;
        public int inRenderLayer = 0;
        
        private int spottedArrayMemberIndex;
        private int inSpottedArray = 0;

        //public PixelShader pixelshader = PixelShader.Inverse;

        /* Constructors */
        public AbsDraw()
            :this(true)
        { }
        public AbsDraw(bool add)
        {
            if (add && drawable)
            {
                AddToRender();
            }
        }

        /* Methods */
        public abstract AbsDraw CloneMe();
        public abstract void Draw(int cameraIndex);
        public abstract void UpdateCulling();

        public virtual void DrawInDynamicCam(Vector2 camPos)
        { }

        public virtual void SetToSleep(bool sleep)
        {
            if (sleep == inRenderList)
            {
                bool add = !sleep;
                if (add)
                {
                    Ref.draw.AddToRenderList(this);
                }
                else
                {
                    Ref.draw.RemoveFromRenderList(this);
                }
                inRenderList = add;
            }
        }

        public void SetSpriteName(SpriteName name, int add)
        {
            this.SetSpriteName((SpriteName)((int)name + add));
        }

        virtual public void SetSpriteName(SpriteName name)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteMe()
        {
            if (drawable)
            {
                Ref.draw.RemoveFromRenderList(this);
                inRenderList = false;
            }
        }

        public void AddToRender()
        {
            Ref.draw.AddToRenderList(this);
            inRenderList = true;
        }

        public void AddToRender(int layer)
        {
            int storeLay = Ref.draw.CurrentRenderLayer;
            Ref.draw.CurrentRenderLayer = layer;
            AddToRender();
            Ref.draw.CurrentRenderLayer = storeLay;
        }

        virtual public void settingsChangedRefresh() { }

        public abstract void copyAllDataFrom(AbsDraw master);

        protected virtual void changedApperance()
        { }

        virtual public float PaintLayer
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        abstract public float PositionX { get; set; }
        abstract public float PositionY { get; set; }
        abstract public float PositionZ { get; set; }

        abstract public Vector2 PositionXY { get; set; }
        abstract public Vector2 PositionXZ { get; set; }
        abstract public Vector3 PositionXYZ { get; set; }

        abstract public void AddXY(Vector2 value);
    }

    public enum DrawObjType
    {
        Texture2D,
        Mesh,
        MeshGenerated,
        NUM,
        NotDrawable,
    }
}

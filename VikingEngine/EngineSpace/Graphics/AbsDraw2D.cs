using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Graphics
{
    abstract class AbsDraw2D : AbsDraw
    {
    //    abstract class AbsDraw2DSimple : AbsDraw
    //{
        protected static Vector2 drawScale = Vector2.One;
        protected static Vector2 drawOrigin = Vector2.One;
        protected static Sprite DrawSprite;
        
        protected static readonly Color ZeroAlpha = new Color(0, 0, 0, 0);
        
        protected float alpha = 1;
        protected float layer = 1f;

        protected static Rectangle destination = new Rectangle();

        public Vector2 origo = Vector2.Zero;
        public Vector2 position;
        public Vector2 size;
        protected float rotation = 0;

        public AbsDraw2D()
            : base()
        { }

        public AbsDraw2D(bool addToRender)
            : base(addToRender)
        { }
        //public AbsDraw2DSimple()
        //    : base()
        //{ }

        //public AbsDraw2DSimple(bool addToRender)
        //    : base(addToRender)
        //{ }

        virtual protected Color DrawColor()
        {
            
            if (alpha >= 1)
            {
                return pureColor;
            }
            else if (alpha <= 0)
            {
                return ZeroAlpha;
            }
            else
            {
                Vector3 col = pureColor.ToVector3() * alpha;
                return new Color(col.X, col.Y, col.Z, alpha);
            }
        }

        public override Color Color
        {
            get
            {
                return pureColor;
            }
            set
            {
                pureColor = value;
                alpha = value.Alpha();
            }
        }

        override public void ColorAndAlpha(Color color, float alpha)
        {
            pureColor = color;
            this.alpha = alpha;
        }

        override public float Opacity
        {
            get { return alpha; }
            set { alpha = MathHelper.Clamp(value, 0f, 1f); }
        }

        virtual public CamObjType Type
        {
            get { return CamObjType.NotSet; }
        } 

        public ImageLayers Layer
        {
            set { layer = GraphicsLib.ToPaintLayer(value); }
        }
        //override public float PaintLayer
        //{
        //    get { return layer; }
        //    set { layer = value; changedApperance(); }
        //}

        public void LayerAbove(AbsDraw2D aboveThis)
        {
            layer = aboveThis.layer - PublicConstants.LayerMinDiff;
        }
        public void LayerBelow(AbsDraw2D belowThis)
        {
            layer = belowThis.layer + PublicConstants.LayerMinDiff;
        }

        public override void UpdateCulling()
        {
            throw new NotImplementedException();
        }
    

    
        
        
        public void ChangePaintLayer(int layerAdd)
        {
            layer += layerAdd * PublicConstants.LayerMinDiff;
        }

        public virtual bool ViewArea(VectorRect area, bool dimOut)
        { //The object should fit itself to the area it can be viewed through.
            return false;
        }

        public override VikingEngine.Graphics.AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }

        public override void DrawInDynamicCam(Vector2 camPos)
        {
            Vector2 savePos = position;
            position.X -= camPos.X;
            position.Y -= camPos.Y;
            this.Draw(0);
            position = savePos;
        }

        public void OrigoAtCenter()
        {
            origo = VectorExt.V2Half;
            updateCenter();
        }

        public void OrigoAtCenterWidth()
        {
            origo.X = 0.5f;
            updateCenter();
        }
        public void OrigoAtCenterHeight()
        {
            origo.Y = 0.5f;
            updateCenter();
        }

        public void OrigoAtRight()
        {
            origo.X = 1f;
            updateCenter();
        }

        public void OrigoAtBottom()
        {
            origo.Y = 1f;
            updateCenter();
        }

        public void OrigoAtCenterBottom()
        {
            origo.X = 0.5f;
            origo.Y = 1f;
            updateCenter();
        }

        public void OrigoAtTopLeft()
        {
            origo = Vector2.Zero;
            updateCenter();
        }

        virtual public void updateCenter()
        { }
        
        public override void copyAllDataFrom(AbsDraw master)
        {
            AbsDraw2D c = (AbsDraw2D)master;
            c.layer = layer;
            c.Opacity = Opacity;
            c.pureColor = pureColor;
            c.Rotation = rotation;

            c.origo = origo;
            c.Position = position;
            c.Size = size;
        }

        virtual public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = MathExt.NormalizeAngle(value);                
                changedApperance();
            }
        }

        public float RotationDegrees
        {
            get
            {
                return MathHelper.ToDegrees(rotation);
            }
            set
            {
                rotation = MathExt.NormalizeAngle(MathHelper.ToRadians(value));
            }
        }
        virtual public float Width
        {
            get { return size.X; }
            set { size.X = value; changedApperance(); }
        }
        virtual public float Height
        {
            get { return size.Y; }
            set { size.Y = value; changedApperance(); }
        }
        virtual public float Xpos
        {
            get { return position.X; }
            set { position.X = value; changedApperance(); }
        }
        virtual public float Ypos
        {
            get { return position.Y; }
            set { position.Y = value; changedApperance(); }
        }
        virtual public float Right { get { return position.X + size.X; } }
        virtual public float Bottom { get { return position.Y + size.Y; } }

        /// <summary>
        /// Set left edge without affecting the right edge
        /// </summary>
        public void SetLeft(float left)
        {
            size.X += position.X - left;
            position.X = left;
        }

        public void SetRight(float right, bool adjustWidth)
        {
            if (adjustWidth)
            {
                size.X = right - position.X;
            }
            else
            {
                position.X = right - size.X;
            }
        }
        public void SetBottom(float bottom, bool adjustHeight)
        {
            if (adjustHeight)
            {
                size.Y = bottom - position.Y;
            }
            else
            {
                position.Y = bottom - size.Y;
            }
        }

        public override DrawObjType DrawType { get { return DrawObjType.Texture2D; } }
        public VectorRect Area
        {
            get { return new VectorRect(position, size); }
            set { Position = value.Position; Size = value.Size; }
        }
        public Dir4 Facing
        {
            get
            {
                float facing = (int)(rotation / MathHelper.PiOver2 + 0.49f);
                if (facing > 3) { facing -= 4; }
                return (Dir4)facing;
            }
            set
            { Rotation = (int)value * MathHelper.PiOver2; }
        }

        public Vector2 RightTop { get { return new Vector2(position.X + size.X, position.Y); } }
        public Vector2 LeftBottom { get { return new Vector2(position.X, position.Y + size.Y); } }
        public Vector2 BottomRight { get { return position + size; } }

        public Vector2 CenterTop { get { return new Vector2(position.X + size.X * 0.5f, position.Y); } }
        public Vector2 CenterBottom { get { return position + new Vector2(size.X * 0.5f, size.Y); } }
        public Vector2 LeftCenter { get { return new Vector2(position.X, position.Y + size.Y * 0.5f); } }
        public Vector2 RightCenter { get { return position + new Vector2(size.X, size.Y * 0.5f); } }

        public float RealLeft { get { return position.X - origo.X * size.X; } }
        public float RealTop { get { return position.Y - origo.Y * size.Y; } }

        public float RealRight { get { return RealLeft + size.X; } }
        public float RealBottom { get { return RealTop + size.Y; } }

        /// <summary>
        /// Will calc position with the origo in consideration
        /// </summary>
        public Vector2 RealTopLeft { get { return position - origo * size; } }
        /// <summary>
        /// Will calc position with the origo in consideration
        /// </summary>
        public Vector2 RealCenter { get { return RealTopLeft + size * 0.5f; } }

        public VectorRect RealArea()
        {
            return new VectorRect(position - origo * size, size);
        }

        virtual public Vector2 Position
        {
            get { return position; }
            set { position = value; changedApperance(); }
        }
        virtual public Vector2 Center
        {
            get { return position + size * PublicConstants.Half; }
            set { position = value - size * PublicConstants.Half; changedApperance(); }
        }
        virtual public Vector2 Size
        {
            get { return size; }
            set { size = value; changedApperance(); }
        }

        public Vector2 HalfSize
        {
            get { return size * 0.5f; }
        }

        public float Size1D
        {
            get { return size.X; }
            set { size.X = value; size.Y = value; }
        }

        override public float PaintLayer
        {
            get { return layer; }
            set { layer = value; changedApperance(); }
        }

        override public float PositionX { get { return position.X; } set { position.X = value; } }
        override public float PositionY { get { return position.Y; } set { position.Y = value; } }
        override public float PositionZ { get { return 0; } set { lib.DoNothing(); } }

        override public Vector2 PositionXY { get { return position; } set { position = value; } }
        override public Vector2 PositionXZ { get { return position; } set { position = value; } }
        override public Vector3 PositionXYZ { get { return new Vector3(position.X, position.Y, 0); } set { position.X = value.X; position.Y = value.Y; } }

        public override void AddXY(Vector2 value)
        {
            position += value;
        }
    }

    public enum CamObjType
    {
        NotSet,
        Point,
        Parent,
        Sprite,
        DocImage,
        DocText,
        TextSting,
        TextArea,
        TextBox,
    }
}

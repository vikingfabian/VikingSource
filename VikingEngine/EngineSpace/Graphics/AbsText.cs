using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsText : AbsDraw2D
    {
        static protected Vector2[] BorderRender8Directions;

        public static void Init()
        {
            BorderRender8Directions = new Vector2[IntVector2.Dir8Array.Length];
            for (int i = 0; i < IntVector2.Dir8Array.Length; ++i)
            {
                BorderRender8Directions[i] = IntVector2.Dir8Array[i].Vec;
            }
        }

        /* Properties */
        public void SetCenterRelative(Vector2 value)
        {   
            origo = value;
            updateCenter();
        }

        virtual public string TextString { get; set; }

        public LoadedFont Font;
        //{
        //    get;
        //    set;
        //}
        public Align Align
        {
            set { origo = value.Center; }
        }        

        /* Fields */
        protected Vector2 textDrawOrigin;
        //protected Vector2 centerRelative;

        public TextOutlineType outline = TextOutlineType.NoBorder;
        public float outlineThickness = 2;
        public Color outlineColor = Color.Black;

        /* Constructors */
        //public AbsText(Vector2 pos, Vector2 scale, Color color, ImageLayers layer)
        //    : this(pos, scale, color, layer, true)
        //{ }
        public AbsText(bool addToRender = true)
            : base(addToRender)
        {
        }

        public AbsText(Vector2 pos, Vector2 scale, Color color, ImageLayers layer, bool addToRender = true)
            : base(addToRender)
        {
            pureColor = color;
            Visible = true;
            this.Position = pos;
            this.Size = scale;
            Layer = layer;
        }

        /* Methods */
        abstract public Vector2 MeasureText();

        public float MeasureRightPos()
        {
            return position.X + MeasureText().X;
        }

        public float MeasureBottomPos()
        {
            return position.Y + MeasureText().Y;
        }

        public Vector2 MeasureRightBottom()
        {
            return position + MeasureText();
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);

            AbsText c = (AbsText)master;
            c.Font = this.Font;
            c.TextString = this.TextString;

            c.textDrawOrigin = this.textDrawOrigin;
            //c.CenterRelative = this.CenterRelative;
            c.outline = this.outline;
            c.outlineThickness = this.outlineThickness;
            c.outlineColor = this.outlineColor;
        }
      
        public void SetMaxWidth(float maxWidth)
        {
            Vector2 sz = MeasureText();
            if (sz.X > maxWidth)
            {
                float percentLarger = sz.X / maxWidth;
                this.Size /= percentLarger;
            }
        }

        /// <summary>
        /// replaces non-standard symbol and/or foreign character with '*' 
        /// </summary>
        public void CheckCharsSafety()
        {
            TextString = Engine.LoadContent.CheckCharsSafety(TextString, Font);
        }

        override public void updateCenter()
        {
            textDrawOrigin = origo * (MeasureText() / this.size);
        }

        public void SetHeight(float height)
        {
            this.size = HeightToScale(height, Font);

            //var spritefont = Engine.LoadContent.Font(this.Font);
            //this.size = Vector2.One;
            //this.size = new Vector2(height / (spritefont.LineSpacing - spritefont.Spacing));
        }

        public static Vector2 HeightToScale(float height, LoadedFont font)
        {
            var spritefont = Engine.LoadContent.Font(font);
            return new Vector2(height / (spritefont.LineSpacing - spritefont.Spacing));
        }

        public static float ScaleToHeight(float scale, LoadedFont font)
        {
            var spritefont = Engine.LoadContent.Font(font);
            return (spritefont.LineSpacing - spritefont.Spacing) * scale;
        }

        //public void CheckCharsSafety()
        //{
        //    TextString = Engine.LoadContent.CheckCharsSafety(TextString, this.Font);
        //}
    }

    enum TextOutlineType
    {
        NoBorder,
        Border8Dir,
        ShadowSouthEast,
        NUM
    }
}

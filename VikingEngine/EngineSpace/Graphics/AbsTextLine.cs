using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsTextLine: AbsText
    {
        /* Properties */
        public override CamObjType Type
        {
            get
            {
                return CamObjType.TextSting;
            }
        }
        public override DrawObjType DrawType
        {
            get
            {
                return DrawObjType.Texture2D;
            }
        }

        /* Constructors */
        public AbsTextLine(LoadedFont font, Vector2 pos, Vector2 scale,
            Color color, ImageLayers layer, bool addToRender)
            : base(pos, scale, color, layer, addToRender)
        {
            Font = font;
        }

        public AbsTextLine(LoadedFont font, Vector2 pos, Vector2 scale,
            Color color, ImageLayers layer)
            : this(font, pos, scale, color, layer, true)
        { }

        public AbsTextLine(bool addToRender = true)
            : base(addToRender)
        {
        }

        /* Methods */
        abstract public void AddChar(char c);

        public virtual void InitMe(LoadedFont font, Vector2 pos, Vector2 scale,
            Align objCenter, string text, Color color, ImageLayers layer)
        {
            pureColor = color;
            TextString = text;
            Visible = true;
            Font = font;
            this.Position = pos;
            this.Size = scale;
            Layer = layer;
            if (objCenter.Center != Vector2.Zero)
            { Centertext(objCenter); }
        }
        public override bool ViewArea(VectorRect area, bool dimOut)
        {
            const string TestString = "WH";
            float textH = Engine.LoadContent.MeasureString(TestString, Font, out _).Y * size.Y;

            bool inside = position.Y >= area.Y &&
                (position.Y + textH) <= area.Bottom;
            if (inside)
            {
                Opacity = 1;
            }
            else
            {
                float diff = 0;
                if (position.Y < area.Y)
                {//above
                    diff = area.Y - position.Y;
                }
                else
                {
                    diff = (position.Y + textH) - area.Bottom;
                }
                if (diff > Placements.DOC_FADE_AREA)
                {
                    Opacity = 0;
                }
                else
                {
                    Opacity = 1 - diff / Placements.DOC_FADE_AREA;
                }
            }
            return inside;

        }
        public override Vector2 MeasureText()
        {

            Vector2 result = Engine.LoadContent.MeasureString(TextString, Font, out var error) * this.Size;
            if (error)
            {
                TextString = "ERR";
            }
            return result;
        }

        public VectorRect GetArea()
        {
            return new VectorRect(position, MeasureText());
        }
        public Timer.TextFeed TextFeed(int startTime)
        {
            return new VikingEngine.Timer.TextFeed(startTime, this); 
        }
        public void ChangeStringAndCenter(string text, Align center, Engine.GameState state)
        {
            TextString = text;
            Centertext(center);
        }
        public void FitIntoArea(Vector2 area)
        {
            Vector2 sizeNow = MeasureText();
            float changeX = area.X / sizeNow.X;
            float changeY = area.Y / sizeNow.Y;

            if (changeY < changeX)
            { changeX = changeY; }
            this.size.X = size.X * changeX;
            this.size.Y = size.Y * changeY;

        }
        public void Centertext(Align objCenter)
        {
            origo = objCenter.Center;
        }
        public void InitTextBoxParent(LoadedFont font, Vector2 pos, Vector2 scale,
            string text, Color color, ImageLayers layer, float lineWidth)
        {
            pureColor = color;
            TextString = text;
            Visible = true;
            Font = font;
            this.Position = pos;
            this.Size = scale;
            Layer = layer;
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);
        }
        
        public override string ToString()
        {
            return "Text2D{" + TextString + "}";
        }
    }
}

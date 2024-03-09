using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    abstract class AbsRichBoxMember
    {
        public static List<AbsRichBoxMember> FromText(string text)
        {
            return new List<AbsRichBoxMember>
            {
                new RichBoxText(text)
            };
        }

        public static List<AbsRichBoxMember> FromIconText(SpriteName icon, string text)
        {
            if (icon == SpriteName.NO_IMAGE)
            {
                return new List<AbsRichBoxMember>
                {
                    new RichBoxText(text)
                };
            }
            else
            {
                return new List<AbsRichBoxMember>
                {
                    new RichBoxImage(icon),
                    new RichBoxText(text)
                };
            }
        }

        public static void PrepareNewRow(ref List<AbsRichBoxMember> members)
        {
            if (members == null)
            {
                members = new List<AbsRichBoxMember>();
            }

            if (members.Count > 0)
            {
                members.Add(new RichBoxNewLine());
            }
        }

        abstract public void Create(RichBoxGroup group);

        virtual public void finalizeArea(float width)
        { }

        virtual public void onEnter() { }
        virtual public void onClick() { }
    }

    class RichBoxNewLine : AbsRichBoxMember
    {
        bool newParagraph;
        float lineheight;

        public RichBoxNewLine(bool newParagraph = false, float lineheight = 1f)
        {
            this.newParagraph = newParagraph;
            this.lineheight = lineheight;
        }

        public override void Create(RichBoxGroup group)
        {
            group.newLine(newParagraph, lineheight);
        }
    }

    class RichBoxBeginTitle : AbsRichBoxMember
    {
        int level;
        public RichBoxBeginTitle(int level = 2)
        {
            this.level = level;
        }

        public override void Create(RichBoxGroup group)
        {
            group.prepTitle(level);
        }
    }

    class RichBoxText : AbsRichBoxMember
    {
        string text;
        public Text2 pointer;
        public Color? overrideColor;
        public LoadedFont? overrideFont;

        public RichBoxText(string text, Color? overrideColor = null, LoadedFont? overrideFont = null)
        {
            this.text = text;
            this.overrideColor = overrideColor;
            this.overrideFont = overrideFont;
        }

        public override void Create(RichBoxGroup group)
        {
            var format = group.Format();

            Color col = overrideColor == null ? format.Color : overrideColor.Value;
            LoadedFont font = overrideFont == null ? format.Font : overrideFont.Value;

            List<string> lines = new List<string>(4);
            TextLib.SplitToMultiLine2(text, font,
                AbsText.HeightToScale(format.size, format.Font).Y,
                group.boxWidth, group.position.X - group.topleft.X, lines);

            for (int i = 0; i < lines.Count; ++i)
            {
                Text2 textLine = new Text2(lines[i], format.Font, group.position,
                    format.size, col, group.layer, null, group.addToRender);
                textLine.OrigoAtCenterHeight();
                group.Add(textLine);

                if (i == 0)
                {
                    pointer = textLine;
                }

                if (arraylib.IsLast(i, lines))
                {
                    group.position.X = textLine.MeasureRightPos();
                }
                else
                {
                    group.position.X = group.topleft.X + group.boxWidth;
                    group.newLine();
                }
            }
        }
    }

    class RichboxIntDisplay : RichBoxText
    {
        IntGetSetIx property;
        int propertyIx;
        public RichboxIntDisplay(IntGetSetIx property, int propertyIx)
            : base(property(propertyIx,false, 0).ToString())
        { 
            this.property = property;
            this.propertyIx = propertyIx;
        }

        public void refresh()
        {
            pointer.TextString = property(propertyIx, false, 0).ToString();
        }
    }

    //class RichBoxSpace : AbsRichBoxMember
    //{
    //    float spaces;

    //    public RichBoxSpace(float spaces)
    //    {
    //        this.spaces = spaces;
    //    }

    //    public override void Create(RichBoxGroup group)
    //    {
    //        group.position.X += spaces * group.lineSpacing;
    //    }
    //}
    abstract class AbsRichBoxImage : AbsRichBoxMember
    {
        protected float scale;
        protected float addLeftSpace, addRightSpace;

        public AbsRichBoxImage(float scale, float addLeftSpace, float addRightSpace)
        {
            this.scale = scale;
            this.addLeftSpace = addLeftSpace;
            this.addRightSpace = addRightSpace;
        }

        public override void Create(RichBoxGroup group)
        {
            var rect = SourceRect();
            Vector2 boxsize = new Vector2(group.imageHeight / rect.Height * rect.Width, group.imageHeight);

            float spaceScaleFrom = group.imageHeight * scale;
            float addL = spaceScaleFrom * addLeftSpace;
            float addR = spaceScaleFrom * addRightSpace;

            Vector2 sz = boxsize * scale;
            float totalW = addL + sz.X + addR;

            if (group.RightEdgeSpace() < totalW)
            {
                group.newLine();
            }

            Vector2 center = VectorExt.AddX(group.position, totalW * 0.5f);
            var img = createImg(group, center, sz);

            group.Add(img);

            group.position.X += totalW;
        }

        abstract protected Image createImg(RichBoxGroup group, Vector2 center, Vector2 sz);

        abstract protected Rectangle SourceRect();
    }

    class RichBoxImage : AbsRichBoxImage
    {
        SpriteName sprite;
        public Color? color;
        public Image pointer;

        public RichBoxImage(SpriteName sprite, float scale = 1f, float addLeftSpace = 0, float addRightSpace = 0)
            : base(scale, addLeftSpace, addRightSpace)
        {
            this.sprite = sprite;
            //this.scale = scale;
            //this.addLeftSpace = addLeftSpace;
            //this.addRightSpace = addRightSpace;
        }

        override protected Image createImg(RichBoxGroup group, Vector2 center, Vector2 sz)
        {
            pointer = new Image(sprite, center, sz, group.layer, true, group.addToRender);
            if (color.HasValue)
            {
                pointer.Color = color.Value;
            }
            return pointer;
        }

        protected override Rectangle SourceRect()
        {
            return DataLib.SpriteCollection.Get(sprite).Source;
        }
    }

    class RichBoxTexture : AbsRichBoxImage
    {
        Texture2D tex;
        public ImageAdvanced pointer;

        public RichBoxTexture(Texture2D tex, float scale = 1f, float addLeftSpace = 0, float addRightSpace = 0)
            : base(scale, addLeftSpace, addRightSpace)
        {
            this.tex = tex;
        }

        override protected Image createImg(RichBoxGroup group, Vector2 center, Vector2 sz)
        {
            pointer = new ImageAdvanced(SpriteName.NO_IMAGE, center, sz, group.layer, true, group.addToRender);
            pointer.Texture = tex;
            pointer.SetFullTextureSource();
            return pointer;
        }

        protected override Rectangle SourceRect()
        {
            return new Rectangle(0, 0, tex.Width, tex.Height);
        }
    }

    class RichBoxSpace : AbsRichBoxMember
    {
        float spaces;

        public RichBoxSpace(float spaces = 1f)
        {
            this.spaces = spaces;
        }

        public override void Create(RichBoxGroup group)
        {
            group.position.X += spaces * group.imageHeight * 0.3f;
        }
    }

    class RichBoxTab : AbsRichBoxMember
    {
        float percX;

        public RichBoxTab(float percX = 0.5f)
        {
            this.percX = percX;
        }

        public override void Create(RichBoxGroup group)
        {
            float goalX = group.topleft.X + group.boxWidth * percX;
            if (group.position.X < goalX)
            {
                group.position.X = goalX;
            }
            else
            {
                group.position.X += 2;
            }
        }
    }

    class RichBoxSeperationLine : AbsRichBoxMember
    {
        public Image pointer;
        public RichBoxSeperationLine()
        { }

        public override void Create(RichBoxGroup group)
        {
            var pos = group.seperatingLinePlacement();

            pointer = new Image(SpriteName.WhiteArea, pos,
                new Vector2(group.boxWidth, 2), group.layer, false, group.addToRender);
            pointer.Opacity = 0.3f;
            group.Add(pointer);
        }

        public override void finalizeArea(float width)
        {
            pointer.Width = width;
        }
    }
}

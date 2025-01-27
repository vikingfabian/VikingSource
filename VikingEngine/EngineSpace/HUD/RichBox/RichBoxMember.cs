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
                new RbText(text)
            };
        }

        public static List<AbsRichBoxMember> FromIconText(SpriteName icon, string text)
        {
            if (icon == SpriteName.NO_IMAGE)
            {
                return new List<AbsRichBoxMember>
                {
                    new RbText(text)
                };
            }
            else
            {
                return new List<AbsRichBoxMember>
                {
                    new RbImage(icon),
                    new RbText(text)
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
                members.Add(new RbNewLine());
            }
        }

        abstract public void Create(RichBoxGroup group);

        virtual public void finalizeArea(float width)
        { }

        virtual public void onEnter(RichMenu.RichMenu menu) { }
        virtual public void onClick(RichMenu.RichMenu menu) { }

        virtual public void getButtons(List<AbsRbButton> buttons)
        { }

        virtual public Vector2 Position => throw new NotImplementedException();
        virtual public Vector2 Center => throw new NotImplementedException();
        virtual public Vector2 Size => throw new NotImplementedException();
    }

    class RbNewLine : AbsRichBoxMember
    {
        bool newParagraph;
        float lineheight;

        public RbNewLine(bool newParagraph = false, float lineheight = 1f)
        {
            this.newParagraph = newParagraph;
            this.lineheight = lineheight;
        }

        public override void Create(RichBoxGroup group)
        {
            group.newLine(newParagraph, lineheight);
        }
    }

    class RbBeginTitle : AbsRichBoxMember
    {
        int level;
        public RbBeginTitle(int level = 2)
        {
            this.level = level;
        }

        public override void Create(RichBoxGroup group)
        {
            group.prepTitle(level);
        }
    }

    class RbText : AbsRichBoxMember
    {
        string text;
        public Text2 pointer;
        public Color? overrideColor;
        public LoadedFont? overrideFont;

        public RbText(string text, Color? overrideColor = null, LoadedFont? overrideFont = null)
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

    

    class RbDisplay : RbText
    {
        IntGetSetIx property;
        int propertyIx;
        public RbDisplay(IntGetSetIx property, int propertyIx)
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

            Vector2 sz = boxsize * scale * group.groupScale;
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
        abstract public float Layer { get; }
        

       
    }

    class RbImage : AbsRichBoxImage
    {
        SpriteName sprite;
        public Color? color;
        public Image pointer;

        public RbImage(SpriteName sprite, float scale = 1f, float addLeftSpace = 0, float addRightSpace = 0)
            : base(scale, addLeftSpace, addRightSpace)
        {
            this.sprite = sprite;
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


        public override float Layer => pointer.PaintLayer;
        public override Vector2 Position => pointer.position - pointer.HalfSize;
        public override Vector2 Center => pointer.position;
        public override Vector2 Size => pointer.size;
    }

    class RbOverlapImage : AbsRichBoxImage
    {
        SpriteName sprite;
        public Color? color;
        public Image pointer;

        AbsRichBoxImage masterImage;
        Vector2 percOffset;

        public RbOverlapImage(AbsRichBoxImage masterImage, 
            SpriteName sprite, Vector2 percOffset, float scale = 1f)
            : base(scale, 0, 0)
        {
            this.masterImage = masterImage;
            this.percOffset = percOffset;
            this.sprite = sprite;
        }

        public override void Create(RichBoxGroup group)
        {
            masterImage.Create(group);
            //base.Create(group);
            pointer = createImg(group, masterImage.Center, masterImage.Size * scale);
        }

        override protected Image createImg(RichBoxGroup group, Vector2 center, Vector2 sz)
        {
            pointer = new Image(sprite, center, sz, group.layer, true, group.addToRender);
            pointer.position += masterImage.Size * percOffset;
            pointer.PaintLayer = masterImage.Layer - PublicConstants.LayerMinDiff;
            group.Add(pointer);

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

        public override float Layer => masterImage.Layer;
        public override Vector2 Center => masterImage.Center;
        public override Vector2 Size =>masterImage.Size;
    }

    class RbOverlapText : AbsRichBoxMember
    {
        string text;
        public Text2 pointer;
        public Color? overrideColor;
        public LoadedFont? overrideFont;
        AbsRichBoxMember overMember;
        Vector2 percPosition; 
        float scale;
        Vector2 origo;
        public RbOverlapText(AbsRichBoxMember overMember, string text, Vector2 percPosition, float scale, Vector2 origo, Color? overrideColor = null, LoadedFont? overrideFont = null)
        {
            this.origo = origo;
            this.percPosition = percPosition;
            this.scale = scale;
            this.overMember = overMember;
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
                Text2 textLine = new Text2(lines[i], format.Font, overMember.Position + percPosition * overMember.Size,
                    format.size * scale, col, group.layer -1, null, group.addToRender);
                textLine.SetCenterRelative(origo);
                //textLine.Align 
                //textLine.OrigoAtCenterHeight();
                group.Add(textLine);

                if (i == 0)
                {
                    pointer = textLine;
                }

                //if (arraylib.IsLast(i, lines))
                //{
                //    group.position.X = textLine.MeasureRightPos();
                //}
                //else
                //{
                //    group.position.X = group.topleft.X + group.boxWidth;
                //    group.newLine();
                //}
            }
        }
    }

    class RbTexture : AbsRichBoxImage
    {
        Texture2D tex;
        public ImageAdvanced pointer;

        public RbTexture(Texture2D tex, float scale = 1f, float addLeftSpace = 0, float addRightSpace = 0)
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

        public override float Layer =>  pointer.PaintLayer;
        public override Vector2 Center => pointer.position;
        public override Vector2 Size => pointer.size;
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

    class RichBoxScale : AbsRichBoxMember
    {
        float scale;

        public RichBoxScale(float scale = 1f)
        {
            this.scale = scale;
        }

        public override void Create(RichBoxGroup group)
        {
            group.setScale(scale);
        }
    }

    class RbTab : AbsRichBoxMember
    {
        float percX;

        public RbTab(float percX = 0.5f)
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

    class RbSeperationLine : AbsRichBoxMember
    {
        public Image pointer;
        public RbSeperationLine()
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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    class RichBoxGroup : ImageGroup
    {
        public VectorRect area;
        public VectorRect maxArea;
        
        public Vector2 topleft;
        public float boxWidth;
        public RichBoxSettings settings;

        public Vector2 position;
       
        public float imageHeight;
        public float lineSpacing, lineSpacingHalf;
        public ImageLayers layer;

        public float maxWidth;
        public bool addToRender;

        public int bTitelFormat = 0;
        public Stack<AbsRichBoxMember> parentMember = new Stack<AbsRichBoxMember>();
        public List<List<RichboxButton>> buttonGrid_Y_X = new List<List<RichboxButton>>();

        public RichBoxGroup(Vector2 topleft, float boxWidth, ImageLayers layer, 
            RichBoxSettings settings, List<AbsRichBoxMember> content,
            bool bRemoveDeadHeightSpace = true, 
            bool addToRender = true, 
            bool useDynamicWidth = true)
        {
            this.topleft = topleft;
            this.position = topleft;
            this.boxWidth = boxWidth;
            this.layer = layer;
            this.settings = settings;
            this.addToRender = addToRender;

            prepLine();
            if (bRemoveDeadHeightSpace)
            {
                removeDeadHeightSpace(true);
            }

            foreach (var m in content)
            {
                m.Create(this);
            }

            completeLine();

            if (buttonGrid_Y_X.Last().Count == 0)
            {
                arraylib.RemoveLast(buttonGrid_Y_X);
            }

            if (bRemoveDeadHeightSpace)
            {
                removeDeadHeightSpace(false);
            }

            area = new VectorRect(topleft, new Vector2(boxWidth, position.Y - topleft.Y));
            maxArea = area;
            maxArea.Width = maxWidth;

            finalizeArea(useDynamicWidth, content);
        }

        public TextFormat Format()
        {
            AbsRichBoxMember parent;
            if (parentMember.TryPeek(out parent))
            {
                var button = parent as RichboxButton;
                if (button != null)
                {
                    return button.enabled ? settings.button : settings.buttonDisabled;
                }
            }

            if (bTitelFormat == 0)
            {
                return settings.breadText;
            }
            else if (bTitelFormat == 1)
            {
                return settings.head1;
            }
            else
            {
                return settings.head2;
            }
        }

        void finalizeArea(bool useDynamicWidth, List<AbsRichBoxMember> members)
        {
            float width = useDynamicWidth ? maxWidth : boxWidth;

            foreach (var m in members)
            {
                m.finalizeArea(width);
            }
        }

        public void newLine(bool newParagraph, float lineheight)
        {
            completeLine();

            if (newParagraph)
            {
                position.Y += settings.breadIconHeight * 0.4f * lineheight;
            }

            prepLine();
        }

        public void newLine()
        {
            completeLine();

            prepLine();
        }

        void prepLine()
        {
            if (buttonGrid_Y_X.Count == 0 || buttonGrid_Y_X.Last().Count > 0)
            {
                buttonGrid_Y_X.Add(new List<RichboxButton>());
            }
            position.X = topleft.X;

            bTitelFormat = 0;
            setHeight(settings.breadIconHeight);

            position.Y += lineSpacingHalf;
        }

        public void prepTitle(int level)
        {
            position.Y -= lineSpacingHalf;

            //textFormat = settings.titleText;
            bTitelFormat = level;
            setHeight(settings.titleIconHeight);

            position.Y += lineSpacingHalf;
        }

        void setHeight(float imageHeight)
        {
            this.imageHeight = MathExt.Round(imageHeight);
            lineSpacing =  MathExt.RoundAndEven(imageHeight + Engine.Screen.IconSize * 0.12f);
            lineSpacingHalf = lineSpacing / 2;
        }

        void completeLine()
        {
            float width = position.X - topleft.X;

            maxWidth = lib.LargestValue(width, maxWidth);

            if (width > 0)
            {
                position.Y += lineSpacingHalf;
            }
            else
            {
                position.Y -= lineSpacingHalf;
            }
        }

        public Vector2 seperatingLinePlacement()
        {
            const float Space = 12;

            float moveY = Space + imageHeight;

            var storedPos = position;
            completeLine();
            prepLine();

            position.Y = storedPos.Y + moveY;

            Vector2 linePos = position;
            linePos.Y -= moveY / 2;
            return linePos;
        }

        void removeDeadHeightSpace(bool top)
        {
            if (top)
            {
                position.Y -= imageHeight * 0.1f;
            }
            else
            {
                position.Y -= imageHeight * 0.1f;
            }
        }

        public float RightEdgeSpace()
        {
            return (topleft.X + boxWidth) - position.X;
        }

        //float LineSpacing => imageHeight
    }

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
        public RichBoxBeginTitle(int level=2)
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
            :base(scale, addLeftSpace, addRightSpace)
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
                pointer.Color= color.Value;
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

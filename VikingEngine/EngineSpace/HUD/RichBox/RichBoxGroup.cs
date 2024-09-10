using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
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

        int tryCreatePosition = -1;
        bool lockNewLine = false;

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
            if (!lockNewLine)
            {
                completeLine();

                prepLine();
            }
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


        
        public void TryCreate_Start()
        { 
            addToRender = false;
            tryCreatePosition = images.Count;
            lockNewLine = true;
        }
        public void TryCreate_Complete()
        {
            addToRender = true;
            lockNewLine = false;

            for (int i = tryCreatePosition; i < images.Count; i++)
            {
                images[i].AddToRender();
            }
        }
        public void TryCreate_Undo()
        {
            while (images.Count > tryCreatePosition)
            { 
                images.RemoveAt(images.Count -1);
            }

            addToRender = true;
            lockNewLine = false;
        }
        //float LineSpacing => imageHeight
    }

   
}

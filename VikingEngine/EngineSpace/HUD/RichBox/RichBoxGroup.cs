using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using VikingEngine.EngineSpace.HUD.RichBox;
using VikingEngine.Graphics;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.HUD.RichBox
{
    /// <summary>
    /// Track placement of next item
    /// </summary>
    struct RichBoxCarriage
    {
        public Vector2 position;
        public int lineCount;
    }

    class RichBoxGroup : ImageGroup
    {
        static int NextId = 1;
        public int PageId = NextId++;

        public VectorRect area;
        public VectorRect maxArea;
        
        public Vector2 topleft;
        public float boxWidth;
        public RichBoxSettings settings;

        //public Vector2 position;
        public RichBoxCarriage carriage;
        public RichBoxCarriage storedCarriage;

        public float imageHeight;
        public float lineSpacing, lineSpacingHalf;
        public ImageLayers layer;

        public float maxWidth;
        public bool addToRender;

        public int bTitelFormat = 0;
        public Stack<AbsRichBoxMember> parentMember = new Stack<AbsRichBoxMember>();
        public List<List<AbsRbButton>> buttonGrid_Y_X = new List<List<AbsRbButton>>();
        public List<ControllerSection> controllerSections = new List<ControllerSection>();

        int tryCreatePosition = -1;
        bool lockNewLine = false;
        //public int lineCount = 0;
        public float groupScale = 1f;

        public override void SetOffset(Vector2 position)
        {
            //Debug.Log($"Richbox set offset {position}");
            base.SetOffset(position);

        }

        public RichBoxGroup(Vector2 topleft, float boxWidth, ImageLayers layer, 
            RichBoxSettings settings, List<AbsRichBoxMember> content,
            bool bRemoveDeadHeightSpace = true, 
            bool addToRender = true, 
            bool useDynamicWidth = true)
        {
            this.topleft = topleft;
            carriage.position = topleft;
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

            area = new VectorRect(topleft, new Vector2(boxWidth, carriage.position.Y - topleft.Y));
            maxArea = area;
            maxArea.Width = maxWidth;

            finalizeArea(useDynamicWidth, content);
        }

        public void setScale(float newScale)
        {
            lineSpacing = lineSpacing / groupScale * newScale;
            lineSpacingHalf = lineSpacing / 2;
            groupScale = newScale;
        }

        public TextFormat Format()
        {
            AbsRichBoxMember parent;
            if (parentMember.TryPeek(out parent))
            {
                var button = parent as AbsRbButton;
                if (button != null && button.UseButtonContentSettings())
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
                carriage.position.Y += settings.breadIconHeight * 0.4f * lineheight;
            }

            prepLine();
        }

        public void newLine_SetHeight(float height)
        {
            completeLine();

            carriage.position.Y = height;
           
            prepLine();
        }

        public void storeCarriage()
        {
            storedCarriage = carriage;
        }
        public void restoreCarriage()
        {
            carriage = storedCarriage;
        }

        public void newLine()
        {
            if (!lockNewLine)
            {
                completeLine();

                prepLine();
            }
        }

        public bool LeftCarriage => carriage.position.X == topleft.X;

        void prepLine()
        {
            if (buttonGrid_Y_X.Count == 0 || buttonGrid_Y_X.Last().Count > 0)
            {
                buttonGrid_Y_X.Add(new List<AbsRbButton>());
            }
            carriage.position.X = topleft.X;

            bTitelFormat = 0;
            setHeight(settings.breadIconHeight);

            carriage.position.Y += lineSpacingHalf;

            if (parentMember.TryPeek(out var parent))
            {
                parent.Parent_OnNewLine(this);
            }
        }

        public void prepTitle(int level)
        {
            carriage.position.Y -= lineSpacingHalf;

            //textFormat = settings.titleText;
            bTitelFormat = level;
            setHeight(settings.titleIconHeight);

            carriage.position.Y += lineSpacingHalf;
        }

        void setHeight(float imageHeight)
        {
            this.imageHeight = MathExt.Round(imageHeight);
            lineSpacing =  MathExt.RoundAndEven(imageHeight + Engine.Screen.IconSize * 0.12f) * groupScale;
            lineSpacingHalf = lineSpacing / 2;
        }

        void completeLine()
        {
            float width = carriage.position.X - topleft.X;

            maxWidth = lib.LargestValue(width, maxWidth);

            if (width > 0)
            {
                carriage.position.Y += lineSpacingHalf;
            }
            else
            {
                carriage.position.Y -= lineSpacingHalf;
            }

            carriage.lineCount++;
        }

        public Vector2 seperatingLinePlacement()
        {
            const float Space = 12;

            float moveY = Space + imageHeight;

            var storedPos = carriage.position;
            completeLine();
            prepLine();

            carriage.position.Y = storedPos.Y + moveY;

            Vector2 linePos = carriage.position;
            linePos.Y -= moveY / 2;
            return linePos;
        }

        void removeDeadHeightSpace(bool top)
        {
            if (top)
            {
                carriage.position.Y -= imageHeight * 0.1f;
            }
            else
            {
                carriage.position.Y -= imageHeight * 0.1f;
            }
        }

        public float RightEdgeSpace()
        {
            return (topleft.X + boxWidth) - carriage.position.X;
        }
        
        public void TryCreate_Start()
        { 
            addToRender = false;
            tryCreatePosition = images.Count;
            //lockNewLine = true;
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
            restoreCarriage();
            while (images.Count > tryCreatePosition)
            { 
                images.RemoveAt(images.Count -1);
            }

            addToRender = true;
            lockNewLine = false;
        }

        public VectorRect AreaWithPosOffset()
        {
            VectorRect result = area;
            result.Position += posOffset;

            return result;
        }

        public VectorRect MaxAreaWithPosOffset()
        {
            VectorRect result = maxArea;
            result.Position += posOffset;

            return result;
        }
    }

   
}

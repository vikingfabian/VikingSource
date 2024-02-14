//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.Commander
//{
//    /// <summary>
//    /// Menu that uses imageGroups as buttons to scroll through and select
//    /// </summary>
//    class CardMenu
//    {
//        public int selectedIndex = 0;
//        List<CardMenuButton> page = null;
//        Input.AbsControllerInstance controller;
//        VectorRect safeScreenArea;
//        Vector2 centerPos;
//        float ypos, goalYpos;
//        Graphics.Image selection, okButton, moveButton;
//        ImageLayers layer;
//        Graphics.ImageGroup images;

//        public CardMenu(int pIx, VectorRect safeScreenArea, ImageLayers layer)
//        {
//            controller = Input.Controller.Instance(pIx);
//            this.safeScreenArea = safeScreenArea;
//            centerPos = safeScreenArea.Center;
//            this.layer = layer;
//        }

//        /// <returns>Button click</returns>
//        public bool Update()
//        {
//            if (page != null)
//            {
//                //Imput
//                if (controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.A, Microsoft.Xna.Framework.Input.Buttons.X))
//                {
//                    return true;
//                }

//                int move = controller.JoyStickValue(Stick.Left).Stepping.Y;
//                if (move != 0)
//                {
//                    selectedIndex = Bound.SetBoundsRollover(selectedIndex + move, 0, page.Count - 1);

//                    updateGoalY();
//                }

//                //Move
//                float diff = goalYpos - ypos;
//                if (Math.Abs( diff) < 0.5f)
//                {
//                    ypos = goalYpos;
//                }
//                else
//                {
//                    ypos += diff * 0.2f;
//                }

//                float buttonY = centerPos.Y - ypos;
//                for (int i = 0; i < page.Count; ++i)
//                {
//                    page[i].UpdatePosition(ref buttonY, centerPos);
//                    if (i == selectedIndex)
//                    {
//                        selection.Position = page[i].ParentPosition;
//                        selection.Size = page[i].Size;
//                        okButton.Position = selection.BottomRight;
//                    }
//                }
//            }
//            return false;
//        }

//        private void updateGoalY()
//        {
//            goalYpos = 0;

//            for (int i = 0; i < page.Count; ++i)
//            {
//                if (i == selectedIndex)
//                {
//                    goalYpos += page[i].Size.Y * 0.5f;
//                    break;
//                }
//                else
//                {
//                    goalYpos += page[i].Size.Y;
//                }
//            }
//        }


//        public void NewPage(List<CardMenuButton> page)
//        {
            
//            if (this.page != null)
//            {
//                foreach (CardMenuButton b in this.page)
//                {
//                    b.DeleteMe();
//                }
//            }
//            this.page = page;
            

//            if (selection == null)
//            {
//                Vector2 buttonSz = new Vector2(Engine.Screen.IconSize * 1.6f); 

//                selection = new Graphics.Image(SpriteName.BoardTxtMenuSelRect, Vector2.Zero, Vector2.Zero, layer - 2);
//                okButton = new Graphics.Image(SpriteName.ButtonA, Vector2.Zero, buttonSz, layer - 3, true);
//                moveButton = new Graphics.Image(SpriteName.LeftStick_UD, centerPos, buttonSz, layer, true);

//                images = new Graphics.ImageGroup(selection, okButton, moveButton);
//            }

//            updateWidth();
//            selectedIndex = 0;
//            updateGoalY();
//        }

//        float menuWidth;
//        void updateWidth()
//        {
//            menuWidth = 0;
//            foreach (CardMenuButton b in this.page)
//            {
//                menuWidth = lib.LargestOfTwoValues(menuWidth, b.Size.X);
//            }

//            moveButton.Xpos = centerPos.X - menuWidth * 0.5f - moveButton.Width;
//        }

//        public void DeleteMe()
//        {
//            foreach (CardMenuButton b in this.page)
//            {
//                b.DeleteMe();
//            }

//            images.DeleteAll();
//        }
//    }

//    class CardMenuButton : Graphics.ImageGroupParent2D
//    {
//        Vector2 size;
//        public float slidePos = 0;

//        public CardMenuButton(Vector2 size, ImageLayers layer)
//        {
//            this.size = size;
//        }

//        public void UpdatePosition(ref float y, Vector2 center)
//        {
//            Vector2 pos = Vector2.Zero;
//            pos.Y = y;
//            y += size.Y;

//            float centerDiff = center.Y - (pos.Y + size.Y * 0.5f);
//            if (Math.Abs(centerDiff) < size.Y * 0.5f)
//            {
//                //In center, should slide out a bit
//                const float SlideLength = 20f;
//                float slideDiff = SlideLength - slidePos;
//                if (slideDiff < 1f)
//                {
//                    slidePos = SlideLength;
//                }
//                else
//                {
//                    slidePos += 0.2f * slideDiff;
//                }
//            }
//            else
//            { 
//                //Slide back
//                if (slidePos > 1f)
//                {
//                    slidePos -= 0.25f * slidePos;
//                }
//                else
//                {
//                    slidePos = 0f;
//                }
//            }

//            pos.X = center.X - size.X * 0.5f + slidePos;
//            this.ParentPosition = pos;
//        }


//        public Vector2 Size { get { return size; } }

//        public override string ToString()
//        {
//            return "CardMenu Button (" + images.Count.ToString() + ")";
//        }
//    }
//}

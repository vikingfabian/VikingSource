//#if CCG

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.HUD
{
    /// <summary>
    /// A popup message that waits for player input
    /// </summary>
    class DialogueBox
    {
        protected Graphics.ImageGroup images;
        protected Graphics.Image background;
        protected Graphics.TextBoxSimple textBox;
        DialougeBoxOption[] options;
        OptionGraphics[] optionGraphics;
        public Time blockInputTime = 0;
        protected ImageLayers contertlayer;
        int selectedIndex = -1;
        Graphics.Image selectionSquare;

        public DialogueBox(string title, string text, DialougeBoxOption[] options, VectorRect area, ImageLayers layer)
        {
            contertlayer = layer - 1;
            this.options = options;

            textBox = new Graphics.TextBoxSimple(LoadedFont.Regular, Vector2.Zero, new Vector2(Engine.Screen.TextSize),
                Graphics.Align.Zero, text, Color.White, contertlayer, area.Width * 0.5f);

            Vector2 boxSz = textBox.MeasureText();
            textBox.Position = area.Center - boxSz * 0.5f;

            Graphics.TextG titleText = new Graphics.TextG(LoadedFont.Regular, textBox.Position, new Vector2(Engine.Screen.TextSize * 1.3f),
                Graphics.Align.Zero, title, Color.LightBlue, contertlayer);
            Vector2 titleSz = titleText.MeasureText();
            titleText.Ypos -= titleSz.Y;
            if (titleSz.X > boxSz.X)
            {
                boxSz.X = titleSz.X;
            }

            Vector2 iconSz = new Vector2(Engine.Screen.IconSize);
            Vector2 optTextScale = new Vector2(Engine.Screen.TextSize);
            Vector2 position = textBox.Position;
            position.Y += boxSz.Y;

            images = new Graphics.ImageGroup(new List<Graphics.AbsDraw> 
            {
                titleText, textBox, 
            });

            optionGraphics = new OptionGraphics[options.Length];
            VectorRect selectarea = new VectorRect(Vector2.Zero, new Vector2(boxSz.X, iconSz.Y));
            
            for (int i = 0; i < options.Length; ++i )//each (DialougeBoxOption opt in options)
            {
                OptionGraphics graphics = new OptionGraphics();
                selectarea.Position = position;
                graphics.area = selectarea;
#if CCG
                graphics.icon = new Graphics.Image(((Input.Alternative5ButtonsMap)options[i].button).GetFromSource(CCG.ccgRef.inputMap.activeSource).Icon, 
                    position, iconSz, contertlayer);
                images.Add(graphics.icon);

#endif
                graphics.optText = new Graphics.TextG(LoadedFont.Regular, graphics.icon.RightCenter, optTextScale, Graphics.Align.CenterHeight,
                    options[i].text, Color.White, contertlayer);
                images.Add(graphics.optText);

                position.Y += iconSz.Y * 1.1f;

                optionGraphics[i] = graphics;
            }
         
            VectorRect bgarea = new VectorRect(titleText.Position, new Vector2(boxSz.X, position.Y - titleText.Ypos));
            bgarea.AddRadius(6f);
            background = new Graphics.Image(SpriteName.WhiteArea, bgarea.Position, bgarea.Size, layer);
            background.Color = Color.Black;
            background.Opacity = 0.7f;
            images.Add(background);

            selectionSquare = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, selectarea.Size, ImageLayers.AbsoluteBottomLayer);
            selectionSquare.LayerAbove(background);
            selectionSquare.Opacity = 0.5f;
            images.Add(selectionSquare);

            refreshSelection();
        }

        void refreshSelection()
        {
            if (selectedIndex >= 0)
            {

                for (int i = 0; i < optionGraphics.Length; ++i)
                {
                    if (i == selectedIndex)
                    {
                        selectionSquare.Position = optionGraphics[selectedIndex].area.Position;
                        optionGraphics[i].optText.Color = Color.Black;
                    }
                    else
                    {
                        optionGraphics[i].optText.Color = Color.White;
                    }
                }
                selectionSquare.Visible = true;
            }
            else
            {
                selectionSquare.Visible = false;
            }
        }


        virtual public DialogueResult Update()//Input.AbsControllerInstance controller)
        {
            if (blockInputTime.CountDown())
            {
                if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    if (selectedIndex >= 0)
                    {
                        if (options[selectedIndex].action != null)
                            options[selectedIndex].action();

                        return options[selectedIndex].result;
                    }
                }

                foreach (DialougeBoxOption opt in options)
                {
                    if (opt.button.DownEvent)//controller.KeyDownEvent(opt.button))
                    {
                        if (opt.action != null)
                            opt.action();

                        return opt.result;
                    }
                }

                if (Input.Mouse.bMoveInput)
                {
                    for (int i = 0; i < optionGraphics.Length; ++i)
                    {
                        if (optionGraphics[i].area.IntersectPoint(Input.Mouse.Position))
                        {
                            selectedIndex = i;
                            refreshSelection();
                            break;
                        }
                    }
                }
            }
            return DialogueResult.NoResult;
        }

        public void DeleteMe()
        {
            images.DeleteAll();
        }

        class OptionGraphics
        {
            public Graphics.Image icon;
            public Graphics.TextG optText;
            public VectorRect area;

        }
    }

    

    struct DialougeBoxOption
    {
        public string text;
        public Action action;
        //public SpriteName icon;
        public VikingEngine.Input.IButtonMap button;
        //public Microsoft.Xna.Framework.Input.Buttons button;
        public DialogueResult result;
        //public VectorRect area;

        public DialougeBoxOption(VikingEngine.Input.IButtonMap button, string text, DialogueResult result, Action action)
        {
            this.button = button;
            //this.area = area;
            //this.icon = icon;
            this.text = text;
            this.result = result;
            this.action = action;
        }
    }

    enum DialogueResult
    {
        NoResult,
        Yes,
        No,
        TimeOut,
    }
}
//#endif
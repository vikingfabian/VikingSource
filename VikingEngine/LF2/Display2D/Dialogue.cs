using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    interface IDialogueCallback
    {
        void DialogueClosedEvent();
    }

    class Dialogue : AbsUpdateable
    {
        DialogueMode mode = (DialogueMode)0;
        HUD.DialogueData data;
        const float EdgeSize = 3;
        const float TextInterpose = 20;

        const float Scale = 0.8f;
        static readonly Vector2 FontSize = new Vector2(Scale);
        const float LineHeight = 74 * Scale;
        const float Height = 40 + 2 * LineHeight;

        const float OpeningTime = 400;
        static readonly LoadedFont Font = LoadedFont.TimesS;

        static readonly Color EdgeCol = new Color(103, 157, 255);

        int pageIx = 0;

        int pagePartIx;
        int pageLineIx = 0;
        int showLineIx = 0;
        int letterIx = 0;
        List<string> currentPage;
        List<Graphics.TextG> pageText;
        Graphics.TextG title;
        VectorRect area;
        const int MaxLines = 2;
        Graphics.Image nameBakg;
        Graphics.Image bakgEdge;
        Graphics.Image bakg;
        Graphics.Image button;
        IDialogueCallback callbackObj;

        public Dialogue(HUD.DialogueData data, VectorRect screenArea, IDialogueCallback callbackObj)
            : base(true)
        {
            this.callbackObj = callbackObj;
            this.data = data;

            screenArea.AddToTopSide(-screenArea.Height + Height);
            area = screenArea;
            bakgEdge = new Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Foreground4);
            bakgEdge.Color = EdgeCol;
            area.AddRadius(-EdgeSize);
            bakg = new Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Foreground3);
            
            const float ButtonSz = 32;
            button = new Image(SpriteName.ButtonA, area.BottomRight - Vector2.One * (ButtonSz), Vector2.One * ButtonSz, ImageLayers.Foreground1, true);
            button.Visible = false;
            new Graphics.Motion2d(MotionType.SCALE, button, Vector2.One * 6, MotionRepeate.BackNForwardLoop, 260, true);


            const float TitleBakgHeight = 40;
            const float TitleEdge = 6;
            Vector2 titlePos = new Vector2(area.X + 40, screenArea.Y - TitleBakgHeight);
            title = new TextG(LoadedFont.Lootfest, titlePos + new Vector2(TitleEdge, 0), new Vector2(1), Align.Zero, data.SpeakerName, Color.White, ImageLayers.Foreground2);
            nameBakg = new Image(SpriteName.WhiteArea, titlePos, new Vector2(title.MesureText().X + TitleEdge * PublicConstants.Twice, TitleBakgHeight), ImageLayers.Foreground3);
            nameBakg.Color = EdgeCol;
            if (data.SpeakerName == TextLib.EmptyString)
            {
                title.Visible = false;
                nameBakg.Visible = false;
            }
            nextPage();
        }
        public void Click()
        {
            button.Visible = false;
            if (mode == DialogueMode.Writing)
            {
                //show all text
                int textIx = 0;
                for (int i = pagePartIx * MaxLines; i < currentPage.Count && i < (pagePartIx + 1) * MaxLines; i++)
                {
                    pageText[textIx].TextString = currentPage[i];
                    textIx++;
                }

                mode++;
                button.Visible = true;
            }
            else if (pageLineIx + 1 < currentPage.Count)
            {
                //clear text and keep writing the same page
                clearText();
                showLineIx = -1;
                newLine();
                mode = DialogueMode.Writing;
            }
            else
            {
                pageIx++;
                nextPage();
            }
        }
        void nextPage()
        {
            pagePartIx = 0;

            if (data.Pages.Count > pageIx)
            {
                showLineIx = -1;
                pageLineIx = -1;
                mode = DialogueMode.Writing;
                clearText();
                pageText = new List<TextG>();
                //pageText = new List<TextG>();
                //will only show 2 lines at a time

                currentPage = TextLib.SplitToMultiLine(data.Pages[pageIx], Font, FontSize.X,
                    area.Width - TextInterpose * PublicConstants.Twice);

                newLine();
            }
            else
            {
                //no more pages, close dialouge
                DeleteMe();
                if (callbackObj != null)
                    callbackObj.DialogueClosedEvent();
            }
        }
        void newLine()
        {
            if (pageLineIx + 1 < currentPage.Count && showLineIx + 1 < MaxLines)
            {
                showLineIx++;
                pageLineIx++;

                letterIx = 0;
                pageText.Add(new TextG(Font, new Vector2(area.X + TextInterpose, area.Y + TextInterpose + showLineIx * LineHeight),
                    FontSize, Align.Zero, TextLib.EmptyString, Color.Black, ImageLayers.Foreground2));
            }
            else
            {//done!
                pagePartIx++;
                mode++;
                button.Visible = true;
            }
        }
        public override void Time_Update(float time)
        {
            switch (mode)
            {
                case DialogueMode.Writing:
                    //The game crashes here
                    if (currentPage.Count > pageLineIx &&
                        letterIx < currentPage[pageLineIx].Length &&
                        pageText.Count > showLineIx)
                    {
                        pageText[showLineIx].AddChar(currentPage[pageLineIx][letterIx]);
                        letterIx++;
                    }
                    else
                    {
                        //line done
                        newLine();
                    }
                    break;
            }
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            nameBakg.DeleteMe();
            title.DeleteMe();
            bakg.DeleteMe();
            bakgEdge.DeleteMe();
            clearText();
            button.DeleteMe();
        }
        void clearText()
        {
            if (pageText != null)
            {
                foreach (Graphics.TextG text in pageText)
                {
                    text.DeleteMe();
                }
                pageText.Clear();
            }

        }
    }
    enum DialogueMode
    {
        Writing,
        Waiting,
        NUM
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    

    //class Menu : HUD.AbsMenu, IDialogueCallback
    //{
    //    Dialogue dialogue = null;
    //    VectorRect dialogueArea;
    //    MenuDescription descriptionTimer = null;
    //    static readonly Color ScrollerBarCol = Color.Gray;//new Color(223, 143, 71);
    //    static readonly Color ScrollerBackgCol = Color.Black;//new Color(253, 230, 80);
    //    public const float Scale = 1.6f;
    //    const float Edge = 9 * Scale;
    //    List<Image> background;
    //    static readonly Vector2 DescWarningPos = new Vector2(LoadTiles.MenuRectangleWidth * Scale * 0.2f, 
    //        LoadTiles.MenuRectangleHeight * Scale * 0.76f);
    //    static readonly Vector2 DescPos = new Vector2(LoadTiles.MenuRectangleWidth * Scale * 0.1f,
    //        LoadTiles.MenuRectangleHeight * Scale * 0.9f);

    //    /// <summary>
    //    /// The description fits on the right side of the menu
    //    /// </summary>
    //    bool seperateDesc;
    //    const float SeperateDescXSpace = 20;

    //    public Menu(VectorRect area, VectorRect safeScreenArea, ImageLayers layer, int playerIndex)
    //        : base(area, layer, playerIndex)
    //    {
    //        this.dialogueArea = safeScreenArea;
    //        seperateDesc = safeScreenArea.Right - area.Right >= MenuDescription.Width + SeperateDescXSpace;
    //    }

    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        foreach (Image img in background)
    //        {
    //            img.DeleteMe();
    //        }
    //        background = null;
    //        ShowScroller(false);
    //        HideDesciption();
    //        if (dialogue != null)
    //        {
    //            dialogue.DeleteMe();
    //        }
           
    //    }

    //    public static SpriteName RectangleButtonEnable(bool enable)
    //    {
    //        return enable ? SpriteName.NO_IMAGE : SpriteName.LFMenuRectangleGray;
    //    }
    //    //override public bool InSubMenu
    //    //{
    //    //    get { return optionWheel != null || document != null; }
    //    //}
    //    public override bool InSubMenu
    //    {
    //        get
    //        {
    //            return base.InSubMenu || dialogue != null; 
    //        }
    //    }
        
    //    override protected SpriteName selectionRectangle
    //    {
    //        get { return SpriteName.LFMenuRectangleSelection; }
    //    }
    //    override protected SpriteName selectionSquare
    //    {
    //        get { return SpriteName.LFMenuSquareSelection; }
    //    }
    //    protected override SpriteName pointerTile
    //    {
    //        get
    //        {
    //            return SpriteName.LFMenuPointer;
    //        }
    //    }
    //    override public SpriteName RectangleBackg
    //    {
    //        get { return SpriteName.LFMenuRectangle; }
    //    }
    //    override public SpriteName SquareBackg
    //    {
    //        get { return SpriteName.LFMenuSquare; }
    //    }
    //    override public SpriteName TitleBackg
    //    {
    //        get { return SpriteName.LFMenuTitleRectangle; }
    //    }
        
    //    public static readonly float MenuBlockHeight = LoadTiles.MenuRectangleHeight * Scale;
    //    override public float BlockHeight
    //    { get { return MenuBlockHeight; } }

    //    public override HUD.File File
    //    {
    //        set
    //        {
    //            base.File = value;
    //        }
    //    }
    //    public override bool Visible
    //    {
    //        get
    //        {
    //            return base.Visible;
    //        }
    //        set
    //        {
    //            base.Visible = value;
                
    //            foreach (Image img in background)
    //            {
    //                img.Visible = value;
    //            }
    //        }
    //    }

    //    protected override void OnNewSelection(bool firstUpdate)
    //    {
    //        if (!firstUpdate)
    //            Music.SoundManager.PlayFlatSound(LoadedSound.MenuMove);
    //    }
         
    //    public override void MoveSelection(JoyStickValue e)
    //    {
    //        if (dialogue == null)
    //        {
    //            base.MoveSelection(e);
    //        }
    //    }
    //    protected override void SelectionMovedEvent()
    //    {
    //        Music.SoundManager.PlayFlatSound(LoadedSound.MenuMove);
    //    }
    //    public override void  Click(int pIx, numBUTTON button)
    //    {
 	 
    //        Music.SoundManager.PlayFlatSound(LoadedSound.MenuSelect);
    //        if (dialogue == null)
    //            base.Click(pIx, button);
    //        else
    //            dialogue.Click();
    //    }
    //    public override void Back(int pIx)
    //    {
    //        Music.SoundManager.PlayFlatSound(LoadedSound.MenuBack);
    //        if (dialogue == null)
    //            base.Back(pIx);
    //        else
    //        {
    //            dialogue.DeleteMe();//dialogue.Click();
    //            dialogue = null;
    //        }
            
    //    }
    //    public void DialogueClosedEvent()
    //    {
    //        dialogue = null;
    //    }
    //    override protected void CreateBackground()
    //    {
    //        ImageLayers lay = imageLayer +2;
    //        VectorRect bakgArea = area;
    //        bakgArea.AddRadius(Edge);

    //        float currentHeight = Edge;

    //        //top
    //        Image top = new Image(SpriteName.LFMenuBackTop, bakgArea.Position, new Vector2(bakgArea.Width, LoadTiles.MenuBackgTopHeight * Scale),lay);

    //        background = new List<Image>
    //        {
    //            top
    //        };
    //        //Center Pieces
    //        bool done = false;
    //        float bottom = (int)top.Bottom - 1;
    //        Vector2 centerSize = new Vector2(bakgArea.Width, LoadTiles.MenuBackgCenterHeight * Scale);
            
    //        do
    //        {
    //            float heightleft = area.Height - currentHeight;

    //            if (heightleft <= centerSize.Y)
    //            {
    //                done = true;
    //                centerSize.Y = heightleft;
    //            }
    //            else
    //            {
    //                currentHeight += centerSize.Y;
    //            }
    //            ImageAdvanced center = new ImageAdvanced(SpriteName.LFMenuBackCenter, 
    //                new Vector2(bakgArea.X, (int)bottom), centerSize, lay, false);
    //            bottom = center.Bottom;
    //            center.SourceY += 1;
    //            center.SourceHeight -= 1;

    //            if (done)
    //            {
    //                center.SourceHeight = (int)(centerSize.Y / Scale);
    //                //center.Height += 4;
    //            }
                
    //            background.Add(center);
    //        } while (!done);
    //        //bottom
    //        background.Add(new Image(SpriteName.LFMenuBackBottom, new Vector2(bakgArea.X, (int)bottom -1),
    //            new Vector2(bakgArea.Width, LoadTiles.MenuBackgTopHeight * Scale), lay));
            
    //        //adding an image to cover gaps
    //        bakgArea = area;
    //        bakgArea.AddRadius(-2);
    //        //bakgArea.Height -= Edge;
    //        lay++;
    //        //Image grayFix = new Image(SpriteName.WhiteArea, bakgArea.Position, bakgArea.Size, lay);
    //        //grayFix.Color = new Microsoft.Xna.Framework.Color(112,112,112);
    //        //background.Add(grayFix);
    //    }
    //    HUD.Scroller scroller = null;
    //    protected override void ShowScroller(bool show)
    //    {
    //        if (show)
    //        {
    //            if (scroller == null)
    //            {
    //                scroller = new HUD.Scroller(true, new Vector2(area.Right + Edge - 2, area.Y - Edge + 2),
    //                    new Vector2(Edge * 0.6f, area.Height + Edge * PublicConstants.Twice), this, imageLayer + 2);

    //                scroller.Colors(ScrollerBackgCol, ScrollerBarCol, ScrollerBackgCol);
    //            }
                
    //        }
    //        else
    //        {
    //            if (scroller != null)
    //            {
    //                scroller.DeleteMe();
    //                scroller = null;
    //            }
    //        }
    //    }
    //    protected override void UpdateScroller(Percent value, Percent visualArea)
    //    {
    //        if (scroller != null)
    //        {
    //            scroller.VisualArea = visualArea;
    //            scroller.Set(value, false);
    //        }
    //    }
    //    override protected void HideDesciption()
    //    {
    //        if (descriptionTimer != null)
    //        {
    //            descriptionTimer.Clear();
    //            descriptionTimer = null;
    //        }
    //    }
    //    override protected void ShowDescription(HUD.AbsBlockMenuMember member)
    //    {
    //        Vector2 descWarningPos;
    //        Vector2 descPos;
    //        if (seperateDesc)
    //        {
    //            descWarningPos = area.TopRight;
    //            descWarningPos.X += SeperateDescXSpace;
    //            descPos = descWarningPos;
    //        }
    //        else
    //        {
    //            descWarningPos = member.Position(0) + DescWarningPos;
    //            descPos = member.Position(0) + new Vector2(DescPos.X + member.Size(0).X, DescPos.Y);
    //        }
            

    //        descriptionTimer = new MenuDescription(descWarningPos, descPos, imageLayer, 
    //            member.Description(0));
    //    }
        
    //    public static readonly TextFormat Text = new TextFormat(LoadedFont.Lootfest, new Vector2(1), Color.White);
    //    public override TextFormat TextFormat
    //    {
    //        get { return Text; }
    //    }
    //    static readonly TextFormat textTitle = new TextFormat(LoadedFont.Lootfest, new Vector2(1.1f), Color.Yellow);
    //    public override TextFormat TextFormatTitle
    //    {
    //        get { return textTitle; }
    //    }
    //    public static readonly TextFormat TextDesc = new TextFormat(LoadedFont.Lootfest, new Vector2(0.8f), Color.LightGray);
    //    public static readonly TextFormat TextFormatNote = new TextFormat(LoadedFont.Lootfest, new Vector2(0.8f), Color.LightGray);
    //    public override TextFormat TextFormatDesc
    //    {
    //        get { return TextDesc; }
    //    }
    //}
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    class MenuDescription : AbsUpdateable
    {
        public static readonly float Width = Menu.Scale * LoadTiles.MenuRectangleWidth * 0.9f;
        Vector2 goalPos;
        const int DotTime = 2;
        int currentTime = DotTime;
        int numDots = 0;
        const char Dot = '.';
        Graphics.AbsDraw text;
        Graphics.Image bakgEdge;
        Graphics.Image bakg;
        const float EdgeSize = 3;
        static readonly Vector2 WarningSize = new Vector2(36, 20);
        static readonly Color TextColor = Color.LightGray;
        static readonly Color BakgColor = new Color(5, 15, 60);
        static readonly Color EdgeColor = Color.White;
        string message;
        float textLayer;

        public MenuDescription(Vector2 warningPos, Vector2 messagePos, ImageLayers lay, string message)
            : base(true)
        {
            this.message = message;
            
            lay -= 2;
            if (messagePos.X + Width > Engine.Screen.SafeArea.Right)
            {
                messagePos.X = Engine.Screen.SafeArea.Right - Width;
            }
            const float ReservedHeight = 135;
            if (messagePos.Y + ReservedHeight > Engine.Screen.SafeArea.Bottom)
            {
                messagePos.Y = Engine.Screen.SafeArea.Bottom - ReservedHeight;
            }
            goalPos = messagePos;

            bakgEdge = new Image(SpriteName.WhiteArea, warningPos, WarningSize, lay);
            bakgEdge.PaintLayer += PublicConstants.LayerMinDiff;
            VectorRect area = new VectorRect(warningPos, WarningSize);
            area.AddRadius(-EdgeSize);
            bakg = new Image(SpriteName.WhiteArea, area.Position, area.Size, lay);

            warningPos.Y -= 20;
            TextG text = new TextG(LoadedFont.TimesL, warningPos, new Vector2(0.5f), Align.Zero, TextLib.EmptyString, TextColor, lay);
            this.text = text;
            text.PaintLayer -= PublicConstants.LayerMinDiff;
            textLayer = text.PaintLayer;
            bakg.Color = BakgColor;
            bakgEdge.Color = EdgeColor;

        }
        public override void Time_Update(float time)
        {
            currentTime--;
            if (currentTime <= 0)
            {
                currentTime = DotTime;
                numDots++;
                if (numDots == 4)
                {
                    //show full box
                    const float WarpTime = 160;
                    currentTime = DotTime;
                    Vector2 diff = goalPos - bakgEdge.Position;
                    new Graphics.Motion2d(MotionType.MOVE, bakg, diff, MotionRepeate.NO_REPEATE, WarpTime, true);
                    new Graphics.Motion2d(MotionType.MOVE, bakgEdge, diff, MotionRepeate.NO_REPEATE, WarpTime, true);

                    this.text.DeleteMe();

                    goalPos.X += EdgeSize + 5;
                    Graphics.TextBoxSimple text = new Graphics.TextBoxSimple(LoadedFont.Lootfest,
                        goalPos, new Vector2(0.8f), Align.Zero,
                        message, TextColor, ImageLayers.AbsoluteTopLayer, Width - EdgeSize * 2);
                    text.PaintLayer = textLayer;
                    text.Visible = false;
                    this.text = text;
                    Vector2 goalSize = new Vector2(Width, text.MesureText().Y);
                    Vector2 sizeDiff = goalSize - WarningSize;
                    new Graphics.Motion2d(MotionType.SCALE, bakg, sizeDiff, MotionRepeate.NO_REPEATE, WarpTime, true);
                    new Graphics.Motion2d(MotionType.SCALE, bakgEdge, sizeDiff, MotionRepeate.NO_REPEATE, WarpTime, true);

                }
                else if (numDots > 4)
                {
                    text.Visible = true;
                    DeleteMe();
                }
                else
                {
                    ((TextG)text).AddChar(Dot);

                }
            }
        }
        public void Clear()
        {
            if (!this.IsDeleted)
                DeleteMe();
            text.DeleteMe();
            bakg.DeleteMe();
            bakgEdge.DeleteMe();
        }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.Players
{
    //class LobbyPlayer : Update
    //{
    //    public int PlayerScreenIx = -1;
    //    Vector2 joinPos;
    //    Graphics.TextG text;
    //    public const float LeftListXpos = 0.2f;
    //    //Graphics.TextG description;
    //    int playerIx;
    //    public static List<Vector2> ScreenPositions;

    //    public LobbyPlayer(int ix)
    //        :base(false)
    //    {
    //        IntVector2 gridPos;
    //        switch (ix)
    //        {
    //            default:
    //                gridPos = IntVector2.Zero;
    //                playerIx = int.Player1;
    //                break;
    //            case 1:
    //                gridPos = new IntVector2(1, 0);
    //                playerIx = int.Player2;
    //                break;
    //            case 2:
    //                gridPos = new IntVector2(0, 1);
    //                playerIx = int.Player3;
    //                break;
    //            case 3:
    //                gridPos = new IntVector2(1, 1);
    //                playerIx = int.Player4;
    //                break;

    //        }

    //        float Yadj = -Engine.Screen.Height * 0.1f;
    //        text = new TextG(LoadedFont.TimesBold,
    //            new Vector2(//pos
    //                Engine.Screen.Width * LeftListXpos,
    //                Engine.Screen.Height * (0.25f + 0.15f * ix)),
    //                VectorExt.V2(0.8f), Align.CenterAll, "error", Color.Gray, ImageLayers.Foreground3);
    //        text.Visible = false;
    //        joinPos = text.Position;
    //    }
        
    //    public void UpdateText()
    //    {
    //        Engine.Player player = Engine.XGuide.GetPlayer(playerIx);
    //        text.TextString = player.PublicName;
    //        if (player.Connected)
    //        {
    //            text.Color = Color.Black;
    //        }
    //        else
    //        {
    //            //const string NotConnected = "Not Connected";
    //            //description.TextString = NotConnected;
    //            text.Color = Color.Gray;
    //            //description.Color = Color.Gray;
    //        }

    //        text.Centertext(Align.CenterAll);
    //        //description.Centertext(Align.CenterAll);

    //    }
    //    public override void Time_Update(float time)
    //    {
    //        //dra texten till rätt ställe
    //        Vector2 goalPos = joinPos;
    //        if (Engine.XGuide.GetPlayer(playerIx).IsActive)
    //        {
    //            goalPos = ScreenPositions[PlayerScreenIx];
    //        }

    //        if (text.Position != goalPos)
    //        {
    //            //move text
    //            Vector2 diff = goalPos - text.Position;
    //            if (diff.Length() < 20)
    //            {
    //                text.Position = goalPos;
    //            }
    //            else
    //            {
    //                diff.Normalize();
    //                text.Position += 1f * time * diff;
    //            }
                
    //        }
    //    }
    //    public bool Visible
    //    {
    //        set 
    //        {
    //            text.Visible = value;
    //            //description.Visible = value;

    //        }
    //    }
    //}
}

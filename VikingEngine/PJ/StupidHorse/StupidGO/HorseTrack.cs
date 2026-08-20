using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.PJ.StupidHorse.StupidGO
{
    class StupidWorld
    {
        public List<HorseTrack> tracks;
        public Vector2 riderScale, horseScale;

        public StupidWorld(int gamerCount)
        { 
            tracks = new List<HorseTrack>(gamerCount);

            Vector2 trackSize = new Vector2(Engine.Screen.SafeArea.Width - Screen.IconSize,
                Screen.IconSize);

            riderScale = new Vector2(trackSize.Y * 1.4f);
            horseScale = riderScale * 1.2f;

            float trackYSpace = Screen.IconSize;

            float totalH = trackSize.Y * gamerCount + trackYSpace * (gamerCount -1);

            Vector2 start = Engine.Screen.CenterScreen;
            start.X -= trackSize.X * 0.5f;
            start.Y -= trackSize.Y * 0.5f;

            VectorRect area = new VectorRect(start, trackSize);

            for (int i = 0; i < gamerCount; ++i)
            {
                HorseTrack track = new HorseTrack(area);
                tracks.Add(track);

                area.nextAreaY(1, trackYSpace);
            }
        }
    }

    class HorseTrack
    {
        //public VectorRect area;

        public Vector2 start, stop;
        Graphics.Image image;
        public Graphics.TextG number;

        public HorseTrack(VectorRect area)
        {
            //this.area = area;
            image = new Image(SpriteName.WhiteArea, area.Position, area.Size, StupidLib.Layer_Track);
            image.Color = Color.YellowGreen;

            number = new Graphics.TextG(LoadedFont.Regular, area.Center, Screen.TextSizeV2 * 2f, Align.CenterAll, "0 m", 
                Color.White, StupidLib.Layer_Track-1);

            float edge = area.Height * 0.5f;
            area.AddXRadius(-edge);
            area.Y += area.Height * 0.15f;

            start = area.Position;
            stop = area.RightTop;
        }
    }
}

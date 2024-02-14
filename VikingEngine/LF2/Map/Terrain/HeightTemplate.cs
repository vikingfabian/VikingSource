using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Map.Terrain
{
    //i framtiden den kanske även ska innehålla två 1Dlistor för LOD ridåer från sidled eller upp/ner-led 
    /// <summary>
    /// A topographic 2D map that 
    /// </summary>
    class HeightTemplate
    {
        public const int PosDiff = 4;
        public const int Width = (WorldPosition.ChunkWidth - PosDiff) * PublicConstants.Twice;
        public const int Radius = Width / 2;
        public static readonly IntVector2 ScreenCenterTemplateStart = new IntVector2((WorldPosition.ChunkWidth - Width) / PublicConstants.Twice);
        //static readonly Center Center1Pos = new Center(new Vector2(Radius), Radius);
       // const float ThirdWidth = Width * 0.33f;
        //const float QuarterWidth = Width * 0.25f;
        static readonly Range MoveCenter = new Range(3);

        public byte[,] heights;

        public HeightTemplate(byte[,] heights)
        {
            //handmade version
            this.heights = heights;
        }

    //    public HeightTemplate()
    //    {
    //        heights = new byte[Width, Width];

    //        /*
    //         * gör en cirkel
    //         * cirkeln kan strechas i x eller y
    //         * centrumet kan flyttas för varje lager
    //         * kan ha två eller tre centrum
    //         * slumpa max höjd 2-4
    //         */

    //        //start with the centers
    //       //int numCenters;
    //        List<Center> centers = new List<Center>();// = new List<Vector2>();
    //        byte rnd = Data.RandomSeed.Instance.NextByte();

    //        if (rnd < 160)
    //        {
    //            centers.Add(Center1Pos);
    //        }
    //        else if (rnd < 220)
    //        {
    //            Vector2 c1;
    //            Vector2 c2;

    //            //numCenters = 2;
    //            switch (Data.RandomSeed.Instance.Next(4))
    //            {
    //                default://Hori
    //                    c1 = new Vector2(ThirdWidth, Radius);
    //                    c2 = new Vector2(ThirdWidth * 2, Radius);
    //                    break;
    //                case 1://Verti
    //                    c1 = new Vector2(Radius, ThirdWidth);
    //                    c2 = new Vector2(Radius,ThirdWidth * 2);
    //                    break;
    //                case 2://diagonal left to right
    //                    c1 = new Vector2(ThirdWidth, ThirdWidth);
    //                    c2 = new Vector2(ThirdWidth * 2, ThirdWidth * 2);
    //                    break;
    //                case 3://diagonal right to left
    //                    c1 = new Vector2(ThirdWidth * 2, ThirdWidth);
    //                    c2 = new Vector2(ThirdWidth, ThirdWidth * 2);
    //                    break;
    //            }
    //            int moveX = Data.RandomSeed.Instance.Next(MoveCenter);
    //            int moveY = Data.RandomSeed.Instance.Next(MoveCenter);
    //            float radius2 = ThirdWidth - lib.LargestOfTwoValues(Math.Abs(moveX), Math.Abs(moveY));
                
    //            if (Data.RandomSeed.Bool())
    //            {
    //                c1.X += moveX;
    //                c1.Y += moveY;
    //                centers.Add(new Center(c1, radius2));
    //                centers.Add(new Center(c2, ThirdWidth));
    //            }
    //            else
    //            {
    //                c2.X += moveX;
    //                c2.Y += moveY;
    //                centers.Add(new Center(c1, ThirdWidth));
    //                centers.Add(new Center(c2, radius2));
    //            }
                
    //        }
    //        else
    //        {
    //            //numCenters = 3;
    //           // new List<Vector2> { new Vector2(ThirdWidth), new Vector2(ThirdWidth), new Vector2(ThirdWidth) };
    //            //gör en tillfälligt väldigt enkel version
    //            centers.Add(new Center(new Vector2(QuarterWidth, Radius), QuarterWidth));
    //            centers.Add(new Center(new Vector2(QuarterWidth * 2, Radius), QuarterWidth));
    //            centers.Add(new Center(new Vector2(QuarterWidth * 3, Radius), QuarterWidth));
    //        }

    //        //go through all squares and check them against the centers, pick the highest value
    //        IntVector2 pos = IntVector2.Zero;
    //        for (pos.Y = 0; pos.Y < Width; pos.Y++)
    //        {
    //            for (pos.X = 0; pos.X < Width; pos.X++)
    //            {
    //                int highestValue = 0;
    //                foreach (Center c in centers)
    //                {
    //                    float dist = (c.Positon - pos.Vec).Length();
    //                    if (dist <= c.Radius)
    //                    {
    //                        highestValue = lib.LargestOfTwoValues(highestValue,
    //                            (int)(1 + c.Height * (1 - 0.6f*(dist / c.Radius))));
    //                    }
    //                }
    //                heights[pos.X, pos.Y] = (byte)highestValue;       
    //            }
    //        }
    //    }
    //}
    //struct Center
    //{
    //    static readonly Range Heights = new Range(1, 4);
    //    public float Height;
    //    public Vector2 Positon;
    //    public float Radius;
    //    public Center(Vector2 pos, float r)
    //    {
    //        Positon = pos;
    //        Radius = r;
    //        Height = Data.RandomSeed.Instance.Next(Heights) + 0.2f;
    //    }
    //}
    }
}

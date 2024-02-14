using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.Map.Terrain.AlgoObjects
{
    struct HouseSettings
    {
        public bool Village;
        public Range Length;
        public Range Width;
        public Range Height;
        public Range RoofHeight;
        public IntVector2 windowSize; //4,4
        public Data.MaterialType WallMaterial;
        public Data.MaterialType RoofMaterial;
        public float WindowPercentYpos;//0.5
        public Data.MaterialType windowFrameMaterial;
        // The following three are never assigned to?
        public Data.MaterialType windowMaterial;
        public int WindowFrameSize;
        public int RoofShootOut;

        public Dir4 DoorDir;
        public Data.MaterialType DoorMaterial;
        public int DoorFrameSize;
        // The following two are never assigned to?
        public bool secondFloor;
        public Data.MaterialType secWallMaterial;
    }
    static class House
    {
        static readonly IntVector2 DoorSize = new IntVector2(4, 4);

        public static void Generate(WorldPosition wp, HouseSettings settings, Data.RandomSeedInstance seedInstance)
        {
            bool xdir = seedInstance.Bool();
            wp.Y = LfRef.chunks.GetHighestYpos(wp) + 1;
            //wp.UpdateWorldGridPos();
            IntVector3 size;
            Dimensions lengthD;
            int length;
            int width;
            if (xdir)
            {
                size = new IntVector3(seedInstance.Next(settings.Length), 1, seedInstance.Next(settings.Width));
                lengthD = Dimensions.X;
                length = size.X;
                width = size.Z;
            }
            else
            {
                size = new IntVector3(seedInstance.Next(settings.Width), 1, seedInstance.Next(settings.Length));
                lengthD = Dimensions.Z;
                length = size.Z;
                width = size.X;
            }
            //Floor
            WorldPosition floorY = wp;
            floorY.WorldGrindex.Y -= 1;
            AlgoLib.FillRectangle(floorY, size, (byte)Data.MaterialType.wood_growing);
            //clean the surface above the floor
            AlgoLib.FillRectangle(wp, size, byte.MinValue);
            
            size.Y = seedInstance.Next(settings.Height);
            fourWallsAndWindows(wp, true, (byte)settings.WallMaterial, size, settings);

            //second floor
            if (settings.secondFloor)
            {
                wp.WorldGrindex.Y+=size.Y;
                size.Y = 1;
                int floorAdd = 0;
                if (settings.RoofShootOut > 0)
                {
                    floorAdd = 2;
                }
                else if (settings.WindowFrameSize > 0)
                {
                    floorAdd = 1;
                }
                WorldPosition secFloorWp = wp;
                secFloorWp.WorldGrindex.X+=-floorAdd;
                secFloorWp.WorldGrindex.Z+=-floorAdd;
                IntVector3 secFloorSz = size + floorAdd * PublicConstants.Twice;
                secFloorSz.Y = 1;
                AlgoLib.FillRectangle(secFloorWp, secFloorSz, (byte)settings.windowFrameMaterial);
                wp.WorldGrindex.Y++;
                size.Y = seedInstance.Next(settings.Height);
                fourWallsAndWindows(wp, false, (byte)settings.secWallMaterial, size, settings);
            }
            
            wp.Y += size.Y -1;
            size.Y = seedInstance.Next(settings.RoofHeight); //5
            //wp.UpdateWorldGridPos();
            
            size.AddDimention(lengthD, settings.RoofShootOut * PublicConstants.Twice);
            wp.WorldGrindex.AddDimention(lengthD, -settings.RoofShootOut);

            AlgoLib.Roof(wp, width, length, size.Y, (byte)settings.RoofMaterial, xdir, true,
                (byte)settings.WallMaterial, settings.RoofShootOut, 1);
        }

        static void fourWallsAndWindows(WorldPosition wp, bool door, byte wallMaterial, IntVector3 size, HouseSettings settings)
        {
            AlgoLib.FourWalls(wp, size, 1, wallMaterial);

            for (Dir4 dir = (Dir4)0; dir < Dir4.NUM_NON; dir++)
            {
                int sideLength = size.X;
                if (dir == Dir4.E || dir == Dir4.W)
                {
                    sideLength = size.Z;
                }
                List<float> windowPositions;
                if (sideLength <= 6)
                {
                    windowPositions = new List<float>
                        {
                            0.5f,
                        };
                }
                else
                {
                    windowPositions = new List<float>
                        {
                            0.26f, 0.74f,
                        };
                }

                for (int i = 0; i < windowPositions.Count; i++)
                {
                    if (door && dir == settings.DoorDir && i == 0)
                    {
                        AlgoLib.Door(wp, size, dir, DoorSize, windowPositions[i], (byte)settings.DoorMaterial, (byte)Data.MaterialType.bronze,
                           settings.DoorFrameSize, (byte)settings.windowFrameMaterial);
                    }
                    else
                    {
                        AlgoLib.Window(wp, size, 1, dir, settings.windowSize.X, settings.windowSize.Y,
                            new Vector2(windowPositions[i], settings.WindowPercentYpos), (byte)settings.windowMaterial,
                            settings.WindowFrameSize, (byte)settings.windowFrameMaterial);
                    }
                }
            }
        }
    }
}

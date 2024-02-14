using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Map.Terrain.AlgoObjects
{
    struct HoleSteps2D
    {
        List<bool> heightSteps;
        List<bool> lengthSteps;
        CirkleCounterUp currenHstep;
        CirkleCounterUp currenLstep;
        int sideBordersHeight;
        int sideBordersLength;

        int heightLength;
        int lengthLength;

        public HoleSteps2D(List<bool> heightSteps, List<bool> lengthSteps, int sideBordersHeight, int sideBordersLength)
        {
            this.heightSteps = heightSteps;
            this.lengthSteps = lengthSteps;
            this.sideBordersHeight = sideBordersHeight;
            this.sideBordersLength = sideBordersLength;

            currenHstep = new CirkleCounterUp(heightSteps.Count - 1);
            currenLstep = new CirkleCounterUp(lengthSteps.Count - 1);

            heightLength = 0;
            lengthLength = 0;
        }
        public void Reset(int heightLength, int lengthLength)
        {
            currenHstep.Reset(); currenLstep.Reset();
            this.heightLength = heightLength;
            this.lengthLength = lengthLength;
        }
        public void NextHeightStep()
        {
            currenHstep.Next();
        }
        public void NextLengthStep()
        {
            currenLstep.Next();
        }
        public bool IsHole(int currentX, int currentY)
        {
            if (currentX >= sideBordersLength && (lengthLength - currentX) >= sideBordersLength)
            {
                if (currentY >= sideBordersHeight && (heightLength - currentY) >= sideBordersHeight)
                {
                    return heightSteps[currenHstep.Value] && lengthSteps[currenLstep.Value];
                }
            }
            return false;
        }
    }

    static class AlgoLib
    {
        public static void Window(WorldPosition bottomPos, IntVector3 fourWallSize, int wallThickness, Dir4 side, int objWidth, int objHeight, Vector2 percentPos, byte material,
            int frameSize, byte framMaterial)
        {
            IntVectorVolume vol = PositionOnWall(fourWallSize, wallThickness, side, objWidth, objHeight, percentPos);
            bottomPos.WorldGrindex += vol.Position;

            if (frameSize > 0)
            {
                frame(bottomPos, vol, framMaterial, frameSize, side);
                //FillRectangle(bottomPos, vol.Size, framMaterial);
                //bottomPos.WorldGridPos.Y += frameSize;
                //vol.Size.Y -= frameSize * PublicConstants.TWICE;

                //if (side == Dir4.N || side == Dir4.S)
                //{
                //    bottomPos.WorldGridPos.X += frameSize;
                //    vol.Size.X -= frameSize * PublicConstants.TWICE;
                //}
                //else
                //{
                //    bottomPos.WorldGridPos.Z += frameSize;
                //    vol.Size.Z -= frameSize * PublicConstants.TWICE;
                //}
            }
            FillRectangle(bottomPos, vol.Size, material);
        }

        public static void Door(WorldPosition bottomPos, IntVector3 fourWallSize, Dir4 side, IntVector2 doorSize, float xPercentPos, byte material, byte handleMaterial,
            int frameSize, byte framMaterial)
        {
            //bottomPos.UpdateChunkPos();
            IntVectorVolume vol = PositionOnWall(fourWallSize, 1, side, doorSize.X, 1, new Vector2(xPercentPos, 0));
            vol.Size.Y = doorSize.Y;
            //vol.Position.Y += 1;
            bottomPos.WorldGrindex += vol.Position;

            if (frameSize > 0)
            {
                frame(bottomPos, vol, framMaterial, frameSize, side);
            }
            FillRectangle(bottomPos, vol.Size, material);

            //bottomPos.WorldGridPos += vol.Position;
           // bottomPos.UpdateChunkPos();
            
            new GameObjects.EnvironmentObj.Door(bottomPos, doorSize, material, side, true);
        }

        static void frame(WorldPosition bottomPos, IntVectorVolume vol, byte framMaterial, int frameSize, Dir4 side)
        {
            FillRectangle(bottomPos, vol.Size, framMaterial);
            bottomPos.WorldGrindex.Y += frameSize;
            vol.Size.Y -= frameSize * PublicConstants.Twice;

            if (side == Dir4.N || side == Dir4.S)
            {
                bottomPos.WorldGrindex.X += frameSize;
                vol.Size.X -= frameSize * PublicConstants.Twice;
            }
            else
            {
                bottomPos.WorldGrindex.Z += frameSize;
                vol.Size.Z -= frameSize * PublicConstants.Twice;
            }
        }

        public static IntVectorVolume PositionOnWall(IntVector3 fourWallSize, int wallThickness, Dir4 side, int objWidth, int objHeight, Vector2 percentPos)
        {
            IntVectorVolume result = new IntVectorVolume(IntVector3.Zero, fourWallSize);
            //bool xdir = false;
            Dimensions sideDir = Dimensions.X;
            switch (side)
            {
                case Dir4.N:
                    result.Size.Z = wallThickness;
                   // xdir = true;
                    break;
                case Dir4.S:
                    result.AddToZSide(-result.Size.Z + wallThickness);
                   // xdir = true;
                    break;
                case Dir4.W:
                    result.Size.X = wallThickness;
                    sideDir = Dimensions.Z;
                    break;
                case Dir4.E:
                    result.AddToXSide(-result.Size.X + wallThickness);
                    sideDir = Dimensions.Z;
                    break;
            }

            IntVector2 pos = new IntVector2(
                    percentPos.X * fourWallSize.GetDimension(sideDir) - objWidth * PublicConstants.Half,
                    percentPos.Y * fourWallSize.Y - objHeight * PublicConstants.Half);
            result.Position.SetDimension(sideDir, pos.X);
            result.Position.Y = pos.Y;
            result.Size.SetDimension(sideDir, objWidth);
            result.Size.Y = objHeight;

            return result;
        }


        public static void FillRectangle(WorldPosition bottomPos, IntVector3 size, byte material)
        {
            new WorldVolume(bottomPos, size).Fill(material);
            //IntVector3 pos = IntVector3.Zero;
            
            //for (pos.X = 0; pos.X < size.X; pos.X++)
            //{
            //    for (pos.Y = 0; pos.Y < size.Y; pos.Y++)
            //    {
            //        for (pos.Z = 0; pos.Z < size.Z; pos.Z++)
            //        {
            //            WorldPosition n = bottomPos.GetNeighborPos(pos);
            //            LfRef.chunks.SetIfOpen(n, material);
            //        }
            //    }
            //}
        }

        public static void FourWalls(WorldPosition bottomPos, IntVector3 size, int thickness, byte material)
        {
            IntVector3 pos = IntVector3.Zero;
            for (pos.Z = 0; pos.Z < size.Z; pos.Z++)
            {
                for (pos.X = 0; pos.X < size.X; pos.X++)
                {
                    //Check if it is the outer blocks
                    if (pos.X < thickness || size.X - pos.X <= thickness ||
                        pos.Z < thickness || size.Z - pos.Z <= thickness)
                    {
                        for (pos.Y = 0; pos.Y < size.Y; pos.Y++)
                        {
                            WorldPosition n = bottomPos.GetNeighborPos(pos);
                            LfRef.chunks.Set(n, material);
                        }
                    }
                }
            }
        }
        public static void Roof(WorldPosition bottomPos, int width, int length, float height, byte material, bool xdir, bool sideWalls, 
            byte sideWallMaterial, int sideWallInPush, int sideWallThickness)
        {
            IntVector3 pos = IntVector3.Zero;
            //int width = (xdir ? size.Y : size.X);
            int halfWidth = (int)(width * PublicConstants.Half);

            height = lib.SetMaxFloatVal(height, halfWidth);
            List<IntVector2> line = linePassesGridPoints(new Vector2(halfWidth + 1, height));
            //int length = (xdir ? size.X : size.Y);

            

            //IntVector2 point2;
            WorldPosition n;
            width -= 1;
            int endPos;
            Dimensions sideWays = xdir ? Dimensions.Z : Dimensions.X;
            Dimensions lenghtsWay = xdir ? Dimensions.X : Dimensions.Z;

            foreach (IntVector2 point in line)
            {
                endPos = width - point.X;
                int wallStart = point.X + 1;
                int wallEnd = endPos;

                for (int l = 0; l < length; l++)
                {
                    pos.Y = point.Y;
                    pos.SetDimension(lenghtsWay, l);

                    if (sideWalls)
                    {
                        int wallLenght = lib.SmallestOfTwoValues(l, length - l - 1);
                        if (wallLenght >= sideWallInPush && (wallLenght - sideWallInPush + 1) <= sideWallThickness)
                        {
                            IntVector3 wallPos = pos;
                            for (int x = wallStart; x < wallEnd; x++)
                            {
                                wallPos.SetDimension(sideWays, x);
                                n = bottomPos.GetNeighborPos(wallPos);
                                LfRef.chunks.Set(n, sideWallMaterial);
                            }
                        }
                    }
                    
                    pos.SetDimension(sideWays, point.X);
                    n = bottomPos.GetNeighborPos(pos);
                    LfRef.chunks.Set(n, material);
                    pos.SetDimension(sideWays, endPos);
                    n = bottomPos.GetNeighborPos(pos);
                    LfRef.chunks.Set(n, material);
                }
            }
        }

        static List<IntVector2> linePassesGridPoints(Vector2 size)
        {
            List<IntVector2> result = new List<IntVector2>();
            int stepLength = Convert.ToInt32(lib.LargestOfTwoValues(size.X, size.Y));
            float shortStep = 
                (float)lib.SmallestOfTwoValues(size.X, size.Y) / stepLength;
            for (int i = 0; i < stepLength; i++)
            {
                int shortVal = Convert.ToInt32(shortStep * i);
                if (size.X > size.Y)
                    result.Add(new IntVector2(i, shortVal));
                else
                    result.Add(new IntVector2(shortVal, i));
            }
            return result;
        }

       

        public static void BuildWall(IntVector3 LeftbottomTopPos, IntVector3 size, int thickness, Data.MaterialType material, HoleSteps2D holeSteps)
        {
            //Go from left to right
            byte bMaterial = (byte)material;
            WorldPosition startPos = new WorldPosition(LeftbottomTopPos);

            List<WorldPosition> leftToRightSection = new List<WorldPosition>();
            List<WorldPosition> topToBottomSection = new List<WorldPosition>();
            
            for (int w = 0; w < thickness; w++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    WorldPosition lr1 = startPos;
                    lr1.WorldGrindex.Y += y;
                    WorldPosition tb1 = lr1;
                    lr1.WorldGrindex.Z += w;
                    tb1.WorldGrindex.X += w;

                    WorldPosition lr2 = lr1;
                    lr2.WorldGrindex.Z += size.Z; // StepZ(size.Z);
                    
                    WorldPosition tb2 = tb1;
                    tb2.WorldGrindex.X += size.X;


                    //lr1.UpdateChunkPos();
                    //tb1.UpdateChunkPos();
                    //lr2.UpdateChunkPos();
                    //tb2.UpdateChunkPos();
                }
            }

            //int thicknessTopIndex = thickness -1;
            int heightStep = 0;
            WorldPosition worldX = startPos;
            holeSteps.Reset(size.Y, size.X);
            int thicknessStep = 0;
            System.Diagnostics.Debug.WriteLine("BUILD WALL");
            for (int xpos = 0; xpos < (size.X + thickness); xpos++)
            {
                foreach (WorldPosition lr in leftToRightSection)
                {
                    WorldPosition wp = lr;
                    wp.X = worldX.X;
                    //wp.ChunkGrindex.X = worldX.ChunkGrindex.X;
                    //wp.LocalBlockX = worldX.LocalBlockX;

                    if (!holeSteps.IsHole(xpos, heightStep))
                    {
                        LfRef.chunks.Set(wp, bMaterial);
                        System.Diagnostics.Debug.WriteLine("WallBild x" + xpos.ToString() + " y" + heightStep.ToString()); 
                    }
                    thicknessStep++;
                    if (thicknessStep== thickness)
                    {
                        thicknessStep = 0;
                        heightStep++;
                        holeSteps.NextHeightStep();
                    }
                }
                heightStep = 0;
                ++worldX.WorldGrindex.X;
                holeSteps.NextLengthStep();
            }

            heightStep = 0;
            holeSteps.Reset(size.Y, size.Z);
            thicknessStep = 0;
            WorldPosition worldZ = startPos;
            for (int zpos = 0; zpos < (size.Z + thickness); zpos++)
            {
                foreach (WorldPosition tb in topToBottomSection)
                {
                    WorldPosition wp = tb;
                    wp.Z = worldZ.Z;
                    //wp.ChunkGrindex.Y = worldZ.ChunkGrindex.Y;
                    //wp.LocalBlockZ = worldZ.LocalBlockZ;

                    if (!holeSteps.IsHole(zpos, heightStep))
                    {
                        LfRef.chunks.Set(wp, bMaterial);
                    }
                    
                    thicknessStep++;
                    if (thicknessStep == thickness)
                    {
                        heightStep++;
                        thicknessStep = 0;
                        holeSteps.NextHeightStep();
                    }
                }
                heightStep = 0;
                ++worldZ.WorldGrindex.Z;
                holeSteps.NextLengthStep();
            }

        }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.Map.Terrain.AlgoObjects;

namespace VikingEngine.LF2.Map
{
    /// <summary>
    /// Contains a block volume in the world grid, used to generate terrain
    /// </summary>
    struct WorldVolume
    {
        public WorldPosition Position;
        public IntVector3 Size;

#region INIT

        public WorldVolume(WorldPosition pos)
        {
            this.Position = pos;
            Size = IntVector3.One;
        }

        public WorldVolume(WorldPosition pos, IntVector3 sz)
        {
            Position = pos;
            Size = sz;
        }
        public WorldVolume(IntVector3 pos, IntVector3 sz)
        {
            Position = new WorldPosition(pos);
            Size = sz;
        }
        public WorldVolume(IntVectorVolume vol)
        {
            Position = new WorldPosition(vol.Position);
            Size = vol.Size;
        }
        public WorldVolume(IntVector2 chunkPos)
        {
            Position = new WorldPosition(chunkPos,
                WorldPosition.ChunkHalfWidth,
                WorldPosition.ChunkStandardHeight,
                WorldPosition.ChunkHalfWidth);
            Size = IntVector3.Zero;
        }
#endregion
#region FILL
        public void Fill(Data.MaterialType material)
        {
            Fill((byte)material);
        }

        /// <summary>
        /// Fills a roof.
        /// </summary>
        /// <param name="material">The material to fill with.</param>
        /// <param name="depthPercentage">How deep the triangle goes. 100 percent is all the way down.</param>
        /// <param name="ridgeLongitudinalForward">The direction in which the ridge runs.</param>
        /// <param name="ridgeLateralOffsetPercentage">The lateral offset of the ridge. 0 is all the way to the left, and 100 is right.</param>
        public void FillRoof(Data.MaterialType material, float depthPercentage, Dir4 ridgeLongitudinalForward, float ridgeLateralOffsetPercentage)
        {
            WorldVolume n = this;
            n.SetYLimit();
            IntVector3 pos = IntVector3.Zero;
            IntVector3 start = n.Position.WorldGrindex;
            IntVector3 end = n.EndPos;

            Dir4 posFace = lib.GetPositiveFacing(ridgeLongitudinalForward);
            Dir4 perpendicular = lib.GetPositivePerpendicularFacing(posFace);

            int leftEnd = GetLateralLeft(posFace);
            int rightEnd = GetLateralRight(posFace);
            int width = GetLateralWidth(posFace);
            int bottomOffset = (int)((float)GetVerticalHeight() * (1.0f - depthPercentage));
            start.Y += bottomOffset;
            int lateralRidgeOffset = (int)((float)width * ridgeLateralOffsetPercentage);

            Vector2 A_btmLeft  = new Vector2(leftEnd,  start.Y);
            Vector2 B_btmRight = new Vector2(rightEnd, start.Y);
            Vector2 C_ridge = new Vector2(leftEnd + lateralRidgeOffset, n.GetVerticalTop());

            float ACdeltaX = (C_ridge.X - A_btmLeft.X);
            float division = (ACdeltaX == 0 ? 1 : ACdeltaX);
            float ACk = (C_ridge.Y - A_btmLeft.Y) / division;

            float CBdeltaX = (B_btmRight.X - C_ridge.X);
            division = (CBdeltaX == 0 ? 1 : CBdeltaX);
            float CBk = (B_btmRight.Y - C_ridge.Y) / division;

            float CBoffset = -CBk * width;

            for (pos.Z = start.Z; pos.Z < end.Z; ++pos.Z)
            {
                for (pos.Y = start.Y; pos.Y < end.Y; ++pos.Y)
                {
                    for (pos.X = start.X; pos.X < end.X; ++pos.X)
                    {
                        // Translate to local coordinates
                        float localX = pos.GetDimension(perpendicular) - leftEnd;
                        float localY = pos.Y - start.Y;

                        float localACheight = ACk * localX;
                        float localCBheight = CBk * localX + CBoffset;
                        if (localACheight >= localY && localCBheight >= localY)
                            LfRef.chunks.Set(pos, (byte)material);
                    }
                }
            }

            // Fill below triangular shape
            for (pos.Z = n.Position.WorldGrindex.Z; pos.Z < end.Z; ++pos.Z)
            {
                for (pos.Y = n.Position.WorldGrindex.Y; pos.Y < n.Position.WorldGrindex.Y + bottomOffset; ++pos.Y)
                {
                    for (pos.X = n.Position.WorldGrindex.X; pos.X < end.X; ++pos.X)
                    {
                        LfRef.chunks.Set(pos, (byte)Data.MaterialType.blue);
                    }
                }
            }
        }

        public void Fill(byte material)
        {
            //AlgoLib.FillRectangle(Position, Size, (byte)material);

            WorldVolume n = this;
            n.SetYLimit();
            //byte m = (byte)material;
            IntVector3 pos = IntVector3.Zero;
            IntVector3 end = n.EndPos;

            for (pos.Z = n.Position.WorldGrindex.Z; pos.Z < end.Z; ++pos.Z)
            {
                for (pos.Y = n.Position.WorldGrindex.Y; pos.Y < end.Y; ++pos.Y)
                {
                    for (pos.X = n.Position.WorldGrindex.X; pos.X < end.X; ++pos.X)
                    {
                        LfRef.chunks.Set(pos, material);
                    }
                }
            }
        }
        public void FillWalls(Data.MaterialType material, int thickness)
        {
            AlgoLib.FourWalls(Position, Size, thickness, (byte)material);
        }

        /// <param name="radiusMultiplier">Adjust which blocks that are in radius of center, recommended: 1.1f</param>
        public void FillCylinder(Data.MaterialType material, Dir4 longitudinalDir, CubeFace cutAway, float radiusMultiplier)// = 1.0f)
        {
            FillCylinder(material, lib.FacingToCubeface(longitudinalDir), cutAway, radiusMultiplier);
        }
        /// <param name="radiusMultiplier">Adjust which blocks that are in radius of center, recommended: 1.1f</param>
        public void FillCylinder(Data.MaterialType material, CubeFace longitudinalDir, CubeFace cutAway, float radiusMultiplier)// = 1.0f)
        {
            byte m = (byte)material;
            WorldVolume n = this;
            n.SetYLimit();
            IntVector3 pos = IntVector3.Zero;
            IntVector3 end = n.EndPos;

            Vector3 center = CenterV3;
            Vector3 hwidth = HalfWidthV3;
            Vector2 posDiff = Vector2.Zero;

            IntVector3 start = n.Position.WorldGrindex;
            switch (cutAway)
            {
                case CubeFace.Xnegative: start.X += n.Size.X / 2; break;
                case CubeFace.Xpositive: end.X -= n.Size.X / 2; break;
                case CubeFace.Ynegative: start.Y += n.Size.Y / 2; break;
                case CubeFace.Ypositive: end.Y -= n.Size.Y / 2; break;
                case CubeFace.Znegative: start.Z += n.Size.Z / 2; break;
                case CubeFace.Zpositive: end.Z -= n.Size.Z / 2; break;
                default: break;
            }

            //X DIR
            if (lib.CubeFaceToDimensions(longitudinalDir) == Dimensions.X)
            {
                //look at each block in one plane and see if it is in radius
                for (pos.Y = start.Y; pos.Y < end.Y; ++pos.Y)
                {
                    posDiff.X = (pos.Y - center.Y) / hwidth.Y;
                    for (pos.Z = start.Z; pos.Z < end.Z; ++pos.Z)
                    {
                        posDiff.Y = (pos.Z - center.Z) / hwidth.Z;
                        
                        if (posDiff.Length() <= radiusMultiplier)
                        {
                            //fill the whole length
                            for (pos.X = start.X; pos.X < end.X; ++pos.X)
                            {
                                LfRef.chunks.Set(pos, m);
                            }
                        }
                    }
                }
            }
            //Z DIR
            else if (lib.CubeFaceToDimensions(longitudinalDir) == Dimensions.Z)
            {
                //look at each block in one plane and see if it is in radius
                for (pos.Y = start.Y; pos.Y < end.Y; ++pos.Y)
                {
                    posDiff.X = (pos.Y - center.Y) / hwidth.Y;
                    for (pos.X = start.X; pos.X < end.X; ++pos.X)
                    {
                        posDiff.Y = (pos.X - center.X) / hwidth.X;

                        if (posDiff.Length() <= radiusMultiplier)
                        {
                            //fill the whole length
                            for (pos.Z = start.Z; pos.Z < end.Z; ++pos.Z)
                            {
                                LfRef.chunks.Set(pos, m);
                            }
                        }
                    }
                }
            }
            //Y DIR
            else
            {
                //look at each block in one plane and see if it is in radius
                for (pos.Z = start.Z; pos.Z < end.Z; ++pos.Z)
                {
                    posDiff.X = (pos.Z - center.Z) / hwidth.Z;
                    for (pos.X = start.X; pos.X < end.X; ++pos.X)
                    {
                        posDiff.Y = (pos.X - center.X) / hwidth.X;

                        if (posDiff.Length() <= radiusMultiplier)
                        {
                            //fill the whole length
                            for (pos.Y = start.Y; pos.Y < end.Y; ++pos.Y)
                            {
                                LfRef.chunks.Set(pos, m);
                            }
                        }
                    }
                }
            }
        }

        /// <param name="radiusMultiplier">Adjust which blocks that are in radius of center, reccomended: 1.1f</param>
        public void FillSphere(Data.MaterialType material, float radiusMultiplier)// = 1.0f)
        {
            byte m = (byte)material;
            WorldVolume n = this;
            n.SetYLimit();
            IntVector3 pos = IntVector3.Zero;
            IntVector3 end = n.EndPos;

            Vector3 center = CenterV3;
            Vector3 hwidth = HalfWidthV3;
            Vector3 posDiff = Vector3.Zero;
            
            for (pos.Z = n.Position.WorldGrindex.Z; pos.Z < end.Z; ++pos.Z)
            {
                posDiff.Z = (pos.Z - center.Z) / hwidth.Z;
                for (pos.Y = n.Position.WorldGrindex.Y; pos.Y < end.Y; ++pos.Y)
                {
                    posDiff.Y = (pos.Y - center.Y) / hwidth.Y;
                    for (pos.X = n.Position.WorldGrindex.X; pos.X < end.X; ++pos.X)
                    {
                        posDiff.X = (pos.X - center.X) / hwidth.X;
                        if (posDiff.Length() <= radiusMultiplier)
                        {
                            LfRef.chunks.Set(pos, m);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fills the volume with an arch. Note that the relation height >= width / 2 must be true.
        /// </summary>
        /// <param name="facing">What direction the arch should revolve around</param>
        /// <param name="material">What material to fill with</param>
        public void FillArch(Dir4 facing, Data.MaterialType material)
        {
            int cylRadius = GetLateralWidth(facing) / 2;
            if (GetVerticalHeight() < cylRadius)
                return;

            WorldVolume copy = this;
            copy.SetYLimit();
            copy.SubFromSide(CubeFace.Ypositive, cylRadius);
            copy.Fill(material);
            WorldVolume cylinder = copy;
            cylinder.Size.Y = cylRadius * 2;
            cylinder.AddPosition(CubeFace.Ypositive, copy.GetVerticalHeight() - cylRadius);
            cylinder.FillCylinder(material, facing, CubeFace.Ynegative, 1);
        }

        /// <summary>
        /// Creates an archway with the specified parameters
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="volume"></param>
        public void CreateArchway(Dir4 direction, Data.MaterialType material, int archWidth, int overhang)
        {
            WorldVolume copy = this;
            // Create archway.
            copy.AddToLateralEnds(direction, archWidth);
            copy.AddToLongitudinalEnds(direction, overhang);
            copy.AddToSide(CubeFace.Ypositive, archWidth);
            copy.FillArch(direction, material);
            // Cut passage.
            copy.SubFromLateralEnds(direction, archWidth);
            copy.SubFromSide(CubeFace.Ypositive, archWidth);
            copy.FillArch(direction, Data.MaterialType.NO_MATERIAL);
        }

        /// <summary>
        /// Creates a pillar according to parameters
        /// </summary>
        /// <param name="pillarMaterial">The material of the pillars</param>
        /// <param name="decorationMaterial">The decorative material</param>
        /// <param name="squarePlinthHeight">How high the square shaped plinth-part is</param>
        /// <param name="circularPlinthWidth">How wide the circular plinth-part is</param>
        /// <param name="circularPlinthHeight">Height of the circular plinth-part</param>
        /// <param name="pillarIntrusion">How far in from the edge of the plinth the pillar is</param>
        public void FillPillar(Data.MaterialType pillarMaterial, Data.MaterialType decorationMaterial,
            int squarePlinthHeight, int circularPlinthWidth, int circularPlinthHeight, int pillarIntrusion)
        {
            WorldVolume copy = this;
            CubeFace up = CubeFace.Ypositive;
            int mainPillarHeight = copy.Size.Y - 2 * (circularPlinthHeight + squarePlinthHeight);

            copy.Size.Y = 0;

            copy.AddToSide(up, squarePlinthHeight).Fill(pillarMaterial); // Foundation
            copy = copy.AddPosition(up, squarePlinthHeight).SubFromXZEnds(pillarIntrusion).SetVerticalHeight(circularPlinthHeight);
            copy.FillCylinder(decorationMaterial, up, CubeFace.NUM, 1); // Plinth
            copy = copy.SubFromXZEnds(circularPlinthWidth).AddToSide(up, mainPillarHeight);
            copy.FillCylinder(pillarMaterial, up, CubeFace.NUM, 1); // Main pillar
            copy = copy.SetYPos(copy.GetVerticalTop()).SetVerticalHeight(circularPlinthHeight).AddToXZEnds(circularPlinthWidth);
            copy.FillCylinder(decorationMaterial, up, CubeFace.NUM, 1); // Plinth replica
            copy = copy.AddToXZEnds(pillarIntrusion).AddPosition(up, circularPlinthHeight).SetVerticalHeight(squarePlinthHeight);
            copy.Fill(pillarMaterial); // Foundation replica
        }
#endregion
#region TRANSFORM
        private WorldVolume AddToXSide(int add)
        {
            Position.WorldGrindex.X -= add; Size.X += add;
            return this;
        }

        /// <summary>
        /// Upward
        /// </summary>
        private WorldVolume AddToYSide(int add)
        {
            Position.WorldGrindex.Y -= add; Size.Y += add;
            return this;
        }
        private WorldVolume AddToZSide(int add)
        {
            Position.WorldGrindex.Z -= add; Size.Z += add;
            return this;
        }

        public WorldVolume AddToSide(CubeFace side, int add)
        {
            switch (side)
            {
                case CubeFace.Xpositive: Size.X += add; break;
                case CubeFace.Ypositive: Size.Y += add; break;
                case CubeFace.Zpositive: Size.Z += add; break;

                case CubeFace.Xnegative: AddToXSide(add); break;
                case CubeFace.Ynegative: AddToYSide(add); break;
                case CubeFace.Znegative: AddToZSide(add); break;
            }
            return this;
        }
        public WorldVolume AddToAllSides(int add)
        {
            Position.WorldGrindex -= add;
            Size += add * 2;
            return this;
        }
        public WorldVolume AddToSide(Dir4 side, int add)
        {
             AddToSide(lib.FacingToCubeface(side), add);
             return this;
        }

        public WorldVolume AddToVerticalEnds(int add)
        {
            AddToSide(CubeFace.Ynegative, add);
            AddToSide(CubeFace.Ypositive, add);
            return this;
        }
        public WorldVolume AddToLateralEnds(Dir4 facing, int add)
        {
            Dir4 perp = lib.GetPerpendicularDirection(facing);
            AddToSide(perp, add);
            AddToSide(lib.Invert(perp), add);
            return this;
        }
        public WorldVolume AddToLongitudinalEnds(Dir4 facing, int add)
        {
            AddToSide(facing, add);
            AddToSide(lib.Invert(facing), add);
            return this;
        }

        public WorldVolume AddToXZEnds(int add)
        {
            AddToLateralEnds(Dir4.N, add);
            AddToLongitudinalEnds(Dir4.N, add);
            return this;
        }

        public WorldVolume AddPosition(CubeFace side, int add)
        {
            Position.WorldGrindex.AddSide(side, add);
            return this;
        }

        public WorldVolume SubtractPosition(Dir4 side, int sub)
        {
            Position.WorldGrindex.SubtractSide(side, sub);
            return this;
        }

        public WorldVolume AddPosition(Dir4 side, int add)
        {
            Position.WorldGrindex.AddSide(side, add);
            return this;
        }

        // These two methods are not used and just clutter the code...
        ///// <summary>
        ///// Puts the volume at a place relative to a position
        ///// </summary>
        ///// <param name="facing">direction in which relativity works</param>
        ///// <param name="relPlace">relative placement of the volume</param>
        ///// <param name="wp">the reference position</param>
        //public WorldVolume AlignTo(CubeFace facing, RelativePlacement relPlace, WorldPosition wp)
        //{
        //    Dimensions dim = lib.CubeFaceToDimensions(facing);
        //    Position.WorldGrindex.SetDimension(dim, wp.WorldGrindex.GetDimension(dim));
        //    switch (relPlace)
        //    {
        //        case RelativePlacement.Behind:
        //            AddPosition(facing, Size.GetDimension(dim));
        //            break;
        //        case RelativePlacement.Centered:
        //            AddPosition(facing, Size.GetDimension(dim) / 2);
        //            break;
        //        case RelativePlacement.InFront:
        //            AddPosition(lib.Invert(facing), Size.GetDimension(dim));
        //            break;
        //        default:
        //            throw new NotImplementedException("Bad call, please give a valid relative placement");
        //    }

        //    return this;
        //}
        ///// <summary>
        ///// Puts the volume at a place relative to a position
        ///// </summary>
        ///// <param name="facing">direction in which relativity works</param>
        ///// <param name="relPlace">relative placement of the volume</param>
        ///// <param name="wp">the reference position</param>
        //public WorldVolume AlignTo(Dir4 facing, RelativePlacement relPlace, WorldPosition wp)
        //{
        //    AlignTo(lib.FacingToCubeface(facing), relPlace, wp);
        //    return this;
        //}

        public WorldVolume SubFromSide(CubeFace facing, int amount)
        {
            int maxSize = Size.GetDimension(lib.CubeFaceToDimensions(facing));
            if (amount > maxSize)
                amount = maxSize;
            switch (facing)
            {
                case CubeFace.Xnegative: Size.X -= amount; Position.X += amount; break;
                case CubeFace.Xpositive: Size.X -= amount; break;
                case CubeFace.Ynegative: Size.Y -= amount; Position.Y += amount; break;
                case CubeFace.Ypositive: Size.Y -= amount; break;
                case CubeFace.Znegative: Size.Z -= amount; Position.Z += amount; break;
                case CubeFace.Zpositive: Size.Z -= amount; break;
                default: throw new ArgumentException("Invalid CubeFace");
            }
            return this;
        }
        public WorldVolume SubFromSide(Dir4 facing, int amount)
        {
            SubFromSide(lib.FacingToCubeface(facing), amount);
            return this;
        }
        public WorldVolume SubFromAllSides(int sub)
        {
            Position.WorldGrindex += sub;
            Size -= sub * 2;
            return this;
        }
        public WorldVolume SubFromXZEnds(int sub)
        {
            SubFromLateralEnds(Dir4.N, sub);
            SubFromLongitudinalEnds(Dir4.N, sub);
            return this;
        }
        public WorldVolume SubFromVerticalEnds(int amount)
        {
            int maxSize = Size.Y;
            if (amount * 2 > maxSize)
            {
                amount = maxSize / 2;
            }
            SubFromSide(CubeFace.Ynegative, amount);
            SubFromSide(CubeFace.Ypositive, amount);

            return this;
        }
        public WorldVolume SubFromLateralEnds(Dir4 dir, int amount)
        {
            Dir4 perpendicular = lib.GetPerpendicularDirection(dir);
            int maxSize = Size.GetDimensionXZ(perpendicular);
            if (amount * 2 > maxSize)
            {
                amount = maxSize / 2;
            }
            Dir4 perpOpposite = lib.Invert(perpendicular);
            SubFromSide(perpendicular, amount);
            SubFromSide(perpOpposite, amount);

            return this;
        }
        public WorldVolume SubFromLongitudinalEnds(Dir4 dir, int amount)
        {
            int maxSize = Size.GetDimensionXZ(dir);
            if (amount * 2 > maxSize)
            {
                amount = maxSize / 2;
            }
            Dir4 opposite = lib.Invert(dir);
            SubFromSide(dir, amount);
            SubFromSide(opposite, amount);

            return this;
        }
        public WorldVolume SetLateralWidthCentered(Dir4 dir, int targetWidth)
        {
            int currentWidth = GetLateralWidth(dir);
            int diff = targetWidth - currentWidth;
            if (diff > 0)
            {
                AddToSide(lib.GetPositivePerpendicularFacing(dir), diff / 2);
                AddToSide(lib.Invert(lib.GetPositivePerpendicularFacing(dir)), diff / 2 + diff % 2);
            }
            else
            {
                diff *= -1;
                SubFromSide(lib.GetPositivePerpendicularFacing(dir), diff / 2 + diff % 2);
                SubFromSide(lib.Invert(lib.GetPositivePerpendicularFacing(dir)), diff / 2);
            }
            return this;
        }
        public WorldVolume SetLongitudinalDepth(Dir4 dir, int depth)
        {
            Size.SetDimensionXZ(dir, depth);
            return this;
        }
        public WorldVolume SetVerticalHeight(int height)
        {
            Size.Y = height;
            return this;
        }
        public WorldVolume SetYPos(int y)
        {
            Position.Y = y;
            return this;
        }
#endregion
        public IntVector3 EndPos
        {
            get
            {
                return Position.WorldGrindex + Size;
            }
        }
        public Vector3 CenterV3
        {
            get
            {
                return new Vector3(
                    Position.WorldGrindex.X + Size.X * PublicConstants.Half - PublicConstants.Half,
                    Position.WorldGrindex.Y + Size.Y * PublicConstants.Half - PublicConstants.Half,
                    Position.WorldGrindex.Z + Size.Z * PublicConstants.Half - PublicConstants.Half
                    );
            }
        }

#region Bottom, Top, Left, Right, Front, Back, Centers, Width, Depth and Height in XZ Airplane mode (Dir4)
        // Only implement all in this region with Dir4, not CubeFace, because
        // CubeFace would require two parameters (rarely useful) and be hell to implement
        public int GetVerticalHeight()
        {
            return Size.Y;
        }
        public int GetLateralWidth(Dir4 facing)
        {
            return Size.GetDimension(lib.GetPerpendicularDirection(facing));
        }
        public int GetLongitudinalDepth(Dir4 facing)
        {
            return Size.GetDimension(facing);
        }
        public int GetVerticalCenter()
        {
            return Position.WorldGrindex.Y + GetVerticalHeight() / 2;
        }
        public int GetLateralCenter(Dir4 facing)
        {
            return Position.WorldGrindex.GetDimension(lib.GetPerpendicularDirection(facing)) + GetLateralWidth(facing) / 2;
        }
        public int GetLongitudinalCenter(Dir4 facing)
        {
            return Position.WorldGrindex.GetDimension(facing) + GetLongitudinalDepth(facing) / 2;
        }
        public int GetVerticalBottom()
        {
            return Position.WorldGrindex.Y;
        }
        public int GetVerticalTop()
        {
            return Position.WorldGrindex.Y + Size.Y;
        }
        public int GetLateralLeft(Dir4 facing)
        {
            switch (facing)
            {
                case Dir4.E:  return Position.WorldGrindex.Z;
                case Dir4.N: return Position.WorldGrindex.X;
                case Dir4.S: return Position.WorldGrindex.X + Size.X;
                case Dir4.W:  return Position.WorldGrindex.Z + Size.Z;
                default: throw new ArgumentException("Bad facing!");
            }
        }
        public int GetLateralRight(Dir4 facing)
        {
            switch (facing)
            {
                case Dir4.E:  return Position.WorldGrindex.Z + Size.Z;
                case Dir4.N: return Position.WorldGrindex.X + Size.X;
                case Dir4.S: return Position.WorldGrindex.X;
                case Dir4.W:  return Position.WorldGrindex.Z;
                default: throw new ArgumentException("Bad facing!");
            }
        }
        public int GetLongitudinalFront(Dir4 facing)
        {
            switch (facing)
            {
                case Dir4.E: return Position.WorldGrindex.X + Size.X;
                case Dir4.N: return Position.WorldGrindex.Z + Size.Z;
                case Dir4.S: return Position.WorldGrindex.Z;
                case Dir4.W: return Position.WorldGrindex.X;
                default: throw new ArgumentException("Bad facing!");
            }
        }
        public int GetLongitudinalBack(Dir4 facing)
        {
            switch (facing)
            {
                case Dir4.E: return Position.WorldGrindex.X;
                case Dir4.N: return Position.WorldGrindex.Z;
                case Dir4.S: return Position.WorldGrindex.Z + Size.Z;
                case Dir4.W: return Position.WorldGrindex.X + Size.X;
                default: throw new ArgumentException("Bad facing!");
            }
        }
#endregion

        public Vector3 HalfWidthV3
        {
            get
            {
                return Size.Vec * PublicConstants.Half;
            }
        }

        public void SetYLimit()
        {
            Position.SetYLimit();
            if (Position.WorldGrindex.Y + Size.Y > WorldPosition.ChunkHeight)
            {
                Size.Y -= (Position.WorldGrindex.Y + Size.Y) - WorldPosition.ChunkHeight;
            }
        }

        public override string ToString()
        {
            return "{ Chunk X:" + Position.ChunkX.ToString() + " Chunk Y:" + Position.ChunkY.ToString() + 
                " X:" + Position.X.ToString() + " Y:" + Position.Y.ToString() + " Z:" + Position.Z.ToString() +
                " SIZE X:" + Size.X.ToString() + " SIZE Y:" + Size.Y.ToString() + " SIZE Z:" + Size.Z.ToString() + " }";
        }
    }
}

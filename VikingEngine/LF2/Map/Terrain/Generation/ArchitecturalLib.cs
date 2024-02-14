using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Map.Terrain.Generation
{
    class ArchitecturalLib
    {

        public static void GenerateStairs(WorldPosition wp, int width, int height, Dir4 direction,
            Data.MaterialType material) //= Players.Player.CharacterWidth)
        {
            GenerateStairs(wp, width, height, direction, material, Players.Player.CharacterWidth);
        }

        /// <summary>
        /// Generates stairs according to parameters. Note that it clears terrain immediately in front of each end.
        /// </summary>
        /// <param name="wp">XYZ origin</param>
        /// <param name="width">How wide they are</param>
        /// <param name="height">Number of steps = height difference</param>
        /// <param name="direction">What way to apply diff on</param>
        /// <param name="material">The material of the stairs</param>
        /// <param name="platformSize">The size of the platforms, if any</param>
        public static void GenerateStairs(WorldPosition wp, int width, int height, Dir4 direction,
            Data.MaterialType material, int platformSize) //= Players.Player.CharacterWidth)
        {
            if (height == 0)
                return;

            Dir4 perpendicular = lib.GetPerpendicularDirection(direction);
            Dir4 opposite = lib.Invert(direction);
            int absHeight = Math.Abs(height);

            // Clear area
            WorldVolume clearVol = new WorldVolume(wp, IntVector3.Zero);
            if (direction == Dir4.N)
                clearVol.SubtractPosition(direction, 1);
            else if (direction == Dir4.W)
                clearVol.SubtractPosition(direction, -1);
            clearVol.AddToSide(direction, absHeight + platformSize * PublicConstants.Twice);
            clearVol.AddToSide(CubeFace.Ypositive, absHeight + Players.Player.CharacterHeight);
            if (height < 0)
                clearVol.AddPosition(CubeFace.Ynegative, absHeight);
            clearVol.AddToSide(perpendicular, width);
            if (direction == Dir4.E || direction == Dir4.W)
                clearVol.SubtractPosition(perpendicular, width / 2);
            else
                clearVol.SubtractPosition(perpendicular, -width / 2);
            clearVol.Fill(Data.MaterialType.NO_MATERIAL);

            // Generate stairs
            WorldVolume stairVol = new WorldVolume(clearVol.Position, clearVol.Size);
            stairVol.AddPosition(CubeFace.Ynegative, 1);
            stairVol.Size.Y = 1;
            stairVol.Fill(material);
            stairVol.AddPosition(CubeFace.Ypositive, 1);
            if (height > 0)
                stairVol.AddToSide(opposite, -platformSize);
            else
                stairVol.AddToSide(direction, -platformSize);

            for (int i = 0; i != absHeight; ++i)
            {
                stairVol.Fill(material);
                stairVol.AddPosition(CubeFace.Ypositive, 1);
                if (height > 0)
                    stairVol.AddToSide(opposite, -1);
                else
                    stairVol.AddToSide(direction, -1);
            }
        }

        /// <summary>
        /// Creates an entrance at the given world position
        /// </summary>
        /// <param name="wp">where in world to create the entrance</param>
        /// <param name="direction">direction outwards from inside fort view</param>
        /// <param name="depth">how deep the entrance is</param>
        /// <param name="width">how wide the entrance is</param>
        /// <param name="height">how high the entrance is</param>
        /// <param name="floorMat">the material of the floor</param>
        /// <param name="insideFloorY">the Y position of the floor</param>
        public static void CutArchedEntrance(WorldPosition wp, Dir4 direction,
            int depth, int width, int height, Data.MaterialType floorMat,
            int insideFloorY, int outsideFloorY, bool generateStairs)
        {
            wp.WorldGrindex.Y = insideFloorY + 1;
            if (lib.IsDirAlongAxisZ_NS(direction))
                wp.WorldGrindex.SubtractSide(lib.GetPerpendicularDirection(direction), width);
            int archCenterDiff = height - width / 2;

            WorldVolume vol = new WorldVolume(wp, IntVector3.Zero);
            // Cut base rectangle
            if (lib.IsDirNorthOrWest(direction))
                vol.SubtractPosition(direction, 1);
            vol.AddToLateralEnds(direction, width);
            vol.AddToSide(direction, depth);
            vol.AddToSide(CubeFace.Ypositive, archCenterDiff);
            vol.Fill(Data.MaterialType.NO_MATERIAL);
            // Cut top arch
            WorldVolume arch = vol;
            arch.AddToSide(CubeFace.Ynegative, -(height - width));
            arch.AddToSide(CubeFace.Ypositive, width / 2);
            arch.FillCylinder(Data.MaterialType.NO_MATERIAL, lib.FacingToCubeface(direction), CubeFace.NUM, 1);
            // Create floor layer
            WorldVolume floor = vol;
            floor.AddPosition(CubeFace.Ynegative, 1);
            floor.Size.Y = 1;
            floor.Fill(floorMat);

            if (generateStairs)
            {
                if (direction == Dir4.W)
                    wp.WorldGrindex.SubtractSide(direction, 2);
                int stairsLength = (outsideFloorY - 1) - insideFloorY;
                wp.WorldGrindex.SubtractSide(lib.Invert(direction), depth);
                GenerateStairs(wp, width, stairsLength, direction, floorMat);
            }
        }
        public static WorldVolume CreateWallOnChunkEdge(IntVector2 chunk, int floorY,
            Dir4 facing, int thickness, int height,
            Data.MaterialType mainMat)
        {
            return CreateWallOnChunkEdge(chunk, floorY, facing, thickness, height, mainMat, true);
        }
        /// <summary>
        /// Creates a wall along the side of a chunk, returning the resulting WorldVolume
        /// </summary>
        /// <param name="chunk">The chunk to construct upon</param>
        /// <param name="floorY">height of floor</param>
        /// <param name="facing">what side of the chunk</param>
        /// <param name="thickness">how thick the wall is</param>
        /// <param name="height">how high the wall is</param>
        /// <param name="mainMat">the wall material</param>
        /// <param name="centerOnEdge">If the wall should be centered longitudinally on the edge, default= true
        /// else it is made so that it never goes over the chunk edge</param>
        public static WorldVolume CreateWallOnChunkEdge(IntVector2 chunk, int floorY,
            Dir4 facing, int thickness, int height,
            Data.MaterialType mainMat, bool centerOnEdge) //= true)
        {
            Dir4 perpendicular = lib.GetPerpendicularDirection(facing);
            Dir4 opposite = lib.Invert(facing);

            // Set position
            WorldPosition wp = new WorldPosition(chunk);
            WorldVolume vol = new WorldVolume(wp, IntVector3.Zero);
            vol.Position.Y = floorY + 1; // above floor
            if (facing == Dir4.E || facing == Dir4.S)
            {
                vol.SubtractPosition(opposite, WorldPosition.ChunkWidth - (centerOnEdge ? thickness / 2 : thickness));
            }
            else
            {
                vol.SubtractPosition(opposite, (centerOnEdge ? thickness / 2 : 0));
            }
            
            // Set width
            vol.Size.SetDimension(lib.Dir4ToDimensions(perpendicular), WorldPosition.ChunkWidth);
            // Set thickness
            vol.Size.SetDimension(lib.Dir4ToDimensions(facing), thickness);
            // Set height
            vol.Size.SetDimension(lib.CubeFaceToDimensions(CubeFace.Ypositive), height);
            
            // Fill base wall
            vol.Fill(mainMat);            

            return vol;
        }

        /// <summary>
        /// Creates a random decorational word, choosing among words from the wallMessages variable.
        /// </summary>
        /// <param name="direction">Facing</param>
        /// <param name="decorVol">1x1x1 volume denoting center position</param>
        public static void CreateWordPainting(Dir4 direction, WorldVolume decorVol, Data.MaterialType frameMaterial, string word)
        {
            Dir4 left = lib.GetLateralLeftFacing(direction);

            WorldVolume letterVol = decorVol;
            if (lib.IsOdd(word.Length))
            {
                letterVol.AddPosition(left, 1);
            }
            else // is even
            {
                decorVol.SubFromSide(left, 1);
            }

            // Create frame
            decorVol = decorVol.AddToLateralEnds(direction, word.Length / 2 + 1).AddToVerticalEnds(1);
            decorVol.Fill(frameMaterial);
            // Create word
            Dir4 writeDirection = lib.GetLateralRightFacing(direction);
            letterVol.AddPosition(lib.GetLateralLeftFacing(direction), word.Length / 2);
            
            for (int iLetter = 0; iLetter != word.Length; ++iLetter)
            {
                letterVol.AddPosition(lib.GetLateralRightFacing(direction), 1); ;
                letterVol.Fill(lib.LetterBlockFromChar(char.ToLower(word[iLetter])));
            }
        }

        /// <summary>
        /// Creates a carpet central in a chunk, according to parameters
        /// Used for leading paths between chunks
        /// </summary>
        /// <param name="chunkPos">What chunk</param>
        /// <param name="dir">What direction to face</param>
        /// <param name="floorY">Y position</param>
        /// <param name="halfWidth">Half width of carpet, should be higher than outlineWidth</param>
        /// <param name="carpetMaterial">What material to create the carpet in</param>
        /// <param name="outlineWidth">How big the outline is. Should be lower than halfWidth</param>
        /// <param name="outlineMaterial">What material the outline will be in</param>
        /// <param name="createCenterpiece">Create the centerpiece? Only do this the first time for each chunk.</param>
        public static void CreateCarpetInChunk(IntVector2 chunkPos, Dir4 dir, int floorY, int halfWidth,
            Data.MaterialType carpetMaterial, int outlineWidth, Data.MaterialType outlineMaterial, ref bool createCenterpiece)
        {
            WorldVolume carpetVolume = new WorldVolume(
                new WorldPosition(chunkPos, WorldPosition.ChunkHalfWidth, floorY, WorldPosition.ChunkHalfWidth),
                new IntVector3(0, 1, 0));
            // ...centerpiece.
            if (createCenterpiece)
            {
                WorldVolume centerCarpet = carpetVolume;
                centerCarpet.AddToXZEnds(halfWidth);
                centerCarpet.Fill(outlineMaterial);
                centerCarpet.SubFromXZEnds(outlineWidth);
                centerCarpet.Fill(carpetMaterial);
                createCenterpiece = false;
            }

            // ...leading to doorway.
            carpetVolume.SubtractPosition(lib.Invert(dir), halfWidth - outlineWidth);
            carpetVolume.AddToLateralEnds(dir, halfWidth);
            carpetVolume.AddToSide(dir, WorldPosition.ChunkHalfWidth - (halfWidth - outlineWidth));
            carpetVolume.Fill(outlineMaterial);
            carpetVolume.SubFromLateralEnds(dir, outlineWidth);
            carpetVolume.Fill(carpetMaterial);
        }

        /// <summary>
        /// Create an archway on an existing wall
        /// </summary>
        /// <param name="entranceDir">The direction of the entrance</param>
        /// <param name="wallVolume">WorldVolume representing the wall</param>
        /// <param name="mainMaterial">The archway main material</param>
        /// <param name="decorativeMaterial">The detail material</param>
        /// <param name="height">How high</param>
        /// <param name="width">How wide</param>
        public static void CreateFancyArchwayOnWall(Dir4 entranceDir, WorldVolume wallVolume,
            Data.MaterialType mainMaterial, Data.MaterialType decorativeMaterial, int height, int width)
        {
            WorldVolume copy = wallVolume;
            copy.SetLateralWidthCentered(entranceDir, width + 2);
            copy.Size.Y = height + 1;
            copy.CreateArchway(entranceDir, mainMaterial, 2, 1);
            copy.CreateArchway(entranceDir, decorativeMaterial, 1, 2);
            copy.SubFromLateralEnds(entranceDir, 1).SubFromSide(CubeFace.Ypositive, 1).CreateArchway(entranceDir, mainMaterial, 1, 1);
            copy.AddToLateralEnds(entranceDir, 2).AddToSide(CubeFace.Ypositive, 1).CreateArchway(entranceDir, mainMaterial, 1, 0);
        }
    }
}



using Microsoft.Xna.Framework;

namespace VikingEngine.DSSWars.Map.Path3
{

    class MoveCostLayer
    {
        public const int Dir_NorthToSouth = 0;
        public const int Dir_DiagonalNorthEast = 1;
        public const int Dir_WestToEast = 2;
        public const int Dir_DiagonalSouthEast = 3;

        public const int Layer2TileWidth = 4;
        public const int Layer4TileWidth = 16;
        public const int Lay2WidthOnLay4 = Layer4TileWidth / Layer2TileWidth;

        public const int StoredDirections = 4;
        public IntVector2 size;
        public float MaxMoveCost;
        public MoveCost[] cost_n_ne_e_se;

        public Vector3 tileToWp, tileToWpStart;

        public int layer;
        float toTileScale;
        //public Grid2D<CostLayerTile> tiles;

        public MoveCostLayer(int layer, int tileWidth, IntVector2 size)
        {
            MaxMoveCost = tileWidth * 10f;
            this.toTileScale = 1f / tileWidth;
            this.size = size;
            cost_n_ne_e_se = new MoveCost[size.Area() * StoredDirections];

            tileToWpStart.X = tileWidth * 0.5f;
            tileToWpStart.Z = tileWidth * 0.5f;

            tileToWp.X = tileWidth;
            tileToWp.Z = tileWidth;

        }

        public IntVector2 wpToTile(Vector3 wp)
        { 
            return new IntVector2(wp.X * toTileScale , wp.Z * toTileScale);
        }

        public static int ConvertDir8(int dir8)
        {
            if (dir8 > Dir_DiagonalSouthEast)
            {
                return dir8 - 4;
            }
            return dir8;
        }

        public void Set(IntVector2 layerPos, int direction, MoveCost cost)
        {
            int ix = (layerPos.X + layerPos.Y * size.X) * StoredDirections + direction;
            cost_n_ne_e_se[ix] = cost;
        }

        public void Set(int layerIxStart, int direction, MoveCost cost)
        {
            int ix = layerIxStart + direction;
            cost_n_ne_e_se[ix] = cost;
        }

        public MoveCost Get(IntVector2 layerPos, int direction)
        {
            int ix = (layerPos.X + layerPos.Y * size.X) * StoredDirections + direction;
            return cost_n_ne_e_se[ix];
        }

        public MoveCost Get_dir8(IntVector2 layerPos, int dir8)
        {
            if (dir8 > Dir_DiagonalSouthEast)
            {
                dir8 -= 4;
            }
            int ix = (layerPos.X + layerPos.Y * size.X) * StoredDirections + dir8;
            return cost_n_ne_e_se[ix];
        }

        public MoveCost Get(int layerPos, int direction)
        {
            int ix = layerPos + direction;
            return cost_n_ne_e_se[ix];
        }

        public int GetPositionStart(IntVector2 layerPos)
        {
            return (layerPos.X + layerPos.Y * size.X) * StoredDirections;
        }

        public bool InBounds(IntVector2 position)
        {
            return position.X >= 0 && position.X < size.X &&
                position.Y >= 0 && position.Y < size.Y;
        }

        public static IntVector2 WpToLay4(Vector3 wp)
        { 
            return new IntVector2(
                wp.X * WorldData.TileSubDivitions / MoveCostLayer.Layer4TileWidth, 
                wp.Z * WorldData.TileSubDivitions / MoveCostLayer.Layer4TileWidth);
        }
        public static IntVector2 WpToLay2(Vector3 wp)
        {
            return new IntVector2(
                wp.X * WorldData.TileSubDivitions / MoveCostLayer.Layer2TileWidth, 
                wp.Z * WorldData.TileSubDivitions / MoveCostLayer.Layer2TileWidth);
        }
    }

    class MoveCostLayer4 : MoveCostLayer
    {
        public const byte TileStatus_None = 0;
        public const byte TileStatus_NeedUpdate = 1;
        public const byte TileStatus_Updateing = 2;
        public const byte TileStatus_Initialized = 3;

        
        public Grid2D_L<byte> tileStatus;
        public MoveCostLayer4(WorldData world)
            :base(4, Layer4TileWidth, world.subTileGrid.Size / Layer4TileWidth)
        {
            tileStatus = new Grid2D_L<byte>(size);
        }
    }

    

    //class MoveCostLayers
    //{
       
    //}

    

}

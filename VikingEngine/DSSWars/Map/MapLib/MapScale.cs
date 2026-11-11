using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;

namespace VikingEngine.DSSWars.Map.MapLib
{
    struct MapScale
    {
        const int ScaleUpMap2 = 16;

        //Måste vara delbart med UnitGridSquareWidth
        //Must be in sizes of 8
        public const int TinyMapWidth = 200 * ScaleUpMap2;
        public const int TinyMapHeigth = 176 * ScaleUpMap2;
        public const int SmallMapWidth = 320 * ScaleUpMap2;
        public const int SmallMapHeigth = 232 * ScaleUpMap2;
        public const int MediumMapWidth = 512 * ScaleUpMap2;
        public const int MediumMapHeigth = 432 * ScaleUpMap2;
        public const int LargeMapWidth = 720 * ScaleUpMap2;
        public const int LargeMapHeigth = 624 * ScaleUpMap2;
        public const int HugeMapWidth = 912 * ScaleUpMap2;
        public const int HugeMapHeigth = 832 * ScaleUpMap2;
        public const int EpicMapWidth = 1184 * ScaleUpMap2;
        public const int EpicMapHeigth = 1024 * ScaleUpMap2;
        public const double TileWidthInKm = 0.008;

        public const int CustomMapSize_Min = 160 * ScaleUpMap2;
        public const int CustomMapSize_Max = 2048 * ScaleUpMap2;

        public const int IconToWorldScale = 16;
        public bool bCustomSize = false;
        public IntVector2 customMapSize = new IntVector2(CustomMapSize_Min);
        public MapSizeCategory sizeCategory = MapSizeCategory.Small;

        public IntVector2 Size(bool icon)
        {
            var size = bCustomSize ? customMapSize : SizeDimentions(sizeCategory);
            if (icon)
            {
                size /= IconToWorldScale;
            }
            return size;
        }

        public MapScale()
        { }
        public MapScale(MapSizeCategory sizeCategory)
        {
            this.sizeCategory = sizeCategory;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(bCustomSize);
            if (bCustomSize)
            {
                customMapSize.writeUshort(w);
            }
            else
            {
                w.Write((byte)sizeCategory);
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            bCustomSize = r.ReadBoolean();
            if (bCustomSize)
            {
                customMapSize.readUshort(r);
            }
            else
            { 
                sizeCategory = (MapSizeCategory)r.ReadByte();
            }
        }


        public void setTextureSize(IntVector2 size)
        {
            customMapSize = size * IconToWorldScale;
            bCustomSize = true;
        }

        public void setLoadedIconSize(IntVector2 size)
        {
            customMapSize = size * IconToWorldScale;
            sizeCategory = CustomMapSizeToSize(customMapSize, out bCustomSize);
        }

        //public string SizeString()
        //{
        //    return SizeString(mapSize, Size(false));
        //}

        public string SizeString()
        {
            string name = null;
            switch (sizeCategory)
            {
                case MapSizeCategory.Tiny: name = DssRef.lang.Lobby_MapSizeOptTiny; break;
                case MapSizeCategory.Small: name = DssRef.lang.Lobby_MapSizeOptSmall; break;
                case MapSizeCategory.Medium: name = DssRef.lang.Lobby_MapSizeOptMedium; break;
                case MapSizeCategory.Large: name = DssRef.lang.Lobby_MapSizeOptLarge; break;
                case MapSizeCategory.Huge: name = DssRef.lang.Lobby_MapSizeOptHuge; break;
                case MapSizeCategory.Epic: name = DssRef.lang.Lobby_MapSizeOptEpic; break;
                case MapSizeCategory.EpicPlus: name = DssRef.lang.Lobby_MapSizeOptEpic + "+"; break;
            }

            if (!bCustomSize)
            {
                customMapSize = SizeDimentions(sizeCategory);
            }

            name += " " +
                string.Format(DssRef.lang.Lobby_MapSizeDesc,
                    Math.Round(customMapSize.X * TileWidthInKm),
                    Math.Round(customMapSize.Y * TileWidthInKm));

            return name;
        }

        public static IntVector2 SizeDimentions(MapSizeCategory mapSize)
        {
            IntVector2 result;
            switch (mapSize)
            {
                case MapSizeCategory.Tiny:
                    result = new IntVector2(TinyMapWidth, TinyMapHeigth);
                    break;
                case MapSizeCategory.Small:
                    result = new IntVector2(SmallMapWidth, SmallMapHeigth);
                    break;
                case MapSizeCategory.Medium:
                    result = new IntVector2(MediumMapWidth, MediumMapHeigth);
                    break;
                case MapSizeCategory.Large:
                    result = new IntVector2(LargeMapWidth, LargeMapHeigth);
                    break;
                case MapSizeCategory.Huge:
                    result = new IntVector2(HugeMapWidth, HugeMapHeigth);
                    break;
                case MapSizeCategory.Epic:
                    result = new IntVector2(EpicMapWidth, EpicMapHeigth);
                    break;
                case MapSizeCategory.EpicPlus:
                    result = new IntVector2(CustomMapSize_Max, CustomMapSize_Max);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        public static MapSizeCategory CustomMapSizeToSize(IntVector2 size, out bool bCustomSize)
        {
            var area = size.Area();

            for (MapSizeCategory sz = 0; sz <= MapSizeCategory.Epic; sz++)
            {
                if (SizeDimentions(sz) == size)
                {
                    bCustomSize = false;
                    return sz;
                }
            }

            bCustomSize = true;

            for (MapSizeCategory sz = MapSizeCategory.Small; sz < MapSizeCategory.Epic; sz++)
            {
                if (SizeDimentions(sz).Area() > area)
                {
                    return sz - 1;
                }
            }

            return MapSizeCategory.Epic;
        }

        public MapSizeCategory GetSizeCategory()
        {
            if (bCustomSize)
            {
                return ToMapSize(customMapSize);
            }
            else
            {
                return sizeCategory;
            }
        }


        public static MapSizeCategory ToMapSize(IntVector2 tileSize)
        {
            var area = tileSize.Area();

            for (MapSizeCategory sz = MapSizeCategory.Epic; sz >= 0; sz--)
            {
                if (area > SizeDimentions(sz).Area())
                {
                    return sz + 1;
                }
            }

            return MapSizeCategory.Tiny;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Map.HDvoxel
{
    struct BlockHD
    {
        public const int ColorStep = 16;
        const int StartColor = ColorStep / 2;
        const int ColorStepCount = (byte.MaxValue - StartColor) / ColorStep;

        public Color color;
        public MaterialProperty material;

        public BlockHD(Color color, MaterialProperty material)
        {
            this.color = color;
            this.material = material;
        }

        public BlockHD(Color color)
        {
            this.color = color;
            this.material = MaterialProperty.Unknown;
        }

        public BlockHD(ushort blockValue)
            :this()
        {
            this.BlockValue = blockValue;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(BlockValue);
        }
        public void read(System.IO.BinaryReader r)
        {
            BlockValue = r.ReadUInt16();
        }

        public void SetColor(Color col)
        {
            this.color = col;
            if (material == MaterialProperty.BlockPattern)
            {
                material = MaterialProperty.Unknown;
            }
        }

        public void SetColor(Dimensions dim, byte value)
        {
            switch (dim)
            {
                case Dimensions.X: color.R = value; break;
                case Dimensions.Y: color.G = value; break;
                case Dimensions.Z: color.B = value; break;
            }
        }
        public byte GetColor(Dimensions dim)
        {
            switch (dim)
            {
                case Dimensions.X: return color.R;
                case Dimensions.Y: return color.G;
                case Dimensions.Z: return color.B;
            }
            throw new ArgumentOutOfRangeException();
        }

        public ushort BlockValue
        {
            get
            {
                return BlockHD.ToBlockValue(color, (int)material);
            }
            set
            {
                color = BlockHD.ToColor(value);
                material = BlockHD.ToMaterial(value);
            }
        }

        public bool HasMaterial() { return material != MaterialProperty.Empty; }
        public bool IsEmpty() { return material == MaterialProperty.Empty; }

        public override string ToString()
        {
            if (material == MaterialProperty.BlockPattern)
            {
                return "Pattern-" + ((BlockPatternMaterial)color.R).ToString();
            }
            else
            {
                return material.ToString() + "-R" + color.R.ToString() + " G" + color.G.ToString() + " B" + color.B.ToString();
            }
        }
    //}


    //struct BlockHD
    //{
        

        public static ushort ToBlockValue(Color col, int material)
        {
            if (material == EmptyBlockMaterial)
            {
                return EmptyBlock;
            }

            int r = (col.R / ColorStep) << 12;
            int g = (col.G / ColorStep) << 8;
            int b = (col.B / ColorStep) << 4;

            //r4 + g4 + b4 + mat4
            ushort result = (ushort)(r + g + b + material);

            return result;
        }

        public static ushort ToBlockValue(BlockPatternMaterial pattern)
        {
            int r = (int)pattern << 12;

            ushort result = (ushort)(r + BlockPatternMaterial);

            return result;
        }

        public static Color ToColor(ushort blockValue)
        {
            //61440,3840,240,15

            Color result = new Color(
                (byte)(StartColor + (blockValue >> 12) * ColorStep),
                (byte)(StartColor + ((blockValue >> 8) & 15) * ColorStep),
                (byte)(StartColor + ((blockValue >> 4) & 15) * ColorStep));

            return result;
        }

        public static Color FilterColor(Color col)
        {
            return ToColor(ToBlockValue(col, UnknownMaterial));
        }

        public void tintSteps(int addR, int addG, int addB)
        {
            color = FilterColor(ColorExt.ChangeColor(color, addR * ColorStep, addG * ColorStep, addB * ColorStep));
        }

        public static MaterialProperty ToMaterial(ushort blockValue)
        {
            return (MaterialProperty)(blockValue & 15);
        }
        public static int ToMaterialValue(ushort blockValue)
        {
            return blockValue & 15;
        }

        public const byte EmptyBlockMaterial = 0;
        public const byte BlockPatternMaterial = 15;
        public const byte EndBlockMaterial = BlockPatternMaterial - 1;
        public static readonly byte UnknownMaterial = (byte)MaterialProperty.Unknown;
        public static readonly byte AntiMaterial = (byte)MaterialProperty.AntiBlock;

        public const ushort EmptyBlock = 0;

        public static readonly BlockHD Empty = new BlockHD(EmptyBlock);


        //public static readonly BlockHD EndBlock = new BlockHD(EndBlockMaterial);

        //public byte material;
        //public byte colorR, colorG, colorB;

        //public bool IsEmpty { get { return material == EmptyBlockMaterial; } }
        //public bool HasMaterial { get { return material != EmptyBlockMaterial; } }



        //public BlockHD(byte material)
        //{
        //    this.material = material;
        //    colorR = 0; colorG = 0; colorB = 0;
        //}

        //public BlockHD(Color color)
        //{
        //    this.material = UnknownMaterial;
        //    colorR = color.R;
        //    colorG = color.G;
        //    colorB = color.B;
        //}

        //public BlockHD(BlockPatternMaterial mat)
        //{
        //    this.material = BlockPatternMaterial;
        //    colorR = (byte)mat;
        //    colorG = 0; 
        //    colorB = 0;
        //}

        //public void write(System.IO.BinaryWriter w)
        //{
        //    w.Write(material);
        //    if (material != EmptyBlockMaterial)
        //    {
        //        w.Write(colorR);
        //        if (material != BlockPatternMaterial)
        //        {
        //            w.Write(colorG);
        //            w.Write(colorB);
        //        }
        //    }
        //}
        //public void read(System.IO.BinaryReader r)
        //{
        //    material = r.ReadByte();
        //    if (material != EmptyBlockMaterial)
        //    {
        //        colorR = r.ReadByte();
        //        if (material != BlockPatternMaterial)
        //        {
        //            colorG = r.ReadByte();
        //            colorB = r.ReadByte();
        //        }
        //    }
        //}

        //public override bool Equals(object obj)
        //{
        //    BlockHD other = (BlockHD)obj;

        //    return 
        //        this.material == other.material &&
        //        this.colorR == other.colorR &&
        //        this.colorG == other.colorG &&
        //        this.colorB == other.colorB;
        //}
        //public static bool operator ==(BlockHD value1, BlockHD value2)
        //{
        //    return 
        //        value1.material == value2.material &&
        //        value1.colorR == value2.colorR &&
        //        value1.colorG == value2.colorG &&
        //        value1.colorB == value2.colorB;
        //}
        //public static bool operator !=(BlockHD value1, BlockHD value2)
        //{
        //    return 
        //        value1.material != value2.material ||
        //        value1.colorR != value2.colorR ||
        //        value1.colorG != value2.colorG ||
        //        value1.colorB != value2.colorB;
        //}

        //public Color faceColor(int brightness)
        //{
        //    Color col;

        //    if (material == BlockPatternMaterial)
        //    {
        //        col = BlockPatternMaterialsLib.MaterialColors[colorR];
        //    }
        //    else
        //    {
        //        col = new Color(colorR, colorG, colorB);
        //    }

        //    int r = col.R + brightness;
        //    col.R = r < byte.MaxValue ? (byte)r : byte.MaxValue;
 
        //    int g = col.G + brightness;
        //    col.G = g < byte.MaxValue ? (byte)g : byte.MaxValue;
            
        //    int b = col.B + brightness;
        //    col.B = b < byte.MaxValue ? (byte)b : byte.MaxValue;

        //    return col;
        //}
        public static Color FaceColorTinted(ushort blockValue, int addR, int addG, int addB)
        {
            Color col = BlockHD.ToColor(blockValue);

            col = ColorExt.ChangeColor(col, addR, addG, addB);
            //addR += col.R;
            //if (addR <= byte.MinValue) { col.R = byte.MinValue; }
            //else if (addR >= byte.MaxValue) { col.R = byte.MaxValue; }
            //else { col.R = (byte)addR; }

            //addG += col.G;
            //if (addG <= byte.MinValue) { col.G = byte.MinValue; }
            //else if (addG >= byte.MaxValue) { col.G = byte.MaxValue; }
            //else { col.G = (byte)addG; }

            //addB += col.B;
            //if (addB <= byte.MinValue) { col.B = byte.MinValue; }
            //else if (addB >= byte.MaxValue) { col.B = byte.MaxValue; }
            //else { col.B = (byte)addB; }

            return col;
        }

        //public Color faceColorTinted(int addR, int addG, int addB, IntVector3 wp)
        //{
        //    Color col;

        //    if (material == BlockPatternMaterial)
        //    {
        //        var grid = BlockPatternMaterialsLib.MaterialColors[colorR];
        //        IntVector3 sz = grid.size;

        //        wp.X %= sz.X;
        //        wp.Y %= sz.Y;
        //        wp.Z %= sz.Z;
 
        //        col = grid[wp.X, wp.Y, wp.Z].GetColor();
        //    }
        //    else
        //    {
        //        col = new Color(colorR, colorG, colorB);
        //    }

        //    addR += col.R;
        //    if (addR <= byte.MinValue) { col.R = byte.MinValue; }
        //    else if (addR >= byte.MaxValue) { col.R = byte.MaxValue; }
        //    else { col.R = (byte)addR; }

        //    addG += col.G;
        //    if (addG <= byte.MinValue) { col.G = byte.MinValue; }
        //    else if (addG >= byte.MaxValue) { col.G = byte.MaxValue; }
        //    else { col.G = (byte)addG; }

        //    addB += col.B;
        //    if (addB <= byte.MinValue) { col.B = byte.MinValue; }
        //    else if (addB >= byte.MaxValue) { col.B = byte.MaxValue; }
        //    else { col.B = (byte)addB; }
            
        //    return col;
        //}

        //public override string ToString()
        //{
        //    if (material == BlockPatternMaterial)
        //    {
        //        return "Pattern-" + ((BlockPatternMaterial)colorR).ToString();
        //    }
        //    else
        //    {
        //        return ((MaterialProperty)material).ToString() + "-R" + colorR.ToString() + " G" + colorG.ToString() + " B" + colorB.ToString();
        //    }
        //}

        //public Color GetColor()
        //{
        //    return new Color(colorR, colorG, colorB);
        //}
        //public BlockHD SetColor(Color color)
        //{
        //    colorR = color.R;
        //    colorG = color.G;
        //    colorB = color.B;
        //    onSetColor();
        //    return this;
        //}

        //public BlockHD SetColor(Dimensions dim, byte value)
        //{
        //    switch (dim)
        //    {
        //        case Dimensions.X: colorR = value; break;
        //        case Dimensions.Y: colorG = value; break;
        //        case Dimensions.Z: colorB = value; break;
        //    }
        //    onSetColor();
        //    return this;
        //}

        //public byte GetColor(Dimensions dim)
        //{
        //    switch (dim)
        //    {
        //        case Dimensions.X: return colorR;
        //        case Dimensions.Y: return colorG;
        //        case Dimensions.Z: return colorB;
        //    }
        //    throw new ArgumentOutOfRangeException();
        //}

        //public void onSetColor()
        //{
        //    if (this.material == BlockPatternMaterial)
        //    {
        //        this.material = UnknownMaterial;
        //    }
        //}

        //public Color DarkTintCol()
        //{
        //    return new Color(colorR, Bound.ByteBounds(colorG -4), Bound.ByteBounds(colorB -4));
        //}
        //public Color BlueTintCol()
        //{
        //    return new Color(Bound.ByteBounds(colorR - 4), Bound.ByteBounds(colorG - 4), Bound.ByteBounds(colorB + 4));
        //}
        //public Color YellowTintCol()
        //{
        //    return new Color(Bound.ByteBounds(colorR + 3), Bound.ByteBounds(colorG + 3), colorB);
        //}
        public static Color DarkTintCol(ushort blockValue)
        {
            Color result = ToColor(blockValue);
            result.R -= 2;
            result.G -= 6;
            result.B -= 2;

            return result;
            //return new Color(colorR, Bound.ByteBounds(colorG - 4), Bound.ByteBounds(colorB - 4));
        }
        public static Color BlueTintCol(ushort blockValue)
        {
            Color result = ToColor(blockValue);
            result.R -= 4;
            result.G -= 4;
            result.B += 6;

            return result;
            //return new Color(Bound.ByteBounds(colorR - 4), Bound.ByteBounds(colorG - 4), Bound.ByteBounds(colorB + 4));
        }
        public static Color YellowTintCol(ushort blockValue)
        {
            Color result = ToColor(blockValue);
            result.R += 4;
            result.G += 4;

            return result;
            //return new Color(Bound.ByteBounds(colorR + 3), Bound.ByteBounds(colorG + 3), colorB);
        }

    }

    struct BlockHDPair
    {
        public ushort block1, block2;

        public BlockHDPair(ushort block1, ushort block2)
        {
            this.block1 = block1;
            this.block2 = block2;
        }
    }
}

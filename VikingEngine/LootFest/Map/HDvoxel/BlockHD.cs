using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Map.HDvoxel
{
    struct BlockHD
    {
        public const byte EmptyBlockMaterial = 0;
        public const byte DefaultBlockMaterial = 1;
        //public const byte ProfileBlockMaterial = 2;
        public const byte BlockPatternMaterial = 15;
        public const byte EndBlockMaterial = BlockPatternMaterial - 1;
        public static readonly byte DefaultMaterial = (byte)MaterialProperty.Default;
        public static readonly byte ReplaceMaterial = (byte)MaterialProperty.Replaceable;

        public const ushort EmptyBlock = 0;

        public static readonly BlockHD Empty = new BlockHD(EmptyBlock);

        public const int ColorStep = 16;
        const int StartColor = ColorStep / 2;
        //const int ColorStepCount = (byte.MaxValue - StartColor) / ColorStep;

        public Color color;
        public MaterialProperty material;
        public static ushort JointUp, JointForward, JointBack;

        public BlockHD(Color color, MaterialProperty material)
        {
            this.color = color;
            this.material = material;
        }

        public BlockHD(Color color)
        {
            this.color = color;
            this.material = MaterialProperty.Default;
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
                material = MaterialProperty.Default;
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
            return ToColor(ToBlockValue(col, DefaultMaterial));
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

        
        public static ushort SetMaterialProperty(ushort blockValue, MaterialProperty toMaterial)
        {
            // Clear the lower 4 bits (material)
            // mask to preserve the top 12 bits (RGB) and clear the bottom 4 (material)
            ushort rgbPart = (ushort)(blockValue & ~0b1111);

            // Set the new material
            ushort result = (ushort)(rgbPart | ((int)toMaterial & 0b1111));

            return result;
        }
        



        public static Color FaceColorTinted(ushort blockValue, int addR, int addG, int addB)
        {
            Color col = BlockHD.ToColor(blockValue);

            col = ColorExt.ChangeColor(col, addR, addG, addB);
      
            return col;
        }

        public static Color DarkTintCol(ushort blockValue)
        {
            Color result = ToColor(blockValue);
            result.R -= 2;
            result.G -= 6;
            result.B -= 2;

            return result;
        }
        public static Color BlueTintCol(ushort blockValue)
        {
            Color result = ToColor(blockValue);
            result.R -= 4;
            result.G -= 4;
            result.B += 6;

            return result;
        }
        public static Color YellowTintCol(ushort blockValue)
        {
            Color result = ToColor(blockValue);
            result.R += 4;
            result.G += 4;

            return result;
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

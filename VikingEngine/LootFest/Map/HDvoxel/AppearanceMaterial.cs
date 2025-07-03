using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Map.HDvoxel
{

    struct TwoAppearanceMaterials
    {
        public AppearanceMaterial mat1, mat2;

        public TwoAppearanceMaterials(AppearanceMaterial mat1, AppearanceMaterial mat2)
        {
            this.mat1 = mat1; this.mat2 = mat2;
        }
    }

    struct AppearanceMaterial
    {
        public static AppearanceMaterial Material1, Material2, Material3, Material4, Material5;

        public static void Init() //Lootfest only!
        {
            Material1 = new AppearanceMaterial(Color.Gray, true);
            Material2 = new AppearanceMaterial(new Color(65,74,129), false); //Blå
            Material3 = new AppearanceMaterial(new Color(133,78,65), false);//Röd
            Material4 = new AppearanceMaterial(new Color(65,133,69), false);//Grön
            Material5 = new AppearanceMaterial(new Color(147,143,85), false);//Gul
        }

        public ushort baseColor, redTint, brighter, darker;

        public AppearanceMaterial(Color color, bool red)
            :this()
        {
            BlockHD baseBlock = new BlockHD(color, MaterialProperty.Replaceable);
            setupTints(baseBlock, red);
        }

        public AppearanceMaterial(ushort color)
            : this()
        {
            BlockHD baseBlock = new BlockHD();
            baseBlock.BlockValue = color;
            baseBlock.material = MaterialProperty.Replaceable;

            setupTints(baseBlock, true);
        }

        void setupTints(BlockHD baseBlock, bool bRed)
        {
            BlockHD bright = baseBlock, dark = baseBlock;

            if (bRed)
            {
                BlockHD red = baseBlock;
                red.color.R = Bound.Byte(red.color.R + BlockHD.ColorStep);
                redTint = red.BlockValue;
            }
            else
            {
                redTint = BlockHD.EmptyBlock;
            }

            bright.color.R = Bound.Byte(bright.color.R + BlockHD.ColorStep);
            bright.color.G = Bound.Byte(bright.color.G + BlockHD.ColorStep);
            bright.color.B = Bound.Byte(bright.color.B + BlockHD.ColorStep);

            dark.color.R = Bound.Byte(dark.color.R - BlockHD.ColorStep);
            dark.color.G = Bound.Byte(dark.color.G - BlockHD.ColorStep);
            dark.color.B = Bound.Byte(dark.color.B - BlockHD.ColorStep);

            baseColor = baseBlock.BlockValue;
            
            brighter = bright.BlockValue;
            darker = dark.BlockValue;
        }

        public bool replaceMaterial(ushort fromMaterial, AppearanceMaterial toMaterial, out ushort toTintedMaterial)
        {
            if (fromMaterial == baseColor)
            {
                toTintedMaterial = toMaterial.baseColor;
                return true;
            }

            if (fromMaterial == brighter)
            {
                toTintedMaterial = toMaterial.brighter;
                return true;
            }

            if (fromMaterial == darker)
            {
                toTintedMaterial = toMaterial.darker;
                return true;
            }

            if (fromMaterial == redTint)
            {
                toTintedMaterial = toMaterial.redTint;
                return true;
            }

            toTintedMaterial = 0;
            return false;
        }
    }
}

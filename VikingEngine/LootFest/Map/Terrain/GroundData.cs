using VikingEngine.LootFest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Map.Terrain
{
    enum Biome
    {
        None,
        Snowy_1,
        Barren_2,
        Tundra_3,
        ColdForest_4,
        WarmForest_5,
        Fields_6,
        Meadows_7,
        Grasslands_8,
        Prairie_9,
        Savanna_10,
        Desert_11,
        Burnt_12
    }

    struct BiomeMaterials
    {
        public MaterialType top;
        public MaterialType soil;
        public MaterialType deep;

        public BiomeMaterials(MaterialType top, MaterialType soil, MaterialType deep)
        {
            this.top = top;
            this.soil = soil;
            this.deep = deep;
        }

        public static readonly BiomeMaterials None = new BiomeMaterials(
            MaterialType.NO_MATERIAL, 
            MaterialType.NO_MATERIAL, 
            MaterialType.NO_MATERIAL);
        public static readonly BiomeMaterials Snowy = new BiomeMaterials(
            MaterialType.snow,
            MaterialType.dirty_snow, 
            MaterialType.mountain_gray);
        public static readonly BiomeMaterials Barren = new BiomeMaterials(
            MaterialType.dirty_snow,
            MaterialType.dirt_brown,
            MaterialType.mountain_gray);
        public static BiomeMaterials Tundra(PcgRandom prng)
        {
            
                MaterialType[] leaves = new MaterialType[]
                {
                    MaterialType.mossy_stones_blue,
                    MaterialType.mossy_stones_blue,
                    MaterialType.leaves_dry
                };
                return new BiomeMaterials(leaves[prng.Int(leaves.Length)], MaterialType.dirt_brown, MaterialType.mountain_gray);
            
        }
        public static readonly MaterialType[] leaves = new MaterialType[]
        {
            MaterialType.grass_mossy,
            MaterialType.grass_mossy,
            MaterialType.grass_mossy,
            MaterialType.grass_mossy,
            MaterialType.grass_mossy,
            MaterialType.leaves_dry,
            MaterialType.leaves_red,
            MaterialType.leaves_yellow
        };
        public static BiomeMaterials ColdForest(PcgRandom prng)
        {
             return new BiomeMaterials(leaves[prng.Int(leaves.Length)], MaterialType.dirt_brown, MaterialType.mountain_gray);
            
        }
        public static readonly BiomeMaterials WarmForest = new BiomeMaterials(
            MaterialType.grass_mossy,
            MaterialType.dirt_brown,
            MaterialType.mountain_gray);
        public static readonly BiomeMaterials Field = new BiomeMaterials(
            MaterialType.grass_mossy,
            MaterialType.dirt_brown,
            MaterialType.mountain_blue);
        public static readonly BiomeMaterials Meadow = new BiomeMaterials(
            MaterialType.grass_green,
            MaterialType.dirt_red,
            MaterialType.mountain_blue);
        public static readonly BiomeMaterials Grassland = new BiomeMaterials(
            MaterialType.grass_green,
            MaterialType.dirt_red,
            MaterialType.mountain_blue);
        public static readonly BiomeMaterials Prairie = new BiomeMaterials(
            MaterialType.grass_dry,
            MaterialType.dirt_red,
            MaterialType.mountain_red);
        public static readonly BiomeMaterials Savanna = new BiomeMaterials(
            MaterialType.grass_yellow,
            MaterialType.dirt_sand,
            MaterialType.mountain_red);
        public static readonly BiomeMaterials Desert = new BiomeMaterials(
            MaterialType.desert_yellow,
            MaterialType.dirt_orange,
            MaterialType.mountain_sand);
        public static readonly BiomeMaterials Burnt = new BiomeMaterials(
            MaterialType.desert_black,
            MaterialType.dirt_blue,
            MaterialType.stones_black);
    }

    struct GroundData
    {
        public int height;
        public BiomeMaterials materials;
        public Biome biome;

        public GroundData(int height, Biome biome, PcgRandom prng)
        {
            this.height = height;
            this.biome = biome;
            materials = BiomeMaterials.None;
            RefreshMaterialsFromBiome(prng);
        }

        public GroundData(int height, BiomeMaterials materials)
        {
            this.height = height;
            this.materials = materials;
            biome = Biome.None;
        }

        public void RefreshMaterialsFromBiome(PcgRandom prng)
        {
            switch (biome)
            {
                case Biome.None:
                    materials = BiomeMaterials.None;
                    break;
                case Biome.Snowy_1:
                    materials = BiomeMaterials.Snowy;
                    break;
                case Biome.Barren_2:
                    materials = BiomeMaterials.Barren;
                    break;
                case Biome.Tundra_3:
                    materials = BiomeMaterials.Tundra(prng);
                    break;
                case Biome.ColdForest_4:
                    materials = BiomeMaterials.ColdForest(prng);
                    break;
                case Biome.WarmForest_5:
                    materials = BiomeMaterials.WarmForest;
                    break;
                case Biome.Fields_6:
                    materials = BiomeMaterials.Field;
                    break;
                case Biome.Meadows_7:
                    materials = BiomeMaterials.Meadow;
                    break;
                case Biome.Grasslands_8:
                    materials = BiomeMaterials.Grassland;
                    break;
                case Biome.Prairie_9:
                    materials = BiomeMaterials.Prairie;
                    break;
                case Biome.Savanna_10:
                    materials = BiomeMaterials.Savanna;
                    break;
                case Biome.Desert_11:
                    materials = BiomeMaterials.Desert;
                    break;
                case Biome.Burnt_12:
                    materials = BiomeMaterials.Burnt;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

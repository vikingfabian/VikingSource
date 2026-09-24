using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.DSSWars.Map.Settings;

namespace VikingEngine.DSSWars.Map.MapLib
{
    class WorldBioms
    {

        public Biom[] bioms = new Biom[(int)BiomType.NUM];
        public WorldBioms()
        {
            bioms[(int)BiomType.WetGreen] = new Biom(
                new TileColor(dampColors(new Color(94, 118, 25)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(210, 209, 136)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(68, 85, 20)), SurfaceTextureType.Grass),
                new TileColor(dampColors(new Color(75, 76, 73)), SurfaceTextureType.None),
                //1.1f, 0.6f, 0
                new BiomGroundType()
                {
                    groundType = GroundType.Default,                    
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Sand,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.5f),
                         groundType = GroundType.Rootmat,
                        setTerrain1 = new SetTerrainChance(0.05,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.27f),
                         groundType = GroundType.Rootmat,
                    },
                },

                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {

                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.4f),
                        groundType = GroundType.Fertile,

                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rootmat,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
                );

            bioms[(int)BiomType.Swamp] = new Biom(
                new TileColor(dampColors(new Color(113, 123, 31)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(208, 207, 148)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(40, 43, 19)), SurfaceTextureType.Grass),
                new TileColor(dampColors(new Color(75, 82, 59)), SurfaceTextureType.None),
                //1.1f, 0.2f, 0
                new BiomGroundType()
                {
                    groundType = GroundType.Default,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Marsh,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.5f),
                         groundType = GroundType.Marsh,
                        setTerrain1 = new SetTerrainChance(0.05,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.27f),
                         groundType = GroundType.Marsh,
                    },
                },

                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {

                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.15f),
                        groundType = GroundType.Fertile,

                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Rootmat,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Marsh,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
                );

            bioms[(int)BiomType.Green] = new Biom(
                new TileColor(dampColors(new Color(104, 146, 70)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(255, 254, 181)), SurfaceTextureType.Sand),
                new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(8, 71, 6), -10)), SurfaceTextureType.Grass),
                new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(73, 76, 73), -10)), SurfaceTextureType.None),
                //1f, 0.25f, 0
                new BiomGroundType()
                {
                    groundType = GroundType.Default,
                },

                new BiomGroundType()
                {
                    groundType = GroundType.Sand,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.4f),
                         groundType = GroundType.Rootmat,
                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.5,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.21f),
                         groundType = GroundType.Rootmat,
                    },
                },
                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.3f),
                        groundType = GroundType.Fertile,

                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rootmat,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
                );

            bioms[(int)BiomType.Hills] = new Biom(
               new TileColor(dampColors(new Color(115, 198, 68)), SurfaceTextureType.Grass),

               new TileColor(dampColors(new Color(216, 230, 129)), SurfaceTextureType.Sand),
               new TileColor(dampColors(new Color(70, 151, 41)), SurfaceTextureType.Grass),
               new TileColor(dampColors(new Color(73, 76, 73)), SurfaceTextureType.None),
                //1.2f, 0.1f, 0
                new BiomGroundType()
                {
                    groundType = GroundType.Default,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Sand,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.44f),
                         groundType = GroundType.Rootmat,
                        setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.23f),
                         groundType = GroundType.Rootmat,
                    },
                },

                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {

                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.5f),
                        groundType = GroundType.Fertile,

                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                       
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rootmat,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
               );

            bioms[(int)BiomType.GreenDry] = new Biom(
               new TileColor(dampColors(new Color(208, 188, 119)), SurfaceTextureType.Grass),

               new TileColor(dampColors(new Color(230, 214, 162)), SurfaceTextureType.Sand),
               new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(180, 161, 97), -10)), SurfaceTextureType.Grass),
               new TileColor(dampColors(ColorExt.ChangeBrighness(new Color(124, 128, 107), -10)), SurfaceTextureType.None),
                //0.5f, 0.9f, 0.5f
                new BiomGroundType()
                {
                    groundType = GroundType.Default,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Rubble,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.4f),
                         groundType = GroundType.Rootmat,
                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.4,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.17f),
                         groundType = GroundType.Rootmat,
                    },
                },

                new BiomGroundType[] //Stone noise
                {

                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.1f),
                        groundType = GroundType.Fertile,

                        setTerrain2 = new SetTerrainChance(0.6,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rootmat,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
               );

            bioms[(int)BiomType.YellowDry] = new Biom(
                new TileColor(dampColors(new Color(171, 162, 54)), SurfaceTextureType.Sand),

                new TileColor(dampColors(new Color(255, 237, 130)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(80, 60, 2)), SurfaceTextureType.None),
                new TileColor(dampColors(new Color(81, 79, 68)), SurfaceTextureType.None),
                //0.5f, 0, 0.6f
                new BiomGroundType()
                {
                    groundType = GroundType.Sand,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Sand,
                },
                null,

                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Default,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
                );

            bioms[(int)BiomType.RedDry] = new Biom(
                new TileColor(dampColors(new Color(171, 120, 54)), SurfaceTextureType.Sand),

               new TileColor(dampColors(new Color(255, 220, 130)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(60, 33, 9)), SurfaceTextureType.None),
                new TileColor(dampColors(new Color(90, 79, 65)), SurfaceTextureType.None),
                //0.6f, 0, 0.5f
                new BiomGroundType()
                {
                    groundType = GroundType.Hardpan,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Mud,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.24f),
                         groundType = GroundType.Default,
                        setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.DryWood)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.15,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.14f),
                         groundType = GroundType.Default,
                    },
                },

                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
                );

            bioms[(int)BiomType.DarkLands] = new Biom(
                new TileColor(dampColors(new Color(58, 94, 108)), SurfaceTextureType.None),

               new TileColor(dampColors(new Color(102, 115, 116)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(58, 94, 108)), SurfaceTextureType.None),
                new TileColor(dampColors(new Color(39, 59, 57)), SurfaceTextureType.None),
                //0.6f, 0, 0.6f)
                new BiomGroundType()
                {
                    groundType = GroundType.Rock,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Rubble,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.12f),
                         groundType = GroundType.Default,
                        setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.DryWood)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.15,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.14f),
                         groundType = GroundType.Default,
                    },
                },


                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                })
            {
                mudColor = dampColors(new Color(24, 56, 67)),
                treeHard = LootFest.VoxelModelName.fol_tree_hard_lava,
                treeSoft = LootFest.VoxelModelName.fol_tree_soft_lava,
            };

            bioms[(int)BiomType.Frozen] = new Biom(
                new TileColor(dampColors(new Color(86, 109, 83)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(197, 242, 242)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(40, 53, 47)), SurfaceTextureType.None),
                new TileColor(dampColors(new Color(97, 114, 114)), SurfaceTextureType.None),
                //1.3f, 0.8f, 0.2f
                new BiomGroundType()
                {
                    groundType = GroundType.Frost,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Mud,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.5f),
                         groundType = GroundType.Rootmat,
                        setTerrain1 = new SetTerrainChance(0.5,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.25f),
                         groundType = GroundType.Rootmat,
                    },
                },


                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rubble,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rootmat,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                })
            {
                treeHard = LootFest.VoxelModelName.fol_tree_hard_snow,
                treeSoft = LootFest.VoxelModelName.fol_tree_soft_snow,
            };

            bioms[(int)BiomType.Tundra] = new Biom(
                new TileColor(dampColors(new Color(148, 133, 55)), SurfaceTextureType.Grass),

                new TileColor(dampColors(new Color(178, 188, 152)), SurfaceTextureType.Sand),
                new TileColor(dampColors(new Color(100, 91, 42)), SurfaceTextureType.Grass),
                new TileColor(dampColors(new Color(86, 91, 75)), SurfaceTextureType.None),
                //0.5f, 0.9f, 0.5f
                new BiomGroundType()
                {
                    groundType = GroundType.Default,
                },
                new BiomGroundType()
                {
                    groundType = GroundType.Mud,
                },
                new BiomGroundType[] //Tree noise
                {
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.3f),
                         groundType = GroundType.Rootmat,
                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeSoft)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                        setTerrain2 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TreeHard)
                        { sizeRange = new Range(TerrainContent.TreeMaxSize / 10, TerrainContent.TreeMaxSize) },
                    },
                    new BiomGroundType() {
                         noiseValue = new IntervalF(0, 0.13f),
                         groundType = GroundType.Rootmat,
                    },
                },

                new BiomGroundType[] //Stone noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.08f),
                        groundType = GroundType.Frost,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                        setTerrain2 = new SetTerrainChance(0.008,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.StoneBlock),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.22f),
                        groundType = GroundType.Rock,

                        setTerrain1 = new SetTerrainChance(0.003,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Stones),
                    },
                },
                new BiomGroundType[] //Herb noise
                {
                    new BiomGroundType() {
                        noiseValue = new IntervalF(0, 0.6f),
                        groundType = GroundType.Default,

                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                         setTerrain1 = new SetTerrainChance(0.1,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Herbs),
                    },
                    new BiomGroundType() {
                        noiseValue = new IntervalF(-0.4f, 0),
                        groundType = GroundType.Rootmat,

                        setTerrain1 = new SetTerrainChance(0.2,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.Bush),
                        setTerrain2 = new SetTerrainChance(0.3,
                           TerrainMainType.Foil, (int) TerrainSubFoilType.TallGrass),
                    },
                }
                );

            Color dampColors(Color color)
            {
                color.Deconstruct(out byte r, out byte g, out byte b);

                r = (byte)(26 + contrast(r));
                g = (byte)(16 + contrast(g));
                b = (byte)(10 + contrast(b));

                int contrast(int value)
                {
                    if (value < 140)
                    {
                        return (int)(value * 0.8);
                    }
                    else
                    {
                        return (int)(value * 0.9);
                    }
                }

                return new Color(r, g, b);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.Map.MapLib
{
    enum GroundType : byte
    {
        Default,

        Fertile,

        Rock,

        /// <summary>
        /// Man made solid ground
        /// </summary>
        MadeSolid,

        /// <summary>
        /// Man made grass field
        /// </summary>
        MadeField,

        Rubble,

        /// <summary>
        /// Bio ground, terraform gives fuel
        /// </summary>
        Rootmat,

        Marsh,
        
        Mud,

        /// <summary>
        /// Soft dry desert
        /// </summary>
        Sand,

        /// <summary>
        /// Hard dry clay desert
        /// </summary>
        Hardpan,

        Frost,
        
        NUM
    }

    static class GroundPropertiesLib
    {
        public static GroundTypeProperties[] GroundProperties;

        public static void Init()
        {
            GroundProperties = new GroundTypeProperties[(int)GroundType.NUM];
            GroundProperties[(int)GroundType.Default] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.Yes,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.IfTerraformed,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = true,
                moveCost = 1f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureGrass1, 3),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Fertile] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.ExtraBonus,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.IfTerraformed,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = true,
                moveCost = 1f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureFertile, 1),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Rock] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.Yes,
                canBuildHeavy = GroundCanBuildType.Yes,
                canTerraform = false, //TODO explosives
                moveCost = 0.7f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureRock1, 3),
                sidetexture = new SpriteVariantCount(SpriteName.DssMapTextureRockSide)
            };

            GroundProperties[(int)GroundType.MadeSolid] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.Yes,
                canBuildHeavy = GroundCanBuildType.Yes,
                canTerraform = true, //TODO explosives
                moveCost = 0.7f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureMadeSolid, 1),
                sidetexture = new SpriteVariantCount(SpriteName.DssMapTextureRockSide)
            };

            GroundProperties[(int)GroundType.MadeField] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.Yes,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.IfTerraformed,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = true,
                moveCost = 0.9f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureGrass1, 3),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Rubble] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.IfTerraformed,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = true,
                moveCost = 2f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureRubble),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Rootmat] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.IfTerraformed,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = true,
                moveCost = 1.5f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureRootmat),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };
            
            GroundProperties[(int)GroundType.Marsh] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.No,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = false, //TODO explosives
                moveCost = 3f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureSwamp),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Mud] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.No,
                canBuild = GroundCanBuildType.No,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = false,
                moveCost = 1.5f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureMud),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Sand] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.No,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = false,
                moveCost = 2f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureSand),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Hardpan] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.Yes,
                canBuildHeavy = GroundCanBuildType.Yes,
                canTerraform = false,
                moveCost = 0.8f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureHardPan),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };

            GroundProperties[(int)GroundType.Frost] = new GroundTypeProperties()
            {
                canPlant = GroundCanBuildType.No,
                canBuildLight = GroundCanBuildType.Yes,
                canBuild = GroundCanBuildType.Yes,
                canBuildHeavy = GroundCanBuildType.No,
                canTerraform = false,
                moveCost = 1.4f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureFrozen),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles)
            };
        }
    }

    struct GroundTypeProperties
    {
        public GroundCanBuildType canPlant;
        public GroundCanBuildType canBuildLight;
        public GroundCanBuildType canBuild;
        public GroundCanBuildType canBuildHeavy;

        public bool canTerraform;


        public float moveCost;

        public SpriteVariantCount texture;
        public SpriteVariantCount sidetexture;
    }

    enum GroundCanBuildType
    { 
        ExtraBonus,
        Yes,
        IfTerraformed,
        No,

    }
}

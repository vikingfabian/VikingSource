using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;
using VikingEngine.DSSWars.Resource;

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

        SaltDesert,

        /// <summary>
        /// Hard dry clay desert
        /// </summary>
        Hardpan,

        Frost,
        
        NUM
    }

    enum BuildingGroundRequirement
    { 
        None,
        Plant,
        BuildLight,
        BuildMedium,

        /// <summary>
        /// Exclusive to extra massive walls, cant be found in some areas
        /// </summary>
        BuildHeavy,
    }

    static class GroundPropertiesLib
    {
        public static GroundTypeProperties[] GroundProperties;
        public static readonly FlatArray_Eight<GroundType> TerraformOptions = new FlatArray_Eight<GroundType>(

                GroundType.Default,
                GroundType.MadeField,
                GroundType.MadeSolid,
                GroundType.SaltDesert);
            

        public static GroundTypeProperties Get(GroundType type)
        {
            return GroundProperties[(int)type];
        }

        public static void Init()
        {
            GroundProperties = new GroundTypeProperties[(int)GroundType.NUM];
            GroundProperties[(int)GroundType.Default] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = true,
                moveCost = 1.2f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureGrass1, 3),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = 10,
            };

            GroundProperties[(int)GroundType.Fertile] = new GroundTypeProperties()
            {
                canPlant = true,//GroundCanBuildType.ExtraBonus,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = true,
                moveCost = 1.6f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureFertile, 1),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = 10,
            };

            GroundProperties[(int)GroundType.Rock] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = true,
                canBuildHeavy = true,
                canTerraform = false, //TODO explosives
                moveCost = 1f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureRock1, 3),
                sidetexture = new SpriteVariantCount(SpriteName.DssMapTextureRockSide),
                TerraformFromTimeSec = -1,
            };

            GroundProperties[(int)GroundType.MadeSolid] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = true,
                canBuildHeavy = false,
                canTerraform = true, //TODO explosives
                moveCost = 1f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureMadeSolid, 1),
                sidetexture = new SpriteVariantCount(SpriteName.DssMapTextureRockSide),
                TerraformFromTimeSec = 10,
                TerraformToCost = new CraftBlueprint(CraftResultType.NoSet, 0, 0,
                    new UseResource[] {
                        new UseResource( ItemResourceType.Brick, 5)
                    }, XP.WorkExperienceType.NUM_NONE)
            };

            GroundProperties[(int)GroundType.MadeField] = new GroundTypeProperties()
            {
                canPlant = true,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = true,
                moveCost = 1.7f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureGrass1, 3),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = 60,
            };

            GroundProperties[(int)GroundType.Rubble] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = true,
                moveCost = 2.5f,
                badMountTerrain = true,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureRubble1, 2),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = 40,
                TerraformFromRest = new ItemResource(ItemResourceType.Stone_G, 4)
            };

            GroundProperties[(int)GroundType.Rootmat] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = true,
                moveCost = 1.5f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureRootmat1, 2),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = 60,
                TerraformFromRest = new ItemResource(ItemResourceType.Fuel_G, 8)
            };
            
            GroundProperties[(int)GroundType.Marsh] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = false, //TODO explosives
                moveCost = 3f,
                badMountTerrain = true,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureSwamp),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = 10,
            };

            GroundProperties[(int)GroundType.Mud] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = false,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = false,
                moveCost = 2f,
                badMountTerrain = true,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureMud),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = -1,
            };

            GroundProperties[(int)GroundType.Sand] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = false,
                moveCost = 2.4f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureSand),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = -1,
            };

            GroundProperties[(int)GroundType.SaltDesert] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = false,
                canBuildHeavy = false,
                canTerraform = false,
                moveCost = 1.4f,

                texture = new SpriteVariantCount(SpriteName.MissingImage),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = -1,

                TerraformToCost = new CraftBlueprint(CraftResultType.NoSet, 0, 0,
                    new UseResource[] {
                        new UseResource( ItemResourceType.Salt, 5)
                    }, XP.WorkExperienceType.NUM_NONE)
            };

            GroundProperties[(int)GroundType.Hardpan] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = true,
                canBuildHeavy = true,
                canTerraform = false,
                moveCost = 0.9f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureHardPan),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = -1,
            };

            GroundProperties[(int)GroundType.Frost] = new GroundTypeProperties()
            {
                canPlant = false,
                canBuildLight = true,
                canBuild = true,
                canBuildHeavy = false,
                canTerraform = false,
                moveCost = 1.6f,

                texture = new SpriteVariantCount(SpriteName.DssMapTextureFrozen),
                sidetexture = new SpriteVariantCount(SpriteName.WhiteArea_LFtiles),
                TerraformFromTimeSec = -1,
            };
        }
    }

    struct GroundTypeProperties
    {
        
        public bool canPlant;
        public bool canBuildLight;
        public bool canBuild;
        public bool canBuildHeavy;
        public bool canTerraform;

        public float moveCost;
        public bool badMountTerrain;

        public SpriteVariantCount texture;
        public SpriteVariantCount sidetexture;
        public float TerraformFromTimeSec;
        public ItemResource TerraformFromRest;
        public CraftBlueprint TerraformToCost;
    }

    enum GroundCanBuildType
    { 
        ExtraBonus,
        Yes,
        IfTerraformed,
        No,

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Map;
using VikingEngine.PJ.Moba.GO;

namespace VikingEngine.DSSWars.Map.Generate
{
    class CityTemplateCollection
    {       
        List<CityTemplate_Default> cityTemplates = new List<CityTemplate_Default>();
        CityTemplate_Default fallbackTemplate = null;

        public CityTemplateCollection()
        {
            {
                CityTemplate_Default cityTemplate = new CityTemplate_Default();
                cityTemplate.Init(IntVector2.Zero,
                    new string[]
                   {
                    "TWWWWWWT",
                    "W______W",
                    "W______W",
                    "WrrrHrrW",
                    "W__cXc_W",
                    "W__crc_W",
                    "W___r__W",
                    "TWWWGWWT",

                   }, false);
                cityTemplates.Add(cityTemplate);
            }
            {
                CityTemplate_Default cityTemplate = new CityTemplate_Default();
                cityTemplate.Init(IntVector2.Zero,
                    new string[]
                   {
                    "  WWWW  ",
                    " TW__WT ",
                    "WW_c_cWW",
                    "WrrrHrrW",
                    "W__cXc_W",
                    "WW__r_WW",
                    " TW_rWT ",
                    "  WWGW  ",

                   }, false);
                cityTemplates.Add(cityTemplate);
            }
            {
                CityTemplate_Default cityTemplate = new CityTemplate_Default();
                cityTemplate.Init(IntVector2.Zero,
                    new string[]
                   {
                    "TWWWWWWT",
                    "W___H__W",
                    "W___X__W",
                    "TWWWGWWT",
                    "w__crc_w",
                    "w__crc_w",
                    "w___r__w",
                    "wwwwrwww",

                   }, false);
                cityTemplates.Add(cityTemplate);
            }

            fallbackTemplate = new CityTemplate_Fallback();
        }

        public Grid2D<CityTemplateCellType> getTemplate(City city, WorldData world, out IntVector2 startSubTilePos)
        {
            List<CityTemplate_Default> templates = new List<CityTemplate_Default>(cityTemplates.Count);
            templates.AddRange(cityTemplates);

            while (templates.Count > 0)
            {
                CityTemplate_Default t = arraylib.RandomListMemberPop( templates, world.rnd );
                if (t.followsRequirements(city.tilePos, out int rotation, world))
                { 
                    return t.Get(rotation, out startSubTilePos);
                }                
            }

            return fallbackTemplate.Get(0, out startSubTilePos);
        }
    }

    class CityTemplate_Default
    {
        IntVector2 startSubTilePos = IntVector2.Zero;
        protected bool requireDryLand;
        protected bool requireWater;

        Grid2D<CityTemplateCellType>[] gridRotations = new Grid2D<CityTemplateCellType>[4];

        public Grid2D<CityTemplateCellType> Get(int templateRotation, out IntVector2 startSubTilePos)
        { 
            startSubTilePos = this.startSubTilePos;
            return gridRotations[templateRotation];
        }
        public void Init(IntVector2 startSubTilePos, string[] template, bool requireDryLand)
        {
            this.startSubTilePos = startSubTilePos;
            this.requireDryLand = requireDryLand;
            requireWater = false;
            int hallCount = 0, centerSquareCount = 0, craftCount = 0;

            this.startSubTilePos = startSubTilePos;
            int width = template[0].Length;

            Grid2D<CityTemplateCellType> grid = new Grid2D<CityTemplateCellType>(new IntVector2(width, template.Length));
            IntVector2 templatePos = IntVector2.Zero;
            for (templatePos.Y = 0; templatePos.Y < template.Length; templatePos.Y++)
            {
                string row = template[templatePos.Y];
                for (templatePos.X = 0; templatePos.X < row.Length; templatePos.X++)
                {
                    CityTemplateCellType type;
                    switch (row[templatePos.X])
                    {
                        default:
                            type = CityTemplateCellType.Empty;
                            break;

                        case '_':
                            type = CityTemplateCellType.General;
                            break;
                        case 'W':
                            type = CityTemplateCellType.Wall;
                            break;
                        case 'w':
                            type = CityTemplateCellType.OuterWall;
                            break;
                        case 'T':
                            type = CityTemplateCellType.Tower;
                            break;
                        case 'r':
                            type = CityTemplateCellType.Road;
                            break;
                        case 'c':
                            type = CityTemplateCellType.CraftArea;
                            craftCount++;
                            break;
                        case 'G':
                            type = CityTemplateCellType.Gate;
                            break;
                        case 'H':
                            type = CityTemplateCellType.CityHall;
                            hallCount++;
                            break;
                        case 'X':
                            type = CityTemplateCellType.CityCenterSquare;
                            centerSquareCount++;
                            break;
                    }

                    grid.Set(templatePos.X, templatePos.Y, type);
                }
            }

            if (hallCount != 1 || centerSquareCount != 1 || craftCount < 4)
            {
                throw new Exception("Incomplete template");
            }

            //GATES are always south going
            gridRotations[0] = grid;

            for (int rot = 1; rot < 4; rot++)
            {
                gridRotations[rot] = grid.Rotate(rot);
            }
        }

        virtual public bool followsRequirements(IntVector2 tilePos, out int templateRotation, WorldData world)
        {
            templateRotation = -1;
            checkTile(world, tilePos, out List<Dir4> availableGateRotations, out List<Dir4> availableHarborDirs, out int waterCount, out int landCount);

            if (requireDryLand && waterCount > 0)
            {
                return false;
            }
            if (requireWater && waterCount == 0)
            {
                return false;
            }

            if (availableGateRotations.Count > 0)
            {
                var dir = arraylib.RandomListMember(availableGateRotations, world.rnd);
                templateRotation = (int)lib.OppositeDir(dir);
                return true;
            }
            return false;
        }

        protected void checkTile(WorldData world, IntVector2 tilePos, out List<Dir4> availableGateRotations, out List<Dir4> availableHarborDirs, out int waterCount, out int landCount)
        {
            landCount = 0;
            waterCount = 0;
            availableGateRotations = new List<Dir4>();
            availableHarborDirs = new List<Dir4>();

            for (int i = 0; i < IntVector2.Dir4Array.Length; i++)
            {
                IntVector2 nPos = tilePos + IntVector2.Dir4Array[i];
                if (world.tileGrid.Get(nPos).IsLand())
                {
                    availableGateRotations.Add((Dir4)i);
                }
            }

            for (int i = 0; i < IntVector2.Dir8Array.Length; i++)
            {
                IntVector2 nPos = tilePos + IntVector2.Dir8Array[i];
                if (world.tileGrid.Get(nPos).IsLand())
                {
                    landCount++;
                }
                else
                {
                    waterCount++;
                }
            }
        }
    }

    class CityTemplate_Fallback : CityTemplate_Default
    {
        public CityTemplate_Fallback()
        {
            Init(IntVector2.Zero, new string[]
               {
                    "TWWWWWWT",
                    "W______W",
                    "W___r__W",
                    "WrrrHrrW",
                    "W__cXc_W",
                    "W__crc_W",
                    "W______W",
                    "TWWWWWWT",

               },
               false);
        }

        public override bool followsRequirements(IntVector2 tilePos, out int templateRotation, WorldData world)
        {
            templateRotation = 0;
            return true;
        }
    }

    enum CityTemplateCellType
    { 
        Empty,
        CityHall,
        CityCenterSquare,
        Tower,
        Wall,
        OuterWall,
        General,
        CraftArea,
        Road,
        Gate,
    }
}

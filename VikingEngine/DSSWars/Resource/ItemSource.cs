using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Resource
{
    struct ItemSource
    {
        public static readonly ItemSource None = new ItemSource() { source = ItemSourceType.NONE, sourceId = -1, };

        public ItemSourceType source;
        public int sourceId;

        public ItemSource(TerrainSubFoilType terrain)
        {
            source = ItemSourceType.Terrain;
            sourceId = (int)terrain;
        }

        public ItemSource(TerrainMineType mineType)
        {
            source = ItemSourceType.Mine;
            sourceId = (int)mineType;
        }

        public ItemSource(ItemSourceType source, BuildAndExpandType buiding1)
        {
            this.source = source;
            sourceId = (int)buiding1;
        }

        public ItemSource(BuildAndExpandType buiding1)
        {
            this.source = ItemSourceType.Crafting;
            sourceId = (int)buiding1;
        }

        public void ToHud(RichBoxContent content)
        {
            if (sourceId >= 0)
            {
                content.newLine();
                switch (source)
                {
                    case ItemSourceType.Terrain:
                        label(DssRef.lang.ItemSource_Terrain);
                        terrain(sourceId);

                        void terrain(int terrainType)
                        {
                            if (terrainType >= 0)
                            {
                                IconName.Terrain(TerrainMainType.Foil, terrainType, out var icon, out var name);
                                content.Add(new RbImage(icon));
                                content.hspace();
                                content.Add(new RbText(name));
                            }
                        }

                        break;

                    case ItemSourceType.Farm:
                        label(DssRef.lang.ItemSource_Farm);
                        addBuilding(sourceId);
                        break;

                    case ItemSourceType.Crafting:

                        label(DssRef.lang.ItemSource_CraftStation);
                        addBuilding(sourceId);
                        break;

                    case ItemSourceType.Mine:
                        label(DssRef.lang.ItemSource_Gathering);
                        IconName.Terrain(TerrainMainType.Mine, sourceId, out var icon, out var name);
                        content.Add(new RbImage(icon));
                        content.hspace();
                        content.Add(new RbText(name));
                        break;

                }

                void label(string typeName)
                {
                    content.Add(new RbText(typeName + ":", HudLib.TitleColor_Label));
                    content.space();
                }

                void addBuilding(int building)
                {
                    IconName.Building((BuildAndExpandType)building, out var icon, out var name);
                    content.Add(new RbImage(icon));
                    content.hspace();
                    content.Add(new RbText(TextLib.LargeFirstLetter(name)));
                }
            }
        }
    }
}

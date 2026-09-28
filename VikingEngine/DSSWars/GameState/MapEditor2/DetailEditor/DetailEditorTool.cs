using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameState.MapEditor2.IconEditor;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.DSSWars.GameState.MapEditor2.DetailEditor
{
    class DetailEditorTool
    {
        MapEditorPlayState state;

        public bool editGroundType = false;
        public bool editGroundTypeProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                editGroundType = value;
            }
            return editGroundType;
        }
        public GroundType groundType = GroundType.Default;

        public bool editHeight = true;
        public bool editHeightProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                editHeight = value;
            }
            return editHeight;
        }
        public bool setHeight = false;
        public bool setHeightProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                setHeight = value;
            }
            return setHeight;
        }
        int addHeightValue = 4;
        public int addHeightValueProperty(object tag, bool set, int value)
        {
            if (set)
            {
                addHeightValue = value;
            }
            return addHeightValue;
        }
        int setHeightValue = MapHeight2.WaterPlaneHeight + 10;
        public int setHeightValueProperty(object tag, bool set, int value)
        {
            if (set)
            {
                setHeightValue = value;
            }
            return setHeightValue;
        }


        public DetailEditorTool(MapEditorPlayState state)
        {
            this.state = state;
        }

        public bool actOnTile(IntVector2 subTilePos, bool commit)
        {
            if (commit)
            {
                switch (state.player.display.tab)
                {
                    case DetailEditorTab.Tiles:
                        ref var tile = ref DssRef.world.subTileGrid.GetRef(subTilePos);

                        bool bChange = false;
                        if (editHeight)
                        {
                            if (setHeight)
                            {
                                tile.heightValue = (byte)setHeightValue;
                            }
                            else
                            {
                                tile.heightValue = Bound.Byte(tile.heightValue + addHeightValue);
                            }
                            bChange = true;
                        }

                        if (editGroundType)
                        {
                            tile.groundType = groundType;
                            bChange = true;
                        }

                        if (bChange)
                        {
                            DssRef.world.chunkGrid.GetRef(WP.MaptileToChunk(subTilePos)).subtileVisualEdits++;
                            new Players.Orders.GodBuild(subTilePos);
                        }
                        break;
                }
            }
            return true;
        }
    }
}

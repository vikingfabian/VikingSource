using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using static System.Collections.Specialized.BitVector32;

namespace VikingEngine.DSSWars.Players
{
    class SelectedSubTile
    {
        public bool hasSelection = false;
        public IntVector2 subTilePos;
        public SubTile subTile;
        Mesh model;
        public bool isNew = false;
        public SelectTileResult selectTileResult = SelectTileResult.None;
        public City city;
        public SelectedSubTile(LocalPlayer player, bool isHover)
        {
            model = new Mesh(isHover ? LoadedMesh.SelectSquareDotted : LoadedMesh.SelectSquareSolid, Vector3.Zero, new Vector3(WorldData.SubTileWidth * 1.1f),
                TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);

            model.setVisibleCamera(player.playerData.localPlayerIndex);
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.Visible = false;
        }

        public void update(IntVector2 subTilePos, LocalPlayer player)
        {
            isNew = false;

            if (DssRef.world.subTileGrid.TryGet(subTilePos, out subTile))
            {
                if (this.subTilePos != subTilePos || DssRef.time.oneSecond)
                {
                    this.subTilePos = subTilePos;
                    isNew = true;

                    if (player.cityTab == Display.CityTab.Build)
                    {
                        selectTileResult = SelectTileResult.Build;
                        hasSelection = true;
                        model.position = WP.SubtileToWorldPosXZ_Centered(subTilePos);
                        model.position.Y = subTile.groundY;
                        return;
                    }
                    else
                    {
                        selectTileResult = SelectTileResult.None;
                        hasSelection = false;

                        if (DssRef.world.tileGrid.TryGet(WP.SubtileToTilePos(subTilePos), out var tile))
                        {
                            city = tile.City();
                            if (city.faction == player.faction)
                            {
                                switch (subTile.GeBuildingType())
                                {
                                    case Map.TerrainBuildingType.StoneHall:
                                        selectTileResult = SelectTileResult.CityHall;
                                        break;
                                    case Map.TerrainBuildingType.Square:
                                        selectTileResult = SelectTileResult.Resources;
                                        break;
                                }

                                hasSelection = selectTileResult != SelectTileResult.None;
                                model.position = WP.SubtileToWorldPosXZ_Centered(subTilePos);
                                model.position.Y = subTile.groundY;
                                return;
                            }

                        }
                    }
                }

            }
            else
            {
                selectTileResult = SelectTileResult.None;
                hasSelection = false;
            }
        }

        public bool viewSelection(bool view)
        {
            model.Visible = hasSelection && view;
            return model.Visible;
        }

        //public bool selectable(Faction playerFaction, out City city)
        //{
        //    selectTileResult = SelectTileResult.None;
        //    if (hasSelection)
        //    {
        //        if (DssRef.world.tileGrid.TryGet(WP.SubtileToTilePos(subTilePos), out var tile))
        //        {
        //            city = tile.City();
        //            if (city.faction == playerFaction)
        //            {
        //                switch (subTile.GeBuildingType())
        //                {
        //                    case Map.TerrainBuildingType.StoneHall:
        //                        selectTileResult = SelectTileResult.CityHall;
        //                        break;
        //                    case Map.TerrainBuildingType.Square:
        //                        selectTileResult = SelectTileResult.Resources;
        //                        break;
        //                }

        //                return selectTileResult != SelectTileResult.None;
        //            }
        //        }
        //    }

        //    city = null;
        //    return false;
        //}
    }

    enum SelectTileResult
    { 
        None,
        CityHall,
        Resources,
        
        Build,
    }
}

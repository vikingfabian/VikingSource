using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.Build;
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
        public bool tileOfInterest = false;
        public City city;
        public SelectedSubTile(LocalPlayer player, bool isHover)
        {
            model = CreateOutlineModel(player, isHover);
            //model = new Mesh(isHover ? LoadedMesh.SelectSquareDotted : LoadedMesh.SelectSquareSolid, Vector3.Zero, new Vector3(WorldData.SubTileWidth * 1.1f),
            //    TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);

            //model.setVisibleCamera(player.playerData.localPlayerIndex);
            //model.AddToRender(DrawGame.UnitDetailLayer);
            //model.Visible = false;
        }

        public static Mesh CreateOutlineModel(LocalPlayer player, bool isHover)
        {
            LoadedMesh loadedMesh;
            float scale;

            if (isHover)
            {
                loadedMesh = LoadedMesh.SelectSquareDotted;
                scale = WorldData.SubTileWidth * 1.1f;
            }
            else
            {
                loadedMesh = LoadedMesh.SelectSquareSolid;
                scale = WorldData.SubTileWidth * 1.0f;
            }

            var model = new Mesh(loadedMesh, Vector3.Zero, new Vector3(scale),
                TextureEffectType.Flat, SpriteName.WhiteArea, Color.White, false);

            model.setVisibleCamera(player.playerData.localPlayerIndex);
            model.AddToRender(DrawGame.UnitDetailLayer);
            model.Visible = false;

            return model;
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
                    tileOfInterest = false;
                    if (DssRef.world.tileGrid.TryGet(WP.SubtileToTilePos(subTilePos), out var tile))
                    {
                        city = tile.City();
                    }

                    if (player.InBuildOrdersMode())
                    {
                        selectTileResult = player.buildControls.buildMode;
                        hasSelection = true;
                        model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
                        //model.position.Y = subTile.groundY;

                        
                        return;
                    }
                    else
                    {
                        selectTileResult = SelectTileResult.None;
                        hasSelection = false;

                        if (city != null) 
                        {
                            if (city.faction == player.faction)
                            {
                                switch (subTile.mainTerrain)
                                {
                                    case TerrainMainType.Building:
                                        switch ((TerrainBuildingType)subTile.subTerrain)
                                        {
                                            case Map.TerrainBuildingType.StoneHall:
                                                selectTileResult = SelectTileResult.CityHall;
                                                break;
                                            case Map.TerrainBuildingType.Postal:
                                            case Map.TerrainBuildingType.PostalLevel2:
                                            case Map.TerrainBuildingType.PostalLevel3:
                                                selectTileResult = SelectTileResult.Postal;
                                                break;
                                            case Map.TerrainBuildingType.Recruitment:
                                            case Map.TerrainBuildingType.RecruitmentLevel2:
                                            case Map.TerrainBuildingType.RecruitmentLevel3:
                                                selectTileResult = SelectTileResult.Recruitment;
                                                break;

                                            case Map.TerrainBuildingType.SoldierBarracks:
                                            case Map.TerrainBuildingType.ArcherBarracks:
                                            case Map.TerrainBuildingType.WarmashineBarracks:
                                            case Map.TerrainBuildingType.KnightsBarracks:
                                            case Map.TerrainBuildingType.GunBarracks:
                                            case Map.TerrainBuildingType.CannonBarracks:
                                                selectTileResult = SelectTileResult.Conscript;
                                                break;

                                            case Map.TerrainBuildingType.School:
                                                selectTileResult = SelectTileResult.School;
                                                break;
                                        }

                                        hasSelection = selectTileResult != SelectTileResult.None;
                                        model.position = WP.SubtileToWorldPosXZ_Centered(subTilePos);
                                        model.position.Y = subTile.groundY;
                                        return;

                                    case TerrainMainType.Mine:
                                        tileOfInterest = true;
                                        break;

                                    case TerrainMainType.Foil:
                                        switch ((TerrainSubFoilType)subTile.subTerrain)
                                        {
                                            case TerrainSubFoilType.BogIron:
                                                tileOfInterest = true;
                                                break;
                                        }
                                        break;
                                }
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

        public MayBuildResult mayBuild(LocalPlayer player, out bool upgrade)
        {
            return MayBuild(subTilePos, player, out upgrade, out _);
        }

        public static MayBuildResult MayBuild(IntVector2 subTilePos, LocalPlayer player, out bool upgrade, out City city)
        {
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out var subTile))
            { 
                    if (DssRef.world.tileGrid.TryGet(WP.SubtileToTilePos(subTilePos), out var tile))
                    {
                        city = tile.City();

                        return MayBuild(city, subTile, player, out upgrade);
                    }
            }

            city = null;
            upgrade = false;
            return MayBuildResult.ERR;
        }

        public static MayBuildResult MayBuild(City city, SubTile subTile, LocalPlayer player, out bool upgrade)
        {
            upgrade = false;
            if (city != null)
            {
                if (city.faction.player == player)
                {
                    //var current = subTile.GeBuildingType();
                    if (subTile.MayBuild(player.buildControls.placeBuildingType, out upgrade))
                    {
                        if (player.mapControls.selection.obj == city)
                        {
                            return MayBuildResult.Yes;
                        }
                        else
                        { 
                            return MayBuildResult.Yes_ChangeCity;
                        }
                    }
                    else
                    { 
                        return MayBuildResult.No_Occupied;
                    }
                }
            }

            return MayBuildResult.No_OutsideRegion;
        }

        //public MayBuildResult MayBuild(LocalPlayer player, out bool upgrade)
        //{
        //    upgrade = false;
        //    if (city != null)
        //    {
        //        if (city.faction.player == player)
        //        {
        //            //var current = subTile.GeBuildingType();
        //            if (subTile.MayBuild(player.buildControls.placeBuildingType, out upgrade))
        //            {
        //                if (player.mapControls.selection.obj == city)
        //                {
        //                    return MayBuildResult.Yes;
        //                }
        //                else
        //                {
        //                    return MayBuildResult.Yes_ChangeCity;
        //                }
        //            }
        //            else
        //            {
        //                return MayBuildResult.No_Occupied;
        //            }
        //        }
        //    }

        //    return MayBuildResult.No_OutsideRegion;
        //}

        public static bool MayDemolish(IntVector2 subTilePos, LocalPlayer player, out City city)
        {
            if (DssRef.world.subTileGrid.TryGet(subTilePos, out var subTile))
            {
                IntVector2 tilePos = WP.SubtileToTilePos(subTilePos);
                if (DssRef.world.tileGrid.TryGet(tilePos, out var tile))
                {
                    city = tile.City();
                    
                        if (city.faction.player == player)
                        {
                            if (tilePos != city.tilePos) //center tile is protected
                            {
                                var buildingType = BuildLib.GetType(subTile.mainTerrain, subTile.subTerrain);
                                if (buildingType != BuildAndExpandType.NUM_NONE)
                                {
                                    return true;
                                }
                            }
                        }
                    
                }
            }
            city = null;
            return false;
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
        Postal,
        Recruitment,
        Conscript,
        School,

        //Resources,
        
        Build,
        ClearTerrain,
        Demolish,
    }

    enum MayBuildResult
    { 
        ERR,
        Yes,
        YesUpgrade,
        Yes_ChangeCity,
        No_OutsideRegion,
        No_Occupied,

    }
}

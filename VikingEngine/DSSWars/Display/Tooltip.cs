﻿using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.DSSWars.Display
{
    class Tooltip
    {
        public const int Food_BlueprintId = 1;

        Graphics.ImageGroup images = new Graphics.ImageGroup(128);
        bool current_menuToolTip;
        public bool refresh = false;
        Vector2 size;
        public int tooltip_id = int.MinValue;
        public int tooltip_id_timesec;
        public void updateMapTip(Players.LocalPlayer player, bool refreshTime)
        {
            if (current_menuToolTip && images.HasMembers)
            {
                images.DeleteAll();
                current_menuToolTip = false;
            }
            if (player.diplomacyMap == null)
            {
                if (player.mapControls.hover.isNew 
                    || player.mapControls.hover.subTile.isNew 
                    || refreshTime)
                {
                    images.DeleteAll();

                    var order = player.orders.orderOnSubTile(player.mapControls.hover.subTile.subTilePos);
                    if (order != null)
                    {
                        hoverTip(player, order);
                    }
                    else if (player.mapControls.hover.subTile.hasSelection)
                    {
                        //SUBTILE tooltip
                        hoverTip(player, player.mapControls.hover.subTile);
                    }
                    else if (player.mapControls.hover.obj != null)
                    {
                        hoverTip(player, player.mapControls.hover.obj);
                    }
                    else if (player.mapControls.hover.subTile.tileOfInterest)
                    {
                        //SUBTILE tooltip
                        hoverTip(player, player.mapControls.hover.subTile);
                    }

                }
            }
            else
            {
                if (!player.diplomacyMap.hasSelection())
                {
                    images.DeleteAll();
                }
            }
            baseUpdate(player, false);
        }

        void baseUpdate(Players.LocalPlayer player, bool menuToolTip)
        {
            if (images.HasMembers)
            {
                if (player.input.inputSource.IsController)
                {
                    if (!images.HasOffset())
                    {
                        if (menuToolTip)
                        {
                            //images.SetOffset(new Vector2(
                            //    player.hud.displays.headDisplay.area.Right + 10,
                            //    player.hud.displays.controllerSelectionPos().Y)
                            //    );
                        }
                        else
                        {
                            images.SetOffset(player.playerData.view.DrawAreaF.Center + Engine.Screen.SmallIconSizeV2);
                        }
                    }
                }
                else
                {
                    Vector2 offset = Input.Mouse.Position + Engine.Screen.SmallIconSizeV2;
                    Vector2 maxPos = offset + size;

                    if (maxPos.X > Engine.Screen.SafeArea.Right)
                    { 
                        offset.X = Input.Mouse.Position.X - (Engine.Screen.SmallIconSize + size.X);
                    }

                    if (maxPos.Y > Engine.Screen.SafeArea.Bottom)
                    {
                        offset.Y = Engine.Screen.SafeArea.Bottom - size.Y;
                    }

                    images.SetOffset(offset);
                }                
            }
        }

        public void updateDiplayTip(Players.LocalPlayer player, bool hoversButton)
        {
            if (!hoversButton)
            {
                images.DeleteAll();
            }
            else
            {
                baseUpdate(player, hoversButton);
            }
        }

        void hoverTip(Players.LocalPlayer player, AbsOrder order)
        {
            RichBoxContent content = order.ToHud();
            create(player, content, false);
        }

        void hoverTip(Players.LocalPlayer player, Players.SelectedSubTile subTile)
        {
            RichBoxContent content = new RichBoxContent();
            if (subTile.selectTileResult != Players.SelectTileResult.None)
            {
                content.Add(new RbBeginTitle(2));
                content.Add(new RbImage(player.input.Select.Icon));

                RbText title = null;
                bool avaialableAction = true;
                switch (subTile.selectTileResult)
                {
                    case Players.SelectTileResult.Build:
                        var buildOpt = BuildLib.BuildOptions[(int)player.buildControls.placeBuildingType];
                        title = new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Build_PlaceBuilding, buildOpt.Label()));
                        content.Add(title);
                        content.newLine();
                        //CraftBlueprint blueprint = ResourceLib.Blueprint(player.BuildControls.placeBuildingType);
                        var bp = player.buildControls.placeBuildingOption().blueprint;
                        bp.toMenu(content, subTile.city);

                        var mayBuild = player.mapControls.hover.subTile.MayBuild(player, out bool upgrade);
                        
                        switch (mayBuild)
                        { 
                            case Players.MayBuildResult.Yes_ChangeCity:
                                content.text(DssRef.lang.BuildHud_OutsideCity).overrideColor = HudLib.InfoYellow_Light; 
                                break;

                            case Players.MayBuildResult.No_OutsideRegion:
                                avaialableAction = false;
                                content.text(DssRef.lang.BuildHud_OutsideFaction).overrideColor = HudLib.NotAvailableColor;
                                break;

                            case Players.MayBuildResult.No_Occupied:
                                avaialableAction = false;
                                content.text(DssRef.lang.BuildHud_OccupiedTile).overrideColor = HudLib.NotAvailableColor;
                                break;
                        }

                        if (subTile.city.buildingStructure.buildingLevel_logistics < 2)
                        {
                            content.text(string.Format(DssRef.lang.BuildHud_Queue, player.orders.buildQueue(subTile.city), subTile.city.MaxBuildQueue())).overrideColor = subTile.city.availableBuildQueue(player) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        }
                        buildOpt.blueprint.requirementToHud(content, subTile.city, out _);

                        content.Add(new RbSeperationLine());
                        content.newParagraph();
                        content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                        bp.listResources(content, subTile.city);

                        break;
                    case Players.SelectTileResult.Demolish:
                        title = new RbText(DssRef.lang.Build_DestroyBuilding);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.ClearTerrain:
                        title = new RbText(DssRef.lang.Build_ClearTerrain);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.CityHall:
                        title = new RbText(DssRef.lang.Hud_SelectCity);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.Postal:
                        {
                            title = new RbText(DssRef.lang.BuildingType_Postal);                            
                            content.Add(title);

                            content.newLine();
                            if (subTile.city.GetDelivery(subTile.subTilePos, out DeliveryStatus status))
                            {
                                status.tooltip(player, subTile.city, content);
                            }
                        }
                        break;
                    case Players.SelectTileResult.Recruitment:
                        {
                            title = new RbText(DssRef.lang.BuildingType_Recruitment);
                            content.Add(title);

                            content.newLine();
                            if (subTile.city.GetDelivery(subTile.subTilePos, out DeliveryStatus status))
                            {
                                status.tooltip(player, subTile.city, content);
                            }
                        }
                        break;
                    case Players.SelectTileResult.School:
                        {
                            title = new RbText(DssRef.todoLang.BuildingType_School);
                            content.Add(title);

                            content.newLine();
                            //if (subTile.city.GetDelivery(subTile.subTilePos, out DeliveryStatus status))
                            //{
                            //    status.tooltip(player, subTile.city, content);
                            //}
                        }
                        break;
                    case Players.SelectTileResult.Conscript:
                        {
                            title = new RbText(DssRef.lang.Conscription_Title);
                            content.Add(title);

                            content.newLine();
                            if (subTile.city.GetConscript(subTile.subTilePos, out BarracksStatus status))
                            {
                                status.tooltip(player, subTile.city, content);
                            }
                        }
                        break;
                }
                title.overrideColor = avaialableAction ? HudLib.TitleColor_Action: HudLib.NotAvailableColor;

                content.Add(new RbSeperationLine());
                content.newParagraph();
             
            }
            content.h2(DssRef.lang.TerrainType).overrideColor = HudLib.TitleColor_TypeName;
            content.text(subTile.subTile.TypeToString());
            
            create(player, content, false);
        }

        void hoverTip(Players.LocalPlayer player, GameObject.AbsGameObject obj)
        {
            RichBoxContent content = new RichBoxContent();

            bool attackTarget = player.armyControls != null &&
                obj.GetFaction() != player.faction;

            if (attackTarget)
            {
                content.h2(DssRef.lang.ArmyOption_Attack).overrideColor = HudLib.TitleColor_Attack;
            }

            string name = obj.Name(out _);
            if (name != null)
            {
                content.text(name).overrideColor = HudLib.TitleColor_Name;
            }
            content.h2(obj.TypeName()).overrideColor = HudLib.TitleColor_TypeName;

            if (obj.GetFaction() != player.faction)
            {
                var relation = DssRef.diplomacy.GetRelationType(player.faction, obj.GetFaction());

                content.newLine();
                content.Add(new RbText(obj.GetFaction().PlayerName, HudLib.TitleColor_Name));
                content.newLine();
                content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
                content.Add(new RbText(Diplomacy.RelationString(relation), HudLib.TextColor_Relation));
                content.newLine();
                
            }

            if (attackTarget)
            {
                if (!DssRef.diplomacy.InWar(player.faction, obj.GetFaction()))
                {
                    content.Add(new RbSeperationLine());

                    RelationType rel = DssRef.diplomacy.GetRelationType(player.faction, obj.GetFaction());
                    
                    content.h1(DssRef.lang.Hud_WardeclarationTitle);
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                    content.newLine();
                    HudLib.ResourceCost(content, ResourceType.DiplomaticPoint, Diplomacy.DeclareWarCost(rel), player.diplomaticPoints.Int());
                    content.Add(new RbSeperationLine());
                }
                else
                {
                    content.newParagraph();
                }

                var attacker = player.mapControls.selection.obj as Army;
                var defender = obj as AbsMapObject;
                if (attacker != null && defender!= null)
                {
                    content.Add(new RbBeginTitle(2));
                    content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                    content.Add(new RbText(string.Format(DssRef.lang.Hud_StrengthRating, string.Empty)));//"Strength ratings:"));
                    
                    content.newLine();
                    content.Add(new RbTexture(player.faction.flagTexture, 1f, 0, 0.2f));
                    
                    content.Add(new RbText(": " + TextLib.OneDecimal(attacker.strengthValue)));//string.Format(HudLib.OneDecimalFormat, attacker.strengthValue)));
                    content.newLine();
                    content.text(DssRef.lang.Hud_Versus);
                    content.newLine();
                    content.Add(new RbTexture(obj.GetFaction().flagTexture, 1f, 0, 0.2f));
                    content.Add(new RbText(": " + TextLib.OneDecimal(defender.strengthValue)));
                    content.newLine();
                }
            }
            else
            {
                switch (obj.gameobjectType())
                {
                    case GameObjectType.City:
                        {   
                            var mapObj = obj as AbsMapObject;
                            if (mapObj != null)
                            {
                                const int LowAmount = 10;
                                var city = mapObj.GetCity();
                                content.newLine();
                                HudLib.CityResource(content, city, ItemResourceType.Food_G);

                                if (city.res_food.amount <= LowAmount)
                                {
                                    if (city.res_water.amount <= 2)
                                    {
                                        HudLib.CityResource(content, city, ItemResourceType.Water_G);
                                    }
                                    if (city.res_rawFood.amount <= LowAmount)
                                    {
                                        HudLib.CityResource(content, city, ItemResourceType.RawFood_Group);
                                    }
                                    if (city.res_fuel.amount <= LowAmount)
                                    {
                                        HudLib.CityResource(content, city, ItemResourceType.Fuel_G);
                                    }
                                }


                                warStrength(mapObj);
                            }
                        }
                        break;

                    case GameObjectType.Army:
                        {
                            var mapObj = obj as AbsMapObject;
                            if (mapObj != null)
                            {
                                
                                var army = obj.GetArmy();
                                if (army.food < army.foodUpkeep * 2)
                                {
                                    HudLib.ItemCount(content, SpriteName.WarsResource_Food, DssRef.lang.Resource_TypeName_Food, TextLib.OneDecimal(army.food));
                                }
                                warStrength(mapObj);
                                //content.newLine();
                                //content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                                //content.Add(new RichBoxText(TextLib.OneDecimal(mapObj.strengthValue)));

                                content.newLine();
                                content.Add(new RbImage(SpriteName.WarsGroupIcon));
                                content.space(1);

                                
                                var typeCounts = army.Status().getTypeCounts_Sorted(army.faction);

                                foreach (var kv in typeCounts)
                                {
                                    //AbsSoldierData typeData = DssRef.unitsdata.Get(kv.Key);
                                    content.Add(new RbText(kv.Value.ToString()));
                                    content.Add(new RbImage(AllUnits.UnitFilterIcon(kv.Key)));
                                    content.space(2);
                                }

                            }
                        }
                        break;

                    case GameObjectType.ObjectCollection:
                        obj.GetCollection().Tooltip(content);
                        break;

                    case GameObjectType.Worker:
                        obj.GetWorker().toolTip(content);
                        break;
                }

                void warStrength(AbsMapObject mapObj)
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                    content.Add(new RbText(TextLib.OneDecimal(mapObj.strengthValue)));
                }
            }
            
            create(player, content, false);
        }

        public void create(Players.LocalPlayer player, List<AbsRichBoxMember> content, bool menuToolTip, int tooltip_id = -1)
        {
            images.DeleteAll();
            current_menuToolTip = menuToolTip;

            float edge = Engine.Screen.BorderWidth;
            float width = Engine.Screen.IconSize * 8;

            RichBoxGroup richBox = new RichBoxGroup(new Vector2(edge),
                width, ImageLayers.Lay3, HudLib.RbSettings, content);

            var area = richBox.maxArea;
            area.AddRadius(edge);

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size,
                ImageLayers.Lay4);
            bg.ColorAndAlpha(Color.Black, 0.95f);
            size = area.Size;

            images.Add(bg);
            images.Add(richBox);

            baseUpdate(player, menuToolTip);

            ++tooltip_id_timesec;
            if (this.tooltip_id != tooltip_id)
            { 
                this.tooltip_id = tooltip_id;
                tooltip_id_timesec = 0;
            }
        }
    }
}

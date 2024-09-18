using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.CarBall;

namespace VikingEngine.DSSWars.Display
{
    class Tooltip
    {
        Graphics.ImageGroup images = new Graphics.ImageGroup(128);
        public bool refresh = false;
        Vector2 size;
        public void updateMapTip(Players.LocalPlayer player, bool refreshTime)
        {
            if (player.diplomacyMap == null)
            {
                if (player.mapControls.hover.isNew 
                    || player.mapControls.hover.subTile.isNew 
                    || refreshTime)
                {
                    images.DeleteAll();

                    if (player.mapControls.hover.subTile.hasSelection)
                    {
                        hoverTip(player, player.mapControls.hover.subTile);
                    }
                    else if (player.mapControls.hover.obj != null)
                    {
                        hoverTip(player, player.mapControls.hover.obj);
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
                            images.SetOffset(VectorExt.AddX(player.hud.displays.headDisplay.area.RightBottom, 10));
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

        void hoverTip(Players.LocalPlayer player, Players.SelectedSubTile subTile)
        {
            RichBoxContent content = new RichBoxContent();
            if (subTile.selectTileResult != Players.SelectTileResult.None)
            {
                content.Add(new RichBoxBeginTitle(2));
                content.Add(new RichBoxImage(player.input.Select.Icon));

                RichBoxText title = null;
                bool avaialableAction = true;
                switch (subTile.selectTileResult)
                {
                    case Players.SelectTileResult.Build:
                        title = new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Build_PlaceBuilding, player.BuildControls.placeBuildingType));
                        content.Add(title);
                        content.newLine();
                        //CraftBlueprint blueprint = ResourceLib.Blueprint(player.BuildControls.placeBuildingType);
                        player.BuildControls.placeBuildingType.blueprint.toMenu(content);

                        var mayBuild = player.mapControls.hover.subTile.MayBuild(player);
                        
                        switch (mayBuild)
                        { 
                            case Players.MayBuildResult.Yes_ChangeCity:
                                content.text(DssRef.todoLang.BuildHud_OutsideCity).overrideColor = HudLib.InfoYellow_Light; 
                                break;

                            case Players.MayBuildResult.No_OutsideRegion:
                                avaialableAction = false;
                                content.text(DssRef.todoLang.BuildHud_OutsideFaction).overrideColor = HudLib.NotAvailableColor;
                                break;

                            case Players.MayBuildResult.No_Occupied:
                                avaialableAction = false;
                                content.text(DssRef.todoLang.BuildHud_OccupiedTile).overrideColor = HudLib.NotAvailableColor;
                                break;
                        }
                        
                        break;
                    case Players.SelectTileResult.Destroy:
                        title = new RichBoxText(DssRef.todoLang.Build_DestroyBuilding);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.ClearTerrain:
                        title = new RichBoxText(DssRef.todoLang.Build_ClearTerrain);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.CityHall:
                        title = new RichBoxText(DssRef.todoLang.Hud_SelectCity);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.Postal:
                        title = new RichBoxText(DssRef.todoLang.BuildingType_Postal);
                        content.Add(title);
                        break;
                    case Players.SelectTileResult.Recruitment:
                        title = new RichBoxText(DssRef.todoLang.BuildingType_Recruitment);
                        content.Add(title);
                        break;
                    case Players.SelectTileResult.Barracks:
                        title = new RichBoxText(DssRef.todoLang.Conscription_Title);
                        content.Add(title);
                        break;
                }
                title.overrideColor = avaialableAction ? HudLib.TitleColor_Action: HudLib.NotAvailableColor;

                content.Add(new RichBoxSeperationLine());
                content.newParagraph();
             
            }
            content.h2(DssRef.todoLang.TerrainType).overrideColor = HudLib.TitleColor_TypeName;
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

            string name = obj.Name();
            if (name != null)
            {
                content.text(name).overrideColor = HudLib.TitleColor_Name;
            }
            content.h2(obj.TypeName()).overrideColor = HudLib.TitleColor_TypeName;

            if (obj.GetFaction() != player.faction)
            {
                var relation = DssRef.diplomacy.GetRelationType(player.faction, obj.GetFaction());

                content.newLine();
                content.Add(new RichBoxText(obj.GetFaction().PlayerName, HudLib.TitleColor_Name));
                content.newLine();
                content.Add(new RichBoxImage(Diplomacy.RelationSprite(relation)));
                content.Add(new RichBoxText(Diplomacy.RelationString(relation), HudLib.TextColor_Relation));
                content.newLine();
                
            }

            if (attackTarget)
            {
                if (!DssRef.diplomacy.InWar(player.faction, obj.GetFaction()))
                {
                    content.Add(new RichBoxSeperationLine());

                    RelationType rel = DssRef.diplomacy.GetRelationType(player.faction, obj.GetFaction());
                    
                    content.h1(DssRef.lang.Hud_WardeclarationTitle);
                    content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                    content.newLine();
                    HudLib.ResourceCost(content, GameObject.Resource.ResourceType.DiplomaticPoint, Diplomacy.DeclareWarCost(rel), player.diplomaticPoints.Int());
                    content.Add(new RichBoxSeperationLine());
                }
                else
                {
                    content.newParagraph();
                }

                var attacker = player.mapControls.selection.obj as Army;
                var defender = obj as AbsMapObject;
                if (attacker != null && defender!= null)
                {
                    content.Add(new RichBoxBeginTitle(2));
                    content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                    content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_StrengthRating, string.Empty)));//"Strength ratings:"));
                    
                    content.newLine();
                    content.Add(new RichBoxTexture(player.faction.flagTexture, 1f, 0, 0.2f));
                    
                    content.Add(new RichBoxText(": " + TextLib.OneDecimal(attacker.strengthValue)));//string.Format(HudLib.OneDecimalFormat, attacker.strengthValue)));
                    content.newLine();
                    content.text(DssRef.lang.Hud_Versus);
                    content.newLine();
                    content.Add(new RichBoxTexture(obj.GetFaction().flagTexture, 1f, 0, 0.2f));
                    content.Add(new RichBoxText(": " + TextLib.OneDecimal(defender.strengthValue)));
                    content.newLine();
                }
            }
            else
            {
                switch (obj.gameobjectType())
                {
                    case GameObjectType.City:
                    case GameObjectType.Army:
                        var mapObj = obj as AbsMapObject;
                        if (mapObj != null)
                        {
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                            content.Add(new RichBoxText(TextLib.OneDecimal(mapObj.strengthValue)));
                            if (obj.gameobjectType() == GameObjectType.Army)
                            {
                                content.newLine();
                                content.Add(new RichBoxImage(SpriteName.WarsGroupIcon));
                                content.space(1);


                                var typeCounts = obj.GetArmy().Status().getTypeCounts_Sorted();

                                //foreach (var kv in typeCounts)
                                //{
                                //    AbsSoldierData typeData = DssRef.unitsdata.Get(kv.Key);
                                //    content.Add(new RichBoxText(kv.Value.ToString()));
                                //    content.Add(new RichBoxImage(typeData.icon));
                                //    content.space(2);
                                //}
                            }
                        }
                        break;

                    case GameObjectType.ObjectCollection:
                        obj.GetCollection().Tooltip(content);
                        break;
                }
            }
            
            create(player, content, false);
        }

        public void create(Players.LocalPlayer player, List<AbsRichBoxMember> content, bool menuToolTip)
        {
            images.DeleteAll();

            float edge = Engine.Screen.BorderWidth;
            float width = Engine.Screen.IconSize * 8;

            RichBoxGroup richBox = new RichBoxGroup(new Vector2(edge),
                width, ImageLayers.Lay3, HudLib.RbSettings, content);

            var area = richBox.maxArea;
            area.AddRadius(edge);

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size,
                ImageLayers.Lay4);
            bg.ColorAndAlpha(Color.Black, 0.7f);
            size = area.Size;

            images.Add(bg);
            images.Add(richBox);

            baseUpdate(player, menuToolTip);
        }
    }
}

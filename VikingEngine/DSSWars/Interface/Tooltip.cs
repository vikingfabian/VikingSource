using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;

using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.NPC;
using VikingEngine.PJ.CarBall;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Interface
{
    class Tooltip
    {
        public const int Food_BlueprintId = 1;

        Graphics.ImageGroup images = new Graphics.ImageGroup(128);
        bool current_menuToolTip;
        public bool refresh = false;
        Vector2 size;
        public static int tooltip_id = int.MinValue;
        public static float tooltip_id_timestampsec;
        public void updateMapTip(Players.LocalPlayer player, bool refreshTime, bool aboveMouse)
        {
            if (player.gameControls.input.mousePan.IsDown)
            {
                images.DeleteAll();
            }
            else if (player.gameControls.diplomacy == null && !player.gameControls.input.mousePan.IsDown)
            {
                if (player.gameControls.map.HasRectangleSelect())
                {
                    if (player.gameControls.map.hover.obj != null)
                    {
                        hoverTip(player, player.gameControls.map.hover.obj);
                    }
                    else
                    {
                        images.DeleteAll();
                    }
                }
                else if (player.gameControls.map.hover.isNew 
                    || player.gameControls.map.hover.subTile.isNew 
                    || refreshTime)
                {
                    images.DeleteAll();

                    var order = player.orders.orderOnSubTile(player.gameControls.map.hover.subTile.subTilePos);
                    if (order != null)
                    {
                        hoverTip(player, order);
                    }
                    else if (player.gameControls.map.hover.subTile.hasSelection)
                    {
                        //SUBTILE tooltip
                        hoverTip(player, player.gameControls.map.hover.subTile);
                    }
                    else if (player.gameControls.map.hover.obj != null)
                    {
                        hoverTip(player, player.gameControls.map.hover.obj);
                    }
                    else if (player.gameControls.map.hover.subTile.tileOfInterest)
                    {
                        //SUBTILE tooltip
                        hoverTip(player, player.gameControls.map.hover.subTile);
                    }

                }
            }
            else
            {
                //Relation arrow
                if (player.gameControls.diplomacy.relationArrowHover.HasValue())
                {
                    RichBoxContent content = new RichBoxContent();

                    //input(map.mouseSelect.Icon, DssRef.lang.InputActionName_ControllerSelect);
                    content.newLine();
                    content.Add(new RbImage(player.gameControls.input.mouseSelect.Icon));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_SelectInput, HudLib.TitleColor_Action));


                    content.newLine();
                    content.h2(DssRef.lang.Diplomacy_RelationWithOthers, HudLib.TitleColor_Label);
                    content.newLine();

                    Faction thirdPartFaction = player.gameControls.diplomacy.relationArrowHover.GetFaction();
                    var relation = DssRef.world.diplomacy.GetRelation(player.gameControls.diplomacy.mainSelection(out _).pfaction, thirdPartFaction.pfaction).Relation;

                    content.Add(thirdPartFaction.FlagTextureToHud());
                    content.hspace();
                    content.Add(new RbText(thirdPartFaction.PlayerName));

                    content.Add(new RbText(": "));
                    IconName.Relation(relation, out SpriteName relIcon, out string relName);
                    content.Add(new RbImage(relIcon));
                    content.hspace();
                    content.Add(new RbText(relName));

                    create(player, content, false);
                }
                else if (!player.gameControls.diplomacy.hasSelection())
                {
                    images.DeleteAll();
                }
            }
            baseUpdate(player, false, aboveMouse);
        }

        void baseUpdate(Players.LocalPlayer player, bool menuToolTip, bool aboveMouse)
        {
            if (images.HasMembers)
            {
                //if (player.gameControls.input.inputSource.IsXnaController)
                //{
                //    if (!images.HasOffset())
                //    {
                //        if (menuToolTip)
                //        {
                //            //images.SetOffset(new Vector2(
                //            //    player.hud.displays.headDisplay.area.Right + 10,
                //            //    player.hud.displays.controllerSelectionPos().Y)
                //            //    );
                //        }
                //        else
                //        {
                //            images.SetOffset(player.playerData.view.DrawAreaF.Center + Engine.Screen.SmallIconSizeV2);
                //        }
                //    }
                //}
                //else
                {
                    Vector2 offset = player.gameControls.input.mouse.Position;// + Engine.Screen.SmallIconSizeV2;
                    offset.X += Engine.Screen.IconSize;
                    if (aboveMouse)
                    {
                        offset.Y -= 5 + size.Y;
                    }
                    else
                    {
                        offset.Y += Engine.Screen.SmallIconSize;
                    }
                    

                    Vector2 maxPos = offset + size;

                    if (maxPos.X > Engine.Screen.SafeArea.Right)
                    { 
                        offset.X = player.gameControls.input.mouse.Position.X - (Engine.Screen.IconSize + size.X);
                    }

                    if (offset.Y < Engine.Screen.SafeArea.Y)
                    {
                        offset.Y = Engine.Screen.SafeArea.Y;
                    }
                    else if (maxPos.Y > Engine.Screen.SafeArea.Bottom)
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
                baseUpdate(player, hoversButton, false);
            }
        }

        void hoverTip(Players.LocalPlayer player, AbsOrder order)
        {
            RichBoxContent content = order.ToHud();
            create(player, content, false);
        }

        void hoverTip(Players.LocalPlayer player, Players.SelectedSubTile subTile)
        {
            if (StartupSettings.BlockTooltip) return;

            RichBoxContent content = new RichBoxContent();
            if (subTile.selectTileResult != Players.SelectTileResult.None)
            {
                content.Add(new RbBeginTitle(2));
                content.Add(new RbImage(player.gameControls.input.mouseSelect.Icon));
                content.space(0.5f);

                RbText title = null;
                bool avaialableAction = true;
                switch (subTile.selectTileResult)
                {

                    case Players.SelectTileResult.Build:
                        var buildOpt = BuildLib.BuildOptions[(int)player.gameControls.build.placeBuildingType];
                        title = new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Build_PlaceBuilding, buildOpt.Label()));
                        content.Add(title);

                        cancelInput();

                        //CraftBlueprint blueprint = ResourceLib.Blueprint(player.BuildControls.placeBuildingType);
                        content.newLine();
                        var bp = player.gameControls.build.placeBuildingOption().blueprint;
                        bp.toMenu(content, subTile.city);

                        var mayBuild = player.gameControls.map.hover.subTile.mayBuild(player, out bool upgrade);
                         mayBuild = player.gameControls.build.adjustMayBuild(mayBuild);
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

                        //if (subTile.city.buildingStructure.buildingLevel_logistics < 2)
                        //{
                        //    content.text(string.Format(DssRef.lang.BuildHud_Queue, player.orders.buildQueue(subTile.city), subTile.city.MaxBuildQueue())).overrideColor = subTile.city.availableBuildQueue(player) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        //}
                        buildOpt.blueprint.requirementToHud(content, subTile.city, out _);

                        content.Add(new RbSeperationLine());
                        content.newParagraph();
                        content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                        bp.listResources(content, subTile.city);

                        break;
                    case Players.SelectTileResult.Demolish:
                        title = new RbText(DssRef.lang.Build_DestroyBuilding);
                        content.Add(title);

                        cancelInput();
                        break;

                    case Players.SelectTileResult.ClearTerrain:
                        title = new RbText(DssRef.lang.Build_ClearTerrain);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.CityHall:
                        title = new RbText(DssRef.lang.Hud_SelectCity);
                        content.Add(title);
                        break;

                    case Players.SelectTileResult.Wall:
                        title = new RbText(DssRef.lang.Defence_GuardPost);
                        content.Add(title);

                        if (player.gameControls.map.selection.obj != null &&
                            player.gameControls.map.selection.obj.IsGuardGroup())
                        {
                            content.newLine();
                            content.Add(new RbBeginTitle(2));
                            content.Add(new RbImage(player.gameControls.input.mouseOrder.Icon));
                            content.space(0.5f);
                            content.Add(new RbText(DssRef.lang.Tutorial_MoveInput, HudLib.TitleColor_Action));
                        }

                        content.Add(new RbSeperationLine());
                        DefenceMenu.WallDefenceToHud(content, (TerrainWallType)subTile.subTile.subTerrain, false);

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
                    case Players.SelectTileResult.GoldDeliver:
                        {
                            title = new RbText(DssRef.lang.BuildingType_GoldDelivery);
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
                            title = new RbText(DssRef.lang.BuildingType_School);
                            content.Add(title);
                            content.newLine();
                        }
                        break;

                    case SelectTileResult.ResearchCenter:
                        title = new RbText(DssRef.lang.BuildingType_ReseachCenter);
                        content.Add(title);
                        break;

                    case SelectTileResult.BookPress:
                        title = new RbText(DssRef.lang.BuildingType_Bookpress);
                        content.Add(title);
                        break;

                    case SelectTileResult.CessPit:
                        title = new RbText(DssRef.lang.BuildingType_Cesspit);
                        content.Add(title);
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
                title.overrideColor = avaialableAction ? HudLib.TitleColor_Action : HudLib.NotAvailableColor;

                content.Add(new RbSeperationLine());
                content.newParagraph();

            }
            //else
            //{
            //    lib.DoNothing();
            //}
            content.h2(DssRef.lang.TerrainType, HudLib.TitleColor_Label);
            content.newLine();
            IconName.Terrain(subTile.subTile.mainTerrain, subTile.subTile.subTerrain, out SpriteName tileIcon, out string tileName);
            if (tileIcon != SpriteName.NO_IMAGE)
            {
                content.Add(new RbImage(tileIcon));
                content.hspace();
            }
            content.Add(new RbText(tileName));

            if (BuildLib.BuildTypeFromTerrain(subTile.subTile.mainTerrain, subTile.subTile.subTerrain) != BuildAndExpandType.NUM_NONE)
            {
                content.newLine();
                player.gameControls.input.Build.ToRichContent(content);
                content.hspace();
                content.Add(new RbImage(SpriteName.WarsConstructBuildingIcon));
                content.hspace();
                content.Add(new RbText(DssRef.lang.Hud_Copy));
                HudLib.BulletSeperationPoint(content);
                content.Add(new RbText(DssRef.lang.Building_BuildAction));
            }

            if (subTile.subTile.mainTerrain == TerrainMainType.Building)
            {
                switch ((TerrainBuildingType)subTile.subTile.subTerrain)
                {
                    case TerrainBuildingType.BoarHabitat:
                    case TerrainBuildingType.FowlHabitat:
                    case TerrainBuildingType.OxHabitat:
                    case TerrainBuildingType.PonyHabitat:
                    case TerrainBuildingType.WolfHabitat:
                    case TerrainBuildingType.CatHabitat:
                    case TerrainBuildingType.ElephantHabitat:
                        content.Add(new RbSeperationLine());
                        content.h2(DssRef.lang.Tutorial_ToCapture, HudLib.TitleColor_Head2);
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, DssRef.lang.BuildingType_TrapperHut)));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.BuildHud_AreaRadius, DssConst.TrapperHutRadius)));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(TextLib.LabelColon( DssRef.lang.Work_OrderPrioTitle)));
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsWorkMove));
                        content.hspace();
                        content.Add(new RbText(DssRef.lang.Work_Move));
                        break;

                    case TerrainBuildingType.Smoker:
                        content.newParagraph();
                        content.Add(new RbSeperationLine());
                        Resource.CraftResourceLib.ConservedFood_Smoked.toMenu(content, subTile.city);
                        break;

                    case TerrainBuildingType.Dryer:
                        content.newParagraph();
                        content.Add(new RbSeperationLine());
                        Resource.CraftResourceLib.ConservedFood_Dried.toMenu(content, subTile.city);
                        break;

                    case TerrainBuildingType.Work_CoalPit:
                        content.newParagraph();
                        content.Add(new RbSeperationLine());
                        Resource.CraftResourceLib.Charcoal.toMenu(content, subTile.city);
                        break;

                    case TerrainBuildingType.Brewery:
                        content.newParagraph();
                        content.Add(new RbSeperationLine());
                        Resource.CraftResourceLib.Beer.toMenu(content, subTile.city);
                        break;

                }
            }

            //content.text(subTile.subTile.TypeToString());

            create(player, content, false);

            void cancelInput()
            {
                content.newLine();
                foreach (var icon in player.gameControls.input.cancelIcons())
                {
                    content.Add(new RbImage(icon));
                    content.hspace();
                }
                content.Add(new RbText(DssRef.lang.Hud_Cancel, HudLib.TitleColor_Action));
            }
        }

        void hoverTip(Players.LocalPlayer player, GameObject.AbsGameObject obj)
        {
            if (StartupSettings.BlockTooltip) return;

            RichBoxContent content = new RichBoxContent();

            if (obj.pfaction.TryGetFactionAndPlayer(out var tFaction, out var tPlayer))
            {
                bool attackTarget = false;

                if (tFaction != null)
                {
                    //attackTarget = player.gameControls.army != null &&
                    //    tFaction != player.pfaction.GetFaction();

                    attackTarget = player.armyMayAttackObj(obj as AbsMapObject);

                    if (attackTarget)
                    {
                        content.h2(DssRef.lang.ArmyOption_Attack).overrideColor = HudLib.TitleColor_Attack;
                        content.newLine();
                    }
                }

                obj.toTooltip(new ObjectHudArgs(content, player, false));

                if (attackTarget)
                {
                    if (DssRef.world.diplomacy.GetRelation(player.pfaction, obj.pfaction).InWar())
                    {
                        content.newParagraph();
                    }
                    else
                    {
                        content.Add(new RbSeperationLine());

                        RelationType rel = DssRef.world.diplomacy.GetRelation(player.pfaction, obj.pfaction).Relation;

                        if (tPlayer.IsRemotePlayer())
                        {
                            content.h2(DssRef.lang.Battle_DeclarWarReminder, HudLib.InfoYellow_Light);
                            content.icontext(player.gameControls.input.mouseOrder.Icon, DssRef.todoLang.Diplomacy_OpenPlayerToPlayer, HudLib.TitleColor_Action);
                            content.Add(new RbSeperationLine());
                        }
                        else
                        {
                            content.h1(DssRef.lang.Hud_WardeclarationTitle);
                            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost);
                            content.newLine();
                            HudLib.ResourceCost(content, ResourceType.DiplomaticPoint, Diplomacy.DeclareWarCost(rel), player.diplomaticPoints.Int());
                            content.Add(new RbSeperationLine());
                        }
                    }

                    var attacker = player.gameControls.map.selection.obj as Army;
                    var defender = obj as AbsMapObject;

                    if (attacker != null &&
                        defender != null)
                    {

                        content.Add(new RbBeginTitle(2));
                        content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                        content.Add(new RbText(DssRef.lang.Hud_StrengthRating));//"Strength ratings:"));

                        content.newLine();
                        content.Add(new RbTexture(player.flagTexture, 1f, 0, 0.2f));

                        content.Add(new RbText(": " + TextLib.OneDecimal(attacker.strengthValue)));//string.Format(HudLib.OneDecimalFormat, attacker.strengthValue)));
                        content.newLine();
                        content.text(DssRef.lang.Hud_Versus);
                        content.newLine();
                        content.Add(new RbTexture(tPlayer.flagTexture, 1f, 0, 0.2f));
                        content.Add(new RbText(": " + TextLib.OneDecimal(defender.strengthValue)));
                        content.newLine();
                    }
                }

            }
            create(player, content, false);

            
        }

        public void clear()
        { 
            images.DeleteAll();
        }

        public void create(Players.LocalPlayer player, List<AbsRichBoxMember> content, bool menuToolTip, int tooltip_id = -1)
        {
            images.DeleteAll();

            if (content.Count > 0)
            {

                current_menuToolTip = menuToolTip;

                float edge = 8;
                float width = Engine.Screen.IconSize * 8;

                RichBoxGroup richBox = new RichBoxGroup(new Vector2(edge),
                    width, HudLib.MapToolTipLayer, HudLib.RbSettings, content);

                var area = richBox.maxArea;
                area.AddRadius(edge);
                var backgroundTextures = new NineSplitAreaTexture(HudLib.TooltipSettings.windowBackground, area, HudLib.MapToolTipLayer + 2);
                images.Add(backgroundTextures.images);
                //Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size,
                //    ImageLayers.Lay4);
                //bg.ColorAndAlpha(Color.Black, 0.95f);
                size = area.Size;

                //images.Add(bg);

                images.Add(richBox);

                baseUpdate(player, menuToolTip, false);

                //++tooltip_id_timesec;
                //if (this.tooltip_id != tooltip_id)
                //{ 
                //    this.tooltip_id = tooltip_id;
                //    tooltip_id_timesec = 0;
                //}
            }
        }
    }
}

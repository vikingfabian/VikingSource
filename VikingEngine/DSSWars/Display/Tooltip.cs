using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.GameObject;
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
            content.text(subTile.subTile.TypeToString());

            //if (subTile.selectable(player.faction, out _))
            //{
                switch (subTile.selectTileResult)
                {
                    case Players.SelectTileResult.CityHall:
                        content.text("[]Select City");
                        break;
                    case Players.SelectTileResult.Resources:
                        content.text("[]Select Resources");
                        break;
                }
            //}
            create(player, content, false);
        }

        void hoverTip(Players.LocalPlayer player, GameObject.AbsGameObject obj)
        {
            RichBoxContent content = new RichBoxContent();

            bool attackTarget = player.armyControls != null &&
                obj.GetFaction() != player.faction;

            if (attackTarget)
            {
                content.Add(new RichBoxText("Attack:", Color.Red));
                content.Add(new RichBoxNewLine());
            }

            string name = obj.Name();
            if (name != null)
            {
                content.text(name).overrideColor = Color.LightYellow;
            }
            content.h2(obj.TypeName()).overrideColor = Color.LightGray;

            if (obj.GetFaction() != player.faction)
            {
                var relation = DssRef.diplomacy.GetRelationType(player.faction, obj.GetFaction());

                content.newLine();
                content.Add(new RichBoxText(obj.GetFaction().PlayerName, Color.LightYellow));
                content.newLine();
                content.Add(new RichBoxImage(Diplomacy.RelationSprite(relation)));
                content.Add(new RichBoxText(Diplomacy.RelationString(relation), Color.LightBlue));
                content.newLine();
                
            }

            if (attackTarget)
            {
                if (!DssRef.diplomacy.InWar(player.faction, obj.GetFaction()))
                {
                    content.Add(new RichBoxSeperationLine());

                    RelationType rel = DssRef.diplomacy.GetRelationType(player.faction, obj.GetFaction());
                    content.h2("War declaration");
                    content.text("Cost: " + Diplomacy.DeclareWarCost(rel) + " diplomacy points");
                    string diplomacy = "Diplomatic points: {0}";
                    content.text(string.Format(diplomacy, player.diplomaticPoints.ToString()));

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
                    content.Add(new RichBoxText("Strength ratings:"));
                    
                    content.newLine();
                    content.Add(new RichBoxTexture(player.faction.flagTexture, 1f, 0, 0.2f));
                    
                    content.Add(new RichBoxText(": " + TextLib.OneDecimal(attacker.strengthValue)));//string.Format(HudLib.OneDecimalFormat, attacker.strengthValue)));
                    content.newLine();
                    content.text("VS.");
                    content.newLine();
                    content.Add(new RichBoxTexture(obj.GetFaction().flagTexture, 1f, 0, 0.2f));
                    content.Add(new RichBoxText(": " + TextLib.OneDecimal(defender.strengthValue)));
                    content.newLine();
                }
            }
            else
            {
                var mapObj = obj.RelatedMapObject();
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

                        foreach (var kv in typeCounts)
                        {
                            AbsSoldierData typeData = DssRef.unitsdata.Get(kv.Key);
                            content.Add(new RichBoxText(kv.Value.ToString()));
                            content.Add(new RichBoxImage(typeData.icon));
                            content.space(2);
                        }
                    }
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

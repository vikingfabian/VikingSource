using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Communication
{
    class DiplomacyMap
    {
        List<QuestFlag> questFlags = new List<QuestFlag>();
        RelationFlag[] relationFlags;
        LocalPlayer player;
        DiplomacyMapSelection hoverGui, selectGui;
        RelationFlag selected = null, currentHover;
        public int relationArrowHover = -1;

        int flashCount = 4;
        bool viewFlash = true;
        Timer.Basic flashTimer = new Timer.Basic(800, true);

        const int PreviousFactionsLookedAtCount = 5;
        public List<Faction> previousFactionsLookedAt = new List<Faction>(PreviousFactionsLookedAtCount +1);

        Vector2 relIconSize, relBgSize;
        RelationArrows relationArrows;

        public DiplomacyMap(LocalPlayer player) 
        { 
            this.player = player;
            relationFlags = new RelationFlag[DssRef.world.factions.Array.Length];
            

            relIconSize = Screen.IconSizeV2 * 0.6f;
            relBgSize = relIconSize * 2.2f;
            relationArrows = new RelationArrows(relBgSize);

            for (int i = 0; i < relationFlags.Length; i++)
            {
                relationFlags[i] = new RelationFlag(i);
            }

            hoverGui = new DiplomacyMapSelection(false);
            selectGui = new DiplomacyMapSelection(true);
           
            foreach (var factory in DssRef.state.events.factories)
            {
                questFlags.Add(new QuestFlag()
                {
                    GameObject = factory,
                    tilePos = factory.tilePos,
                    icon = new Graphics.Image(SpriteName.WarsFactoryIcon, Vector2.Zero, Screen.IconSizeV2, HudLib.DiplomacyDisplayLayer - 4, true),
                });
            }

            if (DssRef.settings.darkLordPlayer != null && DssRef.settings.darkLordPlayer.darkLordUnit != null && DssRef.settings.darkLordPlayer.darkLordUnit.Alive())
            {
                questFlags.Add(new QuestFlag()
                {
                    GameObject = DssRef.settings.darkLordPlayer.darkLordUnit,
                    icon = new Graphics.Image(SpriteName.WarsDarkLordBossIcon, Vector2.Zero, Screen.IconSizeV2, HudLib.DiplomacyDisplayLayer - 4, true),
                });
            }
        }

        public Vector2 flagPosition(Faction faction)
        {
            relationFlags[faction.myIndex].updatePos(player, faction);
            return relationFlags[faction.myIndex].position;
        }

        public void update()
        {
            bool overHud = false;
            if (player.gameControls.input.inputSource.HasMouse)
            {
                overHud = player.hud.hudMouseOver();
            }
            else 
            {
                overHud = selected != null;
            }

            RelationFlag newHover = null;
            
            float controller_closestDist = float.MaxValue;
            
            VectorRect hoverArea=VectorRect.Zero;

            player.playerData.view.Camera.RecalculateMatrices();
            bool viewBounds = DssRef.state.localPlayers.Count > 1;

            foreach (var rel in relationFlags)
            {
                Faction faction = DssRef.world.faction(rel.faction);
                
                if (faction != null &&
                    faction.player != null &&
                    faction.isAlive &&
                    !faction.HasZeroUnits() &&
                    rel.inCullingView &&
                    (!player.mapLayersManager.current.DrawFullOverview || faction.displayInFullOverview() || rel == selected))
                {
                    
                    rel.updatePos(player, faction);

                    if (rel.ImageGroup == null)
                    {
                        int layerAdd = rel.cityPos ? 0 : -3;

                        rel.flag = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, relIconSize, HudLib.DiplomacyDisplayLayer + layerAdd, true);
                        rel.flag.Texture = faction.player.flagTexture;
                        rel.flag.SetFullTextureSource();

                        rel.bg = new Graphics.Image(SpriteName.WarsRelationFlag, rel.flag.position, relBgSize, HudLib.DiplomacyDisplayLayer + 1 + layerAdd, true);
                        rel.bg.Color = faction.Color();
                        //rel.bg.ColorAndAlpha(Color.Black, 0.8f);
                        rel.bg.Height *= 1.5f;
                        rel.bg.Ypos += rel.bg.Height * 0.25f;

                        rel.relationIcon = new Graphics.Image(SpriteName.WarsRelationNeutral, rel.flag.position, relIconSize, HudLib.DiplomacyDisplayLayer - 1 + layerAdd, true);
                        rel.relationIcon.Ypos += rel.flag.Height * 0.9f;

                        rel.ImageGroup = new Graphics.ImageGroupParent2D(rel.flag, rel.bg, rel.relationIcon);
                    }

                    rel.ImageGroup.ParentPosition = rel.position;
                    IconName.Relation(rel.relation, out SpriteName relIcon, out string relName);
                    rel.relationIcon.SetSpriteName(relIcon);

                    bool visible = true;

                    if (viewBounds)
                    {
                        visible = player.playerData.view.DrawAreaF.IntersectPoint(rel.flag.position);
                    }

                    rel.ImageGroup.SetVisible(visible);

                    if (faction == player.pfaction.GetFaction())
                    {
                        rel.relationIcon.Visible = false;
                    }
                    else
                    {
                        if (visible && !overHud)
                        {
                            if (player.gameControls.input.inputSource.ControllerMode)
                            {
                                float dist = (player.gameControls.map.XPointerPos() - rel.bg.RealCenter).Length();
                                if (dist < controller_closestDist)
                                {
                                    controller_closestDist = dist;
                                    newHover = rel;
                                    hoverArea = rel.bg.RealArea();
                                }
                            }
                            else
                            {
                                var area = rel.bg.RealArea();
                                if (area.IntersectPoint(player.gameControls.input.mouse.Position))
                                {
                                    newHover = rel;
                                    hoverArea = area;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (rel.flag != null)
                    {
                        rel.ImageGroup.SetVisible(false);
                    }
                }
            }

            if (flashCount > 0) 
            {
                if (flashTimer.Update())
                {
                    flashCount--;
                    viewFlash = !viewFlash;

                    if (flashCount <= 0)
                    {
                        foreach (var quest in questFlags)
                        {
                            quest.icon.Layer = HudLib.DiplomacyDisplayLayer + 6;
                        }
                    }
                }
            }

            foreach (var quest in questFlags)
            {
                if (viewFlash)
                {
                    
                    quest.icon.Position = player.playerData.view.From3DToScreenPos(quest.GameObject.position);
                    quest.icon.Visible = player.playerData.view.DrawAreaF.IntersectPoint(quest.icon.Position);
                }
                else
                {
                    quest.icon.Visible = false;
                }
            }

            if (newHover != currentHover)
            {
                currentHover = newHover;
                player.hud.needRefresh=true;
            }

            relationArrowHover = -1;

            relationArrows.preUpdate();

            if (!overHud)
            {
                if (selected != null && relationArrows.factionArrowHover(player, out relationArrowHover))
                {
                    currentHover = null;
                    if (player.gameControls.input.mouseSelect.DownEvent)
                    {
                        selectFaction(relationArrowHover);
                    }
                }
                else if (currentHover != null)
                {
                    if (player.gameControls.input.mouseSelect.DownEvent)
                    {
                        selectFaction(currentHover.faction);
                        //selected = currentHover;

                        //SoundLib.select_faction.Play();
                        //player.hud.needRefresh = true;

                        //if (player.gameControls.input.inputSource.IsController)
                        //{
                        //    player.gameControls.setMenuFocus(true, true);
                        //}

                        //var faction = DssRef.world.faction(selected.faction);

                        //previousFactionsLookedAt.Remove(faction);
                        //if (previousFactionsLookedAt.Count > PreviousFactionsLookedAtCount)
                        //{ 
                        //    arraylib.RemoveLast(previousFactionsLookedAt);
                        //}
                        //previousFactionsLookedAt.Insert(0, faction);
                    }
                }
                else
                {
                    //hoverbox.Visible = false;

                    if (player.gameControls.input.mouseSelect.DownEvent)
                    {
                        cancel();
                    }
                }
            }

            Faction selectedFaction = null;
            Vector2 selectedFlagPos = Vector2.Zero;

            if (selected != null)
            {
                selectedFaction = DssRef.world.faction(selected.faction);

                selectedFlagPos = selected.position;
                
                if (player.gameControls.input.cancelDownEvent())
                {
                    cancel();
                    selectedFaction = null;
                }
            }

            relationArrows.update(selectedFaction, selectedFlagPos, this);

            //updateSelectBox(currentHover, hoverbox);
            //updateSelectBox(selected, seletionbox);

            hoverGui.updateSelectBox(player, currentHover);
            selectGui.updateSelectBox(player, selected);



        }

        void selectFaction(int factionIx)
        {
            //selected = faction;
            selected = relationFlags[factionIx];

            SoundLib.select_faction.Play();
            player.hud.needRefresh = true;

            if (player.gameControls.input.inputSource.ControllerMode)
            {
                player.gameControls.setMenuFocus(true, true);
            }

            var faction = DssRef.world.faction(factionIx);

            previousFactionsLookedAt.Remove(faction);
            if (previousFactionsLookedAt.Count > PreviousFactionsLookedAtCount)
            {
                arraylib.RemoveLast(previousFactionsLookedAt);
            }
            previousFactionsLookedAt.Insert(0, faction);
        }

        //void updateSelectBox(RelationFlag relation, Graphics.Image box)
        //{
        //    if (relation != null)
        //    {
        //        if (relation.bg != null)
        //        {
        //            var hoverArea = relation.bg.RealArea();

        //            hoverArea.AddRadius(4);
        //            box.Area = hoverArea;
        //            box.Visible = relation.bg != null && relation.bg.Visible;
        //        }
        //    }
        //    else
        //    {
        //        box.Visible = false;
        //    }
        //}

        public void cancel()
        {
            if (selected != null)
            {
                player.gameControls.setMenuFocus(false, true);
                selected = null;
                selectGui.Hide();
                //seletionbox.Visible = false;
            }

            player.hud.needRefresh = true;
        }

        public void DeleteMe()
        {
            foreach (var rel in relationFlags)
            {
                if (rel.flag != null)
                {
                    rel.ImageGroup.DeleteMe();
                }
            }

            foreach (var quest in questFlags)
            {
                quest.icon.DeleteMe();
            }

            //hoverbox.DeleteMe();
            //seletionbox.DeleteMe();

            hoverGui.DeleteMe();
            selectGui.DeleteMe();

            relationArrows.DeleteMe();
            relationArrows = null;
        }

        public void asynchUpdate()
        {
            Rectangle2 tileBound = player.cullingTileArea;
            tileBound.AddRadius(1);

            foreach (var rel in relationFlags)
            {
                Faction faction = DssRef.world.faction(rel.faction);
                if (faction != null)
                {
                    bool cityPos;
                    rel.tilePos = faction.landAreaCenter(out cityPos);

                    rel.inCullingView = tileBound.IntersectTilePoint(rel.tilePos);
                    rel.relation = DssRef.world.diplomacy.GetRelation_Safe(player.pfaction, rel.faction).Relation; 
                }
            }

            foreach (var quest in questFlags)
            {
                quest.inCullingView = tileBound.IntersectTilePoint(quest.tilePos);
            }
        }

        public bool hasSelection()
        {
            return selected != null;
        }

        public bool hasSelectionOrHover()
        {
            return selected != null || currentHover != null;
        }

        public Faction mainSelection(out bool selection)
        {
            if (selected != null)
            {
                selection = true;
                return DssRef.world.faction(selected.faction);
            }
            else if (currentHover != null)
            {
                selection = false;
                return DssRef.world.faction(currentHover.faction);
            }

            selection = false;
            return null;
        }

        
    }

    
}

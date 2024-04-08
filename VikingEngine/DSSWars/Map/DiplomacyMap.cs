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
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars
{
    class DiplomacyMap
    {
        List<QuestFlag> questFlags = new List<QuestFlag>();
        RelationFlag[] relationFlags;
        LocalPlayer player;
        Graphics.Image hoverbox, seletionbox;
        RelationFlag selected = null, currentHover;

        int flashCount = 4;
        bool viewFlash = true;
        Timer.Basic flashTimer = new Timer.Basic(800, true);

        public DiplomacyMap(LocalPlayer player) 
        { 
            this.player = player;
            relationFlags = new RelationFlag[DssRef.world.factions.Array.Length];

            for (int i = 0; i < relationFlags.Length; i++)
            {
                relationFlags[i] = new RelationFlag(i);
            }

            hoverbox = new Graphics.Image(SpriteName.WarsRelationFlagOutline, Vector2.Zero, Vector2.One, HudLib.DiplomacyDisplayLayer + 3);
            hoverbox.Visible = false;
            hoverbox.Color = ColorExt.FromAlpha(0.9f);

            seletionbox = new Graphics.Image(SpriteName.WarsRelationFlagOutline, Vector2.Zero, Vector2.One, HudLib.DiplomacyDisplayLayer + 2);
            seletionbox.Visible = false;

            foreach (var factory in DssRef.state.events.factories)
            {
                questFlags.Add(new QuestFlag()
                {
                    GameObject = factory,
                    tilePos = factory.tilePos,
                    icon = new Graphics.Image(SpriteName.WarsFactoryIcon, Vector2.Zero, Screen.IconSizeV2, HudLib.DiplomacyDisplayLayer - 2, true),
                });
            }

            if (DssRef.state.darkLordPlayer.darkLordUnit != null && DssRef.state.darkLordPlayer.darkLordUnit.Alive())
            {
                questFlags.Add(new QuestFlag()
                {
                    GameObject = DssRef.state.darkLordPlayer.darkLordUnit,
                    icon = new Graphics.Image(SpriteName.WarsDarkLordBossIcon, Vector2.Zero, Screen.IconSizeV2, HudLib.DiplomacyDisplayLayer - 2, true),
                });
            }
        }

        public void update()
        {
            bool overHud = false;
            if (player.input.inputSource.HasMouse)
            {
                overHud = player.hud.hudMouseOver();
            }
            else 
            {
                overHud = selected != null;
            }

            RelationFlag newHover = null;
            
            float controller_closestDist = float.MaxValue;
            //currentHover = null;
            VectorRect hoverArea=VectorRect.Zero;

            player.playerData.view.Camera.RecalculateMatrices();
            bool viewBounds = DssRef.state.localPlayers.Count > 1;

            foreach (var rel in relationFlags)
            {
                Faction faction = DssRef.world.factions[rel.faction];

                if (faction != null &&
                    !faction.HasZeroUnits() &&
                    rel.inCullingView &&
                    (!player.drawUnitsView.current.DrawFullOverview || faction.displayInFullOverview || rel == selected))
                {
                    bool cityPos;
                    var landAreaCenter = faction.landAreaCenter(out cityPos);

                    if (rel.ImageGroup == null)
                    {
                        int layerAdd = cityPos ? 0 : -3;


                        rel.flag = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, Vector2.Zero, Engine.Screen.IconSizeV2 * 0.6f, HudLib.DiplomacyDisplayLayer + layerAdd, true);
                        rel.flag.Texture = faction.flagTexture;
                        rel.flag.SetFullTextureSource();

                        rel.bg = new Graphics.Image(SpriteName.WarsRelationFlag, rel.flag.position, rel.flag.size * 2.2f, HudLib.DiplomacyDisplayLayer + 1 + layerAdd, true);
                        rel.bg.Color = faction.Color();
                        //rel.bg.ColorAndAlpha(Color.Black, 0.8f);
                        rel.bg.Height *= 1.5f;
                        rel.bg.Ypos += rel.bg.Height * 0.25f;

                        rel.relationIcon = new Graphics.Image(SpriteName.WarsRelationNeutral, rel.flag.position, Engine.Screen.IconSizeV2 * 0.6f, HudLib.DiplomacyDisplayLayer + layerAdd, true);
                        rel.relationIcon.Ypos += rel.flag.Height * 0.9f;

                        rel.ImageGroup = new Graphics.ImageGroupParent2D(rel.flag, rel.bg, rel.relationIcon);
                    }

                    Vector3 wp = Vector3.Zero;
                    
                    wp.X = landAreaCenter.X+0.5f;
                    wp.Z = landAreaCenter.Y-6;

                    rel.ImageGroup.ParentPosition = player.playerData.view.From3DToScreenPos(wp);
                    rel.relationIcon.SetSpriteName(Diplomacy.RelationSprite(rel.relation));

                    bool visible = true;

                    if (viewBounds)
                    {
                        visible = player.playerData.view.DrawAreaF.IntersectPoint(rel.flag.position);
                    }

                    rel.ImageGroup.SetVisible(visible);

                    if (faction == player.faction)
                    {
                        rel.relationIcon.Visible = false;
                    }
                    else
                    {
                        if (visible && !overHud)
                        {
                            if (player.input.inputSource.IsController)
                            {
                                float dist = (player.mapControls.XPointerPos() - rel.bg.RealCenter).Length();
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
                                if (area.IntersectPoint(Input.Mouse.Position))
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

            if (!overHud)
            {
                if (currentHover != null)
                {
                    //hoverbox.Visible = true;
                    //hoverArea.AddRadius(4);
                    //hoverbox.Area = hoverArea;

                    if (player.input.Select.DownEvent)
                    {
                        selected = currentHover;
                        //seletionbox.Area = hoverArea;
                        //seletionbox.Visible = true;
                        player.hud.needRefresh = true;

                        player.hud.displays.beginMove(2);
                    }
                }
                else
                {
                    //hoverbox.Visible = false;

                    if (player.input.Select.DownEvent)
                    {
                        cancel();
                    }
                }
            }

            if (selected != null)
            {               
                player.hud.displays.updateMove();

                if (player.input.ControllerCancel.DownEvent)
                {
                    cancel();
                }
            }

            updateSelectBox(currentHover, hoverbox);
            updateSelectBox(selected, seletionbox);

        }

        void updateSelectBox(RelationFlag relation, Graphics.Image box)
        {            
            if (relation != null)
            {
                var hoverArea = relation.bg.RealArea();
                
                hoverArea.AddRadius(4);
                box.Area = hoverArea;
                box.Visible = true;
                //seletionbox.Area = hoverArea;
                //seletionbox.Visible = true;
            }
            else
            {
                box.Visible = false;
            }
        }

        private void cancel()
        {
            if (selected != null)
            {
                selected = null;
                seletionbox.Visible = false;
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

            hoverbox.DeleteMe();
            seletionbox.DeleteMe();
        }

        public void asynchUpdate()
        {
            Rectangle2 tileBound = player.cullingTileArea;
            tileBound.AddRadius(1);

            foreach (var rel in relationFlags)
            {
                Faction faction = DssRef.world.factions[rel.faction];
                if (faction != null)
                {
                    bool cityPos;
                    rel.tilePos = faction.landAreaCenter(out cityPos);

                    rel.inCullingView = tileBound.IntersectTilePoint(rel.tilePos);
                    rel.relation = DssRef.diplomacy.GetRelationType(player.faction.parentArrayIndex, rel.faction); 
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
                return DssRef.world.factions[selected.faction];
            }
            else if (currentHover != null)
            {
                selection = false;
                return DssRef.world.factions[currentHover.faction];
            }

            selection = false;
            return null;
        }

        abstract class AbsFlag
        {
            public IntVector2 tilePos;
            public bool inCullingView = false;
        }

        class RelationFlag : AbsFlag 
        {
            public int faction;
            
            public RelationType relation;
            //Vector2 screenCenter;
            public Graphics.ImageAdvanced flag = null;
            public Graphics.Image bg = null;
            public Graphics.Image relationIcon = null;
            public Graphics.ImageGroupParent2D ImageGroup;
                        
            public RelationFlag(int faction)
            {
                this.faction = faction;
            }
        }

        class QuestFlag : AbsFlag
        {
            public Graphics.Image icon = null;
            public AbsWorldObject GameObject = null;
        }
    }

    
}

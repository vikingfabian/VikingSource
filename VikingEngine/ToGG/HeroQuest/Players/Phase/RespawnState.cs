using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    class RespawnState : AbsPlayerPhase
    {
        Unit unit;

        public List<IntVector2> available = new List<IntVector2>(16);
        FindMinValuePointer<IntVector2?> closest = new FindMinValuePointer<IntVector2?>();
        Time cameraControlTime = new Time(2, TimeUnit.Seconds);
        Graphics.ImageGroup titleImages = new Graphics.ImageGroup();

        public RespawnState(LocalPlayer player)
            :base(player)
        {
            this.player = player;
            unit = player.HeroUnit;
        }

        public override void onBegin()
        {
            base.onBegin();

            collectAvailablePositions();

            title();
            player.mapControls.SetAvailableTiles(available, null);

            IntVector2 prevPos = unit.squarePos;

            foreach (var pos in available)
            {
                closest.Next(prevPos.Length(pos), pos);
            }
        }

        public override void update(ref PlayerDisplay display)
        {
            if (hqRef.gamestate.playerTurnPresentation == null)
            {
                cameraControlTime.CountDown();
                if (cameraControlTime.HasTime)
                {
                    if (closest.minMember != null)
                    {
                        toggRef.cam.spectate(closest.minMember.Value);
                    }
                }
            }

            player.mapUpdate(ref display, false);

            bool isTarget = player.HeroUnit.heroCanRestartHere(player.mapControls.selectionIntV2, false) &&
                available.Contains(player.mapControls.selectionIntV2);
            player.availableMapSquare = isTarget ? MapSquareAvailableType.Enabled : MapSquareAvailableType.None;

            if (player.mapControls.isOnNewTile)
            {
                if (isTarget)
                {
                    new RespawnTooltip(player.mapControls);
                }
                else
                {
                    player.mapControls.removeToolTip();
                }
            }

            if (isTarget && toggRef.inputmap.click.DownEvent)
            {
                respawnHere(player.mapControls.selectionIntV2);
                end();//RespawnState = null;
                player.regularTurnStartSetup();
            }
        }

        void title()
        {
            string titleText = "Pick restart position";

            Vector2 textPos = Engine.Screen.SafeArea.CenterTop;
            textPos.Y += Engine.Screen.TextTitleHeight;

            Graphics.Text2 title = new Graphics.Text2(titleText, LoadedFont.Bold, textPos, Engine.Screen.TextTitleHeight,
                HudLib.TitleTextBronze, HudLib.ContentLayer);
            title.OrigoAtCenter();
            titleImages.Add(title);

            VectorRect area = VectorRect.FromCenterSize(textPos, title.MeasureText());
            area.AddRadius(HudLib.ThickBorderEdgeSize + Engine.Screen.BorderWidth);

            var bg = HudLib.ThickBorder(area, HudLib.BgLayer + 1);
            titleImages.images.AddRange(bg.images);
        }

        void collectAvailablePositions()
        {
            addSpawnPoint(unit.restartPos);

            foreach (var m in toggRef.board.metaData.campfires)
            {
                if (m.InteractSettings.activationState)
                {
                    addSpawnPoint(m.position);
                }
            }
        }

        void addSpawnPoint(IntVector2 center)
        {
            if (unit.heroCanRestartHere(center, true))
            {
                add(center);
            }

            int searchRadius = 1;
            bool foundPos = false;

            while (!foundPos)
            {                
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(center, searchRadius));
                while (loop.Next())
                {
                    if (unit.heroCanRestartHere(loop.Position, true))
                    {
                        add(loop.Position);
                        foundPos = true;
                    }
                }

                searchRadius++;
            }

            void add(IntVector2 pos)
            {
                if (available.Contains(pos) == false)
                {
                    available.Add(pos);
                }
            }
        }

        public void respawnHere(IntVector2 startPos)
        {
            unit.health.Value = unit.health.maxValue - 2;
            unit.data.hero.stamina.Value = unit.data.hero.stamina.maxValue - 2;
            unit.data.hero.bloodrage.Value = 0;

            unit.soldierModel.Visible = true;
            unit.SetPosition(startPos);

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqRestartUnit, Network.PacketReliability.Reliable);
            unit.netWriteUnitId(w);
            unit.netWriteStatus(w);
            toggRef.board.WritePosition(w, startPos);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            titleImages.DeleteAll();
        }

        public override PhaseType Type => PhaseType.Respawn;
    }

    class RespawnTooltip : ToggEngine.Display2D.AbsToolTip
    {
        public RespawnTooltip(MapControls mapControls)
            : base(mapControls)
        {
            var members = new List<AbsRichBoxMember>{
                new RbBeginTitle(),
                new RbImage(SpriteName.cmdRegenrate),
                new RbText("Restart here"),
            };

            AddRichBox(members);
        }
    }
}

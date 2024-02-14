using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.Commander.Display
{
    /// <summary>
    /// View info on each player and their VP
    /// </summary>
    class PlayersStatusHUD
    {
        static float NameAreaHeight;
        static float Spacing;
        static float PhaseAreaHeight;

        public static float NamesBottomY;

        List<PlayerOverviewHUDMember> players;

        public PlayersStatusHUD(int playersCount)
        {
            NameAreaHeight = Engine.Screen.IconSize;
            Spacing = Engine.Screen.IconSize * 0.1f;
            PhaseAreaHeight = NameAreaHeight * 0.84f;

            float totalHeight = NameAreaHeight + Spacing + PhaseAreaHeight;

            Vector2 position = Engine.Screen.SafeArea.Position;
            NamesBottomY = position.Y + NameAreaHeight;

            players = new List<PlayerOverviewHUDMember>(playersCount);
            for (int i = 0; i < playersCount; ++i)
            {
                var m = new PlayerOverviewHUDMember(Commander.cmdRef.players.Player(i), position);
                position.Y += m.bgArea.Height + Engine.Screen.BorderWidth;

                players.Add(m);
            }
        }

        public void PlayerTurn(int player)
        {
            for (int i = 0; i < players.Count; ++i)
            {
                players[i].Focus(i == player);
            }
        }

        public void Refresh()
        {
            foreach (PlayerOverviewHUDMember p in players)
            {
                p.refresh();
            }
        }

        //----------
        class PlayerOverviewHUDMember
        {
            public const float BgTransparentsy = 0.5f;

            Graphics.TextG VPcount;
            Graphics.Image background;
            Graphics.ImageGroupParent2D images;

            Commander.Players.AbsCmdPlayer player;
            

            public VectorRect bgArea;

            public PlayerOverviewHUDMember(Commander.Players.AbsCmdPlayer player, Vector2 position)
            {
                images = new Graphics.ImageGroupParent2D();
                this.player = player;
                
                bgArea = new VectorRect(Vector2.Zero, new Vector2(NameAreaHeight));
                float edge = Engine.Screen.IconSize * 0.1f;
                float contentHeight = bgArea.Height - edge * 2f;

                Vector2 startPos = new Vector2(edge);

                var shield = new Graphics.ImageAdvanced(player.relationVisuals.shield, 
                    startPos, new Vector2(contentHeight),  HudLib.StatusHudLayer, false);
                images.Add(shield);

                if (player.relationVisuals.shield == SpriteName.NO_IMAGE)
                {
                    new SteamWrapping.LoadGamerIcon(shield, null, true);
                }

                var name = new Graphics.TextG(LoadedFont.Regular, shield.RightTop, Vector2.One, Graphics.Align.Zero,
                    player.pData.PublicName(LoadedFont.Regular), Color.White, HudLib.StatusHudLayer);
                name.SetHeight(contentHeight);
                images.Add(name);

                Vector2 vpPos = new Vector2(name.MeasureRightPos() + Engine.Screen.SmallIconSize, name.Ypos);
                bgArea.SetRight(vpPos.X, true);

                if (player.setup.collectingVP != null)
                {
                    var VPicon = new Graphics.Image(SpriteName.cmd1Honor, vpPos, new Vector2(contentHeight), HudLib.StatusHudLayer);
                    VPcount = new Graphics.TextG(LoadedFont.Regular, VPicon.RightTop, name.Size,
                        Graphics.Align.Zero, "00", Color.White, HudLib.StatusHudLayer);
                    images.Add(VPicon); images.Add(VPcount);

                    var vpGoalCount = new Graphics.TextG(LoadedFont.Regular, 
                        new Vector2(VPcount.MeasureRightPos(), vpPos.Y + contentHeight), 
                        Vector2.One, new Graphics.Align(Vector2.UnitY), 
                        "/" + player.setup.collectingVP.Value.ToString(), Color.White, HudLib.StatusHudLayer);
                    vpGoalCount.SetHeight(contentHeight * 0.6f);
                    images.Add(vpGoalCount);

                    bgArea.SetRight(vpGoalCount.MeasureRightPos() + edge, true);
                }
                background = new Graphics.Image(SpriteName.WhiteArea, bgArea.Position, bgArea.Size,
                    HudLib.StatusHudLayer + 1);
                background.Color = Color.Black;
                background.Opacity = 0.5f;

                
                images.Add(background);

                images.ParentPosition = position;
            }

            public void refresh()
            {
                if (VPcount != null)
                {
                    VPcount.TextString = player.VictoryPoints.ToString();
                }
            }

            public void Focus(bool focus)
            {
                float opacity = focus ? 1f : 0.5f;

                images.SetOpacity(opacity);
                background.Opacity = opacity * BgTransparentsy;
            }
            
            public Vector2 PhaseInfoPos()
            {
                Vector2 result = images.ParentPosition;
                result.Y += bgArea.Height + Spacing;

                return result;
            }
        }
    }
}

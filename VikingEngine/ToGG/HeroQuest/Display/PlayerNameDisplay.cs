using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest
{
    class PlayerNameDisplay
    {
        Graphics.Image background;
        Graphics.ImageGroup images;
        PlayerVisualStateDisplay stateDisplay = null;
        bool hiddenStrategy = false;

        public PlayerNameDisplay(AbsHQPlayer player)
        {
            images = new Graphics.ImageGroup();

            refreshImages(player);
        }

        public void refreshImages(AbsHQPlayer player)
        {
            images.DeleteAll();

            Vector2 position = Engine.Screen.SafeArea.Position;
            position.Y = Ypos(player.heroIndex);//(NameAreaHeight + Spacing) * player.heroIndex;

            var bgArea = new VectorRect(position, new Vector2(NameAreaHeight));
            float edge = Engine.Screen.IconSize * 0.1f;
            float contentHeight = bgArea.Height - edge * 2f;

            position += new Vector2(edge);
            var gamerIcon = new Graphics.ImageAdvanced(SpriteName.MissingImage,
                position, new Vector2(contentHeight), HudLib.StatusHudLayer, false);
            images.Add(gamerIcon);

            new SteamWrapping.LoadGamerIcon(gamerIcon, player.pData);

            var name = new Graphics.TextG(LoadedFont.Regular, VectorExt.AddX(gamerIcon.RightTop, edge), Vector2.One, Graphics.Align.Zero,
                player.pData.PublicName(LoadedFont.Regular), Color.White, HudLib.StatusHudLayer);
            name.SetHeight(contentHeight);
            images.Add(name);

            Vector2 statusIconsPos = new Vector2(name.MeasureRightPos() + Engine.Screen.SmallIconSize, gamerIcon.Ypos);

            player.unitsColl.unitsCounter.Reset();
            while (player.unitsColl.unitsCounter.Next())
            {
                var u = player.unitsColl.unitsCounter.sel;
                Graphics.ImageAdvanced unitImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, statusIconsPos,
                    new Vector2(contentHeight), HudLib.StatusHudLayer, false);
                u.Data.modelSettings.IconSource(unitImage, false);
                images.Add(unitImage);
                //VectorRect source = new VectorRect(unitImage.ImageSource);
                //source.AddRadius(-source.Width * 0.05f);
                //unitImage.ImageSource = source.Rectangle;

                statusIconsPos.X += unitImage.Width - edge;
            }

            bgArea.SetRight( statusIconsPos.X, true);

            background = new Graphics.Image(SpriteName.WhiteArea, bgArea.Position, bgArea.Size,
                HudLib.StatusHudLayer + 1);
            background.Color = Color.Black;
            background.Opacity = 0.5f;


            images.Add(background);
        }

        static float NameAreaHeight => MathExt.Round(Engine.Screen.IconSize * 0.9f);

        static float Ypos(int heroIndex)
        {
            float Spacing = Engine.Screen.IconSize * 0.1f;

            return MathExt.Round(Engine.Screen.SafeArea.Y + (NameAreaHeight + Spacing) * heroIndex);
        }

        public static float Bottom()
        {
            return Ypos(hqRef.players.HeroPlayersCount);
        }

        public void refreshVisualState(PlayerVisualData visualState)
        {            
            if (stateDisplay == null || !stateDisplay.visualState.Equals(visualState))
            {
                removeStateDisplay();
                stateDisplay = new PlayerVisualStateDisplay(visualState, background.Area, ref hiddenStrategy);
            }
        }

        public void viewAttackResult(bool melee, BattleDiceResult result)
        {
            if (stateDisplay == null || stateDisplay.AttackResults == 0)
            {
                var visual = new PlayerVisualData();
                visual.meleeAttack = melee;
                visual.state = PlayerState.Attack;
                refreshVisualState(visual);
            }

            stateDisplay.viewAttackResult(result);
        }

        public void removeStateDisplay()
        {
            if (stateDisplay != null)
            {
                stateDisplay.DeleteMe();
                stateDisplay = null;
            }
        }
    }

    
}

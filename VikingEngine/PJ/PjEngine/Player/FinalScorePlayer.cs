using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    class FinalScorePlayer
    {
        HatImage hatImage = null;
        Graphics.Image border, animal, controllerIcon;
        Graphics.ImageAdvanced button;

        public FinalScorePlayer(GamerData gamer, int winnerindex)
        {
            float height = Engine.Screen.Height * 0.12f;

            Graphics.Text2 number = new Graphics.Text2(
                TextLib.IndexToString(winnerindex) + ".",
                LoadedFont.Bold,
                new Vector2(Engine.Screen.Width * 0.15f,
                    Engine.Screen.Height * (0.15f + 0.2f * winnerindex)),
                    height * 0.8f, 
                    new Color(14,52,90),//Color.White, 
                    ImageLayers.Lay1);

            VectorRect iconArea = new VectorRect(number.Position, new Vector2(height));
            iconArea.X += Engine.Screen.Height * 0.1f;

            LobbyAvatar.GamerAvatarFrameAndJoustAnimal(iconArea, ImageLayers.Lay4, gamer, out border, out animal, out button, out controllerIcon);
            if (gamer.hat != Hat.NoHat)
            {
                hatImage = new HatImage(gamer.hat, animal, AnimalSetup.Get(gamer.joustAnimal));
            }

            if (gamer.networkPeer != null)
            {
                LobbyAvatar.RemoteGamerIconSetup(gamer.networkPeer, button);
            }

            float iconAndNumberSpace = iconArea.Width * 0.3f;
            float statsSpace = iconArea.Width * 1.8f;

            Vector2 iconSz = iconArea.Size * 0.8f;

            Vector2 nextPos =  iconArea.RightCenter + VectorExt.V2FromX( iconArea.Width * 1.4f);

            if (PjRef.storage.Mode == PartyGameMode.Jousting ||
                PjRef.storage.Mode == PartyGameMode.Strategy)
            {
                Graphics.Image wonIcon = new Graphics.Image(
                    PjRef.storage.Mode == PartyGameMode.Jousting ? SpriteName.BirdThrophy : SpriteName.winnerParticle,
                    nextPos, iconSz, ImageLayers.Lay2, true);
                
                nextPos.X += iconAndNumberSpace;
                Display.SpriteText wonText = new Display.SpriteText(gamer.Victories.ToString(), nextPos, iconSz.Y * 0.7f,
                    ImageLayers.Lay1, new Vector2(0, 0.5f), Color.White, true);

                nextPos.X += statsSpace;
            }
            Graphics.Image coinsIcon = new Graphics.Image(SpriteName.birdCoin1, nextPos, iconSz * 0.6f, ImageLayers.Lay2, true);
            
            nextPos.X += iconAndNumberSpace;
            Display.SpriteText coinsText = new Display.SpriteText(gamer.coins.ToString(), nextPos, Engine.Screen.IconSize, 
                ImageLayers.Lay1, new Vector2(0, 0.5f), PjLib.CoinPlusColor, true);


            VectorRect bgBarArea = new VectorRect(Engine.Screen.SafeArea.X, iconArea.Y, Engine.Screen.SafeArea.Width, iconArea.Height);

            bgBarArea.AddXRadius(-Engine.Screen.IconSize);
            bgBarArea.AddYRadius(Engine.Screen.IconSize * 0.5f);

            Graphics.Image bgBar = new Graphics.Image(SpriteName.WhiteArea, bgBarArea.Position, bgBarArea.Size, ImageLayers.Bottom0);
            bgBar.Color = new Color(175, 214, 253);//Color.DarkBlue;


        }
    }
}

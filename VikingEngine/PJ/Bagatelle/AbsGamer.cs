using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    abstract class AbsGamer
    {
        public int ballsLeft = BagLib.BallCount;
        protected BagatellePlayState state;
        //public GamerData data;
        public int levelScore = 0, totalScore = 0;

        Display.SpriteText ballCountText;
        Graphics.Image bumpCountImage;
        protected Graphics.Image border, animal, controllerIcon;
        protected Graphics.ImageAdvanced button;

        public List<Ball> balls = new List<Ball>(4);

        protected VectorRect hudArea;
        public AnimalSetup animalSetup;
        protected int localGamerIndex;

        public AbsGamer(VectorRect hudArea, int localGamerIndex, BagatellePlayState state)
        {
            this.localGamerIndex = localGamerIndex;
            this.state = state;
            //this.data = data;
            this.hudArea = hudArea;
        }

        protected void initGamer()
        {
            GamerData data = GetGamerData();
            animalSetup = AnimalSetup.Get(data.joustAnimal);

            VectorRect iconArea = hudArea;
            iconArea.Width = iconArea.Height;
            iconArea.AddRadius(-iconArea.Height * 0.05f);

            LobbyAvatar.GamerAvatarFrameAndJoustAnimal(iconArea, ImageLayers.Top5, data,
               out border, out  animal, out  button, out  controllerIcon);
            if (data.hat != Hat.NoHat)
            {
                new HatImage(data.hat, animal, animalSetup);
            }

            ballCountText = new Display.SpriteText("0", border.RightCenter, border.Height * 0.42f, ImageLayers.Top2, VectorExt.V2HalfY,
                Color.LightBlue, true);
            //refreshBallCountText();

            bumpCountImage = new Graphics.Image(SpriteName.birdBumpCount0, border.RightCenter, new Vector2(border.Height * 0.8f), ImageLayers.Top2);
            bumpCountImage.origo = VectorExt.V2HalfY;
        }

        public void refreshHud()
        {
            if (balls.Count == 0)
            {
                //ballCountText.SetVisible(true);
                bumpCountImage.Visible = false;

                if (ballsLeft == 1)
                {
                    ballCountText.color = Color.Orange;
                }
                else if (ballsLeft <= 0)
                {
                    ballCountText.color = PjLib.RedNumbersColor;
                }

                ballCountText.Text("*" + ballsLeft.ToString());
            }
            else
            {
                ballCountText.SetVisible(false);
                bumpCountImage.Visible = true;

                int maxBumps = balls[0].bumps;
                foreach (var m in balls)
                {
                    if (m.bumps > maxBumps)
                    {
                        maxBumps = m.bumps;
                    }
                }

                bumpCountImage.SetSpriteName(Ball.BumpCountImageTile(maxBumps));
            }
        }

        public void onLevelOver(int highestScore)
        {
            GetGamerData().coins += levelScore;

            Vector2 pos = hudArea.RightCenter;
            Graphics.Image coin = new Graphics.Image(SpriteName.birdCoin1, pos, new Vector2(hudArea.Height * 0.7f), ImageLayers.Lay1);
            coin.origo = new Vector2(0, 0.5f);

            pos.X += coin.Width * 1.1f;

            var text = new Display.SpriteText(levelScore.ToString(), pos,
                coin.Height * 0.9f, ImageLayers.Lay1, new Vector2(0, 0.5f), PjLib.CoinPlusColor, true);

            if (levelScore >= highestScore)
            {
                new LeadingPlayerCrown(new Vector2(text.Right, pos.Y));
            }
        }
        
        public void fireBallEffects(Ball b)
        {
            //SoundManager.PlaySound(LoadedSound.flowerfire);
            state.board.FireSound();
        }

         public void writeGamer(System.IO.BinaryWriter w)
         {             
             w.Write(NetworkPeer.id);
             w.Write((byte)localGamerIndex);
         }

         public static void ReadGamer(System.IO.BinaryReader r, out byte peerId, out int localIndex)
         {
             peerId = r.ReadByte();
             localIndex = r.ReadByte();
         }

         public int netIdHash()
         {
             return IdToHash(NetworkPeer.id, localGamerIndex);
         }

         public static int IdToHash(byte peerId, int localIndex)
         {
             return peerId * 1000 + localIndex;
         }

         public void RemoveBall(Ball b)
         {
             balls.Remove(b);
             refreshHud();
         }

         public bool isDonePlaying()
         {
             return ballsLeft <= 0 && balls.Count == 0;
         }

         abstract public Network.AbsNetworkPeer NetworkPeer { get; }

         abstract public GamerData GetGamerData();
    }
}

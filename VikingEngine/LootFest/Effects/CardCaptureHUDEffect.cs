using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using VikingEngine.CardGame;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Effects
{
    class CardCaptureHUDEffect : AbsUpdateable
    {
        const float AmountBumpTime = 100;
        Time viewTime = new Time(2, TimeUnit.Seconds);
        Timer.Basic changeTextTimer = new Timer.Basic(AmountBumpTime, false);

        Graphics.TextG amountText = null;
        //PlayerHandCard cardImage;
        int startCount;

        public CardCaptureHUDEffect(Players.Player player, CardType card)
            : base(true)
        {
            startCount = player.Storage.progress.cardCollection[(int)card].TotalCount;
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.CardCaptureEffect, Network.PacketReliability.Reliable);
            w.Write((byte)card);
            w.Write((ushort)startCount);


            Vector2 center = player.SafeScreenArea.Position + player.SafeScreenArea.Size * new Vector2(0.85f, 0.8f);

            init(card, center, startCount == 0 ? Engine.Screen.IconSize * 8f : Engine.Screen.IconSize * 4f);
            
        }

        public CardCaptureHUDEffect(Players.ClientPlayer player, System.IO.BinaryReader r)
            : base(true)
        {
            CardType card = (CardType)r.ReadByte();
            startCount = r.ReadUInt16();

            Vector2 center = player.statusDisplay.bg.LeftCenter;
            center.X -= player.statusDisplay.bg.Height * 0.4f;

            init(card, center, player.statusDisplay.bg.Height * 1.2f);
        }

        void init(CardType card, Vector2 center, float cardHeight)
        {
            //cardImage = null;//new PlayerHandCard(cgRef.cards.Get(card), null);
            //cardImage.image.CenterRelative = VectorExt.V2Half;
            //cardImage.image.Position = center;

            //cardImage.image.Rotation = Ref.rnd.Float(0.05f, 0.4f); //0.3f;
            //cardImage.image.Layer = ImageLayers.Foreground5;
            //cardImage.setHeight(cardHeight);

            //if (startCount == 0)
            //{
            //    Graphics.Motion2d bump = new Graphics.Motion2d(Graphics.MotionType.SCALE, cardImage.image, cardImage.image.Size * 0.5f, Graphics.MotionRepeate.BackNForwardOnce, 100, true);
            //}
            //else
            //{
            //    amountText = new Graphics.TextG(LoadedFont.PhoneText, center, new Vector2(Engine.Screen.TextSize * 1.8f), Graphics.Align.CenterAll, "x" + startCount.ToString(), Color.White, ImageLayers.Foreground4);
            //    Graphics.Motion2d textbump = new Graphics.Motion2d(Graphics.MotionType.SCALE, amountText, amountText.Size * 0.5f, Graphics.MotionRepeate.BackNForwardOnce, AmountBumpTime, true);
            //}
        }

        

        public override void Time_Update(float time_ms)
        {
            //if (viewTime.CountDown())
            //{
            //    cardImage.image.Opacity -= 4f * Ref.DeltaTimeSec;
            //    cardImage.image.Xpos += cardImage.image.Width * 8f *  Ref.DeltaTimeSec;

            //    if (amountText != null)
            //    {
            //        amountText.Position = cardImage.image.Position;
            //        amountText.Opacity = cardImage.image.Opacity;
            //    }

            //    if (cardImage.image.Opacity <= 0)
            //    {
            //        this.DeleteMe();
            //    }
            //}

            //if (changeTextTimer.Update())
            //{
            //    if (amountText != null)
            //    {
            //        startCount++;
            //        amountText.TextString = "x" + startCount.ToString();
            //    }
            //}
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            //cardImage.DeleteMe();
            if (amountText != null)
            {
                amountText.DeleteMe();
            }
        }

    }


}

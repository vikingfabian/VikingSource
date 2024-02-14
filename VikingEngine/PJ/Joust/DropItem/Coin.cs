using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    class Coin : AbsDropObject
    {
        public static readonly SpriteName[] Animation = new SpriteName[]
        {
            SpriteName.birdCoin1,
            SpriteName.birdCoin2,
            SpriteName.birdCoin3,
            SpriteName.birdCoin4,
            SpriteName.birdCoin5,
            SpriteName.birdCoin6,
        };

        CirkleCounterUp animFrame = new CirkleCounterUp(0, Animation.Length - 1);
        Timer.Basic animationTime = new Timer.Basic(132, true);

        public Coin(Vector2 startPos, float velocityY)
            : base(startPos, SpriteName.birdCoin1, Size(), 0.32f, true, velocityY)
        {

        }

        public override bool Update()
        {
            if (animationTime.Update())
            {
                image.SetSpriteName(Animation[animFrame.Next()]);
            }
            return base.Update();
        }

        public static float Size()
        {
            return 0.06f * Engine.Screen.Height;
        }

        public override JoustObjectType Type
        {
            get { return JoustObjectType.Coin; }
        }
    }

    class RandomItemBox : AbsDropObject
    {
        public RandomItemBox(Vector2 startPos, float velocityY)
            : base(startPos, SpriteName.birdQuestionBox, 0.06f * Engine.Screen.Height, 0.49f, false, velocityY)
        {

        }

        public override JoustObjectType Type
        {
            get { return JoustObjectType.RandomItemBox; }
        }
    }
}

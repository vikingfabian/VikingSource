using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.SpaceWar
{
    class Lorry : AbsGameObject
    {
        const float CoinTailSpacing = 2f;
        const CoinValue DropLoadValue = CoinValue.Value10;
        public static readonly IntVector2 ExplosionModelSplits = new IntVector2(6, 10);

        bool turnState = false;
        Time stateTimer = Time.Zero;
        int turnDir;
        CoinTailPart coinTail;
        RectangleCentered area;

        public Lorry()
            : base()
        {
            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(3f),
                Graphics.TextureEffectType.Flat, SpriteName.spaceWarLorry, Color.White);

            model.ScaleX = model.ScaleZ / SpriteSheet.SpaceWarLorryPixSz.Y * SpriteSheet.SpaceWarLorryPixSz.X;

            model.X = Ref.rnd.Plus_MinusF(WorldMap.MapHalfW);
            model.Z = Ref.rnd.Plus_MinusF(WorldMap.MapHalfW);

            rotation = Rotation1D.Random();

            area = new RectangleCentered(Vector2.Zero, VectorExt.V3XZtoV2(model.Scale * 0.5f));
            var rectbound = new Physics.RectangleRotatedBound(area, rotation);
            bound = new Physics.Bound2DWrapper(true, rectbound);

            resetTail();
            SpaceRef.go.Add(this);
        }

        void resetTail()
        {
            coinTail?.DeleteMe();

            int coins = Ref.rnd.Int(5, 9);
            int extraValueCoinIx = Ref.rnd.Int(12);

            coinTail = new CoinTailPart(ref coins, extraValueCoinIx, model.Position);
            coinTail.generateCoin();
        }

        public override bool update()
        {
            if (stateTimer.CountDown())
            {
                turnState = !turnState;
                if (turnState)
                {
                    turnDir = Ref.rnd.LeftRight();
                    stateTimer.Seconds = Ref.rnd.Float(1f, 2f);
                }
                else
                {
                    stateTimer.Seconds = Ref.rnd.Float(4f, 10f);
                }
            }

            if (turnState)
            {
                rotation.Radians += 0.01f * turnDir;
                SpaceLib.Rotation1DToQuaterion(model, rotation.Radians);
                velocity.Set(rotation, 0.003f);
            }
            updateMovement();

            if (coinTail != null)
            {
                int coinCount = 0;
                coinTail.Update(model.Position, CoinTailSpacing, ref coinCount);

                if (coinCount == 0)
                { 
                    coinTail = null;

                    dropLoad().addForce(lib.AngleToV2(rotation.getInvert().radians, SpaceLib.DriftSpeed));
                }
            }
            return base.update();
        }

        Coin dropLoad()
        {
            var dropLoad = new Coin(false, DropLoadValue);
            area.Center = bound.MainBound.Center;
            dropLoad.setPosition(VectorExt.V2toV3XZ(area.positionFromPercent(new Vector2(0.5f, 0.9f), rotation.radians)));

            return dropLoad;
        }

        public override void takeDamage(Vector3 damageCenter)
        {
            base.takeDamage(damageCenter);

            DeleteMe();
            Effects.EffectLib.SplitModelExplosion(model, ExplosionModelSplits);

            //Set all coins to drift
            if (coinTail != null)
            {
                coinTail.setDrift();
                dropLoad().setDrift(SpaceLib.DeathRubbleSpeed);
            }

            int extraCoins = Ref.rnd.Int(2, 6);
            for (int i = 0; i < extraCoins; ++i)
            {
                var coin = new Coin(false);
                coin.model.Position = VectorExt.AddXZ(model.Position, Ref.rnd.vector2_cirkle(area.HalfSizeX));
                coin.setDrift(SpaceLib.DeathRubbleSpeed);
            }
        }

        protected override void outsideMapEvent()
        {
            base.outsideMapEvent();
            resetTail();
        }

        override public GameObjectType Type { get { return GameObjectType.Lorry; } }
        override public CollisionType CollisionType { get { return CollisionType.BodyCollision; } }
    }

    class CoinTailPart
    {
        const float CoinsSpacing = 1f;
         CoinTailPart child = null;
         Vector3 position;
        bool extraValue;

         Coin coin = null;

         public CoinTailPart(ref int countLeft, int extraValIx, Vector3 startPos)
         {
             this.position = startPos;
            extraValue = countLeft == extraValIx;
             if (--countLeft > 0)
             {
                 child = new CoinTailPart(ref countLeft, extraValIx, startPos);
             }
         }

         public void generateCoin()
         {
             if (coin == null)
             {
                 coin = new Coin(false, extraValue? CoinValue.Value5 : CoinValue.Value1);                 
             }

             if (child != null)
             {
                 child.generateCoin();
             }
         }

         public void Update(Vector3 parentPos, float maxLength, ref int coinCount)
         {
             Vector3 diff = parentPos - position;
             if (VectorExt.Length(diff.X, diff.Z) > maxLength)
             {
                 diff.Y = 0;
                 position += diff * 0.02f;
             }

             child?.Update(position, CoinsSpacing, ref coinCount);
             
             if (coin != null)
             {
                ++coinCount;
                 coin.setPosition(position);
                 if (coin.isDeleted)
                 {
                     coin = null;
                 }
             }
         }

        public void setDrift()
        {
            coin?.setDrift(SpaceLib.DeathRubbleSpeed);
            child?.setDrift();
        }

        public void DeleteMe()
        {
            coin?.DeleteMe();
            child?.DeleteMe();
        }
    }
}

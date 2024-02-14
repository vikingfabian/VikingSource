using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.SpaceWar
{
    class Coin : AbsGameObject
    {
        public const float Scale = 1.6f;
        public CoinValue value;
        bool freeFloating;

        Time blockGamerTime;
        public Gamer blockGamer;

        public Coin(bool freeFloating, CoinValue value = CoinValue.Value1)
            : base()
        {
            this.value = value;
            this.freeFloating = freeFloating;

            SpriteName sprite;
            float scale = Scale;
            switch (value)
            {
                default: sprite = SpriteName.pjGoldValue1; break;

                case CoinValue.Value5: sprite = SpriteName.pjGoldValue2; break;
                case CoinValue.Value10: sprite = SpriteName.pjGoldValue3; break;
                case CoinValue.Value25: sprite = SpriteName.pjGoldValue4; break;
                case CoinValue.Value100:
                    sprite = SpriteName.pjGoldValue5;
                    scale = Scale * 2f;
                    break;
            }

            model = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, new Vector3(scale), 
                Graphics.TextureEffectType.Flat, sprite, Color.White);

            if (freeFloating)
            {
                model.X = Ref.rnd.Plus_MinusF(WorldMap.MapHalfW);
                model.Z = Ref.rnd.Plus_MinusF(WorldMap.MapHalfW);

                setDrift();
            }
            var cirkbound = new Physics.CircleBound(Vector2.Zero, model.Scale1D * 0.5f);
            bound = new Physics.Bound2DWrapper(true, cirkbound);

            SpaceRef.go.Add(this);
        }

        public void setDrift(float speed = SpaceLib.DriftSpeed)
        {
            velocity.Set(Rotation1D.Random(), Ref.rnd.Plus_MinusPercent(speed, 0.1f));
            freeFloating = true;
        }

        public void addForce(Vector2 force)
        {
            velocity.Add(force);
            freeFloating = true;
        }

        public override bool update()
        {
            if (freeFloating)
            {
                updateMovement();
            }

            if (blockGamer != null)
            {
                if (blockGamerTime.CountDown())
                {
                    blockGamer = null;
                }
            }

            return base.update();
        }

        public void setPosition(Vector3 pos)
        {
            model.Position = pos;
            bound.update(pos, 0);
        }

        public static int GetCoinValue(CoinValue value)
        {
            switch (value)
            {
                default: return 1;

                case CoinValue.Value5: return 5;
                case CoinValue.Value10: return 10;
                case CoinValue.Value25: return 25;
                case CoinValue.Value100: return 100;
            }
        }

        public static List<Coin> DropAmount(int amount, Vector3 pos, Gamer gamer)
        {
            List<Coin> coins = new List<Coin>(8);

            for (CoinValue valueType = CoinValue.Value100; valueType >= CoinValue.Value1; --valueType)
            {
                int value = GetCoinValue(valueType);
                while (amount >= value)
                {
                    amount -= value;
                    var c = new Coin(false, valueType);
                    c.model.Position = pos;
                    c.setDrift(SpaceLib.DeathRubbleSpeed);
                    if (gamer != null)
                    {
                        c.setGamerBlock(gamer);
                    }
                    coins.Add(c);
                }
            }

            return coins;
        }

        public void setGamerBlock(Gamer gamer)
        {
            blockGamer = gamer;
            blockGamerTime.MilliSeconds = 600;
        }

        override public GameObjectType Type { get { return GameObjectType.Coin; } }
        override public CollisionType CollisionType { get { return CollisionType.PickUp; } }
    }
}

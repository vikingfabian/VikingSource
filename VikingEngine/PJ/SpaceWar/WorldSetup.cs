using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar
{
    class WorldSetup
    {
        public WorldSetup(WorldMap map)
        {
            for (int i = 0; i < 200; ++i)
            {
                var a = new Asteroid();
            }

            for (int i = 0; i < 200; ++i)
            {
                var c = new Coin(true);
            }

            for (int i = 0; i < 10; ++i)
            {
                var chest = new Coin(true, CoinValue.Value25);
            }

            for (int i = 0; i < 100; ++i)
            {
                var l = new Lorry();
            }


            const float PercEdgeToCornerShops = 0.2f;
            createShops(Vector2.Zero);

            createShops(map.PlayerBounds.PercentToPosition(new Vector2(PercEdgeToCornerShops, PercEdgeToCornerShops)));
            createShops(map.PlayerBounds.PercentToPosition(new Vector2(1 - PercEdgeToCornerShops, PercEdgeToCornerShops)));

            createShops(map.PlayerBounds.PercentToPosition(new Vector2(PercEdgeToCornerShops, 1 - PercEdgeToCornerShops)));
            createShops(map.PlayerBounds.PercentToPosition(new Vector2(1 - PercEdgeToCornerShops, 1 - PercEdgeToCornerShops)));
        }

        void createShops(Vector2 center)
        {
            GameObjects.ShopSquareType[] shops = new GameObjects.ShopSquareType[]
                {
                    GameObjects.ShopSquareType.AddTail,
                    GameObjects.ShopSquareType.TailExpansion,
                    GameObjects.ShopSquareType.TailKnife,
                    GameObjects.ShopSquareType.NoseBomb,
                };

            for (int i = 0; i < shops.Length; ++i)
            {
                var area = Table.CellPlacement(center, true, i, shops.Length,
                    new Vector2(GameObjects.ShopSquare.Size), new Vector2(GameObjects.ShopSquare.Spacing));

                new GameObjects.ShopSquare(shops[i], area.Center);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class BoardPlacementArray
    {
        bool centerOriented;
        public List<Vector2> placements = new List<Vector2>(16);
        BagatellePlayState state;

        public BoardPlacementArray(BagatellePlayState state)
        {
            this.state = state;
        }

        public void placeRow(ref float rowY, bool centerOriented, VectorRect bounds, PcgRandom rnd)
        {
            float spaceX = rnd.Float(1.12f, 2.2f) * state.BallScale;
            this.centerOriented = centerOriented;
            Vector2 center = new Vector2(bounds.Center.X, rowY);
            rowY += state.BallScale * 0.8f;
            bounds.Height += 100;

            for (int dirIx = 0; dirIx < 2; ++dirIx)
            {
                int dir = dirIx == 0 ? -1 : 1;

                int index = 0;
                Vector2 pos = center;

                if (centerOriented)
                {
                    if (dir < 0)
                    {
                        pos.X += spaceX * dir;
                        index++;
                    }
                }
                else
                {
                    pos.X += spaceX * dir * 0.5f;
                    if (dir > 0)
                    {
                        index = 1;
                    }
                }

                do
                {
                    if (dir < 0)
                    {
                        placements.Insert(0, pos);
                    }
                    else
                    {
                        placements.Add(pos);
                    }
                    pos.X += spaceX * dir;
                    index++;
                } while (bounds.IntersectPoint(pos));
            }

        }

        public BoardPlacementArray rowBelow(ref float rowY)
        {
            BoardPlacementArray result = new BoardPlacementArray(state);
            
            for (int i = 0; i < placements.Count - 1; ++i)
            {
                Vector2 pos = new Vector2((placements[i].X + placements[i + 1].X) * 0.5f, rowY);
                result.placements.Add(pos);
                result.centerOriented = !this.centerOriented;
            }

            rowY += state.BallScale * 1.0f;

            return result;
        }

        public void createItemsOnRow(GameObjectType[] items, PcgRandom rnd)
        {
            ArrayWithSelection<GameObjectType> itemSel = new ArrayWithSelection<GameObjectType>(items);
            itemSel.SelectRandom(rnd);

            foreach (var m in placements)
            {
                createItem(m, itemSel.Selected());
                itemSel.Next(true);
            }
        }

        public void createItem(Vector2 pos, GameObjectType item)
        {
            switch (item)
            {
                case GameObjectType.Peg:
                    new Peg(pos, state.board.NextItemIndex(), false, state);
                    break;

                case GameObjectType.Coin:
                case GameObjectType.CoinOutLine:
                case GameObjectType.BigCoin5:
                case GameObjectType.BigCoin10:
                case GameObjectType.BigCoin20:
                    new Coin(pos, state.board.NextItemIndex(), state, item);
                    break;

                case GameObjectType.CoinPeg:
                    new CoinPeg(pos, state.board.NextItemIndex(), state);
                    break;
                case GameObjectType.Spikes:
                    new Spikes(pos, state.board.NextItemIndex(), state);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void createSideItem(GameObjectType item, PcgRandom rnd)
        {
            List<Vector2> sides = pullRandomSidePlacements(rnd);
            foreach (var m in sides)
            {
                createItem(m, item);
            }
        }

        public List<Vector2> pullRandomSidePlacements(PcgRandom rnd)
        {
            int sideIndex = rnd.Int(placements.Count / 2 - 1);

            int rightIx = placements.Count - 1 - sideIndex;
            List<Vector2> result = new List<Vector2> { placements[sideIndex], placements[rightIx] };

            placements.RemoveAt(rightIx); placements.RemoveAt(sideIndex);

            return result;
        }
    }
}

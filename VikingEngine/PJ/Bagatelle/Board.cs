using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class Board
    {
        BagatellePlayState state;
        public Cannon cannon;
        Vector2[] rowSpawns;
        bool rowSpawnsStartLeft;
        Time spawnTimer = new Time(5, TimeUnit.Seconds);
        int nextItemIndex = 0;
        //public int nextItemMoverIndex = 0;

        Sound.SoundSettings fireSound = new Sound.SoundSettings(LoadedSound.flowerfire, 1.6f, 0, 0.05f, 0);

        public Board(VectorRect activeArea, BagatellePlayState state, ulong seed)
        {
            this.state = state;
            state.board = this;
            cannon = new Cannon(activeArea, state);
            Debug.Log("Board seed: " + seed.ToString());

            PcgRandom rnd = new PcgRandom(seed);

            rowSpawnsStartLeft = rnd.Bool();

            VectorRect pegsArea = activeArea;
            pegsArea.AddToTopSide(-activeArea.Height * 0.15f);
            pegsArea.Height -= activeArea.Height * 0.1f;
            pegsArea.AddXRadius(-activeArea.Height * 0.06f);

            int rowCount = rnd.Int(6, 8);

            rowSpawns = new Vector2[rowCount - 1];

            for (int i = 0; i < rowCount; ++i)
            {
                row(pegsArea, pegsArea.PercentToPosition(new Vector2((float)i / (rowCount - 1))).Y, rnd, i, rowCount - 1 - i);
            }
        }

        bool prevCenterOriented = true;
        int prevCenterOrientCount = 0;

        public void FireSound()
        {
            fireSound.Play(cannon.image.Position);
        }

        void row(VectorRect activeArea, float rowY, PcgRandom rnd, int topIndex, int bottomIndex)
        {
           
            bool centerOriented = rnd.Bool();
            

            if (prevCenterOriented == centerOriented)
            {
                prevCenterOrientCount++;
                if (prevCenterOrientCount >= 2)
                {
                    prevCenterOrientCount = 0;
                    centerOriented = !centerOriented;
                }
            }
            else
            {
                prevCenterOrientCount = 0;
            }
            prevCenterOriented = centerOriented;


            BoardPlacementArray topRow = new BoardPlacementArray(state);
            topRow.placeRow(ref rowY, centerOriented, activeArea, rnd);
            Vector2 spawnPos = new Vector2(0, rowY);

            BoardPlacementArray bottomRow = topRow.rowBelow(ref rowY);

            if (bottomIndex == 0)
            {
                topRow.createSideItem(GameObjectType.BigCoin20, rnd);
                topRow.createItemsOnRow(new GameObjectType[] { GameObjectType.BigCoin5, GameObjectType.BigCoin10 }, rnd);
            }
            else
            {
                int rndPegSidesVal = rnd.Int(100);

                if (rndPegSidesVal < 50 && state.matchCount > 2)
                {
                    topRow.createSideItem(GameObjectType.Spikes, rnd);
                }

                int rndPegRowVal = rnd.Int(100);
                GameObjectType[] pegRow;
                if (rndPegRowVal < 20)
                {
                    pegRow = new GameObjectType[] { GameObjectType.CoinPeg };
                }
                else if (rndPegRowVal < 50)
                {
                    pegRow = new GameObjectType[] { GameObjectType.Peg,  GameObjectType.CoinPeg };
                }
                else
                {
                   pegRow = new GameObjectType[] { GameObjectType.Peg };
                }

                topRow.createItemsOnRow(pegRow, rnd);

                GameObjectType[] coinRow;
                int rndCoinRowVal = rnd.Int(100);

                if (rndCoinRowVal < 40)
                {
                    coinRow = new GameObjectType[] { GameObjectType.Coin };
                }
                else if (rndCoinRowVal < 60)
                {
                    coinRow = new GameObjectType[] { GameObjectType.CoinOutLine };
                }
                else if (rndCoinRowVal < 70 && bottomIndex > 1)
                {
                    coinRow = new GameObjectType[] { GameObjectType.Coin, GameObjectType.BigCoin5 };
                }
                else if (rndCoinRowVal < 80 && topIndex > 0)
                {
                    coinRow = new GameObjectType[] { GameObjectType.Coin, GameObjectType.CoinPeg };
                }
                else
                {
                    coinRow = new GameObjectType[] { GameObjectType.Coin, GameObjectType.CoinOutLine };
                }

                bottomRow.createItemsOnRow(coinRow, rnd);

                if (rowSpawnsStartLeft == lib.IsEven(topIndex))
                {
                    spawnPos.X = activeArea.X - state.BallScale * 2f;
                }
                else
                {
                    spawnPos.X = activeArea.Right + state.BallScale * 2f;
                }
                rowSpawns[topIndex] = spawnPos;
            }           
        }

        static readonly RandomObjects<GameObjectType> SpawnItemChance = new RandomObjects<GameObjectType>(
        
            new ObjectCommonessPair<GameObjectType>(140, GameObjectType.Spikes),
            new ObjectCommonessPair<GameObjectType>(100, GameObjectType.BigCoin10),
            new ObjectCommonessPair<GameObjectType>(100, GameObjectType.Snake),
            new ObjectCommonessPair<GameObjectType>(120, GameObjectType.BumpRefill),
            new ObjectCommonessPair<GameObjectType>(120, GameObjectType.PowerUpBox)
        );

        int prevSpawnRow = -1;
        public void update()
        {
            cannon.update();

            if (PjRef.host)
            {
                if (spawnTimer.CountDown())
                {
                    spawnTimer.Seconds = Ref.rnd.Float(0.2f, 10f);
                    int row;
                    do
                    {
                        row = Ref.rnd.Int(rowSpawns.Length);
                    } while (row == prevSpawnRow);

                    Vector2 pos = rowSpawns[row];
                    int dir = lib.BoolToLeftRight(pos.X < state.gamePlayArea.Center.X);
                    prevSpawnRow = row;


                    new ItemMover(pos, dir, SpawnItemChance.GetRandom(), state);
                }
            }
        }

        public Vector2 ballStartPos()
        {
            return new Vector2(cannon.image.Xpos, cannon.fireY);
        }
        public Vector2 ballStartSpeed()
        {
            return new Vector2(0f, state.Gravity * 30f);
        }
        public int ballStartDir()
        {
            return cannon.moveDir;
        }

        public int NextItemIndex()
        {
            Debug.Log("#Next index: " + nextItemIndex.ToString());
            return nextItemIndex++;
        }
        public int NextItemIndex(int range)
        {
            int result = nextItemIndex;
            nextItemIndex += range;
            return result;
        }

        enum ItemVariant
        {
            Variant1,
            Variant2,
            EveryEven,
            EveryOdd,
            NUM
        }
    }

   
}

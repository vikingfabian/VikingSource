using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class ItemsLayout
    {
        static readonly Range BunkerSzRange = new Range(2, 4);
        static readonly Range SpikeSzRange = new Range(1, 5);

        public ItemsLayout(FieldDataCollection dataCollection)
        {
            bool useDamageTraps = GolfRef.gamestate.matchCount > 1;
            new GO.LaunchCannon(); 

            foreach (var kv in dataCollection.areas)
            {
                FieldArea area = kv.Value;
                
                int coins = area.randomItemCount(0, 8);
                for (int i = 0; i < coins; ++i)
                {
                    new Coin(area.RandomSquareWp(), CoinValue.Value1, true);
                }

                if (area.HasSquares && Ref.rnd.Chance(0.2))
                {
                    new Bomb(area.RandomSquare());
                }

                double areaChance = Ref.rnd.Double();
                if (areaChance < 0.12)
                {
                    IntVector2 sz = new IntVector2(BunkerSzRange.GetRandom(), BunkerSzRange.GetRandom());
                    Rectangle2 rndArea;
                    if (area.randomArea(sz, true, out rndArea))
                    {
                        GolfRef.field.addTerrain(rndArea, FieldTerrainType.Sand);
                    }
                }
                else if (areaChance < 0.4)
                {
                    if (useDamageTraps)
                    {
                        IntVector2 sz = new IntVector2(SpikeSzRange.GetRandom(), SpikeSzRange.GetRandom());
                        Rectangle2 rndArea;
                        if (area.randomArea(sz, true, out rndArea))
                        {
                            GolfRef.field.addTerrain(rndArea, FieldTerrainType.Spikes);
                        }
                    }
                }

                if (area.HasSquares)
                {
                    new FlagPoint(area.RandomSquareWp());
                }

                GolfRef.field.createTerrainTextures();
            }

            foreach (var kv in dataCollection.chokes)
            {
                Choke choke = kv.Value;

                var rndChoke = Ref.rnd.Double();
                if (rndChoke < 0.2)
                {
                    var squares = choke.squares();

                    foreach (var m in squares)
                    {
                        new BlockingBox(m);
                    }
                }
                else if (rndChoke < 0.4)
                {                   
                    new GO.ObsticleBug(choke.start, choke.end);
                }
            }
        }
    }
}

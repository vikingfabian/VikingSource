using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Data
{
    static class NameGenerator
    {
        static PcgRandom random = new PcgRandom();

        public static string RandomLetters(int seed)
        {
            random.SetSeed(seed);
            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);
            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);
            
            return $"{TextLib.FirstLetters(title, 3)}{TextLib.FirstLetters(adjective, 3)}{TextLib.FirstLetters(color, 3)}{TextLib.FirstLetters(creature, 3)}{TextLib.FirstLetters(place, 3)}{TextLib.FirstLetters(symbol, 3)}";
        }

        public static string ArmyName(int armyId)
        {
            random.SetSeed(armyId + DssRef.world.metaData.objSeed);

            if (Ref.gamesett.language == LanguageType.Thai)
            {
                // Thai Grammar: usually Noun -> Color -> Adjective
                // We use "|" delimiters so ThaiConv can replace them with Zero-Width Spaces.

                int namingVariant = random.Int(12);

                switch (namingVariant)
                {
                    case 0: // English: Adjective Color Creature -> Thai: Creature Color Adjective
                        {
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{creature}|{color}|{adjective}");
                        }

                    case 1: // English: Creature of the Adjective Place -> Thai: Creature Place Adjective
                        {
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
                            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            // Thai doesn't strictly need "of the". Juxtaposition works well.
                            return TextLib.ThaiConv($"{creature}|{place}|{adjective}");
                        }

                    case 2: // English: Color Place -> Thai: Place Color
                        {
                            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);

                            return TextLib.ThaiConv($"{place}|{color}");
                        }

                    case 3: // English: Adjective Color Place -> Thai: Place Color Adjective
                        {
                            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{place}|{color}|{adjective}");
                        }

                    case 4: // English: Title of the Symbol -> Thai: Title Symbol
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);

                            return TextLib.ThaiConv($"{title}|{symbol}");
                        }

                    case 5: // English: Title of the Creature -> Thai: Title Creature
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);

                            return TextLib.ThaiConv($"{title}|{creature}");
                        }

                    case 6: // English: Title of the Adj Creature -> Thai: Title Creature Adjective
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{title}|{creature}|{adjective}");
                        }

                    case 7: // English: Adjective Title -> Thai: Title Adjective
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{title}|{adjective}");
                        }

                    case 8: // English: Color Adj Title -> Thai: Title Color Adjective
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{title}|{color}|{adjective}");
                        }

                    case 9: // English: Adj Symbol Title -> Thai: Title Symbol Adjective
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{title}|{symbol}|{adjective}");
                        }

                    case 10: // English: Color Symbol -> Thai: Symbol Color
                        {
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);

                            return TextLib.ThaiConv($"{symbol}|{color}");
                        }

                    case 11: // English: Adj Color Symbol -> Thai: Symbol Color Adjective
                        {
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);

                            return TextLib.ThaiConv($"{symbol}|{color}|{adjective}");
                        }

                    default:
                        throw new NotImplementedException();
                }
            }
            else
            {

                int namingVariant = random.Int(12);

                switch (namingVariant)
                {
                    case 0:
                        {
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);

                            return $"{adjective} {color} {creature}";
                        }

                    case 1:
                        {
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);

                            return string.Format(DssRef.lang.NameGenerator_AOfTheB, creature, $"{adjective} {place}");
                            //return $"{creature} of the {adjective} {place}";
                        }

                    case 2:
                        {
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);

                            return $"{color} {place}";
                        }

                    case 3:
                        {
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);

                            return $"{adjective} {color} {place}";
                        }

                    case 4:
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);

                            return string.Format(DssRef.lang.NameGenerator_AOfTheB, title, symbol);
                            //return $"{title} of the {symbol}";
                        }

                    case 5:
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);

                            return string.Format(DssRef.lang.NameGenerator_AOfTheB, title, creature);
                            //return $"{title} of the {creature}";
                        }
                    case 6:
                        {
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);

                            return string.Format(DssRef.lang.NameGenerator_AOfTheB, title, $"{adjective} {creature}");
                            //return $"{title} of the {adjective} {creature}";
                        }

                    case 7:
                        {

                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);

                            return $"{adjective} {title}";
                        }
                    case 8:
                        {
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);

                            return $"{color} {adjective} {title}";
                        }

                    case 9:
                        {
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);
                            string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);

                            return $"{adjective} {symbol} {title}";
                        }

                    case 10:
                        {
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);

                            return $"{color} {symbol}";
                        }

                    case 11:
                        {
                            string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                            string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                            string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols, random);

                            return $"{adjective} {color} {symbol}";
                        }

                    default:
                        throw new NotImplementedException();
                }
            }
        }


        public static string CityName(IntVector2 pos)
        {
            random.SetSeed(pos.X * 3 + pos.Y * 11 + DssRef.world.metaData.objSeed);

            string cityName = "";

            bool west = pos.X < DssRef.world.Size.X * 0.75;
            bool north = pos.Y < DssRef.world.HalfSize.Y;

            // Randomly decide to add a space and suffix or just append a suffix
            if (random.Chance(0.6))
            {
                syllables(west, north, random.Int(1, 4), ref cityName);
                townSyffix(west, north, random.Chance(0.4), ref cityName);
            }
            else
            {
                //split name
                syllables(west, north, random.Int(1, 3), ref cityName);
                cityName += " ";
                syllables(west, north, random.Int(1, 3), ref cityName);
                townSyffix(west, north, random.Chance(0.1), ref cityName);
            }

            return TextLib.LargeFirstLetter(cityName);
        }

        static void syllables(bool west, bool north, int count, ref string cityName)
        {
            List<string> syllables = null;

            for (int i = 0; i < count; i++)
            {
                if (i == 0 || random.Chance(0.5))
                {
                    if (random.Chance(0.3))
                    {
                        syllables = DssRef.lang.NameGenerator_City_GeneralSyllables;//generalSyllables;
                    }
                    else if (random.Chance(0.5))
                    {
                        syllables = west ? DssRef.lang.NameGenerator_City_WestSyllables : DssRef.lang.NameGenerator_City_EastSyllables;//westSyllables : eastSyllables;
                    }
                    else
                    {
                        syllables = north ? DssRef.lang.NameGenerator_City_NorthSyllables : DssRef.lang.NameGenerator_City_SouthSyllables;//northSyllables : southSyllables;
                    }
                }

                cityName += arraylib.RandomListMember(syllables, random);
            }
        }

        static void townSyffix(bool west, bool north, bool space, ref string cityName)
        {   
            List<string> suffixes;
            if (random.Chance(0.3))
            {
                suffixes = DssRef.lang.NameGenerator_City_GeneralTownSuffixes;//generalTownSuffixes;
            }
            else if (random.Chance(0.5))
            {
                suffixes = west ? DssRef.lang.NameGenerator_City_WestTownSuffixes : DssRef.lang.NameGenerator_City_EastTownSuffixes;//westTownSuffixes : eastTownSuffixes;
            }
            else
            {
                if (north)
                {
                    suffixes = DssRef.lang.NameGenerator_City_NorthTownSuffixes;//northTownSuffixes;
                }
                else
                {
                    suffixes = DssRef.lang.NameGenerator_City_SouthTownSuffixes;//southTownSuffixes;
                    space = false;
                }
            }
                
            if (space)
            {
                cityName += " ";
            }
            cityName += arraylib.RandomListMember(suffixes, random);
            
        }

    }
}

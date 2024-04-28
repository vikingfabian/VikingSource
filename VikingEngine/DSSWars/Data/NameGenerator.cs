using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Data
{
    static class NameGenerator
    {
        static PcgRandom random = new PcgRandom();
        
        public static string ArmyName(int armyId)
        {
            random.SetSeed(armyId + DssRef.world.metaData.objSeed);
            int namingVariant = random.Int(12);

            switch (namingVariant)
            {
                case 0:
                    {
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors, random);
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
;
                        return $"{adjective} {color} {creature}";
                    }

                case 1:
                    {
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                        string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places, random);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, creature, $"{adjective} {place}");
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

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, title, symbol);
                        //return $"{title} of the {symbol}";
                    }

                case 5:
                    {
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, title, creature);
                        //return $"{title} of the {creature}";
                    }
                case 6:
                    {
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles, random);
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives, random);
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures, random);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, title, $"{adjective} {creature}");
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

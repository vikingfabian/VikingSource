﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.Graphics;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.DSSWars.Data
{
    static class NameGenerator
    {
        public static string ArmyName()
        {
            int namingVariant = Ref.rnd.Int(12);

            switch (namingVariant)
            {
                case 0:
                    {
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors);
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures);
;
                        return $"{adjective} {color} {creature}";
                    }

                case 1:
                    {
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures);
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, creature, $"{adjective} {place}");
                        //return $"{creature} of the {adjective} {place}";
                    }

                case 2:
                    {
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors);
                        string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places);

                        return $"{color} {place}";
                    }

                case 3:
                    {
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors);
                        string place = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Places);

                        return $"{adjective} {color} {place}";
                    }

                case 4:
                    {
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles);
                        string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, title, symbol);
                        //return $"{title} of the {symbol}";
                    }

                case 5:
                    {
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles);
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, title, creature);
                        //return $"{title} of the {creature}";
                    }
                case 6:
                    {
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles);
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string creature = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Creatures);

                        return string.Format(DssRef.lang.NameGenerator_XOfTheY, title, $"{adjective} {creature}");
                        //return $"{title} of the {adjective} {creature}";
                    }

                case 7:
                    {
                       
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles);

                        return $"{adjective} {title}";
                    }
                case 8:
                    {
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors);
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles);

                        return $"{color} {adjective} {title}";
                    }

                case 9:
                    {
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols);
                        string title = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Titles);

                        return $"{adjective} {symbol} {title}";
                    }

                case 10:
                    {
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors);
                        string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols);

                        return $"{color} {symbol}";
                    }

                case 11:
                    {
                        string adjective = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Adjectives);
                        string color = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Colors);
                        string symbol = arraylib.RandomListMember(DssRef.lang.NameGenerator_Army_Symbols);

                        return $"{adjective} {color} {symbol}";
                    }

                default:
                    throw new NotImplementedException();
            }

        }


        public static string CityName(IntVector2 pos)
        {
            
            string cityName = "";

            bool west = pos.X < DssRef.world.Size.X * 0.75;
            bool north = pos.Y < DssRef.world.HalfSize.Y;

            // Randomly decide to add a space and suffix or just append a suffix
            if (Ref.rnd.Chance(0.6))
            {
                syllables(west, north, Ref.rnd.Int(1, 4), ref cityName);
                townSyffix(west, north, Ref.rnd.Chance(0.4), ref cityName);
            }
            else
            {
                //split name
                syllables(west, north, Ref.rnd.Int(1, 3), ref cityName);
                cityName += " ";
                syllables(west, north, Ref.rnd.Int(1, 3), ref cityName);
                townSyffix(west, north, Ref.rnd.Chance(0.1), ref cityName);
            }

            return TextLib.LargeFirstLetter(cityName);
        }

        static void syllables(bool west, bool north, int count, ref string cityName)
        {
            List<string> syllables = null;

            for (int i = 0; i < count; i++)
            {
                if (i == 0 || Ref.rnd.Chance(0.5))
                {
                    if (Ref.rnd.Chance(0.3))
                    {
                        syllables = DssRef.lang.NameGenerator_City_GeneralSyllables;//generalSyllables;
                    }
                    else if (Ref.rnd.Chance(0.5))
                    {
                        syllables = west ? DssRef.lang.NameGenerator_City_WestSyllables : DssRef.lang.NameGenerator_City_EastSyllables;//westSyllables : eastSyllables;
                    }
                    else
                    {
                        syllables = north ? DssRef.lang.NameGenerator_City_NorthSyllables : DssRef.lang.NameGenerator_City_SouthSyllables;//northSyllables : southSyllables;
                    }
                }

                cityName += arraylib.RandomListMember(syllables);
            }
        }

        static void townSyffix(bool west, bool north, bool space, ref string cityName)
        {   
            List<string> suffixes;
            if (Ref.rnd.Chance(0.3))
            {
                suffixes = DssRef.lang.NameGenerator_City_GeneralTownSuffixes;//generalTownSuffixes;
            }
            else if (Ref.rnd.Chance(0.5))
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
            cityName += arraylib.RandomListMember(suffixes);
            
        }

    }
}
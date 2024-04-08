using System;
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
        static readonly List<string> adjectives = new List<string> {
            "Valiant", "Mystic", "Dark", "Golden", "Ancient", "Frozen", "Eternal",
            "Shadowy", "Bright", "Crimson", "Fierce", "Glorious", "Noble", "Savage",
            "Vengeful", "Brave", "Stormy", "Majestic", "Ruthless", "Cunning", "Radiant",
            "Twilight", "Dawn", "Dusk", "Iron", "Silver", "Spectral", "Celestial", "Infernal",
            "Enchanted", "Arcane", "Hidden", "Lost", "Forgotten", "Legendary", "Mythic",
            "Silent", "Thundering", "Burning", "Shattered", "Wandering", "Ethereal", "Phantom",
            "Emerald", "Ruby", "Sapphire", "Diamond", "Jade", "Strong"
        };

        static readonly List<string> colors = new List<string> {
            "Red", "Black", "White", "Emerald", "Azure", "Scarlet", "Violet", "Indigo",
            "Gold", "Silver", "Bronze", "Copper", "Sapphire", "Ruby", "Amethyst",
            "Jade", "Cerulean", "Crimson", "Magenta", "Ebony", "Ivory", "Teal", "Turquoise",
            "Maroon", "Olive", "Peach", "Grey", "Charcoal", "Lavender", "Lime", "Navy",
            "Ochre", "Plum", "Quartz", "Salmon", "Tan", "Ultramarine", "Vermilion", "Wisteria",
            "Xanadu", "Yellow", "Zaffre", "Azure", "Blue", "Green", "Honeydew",
            "Iris", "Jasmine", "Khaki"
        };

        static readonly List<string> creatures = new List<string> {
            "Dragons", "Wolves", "Eagles", "Lions", "Knights", "Griffins", "Centaurs",
            "Elves", "Dwarves", "Giants", "Angels", "Mermaids", "Unicorns",
            "Phoenixes", "Stags", "Horses", "Falcons", "Tigers", "Bears", "Panthers",
            "Eagles", "Hawks", "Dolphins", "Whales", "Elephants", "Leopards", "Cheetahs",
            "Ravens", "Owls", "Peacocks", "Swans", "Foxes", "Deer",
            "Paladins", "Sorcerers", "Mages", "Rogues", "Samurai", "Ninjas",
            "Archers", "Rangers", "Clerics", "Priests", "Shamans", "Druids",
            "Sphinxes", "Pegasus", "Cougars", "Jaguars", "Bulls", "Serpents"
        };

        static readonly List<string> places = new List<string> {
            "Forest", "Waste", "Ruin", "Oak", "Mountain", "Lake", "River", "Sea",
            "Castle", "Tower", "Dungeon", "Cavern", "Palace", "Temple", "Shrine",
            "Garden", "Village", "City", "Kingdom", "Empire", "Desert", "Glacier",
            "Volcano", "Valley", "Cliff", "Fortress", "Harbor", "Isle", "Peninsula",
            "Plain", "Quagmire", "Reef", "Savannah", "Tundra", "Underworld", "Vortex",
            "Wellspring", "Grove", "Meadow", "Fjord", "Canyon", "Plateau", "Marsh",
            "Swamp", "Forest Glade", "Moon", "Star", "Galaxy", "Nebula", "Asteroid",
            "Comet", "Meteor", "Black Hole", "Void", "Nexus", "Dimension", "Sanctuary",
            "Arena", "Coliseum", "Academy", "Library", "Archive"
        };

        static readonly List<string> titles = new List<string> {
            "Legion", "Brigade", "Cohort", "Battalion", "Regiment", "Division", "Company",
            "Squadron", "Platoon", "Troop", "Detachment", "Contingent", "Phalanx", "Squad",
            "Team", "Unit", "Force", "Host", "Horde", "Army", "Navy", "Fleet", "Flotilla",
            "Wing", "Group", "Pack", "Circle", "Council", "Assembly", "Guild", "Order",
            "Fellowship", "Clan", "Tribe", "Kinship", "Dynasty", "Empire", "Kingsmen",
            "Principality", "Duchy", "Barony", "Chapter", "Covenant", "Syndicate",
            "Coalition", "Alliance", "Confederation", "Federation", "League", "Society",
            "Academy", "Institute", "Men", "People", "Power"
        };

        static readonly List<string> symbols = new List<string> {
            "Lily", "Tower", "Spear", "Shield", "Crown", "Sword", "Castle", "Star",
            "Moon", "Sun", "Comet", "Flame", "Wave", "Mountain", "Tree", "Forest",
            "River", "Stone", "Anvil", "Hammer", "Axe", "Bow", "Arrow", "Quiver",
            "Helmet", "Gauntlet", "Armor", "Chain", "Key", "Ring", "Lock", "Book", "Scroll",
            "Potion", "Orb", "Throne", "Banner", "Ring", "Gem", "Pyramid", "Obelisk",
            "Tower", "Bridge", "Gate", "Wall", "Chalice", "Lantern", "Candle", "Bell",
            "Feather", "Glass", "Compass"
        };

        public static string ArmyName()
        {
            int namingVariant = Ref.rnd.Int(12);

            switch (namingVariant)
            {
                case 0:
                    {
                        string adjective = arraylib.RandomListMember(adjectives);
                        string color = arraylib.RandomListMember(colors);
                        string creature = arraylib.RandomListMember(creatures);
;
                        return $"{adjective} {color} {creature}";
                    }

                case 1:
                    {
                        string creature = arraylib.RandomListMember(creatures);
                        string adjective = arraylib.RandomListMember(adjectives);
                        string place = arraylib.RandomListMember(places);

                        return $"{creature} of the {adjective} {place}";
                    }

                case 2:
                    {
                        string color = arraylib.RandomListMember(colors);
                        string place = arraylib.RandomListMember(places);

                        return $"{color} {place}";
                    }

                case 3:
                    {
                        string adjective = arraylib.RandomListMember(adjectives);
                        string color = arraylib.RandomListMember(colors);
                        string place = arraylib.RandomListMember(places);

                        return $"{adjective} {color} {place}";
                    }

                case 4:
                    {
                        string title = arraylib.RandomListMember(titles);
                        string symbol = arraylib.RandomListMember(symbols);

                        return $"{title} of the {symbol}";
                    }

                case 5:
                    {
                        string title = arraylib.RandomListMember(titles);
                        string creature = arraylib.RandomListMember(creatures);

                        return $"{title} of the {creature}";
                    }
                case 6:
                    {
                        string title = arraylib.RandomListMember(titles);
                        string adjective = arraylib.RandomListMember(adjectives);
                        string creature = arraylib.RandomListMember(creatures);

                        return $"{title} of the {adjective} {creature}";
                    }

                case 7:
                    {
                       
                        string adjective = arraylib.RandomListMember(adjectives);
                        string title = arraylib.RandomListMember(titles);

                        return $"{adjective} {title}";
                    }
                case 8:
                    {
                        string color = arraylib.RandomListMember(colors);
                        string adjective = arraylib.RandomListMember(adjectives);
                        string title = arraylib.RandomListMember(titles);

                        return $"{color} {adjective} {title}";
                    }

                case 9:
                    {
                        string adjective = arraylib.RandomListMember(adjectives);
                        string symbol = arraylib.RandomListMember(symbols);
                        string title = arraylib.RandomListMember(titles);

                        return $"{adjective} {symbol} {title}";
                    }

                case 10:
                    {
                        string color = arraylib.RandomListMember(colors);
                        string symbol = arraylib.RandomListMember(symbols);

                        return $"{color} {symbol}";
                    }

                case 11:
                    {
                        string adjective = arraylib.RandomListMember(adjectives);
                        string color = arraylib.RandomListMember(colors);
                        string symbol = arraylib.RandomListMember(symbols);

                        return $"{adjective} {color} {symbol}";
                    }

                default:
                    throw new NotImplementedException();
            }

        }

        static readonly List<string> generalSyllables = new List<string> 
        {
            "ar", "bel", "car", "dun", "el", "fen", "glen", "hal", "iver", "jun",
            "kel", "lim", "mon", "nor", "oak", "pel", "quen", "ril", "sen", "tal",
            "urn", "vel", "wel", "xen", "yel", "zel", "ash", "bro", "cre", "dell",
            "eck", "fay", "gil", "her", "isk", "jor", "kay", "lon", "mire", "nock",
            "orp", "penn", "quill", "rost", "sarn", "til", "ud", "vern", "wist", "yarn", "zorn"
        };
        static readonly List<string> generalTownSuffixes = new List<string> 
        { "town", "ford", "burg", "ville", "stead", "wick", "mont", "field", "port", "dale" };

        static readonly List<string> northSyllables = new List<string>
        {
            "fjor", "skol", "varg", "ulv", "frost", "bjorn", "stor", "hvit", "jarn", "sne",
            "kvist", "lund", "nord", "olf", "pil", "rune", "sig", "thor", "ulf", "vald",
            "yng", "aeg", "brim", "drak", "eir", "frej", "gim", "halv", "ivar", "jo",
            "keld", "lyng", "magn", "natt", "odin", "pryd", "quor", "rost", "sif", "tjorn",
            "ulfr", "vid", "wind", "xil", "yrl", "zorn", "aesk", "brok", "dahl", "eng"
        };
        static readonly List<string> northTownSuffixes = new List<string> 
        { "vik", "stad", "fjord", "berg", "nes", "dal", "heim", "gard", "havn", "land", "ul" };

        static readonly List<string> westSyllables = new List<string>
        {
            "win", "lan", "ham", "ford", "ster", "burg", "shire", "well", "ton", "wick",
            "bard", "clif", "dell", "es", "graf", "holt", "ire", "jest", "kent", "ly",
            "moor", "nor", "ox", "perry", "quen", "rift", "sward", "tre", "ulm", "ver",
            "war", "yate", "zeal", "ard", "beam", "cove", "dale", "eft", "gale", "heath",
            "ingle", "keel", "leith", "marsh", "neath", "ope", "pale", "quill", "rove", "scale", "thatch"
        };
        static readonly List<string> westTownSuffixes = new List<string> 
        { "ton", "burg", "ford", "ham", "shire", "caster", "wick", "bury", "stead", "ville" };

        static readonly List<string> eastSyllables = new List<string>
        {
            "jin", "shi", "yuan", "qing", "lu", "chun", "ming", "nan", "ping", "zhou",
            "bai", "dong", "fu", "guo", "hui", "kai", "lan", "mei", "ni", "ou",
            "pei", "qin", "ran", "su", "tai", "wei", "xi", "yang", "zhu", "an",
            "bo", "ci", "da", "en", "fei", "gang", "hao", "ji", "ken", "lei",
            "mo", "ning", "po", "qi", "rou", "sen", "ting", "wan", "xing", "yu", "zen"
        };
        static readonly List<string> eastTownSuffixes = new List<string> 
        { "yang", "shan", "ji", "an", "hai", "cheng", "lin", "tai", "kou", "fu" };
        
        static readonly List<string> southSyllables = new List<string>
        {
            "the", "ne", "ly", "ca", "re", "si", "mar", "pol", "ath", "cor",
            "del", "eph", "ga", "hel", "io", "kos", "la", "me", "ni", "ol",
            "pa", "rho", "se", "ty", "ur", "ve", "xan", "yra", "ze", "al",
            "bra", "cy", "dra", "ero", "fy", "gre", "hy", "ile", "jo", "kle",
            "leu", "my", "nos", "ope", "phy", "que", "ra", "syr", "tha", "vyr", "wyn"
        };
        static readonly List<string> southTownSuffixes = new List<string> 
        { "polis", "ium", "os", "us", "a", "on", "ora", "aca", "es", "ae" };
        

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
                        syllables = generalSyllables;
                    }
                    else if (Ref.rnd.Chance(0.5))
                    {
                        syllables = west ? westSyllables : eastSyllables;
                    }
                    else
                    {
                        syllables = north ? northSyllables : southSyllables;
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
                suffixes = generalTownSuffixes;
            }
            else if (Ref.rnd.Chance(0.5))
            {
                suffixes = west ? westTownSuffixes : eastTownSuffixes;
            }
            else
            {
                if (north)
                {
                    suffixes = northTownSuffixes;
                }
                else
                {
                    suffixes = southTownSuffixes;
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

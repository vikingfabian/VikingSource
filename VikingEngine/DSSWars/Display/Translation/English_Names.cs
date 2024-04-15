using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class English
    {
        //The name generator create unique names for the armies by combining random words

        /// <summary>
        /// A way to merge to random words
        /// </summary>
        public override string NameGenerator_XOfTheY => "{0} of the {1}";

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

        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;


        /*
        * The name generator create unique names for cities by combining random syllables
        * The syllables are divided into general, north (nordic flavor), west (old english), east (asian) and south (mediterrian)
        * When localizing to a languge close to English, there is no need for a translation
        */

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

        public override List<string> NameGenerator_City_GeneralSyllables => generalSyllables;
        public override List<string> NameGenerator_City_GeneralTownSuffixes => generalTownSuffixes;
        public override List<string> NameGenerator_City_NorthSyllables => northSyllables;
        public override List<string> NameGenerator_City_NorthTownSuffixes => northTownSuffixes;
        public override List<string> NameGenerator_City_WestSyllables => westSyllables;
        public override List<string> NameGenerator_City_WestTownSuffixes => westTownSuffixes;
        public override List<string> NameGenerator_City_EastSyllables => eastSyllables;
        public override List<string> NameGenerator_City_EastTownSuffixes => eastTownSuffixes;
        public override List<string> NameGenerator_City_SouthSyllables => southSyllables;
        public override List<string> NameGenerator_City_SouthTownSuffixes => southTownSuffixes;


    }
}

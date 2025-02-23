using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class German
    {
        /// <summary>
        /// Eine Möglichkeit, zwei zufällige Wörter zu kombinieren
        /// </summary>
        public override string NameGenerator_AOfTheB => "{0} von {1}";

        static readonly List<string> adjectives = new List<string> {
    "Tapfer", "Mystisch", "Dunkel", "Golden", "Uralte", "Gefrorene", "Ewige",
    "Schattenhafte", "Helle", "Purpurne", "Wilde", "Ruhmreiche", "Edle", "Grausame",
    "Rachsüchtige", "Mutige", "Stürmische", "Majestätische", "Erbarmungslose", "Gerissene", "Strahlende",
    "Zwielichtige", "Morgendämmernde", "Abenddämmernde", "Eiserne", "Silberne", "Spektrale", "Himmlische", "Höllische",
    "Verzauberte", "Arkane", "Verborgene", "Verlorene", "Vergessene", "Legendäre", "Mythische",
    "Stille", "Donnernde", "Brennende", "Zerschmetterte", "Wandernde", "Ätherische", "Phantomhafte",
    "Smaragdgrüne", "Rubinrote", "Saphirblaue", "Diamantene", "Jadegrüne", "Starke"
};

        static readonly List<string> colors = new List<string> {
    "Rot", "Schwarz", "Weiß", "Smaragdgrün", "Azurblau", "Scharlachrot", "Violett", "Indigo",
    "Gold", "Silber", "Bronze", "Kupfer", "Saphir", "Rubin", "Amethyst",
    "Jade", "Zyan", "Karminrot", "Magenta", "Ebenholz", "Elfenbein", "Türkis",
    "Bordeauxrot", "Olivgrün", "Pfirsichfarben", "Grau", "Kohle", "Lavendel", "Limettengrün", "Marineblau",
    "Ocker", "Pflaume", "Quarz", "Lachsfarben", "Sandfarben", "Ultramarin", "Zinnoberrot", "Wisteria",
    "Xanadu", "Gelb", "Zaffre", "Blau", "Grün", "Honigtau",
    "Iris", "Jasmin", "Khaki"
};

        static readonly List<string> creatures = new List<string> {
    "Drachen", "Wölfe", "Adler", "Löwen", "Ritter", "Greife", "Zentauren",
    "Elfen", "Zwerge", "Riesen", "Engel", "Meerjungfrauen", "Einhörner",
    "Phönixe", "Hirsche", "Pferde", "Falken", "Tiger", "Bären", "Panther",
    "Eulen", "Pfaue", "Schwäne", "Füchse", "Hirsche",
    "Paladine", "Zauberer", "Magier", "Schurken", "Samurai", "Ninjas",
    "Bogenschützen", "Waldläufer", "Kleriker", "Priester", "Schamanen", "Druiden",
    "Sphinxen", "Pegasus", "Pumas", "Jaguare", "Stiere", "Schlangen"
};

        static readonly List<string> places = new List<string> {
    "Wald", "Ödland", "Ruinen", "Eiche", "Berg", "See", "Fluss", "Meer",
    "Burg", "Turm", "Verlies", "Höhle", "Palast", "Tempel", "Schrein",
    "Garten", "Dorf", "Stadt", "Königreich", "Imperium", "Wüste", "Gletscher",
    "Vulkan", "Tal", "Klippe", "Festung", "Hafen", "Insel", "Halbinsel",
    "Ebene", "Sumpf", "Riff", "Savanne", "Tundra", "Unterwelt", "Wirbel",
    "Quelle", "Hain", "Wiese", "Fjord", "Canyon", "Plateau", "Marschland",
    "Sumpf", "Lichtung", "Mond", "Stern", "Galaxie", "Nebel", "Asteroid",
    "Komet", "Meteor", "Schwarzes Loch", "Nichts", "Nexus", "Dimension", "Zuflucht",
    "Arena", "Kolosseum", "Akademie", "Bibliothek", "Archiv"
};

        static readonly List<string> titles = new List<string> {
    "Legion", "Brigade", "Kohorte", "Bataillon", "Regiment", "Division", "Kompanie",
    "Schwadron", "Zug", "Truppe", "Abteilung", "Kontingent", "Phalanx", "Trupp",
    "Team", "Einheit", "Streitmacht", "Schar", "Horde", "Armee", "Marine", "Flotte",
    "Flügel", "Gruppe", "Meute", "Kreis", "Rat", "Versammlung", "Gilde", "Orden",
    "Bruderschaft", "Clan", "Stamm", "Sippe", "Dynastie", "Reich", "Gefolgschaft",
    "Fürstentum", "Herzogtum", "Baronie", "Kapitel", "Bündnis", "Verschwörung",
    "Koalition", "Allianz", "Konföderation", "Föderation", "Liga", "Gesellschaft",
    "Akademie", "Institut", "Männer", "Volk", "Macht"
};

        static readonly List<string> symbols = new List<string> {
    "Lilie", "Turm", "Speer", "Schild", "Krone", "Schwert", "Burg", "Stern",
    "Mond", "Sonne", "Komet", "Flamme", "Welle", "Berg", "Baum", "Wald",
    "Fluss", "Stein", "Amboss", "Hammer", "Axt", "Bogen", "Pfeil", "Köcher",
    "Helm", "Handschuh", "Rüstung", "Kette", "Schlüssel", "Ring", "Schloss", "Buch", "Schriftrolle",
    "Trank", "Kugel", "Thron", "Banner", "Pyramide", "Obelisk",
    "Brücke", "Tor", "Mauer", "Kelch", "Laterne", "Kerze", "Glocke",
    "Feder", "Glas", "Kompass"
};

        /// <summary>
        /// Rückgabe statischer Listen ist wichtig für die Leistung.
        /// </summary>
        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;

        static readonly List<string> generalSyllables = new List<string>
{
    "al", "ber", "dor", "fels", "wald", "hain", "stein", "fluss", "burg", "tal",
    "hor", "moor", "bruck", "furt", "see", "loh", "mark", "fels", "ran", "sen", "dorf",
    "thurm", "wald", "brun", "fenn", "raben", "schwarz", "kron", "zorn", "eck", "bern",
    "bach", "glan", "hardt", "kast", "lind", "nock", "ober", "roth", "schön", "thal"
};

        static readonly List<string> generalTownSuffixes = new List<string>
{ "stadt", "dorf", "furt", "burg", "tal", "hain", "feld", "ried", "bach", "brück", "hof" };

        static readonly List<string> northSyllables = new List<string>
{
    "fjord", "skarn", "bjorn", "thor", "grim", "ulf", "val", "odin", "nord", "stor",
    "hrafn", "sne", "jarn", "drak", "brim", "sig", "hvit", "lyng", "magn", "sif", "vid",
    "ulfr", "aesk", "brok", "dahl", "keld", "thjorn", "run", "stein", "ask", "varg"
};

        static readonly List<string> northTownSuffixes = new List<string>
{ "heim", "stad", "fjord", "berg", "dal", "gard", "vik", "nes", "havn", "lund" };

        static readonly List<string> westSyllables = new List<string>
{
    "win", "lan", "ham", "ford", "shire", "ton", "wick", "bard", "cliff", "holt",
    "moor", "ox", "perry", "quen", "rift", "sward", "tre", "ver", "war", "beam",
    "cove", "dale", "gale", "heath", "keel", "marsh", "pale", "quill", "thatch"
};

        static readonly List<string> westTownSuffixes = new List<string>
{ "ton", "burg", "ford", "ham", "shire", "caster", "wick", "bury", "stead", "ville" };

        static readonly List<string> eastSyllables = new List<string>
{
    "jin", "shi", "yuan", "qing", "lu", "chun", "ming", "nan", "ping", "zhou",
    "bai", "dong", "fu", "guo", "hui", "kai", "lan", "mei", "ni", "ou",
    "pei", "qin", "ran", "su", "tai", "wei", "xi", "yang", "zhu", "an"
};

        static readonly List<string> eastTownSuffixes = new List<string>
{ "yang", "shan", "ji", "an", "hai", "cheng", "lin", "tai", "kou", "fu" };

        static readonly List<string> southSyllables = new List<string>
{
    "the", "ne", "ly", "ca", "re", "si", "mar", "pol", "ath", "cor",
    "del", "eph", "ga", "hel", "io", "kos", "la", "me", "ni", "ol"
};

        static readonly List<string> southTownSuffixes = new List<string>
{ "polis", "ium", "os", "us", "a", "on", "ora", "aca", "es", "ae" };

        /// <summary>
        /// Returning static lists is important for performance
        /// </summary>
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

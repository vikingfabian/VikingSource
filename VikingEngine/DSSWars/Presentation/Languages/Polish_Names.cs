using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Polish
    {
        //The name generator create unique names for the armies by combining random words
        //Names does not require one-to-one translations, the number of names in the lists is not important, the game will adapt is there is more or less options

        /// <summary>
        /// Sposób łączenia dwóch losowych słów. W polskim najlepiej sprawdza się bezpośrednie połączenie (np. Legion Smoka)
        /// </summary>
        public override string NameGenerator_AOfTheB => "{0} {1}";

        static readonly List<string> adjectives = new List<string> {
            "Waleczny", "Mistyczny", "Mroczny", "Złoty", "Starożytny", "Mroźny", "Wieczny",
            "Cienisty", "Świetlisty", "Karmazynowy", "Zaciekły", "Chwalebny", "Szlachetny", "Dziki",
            "Mściwy", "Odważny", "Burzowy", "Majestatyczny", "Bezlitosny", "Przebiegły", "Promienny",
            "Zmierzchu", "Świtu", "Zmroku", "Żelazny", "Srebrny", "Widmowy", "Niebiański", "Piekielny",
            "Zaklęty", "Tajemny", "Ukryty", "Zagubiony", "Zapomniany", "Legendarny", "Mityczny",
            "Cichy", "Grzmiący", "Płonący", "Strzaskany", "Wędrowny", "Eteryczny", "Fantomowy",
            "Szmaragdowy", "Rubinowy", "Szafirowy", "Diamentowy", "Jadeitowy", "Silny"
        };

        static readonly List<string> colors = new List<string> {
            "Czerwony", "Czarny", "Biały", "Szmaragdowy", "Lazurowy", "Szkarłatny", "Fioletowy", "Indygo",
            "Złoty", "Srebrny", "Brązowy", "Miedziany", "Szafirowy", "Rubinowy", "Ametystowy",
            "Jadeitowy", "Błękitny", "Karmazynowy", "Magenta", "Hebanowy", "Kościany", "Morski", "Turkusowy",
            "Kasztanowy", "Oliwkowy", "Brzoskwiniowy", "Szary", "Węglowy", "Lawendowy", "Limonkowy", "Granatowy",
            "Ugier", "Śliwkowy", "Kwarcowy", "Łososiowy", "Piaskowy", "Ultramaryna", "Cynobrowy", "Wisteriowy",
            "Xanadu", "Żółty", "Zaffre", "Lazurowy", "Niebieski", "Zielony", "Miodowy",
            "Irysowy", "Jaśminowy", "Khaki"
        };

        static readonly List<string> creatures = new List<string> {
            "Smoków", "Wilków", "Orłów", "Lwów", "Rycerzy", "Gryfów", "Centaurów",
            "Elfów", "Krasnoludów", "Gigantów", "Aniołów", "Syren", "Jednorożców",
            "Feniksów", "Jeleni", "Koni", "Sokołów", "Tygrysów", "Niedźwiedzi", "Panter",
            "Orłów", "Jastrzębi", "Delfinów", "Wielorybów", "Słoni", "Leopardów", "Gepardów",
            "Kruków", "Sów", "Pawi", "Łabędzi", "Lisów", "Jeleni",
            "Paladynów", "Czarodziejów", "Magów", "Łotrów", "Samurajów", "Ninjów",
            "Łuczników", "Strażników", "Kleryków", "Kapłanów", "Szamanów", "Druidów",
            "Sfinksów", "Pegazów", "Pum", "Jaguarów", "Byków", "Węży"
        };

        static readonly List<string> places = new List<string> {
            "Lasu", "Pustkowia", "Ruin", "Dębu", "Góry", "Jeziora", "Rzeki", "Morza",
            "Zamku", "Wieży", "Lochu", "Jaskini", "Pałacu", "Świątyni", "Sanktuarium",
            "Ogrodu", "Wioski", "Miasta", "Królestwa", "Imperium", "Pustyni", "Lodowca",
            "Wulkanu", "Doliny", "Klifu", "Twierdzy", "Portu", "Wyspy", "Półwyspu",
            "Równiny", "Grzęzawiska", "Rafy", "Sawanny", "Tundry", "Zaświatów", "Wiru",
            "Źródła", "Gaju", "Polany", "Fiordu", "Kanionu", "Płaskowyżu", "Bagna",
            "Moczaru", "Leśnej Polany", "Księżyca", "Gwiazd", "Galaktyki", "Mgławicy", "Asteroidy",
            "Komety", "Meteoru", "Czarnej Dziury", "Otchłani", "Nexusu", "Wymiaru", "Sanktuarium",
            "Areny", "Koloseum", "Akademii", "Biblioteki", "Archiwum"
        };

        static readonly List<string> titles = new List<string> {
            "Legion", "Brygada", "Kohorta", "Batalion", "Regiment", "Dywizja", "Kompania",
            "Eskadra", "Pluton", "Oddział", "Kontyngent", "Falanga", "Drużyna", "Gwardia",
            "Zespół", "Jednostka", "Siły", "Zastęp", "Horda", "Armia", "Marynarka", "Flota", "Flotylla",
            "Skrzydło", "Grupa", "Wataha", "Krąg", "Rada", "Zgromadzenie", "Gildia", "Zakon",
            "Bractwo", "Klan", "Plemię", "Ród", "Dynastia", "Imperium", "Ludzie Króla",
            "Księstwo", "Księstwo", "Baronia", "Kapituła", "Przymierze", "Syndykat",
            "Koalicja", "Sojusz", "Konfederacja", "Federacja", "Liga", "Stowarzyszenie",
            "Akademia", "Instytut", "Ludzie", "Ludy", "Potęga"
        };

        static readonly List<string> symbols = new List<string> {
            "Lilii", "Wieży", "Włóczni", "Tarczy", "Korony", "Miecza", "Zamku", "Gwiazdy",
            "Księżyca", "Słońca", "Komety", "Płomienia", "Fali", "Góry", "Drzewa", "Lasu",
            "Rzeki", "Kamienia", "Kowadła", "Młota", "Topora", "Łuku", "Strzały", "Kołczanu",
            "Hełmu", "Rękawicy", "Pancerza", "Łańcucha", "Klucza", "Pierścienia", "Zamka", "Księgi", "Zwoju",
            "Mikstury", "Kostura", "Tronu", "Sztandaru", "Sygnetu", "Klejnotu", "Piramidy", "Obelisku",
            "Wieży", "Mostu", "Bramy", "Muru", "Kielicha", "Latarni", "Świecy", "Dzwonu",
            "Pióra", "Klepsydry", "Kompasu"
        };

        /// <summary>
        /// Returning static lists is important for performance
        /// </summary>
        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;


        /*
        * Generator nazw tworzy unikalne nazwy miast poprzez łączenie losowych sylab.
        * Sylaby są podzielone na ogólne, północne (klimat nordycki), zachodnie (staroangielski/germański),
        * wschodnie (azjatycki) i południowe (śródziemnomorski/antyczny).
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
        { "gród", "ice", "ów", "sko", "no", "wola", "in", "sk", "ycze", "owo" };

        static readonly List<string> northSyllables = new List<string>
        {
            "fjor", "skol", "varg", "ulv", "frost", "bjorn", "stor", "hvit", "jarn", "sne",
            "kvist", "lund", "nord", "olf", "pil", "rune", "sig", "thor", "ulf", "vald",
            "yng", "aeg", "brim", "drak", "eir", "frej", "gim", "halv", "ivar", "jo",
            "keld", "lyng", "magn", "natt", "odin", "pryd", "quor", "rost", "sif", "tjorn",
            "ulfr", "vid", "wind", "xil", "yrl", "zorn", "aesk", "brok", "dahl", "eng"
        };
        static readonly List<string> northTownSuffixes = new List<string>
        { "gard", "heim", "vik", "stad", "nes", "berg", "land", "fjord", "havn", "borg" };

        static readonly List<string> westSyllables = new List<string>
        {
            "win", "lan", "ham", "ford", "ster", "burg", "shire", "well", "ton", "wick",
            "bard", "clif", "dell", "es", "graf", "holt", "ire", "jest", "kent", "ly",
            "moor", "nor", "ox", "perry", "quen", "rift", "sward", "tre", "ulm", "ver",
            "war", "yate", "zeal", "ard", "beam", "cove", "dale", "eft", "gale", "heath",
            "ingle", "keel", "leith", "marsh", "neath", "ope", "pale", "quill", "rove", "scale", "thatch"
        };
        static readonly List<string> westTownSuffixes = new List<string>
        { "burg", "wald", "ford", "styn", "mont", "bury", "ville", "hoff", "furt", "berg" };

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

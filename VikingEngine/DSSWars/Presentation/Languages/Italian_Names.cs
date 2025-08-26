using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Italian
    {
        //The name generator create unique names for the armies by combining random words
        //Names does not require one-to-one translations, the number of names in the lists is not important, the game will adapt is there is more or less options

        /// <summary>
        /// A way to merge to random words
        /// </summary>
        public override string NameGenerator_AOfTheB => "{0} di {1}";

        static readonly List<string> adjectives = new List<string> {
            "Valorosi", "Mistica", "Oscuri", "Dorati", "Antichi", "Ghiacciati", "Eterni",
            "Ombrosi", "Luminosi", "Cremisi", "Fieri", "Gloriosi", "Nobili", "Selvaggi",
            "Vendicativi", "Coraggiosi", "Tempestosi", "Maestosi", "Spietati", "Astuti", "Radianti",
            "Crepuscolari", "Dell’alba", "Del tramonto", "Ferrei", "Argento", "Spettrali", "Celesti", "Infernali",
            "Incantati", "Arcani", "Nascosti", "Perduti", "Dimenticati", "Leggendari", "Mitici",
            "Silenziosi", "Tonanti", "Fiammeggianti", "Infranti", "Erranti", "Eterei", "Fantasma",
            "Smeraldo", "Rubino", "Zaffiro", "Diamante", "Giada", "Forti"
        };

        static readonly List<string> colors = new List<string> {
            "Rosso", "Nero", "Bianco", "Smeraldo", "Azzurro", "Scarlatto", "Viola", "Indaco",
            "Oro", "Argento", "Bronzo", "Rame", "Zaffiro", "Rubino", "Ametista",
            "Giada", "Ceruleo", "Cremisi", "Magenta", "Ebano", "Avorio", "Verde acqua", "Turchese",
            "Granata", "Oliva", "Pesca", "Grigio", "Antracite", "Lavanda", "Lime", "Marina",
            "Ocra", "Prugna", "Quarzo", "Salmone", "Beige", "Oltremare", "Vermiglione", "Glicine",
            "Xanadu", "Giallo", "Zaffiro", "Azzurro", "Blu", "Verde", "Miele",
            "Iris", "Gelsomino", "Cachi"
        };

        static readonly List<string> creatures = new List<string> {
            "Draghi", "Lupi", "Aquile", "Leoni", "Cavalieri", "Grifoni", "Centaurs",
            "Elfi", "Nani", "Giganti", "Angeli", "Sirene", "Unicorni",
            "Fenici", "Cervi", "Cavalli", "Falchi", "Tigri", "Orsi", "Pantere",
            "Aquile", "Poiane", "Delfini", "Balene", "Elefanti", "Leopardi", "Ghepardi",
            "Corvi", "Gufi", "Pavoni", "Cigni", "Volpi", "Cervi",
            "Paladini", "Stregoni", "Maghi", "Furfanti", "Samurai", "Ninja",
            "Arcieri", "Esploratori", "Chierici", "Preti", "Sciamani", "Druidi",
            "Sfingi", "Pegasi", "Puma", "Giaguari", "Tori", "Serpenti"
        };

        static readonly List<string> places = new List<string> {
            "Foresta", "Deserto", "Rovina", "Quercia", "Montagna", "Lago", "Fiume", "Mare",
            "Castello", "Torre", "Segrete", "Caverna", "Palazzo", "Tempio", "Santuario",
            "Giardino", "Villaggio", "Città", "Regno", "Impero", "Deserto", "Ghiacciaio",
            "Vulcano", "Valle", "Rupe", "Fortezza", "Porto", "Isola", "Penisola",
            "Pianura", "Pantano", "Barriera", "Savana", "Tundra", "Sotterranei", "Vortice",
            "Sorgente", "Boschetto", "Prato", "Fiordo", "Canyon", "Altopiano", "Palude",
            "Palude", "Radura", "Luna", "Stella", "Galassia", "Nebulosa", "Asteroide",
            "Cometa", "Meteora", "Buco Nero", "Vuoto", "Nesso", "Dimensione", "Santuario",
            "Arena", "Colosseo", "Accademia", "Biblioteca", "Archivio"
        };

        static readonly List<string> titles = new List<string> {
            "Legione", "Brigata", "Coorte", "Battaglione", "Reggimento", "Divisione", "Compagnia",
            "Squadrone", "Plotone", "Truppa", "Distaccamento", "Contingente", "Falange", "Squadra",
            "Team", "Unità", "Forza", "Schiera", "Orda", "Esercito", "Marina", "Flotta", "Flottiglia",
            "Ala", "Gruppo", "Branco", "Cerchia", "Consiglio", "Assemblea", "Gilda", "Ordine",
            "Compagnia", "Clan", "Tribù", "Casata", "Dinastia", "Impero", "Uomini del Re",
            "Principato", "Ducato", "Baronia", "Capitolo", "Patto", "Sindacato",
            "Coalizione", "Alleanza", "Confederazione", "Federazione", "Lega", "Società",
            "Accademia", "Istituto", "Uomini", "Popolo", "Potere"
        };

        static readonly List<string> symbols = new List<string> {
            "Giglio", "Torre", "Lancia", "Scudo", "Corona", "Spada", "Castello", "Stella",
            "Luna", "Sole", "Cometa", "Fiamma", "Onda", "Montagna", "Albero", "Foresta",
            "Fiume", "Pietra", "Incudine", "Martello", "Ascia", "Arco", "Freccia", "Faretra",
            "Elmo", "Guanto", "Armatura", "Catena", "Chiave", "Anello", "Serratura", "Libro", "Pergamena",
            "Pozione", "Globo", "Trono", "Stendardo", "Anello", "Gemma", "Piramide", "Obelisco",
            "Torre", "Ponte", "Porta", "Muro", "Calice", "Lanterna", "Candela", "Campana",
            "Piuma", "Vetro", "Bussola"
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
        * The name generator create unique names for cities by combining random syllables
        * The syllables are divided into general, north (nordic flavor), west (old english), east (asian) and south (mediterrian)
        * When localizing to a languge close to English, there is no need for a translation
        * 
        * The number of names in the lists is not important, the game will adapt is there is more or less options
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
            "guerra", "yate", "zeal", "ard", "beam", "cove", "dale", "eft", "gale", "heath",
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
            "", "ne", "ly", "ca", "re", "si", "mar", "pol", "ath", "cor",
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

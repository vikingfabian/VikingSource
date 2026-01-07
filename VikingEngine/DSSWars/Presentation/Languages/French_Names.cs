using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class French
    {
        //The name generator create unique names for the armies by combining random words
        //Names does not require one-to-one translations, the number of names in the lists is not important, the game will adapt is there is more or less options

        /// <summary>
        /// A way to merge to random words
        /// </summary>
        public override string NameGenerator_AOfTheB => "{0} sur {1}";

        static readonly List<string> adjectives = new List<string> {
            "Vaillant", "Mystique", "Sombre", "Dorée", "Ancien", "Gelé", "Eternel",
            "Obscur", "Brillant", "Pourpre", "Féroce", "Glorieux", "Noble", "Sauvage",
            "Vengeur", "Brave", "Majestique", "Futé", "Radiant",
            "Argenté", "Spectral", "Celeste", "Infernal",
            "Enchanté", "Arcanique", "Caché", "Perdu", "Oublié", "Légendaire", "Mythique",
            "Silencieux", "Brûlant", "Brisé", "Éthéré", "Fantôme",
            "Puissant"
        };

        static readonly List<string> colors = new List<string> {
            "Rouge", "Noir", "Blanc", "Émeraude", "Azur", "Écarlate", "Violet", "Indigo",
            "Or", "Argent", "Bronze", "Cuivre", "Saphir", "Rubis", "Améthyste",
            "Jade", "Céruléen", "Cramoisi", "Magenta", "Ébène", "Ivoire", "Sarcelle", "Turquoise",
            "Marron", "Olive", "Pêche", "Gris", "Charbon", "Lavande", "Citron vert", "Marine",
            "Ocre", "Prune", "Quartz", "Saumon", "Beige", "Outremer", "Vermillon", "Glycine",
            "Xanadu", "Jaune", "Zaffre", "Azur", "Bleu", "Vert", "Miellat",
            "Iris", "Jasmin", "Kaki"
        };

        static readonly List<string> creatures = new List<string> {
            "Dragons", "Loups", "Aigles", "Lions", "Chevaliers", "Griffons", "Centaure",
            "Elfes", "Nains", "Géants", "Anges", "Sirènes", "Licornes",
            "Phénix", "Cerfs", "Chevaux", "Faucons", "Tigres", "Ours", "Panthères",
            "Aigles", "Faucons", "Dauphins", "Baleines", "Éléphants", "Léopards", "Guépards",
            "Corbeaux", "Hiboux", "Paons", "Cygnes", "Renards", "Daims",
            "Paladins", "Sorcier", "Mages", "Voleurs", "Samouraïs", "Ninjas",
            "Archers", "Rôdeurs", "Clercs", "Prêtres", "Chamans", "Druides",
            "Sphinx", "Pégase", "Couguars", "Jaguars", "Taureaux", "Serpents"
        };

        static readonly List<string> places = new List<string> {
            "Forêt", "Désert", "Ruine", "Chêne", "Montagne", "Lac", "Rivière", "Mer",
            "Château", "Tour", "Donjon", "Caverne", "Palais", "Temple", "Sanctuaire",
            "Jardin", "Village", "Ville", "Royaume", "Empire", "Désert", "Glacier",
            "Volcan", "Vallée", "Falaise", "Forteresse", "Port", "Île", "Péninsule",
            "Plaine", "Marécage", "Récif", "Savane", "Toundra", "Enfers", "Vortex",
            "Source", "Bosquet", "Prairie", "Fjord", "Canyon", "Plateau", "Marais",
            "Marécage", "Clairière", "Lune", "Étoile", "Galaxie", "Nébuleuse", "Astéroïde",
            "Comète", "Météore", "Trou noir", "Vide", "Nexus", "Dimension", "Sanctuaire",
            "Arène", "Colisée", "Académie", "Bibliothèque", "Archives"
        };

        static readonly List<string> titles = new List<string> {
            "Légion", "Brigade", "Cohorte", "Bataillon", "Régiment", "Division", "Compagnie",
            "Escadron", "Peloton", "Troupe", "Détachement", "Contingent", "Phalange", "Escouade",
            "Équipe", "Unité", "Force", "Hôte", "Horde", "Armée", "Marine", "Flotte", "Flottille",
            "Aile", "Groupe", "Meute", "Cercle", "Conseil", "Assemblée", "Guilde", "Ordre",
            "Communauté", "Clan", "Tribu", "Parenté", "Dynastie", "Empire", "Homme du roi",
            "Principauté", "Duché", "Baronnie", "Chapitre", "Pacte", "Syndicat",
            "Coalition", "Alliance", "Confédération", "Fédération", "Ligue", "Société",
            "Académie", "Institut", "Hommes", "Peuple", "Puissance"
        };

        static readonly List<string> symbols = new List<string> {
            "Lys", "Tour", "Lance", "Bouclier", "Couronne", "Épée", "Château", "Étoile",
            "Lune", "Soleil", "Comète", "Flamme", "Vague", "Montagne", "Arbre", "Forêt",
            "Rivière", "Pierre", "Enclume", "Marteau", "Hache", "Arc", "Flèche", "Carquois",
            "Casque", "Gantelet", "Armure", "Chaîne", "Clé", "Anneau", "Serrure", "Livre", "Parchemin",
            "Potion", "Orbe", "Trône", "Bannière", "Anneau", "Gemme", "Pyramide", "Obélisque",
            "Tour", "Pont", "Porte", "Mur", "Calice", "Lanterne", "Bougie", "Cloche",
            "Plume", "Verre", "Boussole"
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
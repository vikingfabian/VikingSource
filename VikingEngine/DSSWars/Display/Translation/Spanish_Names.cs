using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class Spanish : AbsLanguage
    {
        /// <summary>
        /// Una forma de combinar dos palabras aleatorias
        /// </summary>
        public override string NameGenerator_AOfTheB => "{0} de los {1}";

        static readonly List<string> adjectives = new List<string> {
    "Valiente", "Místico", "Oscuro", "Dorado", "Antiguo", "Congelado", "Eterno",
    "Sombrío", "Brillante", "Carmesí", "Feroz", "Glorioso", "Noble", "Salvaje",
    "Vengativo", "Bravo", "Tormentoso", "Majestuoso", "Despiadado", "Astuto", "Radiante",
    "Crepuscular", "Amanecer", "Anochecer", "De Hierro", "Plateado", "Espectral", "Celestial", "Infernal",
    "Encantado", "Arcano", "Oculto", "Perdido", "Olvidado", "Legendario", "Mítico",
    "Silencioso", "Atronador", "Ardiente", "Destrozado", "Errante", "Etéreo", "Fantasma",
    "Esmeralda", "Rubí", "Zafiro", "Diamante", "Jade", "Fuerte"
};

        static readonly List<string> colors = new List<string> {
    "Rojo", "Negro", "Blanco", "Esmeralda", "Azur", "Escarlata", "Violeta", "Índigo",
    "Dorado", "Plateado", "Bronce", "Cobre", "Zafiro", "Rubí", "Amatista",
    "Jade", "Cerúleo", "Carmesí", "Magenta", "Ébano", "Marfil", "Verde azulado", "Turquesa",
    "Granate", "Oliva", "Melocotón", "Gris", "Carbón", "Lavanda", "Lima", "Azul marino",
    "Ocre", "Ciruela", "Cuarzo", "Salmón", "Bronceado", "Ultramarino", "Bermellón", "Glicina",
    "Xanadú", "Amarillo", "Zafiro", "Azur", "Azul", "Verde", "Miel",
    "Lirio", "Jazmín", "Caqui"
};

        static readonly List<string> creatures = new List<string> {
    "Dragones", "Lobos", "Águilas", "Leones", "Caballeros", "Grifos", "Centauros",
    "Elfos", "Enanos", "Gigantes", "Ángeles", "Sirenas", "Unicornios",
    "Fénix", "Ciervos", "Caballos", "Halcones", "Tigres", "Osos", "Panteras",
    "Águilas", "Halcones", "Delfines", "Ballenas", "Elefantes", "Leopardos", "Guepardos",
    "Cuervos", "Búhos", "Pavos reales", "Cisnes", "Zorros", "Ciervos",
    "Paladines", "Hechiceros", "Magos", "Pícaros", "Samuráis", "Ninjas",
    "Arqueros", "Exploradores", "Clérigos", "Sacerdotes", "Chamanes", "Druidas",
    "Esfinges", "Pegaso", "Pumas", "Jaguares", "Toros", "Serpientes"
};

        static readonly List<string> places = new List<string> {
    "Bosque", "Desperdicio", "Ruina", "Roble", "Montaña", "Lago", "Río", "Mar",
    "Castillo", "Torre", "Mazmorra", "Caverna", "Palacio", "Templo", "Santuario",
    "Jardín", "Aldea", "Ciudad", "Reino", "Imperio", "Desierto", "Glaciar",
    "Volcán", "Valle", "Acantilado", "Fortaleza", "Puerto", "Isla", "Península",
    "Llanura", "Pantano", "Arrecife", "Sabana", "Tundra", "Inframundo", "Vórtice",
    "Manantial", "Arboleda", "Pradera", "Fiordo", "Cañón", "Meseta", "Ciénaga",
    "Pantano", "Claro del Bosque", "Luna", "Estrella", "Galaxia", "Nebulosa", "Asteroide",
    "Cometa", "Meteorito", "Agujero Negro", "Vacío", "Nexo", "Dimensión", "Santuario",
    "Arena", "Coliseo", "Academia", "Biblioteca", "Archivo"
};

        static readonly List<string> titles = new List<string> {
    "Legión", "Brigada", "Cohorte", "Batallón", "Regimiento", "División", "Compañía",
    "Escuadrón", "Pelotón", "Tropa", "Destacamento", "Contingente", "Falange", "Escuadra",
    "Equipo", "Unidad", "Fuerza", "Anfitrión", "Horda", "Ejército", "Marina", "Flota", "Flotilla",
    "Ala", "Grupo", "Manada", "Círculo", "Consejo", "Asamblea", "Gremio", "Orden",
    "Compañerismo", "Clan", "Tribu", "Parentela", "Dinastía", "Imperio", "Hombres del Rey",
    "Principado", "Ducado", "Baronía", "Capítulo", "Pacto", "Sindicato",
    "Coalición", "Alianza", "Confederación", "Federación", "Liga", "Sociedad",
    "Academia", "Instituto", "Hombres", "Pueblo", "Poder"
};

        static readonly List<string> symbols = new List<string> {
    "Lirio", "Torre", "Lanza", "Escudo", "Corona", "Espada", "Castillo", "Estrella",
    "Luna", "Sol", "Cometa", "Llama", "Ola", "Montaña", "Árbol", "Bosque",
    "Río", "Piedra", "Yunque", "Martillo", "Hacha", "Arco", "Flecha", "Carcaj",
    "Casco", "Guantelete", "Armadura", "Cadena", "Llave", "Anillo", "Cerradura", "Libro", "Pergamino",
    "Poción", "Orbe", "Trono", "Estandarte", "Anillo", "Gema", "Pirámide", "Obelisco",
    "Torre", "Puente", "Puerta", "Muro", "Cáliz", "Linterna", "Vela", "Campana",
    "Pluma", "Vidrio", "Brújula"
};

        /// <summary>
        /// Devolver listas estáticas es importante para el rendimiento
        /// </summary>
        public override List<string> NameGenerator_Army_Adjectives => adjectives;
        public override List<string> NameGenerator_Army_Colors => colors;
        public override List<string> NameGenerator_Army_Creatures => creatures;
        public override List<string> NameGenerator_Army_Places => places;
        public override List<string> NameGenerator_Army_Titles => titles;
        public override List<string> NameGenerator_Army_Symbols => symbols;

        /*
* El generador de nombres crea nombres únicos para las ciudades combinando sílabas aleatorias
* Las sílabas se dividen en general, norte (sabor nórdico), oeste (antiguo inglés), este (asiático) y sur (mediterráneo)
* Cuando se localiza a un idioma cercano al inglés, no es necesario traducir
* 
* El número de nombres en las listas no es importante, el juego se adaptará si hay más o menos opciones
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

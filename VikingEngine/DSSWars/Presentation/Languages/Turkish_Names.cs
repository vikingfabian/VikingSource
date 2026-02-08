using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class Turkish
    {
        //The name generator create unique names for the armies by combining random words
        //Names does not require one-to-one translations, the number of names in the lists is not important, the game will adapt is there is more or less options

        /// <summary>
        /// A way to merge to random words
        /// </summary>
        public override string NameGenerator_AOfTheB => "{1} {0}";

        static readonly List<string> adjectives = new List<string> {
            "Yiğit", "Mistik", "Karanlık", "Altın", "Antik", "Donmuş", "Ebedi",
            "Gölge", "Parlak", "Kızıl", "Azılı", "Şanlı", "Soylu", "Vahşi",
            "Kindar", "Cesur", "Kavgacı", "Haşmetli", "Gaddar", "Kurnaz", "Radyant",
            "Alacakaranlık", "Şafak", "Seher", "Demir", "Gümüş", "Spektral", "Göksel", "Şeytani",
            "Büyülü", "Esrarlı", "Saklı", "Kayıp", "Unutulmuş", "Efsanevi", "Mitik",
            "Sessiz", "Gümbürtülü", "Yanan", "Derbeder", "Gezgin", "Uhrevi", "Hayalet",
            "Zümrüt", "Yakut", "Safir", "Elmas", "Yeşim", "Güçlü"
        };

        static readonly List<string> colors = new List<string> {
            "Kırmızı", "Siyah", "Beyaz", "Zümrüt", "Azur", "Kızıl", "Menekşe", "Indigo",
            "Sapsarı", "Gümüş", "Bronz", "Bakır", "Safir", "Yakut", "Ametist",
            "Yeşim", "Gök Mavisi", "Kan Kırmızı", "Fuşya", "Abanoz", "Fildişi", "Cem Göbeği", "Turkuaz",
            "Kestane", "Zeytin", "Şeftali", "Gri", "Kömür", "Lavanta", "Kireç", "Lacivert",
            "Okre", "Erik", "Kuvars", "Turuncu", "Koyu", "Koyu Mavi", "Vermilyon", "Morsalkım",
            "Kuşkonmaz", "Sarı", "Kobalt", "Azur", "Mavi", "Yeşil", "Yeşilimsi",
            "Süsen", "Yasemin", "Haki"
        };

        static readonly List<string> creatures = new List<string> {
            "Ejderhalar", "Kurtlar", "Kartallar", "Aslanlar", "Şövalyeler", "Griffinler", "Sentorlar",
            "Elfler", "Cüceler", "Devler", "Melekler", "Denizkızları", "Tek Boynuzlular",
            "Anka Kuşları", "Alageyikler", "Atlar", "Şahinler", "Kaplanlar", "Ayılar", "Panterler",
            "Kartallar", "Doğan", "Yunuslar", "Balinalar", "Filler", "Leoparlar", "Çitalar",
            "Kuzgunlar", "Baykuşlar", "Tavuskuşları", "Kuğu", "Tilkiler", "Geyikler",
            "Paladinler", "Büyücüler", "Sihirbazlar", "Serseriler", "Samuraylar", "Ninjalar",
            "Okçular", "Korucular", "Papazlar", "Rahipler", "Şamanlar", "Druidler",
            "Sfenksler", "Kanatlı Atlar", "Pumalar", "Jaguarlar", "Boğalar", "Yılanlar"
        };

        static readonly List<string> places = new List<string> {
            "Orman", "Çorak Topraklar", "Harabe", "Meşe", "Dağ", "Göl", "Nehir", "Deniz",
            "Kale", "Kule", "Zindan", "Mağara", "Şato", "Tapınak", "Türbe",
            "Bahçe", "Köy", "Şehir", "Krallık", "İmparatorluk", "Çöl", "Buzul",
            "Volkan", "Vadi", "Uçurum", "Hisar", "Liman", "Adacık", "Yarımada",
            "Ova", "Bataklık", "Resif", "Savana", "Tundra", "Yeraltı Dünyası", "Girdap",
            "Pınar", "Kavaklık", "Çayır", "Haliç", "Kanyon", "Plato", "Sazlık",
            "Batak", "Koru", "Ay", "Yıldız", "Galaksi", "Nebula", "Asteroit",
            "Kuyruklu Yıldız", "Meteor", "Kara Delik", "Boşluk", "Nexus", "Boyut", "Mabed",
            "Arena", "Stadyum", "Akademi", "Kütüphane", "Arşiv"
        };

        static readonly List<string> titles = new List<string> {
            "Lejyonu", "Tugayı", "Kohortu", "Taburu", "Alayı", "Tümeni", "Bölüğü",
            "Filikası", "Takımı", "Birliği", "Müfrezesi", "Grubu", "Falanksı", "Mangası",
            "Timi", "Birimi", "Kuvveti", "Sefer Kuvveti", "Akıncıları", "Ordusu", "Donanması", "Filosu", "Küçük Filosu",
            "Kanadı", "Grubu", "Sancağı", "Topluluğu", "Konseyi", "Meclisi", "Loncası", "Tarikatı",
            "Kardeşliği", "Klanı", "Kabilesi", "Hısımlığı", "Hanedanlığı", "İmparatorluğu", "Kralın Muhafızları",
            "Prensliği", "Dükalığı", "Baronluğu", "Kolu", "Paktı", "Teşkilatı",
            "Koalisyonu", "İttifakı", "Konfederasyonu", "Federasyonu", "Ligi", "Cemiyeti",
            "Akademisi", "Enstitüsü", "Adamları", "Halkı", "İktidarı"
        };

        static readonly List<string> symbols = new List<string> {
            "Zambak", "Kule", "Mızrak", "Kalkan", "Taç", "Kılıç", "Kale", "Yıldız",
            "Ay", "Güneş", "Kuyrkluyldız", "Alev", "Dalga", "Dağ", "Ağaç", "Orman",
            "Nehir", "Taş", "Örs", "Çekiç", "Balta", "Yay", "Ok", "Sadak",
            "Miğfer", "Eldiven", "Zırh", "Zincir", "Anahtar", "Yüzük", "Kilit", "Kitap", "Parşömen",
            "İksir", "Küre", "Taht", "Sancak", "Yüzsük", "Mücevher", "Piramit", "Dikilitaş",
            "Kule", "Köprü", "Geçit", "Duvar", "Kadeh", "Fener", "Mum", "Çan",
            "Tüy", "Pencere", "Pusula"
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

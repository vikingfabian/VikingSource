using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    partial class AbsLanguage
    {

        public abstract string NameGenerator_XOfTheY { get; }

        public abstract List<string> NameGenerator_Army_Adjectives { get; }
        public abstract List<string> NameGenerator_Army_Colors { get; }
        public abstract List<string> NameGenerator_Army_Creatures { get; }
        public abstract List<string> NameGenerator_Army_Places { get; }
        public abstract List<string> NameGenerator_Army_Titles { get; }
        public abstract List<string> NameGenerator_Army_Symbols { get; }


        public abstract List<string> NameGenerator_City_GeneralSyllables { get; }
        public abstract List<string> NameGenerator_City_GeneralTownSuffixes { get; }
        public abstract List<string> NameGenerator_City_NorthSyllables { get; }
        public abstract List<string> NameGenerator_City_NorthTownSuffixes { get; }
        public abstract List<string> NameGenerator_City_WestSyllables { get; }
        public abstract List<string> NameGenerator_City_WestTownSuffixes { get; }
        public abstract List<string> NameGenerator_City_EastSyllables { get; }
        public abstract List<string> NameGenerator_City_EastTownSuffixes { get; }
        public abstract List<string> NameGenerator_City_SouthSyllables { get; }
        public abstract List<string> NameGenerator_City_SouthTownSuffixes { get; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace VikingEngine.LF2
{
    static class LanguageManager
    {
        //public static LanguageType CurrentLanguage = LanguageType.English;
        public static Data.Languages.AbsLanguage Wrapper;// = new Data.Languages.English();
        public static void Init()
        {
            string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            culture = culture.Remove(2, culture.Length -2);
            switch (culture)
            {
                default:
                    Wrapper = new Data.Languages.English();
                    break;
                //case "de":
                //    Wrapper = new Data.Languages.German();
                //    break;
                //fr = french
                //ja = japanese
            }
            
        }
    }
    enum LanguageType
    {
        English, German
    }
}

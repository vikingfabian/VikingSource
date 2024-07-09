using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.Engine;

namespace VikingEngine.DSSWars.Display.Translation
{
    class Translation
    {
        public List<LanguageType> available()
        { 
            return new List<LanguageType> { 
                LanguageType.English, 
                LanguageType.Chinese, 
                LanguageType.Russian,
                LanguageType.Spanish,
            };
        }

        public SpriteName sprite(LanguageType language)
        {
            switch (language)
            {
                case LanguageType.English:
                    return SpriteName.LangButton_English;

                case LanguageType.Chinese:
                    return SpriteName.LangButton_Chinese;

                case LanguageType.Russian:
                    return SpriteName.LangButton_Russian;

                case LanguageType.Spanish:
                    return SpriteName.LangButton_Spanish;

                default:
                    throw new NotImplementedException();
            }
        }

        public void setupLanguage(bool onContentLoad)
        {
            bool onChange = !onContentLoad;

            //Steam does not work for some reason

            //if (Ref.gamesett.language == LanguageType.NONE)
            //{
            //    Ref.gamesett.language = LanguageType.English;

            //    if (Ref.steam.isInitialized)
            //    {
            //        //https://partner.steamgames.com/doc/store/localization/languages
            //        string lang = SteamAPI.SteamApps().GetCurrentGameLanguage();
            //        switch (lang)
            //        {
            //            case "zh-TW":
            //            case "zh-CN":
            //                onChange = true;
            //                Ref.gamesett.language = LanguageType.Chinese;
            //                break;

            //        }
            //    }
            //}

            switch (Ref.gamesett.language)
            {
                default:
                    DssRef.lang = new English();
                    Ref.langOpt = new HUD.OptionsLanguage_English();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Chinese:
                    DssRef.lang = new SimplifiedChinese();
                    Ref.langOpt = new HUD.OptionsLanguage_SimplifiedChinese();
                    LoadContent.setFontLanguage(FontLanguage.Chinese);

                    if (onChange)
                    {
                        Ref.gamesett.UiScale = Math.Max(Ref.gamesett.UiScale, 1.2f);
                    }
                    break;

                case LanguageType.Russian:
                    DssRef.lang = new Russian();
                    Ref.langOpt = new HUD.OptionsLanguage_Russian();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Spanish:
                    DssRef.lang = new Spanish();
                    Ref.langOpt = new HUD.OptionsLanguage_Spanish();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;
            }

            //if (onChange && onContentLoad)
            //{ 
            //    Ref.
            //}
        }
    }

   
}

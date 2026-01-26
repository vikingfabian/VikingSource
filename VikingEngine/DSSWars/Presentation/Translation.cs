using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Translation.OptionLanguages;

namespace VikingEngine.DSSWars.Presentation
{
    class Translation
    {
        public List<LanguageType> available()
        { 
            return new List<LanguageType> { 
                LanguageType.English,
                LanguageType.German,
                LanguageType.French,
                LanguageType.Spanish,
                LanguageType.Portuguese,
                LanguageType.Italian,
                LanguageType.Polish,
                LanguageType.Turkish,
                LanguageType.Russian,
                LanguageType.Chinese,
                LanguageType.Thai,
                LanguageType.Korean,
                LanguageType.Japanese,              
                
            };
        }

        public SpriteName sprite(LanguageType language)
        {
            switch (language)
            {
                default:
#if DEBUG
                    throw new NotImplementedException();
#endif

                case LanguageType.English:
                    return SpriteName.LangButton_English;

                case LanguageType.German:
                    return SpriteName.LangButton_German;

                case LanguageType.Polish:
                    return SpriteName.LangButton_Polish;

                case LanguageType.Chinese:
                    return SpriteName.LangButton_Chinese;

                case LanguageType.Japanese:
                    return SpriteName.LangButton_Japanese;

                case LanguageType.Thai:
                    return SpriteName.LangButton_Thai;

                case LanguageType.Korean:
                    return SpriteName.LangButton_Korean;

                case LanguageType.Russian:
                    return SpriteName.LangButton_Russian;

                case LanguageType.Spanish:
                    return SpriteName.LangButton_Spanish;

                case LanguageType.Portuguese:
                    return SpriteName.LangButton_Portuguese;

                case LanguageType.French:
                    return SpriteName.LangButton_Frensh;

                case LanguageType.Italian:
                    return SpriteName.LangButton_Italian;

                case LanguageType.Turkish:
                    return SpriteName.LangButton_Turkish;

                //default:
                //    throw new NotImplementedException();
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
                    Ref.langOpt = new OptionsLanguage_English();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Chinese:
                    DssRef.lang = new SimplifiedChinese();
                    Ref.langOpt = new OptionsLanguage_SimplifiedChinese();
                    LoadContent.setFontLanguage(FontLanguage.Chinese);

                    if (onChange)
                    {
                        Ref.gamesett.UiScale = Math.Max(Ref.gamesett.UiScale, 1.2f);
                    }
                    break;

                case LanguageType.Japanese:
                    DssRef.lang = new Japanese();
                    Ref.langOpt = new OptionsLanguage_Japanese();
                    LoadContent.setFontLanguage(FontLanguage.Japanese);

                    if (onChange)
                    {
                        Ref.gamesett.UiScale = Math.Max(Ref.gamesett.UiScale, 1.2f);
                    }
                    break;

                case LanguageType.Korean:
                    DssRef.lang = new Korean();
                    Ref.langOpt = new OptionsLanguage_Korean();
                    LoadContent.setFontLanguage(FontLanguage.Korean);

                    if (onChange)
                    {
                        Ref.gamesett.UiScale = Math.Max(Ref.gamesett.UiScale, 1.2f);
                    }
                    break;

                case LanguageType.Thai:
                    DssRef.lang = new Thai();
                    Ref.langOpt = new OptionsLanguage_Thai();
                    LoadContent.setFontLanguage(FontLanguage.Thai);

                    if (onChange)
                    {
                        Ref.gamesett.UiScale = Math.Max(Ref.gamesett.UiScale, 1.1f);
                    }
                    break;

                case LanguageType.German:
                    DssRef.lang = new German();
                    Ref.langOpt = new OptionsLanguage_German();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Polish:
                    DssRef.lang = new Polish();
                    Ref.langOpt = new OptionsLanguage_Polish();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Russian:
                    DssRef.lang = new Russian();
                    Ref.langOpt = new OptionsLanguage_Russian();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Spanish:
                    DssRef.lang = new Spanish();
                    Ref.langOpt = new OptionsLanguage_Spanish();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Turkish:
                    DssRef.lang = new Turkish();
                    Ref.langOpt = new OptionsLanguage_Turkish();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.French:
                    DssRef.lang = new French();
                    Ref.langOpt = new OptionsLanguage_French();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Portuguese:
                    DssRef.lang = new Portuguese();
                    Ref.langOpt = new OptionsLanguage_Portuguese();
                    LoadContent.setFontLanguage(FontLanguage.Western);
                    break;

                case LanguageType.Italian:
                    DssRef.lang = new Italian();
                    Ref.langOpt = new OptionsLanguage_Italian();
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

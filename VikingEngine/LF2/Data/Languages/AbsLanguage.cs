using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data.Languages
{
    abstract class AbsLanguage
    {
        protected string FeedbackTag = "GamefarmContact";


        abstract public string ThisIsTrial();
        

        abstract public string GameMenuOpenMap();
        abstract public string GameMenuAppearance();
        abstract public string GameMenuSettings();
        abstract public string GameMenuSoundLevel();
        abstract public string GameMenuMusicLevel();
        abstract public string GameMenuSoundHigh();
        abstract public string GameMenuSoundLow();
        abstract public string GameMenuSoundOff();


        abstract public string DeathTitle();
        abstract public string DeathDesc();
        abstract public string DeathContinue();
        abstract public string DeathContinueYes();
        abstract public string DeathContinueNo();
        
        
        //abstract public string LostControllerFriend();
        
        abstract public string AppearanceHatTitle();
        abstract public string AppearanceBeardTitle();
        abstract public string AppearanceSkinTitle();
        abstract public string AppearanceClothColorTitle();
        abstract public string AppearanceHairColorTitle();
        abstract public string AppearanceHairNon();
        abstract public string AppearanceHatNon();
        abstract public string AppearanceHatVendel();
        abstract public string AppearanceHatHorned();
        abstract public string AppearanceHatKnight();
        abstract public string AppearanceFacialNon();
        abstract public string AppearanceFacialSmallBeard();
        abstract public string AppearanceFacialLargeBeard();
        abstract public string AppearanceFacialPlumbersMustache();
        abstract public string AppearanceFacialBikersMustache();

        
        #region MENU
        abstract public string Resume();
        abstract public string MenuStartGame();
        abstract public string MenuStartTrial();
        abstract public string MenuExitGame();

        #region PROMT_MESSAGES
        abstract public string LoadingTerrain();
        abstract public string LostControllerTitle();
        abstract public string LostControllerFriend();
        abstract public string LostControllerIgnoreFriend();
        #endregion

        #region ERROR_MESSAGES
        abstract public string ErrNotSignedInTitle();
        abstract public string ErrNotSignedInDesc();
        abstract public string ErrNoLiveTitle();
        abstract public string ErrNoLiveDesc();
         //Message(LootFest.LanguageManager.Wrapper.ErrNotSignedInTitle(), LootFest.LanguageManager.Wrapper.ErrNotSignedInDesc(), pix);
         //   }
         //   else if (!p.Live)
         //   {
         //       Message(LootFest.LanguageManager.Wrapper.ErrNoLiveTitle(), LootFest.LanguageManager.Wrapper.ErrNoLiveDesc(), pix);
        #endregion


        #endregion
        #region ITEMS
        abstract public string ItemTypeShield();
        abstract public string WeaponTypeSword();
        abstract public string WeaponTypeBow();

        #endregion
        #region LIVE_DESIGN
        

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data.Languages
{
    class English : AbsLanguage
    {
        //override public string XXX() { return ""; }
        //override public string XXX() { return "xx"; }

        #region MENU
        override public string Resume() { return "Resume"; }
        override public string MenuStartGame() { return "xx"; }
        override public string MenuStartTrial() { return "xx"; }
        override public string MenuExitGame() { return "xx"; }
        #endregion

        #region PROMT_MESSAGES
        override public string LoadingTerrain() { return "Loading terrain"; }
        override public string LostControllerTitle() { return "Controller is disconnected"; }
        override public string LostControllerFriend() { return "Waiting for your friend to reconnect his controller."; }
        override public string LostControllerIgnoreFriend() { return "Ignore"; }
        #endregion

        #region ERROR_MESSAGES
        override public string ErrNotSignedInTitle() { return "Not signed in"; }
        override public string ErrNotSignedInDesc() { return "You are required to sign in to a profile"; }
        override public string ErrNoLiveTitle() { return "Disconnected"; }
        override public string ErrNoLiveDesc() { return "Xbox Live connection is required"; }
        #endregion

        #region ITEMS
        override public string ItemTypeShield() { return "Shield"; }
        override public string WeaponTypeSword() { return "Sword"; }
        override public string WeaponTypeBow() { return "Bow"; }
        #endregion
        
        #region LIVE_DESIGN

        #endregion

        
        override public string ThisIsTrial() { return "You are in Trial mode"; }
        override public string DeathTitle() { return "You were knocked out!"; }
        override public string DeathDesc() { return "You will be moved to your spawn point"; }

        override public string DeathContinue() { return "Continue?"; }
        override public string DeathContinueYes() { return "Yes"; }
        override public string DeathContinueNo() { return "Exit game"; }
        //override public string LostControllerTitle() { return "Controller is disconnected"; }
        //override public string LostControllerFriend() { return "Waiting for your friend to reconnect his controller."; }
        
        override public string AppearanceHatTitle() { return "Hat"; }
        override public string AppearanceBeardTitle() { return "Facial hair"; }
        override public string AppearanceSkinTitle() { return "Skin color"; }
        override public string AppearanceClothColorTitle() { return "Tunic color"; }
        override public string AppearanceHairColorTitle() { return "Hair color"; }
        public override string AppearanceHairNon()
        {
            return "No hair";
        }
        override public string AppearanceHatNon() { return "No Hat"; }
        override public string AppearanceHatVendel() { return "Vendel helmet"; }
        override public string AppearanceHatHorned() { return "Horned helmet"; }
        override public string AppearanceHatKnight() { return "Knight helmet"; }
        override public string AppearanceFacialNon() { return "No Hair"; }
        override public string AppearanceFacialSmallBeard() { return "Small beard"; }
        override public string AppearanceFacialLargeBeard() { return "Large beard"; }
        override public string AppearanceFacialPlumbersMustache() { return "Plumber mustache"; }
        override public string AppearanceFacialBikersMustache() { return "Biker mustache"; }

        override public string GameMenuOpenMap() { return "Open map"; }
        override public string GameMenuAppearance() { return "Appearance"; }
        override public string GameMenuSettings() { return "Settings"; }
        //override public string GameMenuSoundSettings() { return "Sound"; }
        override public string GameMenuSoundLevel() { return "Sound"; }
        override public string GameMenuMusicLevel() { return "Music"; }
        override public string GameMenuSoundHigh() { return "High"; }
        override public string GameMenuSoundLow() { return "Low"; }
        override public string GameMenuSoundOff() { return "Off"; }

           }
}

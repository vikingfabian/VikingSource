using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.HUD;

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        
        //void openPage(int i, HUD.Link data)
        //{
        //    openPage((MenuPageName)data.Value1);
        //}
        //void openPage(int type)
        //{
        //    this.openPage((MenuPageName)type);
        //}
        void openPage(MenuPageName type)
        {
            //if (PlatformSettings.PC_platform)
            //    Engine.Input.CenterMouse = false;

            if (type != MenuPageName.TalkingToNPC && type != MenuPageName.ChestDialogue)
            {
                lastInteraction = hero.InteractingWith;
                hero.InteractingWith = null;
            }
            if (hero.Alive || type == MenuPageName.GameOver)
            {
                currentMenu = type;
                //if (voxelDesigner != null)
                //{
                //    EndCreationMode();
                //}
                if (mode == PlayerMode.Map)
                { openMiniMap(false); }

                mode = PlayerMode.InMenu;

                beginOpenMenu();

                if (type != MenuPageName.EMPTY)
                {
                    mFile = new File();
                    switch (type)
                    {

                        //case MenuPageName.TalkingToNPC:
                            

                        //    break;

                        //case MenuPageName.Manual:
                           
                        //    break;
                        //case MenuPageName.Backpack:
                           
                        //    break;

                        //#region TRAVEL
                        //case MenuPageName.Travel:
                           
                        //    break;
                        //#endregion
                        //#region MESSAGES
                        //case MenuPageName.Messages:
                            
                        //    break;
                        //#endregion
                        //#region SETTINGS
                        //case MenuPageName.Settings:
                            
                        //    //if (Map.World.RunningAsHost)
                        //    //    mFile.AddTextLink("Restore chunk", "Changes are stored in a backup file for about 48hours", (int)Link.BeginRestoreChunkOptions);
                        //    break;
                        //#endregion
                        //#region NETWORK_SETTINGS

                        //case MenuPageName.NetworkSettings:
                            

                        //    break;
                        //#endregion
                        //#region GAME_OVER
                        //case MenuPageName.GameOver:
                           
                            

                        //    break;
                        //#endregion
                        #region MAIN_MENU
                        case MenuPageName.MainMenu:

                            mFile = mainMenu();



                            break;
                        #endregion


                        case MenuPageName.LostController:
                            mFile.AddTitle(LanguageManager.Wrapper.LostControllerTitle());
                            mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.CloseMenu);
                            break;
                        //case MenuPageName.FriendLostController:
                        //    mFile.AddTitle(LanguageManager.Wrapper.LostControllerFriend());
                        //    mFile.AddIconTextLink(SpriteName.LFIconGoBack, LanguageManager.Wrapper.LostControllerIgnoreFriend(), (int)Link.CloseMenu);
                        //    break;
                        #region APPEARANCE
                        case MenuPageName.ChangeApperance:
                            pageAppearance();
                            break;
                        #endregion
                        #region DEBUG
                        case MenuPageName.Debug:
                            pageDebug();
                            return;
                        #endregion

                    }

                    menu.File = mFile;
                }
                //change camera

                //Vector3 target = hero.Position;
                //target.Y -= 4;
                //ControllerLink.view.Camera.GoalTarget(target);
            }

        }
    }
}

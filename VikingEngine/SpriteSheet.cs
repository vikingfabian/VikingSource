using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest;

namespace VikingEngine
{
    class SpriteSheet : Engine.AbsSpriteSheetLayout
    {
        public const int BlockTextureWidth = 768;
        public const int CommanderBoardEdgeHeight = 24;

        public static readonly IntVector2 CmdValueBoxTileSz = new IntVector2(2, 1);
        public static readonly IntVector2 CmdStatsValueBgSz = new IntVector2(40, 32);

        public static readonly IntVector2 WarsTitleSize = new IntVector2(9, 4);

        public static readonly IntVector2 CmdStrategyCardSz = new IntVector2(4, 2);
        public static readonly IntVector2 CmdStrategyCardPixSz = new IntVector2(40, 24);

        public static readonly IntVector2 HqTutorialSz = new IntVector2(15, 6);

        public static readonly IntVector2 DoomBannerSize = new IntVector2(44, 96);
        public static readonly IntVector2 DamageSlashEffectSz = new IntVector2(128, 16);

        public static readonly IntVector2 OnlinePlayerCountIconSz = new IntVector2(96, 34);

        public static readonly IntVector2 PjTitleTextSz = new IntVector2(15, 5);
        public static readonly IntVector2 SpaceWarLorryPixSz = new IntVector2(46, 109);
        public static readonly IntVector2 CballGoalTextSz = new IntVector2(2, 5);

        public static readonly IntVector2 CballGoliePaddleSz = new IntVector2(10, 46);

        public static readonly IntVector2 DSSCardSz = new IntVector2(6, 8);

        public static readonly IntVector2 PjModeSz = new IntVector2(112, 64);

        public static readonly IntVector2 DSS2Logo = new IntVector2(12, 7);

        public const int GolfGrassTexWidth = 120;
        public const int CmdLetterSz = 18;
        const int NineSplitSz = 30;

        public SpriteSheet()
        {
            this.Settings(4096, 128);
            this.TileSheetIx = LoadedTexture.SpriteSheet;

            {
                add(SpriteName.KeyA);
                add(SpriteName.KeyB);
                add(SpriteName.KeyC);
                add(SpriteName.KeyD);
                add(SpriteName.KeyE);
                add(SpriteName.KeyF);
                add(SpriteName.KeyG);
                add(SpriteName.KeyH);
                add(SpriteName.KeyI);
                add(SpriteName.KeyJ);
                add(SpriteName.KeyK);
                add(SpriteName.KeyL);
                add(SpriteName.KeyM);
                add(SpriteName.KeyN);
                add(SpriteName.KeyO);
                add(SpriteName.KeyP);
                add(SpriteName.KeyQ);
                add(SpriteName.KeyR);
                add(SpriteName.KeyS);
                add(SpriteName.KeyT);
                add(SpriteName.KeyU);
                add(SpriteName.KeyV);
                add(SpriteName.KeyX);
                add(SpriteName.KeyY);
                add(SpriteName.KeyZ);
                add(SpriteName.Key1);
                add(SpriteName.Key2);
                add(SpriteName.Key3);
                add(SpriteName.Key4);
                add(SpriteName.Key5);
                add(SpriteName.Key6);
                add(SpriteName.Key7);
                add(SpriteName.Key8);
                add(SpriteName.Key9);
                add(SpriteName.Key0);
                add(SpriteName.KeyCtrl);
                add(SpriteName.KeyAlt);
                add(SpriteName.KeyEsc);
                add(SpriteName.KeyF1);
                add(SpriteName.KeyF2);
                add(SpriteName.KeyF3);
                add(SpriteName.KeyF4);
                add(SpriteName.KeyF5);
                add(SpriteName.KeyF6);
                add(SpriteName.KeyF7);
                add(SpriteName.KeyF8);
                add(SpriteName.KeyF9);
                add(SpriteName.KeyF10);
                add(SpriteName.KeyF11);
                add(SpriteName.KeyF12);
                add(SpriteName.KeyShift);
                add(SpriteName.KeyArrowUp);
                add(SpriteName.KeyArrowDown);
                add(SpriteName.KeyArrowRight);
                add(SpriteName.KeyArrowLeft);
                add(SpriteName.KeyEnter);
                add(SpriteName.KeyTab);
                add(SpriteName.KeyBack);
                add(SpriteName.KeySpace);
                add(SpriteName.LfMenuMoreMenusArrow);
                add(SpriteName.LfCheckYes);
                add(SpriteName.LfCheckNo);
                add(SpriteName.IconSandGlass);
                add(SpriteName.IconApperanceHat);
                add(SpriteName.IconApperanceShirt);
                add(SpriteName.IconApperanceBeard);
                add(SpriteName.IconApperanceHair);
                add(SpriteName.IconApperanceEyes);
                add(SpriteName.IconApperanceMouth);
                add(SpriteName.MissingImage);
                add(SpriteName.IconBuildArrow);
                add(SpriteName.IconBuildSelection);
                add(SpriteName.IconBuildAdd);
                add(SpriteName.IconBuildRemove);
                add(SpriteName.IconBuildMoveSelection);
                add(SpriteName.IconColorPick);
                add(SpriteName.InterfaceIconCamera);
                add(SpriteName.InterfaceTextInput);
                //add(SpriteName);

                add(SpriteName.KeyW);
                add(SpriteName.KeyPlus);
                add(SpriteName.KeyStar);
                add(SpriteName.KeyQuestionmark);
                add(SpriteName.KeySlash);
                add(SpriteName.KeyBackslash);
                add(SpriteName.KeyPeriod);
                add(SpriteName.KeyComma);
                add(SpriteName.KeyMinus);
                add(SpriteName.KeyTilde);
                add(SpriteName.KeyOpenBracket);
                add(SpriteName.KeyCloseBracket);
                add(SpriteName.KeyUnknown);
                add(SpriteName.KeyCapsLock);
                add(SpriteName.KeyPgUp);
                add(SpriteName.KeyPgDown);
                add(SpriteName.KeyEnd);
                add(SpriteName.KeyHome);
                add(SpriteName.KeyInsert);
                add(SpriteName.KeyPipe);
                add(SpriteName.KeyPrintScreen);

                add(SpriteName.KeyDelete);
                add(SpriteName.KeyQuote);
                add(SpriteName.KeySemiColon);
                currentIndex += 3;
                add(SpriteName.VoxelEditorIcon);
                add(SpriteName.EditorToolRoad);
                add(SpriteName.EditorToolCube);
                add(SpriteName.EditorToolPyramid);
                add(SpriteName.EditorToolCone);
                add(SpriteName.EditorToolWedge);
                add(SpriteName.EditorToolCylinder);
                add(SpriteName.EditorToolSphere);
                add(SpriteName.EditorToolPencil);
                add(SpriteName.EditorToolReColorPencil);

                add(SpriteName.MenuPixelIconMusicVol);
                add(SpriteName.MenuPixelIconSoundVol);
                add(SpriteName.AutomationGearIcon);
                add(SpriteName.MenuPixelIconManual);
                add(SpriteName.MenuPixelIconPlay);
                add(SpriteName.PixController1);
                add(SpriteName.PixController2);
                add(SpriteName.PixController3);
                add(SpriteName.PixController4);
                add(SpriteName.PixController5);
                add(SpriteName.PixController6);
            }

            currentIndex = numTilesWidth * 1;
            {
                add(SpriteName.LFAppearHairCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearEyes, currentIndex, 2, 2);
                add(SpriteName.LFAppearMouth, currentIndex, 2, 2);
                add(SpriteName.LFAppearBeardCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearBeard, currentIndex, 2, 2);
                add(SpriteName.LFAppearSkinCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearClothCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearPantsCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearShoeCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearBeltCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearBeltDetailCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearHatMainCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearHatDetailCol, currentIndex, 2, 2);
                add(SpriteName.LFAppearHat, currentIndex, 2, 2);
                add(SpriteName.LfHandgranade, currentIndex, 2, 2);
                add(SpriteName.LfCardItemIcon, currentIndex, 2, 2);
                add(SpriteName.LFHudShield, currentIndex, 2, 2);
                add(SpriteName.LFHudHeart, currentIndex, 2, 2);
                add(SpriteName.LFHudHeartEmpty, currentIndex, 2, 2);
                add(SpriteName.LFHudCoins, currentIndex, 2, 2);
                add(SpriteName.LFHudAmmo, currentIndex, 2, 2);
                add(SpriteName.LFHudAmmoEmpty, currentIndex, 2, 2);
                add(SpriteName.LFHudKey, currentIndex, 2, 2);
                add(SpriteName.LFHudKeyEmpty, currentIndex, 2, 2);
                add(SpriteName.LFSaveIcon, currentIndex, 2, 2);

                add(SpriteName.LFPrimaryAttackHudBG, currentIndex, 2, 2);
                add(SpriteName.LFSecondaryAttackHudBG, currentIndex, 2, 2);
                add(SpriteName.LFItemHudBG, currentIndex, 2, 2);
                add(SpriteName.LFAppleItemIcon, currentIndex, 2, 2);
                add(SpriteName.LFBasicSuitIcon1, currentIndex, 2, 2);
                add(SpriteName.LFBasicSuitIcon2, currentIndex, 2, 2);
                add(SpriteName.LFSwordsmanIcon1, currentIndex, 2, 2);
                add(SpriteName.LFNum0, currentIndex, 2, 2);
                add(SpriteName.LFNum1, currentIndex, 2, 2);
                add(SpriteName.LFNum2, currentIndex, 2, 2);
                add(SpriteName.LFNum3, currentIndex, 2, 2);
                add(SpriteName.LFNum4, currentIndex, 2, 2);
                add(SpriteName.LFNum5, currentIndex, 2, 2);
                add(SpriteName.LFNum6, currentIndex, 2, 2);
                add(SpriteName.LFNum7, currentIndex, 2, 2);
                add(SpriteName.LFNum8, currentIndex, 2, 2);
                add(SpriteName.LFNum9, currentIndex, 2, 2);
                add(SpriteName.LFSwordsmanIcon2, currentIndex, 2, 2);
                add(SpriteName.LFDaneIcon1, currentIndex, 2, 2);
                add(SpriteName.LFDaneIcon2, currentIndex, 2, 2);
                add(SpriteName.LFDualIcon1, currentIndex, 2, 2);
                add(SpriteName.LFDualIcon2, currentIndex, 2, 2);
                add(SpriteName.LFSpearmanIcon1, currentIndex, 2, 2);
                add(SpriteName.LFSpearmanIcon2, currentIndex, 2, 2);
                add(SpriteName.LFArcherIcon2, currentIndex, 2, 2);
                add(SpriteName.LFLevelCompleteIcon, currentIndex, 2, 2);
                add(SpriteName.LFLevelUnCompleteIcon, currentIndex, 2, 2);

                add(SpriteName.LfFinalLevelUnCompleteIcon, currentIndex, 2, 2);
                add(SpriteName.LfFinalLevelCompleteIcon, currentIndex, 2, 2);
                add(SpriteName.LfChallengeLevelUnCompleteIcon, currentIndex, 2, 2);
                add(SpriteName.LfChallengeLevelCompleteIcon, currentIndex, 2, 2);
                add(SpriteName.LFApplePieItemIcon, currentIndex, 2, 2);
                add(SpriteName.LFHudAmmoGray, currentIndex, 2, 2);
                add(SpriteName.LfWolfShieldHead, currentIndex, 2, 2);
                add(SpriteName.LfLifeIcon, currentIndex, 2, 2);
                add(SpriteName.LfShapeshifterIcon2, currentIndex, 2, 2);
                add(SpriteName.LfShapeshifterIcon1, currentIndex, 2, 2);
                add(SpriteName.LfGunMashine, currentIndex, 2, 2);
                add(SpriteName.LfGunSideArm, currentIndex, 2, 2);
            }


            currentIndex = numTilesWidth * 3;
            {
                add(SpriteName.cgDeck, currentIndex, 2, 2);
                add(SpriteName.cgHand, currentIndex, 2, 2);
                add(SpriteName.cgAttack, currentIndex, 2, 2);
                add(SpriteName.LFNumMinus, currentIndex, 2, 2);
                add(SpriteName.LFNumPlus, currentIndex, 2, 2);
                add(SpriteName.LfMountIcon, currentIndex, 2, 2);
                add(SpriteName.LFNumStar, currentIndex, 2, 2);
                add(SpriteName.LfMountHorseIcon, currentIndex, 2, 2);
                add(SpriteName.LFHudMountHeart, currentIndex, 2, 2);
                add(SpriteName.LfClosedLock, currentIndex, 2, 2);
                add(SpriteName.EditorForwardArrow, currentIndex, 2, 2);
                add(SpriteName.LfBarrelIcon, currentIndex, 2, 2);
                add(SpriteName.LfChatBobbleIcon, currentIndex, 2, 2);
                currentIndex += 2;
                add(SpriteName.cgManaGlow, currentIndex, 2, 2);

                add(SpriteName.GenericButton0, currentIndex, 2, 2);
                add(SpriteName.GenericButton1, currentIndex, 2, 2);
                add(SpriteName.GenericButton2, currentIndex, 2, 2);
                add(SpriteName.GenericButton3, currentIndex, 2, 2);
                add(SpriteName.GenericButton4, currentIndex, 2, 2);
                add(SpriteName.GenericButton5, currentIndex, 2, 2);
                add(SpriteName.GenericButton6, currentIndex, 2, 2);
                add(SpriteName.GenericButton7, currentIndex, 2, 2);
                add(SpriteName.GenericButton8, currentIndex, 2, 2);
                add(SpriteName.GenericButton9, currentIndex, 2, 2);
                add(SpriteName.GenericButton10, currentIndex, 2, 2);
                add(SpriteName.GenericButton11, currentIndex, 2, 2);
                add(SpriteName.GenericButton12, currentIndex, 2, 2);
                add(SpriteName.GenericButton13, currentIndex, 2, 2);
                add(SpriteName.GenericButton14, currentIndex, 2, 2);
                add(SpriteName.GenericButtonAny, currentIndex, 2, 2);

                add(SpriteName.PsButtonTriangle, currentIndex, 2, 2);
                add(SpriteName.PsButtonCirkle, currentIndex, 2, 2);
                add(SpriteName.PsButtonSquare, currentIndex, 2, 2);
                add(SpriteName.PsButtonCross, currentIndex, 2, 2);
                add(SpriteName.PsTouchPad, currentIndex, 2, 2);
                add(SpriteName.PsButtonShare, currentIndex, 2, 2);
                add(SpriteName.PsButtonOptions, currentIndex, 2, 2);
                add(SpriteName.PsGuideButton, currentIndex, 2, 2);

                add(SpriteName.LfPickAxe, currentIndex, 2, 2);
                add(SpriteName.LfNpcSpeechArrow, currentIndex, 2, 2);
                add(SpriteName.LfEmoIcon2, currentIndex, 2, 2);
                add(SpriteName.LfOpenLock, currentIndex, 2, 2);
                add(SpriteName.LfCraftItemIcon, currentIndex, 2, 2);
                add(SpriteName.LFLevelIcon, currentIndex, 2, 2);
                add(SpriteName.LfFinalLevelIcon, currentIndex, 2, 2);
                add(SpriteName.LfChallengeLevelIcon, currentIndex, 2, 2);

                add(SpriteName.LFLobbyIcon, currentIndex, 2, 2);
                add(SpriteName.LFTutorialLevelIcon, currentIndex, 2, 2);

                add(SpriteName.LFExpressLoot, currentIndex, 2, 2);
                add(SpriteName.LFExpressSad, currentIndex, 2, 2);
                add(SpriteName.LFExpressAngry, currentIndex, 2, 2);
                add(SpriteName.LFExpressTease, currentIndex, 2, 2);
                add(SpriteName.LFExpressThumbsUp, currentIndex, 2, 2);
                add(SpriteName.LFExpressHi, currentIndex, 2, 2);
                add(SpriteName.LFExpressLaugh, currentIndex, 2, 2);
                add(SpriteName.LFExpressDuck, currentIndex, 2, 2);

                currentIndex += 4;
                add(SpriteName.ButtonVIEW, currentIndex, 2, 2);
                add(SpriteName.ButtonMENU, currentIndex, 2, 2);
                add(SpriteName.MouseButtonDoubbleLeft, currentIndex, 2, 2);
                add(SpriteName.ControllerVibration, currentIndex, 2, 2);

            }

            currentIndex = numTilesWidth * 5;
            add(SpriteName.LFNewSaveFileIcon, currentIndex, 3, 3);
            add(SpriteName.cgBaseMinionGreen, currentIndex, 3, 3);
            add(SpriteName.cgBaseMinionYellow, currentIndex, 3, 3);
            add(SpriteName.cgBaseMinionOrange, currentIndex, 3, 3);
            add(SpriteName.cgBaseMinionPurple, currentIndex, 3, 3);
            add(SpriteName.cgBaseMinionCyan, currentIndex, 3, 3);
            add(SpriteName.cgBaseMinionRed, currentIndex, 3, 3);
            add(SpriteName.cgAttackMinionGreen, currentIndex, 3, 3);
            add(SpriteName.cgAttackMinionYellow, currentIndex, 3, 3);
            add(SpriteName.cgAttackMinionBrown, currentIndex, 3, 3);
            add(SpriteName.cgAttackMinionPurple, currentIndex, 3, 3);

            add(SpriteName.cgAttackMinionBlue, currentIndex, 3, 3);
            add(SpriteName.cgAttackMinionRedBrown, currentIndex, 3, 3);
            add(SpriteName.cgDamage, currentIndex, 3, 3);
            add(SpriteName.cgNoMovement, currentIndex, 2, 3);
            add(SpriteName.cgMove1, currentIndex, 2, 3);
            add(SpriteName.cgMove2, currentIndex, 2, 3);
            add(SpriteName.cgMove3, currentIndex, 2, 3);
            add(SpriteName.cgTargetAllOpponents, currentIndex, 3, 3);
            add(SpriteName.cgTargetRow, currentIndex, 3, 3);
            add(SpriteName.cgTargetCol, currentIndex, 3, 3);
            add(SpriteName.cgTargetAll, currentIndex, 3, 3);
            add(SpriteName.cgCastleIcon, currentIndex, 3, 3);
            add(SpriteName.Keyboard, currentIndex, 4, 3);
            add(SpriteName.cgDiagonalForward, currentIndex, 3, 3);
            add(SpriteName.cgProjectileAll4Dir, currentIndex, 3, 3);
            add(SpriteName.cgProjectile, currentIndex, 3, 3);
            add(SpriteName.cgPlaceInFrontOf, currentIndex, 3, 3);
            add(SpriteName.cgCrosshairIcon, currentIndex, 3, 3);
            add(SpriteName.LfMithrilIngot, currentIndex, 3, 3);
            add(SpriteName.LfEmoIcon1, currentIndex, 3, 3);
            add(SpriteName.LfNpcCraftArrow, currentIndex, 3, 3);
            add(SpriteName.LfSadTrollFace, currentIndex, 3, 3);
            add(SpriteName.LfClientCompassArrow, currentIndex, 3, 3);
            add(SpriteName.LfBabyIcon, currentIndex, 3, 3);
            add(SpriteName.LfCheckpointFlag, currentIndex, 3, 3);
            add(SpriteName.LfTransportIcon, currentIndex, 3, 3);
            add(SpriteName.LfBabyProgressIcon, currentIndex, 3, 3);
            add(SpriteName.cgSpiderBot, currentIndex, 3, 3);
            add(SpriteName.cgTrollBoss, currentIndex, 3, 3);

            currentIndex = numTilesWidth * 8;
            add(SpriteName.cgCardFrontMinion, currentIndex, 5, 7);
            add(SpriteName.cgCardFrontScroll, currentIndex, 5, 7);
            add(SpriteName.cgCardBack, currentIndex, 5, 7);
            add(SpriteName.LfWeaponSmithHead1, currentIndex, 4, 4);
            add(SpriteName.LfWeaponSmithHead2, currentIndex, 4, 4);
            add(SpriteName.LfGranpaHead1, currentIndex, 4, 4);
            add(SpriteName.LfGranpaHead2, currentIndex, 4, 4);


            add(SpriteName.LfTutPrimaryAttack, currentIndex, 4, 4);
            add(SpriteName.LfTutBrokenTarget, currentIndex, 4, 4);
            add(SpriteName.LfTutSpecialAttack, currentIndex, 6, 4);
            add(SpriteName.LfTutJump, currentIndex, 4, 4);

            add(SpriteName.LfVeteranHead1, currentIndex, 4, 4);
            add(SpriteName.LfVeteranHead2, currentIndex, 4, 4);
            add(SpriteName.LfFatherHead1, currentIndex, 4, 4);
            add(SpriteName.LfFatherHead2, currentIndex, 4, 4);

            currentIndex = numTilesWidth * 12 + 15;
            add(SpriteName.Undo, 2, 2);
            add(SpriteName.IconBuildStamp, 2, 2);
            add(SpriteName.EditorMouseToolsIcon, 2, 2);
            add(SpriteName.EditorRemoveCross, 2, 2);
            add(SpriteName.width, 2, 2);
            add(SpriteName.height, 2, 2);
            add(SpriteName.length, 2, 2);
            add(SpriteName.EditorEmptyBlockFrame, 2, 2);
            add(SpriteName.EditorPencilShadow, 2, 2);
            add(SpriteName.EditorPencilShadow1, 2, 2);
            add(SpriteName.EditorPencilShadow2, 2, 2);
            add(SpriteName.EditorPencilShadow3, 2, 2);
            add(SpriteName.EditorPencilShadow4, 2, 2);
            add(SpriteName.EditorPencilShadow5, 2, 2);
            add(SpriteName.EditorPencilShadow6, 2, 2);

            IntVector2 CreditsSz = new IntVector2(198, 54);
            addWithSizeDef(SpriteName.CreditsProgrammerArt, currentIndex, CreditsSz.X, CreditsSz.Y);
            addWithSizeDef(SpriteName.CreditsMusic, currentIndex, CreditsSz.X, CreditsSz.Y);
            addWithSizeDef(SpriteName.CreditsPlaytest, currentIndex, CreditsSz.X, CreditsSz.Y);
            addWithSizeDef(SpriteName.CreditsTranslation, currentIndex, CreditsSz.X, CreditsSz.Y);

            currentIndex = numTilesWidth * 15;
            add(SpriteName.ButtonA, currentIndex, 2, 2);
            add(SpriteName.ButtonB, currentIndex, 2, 2);
            add(SpriteName.ButtonX, currentIndex, 2, 2);
            add(SpriteName.ButtonY, currentIndex, 2, 2);
            add(SpriteName.ButtonLB, currentIndex, 2, 2);
            add(SpriteName.ButtonRB, currentIndex, 2, 2);
            add(SpriteName.ButtonLT, currentIndex, 2, 2);
            add(SpriteName.ButtonRT, currentIndex, 2, 2);
            add(SpriteName.ButtonBACK, currentIndex, 2, 2);
            add(SpriteName.ButtonSTART, currentIndex, 2, 2);
            add(SpriteName.LeftStick_LR, currentIndex, 2, 2);
            add(SpriteName.RightStick_LR, currentIndex, 2, 2);
            add(SpriteName.LeftStick_UD, currentIndex, 2, 2);
            add(SpriteName.RightStick_UD, currentIndex, 2, 2);
            add(SpriteName.Dpad, currentIndex, 2, 2);
            add(SpriteName.DpadUpDown, currentIndex, 2, 2);
            add(SpriteName.DpadLeftRight, currentIndex, 2, 2);
            add(SpriteName.DpadUp, currentIndex, 2, 2);
            add(SpriteName.DpadDown, currentIndex, 2, 2);
            add(SpriteName.DpadLeft, currentIndex, 2, 2);
            add(SpriteName.DpadRight, currentIndex, 2, 2);
            add(SpriteName.LSClick, currentIndex, 2, 2);
            add(SpriteName.RSClick, currentIndex, 2, 2);
            add(SpriteName.ButtonLG, currentIndex, 2, 2);
            add(SpriteName.ButtonRG, currentIndex, 2, 2);
            add(SpriteName.TouchSurface1, currentIndex, 2, 2);
            add(SpriteName.TouchSurface2, currentIndex, 2, 2);
            add(SpriteName.TouchSurface1UpDown, currentIndex, 2, 2);
            add(SpriteName.TouchSurface2UpDown, currentIndex, 2, 2);
            add(SpriteName.TouchSurface1LeftRight, currentIndex, 2, 2);
            add(SpriteName.TouchSurface2LeftRight, currentIndex, 2, 2);
            add(SpriteName.ControllerIconP1, currentIndex, 2, 2);
            add(SpriteName.ControllerIconP2, currentIndex, 2, 2);
            add(SpriteName.ControllerIconP3, currentIndex, 2, 2);
            add(SpriteName.ControllerIconP4, currentIndex, 2, 2);
            add(SpriteName.GyroMove, currentIndex, 2, 2);
            add(SpriteName.GyroPitch, currentIndex, 2, 2);
            add(SpriteName.GyroYaw, currentIndex, 2, 2);
            add(SpriteName.GyroRoll, currentIndex, 2, 2);
            add(SpriteName.LeftStick, currentIndex, 2, 2);
            add(SpriteName.RightStick, currentIndex, 2, 2);

            add(SpriteName.MouseButtonLeft, currentIndex, 2, 2);
            add(SpriteName.MouseButtonRight, currentIndex, 2, 2);
            add(SpriteName.MouseButtonMiddle, currentIndex, 2, 2);
            add(SpriteName.MouseButtonX1, currentIndex, 2, 2);
            add(SpriteName.MouseButtonX2, currentIndex, 2, 2);
            add(SpriteName.MouseAllDir, currentIndex, 2, 2);
            add(SpriteName.MouseLR, currentIndex, 2, 2);
            add(SpriteName.MouseUD, currentIndex, 2, 2);
            add(SpriteName.FlipHori, currentIndex, 2, 2);
            add(SpriteName.FlipVerti, currentIndex, 2, 2);
            add(SpriteName.RotateCCW, currentIndex, 2, 2);
            add(SpriteName.RotateCW, currentIndex, 2, 2);
            add(SpriteName.CamZoom, currentIndex, 2, 2);
            add(SpriteName.CamAngleY, currentIndex, 2, 2);
            add(SpriteName.CamAngleX, currentIndex, 2, 2);
            add(SpriteName.Xdir, currentIndex, 2, 2);
            add(SpriteName.Ydir, currentIndex, 2, 2);
            add(SpriteName.Zdir, currentIndex, 2, 2);
            add(SpriteName.PrevFrame, currentIndex, 2, 2);
            add(SpriteName.NextFrame, currentIndex, 2, 2);
            add(SpriteName.cgTurnOver, currentIndex, 2, 2);
            add(SpriteName.cgButtonHighlight, currentIndex, 2, 2);
            add(SpriteName.Mouse, currentIndex, 2, 2);

            currentIndex = numTilesWidth * 17;
            add(SpriteName.cgCrocodile, currentIndex, 3, 3);
            add(SpriteName.cgGoblinScout, currentIndex, 3, 3);
            add(SpriteName.cgGoblinLineman, currentIndex, 3, 3);
            add(SpriteName.cgGoblinKing, currentIndex, 3, 3);
            add(SpriteName.cgFatBird2, currentIndex, 3, 3);
            add(SpriteName.cgFatBird, currentIndex, 3, 3);
            add(SpriteName.cgStatueBoss, currentIndex, 3, 3);
            add(SpriteName.cgBee, currentIndex, 3, 3);
            add(SpriteName.cgSkeletonBoss, currentIndex, 3, 3);
            add(SpriteName.cgSkeleton, currentIndex, 3, 3);
            add(SpriteName.cgZombie, currentIndex, 3, 3);
            add(SpriteName.cgHen, currentIndex, 3, 3);
            add(SpriteName.cgSpitBird, currentIndex, 3, 3);
            add(SpriteName.cgSheep, currentIndex, 3, 3);
            add(SpriteName.cgPitbull, currentIndex, 3, 3);
            add(SpriteName.cgPig, currentIndex, 3, 3);
            add(SpriteName.cgLizard, currentIndex, 3, 3);
            add(SpriteName.cgOrcArcher, currentIndex, 3, 3);
            add(SpriteName.cgOldHog, currentIndex, 3, 3);
            add(SpriteName.cgHorseWhite, currentIndex, 3, 3);
            add(SpriteName.cgHorseRed, currentIndex, 3, 3);
            add(SpriteName.cgHorseBrown, currentIndex, 3, 3);
            add(SpriteName.cgHog, currentIndex, 3, 3);
            add(SpriteName.cgHogBaby, currentIndex, 3, 3);
            add(SpriteName.cgHarpy, currentIndex, 3, 3);
            add(SpriteName.cgGoblinWolfrider, currentIndex, 3, 3);
            add(SpriteName.cgBat2, currentIndex, 3, 3);
            add(SpriteName.cgBat1, currentIndex, 3, 3);
            add(SpriteName.cgSlime, currentIndex, 3, 3);
            add(SpriteName.cgBigOrcBoss, currentIndex, 3, 3);
            add(SpriteName.cgFrog, currentIndex, 3, 3);
            add(SpriteName.cgGoblinBerserk, currentIndex, 3, 3);
            add(SpriteName.cgElfWardancer, currentIndex, 3, 3);
            add(SpriteName.cgElfKnight, currentIndex, 3, 3);
            add(SpriteName.cgElfArcher, currentIndex, 3, 3);
            add(SpriteName.cgOrcLineman, currentIndex, 3, 3);
            add(SpriteName.cgOrcKnight, currentIndex, 3, 3);
            add(SpriteName.cgOrcRider, currentIndex, 3, 3);
            add(SpriteName.cgZombieLeader, currentIndex, 3, 3);
            add(SpriteName.cgMummy, currentIndex, 3, 3);
            add(SpriteName.cgGhost, currentIndex, 3, 3);
            add(SpriteName.cgUnderConstruction, currentIndex, 3, 3);

            currentIndex = numTilesWidth * 20;

            add(SpriteName.LFAppearHatTypeCap, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeWitch, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeFootball, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypePirate1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypePirate2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypePirate3, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeSanta1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeSanta2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeSanta3, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeBaby1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeBaby2, currentIndex, 2, 2);

            add(SpriteName.LFAppearHatTypeArrow, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeBucket, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeCoif1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeCoif2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHigh1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHigh2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHunter1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHunter2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHunter3, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeLow1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeLow2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeMini1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeMini2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeMini3, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeTurban1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeTurban2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHeadband1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHeadband2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeHeadband3, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypecrown1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypecrown2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypecrown3, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeprincess1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeprincess2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeMaskTurtle1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeMaskZorro1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeMaskZorro2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHatTypeZelda, currentIndex, 2, 2);

            add(SpriteName.LFAppearHairTypeNormal, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeSpiky1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeSpiky2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeRag1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeRag2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeBald1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeBald2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeGirlyShort1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeGirlyShort2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeGirlyLong1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeGirlyLong2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeEmo1, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeEmo2, currentIndex, 2, 2);
            add(SpriteName.LFAppearHairTypeEmo3, currentIndex, 2, 2);

            add(SpriteName.LFAppearMouthTypeNoMouth, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeSmile, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeBigSmile, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeSideSmile1, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeSideSmile2, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeWideSmile, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeopen_smile, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeLoony, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeStraight, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeteeth1, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeteeth2, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeteeth3, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeOMG, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeHmm, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeSour, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypesouropen, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeOrc, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypevampire, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeGasp, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeLaugh, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeGirly1, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeGirly2, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypePirate, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeBaby1, currentIndex, 2, 2);
            add(SpriteName.LFAppearMouthTypeBaby2, currentIndex, 2, 2);

            add(SpriteName.LFAppearEyeTypeNoEyes, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeNormal, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSunshine, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeWink, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeLoony, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSlim, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeFrown, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeHardShut, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeEvil, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeRed, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSleepy, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSleepyCross, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCross, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCrossed1, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCrossed2, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCrossed3, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCrossed4, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCrossed5, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSad1, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSad2, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSad3, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSad4, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSad5, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeCyclops, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeVertical, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeGirly1, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeGirly2, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeGirly3, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypePirate, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSunGlasses1, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeSunGlasses2, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeEmo1, currentIndex, 2, 2);
            add(SpriteName.LFAppearEyeTypeEmo2, currentIndex, 2, 2);

            //BIRD TILES
            currentIndex = numTilesWidth * 24;
            add(SpriteName.birdWhiteSoftBox, currentIndex, 4, 4);
            add(SpriteName.pjNumMinus);
            add(SpriteName.birdCannonBall, currentIndex, 2, 1);
            add(SpriteName.birdCannonBallSmoke1);
            add(SpriteName.birdCannonBallSmoke2);
            add(SpriteName.birdFireball);
            add(SpriteName.birdFireballParticle);
            add(SpriteName.birdCoin1);
            add(SpriteName.birdCoin2);
            add(SpriteName.birdCoin3);
            add(SpriteName.birdCoin4);
            add(SpriteName.birdCoin5);
            add(SpriteName.birdCoin6);
            add(SpriteName.birdCoinParticleYellow);
            add(SpriteName.birdFlapParticle);
            add(SpriteName.birdFeather1);
            add(SpriteName.birdFeather2);
            add(SpriteName.birdFeather3);
            add(SpriteName.birdFeather4);
            add(SpriteName.birdLaserGun);
            add(SpriteName.birdShellShield);
            add(SpriteName.birdPlayerCount);
            add(SpriteName.joustJumpUpShield);
            currentIndex++;

            add(SpriteName.sheepFurWhite);
            add(SpriteName.sheepFur4);
            add(SpriteName.sheepFurPink);
            add(SpriteName.sheepFur2);
            add(SpriteName.pigBacon);
            add(SpriteName.fishScale1);
            add(SpriteName.fishScale2);
            add(SpriteName.fishScale3);
            add(SpriteName.fishScale4);

            add(SpriteName.birdPlayerNumberIcon);
            add(SpriteName.birdChangeDirBox);
            add(SpriteName.birdEdgeArrows);
            addQtile(SpriteName.birdHeart, Corner.NW);
            addQtile(SpriteName.birdHeartEmpty, Corner.NE);
            currentIndex++;
            add(SpriteName.birdSpeedUp);
            add(SpriteName.birdQuestionBox);
            //add(SpriteName.birdJoustWinner, currentIndex, 4, 1);
            //add(SpriteName.birdJoustDraw, currentIndex, 3, 1);
            //add(SpriteName.birdJoustTimesUp, currentIndex, 4, 1);

            add(SpriteName.pigBlingGlasses);
            add(SpriteName.hatBlingCap);
            add(SpriteName.hatHigh);
            add(SpriteName.hatViking);
            add(SpriteName.hatFez);
            add(SpriteName.hatPirate);
            add(SpriteName.hatCowboy);
            add(SpriteName.hatIndian);
            add(SpriteName.hatVlc);
            add(SpriteName.hatHalo);
            add(SpriteName.hatRobinHood);
            add(SpriteName.hatChidCap1);
            add(SpriteName.hatChidCap2);
            add(SpriteName.hatChidCap3);
            add(SpriteName.hatEnglish);
            add(SpriteName.hatGooglieEyes);
            add(SpriteName.hatSkyMask);
            add(SpriteName.hatBow);
            add(SpriteName.hatBunny);
            add(SpriteName.hatRainbow);
            add(SpriteName.hatKing);
            add(SpriteName.hatPrincess);
            add(SpriteName.hatRiddler);
            add(SpriteName.hatShades);
            add(SpriteName.hatButterix);
            add(SpriteName.hatScottish);
            add(SpriteName.hatLoveEyes);
            add(SpriteName.hatFrank);
            add(SpriteName.hatUniHorn);

            add(SpriteName.voidParticle1);
            add(SpriteName.voidParticle2);
            add(SpriteName.voidParticle3);

            add(SpriteName.fishBlingGlasses);
            add(SpriteName.hamsterPinkFur);
            add(SpriteName.hamsterOrangeFur);
            add(SpriteName.cowSpottySkin);
            add(SpriteName.cowBrownSkin);
            add(SpriteName.pigVoidBacon);
            add(SpriteName.cowMeat);
            add(SpriteName.goatFur);
            add(SpriteName.dogBone);

            add(SpriteName.rainbowParticle1);
            add(SpriteName.rainbowParticle2);
            add(SpriteName.rainbowParticle3);
            add(SpriteName.rainbowParticle4);
            add(SpriteName.rainbowParticle5);
            add(SpriteName.rainbowParticle6);
            add(SpriteName.fishBlingScale);
            add(SpriteName.sheepRainbowFur);
            add(SpriteName.dogBoneVoid);
            add(SpriteName.sheepFurZomb);
            add(SpriteName.fishBoneZomb);
            add(SpriteName.pigBaconZomb);
            add(SpriteName.birdFeatherZomb);

            add(SpriteName.mobaHenBlue);
            add(SpriteName.mobaHenRed);
            addWithSizeDef(SpriteName.mobaNinjaStarBlue, currentIndex, 15, 15);
            addWithSizeDef(SpriteName.mobaNinjaStarRed, currentIndex, 15, 15);

            add(SpriteName.pjGoldValue1);
            add(SpriteName.pjGoldValue2);
            add(SpriteName.pjGoldValue3);
            add(SpriteName.pjGoldValue4);

            add(SpriteName.golfStunnStars);
            addWithSizeDef(SpriteName.cirkleThick1Diameter26, currentIndex, 1, 1, 26, 26);
            add(SpriteName.golfBomb);
            add(SpriteName.golfBombRed);

            add(SpriteName.joustDiveBombFire);
            add(SpriteName.joustFallBoost);
            add(SpriteName.joustAirTrickWave);
            addQtile(SpriteName.pjRoundFireParticle1, Corner.NW);
            addQtile(SpriteName.pjRoundFireParticle2, Corner.NE);
            addQtile(SpriteName.pjRoundFireParticle3, Corner.SW);
            currentIndex++;
            add(SpriteName.PjJumpCurve, 2, 1);

            currentIndex += 3;

            addFullQtile(SpriteName.spaceWarAstreoidParticleRed1, SpriteName.spaceWarAstreoidParticleRed2,
                SpriteName.spaceWarAstreoidParticleRed3, SpriteName.spaceWarAstreoidParticleRed4);
            addFullQtile(SpriteName.spaceWarAstreoidParticleRed5, SpriteName.spaceWarAstreoidParticleBlue1,
                 SpriteName.spaceWarAstreoidParticleRed6, SpriteName.spaceWarAstreoidParticleBlue2);
            addFullQtile(SpriteName.spaceWarAstreoidParticleBlue3, SpriteName.spaceWarAstreoidParticleBlue4,
                SpriteName.spaceWarAstreoidParticleBlue5, SpriteName.spaceWarAstreoidParticleBlue6);

            addFullQtile(SpriteName.spaceWarStarWhite, SpriteName.golfChickenFlagFeather,
                 SpriteName.m3GameSpeedArrow, SpriteName.m3GameSpeedArrowGray);

            //--


            //--
            currentIndex = numTilesWidth * 25 + 1;
            {
                add(SpriteName.WhiteArea_LFtiles, 2, 2);
            }

            currentIndex = numTilesWidth * 25 + 6;
            {
                add(SpriteName.birdBobbleArrow, currentIndex, 2, 2);
                add(SpriteName.birdLobbyStartButton, currentIndex, 2, 2);
                add(SpriteName.birdLobbyMenuButton, currentIndex, 2, 2);
                add(SpriteName.pauseSymbol, currentIndex, 2, 2);

                add(SpriteName.sheepWhiteWingUp, currentIndex, 2, 2);
                add(SpriteName.sheepWhiteWingDown, currentIndex, 2, 2);
                add(SpriteName.sheepWhiteDead, currentIndex, 2, 2);//p1
                add(SpriteName.sheepPinkWingUp, currentIndex, 2, 2);
                add(SpriteName.sheepPinkWingDown, currentIndex, 2, 2);//p3
                add(SpriteName.sheepPinkDead, currentIndex, 2, 2);
                add(SpriteName.fishP1WingUp, currentIndex, 2, 2);
                add(SpriteName.fishP1WingDown, currentIndex, 2, 2);

                add(SpriteName.sheepP2WingUp, currentIndex, 2, 2);
                add(SpriteName.sheepP2WingDown, currentIndex, 2, 2);
                add(SpriteName.sheepP2Dead, currentIndex, 2, 2);
                add(SpriteName.sheepP4WingUp, currentIndex, 2, 2);
                add(SpriteName.sheepP4WingDown, currentIndex, 2, 2);
                add(SpriteName.sheepP4Dead, currentIndex, 2, 2);
                add(SpriteName.fishP2WingUp, currentIndex, 2, 2);
                add(SpriteName.fishP2WingDown, currentIndex, 2, 2);

                add(SpriteName.pigP1WingUp, currentIndex, 2, 2);
                add(SpriteName.pigP1WingDown, currentIndex, 2, 2);
                add(SpriteName.pigP1Dead, currentIndex, 2, 2);
                add(SpriteName.pigP3WingUp, currentIndex, 2, 2);
                add(SpriteName.pigP3WingDown, currentIndex, 2, 2);
                add(SpriteName.pigP3Dead, currentIndex, 2, 2);
                add(SpriteName.fishP4WingUp, currentIndex, 2, 2);
                add(SpriteName.fishP4WingDown, currentIndex, 2, 2);

                add(SpriteName.pigP2WingUp, currentIndex, 2, 2);
                add(SpriteName.pigP2WingDown, currentIndex, 2, 2);
                add(SpriteName.pigP2Dead, currentIndex, 2, 2);
                add(SpriteName.fishP3WingUp, currentIndex, 2, 2);
                add(SpriteName.fishP3WingDown, currentIndex, 2, 2);

                add(SpriteName.pigP4WingUp, currentIndex, 2, 2);
                add(SpriteName.pigP4WingDown, currentIndex, 2, 2);
                add(SpriteName.pigP4Dead, currentIndex, 2, 2);
                add(SpriteName.fishDead, currentIndex, 2, 2);

                add(SpriteName.birdP1WingUp, currentIndex, 2, 2);
                add(SpriteName.birdP1WingDown, currentIndex, 2, 2);
                add(SpriteName.birdP1Dead, currentIndex, 2, 2);
                add(SpriteName.birdP2WingUp, currentIndex, 2, 2);
                add(SpriteName.birdP2WingDown, currentIndex, 2, 2);
                add(SpriteName.birdP2Dead, currentIndex, 2, 2);
                add(SpriteName.birdP3WingUp, currentIndex, 2, 2);
                add(SpriteName.birdP3WingDown, currentIndex, 2, 2);
                add(SpriteName.birdP3Dead, currentIndex, 2, 2);
                add(SpriteName.birdP4WingUp, currentIndex, 2, 2);
                add(SpriteName.birdP4WingDown, currentIndex, 2, 2);
                add(SpriteName.birdP4Dead, currentIndex, 2, 2);

                add(SpriteName.birdUserHand, currentIndex, 2, 2);
                add(SpriteName.birdControllerIcon, currentIndex, 3, 2);

                add(SpriteName.pigBlingWingUp, currentIndex, 2, 2);
                add(SpriteName.pigBlingWingDown, currentIndex, 2, 2);
                add(SpriteName.pigBlingDead, currentIndex, 2, 2);
                add(SpriteName.birdPlayerFrame, currentIndex, 2, 2);
                add(SpriteName.birdPlayerFrameBling, currentIndex, 2, 2);
                add(SpriteName.emptyAnimal, currentIndex, 2, 2);
                currentIndex += 2;
                add(SpriteName.birdTeam1Frame, 2, 2);
                add(SpriteName.birdTeam2Frame, 2, 2);

            }

            currentIndex = numTilesWidth * 27 + 6;
            {
                add(SpriteName.goatDead, currentIndex, 2, 2);
                add(SpriteName.goatWingDown, currentIndex, 2, 2);
                add(SpriteName.goatWingUp, currentIndex, 2, 2);
                add(SpriteName.pugVoidDead, currentIndex, 2, 2);
                add(SpriteName.pugVoidWingDown, currentIndex, 2, 2);
                add(SpriteName.pugVoidWingUp, currentIndex, 2, 2);
                add(SpriteName.dogBrownDead, currentIndex, 2, 2);
                add(SpriteName.dogBrownWingDown, currentIndex, 2, 2);
                add(SpriteName.dogBrownWingUp, currentIndex, 2, 2);
                add(SpriteName.pugDead, currentIndex, 2, 2);
                add(SpriteName.pugWingDown, currentIndex, 2, 2);
                add(SpriteName.pugWingUp, currentIndex, 2, 2);
                add(SpriteName.cowSpottyDead, currentIndex, 2, 2);
                add(SpriteName.cowSpottyWingDown, currentIndex, 2, 2);
                add(SpriteName.cowSpottyWingUp, currentIndex, 2, 2);
                add(SpriteName.cowBrownDead, currentIndex, 2, 2);
                add(SpriteName.cowBrownWingDown, currentIndex, 2, 2);
                add(SpriteName.cowBrownWingUp, currentIndex, 2, 2);
                add(SpriteName.hamsterOrangeDead, currentIndex, 2, 2);
                add(SpriteName.hamsterOrangeWingDown, currentIndex, 2, 2);
                add(SpriteName.hamsterOrangeWingUp, currentIndex, 2, 2);
                add(SpriteName.hamsterPinkDead, currentIndex, 2, 2);
                add(SpriteName.hamsterPinkWingDown, currentIndex, 2, 2);
                add(SpriteName.hamsterPinkWingUp, currentIndex, 2, 2);
                add(SpriteName.catRainbowDead, currentIndex, 2, 2);
                add(SpriteName.catRainbowWingDown, currentIndex, 2, 2);
                add(SpriteName.catRainbowWingUp, currentIndex, 2, 2);
                add(SpriteName.catRainbowAngel, currentIndex, 2, 2);

                add(SpriteName.catRedDead, currentIndex, 2, 2);
                add(SpriteName.catRedWingDown, currentIndex, 2, 2);
                add(SpriteName.catRedWingUp, currentIndex, 2, 2);
                add(SpriteName.catRedAngel, currentIndex, 2, 2);
                add(SpriteName.catBlueDead, currentIndex, 2, 2);


                add(SpriteName.catBlueWingDown, currentIndex, 2, 2);
                add(SpriteName.catBlueWingUp, currentIndex, 2, 2);
                add(SpriteName.catBlueAngel, currentIndex, 2, 2);
                add(SpriteName.pigVoidWingUp, currentIndex, 2, 2);
                add(SpriteName.pigVoidWingDown, currentIndex, 2, 2);
                add(SpriteName.pigVoidDead, currentIndex, 2, 2);

                add(SpriteName.sheepRainbowWingUp, currentIndex, 2, 2);
                add(SpriteName.sheepRainbowWingDown, currentIndex, 2, 2);
                add(SpriteName.sheepRainbowDead, currentIndex, 2, 2);
                add(SpriteName.fishBlingWingUp, currentIndex, 2, 2);
                add(SpriteName.fishBlingWingDown, currentIndex, 2, 2);
                add(SpriteName.birdPlayerFrameRainbow, currentIndex, 2, 2);
                add(SpriteName.birdPlayerFrameVoid, currentIndex, 2, 2);
                add(SpriteName.birdShopButton, currentIndex, 2, 2);

                add(SpriteName.birdZombWingUp, currentIndex, 2, 2);
                add(SpriteName.birdZombWingDown, currentIndex, 2, 2);
                add(SpriteName.birdZombDead, currentIndex, 2, 2);
                add(SpriteName.pigZombWingUp, currentIndex, 2, 2);
                add(SpriteName.pigZombWingDown, currentIndex, 2, 2);
                add(SpriteName.pigZombDead, currentIndex, 2, 2);
                add(SpriteName.sheepZombWingUp, currentIndex, 2, 2);
                add(SpriteName.sheepZombWingDown, currentIndex, 2, 2);
                add(SpriteName.sheepZombDead, currentIndex, 2, 2);
                add(SpriteName.fishZombWingUp, currentIndex, 2, 2);
                add(SpriteName.fishZombWingDown, currentIndex, 2, 2);

                add(SpriteName.golfChickenFlag, currentIndex, 2, 2);
                addWithSizeDef(SpriteName.DisconnectSquare, currentIndex, 44, 44);
            }

            currentIndex = numTilesWidth * 29;
            {
                add(SpriteName.Dlc1True, currentIndex, 2, 3);
                add(SpriteName.Dlc2True, currentIndex, 2, 3);
                add(SpriteName.Dlc1False, currentIndex, 2, 3);
                add(SpriteName.Dlc2False, currentIndex, 2, 3);
                add(SpriteName.CountDownNumber1, currentIndex, 2, 3);
                add(SpriteName.CountDownNumber2, currentIndex, 2, 3);
                add(SpriteName.CountDownNumber3, currentIndex, 2, 3);
                add(SpriteName.birdUnlockModeDlc, currentIndex, 2, 3);
                add(SpriteName.birdNetworkIcon, currentIndex, 3, 3);

                add(SpriteName.spaceWarAstreoidRedLarge, currentIndex, 3, 3);
                add(SpriteName.spaceWarAstreoidBlueLarge, currentIndex, 3, 3);

                IntVector2 SpaceWarPoliceSz = new IntVector2(40, 66);
                addWithSizeDef(SpriteName.spaceWarPolice1, currentIndex, 1, 1, SpaceWarPoliceSz.X, SpaceWarPoliceSz.Y);
                addWithSizeDef(SpriteName.spaceWarPolice2, currentIndex, 1, 1, SpaceWarPoliceSz.X, SpaceWarPoliceSz.Y);

                const int ShopW = 94;
                addWithSizeDef(SpriteName.spaceWarShopSquareOn, currentIndex, 1, 1, ShopW, ShopW);
                addWithSizeDef(SpriteName.spaceWarShopSquareOff, currentIndex, 1, 1, ShopW, ShopW);
            }

            currentIndex = numTilesWidth * 32;
            {
                add(SpriteName.DlcZombie, currentIndex, 2, 2);
                add(SpriteName.BirdTimesUp1, currentIndex, 2, 2);
                add(SpriteName.BirdTimesUp2, currentIndex, 2, 2);
                add(SpriteName.BirdThrophy, currentIndex, 2, 2);
                add(SpriteName.BirdNoThrophy, currentIndex, 2, 2);
                add(SpriteName.BirdRoundFlag, currentIndex, 2, 2);
                add(SpriteName.fishP1Dead, currentIndex, 2, 2);
                add(SpriteName.fishP2Dead, currentIndex, 2, 2);
                add(SpriteName.fishP4Dead, currentIndex, 2, 2);
                add(SpriteName.fishP3Dead, currentIndex, 2, 2);
                add(SpriteName.fishBlingDead, currentIndex, 2, 2);
                add(SpriteName.birdPlayerFrameZomb, currentIndex, 2, 2);
                add(SpriteName.birdLobbyReturnButton, currentIndex, 2, 2);

                add(SpriteName.MenuIconExit, currentIndex, 4, 2);
                add(SpriteName.MenuIconResume, currentIndex, 2, 2);
                add(SpriteName.MenuIconMusicVol, currentIndex, 2, 2);
                add(SpriteName.MenuIconSoundVol, currentIndex, 2, 2);
                add(SpriteName.MenuIconCredits, currentIndex, 2, 2);
                add(SpriteName.MenuIconSettings, currentIndex, 2, 2);
                currentIndex += 2;
                //add(SpriteName.MenuIconFullScreen, currentIndex, 2, 2);
                add(SpriteName.birdLobbyExitButton, currentIndex, 2, 2);
                add(SpriteName.MenuIconScreenResolution, currentIndex, 2, 2);
                add(SpriteName.MenuIconApplyResolution, currentIndex, 2, 2);

                currentIndex += 2;
                add(SpriteName.MenuIconMultiMonitor, currentIndex, 2, 2);

                add(SpriteName.mrwWingUp, currentIndex, 2, 2);
                add(SpriteName.mrwWingDown, currentIndex, 2, 2);
                add(SpriteName.mrwWingDead, currentIndex, 2, 2);
                currentIndex += 2;

                add(SpriteName.birdAnyoneNetwork, currentIndex, 2, 2);
                add(SpriteName.birdFriendsNetwork, currentIndex, 2, 2);
                add(SpriteName.birdNoNetwork, currentIndex, 2, 2);
                add(SpriteName.birdJoinNetwork, currentIndex, 3, 2);
                //add(SpriteName.birdOptionsButtonUnselected, currentIndex, 2, 2);
                //add(SpriteName.birdOptionsButtonSelected, currentIndex, 2, 2);
                add(SpriteName.birdZeroLobbies, currentIndex, 4, 2);
                add(SpriteName.birdFriendlyLobbyIcon, currentIndex, 1, 2);
                add(SpriteName.birdUnknownLobbyIcon, currentIndex, 1, 2);
                add(SpriteName.birdCoinValue5, currentIndex, 2, 2);
                add(SpriteName.birdCoinValue10, currentIndex, 2, 2);
                add(SpriteName.birdCoinValue20, currentIndex, 2, 2);

                add(SpriteName.mobaPigBlue, currentIndex, 2, 2);
                add(SpriteName.mobaPigRed, currentIndex, 2, 2);
                add(SpriteName.mobaCatBlue, currentIndex, 2, 2);
                add(SpriteName.mobaCatRed, currentIndex, 2, 2);
                add(SpriteName.modaTowerBlue, currentIndex, 2, 2);
                add(SpriteName.mobaTowerRed, currentIndex, 2, 2);
                add(SpriteName.mobaGoatBlue, currentIndex, 2, 2);
                add(SpriteName.mobaGoatRed, currentIndex, 2, 2);

                add(SpriteName.golfHole, currentIndex, 2, 2);
                add(SpriteName.golfSand1Bg, currentIndex, 2, 2);
                add(SpriteName.golfSand2Bg, currentIndex, 2, 2);
                add(SpriteName.golfGraySand1Bg, currentIndex, 2, 2);
                add(SpriteName.golfGraySand2Bg, currentIndex, 2, 2);

                add(SpriteName.golfSand1Top, currentIndex, 2, 2);
                add(SpriteName.golfSand2Top, currentIndex, 2, 2);
                add(SpriteName.golfGraySand1Top, currentIndex, 2, 2);
                add(SpriteName.golfGraySand2Top, currentIndex, 2, 2);

                addWithSizeDef(SpriteName.birdModeMatch3, currentIndex, PjModeSz.X, PjModeSz.Y);
                addWithSizeDef(SpriteName.birdModeGolf, currentIndex, PjModeSz.X, PjModeSz.Y);
                addWithSizeDef(SpriteName.birdModeCarBall, currentIndex, PjModeSz.X, PjModeSz.Y);

            }

            currentIndex = numTilesWidth * 34;
            {
                add(SpriteName.birdRotatingCrown1);
                add(SpriteName.birdRotatingCrown2);
                add(SpriteName.birdRotatingCrown3);
                add(SpriteName.birdRotatingCrown4);
                add(SpriteName.birdRotatingCrown5);
                add(SpriteName.birdRotatingCrown6);
                add(SpriteName.blingParticle1);
                add(SpriteName.blingParticle2);
                add(SpriteName.blingParticle3);
                add(SpriteName.winnerParticle);

                addWithSizeDef(SpriteName.birdPlayerFrameClose, currentIndex, 6, 6, 32 - 12, 32 - 12);
                add(SpriteName.BirdShieldIcon);

                add(SpriteName.easterShell);
                add(SpriteName.easterShellTop);
                add(SpriteName.easterShellBottom);
                add(SpriteName.easterChick1);
                add(SpriteName.easterChick2);
                add(SpriteName.birdSpikeBall);
                add(SpriteName.MousePointer);
                add(SpriteName.ClickCirkleEffect);
                add(SpriteName.birdBallTrace);
                add(SpriteName.birdBumpCount0);
                add(SpriteName.birdBumpCount1);
                add(SpriteName.birdBumpCount2);
                add(SpriteName.birdBumpCount3);
                add(SpriteName.birdCoinParticleBlue);
                add(SpriteName.birdCoinParticleGreen);
                add(SpriteName.bagBallExitFireParticleA1);
                add(SpriteName.bagBallExitFireParticleA2);
                add(SpriteName.bagBallExitFireParticleA3);
                add(SpriteName.bagBallExitFireParticleB1);
                add(SpriteName.bagBallExitFireParticleB2);
                add(SpriteName.bagBallExitFireParticleB3);
                add(SpriteName.bagBallExitFireParticleC1);
                add(SpriteName.bagBallExitFireParticleC2);
                add(SpriteName.bagBallExitFireParticleC3);

                add(SpriteName.birdCoinHidden);
                add(SpriteName.bagSplitPickupEffect, currentIndex, 2, 1);
                add(SpriteName.cballBall);
                add(SpriteName.birdLocalPlayersCountIcon, currentIndex, 2, 1);
                add(SpriteName.birdUnlockModeDlcArrow, currentIndex, 2, 1);

                add(SpriteName.cballBallAnim1);
                add(SpriteName.cballBallAnim2);
                add(SpriteName.cballBallAnim3);
                add(SpriteName.cballBallAnim4);
                add(SpriteName.cballBallAnim5);
                add(SpriteName.cballBallAnim6);
                add(SpriteName.cballBallAnim7);
                add(SpriteName.cballBallAnim8);

                add(SpriteName.cballPigPinkHead);
                add(SpriteName.cballSheepWhiteHead);
                add(SpriteName.cballBirdYellowHead);
                add(SpriteName.cballFishGreenHead);


                add(SpriteName.cballPigPinkHeadBlink);
                add(SpriteName.cballSheepWhiteHeadBlink);
                add(SpriteName.cballBirdYellowHeadBlink);
                add(SpriteName.cballFishGreenHeadBlink);


                add(SpriteName.cballPigPinkArms);
                add(SpriteName.cballSheepWhiteArms);
                add(SpriteName.cballBirdYellowArms);
                add(SpriteName.cballFishGreenArms);


                add(SpriteName.cballPigPinkArmsL);
                add(SpriteName.cballSheepWhiteArmsL);
                add(SpriteName.cballBirdYellowArmsL);
                add(SpriteName.cballFishGreenArmsL);

                add(SpriteName.cballPigPinkArmsR);
                add(SpriteName.cballSheepWhiteArmsR);
                add(SpriteName.cballBirdYellowArmsR);
                add(SpriteName.cballFishGreenArmsR);

                addFullQtile(SpriteName.cballHat1,
                     SpriteName.cballHat2,
                     SpriteName.cballHat3,
                     SpriteName.cballHat4);

                addQtile(SpriteName.cballHat5, Corner.NW, currentIndex);
                addQtile(SpriteName.cballHat6, Corner.NE, currentIndex);
                addQtile(SpriteName.cballHat7, Corner.SW, currentIndex);

                currentIndex += 1;

                add(SpriteName.golfArrowMark);
                add(SpriteName.golfArrowMarkWide);
                add(SpriteName.golfArrow1);
                add(SpriteName.golfArrow2);
                add(SpriteName.golfArrow3);
                add(SpriteName.golfArrow4);
                add(SpriteName.golfArrow5);
                add(SpriteName.golfArrow6);
                add(SpriteName.golfArrow7);
                add(SpriteName.golfArrow8);
                add(SpriteName.golfArrow9);
                add(SpriteName.golfArrow10);
                addWithSizeDef(SpriteName.golfBrickBrown, currentIndex, 1, 1, 30, 30);
                addWithSizeDef(SpriteName.golfBrickBlue, currentIndex, 1, 1, 30, 30);

                addWithSizeDef(SpriteName.golfSpikeSquare, currentIndex, 1, 1, 30, 30);
                add(SpriteName.spaceWarRocket1);
                add(SpriteName.spaceWarRocket2);
                add(SpriteName.spaceWarShopAddTail, 2, 1);
                add(SpriteName.pjBarrel);

                const int BrickTexWidth = 18;
                addWithSizeDef(SpriteName.m3BrickYellow, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3BrickRed, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3BrickOrange, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3BrickPurple, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3BrickBlue, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3BrickGreen, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3BrickStone, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);

                addWithSizeDef(SpriteName.m3PrevBrickYellow, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3PrevBrickRed, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3PrevBrickOrange, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3PrevBrickPurple, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3PrevBrickBlue, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3PrevBrickGreen, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);

                addWithSizeDef(SpriteName.cballScoreOutline, currentIndex, 1, 1, 18, 18);
                addWithSizeDef(SpriteName.cballScoreFilled, currentIndex, 1, 1, 18, 18);
                addWithSizeDef(SpriteName.cballBallLarge, currentIndex, 1, 1, 26, 26);

                addWithSizeDef(SpriteName.cballGloveBlue, currentIndex, 1, 1, 17, 17);
                addWithSizeDef(SpriteName.cballGloveRed, currentIndex, 1, 1, 17, 17);

                addFullQtile(SpriteName.cballTrack1, SpriteName.cballTrack2, SpriteName.cballTrack3, SpriteName.cballTrack4);
                addFullQtile(SpriteName.cballTrack5, SpriteName.cballTrack6, SpriteName.cballTrack7, SpriteName.cballTrack8);

                addWithSizeDef(SpriteName.m3GrayBrickYellow, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3GrayBrickRed, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3GrayBrickOrange, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3GrayBrickPurple, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3GrayBrickBlue, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3GrayBrickGreen, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);
                addWithSizeDef(SpriteName.m3GrayBrickStone, currentIndex, 1, 1, BrickTexWidth, BrickTexWidth);

                {
                    int index = currentIndex;
                    addWithSizeDef(SpriteName.spaceWarRocketFireWide, index, 15, 15);
                    addWithSizeDef(SpriteName.spaceWarRocketFire, index, 16, 0, 15, 15);
                    addQtile(SpriteName.FatArrow16pix, Corner.SW, index);
                    addQtile(SpriteName.FatArrow16pix_Triangle, Corner.SE, index);
                    currentIndex = index + 1;
                }
                addWithSizeDef(SpriteName.spaceWarCargoIcon, currentIndex, 22, 22);
                addWithSizeDef(SpriteName.Ticket, currentIndex, 20, 20);
                add(SpriteName.PjHoldButtonBar, 3, 1);
            }

            currentIndex = numTilesWidth * 35;
            {
                add(SpriteName.bagPeg1_off, currentIndex, 2, 2);
                add(SpriteName.bagPeg1_on, currentIndex, 2, 2);
                add(SpriteName.bagPeg2_off, currentIndex, 2, 2);
                add(SpriteName.bagPeg2_on, currentIndex, 2, 2);
                add(SpriteName.bagPeg3_off, currentIndex, 2, 2);
                add(SpriteName.bagPeg3_on, currentIndex, 2, 2);
                add(SpriteName.bagPeg4_off, currentIndex, 2, 2);
                add(SpriteName.bagPeg4_on, currentIndex, 2, 2);
                add(SpriteName.bagPeg5_off, currentIndex, 2, 2);
                add(SpriteName.bagPeg5_on, currentIndex, 2, 2);

                add(SpriteName.bagSnakeHead, currentIndex, 2, 2);

                add(SpriteName.bagCoinPeg_off, currentIndex, 2, 2);
                add(SpriteName.bagCoinPeg_on, currentIndex, 2, 2);

                add(SpriteName.bagSnakePeg1_off, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg1_on, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg2_off, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg2_on, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg3_off, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg3_on, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg4_off, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg4_on, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg5_off, currentIndex, 2, 2);
                add(SpriteName.bagSnakePeg5_on, currentIndex, 2, 2);

                add(SpriteName.bagPegFlashAnim1, currentIndex, 2, 2);
                add(SpriteName.bagPegFlashAnim2, currentIndex, 2, 2);
                add(SpriteName.bagPegFlashAnim3, currentIndex, 2, 2);
                add(SpriteName.bagPegFlashAnim4, currentIndex, 2, 2);
                add(SpriteName.bagPegFlashAnim5, currentIndex, 2, 2);
                add(SpriteName.bagPegFlashAnim6, currentIndex, 2, 2);

                add(SpriteName.bagBumpRefill, currentIndex, 2, 2);
                addWithSizeDef(SpriteName.bagRails, currentIndex, 1, 0, 158, 64);
                add(SpriteName.birdLock, currentIndex, 2, 2);
                add(SpriteName.birdUnLock, currentIndex, 2, 2);

                addWithSizeDef(SpriteName.birdModeBagatelle, currentIndex, PjModeSz.X, PjModeSz.Y);
                addWithSizeDef(SpriteName.birdModeJoust, currentIndex, PjModeSz.X, PjModeSz.Y);
                addWithSizeDef(SpriteName.birdModeContruction, currentIndex, PjModeSz.X, PjModeSz.Y);

                addWithSizeDef(SpriteName.birdOnlinePlayersCountIcon, currentIndex, 0, 0, OnlinePlayerCountIconSz.X, OnlinePlayerCountIconSz.Y);
                addWithSizeDef(SpriteName.birdModeNextButton, currentIndex, 0, 0, 20, 64);

                add(SpriteName.birdButtonPressUp, currentIndex, 2, 2);
                add(SpriteName.birdButtonPressDown, currentIndex, 2, 2);
                add(SpriteName.birdBumpWave, currentIndex, 2, 2);

                currentIndex += 12;

                add(SpriteName.cballCarBlue, currentIndex, 2, 2);
                add(SpriteName.cballCarRed, currentIndex, 2, 2);
                add(SpriteName.cballCarRedTurnL, currentIndex, 2, 2);
                add(SpriteName.cballCarBlueTurnL, currentIndex, 2, 2);
                add(SpriteName.cballCarRedTurnR, currentIndex, 2, 2);
                add(SpriteName.cballCarBlueTurnR, currentIndex, 2, 2);
                add(SpriteName.cballCarOutline, currentIndex, 2, 2);
                add(SpriteName.cballCarTurnArrow, currentIndex, 2, 2);
                addWithSizeDef(SpriteName.cballGoliePaddleBlue, currentIndex, 1, 1, CballGoliePaddleSz.X, CballGoliePaddleSz.Y);
                addWithSizeDef(SpriteName.cballGoliePaddleRed, currentIndex, 1, 1, CballGoliePaddleSz.X, CballGoliePaddleSz.Y);
                add(SpriteName.m3RotateArrow, 2, 2);
                addWithSizeDef(SpriteName.BluePrintSquareFull, currentIndex, 52, 52);
                addWithSizeDef(SpriteName.BluePrintSquareHalf, currentIndex, 48, 48);

            }

            currentIndex = numTilesWidth * 37;
            {
                add(SpriteName.ArrowKeys, currentIndex, 2, 2);
                add(SpriteName.CamPan, currentIndex, 2, 2);
                add(SpriteName.MouseScroll, currentIndex, 2, 2);
                add(SpriteName.CamPersonMode, currentIndex, 2, 2);
                add(SpriteName.ButtonDisabledCross, currentIndex, 2, 2);

                addWithSizeDef(SpriteName.pjButtonTexBlue, currentIndex, 1, 1, 62, 62);
                addWithSizeDef(SpriteName.pjButtonTexRed, currentIndex, 1, 1, 62, 62);

                addWithSizeDef(SpriteName.pjOptionButtonTexOn, currentIndex, 1, 1, 46, 46);
                addWithSizeDef(SpriteName.pjOptionButtonTexOff, currentIndex, 1, 1, 46, 46);

                add(SpriteName.MenuIconMonitorFrame, currentIndex, 2, 2);
                add(SpriteName.MenuIconMonitorArrowsOut, 2, 2);
                add(SpriteName.MenuIconMonitorArrowsIn, 2, 2);

                addWithSizeDef(SpriteName.DlcBoxPrime, currentIndex, 36, 36);
                addWithSizeDef(SpriteName.DlcBoxBling, currentIndex, 36, 36);
                addWithSizeDef(SpriteName.DlcBoxZombie, currentIndex, 36, 36);

                add(SpriteName.spaceWarFogLine1, 8, 2);
                add(SpriteName.spaceWarFogLine2, 8, 2);
                add(SpriteName.spaceWarFogLine3, 8, 2);
                add(SpriteName.spaceWarFogLine4, 8, 2);

                add(SpriteName.spaceWarFogCirkle1, 2, 2);
                add(SpriteName.spaceWarFogCirkle2, 2, 2);
                add(SpriteName.spaceWarFogCirkle3, 2, 2);
                add(SpriteName.spaceWarFogCirkle4, 2, 2);
                add(SpriteName.spaceWarFogCirkle5, 2, 2);
                add(SpriteName.spaceWarFogCirkle6, 2, 2);
                add(SpriteName.spaceWarFogCirkle7, 2, 2);
                add(SpriteName.spaceWarFogCirkle8, 2, 2);
                add(SpriteName.spaceWarFogCirkle9, 2, 2);
                add(SpriteName.spaceWarFogCirkle10, 2, 2);
                add(SpriteName.spaceWarFogCirkle11, 2, 2);

                add(SpriteName.spaceWarAstreoidRed, 2, 2);
                add(SpriteName.spaceWarAstreoidBlue, 2, 2);

                add(SpriteName.spaceWarShipYellow, 2, 2);
                add(SpriteName.spaceWarShipYellowTailMid, 2, 2);
                add(SpriteName.spaceWarShipYellowTailEnd, 2, 2);

                add(SpriteName.pjGoldValue5, 2, 2);
                add(SpriteName.spaceWarTurnArrowOnShield, 2, 2);
                add(SpriteName.spaceWarTurnArrow, 2, 2);
                add(SpriteName.spaceWarShipTailShield, 2, 2);
                add(SpriteName.spaceWarShipShield, 2, 2);

                add(SpriteName.meatpie1, 2, 2);
                add(SpriteName.meatpie2, 2, 2);
                add(SpriteName.meatpie3, 2, 2);
                add(SpriteName.meatpie4, 2, 2);
                add(SpriteName.meatpie5, 2, 2);
                add(SpriteName.meatpie6, 2, 2);

            }

            currentIndex = numTilesWidth * 39;
            {
                add(SpriteName.pjNum0);
                add(SpriteName.pjNum1);
                add(SpriteName.pjNum2);
                add(SpriteName.pjNum3);
                add(SpriteName.pjNum4);
                add(SpriteName.pjNum5);
                add(SpriteName.pjNum6);
                add(SpriteName.pjNum7);
                add(SpriteName.pjNum8);
                add(SpriteName.pjNum9);
                add(SpriteName.pjNumPlus);
                add(SpriteName.pjNumMinus);
                add(SpriteName.pjNumMultiply);
                add(SpriteName.pjNumEquals);
                add(SpriteName.pjNumArrowR);
                add(SpriteName.pjNumExpression);
                add(SpriteName.pjNumQuestion);

                add(SpriteName.cballPigOrangeHead);
                add(SpriteName.cballPigOrangeHeadBlink);
                add(SpriteName.cballPigOrangeArms);
                add(SpriteName.cballPigOrangeArmsL);
                add(SpriteName.cballPigOrangeArmsR);

                add(SpriteName.cballSheepBlackHead);
                add(SpriteName.cballSheepBlackHeadBlink);
                add(SpriteName.cballSheepBlackArms);
                add(SpriteName.cballSheepBlackArmsL);
                add(SpriteName.cballSheepBlackArmsR);

                add(SpriteName.cballBirdPinkHead);
                add(SpriteName.cballBirdPinkHeadBlink);
                add(SpriteName.cballBirdPinkArms);
                add(SpriteName.cballBirdPinkArmsL);
                add(SpriteName.cballBirdPinkArmsR);

                add(SpriteName.cballFishOrangeHead);
                add(SpriteName.cballFishOrangeHeadBlink);
                add(SpriteName.cballFishOrangeArms);
                add(SpriteName.cballFishOrangeArmsL);
                add(SpriteName.cballFishOrangeArmsR);

                add(SpriteName.PjHeartFull);
                add(SpriteName.PjHeartBroken);
                add(SpriteName.PjDeathSpeechBobble);
                add(SpriteName.PjSmackIcon);
            }

            currentIndex = numTilesWidth * 40;
            {
                add(SpriteName.LittleUnitIconTower, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconVikingman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconSpearman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconSoldierMan, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconSlingman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconHealerMan, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconDogman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconFatman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconArcherman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitShadow, currentIndex, 3, 3);
                add(SpriteName.LittleUnitSelectionFilled, currentIndex, 3, 3);
                add(SpriteName.LittleUnitSelectionDotted, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconScoutman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconFarmerman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconFarmerHordeman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconBallistaman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconKnightman, currentIndex, 3, 3);
                add(SpriteName.LittleHaltOrder, currentIndex, 3, 3);
                add(SpriteName.LittleMoveOrder, currentIndex, 3, 3);
                add(SpriteName.LittleThumbBobble, currentIndex, 3, 3);

                add(SpriteName.bagBallExitFire1, currentIndex, 2, 3);
                add(SpriteName.bagBallExitFire2, currentIndex, 2, 3);
                add(SpriteName.bagBallExitFire3, currentIndex, 2, 3);
                add(SpriteName.bagBallExitFire4, currentIndex, 2, 3);
                add(SpriteName.bagBallExitFire5, currentIndex, 2, 3);
                add(SpriteName.bagBallExitFire6, currentIndex, 2, 3);
                add(SpriteName.bagCannon, currentIndex, 2, 3);

                add(SpriteName.LittleUnitIconCrossBowman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconRocketLauncherman, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconBombThrowerman, currentIndex, 3, 3);

                add(SpriteName.LittleUnitIconRamMan, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconLongSwordMan, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconJavelinerMan, currentIndex, 3, 3);

                add(SpriteName.LittleUnitIconKingMan, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconMillMan, currentIndex, 3, 3);
                add(SpriteName.LittleUnitIconBannerMan, currentIndex, 3, 3);

                add(SpriteName.WhiteCirkle, currentIndex, 3, 3);
                add(SpriteName.cmdLargePlainCheckOn, currentIndex, 3, 3);
                add(SpriteName.cmdLargePlainCheckOff, currentIndex, 3, 3);

            }

            currentIndex = numTilesWidth * 43;
            add(SpriteName.LittleFlagIconNeutral);
            add(SpriteName.LittleFlagIconMan);
            add(SpriteName.LittleFlagIconOrc);
            add(SpriteName.LittleMinusCoin);
            add(SpriteName.LittleStatsUnitTarget);
            add(SpriteName.LittleStatsUnitTargetArrow);

            currentIndex = numTilesWidth * 44;
            {
                add(SpriteName.LittleMoveArrowFilled, currentIndex, 2, 2);
                add(SpriteName.LittleMoveArrowOutline, currentIndex, 2, 2);
                currentIndex += 4;
                add(SpriteName.LittleStatsSoldiersCount, currentIndex, 2, 2);
                add(SpriteName.LittleStatsAddPig, currentIndex, 2, 2);
                add(SpriteName.LittleThumbDown, currentIndex, 2, 2);
                add(SpriteName.LittleThumbUp, currentIndex, 2, 2);
                add(SpriteName.LittleStatsNotStructure, currentIndex, 2, 2);
                add(SpriteName.LittleStatsProjectileAttack, currentIndex, 2, 2);
                add(SpriteName.LittleStatsAttack, currentIndex, 2, 2);
                add(SpriteName.LittleStatsRange, currentIndex, 2, 2);
                add(SpriteName.LittleStatsMoveArrows, currentIndex, 2, 2);
                add(SpriteName.LittleStatsMoveShoe, currentIndex, 2, 2);
                add(SpriteName.LittleStatsDamage, currentIndex, 2, 2);
                add(SpriteName.LittleStatsTime, currentIndex, 2, 2);
                add(SpriteName.LittleStatsHealth, currentIndex, 2, 2);

                add(SpriteName.LittleStatsProjectileArmor, currentIndex, 2, 2);
                add(SpriteName.LittleStatsNotSoldier, currentIndex, 2, 2);
                add(SpriteName.LittleStatsBomb, currentIndex, 2, 2);
                add(SpriteName.LittleStatsBombRadius, currentIndex, 2, 2);
                add(SpriteName.LittleStatsTargetStructure, currentIndex, 2, 2);
                add(SpriteName.LittleStatsStructure, currentIndex, 2, 2);
                add(SpriteName.LittleStatsBanner, currentIndex, 2, 2);

                add(SpriteName.warsDefaultPlayerIcon, currentIndex, 2, 2);
                add(SpriteName.LittleStatsScoutMovement, currentIndex, 2, 2);
                add(SpriteName.warsMapSettings, currentIndex, 2, 2);

                add(SpriteName.golfBugWalkS1, 2, 2);
                add(SpriteName.golfBugWalkS2, 2, 2);
                add(SpriteName.golfBugWalkN1, 2, 2);
                add(SpriteName.golfBugWalkN2, 2, 2);
                add(SpriteName.golfBugHideS1, 2, 2);
                add(SpriteName.golfBugHideS2, 2, 2);
                add(SpriteName.golfBugHideN1, 2, 2);
                add(SpriteName.golfBugHideN2, 2, 2);
                add(SpriteName.golfBugDead, 2, 2);

                const int WingSz = 47;
                addWithSizeDef(SpriteName.pjCoinWings1, currentIndex, WingSz, WingSz);
                addWithSizeDef(SpriteName.pjCoinWings2, currentIndex, WingSz, WingSz);
                addWithSizeDef(SpriteName.pjCoinWings3, currentIndex, WingSz, WingSz);
                addWithSizeDef(SpriteName.pjCoinWings4, currentIndex, WingSz, WingSz);
            }

            currentIndex = numTilesWidth * 46;
            {
                add(SpriteName.birdLobbyVisibilityAnyone, currentIndex, 10, 4);
                add(SpriteName.birdLobbyVisibilityFriends, currentIndex, 10, 4);
                add(SpriteName.birdLobbyVisibilityHidden, currentIndex, 10, 4);
                add(SpriteName.LittleCardBgMan, currentIndex, 3, 4);


                add(SpriteName.warsTitle, currentIndex, WarsTitleSize.X, WarsTitleSize.Y);


                addWithSizeDef(SpriteName.spaceWarLorry, currentIndex, 1, 1, SpaceWarLorryPixSz.X, SpaceWarLorryPixSz.Y);

            }

            //if (PlatformSettings.RunProgram == StartProgram.ToGG)
            {
                currentIndex = numTilesWidth * 50;
                {
                    add(SpriteName.cmdUnitElf_Scout, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitElf_Archer, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitElf_Faun, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitElf_LightCavalry, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitElf_HeavyCavalry, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitElf_Wardancer, currentIndex, 4, 4);

                    add(SpriteName.cmdUnitHuman_LightArcher, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_HeavyArcher, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_Scout, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_LightSpearman, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_Warrior, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_HeavySpearman, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_LightCavalry, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitHuman_HeavyCavalry, currentIndex, 4, 4);

                    add(SpriteName.cmdUnitOrc_LightSwordsman, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_HeavySwordsman, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_LightWolfRider, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_HeavyWolfRider, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_LightArcher, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_GoblinGuard, currentIndex, 4, 4);

                    add(SpriteName.cmdUnitUndead_LightCavalry, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_HeavyCavalry, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_MedInfantry, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_LightArcher, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_HeavyArcher, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_Warrior, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_LightInfantry, currentIndex, 4, 4);


                    add(SpriteName.cmdUnitTent, currentIndex, 4, 4);
                    add(SpriteName.hqPetDog, currentIndex, 4, 4);
                    add(SpriteName.hqPetCat, currentIndex, 4, 4);
                    add(SpriteName.hqCyclops, currentIndex, 4, 4);

                    add(SpriteName.cmdUnitPracticeDummy, currentIndex, 4, 4);
                }

                currentIndex = numTilesWidth * 54;
                {
                    add(SpriteName.cmdBanner, currentIndex, 4, 4);
                    add(SpriteName.BackStabArrow, currentIndex, 4, 4);

                    add(SpriteName.cmdHero_Magician, currentIndex, 4, 4);
                    add(SpriteName.cmdHero_Knight, currentIndex, 4, 4);
                    add(SpriteName.cmdHero_Archer, currentIndex, 4, 4);

                    add(SpriteName.cmdUnitChariot, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_Necromancer, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_Hero, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitBallista, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitPracticeDummyArmor, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitUndead_HeavySwordBeast, 4, 4);
                    add(SpriteName.cmdUnitUndead_SpearBeast, 4, 4);

                    add(SpriteName.cmdUnitUnknownDummy1, 4, 4);
                    add(SpriteName.cmdUnitUnknownDummy2, 4, 4);
                    add(SpriteName.cmdUnitUnknownDummy3, 4, 4);
                    add(SpriteName.cmdUnitUnknownDummy4, 4, 4);

                    add(SpriteName.cmdUnitBatteringRam, 4, 4);
                    add(SpriteName.cmdUnitSupplyWagon, 4, 4);
                    add(SpriteName.cmdUnitBoneDragon, 4, 4);

                    add(SpriteName.hqHero_Dwarf, 4, 4);
                    add(SpriteName.hqHero_Recruit_Sword, 4, 4);
                    add(SpriteName.hqHero_Amazon, 4, 4);
                    add(SpriteName.hqHero_Chibi, 4, 4);
                    add(SpriteName.hqPetFluffYellow, 4, 4);
                    add(SpriteName.hqHero_PaladinF, 4, 4);
                    add(SpriteName.hqSpikeTrap, 4, 4);

                    add(SpriteName.hqOgre, 4, 4);
                    add(SpriteName.hqNaga, 4, 4);
                    add(SpriteName.hqNagaCommander, 4, 4);
                    add(SpriteName.hqFireLizard, 4, 4);
                    add(SpriteName.hqCaveSpider, 4, 4);
                    add(SpriteName.hqBat, 4, 4);


                    //add(SpriteName.cmdUnitHiResTest, 4, 4);
                }

                currentIndex = numTilesWidth * 58;
                {
                    add(SpriteName.hqHero_KhajaF, currentIndex, 4, 4);
                    add(SpriteName.hqTrapDecoy, currentIndex, 4, 4);
                    add(SpriteName.hqFoodStorage, currentIndex, 4, 4);
                    add(SpriteName.hqDarkPriest, currentIndex, 4, 4);

                    add(SpriteName.hqPetRat, currentIndex, 4, 4);
                    add(SpriteName.hqPetRatBigSack, currentIndex, 4, 4);
                    add(SpriteName.cmdGoblinRunner, currentIndex, 4, 4);

                    add(SpriteName.cmdGuardDog, currentIndex, 4, 4);
                    add(SpriteName.cmdCannonTroll, currentIndex, 8, 4);
                    add(SpriteName.cmdGoblinFoodBoss, currentIndex, 4, 4);
                    add(SpriteName.cmdGoblinBloated, currentIndex, 4, 4);

                    add(SpriteName.cmdRoyalGuardArcher, currentIndex, 4, 4);
                    add(SpriteName.cmdRoyalGuardMelee, currentIndex, 4, 4);
                    add(SpriteName.cmdOrcGuard, currentIndex, 4, 4);
                    add(SpriteName.cmdUnitOrc_GoblinSoldier, currentIndex, 4, 4);
                    add(SpriteName.hqHero_Recruit_Bow, currentIndex, 4, 4);
                }

                currentIndex = numTilesWidth * 62;
                {
                    add(SpriteName.cmdBridge, 2, 2);
                    addWithSizeDef(SpriteName.cmdWarningTriangle, currentIndex, 36, 36);
                    //add(SpriteName.cmdLeader, 2, 2);
                    currentIndex += 2;
                    //add(SpriteName.cmdOrderCheckFlat, 2, 2);
                    add(SpriteName.cmdSelectionDotted, 2, 2);
                    add(SpriteName.cmdSelectionFull, 2, 2);
                    add(SpriteName.cmdArrowAttack, 2, 2);
                    add(SpriteName.cmdCCAttack, 2, 2);
                    add(SpriteName.cmdStatsMoveBox, 2, 2);
                    add(SpriteName.cmdStatsMeleeBox, 2, 2);
                    add(SpriteName.cmdStatsHealthBox, 2, 2);
                    add(SpriteName.cmdStatsRangedBox, 2, 2);
                    add(SpriteName.cmdCoin, 2, 2);
                    add(SpriteName.cmd1Honor, 2, 2);
                    add(SpriteName.cmd2Honor, 2, 2);
                    add(SpriteName.cmd3Honor, 2, 2);
                    add(SpriteName.cmdPointer, 2, 2);
                    add(SpriteName.cmdAttackDirectionRed, 2, 2);
                    //currentIndex += 2;
                    //add(SpriteName.cmdCheckOutline, 2, 2);
                    //add(SpriteName.cmdCheckBroken1, 2, 2);
                    //add(SpriteName.cmdCheckBroken2, 2, 2);
                    
                    
                    //add(SpriteName.cmdIconSpecial, 2, 2);
                    //add(SpriteName.cmdIconSupport_1, 2, 2);
                    //add(SpriteName.cmdIconSupport_2, 2, 2);
                    currentIndex += 6;
                    add(SpriteName.cmdIconFollowUp, 2, 2);
                    add(SpriteName.cmdHighlightedSquare, 2, 2);
                    add(SpriteName.BackStabIcon, 2, 2);
                    add(SpriteName.cmdTutVideo_Pointer, 2, 2);
                    add(SpriteName.cmdTutVideo_PointerClick, 2, 2);
                    add(SpriteName.cmdTutVideo_FriendUnit, 2, 2);
                    add(SpriteName.cmdTutVideo_EnemyUnit, 2, 2);
                    add(SpriteName.cmdPointerAttack, 2, 2);
                    // add(SpriteName.cmdWhiteFlatCirkle, 2, 2);
                    //add(SpriteName.cmdCanAttackRangedIcon_Move, 2, 2);

                    // add(SpriteName.cmdCanMoveIcon_Highlight, 2, 2);
                    //add(SpriteName.cmdSupporterIcon1, 2, 2);
                    currentIndex += 2;
                    add(SpriteName.cmdMinusHeart, 2, 2);
                    add(SpriteName.cmdInteractiveSquareFrame_Attack, 2, 2);

                    add(SpriteName.lineofsightEyeCenter, 2, 2);
                    addWithSizeDef(SpriteName.lineofsightEyeButton, currentIndex, 1, 1, 34, 34);
                    addWithSizeDef(SpriteName.lineofsightEyeButtonHighlighted, currentIndex, 1, 1, 34, 34);

                    add(SpriteName.DoomClockFace, 2, 2);
                    add(SpriteName.DoomClockArm, 2, 2);

                    add(SpriteName.cmdInteractiveSquareFrame, 2, 2);


                    //addWithSizeDef(SpriteName.cmdStatsArmorBg, currentIndex, 32, 41);
                    //addWithSizeDef(SpriteName.cmdStatsIconBg, currentIndex, 41, 41);
                    ////currentIndex -= 1;

                    //addWithSizeDef(SpriteName.cmdStatsValueBg, currentIndex, CmdStatsValueBgWidth, CmdStatsValueBgHeight);

                    ////currentIndex += 1;
                    //addWithSizeDef(SpriteName.cmdStatsRangeValueBg, currentIndex, 44, 44);

                    //addWithSizeDef(SpriteName.cmdStatsUnitBg, currentIndex, 48, 48);
                    add(SpriteName.cmdCardPortraitBoxLarge, 2, 2);

                    add(SpriteName.cmdAttackTerrain, 2, 2);
                    add(SpriteName.WebTile, 2, 2);
                    add(SpriteName.hqLeverStick, 2, 2);

                    currentIndex += 1;

                    add(SpriteName.hqBatteResultBobbleDamage, 2, 2);

                    add(SpriteName.hqBatteResultBobblePierce, 2, 2);
                    add(SpriteName.hqBatteResultBobbleBg, 2, 2);

                    currentIndex += 1;
                    add(SpriteName.cmdCard5by1, 10, 2);
                    add(SpriteName.cmdStrategyOrder3, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);
                    add(SpriteName.cmdStrategyDoubletime, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);
                    add(SpriteName.cmdStrategyWideAdv, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);
                    add(SpriteName.cmdStrategyCloseEncount, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);
                    add(SpriteName.cmdStrategyDarkSky, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);
                    add(SpriteName.cmdStrategyRush, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);
                    //add(SpriteName.cmdStrategyBg, CmdStrategyCardSz.X, CmdStrategyCardSz.Y);


                }

                currentIndex = numTilesWidth * 64;
                {
                    add(SpriteName.cmdShieldP2);
                    add(SpriteName.cmdShieldP1);

                    add(SpriteName.cmdStamina);
                    add(SpriteName.cmdBloodRage);
                    add(SpriteName.cmdArmorResult);
                    add(SpriteName.cmdSlotMashineHit);
                    add(SpriteName.cmdSlotMashineRetreat);
                    add(SpriteName.cmdSlotMashineHitCritical);
                    add(SpriteName.cmdPotionHealth);
                    add(SpriteName.cmdPotionStamina);
                    add(SpriteName.cmdStunnIcon);
                    add(SpriteName.cmdStaminaStep);
                    add(SpriteName.cmdAttackTargetMelee2);
                    add(SpriteName.cmdAttackTargetRanged2);
                    add(SpriteName.cmdIconTimeOut);
                    add(SpriteName.cmdSlotMashineMiss);

                    add(SpriteName.cmdCard5by1Small, 5, 1);
                    add(SpriteName.cmdStatsMelee);
                    add(SpriteName.cmdStatsRanged);
                    add(SpriteName.cmdStatsRangedLength);
                    add(SpriteName.cmdStatsHealth);
                    add(SpriteName.cmdStatsMove);
                    addWithSizeDef(SpriteName.cmdMoveLengthHudIcon, currentIndex, 25, 25);

                    //addQtile(SpriteName.cmdStatsMeleeDot, Corner.NW);
                    //addQtile(SpriteName.cmdStatsRangedDot, Corner.NE);
                    addWithSizeDef(SpriteName.cmdUnitMoveGui_Small, currentIndex, 15, 15);
                    currentIndex--;
                    addQtile(SpriteName.BronzeChestOpen, Corner.NE, currentIndex);
                    addQtile(SpriteName.BronzeChestClosed, Corner.SE, currentIndex);

                    //currentIndex++;
                    add(SpriteName.cmdInfoBobble);
                    add(SpriteName.cmdSpyglass);
                    add(SpriteName.cmdUnitMeleeGui);
                    add(SpriteName.cmdUnitRangedGui);

                    add(SpriteName.cmdBattleHistoryButton);
                    add(SpriteName.cmdPhaseCirkle);
                    add(SpriteName.cmdIgnoreRetreat);
                    add(SpriteName.cmdNoDefence);


                    add(SpriteName.cmdSpyglassHighlighted);

                    add(SpriteName.lineofsightEye);

                    //add(SpriteName.lineofsightEyeButton);
                    //add(SpriteName.lineofsightEyeCenter);
                    //currentIndex += 5;
                    add(SpriteName.defaultGamerIcon);
                    add(SpriteName.cmdTileMountBrickWallTorchSprite);

                    addFullQtile(SpriteName.GoldChestOpen, SpriteName.SilverChestOpen, SpriteName.GoldChestClosed, SpriteName.SilverChestClosed, currentIndex);

                    addWithSizeDef(SpriteName.cmdBrokenHeart, currentIndex, 20, 20);
                    //addWithSizeDef(SpriteName.cmdUnitStatusGuiBar, currentIndex, 10, 18);
                    var UnitStatusGuiIndex = currentIndex;
                    addWithSizeDef(SpriteName.cmdUnitStatusGui_Stamina, UnitStatusGuiIndex, 0, 0, 12, 12);
                    addWithSizeDef(SpriteName.cmdUnitStatusGui_Health, UnitStatusGuiIndex, 0, 16, 12, 12);
                    addWithSizeDef(SpriteName.cmdUnitStatusGui_StaminaBar, UnitStatusGuiIndex, 16, 0, 6, 11);
                    addWithSizeDef(SpriteName.cmdUnitStatusGui_HealthBar, UnitStatusGuiIndex, 21, 0, 6, 11);
                    addWithSizeDef(SpriteName.cmdUnitStatusGui_EmptyBar, UnitStatusGuiIndex, 26, 0, 6, 11);

                    currentIndex = UnitStatusGuiIndex + 1;
                    add(SpriteName.cmdMoveArrowDotMy);
                    add(SpriteName.cmdMoveArrowDotStamina);
                    add(SpriteName.cmdMoveArrowDotLocked);
                    add(SpriteName.cmdMoveArrowDotAlly);

                    add(SpriteName.cmdMoveArrowMy);
                    add(SpriteName.cmdMoveArrowStamina);
                    add(SpriteName.cmdMoveArrowLocked);
                    add(SpriteName.cmdMoveArrowAlly);
                    add(SpriteName.cmdClientPointer);
                    add(SpriteName.cmdIcon2Hit);
                    add(SpriteName.cmdIconSurge);

                    add(SpriteName.cmdHitFlatSymbol);
                    add(SpriteName.cmdRetreatFlatSymbol);

                    add(SpriteName.cmdArmor2Result);
                    add(SpriteName.cmdArmorHeavy);
                    add(SpriteName.cmdArmorDodge);
                    add(SpriteName.cmdDodge);
                    addWithSizeDef(SpriteName.cmdPetTargetGui, currentIndex, 25, 25);

                    add(SpriteName.hqLeverBase, 2, 1);
                    add(SpriteName.hqLeverCog);

                    add(SpriteName.hqStrategyArrowRain, 2, 1);
                    add(SpriteName.hqStrategyShieldWall, 2, 1);

                    add(SpriteName.toggTutorialArrow);

                    add(SpriteName.cmdDiceIcon);

                    add(SpriteName.cmdDodgeRanged);
                    add(SpriteName.cmdDodgeLongRanged);
                    add(SpriteName.cmdUnitLongRangedGui);

                    add(SpriteName.cmdDiceMelee);
                    add(SpriteName.cmdDiceRanged);

                    add(SpriteName.cmdDiceArmorLight);
                    add(SpriteName.cmdDiceArmorHeavy);
                    add(SpriteName.cmdDiceAttack);

                    add(SpriteName.cmdDiceDodge);
                    add(SpriteName.cmdArmorLight);

                    add(SpriteName.cmdHeartValueBox, currentIndex, CmdValueBoxTileSz.X, CmdValueBoxTileSz.Y);
                    add(SpriteName.cmdStaminaValueBox, currentIndex, CmdValueBoxTileSz.X, CmdValueBoxTileSz.Y);
                    add(SpriteName.cmdBloodrageValueBox, currentIndex, CmdValueBoxTileSz.X, CmdValueBoxTileSz.Y);

                    add(SpriteName.cmdStaminaGrayed);
                    add(SpriteName.cmdBloodrageGrayed);
                    add(SpriteName.cmdHeartGrayed);
                    add(SpriteName.cmdPierce);
                    add(SpriteName.cmdIcon3Hit);

                    {
                        addWithSizeDef(SpriteName.cmd0, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd1, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd2, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd3, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd4, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd5, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd6, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd7, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd8, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmd9, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmdDiv, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmdPlus, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmdMinus, currentIndex, CmdLetterSz, CmdLetterSz);
                        addWithSizeDef(SpriteName.cmdExclamation, currentIndex, CmdLetterSz, CmdLetterSz);
                    }
                    add(SpriteName.cmdHudSideLabelRed, 2, 1);
                    add(SpriteName.cmdHudSideLabelBlue, 2, 1);
                    add(SpriteName.cmdHudSideLabelLightBlue, 2, 1);

                    addWithSizeDef(SpriteName.cmdHudBorderThick, currentIndex, NineSplitSz, NineSplitSz);
                    addWithSizeDef(SpriteName.cmdHudBorderThin, currentIndex, NineSplitSz, NineSplitSz);
                    addWithSizeDef(SpriteName.cmdHudBorderThinDark, currentIndex, NineSplitSz, NineSplitSz);
                    addWithSizeDef(SpriteName.cmdHudBorderTooltip, currentIndex, NineSplitSz, NineSplitSz);

                    addWithSizeDef(SpriteName.cmdHudTooltipArrow, currentIndex, 15, 15);
                    addWithSizeDef(SpriteName.cmdHudBorderButton, currentIndex, NineSplitSz, NineSplitSz);

                    addWithSizeDef(SpriteName.cmdHudOptionsOn, currentIndex, 17, 17);
                    addWithSizeDef(SpriteName.cmdHudOptionsOff, currentIndex, 17, 17);

                    addWithSizeDef(SpriteName.cmdConvertArrow, currentIndex, 20, 20);

                    addWithSizeDef(SpriteName.cmdIconSurgeSmall, currentIndex, 24, 24);
                    addWithSizeDef(SpriteName.cmdIconBlockSmall, currentIndex, 24, 24);
                    addWithSizeDef(SpriteName.cmdIconHit1Small, currentIndex, 24, 24);

                    addWithSizeDef(SpriteName.cmdHudCheckOff, currentIndex, 24, 24);
                    addWithSizeDef(SpriteName.cmdHudCheckOn, currentIndex, 24, 24);

                    addWithSizeDef(SpriteName.cmdStrategyBg, currentIndex, 1, 1, CmdStrategyCardPixSz.X, CmdStrategyCardPixSz.Y);

                    add(SpriteName.DoomBarFrame);
                }

                currentIndex = numTilesWidth * 65;
                {
                    add(SpriteName.hqBatteResultBobbleShield, 2, 2);
                    add(SpriteName.cmdTileWallIvy1, 2, 2);
                    add(SpriteName.cmdTileGrassBRockModel, 2, 2);
                    add(SpriteName.pjMenuIcon, 2, 2);
                    add(SpriteName.pjForwardIcon, 2, 2);
                    add(SpriteName.pjPlayIcon, 2, 2);
                    addWithSizeDef(SpriteName.cmdUnitGuiArrow, currentIndex, 63, 63);
                    add(SpriteName.cmdAttackDirectionWhite, 2, 2);
                    add(SpriteName.hqSpikeDamageEffect, 2, 2);
                    add(SpriteName.cmdHudBorderPopup, currentIndex, 2, 2);
                    addWithSizeDef(SpriteName.cmdUnitGuiArrow_Enemy, currentIndex, 63, 63);

                    add(SpriteName.cmdUnitCardNameBannerLeft, 1, 2);
                    add(SpriteName.cmdUnitCardNameBannerMid, 1, 2);
                    add(SpriteName.cmdUnitCardNameBannerRight, 1, 2);

                    add(SpriteName.cmdLockMechanical, 2, 2);

                    add(SpriteName.hqTier1Treasure, 2, 2);
                    add(SpriteName.hqTier1TreasureOpen, 2, 2);
                    add(SpriteName.hqTier2Treasure, 2, 2);
                    add(SpriteName.hqTier2TreasureOpen, 2, 2);
                    add(SpriteName.hqTier3Treasure, 2, 2);
                    add(SpriteName.hqTier3TreasureOpen, 2, 2);

                    add(SpriteName.speachBobbleYellow, 1, 2);
                    add(SpriteName.speachBobbleRed, 1, 2);
                    add(SpriteName.speachBobbleWhite, 1, 2);

                    currentIndex += 2;

                    const int SpringTrapSz = 48;
                    addWithSizeDef(SpriteName.hqSpringTrap, currentIndex, 1, 1, SpringTrapSz, SpringTrapSz);
                    addWithSizeDef(SpriteName.hqSpringTrapReleased, currentIndex, 1, 1, SpringTrapSz, SpringTrapSz);

                    add(SpriteName.speachBobbleDarkRed, 1, 2);
                    add(SpriteName.hqRougeTrap, currentIndex, 2, 2);

                    add(SpriteName.cmdLockRuneH, currentIndex, 2, 2);
                    add(SpriteName.cmdLockRuneB, currentIndex, 2, 2);
                    add(SpriteName.cmdLockRuneF, currentIndex, 2, 2);
                    add(SpriteName.cmdLockRuneA, currentIndex, 2, 2);
                    addWithSizeDef(SpriteName.cmdCardPortraitBoxSmall, currentIndex, 48, 48);
                }

                currentIndex = numTilesWidth * 67;
                {
                    add(SpriteName.hqStrategyMoveAttack, 2, 1);
                    add(SpriteName.hqStrategyMoveMove, 2, 1);
                    add(SpriteName.hqStrategyAttackAttack, 2, 1);
                    add(SpriteName.hqStrategyRest, 2, 1);

                    add(SpriteName.DoomSkull);
                    add(SpriteName.DoomSkullGray);
                    add(SpriteName.DoomClockIcon);
                    add(SpriteName.DoomClockIconGray);

                    add(SpriteName.cmdTileMountainwallFloorEdge1, 2, 1);
                    add(SpriteName.cmdTileMountainwallFloorEdge2, 2, 1);
                    add(SpriteName.cmdTileGroundEdgeGrass1, 2, 1);

                    addFullQtile(SpriteName.cmdTileMountainGroundStone1,
                        SpriteName.cmdTileMountainGroundStone2,
                        SpriteName.cmdTileMountainGroundStone3,
                        SpriteName.cmdTileMountainGroundStone4);

                    addQtile(SpriteName.cmdTileMountainGroundStone5, Corner.NW, currentIndex);
                    addQtile(SpriteName.cmdUnitLongRangedGui_Small, Corner.NE, currentIndex);
                    addQtile(SpriteName.cmdAttackDirectionRedArrow, Corner.SW, currentIndex);
                    currentIndex++;

                    add(SpriteName.cmdTileMountainwallFloorEdge3, 2, 1);

                    add(SpriteName.hqStrategyLineOfFire, 2, 1);
                    add(SpriteName.hqStrategySwing3, 2, 1);
                    add(SpriteName.hqStrategyAimedShot, 2, 1);
                    add(SpriteName.hqStrategyPiercingShot, 2, 1);

                    addFullQtile(SpriteName.cmdIconHourglassSmall, SpriteName.cmdIconStaminaSmall,
                        SpriteName.cmdIconHourglassSmallGray, SpriteName.cmdIconStaminaSmallGray, currentIndex);

                    addFullQtile(SpriteName.cmdIconBloodrageSmall, SpriteName.cmdUnitMeleeGui_Small,
                        SpriteName.cmdIconBloodrageSmallGray, SpriteName.cmdUnitRangedGui_Small, currentIndex);

                    add(SpriteName.KillMarkSkullflower);
                    add(SpriteName.KillMarkSwordNecklace);

                    var indexStored = currentIndex;
                    addWithSizeDef(SpriteName.DamageSlashEffect1, currentIndex, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);
                    addWithSizeDef(SpriteName.DamageSlashEffect2, indexStored, 0, DamageSlashEffectSz.Y, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);
                    indexStored = currentIndex;
                    addWithSizeDef(SpriteName.DamageSlashEffect3, currentIndex, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);
                    addWithSizeDef(SpriteName.DamageSlashEffect4, indexStored, 0, DamageSlashEffectSz.Y, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);
                    indexStored = currentIndex;
                    addWithSizeDef(SpriteName.DamageSlashEffect5, currentIndex, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);
                    addWithSizeDef(SpriteName.DamageSlashEffect6, indexStored, 0, DamageSlashEffectSz.Y, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);
                    indexStored = currentIndex;
                    addWithSizeDef(SpriteName.DamageSlashEffect7, currentIndex, DamageSlashEffectSz.X, DamageSlashEffectSz.Y);

                    add(SpriteName.FurnishBox);
                    add(SpriteName.FurnishBarrel);
                    add(SpriteName.FurnishRat);
                    add(SpriteName.FurnishRatRunning);
                    add(SpriteName.FurnishSparrow);
                    add(SpriteName.FurnishSparrowFlying);

                    int doorIx = currentIndex;
                    addWithSizeDef(SpriteName.hqDoorHoleOutLine, doorIx, 1, 1, 46, 8);
                    addWithSizeDef(SpriteName.hqDoorOutLine, doorIx, 1, 17, 46, 8);
                    //currentIndex++;

                    addWithSizeDef(SpriteName.cmdHudPopupButton, currentIndex, NineSplitSz, NineSplitSz);
                    addWithSizeDef(SpriteName.cmdHudBorderPopupButton, currentIndex, NineSplitSz, NineSplitSz);
                    addWithSizeDef(SpriteName.cmdHudBorderThickButton, currentIndex, NineSplitSz, NineSplitSz);

                    add(SpriteName.cmdArrowPierce);
                    add(SpriteName.cmdHudCross);
                    add(SpriteName.equipHand1);
                    add(SpriteName.equipHand2);
                    add(SpriteName.equipArmor);
                    add(SpriteName.equipTrinket);
                    add(SpriteName.cmdBackpack);
                    add(SpriteName.cmdItemPouch);

                    addFullQtile(SpriteName.cmdUnitMeleeGui_EnemySmall,
                        SpriteName.cmdUnitMoveGui_EnemySmall,
                        SpriteName.cmdUnitRangedGui_EnemySmall,
                        SpriteName.cmdUnitLongRangedGui_EnemySmall);

                    add(SpriteName.RedErrorCross);
                    add(SpriteName.cmdArrowRegular);

                    add(SpriteName.cmdUnitCardPropertyBox, 4, 1);

                    add(SpriteName.cmdUnitCardArmorBg);
                    add(SpriteName.cmdUnitCardSkillBg, 2, 1);
                    add(SpriteName.cmdUnitCardRangeBg, 2, 1);

                    add(SpriteName.cmdBerserkerAxe);
                    add(SpriteName.cmdElfBow);
                    add(SpriteName.cmdStartBow);
                    add(SpriteName.cmdStartSword);
                    add(SpriteName.cmdKnightSword);
                    add(SpriteName.cmdWhip);
                    add(SpriteName.cmdStartDagger);
                    add(SpriteName.cmdArcherDagger);
                    add(SpriteName.cmdSpear);
                    add(SpriteName.cmdThrowKnife);
                    add(SpriteName.cmdProtectionRune);
                    add(SpriteName.cmdWaterBottle);
                    add(SpriteName.cmdPotionPheonix);
                    add(SpriteName.cmdRoundShield);
                    add(SpriteName.cmdLeatherArmor);
                    add(SpriteName.cmdMailArmor);
                    add(SpriteName.cmdElfArmor);
                    add(SpriteName.cmdApple);

                    add(SpriteName.cmdGrapple);
                    add(SpriteName.cmdImmobile);
                    add(SpriteName.cmdMoveToSlot);
                    add(SpriteName.cmdAttackUp);
                    add(SpriteName.cmdAttackDown);
                    add(SpriteName.cmdMoveUp);
                    add(SpriteName.cmdMoveDown);
                    add(SpriteName.cmdUndeadIcon);
                    add(SpriteName.cmdMonsterCommander);
                    add(SpriteName.cmdRegenrate);
                    add(SpriteName.cmdRetaliate);
                    add(SpriteName.cmdParry);
                    add(SpriteName.cmdFlying);
                    add(SpriteName.cmdFrenzy);
                    add(SpriteName.cmdPush);
                    add(SpriteName.cmdPull);
                    add(SpriteName.cmdSwing);
                    add(SpriteName.cmdDarkHeal);
                    add(SpriteName.cmdPoisionIcon);
                    add(SpriteName.cmdStaminaDrain);
                    add(SpriteName.cmdWebbed);
                    add(SpriteName.cmdPullLever);
                    add(SpriteName.cmdLightAction);
                    add(SpriteName.cmdNoMove);
                    add(SpriteName.cmdDullWeapon);
                    add(SpriteName.cmdThrowIcon);
                    add(SpriteName.cmdDefencelessIcon);
                    add(SpriteName.cmdAddToBackpack);

                    addWithSizeDef(SpriteName.cmdHudBorderButtonGray, currentIndex, NineSplitSz, NineSplitSz);

                }

                currentIndex = numTilesWidth * 68;
                {
                    add(SpriteName.cmdTileGrassBTreeModel, 2, 3);
                    addWithSizeDef(SpriteName.DoomBanner, currentIndex, DoomBannerSize.X, DoomBannerSize.Y);
                    addWithSizeDef(SpriteName.DoomBannerHighlight, currentIndex, DoomBannerSize.X, DoomBannerSize.Y);

                    add(SpriteName.hqCampFireOff, 3, 3);
                    add(SpriteName.hqCampFireOn, 3, 3);

                    addWithSizeDef(SpriteName.QuestBanner, currentIndex, DoomBannerSize.X, DoomBannerSize.Y);
                    addWithSizeDef(SpriteName.QuestBannerHighlight, currentIndex, DoomBannerSize.X, DoomBannerSize.Y);
                    add(SpriteName.hqAlarmBell, 2, 3);

                    add(SpriteName.WarsRelationFlagOutline, 2, 3);
                    add(SpriteName.WarsRelationFlag, 2, 3);                            
                }
            }

            currentIndex = numTilesWidth * 71;
            {
                addWithSizeDef(SpriteName.DlcBoxPrimeLarge, currentIndex, 1, 1, 197, 197);
                add(SpriteName.birdToasty, currentIndex, 6, 7);

                addWithSizeDef(SpriteName.cballEdgeTexture, currentIndex, 1, 1, 192, 192);
            }

            currentIndex = numTilesWidth * 78;
            {
                add(SpriteName.pjTitleText, currentIndex, PjTitleTextSz.X, PjTitleTextSz.Y);
                add(SpriteName.cballGoalBlue, CballGoalTextSz.X, CballGoalTextSz.Y);
                add(SpriteName.cballGoalRed, CballGoalTextSz.X, CballGoalTextSz.Y);

                add(SpriteName.birdWolfScare, currentIndex, 6, 5);

                add(SpriteName.hqRabidLizard, currentIndex, 6, 5);
                add(SpriteName.hqNagaBoss, currentIndex, 5, 5);
                add(SpriteName.cmdGoblinKnight, currentIndex, 5, 5);
            }

            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                currentIndex = numTilesWidth * 83;
                {
                    //add(SpriteName.rtsAIFlaySymbol);
                    currentIndex += 26;
                    //add(SpriteName.rtsBBTree);
                    //add(SpriteName.rtsBBStone);
                    add(SpriteName.rtsBattleHistory,2,2);
                    add(SpriteName.rtsAllianceIcon, 2, 2);
                    add(SpriteName.rtsRelationNeurtal, 2, 2);
                    add(SpriteName.rtsIncome, 2, 2);
                    add(SpriteName.rtsUpkeep, 2, 2);
                    add(SpriteName.rtsMoney, 2, 2);
                    add(SpriteName.rtsHonor, 2, 2);
                    add(SpriteName.rtsAPbar, currentIndex, 4, 1);
                    add(SpriteName.rtsIncomeSum, 2, 2);

                    add(SpriteName.rtsCityLink);
                    add(SpriteName.rtsArmyLink);
                    add(SpriteName.rtsWarningPopBG);
                    add(SpriteName.rtsWarningPopBgArrow);
                    add(SpriteName.rtsWarningPopReady);
                    add(SpriteName.rtsWarningPopExpression);
                    add(SpriteName.rtsHomeIcon);
                    add(SpriteName.rtsShipRemove);
                    add(SpriteName.rtsShipBuild);
                    add(SpriteName.rtsShip);
                    add(SpriteName.rtsRelationAllied);
                    add(SpriteName.rtsRelationAtWar);
                    add(SpriteName.rtsRelationNemesis);

                    add(SpriteName.rtsLoading0of8);
                    add(SpriteName.rtsLoading1of8);
                    add(SpriteName.rtsLoading2of8);
                    add(SpriteName.rtsLoading3of8);
                    add(SpriteName.rtsLoading4of8);
                    add(SpriteName.rtsLoading5of8);
                    add(SpriteName.rtsLoading6of8);
                    add(SpriteName.rtsLoading7of8);
                    add(SpriteName.rtsLoading8of8);

                    add(SpriteName.rtsFactionFlagIcon);

                    add(SpriteName.rtsApDotFull);
                    add(SpriteName.rtsApDotHalf);
                    add(SpriteName.rtsApDotEmpty);
                    add(SpriteName.rtsDisbandArmy);
                    add(SpriteName.rtsAnalyzeBattle);

                    add(SpriteName.rtsBattleXpMeter, currentIndex, 4, 2);
                    add(SpriteName.rtsFatigueMeter, currentIndex, 4, 2);
                    add(SpriteName.rtsBattleIcon);
                }


                //currentIndex = numTilesWidth * 85;
                //{
                //    add(SpriteName.rtsCardBg, currentIndex, DSSCardSz.X, DSSCardSz.Y);
                //}
            }

            currentIndex = numTilesWidth * 90;
            {
                add(SpriteName.WarsBuild_School, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_PostalLevel2, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_PostalLevel3, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_RecruitmentLevel2, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_RecruitmentLevel3, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_GoldDeliver, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_GoldDeliverLevel2, currentIndex, 3, 3);
                add(SpriteName.WarsBuild_GoldDeliverLevel3, currentIndex, 3, 3);

                add(SpriteName.WarsFlagType_LongBanner, 3, 3);
                add(SpriteName.WarsFlagType_Banner, 3, 3);
                add(SpriteName.WarsFlagType_SlimBanner, 3, 3);

                add(SpriteName.WarsFlagType_Flag, 3, 3);
                add(SpriteName.WarsFlagType_FlagRound, 3, 3);
                add(SpriteName.WarsFlagType_FlagLarge, 3, 3);
                add(SpriteName.WarsFlagType_Streamer, 3, 3);
                add(SpriteName.WarsFlagType_Triangle, 3, 3);
                add(SpriteName.WarsBuild_GardenFourBushes, 3, 3);
                add(SpriteName.WarsBuild_GardenLongTree, 3, 3);
                add(SpriteName.WarsBuild_GardenWalledBush, 3, 3);
                add(SpriteName.WarsBuild_GardenMemoryStone, 3, 3);
                add(SpriteName.WarsBuild_GardenBird, 3, 3);
                add(SpriteName.WarsBuild_PavementLamp, 3, 3);
                add(SpriteName.WarsBuild_PavemenFountain, 3, 3);
                add(SpriteName.WarsBuild_PavementRectFlower, 3, 3);
                add(SpriteName.WarsBuild_StoneWall, 3, 3);
                add(SpriteName.WarsBuild_StoneWallGreen, 3, 3);
                add(SpriteName.WarsBuild_StoneWallBlueRoof, 3, 3);
                add(SpriteName.WarsBuild_StoneWallWoodHouse, 3, 3);
                add(SpriteName.WarsBuild_StoneTower, 3, 3);
                add(SpriteName.WarsBuild_StoneGate, 3, 3);
                add(SpriteName.WarsBuild_StoneHouse, 3, 3);
                add(SpriteName.WarsBuild_GardenGrass, 3, 3);
                add(SpriteName.WarsBuild_Statue_Lion, 3, 3);
                add(SpriteName.WarsBuild_Statue_Leader, 3, 3);
                add(SpriteName.WarsBuild_Statue_Horse, 3, 3);
                add(SpriteName.WarsBuild_Statue_Pillar, 3, 3);
            }

            if (PlatformSettings.RunProgram == StartProgram.ToGG ||
                PlatformSettings.RunProgram == StartProgram.DSS)
            {
                currentIndex = numTilesWidth * 93;
                {
                    add(SpriteName.unitEmoteSmile);
                    add(SpriteName.unitEmoteAngry);
                    add(SpriteName.unitEmoteSnore);
                    add(SpriteName.unitEmoteCry);
                    add(SpriteName.unitEmoteBitter);
                    add(SpriteName.unitEmoteLaugh);
                    add(SpriteName.unitEmoteWink);
                    add(SpriteName.unitEmoteEvil);
                    add(SpriteName.unitEmoteThumbUp);
                    add(SpriteName.unitEmoteThumbDown);
                    add(SpriteName.unitEmoteLove);
                    add(SpriteName.unitEmoteQuestion);

                    add(SpriteName.speachBobbleTexture);
                    add(SpriteName.speachBobbleArrow);

                    add(SpriteName.cmdInactiveMonster);
                    add(SpriteName.unitEmoteAlerted);

                    add(SpriteName.equipQuickbelt);
                    add(SpriteName.hqStrategyLootRun, 2, 1);
                    add(SpriteName.hqStrategyPoisionBomb, 2, 1);
                    add(SpriteName.hqStrategyLeapAttack, 2, 1);
                    add(SpriteName.hqStrategyTrapper, 2, 1);

                    add(SpriteName.cmdRemoveTrap);
                    add(SpriteName.cmdLootAction);

                    add(SpriteName.cmdActionTargetSeletion);
                    add(SpriteName.cmdThrowBombAction);
                    add(SpriteName.cmdBleedIcon);
                    add(SpriteName.cmdThiefDaggersTier2);
                    add(SpriteName.cmdThiefDaggersTier1);
                    add(SpriteName.cmdPosionousDarts);

                    add(SpriteName.cmdHiddenIcon);
                    add(SpriteName.cmdSmokeBomb);

                    add(SpriteName.cmdLootFinder);
                    add(SpriteName.cmdDeliveryService);
                    add(SpriteName.cmdLootCollector);

                    add(SpriteName.hqStrategyKnifeDance, 2, 1);
                    add(SpriteName.KillMarkGraveStone);
                    add(SpriteName.KillMarkRose);
                    add(SpriteName.cmdAiBrain);
                    add(SpriteName.cmdAiBrainObjective);
                    add(SpriteName.cmdAiBrainObjectiveAlways);
                    add(SpriteName.cmdSlipThrough);
                    add(SpriteName.cmdMapEntranceIcon);

                    add(SpriteName.cmdKeyRuneH);
                    add(SpriteName.cmdKeyRuneB);
                    add(SpriteName.cmdKeyRuneF);
                    add(SpriteName.cmdKeyRuneA);

                    add(SpriteName.hqStrategyStunBomb, 2, 1);
                    add(SpriteName.hqStrategyHeroic, 2, 1);
                    add(SpriteName.hqStrategyEarthShake, 2, 1);

                    add(SpriteName.cmdBark);
                    add(SpriteName.cmdCarryFood);
                    add(SpriteName.cmdTarget2);
                    add(SpriteName.cmdTarget3);

                    add(SpriteName.toggPropertyTex);
                    add(SpriteName.toggConditionPositiveTex);
                    add(SpriteName.toggConditionNegativeTex);
                    add(SpriteName.toggConditionNeutralTex);

                    add(SpriteName.cmdUnitMoveGui);
                    add(SpriteName.cmdOrderCheckFlat);
                    add(SpriteName.cmdOrderCheckOutline);
                    add(SpriteName.cmdSupporterIcon1);
                    add(SpriteName.cmdSupporterIcon2);

                }
            }


            //BOTTOM
            if (PlatformSettings.RunProgram == StartProgram.DSS)
            {
                currentIndex = numTilesWidth * 94;
                {
                    add(SpriteName.WarsRelationNeutral, 2, 2);
                    add(SpriteName.WarsRelationPeace, 2, 2);
                    add(SpriteName.WarsRelationAlly, 2, 2);
                    add(SpriteName.WarsRelationGood, 2, 2);
                    add(SpriteName.WarsRelationTruce, 2, 2);
                    add(SpriteName.WarsStrengthIcon, 2, 2);
                    add(SpriteName.WarsGroupIcon, 2, 2);
                    add(SpriteName.WarsSoldierIcon, 2, 2);
                    add(SpriteName.WarsRelationWar, 2, 2);
                    add(SpriteName.WarsRelationTotalWar, 2, 2);
                    add(SpriteName.rtsIncomeTime, 2, 2);
                    add(SpriteName.rtsUpkeepTime, 2, 2);

                    add(SpriteName.WarsUnitIcon_Soldier, 2, 2);
                    add(SpriteName.WarsUnitIcon_Archer, 2, 2);
                    add(SpriteName.WarsUnitIcon_Ballista, 2, 2);
                    add(SpriteName.WarsUnitIcon_Knight, 2, 2);
                    add(SpriteName.WarsUnitIcon_Honorguard, 2, 2);
                    add(SpriteName.WarsUnitIcon_Pikeman, 2, 2);
                    add(SpriteName.WarsUnitIcon_Trollcannon, 2, 2);
                    add(SpriteName.WarsUnitIcon_Viking, 2, 2);
                    add(SpriteName.WarsUnitIcon_Greensoldier, 2, 2);
                    add(SpriteName.WarsUnitIcon_Folkman, 2, 2);

                    addWithSizeDef(SpriteName.TextureHueSaturation, currentIndex, 1, 1, 45, 45);
                    addWithSizeDef(SpriteName.TextureDarknessGradient, currentIndex, 1, 1, 45, 45);

                    add(SpriteName.LangButton_English, 8, 2);
                    add(SpriteName.LangButton_Russian, 8, 2);
                    add(SpriteName.LangButton_Spanish, 8, 2);
                    add(SpriteName.LangButton_Portuguese, 8, 2);
                    add(SpriteName.LangButton_German, 8, 2);
                    add(SpriteName.LangButton_Japanese, 8, 2);
                    add(SpriteName.LangButton_Frensh, 8, 2);
                    add(SpriteName.LangButton_Chinese, 8, 2);
                    add(SpriteName.WarsBluePrint, 2, 2);
                    add(SpriteName.WarsUnitIcon_TwoHand, 2, 2);
                    add(SpriteName.WarsBattleIcon, 2, 2);
                    add(SpriteName.WarsWorker, 2, 2);
                    add(SpriteName.WarsWorkerAdd, 2, 2);
                    add(SpriteName.WarsWorkerSub, 2, 2);
                    add(SpriteName.WarsBogIron, 2, 2);
                    add(SpriteName.WarsUnitIcon_Hammerknight, 2, 2);
                }

                currentIndex = numTilesWidth * 96;
                {
                    add(SpriteName.WarsDiplomaticPoint);
                    add(SpriteName.WarsCommandPoint);
                    add(SpriteName.WarsDiplomaticAdd);
                    add(SpriteName.WarsCommandAdd);
                    add(SpriteName.WarsDiplomaticAddTime);
                    add(SpriteName.WarsCommandAddTime);
                    add(SpriteName.WarsDiplomaticSub);
                    add(SpriteName.WarsCommandSub);
                    add(SpriteName.WarsDiplomaticSubTime);
                    add(SpriteName.WarsCommandSubTime);

                    add(SpriteName.WarsGuard);
                    add(SpriteName.WarsGuardAdd);
                    add(SpriteName.WarsHammer);
                    add(SpriteName.WarsHammerAdd);
                    add(SpriteName.WarsHammerSub);

                    add(SpriteName.WarsDarkLordBossIcon);
                    add(SpriteName.WarsFactoryIcon);
                    add(SpriteName.ColorPickerCircle);
                    add(SpriteName.WarsNightmareEyeGlow);
                    currentIndex += 1;
                    add(SpriteName.WarsResource_IronArmor);
                     add(SpriteName.WarsResource_MithrilArmor);
                     add(SpriteName.WarsResource_BronzeArmor);
                    
                    add(SpriteName.WarsResource_Sharpstick);
                    add(SpriteName.WarsResource_Sword);
                    add(SpriteName.WarsResource_MithrilSword);
                    add(SpriteName.WarsResource_Bow);
                    add(SpriteName.WarsResource_Longbow);
                    add(SpriteName.WarsResource_Mithrilbow);
                    currentIndex += 6;
                    add(SpriteName.WarsDelivery);
                    add(SpriteName.WarsTrade);
                    add(SpriteName.WarsResource_Beer);
                    add(SpriteName.WarsResource_Iron);
                    add(SpriteName.WarsResource_Silver);
                    add(SpriteName.WarsResource_Copper);
                    add(SpriteName.WarsResource_Gold);
                    add(SpriteName.WarsResource_Mithril);
                    currentIndex += 5;
                    add(SpriteName.WarsResource_SkinAndLinen);
                    currentIndex += 5;
                    add(SpriteName.WarsResource_Stone);
                    currentIndex += 2;
                    add(SpriteName.WarsResource_RawMeat);
                    currentIndex += 5;
                    add(SpriteName.WarsHome);
                    add(SpriteName.WarsResource_Water);
                    add(SpriteName.WarsResource_WaterAdd);
                    add(SpriteName.WarsResource_Wood);
                    add(SpriteName.WarsResource_Food);
                    add(SpriteName.WarsResource_RawFood);
                    add(SpriteName.WarsResource_Linen);
                    add(SpriteName.WarsResource_Wheat);
                    add(SpriteName.WarsResource_Egg);
                    add(SpriteName.WarsResource_PaddedArmor);
                    add(SpriteName.WarsResource_FullPlateArmor);
                    add(SpriteName.WarsResource_IronOre);
                    add(SpriteName.WarsResource_GoldOre);
                    add(SpriteName.WarsResource_Ballista);
                    add(SpriteName.WarsResource_TwoHandSword);
                    add(SpriteName.WarsResource_KnightsLance);
                    add(SpriteName.WarsResource_Fuel);

                    add(SpriteName.WarsStockpileAdd);
                    add(SpriteName.WarsStockpileStop);
                    add(SpriteName.WarsResource_LinenCloth);

                    add(SpriteName.WarsProtectedStockpileOn);
                    add(SpriteName.WarsProtectedStockpileOff);
                    add(SpriteName.WarsStockpileAdd_Protected);
                    add(SpriteName.warsCheckYes);
                    add(SpriteName.warsCheckNo);
                    add(SpriteName.WarsCultureIcon);
                    add(SpriteName.WarsResource_FoodEmpty);
                    add(SpriteName.WarsBedIcon);
                    currentIndex += 1;
                    add(SpriteName.warsBulletPoint);
                    currentIndex++;
                    
                    add(SpriteName.WarsMapIcon);
                    add(SpriteName.WarsGovernmentIcon);
                    add(SpriteName.WarsResource_FoodAdd);
                    add(SpriteName.WarsResource_FoodSub);

                    add(SpriteName.WarsWorkMove);
                    add(SpriteName.WarsWorkCollect);
                    add(SpriteName.WarsWorkMine);
                    add(SpriteName.WarsWorkFarm);
                    add(SpriteName.WarsFollowFactionYes);
                    add(SpriteName.WarsFollowFactionNo);
                    add(SpriteName.WarsUnitLevelLegend);
                    add(SpriteName.WarsUnitLevelMaster);
                    add(SpriteName.WarsUnitLevelProfessional);
                    add(SpriteName.WarsUnitLevelSkillful);
                    add(SpriteName.WarsUnitLevelBasic);
                    add(SpriteName.WarsUnitLevelMinimal);

                    add(SpriteName.WarsResource_Rapeseed);
                    add(SpriteName.WarsResource_Hemp);


                    add(SpriteName.warsFolder_carton);
                    add(SpriteName.warsFolder_white);
                    add(SpriteName.warsFolder_orange);
                    add(SpriteName.warsFolder_yellow);
                    add(SpriteName.warsFolder_green);
                    add(SpriteName.warsFolder_pink);
                    add(SpriteName.warsFolder_blue);
                    add(SpriteName.warsFolder_cyan);

                    add(SpriteName.warsResourceChunkAvailable);
                    add(SpriteName.warsResourceChunkNotAvailable);

                }

                currentIndex = numTilesWidth * 97;
                {
                    add(SpriteName.WarsBuild_Barracks, 3, 3);
                    add(SpriteName.WarsBuild_Brewery, 3, 3);
                    add(SpriteName.WarsBuild_Tavern, 3, 3);
                    add(SpriteName.WarsBuild_LinenFarms, 3, 3);
                    add(SpriteName.WarsBuild_WheatFarms, 3, 3);
                    add(SpriteName.WarsBuild_WorkerHuts, 3, 3);
                    add(SpriteName.WarsBuild_Smith, 3, 3);
                    add(SpriteName.WarsBuild_Cook, 3, 3);
                    add(SpriteName.WarsBuild_Statue, 3, 3);
                    add(SpriteName.WarsBuild_HenPen, 3, 3);
                    add(SpriteName.WarsBuild_PigPen, 3, 3);
                    currentIndex += 3;
                    add(SpriteName.WarsBuild_Postal, 3, 3);
                    add(SpriteName.WarsBuild_Recruitment, 3, 3);
                    add(SpriteName.WarsBuild_Nobelhouse, 3, 3);
                    add(SpriteName.WarsBuild_Carpenter, 3, 3);
                    add(SpriteName.WarsBuild_WorkBench, 3, 3);
                    add(SpriteName.WarsBuild_CoalPit, 3, 3);
                    add(SpriteName.WarsBuild_Pavement, 3, 3);
                    add(SpriteName.WarsBuild_PavementFlowers, 3, 3);
                    add(SpriteName.WarsBuild_Bank, 3, 3);
                    add(SpriteName.WarsBuild_Storehouse, 3, 3);
                    add(SpriteName.WarsCityHall, 3, 3);
                    add(SpriteName.WarsBuild_RapeseedFarms, 3, 3);
                    add(SpriteName.WarsBuild_HempFarms, 3, 3);

                    add(SpriteName.WarsBuild_Logistics, 3, 3);
                    add(SpriteName.WarsBuild_Smelter, 3, 3);
                    add(SpriteName.WarsBuild_WoodCutter, 3, 3);
                    add(SpriteName.WarsBuild_StoneCutter, 3, 3);
                    add(SpriteName.WarsBuild_Embassy, 3, 3);
                    add(SpriteName.WarsBuild_WaterReservoir, 3, 3);
                    add(SpriteName.WarsBuild_KnightBarrack, 3, 3);
                    add(SpriteName.WarsBuild_Foundry, 3, 3);
                    add(SpriteName.WarsBuild_Chemist, 3, 3);

                    add(SpriteName.WarsBuild_Armory, 3, 3);
                    add(SpriteName.WarsBuild_Gunmaker, 3, 3);
                    add(SpriteName.WarsBuild_Coinminter, 3, 3);

                    add(SpriteName.WarsBuild_SoldierBarracks, 3, 3);
                    add(SpriteName.WarsBuild_ArcherBarracks, 3, 3);
                    add(SpriteName.WarsBuild_WarmashineBarracks, 3, 3);
                    add(SpriteName.WarsBuild_GunBarracks, 3, 3);
                    add(SpriteName.WarsBuild_CannonBarracks, 3, 3);


                    
                    //add(SpriteName.WarsBuild_SmallCityHouse, 3, 3);
                    //add(SpriteName.WarsBuild_BigCityHouse, 3, 3);
                    //add(SpriteName.WarsBuild_CitySquare, 3, 3);
                    //add(SpriteName.WarsBuild_CobbleStones, 3, 3);






                }

                currentIndex = numTilesWidth * 100;
                {
                    add(SpriteName.WarsSpecializeField);
                    add(SpriteName.WarsSpecializeSea);
                    add(SpriteName.WarsSpecializeAntiCavalry);
                    add(SpriteName.WarsSpecializeSiege);
                    add(SpriteName.warsArmyTag_Lightning);
                    add(SpriteName.warsArmyTag_Fire);
                    add(SpriteName.warsArmyTag_Shield);
                    add(SpriteName.warsArmyTag_Hit);
                    add(SpriteName.warsArmyTag_Retreat);
                    add(SpriteName.warsArmyTag_HitExpress);
                    add(SpriteName.warsArmyTag_GoldShield);
                    add(SpriteName.WarsSpecializeTradition);
                    add(SpriteName.warsArmyTag_Return);
                    add(SpriteName.warsArmyTag_RoundShield);
                    add(SpriteName.warsArmyTag_BrokenShield);   
                    add(SpriteName.warsArmyTag_Anchor);

                    add(SpriteName.WarsResource_Crossbow);
                    add(SpriteName.WarsResource_Sulfur);
                    add(SpriteName.WarsResource_BlackPowder);
                    add(SpriteName.WarsResource_Lead);
                    add(SpriteName.WarsResource_LeadOre);
                    add(SpriteName.WarsResource_TinOre);
                    add(SpriteName.WarsResource_Tin);
                    add(SpriteName.WarsWoodCraftIcon);
                    add(SpriteName.WarsResource_Wagon2Wheel);
                    add(SpriteName.WarsResource_Wagon4Wheel);
                    add(SpriteName.WarsResource_ShortSword);
                    add(SpriteName.WarsResource_Longsword);
                    add(SpriteName.WarsResource_Steel);
                    add(SpriteName.WarsResource_HeavyIronArmor);
                    add(SpriteName.WarsResource_LightPlateArmor);
                    add(SpriteName.WarsResource_BloomeryIron);
                    add(SpriteName.WarsResource_MithrilAlloy);
                    add(SpriteName.WarsResource_Bronze);
                    add(SpriteName.WarsResource_SilverOre);
                    add(SpriteName.WarsResource_Ox);
                    add(SpriteName.WarsResource_BronzeSword);
                    add(SpriteName.WarsResource_HeavyPaddedArmor);
                    add(SpriteName.WarsResource_ThrowSpear);
                    add(SpriteName.WarsResource_Slingshot);
                    add(SpriteName.WarsResource_BronzeShotgun);
                    add(SpriteName.WarsResource_BronzeRifle);
                    add(SpriteName.WarsResource_IronShotgun);
                    add(SpriteName.WarsResource_IronRifle);
                    add(SpriteName.WarsResource_BronzeManCannon);
                    add(SpriteName.WarsResource_IronManCannon);
                    add(SpriteName.WarsResource_BronzeSiegeCannon);
                    add(SpriteName.WarsResource_IronSiegeCannon);
                    add(SpriteName.WarsResource_Warhammer);
                    add(SpriteName.WarsResource_Catapult);
                    add(SpriteName.WarsResource_Trebuchet);
                    add(SpriteName.WarsResource_CastIron);
                    add(SpriteName.WarsResource_Manuballista);
                    add(SpriteName.WarsResource_Bullets);
                    add(SpriteName.WarsResource_GunPowder);
                    add(SpriteName.WarsResource_CopperOre);
                    add(SpriteName.WarsResource_Toolkit);
                    add(SpriteName.WarsResource_HandSpear);
                    add(SpriteName.WarsResource_CoolingFluid);
                    add(SpriteName.WarsTechnology_Unlocked);
                    add(SpriteName.WarsTechnology_Locked);
                    add(SpriteName.WarsFletcherArrowIcon);
                    add(SpriteName.WarsWorkSmelting);
                    add(SpriteName.WarsIncreaseArrowUp);
                    add(SpriteName.WarsDecreaseArrowDown);

                    add(SpriteName.WarsHudScrollerSlider);
                    add(SpriteName.WarsHudScrollerBg);
                    add(SpriteName.WarsHudTabSelected);
                    add(SpriteName.WarsHudTabNotSelected);
                    add(SpriteName.WarsHudPrimaryButton);
                    add(SpriteName.WarsHudPrimaryButtonDisabled);
                    add(SpriteName.WarsHudOptionSelected);
                    add(SpriteName.WarsHudOptionNotSelected);
                    add(SpriteName.WarsHudRoundButton);
                    const int WarsCheckSize = 20;
                    addWithSizeDef(SpriteName.WarsHudCheckYes, currentIndex, WarsCheckSize, WarsCheckSize);
                    addWithSizeDef(SpriteName.WarsHudCheckNo, currentIndex, WarsCheckSize, WarsCheckSize);
                    addWithSizeDef(SpriteName.WarsHudOptionYes, currentIndex, WarsCheckSize, WarsCheckSize);
                    addWithSizeDef(SpriteName.WarsHudOptionNo, currentIndex, WarsCheckSize, WarsCheckSize);
                    add(SpriteName.WarsHudMenuBg);

                    //addWithSizeDef(SpriteName.WarsHudDragButtonLeft, currentIndex, TileHalfSize, TileSize);
                    add(SpriteName.WarsHudDragButton, 2, 1);
                    //addWithSizeDef(SpriteName.WarsHudDragButtonRight, currentIndex, TileHalfSize, TileSize);
                    
                    add(SpriteName.WarsHudInfoIcon);
                    add(SpriteName.WarsHudHoverArea);

                    add(SpriteName.WarsHudHeadBarButton);
                    add(SpriteName.WarsHudHeadBarPlayIcon);
                    add(SpriteName.WarsHudHeadBarPauseIcon);
                    add(SpriteName.WarsHudHeadBarMenuIcon);
                    add(SpriteName.WarsHudHeadBarSecondaryBg);
                    add(SpriteName.WarsHudToolDownButton);

                    add(SpriteName.WarsHudOutlineButton);
                    add(SpriteName.WarsHudSecondaryButton);
                    add(SpriteName.WarsHudSecondaryButtonDisabled);
                    add(SpriteName.WarsHudHeadBarTabSelected);
                    add(SpriteName.WarsHudHeadBarTabNotSelected);

                    add(SpriteName.WarsHudDropDownArrow);
                    add(SpriteName.WarsHudYellowDot);
                    add(SpriteName.WarsHudBrownDot);
                    add(SpriteName.WarsHudRoundButtonNotSelected);
                    add(SpriteName.WarsHudListArrowSelected);
                    add(SpriteName.WarsHudListArrowDefault);
                    add(SpriteName.WarsHudListArrowNotSelected);

                    add(SpriteName.WarsHudIconExit);
                    add(SpriteName.WarsHudIconSettings);
                    add(SpriteName.WarsHudIconAdd);
                    add(SpriteName.WarsHudIconOpen);
                    add(SpriteName.WarsHudIconStart);

                    add(SpriteName.WarsHudMessageBg);
                    add(SpriteName.WarsHudIconEditor);

                    add(SpriteName.WarsHudHeadBarOutlineButton);
                    add(SpriteName.WarsHudSubTabSelected);
                    add(SpriteName.WarsHudSubTabNotSelected);
                    add(SpriteName.WarsHudToggleNotSelected);
                }

                currentIndex = numTilesWidth * 101;
                {
                    add(SpriteName.WarsUnitIcon_Javelin, 2, 2);
                    add(SpriteName.WarsUnitIcon_BronzeShotgun, 2, 2);
                    add(SpriteName.WarsUnitIcon_BronzeRifle, 2, 2);
                    add(SpriteName.WarsUnitIcon_MithrilMan, 2, 2);
                    add(SpriteName.WarsUnitIcon_Longsword, 2, 2);
                    add(SpriteName.WarsUnitIcon_Manuballista, 2, 2);
                    add(SpriteName.WarsUnitIcon_Catapult, 2, 2);
                    add(SpriteName.WarsUnitIcon_BronzeManCannon, 2, 2);
                    add(SpriteName.WarsUnitIcon_BronzeSiegeCannon, 2, 2);
                    add(SpriteName.WarsUnitIcon_IronManCannon, 2, 2);
                    add(SpriteName.WarsUnitIcon_IronSiegeCannon, 2, 2);
                    add(SpriteName.WarsUnitIcon_Slingshot, 2, 2);
                    add(SpriteName.WarsUnitIcon_MithrilArcher, 2, 2);
                    add(SpriteName.WarsUnitIcon_Spearman, 2, 2);
                    add(SpriteName.WarsHudFlagBorder, 2, 2);
                    add(SpriteName.WarsHudHeadBarBg, 2, 2);
                    add(SpriteName.WarsHudToolUpButton, 1, 2);
                    //add(SpriteName.DSS2MainMenu, DSS2Logo.X, DSS2Logo.Y); x = 5
                }

                currentIndex = numTilesWidth * 106;
                {
                    //currentIndex += 2;
                    add(SpriteName.warsFoliageSquareShadow, 2, 2);
                    currentIndex += 2;
                    add(SpriteName.warsFoliageRoundShadow, 2, 2);
                    add(SpriteName.warsFoliageDirtRoad, 2, 2);

                    //add(SpriteName.rtsTownSide, 2, 2);
                    //add(SpriteName.rtsTownTop, 2, 2);
                    //add(SpriteName.rtsBlockSide, 2, 2);
                    //addWithSizeDef(SpriteName.rtsFlagTextureBase, currentIndex, 48, 48);
                    //add(SpriteName.rtsApBarTex, 4, 2);
                }


            }
            if (PlatformSettings.RunProgram == StartProgram.ToGG ||
                PlatformSettings.RunProgram == StartProgram.DSS)
            {

                currentIndex = numTilesWidth * 108;
                {
                    add(SpriteName.cmdTileHouseGround1, 2, 2);
                    add(SpriteName.cmdTileHouseGround2, 2, 2);
                    add(SpriteName.cmdTileHouseGround3, 2, 2);
                    add(SpriteName.cmdTileHouseGround4, 2, 2);
                    add(SpriteName.cmdTileHouseTop, 2, 2);
                    add(SpriteName.cmdTileHouseWall1, 2, 2);
                    add(SpriteName.cmdTileHouseWall2, 2, 2);
                    add(SpriteName.cmdTileHouseWall3, 2, 2);

                    addWithSizeDef(SpriteName.hqRegularDoor, currentIndex, 48, 64);
                }

                currentIndex = numTilesWidth * 110;
                {
                    add(SpriteName.cmdTileMountain_Old, currentIndex, 2, 2);
                    add(SpriteName.cmdTileGrass1, currentIndex, 2, 2);
                    add(SpriteName.cmdTileGrass2, currentIndex, 2, 2);
                    add(SpriteName.cmdTileGrass3, currentIndex, 2, 2);
                    add(SpriteName.cmdTileGrass4, currentIndex, 2, 2);
                    add(SpriteName.cmdTileForest, currentIndex, 2, 2);
                    add(SpriteName.cmdTileWater, currentIndex, 2, 2);
                    add(SpriteName.cmdTileHill, currentIndex, 2, 2);
                    add(SpriteName.cmdTileTown, currentIndex, 2, 2);
                    add(SpriteName.cmdTileEmpty, currentIndex, 2, 2);
                    add(SpriteName.cmdTileRubble, currentIndex, 2, 2);
                    add(SpriteName.cmdTileSwamp, currentIndex, 2, 2);
                    add(SpriteName.cmdTileMountain, currentIndex, 2, 2);

                    addWithSizeDef(SpriteName.cmdTileEdgeGrass, currentIndex, TileSize * 2, CommanderBoardEdgeHeight);
                    add(SpriteName.cmdSlotMashineFrame, currentIndex, 2, 2);

                    add(SpriteName.cmdIconButtonYes, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonNo, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonBack, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonBlank, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonNextPhase, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonNextPhaseGrayed, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonNextPlayer, currentIndex, 2, 2);
                    add(SpriteName.cmdIconButtonNextPlayerGrayed, currentIndex, 2, 2);
                    currentIndex += 4;
                    addWithSizeDef(SpriteName.cmdTutVideo_BoardTile, currentIndex, 48, 48);

                    
                }
            }

            currentIndex = numTilesWidth * 112;
            {
                //add(SpriteName.TutorialBgTex, currentIndex, 7, 6);

                //currentIndex = numTilesWidth * 112 + 7;
                add(SpriteName.warsSceneBg, currentIndex, 6, 6);
                add(SpriteName.BlueprintBg, currentIndex, 9, 5);


                add(SpriteName.hqTutorial1, HqTutorialSz.X, HqTutorialSz.Y);
                add(SpriteName.hqTutorial2, HqTutorialSz.X, HqTutorialSz.Y);
                add(SpriteName.hqTutorial3, HqTutorialSz.X, HqTutorialSz.Y);

                add(SpriteName.warsWorkerPromoHammer, 6, 6);
                add(SpriteName.warsWorkerPromoBox, 6, 6);

                add(SpriteName.warsWorkerPromoCannon, 9, 6);

                add(SpriteName.DSS2MainMenu, DSS2Logo.X, DSS2Logo.Y);
            }

            currentIndex = numTilesWidth * 118;
            {
                add(SpriteName.EditorMultiSelection, currentIndex, 4, 4);

                addWithSizeDef(SpriteName.golfGrassGreen, currentIndex, 1, 1, GolfGrassTexWidth, GolfGrassTexWidth);
                addWithSizeDef(SpriteName.golfGrassBlue, currentIndex, 1, 1, GolfGrassTexWidth, GolfGrassTexWidth);
                addWithSizeDef(SpriteName.golfGrassRed, currentIndex, 1, 1, GolfGrassTexWidth, GolfGrassTexWidth);
                addWithSizeDef(SpriteName.golfGrassYellow, currentIndex, 1, 1, GolfGrassTexWidth, GolfGrassTexWidth);
                addWithSizeDef(SpriteName.golfGrassGray, currentIndex, 1, 1, GolfGrassTexWidth, GolfGrassTexWidth);

                add(SpriteName.WarsGroundTex, 4, 4);
            }
            currentIndex = numTilesWidth * 122;
            add(SpriteName.LFMenuRectangleSelection, currentIndex, 3, 3);
            currentIndex++;
            add(SpriteName.birdGroundTex, currentIndex, 4, 3);

            currentIndex = numTilesWidth * 125;
            {
                add(SpriteName.LFEdge);
                add(SpriteName.InterfaceBorder);
                add(SpriteName.toggDieTexAttackSide);
                add(SpriteName.toggDieTexAttackFront);
                add(SpriteName.toggDieTexMeleeAttackFront);
                add(SpriteName.toggDieTexRangeAttackFront);


                add(SpriteName.hqDieTexSide);
                add(SpriteName.hqDieTexCritiqualHit);
                add(SpriteName.hqDieTexHit1);
                add(SpriteName.hqDieTexHit2);
                add(SpriteName.hqDieTexHit3);
                add(SpriteName.hqDieTexSurge1);
                add(SpriteName.hqDieTexMiss);

                add(SpriteName.hqDieTexDefenceSide);
                add(SpriteName.hqDieTexDefenceDodgeFront);
                add(SpriteName.hqDieTexDefenceArmorFront);
                add(SpriteName.hqDieTexDefenceHeavyArmorFront);
                add(SpriteName.hqDieTexBlock1);
                add(SpriteName.hqDieTexBlock2);
                add(SpriteName.hqDieTexDodge);
                add(SpriteName.hqDieTexDodgeRanged);
                add(SpriteName.hqDieTexDodgeLongRange);
                add(SpriteName.hqDieTexSurge2);

                add(SpriteName.cmdDieTexSide);
                add(SpriteName.cmdDieTexRetreat);
                add(SpriteName.cmdDieTexHit);
                add(SpriteName.cmdDieTexMiss);

                addWithSizeDef(SpriteName.hqRegularDoorEdge, currentIndex, 32, 8);
            }
                       

            currentIndex = numTilesWidth * 126;
            {
                add(SpriteName.LfAchievementFinalBoss, 2, 2);
                add(SpriteName.LfAchievementAllLevels, currentIndex, 2, 2);
                add(SpriteName.LfAchievementAllCards, currentIndex, 2, 2);
                add(SpriteName.LfAchievementFinalBoss_lock, currentIndex, 2, 2);
                add(SpriteName.LfAchievementAllLevels_lock, currentIndex, 2, 2);
                add(SpriteName.LfAchievementAllCards_lock, currentIndex, 2, 2);
                add(SpriteName.cgSacrificeForCards, currentIndex, 4, 2);
                add(SpriteName.cgSacrificeForMana, currentIndex, 4, 2);
                add(SpriteName.birdSkyTex, currentIndex, 2, 2);
                currentIndex += 16;
                add(SpriteName.EditorPencilCube, 2, 2);
                add(SpriteName.EditorFillAreaTexture, 2, 2);

                add(SpriteName.cmdTileOpenWater, currentIndex, 2, 2);
                add(SpriteName.cmdTileCastle, currentIndex, 2, 2);
                add(SpriteName.cmdTilePalisad, currentIndex, 2, 2);
                add(SpriteName.cmdTilePavedRoad, currentIndex, 2, 2);
                add(SpriteName.cmdTileStoneWall, currentIndex, 2, 2);
                add(SpriteName.cmdTileStoneGate, currentIndex, 2, 2);

                add(SpriteName.cmdTileDungeonGround, currentIndex, 2, 2);
                add(SpriteName.cmdTileDungeonWall, currentIndex, 2, 2);

                add(SpriteName.cmdIconButtonReadyCheckGray, currentIndex, 2, 2);
                add(SpriteName.cmdIconButtonReadyCheck, currentIndex, 2, 2);

                add(SpriteName.cmdTileMountainwall1, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainwallTop, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainGround1, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainGround2, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainGround3, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainGround4, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainwall2, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountainwall3, currentIndex, 2, 2);

                add(SpriteName.cmdTileMountBrickWall, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountBrickWallTorch, currentIndex, 2, 2);
                add(SpriteName.cmdTileRedBrickGround1, currentIndex, 2, 2);
                add(SpriteName.cmdTileRedBrickGround2, currentIndex, 2, 2);
                add(SpriteName.cmdTileRedBrickGround3, currentIndex, 2, 2);
                add(SpriteName.cmdTileRedBrickGround4, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountBrickGround1, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountBrickGround2, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountBrickGround3, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountBrickGround4, currentIndex, 2, 2);
                add(SpriteName.cmdTileMountBrickWallTop, currentIndex, 2, 2);

                add(SpriteName.cmdTileGrassBTree, 2, 2);
                add(SpriteName.cmdTileGrassB1, 2, 2);
                add(SpriteName.cmdTileGrassB2, 2, 2);
                add(SpriteName.cmdTileGrassB3, 2, 2);
                add(SpriteName.cmdTileGrassBMud1, 2, 2);
                add(SpriteName.cmdTileGrassBMud2, 2, 2);
                add(SpriteName.cmdTileGrassBRock, 2, 2);
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    class DebugTiles : Engine.AbsLoadTiles
    {
        public DebugTiles()
        {
            addTexture(SpriteName.LFAllBlockTextures, LoadTiles.BlockTextureWidth, LoadTiles.BlockTextureWidth, LoadedTexture.LF_TargetSheet);
        }
    }

    class LoadTiles : Engine.AbsLoadTiles
    {
        const int WaterMapWidth = 64;
        public const int BlockTextureWidth = 1024;

        public const int MenuBackgWidth = 216;
        public const int MenuBackgTopHeight = 16;
        public static int MenuBackgCenterHeight = 64;
        public const int MenuRectangleWidth = 200;
        public const int MenuRectangleHeight = 40;

        public const int BoxLineUpW = 384;
        public const int BoxLineUpH = 151;

        const float EquipButtonScale = 1.4f;
        const int EquipButtonHeight = 48;
        public static readonly Vector2 EquipButtonSize = new Vector2(EquipButtonScale * 64, EquipButtonScale * EquipButtonHeight);

        public LoadTiles()
        {
            //Textures
            const int TextureW = 32;
            addTexture(SpriteName.LFVoxelIconBase, TextureW, TextureW, LoadedTexture.LF_TargetSheet);
            addTexture(SpriteName.LFBlockTexturesBase, BlockTextureWidth, BlockTextureWidth, LoadedTexture.BlockTextures);
            const int WhiteAreaSize = 256;
            addTexture(SpriteName.WhiteArea, WhiteAreaSize, WhiteAreaSize, LoadedTexture.WhiteArea);
            this.Settings(1024, 32);
            this.TileSheetIx = LoadedTexture.LFTiles;
            currentIndex++;
            add(SpriteName.LFMetalShine);
            add(SpriteName.LFBricks);
            currentIndex+= 4;
            add(SpriteName.TextureWhiteEdge);
            currentIndex= 8;
            add(SpriteName.LFLava);
            add(SpriteName.LFWater);
            currentIndex+= 3;
            add(SpriteName.LFMultiSelection, currentIndex, 3, 3);
            add(SpriteName.BoardTxtSelectionStar, currentIndex, 2, 2);
            add(SpriteName.LFDottedLine, currentIndex, 4, 4);
            int storeIndex = add(SpriteName.BoardTitleBakg, currentIndex, 5, 1);
            ImageFile2 titleBackg = DataLib.Images.imageFiles[storeIndex];
            titleBackg.Source.X += 2; titleBackg.Source.Width -= 2;
            DataLib.Images.imageFiles[storeIndex] = titleBackg;

            currentIndex = numTilesWidth;
            add(SpriteName.LFEdge);
            add(SpriteName.InterfaceBorder);
            add(SpriteName.InterfaceBorderDot);
            add(SpriteName.TextureDots);
            add(SpriteName.TextureWhiteGradientWE);
            add(SpriteName.TextureWhiteGradientNS);
            add(SpriteName.TextureFramedWhiteArea);

            currentIndex = numTilesWidth - 4;
            add(SpriteName.Board_square_diagonal1);
            add(SpriteName.Board_square_diagonal2);
            add(SpriteName.Board_square_dot_corner_SE);
            add(SpriteName.Board_square_dot_corner_SW);

            //2
            add(SpriteName.LFMessageBackground, numTilesWidth * 2, 6, 1);
            add(SpriteName.LFMenuLootMessage, currentIndex, 3, 1);
            
            
            currentIndex = numTilesWidth + 22;
            add(SpriteName.Board_square_gentle);
            add(SpriteName.Board_square_double_edge);
            add(SpriteName.Board_square_edge_dot);
            add(SpriteName.Board_square_cirkle_dot);
            add(SpriteName.Board_cirkle);
            add(SpriteName.Board_cirkle_dark);
            add(SpriteName.Board_CC_arrow_south);
            add(SpriteName.Board_square_diagonal_cross);
            add(SpriteName.Board_wood_edge);
            add(SpriteName.Board_square_dot_corner_NE);
            add(SpriteName.Board_square_dot_corner_NW);
             

            //3
            currentIndex = numTilesWidth * 2 + 22;
            add(SpriteName.Board_question);
            add(SpriteName.Board_expression);
            add(SpriteName.Board_square_edge);
            add(SpriteName.Board_CC_arrow_west);
            add(SpriteName.Board_CC_arrow_east);
            add(SpriteName.Board_CC_arrow_north);
            add(SpriteName.Board_wood);
            currentIndex += 2;
            add(SpriteName.Board_square_dot_corners);

            //4
            currentIndex = numTilesWidth * 3 + 22;
            add(SpriteName.Board_arrow_west);
            add(SpriteName.Board_arrow_east);
            add(SpriteName.Board_arrow_north);
            add(SpriteName.Board_arrow_south);
            currentIndex += 2;
            add(SpriteName.Board_wood_arrow_west);
            add(SpriteName.Board_wood_arrow_east);
            add(SpriteName.Board_wood_arrow_north);
            add(SpriteName.Board_wood_arrow_south);
            //5
            currentIndex = numTilesWidth * 4;
            add(SpriteName.Board_square_soft_edge);
            add(SpriteName.Board_dirt);
            add(SpriteName.BoardPlayerCol1);
            add(SpriteName.BoardPlayerCol2);
            add(SpriteName.Board_pixelated);
            add(SpriteName.Board_plastic);
            add(SpriteName.Board_leather);
            add(SpriteName.Board_grass);
            add(SpriteName.Board_marble);
            add(SpriteName.Board_flat);
            add(SpriteName.Board_edge_flat);
            add(SpriteName.BoardPieceTex);

            currentIndex += 9;
            add(SpriteName.Board_CW_arrow_south);
            add(SpriteName.Board_CW_arrow_north);
            add(SpriteName.Board_CW_arrow_west);
            add(SpriteName.Board_CW_arrow_east);

            add(SpriteName.Board_wood_CW_arrow_south);
            add(SpriteName.Board_wood_CW_arrow_north);
            add(SpriteName.Board_wood_CW_arrow_west);
            add(SpriteName.Board_wood_CW_arrow_east);

            
            add(SpriteName.Board_wood_CC_arrow_west);
            add(SpriteName.Board_wood_CC_arrow_east);
            add(SpriteName.Board_wood_CC_arrow_north);
            add(SpriteName.Board_wood_CC_arrow_south);
            

            add(SpriteName.BoardTxtMenuSelRect, numTilesWidth * 5, 16, 2);
            add(SpriteName.BoardTxtMenuSelSquare, currentIndex, 2, 2);
            //row 7
            add(SpriteName.BoardTxtGrayBakg, numTilesWidth * 7, 7, 4);
            currentIndex= numTilesWidth * 7 + 15;
            add(SpriteName.LFMenuItemBackground, currentIndex, 2, 1);
            add(SpriteName.LFMenuItemBackgroundGray, currentIndex, 2, 1);
            addQtile(SpriteName.LFIconItemSpeed, Corner.TopLeft);
            addQtile(SpriteName.LFIconItemDefence, Corner.TopRight);
            addQtile(SpriteName.LFIconItemPower, Corner.BottomLeft);
            currentIndex++;
            add(SpriteName.LFMenuItemBackgroundShop, currentIndex, 3, 1);
            add(SpriteName.LFMenuLargeShopButton, currentIndex, 6, 1);

            currentIndex = numTilesWidth * 11;
            MenuBackgCenterHeight = 2 * this.TileSize;
            addWithSizeDef(SpriteName.LFMenuBackCenter, currentIndex, MenuBackgWidth, MenuBackgCenterHeight);
            currentIndex = numTilesWidth * 9 + 7;
            addWithSizeDef(SpriteName.LFMenuRectangleGray, currentIndex, MenuRectangleWidth, MenuRectangleHeight);
            currentIndex = numTilesWidth * 11 + 7; 
            addWithSizeDef(SpriteName.LFMenuRectangle, currentIndex, MenuRectangleWidth, MenuRectangleHeight);
            addWithSizeDef(SpriteName.LFMenuSquare, currentIndex, MenuRectangleHeight, MenuRectangleHeight);
            
            addWithSizeDef(SpriteName.LFMenuEquipButtonX, currentIndex, 2 * this.TileSize, EquipButtonHeight);
            addWithSizeDef(SpriteName.LFMenuEquipButtonA, currentIndex, 2 * this.TileSize, EquipButtonHeight);
            addWithSizeDef(SpriteName.LFMenuEquipButtonB, currentIndex, 2 * this.TileSize, EquipButtonHeight);
            addWithSizeDef(SpriteName.LFMenuEquipButtonLB, currentIndex, 2 * this.TileSize, EquipButtonHeight);
            addWithSizeDef(SpriteName.LFMenuEquipButtonRB, currentIndex, 2 * this.TileSize, EquipButtonHeight);

            currentIndex = numTilesWidth * 13 + 7;
            addWithSizeDef(SpriteName.LFMenuTitleRectangle, currentIndex, 6 * this.TileSize, this.TileSize - 1);
            currentIndex = numTilesWidth * 13 + 14;
            add(SpriteName.MenuTabHighlighted);
            add(SpriteName.MenuTab);
            currentIndex++;
            add(SpriteName.WeaponDaneAxeIron);
            add(SpriteName.WeaponDaneAxeBronze);
            add(SpriteName.WeaponDaneAxeSilver);
            add(SpriteName.WeaponDaneAxeGold);
            add(SpriteName.WeaponDaggerIron);
            add(SpriteName.WeaponDaggerBronze);
            add(SpriteName.WeaponDaggerSilver);
            add(SpriteName.WeaponDaggerGold);
            add(SpriteName.WeaponDaggerMithril);
            add(SpriteName.WeaponSickle);
            add(SpriteName.InterfaceStar);
            add(SpriteName.IconQuestLog);
            add(SpriteName.IconPrivateHome);
            add(SpriteName.IconFreeBuildArea);
            add(SpriteName.IconEnemyOutpost);

            currentIndex = numTilesWidth * 14;
            addWithSizeDef(SpriteName.LFMenuBackTop, currentIndex, MenuBackgWidth, MenuBackgTopHeight);
            add(SpriteName.LFMenuRectangleSelection, currentIndex, 6, 1);
            add(SpriteName.LFMenuSquareSelection);
            add(SpriteName.TextureSkydome, currentIndex, 2, 2);
            currentIndex++;
            add(SpriteName.ItemRepairKit);
            currentIndex = numTilesWidth * 14 + 27;
            add(SpriteName.WeaponBuildHammer);

            currentIndex = numTilesWidth * 15;
            addWithSizeDef(SpriteName.LFMenuBackBottom, currentIndex, MenuBackgWidth, MenuBackgTopHeight);
            currentIndex = numTilesWidth * 15 +  17;

            
            currentIndex = numTilesWidth * 16;
            addWithSizeDef(SpriteName.TextureHealthbarSmall, numTilesWidth * 16, 41, 3);
            addWithSizeDef(SpriteName.TextureHealthbarMedium, numTilesWidth * 8 + 11, LF2.MiniHealthBar.TextureWidth, LF2.MiniHealthBar.TextureHeight);
            addWithSizeDef(SpriteName.TextureHealthbarBGMedium, numTilesWidth * 8 + 8, LF2.MiniHealthBar.BGTextureWidth, LF2.MiniHealthBar.BGTextureHeight);
            addWithSizeDef(SpriteName.TextureHealthbarBGEndMedium, numTilesWidth * 8 + 10, 26, 0, LF2.MiniHealthBar.BGEndTextureWidth, LF2.MiniHealthBar.BGTextureHeight);

            currentIndex = numTilesWidth * 16 + 2;

            add(SpriteName.ImageMissingIcon);
            add(SpriteName.DpadLeftRight);

             add(SpriteName.rtsSpearmen); 
             add(SpriteName.rtsCavalry); 
             add(SpriteName.rtsBallista); 
             add(SpriteName.rtsSeaWarrior);
             add(SpriteName.rtsBattleXpMeter, currentIndex, 3, 1);
             add(SpriteName.rtsFatigueMeter, currentIndex, 3, 1); 
             add(SpriteName.rtsBattleIcon); 
             add(SpriteName.rtsArmyBanner); 
             add(SpriteName.rtsBorderDot); 
             add(SpriteName.rtsCirkleLoadingBar);

            currentIndex = numTilesWidth * 17;
            add(SpriteName.FolderHouse);
            add(SpriteName.FolderCastle);
            add(SpriteName.FolderSpace);
            add(SpriteName.FolderTerrain);
            add(SpriteName.FolderCharacer);
            add(SpriteName.FolderSquares);
            add(SpriteName.FolderVeihcle);
            add(SpriteName.FolderArt);
            add(SpriteName.FolderFurniture);
            currentIndex++;
            add(SpriteName.FolderAnimals);
            add(SpriteName.FolderRoadSign);
            add(SpriteName.FolderSmileys);
            add(SpriteName.FolderTools);
            add(SpriteName.FolderTimer);
            add(SpriteName.FolderQuestion);
            add(SpriteName.ButtonA);
            add(SpriteName.ButtonB);
            add(SpriteName.ButtonX);
            add(SpriteName.ButtonY);
            add(SpriteName.ButtonBACK);
            add(SpriteName.ButtonSTART);
            add(SpriteName.ButtonRB);
            add(SpriteName.ButtonLB);
        
            add(SpriteName.ButtonLT);
            add(SpriteName.ButtonRT);
            add(SpriteName.LeftStick);
            add(SpriteName.RightStick);
            add(SpriteName.Dpad);
            add(SpriteName.DpadUp);
            add(SpriteName.DpadDown);
            add(SpriteName.IconSplitScreen);

            currentIndex = numTilesWidth * 18;
            add(SpriteName.GoodsMeat);
            add(SpriteName.GoodsGrilledMeat);
            add(SpriteName.IconBuildArrow);
            add(SpriteName.IconBuildSelection);
            add(SpriteName.IconBuildAdd);
            add(SpriteName.IconBuildRemove);
            add(SpriteName.IconVillage);
            add(SpriteName.IconBuildMoveSelection);
            add(SpriteName.InterfaceIconCamera);
            add(SpriteName.IconColorPick);
            add(SpriteName.IconMusic);
            add(SpriteName.IconInfo);
            add(SpriteName.IconClient);
            add(SpriteName.IconHost);
            add(SpriteName.IconWeight);
            add(SpriteName.IconSandGlass);
            add(SpriteName.IconCheckYes);
            add(SpriteName.IconCheckNo);
            add(SpriteName.TxForwardArrow);
            add(SpriteName.BoardIconYes);
            add(SpriteName.BoardIconNo);
            add(SpriteName.BoardIconController);
            add(SpriteName.BoardIconOptions);
            add(SpriteName.BoardIconWrench);
            add(SpriteName.BoardIconDelete);
            add(SpriteName.BoardIconBack);
            add(SpriteName.BoardIconPlayer);
            add(SpriteName.BoardIconAddPlayer);
            add(SpriteName.BoardIconMenuWheel);
            add(SpriteName.BoardIconEditText);
            add(SpriteName.BoardIconPiece);
            add(SpriteName.BoardIconChat);

            currentIndex = numTilesWidth * 19;
            add(SpriteName.InterfaceTriangleArrowS);
            add(SpriteName.DpadLeft);
            add(SpriteName.DpadRight);


            currentIndex = numTilesWidth * 19 + 9;
           
            add(SpriteName.IconCastle); add(SpriteName.LFGoalIcon); 
            add(SpriteName.IconHomeBase);
            currentIndex+= 4;
            add(SpriteName.BoardGameSymbol, currentIndex, 2, 2);
            add(SpriteName.BoardIconDice);
            add(SpriteName.BoardMoveAttack);
            add(SpriteName.BoardMoveSpecial);
            add(SpriteName.BoardAttack);
            add(SpriteName.BoardMove);
            add(SpriteName.BoardMoveJump);
            add(SpriteName.BoardPieceCenter);
            add(SpriteName.LeftStick_UD);
            add(SpriteName.RightStick_UD);
            add(SpriteName.LeftStick_LR);
            add(SpriteName.RightStick_LR);
            add(SpriteName.BoardIconDownload);
            add(SpriteName.BoardIconAccessory);
            add(SpriteName.BoardIconCogs);
            //--
            currentIndex = numTilesWidth * 20 + 3;
            //add(SpriteName.ControllerStart);
            //currentIndex++;
            //add(SpriteName.ControllerBack);
            add(SpriteName.ControllerDpadUp);
            add(SpriteName.LFIconGoBack);
            add(SpriteName.LFLSClick);
            add(SpriteName.LFRSClick);
            currentIndex++;//add(SpriteName.ControllerDpadLeftRight);
            add(SpriteName.LFIconLetter);
            add(SpriteName.LFIconHart);
            add(SpriteName.LFIconCoins);
            add(SpriteName.LFArrow);
            add(SpriteName.LFApplePie);
            add(SpriteName.LFGranpa);
            add(SpriteName.LFHealIcon);
            add(SpriteName.IconMapCameraView);
            currentIndex+=2;
            add(SpriteName.BoardIconCounter);
            add(SpriteName.BoardIconAddCounter);
            add(SpriteName.BoardIconAddDice);
            add(SpriteName.BoardIconExpression);
            add(SpriteName.BoardIconAddExpression);
            add(SpriteName.BoardIconAddPiece);
            add(SpriteName.BoardIconMultiplayer);
            add(SpriteName.BoardIconCopy);
            add(SpriteName.BoardIconAddController);
            add(SpriteName.BoardIconRules);
            add(SpriteName.BoardIconAddRules);
            add(SpriteName.BoardIconGameRoom);
            add(SpriteName.BoardIconAddGameRoom);
            add(SpriteName.BoardIconShare);

            currentIndex = numTilesWidth * 21;
            add(SpriteName.GoodsGlass);
            add(SpriteName.GoodsGlassBroken);
            add(SpriteName.GoodsGemRed);
            add(SpriteName.GoodsGemChrystal);
            add(SpriteName.GoodsGemDiamond);
            add(SpriteName.GoodsScroll);
            add(SpriteName.GoodsGemBlack);
            add(SpriteName.GoodsApple);
            add(SpriteName.GoodsAppleGrilled);
            add(SpriteName.GoodsWine);
            add(SpriteName.GoodsBread);
            add(SpriteName.GoodsSeed);
            add(SpriteName.IconTown);
            add(SpriteName.InterfaceOptionList);
            add(SpriteName.InterfaceTextInput);

            add(SpriteName.ArmourSkin);
            add(SpriteName.ArmourIron);
            add(SpriteName.ArmourMithril);
            add(SpriteName.ArmourBronze);
            add(SpriteName.ArmourLeather);
            add(SpriteName.HelmetBronze);
            add(SpriteName.WeaponShortBow);
            add(SpriteName.WeaponLongBow);
            add(SpriteName.WeaponSpecialBow);
            add(SpriteName.WeaponAxeBronze);
            add(SpriteName.WeaponAxeIron);
            add(SpriteName.WeaponAxeSilver);
            add(SpriteName.WeaponAxeGold);
            add(SpriteName.WeaponAxeMithril);
            add(SpriteName.ArmourSilver);
            add(SpriteName.ArmourGold);
            add(SpriteName.ArmourScaled);
        
 
            currentIndex = numTilesWidth * 22;
            add(SpriteName.GoodsGrapes);
            add(SpriteName.GoodsTusk);
            add(SpriteName.GoodsTeeth);
            add(SpriteName.GoodsHorn);
            add(SpriteName.GoodsNoseHorn);
            add(SpriteName.GoodsLeather);
            add(SpriteName.GoodsSkinn);
            add(SpriteName.GoodsLeatherScaled);
            add(SpriteName.GoodsFur);
            add(SpriteName.EffectStar);
            add(SpriteName.EffectGreenBubble);
            add(SpriteName.IconCheckboxOff);
            add(SpriteName.IconCheckboxOn);
            add(SpriteName.GoodsFeather);
            add(SpriteName.IconEggNest);
            
            add(SpriteName.WeaponSwordIron);
            add(SpriteName.WeaponSwordSilver);
            add(SpriteName.WeaponSwordMithril);
            add(SpriteName.WeaponSwordBronze);
            add(SpriteName.WeaponSwordGold);
            add(SpriteName.RingBasic);
            add(SpriteName.RingRed);
            add(SpriteName.RingBlue);
            add(SpriteName.RingWhite);
            add(SpriteName.RingBlack);
            add(SpriteName.RingGreen);
            add(SpriteName.LFIconMap);
            add(SpriteName.LFIconOneHandR);
            add(SpriteName.LFIconOneHandL);
            add(SpriteName.LFIconTwoHands);
            add(SpriteName.LFIconQuickDraw);
            add(SpriteName.WeaponSlingshot);
            //--
            currentIndex = numTilesWidth * 23;
            add(SpriteName.IconQualityLow);
            add(SpriteName.IconQualityMed);
            add(SpriteName.IconQualityHigh);
            add(SpriteName.IconMenuSettings);
            add(SpriteName.GoodsStoneGranit);
            add(SpriteName.GoodsStoneMarmour);
            add(SpriteName.GoodsStoneSand);
            add(SpriteName.GoodsStoneFlint);
            add(SpriteName.GoodsMetalIron);
            add(SpriteName.GoodsMetalSilver);
            add(SpriteName.GoodsMetalBronze);
            add(SpriteName.GoodsMetalGold);
            add(SpriteName.GoodsMetalMithril);
            add(SpriteName.GoodsStick);
            add(SpriteName.GoodsWood);
            add(SpriteName.LFIconCreativeMode);
            add(SpriteName.LFIconFireEnchant);
            add(SpriteName.LFIconLightningEnchant);
            add(SpriteName.LFIconPoisionEnchant);
            add(SpriteName.LFIconEvilEnchant);
            add(SpriteName.LFMenuPointer);

            addFullQtile(SpriteName.LFIconAttackTypeSword,SpriteName.LFIconAttackTypeArrow,SpriteName.LFIconAttackType3Arrows,SpriteName.LFIconAttackTypeForward);
            addFullQtile(SpriteName.LFIconAttackTypeMagic, SpriteName.LFIconAttackTypeMagicBlast, SpriteName.IconMapFlag, SpriteName.IconMapQuest); 
       
            add(SpriteName.HelmetLeather);
            add(SpriteName.HelmetIron);
            add(SpriteName.HelmetMithril);
            add(SpriteName.HelmetGold);
            add(SpriteName.LFIconEquip);
            add(SpriteName.LFIconMirror);
            add(SpriteName.LFIconDiscardItem);
            add(SpriteName.Compass, currentIndex, 2, 2);


            currentIndex = numTilesWidth * 24;
            add(SpriteName.IconLock);
            add(SpriteName.IconBomb);
            add(SpriteName.IconThrowSpear);
            add(SpriteName.IconBackpack);
            add(SpriteName.IconChest);
            add(SpriteName.IconApperanceHat);
            add(SpriteName.IconApperanceShirt);
            add(SpriteName.IconApperanceBeard);
            add(SpriteName.IconApperanceHair);
            currentIndex += 2;
            add(SpriteName.GoodsBloodFingerHerb);
            add(SpriteName.GoodsFrogHeartHerb);
            add(SpriteName.GoodsFireStarHerb);
            add(SpriteName.GoodsBlueRoseHerb);
            add(SpriteName.ItemGreenBottle);
            add(SpriteName.ItemBlueBottle);
            add(SpriteName.ItemRedBottle);
            add(SpriteName.ItemYellowBottle);
            add(SpriteName.ItemPurpleBottle);
            
            add(SpriteName.WeaponStaff);
            add(SpriteName.ShieldBuckle);
            add(SpriteName.ShieldRound);
            add(SpriteName.ShieldSquare);
            add(SpriteName.WeaponHandSpear);
            add(SpriteName.WeaponStick);
            add(SpriteName.ItemWaterFull);
            add(SpriteName.ItemWaterEmpty);
            add(SpriteName.LFIconEat);
            add(SpriteName.MapCameraAngle);

            currentIndex = numTilesWidth * 25 + 2;
            add(SpriteName.IconSour);
            add(SpriteName.IconLaugh);
            currentIndex++;
            add(SpriteName.IconTeasing);
            add(SpriteName.IconThumbUp);
            currentIndex += 2;
            add(SpriteName.IconHand);
            currentIndex = numTilesWidth * 25 + 10;
            addQtile(SpriteName.CompassNSymbol, Corner.TopLeft);
            currentIndex++;
            add(SpriteName.ControllerGuide);
            add(SpriteName.IconQuestExpression);
            currentIndex++;//add(SpriteName.ControllerQuickTypeXAB);
            add(SpriteName.ItemGoldenArrow);
            add(SpriteName.ItemGoldenBomb);
            add(SpriteName.TrophyStick);
            add(SpriteName.IconBossKey);
            add(SpriteName.InterfaceThinCirkle, currentIndex, 2, 2);
            add(SpriteName.IconOutPost);
            add(SpriteName.GoodsSlingstone);
            add(SpriteName.WeaponLongSwordBronze);
            add(SpriteName.WeaponLongSwordIron);
            add(SpriteName.WeaponLongSwordSilver);
            add(SpriteName.WeaponLongSwordGold);
            add(SpriteName.WeaponLongSwordMithril);
            add(SpriteName.IconEnemyOutpost);
            add(SpriteName.IconTravel);
            add(SpriteName.LFIconBuy);
            add(SpriteName.BoardMenuPointer, currentIndex, 2, 2);

            currentIndex = numTilesWidth * 26 + 20;
            add(SpriteName.TrophyLighting);
            add(SpriteName.IconMenuMultiplayer);
            add(SpriteName.TrophyLocked);
            add(SpriteName.TrophyUnlocked);
            add(SpriteName.IconApperanceEyes);
            add(SpriteName.IconApperanceMouth);
            add(SpriteName.IconApperanceBelt);
            add(SpriteName.TrophySpear);
            add(SpriteName.TrophySword);
            add(SpriteName.TrophyAxe);

            currentIndex = numTilesWidth * 27 + 18;
            add(SpriteName.IconMenuCloseGame);
            add(SpriteName.TrophyKillMonsters);
            add(SpriteName.TrophyCompleteGame);
            add(SpriteName.TrophySuperChallenge);
            add(SpriteName.TrophyTrippleKill);
            add(SpriteName.TrophyBarrelBlast);
            add(SpriteName.TrophyHealthy);
            add(SpriteName.IconEggNestDestroyed);
            add(SpriteName.WeaponSwordWood);

            add(SpriteName.ItemCookie);
            add(SpriteName.ItemGoldenCookie);
            add(SpriteName.ItemCandle);
            currentIndex++;
            add(SpriteName.ItemBeer);
            

            //--
            add(SpriteName.BoardTxtMenuRect, numTilesWidth * 26, 16, 2);
            add(SpriteName.BoardTxtMenuSquare, currentIndex, 2, 2);

#region PLATFORM
            currentIndex = numTilesWidth * 28;     
            add(SpriteName.PlatformWepStone);
            add(SpriteName.PlatformWepDrill);
            add(SpriteName.PlatformBulbOn);
            add(SpriteName.PlatformBulvOff);
#endregion
            add(SpriteName.GoodsBlackTooth);
            add(SpriteName.GoodsBladderStone);
            add(SpriteName.GoodsRib);
            add(SpriteName.GoodsPlastma);
            add(SpriteName.GoodsThread);
            add(SpriteName.GoodsCoal);
            add(SpriteName.IconEndBossTomb);
            add(SpriteName.LFIconRefine);
            add(SpriteName.ItemLuckyPaw);
            add(SpriteName.GoodsInk);
            add(SpriteName.GoodsPoisionSting);
            add(SpriteName.GoodsHonny);
            add(SpriteName.GoodsVax);
            add(SpriteName.GoodsBlackEye);
            add(SpriteName.ItemCoinOrc);
            add(SpriteName.ItemCoinDwarf);
            add(SpriteName.ItemCoinSouth);
            add(SpriteName.ItemCoinElf);
            add(SpriteName.ItemCoinAncient);
            add(SpriteName.ItemCoinNordic);
            add(SpriteName.LFIconScrap);
            add(SpriteName.ItemOrcMead);
            add(SpriteName.GoodsPaw);

            add(SpriteName.WeaponPickAxe);
            add(SpriteName.GoodsHolyWater);
            add(SpriteName.LFIconBlessed);

            currentIndex = numTilesWidth * 29;
            add(SpriteName.BoardIconHistory);
            add(SpriteName.BoardIconRemovePiece);
            add(SpriteName.BoardIconFolder);
            add(SpriteName.BoardIconMovePiece);
            add(SpriteName.BoardIconListDot);
            add(SpriteName.BoardSquareBoard);
            add(SpriteName.BoardHexBoard);
            add(SpriteName.BoardPolyBoard);
            add(SpriteName.BoardProps);
            add(SpriteName.BoardIconCut);
            add(SpriteName.BoardIconPaste);
            add(SpriteName.BoardTurnMarker, currentIndex, 2, 2);
            add(SpriteName.BoardWatch);
            add(SpriteName.BoardAddWatch);
            add(SpriteName.BoardIconChatFolder);
            add(SpriteName.BoardIconCardDeckBack);
            add(SpriteName.BoardIconCardDeckFace);
            add(SpriteName.BoardIconCardFace);
            add(SpriteName.BoardIconCardBack);
            add(SpriteName.BoardIconAddCardDeck);
            add(SpriteName.BoardIconAddCard);
            add(SpriteName.BoardIconPickCard);
            add(SpriteName.BoardPieceProperties);
            add(SpriteName.BoardAddPieceProperties);
            add(SpriteName.BoardPiecePropertiesFolder);
            add(SpriteName.BoardAddPiecePropertiesFolder);
            add(SpriteName.BoardIconMissingPlayer);

            add(SpriteName.BoardCrown);
            add(SpriteName.BoardAddCrown);
            add(SpriteName.BoardRemoveCrown);
            add(SpriteName.BoardModelEditor);

            currentIndex = numTilesWidth * 30;

            add(SpriteName.BoardMoveSchedule);
            add(SpriteName.BoardRemoveMoveSchedule);
            add(SpriteName.BoardIconRemoveGameRoom);
            add(SpriteName.BoardSquareInfo);
            add(SpriteName.BoardRotatePiece);
            add(SpriteName.BoardPieceUpsideDown);
            add(SpriteName.BoardDuplicatePiece);
            add(SpriteName.BoardExit);
            add(SpriteName.BoardActionArrow);
            add(SpriteName.MoveDownArrow);
            add(SpriteName.MoveUpArrow);
            currentIndex += 2;
            add(SpriteName.BoardChangeOwner);
            add(SpriteName.BoardRandomPlayer);
            add(SpriteName.InsertIcon);


            this.TileSheetIx = LoadedTexture.BlockTextures;
            addWithSizeDef(SpriteName.TextureVoxelFace, 0, BlockTextureWidth, BlockTextureWidth);

            //this.TileSheetIx = LoadedTexture.box_lineup;
            //addWithSizeDef(SpriteName.TextureBoxLineUp, 0, BoxLineUpW, BoxLineUpH);

            this.TileSheetIx = LoadedTexture.waterheigth;
            addWithSizeDef(SpriteName.TextureWaterMap, 0, WaterMapWidth, WaterMapWidth);
            
        }
    }
}

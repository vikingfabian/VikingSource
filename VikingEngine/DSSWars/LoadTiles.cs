//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;

//namespace VikingEngine.DSSWars
//{
//    class LoadTiles : Engine.AbsSpriteSheetLayout
//    {
//        public const int TextureWidth = 1024;

//        public const int CardTileW = 3, CardTileH = 4;

//        public LoadTiles()
//        {
//            //Textures
//            this.Settings(TextureWidth, 16);
//            this.TileSheetIx = LoadedTexture.SpriteSheet;

//            //add(SpriteName.rtsTownSide);
//            //add(SpriteName.rtsTownTop);
//            //add(SpriteName.rtsBlockSide);

//            //addWithSizeDef(SpriteName.rtsFlagTextureBase, currentIndex, 48, 48);
//            //add(SpriteName.rtsApBarTex, 5, 2, 1);

//            currentIndex = numTilesWidth * 2 -2;
//           // add(SpriteName.rtsBlockTop);

//            currentIndex = numTilesWidth * 3 + 5;
//            //add(SpriteName.rtsAIFlaySymbol);
//            //add(SpriteName.rtsBBTree);
//            //add(SpriteName.rtsBBStone);

//            //add(SpriteName.rtsSoldierCount);
//            //add(SpriteName.rtsCityIconFill);
//            //add(SpriteName.rtsArmyIconFill);
//            //add(SpriteName.rtsCityIcon);
//            //add(SpriteName.rtsArmyIcon);
//            add(SpriteName.rtsBattleHistory);
//            add(SpriteName.rtsAllianceIcon);
//            add(SpriteName.rtsRelationNeurtal);

//            currentIndex = numTilesWidth * 4;
//            add(SpriteName.rtsSelection1, currentIndex, 2, 2);
//            add(SpriteName.rtsSelection2, currentIndex, 2, 2);

//             //add(SpriteName.rtsSelectionButton);
//             add(SpriteName.rtsIncome);
//             add(SpriteName.rtsUpkeep);
//             add(SpriteName.rtsMoney);
//             add(SpriteName.rtsHonor);
//             add(SpriteName.rtsAPbar, currentIndex, 4, 1);
//             add(SpriteName.rtsIncomeSum);

//             add(SpriteName.rtsCityLink);
//             add(SpriteName.rtsArmyLink);

//             currentIndex = numTilesWidth * 5 + 5;
//             add(SpriteName.rtsWarningPopBG);
//             add(SpriteName.rtsWarningPopBgArrow);
//             add(SpriteName.rtsWarningPopReady);
//             add(SpriteName.rtsWarningPopExpression);
//             add(SpriteName.rtsHomeIcon);
//             add(SpriteName.rtsShipRemove);
//             add(SpriteName.rtsShipBuild);
//             add(SpriteName.rtsShip);
//             add(SpriteName.rtsRelationAllied);
//             add(SpriteName.rtsRelationAtWar);
//             add(SpriteName.rtsRelationNemesis);

//             currentIndex = numTilesWidth * 6;
//            add(SpriteName.rtsLoading0of8);
//            add(SpriteName.rtsLoading1of8);
//            add(SpriteName.rtsLoading2of8);
//            add(SpriteName.rtsLoading3of8);
//            add(SpriteName.rtsLoading4of8);
//            add(SpriteName.rtsLoading5of8);
//            add(SpriteName.rtsLoading6of8);
//            add(SpriteName.rtsLoading7of8);
//            add(SpriteName.rtsLoading8of8);

//            add(SpriteName.rtsFactionFlagIcon);

//            add(SpriteName.rtsApDotFull);
//            add(SpriteName.rtsApDotHalf);
//            add(SpriteName.rtsApDotEmpty);
//            add(SpriteName.rtsDisbandArmy);
//            add(SpriteName.rtsAnalyzeBattle);

//            currentIndex = numTilesWidth * 7;
//            add(SpriteName.rtsCardBg, currentIndex, CardTileW, CardTileH);
//            add(SpriteName.rtsCavalry, currentIndex, 3, 3);
//            add(SpriteName.rtsSpearmen, currentIndex, 3, 3);
//            add(SpriteName.rtsBallista, currentIndex, 3, 3);
//            add(SpriteName.rtsSeaWarrior, currentIndex, 3, 3);

//            currentIndex = numTilesWidth * 10 + 3;
//            add(SpriteName.rtsHeadCityProfileIcon, currentIndex, 2, 2);
//            add(SpriteName.rtsLargeCityProfileIcon, currentIndex, 2, 2);
//            add(SpriteName.rtsSmallCityProfileIcon, currentIndex, 2, 2);

//            add(SpriteName.rtsSplitArmy, currentIndex, 2, 2);
//            add(SpriteName.rtsCombineArmies, currentIndex, 2, 2);

//            currentIndex = numTilesWidth * 12;
//            add(SpriteName.rtsBattleXpMeter, currentIndex, 2, 1);
//            add(SpriteName.rtsFatigueMeter, currentIndex, 2, 1);
//            add(SpriteName.rtsBattleIcon);
//            //add(SpriteName.rtsPlayCommanderButton, currentIndex, 8, 2);


//        }
//    }
//}

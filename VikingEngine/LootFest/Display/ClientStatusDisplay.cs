using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.SteamWrapping;

namespace VikingEngine.LootFest.Display
{
    class ClientStatusDisplay
    {
        /* Fields */
        public Graphics.Image bg; 
        Graphics.Image textBar, direction, coinsIcon;
        Graphics.Image suitSpecialIcon, suitSpecialIconBg, itemIcon, itemBg;
        LootFest.Display.SpriteText speciallAmmoCount, itemCount, coinsCount;

        bool viewArrow = true;
        VikingEngine.LootFest.BlockMap.LevelEnum prevLvl = BlockMap.LevelEnum.NUM_NON;
        Graphics.Image levelIcon;
        LootFest.Display.SpriteText levelNumber;

        Graphics.ImageAdvanced gamerPic;

        Graphics.TextG name;
        Players.ClientPlayer clientPlayer;
        List<Graphics.Image> hearts = new List<Graphics.Image>(4);

        //SteamImageLoadData steamImage;

        /* Constructors */
        public ClientStatusDisplay(Players.ClientPlayer p)
        {
            this.clientPlayer = p;
            Vector2 sz = new Vector2(6f, 2.2f) * Engine.Screen.IconSize;
            bg = new Graphics.Image(SpriteName.WhiteArea, Engine.Screen.SafeArea.RightTop, sz, ImageLayers.Background4);
            bg.Color = Color.SaddleBrown;

            if (PlatformSettings.SteamAPI)
            {
                gamerPic = new Graphics.ImageAdvanced(SpriteName.MissingImage, bg.Position,
                    new Vector2(bg.Height * 0.7f), ImageLayers.NUM, false);
                gamerPic.LayerAbove(bg);

                new LoadGamerIcon(gamerPic, p.pData.netPeer(), false);
                //TryLoadSteamImage();
            }

            textBar = new Graphics.Image(SpriteName.WhiteArea, new Vector2(bg.Xpos, gamerPic.Bottom),
                new Vector2(bg.Width, bg.Height - gamerPic.Height), ImageLayers.NUM);
            textBar.LayerAbove(bg);
            textBar.Color = new Color(81, 40, 11);

            name = new Graphics.TextG(LoadedFont.Regular, bg.CenterTop, new Vector2(Engine.Screen.TextSize * 1f),
                Graphics.Align.CenterWidth, p.pData.PublicName(LoadedFont.Regular), Color.White, ImageLayers.NUM);
            name.LayerAbove(textBar);
            name.Ypos += sz.Y * 0.66f;

            direction = new Graphics.Image(SpriteName.LfClientCompassArrow, gamerPic.Center, gamerPic.Size * 1f, ImageLayers.NUM, true);
            direction.Opacity = 0.6f;
            direction.LayerAbove(gamerPic);

            levelIcon = new Graphics.Image(SpriteName.MissingImage, Vector2.Zero, gamerPic.Size * 0.5f, ImageLayers.AbsoluteTopLayer);
            levelIcon.Visible = false;
          
            levelNumber = new SpriteText("", Vector2.Zero, levelIcon.Height, ImageLayers.Background3, new Vector2(0, 0.5f), Color.White, true);
            
            float hudIconsStartX = gamerPic.Right + Engine.Screen.IconSize * 0.1f;

            coinsIcon = new Graphics.Image(SpriteName.LFHudCoins, new Vector2(hudIconsStartX, gamerPic.Ypos + gamerPic.Height * 0.3f),
                new Vector2(Engine.Screen.IconSize * 0.5f), ImageLayers.NUM);
            coinsIcon.LayerAbove(bg);
            coinsCount = new SpriteText("0", coinsIcon.RightTop, coinsIcon.Height, ImageLayers.Background3, Vector2.Zero, CoinsHUD.TextCol, true);

            Vector2 itemIconSz = new Vector2(Engine.Screen.IconSize * 0.6f);
            float textH = itemIconSz.Y * 0.96f;
            Vector2 itemsPos = new Vector2(hudIconsStartX, gamerPic.Bottom - itemIconSz.Y);

            suitSpecialIconBg = new Graphics.Image(SpriteName.LFSecondaryAttackHudBG, itemsPos, itemIconSz, ImageLayers.NUM);
            suitSpecialIconBg.LayerAbove(bg);
            suitSpecialIcon = new Graphics.Image(SpriteName.MissingImage, itemsPos, itemIconSz, ImageLayers.NUM);
            suitSpecialIcon.LayerAbove(suitSpecialIconBg);
            itemsPos.X += itemIconSz.X * 1f;
            speciallAmmoCount = new SpriteText("0", itemsPos, textH, ImageLayers.Background3, Vector2.Zero, Color.White, true);

            itemsPos.X += itemIconSz.X * 1.2f;
            itemBg = new Graphics.Image(SpriteName.LFItemHudBG, itemsPos, itemIconSz, ImageLayers.NUM);
            itemBg.LayerAbove(bg);
            itemIcon = new Graphics.Image(SpriteName.MissingImage, itemsPos, itemIconSz, ImageLayers.NUM);
            itemIcon.LayerAbove(itemBg);
            itemsPos.X += itemIconSz.X * 1f;
            itemCount = new SpriteText("0", itemsPos, textH, ImageLayers.Background3, Vector2.Zero, Color.White, true);

            update();
        }

        public void refreshPosition(int clientIndex)
        {
            Vector2 sz = bg.Size;
            bg.Position = Engine.Screen.SafeArea.RightTop + new Vector2(-sz.X, sz.Y * 1.1f * clientIndex);

            gamerPic.Position = bg.Position;

            textBar.Position = new Vector2(bg.Xpos, gamerPic.Bottom);

            name.Position = bg.CenterTop;
            name.Ypos += sz.Y * 0.66f;

            direction.Position = gamerPic.Center;

            float hudIconsStartX = gamerPic.Right + Engine.Screen.IconSize * 0.1f;
            coinsIcon.Position = new Vector2(hudIconsStartX, gamerPic.Ypos + gamerPic.Height * 0.3f);
            coinsCount.SetPosition(coinsIcon.RightTop);

            Vector2 itemsPos = new Vector2(hudIconsStartX, gamerPic.Bottom - suitSpecialIconBg.Height);
            suitSpecialIconBg.Position = itemsPos;
            suitSpecialIcon.Position = itemsPos; 
            itemsPos.X += suitSpecialIconBg.Width * 1f;

            itemBg.Position = itemsPos;
            itemIcon.Position = itemsPos;
            itemsPos.X += suitSpecialIconBg.Width * 1f;
            itemCount.SetPosition(itemsPos);

            levelIcon.Position = new Vector2(bg.Xpos, bg.Ypos + gamerPic.Height * 0.5f);
            levelNumber.SetPosition(levelIcon.RightCenter);
        }

        /* Novelty Methods */
        public void update()
        {
            //if (steamImage.state == SteamImageLoadState.ImageNotLoadedYet_RetrySoon)
            //{
            //    TryLoadSteamImage();
            //}

            bool new_viewArrow = LfRef.gamestate.LocalHostingPlayer != null && LfRef.gamestate.LocalHostingPlayer.hero.LevelEnum == clientPlayer.inLevel;
            if (new_viewArrow != viewArrow)
            {
                viewArrow = new_viewArrow;

                if (viewArrow)
                {
                    levelNumber.DeleteMe();
                    prevLvl = BlockMap.LevelEnum.NUM_NON;
                }

                direction.Visible = viewArrow;
                levelIcon.Visible = !viewArrow;
            }


            if (viewArrow)
            {
                Rotation1D angle = new Rotation1D(LfRef.gamestate.LocalHostingPlayer.hero.AngleDirToObject(clientPlayer.hero));
                //angle.Add(-MathHelper.PiOver2);
                angle.Add(-LfRef.gamestate.LocalHostingPlayer.localPData.view.Camera.TiltX -MathHelper.PiOver2);
                direction.Position = gamerPic.Center;// + angle.Direction(gamerPic.Width * 0.4f);

               // angle.Add(-MathHelper.PiOver2);//rätta till, pilen pekar åt höger
                direction.Rotation = angle.Radians;
            }
            else
            {
                if (clientPlayer.inLevel != prevLvl)
                {
                    prevLvl = clientPlayer.inLevel;
                    SpriteName symb; int? number;
                    BlockMap.LevelsManager.LevelSymbol(clientPlayer.inLevel, out symb, out number);

                    levelIcon.SetSpriteName(symb);
                    if (number == null)
                    {
                        levelNumber.DeleteMe();
                    }
                    else
                    {
                        levelNumber.Text(number.Value.ToString());
                    }
                }
            }
        }

        public void refreshStatus(GO.SuitType suit, int ammo, int hp, int maxHp, int coins, 
            GO.Gadgets.ItemType item, int itemAmount)
        {
            for (int i = 0; i < maxHp; ++i)
            {
                 Graphics.Image heart;

                if (hearts.Count <= i)
                {
                    heart = new Graphics.Image(SpriteName.LFHudHeart, Vector2.Zero,// gamerPic.RightTop, 
                        new Vector2(Engine.Screen.IconSize * 0.45f), ImageLayers.NUM, true);
                    
                    //heart.Xpos = coinsIcon.Xpos + heart.Width * 0.5f;
                    //heart.Ypos += heart.Height * 0.54f;

                   // heart.Xpos += heart.Width * i;
                    hearts.Add(heart);
                    heart.LayerAbove(bg);
                }

                heart = hearts[i];
                heart.Position = new Vector2(coinsIcon.Xpos + heart.Width * 0.5f + heart.Width * i, gamerPic.Ypos + heart.Height * 0.54f);

                heart.SetSpriteName(i < hp ? SpriteName.LFHudHeart : SpriteName.LFHudHeartEmpty);
            }

            while (hearts.Count > maxHp)
            {
                int last = hearts.Count - 1;
                hearts[last].DeleteMe();
                hearts.RemoveAt(last);
            }

            SpriteName suitSpecialIconTile = SpriteName.MissingImage;
            switch (suit)
            {
                case GO.SuitType.Archer:
                    suitSpecialIconTile = SpriteName.LFArcherIcon2;
                    break;
                case GO.SuitType.BarbarianDane:
                    suitSpecialIconTile = SpriteName.LFDaneIcon2;
                    break;
                case GO.SuitType.BarbarianDual:
                    suitSpecialIconTile = SpriteName.LFDualIcon2;
                    break;
                case GO.SuitType.Basic:
                    suitSpecialIconTile = SpriteName.LFBasicSuitIcon2;
                    break;
                case GO.SuitType.FutureSuit:
                    suitSpecialIconTile = SpriteName.LfHandgranade;
                    break;
                case GO.SuitType.ShapeShifter:
                    suitSpecialIconTile = SpriteName.LfShapeshifterIcon2;
                    break;
                case GO.SuitType.SpearMan:
                    suitSpecialIconTile = SpriteName.LFSpearmanIcon2;
                    break;
                case GO.SuitType.Swordsman:
                    suitSpecialIconTile = SpriteName.LFSwordsmanIcon2;
                    break;
            }
            SpriteName itemTile = VikingEngine.LootFest.GO.Gadgets.Item.ItemIcon(item);


            suitSpecialIcon.SetSpriteName(suitSpecialIconTile);
            speciallAmmoCount.Text(ammo.ToString());
            itemIcon.SetSpriteName(itemTile);

            if (itemAmount == byte.MaxValue)
            {
                itemCount.DeleteMe();
            }
            else
            {
                itemCount.Text(itemAmount.ToString());
            }

            coinsCount.Text(coins.ToString());
        }

        public void DeleteMe()
        {
            bg.DeleteMe();
            textBar.DeleteMe();
            name.DeleteMe();
            gamerPic.DeleteMe();
            direction .DeleteMe();
            coinsIcon.DeleteMe();
            suitSpecialIcon.DeleteMe();
            suitSpecialIconBg.DeleteMe();
            itemIcon.DeleteMe();
            itemBg.DeleteMe();
            speciallAmmoCount.DeleteMe();
            itemCount.DeleteMe();
            coinsCount.DeleteMe();

            levelIcon.DeleteMe();
            levelNumber.DeleteMe();

            foreach (var h in hearts)
            {
                h.DeleteMe();
            }
        }
        
    }
}

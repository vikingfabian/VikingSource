using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.Graphics;
using VikingEngine.Engine;

namespace VikingEngine.LootFest
{
    abstract class AbsHUD2
    {
        protected const float ShakeTimeSec = 0.13f;
        protected Time shakeTimer = 0;

        public VectorRect Area;
        public const ImageLayers HUDLayer = LfLib.Layer_StatusDisplay; //ImageLayers.Lay7;
        public static float HUDStandardHeight;

        public static void InitHUD()
        {
            HUDStandardHeight = Screen.MinWidthHeight * 0.07f;
        }

        abstract public void DeleteMe();

        protected static Graphics.Motion2d addItem_BumpMotion(Graphics.AbsDraw2D img)
        {
            return new Graphics.Motion2d(Graphics.MotionType.SCALE, img, img.Size * new Vector2(-0.4f, 0.5f), Graphics.MotionRepeate.BackNForwardOnce,
                90, true);
        }

        virtual public void emptyBump() { shakeTimer.Seconds = ShakeTimeSec; }

        protected Vector2 shakeValue()
        {
            return new Vector2(0.16f * HUDStandardHeight, 0.08f * HUDStandardHeight) * (float)(Math.Sin(shakeTimer.MilliSeconds * 0.1f));
        }
    }

    class CoinsHUD : AbsHUD2
    {
        public static readonly Color TextCol = Color.Yellow;
        Graphics.Image icon;
        //Graphics.TextS amountText;
        Display.SpriteText amountText;
        Graphics.Motion2d bump = null;

        Vector2 coinIconSz;

        public CoinsHUD(Vector2 pos)
        {
            coinIconSz = new Vector2(HUDStandardHeight * 0.8f);
            icon = new Graphics.Image(SpriteName.LFHudCoins, pos + new Vector2(HUDStandardHeight * 0.5f), coinIconSz, HUDLayer, true);
            

            amountText = new Display.SpriteText("0",
                pos + new Vector2(HUDStandardHeight * 0.9f, HUDStandardHeight * 0.5f),
                HUDStandardHeight * 0.5f,
                HUDLayer - 1,
                new Vector2(0, 0.5f),
                TextCol,
                true);


            UpdateAmount(0);

            Area = new VectorRect(pos, coinIconSz);
            Area.Width += HUDStandardHeight;
        }

        public void UpdateAmount(int amount)
        {
            amountText.Text(amount.ToString());
        }

        public void OnPickUp()
        {
            if (bump != null && !bump.IsDeleted)
            {
                bump.DeleteMe();
                icon.Size = coinIconSz;
            }
            bump = addItem_BumpMotion(icon);
            //bump = new Graphics.Motion2d(Graphics.MotionType.SCALE, icon, icon.Size * new Vector2(-0.4f, 0.5f), Graphics.MotionRepeate.BackNForwardOnce,
            //    90, true);
        }

        public override void DeleteMe()
        {
            icon.DeleteMe();
            amountText.DeleteMe();
        }
    }

    class SuitUseHUD : AbsHUD2
    {
        /* Fields */
        public bool useDotsForAmount;

        protected Graphics.Image itemIcon, itemShadow, bg;
        protected Image buttonIcon;

        Graphics.ImageGroupParent2D amountDots = null;
        Display.SpriteText amountText = null;
        protected IButtonMap button;
        Vector2 buttonIconUpScale, buttonIconDownScale;
        Vector2 amountDotSize;
        Player player;

        /* Constructors */
        public SuitUseHUD(Player player, VectorRect previousHud, SpriteName itemIc, SpriteName bgTile, IButtonMap button, bool useDotsForAmount)
        {
            this.player = player;
            this.button = button;
            this.useDotsForAmount = useDotsForAmount;
            Vector2 pos = previousHud.RightTop;
            pos.X += HUDStandardHeight * 0.6f;

            bg = new Graphics.Image(bgTile, pos, new Vector2(HUDStandardHeight), HUDLayer + 1);
            itemIcon = new Graphics.Image(itemIc, bg.Center, bg.Size * 0.8f, HUDLayer, true);
            itemShadow = (Graphics.Image)itemIcon.CloneMe();
            itemShadow.Color = Color.Black;
            itemShadow.Opacity = 0.3f;
            itemShadow.PaintLayer += PublicConstants.LayerMinDiff;
            itemShadow.Position += itemIcon.Size * 0.06f;

            amountDotSize = bg.Size * 0.2f;
            
            buttonIcon = new Image(button.Icon, pos + bg.Size * new Vector2(1f), bg.Size * 0.6f, HUDLayer, true, true);
            buttonIcon.ChangePaintLayer(-1);

            buttonIconUpScale = buttonIcon.Size;
            buttonIconDownScale = buttonIconUpScale * 1.15f;

            Area = new VectorRect(pos, itemIcon.Size);
        }

        virtual public void update()
        {
            updateButtonScale();

            if (shakeTimer.MilliSeconds > 0)
            {
                amountDots.ParentPosition = shakeValue();
                if (shakeTimer.CountDown())
                {
                    amountDots.ParentPosition = Vector2.Zero;
                }
            }
        }
        protected void updateButtonScale()
        {
            buttonIcon.Size = button.IsDown ? buttonIconDownScale : buttonIconUpScale;
        }
       

        override public void emptyBump()
        {
            if (amountDots != null)
            {
                shakeTimer.Seconds = ShakeTimeSec;
            }
        }

        int prevAmount; int prevMaxAmount = int.MinValue;
        public void UpdateAmount(int amount, int maxAmount)
        {
            if (prevMaxAmount != maxAmount)
            {
                prevMaxAmount = maxAmount;
                prevAmount = int.MaxValue;
            }

            if (prevAmount != amount)
            {
                if (useDotsForAmount)
                {
                    clearAmount();

                    if (maxAmount > 0)
                    {
                        const int RowLength = 4;

                        Vector2 spacing = (bg.Size * 0.8f) / RowLength;

                        Vector2 startPos = new Vector2(bg.Xpos + bg.Width * 0.2f, bg.Ypos + bg.Height * 0.85f);
                        int rowIndex = 0;

                        amountDots = new Graphics.ImageGroupParent2D(maxAmount);//new List<Graphics.Image>(maxAmount);
                        for (int i = 0; i < maxAmount; ++i)
                        {
                            Graphics.Image dot = new Graphics.Image(SpriteName.LFHudAmmo, startPos, amountDotSize, HUDLayer, true);
                            dot.ChangePaintLayer(-2 - i);
                            dot.Xpos += rowIndex * spacing.X;
                            rowIndex++;
                            if (rowIndex >= RowLength)
                            {
                                rowIndex = 0;
                                startPos.Y += spacing.Y;
                            }

                            amountDots.Add(dot);
                        }
                    }

                    for (int i = 0; i < maxAmount; ++i)
                    {
                        Graphics.AbsDraw2D img = amountDots.images[i].image;
                        if (i == amount - 1)
                        { img.Size = amountDotSize * 1.4f; }
                        else
                        { img.Size = amountDotSize; }

                        if (i < amount)
                        {
                            img.SetSpriteName(SpriteName.LFHudAmmo);
                            if (i >= prevAmount)
                            {
                                addItem_BumpMotion(img);
                            }
                        }
                        else
                        {
                            img.SetSpriteName(SpriteName.LFHudAmmoEmpty);
                        }

                    }
                }

                else //use text
                {
                    if (amountText == null)
                    {
                        amountText = new Display.SpriteText(amount.ToString(),
                            new Vector2(bg.Xpos - bg.Width * 0.1f, bg.Ypos + bg.Height * 0.85f),
                            bg.Height * 0.38f, HUDLayer, new Vector2(0, 0), Color.White, true);
                    }
                    else
                    { amountText.Text(amount.ToString()); }
                }

                prevAmount = amount;
            }
        }

        public void clearAmount()
        {
            if (amountDots != null)
            {
                amountDots.DeleteMe();
                amountDots = null;
            }
            if (amountText != null)
            {
                amountText.DeleteMe();
                amountText = null;
            }
        }

        public void setIconTile(SpriteName tile)
        {
            itemIcon.SetSpriteName(tile);
            itemShadow.SetSpriteName(tile);
        }

        public override void DeleteMe()
        {
            itemIcon.DeleteMe();
            itemShadow.DeleteMe();
            bg.DeleteMe();
            buttonIcon.DeleteMe();
            clearAmount();
        }
    }

    class ItemHUD : SuitUseHUD
    {
        Vector2 bgPos;

        public ItemHUD(VectorRect previousHud, Players.Player p)
            :base(p, previousHud, SpriteName.NO_IMAGE, SpriteName.LFItemHudBG, p.inputMap.useItem, false)
        {
            bgPos = bg.Position;
        }

        public void Refresh(GO.Gadgets.Item item)
        {
            if (item.amount == 0 || item.Type == GO.Gadgets.ItemType.NUM_NON)
            {
                itemIcon.Visible = false;
                itemShadow.Visible = false;
            }
            else
            {
                setIconTile(item.Icon);
                itemIcon.Visible = true;
            }

            if (item.amount <= 1)
            {
                clearAmount();
            }
            else
            {
                UpdateAmount(item.amount, item.MaxAmount);
            }
        }

        override public void update()
        {
            updateButtonScale();
            if (shakeTimer.MilliSeconds > 0)
            {
                bg.Position = bgPos + shakeValue();
                if (shakeTimer.CountDown())
                {
                    bg.Position = bgPos;
                }
            }
        }

        override public void emptyBump()
        { shakeTimer.Seconds = ShakeTimeSec; }
    }

}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2
{
    class Health : AbsHUD
    {
        const float HeightAdj = 10;
        const float BarWidth = HUDStandardHeight * 5;
        const float BarWidthMinimal = HUDStandardHeight * 2;
        HUD.ProcessBar healthBar;

        Graphics.Image magicIcon;
        HUD.ProcessBar magicBar;
        Graphics.TextG magicText;
        Graphics.Image hourGlass;

        public Health(bool minimalSpace)
            :base(SpriteName.LFIconHart)
        {
            healthBar = new HUD.ProcessBar(Vector2.Zero,
                new Vector2(minimalSpace ? BarWidthMinimal : BarWidth, HUDStandardHeight - HeightAdj * PublicConstants.Twice), HUDLayer, Percent.Full, Color.Red);
            healthBar.BackgroundColor = Color.White;


            magicBar = new HUD.ProcessBar(Vector2.Zero,
                healthBar.Size, HUDLayer, Percent.Full, Color.Blue);
            magicBar.BackgroundColor = Color.White;

            magicIcon = new Image(SpriteName.ItemWaterFull, Vector2.Zero, icon.Size, HUDLayer, false);
            magicText = new TextG(LoadedFont.Lootfest, Vector2.Zero, new Vector2(0.6f), Align.CenterHeight, "+2", Color.White, HUDLayer);

            hourGlass = new Image(SpriteName.IconSandGlass, Vector2.Zero, new Vector2(32), HUDLayer, true);
            hourGlass.PaintLayer -= PublicConstants.LayerMinDiff * 3;

            if (!PlatformSettings.ViewUnderConstructionStuff)
            {
                magicBar.Visible = false;
                magicIcon.Visible = false;
                magicText.Visible = false;
            }
        }

        public override Vector2 Position
        {
            set
            {
                base.Position = value;
                healthBar.Position = new Vector2(icon.Right, value.Y + HeightAdj);
                magicBar.Position = healthBar.Position + new Vector2(healthBar.Width * 0.1f, healthBar.Height + 2);
                magicIcon.Position = new Vector2(magicBar.Right, healthBar.Center.Y - magicIcon.Height * 0.2f);
                magicText.Position = new Vector2(magicIcon.Right - 10, magicIcon.Center.Y + 10);

                hourGlass.Position = magicBar.Center;
            }
        }

        public void UpdateValue(Percent health, Percent magic, int magicBottles, bool refillingMagic)
        {
            healthBar.Value = health;
            magicBar.Value = magic;
            if (magicBottles <= 0)
                magicText.TextString = TextLib.EmptyString;
            else
                magicText.TextString = "+" + magicBottles.ToString();

            hourGlass.Visible = refillingMagic;
            if (refillingMagic)
            {
                magicBar.Color = Color.DarkGray;
                magicBar.BackgroundColor = Color.Gray;
                magicBar.Transparentsy = 0.5f;
            }
            else
            {
                magicBar.Color = Color.Blue;
                magicBar.BackgroundColor = Color.White;
                magicBar.Transparentsy = 1;
            }
        }
        override public float Width
        {
            get { return healthBar.Width + HUDStandardHeight; }
        }
        override public void DeleteMe()
        {
            base.DeleteMe();
            healthBar.DeleteMe(); 
        }
    }
}

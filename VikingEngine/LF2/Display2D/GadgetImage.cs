using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.LF2.GameObjects.Gadgets;
using VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget;

namespace VikingEngine.LF2
{
    class GadgetImage : Graphics.Parent
    {
        public const float IconSize = 45;//32;
        public const float Edge = 8;
        public static readonly Vector2 ItemImageSize = new Vector2(IconSize * 2 + Edge* PublicConstants.Twice, IconSize + Edge* PublicConstants.Twice);
        public static readonly Vector2 ShopImageSize = new Vector2(IconSize * 3 + Edge * PublicConstants.Twice, IconSize + Edge * PublicConstants.Twice);
        static readonly Vector2 EdgePercentSize = new Vector2(Edge / ItemImageSize.X, Edge / ItemImageSize.Y);
        static readonly Vector2 ValueBarPercentSize = new Vector2(5 / ItemImageSize.X, 6 / ItemImageSize.Y);

        public GadgetImage(ImageLayers layer, GameObjects.Gadgets.IGadget gadget, bool viewAmount)
        {
            parentImage = new Graphics.Point2D(Vector2.Zero, ItemImageSize);
            parentImage.Layer = layer;
            StandardRelation = new ChildRelation(true, false, false, true, true);

            switch (gadget.GadgetType)
            {
                case GameObjects.Gadgets.GadgetType.Goods:
                    GameObjects.Gadgets.Goods goods = (GameObjects.Gadgets.Goods)gadget;

                    SpriteName qualityTile;
                    switch (goods.Quality)
                    {
                        default:
                            qualityTile = SpriteName.IconQualityHigh;
                            break;
                        case GameObjects.Gadgets.Quality.Medium:
                            qualityTile = SpriteName.IconQualityMed;
                            break;
                        case GameObjects.Gadgets.Quality.Low:
                            qualityTile = SpriteName.IconQualityLow;
                            break;
                    }
                    amoutAndIcon(goods.Amount, viewAmount, goods.Icon, layer);
                    
                    if (GameObjects.Gadgets.Goods.QualityType(goods.Type))
                    {
                        Image quality = new Image(qualityTile, Vector2.Zero, Vector2.One * IconSize, layer);
                        AddChild(quality, new Vector2(0.38f, 0) + EdgePercentSize, Vector2.One, 0, 0);
                    }
                    break;
                case GadgetType.Item:
                    GameObjects.Gadgets.Item item = (GameObjects.Gadgets.Item)gadget;
                    amoutAndIcon(item.Amount, viewAmount, item.Icon, layer);
                    break;

                case GameObjects.Gadgets.GadgetType.Weapon:
                    AbsWeaponGadget2 weapon = (AbsWeaponGadget2)gadget;

                    Image wepIcon = new Image(weapon.Icon, Vector2.Zero, Vector2.One * IconSize, layer);
                    Vector2 wepIconRelPos = new Vector2(0.0f, 0) + EdgePercentSize;
                    AddChild(wepIcon, wepIconRelPos, Vector2.One, 0, -1);

                    if (weapon.Enchantment != GameObjects.Magic.MagicElement.NoMagic)
                    {
                        Image magicIcon = new Image(GameObjects.Magic.MagicLib.Icon(weapon.Enchantment), Vector2.Zero, wepIcon.Size * 1.4f, layer);
                        AddChild(magicIcon, wepIconRelPos, Vector2.One, 0, -2);
                    }

                    Image hands = new Image(GameObjects.Gadgets.GadgetLib.HandsIcon(weapon.Hands), Vector2.Zero, Vector2.One * IconSize, layer);
                    AddChild(hands, new Vector2(0.4f, 0.12f) + EdgePercentSize, Vector2.One, 0, 0);
                    break;

                case GadgetType.Armor:
                    Armor armor = (Armor)gadget;

                    Image armIcon = new Image(armor.Icon, Vector2.Zero, Vector2.One * IconSize, layer);
                    Vector2 armIconRelPos = new Vector2(0.0f, 0) + EdgePercentSize;
                    AddChild(armIcon, armIconRelPos, Vector2.One, 0, -1);

                    createValueBar(SpriteName.LFIconItemDefence, armor.Protection.TextValue / 10, 0, layer);
                    break;
                case GadgetType.Shield:
                    Shield shield = (Shield)gadget;

                    Image shieldIcon = new Image(shield.Icon, Vector2.Zero, Vector2.One * IconSize, layer);
                    Vector2 shiledIconRelPos = new Vector2(0.3f, 0) + EdgePercentSize;
                    AddChild(shieldIcon, shiledIconRelPos, Vector2.One, 0, -1);
                    break;
                case GadgetType.Jevelery:
                    //GameObjects.Gadgets. item = (GameObjects.Gadgets.Item)gadget;
                    Image jeveleryIcon = new Image(gadget.Icon, Vector2.Zero, Vector2.One * IconSize, layer);
                    Vector2 jeveleryIconRelPos = new Vector2(0.0f, 0) + EdgePercentSize;
                    AddChild(jeveleryIcon, jeveleryIconRelPos, Vector2.One, 0, -1);
                    break;
            }
        }

        void amoutAndIcon(int amount, bool viewAmount, SpriteName icon, ImageLayers layer)
        {
            if (viewAmount)
            {
                TextG a = new TextG(LoadedFont.Lootfest, Vector2.One, new Vector2(0.018f * IconSize), Align.CenterAll, amount.ToString(), Color.White, layer);//scale:Vector2.One * 0.6f
                AddChild(a, new Vector2(0.16f, 0.4f) + EdgePercentSize, Vector2.One, 0, 0);
            }
            Image img = new Image(icon, Vector2.Zero, Vector2.One * IconSize, layer);
            AddChild(img, new Vector2(0.3f, 0) + EdgePercentSize, Vector2.One, 0, -1);
        }

        void createValueBar(SpriteName barIcon, int value, int row, ImageLayers layer)
        {
            const int MaxCols = 5;
            if (value > MaxCols)
            {
                createValueBar(barIcon, MaxCols, row, layer);
                createValueBar(barIcon, value - MaxCols, row + 1, layer);
            }
            else
            {

                Vector2 iconSz = Vector2.One * 16;
                Vector2 startPos = new Vector2(0.56f, 0.15f);

                for (int i = 0; i < value; i++)
                {
                    Image img = new Image(barIcon, Vector2.Zero, iconSz, layer);
                    img.PaintLayer += (row * MaxCols + i) * PublicConstants.LayerMinDiff;
                    AddChild(img, new Vector2(startPos.X + i * ValueBarPercentSize.X, startPos.Y + row * ValueBarPercentSize.Y), Vector2.One, 0, -1);
                }
            }
        }
    }
}

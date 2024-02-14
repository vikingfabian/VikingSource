using Microsoft.Xna.Framework;
using System;
using VikingEngine.PJ.Display;

namespace VikingEngine.PJ.SpaceWar.GameObjects
{
    class ShopSquare
    {
        public const int TailCost = 1;
        public const int ShieldCost = 1;
        public const int KnifeCost = 1;
        public const int NoseBombCost = 2;
        public const float Size = 6f;
        public const float Spacing = Size;

        public static readonly Color ShopColor = new Color(199, 255, 255);

        public VectorRect area;
        Graphics.SplitScreenModel frame;
        public ShopSquareType type;
        public int price;

        public ShopSquare(ShopSquareType type, Vector2 center)
        {
            this.type = type;

            area = VectorRect.FromCenterSize(center, new Vector2(Size + SpaceShip.AbsBodySegment.BodyWidth));
            Vector3 pos = VectorExt.V2toV3XZ(center, -0.04f);
            const float ContentY = -0.08f;
            Vector3 iconPos = VectorExt.AddZ(pos, 0.15f * Size);
            iconPos.Y = ContentY;

            switch (type)
            {
                case ShopSquareType.AddTail:
                    {
                        price = TailCost;

                        Graphics.Mesh icon = new Graphics.Mesh(LoadedMesh.plane, iconPos, Size * 0.5f * new Vector3(2, 0, 1),
                            Graphics.TextureEffectType.Flat, SpriteName.spaceWarShopAddTail, Color.White);
                    }
                    break;
                case ShopSquareType.TailExpansion:
                    {
                        price = ShieldCost;

                        Graphics.Mesh icon = new Graphics.Mesh(LoadedMesh.plane, iconPos, new Vector3(Size * 0.5f),
                           Graphics.TextureEffectType.Flat, SpriteName.spaceWarShipTailShield, Color.Gray);

                    }
                    break;
                case ShopSquareType.TailKnife:
                    {
                        price = KnifeCost;

                        Graphics.Mesh icon = new Graphics.Mesh(LoadedMesh.plane, iconPos, new Vector3(Size * 0.5f, 0, Size * 0.2f),
                            Graphics.TextureEffectType.Flat, SpriteName.WhiteArea, Color.LightGreen);
                        icon.Opacity = 0.6f;
                    }
                    break;
                case ShopSquareType.NoseBomb:
                    {
                        price = NoseBombCost;

                        Graphics.Mesh icon = new Graphics.Mesh(LoadedMesh.plane, iconPos, new Vector3(Size * 0.5f),
                            Graphics.TextureEffectType.Flat, SpriteName.birdSpikeBall, Color.Blue);
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            frame = new Graphics.SplitScreenModel(LoadedMesh.plane, 
                pos, new Vector3(Size), Graphics.TextureEffectType.Flat,
                SpriteName.spaceWarShopSquareOn, Color.White);

            SpaceRef.go.shops.Add(this);
            SpriteText3D priceText = new SpriteText3D(price.ToString(), new Vector3(pos.X, ContentY, pos.Z - 0.15f * Size), Size * 0.16f, ShopColor);
        }

        public void updateVisuals(bool canBuy, int player)
        {
            frame.model(player).SetSpriteName(canBuy ? SpriteName.spaceWarShopSquareOn : SpriteName.spaceWarShopSquareOff);
        }

        
    }

    enum ShopSquareType
    {
        AddTail,
        TailExpansion,
        TailKnife,
        NoseBomb,
    }
}

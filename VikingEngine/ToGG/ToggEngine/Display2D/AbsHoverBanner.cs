using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class AbsHoverBanner : HUD.AbsButtonGui
    {
        SpriteName sprite, hoverSprite;

        public AbsHoverBanner(float rightPos, SpriteName sprite, SpriteName hoverSprite)
            : base(new HUD.ButtonGuiSettings(Color.White, HudLib.SelectionOutlineThickness, Color.White, Color.Black))
        {
            this.sprite = sprite;
            this.hoverSprite = hoverSprite;

            float width = Engine.Screen.IconSize;
            Vector2 sz = new Vector2(width, width / SpriteSheet.DoomBannerSize.X * SpriteSheet.DoomBannerSize.Y);

            this.area = new VectorRect(new Vector2(rightPos - sz.X, 0), sz);

            baseImage = new Graphics.Image(sprite, area.Position, area.Size, HudLib.ContentLayer, false);
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);

            if (enter)
            {
                baseImage.SetSpriteName(hoverSprite);
            }
            else
            {
                baseImage.SetSpriteName(sprite);
            }
        }
    }

    abstract class AbsQuestBanner : AbsHoverBanner
    {
        public AbsQuestBanner(float rightPos)
            : base(rightPos, SpriteName.QuestBanner, SpriteName.QuestBannerHighlight)
        { }
    }
}

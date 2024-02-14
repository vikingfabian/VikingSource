using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Display
{
    class GuiCardCaptureIcon: AbsGuiButton
    {
        public GuiCardCaptureIcon(CardType card, int count, GuiLayout layout)
            : base(null, null, false, layout)
        {
            background.Size = size;

            float borderSize = size.X / 8;
            //CardGame.cgRef.cards.Get(card).Minion.cardPortrait
            Image iconImg = new Image(SpriteName.NO_IMAGE, new Vector2(borderSize), size - new Vector2(2 * borderSize), layout.Layer - 1);
            

            SpriteText countText = new SpriteText(count.ToString(), iconImg.BottomRight, iconImg.Height * 0.4f, layout.Layer - 2,
                Vector2.One, Color.White, true);

            this.AddAndUpdate(iconImg);

            foreach (var img in countText.letters)
            { this.AddAndUpdate(img); }
        }

        protected override GuiMemberSizeType SizeType { get { return GuiMemberSizeType.SquareDoubleSize; } }
    }
}

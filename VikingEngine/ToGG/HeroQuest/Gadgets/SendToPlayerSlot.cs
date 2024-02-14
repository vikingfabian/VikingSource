using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class SendToPlayerButton : Button
    {
        public SendToPlayerSlot slot;
        
        public SendToPlayerButton(VectorRect area, AbsHQPlayer player)
            : base(area, HudLib.BackpackLayer, ButtonTextureStyle.StandardEdge)
        {
            grayImagesOnDisable = true;
            slot = new SendToPlayerSlot(player);
            VectorRect imgAr = area;
            imgAr.AddPercentRadius(-0.1f);
            Graphics.ImageAdvanced unitImage = new Graphics.ImageAdvanced(
                SpriteName.NO_IMAGE, imgAr.Position, imgAr.Size, HudLib.BackpackLayer - 1,
                false);
            player.HeroUnit.data.modelSettings.IconSource(unitImage, true);
            imagegroup.Add(unitImage);
        }

        protected override void createToolTip()
        {
            base.createToolTip();

            var richbox = new List<AbsRichBoxMember>{
                new RichBoxText("Give items to " + slot.player.HeroUnit.data.Name),
            };

            HudLib.AddTooltipText(tooltip, richbox,
                Dir4.N, this.area, null);
        }
    }

    class SendToPlayerSlot : ItemSlot
    {
        public Players.AbsHQPlayer player;
        public SendToPlayerSlot(Players.AbsHQPlayer player)
            : base(SlotType.SendToPlayer)
        {
            this.player = player;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display3D
{
    class UnitMessageRichbox : AbsUnitMessage
    {
        VectorRect area;
        List<AbsRichBoxMember> rbMembers;

        public UnitMessageRichbox(AbsUnit unit, string text, bool startNow = true)
            :this(unit, new List<AbsRichBoxMember> { new RbText(text) }, startNow)
        { }

        public UnitMessageRichbox(AbsUnit unit, SpriteName icon, string text, bool startNow = true)
            : this(unit, AbsRichBoxMember.FromIconText(icon, text), startNow)
        { }

        public UnitMessageRichbox(AbsUnit unit, List<AbsRichBoxMember> rbMembers, bool startNow)
            : base(unit)
        {
            this.rbMembers = rbMembers;
            if (startNow)
            {
                start();
            }
        }

        public override void start()
        {
            base.start();

            const int BubbleTextureEdge = 4;
            const int NineSplitScale = 2;

            Vector2 edge = new Vector2(BubbleTextureEdge * NineSplitScale);
            
            RichBoxGroup richBox = new RichBoxGroup(edge,
                HudLib.UnitMessageRichBoxSett.breadIconHeight * 8, ImageLayers.Top0, 
                HudLib.UnitMessageRichBoxSett, rbMembers, true, false);

            area = richBox.maxArea;
            area.Position = Vector2.Zero;
            area.Size += edge * 2f;

            var bgTexture = new HUD.NineSplitAreaTexture(SpriteName.speachBobbleTexture, 1, BubbleTextureEdge,
                area, NineSplitScale, true, ImageLayers.Bottom0, true, false);
            
            Graphics.Image bobbleArrow = new Graphics.Image(SpriteName.speachBobbleArrow,
                area.CenterBottom, new Vector2(18 * NineSplitScale), ImageLayers.Lay0, false, false);
            bobbleArrow.Ypos -= 3 * NineSplitScale + 1;

            area.Height += 14 * NineSplitScale;

            model = new Graphics.RenderTargetBillboard(basePos, area.Height * 0.003f, true);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;

            model.images = richBox.images;
            model.images.AddRange(bgTexture.images);
            model.images.Add(bobbleArrow);

            model.createTexture(area.Size, model.images, null);
            model.setModelSizeFromTexHeight();

            completeInit(unit);
        }

        public override float MessageHeight => model.ScaleY;
    }
}

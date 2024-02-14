using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.ToggEngine.Display3D
{
   
    class UnitMessageItem : AbsUnitMessage
    {
        const float Height = 0.25f;

        SpriteName icon;
        string item;

        public UnitMessageItem(AbsUnit unit, HeroQuest.Gadgets.AbsItem item, bool startNow)
            : base(unit)
        {
            icon = item.Icon;
            this.item = item.NameAndCount();

            if (startNow)
            {
                start();
            }
        }

        public override void start()
        {
            base.start();

            const float TexHeight = 64;

            Graphics.Image backpackIcon = new Graphics.Image(SpriteName.cmdAddToBackpack,
                Vector2.Zero, new Vector2(TexHeight), ImageLayers.Lay0, false, false);

            Graphics.Image itemIcon = new Graphics.Image(icon,
                VectorExt.AddX(backpackIcon.position, TexHeight * 1.2f),
                new Vector2(TexHeight), ImageLayers.Lay0, false, false);
            Graphics.Text2 name = new Graphics.Text2(item, LoadedFont.Regular,
                itemIcon.RightCenter, TexHeight * 0.8f, Color.LightYellow,
                ImageLayers.Lay0, null, false);
            name.OrigoAtCenterHeight();

            float w = MathExt.Ceiling(name.MeasureRightPos());

            model = new Graphics.RenderTargetBillboard(basePos,
                Height, true);
            model.FaceCamera = false;
            model.Rotation = toggLib.PlaneTowardsCam;

            model.images = new List<Graphics.AbsDraw>
            {
                backpackIcon,
                itemIcon,
                name
            };
            model.createTexture(new Vector2(w, TexHeight), model.images, null);
            model.setModelSizeFromTexHeight();

            completeInit(unit);
        }

        public override float MessageHeight => Height;
    }

    
}

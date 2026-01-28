using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VikingEngine.CardDesign.CardGraphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.CardDesign.CardData
{
    class ResourcePool
    {
        public Resource resource;
        public Number startCount = new Number(0);
        public Number maxCount = Number.Endless;

        AbsAction emptyEvent = null;
        AbsAction fullEvent = null;

    }

    class ResourceList : List<Resource>
    {
        public ResourceList()
            : base(4)
        { }


        public bool HasValue
        {
            get
            {
                foreach (var item in this)
                {
                    if (item.amount.value > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void ToMenu(RichBoxContent content)
        {
            if (HasValue)
            {
                //for (DefaultResourceType type = 0; type < DefaultResourceType.NUM_NONE; ++type)
                //{
                //    int value = Get(type);
                //    if (value != 0)
                //    {
                //IconName.Resource(type, out var icon, out _);
                foreach (var item in this)
                {
                    item.ToMenu(content);
                    content.space(2);
                    //}
                }
            }
            else
            {
                content.Add(new RbText("None"));
            }
        }

        public void ToCard(List<Graphics.AbsDraw> images, Vector2 pos, float width)
        {
            float startX = pos.X;
            float right = pos.X + width;
            foreach (var item in this)
            {
                var tag = item.Get();
                //IconName.Resource(type, out var icon, out _);
                Graphics.Image iconImg = new Graphics.Image(tag.icon, pos, new Vector2(CardFace.IconSize), ImageLayers.Top4, false, false);
                var valueText = new SpriteText(item.amount.ToString(), iconImg.Area.PercentToPosition(0.7f, 0.7f), CardFace.IconSize * 0.6f, ImageLayers.Top0, new Vector2(0.5f), Color.White);

                pos.X += Math.Max(CardFace.IconSize * 1.2f, CardFace.IconSize * 0.8f + valueText.size.X * 0.5f);
                if (pos.X > right)
                {
                    pos.X = startX;
                    pos.Y += CardFace.IconSize * 1.2f;
                }

                images.Add(iconImg);
                images.AddRange(valueText.letters);
            }

        }

        public int CostProperty(object tag, bool set, int value)
        {
            Id id = (Id)tag;

            for (int index = 0; index < Count; index++)
            {
                if (this[index].id == id)
                {
                    if (set)
                    {
                        this[index].Set(value);
                    }
                    return this[index].amount.value;
                }
            }

            return 0;
        }
    }
}

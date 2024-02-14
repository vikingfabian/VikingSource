using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2
{
    class ItemHUD : AbsCollectionHUD
    {
        float visibleTime = 0;
        bool fadeState = false;

        public ItemHUD(GameObjects.Gadgets.GoodsType type)
            : base(GameObjects.Gadgets.GadgetLib.GadgetIcon(type))
        {
            Visible = false;
        }

        public void BumpVisibility(int amount)
        {
            if (!icon.Visible)
            {
                AddToUpdateList(true);
            }
            fadeState = false;
            Visible = true;
            Transparentsy = 1;
            this.text.TextString = amount.ToString();
            visibleTime = 10000;
        }
        public override void Time_Update(float time)
        {
            if (fadeState)
            {
                Transparentsy -= 0.2f;
                if (Transparentsy <= 0)
                {
                    AddToUpdateList(false);
                    Visible = false;
                }
            }
            else
            {
                visibleTime -= time;
                if (visibleTime <= 0)
                {
                    fadeState = true;
                    
                }
            }
        }

        float Transparentsy
        {
            set
            {
                icon.Transparentsy = value;
                text.Transparentsy = value;
            }
            get
            {
                return icon.Transparentsy;
            }
        }
    }
}

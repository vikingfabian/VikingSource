using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest
{
    class MountHealthbar : HealthBar2
    {
        Graphics.Image headIcon;

        public MountHealthbar(Vector2 startPos, int maxHealth)
            :base(startPos + new Vector2(HUDStandardHeight * 0.6f), maxHealth)
        {
            layer--;

            headIcon = new Graphics.Image(SpriteName.LfMountHorseIcon, Area.Position - new Vector2(HUDStandardHeight * 0.9f, 0f), 
                new Vector2(HUDStandardHeight * 0.9f), layer, false);
        }

        protected override SpriteName heartIcon
        {
            get
            {
                return SpriteName.LFHudMountHeart;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            headIcon.DeleteMe();
        }
    }
}

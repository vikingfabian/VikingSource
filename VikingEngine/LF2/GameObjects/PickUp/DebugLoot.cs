using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    class DebugLoot : Coin
    {
        public DebugLoot(Vector3 position)
            : base(position, 0)
        {
        }

        protected override Gadgets.IGadget item
        {
            get
            {
                return new GameObjects.Gadgets.Shield(Gadgets.ShieldType.Round);
            }
        }
    }
}

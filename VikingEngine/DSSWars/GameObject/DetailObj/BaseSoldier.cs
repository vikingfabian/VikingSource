using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{

    class BaseSoldier : AbsSoldierUnit
    {
        public override bool IsShipType()
        {
            return false;
        }

        protected override DetailUnitModel initModel(bool bannerman)
        {
            updateGroudY(true);
            if (bannerman)
            {
                return new BannerManModel(this);
            }
            else
            {
                return new SoldierUnitAdvancedModel(this);
            }
        }

        public override bool IsSingleTarget()
        {
            return group.soldiers.Count == 1;
        }

        
    }
}

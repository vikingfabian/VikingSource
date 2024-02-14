using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander.Players;

namespace VikingEngine.ToGG
{
    class BoardDesignPlayer : LocalPlayer
    {
        public BoardDesignPlayer()
            : base(Engine.XGuide.LocalHost, 0, new Data.PlayerSetup())
        {
            mapControls.setMapselectionVisible(true);
            
        }

        public override void onNewTile()
        {
        }

        public override void Update()
        {
            mapControls.updateMapMovement(false);
           
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.Process
{
    class TakeDamage : OneTimeTrigger
    {
        Players.Player player;

        public TakeDamage(Players.Player player)
            :base(false)
        {
            this.player = player;
            AddToOrRemoveFromUpdateList(true);
        }
        public override void Time_Update(float time)
        {
            player.HandleDamage();
        }
    }
}

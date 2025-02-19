using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    class SoldierControls
    {
        List<SoldierGroup> groups;
        public SoldierControls(List<SoldierGroup> groups)
        { 
        this.groups = groups;
        }

        public void mapExecute(LocalPlayer player)
        {
            if (player.mapControls.armyMayAttackHoverObj())
            {
                var target = player.mapControls.hover.obj.GetSoldierGroup();
                foreach (SoldierGroup group in groups)
                {
                    new AttackCommand(group, target);
                }
            }
            else
            {
                foreach (SoldierGroup group in groups)
                {
                    new MoveCommand(group, player.mapControls.mousePosition);
                }
            }
        }
    }
}

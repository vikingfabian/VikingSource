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
                    new AttackCommand(group, target, false);
                }
                new AttackHereAnimation(target, player.playerData.view.ScreenIndex);
            }
            else
            {
                var pos = WP.SubtileToWorldPosXZgroundY_Centered(player.mapControls.subTilePosition);
                foreach (SoldierGroup group in groups)
                {
                    new MoveCommand(group, pos, false);

                    if (group.InGuardPost())
                    {
                        new GuardPostTransform(group, -1, false);
                    }

                    if (player.mapControls.hover.subTile.selectTileResult == SelectTileResult.Wall)
                    {
                        new EnterPostCommand(group, player.mapControls.hover.subTile.subTilePos, true);
                    }
                }
                new MoveHereAnimation(pos);
            }
        }
    }
}

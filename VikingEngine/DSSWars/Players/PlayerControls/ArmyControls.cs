using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Map;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Players
{
    class ArmyControls
    {
        LocalPlayer player;
        GameObject.Army army;
        PathVisuals pathVisuals;

        public bool newSquare = false;

        PathFindState pathState = PathFindState.None;
        WalkingPath path = null, newPath = null;

        public ArmyControls(LocalPlayer player, GameObject.Army army)
        {
            this.player = player;
            pathVisuals = new PathVisuals(player.playerData.localPlayerIndex);
            this.army = army;
            newSquare = true;
        }

        public void update()
        {
            if (army.isDeleted)
            {
                player.clearSelection();
            }
            else
            {
                if (player.mapControls.onNewTile)
                {
                    newSquare = true;
                }

                if (pathState != PathFindState.None)
                {
                    if (pathState == PathFindState.NewPath)
                    {
                        path = newPath;
                        newPath = null;

                        pathVisuals.refresh(path, false, false);
                    }
                    else
                    {
                        path = null;
                        pathVisuals.DeleteMe();
                    }

                    pathState = PathFindState.None;
                }

                if (player.input.Stop.DownEvent)
                {
                    SoundLib.orderstop.Play();
                    army.haltMovement();
                }
            }
        }

        public void asynchUpdate()
        {
            if (newSquare)
            {
                newSquare = false;

                if (army.tilePos != player.mapControls.tilePosition &&
                    DssRef.world.tileGrid.InBounds(player.mapControls.tilePosition))
                {
                    PathFinding pf = DssRef.state.pathFindingPool.Get();
                    {
                        newPath = pf.FindPath(army.tilePos, army.rotation, player.mapControls.tilePosition,
                            false);
                    }
                    DssRef.state.pathFindingPool.Return(pf);

                    pathState = PathFindState.NewPath;
                }
                else
                {
                    pathState = PathFindState.NoPath;
                }
            }
        }

        public void clearState()
        {
            pathVisuals.DeleteMe();
        }

        public void moveOrderEffect()
        {
            new PathFlashEffect(pathVisuals);
            pathVisuals = new PathVisuals(player.playerData.localPlayerIndex);
        }

        public void mapExecute()
        {
            if (player.mapControls.hover.obj != null &&
                player.mapControls.hover.obj.GetFaction() != player.faction)
            {
                SoundLib.ordermove.Play();
                army.Order_Attack(player.mapControls.hover.obj.RelatedMapObject());
            }
            else
            {
                SoundLib.ordermove.Play();
                army.Order_MoveTo(player.mapControls.tilePosition);
            }
        }

        enum PathFindState
        {
            None,
            NewPath,
            NoPath,
        }

    }
}

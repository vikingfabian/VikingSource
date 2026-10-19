using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Players
{
    class ArmyControlsMember
    {
        public GameObject.Army army;
        public PathVisuals pathVisuals;
        public WalkingPath path_old = null, newPath_old = null;

        PathFindState pathState = PathFindState.None;
        public bool isAlive = true;
        LocalPlayer player;
        Graphics.AbsVoxelObj detailBanner;

        public ArmyControlsMember(GameObject.Army army)
        {
            this.army = army;
        }

        public void initControls(LocalPlayer player)
        {
            this.player = player;
            pathVisuals = new PathVisuals(player.playerData.localPlayerIndex);
        }

        public void update()
        {
            if (isAlive)
            {
                if (army.isDeleted)
                {
                    pathVisuals.DeleteMe();
                    isAlive = false;
                }
                else if (pathState != PathFindState.None)
                {
                    if (pathState == PathFindState.NewPath)
                    {

                        path_old = newPath_old;
                        newPath_old = null;

                        pathVisuals.refresh(path_old, false, false);
                    }
                    else
                    {

                        path_old = null;
                        pathVisuals.DeleteMe();
                    }

                    pathState = PathFindState.None;
                }

                if (player.mapLayersManager.current.DrawNearLayer)
                {
                    if (detailBanner == null)
                    {

                        detailBanner = army.pfaction.GetFaction().AutoLoadModelInstance_batched(
                             LootFest.VoxelModelName.armystand_detail, 0.3f);
                        //detailBanner.AddToRender(DrawGame.UnitDetailLayer);
                    }

                    detailBanner.position = army.position;
                }
                
            }
        }

        public void asynchUpdate(LocalPlayer player)
        {
            if (pathState == PathFindState.None && isAlive)
            {
                if (army.maptilePos != player.gameControls.map.sumtilePosition &&
                    DssRef.world.tileGrid.InBounds(player.gameControls.map.sumtilePosition))
                {
                    PathFinding pf = DssRef.world.pathFindingPool.GetPf();
                    {
                        newPath_old = pf.FindPath(DssRef.state.PathThreadCount(), army.maptilePos, conv.ToDir8_INT(army.rotation), player.gameControls.map.sumtilePosition,
                            false);
                    }
                    DssRef.world.pathFindingPool.Return(pf);

                    pathState = PathFindState.NewPath;
                }
                else
                {
                    pathState = PathFindState.NoPath;
                }
            }
        }

        public void DeleteMe()
        {
            pathVisuals.DeleteMe();
            detailBanner?.DeleteMe();
        }

        enum PathFindState
        {
            None,
            NewPath,
            NoPath,
        }
    }
}

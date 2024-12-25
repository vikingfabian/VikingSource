using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{    

    class ArmyControls
    {
        LocalPlayer player;
        
        public bool newSquare = false;

        
        List<ArmyControlsMember> armies;
        
        public ArmyControls(LocalPlayer player, List<AbsMapObject> list)
        {
            this.player = player;
            armies = new List<ArmyControlsMember>(list.Count);
            foreach (AbsMapObject item in list)
            {
                armies.Add(new ArmyControlsMember(player, item.GetArmy()));
            }

            newSquare = true;
        }

        public void update()
        {
            
            if (player.mapControls.onNewTile)
            {
                newSquare = true;
            }

            bool alive = false;

            foreach (var m in armies)
            { 
                m.update();
                alive |= m.isAlive;
            }

            if (!alive)
            {
                player.clearSelection();
                return;
            }

            if (player.input.Stop.DownEvent)
            {
                SoundLib.orderstop.Play();
                foreach (var m in armies)
                {
                    if (m.isAlive)
                    {
                        m.army.haltMovement();
                    }
                }

            }
            
        }

        public void asynchUpdate()
        {
            if (newSquare)
            {
                newSquare = false;

                foreach (var m in armies)
                {
                    m.asynchUpdate(player);
                }
            }
        }

        public void clearState()
        {
            foreach (var m in armies)
            {
                m.pathVisuals.DeleteMe();
            }
            
        }

        public void moveOrderEffect()
        {
            foreach (var m in armies)
            {
                if (m.isAlive)
                {
                    new PathFlashEffect(m.pathVisuals);
                    m.pathVisuals = new PathVisuals(player.playerData.localPlayerIndex);
                }
            }
                
        }

        public void mapExecute()
        {
            if (player.mapControls.armyMayAttackHoverObj())
            {
                var target = player.mapControls.hover.obj.RelatedMapObject();
                if (target != null)
                {
                    SoundLib.ordermove.Play();
                    foreach (var m in armies)
                    {
                        if (m.isAlive)
                        {

                            m.army.Order_Attack(target);
                        }
                    }
                }
            }
            else
            {
                SoundLib.ordermove.Play();
                foreach (var m in armies)
                {
                    if (m.isAlive)
                    {
                        m.army.Ai_Order_MoveTo(player.mapControls.tilePosition);
                    }
                }
            }
        }
    }

    class ArmyControlsMember
    {
        public GameObject.Army army;
        public PathVisuals pathVisuals;
        public WalkingPath path = null, newPath = null;

        PathFindState pathState = PathFindState.None;

        public bool isAlive = true;

        public ArmyControlsMember(LocalPlayer player, GameObject.Army army)
        {
            pathVisuals = new PathVisuals(player.playerData.localPlayerIndex);
            this.army = army;
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
            }
        }

        public void asynchUpdate(LocalPlayer player)
        {
            if (pathState == PathFindState.None && isAlive)
            {
                if (army.tilePos != player.mapControls.tilePosition &&
                    DssRef.world.tileGrid.InBounds(player.mapControls.tilePosition))
                {
                    PathFinding pf = DssRef.state.pathFindingPool.GetPf();
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

        enum PathFindState
        {
            None,
            NewPath,
            NoPath,
        }
    }
}

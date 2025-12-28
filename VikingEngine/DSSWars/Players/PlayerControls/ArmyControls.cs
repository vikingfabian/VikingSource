using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Players
{    

    class ArmyControls
    {
        LocalPlayer player;
        
        public bool newSquare = false;
  
        ArmyCollection collection;

        public ArmyControls(LocalPlayer player, ArmyCollection collection)
        {
            this.player = player;
            this.collection = collection;
           
            foreach (var item in collection.objects)
            {
                item.initControls(player);
            }

            newSquare = true;
        }

        public void update()
        {
            
            if (player.gameControls.map.onNewTile)
            {
                newSquare = true;
            }

            bool alive = false;

            foreach (var m in collection.objects)
            { 
                m.update();
                alive |= m.isAlive;
            }

            if (!alive)
            {
                player.gameControls.clearSelection();
                return;
            }

            if (player.gameControls.input.StopStart.DownEvent)
            {
                SoundLib.orderstop.Play();
                foreach (var m in collection.objects)
                {
                    if (m.isAlive)
                    {
                        m.army.haltMovement();
                    }
                }

            }
            
        }

        public void asynchPathUpdate()
        {
            if (newSquare)
            {
                newSquare = false;

                lock (collection.objects)
                {
                    foreach (var m in collection.objects)
                    {
                        m.asynchUpdate(player);
                    }
                }
            }
        }

        public void clearState()
        {
            //foreach (var m in collection.objects)
            //{
            //    m.pathVisuals.DeleteMe();
            //}
            collection.DeleteMembers(false);
        }

        public void moveOrderEffect()
        {
            foreach (var m in collection.objects)
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
            if (player.gameControls.map.armyMayAttackHoverObj())
            {
                var target = player.gameControls.map.hover.obj.RelatedMapObject();
                if (target != null)
                {
                    if (DssRef.diplomacy.GetRelation(player.faction, target.GetFaction()).InWar())
                    {
                        mapExecuteAttack(target);
                    }
                    else
                    {
                        new PopMenu(player, new HUD.RichBox.RbAction1Arg<AbsMapObject>(mapExecuteAttack, target));
                    }
                    
                }
            }
            else
            {
                SoundLib.ordermove.Play();
                //int radius = 0;
                ForXYEdgeLoop nextPlacementLoop = new ForXYEdgeLoop(player.gameControls.map.tilePosition, player.gameControls.map.tilePosition);

                foreach (var m in collection.objects)
                {
                    if (m.isAlive)
                    {
                        bool continueLoop = nextPlacementLoop.Next();
                        if (!continueLoop)
                        {
                            nextPlacementLoop.ExpandRadius();
                        }
                        m.army.Ai_Order_MoveTo(nextPlacementLoop.Position);
                        
                    }
                }

                moveOrderEffect();
            }
        }

        public void mapExecuteAttack(AbsMapObject target)
        {
            if (target != null)
            {
                SoundLib.ordermove.Play();
                foreach (var m in collection.objects)
                {
                    if (m.isAlive)
                    {
                        m.army.Order_Attack(target);
                    }
                }
            }

            moveOrderEffect();
        }
    }

   
}

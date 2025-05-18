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
            
            if (player.gameControls.mapControls.onNewTile)
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
            if (player.gameControls.mapControls.armyMayAttackHoverObj())
            {
                var target = player.gameControls.mapControls.hover.obj.RelatedMapObject();
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
            }
            else
            {
                SoundLib.ordermove.Play();
                foreach (var m in collection.objects)
                {
                    if (m.isAlive)
                    {
                        m.army.Ai_Order_MoveTo(player.gameControls.mapControls.tilePosition);
                    }
                }
            }
        }
    }

   
}

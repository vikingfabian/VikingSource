using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    class AbsItemDragNDrop
    {
        protected Input.MouseDragHelper mouseDrag;
        protected Vector2 mousePosDiff;
        public AbsItem item = null;

        public AbsItemDragNDrop()
        {
            mouseDrag = new Input.MouseDragHelper();
        }

        public Input.MouseDragState DragState
        {
            get { return mouseDrag.state; }
        }
    }

    class QuickItemDragNDrop : AbsItemDragNDrop
    {
        ItemSlot slot;       

        List<IntVector2> targetSquares;
        public bool availableTile = false;

        public QuickItemDragNDrop(ItemSlot slot)
            :base()
        {
            this.slot = slot;
            //mouseDrag = new Input.MouseDragHelper();
        }

        public bool update(LocalPlayer player)
        {
            bool dragDropEvent = mouseDrag.update();

            if (dragDropEvent)
            {
                if (mouseDrag.state == Input.MouseDragState.Drag)
                {                    
                    item = slot.pullItem();
                    mousePosDiff = item.itemImage.Position - mouseDrag.mouseDownPos;
                    item.itemImage.Visible = true;

                    bool attackTarget;
                    targetSquares = item.quickUse_TargetSquares(player.HeroUnit, out attackTarget);

                    if (attackTarget)
                    {
                        player.mapControls.SetAvailableTiles(null, targetSquares);
                    }
                    else
                    {
                        player.mapControls.SetAvailableTiles(targetSquares);
                    }

                    Display3D.UnitStatusGuiSettings? unitsGui = item.targetUnitsGui();
                    if (unitsGui == null)
                    {
                        hqRef.playerHud.unitsGui.clear();
                    }
                    else
                    {
                        hqRef.playerHud.unitsGui.refresh(toggRef.board.collectUnits(targetSquares), unitsGui.Value);
                    }
                }
                else if (mouseDrag.state == Input.MouseDragState.Drop)
                {
                    updateAvailable(player);

                    if (availableTile)
                    {
                        item.quickUse(player, player.mapControls.selectionIntV2);
                    }
                    else
                    {
                        slot.addItem(item);
                    }
                    return true;
                }
            }

            if (mouseDrag.state == Input.MouseDragState.Drag)
            {
                item.itemImage.Position = Input.Mouse.Position + mousePosDiff;
                if (player.mapControls.isOnNewTile)
                {
                    updateAvailable(player);
                };
            }
            
            return false;
        }

        void updateAvailable(LocalPlayer player)
        {
            availableTile = targetSquares.Contains(player.mapControls.selectionIntV2);
        }       
    }

    
}

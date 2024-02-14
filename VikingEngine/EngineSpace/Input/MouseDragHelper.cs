using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Input
{
    class MouseDragHelper
    {
        const float DragToPickDistance = 10;
        const float MaxClickTimeMs = 200;

        public MouseDragState state = MouseDragState.Start;
        TimeStamp mouseDownTime;
        public Vector2 mouseDownPos;

        public bool dragOnKeyDown;
        //public bool pickUpEvent = false;
        //public bool dropEvent = false;

        public MouseDragHelper()
        {
            mouseDownTime = TimeStamp.Now();
            mouseDownPos = Input.Mouse.Position;
        }

        /// <returns>New State</returns>
        public bool update()
        {
            bool newState = false;
            switch (state)
            {
                case MouseDragState.Start:
                    if (Input.Mouse.ButtonUpEvent(MouseButton.Left))
                    {
                        state = MouseDragState.Drag;
                        dragOnKeyDown = false;
                        //pickUpEvent = true;
                        newState = true;
                    }
                    else if ((mouseDownPos - Input.Mouse.Position).Length() >= DragToPickDistance ||
                        mouseDownTime.msPassed(MaxClickTimeMs))
                    {
                        state = MouseDragState.Drag;
                        dragOnKeyDown = true;
                        //pickUpEvent = true;
                        newState = true;
                    }
                    break;

                case MouseDragState.Drag:
                    //fix to sweeping click
                    if (Input.Mouse.ButtonUpEvent(MouseButton.Left) &&
                        mouseDownTime.msPassed(90) == false)
                    {
                        //Is sweeping click, revert to click-pick
                        dragOnKeyDown = false;
                    }


                    bool dropEvent = false;
                    if (dragOnKeyDown)
                    {
                        dropEvent = Input.Mouse.ButtonUpEvent(MouseButton.Left);
                    }
                    else
                    {
                        dropEvent = Input.Mouse.ButtonDownEvent(MouseButton.Left);
                    }

                    if (dropEvent)
                    {
                        state = MouseDragState.Drop;
                        dropEvent = true;
                        newState = true;
                    }
                    break;
            }

            return newState;
        }

        public void replaceItemOnDrop()
        {
            state = MouseDragState.Drag;
            dragOnKeyDown = false;

            mouseDownPos = Input.Mouse.Position;
        }
    }

    enum MouseDragState
    {
        Start,
        Drag,
        //IsClick,
        Drop,
    }
}

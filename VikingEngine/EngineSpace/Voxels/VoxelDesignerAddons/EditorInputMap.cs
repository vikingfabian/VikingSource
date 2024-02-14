using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Input;

namespace VikingEngine.Voxels
{
    class EditorInputMap
    {
        const float DefaultPencilMoveSpeed = 0.01f;
        const float ZoomSpeed = 0.05f;
        const float RotateCamSpeed = 0.003f;

        public IDirectionalMap moveXZ;
        public IDirectionalMap cameraXMoveY;
        public IButtonMap toggleCameraMode;
        public IDirectionalMap cameraZoom;
        public IButtonMap draw;
        public IButtonMap erase;
        public IButtonMap select;
        public IButtonMap colorPick;
        public IButtonMap cancel;

        public IButtonMap mirrorX;
        public IButtonMap mirrorY;

        public IButtonMap undo;
        public IButtonMap previous;
        public IButtonMap next;

        public IButtonMap OpenClose;

        public IButtonMap keyboardYmovementToggel = new KeyboardButtonMap(Keys.LeftShift);
        public IButtonMap mouseUseButton = new MouseButtonMap(MouseButton.Left);
        public IButtonMap mouseToolMenu = new KeyboardButtonMap(Keys.LeftControl); //


        float movePencilTime = 0;
        public PaintFillType mouseTool = PaintFillType.Fill;

        public bool useMouseInput;

        public bool stampSelection()
        {
            if (useMouseInput)
            {
                return Input.Mouse.ButtonDownEvent(MouseButton.Left);
            }
            else
            {
                return draw.DownEvent;
            }
        }

        public bool cancelSelection()
        {
            if (useMouseInput)
            {
                return Input.Mouse.ButtonDownEvent(MouseButton.Right);
            }
            else
            {
                return cancel.DownEvent;
            }
        }

        public Vector3 pencilMovement(int playerIndex, float settingsMoveSpeed)
        {
            if (toggleCameraMode.IsDown)
            {
                return Vector3.Zero;
            }

            Vector2 xz = moveXZ.direction;

            Vector3 result = new Vector3(xz.X, 0, xz.Y);

            if (!toggleCameraMode.IsDown && cameraXMoveY != null)
            {
                result.Y = -cameraXMoveY.direction.Y;
            }
            
            if (VectorExt.HasValue(result))
            {
                movePencilTime += Ref.DeltaGameTimeMs;

                result *= controllerMoveSpeed(settingsMoveSpeed) * Ref.DeltaTimeMs;
            }
            else
            {
                movePencilTime = 0;
            }

            if (Input.Mouse.bMoveInput && !toggleCameraMode.IsDown)
            {
                float zoom = Engine.XGuide.GetPlayer(playerIndex).view.Camera.CurrentZoom;
                Vector2 move = Input.Mouse.RealMoveDistance * (zoom * 0.003f + 0.006f);

                result.X += move.X;
                result.Z += move.Y;
            }

            if (keyboardYmovementToggel.IsDown)
            {
                result.Y -= result.Z;
                result.Z = 0;
            }

            return result;
        }

        //Vector2 mouseWorldMove(int playerIndex)
        //{
        //    if (Input.Mouse.bMoveInput)
        //    {
        //        float TestLength = Input.Mouse.RealMoveDistance.Length();

        //        var view = Engine.XGuide.GetPlayer(playerIndex).view;
        //        bool hasValue1;
        //        bool hasValue2;
        //        Plane movePlane = new Plane(-view.Camera.lookDir(), view.Camera.CurrentZoom);
        //        Vector3 testFrom = view.Camera.CastRayInto3DPlane(Engine.Screen.CenterScreen,
        //            view.Viewport, movePlane, out hasValue1);
        //        Vector3 testTo = view.Camera.CastRayInto3DPlane(Engine.Screen.CenterScreen + VectorExt.V2FromX(TestLength),
        //            view.Viewport, movePlane, out hasValue2);

        //        //if (hasValue1 && hasValue2 == false)
        //        //{
        //        //    return Vector2.Zero;
        //        //}

        //        float pixelToMoveLegth = (testTo - testFrom).Length() / TestLength;
        //        Debug.Log(Input.Mouse.RealMoveDistance.ToString());
        //        Vector2 move = Input.Mouse.RealMoveDistance * pixelToMoveLegth;

        //        return move;
        //    }
        //    return Vector2.Zero;
        //}

        public float zoom()
        {
            float mouse = -lib.ToLeftRight(Input.Mouse.ScrollValue) * 10f;
            float control = 0;

            if (toggleCameraMode.IsDown)
            {
                control = ZoomSpeed * moveXZ.directionAndTime.Y;
            }

            return mouse + control;
        }

        public Vector2 cameraRotation(bool inMenu, int playerIndex)
        {
            Vector2 controller = Vector2.Zero;
            if (cameraXMoveY != null)
            {
                controller = cameraXMoveY.directionAndTime* RotateCamSpeed;
                controller.Y *= -1;

                if ((inMenu || toggleCameraMode.IsDown) == false)
                {
                    controller.Y = 0;
                }
            }

            Vector2 mouse = Vector2.Zero;

            if (!inMenu && toggleCameraMode.IsDown)
            {
                mouse = Input.Mouse.RealMoveDistance * 0.005f;
            }

            //if (VectorExt.Corrupted(controller) || VectorExt.Corrupted(mouse))
            //{
            //    lib.DoNothing();
            //}

            return controller + mouse;
        }

        float controllerMoveSpeed(float settingsMoveSpeed)
        {
            float moveAccelerate = 1f;

            if (movePencilTime > 2000)
            {
                moveAccelerate = 4f;
            }
            else if (movePencilTime > 600)
            {
                moveAccelerate = 2f;
            }

            return  DefaultPencilMoveSpeed* settingsMoveSpeed * moveAccelerate;
        }

        public void keyboardSetup()
        {
            useMouseInput = true;

            moveXZ = new AlternativeDirectionalMap(Input.PlayerInputMap.arrowKeys, Input.PlayerInputMap.WASD);
            cameraXMoveY = null;//cameraXMoveY = directionalMappings[(int)DirActionType.GamePlayerMovement];
            toggleCameraMode = new MouseButtonMap(MouseButton.Right);
            cameraZoom =
                new DirectionalButtonsMap(new KeyboardButtonMap(Keys.OemPlus), new KeyboardButtonMap(Keys.OemMinus), new NoButtonMap(), new NoButtonMap());

            draw = new NoButtonMap();//new AlternativeButtonsMap(new KeyboardButtonMap(Keys.Space), new MouseButtonMap(MouseButton.Left));
            erase = new NoButtonMap();//new AlternativeButtonsMap(new KeyboardButtonMap(Keys.LeftAlt), new MouseButtonMap(MouseButton.Right));
            select = new NoButtonMap();//new AlternativeButtonsMap(new KeyboardButtonMap(Keys.LeftControl), new MouseButtonMap(MouseButton.Middle));
            colorPick = new KeyboardButtonMap(Keys.LeftAlt);
            cancel = new KeyboardButtonMap(Keys.Delete);
            mirrorX = new KeyboardButtonMap(Keys.D1);
            mirrorY = new KeyboardButtonMap(Keys.D2);
            undo = new KeyboardButtonMap(Keys.Z);
            previous = new KeyboardButtonMap(Keys.PageUp);
            next = new KeyboardButtonMap(Keys.PageDown);

            OpenClose = new KeyboardButtonMap(Keys.Tab);
        }

        public void xboxSetup(int controllerIndex, IDirectionalMap gamemove, IDirectionalMap gamecamera)
        {
            useMouseInput = false;

            moveXZ = gamemove;
            cameraXMoveY = gamecamera;
            cameraZoom =
                new DirectionalButtonsMap(new XboxButtonMap(Buttons.DPadUp, controllerIndex), new XboxButtonMap(Buttons.DPadDown, controllerIndex), new NoButtonMap(), new NoButtonMap());
            toggleCameraMode = new XboxButtonMap(Buttons.LeftTrigger, controllerIndex);

            draw = new XboxButtonMap(Buttons.RightShoulder, controllerIndex);
            erase = new XboxButtonMap(Buttons.LeftShoulder, controllerIndex);
            select = new XboxButtonMap(Buttons.RightTrigger, controllerIndex);
            colorPick = new XboxButtonMap(Buttons.X, controllerIndex);
            cancel = new XboxButtonMap(Buttons.B, controllerIndex);
            mirrorX = new XboxButtonMap(Buttons.X, controllerIndex);
            mirrorY = new XboxButtonMap(Buttons.Y, controllerIndex);
            undo = new XboxButtonMap(Buttons.Y, controllerIndex);
            previous = new XboxButtonMap(Buttons.DPadLeft, controllerIndex);
            next = new XboxButtonMap(Buttons.DPadRight, controllerIndex);
            OpenClose = new XboxButtonMap(Buttons.Back, controllerIndex);
        }

        //public void ps4Setup(int controllerIndex, IDirectionalMap gamemove, IDirectionalMap gamecamera)
        //{
        //    useMouseInput = false;

        //    moveXZ = gamemove;
        //    cameraXMoveY = gamecamera;
        //    cameraZoom = new DirectionalGenericDpadMap(PS4InputLib.Dpad, false, controllerIndex);
        //    toggleCameraMode = new GenericControllerButtonMap(PS4InputLib.L2, controllerIndex);

        //    draw = new GenericControllerButtonMap(PS4InputLib.R1, controllerIndex);
        //    erase = new GenericControllerButtonMap(PS4InputLib.L1, controllerIndex);

        //    select = new GenericControllerButtonMap(PS4InputLib.R2, controllerIndex);
        //    colorPick = new GenericControllerButtonMap(PS4InputLib.Square, controllerIndex);
        //    cancel = new GenericControllerButtonMap(PS4InputLib.Cirkle, controllerIndex);
        //    mirrorX = new GenericControllerButtonMap(PS4InputLib.Square, controllerIndex);
        //    mirrorY = new GenericControllerButtonMap(PS4InputLib.Triangle, controllerIndex);
        //    undo = new GenericControllerButtonMap(PS4InputLib.Triangle, controllerIndex);
        //    previous = new XboxButtonMap(Buttons.DPadLeft, controllerIndex);
        //    next = new XboxButtonMap(Buttons.DPadRight, controllerIndex);
        //    OpenClose = new GenericControllerButtonMap(PS4InputLib.Share, controllerIndex);
        //}
    }
}

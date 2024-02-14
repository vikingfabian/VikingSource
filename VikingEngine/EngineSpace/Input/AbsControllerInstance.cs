//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.Input
//{
//    abstract class AbsControllerInstance
//    {
//        abstract public int Index { get; }

//        abstract public void Update();
//        abstract public bool Connected { get; }

//        /// <summary>
//        /// is the button down/up right now
//        /// </summary>
//        /// <returns>is keydown</returns>
//        abstract public bool IsButtonDown(Buttons button);

//        abstract public bool IsButtonDown(ButtonGroup buttons);

//        abstract public bool KeyDownEvent(Buttons button1, Buttons button2, Buttons button3);

//        abstract public bool KeyDownEvent(Buttons button1, Buttons button2);

//        abstract public bool KeyDownEvent(ButtonGroup buttons);

//        /// <summary>
//        /// If button was pressed this frame
//        /// </summary>
//        virtual public bool KeyDownEvent(Buttons button)
//        {
//            throw new NotImplementedException();
//        }


//        /// <summary>
//        /// If button was released this frame
//        /// </summary>
//        abstract public bool KeyUpEvent(Buttons button);

//        /// <summary>
//        /// if the key status was different previous frame
//        /// </summary>
//        abstract public bool KeyChangedEvent(Buttons button);

//        /// <summary>
//        /// if the key status was different previous frame
//        /// </summary>
//        abstract public bool KeyChangedEvent(Buttons button, out bool keyDown);


//        protected delegate bool CheckMethod(Buttons button);

//        abstract protected bool checkKeyDownEvent(Buttons button);
//        abstract protected bool checkKeyUpEvent(Buttons button);
//        abstract protected bool checkButtonPressed(Buttons button);



//        const float TriggerBuffer = 0.01f;
//        abstract public float LeftTrigger { get; }
//        abstract public float RightTrigger { get; }

//        /// <returns>The stick has an input value</returns>
//        abstract public bool bJoyStick(Stick stickType);

//        public bool bLeftStick { get { return bJoyStick(Stick.Left); } }
//        public bool bRightStick { get { return bJoyStick(Stick.Right); } }
//        public bool bDpad { get { return bJoyStick(Stick.D); } }


//        virtual protected Vector2 RawJoyStickValue(Stick stickType)
//        {
//            throw new NotImplementedException();
//        }
 

//        abstract public JoyStickValue JoyStickValue(Stick stickType);

//        //abstract public bool IsConnected { get; }
//    }
//}

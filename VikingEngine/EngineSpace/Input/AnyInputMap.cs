//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.Input
//{
//    /// <summary>
//    /// Uses input from all devices
//    /// </summary>
//    class AnyInputMap : PlayerInputMap
//    {
//        public Input.PlayerInputSource activeSource = PlayerInputSource.KeyboardMouse;
//        //public IButtonMap[] buttonMappings = new IButtonMap[(int)ButtonActionType.NUM];
//        //public IDirectionalMap[] directionalMappings = new IDirectionalMap[(int)DirActionType.NUM];

//        public AnyInputMap()
//            : base()
//        {
//            List<PlayerInputMap> inputDevices = new List<PlayerInputMap>(5);

//            PlayerInputMap keyboard = new PlayerInputMap();
//            keyboard.keyboardSetup();
//            inputDevices.Add(keyboard);

//            for (int i = 0; i < 4; ++i)
//            {
//                if (Input.Controller.Instance(i).Connected)
//                {
//                    PlayerInputMap controller = new PlayerInputMap();
//                    //controller.playerIndex = controller.playerIndex;
//                    controller.xboxSetup();
//                    inputDevices.Add(controller);
//                }
//            }

//            for (int i = 0; i < buttonMappings.Length; ++i)
//            {
//                Alternative5ButtonsMap buttonMap = new Alternative5ButtonsMap();
//                foreach (var m in inputDevices)
//                {
//                    buttonMap.add(m.buttonMappings[i]);
//                }

//                this.buttonMappings[i] = buttonMap;
//            }

//            for (int i = 0; i < directionalMappings.Length; ++i)
//            {
//                Alternative5DirectionalMap dirMap = new Alternative5DirectionalMap();
//                foreach (var m in inputDevices)
//                {
//                    dirMap.add(m.directionalMappings[i]);
//                }

//                this.directionalMappings[i] = dirMap;
//            }
//        }

//        public void refreshSourceFromButton()
//        {
//            for (int i = 0; i < buttonMappings.Length; ++i)
//            {
//                if (buttonMappings[i] != null && buttonMappings[i].IsDown)
//                {
//                    activeSource = ((Alternative5ButtonsMap)buttonMappings[i]).activeSource();
//                    return;
//                }
//            }
//        }

//        public override SpriteName ButtonIcon(ButtonActionType action)
//        {
//            return ((Alternative5ButtonsMap)buttonMappings[(int)action]).GetFromSource(activeSource).Icon;
//        }
//    }
//}

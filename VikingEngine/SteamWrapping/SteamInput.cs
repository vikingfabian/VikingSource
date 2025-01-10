using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class SteamInput
    {
        public SteamInput(string description, string startText) 
        {
           bool result =  SteamAPI.SteamUtils().ShowFloatingGamepadTextInput((int)EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine,10, 10, 100, 50);
        }
    }
}

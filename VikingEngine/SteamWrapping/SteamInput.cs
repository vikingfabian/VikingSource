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
            SteamAPI.SteamUtils().ShowGamepadTextInput((int)EGamepadTextInputMode.k_EGamepadTextInputModeNormal, (int)EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, description, 32, startText);
        }
    }
}

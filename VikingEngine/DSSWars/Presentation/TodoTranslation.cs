using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        //Option language


        public string HUD_DisplayName => "Display name";
        public string Lobby_PlayerProfileNumbered => "Profile {0}";
        public string Lobby_CharacterCreationNumbered => "Character {0}";
        public string Lobby_PlayerProfileEdit => "Edit player profile";

        public string Lobby_CharacterCreationEdit => "Open character creator";

        public string ProfileEditor_TunicColor => "Tunic";
        public string ProfileEditor_PantsColor => "Pants";
        public string ProfileEditor_LeaderColor => "Leader";


        public string Editor_ConvertAnimationToLayers => "Convert animation to layers";
        public string Editor_StampAllFrames => "Stamp on all frames";
    }

}
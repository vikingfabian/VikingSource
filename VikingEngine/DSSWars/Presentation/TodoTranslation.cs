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
        public string HUD_Filter => "Filter";
        public string HUD_Scale => "Scale";
        public string HUD_Tags => "Tags";
        public string HUD_ClickToCancel => "Click to cancel";


        public string ObjectTag_Description => "Add a symbol on the map";
        public string HudPins => "HUD pins";
        public string HudPins_Description => "Stick information to the screen";


        public string Lobby_PlayerProfileNumbered => "Profile {0}";
        public string Lobby_CharacterCreationNumbered => "Character {0}";
        public string Lobby_PlayerProfileEdit => "Edit player profile";

        public string ProfileEditor_TunicColor => "Tunic";
        public string ProfileEditor_PantsColor => "Pants";
        public string ProfileEditor_LeaderColor => "Leader";

        public string Editor_ConvertAnimationToLayers => "Convert animation to layers";
        public string Editor_StampAllFrames => "Stamp on all frames";

        public string Editor_DisplayOptions => "Diplay options";
        public string Editor_CharacterCreator => "Character creator";
        public string Editor_CharacterCreator_Description => "Military models appearance editor";
        public string Editor_HatGenre => "Hat display mode";
        public string Editor_HatGenre_FollowWeapon => "Follow weapon";
        public string Editor_HatGenre_Uniform => "Uniform";
        public string Editor_CopyPasteSelectedColor => "Copy from selected color";

        public string Character_Accessories=> "Accessories";
        public string Character_Hat => "Hat";
        public string Character_Head => "Head";
        public string Character_Body => "Body";
        public string Character_Arms => "Arms";
        public string Character_Back => "Back";
        public string Character_Face => "Face";

        public string BuildingType_Tavern => "Common Hall";

        public string Settings_CraftMultiplier => "Craft time multiplier";
        public string Settings_ChildMultiplier_Description => "Increases the speed new workers are added";
        public string Settings_CasualControls => "Casual player controls";
        public string Settings_CasualControls_Description => "Simplifies gameplay by reducing choices to key decisions. Only money is used as a resource.";

        public string WarsResourceGroup_Metal => "Metal";
        public string Work_Craft => "Craft";
        public string Work_OnlyCraftOnFullStock => "Only craft on full stockpile";

        public string ExperienceType_Smelting => "Smeleting";
        public string Catergory_Optimize => "Optimize";
        public string BuildCategory_Road => "Road";
        public string Technology_ModernFarming => "Modern farming";

        public string ExportImportDescription => "For sharing save files with other player, all files are this folder: {0}";

        public string CityCultureDescription => "Culuture will give it a special bonus to the city";
    }

}
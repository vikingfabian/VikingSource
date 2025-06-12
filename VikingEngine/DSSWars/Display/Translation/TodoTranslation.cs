using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Display.Translation
{
    class TodoTranslation
    {
        //Option language

        public string MouseButtonAction_PanAndCancel => "Pan and Cancel";
        public string MouseButtonAction_PanAndOrderAndCancel => "Pan, Order and Cancel";

        //##

        public string Work_BadValueDescription => "Resources can go below zero and slightly exceed the stockpile limit. The bounds are only enforced when the work queue is created.";

        public string Work_SelectCategory => "Select item category";
        public string Hud_RemoveFromList => "Remove from list";


        public string Hud_ReturnToPrevious => "Return";
        public string Hud_Close => "Close";

        public string Hud_Low => "Low";
        public string Hud_Medium => "Medium";
        public string Hud_High => "High";

        public string Hud_Copy => "Copy";
        public string Hud_Paste => "Paste";
        public string Hud_Cut => "Cut";
        public string Hud_SaveCompleted => "Save completed";
        public string Settings_WaterMultiplier => "Water multiplier";
        public string Settings_WaterMultiplier_Description => "How much water cities produce and store. A high value will lower computer performance.";

        public string Settings_ChildMultiplier => "Child birth multiplier";
        public string Settings_CraftMultiplier => "Craft speed multiplier";
        public string Settings_CraftMultiplier_Description => "Low speed will give fast production";

        public string FastProduction => "Fast production";
        public string SlowProduction => "Slow production";

        /// <summary>
        /// Label for a list of items blocked from production
        /// </summary>
        public string BlocksProduction => "Will not produce";

        //public string CityAutomation_WaitForMaxPopulation => "Wait for population to max out";
        public string Automation_AutomationFocus_NoFocus => "All";
        public string CityAutomation_SoldierQuality => "Soldier quality";
        public string CityAutomation_SoldierWeaponType => "Weapon type";
        public string WarsResourceGroup_Resources => "Resources";
        public string WarsResourceGroup_Weapons => "Weapons";

        public string WarsResourceGroup_AllWeaponTypes => "Mixed";
        public string WarsResourceGroup_MeleeHandWeapons => "Melee";
        public string WarsResourceGroup_RangedHandWeapons => "Ranged";
        public string WarsResourceGroup_Warmachines => "Warmachine";


        public string FactionSettings_Titel => "Faction wide settings";
        public string FactionSettings_Description => "Will include all your cities";

        public string Conscript_MaxPopulation => "Max population";
        public string Conscript_MaxPopulation_Description => "Will only recruit when the population is maxed out";

        public string Conscript_FoodAbundance => "Max food stock";
        public string Conscript_FoodAbundance_Description => "Will only recruit when the food has reached maximum stockpile";

        /// <summary>
        /// General settings will go through all items in a list and apply to all of them (to their checkbox)
        /// </summary>
        public string GeneralSetting_On = "Set to: On";
        public string GeneralSetting_Off = "Set to: Off";
        public string GeneralSetting_AllBuildingsDescription => "Will apply to all buildings";

        public string GeneralSetting_ApplyMessage => "Change applied to {0} buildings";

        public string MustTurnOffSteamInput => "To use controllers, you must turn off Steam Input";

        public string Technology_GainTitle => "Ways to gain technology";
        public string Technology_LevelUp => "Level up";
        public string Technology_ForEachLevelUp => "When a worker level up, in the technology field: {0}";

        public string VoxelEditor_Description => "Create blocky models";


        public string Editor_Tool = "Tool";
        public string Editor_SelectOptionsMenu = "Selection options";
        public string Editor_Continous => "Continous";
        public string Editor_Tool_PencilSize => "Pencil size";
        public string Editor_Tool_SizeTolerance => "Size tolerance";
        public string Editor_Tool_RoundPencil => "Round pencil";
        public string Editor_Tool_EdgeSize => "Edge size";
        public string Editor_Tool_PercentFill => "Percent fill";
        public string Editor_Tool_ClearAbove => "Clear above";
        public string Editor_Tool_FillBelow => "Fill below";
        public string Editor_Tool_ => "Fill below";
        public string Editor_UserModels => "User models";
        public string Editor_UserModels_Description => "Browse models you saved";

        public string Editor_RetailModels => "Retail models";
        public string Editor_RetailModels_Description => "Load models from the game";

        public string Editor_ModTemplates => "Templates for modding";
        public string Editor_ExportAsOBJ => "Export as .OBJ";
        public string Editor_SelectAll => "Select All";

        public string Editor_Canvas_Title => "Canvas";
        public string Editor_Canvas_Size => "Size";
        public string Editor_Canvas_Dimension_X => "X"; //done
        public string Editor_Canvas_Dimension_Y => "Y"; //done
        public string Editor_Canvas_Dimension_Z => "Z"; //done
        public string Editor_Canvas_SizePresets => "Size presets";
        public string Editor_Canvas_Move => "Move";
        public string Editor_Canvas_Move_Up => "Up";
        public string Editor_Canvas_Move_Down => "Down";
        public string Editor_Canvas_RotateClockwise => "Rotate clockwise";
        public string Editor_Canvas_RotateCounterClockwise => "Rotate counter clockwise";
        public string Editor_Canvas_Mirror => "Mirror";

        public string Editor_Canvas_RotateFlip_Title => "Rotate/Flip";
        public string Editor_Canvas_FlipVertical => "Flip up and down";
        public string Editor_Canvas_FlipOrientation => "Flip lying/standing";
        public string Editor_Canvas_ClearAll_Description => "Removes all blocks and all frames";


        public string Editor_Animation => "Animation";
        public string Editor_Animation_RemoveCurrentFrame => "Remove current frame";
        public string Editor_Animation_AddFrameCopy => "Add frame as copy";
        public string Editor_Animation_AddEmptyFrame => "Add empty frame";
        public string Editor_Animation_MoveDescription => "Change frame position";
        public string Editor_Animation_AllFrames => "All frames";
        public string Editor_Animation_AllFrames_ActionDescription => "Make the same action on all frames";


        public string Editor_SettingsMenu => "Settings";
        public string Hud_Exit => "Exit";
        public string Editor_Canvas_Clear => "Clear";

        public string Editor_Stamp => "Stamp";
        public string Editor_StampOtherFrames => "Stamp in other frames";
        public string Editor_StampOtherFrames_Description => "Paste the voxels in this frames";
        public string Editor_PasteToFrame => "Paste the voxels in this frame";
        public string Editor_ClearAllFrames => "Clear in All frames";
        public string Editor_ClearOtherFrames => "Clear Other Frames";


        public string Editor_Settings_MoveSpeed => "Move speed";
        public string Editor_Settings_BackgroundColor => "Background color";
        public string Editor_Settings_HideHUD => "Hide HUD";


        public string Editor_Color = "Color";
        public string Editor_ColorsInUseLabel => "Colors in use";
        public string Editor_Color_BrighterPlus => "Brighter +";
        public string Editor_Color_Brighter => "Brighter";
        public string Editor_Color_Darker => "Darker";
        public string Editor_Color_DarkerPlus => "Darker +";
        public string Editor_Color_RedTint => "Red tint";
        public string Editor_Color_Tint = "Tint";
        public string Editor_Color_GreenTint => "Green tint";
        public string Editor_Color_BlueTint => "Blue tint";
        public string Editor_Color_YellowTint => "Yellow tint";
        public string Editor_Color_PurpleTint => "Purple tint";
        public string Editor_NoColor => "Empty";

        /// <summary>
        /// User may change one color to another, across the model
        /// </summary>
        public string Editor_Color_Recolor => "Recolor";
        public string Editor_Color_RecolorTo => "Recolor to";

        public string Editor_Material_Set => "Set material";

        public string Editor_Preview => "Preview";
        public string Editor_CombineWithCurrent => "Combine with current model";

        public string Editor_PickedColor => "Picked";
        public string Editor_ColorRGBvalues => "R:{0} G:{1} B:{2}";
        //public string Editor_ => "";
        //public string Editor_ => "";
        //public string Editor_ => "";
        //public string Editor_ => "";
        //public string Editor_ => "";
        //public string Editor_ => "";


    }

}
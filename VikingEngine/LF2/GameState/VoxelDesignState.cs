using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2.GameState
{
    class VoxelDesignState : Voxels.AbsVoxelDesignerState
    {
        Editor.VoxelDesigner vDesigner;
        public VoxelDesignState(int player)
            : base(true)
        {
            //default:
            //        Music.SoundManager.PlayFlatSound(LoadedSound.block_place_1);
            //        break;
            //    case FillType.Delete:
            //        Music.SoundManager.PlayFlatSound(LoadedSound.tool_dig);
            //        break;
            //    case FillType.Select:
            //        Music.SoundManager.PlayFlatSound(LoadedSound.tool_select);
            //Engine.LoadContent.LoadSound(new List<Engine.FileDirAndName>
            //{
            //    new FileDirAndName(LoadedSound.tool_dig),
            //    new FileDirAndName(LoadedSound.tool_select),
            //    new FileDirAndName(LoadedSound.block_place_1),
            //});
            vDesigner = new Editor.VoxelDesigner(player);
            desinger = vDesigner;
        }

        protected override string modeTitle
        {
            get
            {
                return vDesigner.Mode.ToString();
            }
        }

        public static List<HUD.ButtonDescriptionData> ButtonDescription(Players.PlayerMode mode)
        {
            List<HUD.ButtonDescriptionData> buttons = null;

            switch (mode)
            {
                case Players.PlayerMode.InMenu:
                    buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Exit", SpriteName.ButtonSTART),
                        new HUD.ButtonDescriptionData("Move", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Select", SpriteName.ButtonA),
                        new HUD.ButtonDescriptionData("Back", SpriteName.ButtonB),
                        new HUD.ButtonDescriptionData("Page Up", SpriteName.ButtonLB),
                        new HUD.ButtonDescriptionData("Page Down", SpriteName.ButtonRB),
                    };
                    break;
                case Players.PlayerMode.Creation:
                    buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Move XZ", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Move Y", SpriteName.RightStick_UD),
                        new HUD.ButtonDescriptionData("Rotate Camera", SpriteName.RightStick_LR),

                        new HUD.ButtonDescriptionData("Add block", SpriteName.ButtonRB),
                        new HUD.ButtonDescriptionData("Remove block", SpriteName.ButtonLB),
                        new HUD.ButtonDescriptionData("Select", SpriteName.ButtonRT),

                        new HUD.ButtonDescriptionData("Undo", SpriteName.ButtonY),
                        new HUD.ButtonDescriptionData("Pick color", SpriteName.ButtonX),
                        new HUD.ButtonDescriptionData("Menu", SpriteName.ButtonSTART),

                        new HUD.ButtonDescriptionData("Camera zoom", SpriteName.ButtonLT, SpriteName.LeftStick_UD),
                        new HUD.ButtonDescriptionData("Camera pitch", SpriteName.ButtonLT, SpriteName.RightStick_UD),
                    };
                    break;
                case Players.PlayerMode.CreationSelection:
                    buttons = new List<HUD.ButtonDescriptionData>
                    {
                        new HUD.ButtonDescriptionData("Move XZ", SpriteName.LeftStick),
                        new HUD.ButtonDescriptionData("Move Y", SpriteName.RightStick_UD),
                        new HUD.ButtonDescriptionData("Rotate Camera", SpriteName.RightStick_LR),
                        new HUD.ButtonDescriptionData("Rotate selection", SpriteName.Dpad),
                        new HUD.ButtonDescriptionData("Stamp", SpriteName.ButtonRB),
                        new HUD.ButtonDescriptionData("Mirror", SpriteName.ButtonX),
                        new HUD.ButtonDescriptionData("Up n down", SpriteName.ButtonY),
                        new HUD.ButtonDescriptionData("Unselect", SpriteName.ButtonB),
                    };
                    break;
            }
            return buttons;
        }
             
        protected override List<HUD.ButtonDescriptionData> buttonDescription()
        {
            const string NextFrame = "Next frame";
            List<HUD.ButtonDescriptionData> result = GameState.VoxelDesignState.ButtonDescription(vDesigner.Mode);
            switch  (vDesigner.Mode)
            {
                case Players.PlayerMode.CreationSelection:
                    result.Add(new HUD.ButtonDescriptionData(NextFrame, SpriteName.ButtonLT, SpriteName.Dpad));
                    break;
                case Players.PlayerMode.Creation:
                    result.Add(new HUD.ButtonDescriptionData(NextFrame, SpriteName.Dpad));
                    break;
            }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.Voxels
{
    abstract class AbsVoxelDesignerState : Engine.GameState
    {
        protected AbsVoxelDesigner desinger;

        public AbsVoxelDesignerState(bool replaceState)
            : base(replaceState)
        { }

        public override void FirstUpdate()
        {
            base.FirstUpdate();
            Ref.draw.ClrColor = Color.CornflowerBlue;
        }

        HUD.ButtonLayout buttonLayout = null;
        
        private void refreshButtonDesc()
        {
            if (buttonLayout != null)
            {
                buttonLayout.DeleteMe();
            }
            List<HUD.ButtonDescriptionData> data = buttonDescription();
            if (data != null)
            {
                buttonLayout = new HUD.ButtonLayout(data, Engine.Screen.Area, Engine.Screen.SafeArea, modeTitle, null);
            }
            else
            {
                buttonLayout = null;
            }
        }

        virtual protected string modeTitle { get { return "Button layout"; } }
        virtual protected List<HUD.ButtonDescriptionData> buttonDescription() { return null; }

        public override void Time_Update(float time)
        {
            desinger.Time_Update(time);

            if (Ref.music != null)
                Ref.music.Update();
        }

        public override void BeginInputDialogueEvent(KeyboardInputValues keyInputValues)
        {
            desinger.BeginInputDialogueEvent(keyInputValues);
        }
        public override void TextInputEvent(string input, object tag)
        {
            desinger.TextInputEvent(input, tag);
        }
        public override void TextInputCancelEvent(int playerIndex)
        {
            desinger.TextInputCancelEvent(playerIndex);
        }
        
    }
}

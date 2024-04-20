using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.Engine;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Display.CutScene
{
    abstract class AbsSaveScene : AbsCutScene
    {
        protected Graphics.TextG progress;
        protected Graphics.TextG pressAnyKey;
        protected Graphics.Image bg;
        protected SaveStateMeta meta;

        public AbsSaveScene()
             : base()
        {
            progress = new Graphics.TextG(LoadedFont.Bold, Screen.CenterScreen, Screen.TextTitleScale, Graphics.Align.CenterAll,
                string.Format(SaveString, "..."),
                Color.White, HudLib.CutContentLayer);

            VectorRect area = VectorRect.FromCenterSize(progress.position, progress.MeasureText());

            pressAnyKey = new Graphics.TextG(LoadedFont.Regular, VectorExt.AddY(progress.position, area.Height),
                Screen.TextBreadScale, Graphics.Align.CenterAll, DssRef.lang.Progressbar_PressAnyKey,
                Color.White, HudLib.CutContentLayer);
            pressAnyKey.Visible = false;

            area.Size.Y *= 2f;
            area.AddHeight(Screen.IconSize);
            area.AddXRadius(Screen.IconSize * 2);

            bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.CutSceneBgLayer);
            bg.Color = Color.Black;
            bg.Opacity = 0.8f;
        }

        abstract protected string SaveString { get; }
        protected void displaySaveComplete()
        {
            progress.TextString = string.Format(SaveString, DssRef.lang.Progressbar_ProgressComplete);
            progress.Color = Color.Gray;
            pressAnyKey.Visible = true;
        }
        public override void Close()
        {
            progress.DeleteMe();
            bg.DeleteMe();
            pressAnyKey.DeleteMe();

            base.Close();
        }
    }

    class SaveScene : AbsSaveScene, DataStream.IStreamIOCallback
    {
        int holdTime = 4;
        int state_0Hold_1Save_2meta_3Done = 0;
        
        SaveGamestate saveGamestate;
        public SaveScene() 
            :base()
        { 
            //progress = new Graphics.TextG(LoadedFont.Bold, Screen.CenterScreen, Screen.TextTitleScale, Graphics.Align.CenterAll, 
            //    string.Format(DssRef.lang.Progressbar_SaveProgress, "..."), 
            //    Color.White, HudLib.CutContentLayer);
            
            //VectorRect area = VectorRect.FromCenterSize(progress.position, progress.MeasureText());

            //pressAnyKey = new Graphics.TextG(LoadedFont.Regular, VectorExt.AddY(progress.position, area.Height),
            //    Screen.TextBreadScale, Graphics.Align.CenterAll, DssRef.lang.Progressbar_PressAnyKey,
            //    Color.White, HudLib.CutContentLayer);
            //pressAnyKey.Visible = false;

            //area.Size.Y *= 2f;
            //area.AddHeight(Screen.IconSize);
            //area.AddXRadius(Screen.IconSize * 2);

            //bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.CutSceneBgLayer);
            //bg.Color = Color.Black;
            //bg.Opacity = 0.8f;
        }

        protected override string SaveString => DssRef.lang.Progressbar_SaveProgress;

        /// <summary>
        /// Callback from meta save
        /// </summary>
        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            state_0Hold_1Save_2meta_3Done++;

            displaySaveComplete();
        }

        

        override public void Time_Update(float time)
        {
            switch (state_0Hold_1Save_2meta_3Done)
            {
                case 0:
                    if (--holdTime <= 0)
                    {
                        //Begin save
                        state_0Hold_1Save_2meta_3Done++;
                        meta = new SaveStateMeta();
                        saveGamestate = new SaveGamestate();
                        saveGamestate.save();
                    }
                    break;
                case 1:
                    if (saveGamestate.complete)
                    {
                        state_0Hold_1Save_2meta_3Done++;

                        DssRef.storage.meta.saveState1 = meta;
                        DssRef.storage.meta.Save(this);

                        progress.TextString = string.Format(DssRef.lang.Progressbar_SaveProgress, "meta"); 
                    }
                    break;
                //case 2:

                //    break;

                case 3:
                    if (InputLib.AnyKeyDownEvent())
                    { 
                        Close();
                    }
                    break;

            }
        }

        
    }

    class LoadScene : AbsSaveScene
    {
        int state_0load_1complete = 0;
        SaveGamestate saveGamestate;
        public LoadScene(SaveStateMeta load)
            : base()
        {
            this.meta = load;
            saveGamestate = new SaveGamestate();
            saveGamestate.load();
        }

        protected override string SaveString => DssRef.lang.Progressbar_LoadProgress;

        override public void Time_Update(float time)
        {
            if (state_0load_1complete == 0)
            {
                if (saveGamestate.complete)
                {
                    displaySaveComplete();
                    DssRef.state.OnLoadComplete();
                    state_0load_1complete++;
                }
            }
            else
            {
                if (InputLib.AnyKeyDownEvent())
                {
                    Close();
                    
                }
            }
        }
    }
}

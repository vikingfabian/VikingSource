using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
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
        public bool ExitGame = false;
        int holdTime = 4;
        int state_0Hold_1Save_2meta_3Done = 0;

        SaveGamestate saveGamestate;
        bool autoSave;
        public SaveScene(bool auto)
            : base()
        {
            this.autoSave = auto;
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
            if (StartupSettings.Saves)
            {

                switch (state_0Hold_1Save_2meta_3Done)
                {
                    case 0:
                        if (--holdTime <= 0)
                        {
                            //Begin save
                            state_0Hold_1Save_2meta_3Done++;
                            meta = new SaveStateMeta(autoSave);
                            saveGamestate = new SaveGamestate(meta);
                            saveGamestate.save();
                        }
                        break;
                    case 1:
                        if (saveGamestate.complete)
                        {
                            state_0Hold_1Save_2meta_3Done++;

                            DssRef.storage.meta.AddSave(meta, this);
                        }
                        break;
                    case 2:
                        //Wait for callback
                        break;

                    case 3:
                        if (ExitGame)
                        {
                            DssRef.state.exit();
                        }
                        else if (autoSave)
                        {
                            RichBoxContent content = new RichBoxContent();
                            content.h1(DssRef.lang.GameMenu_AutoSave);
                            DssRef.state.localPlayers[0].hud.messages.Add(content);

                            Close();
                        }
                        else
                        {
                            if (InputLib.AnyKeyDownEvent())
                            {
                                Close();
                            }
                        }
                        break;
                }
            }
            else
            {
                Close();
                if (ExitGame)
                {
                    DssRef.state.exit();
                }
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
            saveGamestate = new SaveGamestate(load);
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

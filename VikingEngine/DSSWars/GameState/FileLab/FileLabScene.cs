using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.GameState.FileLab
{
    class FileLabScene : Engine.GameState
    {
        RichMenu menu;
        TextBoxSimple textLog;

        int readSuccessCount;
        int readBeginCount;

        public FileLabScene():base() 
        {
            openMenu();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            bool mouseOver = false;

            menu.updateMouseInput(ref mouseOver);
        }

            void openMenu()
        {
            if (menu == null)
            {
                var objectMenuArea = Screen.SafeArea;
                objectMenuArea.Width = (int)(Engine.Screen.IconSize * 9f);

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                mainMenu();

                VectorRect logArea = Screen.SafeArea;
                logArea.AddToLeftSide(-(objectMenuArea.Width + Engine.Screen.IconSize));
                textLog = new TextBoxSimple(LoadedFont.Console, logArea.Position, Vector2.One, Align.Zero, "-Text log-",
                    Color.White, ImageLayers.Background0, logArea.Width, true);

            }
        }
        void mainMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("File lab", HudLib.TitleColor_Head);


            content.newLine();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("1. Write 100 files - date marked") },
                new RbAction1Arg<bool>(writeFiles, true)));
            
            content.newLine();

            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("2. Read files - date marked, main core") },
               new RbAction2Arg<bool, bool>(readFiles, true, true)));
            HudLib.InfoButton(content, new RbTooltip_Text("Expect the game to freeze for a moment!"));

            content.newLine();

            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("3. Read files - date marked, second core") },
               new RbAction2Arg<bool, bool>(readFiles, true, false)));

            content.newLine();

            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("4. Write 100 files - no mark") },
                new RbAction1Arg<bool>(writeFiles, false)));

            content.newLine();

            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("5. Read files -  no mark, main core") },
               new RbAction2Arg<bool, bool>(readFiles, false, true)));
            HudLib.InfoButton(content, new RbTooltip_Text("Expect the game to freeze for a moment!"));

            content.newLine();

            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("6. Read files -  no mark, second core") },
               new RbAction2Arg<bool, bool>(readFiles, false, false)));

            content.newParagraph();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Clear log") },
               new RbAction(()=> { textLog.TextString = string.Empty; })));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                 new RbText(DssRef.lang.Hud_Exit) },
              new RbAction(() => { new ExitToLobby(true); })));

            Refresh(content);
        }

        void writeFiles(bool dateMark)
        {
            DataStream.FilePath path = filepath(dateMark);

            print("Write begin: " + path.CompleteDirectory);

            System.IO.Directory.CreateDirectory(path.CompleteDirectory);
            
            for (int i = 0; i < 100; i++)
            {
                var name = fileInstancePath(path, i);
                DataStream.BeginReadWrite.BinaryIO(true, name, write, null, null, true);
            }

            print("Write complete: " + path.CompleteDirectory);
        }

        void readFiles(bool dateMark, bool mainCore)
        {
            readSuccessCount = 0;
            readBeginCount = 0;
            DataStream.FilePath path = filepath(dateMark);

            print("Read begin: " + path.CompleteDirectory);

            if (mainCore)
            {
                execute();
            }
            else
            { 
                Task.Factory.StartNew(execute);
            }

            void execute()
            {
                for (int i = 0; i < 100; i++)
                {
                    var name = fileInstancePath(path, i);
                    DataStream.FileToDiskManager.ReadBinaryIO(name, read);
                }


                print("Read complete: " + path.CompleteDirectory);
                print($"Result: Begin {readBeginCount}, Success {readSuccessCount}");
            }
        }

        DataStream.FilePath filepath(bool dateMark)
        {
            DataStream.FilePath path = new DataStream.FilePath(Ref.steam.UserCloudPath + DataStream.FilePath.Dir + "FileTest_" +
               (dateMark ? "datemark" : "no_mark"), "name", ".sav");
            path.UseTimeMark = dateMark;

            return path;
        }

        DataStream.FilePath fileInstancePath(DataStream.FilePath path, int index)
        {
            var name = path;
            name.FileName = Data.NameGenerator.RandomLetters(index);

            return name;
        }

        void print(string text)
        { 
            textLog.TextString+= Environment.NewLine + text;
        }

        

        public void write(System.IO.BinaryWriter w)
        {
            for (int i = 0; i < 100; i++)
            {
                w.Write(Ref.rnd.Int());
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            readBeginCount++;

            for (int i = 0; i < 100; i++)
            {
                var intVal = r.ReadInt32();
            }

            readSuccessCount++;
        }

        public void Refresh(RichBoxContent content)
        {
            //openMenu();
            menu.Refresh(content);
        }
    }
}

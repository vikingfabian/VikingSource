using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class MapEditor2_Scene : AbsDssState, IStreamIOCallback
    {
        public IconWorldDataMeta iconMeta = new IconWorldDataMeta();
        public MapEditor2Display display;
        MessageGroup_Editor messages;
        public MapEditor3_Tool tool;
        bool loadingState = false;
        public Map2Generator generator = new Map2Generator();
        public Map2GenerateSettings generateSettings = new Map2GenerateSettings();
        public GeneratorMap map;
        //public bool iconState = true;

        public IconMapStorage storage = new IconMapStorage();

        List<InputMap> controller;

        public ProcessState process = ProcessState.None;

        public MapEditor2_Scene()
            : base()
        {
            messages = new MessageGroup_Editor();

            display = new MapEditor2Display(this);
            tool = new MapEditor3_Tool(this);
            map = new GeneratorMap(display.topRight);
            new Interface.EditorBackground();

            controller = new List<InputMap>{
                Ref.gamesett.keyboardMap,
            };
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (process != ProcessState.None)
            {
                display.updateProcessPage();

                if (process == ProcessState.WorkshopUpload)
                {
                    if (Ref.steam.workshopUploadState != SteamWrapping.WorkshopUploadState.Uploading)
                    {
                        RichBoxContent content = new RichBoxContent();
                        content.h2("Upload", HudLib.TitleColor_Head);

                        content.newLine();
                        content.Add(new RbImage(SpriteName.WarsHudIconExport));
                        content.space();
                        content.Add(new RbText(Ref.steam.workshopUploadState.ToString()));

                        messages.Add(content);

                        //--
                        //display.onProcessComplete();
                        endProcess();
                    }
                }
                return;
            }

            if (loadingState)
            {
                if (generator.complete())
                {
                    loadingState = false;
                    display.loadingDisplay.Hide();
                    display.refreshMenu();

                    if (generator.currentPass <= Map2Pass.NewWorld)
                    {
                        map.hide();
                    }
                    else if (generator.currentPass < Map2Pass.Icon)
                    {
                        map.generateNodes(generator.nodeMap);
                    }
                    else
                    {
                        map.generateIcon(generator.ActiveIconWorld);
                        if (generator.currentPass >= Map2Pass.ScaleUp)
                        {
                            map.scale = 0.25f;
                        }
                    }
                }
            }
            else
            {
                bool mouseOverHud = false;
                display.update(ref mouseOverHud);

                messages.Update(ref mouseOverHud);

                if (!mouseOverHud)
                {
                    foreach (var input in controller)
                    {
                        map.userInput(input, mouseOverHud);

                        tool.paintInput(input);
                    }
                }
            }
        }

        bool redrawLock = false;
        public void redrawPixels()
        {
            if (display.tab == Map2GeneratorTab.Bioms && generator.currentPass < Map2Pass.Bioms)
            {
                generator.currentPass = Map2Pass.Bioms;
            }

            if (!redrawLock)
            {
                redrawLock = true;

                Task.Run(() =>
                {
                    generator.processTexturePixels();
                    map.refreshTexture(generator.ActiveIconWorld);
                    redrawLock = false;
                });
            }
        }

        public void generatePass(Map2Pass start, Map2Pass end)
        {
            if (start == Map2Pass.NewWorld && generator.currentPass > Map2Pass.NewWorld)
            {
                iconMeta = new IconWorldDataMeta();
            }

            if (start <= Map2Pass.Empty)
            {
                map.resetPos();
            }

            loadingState = true;
            display.loadingDisplay.Show();
            generator.generatePass(generateSettings, start, end);
        }

        public void revertToIconPass()
        {
            loadingState = true;
            display.loadingDisplay.Show();
            generator.revertToIconPass();
        }

        public void generateHeightMap()
        {
            loadingState = true;
            display.loadingDisplay.Show();
            generator.generateHeightMap();
        }

        public void saveIconMap()
        {
            SoundLib.saving.Play();
            process = ProcessState.Saving;
            //display.processMenu();
            storage.save(iconMeta, generator.ActiveIconWorld, map.texture.texture, this);
        }

        public void uploadIconMap()
        {
            SoundLib.saving.Play();
            process = ProcessState.WorkshopUploadSave;
            //display.processMenu();
            storage.save(iconMeta, generator.ActiveIconWorld, map.texture.texture, this);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (save)
            {
                RichBoxContent content = new RichBoxContent();
                content.h2(LoadContent.CheckCharsSafety(iconMeta.name, LoadedFont.Regular), HudLib.TitleColor_Name);

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudIconSave));
                content.space();
                content.Add(new RbText(DssRef.lang.Hud_SaveCompleted));

                messages.Add(content);
            }

            if (process == ProcessState.WorkshopUploadSave)
            {
                process = ProcessState.WorkshopUpload;
                storage.SavePath(iconMeta.name, out DataStream.FilePath path, out DataStream.FilePath iconPath);
                Ref.steam.BeginUpload(new SteamWrapping.WorkshopItem()
                {
                    itemName = iconMeta.name,
                    itemDescription = "none",
                    itempath = path,
                    iconpath = iconPath,
                    itemTags = new List<string>() { VikingEngine.DSSWars.Data.DssWorkshop.IconMap_Tag },
                });                
            }
            else
            {
                //display.onProcessComplete();
                endProcess();
                
            }
            //        break;
            //}
            
        }

        void endProcess()
        { 
            process = ProcessState.None;
            display.refreshMenu();
        }
    }

    enum ProcessState
    {
        None,
        Saving,

        WorkshopUploadSave,
        WorkshopUpload,
    }
}

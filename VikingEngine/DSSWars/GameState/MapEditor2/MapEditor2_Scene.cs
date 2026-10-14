using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
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
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.PJ.Joust;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

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
        public MapEditHistory editHistory = new MapEditHistory();
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

                switch (process)
                {
                    case ProcessState.WorkshopUpload:
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
                        break;

                    case ProcessState.Loading:
                        if (display.menu.menuStack.Count == 0)
                        {
                            endProcess();
                        }
                        break;
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
            if (end >= Map2Pass.NodeGrid)
            {
                editHistory.AddStorePoint(this);
            }

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
            generator.generatePass(generateSettings, start, end, null);
        }

        public void onLoad(IconWorldData data)
        {
            

            generator.currentPass = Map2Pass.Bioms;
            editHistory.undo(this);

            generator.iconWorld = data;
            loadingState = true;
            display.loadingDisplay.Show();
            generator.refreshPass();
        }

        public void undo()
        {
            editHistory.undo(this);
            loadingState = true;
            display.loadingDisplay.Show();
            generator.refreshPass();
        }

        public void revertToIconPass()
        {
            loadingState = true;
            display.loadingDisplay.Show();
            generator.revertToIconPass();
        }

        public void generateHeightMap()
        {
            editHistory.AddStorePoint(this);

            loadingState = true;
            display.loadingDisplay.Show();
            generator.generateHeightMap();
        }

        public void fitCanvasToHeightMap()
        {
            editHistory.AddStorePoint(this);

            var size = generator.heightMapTexture.Size();
            size.X = Bound.Max(size.X, WorldData.CustomMapSize_Max);
            size.Y = Bound.Max(size.Y, WorldData.CustomMapSize_Max);
            generateSettings.customMapSize = size;
            generateSettings.bCustomSize = true;

            generator.generatePass(generateSettings, 0, Map2Pass.Icon, ()=>
            {
                loadingState = true;
                display.loadingDisplay.Show();
                generator.generateHeightMap();
            });
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
                
                Ref.steam.BeginUpload(WorkshopItemSetup());                
            }
            else
            {
                //display.onProcessComplete();
                endProcess();
                
            }
            //        break;
            //}
            
        }

        SteamWrapping.WorkshopItem WorkshopItemSetup()
        {
            storage.SavePath(iconMeta.name, out DataStream.FilePath path, out DataStream.FilePath iconPath);

            var result = new SteamWrapping.WorkshopItem()
            {
                itemName = iconMeta.name,
                //itemDescription = "none",
                itempath = path,
                iconpath = iconPath,
                itemTags = new List<string>() { VikingEngine.DSSWars.Data.DssWorkshop.IconMap_Tag },
            };

            result.itemDescription =
                $"{WorldData.SizeString(WorldData.ToMapSize(generateSettings.customMapSize), generateSettings.customMapSize)}. {string.Format(HudLib.EngineVersionString, Engine.LoadContent.EngineVersion)}.";

            return result;
        }

        public void endProcess()
        { 
            process = ProcessState.None;
            display.refreshMenu();
        }
    }



    enum ProcessState
    {
        None,
        Loading,
        Saving,

        WorkshopUploadSave,
        WorkshopUpload,
    }
}

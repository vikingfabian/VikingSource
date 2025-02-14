using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapEditor_Generator : AbsDssState
    {
        public Map.Generate.MapGenerateSettings GenerateSettings;
        MapGeneratorDisplay display;
        public MapEditorUserStorage userStorage;
        GeneratorMap map;
        MapGenerator_BackgroundLoading mapBackgroundLoading;
        bool loadingState = false;
        public CustomMapStorage mapStorage = new CustomMapStorage();
        public MapEditor_Generator()
            :base() 
        {
            userStorage = new MapEditorUserStorage();
            GenerateSettings = new Map.Generate.MapGenerateSettings();
            display = new MapGeneratorDisplay(this);
            new Display.EditorBackground();
            map = new GeneratorMap(display.topRight);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingState)
            {
                mapBackgroundLoading.Update();
                if (mapBackgroundLoading.Complete())
                {
                    loadingState = false;
                    map.generate();
                    display.refreshMenu();
                }
            }
            else
            {
                bool mouseOverHud = false;
                display.update(ref mouseOverHud);

                map.userInput(mouseOverHud);                
            }
        }

        public bool canRunPass(GenerateMapPass pass)
        {
            switch (pass)
            {
                default:
                    return mapBackgroundLoading != null && DssRef.world != null;

                case GenerateMapPass.Clear:
                    return true;

                case GenerateMapPass.Build:
                case GenerateMapPass.Dig:
                case GenerateMapPass.CleanUp:
                    return DssRef.world != null && DssRef.world.generatePassCompleted < GenerateMapPass.Cities;
            }
        }

        public void generatePass(GenerateMapPass pass)
        {
            loadingState = true;

            if (pass == GenerateMapPass.Clear || pass == GenerateMapPass.AllTerrain)
            {
                mapBackgroundLoading = new MapGenerator_BackgroundLoading();
                mapStorage.autoName = $"CustomMap W{GenerateSettings.customMapSize.X} H{GenerateSettings.customMapSize.Y} id{Ref.rnd.Int(9999)}";
            }
            mapBackgroundLoading.generateSettings = GenerateSettings;
            mapBackgroundLoading.generate(pass);
        }

        
        public void startNewGame()
        {
            var state = new LobbyState(false);
            state.playOnCustomMap(mapBackgroundLoading);
        }

        public void saveMap()
        { 
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.MapEditor;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class MapEditor2_Scene : AbsDssState
    {
        MapEditor2Display display;
        bool loadingState = false;
        Generator2 generator = new Generator2();
        GeneratorMap map;
        public MapEditor2_Scene()
            : base()
        {
            display = new MapEditor2Display(this);
            map = new GeneratorMap(display.topRight);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingState)
            {
                //mapBackgroundLoading.Update();
                //if (mapBackgroundLoading.Complete())
                //{
                //    loadingState = false;
                //    display.loadingDisplay.Hide();
                //    map.generate();
                //    display.refreshMenu();
                //}
                if (generator.complete())
                {
                    loadingState = false;
                    display.loadingDisplay.Hide();
                    map.generate2(generator.world);
                }
            }
            else
            {
                bool mouseOverHud = false;
                display.update(ref mouseOverHud);

                map.userInput(mouseOverHud);
            }
        }

        public void generatePass(GenerateMapPass pass)
        {
            loadingState = true;
            display.loadingDisplay.Show();
            generator.generate();
            //if (pass == GenerateMapPass.Clear || pass == GenerateMapPass.AllTerrain)
            //{
            //    mapBackgroundLoading = new MapGenerator_BackgroundLoading();

            //    mapStorage.autoName = $"CustomMap W{GenerateSettings.customMapSize.X} H{GenerateSettings.customMapSize.Y} id{Ref.rnd.Int(9999)}";
            //}
            //mapBackgroundLoading.generateSettings = GenerateSettings;
            //mapBackgroundLoading.generate(pass);
        }
    }
}

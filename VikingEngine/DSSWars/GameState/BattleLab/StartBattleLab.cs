using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map.Generate;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class StartBattleLab : AbsStartPlayState
    {
        bool lab;
        public StartBattleLab(MapBackgroundLoading loading)
            : base()
        {
            lab = true;
            BattleLabStorage.Singleton = new BattleLabStorage();

            if (loading == null)
            {

                loading = new MapBackgroundLoading(null as SaveStateMeta);
            }

            this.loading = loading;
        
        }

        public StartBattleLab()
            : base()
        {
            lab = false;
            MapGenerateSettings generateSettings = new MapGenerateSettings();
            generateSettings.customSeed = true;
            generateSettings.useGenerate = true;
            generateSettings.DigChainsCount_per100Tiles *= 0.2f;
            generateSettings.repeatBuildDigCount = 2;

            generateSettings.setCustomSize(WorldData.SizeDimentions(MapSize.Tiny));
            generateSettings.StartAs = MapStartAs.Land;
            
            loading = new MapBackgroundLoading(generateSettings);
        }

        protected override void onLoadComplete()
        {
            new BattleLabPlayState();
        }
    }
}

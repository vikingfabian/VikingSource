using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Map2;

namespace VikingEngine.DSSWars.GameState.MapEditor2
{
    class StartDetailEditor : AbsStartPlayState
    {
        public StartDetailEditor(MapBackgroundLoading loading)
            : base()
        {
            DssRef.settings.playType = PlayStateType.MapEditor;
            BattleLabStorage.Singleton = new BattleLabStorage();

            if (loading == null)
            {
                loading = new MapBackgroundLoading(null as SaveStateMeta);
            }

            this.loading = loading;

        }

        public StartDetailEditor()
            : base()
        {

            Map2GenerateSettings generateSettings = new Map2GenerateSettings();
            generateSettings.useGenerate = true;
            generateSettings.setCustomSize(WorldData.SizeDimentions(MapSize.Tiny));
            generateSettings.StartAs = MapStartAs.Land;

            loading = new MapBackgroundLoading(generateSettings);
        }

        protected override void onLoadComplete()
        {
            new DetailEditor.MapEditorPlayState();
        }
    }
}

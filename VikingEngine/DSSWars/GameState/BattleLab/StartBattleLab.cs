using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Generate;

namespace VikingEngine.DSSWars.GameState.BattleLab
{
    class StartBattleLab : AbsStartPlayState
    {
        public StartBattleLab(MapBackgroundLoading loading)
            : base()
        {
            if (loading == null)
            {
                loading = new MapBackgroundLoading(null);
            }

            this.loading = loading;
        }

        protected override void onLoadComplete()
        {
            new BattleLabPlayState();
        }
    }
}

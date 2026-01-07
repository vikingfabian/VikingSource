using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.FlagEditor;

namespace VikingEngine.DSSWars.GameState
{
    class StartExtra : AbsDssState
    {
        int waitUpdates = 2;
        int ProfileIx;
        bool controller;
        public StartExtra()
            : base()
        {
            
            draw.ClrColor = Color.Black;
            Ref.lobby?.disconnect(null);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (--waitUpdates <= 0)
            {
                VikingEngine.ToGG.toggLib.Init();
                VikingEngine.ToGG.Commander.BattleLib.Init();
                new ToGG.ToggEngine.Map.SquareDic();
                ToGG.ToggEngine.Map.MainTerrainProperties.Init();
                new VikingEngine.ToGG.InputMap(0);
                //new Network.Session();

                ToGG.Commander.LevelSetup.GameSetup setup = new ToGG.Commander.LevelSetup.GameSetup();
                setup.lobbyMembers = new List<ToGG.AbsLobbyMember>
                {
                    new ToGG.LocalLobbyMember(0),
                    new ToGG.AiLobbyMember(),
                };

                new ToGG.Commander.CmdPlayState(setup);
                DssRef.stats.start_commander.addOne();
            }
        }
    }
}

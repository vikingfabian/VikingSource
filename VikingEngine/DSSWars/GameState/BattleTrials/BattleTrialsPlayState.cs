using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.BattleLab;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.GameState.BattleTrials
{
    class BattleTrialsPlayState : AbsBattlePlayState
    {
        public BattleTrialsPlayState()
            : base()
        { }

        public override PlayStateType PlayType()
        {
            return PlayStateType.BattleTrials;
        }

        protected override LocalPlayer createLocalPlayer(Faction faction)
        {
            return new LocalPlayer(faction, true);
        }

        protected override void initPlayers()
        {
            //base.initPlayers();
            var enemy = new Faction(DssRef.world, FactionType.DarkLord);
            DssRef.settings.darkLordPlayer = new Players.DarkLordPlayer(enemy, true);

            var human = new Faction(DssRef.world, FactionType.Player);
            
            int playerCount = 1;

            localPlayers = new List<Players.LocalPlayer>(playerCount);
            var local = createLocalPlayer(human);
            local.assignPlayer(0, playerCount, true);
            localPlayers.Add(local);

            var factionsCounter = DssRef.world.factions.counter();
            while (factionsCounter.Next())
            {
                factionsCounter.sel.initDiplomacy(DssRef.world);
            }
        }

        protected override void initScenario()
        {
            //Hard coded demo scenario
            IntVector2 center = new IntVector2(98, 143);

            var manager =  new BattleLab.BattleSetupManager();
            manager.beginBattleSetup(center);
            manager.addSoldier(8, Resource.ItemResourceType.Sword, BattleSetupManager.BothPlayers);
            manager.addSoldier(4, Resource.ItemResourceType.HandSpear, BattleSetupManager.BothPlayers);
            manager.addSoldier(4, Resource.ItemResourceType.Bow, BattleSetupManager.BothPlayers);
            manager.addSoldier(4, Resource.ItemResourceType.HandSpear, BattleSetupManager.BothPlayers);
            manager.addSoldier(4, Resource.ItemResourceType.KnightsLance, BattleSetupManager.BothPlayers);
            manager.addSoldier(2, Resource.ItemResourceType.TwoHandSword, BattleSetupManager.BothPlayers);

            manager.addSoldier(8, Resource.ItemResourceType.Ballista, BattleSetupManager.BothPlayers);

            manager.startBattle(false, BattleSetupManager.NoPlayer);
            manager.addTimedAttackFromEnemy(10);

            LocalHost().gameControls.map.cameraFocus = manager.friendlyArmy;



        }
    }
}

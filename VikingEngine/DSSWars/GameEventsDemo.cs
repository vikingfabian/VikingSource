using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars
{
    class GameEventsDemo : GameEvents
    {
        Time maxDemoTime = new Time(60f, TimeUnit.Minutes);
        City defendingCity;

        public override void onGameStarted()
        {
            base.onGameStarted();

            var enemy = DssRef.state.LocalHost().getPin("enemy");
            if (enemy != null)
            {
                Faction attacker = DssRef.world.tileGrid.Get(enemy.tilePos).Faction();

                var defend = DssRef.state.LocalHost().getPin("defend");
                defendingCity = DssRef.world.tileGrid.Get(defend.tilePos).City();

                new Timer.TimedAction0ArgTrigger_InGame(() =>
                {
                    DssRef.diplomacy.declareWar(attacker, DssRef.state.LocalHost().faction);
                    attacker.player.GetAiPlayer().armyAi_enabled = false;

                    const int FirstAttackerId = 6;
                    var firstAttacker = attacker.armies.GetIndex_Safe(FirstAttackerId);
                    firstAttacker.Order_Attack(defendingCity);
                    firstAttacker.setMaxFood();

                }, 20);

                new Timer.TimedAction0ArgTrigger_InGame(() =>
                {
                    var armiesC = attacker.armies.counter();
                    while (armiesC.Next())
                    {
                        armiesC.sel.Order_Attack(defendingCity);
                        armiesC.sel.setMaxFood();
                    }
                }, 15 * TimeExt.MinuteInSeconds);

                DssRef.state.LocalHost().clearPins();


                var mission = new RichBoxContent();
                mission.h1(".Mission Objective", HudLib.TitleColor_Head);
                mission.text(".Defend against the attack from south");
                DssRef.state.LocalHost().hud.messages.Add(mission);
            }
        }

        public override void asyncUpdate(float time)
        {
            base.asyncUpdate(time);
            if (maxDemoTime.CountDownGameTime_IfActive())
            {
                Ref.update.AddSyncAction(new SyncAction(onDemoTimeUp));
            }
        }

        void onDemoTimeUp()
        {
            DssRef.state.localPlayers.First().hud.messages.Add(".Times' up!", ".The demo will end in one minute");
            new Timer.TimedAction0ArgTrigger(viewTimesUpScreen, TimeExt.MinutesToMS(1f));
        }

        void viewTimesUpScreen()
        {
            new EndScene(GameEndReason.TimesUp, false);
        }
    }
}

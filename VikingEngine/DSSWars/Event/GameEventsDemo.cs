using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Event
{
    class GameEventsDemo : EventManager
    {
        Time maxDemoTime = new Time(65f, TimeUnit.Minutes);
        City defendingCity;
        int demoState_1start_2end = 0;
        List<Army> attackerArmies;

        public override void onGameStarted()
        {
            base.onGameStarted();

            var citiesC = DssRef.state.LocalHost().faction.cities.counter();
            while (citiesC.Next())
            {
                citiesC.sel.res_Palisade.amount += 20;
                citiesC.sel.res_food.amount += 500;
            }

            var enemy = DssRef.state.LocalHost().getPin("enemy");
            if (enemy != null)
            {
                Faction attacker = DssRef.world.tileGrid.Get(enemy.tilePos).Faction();
                attacker.addGold_factionWide(100000);

                var defend = DssRef.state.LocalHost().getPin("defend");
                defendingCity = DssRef.world.tileGrid.Get(defend.tilePos).City();

                new Timer.TimedAction0ArgTrigger_InGame(() =>
                {
                    DssRef.diplomacy.declareWar(attacker, DssRef.state.LocalHost().faction);
                    attacker.player.GetAiPlayer().armyAi_enabled = false;

                    const int FirstAttackerId = 4;
                    var firstAttacker = attacker.armies.GetIndex_Safe(FirstAttackerId);
                    firstAttacker.Order_Attack(defendingCity);
                    firstAttacker.setMassiveFood();

                }, 20);

                new Timer.TimedAction0ArgTrigger_InGame(() =>
                {
                    List<Army> all = new List<Army>(8);

                    var armiesC = attacker.armies.counter();
                    while (armiesC.Next())
                    {
                        if (!armiesC.sel.isDeleted)
                        {
                            armiesC.sel.Order_Attack(defendingCity);
                            armiesC.sel.setMassiveFood();

                            all.Add(armiesC.sel);
                        }
                    }
                    attackerArmies = all;
                    demoState_1start_2end = 1;
                },
#if DEBUG
                1);
#else
                15 * TimeExt.MinuteInSeconds);
#endif

                DssRef.state.LocalHost().clearPins();


                var mission = new RichBoxContent();
                mission.h1(DssRef.lang.Demo_MissionObjective_Title, HudLib.TitleColor_Head);
                mission.text(DssRef.lang.Demo_MissionObjective_Description);
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

            if (demoState_1start_2end == 1)
            {
                bool lostCity = defendingCity.faction.player.IsAi();

                if (lostCity)
                {
                    onDemoVictory(false);
                    return;
                }


                bool allDefeated = true;
                foreach (var m in attackerArmies)
                {
                    if (!m.defeated())
                    {
                        allDefeated = false;
                        break;
                    }
                }


                if (allDefeated)
                {
                    onDemoVictory(true);
                }
            }
        }

        void onDemoVictory(bool victory)
        {
            if (demoState_1start_2end == 1)
            {
                demoState_1start_2end = 2;

                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    DssRef.state.LocalHost().hud.messages.Add(DssRef.lang.Demo_Complete_Title, DssRef.lang.Demo_EndInOneMinuteDescription);
                    new Timer.TimedAction1ArgTrigger_InGame<GameEndReason>(viewEndScreen, victory? GameEndReason.Victory : GameEndReason.Defeat, TimeExt.MinuteInSeconds * 1f);
                }));
            }
        }

        public override void onTutorialEnd()
        {
            base.onTutorialEnd();
            onDemoTimeUp();
        }

        void onDemoTimeUp()
        {
            DssRef.state.LocalHost().hud.messages.Add(DssRef.lang.Demo_TimesUp_Title, DssRef.lang.Demo_EndInOneMinuteDescription);
            new Timer.TimedAction1ArgTrigger_InGame<GameEndReason>(viewEndScreen, GameEndReason.TimesUp, TimeExt.MinuteInSeconds *1f);
        }

        void viewEndScreen(GameEndReason endReason)
        {
            new EndScene(endReason, false);
        }
    }
}

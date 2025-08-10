using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Interface.CutScene;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Event
{
    class GameEventsDemo : EventManager
    {
#if DEBUG
        Time maxDemoTime = new Time(90f, TimeUnit.Minutes);
#else
        Time maxDemoTime = new Time(90f, TimeUnit.Minutes);
#endif
        City defendingCity;
        int demoState_1start_2end = 0;
        List<Army> attackerArmies;
        bool endPreWarning = false;
        float endPreWarningTime = 15 * TimeExt.MinuteInMs;

        public override void onGameStarted()
        {
            base.onGameStarted();

            if (!DssRef.state.LocalHost().profile.casualControls)
            {
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

                    //1. Send one army
                    new Timer.TimedAction0ArgTrigger_InGame(() =>
                    {
                        DssRef.diplomacy.declareWar(attacker, DssRef.state.LocalHost().faction);
                        attacker.player.GetAiPlayer().armyAi_enabled = false;

                        const int FirstAttackerId = 4;
                        var firstAttacker = attacker.armies.GetIndex_Safe(FirstAttackerId);
                        firstAttacker.Order_Attack(defendingCity);
                        firstAttacker.setMassiveFood();


                        //2. Send all armies
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


                            //3. Turn Ai back on
                            new Timer.TimedAction0ArgTrigger_InGame(() =>
                            {
                                attacker.player.GetAiPlayer().armyAi_enabled = true;
                            }, 15 * TimeExt.MinuteInSeconds); //3.

                        },
#if DEBUG
                        1);
#else
                15 * TimeExt.MinuteInSeconds);//2.
#endif
                    }, 20);//1.


                    DssRef.state.LocalHost().clearPins();


                    var mission = new RichBoxContent();
                    mission.h1(DssRef.lang.Demo_MissionObjective_Title, HudLib.TitleColor_Head);
                    mission.text(DssRef.lang.Demo_MissionObjective_Description);
                    DssRef.state.LocalHost().hud.messages.Add(mission);
                }
            }
        }

        public override void asyncUpdate(float time)
        {
            base.asyncUpdate(time);
            if (maxDemoTime.CountDownGameTime_IfActive())
            {
               
                Ref.update.AddSyncAction(new SyncAction(()=>
                {
                    viewEndScreen(GameEndReason.TimesUp);
                }));
            }

            if (!endPreWarning && maxDemoTime.MilliSeconds < endPreWarningTime)
            {
                endPreWarning = true;
                Ref.update.AddSyncAction(new SyncAction(endPreWarningMessage));
            }

            if (demoState_1start_2end == 1)
            {
                bool lostCity = defendingCity.GetPlayer().IsBot();

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

        protected void endPreWarningMessage()
        {
            DssRef.state.LocalHost().hud.messages.Add(DssRef.lang.Demo_TimesUp_Title, string.Format( DssRef.lang.Demo_EndInXMinuteDescription, 15));
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

        

        
    }
}

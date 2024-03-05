using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Battle;

namespace VikingEngine.DSSWars.GameObject
{

    partial class SoldierGroup
    {
        public Vector3 battleWp;
        public bool battleWalkPath = false;

        void update_battlePreparations(float time, bool fullUpdate)
        {
            //if (army.battleGroup.battleState)
            //{
            //    var closest_sp = attacking_soldierGroupOrCity;
            //    if (closest_sp != null)
            //    {
            //        if (groupCollisionDistance(closest_sp) < 0.02f)
            //        {
            //            //SPLIT GROUP
            //            //newObjective = GroupObjective_IsSplit;

            //            var soldiersC = soldiers.counter();
            //            while (soldiersC.Next())
            //            {
            //                soldiersC.sel.setAttackState();
            //            }
            //        }
            //        else
            //        {
            //            //Group attack move
            //            bool walking = !updateWalking(battleWp, false, Rotation1D.D0, time);

            //            var soldiersC = soldiers.counter();
            //            while (soldiersC.Next())
            //            {
            //                soldiersC.sel.update_GroupLocked(walking);
            //            }
            //        }
            //    }
            //    //bool walking = !updateWalking(battleWp, false, Rotation1D.D0, time);

            //    //var soldiersC = soldiers.counter();
            //    //while (soldiersC.Next())
            //    //{
            //    //    soldiersC.sel.update_GroupLocked(walking);
            //    //}
            //}
            //else
            //{
                bool walking = !updateWalking(battleWp, true, army.battleDirection, time);

                var soldiersC = soldiers.counter();
                while (soldiersC.Next())
                {
                    soldiersC.sel.update_GroupLocked(walking);
                }
            //}
        }

        public void asynchFindBattleTarget(BattleGroup battle)
        {   
            refreshAttacking();

            if (attacking_soldierGroupOrCity == null ||
                groupObjective != GroupObjective_IsSplit)
            {
                //Version 3: hämtar från battlegroup
                AbsGroup nearest = null;
                float distanceValue = float.MaxValue;
                var battleUnitsC = battle.MembersCounter();
                while (battleUnitsC.Next())
                {
                    if (DssRef.diplomacy.InWar(army.faction, battleUnitsC.sel.faction))
                    {
                        var type = battleUnitsC.sel.gameobjectType();
                        if (type == GameObjectType.Army)
                        {
                            var groupsC = battleUnitsC.sel.GetArmy().groups.counter();
                            while (groupsC.Next())
                            {
                                if (groupsC.sel.soldiers.Count > 0)
                                {
                                    var dist = distanceValueTo(groupsC.sel);

                                    if (dist < distanceValue)
                                    {
                                        distanceValue = dist;
                                        nearest = groupsC.sel;
                                    }
                                }
                            }
                        }
                        else if (type == GameObjectType.City)
                        {
                            if (battleUnitsC.sel.GetCity().guardCount > 0)
                            {
                                var dist = distanceValueTo(battleUnitsC.sel);
                                if (dist < distanceValue)
                                {
                                    distanceValue = dist;
                                    nearest = battleUnitsC.sel;
                                }
                            }
                        }
                    }
                }

                //closestOpponent = nearest;
                if (nearest != null)
                {
                    addAttackTarget(nearest);
                }
            }

        }
    }
}

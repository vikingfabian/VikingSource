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
        const float BattlePrepSpeedBoost = 1.4f;

        //public Vector3 battleWp;
        public bool battleWalkPath = false;
        //public IntVector2 prevBattleGridPos;
        public float battleQueTime = 0;

        void update_battlePreparations(float time, bool fullUpdate)
        {            
            bool walking = !updateWalking(goalWp, true, army.battleDirection, time * BattlePrepSpeedBoost);

            var soldiersC = soldiers.counter();
            while (soldiersC.Next())
            {
                soldiersC.sel.update_GroupLocked(walking);
            }

            groupObjective = GroupObjective_FollowArmyObjective;
            groupIsIdle = !walking;
        }

        public bool asynchFindBattleTarget(BattleGroup battle)
        {
            if (army.id == 2072)
            {
                lib.DoNothing();
            }

            refreshAttacking();

            if (attacking_soldierGroupOrCity == null)
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

            return attacking_soldierGroupOrCity != null;
        }

        public void setBattleNode(Vector3 wp)
        {
            goalWp = wp;
            var soldiersC = soldiers.counter();
            while (soldiersC.Next())
            {
                soldiersC.sel.setBattleNode();
            }
        }

        public void setBattleWalkingSpeed()
        {
            //AbsSoldierData typeData = FirstSoldierData();
            
            //TODO pick subtile
            walkSpeed = typeCurrentData.walkingSpeed * terrainSpeedMultiplier;
        }
    }
}

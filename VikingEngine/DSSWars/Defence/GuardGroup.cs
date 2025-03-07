using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Defence
{
    class GuardGroup : SoldierGroup
    {
        public int assignedToPost_IdAndPosition = -1;
        public float postYPos;
       

        public GuardGroup(City city, SoldierConscriptProfile conscript, Vector3 startPos)
            :base(city, conscript, startPos) 
        {

        }

        public void TeleportToDefencePost(City city, int IdAndPosition, int defenceIndex)
        {
            //setRestingMode(true);

            city.defence_assignGuard_toIndex(this, defenceIndex);
            var subPos = conv.IntToIntVector2(IdAndPosition);

            Vector3 center = WP.SubtileToWorldPosXZgroundY_Centered(subPos);

            float wallHeight;
            var tile = DssRef.world.subTileGrid.Get(subPos);
            switch ((TerrainWallType)tile.subTerrain)
            {
                default:
                    wallHeight = WorldData.SubTileWidth * 0.8f;
                    break;
                case TerrainWallType.StoneTower:
                    wallHeight = WorldData.SubTileWidth * 1.4f;
                    break;

            }

            postYPos = center.Y + wallHeight;

            setArmyPlacement2(center, false, true);
        }

        void setRestingMode(bool set)
        {
            if (set != restingGuardMode)
            {
                restingGuardMode = set;

                if (set)
                {
                    int count = 0;
                    var soldiersC = soldiers.counter();
                    while (soldiersC.Next())
                    {
                        count++;
                        if (count == 1)
                        {
                            soldiersC.sel.groupOffset = Vector2.Zero;
                        }
                        else
                        {
                            soldiersC.sel.DeleteMe(DeleteReason.Transform, false);
                            soldiersC.RemoveAtCurrent();
                        }
                    }
                    soldierCount = count;
                }
                else
                {
                    var first = FirstSoldier();
                    if (first != null)
                    {
                        refillGuardUnits(first.SoldierProfile(), soldierCount - 1, first.model != null);
                    }
                }
            }            
        }

        public override void update(float time, bool fullUpdate)
        {
            if (attackTarget_soldierGroupOrCity != null)
            {
                lib.DoNothing();
            }
            base.update(time, fullUpdate);
        }

        public override void setGroundY()
        {
            if (assignedToPost_IdAndPosition >= 0)
            {
                position.Y = postYPos;
            }
            else
            {
                base.setGroundY();
            }
        }

        protected override void createAllSoldiers(AbsSoldierProfile typeProfile, int count, bool createModels)
        {
            soldiers = new SpottedArray<AbsSoldierUnit>(count);
            soldierData = soldierConscript.init(typeProfile);

            if (typeProfile.IsShip())
            {
                soldierConscript.shipSetup(ref soldierData);
            }

            if (count > 0)
            {
                AbsSoldierUnit unit = createUnit(typeProfile, IntVector2.Zero, tilePos, ref soldierData, createModels);
                unit.firstUpdate();
                refillGuardUnits(typeProfile, count-1, createModels);
            }
        }

        private void refillGuardUnits(AbsSoldierProfile typeProfile, int count, bool createModels)
        {
            for (int i = 0; i < count; ++i)
            {
                AbsSoldierUnit unit = createUnit(typeProfile, IntVector2.AllDiagonalsArray[i], tilePos, ref soldierData, createModels);
                unit.firstUpdate();
            }
        }

        public override GuardGroup GetGuardGroup()
        {
            return this;
        }

        public override bool InGuardPost()
        {
            return assignedToPost_IdAndPosition >= 0;
        }
    }


}

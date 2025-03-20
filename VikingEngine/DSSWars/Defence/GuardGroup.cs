using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Command;

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
        public GuardGroup(AbsArmy army, System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
            :base (army, r, version, pointers)  
        {
        }
        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);
            w.Write(assignedToPost_IdAndPosition);
        }

        public override void readGameState(BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subVersion, pointers);

            assignedToPost_IdAndPosition = r.ReadInt32();
            if (assignedToPost_IdAndPosition >= 0)
            {
                //refreshSoldierDefence();
                onEnterGuard(GetCity(), assignedToPost_IdAndPosition);
                refreshGuardPosition();
            }

            goalWp = position;
        }

        //public void refreshSoldierDefence()
        //{
            
        //}

        public override void completeTransform(SoldierTransformType transformType, int positionId)
        {
            if (transformType == SoldierTransformType.EnterGuard)
            {
                var city = this.army.GetCity();
                TeleportToDefencePost(city, positionId, city.defenceIxFromPosId(positionId));
            }
            else if (transformType == SoldierTransformType.ExitGuard)
            {
                onExitGuard();
                setGroundY();
            }
            else
            {
                base.completeTransform(transformType, positionId);
            }

            inShipOrGuardTransform = false;
        }



        public void TeleportToDefencePost(City city, int IdAndPosition, int defenceIndex)
        {
            city.defence_assignGuard_toIndex(this, defenceIndex);

            refreshGuardPosition();


        }

        void refreshGuardPosition()
        {
            IntVector2 subPos = conv.IntToIntVector2(assignedToPost_IdAndPosition);
            Vector3 center = WP.SubtileToWorldPosXZgroundY_Centered(subPos);
            var tile = DssRef.world.subTileGrid.Get(subPos);
            postYPos = center.Y + tile.BuildingHeight();
            setArmyPlacement2(center, false, true);
        }

        public void onEnterGuard(City city, int IdAndPosition)
        {
           assignedToPost_IdAndPosition = IdAndPosition;
           soldierConscript.conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool knight, out bool warmashine);

            if (DssRef.world.subTileGrid.TryGet(conv.IntToIntVector2(assignedToPost_IdAndPosition), out SubTile subTile))
            {
                if (ranged)
                {
                    //var tile = DssRef.world.subTileGrid.Get(conv.IntToIntVector2(IdAndPosition));
                    soldierAttackRangeBonus = subTile.BuildingHeight() * 2f;
                }
                else
                {
                    soldierAttackRangeBonus = 0.03f;
                }

            
                switch (subTile.GetWallType())
                {
                    case Map.TerrainWallType.NUM_NONE:
                        damageBlockChance = 0;
                        break;
                    case Map.TerrainWallType.DirtWall:
                    case Map.TerrainWallType.DirtTower:
                        damageBlockChance = DssConst.GuardPostDefenceChance_Dirt;
                        break;
                    case Map.TerrainWallType.WoodWall:
                    case Map.TerrainWallType.WoodTower:
                        damageBlockChance = DssConst.GuardPostDefenceChance_Wood;
                        break;
                    default:
                        damageBlockChance = DssConst.GuardPostDefenceChance_Stone;
                        break;
                }
            }
        }

        void onExitGuard()
        { 
            assignedToPost_IdAndPosition = -1;
            soldierAttackRangeBonus = 0;
            damageBlockChance = 0;
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

        public bool IsAssignedTo(int postIdAndPosition)
        { 
            if (assignedToPost_IdAndPosition == postIdAndPosition)
                return true;

            if (hasCommand(command))
                return true;

            return false;

            bool hasCommand(AbsCommand command)
            { 
                if (command == null) return false;

                if (command.isEnterPost(postIdAndPosition)) return true;

                return hasCommand(command.nextCommand);
            }
        }

        public override bool InGuardPost()
        {
            return assignedToPost_IdAndPosition >= 0;
        }
    }
}

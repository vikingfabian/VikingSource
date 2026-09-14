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
            : base(city, conscript, startPos)
        {
        }

        public GuardGroup(AbsArmy army, System.IO.BinaryReader r, int version, ObjectPointerCollection pointers)
            : base(army, r, version, pointers)
        {
        }

        public GuardGroup(AbsArmy army)
            : base(army)
        { }

        public override void writeGameState(BinaryWriter w, bool includePosition)
        {
            base.writeGameState(w, includePosition);
       
            w.Write(assignedToPost_IdAndPosition);
        }

        public override void readGameState(AbsArmy tArmy, BinaryReader r, int subVersion, bool needInit, bool includePosition, ObjectPointerCollection pointers)
        {
            base.readGameState(tArmy, r, subVersion, needInit, includePosition, pointers);
        
            assignedToPost_IdAndPosition = r.ReadInt32();
            if (assignedToPost_IdAndPosition >= 0)
            {
                onEnterGuard(GetCity(), assignedToPost_IdAndPosition);
                refreshGuardPosition(false, false);
            }

            goalWp = position;
        }

        public override void completeTransform(SoldierTransformType transformType, int positionId)
        {
            if (transformType == SoldierTransformType.EnterGuard)
            {
                if (army.TryGetTarget(out var tArmy))
                {
                    var city = tArmy.GetCity();

                    TeleportToDefencePost(city, positionId, city.defenceIxFromPosId(positionId));
                }
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

            refreshGuardPosition(true, true);


        }

        public override float GroupMoveBoundRadius()
        {
            return WorldData.SubTileHalfWidth;
        }
        void refreshGuardPosition(bool hostedAction, bool bRefreshArmyPos)
        {
            IntVector2 subPos = conv.IntToIntVector2(assignedToPost_IdAndPosition);
            Vector3 center = WP.SubtileToWorldPosXZgroundY_Centered(subPos);
            if (DssRef.world.subTileGrid.TryGet(subPos, out var tile))
            {
                postYPos = center.Y + tile.BuildingHeight();
                if (bRefreshArmyPos)
                {
                    setArmyPlacement2(center, false, true, hostedAction);
                }
            }
        }

        public void onEnterGuard(City city, int IdAndPosition)
        {
            assignedToPost_IdAndPosition = IdAndPosition;
            soldierConscript.conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);

            if (DssRef.world.subTileGrid.TryGet(conv.IntToIntVector2(assignedToPost_IdAndPosition), out SubTile subTile))
            {
                if (ranged)
                {
                    soldierAttackRangeBonus = subTile.BuildingHeight() * 2f;
                }
                else
                {
                    soldierAttackRangeBonus = 0.03f;
                }

                damageBlockChance_fromTerrain = DefenceStatus.WallDefenceChance(subTile.GetWallType(), out soldierAttackDamageBonus);
               
            }
        }

        void onExitGuard()
        {
            EnterPostCommand.ExitPost(this);
            assignedToPost_IdAndPosition = -1;
            soldierAttackRangeBonus = 0;
            soldierAttackDamageBonus = 0;
            damageBlockChance_fromTerrain = 0;
        }

        public override void setGroundY()
        {
            if (assignedToPost_IdAndPosition >= 0)
            {
                if (postYPos == 0)
                {
                    refreshGuardPosition(true, false);
                }
                position.Y = postYPos;
            }
            else
            {
                base.setGroundY();
            }
        }
        protected override void createAllSoldiers(UnitBuildType type, int count, bool createModels)
        {
            var typeProfile = DssRef.units.Get(type);
            soldiers = new SpottedArray<AbsSoldierUnit>(count);
            soldierData = soldierConscript.createSoldierData();

            if (typeProfile.IsShip())
            {
                soldierConscript.shipSetup(ref soldierData);
            }

            if (count > 0)
            {
                AbsSoldierUnit unit = createUnit(typeProfile, IntVector2.Zero, false, tilePos, ref soldierData, createModels);
                unit.firstUpdate();
                refillGuardUnits(typeProfile, count - 1, createModels);
            }

            setGroundY();
        }

        private void refillGuardUnits(AbsSoldierBuilder typeProfile, int count, bool createModels)
        {

            for (int i = 0; i < count; ++i)
            {
                if (i < IntVector2.AllDiagonalsArray.Length)
                {
                    AbsSoldierUnit unit = createUnit(typeProfile, IntVector2.AllDiagonalsArray[i], 
                        false, tilePos, ref soldierData, createModels);
                    unit.firstUpdate();
                }
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

        public override bool IsArmyGroup()
        {
            return false;
        }
        public override bool IsGuardGroup()
        {
            return true;
        }
        public override bool InGuardPost()
        {
            return assignedToPost_IdAndPosition >= 0;
        }

        public override string TypeName()
        {
            return DssRef.lang.Conscript_Soldiers_GuardType;
        }
    }
}

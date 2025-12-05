using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Defence
{
    struct DefenceStatus
    {
        public static readonly DefenceStatus Empty = new DefenceStatus();

        public const int NoSoldiers = ushort.MaxValue;
        public int soldierGroupId;
        public int idAndPosition;
        public bool autoAssign;
        public bool active; //not destroyed
        public bool tower;
        public void init(IntVector2 subtilepos, bool tower)
        {
            this.tower = tower;
            soldierGroupId = NoSoldiers;
            idAndPosition = conv.IntVector2ToInt(subtilepos);
            active = true;
        }

        public bool CheckIsEmpty(City city)
        {
            if (soldierGroupId == NoSoldiers)
            {
                return true;
            }

            var guard = city.groups.GetIndex_Safe(soldierGroupId)?.GetGuardGroup();
            if (guard == null || guard.isDeleted)
            {
                soldierGroupId = NoSoldiers;
                return true;
            }

            if (guard.assignedToPost_IdAndPosition < 0)
            {
                //Is he walking towards
                var command_sp = guard.command;
                if (command_sp != null)
                {
                    if (command_sp.isEnterPost(idAndPosition))
                    {
                        return false; //got guard walking towards it
                    }
                }
                soldierGroupId = NoSoldiers;
                return true;
            }
            else if (guard.assignedToPost_IdAndPosition != idAndPosition)
            {
                soldierGroupId = NoSoldiers;
                return true;
            }

            return false; //occupied
        }

        /// <summary>
        /// Is the assigned soldiers actually there
        /// </summary>
        /// <returns>Need save</returns>
        public bool checkSoldierAssignment(City city)
        {
            if (soldierGroupId != NoSoldiers)
            {
                var group = city.groups.GetIndex_Safe(soldierGroupId);
                if (group == null || !group.GetGuardGroup().IsAssignedTo(idAndPosition))
                {
                    soldierGroupId = NoSoldiers;
                    return true;
                }
            }

            return false;
        }

        public bool AvailableForAutoAssign()
        { 
            return autoAssign && soldierGroupId == NoSoldiers;
        }

        public bool AvailableForAutoAssign(City city, bool autoTurnOn)
        {
            autoAssign |= autoTurnOn;
            checkSoldierAssignment(city);
            return autoAssign && soldierGroupId == NoSoldiers;
        }

        public Vector3 WorldPos()
        {
            var subPos = conv.IntToIntVector2(idAndPosition);
            return WP.SubtileToWorldPosXZ_Centered(subPos);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)soldierGroupId);
            w.Write(idAndPosition);

            EightBit bools = new EightBit(active, autoAssign, tower);
            bools.write(w);
        }

        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            soldierGroupId = r.ReadUInt16();
            idAndPosition = r.ReadInt32();

            EightBit bools =EightBit.FromStream(r);
            active = bools.Get(0);
            autoAssign = bools.Get(1);
            tower = bools.Get(2);
        }

        public static float WallDefenceChance(TerrainWallType wallType, out int soldierAttackDamageBonus)
        {
            soldierAttackDamageBonus = 3;

            switch (wallType)
            {
                case Map.TerrainWallType.NUM_NONE:
                    return 0;

                case Map.TerrainWallType.Palisade:                    
                    soldierAttackDamageBonus = 2; 
                    return DssConst.GuardPostDefenceChance_Palisade;
                
                case Map.TerrainWallType.DirtWall:
                case Map.TerrainWallType.DirtTower:
                    return DssConst.GuardPostDefenceChance_Dirt;

                case Map.TerrainWallType.WoodWall:
                case Map.TerrainWallType.WoodTower:
                    return DssConst.GuardPostDefenceChance_Wood;

                default:
                    return DssConst.GuardPostDefenceChance_Stone;
            }
        }
}
}

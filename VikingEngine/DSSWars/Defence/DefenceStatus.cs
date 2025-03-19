using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Defence
{
    struct DefenceStatus
    {
        public const int NoSoldiers = ushort.MaxValue;
        public int soldierGroupId;
        public int idAndPosition;
        public bool autoAssign;
        public bool active; //not destroyed
        public void init(IntVector2 subtilepos)
        {
            soldierGroupId = NoSoldiers;
            idAndPosition = conv.IntVector2ToInt(subtilepos);
            active = true;
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

        public Vector3 WorldPos()
        {
            var subPos = conv.IntToIntVector2(idAndPosition);
            return WP.SubtileToWorldPosXZ_Centered(subPos);
        }
    }
}

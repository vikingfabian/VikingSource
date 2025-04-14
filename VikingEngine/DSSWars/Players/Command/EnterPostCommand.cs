using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Players.Command
{

    class EnterPostCommand : AbsCommand
    {
        IntVector2 subTile;
        public int id;

        public EnterPostCommand(SoldierGroup group, IntVector2 subTile, bool queueCommand)
            : base(group, queueCommand)
        {
            this.subTile = subTile;
            this.id = conv.IntVector2ToInt(subTile);
        }

        public EnterPostCommand(SoldierGroup group, int postId, bool queueCommand)
            : base(group, queueCommand)
        {
            this.subTile = conv.IntToIntVector2(postId);
            this.id = postId;
            group.wakeupSoldiers();
        }

        public void claimPost(SoldierGroup group, City city, int defenceIndex)
        {
            if (city.defenceBuildings.InBound( defenceIndex))
            {
                ref var defence = ref city.defenceBuildings.array[defenceIndex];

                defence.soldierGroupId = group.parentArrayIndex;
            }
        }

        public static bool tryClaimPost(SoldierGroup group, City city, IntVector2 subtilePos)
        {
            //var id = conv.IntVector2ToInt(subtilePos);
            int defenceIndex = city.defenceIxFromSubTile(subtilePos);
            
            ref var defence = ref city.defenceBuildings.array[defenceIndex];

            if (defence.soldierGroupId == DefenceStatus.NoSoldiers)
            {
                defence.soldierGroupId = group.parentArrayIndex;
                return true;
            }

            return false;
        }

        public static void ExitPost(GuardGroup group)
        {
            var city = group.army.GetCity();
            int defenceIndex = city.defenceIxFromPosId(group.assignedToPost_IdAndPosition);
            if (city.defenceBuildings.InBound(defenceIndex))
            {
                ref var defence = ref city.defenceBuildings.array[defenceIndex];
                defence.soldierGroupId = DefenceStatus.NoSoldiers;
            }
        }

        //void init(SoldierGroup group, City city)
        //{

        //    var defence = city.defenceBuildings.Array[id];
        //    defence.soldierGroupId = group.parentArrayIndex;
        //    city.defenceBuildings[id] = defence;
        //}


        public override void begin(SoldierGroup group)
        {
            base.begin(group);
            new GuardPostTransform(group, id, false);
        }

        public override bool hasPathCommand(out bool pathTowardsUnit)
        {
            pathTowardsUnit = false;
            return false;
        }
        public override bool isEnterPost(int postId)
        {
            return postId == id;
        }

        protected override CommandType GetCommandType()
        {
            return CommandType.EnterPost;
        }
    }

}

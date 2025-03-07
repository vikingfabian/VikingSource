using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public int selectedDefenceBuilding = -1;
        public List<DefenceStatus> defenceBuildings = new List<DefenceStatus>();

        public int defenceIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            for (int i = 0; i < defenceBuildings.Count; ++i)
            {
                if (defenceBuildings[i].idAndPosition == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public void defence_assignGuard_toIndex(GuardGroup guard, int index)
        {
            var defence = defenceBuildings[index];
            guard.assignedToPost_IdAndPosition = defence.idAndPosition;

            defence.soldierGroupId = guard.parentArrayIndex;
            defenceBuildings[index] = defence;
        }

        public void debugGuardConscript(ItemResourceType weapon)
        {
            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile()
                {
                    weapon = weapon,
                    armorLevel = ItemResourceType.IronArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.CityGuard,
                },
                skillBonus = 1,
            };

            Vector3 startPos = WP.ToWorldPos(VectorExt.AddY(tilePos, 1));
            for (int i = 0; i < 1; i++)
            {
                new GuardGroup(this, soldierProfile, startPos);
            }
        }

        public void debugGuardConscript(int idAndPosition)
        {
            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile()
                {
                    weapon =  ItemResourceType.Bow,
                    armorLevel = ItemResourceType.IronArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.CityGuard,
                },
                skillBonus = 1,
            };

            Vector3 startPos = WP.ToWorldPos(VectorExt.AddY(tilePos, 1));
            
            var guard = new GuardGroup(this, soldierProfile, startPos);
            guard.TeleportToDefencePost(this, idAndPosition, selectedDefenceBuilding);
        }
    }
}

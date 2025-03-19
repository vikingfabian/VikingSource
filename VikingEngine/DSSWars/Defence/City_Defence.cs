using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest.Players;
using VikingEngine.LootFest.GO.NPC;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public int selectedDefenceBuilding = -1;
        public List<DefenceStatus> defenceBuildings = new List<DefenceStatus>();

        public int defenceIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            return defenceIxFromPosId(id);
        }

        void assignNewGuardGroup(GuardGroup group)
        {
            //Find a free guard post or move to a guard house (or city center)
            Task.Factory.StartNew(() =>
            {

                int closestIx = -1;
                float closestDist = float.MaxValue;

                lock (defenceBuildings)
                {
                    for (int i = 0; i < defenceBuildings.Count; ++i)
                    {
                        var defence = defenceBuildings[i];
                        if (defence.checkSoldierAssignment(this))
                        {
                            defenceBuildings[i] = defence;
                        }

                        if (defence.AvailableForAutoAssign())
                        {
                            float dist = (defence.WorldPos() - group.position).PlaneXZLength();
                            if (dist < closestDist)
                            {
                                closestIx = i;
                                closestDist = dist; 
                            }
                        }
                    }

                    if (closestIx >= 0)
                    {
                        var defence = defenceBuildings[closestIx];
                        if (inRender_detailLayer)
                        {
                            new MoveCommand(group, defence.WorldPos(), false);
                            new EnterPostCommand(group, defence.idAndPosition, true).claimPost(group, this, closestIx);
                        }
                        else
                        {
                            group.completeTransform(SoldierTransformType.EnterGuard, defence.idAndPosition);
                        }
                    }
                }

               
            });
        }

        public int defenceIxFromPosId(int idAndPosition)
        {
            lock (defenceBuildings)
            {
                for (int i = 0; i < defenceBuildings.Count; ++i)
                {
                    if (defenceBuildings[i].idAndPosition == idAndPosition)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public void defence_assignGuard_toIndex(GuardGroup guard, int index)
        {
            var defence = defenceBuildings[index];
            guard.onEnterGuard(this, defence.idAndPosition);
            defence.soldierGroupId = guard.parentArrayIndex;
            defenceBuildings[index] = defence;

            switch (DssRef.world.subTileGrid.Get(conv.IntToIntVector2(defence.idAndPosition)).GetWallType())
            {
                case Map.TerrainWallType.NUM_NONE:
                    guard.damageBlockChance = 0;
                    break;
                case Map.TerrainWallType.DirtWall:
                case Map.TerrainWallType.DirtTower:
                    guard.damageBlockChance = DssConst.GuardPostDefenceChance_Dirt;
                    break;
                case Map.TerrainWallType.WoodWall:
                case Map.TerrainWallType.WoodTower:
                    guard.damageBlockChance = DssConst.GuardPostDefenceChance_Wood;
                    break;
                default:
                    guard.damageBlockChance = DssConst.GuardPostDefenceChance_Stone;
                    break;
            }
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

        public void debugGuardConscript(int idAndPosition, bool ranged)
        {
            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile()
                {
                    weapon = ranged? ItemResourceType.Bow : ItemResourceType.Sword,
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

        public void newGamePlaceGuard(int idAndPosition, int postIndex)
        {
            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile()
                {
                    weapon = ItemResourceType.Bow,
                    armorLevel = ItemResourceType.IronArmor,
                    training = TrainingLevel.Basic,
                    specialization = SpecializationType.CityGuard,
                },
                skillBonus = 1,
            };

            Vector3 startPos = WP.ToWorldPos(VectorExt.AddY(tilePos, 1));

            var guard = new GuardGroup(this, soldierProfile, startPos);
            guard.TeleportToDefencePost(this, idAndPosition, postIndex);
            soldiersCount += guard.soldierCount;
           //HousingCount_Guard -= guard
        }
    }
}

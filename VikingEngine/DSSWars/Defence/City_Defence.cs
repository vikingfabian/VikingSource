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
using VikingEngine.EngineSpace;
using VikingEngine.DebugExtensions;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City
    {
        public int selectedDefenceBuilding = -1;
        public StructList<DefenceStatus> defenceBuildings = new StructList<DefenceStatus>(64);

        public int defenceIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            return defenceIxFromPosId(id);
        }

        public void addDefenceBuilding_async(IntVector2 subPos, bool tower)
        {   
            lock (defenceBuildings.array)
            {
                DefenceStatus newDefence = new DefenceStatus();
                newDefence.init(subPos, tower);
                newDefence.autoAssign = true;

                for (int i = 0; i < defenceBuildings.Count; ++i)
                {
                    if (!defenceBuildings.array[i].active)
                    {
                        defenceBuildings[i] = newDefence;
                        return;
                    }
                }

                defenceBuildings.Add(newDefence);
            }            
        }

        public void destroyDefenceBuilding_async(IntVector2 subPos)
        {
            int id = conv.IntVector2ToInt(subPos);
            lock (defenceBuildings.array)
            {
                for (int i = 0; i < defenceBuildings.Count; ++i)
                {
                    if (defenceBuildings.array[i].idAndPosition == id)
                    {
                        var soldiers = defenceBuildings.array[i].soldierGroupId;
                        if (soldiers != DefenceStatus.NoSoldiers)
                        {
                            var group = groups.GetIndex_Safe(soldiers);
                            group?.completeTransform(SoldierTransformType.ExitGuard, 0);
                        }
                        defenceBuildings.array[i] = DefenceStatus.Empty;
                        return;
                    }
                }
            }
        }

        void assignNewGuardGroup(GuardGroup group)
        {
            //Find a free guard post or move to a guard house (or city center)
            Task.Factory.StartNew(() =>
            {
                try
                {
                int closestIx = -1;
                float closestDist = float.MaxValue;

                    lock (defenceBuildings.array)
                    {
                        for (int i = 0; i < defenceBuildings.Count; ++i)
                        {
                            var defence = defenceBuildings.array[i];
                            if (defence.checkSoldierAssignment(this))
                            {
                                defenceBuildings.array[i] = defence;
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
                            var defence = defenceBuildings.array[closestIx];
                            if (inRender_detailLayer)
                            {
                                new MoveCommand(group, defence.WorldPos(), float.MinValue, false);
                                new EnterPostCommand(group, defence.idAndPosition, true).claimPost(group, this, closestIx);
                            }
                            else
                            {
                                group.completeTransform(SoldierTransformType.EnterGuard, defence.idAndPosition);
                            }
                        }
                        else
                        {
                            Rotation1D dir = new Rotation1D(Ref.peRnd.Rotation());
                            float dist = Ref.peRnd.Float(WorldData.SubTileHalfWidth, WorldData.SubTileWidth * 2f);

                            group.goalWp = VectorExt.AddXZ(group.position, dir.Direction(dist));
                        }
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
                

               
            });
        }

        public int defenceIxFromPosId(int idAndPosition)
        {
            lock (defenceBuildings.array)
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

        public bool tryGetDefence(IntVector2 position, out DefenceStatus defence)
        {
            int idAndPosition = conv.IntVector2ToInt(position);
            lock (defenceBuildings.array)
            {
                for (int i = 0; i < defenceBuildings.Count; ++i)
                {
                    if (defenceBuildings[i].idAndPosition == idAndPosition)
                    {
                        defence = defenceBuildings[i];
                        return true;
                    }
                }
            }

            defence = DefenceStatus.Empty;
            return false;
        }

        public void setAllDefenceAutoAssign(bool toValue, bool towersOnly, bool message = true)
        {
            Task.Run(() =>
            {
                try
                {
                    int count = 0;
                    lock (defenceBuildings.array)
                    {                        
                        for (int i = 0; i < defenceBuildings.Count; ++i)
                        {
                            ref var defence = ref defenceBuildings.array[i];
                            if (!towersOnly || defence.tower)
                            {
                                if (defence.autoAssign != toValue)
                                {
                                    ++count;
                                    defence.autoAssign = toValue;
                                }
                            }
                        }
                    }

                    if (message)
                    {
                        var player = GetPlayer().GetLocalPlayer();
                        if (player != null)
                        {
                            Ref.update.AddSyncAction(new SyncAction2Arg<bool, int>(player.hud.messages.changedAllBuildings, toValue, count));
                        }
                    }
                }
                catch (Exception e)
                {
                    BlueScreen.ThreadException = e;
                }
            });
        }

        public void defence_assignGuard_toIndex(GuardGroup guard, int index)
        {
            var defence = defenceBuildings[index];
            guard.onEnterGuard(this, defence.idAndPosition);
            defence.soldierGroupId = guard.myIndex;
            defenceBuildings[index] = defence;

            //guard.refreshSoldierDefence();
            //switch (DssRef.world.subTileGrid.Get(conv.IntToIntVector2(defence.idAndPosition)).GetWallType())
            //{
            //    case Map.TerrainWallType.NUM_NONE:
            //        guard.damageBlockChance = 0;
            //        break;
            //    case Map.TerrainWallType.DirtWall:
            //    case Map.TerrainWallType.DirtTower:
            //        guard.damageBlockChance = DssConst.GuardPostDefenceChance_Dirt;
            //        break;
            //    case Map.TerrainWallType.WoodWall:
            //    case Map.TerrainWallType.WoodTower:
            //        guard.damageBlockChance = DssConst.GuardPostDefenceChance_Wood;
            //        break;
            //    default:
            //        guard.damageBlockChance = DssConst.GuardPostDefenceChance_Stone;
            //        break;
            //}
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

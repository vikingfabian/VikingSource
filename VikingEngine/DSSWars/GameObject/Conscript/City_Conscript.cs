using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject.Conscript;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ.Joust.DropItem;

namespace VikingEngine.DSSWars.GameObject
{

    /*
     * conscription är bunden till barracks
     * varje barrack tränar 1 grupp åt gången, välj 0-5 eller unlimited i kö, välj träning profil och slutmål
     * varje stad väljer conscription (och till vilken stad)
     */

    partial class City
    {
        public int selectedConscript = -1;
        public List<BarracksStatus> conscriptBuildings = new List<BarracksStatus>();
        Time conscriptDelay = Time.Zero;
        IntVector2 recruitToTile;
        public void async_conscriptUpdate(float time)
        {
            if (conscriptDelay.HasTime)
            {
                conscriptDelay.CountDown(time);
            }

            lock (conscriptBuildings)
            {
                for (int i = 0; i < conscriptBuildings.Count; i++)
                {
                    BarracksStatus status = conscriptBuildings[i];
                    if (i != selectedConscript || !conscriptDelay.HasTime || status.active == ConscriptActiveStatus.Training)
                    {
                        switch (status.active)
                        {
                            case ConscriptActiveStatus.Idle:
                                if (status.CountDownQue())
                                {
                                    status.active++;
                                    status.inProgress = status.profile;
                                    status.menCollected = 0;
                                    status.equipmentCollected = 0;
                                }
                                break;

                            case ConscriptActiveStatus.CollectingEquipment:
                                ItemResourceType weaponItem = ConscriptProfile.WeaponItem(status.inProgress.weapon);
                                ItemResourceType armorItem = ConscriptProfile.ArmorItem(status.inProgress.armorLevel);
                                int needEquipment = DssConst.SoldierGroup_DefaultCount - status.equipmentCollected;
                                int availableWeapons = GetGroupedResource(weaponItem).amount;
                                int availableArmor;
                                if (status.inProgress.armorLevel == ArmorLevel.None)
                                {
                                    availableArmor = needEquipment;
                                }
                                else
                                {
                                    availableArmor = GetGroupedResource(armorItem).amount;
                                }

                                int collectEquipment = lib.SmallestValue(needEquipment, availableWeapons, availableArmor);
                                status.equipmentCollected += collectEquipment;

                                AddGroupedResource(weaponItem, -collectEquipment);

                                if (status.inProgress.armorLevel != ArmorLevel.None)
                                {
                                    AddGroupedResource(armorItem, -collectEquipment);
                                }

                                if (status.equipmentCollected == DssConst.SoldierGroup_DefaultCount)
                                {
                                    status.active++;
                                }
                                break;

                            case ConscriptActiveStatus.CollectingMen:
                                int needMen = DssConst.SoldierGroup_DefaultCount - status.menCollected;
                                int collectMen = lib.SmallestValue(workForce, needMen);
                                workForce -= collectMen;
                                status.menCollected += collectMen;

                                if (status.menCollected == DssConst.SoldierGroup_DefaultCount)
                                {
                                    status.active++;
                                    status.countdown = new TimeInGameCountdown(new TimeLength(ConscriptProfile.TrainingTime(status.inProgress.training, status.nobelmen)));
                                }
                                break;

                            case ConscriptActiveStatus.Training:
                                if (status.countdown.TimeOut())
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<ConscriptProfile, int>(conscriptArmy, status.inProgress, 1));

                                    status.active = ConscriptActiveStatus.Idle;

                                    status.menCollected = 0;
                                    status.equipmentCollected = 0;

                                    if (status.inProgress.weapon == MainWeapon.KnightsLance &&
                                        status.inProgress.armorLevel == ArmorLevel.Heavy &&
                                        status.inProgress.training == TrainingLevel.Professional)
                                    {
                                        DssRef.achieve.UnlockAchievement_async(AchievementIndex.elite_knights);
                                    }

                                    if (Culture == CityCulture.Archers && status.inProgress.RangedUnit())
                                    {
                                        DssRef.state.progress.onCultureBuild(true);
                                    }
                                    else if (Culture == CityCulture.Warriors && !status.inProgress.RangedUnit())
                                    {
                                        DssRef.state.progress.onCultureBuild(false);
                                    }
                                }
                                break;
                        }
                    }
                    conscriptBuildings[i] = status;
                }
            }
        }

        public void onConscriptChange()
        {
            lock (conscriptBuildings)
            {
                conscriptDelay.Seconds = 1;

                BarracksStatus status = conscriptBuildings[selectedConscript];
                status.returnItems(this);
                //if (status.active == ConscriptActiveStatus.CollectingEquipment ||
                //    status.active == ConscriptActiveStatus.CollectingMen)
                //{
                //    //return items
                //    ItemResourceType weaponItem = ConscriptProfile.WeaponItem(status.inProgress.weapon);
                //    ItemResourceType armorItem = ConscriptProfile.ArmorItem(status.inProgress.armorLevel);

                //    AddGroupedResource(weaponItem, status.equipmentCollected);

                //    if (status.inProgress.armorLevel != ArmorLevel.None)
                //    {
                //        AddGroupedResource(armorItem, status.equipmentCollected);
                //    }

                //    workForce += status.menCollected;

                //    status.active = ConscriptActiveStatus.Idle;

                   
                //}
                conscriptBuildings[selectedConscript] = status;
            }
        }

        public void conscriptArmy(ConscriptProfile profile, int count)
        {
            Army army = recruitToClosestArmy();

            if (army == null)
            {
                //IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);

                army = faction.NewArmy(recruitToTile);
            }

            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = profile,
                skillBonus = 1,
            };

            if (Culture == CityCulture.Archers && soldierProfile.conscript.RangedUnit())
            {
                soldierProfile.skillBonus = 1.2f;
            }
            else if (Culture == CityCulture.Warriors && !soldierProfile.conscript.RangedUnit())
            {
                soldierProfile.skillBonus = 1.2f;
            }

            for (int i = 0; i < count; i++)
            {
                new SoldierGroup(army, soldierProfile);
            }
            army?.OnSoldierPurchaseCompleted();
        }

        public void debugConscript(MainWeapon weapon)
        {
            Army army = recruitToClosestArmy();

            if (army == null)
            {
                //IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);

                army = faction.NewArmy(recruitToTile);
            }

            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile() {
                    weapon = weapon,
                    armorLevel = ArmorLevel.Medium,
                    training = TrainingLevel.Basic,
                },
                skillBonus = 1,
            };

            for (int i = 0; i < 1; i++)
            {
                new SoldierGroup(army, soldierProfile);
            }
            army?.OnSoldierPurchaseCompleted();
        }

        public void CalcRecruitToTile()
        {
            foreach (IntVector2 dir in IntVector2.Dir4Array)
            {
                IntVector2 pos = tilePos + dir * 2;
                Tile t = DssRef.world.tileGrid.Get(pos);
                if (t.IsLand())
                {
                    recruitToTile = pos;
                    return;
                }
            }

            ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, 2));

            while (edgeLoop.Next())
            {
                Tile t = DssRef.world.tileGrid.Get(edgeLoop.Position);
                if (t.IsLand())
                {
                    recruitToTile = edgeLoop.Position;
                    return;
                }
            }
            foreach (IntVector2 dir in IntVector2.Dir4Array)
            {
                IntVector2 pos = tilePos + dir;
                Tile t = DssRef.world.tileGrid.Get(pos);
                if (t.IsLand())
                {
                    recruitToTile = pos;
                    return;
                }
            }
            Debug.LogError("GetFreeTile" + tilePos.ToString());
            recruitToTile = tilePos;
        }

        public void createStartupBarracks()
        {
            if (conscriptBuildings.Count == 0 &&
                !DssRef.storage.runTutorial)
            {
                IntVector2 pos = WP.ToSubTilePos_TopLeft(tilePos);
                pos.X += 4;
                pos.Y += 5;
                var subTile = DssRef.world.subTileGrid.Get(pos);
                subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.Barracks, 1);
                DssRef.world.subTileGrid.Set(pos, subTile);

                BarracksStatus newBarrack = new BarracksStatus()
                {
                    nobelmen = false,
                    idAndPosition = conv.IntVector2ToInt(pos),
                };
                newBarrack.profile.armorLevel = ArmorLevel.Light;

                conscriptBuildings.Add(newBarrack);
            }
        }

        public void addBarracks(IntVector2 subPos, bool nobelmen)
        {
            BarracksStatus consriptProfile = new BarracksStatus()
            {
                nobelmen = nobelmen,
                idAndPosition = conv.IntVector2ToInt(subPos),
            };

            consriptProfile.profile.defaultSetup(nobelmen);
            //if (nobelmen)
            //{
            //    consriptProfile.profile.training = TrainingLevel.Basic;
            //}

            lock (conscriptBuildings)
            {
                conscriptBuildings.Add(consriptProfile);
            }
        }
    }

    


    
}

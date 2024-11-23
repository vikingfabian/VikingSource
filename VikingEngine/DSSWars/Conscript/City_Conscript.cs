using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Component;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

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
                                ItemResourceType weaponItem = status.inProgress.weapon;
                                ItemResourceType armorItem = status.inProgress.armorLevel;
                                int needEquipment = DssConst.SoldierGroup_DefaultCount - status.equipmentCollected;
                                int availableWeapons = GetGroupedResource(weaponItem).amount;
                                int availableArmor;
                                if (status.inProgress.armorLevel == ItemResourceType.NONE)
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

                                if (status.inProgress.armorLevel !=  ItemResourceType.NONE)
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
                                int collectMen = lib.SmallestValue(workForce.amount, needMen);
                                workForce.amount -= collectMen;
                                status.menCollected += collectMen;

                                if (status.menCollected == DssConst.SoldierGroup_DefaultCount)
                                {
                                    status.active++;
                                    status.countdown = new TimeInGameCountdown(new TimeLength(ConscriptProfile.TrainingTime(status.inProgress.training, status.type)));
                                }
                                break;

                            case ConscriptActiveStatus.Training:
                                if (status.countdown.TimeOut())
                                {
                                    Ref.update.AddSyncAction(new SyncAction2Arg<ConscriptProfile, int>(conscriptArmy, status.inProgress, 1));

                                    status.active = ConscriptActiveStatus.Idle;

                                    status.menCollected = 0;
                                    status.equipmentCollected = 0;

                                    if (status.inProgress.weapon == ItemResourceType.KnightsLance &&
                                        (status.inProgress.armorLevel == ItemResourceType.FullPlateArmor || status.inProgress.armorLevel == ItemResourceType.MithrilArmor) &&
                                        status.inProgress.training == TrainingLevel.Professional)
                                    {
                                        DssRef.achieve.UnlockAchievement_async(AchievementIndex.elite_knights);
                                    }

                                    switch (Culture)
                                    { 
                                        case CityCulture.Archers:
                                            DssRef.state.progress.onCultureBuild(true);
                                            break;
                                        case CityCulture.Warriors:
                                            DssRef.state.progress.onCultureBuild(false);
                                            break;
                                    }
                                    //if (Culture == CityCulture.Archers && status.inProgress.RangedManUnit())
                                    //{
                                    //    DssRef.state.progress.onCultureBuild(true);
                                    //}
                                    //else if (Culture == CityCulture.Warriors && status.inProgress.MeleeSoldier())
                                    //{
                                    //    DssRef.state.progress.onCultureBuild(false);
                                    //}
                                }
                                break;
                        }
                    }
                    conscriptBuildings[i] = status;
                }
            }
        }

        public void toggleConscriptStop()
        {
            toggleConscriptStop(selectedConscript);
        }

        public bool toggleConscriptStop(int index)
        {
            if (arraylib.InBound(conscriptBuildings, index))
            {
                BarracksStatus currentStatus = conscriptBuildings[index];
                currentStatus.que = currentStatus.que > 0? 0 : 100;
                conscriptBuildings[index] = currentStatus;
                return currentStatus.que > 0;
            }
            return false;
        }

        public void copyConscript(LocalPlayer player)
        {
            copyConscript(player, selectedConscript);
        }

        public void copyConscript(LocalPlayer player, int index)
        {
            if (arraylib.InBound(conscriptBuildings, index))
            {
                BarracksStatus currentStatus = conscriptBuildings[index];
                switch (currentStatus.type)
                {
                    case BarracksType.Soldier:
                        player.soldierConscriptCopy = currentStatus.profile;
                        break;
                    case BarracksType.Archer:
                        player.archerConscriptCopy = currentStatus.profile;
                        break;
                    case BarracksType.Warmashine:
                        player.warmashineConscriptCopy = currentStatus.profile;
                        break;
                    case BarracksType.Knight:
                        player.knightConscriptCopy = currentStatus.profile;
                        break;
                    case BarracksType.Gun:
                        player.gunConscriptCopy = currentStatus.profile;
                        break;
                    case BarracksType.Cannon:
                        player.cannonConscriptCopy = currentStatus.profile;
                        break;

                }
            }
        }

        public void pasteConscript(LocalPlayer player)
        {
            pasteConscript(player, selectedConscript);
        }

        public void pasteConscript(LocalPlayer player, int index)
        {
            if (arraylib.InBound(conscriptBuildings, index))
            {
                BarracksStatus currentStatus = conscriptBuildings[index];

                switch (currentStatus.type)
                {
                    case BarracksType.Soldier:
                        currentStatus.profile=player.soldierConscriptCopy;
                        break;
                    case BarracksType.Archer:
                        currentStatus.profile= player.archerConscriptCopy;
                        break;
                    case BarracksType.Warmashine:
                        currentStatus.profile=player.warmashineConscriptCopy ;
                        break;
                    case BarracksType.Knight:
                        currentStatus.profile=player.knightConscriptCopy;
                        break;
                    case BarracksType.Gun:
                        currentStatus.profile=player.gunConscriptCopy;
                        break;
                    case BarracksType.Cannon:
                        currentStatus.profile=player.cannonConscriptCopy;
                        break;

                }

                conscriptBuildings[index] = currentStatus;
            }
        }

        //public void toggleConscriptStop()
        //{
        //    toggleConscriptStop(selectedConscript);
        //}

        //public void toggleConscriptStop(int index)
        //{
        //    if (arraylib.InBound(deliveryServices, index))
        //    {
        //        DeliveryStatus currentStatus = deliveryServices[index];
        //        currentStatus.que = currentStatus.que > 0 ? 0 : 100;
        //        deliveryServices[index] = currentStatus;
        //    }
        //}

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
                IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);

                army = faction.NewArmy(onTile);
            }

            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = profile,
                skillBonus = 1,
            };

            switch (Culture)
            {
                case CityCulture.Archers:
                    if (soldierProfile.conscript.RangedManUnit())
                    {
                        soldierProfile.skillBonus = 1.2f;
                    }
                    break;
                case CityCulture.Warriors:
                    if (soldierProfile.conscript.MeleeSoldier())
                    {
                        soldierProfile.skillBonus = 1.2f;
                    }
                    break;
                case CityCulture.Nobelmen:
                    if (soldierProfile.conscript.KnightUnit())
                    {
                        soldierProfile.skillBonus = 1.2f;
                    }
                    break;
                case CityCulture.Seafaring:
                    if (soldierProfile.conscript.specialization == SpecializationType.Sea)
                    {
                        soldierProfile.skillBonus = 1.2f;
                    }
                    break;
                case CityCulture.SiegeEngineer:
                    if (soldierProfile.conscript.Warmashine())
                    {
                        soldierProfile.skillBonus = 1.2f;
                    }
                    break;

            }
            
            for (int i = 0; i < count; i++)
            {
                new SoldierGroup(army, soldierProfile);
            }
            army?.OnSoldierPurchaseCompleted();
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
                subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, 1);
                DssRef.world.subTileGrid.Set(pos, subTile);
                //EditSubTile edit = new EditSubTile(pos, subTile, true, false, false);
                //edit.Submit();

                BarracksStatus newBarrack = new BarracksStatus()
                {
                    profile = new ConscriptProfile() { weapon = ItemResourceType.SharpStick },
                    type = BarracksType.Soldier,
                    idAndPosition = conv.IntVector2ToInt(pos),
                };
                newBarrack.profile.armorLevel = ItemResourceType.PaddedArmor;

                conscriptBuildings.Add(newBarrack);
            }
        }

        public void addBarracks(IntVector2 subPos, BarracksType type)
        {
            BarracksStatus consriptProfile = new BarracksStatus()
            {
                type = type,
                idAndPosition = conv.IntVector2ToInt(subPos),
            };

            consriptProfile.profile.defaultSetup(type);
            //if (nobelmen)
            //{
            //    consriptProfile.profile.training = TrainingLevel.Basic;
            //}

            lock (conscriptBuildings)
            {
                conscriptBuildings.Add(consriptProfile);
            }
        }

        public int conscriptIxFromSubTile(IntVector2 subTilePos)
        {
            int id = conv.IntVector2ToInt(subTilePos);
            for (int i = 0; i < conscriptBuildings.Count; ++i)
            {
                if (conscriptBuildings[i].idAndPosition == id)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool GetConscript(IntVector2 subTilePos, out BarracksStatus status)
        {
            var index = conscriptIxFromSubTile(subTilePos);
            if (arraylib.InBound(conscriptBuildings, index))
            {
                status = conscriptBuildings[index];
                return true;
            }

            status = new BarracksStatus();
            return false;
        }
    }   
}

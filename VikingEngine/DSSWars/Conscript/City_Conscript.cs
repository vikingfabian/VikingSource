using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Interface.Component;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
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
        public IntVector2 recruitToTile;
        public void async_conscriptUpdate(float time)
        {
            if (myIndex == 153)
            {
                lib.DoNothing();
            }
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
                                    status.unitsCollected = 0;
                                }
                                break;

                            case ConscriptActiveStatus.CollectingEquipment:
                               
                                status.payItems(this, CommitOption.Commit, out int totalMen, out bool allCollected);

                                if (allCollected &&
                                    (status.profile.specialization != SpecializationType.CityGuard || AvailableGuardHousing() >= totalMen))
                                {
                                    status.active++;
                                    status.countdown = new TimeInGameCountdown(new TimeLength(ConscriptProfile.TrainingTime(status.inProgress.training, status.inProgress.animal, status.type)));
                                }
                                break;

                            case ConscriptActiveStatus.Training:
                                if (status.countdown.TimeOut())
                                {
                                    Vector3 startPos = WP.SubtileToWorldPosXZgroundY_Centered(conv.IntToIntVector2(status.idAndPosition));
                                    Ref.update.AddSyncAction(new SyncAction3Arg<ConscriptProfile, Vector3, int>(conscriptArmyLink, status.inProgress, startPos, 1));

                                    status.active = ConscriptActiveStatus.Idle;
                                    status.unitsNeeded = 0;
                                    status.unitsCollected = 0;

                                    if (GetPlayer().IsLocalPlayer())
                                    {
                                        if (status.inProgress.specialization == SpecializationType.CityGuard)
                                        {
                                            DssRef.stats.guardsRecruited++;
                                        }

                                        if (status.inProgress.man == ItemResourceType.NobleMen && status.inProgress.animal == ItemResourceType.WarHorse)
                                        {
                                            DssRef.achieve.UnlockAchievement_async(AchievementIndex.knights);
                                        }

                                        switch (status.inProgress.weapon)
                                        {
                                            case ItemResourceType.LongSword:
                                                if (status.inProgress.armorLevel == ItemResourceType.LightPlateArmor ||
                                                    status.inProgress.armorLevel == ItemResourceType.FullPlateArmor)
                                                {
                                                    DssRef.achieve.UnlockAchievement_async(AchievementIndex.men_of_steel);
                                                }
                                                break;
                                            //case ItemResourceType.KnightsLance:
                                            //    DssRef.achieve.UnlockAchievement_async(AchievementIndex.knights);
                                            //    break;
                                            case ItemResourceType.ManCannonIron:
                                            case ItemResourceType.SiegeCannonIron:
                                                DssRef.achieve.UnlockAchievement_async(AchievementIndex.iron_cannon);
                                                break;
                                        }

                                        switch (status.inProgress.animal)
                                        {
                                            case ItemResourceType.WarHorse:
                                                if (status.inProgress.man == ItemResourceType.NobleMen)
                                                {
                                                    DssRef.achieve.UnlockAchievement_async(AchievementIndex.knights);
                                                }
                                                break;
                                            case ItemResourceType.AlphaWarg:
                                                DssRef.achieve.UnlockAchievement_async(AchievementIndex.the_alpha);
                                                break;

                                            case ItemResourceType.Oliphant:
                                                switch (status.inProgress.weapon)
                                                {
                                                    case ItemResourceType.ManCannonBronze:
                                                    case ItemResourceType.ManCannonIron:
                                                        DssRef.achieve.UnlockAchievement_async(AchievementIndex.cannonphant);
                                                        break;
                                                }
                                                break;

                                        }
                                    }
                                }
                                //else if (status.countdown.TimePassed().seconds < 10 &&
                                //    !status.inProgress.Equals(status.profile))
                                //{
                                //    status.active = ConscriptActiveStatus.Idle;
                                //    status.unitsNeeded = 0;
                                //    status.unitsCollected = 0;
                                //    status.RevertCountDown();
                                //    status.returnItems(this);
                                //}

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
                //status.returnItems(this);
                if (status.countdown.TimePassed().seconds < 10 )
                    //&&
                    //!status.inProgress.Equals(status.profile))
                {
                    status.active = ConscriptActiveStatus.Idle;
                    status.unitsNeeded = 0;
                    status.unitsCollected = 0;
                    status.RevertCountDown();
                    status.returnItems(this);
                }

                conscriptBuildings[selectedConscript] = status;
            }
        }

        public void queueToAllConscripts(int count, LocalPlayer player)
        {
            for (int i = 0; i < conscriptBuildings.Count; ++i)
            {
                if (player == null ||
                    player.conscriptSubTab == BuildAndExpandType.ALL ||
                    player.conscriptSubTab == conscriptBuildings[i].type)
                {
                    var status = conscriptBuildings[i];
                    if (count == 1)
                    {
                        status.que++;
                    }
                    else
                    {
                        status.que = count;
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

        void haltAllConscriptProgress()
        {
            lock (conscriptBuildings)
            {
                for (int i = 0; i < conscriptBuildings.Count; i++)
                {
                    BarracksStatus currentStatus = conscriptBuildings[i];
                    currentStatus.que = currentStatus.que > 0 ? 0 : 100;
                    conscriptBuildings[i] = currentStatus;
                }
            }
        }

        public void copyConscript(LocalPlayer player, int index)
        {
            if (arraylib.InBound(conscriptBuildings, index))
            {
                BarracksStatus currentStatus = conscriptBuildings[index];
                switch (currentStatus.type)
                {
                    case Build.BuildAndExpandType.SoldierBarracks:
                        player.soldierConscriptCopy = currentStatus;
                        break;
                    case Build.BuildAndExpandType.ArcherBarracks:
                        player.archerConscriptCopy = currentStatus;
                        break;
                    case Build.BuildAndExpandType.WarmachineBarracks:
                        player.warmachineConscriptCopy = currentStatus;
                        break;
                    //case Build.BuildAndExpandType.KnightsBarracks:
                    //    player.knightConscriptCopy = currentStatus;
                    //    break;
                    case Build.BuildAndExpandType.GunBarracks:
                        player.gunConscriptCopy = currentStatus;
                        break;
                    case Build.BuildAndExpandType.CannonBarracks:
                        player.cannonConscriptCopy = currentStatus;
                        break;

                }
            }
        }

        public void pasteConscriptToAll(LocalPlayer player)
        {
            for (int i = 0; i < conscriptBuildings.Count; ++i)
            {
                if (player.conscriptSubTab == BuildAndExpandType.ALL ||
                    player.conscriptSubTab == conscriptBuildings[i].type)
                {
                    pasteConscript(player, i);
                }
            }
        }

        public void pasteConscript(LocalPlayer player)
        {
            if (selectedConscript < 0)
            {
                pasteConscriptToAll(player);
            }
            else
            {
                pasteConscript(player, selectedConscript);
            }
        }

        public void pasteConscript(LocalPlayer player, int index)
        {
            if (arraylib.InBound(conscriptBuildings, index))
            {
                BarracksStatus currentStatus = conscriptBuildings[index];

                switch (currentStatus.type)
                {
                    case Build.BuildAndExpandType.SoldierBarracks:
                        currentStatus.paste(player.soldierConscriptCopy);
                        break;
                    case Build.BuildAndExpandType.ArcherBarracks:
                        currentStatus.paste(player.archerConscriptCopy);
                        break;
                    case Build.BuildAndExpandType.WarmachineBarracks:
                        currentStatus.paste(player.warmachineConscriptCopy) ;
                        break;
                    //case Build.BuildAndExpandType.KnightsBarracks:
                    //    currentStatus.paste(player.knightConscriptCopy);
                    //    break;
                    case Build.BuildAndExpandType.GunBarracks:
                        currentStatus.paste(player.gunConscriptCopy);
                        break;
                    case Build.BuildAndExpandType.CannonBarracks:
                        currentStatus.paste(player.cannonConscriptCopy);
                        break;

                }

                conscriptBuildings[index] = currentStatus;
            }
        }

        public Vector3 defaultConscriptPos()
        {
            Vector3 startPos;
            if (conscriptBuildings.Count > 0)
            {
                startPos = WP.SubtileToWorldPosXZgroundY_Centered(conv.IntToIntVector2(conscriptBuildings[0].idAndPosition));
            }
            else
            {
                startPos = WP.ToWorldPos(tilePos);
            }

            return startPos;
        }

        public IntVector2 defaultConscriptSubtilePos()
        {
            if (conscriptBuildings.Count > 0)
            {
               return conv.IntToIntVector2(conscriptBuildings[0].idAndPosition);
            }
            else
            {
                return cityHallSubtilePos;
            }
        }

        

        public void conscriptSettlerLink()
        {
            conscriptSettler(null, false);
        }

        public void conscriptSettlerLink_Free()
        {
            SettlerBp().addResources(this);
            conscriptSettlerLink();
        }

        public CraftBlueprint SettlerBp()
        {
            return cityCulture == CityCulture.Nomads ? ConscriptDataLib.CraftNomadSettler : ConscriptDataLib.CraftSettler;
        }

        public void aiConscriptSettler(City settleArea)
        {
            conscriptSettler(settleArea, true);
        }
        public Army conscriptSettler(City settleArea, bool checkIfExists)
        {
            Army army = recruitToClosestArmy();
            
            if ((!checkIfExists || army == null || !army.HasSettler(out _)) &&
                SettlerBp().tryPayResources(this))
            {
                army = conscriptArmy(new ConscriptProfile()
                {
                    weapon = ItemResourceType.Settler,
                    armorLevel = ItemResourceType.NONE,
                    specialization = SpecializationType.None,
                    training = TrainingLevel.Minimal,
                }, defaultConscriptPos(), 1) as Army;

                if (settleArea != null)
                {
                    army.Ai_Order_MoveTo(settleArea.tilePos);
                }
            }

            return army;
        }

        public void conscriptArmyLink(ConscriptProfile profile, Vector3 startPos, int count)
        {
            conscriptArmy(profile, startPos, count);
        }

        public AbsArmy conscriptArmy(ConscriptProfile profile, Vector3 startPos, int count)
        {
            AbsArmy army = null;

            if (profile.specialization != SpecializationType.CityGuard)
            {   
                army = recruitToClosestArmy();

                if (army == null)
                {
                    army = GetFaction().NewArmy(recruitToTile);
                }
            }
            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = profile,
                skillBonus = profile.man == ItemResourceType.NobleMen? DssConst.NobelMenSkillBonus : 1,
            };

            soldierProfile.conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);


            switch (cityCulture)
            {
               
                case CityCulture.Archers:
                    if (rangedMan)
                    {
                        soldierProfile.skillBonus *= 1.2f;
                    }
                    break;
                case CityCulture.Warriors:
                    if (meleeMan)
                    {
                        soldierProfile.skillBonus *= 1.2f;
                    }
                    break;
                case CityCulture.Noblemen:
                    if (profile.man == ItemResourceType.NobleMen)
                    {
                        soldierProfile.skillBonus *= 1.2f;
                    }
                    break;
                case CityCulture.Seafaring:
                    if (soldierProfile.conscript.specialization == SpecializationType.Sea)
                    {
                        soldierProfile.skillBonus *= 1.2f;
                    }
                    break;
                case CityCulture.SiegeEngineer:
                    if (warmachine)
                    {
                        soldierProfile.skillBonus *= 1.2f;
                    }
                    break;
                case CityCulture.Wheelwright:
                    if (wagonRide)
                    {
                        soldierProfile.mobileBonus_PercAdd = Culture.WheelWhrightBonus;
                    }
                    break;
            }

            if (profile.specialization == SpecializationType.CityGuard)
            {
                for (int i = 0; i < count; i++)
                {
                    var group = new GuardGroup(this, soldierProfile, startPos);
                    soldiersCount += group.soldierCount;

                    assignNewGuardGroup(group);
                    
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    new SoldierGroup(army, soldierProfile, startPos);
                }
                army?.GetArmy().OnSoldierPurchaseCompleted();
            }

            return army;
        }

        public void debugConscript(ItemResourceType weapon)
        {
            Army army = recruitToClosestArmy();

            if (army == null)
            {
                army = GetFaction().NewArmy(recruitToTile);
            }

            SoldierConscriptProfile soldierProfile = new SoldierConscriptProfile()
            {
                conscript = new ConscriptProfile() {
                    weapon = weapon,
                    armorLevel =  ItemResourceType.IronArmor,
                    training = TrainingLevel.Professional,
                },
                skillBonus = 1,
            };

            Vector3 startPos = WP.ToWorldPos(tilePos);
            for (int i = 0; i < 5; i++)
            {
                new SoldierGroup(army, soldierProfile, startPos);
            }

            army.setAsStartArmy();
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
                cityType > CityType.Campsite &&
                barracksReservedSpot.X > 0 &&
                !DssRef.storage.runTutorial)
            {
                ref var subTile = ref DssRef.world.subTileGrid.GetRef(barracksReservedSpot);
                subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.SoldierBarracks, 1);
                
                BarracksStatus newBarrack = new BarracksStatus(Build.BuildAndExpandType.SoldierBarracks);
                newBarrack.idAndPosition = conv.IntVector2ToInt(barracksReservedSpot);
                newBarrack.profile.armorLevel = ItemResourceType.PaddedArmor;

                conscriptBuildings.Add(newBarrack);


            }
        }

        public void addBarracks(IntVector2 subPos, Build.BuildAndExpandType type)
        {
            BarracksStatus consriptProfile = new BarracksStatus(type)
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
            };

            lock (conscriptBuildings)
            {
                conscriptBuildings.Add(consriptProfile);
            }
        }

        public void destroyBarracks(IntVector2 subPos)
        {
            lock (conscriptBuildings)
            {
               int index =  conscriptIxFromSubTile(subPos);
                conscriptBuildings[index].returnItems(this);
                conscriptBuildings.RemoveAt(index);
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

        public bool hasConscriptId(int id)
        {
            for (int i = 0; i < conscriptBuildings.Count; ++i)
            {
                if (conscriptBuildings[i].idAndPosition == id)
                {
                    return true;
                }
            }

            return false;
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

        //protected void DispandGuards()
        //{
        //    var counter = groups.counter();
        //    while (counter.Next())
        //    {
        //        counter.sel.DeleteMe(DeleteReason.Disband, false);
        //    }
        //}
    }   
}

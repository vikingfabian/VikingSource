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

        public List<BarracksStatus> barracks = new List<BarracksStatus>();

        public void async_conscriptUpdate()
        {
            lock (barracks)
            {
                for (int i = 0; i < barracks.Count; i++)
                {
                    BarracksStatus status = barracks[i];
                    switch (status.active)
                    { 
                        case ConscriptActiveStatus.Idle:
                            if (status.CountDownQue())
                            {
                                status.active++;
                                status.inProgress = status.profile;
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

                            var weapon = GetGroupedResource(weaponItem);
                            weapon.amount -= collectEquipment;
                            SetGroupedResource(weaponItem, weapon);

                            if (status.inProgress.armorLevel != ArmorLevel.None)
                            {
                                var armor = GetGroupedResource(armorItem);
                                armor.amount -= collectEquipment;
                                SetGroupedResource(armorItem, armor);
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
                                status.countdown = new TimeInGameCountdown(new TimeLength(ConscriptProfile.TrainingTime(status.inProgress.training)));
                            }
                            break;

                        case ConscriptActiveStatus.Training:
                            if (status.countdown.TimeOut())
                            {
                                Ref.update.AddSyncAction(new SyncAction1Arg<ConscriptProfile>(conscriptArmy, status.inProgress));

                                status.active = ConscriptActiveStatus.Idle;
                                
                                status.menCollected = 0;
                                status.equipmentCollected = 0;
                            }
                            break;
                    }

                    barracks[i] = status;
                }
            }
        }

        public void conscriptArmy(ConscriptProfile profile)
        {
            Army army = recruitToClosestArmy();

            if (army == null)
            {
                IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);

                army = faction.NewArmy(onTile);//new Army(faction, onTile);
            }

            SoldierProfile soldierProfile = new SoldierProfile()
            {
                conscript = profile,
                skillBonus = 1,
            };

            if (Culture == CityCulture.Archers && soldierProfile.RangedUnit())
            {
                soldierProfile.skillBonus = 1.2f;
            }
            else if (Culture == CityCulture.Warriors && !soldierProfile.RangedUnit())
            {
                soldierProfile.skillBonus = 1.2f;
            }

            new SoldierGroup(army, soldierProfile);
            
            army?.OnSoldierPurchaseCompleted();
        }

        public void createStartupBarracks()
        {
            if (barracks.Count == 0)
            {
                IntVector2 pos = WP.ToSubTilePos_TopLeft(tilePos);
                pos.X += 4;
                pos.Y += 5;
                var subTile = DssRef.world.subTileGrid.Get(pos);
                subTile.SetType(TerrainMainType.Building, (int)TerrainBuildingType.Barracks, 1);
                DssRef.world.subTileGrid.Set(pos, subTile);

                BarracksStatus newBarrack = new BarracksStatus()
                {
                    idAndPosition = conv.IntVector2ToInt(pos),
                    //que = 2,
                };
                newBarrack.profile.armorLevel = ArmorLevel.Light;

                barracks.Add(newBarrack);
            }
        }

        public void addBarracks(IntVector2 subPos)
        {
            BarracksStatus consriptProfile = new BarracksStatus()
            {
                idAndPosition = conv.IntVector2ToInt(subPos),
            };
            lock (barracks)
            {
                barracks.Add(consriptProfile);
            }
        }
    }

    


    
}

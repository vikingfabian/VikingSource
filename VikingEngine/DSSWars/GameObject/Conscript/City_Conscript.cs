using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
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
                        
            new SoldierGroup(army, profile);
            
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

    class ConscriptMenu
    {
        City city;
        LocalPlayer player;
        //ConsriptProfile currentProfile = new ConsriptProfile();

        public void ToHud(City city, LocalPlayer player, RichBoxContent content)
        {
            content.newLine();

            this.city = city;
            this.player = player;

            
            if (arraylib.InBound(city.barracks, city.selectedConscript))
            {
                BarracksStatus currentProfile = get();

                content.Add(new RichBoxBeginTitle(1));
                
                
                content.Add(new RichBoxText(DssRef.todoLang.Conscription_Title + " " + currentProfile.idAndPosition.ToString()));
                content.space();
                content.Add(new RichboxButton(new List<AbsRichBoxMember>
                    { new RichBoxSpace(), new RichBoxText(DssRef.todoLang.Hud_EndSessionIcon),new RichBoxSpace(), },
                    new RbAction(() => { city.selectedConscript = -1; })));

                content.newParagraph();

                HudLib.Label(content, "Weapon");
                content.newLine();
                for (MainWeapon weapon = 0; weapon < MainWeapon.NUM; weapon++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Weapon(weapon))
                }, 
                new RbAction1Arg<MainWeapon>(weaponClick, weapon),
                new RbAction1Arg<MainWeapon>(weaponTooltip, weapon)
                );
                    button.setGroupSelectionColor(HudLib.RbSettings, weapon == currentProfile.profile.weapon);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, "Armor");
                content.newLine();
                for (ArmorLevel armorLvl = 0; armorLvl < ArmorLevel.NUM; armorLvl++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( LangLib.Armor(armorLvl))
                    }, new RbAction1Arg<ArmorLevel>(armorClick, armorLvl),
                    new RbAction1Arg<ArmorLevel>(armorTooltip, armorLvl));
                    button.setGroupSelectionColor(HudLib.RbSettings, armorLvl == currentProfile.profile.armorLevel);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, "Training");
                content.newLine();
                for (TrainingLevel training = 0; training < TrainingLevel.NUM; training++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                   new RichBoxText( LangLib.Training(training))
                }, new RbAction1Arg<TrainingLevel>(trainingClick, training),
                    new RbAction1Arg<TrainingLevel>(trainingTooltip, training));
                    button.setGroupSelectionColor(HudLib.RbSettings, training == currentProfile.profile.training);
                    content.Add(button);
                    content.space();
                }

                content.newParagraph();

                HudLib.Label(content, DssRef.todoLang.Hud_Que);
                content.space();
                HudLib.InfoButton(content, new RbAction(queInfo));
                content.newLine();
                for (int length = 0; length <= BarracksStatus.MaxQue; length++)
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText( length.ToString())
                    }, new RbAction1Arg<int>(queClick, length));
                    button.setGroupSelectionColor(HudLib.RbSettings, length == currentProfile.que);
                    content.Add(button);
                    content.space();
                }
                {
                    var button = new RichboxButton(new List<AbsRichBoxMember>{
                       new RichBoxText(DssRef.todoLang.Hud_NoLimit)
                    }, new RbAction1Arg<int>(queClick, 1000));
                    button.setGroupSelectionColor(HudLib.RbSettings, currentProfile.que > BarracksStatus.MaxQue);
                    content.Add(button);
                }

                if (currentProfile.active != ConscriptActiveStatus.Idle)
                {
                    content.newParagraph();
                    content.Add(new RichBoxSeperationLine());
                    {
                        content.newLine();
                        content.BulletPoint();
                        var text = new RichBoxText(currentProfile.activeStringOf(ConscriptActiveStatus.CollectingEquipment));
                        text.overrideColor = currentProfile.active > ConscriptActiveStatus.CollectingEquipment ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        content.BulletPoint();
                        var text = new RichBoxText(currentProfile.activeStringOf(ConscriptActiveStatus.CollectingMen));
                        text.overrideColor = currentProfile.active > ConscriptActiveStatus.CollectingMen ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                        content.Add(text);
                    }
                    {
                        content.newLine();
                        content.BulletPoint();
                        content.Add(new RichBoxText(currentProfile.longTimeProgress()));
                    }
                }
            }
            else
            {
                
                content.h2("Select barracks");
                if (city.barracks.Count == 0)
                { 
                    content.text("- Empty list -").overrideColor = HudLib.InfoYellow_Light;
                }

                for (int i = 0; i < city.barracks.Count; ++i)
                {
                    content.newLine();

                    BarracksStatus currentProfile = city.barracks[i];
                    var caption = new RichBoxText(
                            LangLib.Weapon(currentProfile.profile.weapon) + ", " +
                            LangLib.Armor(currentProfile.profile.armorLevel) + ", " +
                            LangLib.Training(currentProfile.profile.training)
                        );
                    caption.overrideColor = HudLib.TitleColor_Name;

                    content.Add(new RichboxButton(new List<AbsRichBoxMember>(){
                        new RichBoxBeginTitle(2),
                        caption,
                        new RichBoxNewLine(),
                        new RichBoxText(currentProfile.shortActiveString())
                    }, new RbAction1Arg<int>(selectClick, i)));

                }
            }
        }

       

        void weaponClick(MainWeapon weapon)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.weapon = weapon;
            set(currentProfile);
        }

        void weaponTooltip(MainWeapon weapon)
        {
            string WeaponDamage = "Weapon damage: {0}";

            
            RichBoxContent content = new RichBoxContent();
            content.Add(new RichBoxText(string.Format(WeaponDamage, ConscriptProfile.WeaponDamage(weapon))));

            player.hud.tooltip.create(player, content, true);
        }
        void armorClick(ArmorLevel armor)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.armorLevel = armor;
            set(currentProfile);
        }
        void armorTooltip(ArmorLevel armor)
        {
            string ArmorHealth = "Armor health: {0}";

            
            RichBoxContent content = new RichBoxContent();
            content.Add(new RichBoxText(string.Format(ArmorHealth, ConscriptProfile.ArmorHealth(armor))));

            player.hud.tooltip.create(player, content, true);
        }

        void trainingClick(TrainingLevel training)
        {
            BarracksStatus currentProfile = get();
            currentProfile.profile.training = training;
            
            set(currentProfile);
        }
        void trainingTooltip(TrainingLevel training)
        {
            string TrainingSpeed = "Attack speed: {0}";
            string TrainingTime = "Training time: {0}";

            RichBoxContent content = new RichBoxContent();
            content.text(string.Format(TrainingTime, new TimeLength(ConscriptProfile.TrainingTime(training)).LongString()));
            content.text(string.Format(TrainingSpeed, TextLib.OneDecimal(ConscriptProfile.TrainingAttackSpeed(training))));

            player.hud.tooltip.create(player, content, true);
        }
        void queClick(int length)
        {
            BarracksStatus currentProfile = get();
            currentProfile.que = length;
            set(currentProfile);
        }

        void selectClick(int index)
        {
            city.selectedConscript = index;
        }

        BarracksStatus get()
        {
            return city.barracks[city.selectedConscript];
        }

        void set(BarracksStatus profile)
        {
            city.barracks[city.selectedConscript] = profile;
        }


        void queInfo()
        { 
            RichBoxContent content = new RichBoxContent();

            content.text("Will keep traning soldiers until the que is empty");

            player.hud.tooltip.create(player, content, true);
        }
    }


    
}

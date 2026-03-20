using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Animal;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Conscript
{
    struct BarracksStatus
    {
        public const int MaxQue = 5;

        public ConscriptActiveStatus active;
        public ConscriptProfile profile;

        public ConscriptProfile inProgress;
        public TimeInGameCountdown countdown;
        public BuildAndExpandType type;
        public int menCollected;
        public int menNeeded;
        public int equipmentCollected;

        public int idAndPosition;
        public TrainingLevel maxTrainingLevel;
        public int que;

        public bool requireMaxPopulation;
        public bool requireMaxFood;

        public BarracksStatus(BuildAndExpandType type)
            : this()
        {
            this.type = type;

            switch (type)
            {
                case BuildAndExpandType.SoldierBarracks:
                    profile.weapon = ItemResourceType.SharpStick;
                    break;
                case BuildAndExpandType.ArcherBarracks:
                    profile.weapon = ItemResourceType.SlingShot;
                    break;
                case BuildAndExpandType.WarmachineBarracks:
                    profile.weapon = ItemResourceType.Ballista;
                    break;
                //case BuildAndExpandType.KnightsBarracks:
                //    profile.weapon = ItemResourceType.Warhammer;
                //    break;
                case BuildAndExpandType.GunBarracks:
                    profile.weapon = ItemResourceType.HandCannon;
                    break;
                case BuildAndExpandType.CannonBarracks:
                    profile.weapon = ItemResourceType.ManCannonBronze;
                    break;

            } 
            profile.man = ItemResourceType.Men;
            profile.training = TrainingLevel.Basic;
            maxTrainingLevel = TrainingLevel.Skillful;
        }

        public void checkSpecialization()
        {
            var spec = profile.avaialableSpecializations(type, out bool mayGuard);

            if (profile.specialization == SpecializationType.CityGuard)
            {
                if (!mayGuard)
                {
                    profile.specialization = spec[0];
                }
            }
            else if (!spec.Contains(profile.specialization))
            {
                profile.specialization = spec[0];
            }
        }

        //public void reseet()
        //{ 
            
        //}

        public void paste(BarracksStatus stored)
        {
            profile = stored.profile;
            requireMaxFood = stored.requireMaxFood;
            requireMaxPopulation = stored.requireMaxPopulation;
        }

        public void halt(City city)
        {
            que = 0;

            returnItems(city);

        }

        public int payItems(City city, bool men, CommitOption commit)
        {
            xx
            int needMenEquipment = menNeeded - equipmentCollected;
            var me = this;

            int allItems(int mencount, int animalCount, int unitCount, bool commit)
            {
                payItem(me.profile.weapon, mencount, commit);
                payItem(me.profile.shield, mencount, commit);
                payItem(me.profile.armorLevel, mencount, commit);
                
                payItem(me.profile.weapon, mencount, commit);
                payItem(me.profile.weapon, mencount, commit);
                return -1;
            }            

            int payItem(ItemResourceType item, int count, bool commit)
            {
                if (item == ItemResourceType.NONE)
                {
                    return count;
                }

                if (commit)
                {
                    city.AddGroupedResource(item, count);
                    return 0;
                }
                else
                {
                    int available = city.GetGroupedResource(item).amount;
                    return available;
                }
            }

            return -1;
        }

        public void returnItems(City city)
        {
            if (active == ConscriptActiveStatus.CollectingEquipment ||
                    active == ConscriptActiveStatus.CollectingMen)
            {
                //return items
                ItemResourceType weaponItem = inProgress.weapon;
                ItemResourceType armorItem = inProgress.armorLevel;

                city.AddGroupedResource(weaponItem, equipmentCollected);

                if (inProgress.armorLevel != ItemResourceType.NONE)
                {
                    city.AddGroupedResource(armorItem, equipmentCollected);
                }

                city.workForce.amount += menCollected;

                active = ConscriptActiveStatus.Idle;

            }
        }

        public void followsRequirements(City city, out bool population, out bool food)
        {
            if (requireMaxPopulation)
            {
                population = city.workForce.amount >= city.workersMax() - 10;
            }
            else
            {
                population = true;
            }

            if (requireMaxFood)
            {
                var res_food = city.GetRefGroupedResource(EntityComponent.CityResoureIndex.food);
                food = res_food.amount >= res_food.stockPileLimit - 50;
            }
            else
            {
                food = true;
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            Debug.WriteCheck(w);

            w.Write((byte)active);
            profile.writeGameState(w);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.writeGameState(w);
            }
            switch (active)
            {
                case ConscriptActiveStatus.CollectingEquipment:
                    w.Write((byte)equipmentCollected);
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    w.Write((byte)menCollected);
                    break;

                case ConscriptActiveStatus.Training:
                    countdown.writeGameState(w);
                    break;
            }
            w.Write((byte)type);
            w.Write(idAndPosition);
            w.Write(Bound.Byte(que));
            w.Write((byte)maxTrainingLevel);


            new EightBit(requireMaxPopulation, requireMaxFood).write(w);

            Debug.WriteCheck(w);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            Debug.ReadCheck(r);

            active = (ConscriptActiveStatus)r.ReadByte();
            profile.readGameState(r);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.readGameState(r);
                menNeeded = inProgress.menCost();
            }
            switch (active)
            {
                case ConscriptActiveStatus.CollectingEquipment:
                    equipmentCollected = r.ReadByte();
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    equipmentCollected = DssConst.SoldierGroup_DefaultCount;
                    menCollected = r.ReadByte();
                    break;

                case ConscriptActiveStatus.Training:
                    equipmentCollected = DssConst.SoldierGroup_DefaultCount;
                    menCollected = DssConst.SoldierGroup_DefaultCount;
                    countdown.readGameState(r);
                    break;
            }


            type = (BuildAndExpandType)r.ReadByte();

            idAndPosition = r.ReadInt32();
            que = r.ReadByte();


            maxTrainingLevel = (TrainingLevel)r.ReadByte();
            //maxTrainingLevel = TrainingLevel.Skillful;

            EightBit bools = EightBit.FromStream(r);
            requireMaxPopulation = bools.Get(0);
            requireMaxFood = bools.Get(1);

            Debug.ReadCheck(r);

            checkSpecialization();
        }
        public bool CountDownQue()
        {
            if (que > 0)
            {
                if (que <= MaxQue)
                {
                    --que;
                }

                return true;
            }

            return false;
        }

        public TimeLength TimeLength()
        {
            return new TimeLength(ConscriptProfile.TrainingTime(inProgress.training, type));
        }

        public string activeStringOf(ConscriptActiveStatus status, int menCount, out bool collected)
        {
            string result = null;
            collected = false;

            switch (status)
            {
                case ConscriptActiveStatus.Idle:
                    result = DssRef.lang.Hud_Idle;
                    break;

                case ConscriptActiveStatus.CollectingEquipment:
                    {
                        collected = equipmentCollected >= menCount;
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, equipmentCollected, menCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingEquipment, progress);
                    }
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    {
                        collected = menCollected >= menCount;
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, menCollected, menCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingMen, progress);
                    }
                    break;
            }

            return result;
        }

        public string shortActiveString()
        {
            string result = null;
            if (active == ConscriptActiveStatus.Training)
            {
                result = string.Format(DssRef.lang.Conscription_Status_Training, countdown.RemainingLength().ShortString());
            }
            else
            {
                int menCostProgress = menNeeded;
                result = activeStringOf(active, menCostProgress, out _) + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_ProductionQueue, que <= MaxQue ? que.ToString() : DssRef.lang.Hud_NoLimit);
            }

            return result;
        }

        public string longTimeProgress()
        {
            string remaining;
            if (active == ConscriptActiveStatus.Training)
            {
                remaining = countdown.RemainingLength().LongString();
            }
            else
            {
                remaining = TimeLength().LongString();
            }
            return string.Format(DssRef.lang.Conscription_Status_Training, remaining);
        }

        public void tooltip(LocalPlayer player, City city, RichBoxContent content)
        {
            ItemResourceType weaponItem = profile.weapon;
            bool hasWeapons = city.GetGroupedResource(weaponItem).amount >= DssConst.SoldierGroup_DefaultCount;

            bool hasArmor = true;
            ItemResourceType armorItem = profile.armorLevel;
            if (profile.armorLevel != ItemResourceType.NONE)
            {
                //armorItem = ConscriptProfile.ArmorItem(profile.armorLevel);
                hasArmor = city.GetGroupedResource(armorItem).amount >= DssConst.SoldierGroup_DefaultCount;
            }

            bool hasMen = city.workForce.amount >= DssConst.SoldierGroup_DefaultCount;

            bool available = hasWeapons && hasArmor && hasMen;

            content.Add(new RbImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
            content.space(0.5f);
            SpriteName icon;
            if (profile.specialization == SpecializationType.CityGuard)
            {
                icon = SpriteName.WarsGuard;
            }
            else
            {
                icon = new SoldierConscriptProfile() { conscript = profile }.Icon();
            }

            content.Add(new RbImage(icon));
            content.hspace();
            //ItemResourceType weaponitem = ConscriptProfile.WeaponItem(profile.weapon);
            IconName.Item(weaponItem, out var weaponIcon, out var weaponName);
            

            content.Add(new RbImage(weaponIcon));

            if (profile.armorLevel != ItemResourceType.NONE)
            {
                IconName.Item(armorItem, out var armorIcon, out var armorName);
                //ItemResourceType armoritem = ConscriptProfile.ArmorItem(profile.armorLevel);
                content.Add(new RbImage(armorIcon));
            }
            content.Add(new RbImage((SpriteName)((int)SpriteName.WarsUnitLevelMinimal + (int)profile.training)));

            content.newLine();
            player.gameControls.input.StopStart.ToRichContent(content);
            content.space(0.5f);
            content.Add(new RbText(shortActiveString()));

            content.newLine();
            player.gameControls.input.Copy.ToRichContent(content);
            //content.Add(new RbImage(player.gameControls.input.Copy.Icon));
            content.space(0.5f);
            content.Add(new RbText(DssRef.lang.Hud_CopySetup));
            content.space(2);
            player.gameControls.input.Paste.ToRichContent(content);
            //content.Add(new RbImage(player.gameControls.input.Paste.Icon));
            content.space(0.5f);
            content.Add(new RbText(DssRef.lang.Hud_Paste));

            content.Add(new RbSeperationLine());

            ConscriptMenu.resourcesToMenu(content, city, this);

        }
    }
}

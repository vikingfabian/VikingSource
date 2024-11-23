using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Data;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.DSSWars.Conscript
{
    struct BarracksStatus
    {
        public const int MaxQue = 5;

        public ConscriptActiveStatus active;
        public ConscriptProfile profile;

        public ConscriptProfile inProgress;
        public TimeInGameCountdown countdown;
        public BarracksType type;
        public int menCollected;
        public int equipmentCollected;

        public int idAndPosition;
        public int que;

        public BarracksStatus(BarracksType type)
            : this()
        {
            this.type = type;

            switch (type)
            {
                case BarracksType.Soldier:
                    profile.weapon = ItemResourceType.SharpStick;
                    break;
                case BarracksType.Archer:
                    profile.weapon = ItemResourceType.SlingShot;
                    break;
                case BarracksType.Warmashine:
                    profile.weapon = ItemResourceType.Ballista;
                    break;
                case BarracksType.Knight:
                    profile.weapon = ItemResourceType.Warhammer;
                    break;
                case BarracksType.Gun:
                    profile.weapon = ItemResourceType.HandCannon;
                    break;
            }
        }

        public void halt(City city)
        {
            que = 0;

            returnItems(city);

        }

        public void returnItems(City city)
        {
            if (active == ConscriptActiveStatus.CollectingEquipment ||
                    active == ConscriptActiveStatus.CollectingMen)
            {
                //return items
                ItemResourceType weaponItem = inProgress.weapon;
                ItemResourceType armorItem =inProgress.armorLevel;

                city.AddGroupedResource(weaponItem, equipmentCollected);

                if (inProgress.armorLevel != ItemResourceType.NONE)
                {
                    city.AddGroupedResource(armorItem, equipmentCollected);
                }

                city.workForce.amount += menCollected;

                active = ConscriptActiveStatus.Idle;

                //city.conscriptBuildings[selectedConscript] = status;
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
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
            w.Write((byte)que);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            active = (ConscriptActiveStatus)r.ReadByte();
            profile.readGameState(r);
            if (active != ConscriptActiveStatus.Idle)
            {
                inProgress.readGameState(r);
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
            if (subVersion >= 13 && subVersion < 40)
            {
                bool nobelmen = r.ReadBoolean();
            }

            if (subVersion >= 40)
            {
                type = (BarracksType)r.ReadByte();
            }
            idAndPosition = r.ReadInt32();
            que = r.ReadByte();
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

        public string activeStringOf(ConscriptActiveStatus status)
        {
            string result = null;


            switch (status)
            {
                case ConscriptActiveStatus.Idle:
                    result = DssRef.lang.Hud_Idle;
                    break;

                case ConscriptActiveStatus.CollectingEquipment:
                    {
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, equipmentCollected, DssConst.SoldierGroup_DefaultCount);
                        result = string.Format(DssRef.lang.Conscription_Status_CollectingEquipment, progress);
                    }
                    break;

                case ConscriptActiveStatus.CollectingMen:
                    {
                        var progress = string.Format(DssRef.lang.Language_CollectProgress, menCollected, DssConst.SoldierGroup_DefaultCount);
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
                result = activeStringOf(active) + ", " + string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_Queue, que <= MaxQue ? que.ToString() : DssRef.lang.Hud_NoLimit);
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

            content.Add(new RichBoxImage(available ? SpriteName.warsResourceChunkAvailable : SpriteName.warsResourceChunkNotAvailable));
            content.space(0.5f);
            content.Add(new RichBoxImage(
                            new SoldierConscriptProfile() { conscript = profile }.Icon()
                            ));
            //ItemResourceType weaponitem = ConscriptProfile.WeaponItem(profile.weapon);
            content.Add(new RichBoxImage(ResourceLib.Icon(weaponItem)));

            if (profile.armorLevel != ItemResourceType.NONE)
            {
                //ItemResourceType armoritem = ConscriptProfile.ArmorItem(profile.armorLevel);
                content.Add(new RichBoxImage(ResourceLib.Icon(armorItem)));
            }
            content.Add(new RichBoxImage((SpriteName)((int)SpriteName.WarsUnitLevelMinimal + (int)profile.training)));

            content.newLine();
            content.Add(new RichBoxImage(player.input.Stop.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(shortActiveString()));

            content.newLine();
            content.Add(new RichBoxImage(player.input.Copy.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(DssRef.lang.Hud_CopySetup));
            content.space(2);
            content.Add(new RichBoxImage(player.input.Paste.Icon));
            content.space(0.5f);
            content.Add(new RichBoxText(DssRef.lang.Hud_Paste));
        }
    }

    struct SoldierConscriptProfile
    {
        public ConscriptProfile conscript;
        public float skillBonus;

        public SoldierConscriptProfile()
      
        {
            conscript = new ConscriptProfile();
            conscript.weapon = ItemResourceType.SharpStick;
            skillBonus = 0;
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            conscript.writeGameState(w);
            SaveLib.WriteFloatMultiplier(skillBonus, w);
        }
        public void readGameState(System.IO.BinaryReader r)
        {
            conscript.readGameState(r);
            skillBonus = SaveLib.ReadFloatMultiplier(r);
        }

        public UnitType unitType()
        {
            if (conscript.specialization == SpecializationType.DarkLord)
            {
                return UnitType.DarkLord;
            }
            switch (conscript.weapon)
            {
                case ItemResourceType.Ballista:
                    return UnitType.ConscriptWarmashine;
                case ItemResourceType.KnightsLance:
                    return UnitType.ConscriptCavalry;

                default:
                    return UnitType.Conscript;
            }
        }

        public UnitFilterType filterType()
        {
            switch (conscript.specialization)
            {
                default:
                    switch (conscript.weapon)
                    {
                        case ItemResourceType.SharpStick:
                            return UnitFilterType.SharpStick;

                        case ItemResourceType.BronzeSword:
                        case ItemResourceType.ShortSword:
                        case ItemResourceType.Sword:
                            return UnitFilterType.Sword;
                        case ItemResourceType.Pike:
                            return UnitFilterType.Pike;

                        case ItemResourceType.Warhammer:
                            return UnitFilterType.Warhammer;
                        case ItemResourceType.TwoHandSword:
                            return UnitFilterType.TwohandSword;
                        case ItemResourceType.KnightsLance:
                            return UnitFilterType.Knight;
                        case ItemResourceType.MithrilSword:
                            return UnitFilterType.MithrilKnight;

                        case ItemResourceType.SlingShot:
                            return UnitFilterType.Slingshot;
                        case ItemResourceType.ThrowingSpear:
                            return UnitFilterType.Throwingspear;
                        case ItemResourceType.Bow:
                        case ItemResourceType.LongBow:
                            return UnitFilterType.Bow;

                        case ItemResourceType.Crossbow:
                            return UnitFilterType.CrossBow;
                        case ItemResourceType.MithrilBow:
                            return UnitFilterType.MithrilBow;

                        case ItemResourceType.HandCannon:
                        case ItemResourceType.Rifle:
                            return UnitFilterType.Rifle;
                        case ItemResourceType.HandCulverin:
                        case ItemResourceType.Blunderbus:
                            return UnitFilterType.Shotgun;

                        case ItemResourceType.Ballista:
                            return UnitFilterType.Ballista;
                        case ItemResourceType.Manuballista:
                            return UnitFilterType.ManuBallista;
                        case ItemResourceType.SiegeCannonBronze:
                            return UnitFilterType.SiegeCannonBronze;
                        case ItemResourceType.ManCannonBronze:
                            return UnitFilterType.ManCannonBronze;
                        case ItemResourceType.SiegeCannonIron:
                            return UnitFilterType.SiegeCannonIron;
                        case ItemResourceType.ManCannonIron:
                            return UnitFilterType.ManCannonIron;

                        default:
                            throw new NotImplementedException();

                    }

                case SpecializationType.Green:
                    return UnitFilterType.GreenSoldier;
                case SpecializationType.HonorGuard:
                    return UnitFilterType.HonourGuard;
                case SpecializationType.Viking:
                    return UnitFilterType.Viking;
                case SpecializationType.DarkLord:
                    return UnitFilterType.DarkLord;
            }
        }

        public SpriteName Icon()
        {
            return init(DssRef.profile.bannerman).icon;
        }

        public SoldierData init(AbsSoldierProfile profile)
        {
            if (skillBonus <= 0)
            {
                skillBonus = 1;
            }

            SoldierData soldierData = profile.data;

            soldierData.basehealth = ConscriptProfile.ArmorHealth(conscript.armorLevel);
            soldierData.attackDamage = Convert.ToInt32(ConscriptProfile.WeaponDamage(conscript.weapon) * skillBonus);
            soldierData.attackDamageStructure = soldierData.attackDamage;
            soldierData.attackDamageSea = soldierData.attackDamage;

            soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            switch (conscript.weapon)
            {
                case ItemResourceType.SharpStick:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.03f;
                    soldierData.modelName = LootFest.VoxelModelName.war_folkman;
                    soldierData.icon = SpriteName.WarsUnitIcon_Folkman;
                    break;

                case ItemResourceType.BronzeSword:
                case ItemResourceType.ShortSword:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.03f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_soldier;
                    soldierData.modelVariationCount = 3;
                    soldierData.icon = SpriteName.WarsUnitIcon_Soldier;
                    break;
                case ItemResourceType.Sword:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.04f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_soldier;
                    soldierData.modelVariationCount = 3;
                    soldierData.icon = SpriteName.WarsUnitIcon_Soldier;
                    break;

                case ItemResourceType.LongSword:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.05f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_longsword;
                    soldierData.icon = SpriteName.WarsUnitIcon_Longsword;
                    break;

                case ItemResourceType.Pike:
                    soldierData.arrowWeakness = true;
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.055f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_piker;
                    soldierData.modelVariationCount = 1;
                    soldierData.modelScale *= 1.6f;
                    soldierData.icon = SpriteName.WarsUnitIcon_Pikeman;
                    conscript.specialization = SpecializationType.AntiCavalry;
                    break;

                case ItemResourceType.Warhammer:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.04f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_hammer;
                    soldierData.icon = SpriteName.WarsResource_Warhammer;
                    break;

                case ItemResourceType.TwoHandSword:
                    soldierData.arrowWeakness = true;
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.08f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_twohand;
                    soldierData.modelVariationCount = 1;
                    soldierData.modelScale *= 1.6f;
                    soldierData.icon = SpriteName.WarsUnitIcon_TwoHand;
                    break;

                case ItemResourceType.KnightsLance:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2f;
                    soldierData.attackRange = 0.06f;
                    soldierData.basehealth *= 3;
                    soldierData.mainAttack = AttackType.Melee;
                    //result.attackDamage = 120;
                    soldierData.attackDamageStructure = Convert.ToInt32(30 * skillBonus);
                    soldierData.attackDamageSea = Convert.ToInt32(20 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;
                    soldierData.modelName = LootFest.VoxelModelName.war_knight;
                    soldierData.modelVariationCount = 3;
                    soldierData.modelScale *= 1.5f;
                    soldierData.icon = SpriteName.WarsUnitIcon_Knight;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 3;
                    //soldierData.ArmySpeedBonusLand = 0.8;
                    break;

                case ItemResourceType.MithrilSword:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.055f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_mithrilman;
                    soldierData.icon = SpriteName.WarsUnitIcon_MithrilMan;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;
                    break;

                case ItemResourceType.SlingShot:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.4f;
                    soldierData.mainAttack = AttackType.SlingShot;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.8f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_slingman;
                    soldierData.icon = SpriteName.WarsUnitIcon_Slingshot;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;

                case ItemResourceType.ThrowingSpear:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.3f;
                    soldierData.mainAttack = AttackType.Javelin;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = .5f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_javelin;
                    soldierData.icon = SpriteName.WarsUnitIcon_Javelin;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 6f;
                    break;

                case ItemResourceType.Bow:
                    soldierData.mainAttack = AttackType.Arrow;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.3f;
                    soldierData.modelName = LootFest.VoxelModelName.war_archer;
                    soldierData.modelVariationCount = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Archer;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;

                case ItemResourceType.LongBow:
                    soldierData.mainAttack = AttackType.Arrow;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.7f;
                    soldierData.modelName = LootFest.VoxelModelName.war_archer;
                    soldierData.modelVariationCount = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Archer;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
                    break;

                case ItemResourceType.Crossbow:
                    soldierData.mainAttack = AttackType.Bolt;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.7f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_crossbow;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.LittleUnitIconCrossBowman;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 15f;
                    break;

                case ItemResourceType.MithrilBow:
                    soldierData.mainAttack = AttackType.Arrow;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 2.5f;
                    soldierData.modelName = LootFest.VoxelModelName.war_archer;
                    soldierData.modelVariationCount = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Archer;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 8f;
                    break;


                case ItemResourceType.HandCannon:
                    soldierData.mainAttack = AttackType.GunShot;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.2f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_handcannon;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;
                    break;

                case ItemResourceType.HandCulverin:
                    soldierData.mainAttack = AttackType.GunBlast;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 0.4f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_culvertin;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;
                    break;

                case ItemResourceType.Rifle:
                    soldierData.mainAttack = AttackType.GunShot;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 1.5f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_handcannon;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;
                    break;

                case ItemResourceType.Blunderbus:
                    soldierData.mainAttack = AttackType.GunBlast;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;
                    soldierData.attackRange = 0.5f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_culvertin;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;
                    break;

                case ItemResourceType.Ballista:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = WarmashineProfile.BallistaRange;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Ballista;
                    soldierData.attackDamageStructure = Convert.ToInt32(1500 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

                    soldierData.modelName = LootFest.VoxelModelName.war_ballista;
                    soldierData.modelVariationCount = 2;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsUnitIcon_Ballista;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;
                    break;

                case ItemResourceType.Manuballista:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Ballista;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_manuballista;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;

                    soldierData.icon = SpriteName.WarsResource_Manuballista;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;
                    break;

                case ItemResourceType.SiegeCannonBronze:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.3f;
                    soldierData.attackRange = WarmashineProfile.BallistaRange;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Cannonball;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 50f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_bronzesiegecannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsResource_BronzeSiegeCannon;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 4;
                    break;

                case ItemResourceType.ManCannonBronze:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Cannonball;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 20f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_bronzemancannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;

                    soldierData.icon = SpriteName.WarsResource_BronzeManCannon;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;
                    break;

                case ItemResourceType.SiegeCannonIron:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2.5f;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Cannonball;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 20f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_ironsiegecannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsResource_IronSiegeCannon;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 4;
                    break;

                case ItemResourceType.ManCannonIron:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2.4f;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Cannonball;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 18;

                    soldierData.modelName = LootFest.VoxelModelName.wars_ironmancannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;

                    soldierData.icon = SpriteName.WarsUnitIcon_IronManCannon;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;
                    break;
            }

            switch (conscript.specialization)
            {
                case SpecializationType.Field:
                    soldierData.attackDamage = MathExt.AddPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageSea = MathExt.SubtractPercentage(soldierData.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.Viking:
                case SpecializationType.Sea:
                    soldierData.attackDamage = MathExt.SubtractPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    float seaDamagePerc = conscript.specialization == SpecializationType.Sea ?
                        DssConst.Conscript_SpecializePercentage : DssConst.Conscript_SpecializePercentage * 3f;
                    soldierData.attackDamageSea = MathExt.AddPercentage(soldierData.attackDamageSea, seaDamagePerc);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);

                    if (!conscript.RangedUnit())
                    {
                        soldierData.modelName = LootFest.VoxelModelName.war_sailor;
                        soldierData.modelVariationCount = 2;
                        soldierData.icon = SpriteName.WarsUnitIcon_Viking;
                    }
                    break;

                case SpecializationType.Siege:
                    soldierData.attackDamage = MathExt.SubtractPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageSea = MathExt.SubtractPercentage(soldierData.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageStructure = MathExt.AddPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.HonorGuard:
                    soldierData.modelScale = DssConst.Men_StandardModelScale * 1.2f;
                    soldierData.energyPerSoldier = 0;
                    soldierData.modelName = LootFest.VoxelModelName.little_hirdman;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_Honorguard;
                    break;

                case SpecializationType.Traditional:
                    soldierData.energyPerSoldier *= 0.5f;
                    break;

                case SpecializationType.Green:
                    soldierData.secondaryAttack = AttackType.Arrow;
                    soldierData.secondaryAttackDamage = 100;
                    soldierData.secondaryAttackRange = 1.7f;
                    soldierData.bonusProjectiles = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Greensoldier;
                    break;

                case SpecializationType.DarkLord:
                    soldierData.modelScale = DssConst.Men_StandardModelScale;
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;
                    soldierData.basehealth = DssConst.Soldier_DefaultHealth * 4;
                    soldierData.modelName = LootFest.VoxelModelName.wars_darklord;
                    break;
            }

            soldierData.attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(conscript.training);
            soldierData.attackTimePlusCoolDown /= 1f + skillBonus;


            return soldierData;
        }

        public SoldierData bannermanSetup(SoldierData soldierData)
        {
            soldierData.modelScale = DssConst.Men_StandardModelScale;
            soldierData.canAttackCharacters = false;
            soldierData.canAttackStructure = false;

            soldierData.modelName = LootFest.VoxelModelName.war_bannerman;
            soldierData.modelVariationCount = 1;

            return soldierData;
        }

        public void shipSetup(ref SoldierData soldierData)
        {
            soldierData.modelName = LootFest.VoxelModelName.NUM_NON;

            soldierData.walkingSpeed = DssConst.Men_StandardShipSpeed;

            soldierData.modelScale = DssConst.Men_StandardModelScale * 6f;

            switch (conscript.specialization)
            {
                case SpecializationType.Viking:
                    if (!conscript.RangedUnit())
                    {
                        soldierData.modelName = LootFest.VoxelModelName.wars_viking_ship;

                        soldierData.mainAttack = AttackType.Javelin;
                        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                        soldierData.attackRange = 1f;
                    }
                    soldierData.walkingSpeed *= 1.5f;
                    break;

                case SpecializationType.DarkLord:
                    soldierData.modelName = LootFest.VoxelModelName.wars_knight_ship;

                    soldierData.mainAttack = AttackType.Javelin;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                    soldierData.attackRange = 2f;

                    soldierData.attackDamage = 500;
                    soldierData.attackDamageStructure = soldierData.attackDamage;
                    soldierData.attackDamageSea = soldierData.attackDamage;

                    soldierData.walkingSpeed *= 1.5f;
                    break;
            }

            if (soldierData.modelName == LootFest.VoxelModelName.NUM_NON)
            {
                switch (conscript.weapon)
                {
                    case ItemResourceType.SharpStick:
                        soldierData.modelName = LootFest.VoxelModelName.wars_folk_ship;

                        break;
                    case ItemResourceType.Pike:
                    case ItemResourceType.Sword:
                        soldierData.modelName = LootFest.VoxelModelName.wars_soldier_ship;
                        break;

                    case ItemResourceType.Crossbow:
                    case ItemResourceType.LongBow:
                    case ItemResourceType.Bow:
                        soldierData.modelName = LootFest.VoxelModelName.wars_archer_ship;
                        break;

                    case ItemResourceType.Ballista:
                        soldierData.modelName = LootFest.VoxelModelName.wars_ballista_ship;
                        break;

                    case ItemResourceType.TwoHandSword:
                    case ItemResourceType.KnightsLance:
                        soldierData.modelName = LootFest.VoxelModelName.wars_knight_ship;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }

    struct ConscriptProfile
    {
        public ItemResourceType weapon;
        public ItemResourceType armorLevel;
        public TrainingLevel training;
        public SpecializationType specialization;

        public ConscriptProfile()
        {
            weapon = ItemResourceType.SharpStick;
            armorLevel = ItemResourceType.NONE;

            training = 0;
            specialization = SpecializationType.None;
        }

        public bool RangedUnit()
        {
            return weapon == ItemResourceType.Bow || weapon == ItemResourceType.Crossbow || weapon == ItemResourceType.Ballista;
        }

        public bool RangedManUnit()
        {
            return weapon == ItemResourceType.Bow || weapon == ItemResourceType.Crossbow;    
        }

        public bool MeleeSoldier()
        {
            return weapon == ItemResourceType.SharpStick || weapon == ItemResourceType.Sword || weapon == ItemResourceType.TwoHandSword;
        }

        public bool KnightUnit()
        {
            return weapon == ItemResourceType.TwoHandSword || weapon == ItemResourceType.KnightsLance;
        }

        public bool Warmashine()
        {
            return weapon == ItemResourceType.Ballista;
        }

        public double armySpeedBonus(bool land)
        {
            if (land)
            {
                switch (weapon)
                {
                    case ItemResourceType.KnightsLance:
                        return 0.8;
                    case ItemResourceType.Ballista:
                    case ItemResourceType.Catapult:
                        return -0.5;
                }
            }
            else
            {
                if (specialization == SpecializationType.Sea)
                    return 0.4;
                else if (specialization == SpecializationType.Viking)
                    return 0.6;
            }

            return 0;
        }

        public void defaultSetup(BarracksType type)
        {
            switch (type)
            {
                case BarracksType.Soldier:
                    weapon = ItemResourceType.SharpStick;
                    break;
                case BarracksType.Archer:
                    weapon = ItemResourceType.SlingShot;
                    break;
                case BarracksType.Warmashine:
                    weapon = ItemResourceType.Ballista;
                    break;
                case BarracksType.Knight:
                    weapon = ItemResourceType.Warhammer;
                    training = TrainingLevel.Basic;
                    break;
                case BarracksType.Gun:
                    weapon = ItemResourceType.HandCannon;
                    break;
                case BarracksType.Cannon:
                    weapon = ItemResourceType.ManCannonBronze;
                    break;
            }

        }

        public string TypeName()
        {
            switch (specialization)
            {
                case SpecializationType.HonorGuard:
                    return DssRef.lang.UnitType_HonorGuard;
                case SpecializationType.Viking:
                    return DssRef.lang.UnitType_Viking;
                case SpecializationType.Green:
                    return DssRef.lang.UnitType_GreenSoldier;
                case SpecializationType.DarkLord:
                    return DssRef.lang.UnitType_DarkLord;

                default:
                    switch (weapon)
                    {
                        case ItemResourceType.Bow:
                        case ItemResourceType.LongBow:
                            return DssRef.lang.UnitType_Archer;
                        case ItemResourceType.Crossbow:
                            return DssRef.lang.UnitType_Crossbow;
                        case ItemResourceType.Ballista:
                            return DssRef.lang.UnitType_Ballista;
                        case ItemResourceType.SharpStick:
                            return DssRef.lang.UnitType_Folkman;
                        case ItemResourceType.Pike:
                            return DssRef.lang.UnitType_Pikeman;
                        case ItemResourceType.Sword:
                            return DssRef.lang.UnitType_Soldier;
                        case ItemResourceType.KnightsLance:
                            return DssRef.lang.UnitType_CavalryKnight;
                        case ItemResourceType.TwoHandSword:
                            return DssRef.lang.UnitType_FootKnight;


                        default:
                            return TextLib.Error;
                    }
            }
        }

        public SpecializationType[] avaialableSpecializations()
        {
            SpecializationType[] specializationTypes;
            if (weapon == ItemResourceType.TwoHandSword)
            {
                specializationTypes = new SpecializationType[] { SpecializationType.AntiCavalry };
            }
            else if (weapon == ItemResourceType.Ballista)
            {
                specializationTypes = new SpecializationType[] { SpecializationType.Siege };
            }
            else
            {
                specializationTypes = new SpecializationType[]
                    {
                            SpecializationType.None,
                            SpecializationType.Field,
                            SpecializationType.Sea,
                            SpecializationType.Siege,
                    };
            }

            return specializationTypes;
        }

        public void toHud(RichBoxContent content)
        {
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_WeaponTitle, LangLib.Item(weapon)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_ArmorTitle, LangLib.Item(armorLevel)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_TrainingTitle, LangLib.Training(training)));
            content.text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Conscript_SpecializationTitle, LangLib.SpecializationTypeName(specialization)));
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((byte)weapon);
            w.Write((byte)armorLevel);
            w.Write((byte)training);
            w.Write((byte)specialization);
        }

        public void readGameState(System.IO.BinaryReader r)
        {
            weapon = (ItemResourceType)r.ReadByte();
            armorLevel = (ItemResourceType)r.ReadByte();
            training = (TrainingLevel)r.ReadByte();
            specialization = (SpecializationType)r.ReadByte();
        }

        //make these static
        public static int WeaponDamage(ItemResourceType weapon)
        {
            switch (weapon)
            {
                case ItemResourceType.SharpStick: return DssConst.WeaponDamage_SharpStick;
                case ItemResourceType.BronzeSword: return DssConst.WeaponDamage_BronzeSword;
                case ItemResourceType.ShortSword: return DssConst.WeaponDamage_ShortSword;
                case ItemResourceType.Sword: return DssConst.WeaponDamage_Sword;
                case ItemResourceType.LongSword: return DssConst.WeaponDamage_LongSword;
                case ItemResourceType.Pike: return DssConst.WeaponDamage_Pike;

                case ItemResourceType.Warhammer: return DssConst.WeaponDamage_Warhammer;
                case ItemResourceType.TwoHandSword: return DssConst.WeaponDamage_TwoHandSword;
                case ItemResourceType.KnightsLance: return DssConst.WeaponDamage_KnigtsLance;
                case ItemResourceType.SlingShot: return DssConst.WeaponDamage_Slingshot;
                case ItemResourceType.ThrowingSpear: return DssConst.WeaponDamage_Throwingspear;
                case ItemResourceType.Bow: return DssConst.WeaponDamage_Bow;
                case ItemResourceType.LongBow: return DssConst.WeaponDamage_Longbow;
                case ItemResourceType.Crossbow: return DssConst.WeaponDamage_CrossBow;
                case ItemResourceType.MithrilBow: return DssConst.WeaponDamage_MithrilBow;

                case ItemResourceType.HandCannon: return DssConst.WeaponDamage_Handcannon;
                case ItemResourceType.HandCulverin: return DssConst.WeaponDamage_Handculvetin;
                case ItemResourceType.Rifle: return DssConst.WeaponDamage_Rifle;
                case ItemResourceType.Blunderbus: return DssConst.WeaponDamage_Blunderbus;

                case ItemResourceType.Ballista: return DssConst.WeaponDamage_Ballista;
                case ItemResourceType.Manuballista: return DssConst.WeaponDamage_ManuBallista;
                case ItemResourceType.Catapult: return DssConst.WeaponDamage_Catapult;

                case ItemResourceType.SiegeCannonBronze: return DssConst.WeaponDamage_SiegeCannonBronze;
                case ItemResourceType.ManCannonBronze: return DssConst.WeaponDamage_ManCannonBronze;
                case ItemResourceType.SiegeCannonIron: return DssConst.WeaponDamage_SiegeCannonIron;
                case ItemResourceType.ManCannonIron: return DssConst.WeaponDamage_ManCannonIron;

                default: throw new NotImplementedException();
            }
        }

        //public static Resource.ItemResourceType WeaponItem(ItemResourceType weapon)
        //{
        //    switch (weapon)
        //    {
        //        case ItemResourceType.SharpStick: return Resource.ItemResourceType.SharpStick;
        //        case ItemResourceType.Sword: return Resource.ItemResourceType.Sword;
        //        case ItemResourceType.TwoHandSword: return Resource.ItemResourceType.TwoHandSword;
        //        case ItemResourceType.KnightsLance: return Resource.ItemResourceType.KnightsLance;
        //        case ItemResourceType.Bow: return Resource.ItemResourceType.Bow;
        //        case ItemResourceType.LongBow: return Resource.ItemResourceType.LongBow;
        //        case ItemResourceType.Ballista: return Resource.ItemResourceType.Ballista;

        //        default: throw new NotImplementedException();
        //    }
        //}

        public static int ArmorHealth(ItemResourceType armorLevel)
        {
            switch (armorLevel)
            {
                case ItemResourceType.NONE: return DssConst.ArmorHealth_None;
                case ItemResourceType.PaddedArmor: return DssConst.ArmorHealth_Padded;
                case ItemResourceType.HeavyPaddedArmor: return DssConst.ArmorHealth_HeavyPadded;
                case ItemResourceType.BronzeArmor: return DssConst.ArmorHealth_Bronze;
                case ItemResourceType.IronArmor: return DssConst.ArmorHealth_Mail;
                case ItemResourceType.HeavyIronArmor: return DssConst.ArmorHealth_HeavyMail;
                case ItemResourceType.LightPlateArmor: return DssConst.ArmorHealth_Plate;
                case ItemResourceType.FullPlateArmor: return DssConst.ArmorHealth_FullPlate;
                case ItemResourceType.MithrilArmor: return DssConst.ArmorHealth_Mithril;
                default: throw new NotImplementedException();
            }
        }

        //public static Resource.ItemResourceType ArmorItem(ItemResourceType armorLevel)
        //{
        //    switch (armorLevel)
        //    {
        //        case ItemResourceType.None: return Resource.ItemResourceType.NONE;
        //        case ItemResourceType.PaddedArmor: return Resource.ItemResourceType.PaddedArmor;
        //        case ItemResourceType.Mail: return Resource.ItemResourceType.IronArmor;
        //        case ItemResourceType.FullPlate: return Resource.ItemResourceType.HeavyIronArmor;
        //        default: throw new NotImplementedException();
        //    }
        //}

        public static float TrainingAttackSpeed(TrainingLevel training)
        {
            switch (training)
            {
                case TrainingLevel.Minimal: return DssConst.TrainingAttackSpeed_Minimal;
                case TrainingLevel.Basic: return DssConst.TrainingAttackSpeed_Basic;
                case TrainingLevel.Skillful: return DssConst.TrainingAttackSpeed_Skillful;
                case TrainingLevel.Professional: return DssConst.TrainingAttackSpeed_Professional;
                default: throw new NotImplementedException();
            }
        }

        public static float TrainingTime(TrainingLevel training, BarracksType type)
        {
            float result;
            switch (training)
            {
                case TrainingLevel.Minimal:
                    result = DssConst.TrainingTimeSec_Minimal;
                    break;
                case TrainingLevel.Basic:
                    result = DssConst.TrainingTimeSec_Basic;
                    break;
                case TrainingLevel.Skillful:
                    result = DssConst.TrainingTimeSec_Skillful;
                    break;
                case TrainingLevel.Professional:
                    result = DssConst.TrainingTimeSec_Professional;
                    break;

                default: throw new NotImplementedException();
            }

            switch (type)
            { 
                case BarracksType.Knight:
                    result += DssConst.TrainingTimeSec_NobelmenAdd;
                    break;
                case BarracksType.Gun:
                case BarracksType.Cannon:
                    result /= 2;
                    break;
            }
            
            return result;
        }
    }

    //enum ArmorLevel
    //{
    //    None,
    //    PaddedArmor,
    //    HeavyPaddedArmor,
    //    Mail,
    //    HeavyMail,
    //    Plate,
    //    FullPlate,
    //    Mithril,
    //    NUM
    //}

    //enum MainWeapon
    //{
    //    SharpStick,
    //    Sword,
    //    Pike,
    //    Bow,
    //    CrossBow,
    //    TwoHandSword,
    //    KnightsLance,
    //    Ballista,
    //    Longbow,
    //    NUM
    //}

    enum TrainingLevel
    {
        Minimal,
        Basic,
        Skillful,
        Professional,
        NUM
    }

    enum SpecializationType
    {
        None,
        Field,
        Sea,
        Siege,
        NUM,
        Traditional,
        Viking,
        HonorGuard,
        Green,
        AntiCavalry,
        DarkLord,
    }

    enum ConscriptActiveStatus
    {
        Idle,
        CollectingEquipment,
        CollectingMen,
        Training,
    }

    enum BarracksType
    { 
        Soldier,
        Archer,
        Warmashine,
        Knight,
        Gun,
        Cannon,
    }
}

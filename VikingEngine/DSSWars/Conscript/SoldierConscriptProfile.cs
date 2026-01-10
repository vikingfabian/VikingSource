using Microsoft.Xna.Framework;
using System;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars.Conscript
{

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
            StreamLib.WriteFloatMultiplier(skillBonus, w);
        }
        public void readGameState(System.IO.BinaryReader r)
        {
            conscript.readGameState(r);
            skillBonus = StreamLib.ReadFloatMultiplier(r);
        }

        public UnitType unitType()
        {
            switch (conscript.specialization)
            {
                default:
                    
                    switch (conscript.weapon)
                    {
                        case ItemResourceType.Ballista:
                        case ItemResourceType.Manuballista:
                        case ItemResourceType.Catapult:
                        case ItemResourceType.UN_BatteringRam:
                        case ItemResourceType.SiegeCannonBronze:
                        case ItemResourceType.ManCannonBronze:
                        case ItemResourceType.SiegeCannonIron:
                        case ItemResourceType.ManCannonIron:
                            return UnitType.ConscriptWarmachine;
                        //case ItemResourceType.KnightsLance:
                        //    return UnitType.ConscriptCavalry;

                        default:
                            return UnitType.Conscript;
                    }
                    
                case SpecializationType.CityGuard:
                    return UnitType.CityGuard;
                case SpecializationType.DarkLord:
                    return UnitType.DarkLord;

            }
            
        }

        public UnitFilterType filterType()
        {
            switch (conscript.specialization)
            {
                default:
                    switch (conscript.weapon)
                    {
                        case ItemResourceType.Settler:
                            return UnitFilterType.Settler;
                        case ItemResourceType.SharpStick:
                            return UnitFilterType.SharpStick;

                        case ItemResourceType.BronzeSword:
                        case ItemResourceType.ShortSword:
                        case ItemResourceType.Sword:
                            return UnitFilterType.Sword;
                        case ItemResourceType.LongSword:
                            return UnitFilterType.LongSword;

                        case ItemResourceType.Pike:
                            return UnitFilterType.Pike;
                        case ItemResourceType.HandSpear:
                            return UnitFilterType.SpearAndShield;

                        case ItemResourceType.Warhammer:
                            return UnitFilterType.Warhammer;
                        case ItemResourceType.TwoHandSword:
                            return UnitFilterType.TwohandSword;
                        //case ItemResourceType.KnightsLance:
                        //    return UnitFilterType.Knight;
                        case ItemResourceType.MithrilSword:
                            return UnitFilterType.MithrilKnight;

                        case ItemResourceType.SlingShot:
                        case ItemResourceType.ThrowingSpear:
                            return UnitFilterType.Skirmisher;
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
                        case ItemResourceType.Blunderbuss:
                            return UnitFilterType.Shotgun;

                        case ItemResourceType.Ballista:
                            return UnitFilterType.Ballista;
                        case ItemResourceType.Manuballista:
                            return UnitFilterType.ManuBallista;
                        case ItemResourceType.Catapult:
                            return UnitFilterType.Catapult;
                        case ItemResourceType.SiegeCannonBronze:
                            return UnitFilterType.SiegeCannonBronze;
                        case ItemResourceType.ManCannonBronze:
                            return UnitFilterType.ManCannonBronze;
                        case ItemResourceType.SiegeCannonIron:
                            return UnitFilterType.SiegeCannonIron;
                        case ItemResourceType.ManCannonIron:
                            return UnitFilterType.ManCannonIron;

                        case ItemResourceType.RoseWarrior_dog:
                            return UnitFilterType.RoseWarrior;
                        case ItemResourceType.RoseWarrior_soldier:
                            return UnitFilterType.RoseWarrior;
                        case ItemResourceType.RoseWarrior_tank:
                            return UnitFilterType.RoseWarrior;


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
            return init().icon;
        }

        public SoldierData init()
        {
            //if (skillBonus <= 0)
            //{
            //    skillBonus = 1;
            //}

            SoldierData soldierData = ItemPropertyColl.Get(conscript.weapon).soldierData;
            soldierData.applySkillBonus(skillBonus);


            //if (profile != null)
            //{
            //    soldierData = profile.data;
            //}
            //else
            //{
            //    soldierData = new SoldierData();
            //}
            var armorData = ItemPropertyColl.Get(conscript.armorLevel).soldierData;
            soldierData.basehealth = armorData.basehealth;//ConscriptProfile.ArmorHealth(conscript.armorLevel);
            soldierData.modelData.armor = armorData.modelData.armor;

            soldierData.modelData.specialization = conscript.specialization;
            //soldierData.attackDamage = Convert.ToInt32(ConscriptProfile.WeaponDamage(conscript.weapon, out soldierData.attackSplashCount) * skillBonus);
            //soldierData.attackDamageStructure = soldierData.attackDamage;
            //soldierData.attackDamageSea = soldierData.attackDamage;

            //soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime;

            //switch (conscript.weapon)
            //{
            //    case ItemResourceType.SharpStick:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.03f;
            //        soldierData.modelName = LootFest.VoxelModelName.war_folkman;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Folkman;
            //        break;

            //    case ItemResourceType.BronzeSword:
            //    case ItemResourceType.ShortSword:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.03f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_soldier;
            //        soldierData.modelVariationCount = 3;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Soldier;
            //        break;
            //    case ItemResourceType.Sword:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = DssConst.SwordAttackRange;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_soldier;
            //        soldierData.modelVariationCount = 3;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Soldier;
            //        break;

            //    case ItemResourceType.LongSword:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.05f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_longsword;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Longsword;
            //        break;

            //    case ItemResourceType.Pike:
            //        soldierData.arrowWeakness = true;
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.055f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_piker;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.modelScale *= 1.6f;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Pikeman;
            //        conscript.specialization = SpecializationType.AntiCavalry;
            //        break;

            //    case ItemResourceType.HandSpear:
            //        soldierData.arrowWeakness = true;
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.05f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_spearman;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.modelScale *= 1.0f;
            //        soldierData.icon = SpriteName.LittleUnitIconSpearman;
            //        soldierData.basehealth += DssConst.WeaponHealthAdd_Handspear;
            //        break;

            //    case ItemResourceType.Warhammer:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.04f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_hammer;
            //        soldierData.modelScale *= 1.14f;
            //        soldierData.icon = SpriteName.WarsResource_Warhammer;

            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        break;

            //    case ItemResourceType.TwoHandSword:
            //        soldierData.arrowWeakness = true;
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.08f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_twohand;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.modelScale *= 1.6f;
            //        soldierData.icon = SpriteName.WarsUnitIcon_TwoHand;
            //        soldierData.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.KnightsLance:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 2.5f;
            //        soldierData.attackRange = 0.06f;
            //        soldierData.basehealth *= 3;
            //        soldierData.mainAttack = AttackType.Melee;
            //        //result.attackDamage = 120;
            //        soldierData.attackDamageStructure = Convert.ToInt32(30 * skillBonus);
            //        soldierData.attackDamageSea = Convert.ToInt32(20 * skillBonus);
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;
            //        soldierData.modelName = LootFest.VoxelModelName.war_knight;
            //        soldierData.modelVariationCount = 3;
            //        soldierData.modelScale *= 1.5f;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Knight;
            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 3;

            //        soldierData.rowWidth = 4;
            //        soldierData.columnsDepth = 3;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 1.4f;
            //        soldierData.workForcePerUnit = 2;
            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.hasBannerMan = false;
            //        soldierData.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
            //        //soldierData.ArmySpeedBonusLand = 0.8;
            //        break;

            //    case ItemResourceType.MithrilSword:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.055f;
            //        soldierData.modelScale *= 1.5f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_mithrilman;
            //        soldierData.icon = SpriteName.WarsUnitIcon_MithrilMan;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.8f;
            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        break;

            //    case ItemResourceType.SlingShot:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.4f;
            //        soldierData.mainAttack = AttackType.SlingShot;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;//ArmyPlacement.Mid;
            //        soldierData.attackRange = 1.8f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_slingman;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Slingshot;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.ThrowingSpear:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 1.3f;
            //        soldierData.mainAttack = AttackType.Javelin;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = .5f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_javelin;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Javelin;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 6f;
            //        break;

            //    case ItemResourceType.Bow:
            //        soldierData.mainAttack = AttackType.Arrow;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 1.3f;
            //        soldierData.modelName = LootFest.VoxelModelName.war_archer;
            //        soldierData.modelVariationCount = 2;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Archer;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.LongBow:
            //        soldierData.mainAttack = AttackType.Arrow;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 1.7f;
            //        soldierData.modelName = LootFest.VoxelModelName.war_archer;
            //        soldierData.modelVariationCount = 2;
            //        soldierData.icon = SpriteName.WarsUnitIcon_Archer;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 10f;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.Crossbow:
            //        soldierData.mainAttack = AttackType.Bolt;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 1.7f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_crossbow;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.icon = SpriteName.LittleUnitIconCrossBowman;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 15f;

            //        soldierData.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.MithrilBow:
            //        soldierData.mainAttack = AttackType.Arrow;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 2.5f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_mithrilarcher;
            //        soldierData.modelScale *= 1.3f;

            //        soldierData.icon = SpriteName.WarsUnitIcon_MithrilArcher;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 8f;

            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;


            //    case ItemResourceType.HandCannon:
            //        soldierData.mainAttack = AttackType.GunShot;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 1.2f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_handcannon;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;

            //        soldierData.blockReducingAttack_Inv = DssConst.SmallBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.HandCulverin:
            //        soldierData.mainAttack = AttackType.GunBlast;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 0.4f;
            //        //soldierData.attackSplashCount = 8;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_culvertin;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.Rifle:
            //        soldierData.mainAttack = AttackType.GunShot;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 1.5f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_handcannon;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;

            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.Blunderbuss:
            //        soldierData.mainAttack = AttackType.GunBlast;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
            //        soldierData.attackRange = 0.5f;
            //        //soldierData.attackSplashCount = 8;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_culvertin;
            //        soldierData.modelVariationCount = 1;
            //        soldierData.icon = SpriteName.WarsUnitIcon_BronzeRifle;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 12f;
            //        soldierData.blocksRefillTimeSec = DssConst.LowBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.Ballista:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //        soldierData.attackRange = WarmashineProfile.BallistaRange;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.Ballista;
            //        //soldierData.attackSplashCount = 1;
            //        soldierData.attackDamageStructure = Convert.ToInt32(1500 * skillBonus);
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 32f;

            //        soldierData.modelName = LootFest.VoxelModelName.war_ballista;
            //        soldierData.modelVariationCount = 2;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;

            //        soldierData.icon = SpriteName.WarsUnitIcon_Ballista;

            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

            //        soldierData.rowWidth = 3;
            //        soldierData.columnsDepth = 2;
            //        soldierData.workForcePerUnit = 2;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
            //        soldierData.hasBannerMan = false;

            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_NoWheels;

            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.Manuballista:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //        soldierData.attackRange = 2;
            //        //soldierData.attackSplashCount = 1;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.Ballista;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 32f;

            //        soldierData.modelName = LootFest.VoxelModelName.wars_manuballista;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 1.5f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;

            //        soldierData.icon = SpriteName.WarsResource_Manuballista;
            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

            //        soldierData.rowWidth = 3;
            //        soldierData.columnsDepth = 2;
            //        soldierData.workForcePerUnit = 2;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_NoWheels;

            //        soldierData.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.Catapult:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //        soldierData.attackRange = 2.6f;
            //        //soldierData.attackSplashCount = 3;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.Catapult;
            //        soldierData.attackDamageStructure = Convert.ToInt32(2000 * skillBonus);
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 32f;

            //        soldierData.modelName = LootFest.VoxelModelName.wars_catapult;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 2.3f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;

            //        soldierData.icon = SpriteName.WarsUnitIcon_Catapult;

            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

            //        soldierData.rowWidth = 2;
            //        soldierData.columnsDepth = 2;
            //        soldierData.workForcePerUnit = 2;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_NoWheels;

            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.SiegeCannonBronze:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.3f;
            //        soldierData.attackRange = 2.4f;
            //        //soldierData.attackSplashCount = 12;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.MassiveCannonball;
            //        soldierData.attackDamageStructure = Convert.ToInt32(2000 * skillBonus);
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 100f;

            //        soldierData.modelName = LootFest.VoxelModelName.wars_bronzesiegecannon;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 5f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;

            //        soldierData.icon = SpriteName.WarsUnitIcon_Catapult;

            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

            //        soldierData.rowWidth = 1;
            //        soldierData.columnsDepth = 1;
            //        soldierData.workForcePerUnit = 6;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 2.4f);
            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_NoWheels;
            //        soldierData.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.ManCannonBronze:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //        soldierData.attackRange = 2;
            //        //soldierData.attackSplashCount = 5;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.Cannonball;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 40f;

            //        soldierData.modelName = LootFest.VoxelModelName.wars_bronzemancannon;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;

            //        soldierData.icon = SpriteName.WarsResource_BronzeManCannon;
            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

            //        soldierData.rowWidth = 3;
            //        soldierData.columnsDepth = 2;
            //        soldierData.workForcePerUnit = 2;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
            //        soldierData.hasBannerMan = false;

            //        soldierData.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 1.1f);
            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_Wheels;
            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.SiegeCannonIron:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //        soldierData.attackRange = 2.2f;
            //        //soldierData.attackSplashCount = 2;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.Haubitz;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 40f;

            //        soldierData.modelName = LootFest.VoxelModelName.wars_ironsiegecannon;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 1f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Second;

            //        soldierData.icon = SpriteName.WarsResource_IronSiegeCannon;

            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 4;

            //        soldierData.rowWidth = 3;
            //        soldierData.columnsDepth = 2;
            //        soldierData.workForcePerUnit = 2;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 0.3f);
            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_NoWheels;
            //        soldierData.blockReducingAttack_Inv = DssConst.HeavyBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.ManCannonIron:
            //        soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
            //        soldierData.attackRange = 2.4f;
            //        //soldierData.attackSplashCount = 6;

            //        soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
            //        soldierData.mainAttack = AttackType.Cannonball;
            //        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 40;

            //        soldierData.modelName = LootFest.VoxelModelName.wars_ironmancannon;

            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 1.7f;
            //        soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;

            //        soldierData.icon = SpriteName.WarsUnitIcon_IronManCannon;
            //        soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

            //        soldierData.rowWidth = 3;
            //        soldierData.columnsDepth = 2;
            //        soldierData.workForcePerUnit = 2;

            //        soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.attackStart = new Vector3(0, DssConst.Men_StandardModelScale * 0.4f, DssConst.Men_StandardModelScale * 1f);
            //        soldierData.rotationSpeed = DssConst.WarmashineRotatingSpeed_Wheels;
            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        soldierData.blocksRefillTimeSec = DssConst.BadBlockRefillTimeSec;
            //        break;

            //    case ItemResourceType.RoseWarrior_soldier:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.05f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_rosewarrior;
            //        soldierData.factionColoredModel = false;
            //        soldierData.icon = SpriteName.MissingImage;
            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 1.4f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.rowWidth = 5;
            //        soldierData.columnsDepth = 4;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 1.2f;
            //        soldierData.groupSpacingRndOffset= DssVar.StandardBoundRadius * 1f;
            //        break;

            //    case ItemResourceType.RoseWarrior_dog:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.05f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_rosedog;
            //        soldierData.factionColoredModel = false;
            //        soldierData.icon = SpriteName.MissingImage;
            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 1f;
            //        soldierData.hasBannerMan = false;
            //        soldierData.rowWidth = 5;
            //        soldierData.columnsDepth = 4;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 1.2f;
            //        soldierData.groupSpacingRndOffset = DssVar.StandardBoundRadius * 1f;
            //        break;

            //    case ItemResourceType.RoseWarrior_tank:
            //        soldierData.mainAttack = AttackType.Melee;
            //        soldierData.attackRange = 0.05f;
            //        soldierData.modelName = LootFest.VoxelModelName.wars_rosetank;
            //        soldierData.factionColoredModel = false;
            //        soldierData.icon = SpriteName.MissingImage;
            //        soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
            //        soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
            //        soldierData.hasBannerMan = false;

            //        soldierData.rowWidth =3;
            //        soldierData.columnsDepth = 2;
            //        soldierData.groupSpacingRndOffset = DssVar.StandardBoundRadius * 1f;
            //        soldierData.blockReducingAttack_Inv = DssConst.MediumBlockReduceAttack_Inv;
            //        break;


            //}
            if (conscript.weapon == ItemResourceType.Pike)
            {
                conscript.specialization = SpecializationType.AntiCavalry;
            }

            switch (conscript.specialization)
            {
                case SpecializationType.CityGuard:
                    
                    soldierData.rowWidth = DssConst.SoldierGroup_GuardCount;
                    soldierData.columnsDepth = 1;
                    soldierData.groupSpacing *= 0.5f;
                    soldierData.hasBannerMan = false;
                    break;

                case SpecializationType.Field:
                    soldierData.attackDamage = MathExt.AddPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageSea = MathExt.SubtractPercentage(soldierData.attackDamageSea, DssConst.Conscript_SpecializePercentage);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);
                    break;

                case SpecializationType.Viking:
                case SpecializationType.Sea:
                    conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);

                    soldierData.attackDamage = MathExt.SubtractPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    float seaDamagePerc = conscript.specialization == SpecializationType.Sea ?
                        DssConst.Conscript_SpecializePercentage : DssConst.Conscript_SpecializePercentage * 3f;
                    soldierData.attackDamageSea = MathExt.AddPercentage(soldierData.attackDamageSea, seaDamagePerc);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);

                    if (!ranged)
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
                    soldierData.modelScale = DssConst.Men_ModCharacterScale * 1.2f;
                    soldierData.upkeepMultiplier = 0;
                    soldierData.modelName = LootFest.VoxelModelName.little_hirdman;
                    soldierData.modelVariationCount = 1;
                    soldierData.icon = SpriteName.WarsUnitIcon_Honorguard;
                    break;

                case SpecializationType.Traditional:
                    soldierData.upkeepMultiplier *= 0.5f;
                    break;

                case SpecializationType.Green:
                    soldierData.secondaryAttack = AttackType.Arrow;
                    soldierData.secondaryAttackDamage = 100;
                    soldierData.secondaryAttackRange = 1.7f;
                    soldierData.bonusProjectiles = 2;
                    soldierData.icon = SpriteName.WarsUnitIcon_Greensoldier;
                    break;

                case SpecializationType.DarkLord:
                    //soldierData.modelScale = DssConst.Men_StandardModelScale;
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed;
                    soldierData.defaultArmyPlacement = ArmyPlacementGrid.Row_Behind;
                    soldierData.basehealth = DssConst.Soldier_DefaultHealth * 4;
                    soldierData.modelName = LootFest.VoxelModelName.wars_darklord;
                    soldierData.factionColoredModel = false;

                    soldierData.workForcePerUnit = 0;
                    soldierData.rowWidth = 1;
                    soldierData.columnsDepth = 1;
                    //soldierData.upkeepPerSoldier = 0;

                    soldierData.attackRange = 0.02f;
                    soldierData.basehealth = DssConst.Soldier_DefaultHealth * 4;
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackDamage = 500;
                    soldierData.attackDamageStructure = soldierData.attackDamage;
                    soldierData.attackDamageSea = soldierData.attackDamage;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 0.5f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_darklord;
                    break;
            }

            soldierData.copperUpkeepPerSoldier = DssConst.TrainingGoldUpkeep[(int)conscript.training];
            if (conscript.man == ItemResourceType.NobelMen)
            {
                soldierData.copperUpkeepPerSoldier += DssConst.Nobel_GoldUpkeep;
            }

            soldierData.attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(conscript.training);
            soldierData.attackTimePlusCoolDown /= 1f + skillBonus;

           
            return soldierData;

            
        }

        public SoldierData bannermanSetup(SoldierData soldierData)
        {
            soldierData.attackDamage /= 2;
            soldierData.attackDamageStructure /= 2;

            soldierData.factionColoredModel = true;
            soldierData.modelName = LootFest.VoxelModelName.war_bannerman;
            soldierData.modelVariationCount = 1;

            return soldierData;
        }

        public void shipSetup(ref SoldierData soldierData)
        {
            soldierData.hasBannerMan = false;
            soldierData.modelName = LootFest.VoxelModelName.NUM_NON;

            soldierData.walkingSpeed = DssConst.Men_StandardShipSpeed;

            soldierData.modelScale = DssConst.Men_StandardModelScale * 6f;

            soldierData.modelToShadowScale = new Vector3(0.5f, 1f, 0.8f);
            soldierData.basehealth = soldierData.basehealth * soldierData.rowWidth * soldierData.columnsDepth;
            soldierData.rowWidth = 1;
            soldierData.columnsDepth = 1;
            soldierData.rotationSpeed = DssConst.ShipRotatingSpeed;
            soldierData.modelData.modelType = ModelType.Ship;


            switch (conscript.specialization)
            {
                case SpecializationType.Sea:
                    soldierData.walkingSpeed *= 1.4f;
                    soldierData.rotationSpeed *= 1.2f;
                    break;

                case SpecializationType.Viking:
                    conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);
                    if (!ranged)
                    {
                        soldierData.modelName = LootFest.VoxelModelName.wars_viking_ship;

                        soldierData.mainAttack = AttackType.Javelin;
                        soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 2.5f;
                        soldierData.attackRange = 1f;
                    }
                    soldierData.walkingSpeed *= 1.5f;
                    soldierData.rotationSpeed *= 1.2f;
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
                    case ItemResourceType.Settler:
                    case ItemResourceType.SharpStick:
                    case ItemResourceType.UN_BatteringRam:

                    case ItemResourceType.RoseWarrior_soldier:
                    case ItemResourceType.RoseWarrior_tank:
                    case ItemResourceType.RoseWarrior_dog:
                        soldierData.modelName = LootFest.VoxelModelName.wars_folk_ship;
                        break;

                    case ItemResourceType.Pike:
                    case ItemResourceType.HandSpear:
                    case ItemResourceType.BronzeSword:
                    case ItemResourceType.ShortSword:
                    case ItemResourceType.Sword:
                    case ItemResourceType.ThrowingSpear:
                    case ItemResourceType.LongSword:
                        soldierData.modelName = LootFest.VoxelModelName.wars_soldier_ship;
                        break;

                    case ItemResourceType.Crossbow:
                    case ItemResourceType.LongBow:
                    case ItemResourceType.SlingShot:
                    case ItemResourceType.Bow:
                    case ItemResourceType.HandCannon:
                    case ItemResourceType.HandCulverin:
                    case ItemResourceType.Rifle:
                    case ItemResourceType.Blunderbuss:
                        soldierData.modelName = LootFest.VoxelModelName.wars_archer_ship;
                        break;

                    case ItemResourceType.Ballista:
                    case ItemResourceType.Manuballista:
                    case ItemResourceType.Catapult:

                    case ItemResourceType.SiegeCannonBronze:
                    case ItemResourceType.ManCannonBronze:
                    case ItemResourceType.SiegeCannonIron:
                    case ItemResourceType.ManCannonIron:
                        soldierData.modelName = LootFest.VoxelModelName.wars_ballista_ship;
                        break;

                    case ItemResourceType.Warhammer:
                    case ItemResourceType.TwoHandSword:
                    //case ItemResourceType.KnightsLance:
                    case ItemResourceType.MithrilSword:
                    case ItemResourceType.MithrilBow:
                        soldierData.modelName = LootFest.VoxelModelName.wars_knight_ship;
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;

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
                case ItemResourceType.Manuballista:
                case ItemResourceType.Catapult:
                case ItemResourceType.UN_BatteringRam:
                case ItemResourceType.SiegeCannonBronze:
                case ItemResourceType.ManCannonBronze:
                case ItemResourceType.SiegeCannonIron:
                case ItemResourceType.ManCannonIron:
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
            soldierData.attackDamage = Convert.ToInt32(ConscriptProfile.WeaponDamage(conscript.weapon, out soldierData.attackSplashCount) * skillBonus);
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

                case ItemResourceType.HandSpear:
                    soldierData.arrowWeakness = true;
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.05f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_spearman;
                    soldierData.modelVariationCount = 1;
                    soldierData.modelScale *= 1.0f;
                    soldierData.icon = SpriteName.LittleUnitIconSpearman;
                    soldierData.basehealth += DssConst.WeaponHealthAdd_Handspear;
                    break;

                case ItemResourceType.Warhammer:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.04f;
                    soldierData.modelName = LootFest.VoxelModelName.wars_hammer;
                    soldierData.modelScale *= 1.14f;
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

                    soldierData.rowWidth = 4;
                    soldierData.columnsDepth = 3;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 1.4f;
                    soldierData.workForcePerUnit = 2;
                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    //soldierData.ArmySpeedBonusLand = 0.8;
                    break;

                case ItemResourceType.MithrilSword:
                    soldierData.mainAttack = AttackType.Melee;
                    soldierData.attackRange = 0.055f;
                    soldierData.modelScale *= 1.5f;
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
                    soldierData.modelName = LootFest.VoxelModelName.wars_mithrilarcher;
                    soldierData.modelScale *= 1.3f;
                    
                    soldierData.icon = SpriteName.WarsUnitIcon_MithrilArcher;
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
                    //soldierData.attackSplashCount = 8;
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
                    //soldierData.attackSplashCount = 8;
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
                    //soldierData.attackSplashCount = 1;
                    soldierData.attackDamageStructure = Convert.ToInt32(1500 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

                    soldierData.modelName = LootFest.VoxelModelName.war_ballista;
                    soldierData.modelVariationCount = 2;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsUnitIcon_Ballista;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

                    soldierData.rowWidth = 3;
                    soldierData.columnsDepth = 2;
                    soldierData.workForcePerUnit = 2;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                    break;

                case ItemResourceType.Manuballista:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2;
                    //soldierData.attackSplashCount = 1;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Ballista;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_manuballista;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 1.5f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;

                    soldierData.icon = SpriteName.WarsResource_Manuballista;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

                    soldierData.rowWidth = 3;
                    soldierData.columnsDepth = 2;
                    soldierData.workForcePerUnit = 2;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                    break;

                case ItemResourceType.Catapult:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2.6f;
                    //soldierData.attackSplashCount = 3;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Catapult;
                    soldierData.attackDamageStructure = Convert.ToInt32(2000 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 16f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_catapult;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2.3f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsUnitIcon_Catapult;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

                    soldierData.rowWidth = 2;
                    soldierData.columnsDepth = 2;
                    soldierData.workForcePerUnit = 2;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                    break;

                case ItemResourceType.SiegeCannonBronze:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.3f;
                    soldierData.attackRange = 2.4f;
                    //soldierData.attackSplashCount = 12;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.MassiveCannonball;
                    soldierData.attackDamageStructure = Convert.ToInt32(2000 * skillBonus);
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 50f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_bronzesiegecannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 5f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsUnitIcon_Catapult;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

                    soldierData.rowWidth = 1;
                    soldierData.columnsDepth = 1;
                    soldierData.workForcePerUnit = 6;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                    break;

                case ItemResourceType.ManCannonBronze:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2;
                    //soldierData.attackSplashCount = 5;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Cannonball;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 20f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_bronzemancannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 2f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;

                    soldierData.icon = SpriteName.WarsResource_BronzeManCannon;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

                    soldierData.rowWidth = 3;
                    soldierData.columnsDepth = 2;
                    soldierData.workForcePerUnit = 2;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.5f;
                    break;

                case ItemResourceType.SiegeCannonIron:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2.2f;
                    //soldierData.attackSplashCount = 2;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Haubitz;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 20f;

                    soldierData.modelName = LootFest.VoxelModelName.wars_ironsiegecannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 1f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Back;

                    soldierData.icon = SpriteName.WarsResource_IronSiegeCannon;

                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 4;

                    soldierData.rowWidth = 3;
                    soldierData.columnsDepth = 2;
                    soldierData.workForcePerUnit = 2;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
                    break;

                case ItemResourceType.ManCannonIron:
                    soldierData.walkingSpeed = DssConst.Men_StandardWalkingSpeed * 0.6f;
                    soldierData.attackRange = 2.4f;
                    //soldierData.attackSplashCount = 6;

                    soldierData.basehealth = MathExt.MultiplyInt(0.5, soldierData.basehealth);
                    soldierData.mainAttack = AttackType.Cannonball;
                    soldierData.attackTimePlusCoolDown = DssConst.Soldier_StandardAttackAndCoolDownTime * 18;

                    soldierData.modelName = LootFest.VoxelModelName.wars_ironmancannon;

                    soldierData.modelScale = DssConst.Men_StandardModelScale * 1.7f;
                    soldierData.ArmyFrontToBackPlacement = ArmyPlacement.Mid;

                    soldierData.icon = SpriteName.WarsUnitIcon_IronManCannon;
                    soldierData.energyPerSoldier = DssLib.SoldierDefaultEnergyUpkeep * 2;

                    soldierData.rowWidth = 3;
                    soldierData.columnsDepth = 2;
                    soldierData.workForcePerUnit = 2;

                    soldierData.upkeepPerSoldier = DssLib.SoldierDefaultUpkeep * 2;
                    soldierData.groupSpacing = DssVar.DefaultGroupSpacing * 2.2f;
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
                    conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool knight, out bool warmashine);

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

                    soldierData.workForcePerUnit = 0;
                    soldierData.rowWidth = 1;
                    soldierData.columnsDepth = 1;
                    soldierData.upkeepPerSoldier = 0;
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

            soldierData.modelToShadowScale = new Vector3(0.5f, 1f, 0.8f);
            soldierData.rowWidth = 1;
            soldierData.columnsDepth = 1;

            switch (conscript.specialization)
            {
                case SpecializationType.Viking:
                    conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool knight, out bool warmashine);
                    if (!ranged)
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
                    case ItemResourceType.UN_BatteringRam:
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
                    case ItemResourceType.Blunderbus:
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
                    case ItemResourceType.KnightsLance:
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

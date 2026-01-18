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

        public UnitBuildType unitType()
        {
            if (conscript.vehicle != ItemResourceType.NONE)
            {
                return UnitBuildType.ConscriptWagon;
            }

            if (conscript.animal != ItemResourceType.NONE &&
                ItemPropertyColl.Get(conscript.animal).Filter_IsRidingAnimal)
            {
                return UnitBuildType.ConscriptCavalry;
            }

            switch (conscript.specialization)
            {
                default:
                    return ItemPropertyColl.Get(conscript.weapon).Filter_IsSiegeWeapon ? UnitBuildType.ConscriptWarmachine : UnitBuildType.Conscript;
                //switch (conscript.weapon)
                //{
                //    case ItemResourceType.Ballista:
                //    case ItemResourceType.Manuballista:
                //    case ItemResourceType.Catapult:
                //    case ItemResourceType.UN_BatteringRam:
                //    case ItemResourceType.SiegeCannonBronze:
                //    case ItemResourceType.ManCannonBronze:
                //    case ItemResourceType.SiegeCannonIron:
                //    case ItemResourceType.ManCannonIron:
                //        return UnitType.ConscriptWarmachine;

                //    default:
                //        return UnitType.Conscript;
                //}

                case SpecializationType.CityGuard:
                    return UnitBuildType.CityGuard;
                    //case SpecializationType.DarkLord:
                    //    return UnitType.DarkLord;

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
            SoldierData soldierData = ItemPropertyColl.Get(conscript.weapon).soldierData;
            soldierData.applySkillBonus(skillBonus);

            var armorData = ItemPropertyColl.Get(conscript.armorLevel).soldierData;
            soldierData.basehealth = armorData.basehealth;
            soldierData.modelData.armor = armorData.modelData.armor;

            soldierData.modelData.specialization = conscript.specialization;

            if (conscript.animal != ItemResourceType.NONE &&
                Resource.ItemPropertyColl.Get(conscript.animal).Filter_IsRidingAnimal)
            {
                soldierData.modelData.riding = true;
            }
            
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

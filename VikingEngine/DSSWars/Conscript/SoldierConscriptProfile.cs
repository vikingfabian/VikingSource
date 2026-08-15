using Microsoft.Xna.Framework;
using System;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.DetailObj.Data;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.PJ;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.DSSWars.Conscript
{

    struct SoldierConscriptProfile
    {
        public ConscriptProfile conscript;
        public float skillBonus;
        public float mobileBonus_PercAdd;

        public SoldierConscriptProfile()

        {
            conscript = new ConscriptProfile();
            conscript.weapon = ItemResourceType.SharpStick;
            skillBonus = 1;

        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            conscript.writeGameState(w);
            byte wrote = StreamLib.WriteFloatMultiplier(skillBonus, w);
#if DEBUG
            if (wrote == 0)
            {
                throw new Exception();
            }
            if (skillBonus == 0)
            {
                throw new Exception();
            }
#endif
        }
        public void readGameState(System.IO.BinaryReader r)
        {
            conscript.readGameState(r);
            skillBonus = StreamLib.ReadFloatMultiplier(r);
#if DEBUG
            if (skillBonus == 0)
            {
                throw new Exception();
            }
#endif
        }

        public UnitBuildType unitType()
        {
            if (conscript.vehicle != ItemResourceType.NONE)
            {
                switch (conscript.animal)
                {
                    case ItemResourceType.Elephant:
                    case ItemResourceType.WarElephant:
                    case ItemResourceType.Oliphant:
                        return UnitBuildType.ConscriptBalkong;
                }
                return UnitBuildType.ConscriptWagon;
            }

            if (conscript.animal != ItemResourceType.NONE &&
                ItemPropertyColl.Get(conscript.animal).Filter_IsRidingAnimal)
            {
                //switch (conscript.animal)
                //{
                //    case ItemResourceType.Elephant:
                //    case ItemResourceType.WarElephant:
                //    case ItemResourceType.Oliphant:
                //        if (conscript.vehicle != ItemResourceType.NONE)
                //        { 
                //            return UnitBuildType.ConscriptBalkong;
                //        }
                //        break;
                //}
                if (conscript.weapon == ItemResourceType.SiegeCannonBronze)
                {
                    return UnitBuildType.ConscriptWagon;
                }

                return UnitBuildType.ConscriptCavalry;
            }

            switch (conscript.specialization)
            {
                default:
                    return ItemPropertyColl.Get(conscript.weapon).Filter_IsWarMachine ? UnitBuildType.ConscriptWarmachine : UnitBuildType.Conscript;
               
                case SpecializationType.CityGuard:
                    return UnitBuildType.CityGuard;
            }

                    
        }

        public UnitNameType filterType()
        {
            switch (conscript.specialization)
            {
                default:
                    switch (conscript.weapon)
                    {
                        case ItemResourceType.Settler:
                            return UnitNameType.Settler;
                        case ItemResourceType.SharpStick:
                            return UnitNameType.SharpStick;

                        case ItemResourceType.BronzeSword:
                        case ItemResourceType.ShortSword:
                        case ItemResourceType.Sword:
                            return UnitNameType.Sword;
                        case ItemResourceType.LongSword:
                            return UnitNameType.LongSword;

                        case ItemResourceType.Pike:
                            return UnitNameType.Pike;
                        case ItemResourceType.HandSpear:
                            return UnitNameType.Spear;

                        case ItemResourceType.Warhammer:
                            return UnitNameType.Warhammer;
                        case ItemResourceType.TwoHandSword:
                            return UnitNameType.TwohandSword;
                        //case ItemResourceType.KnightsLance:
                        //    return UnitFilterType.Knight;
                        case ItemResourceType.MithrilSword:
                            return UnitNameType.MithrilKnight;

                        case ItemResourceType.SlingShot:
                        case ItemResourceType.ThrowingSpear:
                            return UnitNameType.Skirmisher;
                        case ItemResourceType.Bow:
                        case ItemResourceType.LongBow:
                            return UnitNameType.Bow;

                        case ItemResourceType.Crossbow:
                            return UnitNameType.CrossBow;
                        case ItemResourceType.MithrilBow:
                            return UnitNameType.MithrilBow;

                        case ItemResourceType.HandCannon:
                        case ItemResourceType.Rifle:
                            return UnitNameType.Rifle;
                        case ItemResourceType.HandCulverin:
                        case ItemResourceType.Blunderbuss:
                            return UnitNameType.Shotgun;

                        case ItemResourceType.Ballista:
                            return UnitNameType.Ballista;
                        case ItemResourceType.Manuballista:
                            return UnitNameType.ManuBallista;
                        case ItemResourceType.Catapult:
                            return UnitNameType.Catapult;
                        case ItemResourceType.SiegeCannonBronze:
                            return UnitNameType.SiegeCannonBronze;
                        case ItemResourceType.ManCannonBronze:
                            return UnitNameType.ManCannonBronze;
                        case ItemResourceType.SiegeCannonIron:
                            return UnitNameType.SiegeCannonIron;
                        case ItemResourceType.ManCannonIron:
                            return UnitNameType.ManCannonIron;

                        case ItemResourceType.RoseWarrior_dog:
                            return UnitNameType.RoseWarrior;
                        case ItemResourceType.RoseWarrior_soldier:
                            return UnitNameType.RoseWarrior;
                        case ItemResourceType.RoseWarrior_tank:
                            return UnitNameType.RoseWarrior;


                        default:
                            throw new NotImplementedException();

                    }

                case SpecializationType.Green:
                    return UnitNameType.GreenSoldier;
                case SpecializationType.HonorGuard:
                    return UnitNameType.HonourGuard;
                case SpecializationType.Viking:
                    return UnitNameType.Viking;
                case SpecializationType.DarkLord:
                    return UnitNameType.DarkLord;
            }
        }

        public SpriteName Icon()
        {
            return createSoldierData().icon;
        }

        public SoldierData createSoldierData()
        {
//#if DEBUG
//            if (skillBonus == 0)
//            {
//                throw new Exception();
//            }
//#endif

            var weaponProperties = ItemPropertyColl.Get(conscript.weapon);
            SoldierData soldierData = weaponProperties.soldierData;

            //soldierData.applySkillBonus(skillBonus, mobileBonus_PercAdd);

            var armorData = ItemPropertyColl.Get(conscript.armorLevel).soldierData;
            
            soldierData.modelData.armor = armorData.modelData.armor;

            soldierData.modelData.shield = conscript.shield;
            
            soldierData.applySkillBonus(skillBonus, mobileBonus_PercAdd);

            soldierData.modelData.specialization = conscript.specialization;

            if (conscript.vehicle != ItemResourceType.NONE)
            {
                var wagonProperties = Resource.ItemPropertyColl.Get(conscript.vehicle);
                var animalProperties = Resource.ItemPropertyColl.Get(conscript.animal);

                if (animalProperties.wagonPull == WagonPull.Balcon)
                {
                    soldierData.columnsDepth = animalProperties.soldierData.columnsDepth;
                    soldierData.rowWidth = animalProperties.soldierData.rowWidth;                    
                    soldierData.boundRadius = animalProperties.soldierData.boundRadius;
                    soldierData.groupSpacing = animalProperties.soldierData.groupSpacing;
                }
                else
                {
                    soldierData.WagonSetup();
                }
                if (conscript.vehicle == ItemResourceType.Wagon4Wheel &&
                   weaponProperties.Filter_IsWarMachine)
                {
                    soldierData.modelData.riding = true;
                }
                

                soldierData.walkingSpeed = new IntervalF(animalProperties.soldierData.lightWagonSpeed, animalProperties.soldierData.heavyWagonSpeed).GetFromPercent(wagonProperties.soldierData.weightClass);

                ridingAnimalSetup(conscript.animal, conscript.mountArmor, ref soldierData);
                wagonSetup(conscript.vehicle, ref soldierData);

                ConscriptUnitCount unitCount = new ConscriptUnitCount(conscript);
                animalSetup(conscript.animal, unitCount.animalsPerUnit, ref soldierData);
            }
            else if (conscript.animal != ItemResourceType.NONE)
            {
                var animalProperties = Resource.ItemPropertyColl.Get(conscript.animal);
                if (animalProperties.Filter_IsRidingAnimal)
                {
                    soldierData.modelData.riding = true;
                    if (conscript.weapon != ItemResourceType.SiegeCannonBronze)
                    {
                        soldierData.columnsDepth = animalProperties.soldierData.columnsDepth;
                        soldierData.rowWidth = animalProperties.soldierData.rowWidth;
                    }
                    soldierData.boundRadius = animalProperties.soldierData.boundRadius;
                    soldierData.groupSpacing = animalProperties.soldierData.groupSpacing;

                    soldierData.walkingSpeed = animalProperties.soldierData.walkingSpeed;

                    ridingAnimalSetup(conscript.animal, conscript.mountArmor, ref soldierData);
                }
                else
                {
                    if (soldierData.UnitCount() == 1)
                    {
                        soldierData.rowWidth = 2;
                    }
                }
                animalSetup(conscript.animal, 1, ref soldierData);
            }
            

            if (conscript.weapon == ItemResourceType.Pike)
            {
                conscript.specialization = SpecializationType.AntiCavalry;
            }

            soldierData.unitFilter = conscript.classify();//out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);

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
                    

                    soldierData.attackDamage = MathExt.SubtractPercentage(soldierData.attackDamage, DssConst.Conscript_SpecializePercentage);
                    float seaDamagePerc = conscript.specialization == SpecializationType.Sea ?
                        DssConst.Conscript_SpecializePercentage : DssConst.Conscript_SpecializePercentage * 3f;
                    soldierData.attackDamageSea = MathExt.AddPercentage(soldierData.attackDamageSea, seaDamagePerc);
                    soldierData.attackDamageStructure = MathExt.SubtractPercentage(soldierData.attackDamageStructure, DssConst.Conscript_SpecializePercentage);

                    if (!soldierData.unitFilter.Contains(UnitFilterType.Ranged))//ranged)
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

            soldierData.attackTimePlusCoolDown /= ConscriptProfile.TrainingAttackSpeed(conscript.training);
            soldierData.attackTimePlusCoolDown /= 1f + skillBonus;

            ShieldProperties.AddToConscript(ref soldierData, ref conscript);
            if (conscript.armorLevel != ItemResourceType.NONE)
            {
                soldierData.basehealth += armorData.basehealth;
            }
            //ShieldProperties.AddToConscript(ref soldierData, ref conscript);

            return soldierData;

            
        }
        void animalSetup(ItemResourceType animal, int unitAnimalCount, ref SoldierData soldierData)
        {
            var animalData = ItemPropertyColl.Get(animal).soldierData;
            soldierData.animalFoodMultiplier = animalData.animalFoodMultiplier * unitAnimalCount;
        }
        void ridingAnimalSetup(ItemResourceType animal, ItemResourceType armor, ref SoldierData soldierData)
        {
            var animalData = ItemPropertyColl.Get(animal).soldierData;
            soldierData.attackDamage += animalData.attackDamage;
            soldierData.basehealth += animalData.basehealth;

            if (armor != ItemResourceType.NONE)
            {
                soldierData.basehealth += ItemPropertyColl.Get(armor).soldierData.basehealth;
            }
        }
        void wagonSetup(ItemResourceType wagon, ref SoldierData soldierData)
        {
            var wagonData = ItemPropertyColl.Get(wagon).soldierData;
            soldierData.attackDamage += wagonData.attackDamage;
            soldierData.basehealth += wagonData.basehealth;

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
            soldierData.boundRadius = DssVar.StandardBoundRadius * 6f;

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
                    //conscript.classify(out bool ranged, out bool rangedMan, out bool meleeMan, out bool warmachine, out bool animalCompanion, out bool animalMount, out bool wagonRide);
                    if (!soldierData.unitFilter.Contains(UnitFilterType.Ranged))
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
                        soldierData.modelName = LootFest.VoxelModelName.ErrorCube;
#if DEBUG
                        throw new NotImplementedException();
#endif
                        break;
                }
            }
        }
    }
}

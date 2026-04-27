using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Work
{
    

    struct WorkerStatus
    {
        static readonly IntervalF EnergyBounds = new IntervalF(DssConst.Worker_Starvation, DssConst.Worker_MaxEnergy);

        public int XpEntityIndex;
        //public WorkExperienceType xpType1, xpType2, xpType3;
        ////5 levels, using 50xp each
        //public byte xp1, xp2, xp3;
        public byte workBonus;

        public WorkType work;
        public int workSubType;
        public int orderId;

        public float processTimeLengthSec;
        public float processTimeStartStampSec;

        public IntVector2 subTileStart;
        public IntVector2 subTileEnd;

        public ItemResource carry;
        public float energy;


        public WorkerStatus(bool reserveIndex)
        {
            if (reserveIndex)
            {
                XpEntityIndex = DssRef.world.ReserveNextWorkXpIndex();
            }
            else
            {
                XpEntityIndex = -1;
            }
        }

        public void xpToHud(RichBoxContent content)
        {
            // Pair the XP values with their respective types
            if (XpEntityIndex >= 0)
            {
                var xpPairs = DssRef.world.listWorkXp(XpEntityIndex);

                foreach (var xpPair in xpPairs)
                {
                    /*if (xpPair.xp > 0 && xpPair.type != WorkExperienceType.NUM_NONE*/
                    //{
                    LangLib.ExperienceType(xpPair.type, out string typeName, out SpriteName typeIcon);
                    var level = xpPair.xp.Level();

                    content.newLine();
                    content.Add(new RbImage(typeIcon));
                    content.space();
                    var typeNameText = new RbText(typeName + ":");
                    typeNameText.overrideColor = HudLib.TitleColor_TypeName;
                    content.Add(typeNameText);

                    content.Add(new RbTab(0.2f));
                    content.Add(new RbImage(LangLib.ExperienceLevelIcon(level)));
                    content.Add(new RbText(LangLib.ExperienceLevel(level)));
                    //}
                }
            }
        }

        const int TimeNetShareDiv = 4;

        public void writeGameState(City city, System.IO.BinaryWriter w, bool netPacket)
        {
            //w.Write((byte)xpType1);
            //w.Write((byte)xpType2);
            //w.Write((byte)xpType3);
            //w.Write(xp1);
            //w.Write(xp2);
            //w.Write(xp3);
            DssRef.world.writeWorkXp(XpEntityIndex, w);

            byte saveEnergy = EnergyBounds.GetValueBytePercentPos_WithBound(energy);
            w.Write(saveEnergy);

            carry.writeGameState(w);

            if (netPacket)
            {
                w.Write((byte)work);
                w.Write((byte)workSubType);
                int secondsPassed = Convert.ToInt32(processTimeStartStampSec - Ref.TotalGameTimeSec);
                w.Write(Bound.Byte(secondsPassed / TimeNetShareDiv));
                w.Write(Bound.Byte((int)processTimeLengthSec / TimeNetShareDiv));
                (subTileEnd - city.cityHallSubtilePos).writeShort(w);
            }
        }
        public void readGameState(City city, System.IO.BinaryReader r, bool netPacket, int subversion)
        {
            //xpType1 = (WorkExperienceType)r.ReadByte();
            //xpType2 = (WorkExperienceType)r.ReadByte();
            //xpType3 = (WorkExperienceType)r.ReadByte();

            //if (subversion < 80)
            //{
            //    XpLib.AdjustVersion80Skill(ref xpType1);
            //    XpLib.AdjustVersion80Skill(ref xpType2);
            //    XpLib.AdjustVersion80Skill(ref xpType3);
            //}

            //xp1 = r.ReadByte();
            //xp2 = r.ReadByte();
            //xp3 = r.ReadByte();
            DssRef.world.readWorkXp(XpEntityIndex, r, subversion);

            energy = EnergyBounds.GetFromBytePercent(r.ReadByte());
            
            carry.readGameState(r, subversion);

            if (netPacket)
            {
                work = (WorkType)r.ReadByte();
                workSubType= r.ReadByte();
                int secondsPassed = r.ReadByte() * TimeNetShareDiv;
                processTimeStartStampSec = Ref.TotalGameTimeSec - secondsPassed;
                processTimeLengthSec = r.ReadByte() * TimeNetShareDiv;
                subTileEnd = IntVector2.FromReadShort(r) + city.cityHallSubtilePos;
                subTileStart = subTileEnd;
            }
        }

        public override string ToString()
        {
            return "Worker (" + work.ToString() + "), carry (" + carry.ToString() + ")";
        }

        public string workString()
        {
            switch (work)
            {
               
                case WorkType.Build:
                    return string.Format(DssRef.lang.WorkerStatus_BuildX, BuildLib.BuildOptions[workSubType].Label());
               
                case WorkType.Upgrade:
                    return DssRef.lang.Upgrade_Order;

                case WorkType.Craft:
                    IconName.Item((ItemResourceType)workSubType, out var icon, out var name);
                    return string.Format(DssRef.lang.Work_CraftX, name);

                case WorkType.DropOff:
                    return DssRef.lang.WorkerStatus_DropOff;

                case WorkType.Eat:
                    return DssRef.lang.WorkerStatus_Eat;
                case WorkType.GatherFoil:
                    return DssRef.lang.WorkerStatus_Gather;
                case WorkType.Idle:
                    return DssRef.lang.Hud_Idle;
                case WorkType.Mine:
                    return DssRef.lang.Work_Mining;
                case WorkType.PickUpProduce:
                case WorkType.PickUpResource:
                    return DssRef.lang.WorkerStatus_PickUpResource;
                case WorkType.Plant:
                    return DssRef.lang.WorkerStatus_Plant;
                
                case WorkType.Starving:
                case WorkType.Exit:
                    return DssRef.lang.WorkerStatus_Exit;
                case WorkType.TrossReturnToArmy:
                    return DssRef.lang.WorkerStatus_TrossReturnToArmy;
                case WorkType.Demolish:
                    return DssRef.lang.Build_DestroyBuilding;
                case WorkType.School:
                    return DssRef.lang.BuildingType_School;

                default:
                    return TextLib.Error;
            }
        }

        void workComplete(Army army)
        {
            switch (work)
            {
                case WorkType.TrossCityTrade:
                    var toCity = DssRef.world.tileGrid.Get(subTileEnd / WorldData.TileSubDivitions).City();

                    ItemResourceType foodType = (ItemResourceType)workSubType;
                    //if (toCity.GetGroupedResource(EntityComponent.CityResoureIndex.ConservedFood).amount >= ItemPropertyColl.DefaultCarry)
                    //{
                    //    foodType = ItemResourceType.ConservedFood;
                    //}
                    //else
                    //{ 
                    //    foodType= ItemResourceType.Food_G;
                    //}

                    ItemResource recieved = toCity.MakeTrade(foodType, carry.amount, DssConst.Worker_TrossWorkerCarryWeight);
                    carry = recieved;

                    createWorkOrder(WorkType.TrossReturnToArmy, 0, 0, WorkExperienceType.NUM_NONE, -1, WP.ToSubTilePos_Centered(army.tilePos), null);
                    break;
                case WorkType.TrossReturnToArmy:
                    if (carry.type == ItemResourceType.ConservedFood)
                    {
                        army.conservedFood += carry.amount;
                    }
                    else
                    {
                        army.food += carry.amount;
                    }
                    //work = WorkType.IsDeleted;
                    DeleteMe();
                    break;
            }

        }

        public void DeleteMe()
        { 
            work = WorkType.IsDeleted;
            DssRef.world.FreeWorkerXp(XpEntityIndex);
            XpEntityIndex = -1;
        }

        int farmGrowthMultiplier(int terrainAmount, City city, bool upgraded)
        {
            //terrainAmount *= 5;
            if (upgraded)
            {
                terrainAmount *= 2;
            }

            if (city.cityCulture == CityCulture.FertileGround)
            {
                return terrainAmount * 2;
            }
            return terrainAmount;
        }


        //void addCraftResult(City city, CraftBlueprint blueprint, bool visualUnit, out bool alwaysNeedMore)
        //{
        //    alwaysNeedMore = false;

        //    blueprint.craftItemResult(out int amount1, out ItemResourceType item1, out int amount2, out ItemResourceType item2);

        //    void result(int add, ItemResourceType item)
        //    {
        //        if (add > 0)
        //        {
        //            switch (item)
        //            {
        //                case ItemResourceType.Food_G:
        //                    city.foodProduction.add(add);
        //                    break;

        //                case ItemResourceType.Fuel_G:
        //                case ItemResourceType.Coal:
        //                    item = ItemResourceType.Fuel_G;
        //                    if (city.Culture == CityCulture.PitMasters)
        //                    {
        //                        add *= 2;
        //                    }
        //                    break;


        //                case ItemResourceType.Iron_G:
        //                case ItemResourceType.Copper:
        //                case ItemResourceType.Tin:
        //                case ItemResourceType.Lead:
        //                case ItemResourceType.Silver:
        //                case ItemResourceType.RawMithril:
        //                    if (city.Culture == CityCulture.Smelters)
        //                    {
        //                        add *= 2;
        //                    }
        //                    break;
        //                case ItemResourceType.Beer:
        //                    if (city.Culture == CityCulture.Brewmaster)
        //                    {
        //                        add += add / 2;
        //                    }
        //                    break;

        //                case ItemResourceType.PaddedArmor:
        //                case ItemResourceType.HeavyPaddedArmor:
        //                    if (city.Culture == CityCulture.Weavers)
        //                    {
        //                        add += 1;
        //                    }
        //                    break;

        //                case ItemResourceType.IronArmor:
        //                case ItemResourceType.HeavyIronArmor:
        //                case ItemResourceType.LightPlateArmor:
        //                case ItemResourceType.FullPlateArmor:
        //                    if (city.Culture == CityCulture.Armorsmith)
        //                    {
        //                        add += 1;
        //                    }
        //                    break;
        //                case ItemResourceType.Bronze:
        //                case ItemResourceType.BronzeSword:

        //                    if (city.Culture == CityCulture.BronzeCasters)
        //                    {
        //                        add *= 2;
        //                    }
        //                    break;

        //                case ItemResourceType.BronzeArmor:
        //                    if (city.Culture == CityCulture.Armorsmith ||
        //                        city.Culture == CityCulture.BronzeCasters)
        //                    {
        //                        add += 1;
        //                    }
        //                    break;

        //                case ItemResourceType.Gold:
        //                case ItemResourceType.CopperCoin:
        //                case ItemResourceType.BronzeCoin:
        //                case ItemResourceType.SilverCoin:
        //                case ItemResourceType.ElfCoin:
        //                    alwaysNeedMore = true;
        //                    break;

        //                case ItemResourceType.TwoHandSword:
        //                    lib.DoNothing();
        //                    break;
        //            }

        //            city.AddGroupedResource(item, add);
        //            if (visualUnit)
        //            {
        //                new ResourceEffect(item, add, VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(subTileEnd), 0.08f), ResourceEffectType.Add);
        //            }
        //        }
        //    }                

        //}

        void breedingRndResult(City city, out bool upgrade, out bool downgrade)
        {
            double upChance = DssConst.BreedingUpChance;
            double downChance = DssConst.BreedingDownChance;

            double rnd = Ref.rnd.Double();
            if (city.cityCulture == CityCulture.AnimalBreeder2)
            {
                upChance *= 2;
                downChance = 0.6;
            }
            
            if (rnd < upChance)
            {
                downgrade = false;
                upgrade = true;
            }
            else if (rnd < downChance)
            {
                downgrade = true;
                upgrade = false;
            }
            else
            {
                downgrade = false;
                upgrade = false;
            }            
        }

        void workComplete(City city, bool visualUnit)
        {
            var faction = city.GetFaction_NoChecks();
            
            WorkExperienceType gainXp= WorkExperienceType.NUM_NONE;

            float energyCost = processTimeLengthSec * DssConst.WorkTeamEnergyCost;
            if (city.cityCulture == CityCulture.CrabMentality)
            {
                energyCost *= 0.5f;
            }
            energy -= energyCost;
            ref SubTile subTile = ref DssRef.world.subTileGrid.GetRef(subTileEnd);

            bool tryRepeatWork = false;

            switch (work)
            {
                case WorkType.Craft:
                    {

                        ItemResourceType workitem = (ItemResourceType)workSubType;
                        ItemPropertyColl.Blueprint(workitem, out var bp1, out var bp2);

                        bool alwaysNeedMore = false;
                        CraftBlueprint useBlueprint;

                        if (bp2 != null && bp2.tryPayResources(city))
                        { //Secondary blueprint has priority
                            useBlueprint = bp2;
                        }
                        else
                        {
                            bp1.payResources(city);
                            useBlueprint = bp1;
                            //gainXp = bp1.experienceType;
                            //addCraftResult(bp1);

                        }
                        gainXp = useBlueprint.experienceType;
                        var me = this;
                                                
                        useBlueprint.craftItemResult(out int amount1, out ItemResourceType item1, out int amount2, out ItemResourceType item2);

                        if (item1 != workitem)
                        {
                            switch (workitem)
                            {
                                //case ItemResourceType.CopperCoin:
                                //case ItemResourceType.BronzeCoin:
                                //case ItemResourceType.SilverCoin:
                                //case ItemResourceType.ElfCoin:
                                //    alwaysNeedMore = true;
                                //    break;

                                case ItemResourceType.SlaughterHen:
                                case ItemResourceType.SlaughterPig:
                                case ItemResourceType.SlaughterOxen:
                                case ItemResourceType.SlaughterKineOxen:

                                case ItemResourceType.SlaughterPony:
                                case ItemResourceType.SlaughterHorse:
                                case ItemResourceType.SlaughterWarHorse:
                                case ItemResourceType.SlaughterDraftHorse:

                                case ItemResourceType.SlaughterWildPig:
                                case ItemResourceType.SlaughterWildHog:
                                case ItemResourceType.SlaughterWarHog:
                                case ItemResourceType.SlaughterStagHog:

                                case ItemResourceType.SlaughterWolf:
                                case ItemResourceType.SlaughterWarg:
                                case ItemResourceType.SlaughterAlphaWarg:

                                case ItemResourceType.SlaughterWildCat:
                                case ItemResourceType.SlaughterLion:
                                case ItemResourceType.SlaughterWarLion:

                                case ItemResourceType.SlaughterElephant:
                                case ItemResourceType.SlaughterWarElephant:
                                case ItemResourceType.SlaughterOliphant:
                                    alwaysNeedMore = true;
                                    if (city.cityCulture == CityCulture.Butchers)
                                    {
                                        increaseMeat(ref item1, ref amount1);
                                        increaseMeat(ref item2, ref amount2);

                                        void increaseMeat(ref ItemResourceType item, ref int amount)
                                        {
                                            if (item == ItemResourceType.RawFood_Group)
                                            {
                                                amount *= 2;
                                            }
                                        }
                                    }
                                    else if (city.cityCulture == CityCulture.Skinner)
                                    {
                                        increaseSkin(ref item1, ref amount1);
                                        increaseSkin(ref item2, ref amount2);

                                        void increaseSkin(ref ItemResourceType item, ref int amount)
                                        {
                                            if (item == ItemResourceType.SkinLinen_Group)
                                            {
                                                amount *= 2;
                                            }
                                        }
                                    }

                                    if (city.cityBiome == CityBiome.Frozen)
                                    {
                                        increaseSkin(ref item1, ref amount1);
                                        increaseSkin(ref item2, ref amount2);

                                        void increaseSkin(ref ItemResourceType item, ref int amount)
                                        {
                                            if (item == ItemResourceType.SkinLinen_Group)
                                            {
                                                amount = MathExt.MultiplyInt(amount, 1.5);
                                            }
                                        }
                                    }
                                    break;

                                case ItemResourceType.PotContainer:
                                    if (city.cityCulture == CityCulture.Potters &&
                                        Ref.peRnd.ChanceF(0.5f))
                                    {
                                        amount1 += 1;
                                    }
                                    break;
                                case ItemResourceType.WoodContainer:
                                    if (city.cityCulture == CityCulture.Coopers &&
                                        Ref.peRnd.ChanceF(0.5f))
                                    {
                                        amount1 += 1;
                                    }
                                    break;
                            }
                        }

                        result(amount1, item1, 1);
                        result(amount2, item2, 2);

                        void result(int add, ItemResourceType item, int number)
                        {
                            if (add > 0)
                            {
                                switch (item)
                                {
                                    //case ItemResourceType.Food_G:
                                    //    city.foodProduction.add(add);
                                    //    break;

                                    case ItemResourceType.Fuel_G:
                                    //case ItemResourceType.Coal:
                                        item = ItemResourceType.Fuel_G;
                                        if (city.cityCulture == CityCulture.PitMasters)
                                        {
                                            add *= 2;
                                        }
                                        break;


                                    case ItemResourceType.Iron_G:
                                    case ItemResourceType.Copper:
                                    case ItemResourceType.Tin:
                                    case ItemResourceType.Lead:
                                    case ItemResourceType.Silver:
                                    case ItemResourceType.RawMithril:
                                        if (city.cityCulture == CityCulture.Smelters)
                                        {
                                            add *= 2;
                                        }
                                        break;
                                    case ItemResourceType.Beer:
                                        if (city.cityCulture == CityCulture.Brewmaster)
                                        {
                                            add += add / 2;
                                        }
                                        break;

                                    case ItemResourceType.ConservedFood:
                                        if (city.cityCulture == CityCulture.Salters)
                                        {
                                            add += add / 4;
                                        }
                                        break;

                                    case ItemResourceType.PaddedArmor:
                                    case ItemResourceType.HeavyPaddedArmor:
                                        if (city.cityCulture == CityCulture.Weavers)
                                        {
                                            add += 1;
                                        }
                                        break;

                                    case ItemResourceType.IronArmor:
                                    case ItemResourceType.HeavyIronArmor:
                                    case ItemResourceType.LightPlateArmor:
                                    case ItemResourceType.FullPlateArmor:
                                        if (city.cityCulture == CityCulture.Armorsmith)
                                        {
                                            add += 1;
                                        }
                                        break;
                                    case ItemResourceType.Bronze:
                                    case ItemResourceType.BronzeSword:

                                        if (city.cityCulture == CityCulture.BronzeCasters)
                                        {
                                            add *= 2;
                                        }
                                        break;

                                    case ItemResourceType.BronzeArmor:
                                        if (city.cityCulture == CityCulture.Armorsmith ||
                                            city.cityCulture == CityCulture.BronzeCasters)
                                        {
                                            add += 1;
                                        }
                                        break;

                                    case ItemResourceType.Gold:
                                    //case ItemResourceType.CopperCoin:
                                    //case ItemResourceType.BronzeCoin:
                                    //case ItemResourceType.SilverCoin:
                                    //case ItemResourceType.ElfCoin:
                                        alwaysNeedMore = true;
                                        break;

                                    case ItemResourceType.Brick:
                                        if (city.cityCulture == CityCulture.Potters)
                                        {
                                            amount1 += 2;
                                        }
                                        break;

                                    case ItemResourceType.BucklerShield:
                                    case ItemResourceType.RoundShield:
                                    case ItemResourceType.HeaterShield:
                                    case ItemResourceType.TowerShield:
                                        if (city.cityCulture == CityCulture.ShieldMaker &&
                                            Ref.peRnd.ChanceF(0.25f))
                                        {
                                            add += 1;
                                        }
                                        break;

                                    case ItemResourceType.Wagon2Wheel:
                                    case ItemResourceType.Wagon4Wheel:
                                    case ItemResourceType.WagonClosed:
                                    case ItemResourceType.WagonIron:
                                    case ItemResourceType.WagonSteel:
                                        if (city.cityCulture == CityCulture.Wainwright &&
                                            Ref.peRnd.ChanceF(0.25f))
                                        {
                                            add += 1;
                                        }
                                        break;
                                }

                                city.AddGroupedResource(item, add);
                                if (visualUnit)
                                {
                                    SpriteText3D.GetOrCreate().init(item, add, VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(me.subTileEnd), 0.08f * number), ResourceEffectType.Add);
                                }
                            }
                        }

                        tryRepeatWork = false;

                        if (alwaysNeedMore || city.GetGroupedResource(workitem).needMore())
                        {
                            if (bp1.hasResources(city))
                            {
                                tryRepeatWork = true;
                            }
                            else if (bp2 != null && bp2.hasResources(city))
                            {
                                tryRepeatWork = true;
                            }
                        }                       

                    }
                    break;

                case WorkType.Eat:
                    int eatAmount = (int)Math.Floor((DssConst.Worker_MaxEnergy - energy) / DssRef.difficulty.FoodEnergySett);

                    city.AddGroupedResource(CityResoureIndex.food, -eatAmount, false);
                    //city.foodSpending.add(eatAmount);
                    energy += eatAmount * DssRef.difficulty.FoodEnergySett;
                    break;


                case WorkType.GatherFoil:
                    {
                        //Resource.ItemResourceType resourceType;
                        TerrainSubFoilType foilType = subTile.GetFoilType();
                        switch (foilType)
                        {
                            case TerrainSubFoilType.TreeSoft:
                                gatherWood(Resource.ItemResourceType.SoftWood, ref subTile, city);
                                gainXp = WorkExperienceType.WoodWork;
                                break;

                            case TerrainSubFoilType.TreeHard:
                                gatherWood(Resource.ItemResourceType.HardWood, ref subTile, city);
                                gainXp = WorkExperienceType.WoodWork;
                                break;

                            case TerrainSubFoilType.DryWood:
                                gatherWood(Resource.ItemResourceType.DryWood, ref subTile, city);
                                gainXp = WorkExperienceType.WoodWork;
                                break;

                            case TerrainSubFoilType.TreeApple:
                            case TerrainSubFoilType.TreeBanana:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Food_G,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(DssConst.OrchidFoodAmount, city, false));

                                subTile.terrainAmount = TerrainContent.OrchardPlucked;

                                gainXp = WorkExperienceType.Farm;
                                break;

                            case TerrainSubFoilType.WheatFarm:
                            case TerrainSubFoilType.WheatFarmUpgraded:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Wheat,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(DssConst.WheatFoodAmount, city, foilType == TerrainSubFoilType.WheatFarmUpgraded));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;

                                gainXp = WorkExperienceType.Farm;
                                break;

                            case TerrainSubFoilType.LinenFarm:
                            case TerrainSubFoilType.LinenFarmUpgraded:

                                carry = new Resource.ItemResource(
                                        ItemResourceType.Linen,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(DssConst.LinenHarvestAmount, city, foilType == TerrainSubFoilType.LinenFarmUpgraded));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                //DssRef.world.subTileGrid.Set(subTileEnd, subTile);

                                gainXp = WorkExperienceType.Farm;
                                break;

                            case TerrainSubFoilType.RapeSeedFarm:
                            case TerrainSubFoilType.RapeSeedFarmUpgraded:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Rapeseed,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(DssConst.RapeSeedFuelAmount, city, foilType == TerrainSubFoilType.RapeSeedFarmUpgraded));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                //DssRef.world.subTileGrid.Set(subTileEnd, subTile);


                                gainXp = WorkExperienceType.Farm;
                                break;

                            case TerrainSubFoilType.HempFarm:
                            case TerrainSubFoilType.HempFarmUpgraded:
                                carry = new Resource.ItemResource(
                                        ItemResourceType.Hemp,
                                        subTile.terrainQuality,
                                        Convert.ToInt32(processTimeLengthSec),
                                        farmGrowthMultiplier(DssConst.HempLinenAndFuelAmount, city, foilType == TerrainSubFoilType.HempFarmUpgraded));

                                subTile.terrainAmount = TerrainContent.FarmCulture_Empty;
                                //DssRef.world.subTileGrid.Set(subTileEnd, subTile);

                                gainXp = WorkExperienceType.Farm;
                                break;

                            case TerrainSubFoilType.StoneBlock:
                            case TerrainSubFoilType.Stones:
                                int amount = 4;
                                if (workBonus > 0)
                                {
                                    amount = MathExt.AddPercentage(amount, workBonus);
                                }

                                if (city.cityCulture == CityCulture.Stonemason)
                                {
                                    amount *= 2;
                                }

                                carry = new ItemResource(ItemResourceType.Stone_G, amount, Convert.ToInt32(processTimeLengthSec), ItemPropertyColl.CarryStones);

                                gainXp = WorkExperienceType.StoneCutter;
                                break;

                            case TerrainSubFoilType.BogIron:
                                carry = new ItemResource(ItemResourceType.IronOre_G, 1, Convert.ToInt32(processTimeLengthSec), TerrainContent.DefaultMineAmount);

                                gainXp = WorkExperienceType.Mining;
                                break;
                            case TerrainSubFoilType.ClayPit:
                                carry = new ItemResource(ItemResourceType.Clay, 1, Convert.ToInt32(processTimeLengthSec), TerrainContent.DefaultMineAmount);

                                gainXp = WorkExperienceType.StoneCutter;
                                break;

                        }

                        //work = WorkType.Idle;                        
                    }
                    break;

                case WorkType.Plant:
                    bool available;
                    int waterCost;
                    switch ((TerrainSubFoilType)subTile.subTerrain)
                    {                       
                        case TerrainSubFoilType.TreeApple:
                        case TerrainSubFoilType.TreeBanana:
                            available = subTile.terrainAmount == TerrainContent.OrchardPlucked;
                            waterCost = DssConst.OrchardWaterCost;
                            break;
                        default:
                            available = subTile.terrainAmount == TerrainContent.FarmCulture_Empty;
                            waterCost = DssConst.PlantWaterCost;
                            break;
                    }

                    if (available)
                    {
                        subTile.terrainAmount++;
                        city.res_water.amount -= waterCost;

                        gainXp = WorkExperienceType.Farm;                        
                    }
                    break;

                case WorkType.PickUpResource:
                    if (subTile.collectionPointer >= 0)
                    {
                        var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                        carry = chunk.pickUp(1f);

                        if (carry.type != ItemResourceType.NONE)
                        {
                            DssRef.state.resources.update(subTile.collectionPointer, ref chunk);

                            if (chunk.count <= 0)
                            {
                                EditSubTile editTile = new EditSubTile(subTileEnd, subTile, false, false, true);
                                editTile.value.collectionPointer = -1;

                                if (subTile.mainTerrain == TerrainMainType.Resourses)
                                {
                                    editTile.value.mainTerrain = TerrainMainType.DefaultLand;
                                    editTile.editTerrain = true;
                                }
                                editTile.Submit();
                                //DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                            }
                        }


                    }
                    //work = WorkType.Idle;
                    break;

                case WorkType.PickUpProduce:
                    {
                        var building = (TerrainBuildingType)subTile.subTerrain;

                        AnimalPenGrowth size;
                        Resource.ItemResourceType resourceType;
                        breedingRndResult(city, out bool upgrade, out bool downgrade);

                        switch (building)
                        {
                            default:
                            case TerrainBuildingType.FowlHabitat:
                                resourceType = Resource.ItemResourceType.Fowl;                                
                                size = TerrainContent.FowlGrowth;
                                break;

                            case TerrainBuildingType.BoarHabitat:
                                resourceType = Resource.ItemResourceType.Boar;
                                size = TerrainContent.BoarGrowth;
                                break;

                            case TerrainBuildingType.OxHabitat:
                                resourceType = Resource.ItemResourceType.Oxen;
                                size = TerrainContent.OxenGrowth;
                                break;

                            case TerrainBuildingType.DogHabitat:
                                resourceType = Resource.ItemResourceType.Dog;
                                size = TerrainContent.DogGrowth;
                                break;

                            case TerrainBuildingType.PonyHabitat:
                                resourceType = Resource.ItemResourceType.Pony;
                                size = TerrainContent.PonyGrowth;
                                break;

                            case TerrainBuildingType.WolfHabitat:
                                resourceType = Resource.ItemResourceType.Wolf;
                                size = TerrainContent.WolfGrowth;
                                break;

                            case TerrainBuildingType.CatHabitat:
                                resourceType = Resource.ItemResourceType.WildCat;
                                size = TerrainContent.WildCatGrowth;
                                break;

                            case TerrainBuildingType.ElephantHabitat:
                                resourceType = Resource.ItemResourceType.Elephant;
                                size = TerrainContent.ElephantGrowth;
                                break;


                            case TerrainBuildingType.FowlPen:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Hen;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Fowl;
                                }
                                size = TerrainContent.FowlGrowth;
                                break;

                            case TerrainBuildingType.HenPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Fowl;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Hen;
                                }
                                size = TerrainContent.HenGrowth;
                                break;

                            
                            case TerrainBuildingType.BoarPen:
                                if (upgrade)
                                {
                                    if (city.cityBiome == CityBiome.Mountain)
                                    {
                                        resourceType = Resource.ItemResourceType.WildPig;
                                    }
                                    else
                                    { 
                                        resourceType= Resource.ItemResourceType.Pig;
                                    }
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Boar;
                                }
                                size = TerrainContent.BoarGrowth;
                                break;

                            case TerrainBuildingType.PigPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Boar;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Pig;
                                }
                                size = TerrainContent.PigGrowth;
                                break;

                            case TerrainBuildingType.OxenPen:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.KineOxen;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Oxen;
                                }
                                size = TerrainContent.OxenGrowth;
                                break;

                            case TerrainBuildingType.KineOxenPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Oxen;
                                }
                                else
                                { 
                                    resourceType = Resource.ItemResourceType.KineOxen;
                                }
                                size = TerrainContent.KineOxenGrowth;
                                break;

                            case TerrainBuildingType.DogCage:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Hound;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Dog;
                                }
                                size = TerrainContent.DogGrowth;
                                break;

                            case TerrainBuildingType.HoundCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Dog;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Hound;
                                }
                                size = TerrainContent.HoundGrowth;
                                break;

                            case TerrainBuildingType.PonyPen:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Horse;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Pony;
                                }
                                size = TerrainContent.PonyGrowth;
                                break;
                            case TerrainBuildingType.HorsePen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Pony;
                                }
                                else if (upgrade)
                                {
                                    if (Ref.rnd.Chance(0.5))
                                    {
                                        resourceType = Resource.ItemResourceType.WarHorse;
                                    }
                                    else
                                    {
                                        resourceType = Resource.ItemResourceType.DraftHorse;
                                    }
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Horse;
                                }                                
                                size = TerrainContent.HorseGrowth;
                                break;

                            case TerrainBuildingType.WarHorsePen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Horse;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WarHorse;
                                }
                                size = TerrainContent.WarHorseGrowth;
                                break;

                            case TerrainBuildingType.DraftHorsePen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Horse;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.DraftHorse;
                                }
                                size = TerrainContent.DraftHorseGrowth;
                                break;

                            case TerrainBuildingType.WildPigPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Boar;
                                }
                                else if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WildHog;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WildPig;
                                }
                                size = TerrainContent.WildPigGrowth;
                                break;

                            case TerrainBuildingType.WildHogPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WildPig;
                                }
                                else if (upgrade)
                                {
                                    if (Ref.rnd.Chance(0.5))
                                    {
                                        resourceType = Resource.ItemResourceType.WarHog;
                                    }
                                    else
                                    {
                                        resourceType = Resource.ItemResourceType.StagHog;
                                    }
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WildHog;
                                }
                                size = TerrainContent.WildHogGrowth;
                                break;

                            case TerrainBuildingType.WarHogPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WildHog;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WarHog;
                                }
                                size = TerrainContent.WarHogGrowth;
                                break;

                            case TerrainBuildingType.StagHogPen:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WildHog;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.StagHog;
                                }
                                size = TerrainContent.StagHogGrowth;
                                break;

                            case TerrainBuildingType.WolfCage:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Warg;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Wolf;
                                }
                                size = TerrainContent.WolfGrowth;
                                break;

                            case TerrainBuildingType.WargCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Wolf;
                                }
                                else if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.AlphaWarg;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Warg;
                                }
                                size = TerrainContent.WargGrowth;
                                break;

                            case TerrainBuildingType.AlphaWargCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Warg;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.AlphaWarg;
                                }
                                size = TerrainContent.AlphaWargGrowth;
                                break;

                            case TerrainBuildingType.WildCatCage:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Lion;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WildCat;
                                }
                                size = TerrainContent.WildCatGrowth;
                                break;
                            case TerrainBuildingType.LionCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WildCat;
                                }
                                else if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WarLion;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Lion;
                                }
                                size = TerrainContent.LionGrowth;
                                break;
                            case TerrainBuildingType.WarLionCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Lion;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WarLion;
                                }
                                size = TerrainContent.WarLionGrowth;
                                break;

                            case TerrainBuildingType.ElephantCage:
                                if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WarElephant;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Elephant;
                                }
                                size = TerrainContent.ElephantGrowth;
                                break;

                            case TerrainBuildingType.WarElephantCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Elephant;
                                }
                                else if (upgrade)
                                {
                                    resourceType = Resource.ItemResourceType.Oliphant;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.WarElephant;
                                }
                                size = TerrainContent.WarElephantGrowth;
                                break;

                            case TerrainBuildingType.OliphantCage:
                                if (downgrade)
                                {
                                    resourceType = Resource.ItemResourceType.WarElephant;
                                }
                                else
                                {
                                    resourceType = Resource.ItemResourceType.Oliphant;
                                }
                                size = TerrainContent.OliphantGrowth;
                                break;
                        }

                        //if (building == TerrainBuildingType.PigPen)
                        //{
                        //    resourceType = Resource.ItemResourceType.Pig;
                        //    min = TerrainContent.PigReady;
                        //    size = TerrainContent.PigMaxSize;
                        //}
                        //else
                        //{
                        //    resourceType = Resource.ItemResourceType.Hen;
                        //    min = TerrainContent.HenReady;
                        //    size = TerrainContent.HenMaxSize;
                        //}

                        if (subTile.terrainAmount >= size.harvestReady)
                        {
                            subTile.terrainAmount -= size.maxSize;

                            EditSubTile editTile = new EditSubTile(subTileEnd, subTile, false, true, false);
                            editTile.Submit();
                            
                            //DssRef.world.subTileGrid.Set(subTileEnd, subTile);


                            carry = new ItemResource(resourceType, 1, Convert.ToInt32(processTimeLengthSec), 1);
                        }


                        gainXp = WorkExperienceType.AnimalCare;
                    }
                    //work = WorkType.Idle;
                    break;

                case WorkType.DropOff:
                    city.dropOffItem(carry, out ItemResource convert1, out ItemResource convert2);
                    carry = ItemResource.Empty;

                    if (visualUnit)
                    {
                        Vector3 pos = VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(subTileEnd), 0.08f);
                        /*new ResourceEffect*/
                        SpriteText3D.GetOrCreate().init(convert1.type, convert1.amount, pos, ResourceEffectType.Add);
                        if (convert2.amount > 0)
                        {
                            /*new ResourceEffect*/
                            SpriteText3D.GetOrCreate().init(convert2.type, convert2.amount, VectorExt.AddY(pos, 0.08f), ResourceEffectType.Add);
                        }
                    }


                    gainXp = WorkExperienceType.Transport;
                    break;

                case WorkType.LocalTrade:
                    ItemResourceType tradeForItem = (ItemResourceType)workSubType;
                    var toCity = DssRef.world.tileGrid.Get(subTileEnd / WorldData.TileSubDivitions).City();
                    int payment = carry.amount;
                    ItemResource recieved = toCity.MakeTrade(tradeForItem, payment);

                    if (city.factionIndex != toCity.factionIndex)
                    {
                        faction.CityTradeImportCounting += payment;
                        toCity.GetFaction().CityTradeExportCounting += payment;
                    }

                    carry = recieved;
                    break;

                case WorkType.Mine:
                    {
                        Resource.ItemResourceType resourceType = ItemResourceType.NONE;
                        int amount = TerrainContent.DefaultMineAmount;

                        if (subTile.mainTerrain == TerrainMainType.Mine)
                        {

                            var mineType = (TerrainMineType)subTile.subTerrain;

                            switch (mineType)
                            {
                                case TerrainMineType.IronOre:
                                    resourceType = ItemResourceType.IronOre_G;
                                    break;
                                case TerrainMineType.Salt:
                                    resourceType = ItemResourceType.Salt;
                                    break;
                                case TerrainMineType.StoneBlock:
                                    resourceType = ItemResourceType.Brick;
                                    break;
                                case TerrainMineType.TinOre:
                                    resourceType = ItemResourceType.TinOre;
                                    break;
                                case TerrainMineType.CopperOre:
                                    resourceType = ItemResourceType.CopperOre;
                                    break;
                                case TerrainMineType.LeadOre:
                                    resourceType = ItemResourceType.LeadOre;
                                    break;
                                case TerrainMineType.Sulfur:
                                    resourceType = ItemResourceType.Sulfur;
                                    break;
                                case TerrainMineType.SilverOre:
                                    resourceType = ItemResourceType.SilverOre;
                                    break;

                                case TerrainMineType.Coal:
                                    resourceType = ItemResourceType.Coal;
                                    amount = TerrainContent.MineAmount_Coal;
                                    break;
                                case TerrainMineType.GoldOre:
                                    resourceType = ItemResourceType.GoldOre;
                                    break;

                                case TerrainMineType.Mithril:
                                    resourceType = ItemResourceType.RawMithril;
                                    break;
                            }


                            if (city.cityCulture == CityCulture.Miners)
                            {
                                amount *= 2;
                            }
                        }
                        else
                        {
                            resourceType = ItemResourceType.Salt;
                            amount = TerrainContent.DryingSaltAmount;
                        }

                        carry = new ItemResource(
                            resourceType,
                            subTile.terrainQuality,
                            Convert.ToInt32(processTimeLengthSec),
                            amount);

                        gainXp = WorkExperienceType.Mining;
                    }
                    break;
                

                    //    ItemResourceType item = (ItemResourceType)workSubType;
                    //    ItemPropertyColl.Blueprint(item, out var bp1, out var bp2);

                    //    bool alwaysNeedMore = false;
                    //    int add = 0;
                    //    if (bp2 != null)
                    //    { //Secondary blueprint has priority
                    //        add = bp2.tryPayResources(city);
                    //    }
                    //    if (add == 0)
                    //    {
                    //        add = bp1.payResources(city);
                    //    }
                    //    gainXp = bp1.experienceType;
                        

                    //    if (add > 0)
                    //    {
                    //        switch (item)
                    //        {
                    //            case ItemResourceType.Food_G:
                    //                city.foodProduction.add(add);
                    //                break;

                    //            case ItemResourceType.Fuel_G:
                    //            case ItemResourceType.Coal:
                    //                item = ItemResourceType.Fuel_G;
                    //                if (city.Culture == CityCulture.PitMasters)
                    //                {
                    //                    add *= 2;
                    //                }
                    //                break;


                    //            case ItemResourceType.Iron_G:
                    //            case ItemResourceType.Copper:
                    //            case ItemResourceType.Tin:
                    //            case ItemResourceType.Lead:
                    //            case ItemResourceType.Silver:
                    //            case ItemResourceType.RawMithril:
                    //                if (city.Culture == CityCulture.Smelters)
                    //                {
                    //                    add *= 2;
                    //                }
                    //                break;
                    //            case ItemResourceType.Beer:
                    //                if (city.Culture == CityCulture.Brewmaster)
                    //                {
                    //                    add += add / 2;
                    //                }
                    //                break;

                    //            case ItemResourceType.PaddedArmor:
                    //            case ItemResourceType.HeavyPaddedArmor:
                    //                if (city.Culture == CityCulture.Weavers)
                    //                {
                    //                    add += 1;
                    //                }
                    //                break;

                    //            case ItemResourceType.IronArmor:
                    //            case ItemResourceType.HeavyIronArmor:
                    //            case ItemResourceType.LightPlateArmor:
                    //            case ItemResourceType.FullPlateArmor:
                    //                if (city.Culture == CityCulture.Armorsmith)
                    //                {
                    //                    add += 1;
                    //                }
                    //                break;
                    //            case ItemResourceType.Bronze:
                    //            case ItemResourceType.BronzeSword:
                                
                    //                if (city.Culture == CityCulture.BronzeCasters)
                    //                {
                    //                    add *= 2;
                    //                }
                    //                break;

                    //            case ItemResourceType.BronzeArmor:
                    //                if (city.Culture == CityCulture.Armorsmith ||
                    //                    city.Culture == CityCulture.BronzeCasters)
                    //                {
                    //                    add += 1;
                    //                }
                    //                break;

                    //            case ItemResourceType.Gold:
                    //            case ItemResourceType.CopperCoin:
                    //            case ItemResourceType.BronzeCoin:
                    //            case ItemResourceType.SilverCoin:
                    //            case ItemResourceType.ElfCoin:
                    //                alwaysNeedMore = true;
                    //                break;

                    //            case ItemResourceType.TwoHandSword:
                    //                lib.DoNothing();
                    //                break;
                    //        }

                    //        city.AddGroupedResource(item, add);

                    //        tryRepeatWork = false;

                    //        if (alwaysNeedMore || city.GetGroupedResource(item).needMore())
                    //        {
                    //            if (bp1.hasResources(city))
                    //            {
                    //                tryRepeatWork = true;
                    //            }
                    //            else if (bp2 != null && bp2.hasResources(city))
                    //            {
                    //                tryRepeatWork = true;
                    //            }
                    //        }

                    //        if (visualUnit)
                    //        {
                    //            /*new ResourceEffect*/SpriteText3D.GetOrCreate().init(item, add, VectorExt.AddY(WP.SubtileToWorldPosXZgroundY_Centered(subTileEnd), 0.08f), ResourceEffectType.Add);
                    //        }
                    //    }
                    //}
                    //break;

                case WorkType.Upgrade:
                case WorkType.Build:
#if DEBUG
                    if (BuildLib.BuildOptions[workSubType].buildType == BuildAndExpandType.OrchardApple)
                    {
                        lib.DoNothing();
                    }
#endif
                    if (orderIsActive(city))
                    {
                        bool upgrade = work == WorkType.Upgrade;
                        var build = BuildLib.BuildOptions[workSubType];
                        if (build.execute_async(city, subTileEnd, ref subTile, upgrade))
                        {

                            EditSubTile edit = new EditSubTile(subTileEnd, subTile, true, true, false);
                            edit.Submit();
                        }
                        gainXp = build.experienceType();
                    }
                    
                    break;
                case WorkType.School:
                    setExperience((WorkExperienceType)workSubType, workBonus);
                    city.onSchoolComplete_async(subTileEnd);
                    work = WorkType.Idle;
                    processTimeStartStampSec = Ref.TotalGameTimeSec;

                    return;

                case WorkType.Demolish:
                    {
                        if (orderIsActive(city))
                        {
                            BuildLib.Demolish(city, subTileEnd);

                            gainXp = WorkExperienceType.HouseBuilding;
                        }
                    }
                    break;
                case WorkType.Starving:
                case WorkType.Exit:
                    //work = WorkType.IsDeleted;
                    DeleteMe();
                    break;
            }

            addExperience(gainXp, city);

            if (tryRepeatWork && energy > 0)
            {
                processTimeLengthSec = finalizeWorkTime(city);
                subTileStart = subTileEnd;
            }
            else if (work != WorkType.IsDeleted)
            {
                work = WorkType.Idle;

                if (orderId >= 0)
                {
                    faction.player.orders?.CompleteOrderId(orderId);
                }
            }

            processTimeStartStampSec = Ref.TotalGameTimeSec;

        }

        public WorkExperience getXpFor(XP.WorkExperienceType type)
        {
            if (XpEntityIndex >= 0)
            {
                return DssRef.world.GetWorkXp(XpEntityIndex, type);
            }
            return WorkExperience.Empty;
        }

        public int GetXpScore()
        {
            return DssRef.world.GetWorkXpScore(XpEntityIndex);           
        }

        public void setXpFor(XP.WorkExperienceType type, byte toXp)
        {
            DssRef.world.SetWorkXp(XpEntityIndex, type, toXp);
            
        }

        public void addExperience(XP.WorkExperienceType type, City city, byte add = 0)
        {
            int entityIx_sp = XpEntityIndex;
            if (type == WorkExperienceType.NUM_NONE || entityIx_sp < 0)
            {
                return;
            }
            ref var xp = ref DssRef.world.GetRefWorkXp(entityIx_sp, type);

            ExperienceLevel level = xp.Level();

            if (add == 0)
            {
                switch (level)
                {
                    case ExperienceLevel.Beginner_1:
                        add = WorkLib.WorkToXPTable[(int)type];
                        add += 5;
                        break;

                    case ExperienceLevel.Practitioner_2:
                        add = WorkLib.WorkToXPTable[(int)type];
                        break;

                    case ExperienceLevel.Expert_3:
                        //expert = true;
                        if (Ref.peRnd.Chance(0.5))
                        {
                            add = WorkLib.WorkToXPTable[(int)type];
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case ExperienceLevel.Master_4:
                        //master = true;
                        if (Ref.peRnd.Chance(0.1))
                        {
                            add = WorkLib.WorkToXPTable[(int)type];
                        }
                        else
                        {
                            return;
                        }
                        break;
                    case ExperienceLevel.Legendary_5:
                        return;
                }
            }
            xp.xp += add;
            ExperienceLevel nextlevel = xp.Level();
            if (nextlevel > level)
            {
                //Level up
                city.addTechPoints(type,
                    level >= ExperienceLevel.Master_4 ? DssConst.TechnologyGain_MasterLevelUp : DssConst.TechnologyGain_AnyLevelUp,
                    TechnologyGainReason.WorkerLevel);
            }

            //if (type != XP.WorkExperienceType.NUM_NONE)
            //{
            //    if (type == xpType1)
            //    {
            //        addTo(ref type, ref xp1, add);
            //    }
            //    else if (type == xpType2)
            //    {
            //        addTo(ref type, ref xp2, add);
            //    }
            //    else if (type == xpType3)
            //    {
            //        addTo(ref type, ref xp3, add);
            //    }
            //    else
            //    {
            //        int lowIx = 0;
            //        int lowVal = xp1;

            //        if (xp2 < lowVal)
            //        { 
            //            lowIx = 1;
            //            lowVal = xp2;
            //        }
            //        if (xp3 < lowVal)
            //        {
            //            lowIx = 2;
            //            lowVal = xp3;
            //        }

            //        switch (lowIx)
            //        {
            //            case 0:
            //                if (shouldReplace(xpType1, xp1, type))
            //                {
            //                    xpType1 = type;
            //                    xp1 = 0;
            //                    addTo(ref type, ref xp1, add);
            //                }
            //                break;
            //            case 1:
            //                if (shouldReplace(xpType1, xp2, type))
            //                {
            //                    xpType2 = type;
            //                    xp2 = 0;
            //                    addTo(ref type, ref xp2, add);
            //                }
            //                break;
            //            case 2:
            //                if (shouldReplace(xpType1, xp3, type))
            //                {
            //                    xpType3 = type;
            //                    xp3 = 0;
            //                    addTo(ref type, ref xp3, add);
            //                }
            //                break;
            //        }

            //        bool shouldReplace(WorkExperienceType previous, byte previousXp, WorkExperienceType newXp)
            //        {
            //            if (previous == WorkExperienceType.NUM_NONE)
            //            {
            //                return true;
            //            }
            //            if (newXp == WorkExperienceType.Transport)
            //            {
            //                return false;
            //            }

            //            if (previous == WorkExperienceType.HouseBuilding &&
            //                skillPriority(newXp) <= 1)
            //            {
            //                //Extra protection for building skill
            //                return false;
            //            }

            //            //Expert levels cannot be replaced with low priority skills (skills that are never required like animal care)
            //            return previousXp < DssConst.WorkLevel_Expert || skillPriority(newXp) >= skillPriority(previous);
            //        }

            //        int skillPriority(WorkExperienceType experienceType)
            //        {
            //            switch (experienceType)
            //            {
            //                case WorkExperienceType.Transport:
            //                    return 0;

            //                case WorkExperienceType.AnimalCare:
            //                case WorkExperienceType.Cook:
            //                case WorkExperienceType.Farm:
            //                case WorkExperienceType.Mining:
            //                case WorkExperienceType.StoneCutter:
            //                    return 1;

            //                default: 
            //                    return 2;

            //                case WorkExperienceType.HouseBuilding:
            //                    return 3;
            //            }
            //        }
            //    }
            //}



            //void addTo(ref XP.WorkExperienceType type, ref byte xp, byte add = 0)
            //{
            //    //bool expert = false;
            //    //bool master = false;
            //    ExperienceLevel level = XpLib.ToLevel(xp);

            //    if (add == 0)
            //    {
            //        switch (level)
            //        {
            //            case ExperienceLevel.Beginner_1:
            //                add = WorkLib.WorkToXPTable[(int)type];
            //                add += 2;
            //                break;
            //            case ExperienceLevel.Practitioner_2:
            //                add = WorkLib.WorkToXPTable[(int)type];
            //                break;
            //            case ExperienceLevel.Expert_3:
            //                //expert = true;
            //                if (Ref.peRnd.Chance(0.5))
            //                {
            //                    add = WorkLib.WorkToXPTable[(int)type];
            //                }
            //                else
            //                {
            //                    return;
            //                }
            //                break;
            //            case ExperienceLevel.Master_4:
            //                //master = true;
            //                if (Ref.peRnd.Chance(0.1))
            //                {
            //                    add = WorkLib.WorkToXPTable[(int)type];
            //                }
            //                else
            //                {
            //                    return;
            //                }
            //                break;
            //            case ExperienceLevel.Legendary_5:
            //                return;
            //        }
            //    }
            //    xp += add;
            //    ExperienceLevel nextlevel = XpLib.ToLevel(xp);
            //    if (nextlevel > level)
            //    {
            //        //Level up
            //        city.addTechPoints(type, 
            //            level>= ExperienceLevel.Master_4 ? DssConst.TechnologyGain_MasterLevelUp : DssConst.TechnologyGain_AnyLevelUp, 
            //            TechnologyGainReason.WorkerLevel);
            //    }
            //}
        }

        void setExperience(XP.WorkExperienceType type, int toLevel)
        {
            if (type != XP.WorkExperienceType.NUM_NONE)
            {
                
                ref var xp = ref DssRef.world.GetRefWorkXp(XpEntityIndex, type);
                xp.setLevel(toLevel);
                
            }



            //void setTo(ref XP.WorkExperienceType type, ref byte xp)
            //{
            //    xp = (byte)(toLevel * DssConst.WorkXpToLevel);
            ////    bool master = false;

            //    //    switch (XpLib.ToLevel(xp))
            //    //    {
            //    //        case ExperienceLevel.Beginner_1:
            //    //            add = WorkLib.WorkToXPTable[(int)type];
            //    //            add += 1;
            //    //            break;
            //    //        case ExperienceLevel.Expert_3:
            //    //            if (Ref.rnd.Chance(0.5))
            //    //            {
            //    //                add = WorkLib.WorkToXPTable[(int)type];
            //    //            }
            //    //            break;
            //    //        case ExperienceLevel.Master_4:
            //    //            master = true;
            //    //            if (Ref.rnd.Chance(0.1))
            //    //            {
            //    //                add = WorkLib.WorkToXPTable[(int)type];
            //    //            }
            //    //            break;
            //    //        case ExperienceLevel.Legendary_5:
            //    //            //add = 0;
            //    //            break;
            //    //    }
            //    //    xp += add;
            //    //    if (xp >= DssConst.WorkLevel_Master &&
            //    //        !master)
            //    //    {
            //    //        city.onMasterLevel(type);
            //    //    }
            //}
        }

        public void cancelWork()
        {
            work = WorkType.Idle;
            processTimeStartStampSec = Ref.TotalGameTimeSec;
        }

        public bool orderIsActive(City city)
        {
            if (orderId >= 0)
            {
                if (city.GetPlayer().orders != null)
                {
                    return city.GetPlayer().orders.GetFromId(orderId) != null;
                }
            }

            return true;

        }

        public void WorkComplete(AbsMapObject mapObject, bool visualUnit)
        {
            switch (mapObject.gameobjectType())
            {
                case GameObjectType.City:
                    workComplete(mapObject.GetCity(), visualUnit);
                    break;

                case GameObjectType.Army:
                    workComplete(mapObject.GetArmy());
                    break;
            }
        }

        void gatherWood(Resource.ItemResourceType resourceType, ref SubTile subTile, City city)
        {
            int amount = subTile.terrainAmount;

            if (workBonus > 0)
            {
                amount = MathExt.AddPercentage(amount, workBonus);
            }

            if (city.cityCulture == CityCulture.Woodcutters)
            {
                amount *= 2;
            }

            carry = new Resource.ItemResource(
                resourceType,
                subTile.terrainQuality,
                Convert.ToInt32(processTimeLengthSec),
                Resource.ItemPropertyColl.Get(resourceType).carryCount);

            DssRef.state.resources.addItem(
                new Resource.ItemResource(
                    resourceType,
                    subTile.terrainQuality,
                    Convert.ToInt32(processTimeLengthSec),
                    amount),
                ref subTile.collectionPointer);

            subTile.SetType(TerrainMainType.Resourses, (int)TerrainResourcesType.Wood, 1);
            EditSubTile editSubTile = new EditSubTile(subTileEnd, subTile, true, true, true);
            editSubTile.Submit();
            //DssRef.world.subTileGrid.Set(subTileEnd, subTile);
        }


        public void createWorkOrder(WorkType work, int subWork, byte workBonus, XP.WorkExperienceType experienceType, int order, IntVector2 targetSubTile, City city)
        {
            this.workBonus = workBonus;
            this.work = work;
            workSubType = subWork;
            orderId = order;
            subTileStart = subTileEnd;
            subTileEnd = targetSubTile;
            processTimeStartStampSec = Ref.TotalGameTimeSec;
            float dist = VectorExt.Length(subTileEnd.X - subTileStart.X, subTileEnd.Y - subTileStart.Y) / WorldData.TileSubDivitions; //Convrst to WP length

            processTimeLengthSec = finalizeWorkTime(experienceType, city) +
                dist / DssVar.Men_StandardWalkingSpeed_PerSec;

            switch (work)
            {

                case WorkType.LocalTrade:
                    {
                        ItemResourceType tradeForItem = (ItemResourceType)workSubType;
                        var toCity = DssRef.world.tileGrid.Get(targetSubTile / WorldData.TileSubDivitions).City();
                        int goldCost = tradeForItem == ItemResourceType.ConservedFood? DssConst.ConservedFoodGoldValue : DssConst.FoodGoldValue;// toCity.SellCost(tradeForItem);

                        carry = new ItemResource(ItemResourceType.Gold, 1, 1, goldCost * DssConst.Worker_TrossWorkerCarryWeight);
                    }
                    break;

                case WorkType.TrossCityTrade:
                    {
                        var toCity = DssRef.world.tileGrid.Get(targetSubTile / WorldData.TileSubDivitions).City();
                        int goldCost = DssConst.FoodGoldValue;//toCity.SellCost(ItemResourceType.Food_G);

                        carry = new ItemResource(ItemResourceType.Gold, 1, 1, goldCost);
                    }
                    break;
            }
        }

        public float finalizeWorkTime(City city)
        {
            return finalizeWorkTime(WorkLib.WorkToExperienceType(work, workSubType, workBonus, subTileEnd, city, out _, out _, out _), city);
        }


        public float finalizeWorkTime(XP.WorkExperienceType experienceType, City city)
        {
            float timeSec;

            switch (work)
            {
                case WorkType.Idle:
                    return 5;
                case WorkType.Eat:
                    return DssConst.WorkTime_Eat;
                case WorkType.PickUpResource:
                    timeSec = DssConst.WorkTime_PickUpResource;
                    break;
                case WorkType.PickUpProduce:
                    timeSec = DssConst.WorkTime_PickUpProduce;
                    break;
                case WorkType.TrossCityTrade:
                    timeSec = DssConst.WorkTime_TrossCityTrade;
                    break;
                case WorkType.LocalTrade:
                    timeSec = DssConst.WorkTime_LocalTrade;
                    break;
                case WorkType.GatherFoil:
                    SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);
                    switch ((TerrainSubFoilType)subTile.subTerrain)
                    {
                        case TerrainSubFoilType.TreeSoft:
                            timeSec = DssConst.WorkTime_GatherFoil_TreeSoft;
                            break;
                        case TerrainSubFoilType.TreeHard:
                            timeSec = DssConst.WorkTime_GatherFoil_TreeHard;
                            break;
                        case TerrainSubFoilType.DryWood:
                            timeSec = DssConst.WorkTime_GatherFoil_DryWood;
                            break;

                        case TerrainSubFoilType.TreeApple:
                        case TerrainSubFoilType.TreeBanana:
                            timeSec = DssConst.WorkTime_PluckOrchards;
                            break;

                        case TerrainSubFoilType.WheatFarm:
                        case TerrainSubFoilType.WheatFarmUpgraded:
                        case TerrainSubFoilType.LinenFarm:
                        case TerrainSubFoilType.LinenFarmUpgraded:
                        case TerrainSubFoilType.RapeSeedFarm:
                        case TerrainSubFoilType.RapeSeedFarmUpgraded:
                        case TerrainSubFoilType.HempFarm:
                        case TerrainSubFoilType.HempFarmUpgraded:
                            timeSec = DssConst.WorkTime_GatherFoil_FarmCulture;
                            break;
                        case TerrainSubFoilType.Stones:
                        case TerrainSubFoilType.StoneBlock:
                            timeSec = DssConst.WorkTime_GatherFoil_Stones;
                            break;

                        case TerrainSubFoilType.BogIron:
                            timeSec = DssConst.WorkTime_BogIron;
                            break;
                        case TerrainSubFoilType.ClayPit:
                            timeSec = DssConst.WorkTime_ClayPit;
                            break;
                        default:
                            return -1;//throw new NotImplementedException();
                            
                    }
                    break;
                //case WorkType.Till:
                //    time = DssConst.WorkTime_Till;
                    //break;
                case WorkType.Plant:
                    if (workBonus == 0)
                    {
                        timeSec = DssConst.WorkTime_Plant;
                    }
                    else
                    { 
                        timeSec = DssConst.WorkTime_Plant_Upgraded;
                    }
                    break;
                case WorkType.Mine:
                    timeSec = DssConst.WorkTime_Mine;
                    break;
                case WorkType.Craft:
                    timeSec = DssConst.WorkTime_Craft * DssRef.difficulty.setting_craftMulti;
                    break;

                case WorkType.Build:
                    timeSec = BuildLib.BuildOptions[workSubType].buildTimeSec;
                    //timeSec = DssConst.WorkTime_Building;

                    if (city.cityCulture == CityCulture.Builders)
                    {
                        timeSec *= 0.5f;
                    }
                    break;
                case WorkType.Upgrade:
                    timeSec = DssConst.WorkTime_UpgradeBuilding;
                    break;
                case WorkType.Demolish:
                    return DssConst.WorkTime_Demolish;

                case WorkType.TrossReturnToArmy:
                case WorkType.DropOff:
                case WorkType.Exit:
                case WorkType.Starving:
                    return 1f;

                case WorkType.School:
#if DEBUG
                    if (StartupSettings.UnlockAllProgress)
                        return 1f;
#endif

                    int toXp = workBonus * DssConst.WorkXpToLevel;
                    int diff = toXp - getXpFor(experienceType).xp;
                    return diff * DssConst.Time_SchoolOneXPSec;
                    //lock (city.schoolBuildings)
                    //{
                    //    var ix = city.SchoolIxFromSubTile(subTileEnd);
                    //    if (arraylib.TryGet(city.schoolBuildings, ix, out SchoolStatus status))
                    //    {
                    //        int toXp = (int)status.toLevel * DssConst.WorkXpToLevel;
                    //    }
                    //}

                default:
                    throw new NotImplementedException();
            }

            timeSec *= WorkLib.WorkTimePerc(getXpFor(experienceType).xp, workBonus);
            return timeSec;
        }
    }
}

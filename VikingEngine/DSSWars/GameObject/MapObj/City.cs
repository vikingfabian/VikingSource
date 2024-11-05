using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Resource;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : GameObject.AbsMapObject
    {
        public const int ExpandGuardSizeCost = 12000;

        //public int index;
        public int areaSize = 0;
        public CityType CityType;
        public List<int> neighborCities = new List<int>();

        Graphics.AbsVoxelObj overviewModel;

        BoundingBox bound;

        //public int totalIncome;

        //float upkeep;
        public int maxGuardSize;
        public int guardCount;

        public FloatingInt childrenAge0 = new FloatingInt();
        public int childrenAge1 = 0;

        public int workForce = 0;
        public int workForceMax = 0;

        //public int maxEpandWorkSize;
        public FloatingInt damages = new FloatingInt();
        public FloatingInt immigrants = new FloatingInt();
        const double ImmigrantsRemovePerSec = 0.1;
        //double workForceAddPerSec;
        public int workHutStyle = 0;
        public int mercenaries = 0;

        public CityDetail detailObj;
        //public List<CityPurchaseOption> cityPurchaseOptions;

        public float ai_armyDefenceValue = 0;
        //public bool nobelHouse = false;
        public bool hasBuilding_carpenter = false;
        public bool hasBuilding_brewery = false;
        public bool hasBuilding_smith = false;
        public int coalpit_buildingCount = 0;
        public int nobelHouse_buildingCount = 0;
        string name = null;

        IntVector2 cullingTopLeft, cullingBottomRight;
        public int cityTileRadius = 0;
        public CityCulture Culture = CityCulture.NUM_NONE;

        public Build.BuildAndExpandType autoExpandFarmType = Build.BuildAndExpandType.WheatFarm;
        bool autoBuild_Work = false;
        bool autoBuild_Farm = false;

        public CityTagBack tagBack = CityTagBack.NONE;
        public CityTagArt tagArt = CityTagArt.None;

        public void AutoExpandType(out bool work, out Build.BuildAndExpandType farm)
        {
            work = autoBuild_Work;
            farm = autoBuild_Farm ? autoExpandFarmType : Build.BuildAndExpandType.NUM_NONE;
        }

        public void haltConscriptAndDelivery()
        {
            for (int i = 0; i < conscriptBuildings.Count; i++)
            {
                BarracksStatus status = conscriptBuildings[i];
                status.halt(this);
                conscriptBuildings[i] = status;
            }

            for (int i = 0; i < deliveryServices.Count; i++)
            {
                var delivery = deliveryServices[i];
                delivery.halt();
                deliveryServices[i] = delivery;
            }
        }

        public bool AutoBuildWorkProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoBuild_Work = value;
                (value? SoundLib.click : SoundLib.back).Play();
            }
            return autoBuild_Work;
        }
        public bool AutoBuildFarmProperty(int index, bool set, bool value)
        {
            if (set)
            {
                autoBuild_Farm = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return autoBuild_Farm;
        }

        public City(int index, IntVector2 pos, CityType type, WorldData world)
        {
            this.parentArrayIndex = index;

            this.tilePos = pos;
            this.CityType = type;
        }

        public City(int index, System.IO.BinaryReader r, int version)
        {
            this.parentArrayIndex = index;
            readMapFile(r, version);
        }

        public void generateCultureAndEconomy(WorldData world, CityCultureCollection cityCultureCollection)
        {
            initEconomy(true);

            double land = 0, water = 0, plain = 0, forest = 0, mountain = 0, dryBiom = 0;

            Rectangle2 cultureArea = Rectangle2.FromCenterTileAndRadius(tilePos, 3);
            double total = cultureArea.Area;
            ForXYLoop loop = new ForXYLoop(cultureArea);

            while (loop.Next())
            {
                var tile = world.tileGrid.Get(loop.Position);
                if (tile.IsWater())
                {
                    ++water;
                }
                else
                {
                    ++land;
                    switch (tile.heightSett().culture)
                    {
                        case TerrainCultureType.Plains:
                            ++plain;
                            break;
                        case TerrainCultureType.Forest:
                            ++forest;
                            break;
                        case TerrainCultureType.Mountain:
                            ++mountain;
                            break;
                    }
                    if (tile.biom == BiomType.YellowDry || tile.biom == BiomType.RedDry)
                    {
                        ++dryBiom;
                    }
                }
            }

            double percWater = water / total;
            double percForest = forest / land;
            double percPlains = plain / land;
            double percMountain = mountain / land;
            double percDry = dryBiom / land;

            workHutStyle = percMountain > 0.5 ? 0 : 1;


            //Collect cultures
            double percX = tilePos.X / (double)world.Size.X;
            double percY = tilePos.Y / (double)world.Size.Y;

            if (percForest >= 0.7 && CityType == CityType.Head)
            {
                cityCultureCollection.LargeGreen.Add(this);
            }
            else if (percDry >= 0.7 && percX >= 0.75)
            {
                cityCultureCollection.DryEast.Add(this);
            }
            else if (percWater >= 0.25 && percY <= 0.25)
            {
                cityCultureCollection.NorthSea.Add(this);
            }            

            if (world.rnd.Chance(0.3))
            {
                //Area specific culture
                if (percDry > 0.05 && percDry < 0.7 && percPlains >= 0.1)
                {
                    Culture = CityCulture.FertileGround;
                }
                else if (percForest >= 0.8)
                {
                    Culture = CityCulture.Woodcutters;
                }
                else if (percMountain > 0.5)
                {
                    Culture = CityCulture.Miners;
                }
                else if (percMountain > 0.3)
                {
                    Culture = CityCulture.Stonemason;
                }
                else if (dryBiom <= 1)
                {
                    Culture = CityCulture.DeepWell;
                }
                else if (percForest >= 0.1)
                {
                    Culture = CityCulture.PitMasters;
                }
                else if (percWater >= 0.25)
                {
                    Culture = CityCulture.Seafaring;
                }
            }

            if (Culture == CityCulture.NUM_NONE)
            {
                Culture = arraylib.RandomListMember(CityCultureCollection.GeneralCultures, world.rnd); 
            }
        }

        public void writeMapFile(System.IO.BinaryWriter w)
        {
            tilePos.writeUshort(w);

            w.Write(Debug.Byte_OrCrash((int)CityType));
            w.Write(Debug.Ushort_OrCrash(areaSize));
            w.Write(Debug.Byte_OrCrash(cityTileRadius));
            w.Write(Debug.Byte_OrCrash(workHutStyle));

            w.Write(Debug.Byte_OrCrash(neighborCities.Count));
            foreach (var n in neighborCities)
            {
                w.Write(Debug.Ushort_OrCrash(n));
            }


            w.Write(Debug.Byte_OrCrash((int)Culture));
        }

        public void readMapFile(System.IO.BinaryReader r, int version)
        {
            tilePos.readUshort(r);

            CityType = (CityType)r.ReadByte();
            areaSize = r.ReadUInt16();
            if (version >= 5)
            {
                cityTileRadius = r.ReadByte();
            }
            workHutStyle = r.ReadByte();

            int neighborCitiesCount = r.ReadByte();
            for (int i = 0; i < neighborCitiesCount; i++)
            {
                neighborCities.Add(r.ReadUInt16());
            }

            if (version >= 6)
            {
                Culture = (CityCulture)r.ReadByte();
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(Convert.ToUInt16(workForce));
            w.Write(Convert.ToUInt16(workForceMax));
            childrenAge0.write16bit(w);
            w.Write((ushort)childrenAge1);

            damages.write16bit(w);
            immigrants.write16bit(w);
            w.Write((ushort)guardCount);
            w.Write((ushort)maxGuardSize);

            w.Write((byte)maxWater);
            w.Write(Debug.Byte_OrCrash((int)(waterAddPerSec * 20)));
            workTemplate.writeGameState(w, true);

            w.Write((short)res_water.amount);


            res_wood.writeGameState(w); //ItemResourceType.Wood_Group,
            res_fuel.writeGameState(w); // ItemResourceType.Fuel_G,
            res_stone.writeGameState(w); // ItemResourceType.Stone_G,
            res_rawFood.writeGameState(w); // ItemResourceType.RawFood_Group,
            res_food.writeGameState(w); // ItemResourceType.Food_G,
            res_beer.writeGameState(w); // ItemResourceType.Beer,
            res_skinLinnen.writeGameState(w); // ItemResourceType.SkinLinen_Group,
            res_ironore.writeGameState(w); // ItemResourceType.IronOre_G,
            res_iron.writeGameState(w); // ItemResourceType.Iron_G,

            res_sword.writeGameState(w); // ItemResourceType.Sword,
            res_sharpstick.writeGameState(w); // ItemResourceType.SharpStick,
            res_twohandsword.writeGameState(w); // ItemResourceType.TwoHandSword,
            res_knightslance.writeGameState(w); // ItemResourceType.KnightsLance,
            res_bow.writeGameState(w); // ItemResourceType.Bow,
            res_ballista.writeGameState(w); // ItemResourceType.Ballista,            

            res_lightArmor.writeGameState(w); // ItemResourceType.LightArmor,
            res_mediumArmor.writeGameState(w); // ItemResourceType.MediumArmor,
            res_heavyArmor.writeGameState(w); // ItemResourceType.HeavyArmor,

            res_longbow.writeGameState(w);
            //foreach (var type in MovableCityResourceTypes)
            //{
            //    GetGroupedResource(type).writeGameState(w);
            //}

            w.Write((ushort)conscriptBuildings.Count);
            foreach (var barracks in conscriptBuildings)
            { 
                barracks.writeGameState(w);
            }

            w.Write((ushort)deliveryServices.Count);
            foreach (var delivery in deliveryServices)
            { 
                delivery.writeGameState(w);
            }
            
            w.Write(autoBuild_Work);
            w.Write(autoBuild_Farm);
            w.Write((byte)autoExpandFarmType);

            w.Write((byte)tagBack);
            if (tagBack != CityTagBack.NONE)
            {
                w.Write((ushort)tagArt);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            workForce = r.ReadUInt16();
            workForceMax = r.ReadUInt16();
            childrenAge0.read16bit(r);
            childrenAge1 = r.ReadUInt16();

            damages.read16bit(r);
            immigrants.read16bit(r);

            guardCount = r.ReadUInt16();
            maxGuardSize = r.ReadUInt16();

            maxWater = r.ReadByte();
            waterAddPerSec = r.ReadByte() / 20f;
            
            workTemplate.readGameState(r, subversion, true);

            res_water.amount = r.ReadInt16();

            res_wood.readGameState(r, subversion); //ItemResourceType.Wood_Group,
            res_fuel.readGameState(r, subversion); // ItemResourceType.Fuel_G,
            res_stone.readGameState(r, subversion); // ItemResourceType.Stone_G,
            res_rawFood.readGameState(r, subversion); // ItemResourceType.RawFood_Group,
            res_food.readGameState(r, subversion); // ItemResourceType.Food_G,
            res_beer.readGameState(r, subversion); // ItemResourceType.Beer,
            res_skinLinnen.readGameState(r, subversion); // ItemResourceType.SkinLinen_Group,
            res_ironore.readGameState(r, subversion); // ItemResourceType.IronOre_G,
            res_iron.readGameState(r, subversion); // ItemResourceType.Iron_G,

            res_sword.readGameState(r, subversion); // ItemResourceType.Sword,
            res_sharpstick.readGameState(r, subversion); // ItemResourceType.SharpStick,
            res_twohandsword.readGameState(r, subversion); // ItemResourceType.TwoHandSword,
            res_knightslance.readGameState(r, subversion); // ItemResourceType.KnightsLance,
            res_bow.readGameState(r, subversion); // ItemResourceType.Bow,
            res_ballista.readGameState(r, subversion); // ItemResourceType.Ballista,            

            res_lightArmor.readGameState(r, subversion); // ItemResourceType.LightArmor,
            res_mediumArmor.readGameState(r, subversion); // ItemResourceType.MediumArmor,
            res_heavyArmor.readGameState(r, subversion); // ItemResourceType.HeavyArmor,

            //if (subversion >= 19 && subversion != SaveGamestate.MergeVersion)
            //{
                res_longbow.readGameState(r, subversion);
            //}
            //foreach (var type in MovableCityResourceTypes)
            //{
            //    GroupedResource resource = new GroupedResource();
            //    resource.readGameState(r, subversion);
            //    SetGroupedResource(type, resource);
            //}

            refreshCitySize();

            conscriptBuildings.Clear();
            int conscriptBuildingsCount = r.ReadUInt16();
            for (int i = 0; i < conscriptBuildingsCount; i++)
            {
                var barrack = new Conscript.BarracksStatus();
                barrack.readGameState(r, subversion);
                conscriptBuildings.Add(barrack);
            }

            deliveryServices.Clear();
            int deliveryServicesCount = r.ReadUInt16();
            for (int i = 0; i < deliveryServicesCount; i++)
            {
                DeliveryStatus status = new Delivery.DeliveryStatus();
                status.readGameState(r, subversion);
                deliveryServices.Add(status);
            }

            autoBuild_Work = r.ReadBoolean();
            autoBuild_Farm = r.ReadBoolean();
            autoExpandFarmType = (Build.BuildAndExpandType)r.ReadByte();

            //if (subversion >= 23 && subversion != SaveGamestate.MergeVersion)
            //{
            tagBack = (CityTagBack)r.ReadByte();
            if (tagBack != CityTagBack.NONE)
            {
                tagArt = (CityTagArt)r.ReadUInt16();
            }
            //}
        }

        public void writeNet(System.IO.BinaryWriter w)
        {

        }
        public void readNet(System.IO.BinaryReader r)
        {

        }

        override public void tagSprites(out SpriteName back, out SpriteName art)
        {
            back = Data.CityTag.BackSprite(tagBack);
            art = Data.CityTag.ArtSprite(tagArt);
        }


        public int expandWorkForceCost()
        {
            return 40000 + workForceMax * 10;
        }

        //public bool canExpandWorkForce(int count)
        //{
        //    return (workForceMax + DssConst.ExpandWorkForce * count) <= maxEpandWorkSize;
        //}

        public bool canIncreaseGuardSize(int count)
        {
            return (maxGuardSize + DssConst.ExpandGuardSize * count) <= workForceMax;
        }

        public void expandWorkForce(int amount)
        {
            workForceMax += amount;
            refreshCitySize();
            detailObj.refreshWorkerSubtiles();//updateWorkerModels();
        }

        public void onWorkHutBuild()
        {
            workForceMax += DssConst.SoldierGroup_DefaultCount;
            refreshCitySize();
        }

        public void expandGuardSize(int amount)
        {
            maxGuardSize += amount;
            refreshCitySize();
        }

        public bool buyCityGuards(bool commit, int count)
        {
            if (canIncreaseGuardSize(count))
            {
                int totalCost = 0;

                if (faction.calcCost(ExpandGuardSizeCost * count, ref totalCost))
                {
                    if (commit)
                    {
                        expandGuardSize(DssConst.ExpandGuardSize * count);
                        faction.payMoney(totalCost, true);
                    }
                    return true;
                }
            }
            return false;
        }

        //public bool buyWorkforce(bool commit, int count)
        //{
        //    if (canExpandWorkForce(count))
        //    {
        //        int totalCost = 0;

        //        if (faction.calcCost(expandWorkForceCost() * count, ref totalCost))
        //        {
        //            if (commit)
        //            {
        //                expandWorkForce(DssConst.ExpandWorkForce * count);
        //                faction.payMoney(totalCost, true);
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public bool buyRepair(bool commit, bool all)
        {
            if (damages.HasValue())
            {
                int cost;
                int count;

                repairCountAndCost(all, out count, out cost);

                int totalCost = 0;
                if (faction.calcCost(cost, ref totalCost))
                {
                    if (commit)
                    {
                        damages.value -= count;
                        faction.payMoney(totalCost, true);
                    }
                    return true;
                }
            }
            return false;
        }

        public void burnItDown()
        {
            damages.value = MaxDamages();
            //guardCount = 0;
            workForce = 0;
        }

        public double MaxDamages()
        {
            return workForceMax * 0.75;
        }

        public void repairCountAndCost(bool all, out int count, out int cost)
        {
            const double BuyToRepair = 0.75;
            count = damages.Int();
            cost = 0;

            if (count > 0)
            {
                if (!all && count > DssConst.ExpandWorkForce)
                {
                    count = DssConst.ExpandWorkForce;
                }

                cost = Convert.ToInt32(((double)expandWorkForceCost() / DssConst.ExpandWorkForce * count) * BuyToRepair);
            }
        }

        public bool buyMercenary(bool commit, int count)
        {
            int totalCost = 0;
            int mercenariesCount = DssLib.MercenaryPurchaseCount * count;

            if (faction.calcCost(buyMercenaryCost(count), ref totalCost) &&
                faction.player.GetLocalPlayer().mercenaryMarket.Int() >= mercenariesCount)
            {
                if (commit)
                {
                    faction.payMoney(totalCost, true);
                    faction.player.GetLocalPlayer().mercenaryMarket.pay(mercenariesCount, true);
                    faction.player.GetLocalPlayer().mercenaryCost += DssRef.difficulty.MercenaryPurchaseCost_Add * count;

                    mercenaries += mercenariesCount;
                }
                return true;
            }

            return false;
        }

        public int buyMercenaryCost(int count)
        {
            double result = MathExt.SumOfLinearIncreases(faction.player.GetLocalPlayer().mercenaryCost, DssRef.difficulty.MercenaryPurchaseCost_Add, count);
            return Convert.ToInt32(result);
        }


        public int GuardUpkeep(int maxGuardSize)
        {
            return (int)(0.2f * maxGuardSize);
        }


        public void onGameStart(bool newGame)
        {
            groupRadius = 0.6f;

            initEconomy(newGame);
            CalcRecruitToTile();

            if (newGame)
            {
                maxGuardSize = workForce / 4;

                guardCount = maxGuardSize;
            }
            refreshCitySize();

            position = new Vector3(tilePos.X, Tile().ModelGroundY(), tilePos.Y);

            detailObj = new CityDetail(this, newGame);

            float iconScale = IconScale();

            VectorVolumeC volume = new VectorVolumeC(position,
                new Vector3(iconScale * 0.5f, 0.1f, iconScale * 0.5f));
            bound = volume.boundingBox();

            name = Data.NameGenerator.CityName(tilePos);
        }

        void initEconomy(bool newGame)
        {
            if (newGame)
            {
                switch (CityType)
                {
                    case CityType.Small:
                        workForceMax = DssConst.SmallCityStartMaxWorkForce;
                        waterAddPerSec = DssConst.WaterAdd_SmallCity;
                        break;
                    case CityType.Large:
                        workForceMax = DssConst.LargeCityStartMaxWorkForce;
                        waterAddPerSec = DssConst.WaterAdd_LargeCity;
                        break;
                    default:
                        workForceMax = DssConst.HeadCityStartMaxWorkForce;
                        waterAddPerSec = DssConst.WaterAdd_HeadCity;
                        break;
                }
                workForce = (int)(workForceMax * 0.75);
                waterAddPerSec += Ref.rnd.Float(DssConst.WaterAdd_RandomAdd);

                if (Culture == CityCulture.DeepWell)
                {
                    waterAddPerSec += DssConst.WaterAdd_SmallCity;
                }

                defaultResourceBuffer();
            }
        }

        void refreshCitySize()
        {
            refreshVisualSize();
        }

        void refreshVisualSize()
        {
            if (CityType != CityType.Factory)
            {
                CityType newType;

                if (workForceMax >= DssConst.HeadCityStartMaxWorkForce)
                {
                    newType = CityType.Head;
                }
                else if (workForceMax >= DssConst.LargeCityStartMaxWorkForce)
                {
                    newType = CityType.Large;
                }
                else
                {
                    newType = CityType.Small;
                }

                if (newType != CityType)
                {
                    CityType = newType;
                    //detailObj.refreshModel();
                    //Task.Factory.StartNew(() =>
                    //{
                    //    createBuildingSubtiles(DssRef.world);
                    //});

                    if (overviewModel != null)
                    {
                        overviewModel.scale = VectorExt.V3(IconScale() * overviewModel.OneBlockScale);
                    }
                }
            }
        }

        public void setFactoryType(bool set)
        {
            if (set)
            {
                if (CityType != CityType.Factory)
                {
                    CityType = CityType.Factory;

                    workForceMax += DssConst.HeadCityStartMaxWorkForce;
                    detailObj.refreshModel();

                    if (overviewModel != null)
                    {
                        overviewModel.scale = VectorExt.V3(IconScale() * overviewModel.OneBlockScale);
                    }

                    DssRef.state.events.onFactoryBuilt(this);
                }
            }
            else
            {
                if (CityType == CityType.Factory)
                {
                    CityType = CityType.Large;

                    workForceMax -= DssConst.HeadCityStartMaxWorkForce;
                    detailObj.refreshModel();

                    if (overviewModel != null)
                    {
                        overviewModel.scale = VectorExt.V3(IconScale() * overviewModel.OneBlockScale);
                    }

                    DssRef.state.events.onFactoryDestroyed(this);
                }
            }
        }

        //public bool canBuyNobelHouse()
        //{
        //    return !nobelHouse &&
        //        workForce >= DssLib.NobelHouseWorkForceReqiurement &&
        //        faction.gold >= DssLib.NobleHouseCost;
        //}

        public bool canEverGetNobelHouse()
        {
            return true;//maxEpandWorkSize >= DssLib.NobelHouseWorkForceReqiurement;
        }

        //public void buyNobelHouseAction()
        //{
        //    if (canBuyNobelHouse() &&
        //        faction.payMoney(DssLib.NobleHouseCost, false))
        //    {
        //        addNobelHouseFeatures();
        //    }
        //}

        //void addNobelHouseFeatures()
        //{
        //    nobelHouse = true;

        //    if (!HasUnitPurchaseOption(UnitType.Knight))
        //    {
        //        var typeData = DssRef.profile.Get(UnitType.Knight);

        //        CityPurchaseOption knightPurchase = new CityPurchaseOption()
        //        {
        //            unitType = UnitType.Knight,
        //            goldCost = typeData.goldCost,
        //        };

        //        cityPurchaseOptions.Add(knightPurchase);
        //    }
        //}

        public bool hasNeededAreaSize()
        {
            int maxFit = CityDetail.WorkersPerTile * CityDetail.HutMaxLevel * areaSize;
            return workForceMax + 2 <= maxFit;
        }

        void createOverViewModel()
        {
            overviewModel?.DeleteMe();

            overviewModel = faction.AutoLoadModelInstance(
               LootFest.VoxelModelName.cityicon, IconScale());
            overviewModel.AddToRender(DrawGame.TerrainLayer);
            overviewModel.position = position;
        }

        float IconScale()
        {
            switch (CityType)
            {
                case CityType.Small:
                    return 0.7f;
                case CityType.Large:
                    return 1f;
                default:
                    return 1.3f;
                case CityType.Factory:
                    return 1.5f;
            }
        }

        //public void updateIncome_asynch()
        //{
        //    totalIncome = Convert.ToInt32(Math.Floor(workForce.value * TaxPerWorker - upkeep - blackMarketCosts.displayValue_sec));
        //}

        public CityEconomyData calcIncome_async()
        {
            return new CityEconomyData()
            {
                workerCount = workForce,//tax = workForce.value * TaxPerWorker,
                cityGuardUpkeep = GuardUpkeep(maxGuardSize),
                blackMarketCosts_Food = blackMarketCosts_food.displayValue_sec,
            };


        }

        public void onNewModel(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            detailObj.model?.onNewModel(name, master, detailObj);
            DSSWars.Faction.SetNewMaster(name, VoxelModelName.cityicon, overviewModel, master);
        }

        public void update()
        {
            updateDetailLevel();

            detailObj.update(Ref.DeltaGameTimeMs, true);

            updateWorkerUnits();
        }

        public override void asynchCullingUpdate(float time, bool bStateA)
        {
            if (inRender_detailLayer)
            {
                lib.DoNothing();
            }
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, ref cullingTopLeft, ref cullingBottomRight);
        }

        public double childAddPerSec()
        {
            if (battleGroup == null &&
                res_food.amount > 0 &&
                homeUsers() < homesTotal())
            {
                var result = workForce / 200.0 * faction.growthMultiplier;
                if (Culture == CityCulture.LargeFamilies)
                {
                    result *= 2;
                }
                return result;
            }
            return 0;
        }

        public bool isMaxHomeUsers()
        {
            return homeUsers() >= homesTotal();
        }

        public int homesUnused()
        {
            return homesTotal() - homeUsers();
        }

        public int homesTotal()
        {
            return workForceMax - damages.Int();
        }

        public int homeUsers()
        {
            return workForce + children();
        }

        public int children()
        {
            return childrenAge0.Int() + childrenAge1;
        }

        public void oneSecUpdate()
        {
            const int MinWorkforce = 8;

            if (parentArrayIndex == 65)
            {
                lib.DoNothing();
            }

            int addWorkers = 0;

            childrenAge0.value += childAddPerSec();

            if (DssRef.time.oneMinute)
            {
                addWorkers = childrenAge1;
                childrenAge1 = childrenAge0.pull();

                if (workForce < MinWorkforce)
                {
                    addWorkers += MinWorkforce - workForce;
                }
            }

            if (battleGroup == null)
            {
                if (immigrants.HasValue())
                {
                    var immigrantsToWork = immigrants.pull(5);
                    addWorkers += immigrantsToWork;

                    immigrants.reduceTowardsZero(ImmigrantsRemovePerSec);
                }

                detailObj.oneSecondUpdate();
            }

            workForce = Bound.Max(workForce + addWorkers, homesTotal());

            if (workForce > Achievements.LargePopulationCount &&
                 !DssRef.achieve.largePopulation &&
                 faction.player.IsPlayer())
            {
                DssRef.achieve.largePopulation = true;
                DssRef.achieve.UnlockAchievement(AchievementIndex.large_population);
            }

            //int waterAddPerSec = 1;
            //if (Culture == CityCulture.DeepWell)
            //{
            //    waterAddPerSec = 2;
            //}
            nextWater.value += waterAddPerSec;
            res_water.amount = Math.Min(res_water.amount + nextWater.pull(), maxWater);

            if (starving)
            { 
                starving = false;
                if (faction.player.IsPlayer())
                {
                    faction.player.GetLocalPlayer().hud.messages.cityLowFoodMessage(this);
                }
            }
        }

        public void asynchGameObjectsUpdate(bool minute)
        {
            collectBattles_asynch();
            detailObj.asynchUpdate();
            //strength
            strengthValue = 2.5f * guardCount / DssConst.SoldierGroup_DefaultCount;

            if (minute)
            {
                blackMarketCosts_food.minuteUpdate();
                foodProduction.minuteUpdate();
                foodSpending.minuteUpdate();
                soldResources.minuteUpdate();
            }
        }

        //public void dominationCheck()
        //{
        //    if (battleGroup == null)
        //    {
        //       var faction = DssRef.world.unitCollAreaGrid.CityDomination(this);

        //        Ref.update.AddSyncAction(new SyncAction1Arg<Faction>(setFaction, faction));
        //       //setFaction(faction);
               
        //    }
        //}

        public void asynchNearObjectsUpdate()
        {
            float defence = 0;

            DssRef.world.unitCollAreaGrid.collectArmies(faction, tilePos, 1,
                DssRef.world.unitCollAreaGrid.armies_nearUpdate);

            foreach (var m in DssRef.world.unitCollAreaGrid.armies_nearUpdate)
            {
                if (m.tilePos.SideLength(tilePos) <= 4)
                {
                    defence += m.strengthValue;
                }
            }

            ai_armyDefenceValue = defence;

            //detailObj.asynchNearObjectsUpdate();
        }

        protected override void setInRenderState()
        {
            if (inRender_overviewLayer)
            {
                if (overviewModel == null)
                {
                    createOverViewModel();
                }
            }
            else
            {
                if (overviewModel != null)
                {
                    overviewModel.DeleteMe();
                    overviewModel = null;
                }
            }

            setWorkersInRenderState();
            detailObj.setDetailLevel(inRender_detailLayer);
        }

        //protected override bool mayAttack(AbsMapObject otherObj)
        //{
        //    return true;
        //}

        override public bool rayCollision(Ray ray)
        {
            float? distance = ray.Intersects(bound);
            return distance.HasValue;
        }

        public override void selectionFrame(bool hover, Selection selection)
        {
            Vector3 pos = position;
            pos.Y += 0.1f;
            Vector3 scale;

            //selection.frameModel.Position = position;
            //selection.frameModel.position.Y += 0.1f;
            switch (CityType)
            {
                case CityType.Small:
                    scale = new Vector3(0.7f);
                    break;
                case CityType.Large:
                    scale = new Vector3(0.96f);
                    break;
                default:
                    scale = new Vector3(1.2f);
                    break;
            }

            selection.OneFrameModel(false, pos, scale, hover, true);
            //frameModel.Scale = new Vector3(1.2f);
            //frameModel.SetSpriteName(SpriteName.WhiteArea_LFtiles);
            //selection.frameModel.LoadedMeshType = hover ? LoadedMesh.SelectSquareDotted : LoadedMesh.SelectSquareSolid;
        }

        public void respawnGuard()
        {
            if (guardCount < maxGuardSize && 
                !InBattle() &&
                spendWorker(1))
            {
                guardCount += 1;
            }
        }

        bool spendWorker(int count)
        {
            if (workForce >= count)
            { 
                workForce-= count;
                return true;
            }

            return false;
        }
       
        //public int GetWeekIncome()
        //{
        //    return income;
        //}

        public override bool Equals(object obj)
        {
            return obj is City && ((City)obj).parentArrayIndex == parentArrayIndex;
        }

        public override string ToString()
        {
            return "City" + parentArrayIndex.ToString();
        }

        public override string Name()
        {
            return name;
        }

        public override string TypeName()
        {
            return DssRef.lang.UnitType_City + " (" + parentArrayIndex + ")";
        }
        //public override SpriteName TypeIcon()
        //{
        //    return SpriteName.WarsCityHall;
        //}

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RichBoxImage( SpriteName.WarsCityHall));
            tagToHud(content);
        }

        

        public override void toHud(ObjectHudArgs args)
        {
           
            base.toHud(args);
            if (args.ShowFull)
            {
                if (faction == args.player.faction)
                {
                    CityDetailsHud(true, args.player, args.content);
                    new Display.CityMenu(args.player, this, args.content);
                }
                else
                {
                    CityDetailsHud(false, args.player, args.content);
                }
            }
        }

        public void CityDetailsHud(bool minimal, LocalPlayer player, RichBoxContent content)
        {
            if (minimal)
            {
                content.Add(new RichBoxImage(SpriteName.WarsWorker));
                content.Add(new RichBoxText(TextLib.LargeNumber(workForce)));
                content.space();
                content.Add(new RichBoxImage(SpriteName.WarsGuard));
                content.Add(new RichBoxText(TextLib.Divition_Large(guardCount, maxGuardSize)));
                content.space();
                content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                content.Add(new RichBoxText(TextLib.OneDecimal(strengthValue)));
            }
            else
            {
                if (damages.HasValue())
                {
                    content.icontext(SpriteName.hqBatteResultBobbleDamage, string.Format(DssRef.lang.CityOption_Damages, damages.Int()));
                }
                HudLib.ItemCount(content, SpriteName.WarsWorkerAdd, DssRef.lang.ResourceType_Children, children().ToString());
                content.space();
                HudLib.InfoButton(content, new RbAction1Arg<City>(player.childrenTooltip, this));

                HudLib.ItemCount(content, SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers, TextLib.Divition_Large(workForce, workForceMax));
                HudLib.ItemCount(content, SpriteName.WarsGuard, DssRef.lang.Hud_GuardCount, TextLib.Divition_Large(guardCount, maxGuardSize));
                content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue)));
                content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Hud_TotalIncome, calcIncome_async().total(this)));
                content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Hud_Upkeep, GuardUpkeep(maxGuardSize)));

                cultureToHud(player, content);

                if (immigrants.HasValue())
                {
                    content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.Hud_Immigrants, immigrants.Int()));
                }

                //if (!player.inTutorialMode)
                {
                    //Properties
                    //if (nobelHouse)
                    //{
                    //    content.newLine();
                    //    HudLib.BulletPoint(content);
                    //    content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse));
                    //}

                    if (CityType == CityType.Factory)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxImage(SpriteName.WarsFactoryIcon));
                        content.Add(new RichBoxText(DssRef.lang.Building_DarkFactory));

                    }
                }
            }

            

        }

        public void cultureToHud(LocalPlayer player, RichBoxContent content)
        {
            content.icontext(SpriteName.WarsCultureIcon, string.Format(DssRef.lang.CityCulture_CultureIsX, Display.Translation.LangLib.CityCulture(Culture, true)));
            content.space();
            HudLib.InfoButton(content, new RbAction(() =>
            {
                RichBoxContent content = new RichBoxContent();
                content.text(Display.Translation.LangLib.CityCulture(Culture, false));

                player.hud.tooltip.create(player, content, true);
            }));//cultureToolTip));
        }

       // void cultureToolTip()
        

        public void AddNeighborCity(int nCityIndex)
        {
            if (nCityIndex >= 0)
            {
                
                if (!neighborCities.Contains(nCityIndex))
                {
                    neighborCities.Add(nCityIndex);
                }
            }
        }
       
        //public void GetStartFactionRegion(Map.Generate.Region region, WorldData world)
        //{
        //    if (faction == null)
        //    {
        //        //faction = new Faction(factions.Count);
        //        //factions.Add(faction);
        //        faction.AddCity(this, true);
                                
        //        int currentWorkforce = this.maxWorkForce;

        //        List<City> checkCities = new List<City>(8);

        //        int loopCount = 0;
        //        while (++loopCount < 100)
        //        {
        //            checkCities.Clear();
        //            checkCities.AddRange(faction.cities.toList());

        //            foreach (City check in checkCities)
        //            {
        //                foreach (int n in check.neighborCities)
        //                {
        //                    //if (!arraylib.InBound(world.cities, n))
        //                    //{ 
        //                    //    lib.DoNothing();
        //                    //}
        //                    City c = world.cities[n];
        //                    if (c.faction == null)
        //                    {
        //                        faction.AddCity(c, true);
        //                        currentWorkforce += c.maxWorkForce;

        //                        if (currentWorkforce >= goalWorkForce)
        //                        {
        //                            faction.availableForPlayer = true;
        //                            return;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static City Get(int index)
        {
            return DssRef.world.cities[index];
        }

        public void SetNeighborToPlayer()
        {
            foreach (int n in neighborCities)
            {
                City c = Get(n);
                if (c.faction != this.faction && c.faction.Owner is Players.AiPlayer)
                {
                    c.faction.Owner.IsPlayerNeighbor = true;
                }
            }
        }

        public bool HasPlayerNeighbor()
        {
            foreach (int n in neighborCities)
            {
                City c = Get(n);
                if (c.faction != this.faction && c.faction.Owner.IsPlayerNeighbor)
                {
                    return true;
                }
            }
            return false;
        }

        //public bool HasUnitPurchaseOption(UnitType type)
        //{
        //    foreach (var m in cityPurchaseOptions)
        //    {
        //        if (m.unitType == type)
        //        {
        //            return m.available;
        //        }
        //    }

        //    return false;   
        //}

        public override void setFaction(Faction faction)
        {
            if (this.faction != faction)
            {
                Faction prevOwner = this.faction;
                this.faction = faction;
                faction.AddCity(this, false);
                if (prevOwner != null)
                {
                    prevOwner.remove(this);
                }
                OnNewOwner();
            }
        }

        override public void OnNewOwner()
        {
            if (DssRef.world != null)
            {
                DssRef.world.BordersUpdated = true;

                detailObj?.onNewOwner();

                if (CityType == CityType.Factory && faction.factiontype != FactionType.DarkLord)
                {
                    setFactoryType(false);
                }
                else if (overviewModel != null)
                {
                    createOverViewModel();
                }

                workTemplate.onFactionChange(faction.workTemplate);
                tradeTemplate.onFactionValueChange(faction.tradeTemplate);
            }
        }

        //public void buySoldiersAction(UnitType type, int count, LocalPlayer player)
        //{
        //    Army army;
        //    bool success = buySoldiers(type, count, true, out army);
        //    if (success)
        //    {
        //        var typeData = DssRef.profile.Get(type);
        //        if (typeData.factionUniqueType >= 0)
        //        {
        //            DssRef.achieve.onFactionUniquePurchase(typeData.factionUniqueType);
        //        }

        //        if (player != null)
        //        {
        //            player.onBuySoldier();
        //        }
        //    }
        //}

        //public bool buySoldiers(UnitType type, int count, bool commit, out Army army, bool ignoreCityPurchaseOptions = false)
        //{//todo check 0 count
        //    var typeData = DssRef.profile.Get(type);

        //    int workersTotCost = typeData.workForceCount() * count;
        //    int moneyTotCost;            

        //    if (ignoreCityPurchaseOptions)
        //    {
        //        moneyTotCost = typeData.goldCost * count;
        //    }
        //    else
        //    {
        //        CityPurchaseOption opt = null;
        //        foreach (var m in cityPurchaseOptions)
        //        {
        //            if (m.unitType == type)
        //            {
        //                opt = m;
        //                break;
        //            }
        //        }

        //        if (opt == null)
        //        {
        //            army = null;
        //            return false;
        //        }

        //        moneyTotCost = opt.goldCost * count;
        //    }

        //    army = null;

        //    bool success = spendMenForDrafting(workersTotCost, false) &&//workForce.value >= workersTotCost &&
        //       faction.gold >= moneyTotCost;


        //    if (success && commit)
        //    {
        //        faction.payMoney(moneyTotCost, true);
        //        //workForce.pay(workersTotCost, true);
        //        spendMenForDrafting(workersTotCost, true);

        //        army = recruitToClosestArmy();

        //        if (army == null)
        //        {
        //            IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);

        //            army = faction.NewArmy(onTile);//new Army(faction, onTile);
        //        }

        //        for (int i = 0; i < count; i++)
        //        {
        //            new SoldierGroup(army, type, !StartupSettings.SkipRecruitTime);
        //        }

        //        army?.OnSoldierPurchaseCompleted();

        //    }
        //    return success;
        //}

        bool spendMenForDrafting(int menCount, bool commit)
        { 
            bool success = mercenaries + workForce >= menCount;

            if (success && commit)
            {
                int mercUse = Math.Min(mercenaries, menCount);
                mercenaries -= mercUse;
                menCount -= mercUse;

                workForce -= menCount;
               
            }

            return success;
        }

        public void createBuildingSubtiles(WorldData world)
        {
            IntVector2 topleft = WP.ToSubTilePos_TopLeft(tilePos);

            TerrainBuildingType tower;
            TerrainBuildingType wall;
            TerrainBuildingType house;
            TerrainBuildingType road;
            double percBuilding;

            switch (this.CityType)
            {
                case CityType.Small:
                    tower = TerrainBuildingType.DirtTower;
                    wall = TerrainBuildingType.DirtWall;
                    house = TerrainBuildingType.SmallHouse;
                    road = TerrainBuildingType.CobbleStones;
                    percBuilding = 0.3;
                    break;
                case CityType.Large:
                    tower = TerrainBuildingType.WoodTower;
                    wall= TerrainBuildingType.WoodWall;
                    house = TerrainBuildingType.SmallHouse;
                    road = TerrainBuildingType.Square;
                    percBuilding = 0.5;
                    break;
                default:
                    tower = TerrainBuildingType.StoneTower;
                    wall = TerrainBuildingType.StoneWall;
                    house = TerrainBuildingType.BigHouse;
                    road = TerrainBuildingType.Square;
                    percBuilding = 0.6;
                    break;

            }

            for (int y = 0; y < WorldData.TileSubDivitions; ++y)
            {
                for (int x = 0; x < WorldData.TileSubDivitions; ++x)
                {
                    TerrainBuildingType buildingType = TerrainBuildingType.NUM_NONE;

                    bool edgeX = x == 0 || x == WorldData.TileSubDivitions_MaxIndex;
                    bool edgeY = y == 0 || y == WorldData.TileSubDivitions_MaxIndex;

                    if (edgeX || edgeY)
                    {
                        if (edgeX && edgeY)
                        {
                            buildingType = tower;
                        }
                        else
                        {
                            buildingType = wall;
                        }
                    }
                    else if (x == 4 && y == 3)
                    {
                        buildingType = TerrainBuildingType.StoneHall;
                    }
                    else if (x == 4 && y == 4)
                    {
                        buildingType = TerrainBuildingType.Square;
                    }
                    else if (x == 3 && y == 4)
                    {
                        buildingType = TerrainBuildingType.Work_Cook;
                    }
                    else if (x == 5 && y == 4)
                    {
                        buildingType = TerrainBuildingType.Work_Bench;
                    }
                    else
                    {
                        buildingType = world.rnd.Chance(percBuilding) ? house : road;
                    }

                    IntVector2 pos = topleft;
                    pos.X += x;
                    pos.Y += y;
                    var subTile = world.subTileGrid.Get(pos);
                    subTile.SetType(TerrainMainType.Building, (int)buildingType, 1);
                    world.subTileGrid.Set(pos, subTile);
                }
            }
        }

        
        
        public Army recruitToClosestArmy()
        {
            return faction.ClosestFriendlyArmy(position, 2.6f);
        }

        public override City GetCity()
        {
            return this;
        }

        public override bool defeatedBy(Faction attacker)
        {
            return faction == attacker;
        }

        public override bool defeated()
        {
            return guardCount <= 0;
        }

        public override bool CanMenuFocus()
        {
            return true;
        }

        public override bool aliveAndBelongTo(int faction)
        {
            return this.faction.parentArrayIndex == faction;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.City;
        }
    }

    struct BorderTile
    {
        public IntVector2 Position;
        public int BorderToCityIndex;

        public BorderTile(IntVector2 position, int borderToCityIndex)
        {
            this.Position = position;
            this.BorderToCityIndex = borderToCityIndex;
        }
    }

    class CityPurchaseOption
    {
        public UnitType unitType;
        public bool available = true;
        public int goldCost;
        //TODO lägg till culture bonus för elit versioner

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Debug.Byte_OrCrash((int)unitType));
            w.Write(available);
            w.Write(Debug.Ushort_OrCrash(goldCost));
        }

        public void read(System.IO.BinaryReader r)
        {
            this.unitType = (UnitType)r.ReadByte();
            available=r.ReadBoolean();
            goldCost = r.ReadUInt16();
        }
    }

    enum CityType
    {
        Small,
        Large,
        Head,
        Factory,
        NUM
    }
}

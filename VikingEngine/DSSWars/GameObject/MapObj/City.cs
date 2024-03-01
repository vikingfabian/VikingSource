﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.GameObject
{
    class City : GameObject.AbsMapObject
    {
        public const int ExpandWorkForce = AbsSoldierData.GroupDefaultCount * 4;
        public const int ExpandGuardSize = AbsSoldierData.GroupDefaultCount;
        public const int ExpandGuardSizeCost = 12000;

        public int index;
        public int areaSize=0;
        public CityType CityType;
        public List<int> neighborCities = new List<int>();
        
        Graphics.AbsVoxelObj overviewModel;

        BoundingBox bound;

        public int income;
        float upkeep;
        public int maxGuardSize;
        public int guardCount;
        //public int guardRecruitDelay
        public FloatingInt_Max workForce = new FloatingInt_Max();
        //public int maxWorkForce;
        public int maxEpandWorkSize;
        public FloatingInt immigrants = new FloatingInt();
        const double ImmigrantsRemovePerSec = 0.1; 
        double workForceAddPerSec;
        public int workHutStyle = 0;
        
        public CityDetail detailObj;
        public List<CityPurchaseOption> cityPurchaseOptions;
        
        public float ai_armyDefenceValue = 0;
        public bool nobelHouse = false;

        public City(int index, IntVector2 pos, CityType type, WorldData world)
        {
            this.index = index;
            this.tilePos = pos;
            this.CityType = type;
        }

        public City(int index, System.IO.BinaryReader r, int version)
        {
            this.index = index;
            read(r, version);
        }

        public void generateCultureAndEconomy(WorldData world, CityCultureCollection cityCultureCollection)
        {
            initEconomy();
            
            double land = 0, water = 0, plain = 0, forest = 0, mountain = 0, dryBiom = 0;

            Rectangle2 cultureArea = Rectangle2.FromCenterTileAndRadius(tilePos, 3);
            double total = cultureArea.Area;
            ForXYLoop loop = new ForXYLoop(cultureArea);
            
            while (loop.Next())
            {
                var tile=  world.tileGrid.Get(loop.Position);
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

            cityPurchaseOptions = new List<CityPurchaseOption>();

            foreach (var type in DssLib.AvailableUnitTypes)
            {
                var typeData = DssRef.unitsdata.Get(type);

                CityPurchaseOption cityPurchase = new CityPurchaseOption()
                {
                    unitType = type,
                    goldCost = typeData.goldCost,
                };

                switch (type)
                {
                    case UnitType.Soldier:
                        if (water <= 2)
                        {
                            if (percPlains >= 0.75)
                            {
                                cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 4;
                            }
                            else if (percPlains >= 0.5)
                            {
                                cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 2;
                            }
                            else if (percPlains >= 0.25)
                            {
                                cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                            }
                        }
                        break;

                    case UnitType.Archer:
                        if (percWater < 0.5)
                        {
                            if (percForest >= 0.75)
                            {
                                cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 4;
                            }
                            else if (percForest >= 0.40)
                            {
                                cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 2;
                            }
                            else if (percForest >= 0.1)
                            {
                                cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                            }
                        }
                        break;

                    case UnitType.Sailor:
                        cityPurchase.available = percWater >= 0.25;

                        if (percWater >= 0.60)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 4;
                        }
                        else if (percWater >= 0.50)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 2;
                        }
                        else if (percWater >= 0.40)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                        }
                        break;

                    case UnitType.Folkman:
                        if (this.CityType == CityType.Small)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                        }
                        else if (this.CityType == CityType.Large)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction / 2;
                        }

                        if (percPlains >= 0.7)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                        }
                        else if (percPlains >= 0.4)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction / 2;
                        }
                        break;

                    case UnitType.Knight:
                        cityPurchase.available = this.CityType >= CityType.Head;
                        //if (this.CityType == CityType.Head)
                        //{
                        //    cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 2;
                        //}

                        if (percPlains >= 0.5)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                        }

                        if (percWater >= 0.30)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                        }
                        break;

                    case UnitType.Ballista:
                        cityPurchase.available = forest > 0;

                        if (forest >= 8 && mountain >= 8)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 4;
                        }
                        else if (forest >= 4 && mountain >= 4)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction * 2;
                        }
                        else if (forest >= 2 && mountain >= 2)
                        {
                            cityPurchase.goldCost -= DssLib.GroupDefaultCultureCostReduction;
                        }
                        break;
                }

                if (cityPurchase.available)
                {
                    cityPurchase.goldCost = Bound.Min(cityPurchase.goldCost, DssLib.GroupMinCost);
                    cityPurchaseOptions.Add(cityPurchase);
                }
            }

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

        }

        public void write(System.IO.BinaryWriter w)
        {            
            tilePos.writeUshort(w);

            w.Write(Debug.Byte_OrCrash((int)CityType));//(byte)CityType);
            w.Write(Debug.Ushort_OrCrash(areaSize));//(ushort)areaSize);
            w.Write(Debug.Byte_OrCrash(workHutStyle));//(byte)workHutStyle);

            w.Write(Debug.Byte_OrCrash(neighborCities.Count));
            foreach(var n in neighborCities)
            {
                w.Write(Debug.Ushort_OrCrash(n));//(ushort)n);
            }

            w.Write(Debug.Byte_OrCrash(cityPurchaseOptions.Count));
            foreach (var opt in cityPurchaseOptions)
            {
                opt.write(w);
            }
            
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            tilePos.readUshort(r);

            CityType = (CityType)r.ReadByte();
            areaSize= r.ReadUInt16();
            workHutStyle= r.ReadByte();

            int neighborCitiesCount=r.ReadByte();
            for(int i =0;i <neighborCitiesCount; i++)
            {
                neighborCities.Add(r.ReadUInt16());
            }

            int cityPurchaseOptionsCount = r.ReadByte();
            cityPurchaseOptions = new List<CityPurchaseOption>(cityPurchaseOptionsCount);            
            for(int i = 0; i < cityPurchaseOptionsCount; ++i)
            {
                CityPurchaseOption cityPurchase = new CityPurchaseOption();
                cityPurchase.read(r);
                cityPurchaseOptions.Add(cityPurchase);
            }

        }

        public void setRenderState(bool inRender)
        {

        }

        public int expandWorkForceCost()
        {
            //const int ExpandWorkForceCost = 20000;
            return 40000 + workForce.max * 10;
        }

        public bool canExpandWorkForce(int count)
        {
            return (workForce.max + ExpandWorkForce * count) <= maxEpandWorkSize;
        }

        public bool canIncreaseGuardSize(int count)
        {
            return (maxGuardSize + ExpandGuardSize * count) <= workForce.max;
        }

        public void expandWorkForce(int amount)
        {
            workForce.max += amount;
            refreshCitySize();

            detailObj.refreshWorkerSubtiles();//updateWorkerModels();
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
                        expandGuardSize(ExpandGuardSize * count);
                        faction.payMoney(totalCost, true);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool buyWorkforce(bool commit, int count)
        {
            if (canExpandWorkForce(count))
            {
                int totalCost = 0;

                if (faction.calcCost(expandWorkForceCost() * count, ref totalCost))
                {
                    if (commit)
                    {
                        expandWorkForce(ExpandWorkForce * count);
                        faction.payMoney(totalCost, true);                        
                    }
                    return true;
                }
            }
            return false;
        }

        

        public int GuardUpkeep(int maxGuardSize)
        {
            return (int)(0.2f * maxGuardSize);
        }
        

        public void onGameStart()
        {
            groupRadius = 0.6f;

            initEconomy();

            maxGuardSize = workForce.Int() / 4;
                        
            guardCount = maxGuardSize;

            refreshCitySize();

            updateIncome_asynch();

            position = new Vector3(tilePos.X, Tile().ModelGroundY(), tilePos.Y);

            detailObj = new CityDetail(this);

            float iconScale = IconScale();            

            VectorVolumeC volume = new VectorVolumeC(position,
                new Vector3(iconScale * 0.5f, 0.1f, iconScale * 0.5f));
            bound = volume.boundingBox();
        }

        void initEconomy()
        {
            switch (CityType)
            {
                case CityType.Small:
                    workForce.value = DssLib.SmallCityStartWorkForce;
                    workForce.max = DssLib.SmallCityMaxWorkForce;
                    break;
                case CityType.Large:
                    workForce.value = DssLib.LargeCityStartWorkForce;
                    workForce.max = DssLib.LargeCityMaxWorkForce;
                    break;
                default:
                    workForce.value = DssLib.HeadCityStartWorkForce;
                    workForce.max = DssLib.HeadCityMaxWorkForce;
                    nobelHouse = true;
                    break;
            }

            int maxFit = MathExt.MultiplyInt(0.8, CityDetail.WorkersPerTile * CityDetail.HutMaxLevel * areaSize);
            maxEpandWorkSize = Bound.Max(workForce.max + ExpandWorkForce * 3, maxFit); //Bound.Max(workForce.max * 2 + MathExt.MultiplyInt(0.2, maxFit), maxFit);
        }

        void refreshCitySize()
        {
            upkeep = GuardUpkeep(maxGuardSize);
            workForceAddPerSec = workForce.max / 200f;

            refreshVisualSize();
        }

        void refreshVisualSize()
        {
            if (CityType != CityType.Factory)
            {
                CityType newType;

                if (workForce.max >= DssLib.HeadCityMaxWorkForce)
                {
                    newType = CityType.Head;
                }
                else if (workForce.max >= DssLib.LargeCityMaxWorkForce)
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
                    Task.Factory.StartNew(() =>
                    {
                        createBuildingSubtiles(DssRef.world);
                    });

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

                    workForce.max += DssLib.HeadCityMaxWorkForce;
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

                    workForce.max -= DssLib.HeadCityMaxWorkForce;
                    detailObj.refreshModel();

                    if (overviewModel != null)
                    {
                        overviewModel.scale = VectorExt.V3(IconScale() * overviewModel.OneBlockScale);
                    }

                    DssRef.state.events.onFactoryDestroyed(this);
                }
            }
        }

        public bool canBuyNobelHouse()
        {
            return !nobelHouse &&
                workForce.value >= DssLib.NobelHouseWorkForceReqiurement &&
                faction.gold >= DssLib.NobelHouseCost;
        }

        public bool canEverGetNobelHouse()
        {
            return maxEpandWorkSize >= DssLib.NobelHouseWorkForceReqiurement;
        }

        public void buyNobelHouseAction()
        {
            if (canBuyNobelHouse() &&
                faction.payMoney(DssLib.NobelHouseCost, false))
            { 
                nobelHouse = true;
                var typeData = DssRef.unitsdata.Get(UnitType.Knight);

                CityPurchaseOption knightPurchase = new CityPurchaseOption()
                {
                    unitType = UnitType.Knight,
                    goldCost = typeData.goldCost,
                };

                cityPurchaseOptions.Add(knightPurchase);
            }
        }

        public bool hasNeededAreaSize()
        {
            int maxFit = CityDetail.WorkersPerTile * CityDetail.HutMaxLevel * areaSize;
            return workForce.max + 2 <= maxFit;
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

        public void updateIncome_asynch()
        {
            income = Convert.ToInt32(workForce.Int() - upkeep);
        }        

        public void onNewModel(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            detailObj.model?.onNewModel(name, master, detailObj);
            DSSWars.Faction.SetNewMaster(name, VoxelModelName.cityicon, overviewModel, master);
        }

        public void update()
        {
            updateDetailLevel();
            if (index == 50)
            { 
                lib.DoNothing();
                
            }

            //battles.checkForUpdatedList();
            
            detailObj.update(Ref.DeltaGameTimeMs, true);
        }

        public void oneSecUpdate()
        {
            double addWorkers = workForceAddPerSec * faction.growthMultiplier;
            if (faction.player.IsAi())
            {
                addWorkers *= Players.AiPlayer.EconomyMultiplier;
            }
            workForce.add(addWorkers);

            if (immigrants.HasValue())
            {
                //var availableImmigrants = Math.Min(immigrants, 5);
                var immigrantsToWork = immigrants.pull(5);//Math.Min(maxWorkForce - workForce, availableImmigrants);
                workForce.value += immigrantsToWork;

                immigrants.reduceTowardsZero(ImmigrantsRemovePerSec);
            }
            
            detailObj.oneSecondUpdate();
        }

        public void asynchGameObjectsUpdate()
        {
            collectBattles_asynch();
            detailObj.asynchUpdate();

            //strength
            strengthValue = 2f * guardCount / AbsSoldierData.GroupDefaultCount;
        }

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

            detailObj.asynchNearObjectsUpdate();
        }

        protected override void setInRenderState()
        {
            if (inRender)
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

            detailObj.setDetailLevel(inRender);
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
            selection.frameModel.Position = position;
            selection.frameModel.position.Y += 0.1f;
            switch (CityType)
            {
                case CityType.Small:
                    selection.frameModel.Scale = new Vector3(0.7f);
                    break;
                case CityType.Large:
                    selection.frameModel.Scale = new Vector3(0.96f);
                    break;
                default:
                    selection.frameModel.Scale = new Vector3(1.2f);
                    break;
            }
            //frameModel.Scale = new Vector3(1.2f);
            //frameModel.SetSpriteName(SpriteName.WhiteArea_LFtiles);
            selection.frameModel.LoadedMeshType = hover ? LoadedMesh.SelectSquareDotted : LoadedMesh.SelectSquareSolid;
        }

        public void respawnGuard()
        {
            if (guardCount < maxGuardSize && 
                !InBattle() &&
                workForce.pay(1, false))
            {
                guardCount += 1;
            }
        }
        
       
        public int GetWeekIncome()
        {
            return income;
        }

        public override bool Equals(object obj)
        {
            return obj is City && ((City)obj).index == index;
        }

        public override string ToString()
        {
            return "City" + index.ToString();
        }

        public override string Name()
        {
            return "City" + index.ToString();
        }

        public override void toHud(Display.ObjectHudArgs args)
        {
            base.toHud(args);
            if (args.player.hud.detailLevel == HudDetailLevel.Minimal)
            {
                args.content.Add(new RichBoxImage(SpriteName.WarsGuard));
                args.content.Add(new RichBoxText(TextLib.LargeNumber(workForce.Int())));
                args.content.space();
                args.content.Add(new RichBoxImage(SpriteName.WarsStrengthIcon));
                args.content.Add(new RichBoxText(string.Format(HudLib.OneDecimalFormat, strengthValue)));
                args.content.space();
                args.content.Add(new RichBoxImage(SpriteName.WarsWorker));
                args.content.Add(new RichBoxText(TextLib.LargeNumber(workForce.Int())));
            }
            else
            {
                args.content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Hud_TotalIncome, income));
                args.content.icontext(SpriteName.WarsGuard, string.Format(DssRef.lang.Hud_GuardCount,TextLib.Divition_Large(guardCount, maxGuardSize)));
                args.content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_StrengthRating, string.Format(HudLib.OneDecimalFormat, strengthValue)));
                args.content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Hud_Upkeep, upkeep));
                args.content.icontext(SpriteName.WarsWorker, string.Format(DssRef.lang.Hud_WorkForce, TextLib.Divition_Large(workForce.Int(), workForce.max)));
                if (immigrants.HasValue())
                {
                    args.content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.Hud_Immigrants, immigrants.Int()));
                }
            }
            if (args.selected && faction == args.player.faction)
            {
                new Display.CityMenu(args.player, this, args.content);
            }

            //Properties
            if (nobelHouse)
            {
                args.content.ListDot();
                args.content.Add(new RichBoxText(DssRef.lang.Building_NobelHouse));
                args.content.newLine();
            }

            if (CityType == CityType.Factory)
            {
                args.content.ListDot();
                args.content.Add(new RichBoxImage(SpriteName.WarsFactoryIcon));
                args.content.Add(new RichBoxText(DssRef.lang.Building_DarkFactory));
                args.content.newLine();
            }
        }

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

        public bool HasUnitPurchaseOption(UnitType type)
        {
            foreach (var m in cityPurchaseOptions)
            {
                if (m.unitType == type)
                {
                    return m.available;
                }
            }

            return false;   
        }

        public override void setFaction(Faction faction)
        {
            if (this.faction != faction)
            {
                if (this.faction != null)
                {
                    this.faction.remove(this);
                }

                this.faction = faction;
                faction.AddCity(this, false);

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
            }
        }

        public void buySoldiersAction(UnitType type, int count)
        {
            Army army;
            bool success = buySoldiers(type, count, true, out army);
            if (success)
            {
                var typeData = DssRef.unitsdata.Get(type);
                if (typeData.factionUniqueType >= 0)
                {
                    DssRef.achieve.onFactionUniquePurchase(typeData.factionUniqueType);
                }
            }
        }

        public bool buySoldiers(UnitType type, int count, bool commit, out Army army, bool ignoreCityPurchaseOptions = false)
        {
            var typeData = DssRef.unitsdata.Get(type);

            int workersTotCost = typeData.workForceCount() * count;
            int moneyTotCost;            

            if (ignoreCityPurchaseOptions)
            {
                moneyTotCost = typeData.goldCost * count;
            }
            else
            {
                CityPurchaseOption opt = null;
                foreach (var m in cityPurchaseOptions)
                {
                    if (m.unitType == type)
                    {
                        opt = m;
                        break;
                    }
                }

                if (opt == null)
                {
                    army = null;
                    return false;
                }

                moneyTotCost = opt.goldCost * count;
            }

            army = null;

            bool success = workForce.value >= workersTotCost &&
               faction.gold >= moneyTotCost;


            if (success && commit)
            {
                faction.payMoney(moneyTotCost, true);
                workForce.pay(workersTotCost, true);

                army = recruitToClosestArmy();

                if (army == null)
                {
                    IntVector2 onTile = DssRef.world.GetFreeTile(tilePos);

                    army = faction.NewArmy(onTile);//new Army(faction, onTile);
                }

                for (int i = 0; i < count; i++)
                {
                    new SoldierGroup(army, type, !StartupSettings.SkipRecruitTime);
                }

                army?.OnSoldierPurchaseCompleted();

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
                    TerrainBuildingType buildingType = TerrainBuildingType.NUM;

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
            return faction.ClosestFriendlyArmy(position, 1.5f);
        }

        public override City GetCity()
        {
            return this;
        }

        public override bool isMelee()
        {
            return false;
        }

        public override bool defeatedBy(Faction attacker)
        {
            return faction == attacker;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            return this.faction == faction;
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

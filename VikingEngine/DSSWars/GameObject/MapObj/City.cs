
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.DSSWars.Stockpile;

namespace VikingEngine.DSSWars.GameObject
{
    partial class City : AbsArmy
    {
        const int MaxWorkerWriteCount = 64;

        public int areaSize = 0;
        public CityType cityType;
        //public List<int> neighborCities = new List<int>();
        public int neighborCitiesCount = 0;

        Graphics.AbsVoxelObj overviewModel;

        BoundingBox bound;

        public FloatingInt childrenAge0 = new FloatingInt();
        public int childrenAge1 = 0;

        public IntVector2 cityHallSubtilePos;
        IntVector2 armySpawnTilePos = IntVector2.Zero;
        public GroupedResource workForce = new GroupedResource();
        bool needServiceMenRefresh = true;
        public GroupedResource freeServiceMen = new GroupedResource();
        //public int totalServiceMen = 0;
        public int workingAndFreeServiceMen = 0;

        public int HousingCount_Workers = 0;
        public int WorkersMaxLimit;
        public int HousingCount_Guard = 0;

        public GroupedResource freeNobelMen = new GroupedResource();
        public int HousingCount_NobelMen = 0;

        public int PenFoodUpkeep_minute = 0;

        public int AvailableGuardHousing()
        {
            return HousingCount_Guard - soldiersCount;
        }

        public FloatingInt immigrants = new FloatingInt();
        
        public int workHutStyle = 0;
        public int mercenaries = 0;

        //public CityDetail detailObj;
        public float ai_armyDefenceValue = 0;

        public BuildingStructure buildingStructure = new BuildingStructure();
        public TerrainStructure terrainStructure = new TerrainStructure();

        //bool customName = false;
        ObjectName name = new ObjectName();


        Intvector2MinMax workerCullingMinMax, guardCullingMinMax;
        //IntVector2 cullingTopLeft, cullingBottomRight;
        //public int cityTileRadius = 0;
        public Rectangle2 cityTileArea;
        public CityCulture Culture = CityCulture.NUM_NONE;
        public CityBiom Biom = CityBiom.None;

        public Build.BuildAndExpandType autoExpandFarmType = Build.BuildAndExpandType.WheatFarm;
        bool autoBuild_Work = false;
        bool autoBuild_Farm = false;

        public CityTagBack tagBack = CityTagBack.NONE;
        public CityTagArt tagArt = CityTagArt.None;

        int starvingTimeSeconds = 0;

        public int previousOwner = -1;
        public float capturePoints = 0;


        public bool CanBuildLogistics(int toLevel)
        {
            if (toLevel == 1)
            {
                return resourceAmount(CityResoureIndex.food)/*res_food.amount*/ >= DssConst.Logistics1FoodStorage;
            }
            else if (toLevel == 2)
            {
                return GetFaction().totalWorkForce > DssConst.Logistics2_PopulationRequirement;
            }

            return false;
        }

        public int MaxBuildPrio()
        { 
            return LevelToMaxBuildPrio(buildingStructure.buildingLevel_logistics);
        }

        public IntVector2 ArmySpawnTilePos()
        {
            if (armySpawnTilePos.HasValue())
            {
                return armySpawnTilePos;
            }

            for (int radius = 2; radius >= 1; --radius)
            {
                ForXYEdgeLoop loop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, radius));
                while (loop.Next())
                {
                    Tile t = DssRef.world.tileGrid.Get(loop.Position);
                    if (t.IsLand())
                    {
                        armySpawnTilePos = loop.Position;
                        return loop.Position;
                    }
                }
            }

            Debug.LogError("GetFreeTile" + tilePos.ToString());
            return tilePos;
        }

       
        public static int LevelToMaxBuildPrio(int level)
        {
            int max = WorkTemplate.MaxPrio;

            switch (level)
            {
                default: return max;
                case 0: max = DssConst.BuildPrio_Start; break;
                case 1: max = DssConst.BuildPrio_LogisticsLevel1; break;
            }

            return max;
        }

        public void upgradeLogistics()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                try
                {
                    if (CityStructure.Find(this, TerrainMainType.Building, (int)TerrainBuildingType.Logistics, out IntVector2 position))
                    {
                        CraftBuildingLib.CraftLogisticsLevel2.payResources(this);

                        EditSubTile edit = new EditSubTile();
                        edit.position = position;
                        edit.value.terrainAmount = 2;
                        edit.editAmount = true;

                        edit.Submit();

                        buildingStructure.buildingLevel_logistics = 2;
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
               
            });
        }

        public bool autoUpgradeLogistics(IntVector2 freeSubTile, bool commit)
        {
            //commit is main thread

            if (CanBuildLogistics(buildingStructure.buildingLevel_logistics + 1))
            {
                if (buildingStructure.buildingLevel_logistics == 0)
                {
                    if (CraftBuildingLib.CraftLogistics.hasResources(this))
                    {
                        if (commit)
                        {
                            var player = GetFaction().player.GetLocalPlayer();
                            if (player != null)
                            {
                                player.orders.addOrder(player.playerData.localPlayerIndex, new BuildOrder(WorkTemplate.MaxPrio, true, this, freeSubTile, Build.BuildAndExpandType.Logistics, false), ActionOnConflict.Cancel);
                            }
                        }
                        return true;
                    }
                }
                else if (buildingStructure.buildingLevel_logistics == 1)
                {
                    if (CraftBuildingLib.CraftLogisticsLevel2.hasResources(this))
                    {
                        if (commit)
                        {
                            CraftBuildingLib.CraftLogisticsLevel2.payResources(this);
                            upgradeLogistics();
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        //public bool availableBuildQueue(LocalPlayer player)
        //{
        //    return MaxBuildQueue() > 1000 || player.orders.buildQueue(this) < MaxBuildQueue();
        //}

        //public int availableBuildQueueLength(LocalPlayer player)
        //{
        //    return MaxBuildQueue() - player.orders.buildQueue(this);
        //}

        public void haltConscriptAndDelivery()
        {
            lock (conscriptBuildings)
            {
                //for (int i = 0; i < conscriptBuildings.Count; i++)
                //{
                //    BarracksStatus status = conscriptBuildings[i];
                //    status.halt(this);
                //    conscriptBuildings[i] = status;
                //}
                queueToAllConscripts(0, null);
            }

            for (int i = 0; i < deliveryServices.Count; i++)
            {
                var delivery = deliveryServices[i];
                delivery.halt();
                deliveryServices[i] = delivery;
            }

            if (casualProgress != null)
            {
                casualProgress.clearBuildQueue();
                casualProgress.clearRecruitQueue();
            }
        }

        public bool AutoBuildWorkProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                autoBuild_Work = value;
                (value? SoundLib.click : SoundLib.back).Play();
            }
            return autoBuild_Work;
        }
        public bool AutoBuildFarmProperty(object tag, bool set, bool value)
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
            this.myIndex = index;
            world.InitCity(this);
            this.tilePos = pos;
            this.cityType = type;
        }

        public City(int index)
        {
            this.myIndex = index;
            DssRef.world.InitCity(this);
        }

        public City(WorldData world, int index, System.IO.BinaryReader r, int version)
        {
            this.myIndex = index;
            world.InitCity(this);
            readMapFile(world, r, version);
        }

        public void generateCultureAndEconomy(WorldData world, CityCultureCollection cityCultureCollection)
        {
            initEconomy(/*true,*/ world);

            CityAreaCulture areaCulture = new CityAreaCulture(this, world);

            workHutStyle = areaCulture.percMountain > 0.5 ? 0 : 1;

            if (areaCulture.percDesolate > 0.5)
            {
                Biom = CityBiom.Desolate;
            }
            else if (areaCulture.frozenBiom > 0.5)
            {
                Biom = CityBiom.Frozen;
            }
            else if (areaCulture.percDry > 0.5)
            {
                Biom = CityBiom.Desert;
            }
            else if (areaCulture.percForest > 0.75)
            {
                Biom = CityBiom.Forest;
            }
            else if (areaCulture.percMountain > 0.5)
            {
                Biom = CityBiom.Mountain;
            }

            if (areaCulture.percForest >= 0.7 && cityType == CityType.Capital)
            {
                cityCultureCollection.LargeGreen.Add(this);
            }
            else if (areaCulture.percDry >= 0.7 && areaCulture.worldPercX >= 0.75)
            {
                cityCultureCollection.DryEast.Add(this);
            }
            else if (areaCulture.percWater >= 0.25 && areaCulture.worldPercY <= 0.25)
            {
                cityCultureCollection.NorthSea.Add(this);
            }
            else if (areaCulture.worldPercY > 0.5f)
            {
                if (areaCulture.worldPercX < 0.3f)
                {
                    cityCultureCollection.WestKingdom.Add(this);
                }
                else
                {
                    cityCultureCollection.DarkLands.Add(this);
                }
            }

            if (world.rnd.Chance(0.3))
            {
                //Area specific culture
                if (areaCulture.percDry > 0.05 && areaCulture.percDry < 0.7 && areaCulture.percPlains >= 0.1)
                {
                    Culture = CityCulture.FertileGround;
                }
                else if (areaCulture.percForest >= 0.8)
                {
                    Culture = CityCulture.Woodcutters;
                }
                else if (areaCulture.percMountain > 0.5)
                {
                    Culture = CityCulture.Miners;
                }
                else if (areaCulture.percMountain > 0.3)
                {
                    Culture = CityCulture.Stonemason;
                }
                else if (areaCulture.dryBiom <= 1)
                {
                    Culture = CityCulture.DeepWell;
                }
                else if (areaCulture.percForest >= 0.1)
                {
                    Culture = CityCulture.PitMasters;
                }
                else if (areaCulture.percWater >= 0.25)
                {
                    Culture = CityCulture.Seafaring;
                }
            }

            if (Culture == CityCulture.NUM_NONE)
            {
                Culture = arraylib.RandomListMember(CityCultureCollection.GeneralCultures, world.rnd);
            }

            casualCityProfile.InitCulture(this, areaCulture);
        }

        public void writeMapFile(System.IO.BinaryWriter w)
        {
            tilePos.writeUshort(w);

            w.Write(Debug.Byte_OrCrash((int)cityType));
            w.Write(Debug.Ushort_OrCrash(areaSize));
            //w.Write(Debug.Byte_OrCrash(cityTileRadius));
            cityTileArea.pos.writeUshort(w);
            cityTileArea.size.writeByte(w);


            w.Write(Debug.Byte_OrCrash(workHutStyle));

            w.Write(Debug.Byte_OrCrash(neighborCitiesCount));
            EcsStaticArrayCounter neighbors = CityNeighbors();
            while (neighbors.Next(out int nCityIx))
            {
                w.Write(Debug.Ushort_OrCrash(nCityIx));
            }

            w.Write(Debug.Byte_OrCrash((int)Culture));

            Debug.WriteCheck(w);
        }

        public void readMapFile(WorldData world, System.IO.BinaryReader r, int saveMapVersion)
        {
            tilePos.readUshort(r);

            cityType = (CityType)r.ReadByte();
            if (saveMapVersion < 9)
            {
                cityType += 2;
            }
            
            areaSize = r.ReadUInt16();

            if (saveMapVersion < 10)
            {
                int cityTileRadius = r.ReadByte();
                cityTileArea = Rectangle2.FromCenterTileAndRadius(tilePos, cityTileRadius);
            }
            else
            {
                cityTileArea.pos.readUshort(r);
                cityTileArea.size.readByte(r);
            }

            workHutStyle = r.ReadByte();

            neighborCitiesCount = 0;
            int readNeighborCities = r.ReadByte();
            for (int i = 0; i < readNeighborCities; i++)
            {
                int cityIx = r.ReadUInt16();
                world.neighborCities.Add(myIndex, ref neighborCitiesCount, cityIx);
            }
            

            Culture = (CityCulture)r.ReadByte();

            workerCullingMinMax = new Intvector2MinMax(tilePos);
            guardCullingMinMax = workerCullingMinMax;

            if (saveMapVersion >= 9)
            {
                Debug.ReadCheck(r);
            }
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            try
            {
                if (myIndex == 153)
                {
                    lib.DoNothing();
                }

                w.Write((byte)cityType);

                //w.Write(Bound.UShort(workForce.amount));
                //w.Write(Bound.UShort(HousingCount_Workers));
                w.Write(workForce.amount);
                w.Write(HousingCount_Workers);
                w.Write(Bound.UShort(HousingCount_Guard));
                w.Write(Bound.Short(freeServiceMen.amount));
                w.Write(Bound.Short(workingAndFreeServiceMen));

                w.Write(Bound.UShort(HousingCount_NobelMen));
                w.Write(Bound.Short(freeNobelMen.amount));
                w.Write(Bound.UShort(PenFoodUpkeep_minute));
                
                cityHallSubtilePos.writeUshort(w);
                citySquareSubtilePos.writeUshort(w);

                Debug.WriteCheck(w);

                childrenAge0.write16bit(w);
                w.Write(Bound.UShort(childrenAge1));

                immigrants.write16bit(w);

                w.Write(Bound.Byte(maxWaterBase));
                w.Write(waterAddPerSec);
                workTemplate.writeGameState(w);

                Debug.WriteCheck(w);
                
                writeResources(w);

                writeWorkerStatuses(w, false, -1);

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

                w.Write((ushort)schoolBuildings.Count);
                foreach (var school in schoolBuildings)
                { school.writeGameState(w); }

                if (arraylib.HasMembers(researchBuildings))
                {
                    w.Write((ushort)researchBuildings.Count);
                    foreach (var research in researchBuildings)
                    {
                        research.writeGameState(w);
                    }
                }
                else
                {
                    w.Write(ushort.MinValue);
                }
                w.Write((byte)experenceOrDistance);

                writeSoldierGroups(w);

                w.Write((ushort)defenceBuildings.Count);
                for (int i = 0; i < defenceBuildings.Count; ++i)//each (var defence in defenceBuildings)
                {
                    defenceBuildings.array[i].writeGameState(w);
                }

                w.Write((ushort)cesspits.Count);
                for (int i = 0; i < cesspits.Count; ++i)
                {
                    cesspits.array[i].writeGameState(w);
                }

                w.Write(autoBuild_Work);
                w.Write(autoBuild_Farm);
                w.Write((byte)autoExpandFarmType);

                w.Write((byte)tagBack);
                if (tagBack != CityTagBack.NONE)
                {
                    w.Write((ushort)tagArt);
                }

                w.Write(res_food_safeguard);

                technology.writeGameState(w, false);
                money.write(w);
                w.Write(automateCity);
                w.Write((byte)automationFocus);
                w.Write((byte)warAutoQuality);
                w.Write((byte)warAutoWeaponType);

                name.write(w);

                casualCityProfile.writeGameState(w);
                if (casualProgress == null)
                {
                    w.Write(false);
                }
                else
                {
                    w.Write(true);
                    casualProgress.writeGameState(w);
                }

                Debug.WriteCheck(w);

                //throw new Exception("test");
            }
            catch (Exception e)
            {
                BlueScreen.AttachMessage =
                   $"workforce {workForce.amount}, HousingCount_Workers {HousingCount_Workers}, HousingCount_Guard {HousingCount_Guard}, cityHallSubtilePos {cityHallSubtilePos}, cityStorageCenter {citySquareSubtilePos}, childrenAge0 {childrenAge0}, childrenAge1 {childrenAge1}, immigrants {immigrants}, ";

                BlueScreen.ThreadException = e;
            }
        }

        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            cityType = (CityType)r.ReadByte();
            
            workForce.amount = r.ReadInt32();
            HousingCount_Workers = r.ReadInt32();
            
            HousingCount_Guard = r.ReadUInt16();
            freeServiceMen.amount = r.ReadInt16();
            
            workingAndFreeServiceMen = r.ReadInt16();

            HousingCount_NobelMen = r.ReadUInt16();
            freeNobelMen.amount = r.ReadInt16();
            PenFoodUpkeep_minute = r.ReadUInt16();

            cityHallSubtilePos.readUshort(r);
            citySquareSubtilePos.readUshort(r);
            

            Debug.ReadCheck(r);

            childrenAge0.read16bit(r);
            childrenAge1 = r.ReadUInt16();

            immigrants.read16bit(r);

            maxWaterBase = r.ReadByte();
            maxWaterTotal = maxWaterBase;
            waterAddPerSec = r.ReadSingle();

            
            workTemplate.readGameState(r, subversion, true);

            Debug.ReadCheck(r);

            readResources(r, subversion);
            
            readWorkerStatuses(r, false, -1, subversion);

            refreshCitySize();
            conscriptBuildings.Clear();
            int conscriptBuildingsCount = r.ReadUInt16();
            for (int i = 0; i < conscriptBuildingsCount; i++)
            {
                var barrack = new Conscript.BarracksStatus();
                barrack.readGameState(r, subversion);
                //check doublette
                if (!hasConscriptId(barrack.idAndPosition))
                {
                    conscriptBuildings.Add(barrack);
                }
            }
            
           
            deliveryServices.Clear();
            int deliveryServicesCount = r.ReadUInt16();
            for (int i = 0; i < deliveryServicesCount; i++)
            {
                DeliveryStatus status = new Delivery.DeliveryStatus();
                status.readGameState(r, subversion);
                deliveryServices.Add(status);
            }


            schoolBuildings.Clear();
            int schoolBuildingsCount = r.ReadUInt16();
            for (int i = 0; i < schoolBuildingsCount; i++)
            {
                XP.SchoolStatus status = new XP.SchoolStatus();
                status.readGameState(r, subversion);
                schoolBuildings.Add(status);
            }
            if (subversion >= 65)
            {
                researchBuildings = null;
                int researchBuildingsCount = r.ReadUInt16();
                if (researchBuildingsCount > 0)
                {
                    researchBuildings = new List<XP.ResearchBuilding>(8);
                    for (int i = 0; i < researchBuildingsCount; i++)
                    {
                        var building = new XP.ResearchBuilding();
                        building.readGameState(r, subversion);
                        researchBuildings.Add(building);
                    }
                }
            }

            if (subversion >= 85)
            {
                experenceOrDistance = (XP.ExperienceOrDistancePrio)r.ReadByte();
            }

            readSoldierGroups(r, subversion, pointers);
     
            defenceBuildings.Clear();
            int defenceBuildingsCount = r.ReadUInt16();
            for (int i = 0; i < defenceBuildingsCount; i++)
            {
                DefenceStatus defence = new DefenceStatus();
                defence.readGameState(r, subversion);
                if (defence.active)
                {
                    defenceBuildings.Add(defence);
                }
            }     
            
            int cesspitCount = r.ReadUInt16();
            if (cesspitCount > 0)
            {
                cesspits.Init(Bound.Min(cesspitCount, 4));
                for (int i = 0; i < cesspitCount; i++)
                { 
                    CesspitStatus cesspit = new CesspitStatus();
                    cesspit.readGameState(r, subversion);
                    cesspits.Add(cesspit);
                }
            }

            autoBuild_Work = r.ReadBoolean();
            autoBuild_Farm = r.ReadBoolean();
            autoExpandFarmType = (Build.BuildAndExpandType)r.ReadByte();

            tagBack = (CityTagBack)r.ReadByte();
            if (tagBack != CityTagBack.NONE)
            {
                tagArt = (CityTagArt)r.ReadUInt16();
            }

            res_food_safeguard = r.ReadBoolean();

            technology.readGameState(r, subversion, false);


            if (subversion < 53)
            {
                int gold = r.ReadInt32();
                money.copper = gold * 100;
            }
            else if (subversion < 67)
            {
                money.copper = r.ReadInt32();
            }
            else
            {
                money.read(r);
            }

            automateCity = r.ReadBoolean();
            automationFocus = (AutomationFocus)r.ReadByte();
            if (subversion >= 60)
            {
                warAutoQuality = (WarAutoQuality)r.ReadByte();
                warAutoWeaponType = (WarAutoWeaponType)r.ReadByte();
            }
            name.read(r, subversion);

            if (subversion >= 68)
            {
                casualCityProfile.readGameState(r, subversion);
                if (r.ReadBoolean())
                {
                    GetCasualProgress().readGameState(this, r, subversion);
                    //casualCityProfile.refreshTech(casualProgress);
                }
            }
            Debug.ReadCheck(r);
        }

        void writeStatusesStartEnd(int part, int workerStatusesCount, out bool meta, out int start, out int end)
        {
            if (part < 0)
            {
                meta = true;
                start = 0;
                end = workerStatusesCount;
            }
            else
            {
                meta = part == 0;
                start = part * MaxWorkerWriteCount;
                end = Math.Min(workerStatusesCount, start + MaxWorkerWriteCount);
            }
        }

        private void writeWorkerStatuses(BinaryWriter w, bool netPacket, int part)
        {
            Debug.WriteCheck(w);

            w.Write((ushort)workerStatuses.Count);
            writeStatusesStartEnd(part, workerStatuses.Count, out bool meta, out int start, out int end);

            if (meta)
            {   
                cityHallSubtilePos.write(w);
            }

            for (int i = start; i < end; i++)
            {
                workerStatuses[i].writeGameState(this, w, netPacket);
            }

            Debug.WriteCheck(w);
        }

        private void readWorkerStatuses(BinaryReader r, bool netPacket, int part, int subversion)
        {
            Debug.ReadCheck(r);

            IntVector2 startPos = WP.ToSubTilePos_Centered(tilePos);

            int workerStatusesCount = r.ReadUInt16();
            writeStatusesStartEnd(part, workerStatusesCount, out bool meta, out int start, out int end);
            
            if (meta)
            {
                if (subversion >= 65)
                {
                    cityHallSubtilePos.read(r);
                }
            }

            for (int i = start; i < end; i++)
            {
                WorkerStatus readWorker = new WorkerStatus()
                {
                    work = WorkType.Idle,
                    processTimeStartStampSec = Ref.TotalGameTimeSec,
                    subTileEnd = startPos,
                    subTileStart = startPos,
                };

                readWorker.readGameState(this, r, netPacket, subversion);

                if (i >= workerStatuses.Count)
                {
                    workerStatuses.Add(readWorker);
                }
                else
                {
                    workerStatuses[i] = readWorker;
                }
            }

            Debug.ReadCheck(r);
        }

        //TODO change on big update
        void writeResources(System.IO.BinaryWriter w)
        {
            w.Write((short)res_water.amount);
            //write(CityResoureIndex.wood);
            //write(CityResoureIndex.fuel);
            //write(CityResoureIndex.stone);
            //write(CityResoureIndex.rawFood);
            //write(CityResoureIndex.food);
            //write(CityResoureIndex.beer);
            //write(CityResoureIndex.coolingfluid);
            //write(CityResoureIndex.skinLinnen);

            //// Ores
            //write(CityResoureIndex.ironore);
            //write(CityResoureIndex.TinOre);
            //write(CityResoureIndex.CopperOre);
            //write(CityResoureIndex.LeadOre);
            //write(CityResoureIndex.SilverOre);
            //write(CityResoureIndex.GoldOre);

            //// Refined metals and materials
            //write(CityResoureIndex.iron);
            //write(CityResoureIndex.Tin);
            //write(CityResoureIndex.Copper);
            //write(CityResoureIndex.Lead);
            //write(CityResoureIndex.Silver);
            //write(CityResoureIndex.RawMithril);
            //write(CityResoureIndex.Sulfur);

            //// Alloys and special materials
            //write(CityResoureIndex.Bronze);
            //write(CityResoureIndex.Steel);
            //write(CityResoureIndex.CastIron);
            //write(CityResoureIndex.BloomeryIron);
            //write(CityResoureIndex.Mithril);

            //// Tools / construction
            //write(CityResoureIndex.Palisade);
            //write(CityResoureIndex.Toolkit);
            //write(CityResoureIndex.Wagon2Wheel);
            //write(CityResoureIndex.Wagon4Wheel);
            //write(CityResoureIndex.BlackPowder);
            //write(CityResoureIndex.GunPowder);
            //write(CityResoureIndex.LedBullet);

            //// Melee weapons
            //write(CityResoureIndex.sharpstick);
            //write(CityResoureIndex.BronzeSword);
            //write(CityResoureIndex.shortsword);
            //write(CityResoureIndex.Sword);
            //write(CityResoureIndex.LongSword);
            //write(CityResoureIndex.HandSpear);
            //write(CityResoureIndex.MithrilSword);

            //// Additional melee / ranged
            //write(CityResoureIndex.Warhammer);
            //write(CityResoureIndex.twohandsword);
            ////write(CityResoureIndex.knightslance);
            //write(CityResoureIndex.SlingShot);
            //write(CityResoureIndex.ThrowingSpear);
            //write(CityResoureIndex.bow);
            //write(CityResoureIndex.longbow);
            //write(CityResoureIndex.crossbow);
            //write(CityResoureIndex.MithrilBow);

            //// Firearms
            //write(CityResoureIndex.HandCannon);
            //write(CityResoureIndex.HandCulvertin);
            //write(CityResoureIndex.Rifle);
            //write(CityResoureIndex.Blunderbuss);

            //// Siege
            //write(CityResoureIndex.BatteringRam);
            //write(CityResoureIndex.ballista);
            //write(CityResoureIndex.Manuballista);
            //write(CityResoureIndex.Catapult);
            //write(CityResoureIndex.SiegeCannonBronze);
            //write(CityResoureIndex.ManCannonBronze);
            //write(CityResoureIndex.SiegeCannonIron);
            //write(CityResoureIndex.ManCannonIron);

            //// Armors
            //write(CityResoureIndex.paddedArmor);
            //write(CityResoureIndex.HeavyPaddedArmor);
            //write(CityResoureIndex.BronzeArmor);
            //write(CityResoureIndex.mailArmor);
            //write(CityResoureIndex.heavyMailArmor);
            //write(CityResoureIndex.LightPlateArmor);
            //write(CityResoureIndex.FullPlateArmor);
            //write(CityResoureIndex.MithrilArmor);


            for (int i = 0; i < CityResoureIndex.COUNT; ++i)
            {
                DssRef.world.cityResouces[resourceComponentStartIndex + i].writeGameState(w);
            }

            //void write(int cityResourceIndex)
            //{
            //    DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex].writeGameState(w);
            //}

            //res_wood.writeGameState(w); // ItemResourceType.Wood_Group
            //res_fuel.writeGameState(w); // ItemResourceType.Fuel_G
            //res_stone.writeGameState(w); // ItemResourceType.Stone_G
            //res_rawFood.writeGameState(w); // ItemResourceType.RawFood_Group
            //res_food.writeGameState(w); // ItemResourceType.Food_G
            //res_beer.writeGameState(w); // ItemResourceType.Beer
            //res_coolingfluid.writeGameState(w); // ItemResourceType.CoolingFluid
            //res_skinLinnen.writeGameState(w); // ItemResourceType.SkinLinen_Group

            //res_ironore.writeGameState(w); // ItemResourceType.IronOre_G
            //res_TinOre.writeGameState(w); // ItemResourceType.TinOre_G
            //res_CupperOre.writeGameState(w); // ItemResourceType.CopperOre_G
            //res_LeadOre.writeGameState(w); // ItemResourceType.LeadOre_G
            //res_SilverOre.writeGameState(w); // ItemResourceType.SilverOre_G
            //res_GoldOre.writeGameState(w);

            //res_iron.writeGameState(w); // ItemResourceType.Iron_G
            //res_Tin.writeGameState(w); // ItemResourceType.Tin_G
            //res_Cupper.writeGameState(w); // ItemResourceType.Copper_G
            //res_Lead.writeGameState(w); // ItemResourceType.Lead_G
            //res_Silver.writeGameState(w); // ItemResourceType.Silver_G
            //res_RawMithril.writeGameState(w); // ItemResourceType.RawMithril
            //res_Sulfur.writeGameState(w); // ItemResourceType.Sulfur

            //res_Bronze.writeGameState(w); // ItemResourceType.Bronze
            //res_Steel.writeGameState(w); // ItemResourceType.Steel
            //res_CastIron.writeGameState(w); // ItemResourceType.CastIron
            //res_BloomeryIron.writeGameState(w); // ItemResourceType.BloomeryIron
            //res_Mithril.writeGameState(w); // ItemResourceType.Mithril

            //res_Palisade.writeGameState(w);
            //res_Toolkit.writeGameState(w);
            //res_Wagon2Wheel.writeGameState(w); // ItemResourceType.Wagon2Wheel
            //res_Wagon4Wheel.writeGameState(w); // ItemResourceType.Wagon4Wheel
            //res_BlackPowder.writeGameState(w); // ItemResourceType.BlackPowder
            //res_GunPowder.writeGameState(w); // ItemResourceType.GunPowder
            //res_LedBullet.writeGameState(w); // ItemResourceType.LedBullet

            //res_sharpstick.writeGameState(w); // ItemResourceType.SharpStick
            //res_BronzeSword.writeGameState(w); // ItemResourceType.BronzeSword
            //res_shortsword.writeGameState(w); // ItemResourceType.ShortSword
            //res_Sword.writeGameState(w); // ItemResourceType.Sword
            //res_LongSword.writeGameState(w); // ItemResourceType.LongSword
            //res_HandSpear.writeGameState(w); // ItemResourceType.HandSpear
            //res_MithrilSword.writeGameState(w); // ItemResourceType.MithrilSword

            //res_Warhammer.writeGameState(w); // ItemResourceType.Warhammer
            //res_twohandsword.writeGameState(w); // ItemResourceType.TwoHandSword
            //res_knightslance.writeGameState(w); // ItemResourceType.KnightsLance
            //res_SlingShot.writeGameState(w); // ItemResourceType.SlingShot
            //res_ThrowingSpear.writeGameState(w); // ItemResourceType.ThrowingSpear
            //res_bow.writeGameState(w); // ItemResourceType.Bow
            //res_longbow.writeGameState(w); // ItemResourceType.LongBow
            //res_crossbow.writeGameState(w); // ItemResourceType.CrossBow
            //res_MithrilBow.writeGameState(w); // ItemResourceType.MithrilBow

            //res_HandCannon.writeGameState(w); // ItemResourceType.HandCannon
            //res_HandCulvertin.writeGameState(w); // ItemResourceType.HandCulvertin
            //res_Rifle.writeGameState(w); // ItemResourceType.Rifle
            //res_Blunderbuss.writeGameState(w); // ItemResourceType.Blunderbus

            //res_BatteringRam.writeGameState(w); // ItemResourceType.BatteringRam
            //res_ballista.writeGameState(w); // ItemResourceType.Ballista
            //res_Manuballista.writeGameState(w); // ItemResourceType.Manuballista
            //res_Catapult.writeGameState(w); // ItemResourceType.Catapult
            //res_SiegeCannonBronze.writeGameState(w); // ItemResourceType.SiegeCannonBronze
            //res_ManCannonBronze.writeGameState(w); // ItemResourceType.ManCannonBronze
            //res_SiegeCannonIron.writeGameState(w); // ItemResourceType.SiegeCannonIron
            //res_ManCannonIron.writeGameState(w); // ItemResourceType.ManCannonIron

            //res_paddedArmor.writeGameState(w); // ItemResourceType.LightArmor
            //res_HeavyPaddedArmor.writeGameState(w); // ItemResourceType.HeavyPaddedArmor
            //res_BronzeArmor.writeGameState(w); // ItemResourceType.BronzeArmor
            //res_mailArmor.writeGameState(w); // ItemResourceType.MediumArmor
            //res_heavyMailArmor.writeGameState(w); // ItemResourceType.HeavyArmor
            //res_LightPlateArmor.writeGameState(w); // ItemResourceType.LightPlateArmor
            //res_FullPlateArmor.writeGameState(w); // ItemResourceType.FullPlateArmor
            //res_MithrilArmor.writeGameState(w); // ItemResourceType.MithrilArmor
        }

        public void readResources(System.IO.BinaryReader r, int subversion)
        {
            res_water.amount = r.ReadInt16();

            //read(CityResoureIndex.wood);
            //read(CityResoureIndex.fuel);
            //read(CityResoureIndex.stone);
            //read(CityResoureIndex.rawFood);
            //read(CityResoureIndex.food);
            //read(CityResoureIndex.beer);
            //read(CityResoureIndex.coolingfluid);
            //read(CityResoureIndex.skinLinnen);

            //// Ores
            //read(CityResoureIndex.ironore);
            //read(CityResoureIndex.TinOre);
            //read(CityResoureIndex.CopperOre);
            //read(CityResoureIndex.LeadOre);
            //read(CityResoureIndex.SilverOre);
            //read(CityResoureIndex.GoldOre);

            //// Refined metals and materials
            //read(CityResoureIndex.iron);
            //read(CityResoureIndex.Tin);
            //read(CityResoureIndex.Copper);
            //read(CityResoureIndex.Lead);
            //read(CityResoureIndex.Silver);
            //read(CityResoureIndex.RawMithril);
            //read(CityResoureIndex.Sulfur);

            //// Alloys and special materials
            //read(CityResoureIndex.Bronze);
            //read(CityResoureIndex.Steel);
            //read(CityResoureIndex.CastIron);
            //read(CityResoureIndex.BloomeryIron);
            //read(CityResoureIndex.Mithril);

            //// Tools / construction
            //read(CityResoureIndex.Palisade);
            //read(CityResoureIndex.Toolkit);
            //read(CityResoureIndex.Wagon2Wheel);
            //read(CityResoureIndex.Wagon4Wheel);
            //read(CityResoureIndex.BlackPowder);
            //read(CityResoureIndex.GunPowder);
            //read(CityResoureIndex.LedBullet);

            //// Melee weapons
            //read(CityResoureIndex.sharpstick);
            //read(CityResoureIndex.BronzeSword);
            //read(CityResoureIndex.shortsword);
            //read(CityResoureIndex.Sword);
            //read(CityResoureIndex.LongSword);
            //read(CityResoureIndex.HandSpear);
            //read(CityResoureIndex.MithrilSword);

            //// Additional melee / ranged
            //read(CityResoureIndex.Warhammer);
            //read(CityResoureIndex.twohandsword);
            ////read(CityResoureIndex.knightslance);
            //read(CityResoureIndex.SlingShot);
            //read(CityResoureIndex.ThrowingSpear);
            //read(CityResoureIndex.bow);
            //read(CityResoureIndex.longbow);
            //read(CityResoureIndex.crossbow);
            //read(CityResoureIndex.MithrilBow);

            //// Firearms
            //read(CityResoureIndex.HandCannon);
            //read(CityResoureIndex.HandCulvertin);
            //read(CityResoureIndex.Rifle);
            //read(CityResoureIndex.Blunderbuss);

            //// Siege
            //read(CityResoureIndex.BatteringRam);
            //read(CityResoureIndex.ballista);
            //read(CityResoureIndex.Manuballista);
            //read(CityResoureIndex.Catapult);
            //read(CityResoureIndex.SiegeCannonBronze);
            //read(CityResoureIndex.ManCannonBronze);
            //read(CityResoureIndex.SiegeCannonIron);
            //read(CityResoureIndex.ManCannonIron);

            //// Armors
            //read(CityResoureIndex.paddedArmor);
            //read(CityResoureIndex.HeavyPaddedArmor);
            //read(CityResoureIndex.BronzeArmor);
            //read(CityResoureIndex.mailArmor);
            //read(CityResoureIndex.heavyMailArmor);
            //read(CityResoureIndex.LightPlateArmor);
            //read(CityResoureIndex.FullPlateArmor);
            //read(CityResoureIndex.MithrilArmor);

            for (int i = 0; i < CityResoureIndex.COUNT; ++i)
            {
                DssRef.world.cityResouces[resourceComponentStartIndex + i].readGameState(r, subversion);
            }

            //void read(int cityResourceIndex)
            //{
            //    DssRef.world.cityResouces[resourceComponentStartIndex + cityResourceIndex].readGameState(r, subversion);
            //}
            //res_wood.readGameState(r, subversion); // ItemResourceType.Wood_Group
            //res_fuel.readGameState(r, subversion); // ItemResourceType.Fuel_G
            //res_stone.readGameState(r, subversion); // ItemResourceType.Stone_G
            //res_rawFood.readGameState(r, subversion); // ItemResourceType.RawFood_Group
            //res_food.readGameState(r, subversion); // ItemResourceType.Food_G
            //res_beer.readGameState(r, subversion); // ItemResourceType.Beer
            //res_coolingfluid.readGameState(r, subversion); // ItemResourceType.CoolingFluid
            //res_skinLinnen.readGameState(r, subversion); // ItemResourceType.SkinLinen_Group

            //res_ironore.readGameState(r, subversion); // ItemResourceType.IronOre_G
            //res_TinOre.readGameState(r, subversion); // ItemResourceType.TinOre_G
            //res_CupperOre.readGameState(r, subversion); // ItemResourceType.CopperOre_G
            //res_LeadOre.readGameState(r, subversion); // ItemResourceType.LeadOre_G
            //res_SilverOre.readGameState(r, subversion); // ItemResourceType.SilverOre_G
            //res_GoldOre.readGameState(r, subversion);


            //res_iron.readGameState(r, subversion); // ItemResourceType.Iron_G
            //res_Tin.readGameState(r, subversion); // ItemResourceType.Tin_G
            //res_Cupper.readGameState(r, subversion); // ItemResourceType.Copper_G
            //res_Lead.readGameState(r, subversion); // ItemResourceType.Lead_G
            //res_Silver.readGameState(r, subversion); // ItemResourceType.Silver_G
            //res_RawMithril.readGameState(r, subversion); // ItemResourceType.RawMithril
            //res_Sulfur.readGameState(r, subversion); // ItemResourceType.Sulfur

            //res_Bronze.readGameState(r, subversion); // ItemResourceType.Bronze
            //res_Steel.readGameState(r, subversion); // ItemResourceType.Steel
            //res_CastIron.readGameState(r, subversion); // ItemResourceType.CastIron
            //res_BloomeryIron.readGameState(r, subversion); // ItemResourceType.BloomeryIron
            //res_Mithril.readGameState(r, subversion); // ItemResourceType.Mithril

            //res_Palisade.readGameState(r, subversion);

            //res_Toolkit.readGameState(r, subversion);
            //res_Wagon2Wheel.readGameState(r, subversion); // ItemResourceType.Wagon2Wheel
            //res_Wagon4Wheel.readGameState(r, subversion); // ItemResourceType.Wagon4Wheel
            //res_BlackPowder.readGameState(r, subversion); // ItemResourceType.BlackPowder
            //res_GunPowder.readGameState(r, subversion); // ItemResourceType.GunPowder
            //res_LedBullet.readGameState(r, subversion); // ItemResourceType.LedBullet

            //res_sharpstick.readGameState(r, subversion); // ItemResourceType.SharpStick
            //res_BronzeSword.readGameState(r, subversion); // ItemResourceType.BronzeSword
            //res_shortsword.readGameState(r, subversion); // ItemResourceType.ShortSword
            //res_Sword.readGameState(r, subversion); // ItemResourceType.Sword
            //res_LongSword.readGameState(r, subversion); // ItemResourceType.LongSword
            //res_HandSpear.readGameState(r, subversion); // ItemResourceType.HandSpear
            //res_MithrilSword.readGameState(r, subversion); // ItemResourceType.MithrilSword

            //res_Warhammer.readGameState(r, subversion); // ItemResourceType.Warhammer
            //res_twohandsword.readGameState(r, subversion); // ItemResourceType.TwoHandSword
            //res_knightslance.readGameState(r, subversion); // ItemResourceType.KnightsLance
            //res_SlingShot.readGameState(r, subversion); // ItemResourceType.SlingShot
            //res_ThrowingSpear.readGameState(r, subversion); // ItemResourceType.ThrowingSpear
            //res_bow.readGameState(r, subversion); // ItemResourceType.Bow
            //res_longbow.readGameState(r, subversion); // ItemResourceType.LongBow
            //res_crossbow.readGameState(r, subversion); // ItemResourceType.CrossBow
            //res_MithrilBow.readGameState(r, subversion); // ItemResourceType.MithrilBow

            //res_HandCannon.readGameState(r, subversion); // ItemResourceType.HandCannon
            //res_HandCulvertin.readGameState(r, subversion); // ItemResourceType.HandCulvertin
            //res_Rifle.readGameState(r, subversion); // ItemResourceType.Rifle
            //res_Blunderbuss.readGameState(r, subversion); // ItemResourceType.Blunderbus

            //res_BatteringRam.readGameState(r, subversion); // ItemResourceType.BatteringRam
            //res_ballista.readGameState(r, subversion); // ItemResourceType.Ballista
            //res_Manuballista.readGameState(r, subversion); // ItemResourceType.Manuballista
            //res_Catapult.readGameState(r, subversion); // ItemResourceType.Catapult
            //res_SiegeCannonBronze.readGameState(r, subversion); // ItemResourceType.SiegeCannonBronze
            //res_ManCannonBronze.readGameState(r, subversion); // ItemResourceType.ManCannonBronze
            //res_SiegeCannonIron.readGameState(r, subversion); // ItemResourceType.SiegeCannonIron
            //res_ManCannonIron.readGameState(r, subversion); // ItemResourceType.ManCannonIron

            //res_paddedArmor.readGameState(r, subversion); // ItemResourceType.LightArmor
            //res_HeavyPaddedArmor.readGameState(r, subversion); // ItemResourceType.HeavyPaddedArmor
            //res_BronzeArmor.readGameState(r, subversion); // ItemResourceType.BronzeArmor
            //res_mailArmor.readGameState(r, subversion); // ItemResourceType.MediumArmor
            //res_heavyMailArmor.readGameState(r, subversion); // ItemResourceType.HeavyArmor
            //res_LightPlateArmor.readGameState(r, subversion); // ItemResourceType.LightPlateArmor
            //res_FullPlateArmor.readGameState(r, subversion); // ItemResourceType.FullPlateArmor
            //res_MithrilArmor.readGameState(r, subversion); // ItemResourceType.MithrilArmor
        }

        public void writeNet_map(System.IO.BinaryWriter w)
        {
            writeMapFile(w);

            w.Write((ushort)factionIndex);

            w.Write((byte)Tile().heightLevel);
        }
        public void readNet_map(WorldData world, System.IO.BinaryReader r)
        {
            readMapFile(world, r, int.MaxValue);
            //guardCount = r.ReadUInt16();
            //maxGuardSize = r.ReadUInt16();

            factionIndex = r.ReadUInt16();
            //faction = DssRef.world.factions[factionIx];

            onGameStart(false);
            int height = r.ReadByte();
            var tile = new Tile();
            tile.heightLevel = height;
            position.Y = tile.ModelGroundY();
            if (overviewModel != null)
            {
                overviewModel.position = position;
            }

            DssRef.world.unitCollAreaGrid.add(this);
        }

        

        public bool net_roundtrip_asyncupdate()
        {
            if (lastNetUpdate.secPassed(10))
            {
                lastNetUpdate.setNow();

                int count = MathExt.Div_Ceiling(workerStatuses.Count, MaxWorkerWriteCount) + 1;

                for (int part = 0; part < count; ++part)
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssCityStatus, Network.PacketReliability.Reliable, out var packet);
                    {
                        w.Write((ushort)myIndex);
                        w.Write((byte)part);
                        writeNet_update(w, part);                        
                    }
                    packet.CheckPacketLength();
                    packet.EndWrite_Asynch();
                }
                return true;
            }

            return false;
        }

        public void writeNet_update(System.IO.BinaryWriter w, int part)
        {
            switch (part)
            {
                case 0:
                    workTemplate.writeGameState(w);
                    writeResources(w);
                    break;

                default:
                    writeWorkerStatuses(w, true, part -1);
                    break;
            }
            

           
        }
        public void readNet_update(System.IO.BinaryReader r, int part)
        {
            switch (part)
            {
                case 0:
                    workTemplate.readGameState(r, int.MaxValue, true);

                    readResources(r, int.MaxValue);
                    break;

                default:
                    readWorkerStatuses(r, true, part - 1, int.MaxValue);
                    break;
            }
            

           
        }

        override public void tagSprites(out SpriteName back, out SpriteName art)
        {
            back = Data.CityTag.BackSprite(tagBack);
            art = Data.CityTag.ArtSprite(tagArt);
        }


        public int expandWorkForceCost()
        {
            return 40000 + HousingCount_Workers * 10;
        }

      
        const int WorkerHutsPerTile = 4;
        const int WorkerHutsPerTile_MaxLevel = WorkerHutsPerTile * HutMaxLevel;
        public const int WorkersPerTile = DssConst.HousingCount_WorkerHut * WorkerHutsPerTile * HutMaxLevel;
        public const int HutMaxLevel = 2;
        int totalWorkerHutAndLevelCount = 0;
        public void refreshWorkerSubtiles()
        {
           
            int goalDisplayCount = WorkersToModelsCount(HousingCount_Workers);
            if (goalDisplayCount > totalWorkerHutAndLevelCount)
            {
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(tilePos, 1));
                        edgeLoop.RandomPosition(true);

                        int maxLoops = 10000;

                        while (goalDisplayCount > totalWorkerHutAndLevelCount)
                        {
                            if (edgeLoop.Next())
                            {

                                if (DssRef.world.tileGrid.TryGet(edgeLoop.Position, out Tile t) &&
                                        t.MayBuild() && t.CityIndex == myIndex)
                                {
                                    const int SubStartTrialCount = 4;
                                    IntVector2 topLeft = WP.ToSubTilePos_TopLeft(edgeLoop.Position);

                                    for (int trialIx = 0; trialIx < SubStartTrialCount; ++trialIx)
                                    {
                                        IntVector2 subPos = topLeft;
                                        subPos.X += Ref.peRnd.Int(1, WorldData.TileSubDivitions - 1);
                                        subPos.Y += Ref.peRnd.Int(1, WorldData.TileSubDivitions - 1);


                                        if (Build.BuildLib.TryAutoBuild(subPos, TerrainMainType.Building, (int)TerrainBuildingType.WorkerHut, 1))
                                        {
                                            ++totalWorkerHutAndLevelCount;

                                            //Place farm curlutures
                                            const int CulturesPerFarm = 10;
                                            int cultureCount = 0;

                                            ForXYEdgeLoop farmLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(subPos, 1));
                                            farmLoop.RandomPosition(true);


                                            while (cultureCount < CulturesPerFarm)
                                            {
                                                while (farmLoop.Next())
                                                {
                                                    TerrainMainType terrain;
                                                    int sub;
                                                    int maxAmount;
                                                    //if (Ref.peRnd.Chance(0.75))
                                                    //{
                                                    //    terrain = TerrainMainType.Foil;
                                                    //    sub = (int)TerrainSubFoilType.WheatFarm;
                                                    //    maxAmount = TerrainContent.FarmCulture_MaxSize;
                                                    //}
                                                    //else
                                                    //{
                                                    //    terrain = TerrainMainType.Building;
                                                    //    //if (Ref.peRnd.Chance(0.4))
                                                    //    //{
                                                    //    //    sub = (int)TerrainBuildingType.PigPen;
                                                    //    //    maxAmount = TerrainContent.PigMaxSize;
                                                    //    //}
                                                    //    //else
                                                    //    //{
                                                    //        sub = (int)TerrainBuildingType.HenPen;
                                                    //        maxAmount = TerrainContent.HenGrowth.maxSize;
                                                    //    //}
                                                    //}
                                                    terrain = TerrainMainType.Foil;
                                                    sub = (int)TerrainSubFoilType.TreeApple;
                                                    maxAmount = TerrainContent.OrchardReady;
                                                        //sub = (int)TerrainSubFoilType.WheatFarm;
                                                        //maxAmount = TerrainContent.FarmCulture_MaxSize;
                                                    //}
                                                    //else
                                                    //{
                                                    //    terrain = TerrainMainType.Building;
                                                    //    if (Ref.peRnd.Chance(0.4))
                                                    //    {
                                                    //        sub = (int)TerrainBuildingType.PigPen;
                                                    //        maxAmount = TerrainContent.PigMaxSize;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        sub = (int)TerrainBuildingType.HenPen;
                                                    //        maxAmount = TerrainContent.HenMaxSize;
                                                    //    }
                                                    //}

                                                    if (Build.BuildLib.TryAutoBuild(farmLoop.Position, terrain, sub, Ref.peRnd.Int(1, maxAmount)))
                                                    {
                                                        ++cultureCount;
                                                        if (cultureCount >= CulturesPerFarm)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }

                                                farmLoop.ExpandRadius();
                                                farmLoop.RandomPosition(true);

                                                if (--maxLoops < 0)
                                                {
                                                    return;
                                                }
                                            }

                                            if (goalDisplayCount <= totalWorkerHutAndLevelCount)
                                            {
                                                return;
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                edgeLoop.ExpandRadius();
                                edgeLoop.RandomPosition(true);
                            }


                            if (--maxLoops < 0)
                            {
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                    
                });



            }
        }

        static int WorkersToModelsCount(int workers)
        {
            return (int)Math.Floor(workers / (double)DssConst.HousingCount_WorkerHut);
        }

        public void onWorkHutBuild(bool build_notDestroy, int size)
        {
            //int count = large ? DssConst.HousingCount_WorkerHutLarge : DssConst.HousingCount_WorkerHut;
            if (build_notDestroy)
            {
                HousingCount_Workers += size;
            }
            else
            {
                HousingCount_Workers -= size;
            }
            //refreshCitySize();
        }

        public void onServiceHouseBuild(bool build_notDestroy, bool large)
        {
            int count = large ? DssConst.HousingCount_ServiceHouse_Large : DssConst.HousingCount_ServiceHouse_Small;
            if (build_notDestroy)
            {
                freeServiceMen.amount += count;
                workingAndFreeServiceMen += count;
            }
            else
            {
                freeServiceMen.amount -= count;
                workingAndFreeServiceMen -= count;
            }
        }

        public void onGuardHouseBuild(bool build_notDestroy, bool large)        
        {
            int count = large ? DssConst.HousingCount_GuardsOffice_Large : DssConst.HousingCount_GuardsOffice_Small;
            if (build_notDestroy)
            {
                HousingCount_Guard += count;
            }
            else
            {
                HousingCount_Guard -= count;
            }
        }

        public void onNobelHouseBuild(bool build_notDestroy, int count)
        {
            if (build_notDestroy)
            {
                HousingCount_NobelMen += count;
            }
            else
            {
                HousingCount_NobelMen -= count;
            }
        }
        //public void useServiceMen(int useInServiceCount)
        //{ 
        //    freeServiceMen.amount -= useInServiceCount;
        //    workingServiceMen += useInServiceCount;
        //}

        //public void expandGuardSize(int amount)
        //{
        //    maxGuardSize += amount;
        //    refreshCitySize();
        //}

        //public void releaseGuardSize(int totalAmount)
        //{
        //    maxGuardSize -= totalAmount;
        //    if (guardCount > maxGuardSize)
        //    {
        //        int releasedWorkers = guardCount - maxGuardSize;
        //        guardCount = maxGuardSize;
        //        addWorkers(releasedWorkers);

        //        faction.gainMoney(DssConst.ReleaseGuardSizeGain, this);
        //    }
        //}

        //public bool buyCityGuards(bool commit, int count)
        //{
        //    if (canIncreaseGuardSize(count, false))
        //    {
        //        int totalCost = 0;

        //        if (faction.calcCost(DssConst.ExpandGuardSizeCost * count, ref totalCost, this))
        //        {
        //            if (commit)
        //            {
        //                expandGuardSize(DssConst.ExpandGuardSize * count);
        //                faction.payMoney(totalCost, true, this);
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public bool releaseCityGuards(bool commit, int count)
        //{
        //    if (canReleaseGuardSize(count))
        //    {
        //            if (commit)
        //            {
        //                (DssConst.ExpandGuardSize * count);
        //                faction.payMoney(totalCost, true);
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public bool buyRepair(bool commit, bool all)
        //{
        //    if (damages.HasValue())
        //    {
        //        int cost;
        //        int count;

        //        repairCountAndCost(all, out count, out cost);

        //        int totalCost = 0;
        //        if (faction.hasMoney(cost, this))
        //        {
        //            if (commit)
        //            {
        //                damages.value -= count;
        //                faction.payMoney(cost, true, this);
        //            }
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public void burnItDown()
        //{
        //    damages.value = MaxDamages();
        //    workForce.amount = 0;
        //}

        public double MaxDamages()
        {
            return HousingCount_Workers * 0.75;
        }

        //public void repairCountAndCost(bool all, out int count, out int cost)
        //{
        //    const double BuyToRepair = 0.75;
        //    count = damages.Int();
        //    cost = 0;

        //    if (count > 0)
        //    {
        //        if (!all && count > DssConst.ExpandWorkForce)
        //        {
        //            count = DssConst.ExpandWorkForce;
        //        }

        //        cost = Convert.ToInt32(((double)expandWorkForceCost() / DssConst.ExpandWorkForce * count) * BuyToRepair);
        //    }
        //}



        //public float GuardUpkeep()
        //{
        //    return soldiersCount * DssConst.UpkeepPerGuard;
        //}

        public void onGameStart(bool newGame)
        {


            groupRadius = 0.6f;

            //initEconomy(newGame, DssRef.world);
            for (StorageType storageType = 0; storageType < StorageType.NUM_NONE; storageType++)
            {
                refreshStorageSize(storageType);
            }

            CalcRecruitToTile();
            armyGoalRotation = rotation.radians;

            position = new Vector3(tilePos.X, Tile().ModelGroundY(), tilePos.Y);
            refreshCitySize();
                        
            if (newGame && cityType > CityType.UnClaimed)
            {
                refreshWorkerSubtiles();
                int freeGuardSpace = 0;
                
                for (int i = 0;i <defenceBuildings.Count;i++) 
                {
                    var post = defenceBuildings[i];
                    if (post.autoAssign)
                    {
                        newGamePlaceGuard(post.idAndPosition, i);
                        if (soldiersCount + freeGuardSpace >= HousingCount_Guard)
                        {
                            break;
                        }
                    }
                }

                setAllDefenceAutoAssign(true, false, false);
            }
            
            if (!name.custom)
            {
                name.name = Data.NameGenerator.CityName(tilePos);
            }
            setTimeOnAllWorkers();

            setTimeOnAllWorkers();
        }

        

        void initEconomy(/*bool newGame,*/ WorldData world)
        {
            workTemplate.initComponents(true, world.cityWork, WorkTemplate.COUNT * myIndex);

            //if (newGame)
            {
                money.AddCopper(500);

                switch (cityType)
                {
                    case CityType.Campsite:
                        HousingCount_Workers = DssConst.CampsiteCityStartMaxWorkForce;
                        waterAddPerSec = Ref.rnd.Float(DssConst.WaterAdd_SmallCity, DssConst.WaterAdd_HeadCity);
                        HousingCount_Guard += DssConst.CampHall_GuardHousing;
                        break;
                    case CityType.Village:
                        HousingCount_Workers = DssConst.SmallCityStartMaxWorkForce;
                        waterAddPerSec = DssConst.WaterAdd_SmallCity;
                        HousingCount_Guard += DssConst.VillageHall_GuardHousing;
                        break;
                    case CityType.Town:
                        HousingCount_Workers = DssConst.LargeCityStartMaxWorkForce;
                        waterAddPerSec = DssConst.WaterAdd_LargeCity;
                        HousingCount_Guard += DssConst.TownHall_GuardHousing;
                        break;
                    default:
                        HousingCount_Workers = DssConst.HeadCityStartMaxWorkForce;
                        waterAddPerSec = DssConst.WaterAdd_HeadCity;
                        HousingCount_Guard += DssConst.CapitalHall_GuardHousing;
                        break;
                }
                workForce.amount = (int)(HousingCount_Workers * 0.75);
                waterAddPerSec += Ref.rnd.Float(DssConst.WaterAdd_RandomAdd);

                if (Culture == CityCulture.DeepWell)
                {
                    waterAddPerSec += DssConst.WaterAdd_HeadCity;
                }

                waterAddPerSec *= DssRef.difficulty.setting_waterMulti;
                maxWaterBase = Convert.ToInt32( DssConst.Maxwater * DssRef.difficulty.setting_waterMulti);
                maxWaterTotal = maxWaterBase;
                casualCityProfile.maxHuts = MathExt.MultiplyInt(maxWaterTotal, 0.66);

                defaultResourceBuffer(world);
            }

            
        }

        

        public bool claimCity(Faction faction, IntVector2 subtile)
        {
            if (cityType == CityType.UnClaimed && faction != null)
            {
                Task.Run(() =>
                {
                    try
                    {
                        DssRef.world.clearCityResources(this);

                        const int TentCount = 4;
                        foreach (var item in Build.CraftBuildingLib.WorkerTent.resources)
                        {
                            SetGroupedResource(item.type, TentCount * item.amount);
                        }
                        SetGroupedResource(ItemResourceType.Iron_G, 20);
                        SetGroupedResource(ItemResourceType.Food_G, ConscriptDataLib.CraftSettlerFood);
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                });

                workForce.amount = DssConst.HousingCount_WorkerTent;
                HousingCount_Workers = DssConst.HousingCount_WorkerTent;
                HousingCount_Guard = DssConst.CampHall_GuardHousing;

                bool newTile = cityHallSubtilePos != subtile;
                cityType = CityType.Campsite;
                
                if (newTile)
                {
                    IntVector2 prevTilePos = tilePos;
                    tilePos = WP.SubtileToTilePos(subtile);

                    ref var prevTile = ref DssRef.world.tileGrid.GetRef(prevTilePos);
                    ref var tile = ref DssRef.world.tileGrid.GetRef(tilePos);

                    prevTile.tileContent = TileContent.NONE;
                    tile.tileContent = TileContent.City;
                    position = WP.ToWorldPos(tilePos, tile.ModelGroundY());
                }

                createCampSite(subtile);

                setFaction(faction, false, false);
                refreshCitySize();

                if (!name.custom)
                {
                    name.name = Data.NameGenerator.CityName(tilePos);
                }

                if (faction.player.IsLocalPlayer())
                {
                    faction.player.GetLocalPlayer().statistics.onCityFound();
                }

                return true;
            }
            return false;
        }

        void refreshCitySize()
        {
            switch (cityType)
            {
                case CityType.Campsite:
                    WorkersMaxLimit = DssConst.CampHall_MaxWorkForce;
                    break;
                case CityType.Village:
                    WorkersMaxLimit = DssConst.VillageHall_MaxWorkForce;
                    break;
                case CityType.Town:
                    WorkersMaxLimit = DssConst.TownHall_MaxWorkForce;
                    break;
                default:
                    WorkersMaxLimit = int.MaxValue;
                    break;
            }

            float iconScale = IconScale();

            VectorVolumeC volume = new VectorVolumeC(position,
                new Vector3(iconScale * 0.5f, 0.1f, iconScale * 0.5f));
            bound = volume.boundingBox();

            refreshVisualSize();
        }

        void refreshVisualSize()
        {
            if (overviewModel != null)
            {
                overviewModel.scale = VectorExt.V3(IconScale() * overviewModel.OneBlockScale);
            }
        }

        public bool canEverGetNobelHouse()
        {
            return true;//maxEpandWorkSize >= DssLib.NobelHouseWorkForceReqiurement;
        }

     
        public bool hasNeededAreaSize()
        {
            int maxFit = WorkersPerTile * HutMaxLevel * areaSize;
            return HousingCount_Workers + 2 <= maxFit;
        }

        void createOverViewModel()
        {
            //faction.profile.modelColorReplace
            var faction = GetFaction_NoChecks();
            if (faction == null)
            {
                setModel(new Graphics.VoxelModelInstance(DssRef.models.voxelModels[LootFest.VoxelModelName.unclaimed_icon], false) { scale = new Vector3(0.06f) });
            }
            else if (faction.player.profile.flag != null)
            {
                setModel(faction.AutoLoadModelInstance(
                   LootFest.VoxelModelName.cityicon, IconScale()));
                //overviewModel?.DeleteMe();
            }
            //if (faction.player?.profile.flag != null)
            //{
                //overviewModel = faction.AutoLoadModelInstance(
                //   LootFest.VoxelModelName.cityicon, IconScale());
                //overviewModel.AddToRender(DrawGame.MidLayer);
                //overviewModel.position = position;
            //}

            void setModel(Graphics.AbsVoxelObj model)
            {
                overviewModel?.DeleteMe();

                overviewModel = model;
                overviewModel.AddToRender(DrawGame.MidLayer);
                overviewModel.position = position;
            }
        }

        float IconScale()
        {
            switch (cityType)
            {
                case CityType.UnClaimed:
                    return 1f;
                case CityType.Campsite:
                    return 0.64f;
                case CityType.Village:
                    return 0.7f;
                case CityType.Town:
                    return 1f;
                default:
                    return 1.3f;
                //case CityType.Factory:
                //    return 1.5f;
            }
        }

        //public void updateIncome_asynch()
        //{
        //    totalIncome = Convert.ToInt32(Math.Floor(workForce.value * TaxPerWorker - upkeep - blackMarketCosts.displayValue_sec));
        //}

        

        public void onNewModel(LootFest.VoxelModelName name, Graphics.VoxelModel master)
        {
            //detailObj.model?.onNewModel(name, master, detailObj);
            DSSWars.Faction.SetNewMaster(name, VoxelModelName.cityicon, overviewModel, master);
        }

        public void update()
        {
            if (myIndex == 441)
            {
                lib.DoNothing();
            }
            updateDetailLevel();

            //detailObj.update(Ref.DeltaGameTimeMs, true);

            if (inRender_detailLayer)
            {
                updateArmyMembers(Ref.DeltaGameTimeMs, true);
            }

            updateWorkerUnits();
        }

        public void update_client()
        {
            updateDetailLevel();
        }

        public int income_oneSecUpdate(double incomeMultiplier)
        {
            CityEconomyData cityEconomy = new CityEconomyData(this);
            
            int income = GetCasual()? cityEconomy.IncomeAndUpkeep_Total_Casual() : cityEconomy.IncomeAndUpkeep_Total();
            previousIncome_copp = income;
            money.copper += income;

            return income;
        }

        //public CityEconomyData calcIncome_async()
        //{
        //    return new CityEconomyData()
        //    {
        //        workerCount = workForce.amount,//tax = workForce.value * TaxPerWorker,

        //        //cityGuardUpkeep = GuardUpkeep(maxGuardSize),
        //        blackMarketCosts_Food_gold = blackMarketCosts_food.displayValue_gold_sec,
        //    };
        //}

        public override void asynchCullingUpdate(float time, bool bStateA)
        {
            Intvector2MinMax minMax = workerCullingMinMax;
            minMax.Combine(guardCullingMinMax);
            if (myIndex == 316)
            {
                lib.DoNothing();
                //var tp = this.tilePos;
            }
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, bStateA, ref minMax.min, ref minMax.max);
        }

        public double childAddPerSec()
        {
            bool requirements = !inBattle;

            if (GetCasual())
            {
                requirements &= workForce.amount < HousingCount_Workers;
            }
            else
            {
                requirements &= resourceAmount(CityResoureIndex.food)/*res_food.amount*/ > 0 &&
                    homeUsers() < workersMax();
            }

            if (requirements)
            {
                var result = Bound.Min( workForce.amount / 600.0 * GetFaction().growthMultiplier, 0.1);
                if (Culture == CityCulture.LargeFamilies)
                {
                    result *= 2;
                }
                return result * DssRef.difficulty.setting_childMulti;
            }
            return 0;
        }

        public bool isMaxHomeUsers()
        {
            return homeUsers() >= HousingCount_Workers;
        }

        public int homesUnused()
        {
            return HousingCount_Workers - homeUsers();
        }

        public int workersMax()
        {
            return Bound.Max( HousingCount_Workers, WorkersMaxLimit);
        }

        public int homeUsers()
        {
            return workForce.amount + children();
        }

        public int children()
        {
            return childrenAge0.Int() + childrenAge1;
        }

        public void oneSecUpdate(bool minute)
        {
            const int MinWorkforce = 8;

            int addWorkers = 0;

            childrenAge0.value += childAddPerSec();

            if (DssRef.time.oneMinute)
            {
                addWorkers = childrenAge1;
                childrenAge1 = childrenAge0.pull();
                
                if (workForce.amount < MinWorkforce)
                {
                    addWorkers += MinWorkforce - workForce.amount;
                }
            }

            if (!inBattle)
            {
                if (immigrants.HasValue())
                {
                    if (workForce.amount + addWorkers < HousingCount_Workers)
                    {
                        var immigrantsToWork = immigrants.pull(DssConst.ImmigrantsTransfereSpeed + DssConst.ImmigrantionTent_TransfereSpeedBonus * buildingStructure.ImmigrationTent_count);
                        addWorkers += immigrantsToWork;
                    }

                    immigrants.reduceTowardsMinValue(DssConst.ImmigrantsRemovePerSec, DssConst.ImmigrantionTent_Capacity * buildingStructure.ImmigrationTent_count);
                }
            }

            workForce.amount = Bound.Max(workForce.amount + addWorkers, HousingCount_Workers);

                        
            nextWater.value += waterAddPerSec;
            maxWaterTotal = maxWaterBase + buildingStructure.WaterResovoir_count * DssConst.WaterResovoirWaterAdd;
            res_water.amount = Math.Min(res_water.amount + nextWater.pull(), maxWaterTotal);

            if (starving)
            {
                starvingTimeSeconds++;
                
                if (starvingTimeSeconds > 15)
                {
                    starvingTimeSeconds = -30;
                    starving = false;

                    var faction = GetFaction();
                    if (faction.player.IsLocalPlayer())
                    {
                        faction.player.GetLocalPlayer().hud.messages.cityLowFoodMessage(this);
                    }
                }
            }
            else
            {
                starvingTimeSeconds = 0;
            }

            if (strengthValue == 0 || capturePoints < 0)
            {
                capturePoints += 10;
            }

            if (capturePoints >= 100)
            {
                //Power check
                cityCaptureCheck();
                capturePoints = -100;                
            }

            casualProgress?.oneSecondUpdate(this);

            if (minute)
            { 
                float addNobel = HousingCount_NobelMen * DssConst.NobelHouseMenAddSpeed_PerManHouse;
                freeNobelMen.amount = Bound.Max(freeNobelMen.amount + Convert.ToInt32(addNobel), HousingCount_NobelMen);

                AddGroupedResource(ItemResourceType.RawFood_Group, -PenFoodUpkeep_minute);
            }
        }

        void cityCaptureCheck()
        {
            Task.Run(() =>
            {
                try
                {
                    Faction faction = GetFaction();
                    Faction newOwner =  DssRef.world.unitCollAreaGrid.cityCaptureCheck(this, strengthValue > 0 ? 0 : 2);
                    if (newOwner != faction && newOwner != null)                    
                    {
                        Ref.update.AddSyncAction(new SyncAction(() =>
                        {
                            if (newOwner.isAlive)
                            {
                                if (faction != null && faction.player.IsLocalPlayer())
                                {
                                    ++faction.player.GetLocalPlayer().statistics.CitiesLost;
                                }
                                if (newOwner.player.IsLocalPlayer())
                                {
                                    ++newOwner.player.GetLocalPlayer().statistics.CitiesCaptured;
                                }

                                setFaction(newOwner, false, false);
                            }
                        }));
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
                
            });

            //        void ExitBattle()
            //        {
            //            if (members.Count == 0)
            //            { return; }

            //            List<City> cities = new List<City>(2);
            //            Dictionary<int, float> cityDominationStrength = new Dictionary<int, float>(4);

            //            var membersC = MembersCounter();
            //            while (membersC.Next())
            //            {
            //                if (membersC.sel.gameobjectType() == GameObjectType.City)
            //                {
            //                    cities.Add(membersC.sel.GetCity());
            //                }

            //                if (cityDominationStrength.ContainsKey(membersC.sel.faction.parentArrayIndex))
            //                {
            //                    cityDominationStrength[membersC.sel.faction.parentArrayIndex] += membersC.sel.strengthValue;
            //                }
            //                else
            //                {
            //                    cityDominationStrength.Add(membersC.sel.faction.parentArrayIndex, membersC.sel.strengthValue);
            //                }

            //                membersC.sel.ExitBattleGroup();
            //            }

            //            int strongestFaction = -1;
            //            float strongest = float.MinValue;

            //            foreach (var kv in cityDominationStrength)
            //            {
            //                if (kv.Value > strongest)
            //                {
            //                    strongestFaction = kv.Key;
            //                    strongest = kv.Value;
            //                }
            //            }

            //            var dominatingFaction = DssRef.world.factions.Array[strongestFaction];

            //            if (cities.Count > 0)
            //            {
            //                foreach (var c in cities)
            //                {
            //                    if (DssRef.diplomacy.InWar(c.faction, dominatingFaction))
            //                    {
            //                        if (c.faction.player.IsPlayer())
            //                        {
            //                            ++c.faction.player.GetLocalPlayer().statistics.CitiesLost;
            //                        }
            //                        if (dominatingFaction.player.IsPlayer())
            //                        {
            //                            ++dominatingFaction.player.GetLocalPlayer().statistics.CitiesCaptured;
            //                        }

            //                        Ref.update.AddSyncAction(new SyncAction1Arg<Faction>(c.setFaction, dominatingFaction));
            //                    }
            //                }
            //            }

            //            for (int i = 0; i < factions.Count; ++i)
            //            {
            //                var f = factions[i];

            //                bool winner = f == dominatingFaction || !DssRef.diplomacy.InWar(f, dominatingFaction);

            //                if (f.player.IsPlayer())
            //                {
            //                    var p = f.player.GetLocalPlayer();
            //                    p.battles.Remove(this);
            //                    if (winner)
            //                    {
            //                        p.statistics.BattlesWon++;
            //                    }
            //                    else
            //                    {
            //                        p.statistics.BattlesLost++;
            //                    }
            //                }
            //            }
            //        }
        }

        public void addWorkers(int add)
        {
            if (workForce.amount + add > HousingCount_Workers)
            {
                //Add rest to immigration
                int rest = workForce.amount + add - HousingCount_Workers;
                workForce.amount = HousingCount_Workers;
                immigrants.value += rest;
            }
            else
            {
                workForce.amount += add;
            }
        }

        public void asynchGameObjectsUpdate(bool minute)
        {
            //collectBattles_asynch();
            //detailObj.asynchUpdate();
            //strength
            //strengthValue = 2.5f * guardCount / DssConst.SoldierGroup_DefaultCount;
            async_SoldiersUpdate(minute);

            if (minute)
            {
                blackMarketCosts_food.minuteUpdate();
                foodProduction.minuteUpdate();
                foodSpending.minuteUpdate();
                soldResources.minuteUpdate();
            }
        }

        static Dictionary<int, float> CityDominationStrength = new Dictionary<int, float>(4);


        public override void asyncNearObjectsUpdate()
        {
            base.asyncNearObjectsUpdate();
       
            float armyDefence = 0;
            const int DominanceTileRadius = 4;

            Faction faction = GetFaction();
            DssRef.world.unitCollAreaGrid.collectArmies(factionIndex, tilePos, 2,
                DssRef.world.unitCollAreaGrid.armies_nearUpdate);

            foreach (var m in DssRef.world.unitCollAreaGrid.armies_nearUpdate)
            {
                if (m.tilePos.SideLength(tilePos) <= DominanceTileRadius)
                {
                    armyDefence += m.strengthValue;
                }
            }

            ai_armyDefenceValue = armyDefence;

            DssRef.world.unitCollAreaGrid.collectOpponentGroups(factionIndex, tilePos, out List<GameObject.SoldierGroup> groups, out List<City> cities);
            //detailObj.asynchFindBattleTarget(groups);

            //if (guardCount <= 0 && armyDefence == 0)
            //{
            //    //Destroyed in battle, domination check

            //    CityDominationStrength.Clear();

            //    foreach (var group in groups)
            //    {
            //        int key = group.GetFaction().parentArrayIndex;
            //        float value = group.strengthValue();

            //        if (CityDominationStrength.TryGetValue(key, out float current))
            //        {
            //            CityDominationStrength[key] = current + value;
            //        }
            //        else
            //        {
            //            CityDominationStrength.Add(key, value);
            //        }
            //    }

            //    if (CityDominationStrength.Count >= 1)
            //    {
            //        int strongestFaction = -1;
            //        float strongest = float.MinValue;

            //        foreach (var kv in CityDominationStrength)
            //        {
            //            if (kv.Value > strongest)
            //            {
            //                strongestFaction = kv.Key;
            //                strongest = kv.Value;
            //            }
            //        }



            //        var dominatingFaction = DssRef.world.factions.Array[strongestFaction];

            //        if (faction.player.IsLocalPlayer())
            //        {
            //            ++faction.player.GetLocalPlayer().statistics.CitiesLost;
            //        }
            //        if (dominatingFaction.player.IsLocalPlayer())
            //        {
            //            ++dominatingFaction.player.GetLocalPlayer().statistics.CitiesCaptured;
            //        }

            //        Ref.update.AddSyncAction(new SyncAction1Arg<Faction>(setFaction, dominatingFaction));
            //    }
            //}
            //            var membersC = MembersCounter();
            //            while (membersC.Next())
            //            {
            //                if (membersC.sel.gameobjectType() == GameObjectType.City)
            //                {
            //                    cities.Add(membersC.sel.GetCity());
            //                }

            //                if (cityDominationStrength.ContainsKey(membersC.sel.faction.parentArrayIndex))
            //                {
            //                    cityDominationStrength[membersC.sel.faction.parentArrayIndex] += membersC.sel.strengthValue;
            //                }
            //                else
            //                {
            //                    cityDominationStrength.Add(membersC.sel.faction.parentArrayIndex, membersC.sel.strengthValue);
            //                }

            //                membersC.sel.ExitBattleGroup();
            //            }

            //            int strongestFaction = -1;
            //            float strongest = float.MinValue;

            //            foreach (var kv in cityDominationStrength)
            //            {
            //                if (kv.Value > strongest)
            //                {
            //                    strongestFaction = kv.Key;
            //                    strongest = kv.Value;
            //                }
            //            }

            //            var dominatingFaction = DssRef.world.factions.Array[strongestFaction];

            //            if (cities.Count > 0)
            //            {
            //                foreach (var c in cities)
            //                {
            //                    if (DssRef.diplomacy.InWar(c.faction, dominatingFaction))
            //                    {
            //                        if (c.faction.player.IsPlayer())
            //                        {
            //                            ++c.faction.player.GetLocalPlayer().statistics.CitiesLost;
            //                        }
            //                        if (dominatingFaction.player.IsPlayer())
            //                        {
            //                            ++dominatingFaction.player.GetLocalPlayer().statistics.CitiesCaptured;
            //                        }

            //                        Ref.update.AddSyncAction(new SyncAction1Arg<Faction>(c.setFaction, dominatingFaction));
            //                    }
            //                }
            //            }

            //            for (int i = 0; i < factions.Count; ++i)
            //            {
            //                var f = factions[i];

            //                bool winner = f == dominatingFaction || !DssRef.diplomacy.InWar(f, dominatingFaction);

            //                if (f.player.IsPlayer())
            //                {
            //                    var p = f.player.GetLocalPlayer();
            //                    p.battles.Remove(this);
            //                    if (winner)
            //                    {
            //                        p.statistics.BattlesWon++;
            //                    }
            //                    else
            //                    {
            //                        p.statistics.BattlesLost++;
            //                    }
            //                }
            //            }
            //        }
            //}

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

            if (myIndex == 30)
            {
                lib.DoNothing();
            }
            setWorkersInRenderState();


            var groupsCounter = groups.counter();
            while (groupsCounter.Next())
            {
                groupsCounter.sel.setDetailLevel(inRender_detailLayer);
            }
            //detailObj.setDetailLevel(inRender_detailLayer);
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

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            Vector3 pos = position;
            pos.Y += 0.1f;
            Vector3 scale;

            switch (cityType)
            {
                case CityType.UnClaimed:
                    scale = new Vector3(1.2f);
                    break;
                case CityType.Campsite:
                    scale = new Vector3(0.6f);
                    break;
                case CityType.Village:
                    scale = new Vector3(0.7f);
                    break;
                case CityType.Town:
                    scale = new Vector3(0.96f);
                    break;
                default:
                    scale = new Vector3(1.2f);
                    break;
            }

            selection.groupModels_terrian.OneFrameModel(pos, scale, hover, true);
        }

        //bool spendWorker(int count)
        //{
        //    if (workForce.amount >= count)
        //    { 
        //        workForce.amount -= count;
        //        return true;
        //    }

        //    return false;
        //}
       
        public override bool Equals(object obj)
        {
            return obj is City && ((City)obj).myIndex == myIndex;
        }

        public override string ToString()
        {
            return "City" + myIndex.ToString() + " \"" + Name(out _) + "\"";
        }

        public override string Name(out bool mayEdit)
        {
            Faction faction = GetFaction();
            if (faction == null)
            {
                mayEdit = false;
                return TextLib.Error;
            }
            mayEdit = faction != null && faction.player != null && faction.player.IsLocalPlayer();
            return name.name;
        }

        public override void NameEditEvent(string result, object tag)
        {
            name.setCustom(result);
        }

        public override string TypeName()
        {
            return DssRef.lang.UnitType_City + " (" + myIndex + ")";
        }
        //public override SpriteName TypeIcon()
        //{
        //    return SpriteName.WarsCityHall;
        //}

        public override void TypeIcon(RichBoxContent content)
        {
            content.Add(new RbImage( SpriteName.WarsCityHall));
            tagToHud(content);
        }

        public void CityPresentationHud(ObjectHudArgs args, bool tooltip)
        {
            Faction faction = GetFaction_Safe();

            if (faction == null)
            {
                args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
                args.content.Add(new RbImage(SpriteName.WarsRelationFlag));
                args.content.space(0.5f);
                args.content.Add(new RbText(DssRef.lang.UnitType_UnclaimedLand, tooltip ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

                args.content.space(1);
                args.content.Add(new RbText(string.Format(DssRef.lang.UnitId, myIndex), HudLib.SecondaryTextColor));
            }
            else
            {
                nameToHud(args.content, !tooltip);

                args.content.Add(new RbBeginTitle(tooltip ? 2 : 1));
                if (!tagToHud(args.content))
                {
                    args.content.Add(faction.FlagTextureToHud());
                }
                args.content.space(0.5f);
                args.content.Add(new RbImage(SpriteName.WarsCityHall));
                args.content.space(0.5f);
                args.content.Add(new RbText(DssRef.lang.UnitType_City, tooltip ? HudLib.TitleColor_TypeName : HudLib.TitleColor_Head));

                args.content.space(1);
                args.content.Add(new RbText(string.Format(DssRef.lang.UnitId, myIndex), HudLib.SecondaryTextColor));

                ownerToHud(args, !tooltip);
            }
        }

        public void tooltip(RichBoxContent content, object tag)
        {
            toTooltip(new ObjectHudArgs() { content = content });
        }


        public override void toTooltip(ObjectHudArgs args)
        {
            CityPresentationHud(args, true);

            if (HasFaction())
            {
                const int LowAmount = 10;

                args.content.newLine();
                HudLib.CityResource(args.content, this, ItemResourceType.Food_G);

                if (resourceAmount(CityResoureIndex.food)/*res_food.amount*/ <= LowAmount)
                {
                    if (res_water.amount <= 2)
                    {
                        HudLib.CityResource(args.content, this, ItemResourceType.Water_G);
                    }
                    if (resourceAmount(CityResoureIndex.rawFood)/*res_rawFood.amount*/ <= LowAmount)
                    {
                        HudLib.CityResource(args.content, this, ItemResourceType.RawFood_Group);
                    }
                    if (resourceAmount(CityResoureIndex.fuel)/*res_fuel.amount*/ <= LowAmount)
                    {
                        HudLib.CityResource(args.content, this, ItemResourceType.Fuel_G);
                    }
                }

                args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                args.content.Add(new RbText(TextLib.OneDecimal(strengthValue)));
            }
        }

        public override void toHud(ObjectHudArgs args)
        {
            
            CityPresentationHud(args, false);
            
            //if (HasFaction())
            //{
                args.content.newLine();
                //if (args.ShowFull)
                {
                    if (GetFaction() == args.player.faction || DssRef.difficulty.setting_gameMode == GameModeMainType.Spectator)
                    {
                        CityDetailsHud(true, args.player, args.content);
                        new Interface.CityMenu(args.player, this, args.content);
                    }
                    else
                    {
                        CityDetailsHud(false, args.player, args.content);
                    }
                }
            //}
        }

        public void waterToHud(RichBoxContent content, bool canInteract)
        {
            content.Add(new RbImage(SpriteName.WarsResource_Water));
            content.space();
            content.Add(new RbText(TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Water) + ": " + string.Format(DssRef.lang.Language_CollectProgress, res_water.amount, maxWaterTotal)));
            content.Add(new RbTab(0.4f));
            content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
            content.Add(new RbText(TextLib.OneDecimal(waterAddPerSec)));
            content.space();
            if (DssRef.difficulty.GodPowers())
            {
                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("= 0", HudLib.GodPower_Color) },
                   new RbAction(() => { waterAddPerSec = 0; }),
                   null, true));

                content.Add(new ArtButton(RbButtonStyle.GodPower, new List<AbsRichBoxMember> { new RbText("+0.1", HudLib.GodPower_Color) },
                    new RbAction(() => { waterAddPerSec += 0.1f; }),
                    null, true));
            }

            if (canInteract)
            {
                HudLib.InfoButton(content,
                   new RbTooltip((RichBoxContent content, object tag) =>
                   {
                       //RichBoxContent content = new RichBoxContent();
                       content.h2(TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Water)).overrideColor = HudLib.TitleColor_Label;
                       content.newLine();
                       content.Add(new RbImage(SpriteName.WarsResource_Water));
                       content.Add(new RbText(string.Format(DssRef.lang.Resource_CurrentAmount, res_water.amount)));

                       content.text(string.Format(DssRef.lang.Resource_MaxAmount, maxWaterTotal));

                       content.newLine();
                       content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
                       content.Add(new RbText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.OneDecimal(waterAddPerSec))));

                       content.newParagraph();
                       HudLib.BulletPoint(content);
                       content.Add(new RbText(DssRef.lang.Resource_WaterReason, HudLib.InfoYellow_Light));
                       content.newLine();
                       HudLib.BulletPoint(content);
                       content.Add(new RbText(DssRef.lang.Resource_WaterAddLimit, HudLib.InfoYellow_Light));
                       HudLib.Description(content, DssRef.lang.Resource_WaterAddLimit);

                       //player.hud.tooltip.create(player, content, true);
                   }));
            }
        }

        public bool ToPinHud(ObjectHudArgs args)
        {
            if (GetFaction() == args.player.faction)
            {
                RichBoxContent buttonContent = new RichBoxContent();
                TypeIcon(buttonContent);
                args.content.Add(new ArtButton(RbButtonStyle.Outline, buttonContent,
                    new RbAction1Arg<City>(args.player.gameControls.nextCity, this),
                    new RbTooltip(this.tooltip)));
                
                return true;
            }
            return false;
        }

        int TotalServiceMen()
        {
            return workingAndFreeServiceMen;
        }

        public void CityDetailsHud(bool minimal, LocalPlayer player, RichBoxContent content)
        {
            Faction faction = GetFaction();

            if (faction == null)
            {
                //Unclaimed view

                if (!player.profile.casualControls)
                {
                    waterToHud(content, false);
                    terrainStructure.miningOverviewHud(player, content);
                }
            }
            else if (minimal)
            {
                content.Add(new RbImage(SpriteName.WarsWorker));
                content.space(0.5f);
                content.Add(new RbText(TextLib.LargeNumber(workForce.amount)));
                //content.space();
                HudLib.BulletSeperationPoint(content);
                //content.space();
                content.Add(new RbImage(SpriteName.WarsStrengthIcon));
                content.space(0.5f);
                content.Add(new RbText(TextLib.OneDecimal(strengthValue)));
            }
            else
            {
                bool interactive = player.faction == faction;

                if (interactive && !player.profile.casualControls)
                {
                    if (automateCity)
                    {
                        content.newParagraph();
                    }
                    else
                    {
                        content.newLine();
                    }

                    if (automateCity || player.tutorial == null || player.tutorial.AdvisorMode())
                    {
                        content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                            new RbText(DssRef.lang.Automation_AutomateCity)
                            }, AutomateCityProperty));
                    }

                    if (automateCity)
                    {
                        content.newLine();
                        HudLib.Label(content, DssRef.lang.Automation_AutomationFocus);

                        content.newLine();
                        foreach (var focus in CityMenu.AvailableAutomationFocuses)
                        {
                            string caption = null;
                            switch (focus)
                            {
                                case AutomationFocus.NoFocus:
                                    caption = DssRef.lang.Hud_None;
                                    break;
                                case AutomationFocus.Food:
                                    caption = TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food);
                                    break;
                                case AutomationFocus.Grow:
                                    caption = DssRef.lang.Automation_AutomationFocus_Grow;
                                    break;
                                case AutomationFocus.Export:
                                    caption = DssRef.lang.Automation_AutomationFocus_Export;
                                    break;
                                case AutomationFocus.Military:
                                    caption = DssRef.lang.Automation_AutomationFocus_War;
                                    break;
                            }

                            var button = new ArtOption(automationFocus == focus,
                                new List<AbsRichBoxMember>
                                {
                                    new RbText(caption),
                                },
                                new RbAction(() =>
                                {
                                    automationFocus = focus;
                                    nextAutoConscriptTime.setTimeFromNow(DssConst.TrainingTimeSec_Basic);
                                }, RbSoundType.Option),
                                new RbTooltip(automationToolTip, focus));

                            content.Add(button);
                        }

                        switch (automationFocus)
                        {
                            case AutomationFocus.Export:
                                content.newParagraph();
                                HudLib.Label(content, DssRef.lang.Automation_AutomationFocus_Export);
                                content.newLine();
                                for (ExportAutoType type = 0; type < ExportAutoType.NUM; type++)
                                {

                                    var optionContent = new List<AbsRichBoxMember>(4);
                                    if (type == ExportAutoType.Resources)
                                    {
                                        optionContent.Add(new RbImage(SpriteName.WarsResource_RawFood));
                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Wood));
                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Stone));
                                        optionContent.Add(new RbSpace());
                                        optionContent.Add(new RbText(DssRef.lang.WarsResourceGroup_Resources, HudLib.SubOptionTextColor));
                                    }
                                    else
                                    {
                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Bow));
                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Sword));
                                        optionContent.Add(new RbImage(SpriteName.WarsResource_PaddedArmor));
                                        optionContent.Add(new RbSpace());
                                        optionContent.Add(new RbText(DssRef.lang.WarsResourceGroup_Weapons, HudLib.SubOptionTextColor));
                                    }

                                    var button = new ArtOption(type == exportAutoType,
                                       optionContent,
                                       new RbAction1Arg<ExportAutoType>((ExportAutoType type) =>
                                       {
                                           exportAutoType = type;
                                       }, type, RbSoundType.Option), null);

                                    content.Add(button);
                                }
                                break;
                            case AutomationFocus.Military:
                                content.newParagraph();
                                HudLib.Label(content, DssRef.lang.CityAutomation_SoldierQuality);
                                content.newLine();
                                for (WarAutoQuality quality = 0; quality < WarAutoQuality.NUM; quality++)
                                {
                                    string caption;
                                    switch (quality)
                                    {
                                        default:
                                            caption = DssRef.lang.Hud_Low;
                                            break;
                                        case WarAutoQuality.Medium:
                                            caption = DssRef.lang.Hud_Medium;
                                            break;
                                        case WarAutoQuality.High:
                                            caption = DssRef.lang.Hud_High;
                                            break;
                                    }

                                    var button = new ArtOption(quality == warAutoQuality,
                                       new List<AbsRichBoxMember>
                                       {
                                            new RbText(caption, HudLib.SubOptionTextColor),
                                       },
                                       new RbAction1Arg<WarAutoQuality>((WarAutoQuality quality) =>
                                       {
                                           warAutoQuality = quality;
                                       }, quality, RbSoundType.Option), new RbTooltip(AutoConscriptLib.autoWarQualityToolTip, quality));

                                    content.Add(button);
                                }
                                content.newParagraph();
                                HudLib.Label(content, DssRef.lang.CityAutomation_SoldierWeaponType);
                                content.newLine();
                                for (WarAutoWeaponType weaponType = 0; weaponType < WarAutoWeaponType.NUM; weaponType++)
                                {
                                    string caption;
                                    SpriteName icon;
                                    switch (weaponType)
                                    {
                                        default:
                                            icon = SpriteName.NO_IMAGE;
                                            caption = DssRef.lang.WarsResourceGroup_AllWeaponTypes;
                                            break;
                                        case WarAutoWeaponType.Melee:
                                            icon = SpriteName.WarsResource_Sword;
                                            caption = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
                                            break;
                                        case WarAutoWeaponType.Ranged:
                                            icon = SpriteName.WarsResource_Bow;
                                            caption = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
                                            break;
                                        case WarAutoWeaponType.Warmachine:
                                            icon = SpriteName.WarsResource_Ballista;
                                            caption = DssRef.lang.WarsResourceGroup_Warmachines;
                                            break;
                                    }

                                    List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(3);
                                    if (icon != SpriteName.NO_IMAGE)
                                    {
                                        buttonContent.Add(new RbImage(icon));
                                        buttonContent.Add(new RbSpace());
                                    }
                                    buttonContent.Add(new RbText(caption, HudLib.SubOptionTextColor));

                                    var button = new ArtOption(weaponType == warAutoWeaponType,
                                       buttonContent,
                                       new RbAction1Arg<WarAutoWeaponType>((WarAutoWeaponType weaponType) =>
                                       {
                                           warAutoWeaponType = weaponType;
                                       }, weaponType, RbSoundType.Option));

                                    content.Add(button);
                                }

                                break;
                        }

                        content.Add(new RbSeperationLine());

                    }
                }

                HudLib.ItemCount(content, SpriteName.WarsWorkerAdd, DssRef.lang.ResourceType_Children, children().ToString());
                content.space();
                if (interactive)
                {
                    HudLib.InfoButton(content, new RbTooltip(childrenTooltip, this));
                }

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsUnitIcon_Immigrant));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_Immigrants, immigrants.Int())));
                content.Add(new RbTab(0.4f));
                content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
                content.space();
                content.Add(new RbImage(SpriteName.WarsBuild_Tent));
                content.space();
                content.Add(new RbText(buildingStructure.ImmigrationTent_count.ToString()));
                content.space();
                if (interactive)
                {
                    HudLib.InfoButton(content, new RbTooltip(immigrantsTooltip, this));
                }

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsWorker));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_Workers, workForce.amount)));
                content.Add(new RbTab(0.4f));
                content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
                content.space();
                content.Add(new RbImage(SpriteName.WarsBuild_WorkerHuts));
                content.space();
                content.Add(new RbText(HousingCount_Workers.ToString()));

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsGuard));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_GuardCount, soldiersCount)));

                if (!player.profile.casualControls)
                {
                    content.Add(new RbTab(0.4f));
                    content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsBuild_GuardOffice));
                    content.space();
                    content.Add(new RbText(HousingCount_Guard.ToString()));
                }

                if (!player.profile.casualControls)
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsServiceMen));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_ServiceMen, freeServiceMen.amount)));
                    content.Add(new RbTab(0.4f));
                    content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsServiceMenTotal));
                    content.space();
                    content.Add(new RbText(TotalServiceMen().ToString()));

                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsNobelman));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, TextLib.LargeFirstLetter(DssRef.todoLang.Resource_TypeName_NobelMen), freeNobelMen.amount)));
                    content.Add(new RbTab(0.4f));
                    content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsBuild_Nobelhouse));
                    content.space();
                    content.Add(new RbText(HousingCount_NobelMen.ToString()));
                }
                //HudLib.ItemCount(content, SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers, TextLib.Divition_Large(workForce.amount, homesTotal()));
                //HudLib.ItemCount(content, SpriteName.WarsGuard, DssRef.lang.Hud_GuardCount, TextLib.Divition_Large(guardCount, maxGuardSize));

                CityEconomyData cityEconomy = new CityEconomyData(this);

                content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue)));
                content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Hud_TotalIncome, Money.CopperToGoldString_Large(cityEconomy.IncomeAndUpkeep_Total())));
                //content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Hud_Upkeep, GuardUpkeep(maxGuardSize)));

                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.rtsIncomeTime));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsWorker));
                    content.space();
                    var textCont = new RbText(string.Format(DssRef.lang.Economy_TaxIncome, Money.CopperToGoldString_Large(cityEconomy.taxIncome_copp)));
                    content.Add(textCont);
                    if (interactive)
                    {
                        content.space();
                        HudLib.InfoButton(content, new RbTooltip(HudLib.taxInfo, this));
                    }
                }
                if (!player.profile.casualControls)
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsServiceMen));
                    content.space();
                    var textCont = new RbText(string.Format(DssRef.lang.Economy_ServicemenUpkeep, Money.CopperToGoldString_Dynamic(cityEconomy.servicemenUpkeep_copp)));
                    content.Add(textCont);
                    if (interactive)
                    {
                        content.space();
                        HudLib.InfoButton(content, new RbTooltip(HudLib.servicemenUpkeepInfo));
                    }
                }
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsGuard));
                    content.space();
                    var textCont = new RbText(string.Format(DssRef.lang.Economy_GuardUpkeep, Money.CopperToGoldString_Dynamic(cityEconomy.cityGuardUpkeep_copp)));
                    content.Add(textCont);
                    if (interactive)
                    {
                        content.space();
                        HudLib.InfoButton(content, new RbTooltip(HudLib.guardUpkeepInfo));
                    }
                }

                if (!player.profile.casualControls)
                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsResource_RawFoodRemove));
                    content.space();
                    content.Add(new RbText(DssRef.lang.WorkQueue_Title + ":"));
                    content.Add(new RbImage(SpriteName.WarsBuild_PigPen));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.todoLang.Economy_AnimalPenUpkeep, TextLib.OneDecimal(cityEconomy.animalPenUpkeep))));

                }

                {
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsHammerAdd));
                    content.space();
                    content.Add(new RbText(".Work queue" + ":"));
                    content.hspace();
                    content.Add(new RbText(WorkerStats_WorkQueueLength.ToString()));

                    HudLib.BulletSeperationPoint(content);
                    content.Add(new RbImage(SpriteName.WarsWorker));
                    content.hspace();
                    content.Add(new RbText((WorkerStats_TotalUnits - WorkerStats_IdleCount).ToString()));

                    content.space();

                    content.Add(new RbImage(SpriteName.unitEmoteSnore));
                    content.hspace();
                    content.Add(new RbText(WorkerStats_IdleCount.ToString()));

                    content.space();
                    HudLib.InfoButton(content, new RbTooltip(workQueueInfo));
                }

                if (!player.profile.casualControls)
                {
                    cultureToHud(player, content, interactive);
                    biomToHud(player, content, interactive);
                }
                if (immigrants.HasValue())
                {
                    content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.Hud_Immigrants, immigrants.Int()));
                }

                if (!player.profile.casualControls)
                {
                    terrainStructure.miningOverviewHud(player, content);
                    //new XP.TechnologyHud().technologyOverviewHud(content, player, this, faction);
                    new XP.TechnologyHud(player, this).technologyOverviewHud(content, faction);
                }
                
            }

            void workQueueInfo(RichBoxContent content, object tag)
            {
                content.h1(".Work queue", HudLib.TitleColor_Label);
                HudLib.Label(content, "Remaining work objectives");
                content.hspace();
                content.Add(new RbText(WorkerStats_WorkQueueLength.ToString()));

                content.newParagraph();
                
                HudLib.Label(content, "Active work teams");
                content.hspace();
                content.Add(new RbImage(SpriteName.WarsWorker));
                content.hspace();
                content.Add(new RbText((WorkerStats_TotalUnits - WorkerStats_IdleCount).ToString()));

                content.newLine();
                
                HudLib.Label(content, "Idle work teams");
                content.hspace();
                content.Add(new RbImage(SpriteName.unitEmoteSnore));
                content.hspace();
                content.Add(new RbText(WorkerStats_IdleCount.ToString()));

                content.newLine();
                content.text(string.Format("Villagers work in teams of {0}", WorkTeamSize), HudLib.InfoYellow_Light);
            }


            void automationToolTip(RichBoxContent content, object tag)
            {
                AutomationFocus focus = (AutomationFocus)tag;
                switch (focus)
                {
                    case AutomationFocus.NoFocus:
                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_NoFocus_Description, HudLib.InfoYellow_Light));
                        break;

                    case AutomationFocus.Food:
                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_WheatFarms));
                        content.space();
                        content.Add(new RbText(string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat)));
                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_Cook));
                        content.space();
                        content.Add(new RbText(DssRef.lang.BuildingType_Cook));
                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_CoalPit));
                        content.space();
                        content.Add(new RbText(DssRef.lang.BuildingType_CoalPit));
                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsResource_Food));
                        content.space();
                        content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_Postal));
                        content.space();
                        content.Add(new RbText(DssRef.lang.BuildingType_Postal));
                        break;

                    case AutomationFocus.Export:
                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_Postal));
                        content.space();
                        content.Add(new RbText(DssRef.lang.BuildingType_Postal));
                        //
                        //content.newLine();
                        //HudLib.BulletPoint(content);
                        //content.space();
                        //content.Add(new RbImage(SpriteName.WarsResource_Wood));
                        //content.space();
                        //content.Add(new RbText(DssRef.lang.Resource_TypeName_Wood));
                        ////
                        //content.newLine();
                        //HudLib.BulletPoint(content);
                        //content.space();
                        //content.Add(new RbImage(SpriteName.WarsResource_Stone));
                        //content.space();
                        //content.Add(new RbText(DssRef.lang.Resource_TypeName_Stone));
                        ////
                        //content.newLine();
                        //HudLib.BulletPoint(content);
                        //content.space();
                        //content.Add(new RbImage(SpriteName.));
                        //content.space();
                        //content.Add(new RbText(DssRef.lang.BuildingType_Postal));


                        break;

                    case AutomationFocus.Military:
                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_SoldierBarracks));
                        content.space();
                        content.Add(new RbText(DssRef.lang.BuildingType_SoldierBarracks));

                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsSoldierIcon));
                        content.space();
                        content.Add(new RbText(DssRef.lang.UnitType_Soldier));

                        break;

                    case AutomationFocus.Grow:
                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_WorkerHuts));
                        content.space();
                        content.Add(new RbText(DssRef.lang.BuildingType_WorkerHut));

                        //
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.space();
                        content.Add(new RbImage(SpriteName.WarsBuild_WheatFarms));
                        content.space();
                        content.Add(new RbText(DssRef.lang.Resource_TypeName_Wheat));

                        break;
                }
            }
        }

        public void workQueueInfo(RichBoxContent content, object tag)
        {
            content.h1(DssRef.lang.WorkQueue_Title, HudLib.TitleColor_Head);
            HudLib.Label(content, DssRef.lang.WorkQueue_Length);
            content.hspace();
            content.Add(new RbImage(SpriteName.WarsIcon_WorkQueueTotal));
            content.hspace();
            content.Add(new RbText(WorkerStats_WorkQueueLength.ToString()));

            content.newParagraph();

            HudLib.Label(content, DssRef.lang.WorkQueue_ActiveWorkers);
            content.hspace();
            content.Add(new RbImage(SpriteName.WarsIcon_WorkQueueActive));
            content.hspace();
            content.Add(new RbText((WorkerStats_TotalUnits - WorkerStats_IdleCount).ToString()));

            content.newLine();

            HudLib.Label(content, DssRef.lang.WorkQueue_IdleWorkers);
            content.hspace();
            content.Add(new RbImage(SpriteName.WarsIcon_WorkQueueIdle));
            content.hspace();
            content.Add(new RbText(WorkerStats_IdleCount.ToString()));

            content.newLine();
            content.text(string.Format(DssRef.lang.WorkTeam_Size, WorkTeamSize), HudLib.InfoYellow_Light);
        }

        public void immigrantsTooltip(RichBoxContent content, object tag)
        {
            content.h2(DssRef.lang.Hud_Immigrants, HudLib.TitleColor_Head);

            content.newLine();
            HudLib.BulletPoint(content);
            content.space();
            content.Add(new RbImage(SpriteName.WarsUnitIcon_Soldier));
            content.space();
            content.Add(new RbText(DssRef.lang.Immigrants_DisbandedSoldiers));

            content.newLine();
            HudLib.BulletPoint(content);
            content.space();
            content.Add(new RbImage(SpriteName.WarsWorkerAdd));
            content.space();
            content.Add(new RbText(DssRef.lang.Immigrants_RefillWorkers));

            content.newLine();
            HudLib.BulletPoint(content);
            content.space();
            content.Add(new RbImage(SpriteName.WarsUnitIcon_Immigrant_RemoveTime));
            content.space();
            content.Add(new RbText(DssRef.lang.Immigrants_UnhousedAreLost));

            content.newParagraph();
            content.Add(new RbImage(SpriteName.WarsBuild_Tent));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.BuildingType_ImmigrationTent, buildingStructure.ImmigrationTent_count), HudLib.TitleColor_TypeName));
            content.newLine();
            content.Add(new RbText(string.Format(DssRef.lang.BuildingType_ImmigrationTent_Description, DssConst.ImmigrantionTent_Capacity), HudLib.InfoYellow_Light));

        }
        public void childrenTooltip(RichBoxContent content, object tag)
        {
            City city = (City)tag;
            content.text(string.Format(DssRef.lang.WorkForce_ChildToManTime, 2));

            content.newParagraph();
            content.h2(DssRef.lang.WorkForce_ChildBirthRequirements, HudLib.TitleColor_Head);

            {
                bool available = inBattle == false;
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
                content.hspace();
                content.Add(new RbImage(SpriteName.WarsRelationPeace));
                content.hspace();
                content.Add(new RbText(DssRef.lang.WorkForce_Peace, HudLib.ResourceCostColor(available)));

            }
            {
                bool available = city.homesUnused() > 0;
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
                content.hspace();
                content.Add(new RbImage(SpriteName.WarsBuild_WorkerHuts));
                content.hspace();
                content.Add(new RbText(string.Format(DssRef.lang.WorkForce_AvailableHomes, city.homesUnused()), HudLib.ResourceCostColor(available)));

            }

            if (!city.GetCasual())
            {

                {
                    bool available = city.resourceAmount(CityResoureIndex.food) /*.res_food.amount*/ > 0;
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
                    content.hspace();
                    content.Add(new RbImage(SpriteName.WarsResource_Food));
                    content.hspace();
                    content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Resource_TypeName_Food, city.resourceAmount(CityResoureIndex.food)/*city.res_food.amount*/), HudLib.ResourceCostColor(available)));
                    //HudLib.ItemCount(content, DssRef.lang.Resource_TypeName_Food, city.res_food.amount.ToString()).overrideColor = HudLib.ResourceCostColor(city.res_food.amount > 0);
                }
                if (cityType < CityType.Capital)
                {
                    bool available = homeUsers() < WorkersMaxLimit;
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
                    content.hspace();
                    content.Add(new RbImage(SpriteName.WarsCityHall));
                    content.hspace();
                    content.Add(new RbText(string.Format(DssRef.lang.CityHall_MaxSupportedWorkers, WorkersMaxLimit), HudLib.ResourceCostColor(available)));
                }

            }
        }
        //        public void CityDetailsHud(bool minimal, LocalPlayer player, RichBoxContent content)
        //        {
        //            Faction faction = GetFaction();

        //            //if (minimal)
        //            //{
        //            //    content.Add(new RbImage(SpriteName.WarsWorker));
        //            //    content.space(0.5f);
        //            //    content.Add(new RbText(TextLib.LargeNumber(workForce.amount)));
        //            //    //content.space();
        //            //    HudLib.BulletSeperationPoint(content);
        //            //    //content.space();
        //            //    content.Add(new RbImage(SpriteName.WarsStrengthIcon));
        //            //    content.space(0.5f);
        //            //    content.Add(new RbText(TextLib.OneDecimal(strengthValue)));
        //            //}
        //            if (faction == null)
        //            {
        //                //Unclaimed view

        //                if (!player.profile.casualControls)
        //                {
        //                    waterToHud(content, false);
        //                    terrainStructure.miningOverviewHud(player, content);
        //                }
        //            }
        //            else
        //            {
        //                bool interactive = player.faction == faction;

        //                if (interactive && !player.profile.casualControls)
        //                {
        //                    if (automateCity)
        //                    {
        //                        content.newParagraph();
        //                    }
        //                    else
        //                    {
        //                        content.newLine();
        //                    }

        //                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
        //                        new RbText(DssRef.lang.Automation_AutomateCity)
        //                    }, AutomateCityProperty));

        //                    if (automateCity)
        //                    {
        //                        content.newLine();
        //                        HudLib.Label(content, DssRef.lang.Automation_AutomationFocus);

        //                        content.newLine();
        //                        foreach (var focus in CityMenu.AvailableAutomationFocuses)
        //                        {
        //                            string caption = null;
        //                            switch (focus)
        //                            {
        //                                case AutomationFocus.NoFocus:
        //                                    caption = DssRef.lang.Hud_None;
        //                                    break;
        //                                case AutomationFocus.Food:
        //                                    caption = TextLib.LargeFirstLetter(DssRef.lang.Resource_TypeName_Food);
        //                                    break;
        //                                case AutomationFocus.Grow:
        //                                    caption = DssRef.lang.Automation_AutomationFocus_Grow;
        //                                    break;
        //                                case AutomationFocus.Export:
        //                                    caption = DssRef.lang.Automation_AutomationFocus_Export;
        //                                    break;
        //                                case AutomationFocus.Military:
        //                                    caption = DssRef.lang.Automation_AutomationFocus_War;
        //                                    break;
        //                            }

        //                            var button = new ArtOption(automationFocus == focus,
        //                                new List<AbsRichBoxMember>
        //                                {
        //                                    new RbText(caption),
        //                                },
        //                                new RbAction(() =>
        //                                {
        //                                    automationFocus = focus;
        //                                    nextAutoConscriptTime.setTimeFromNow(DssConst.TrainingTimeSec_Basic);
        //                                }, RbSoundType.Option),
        //                                new RbTooltip(automationToolTip, focus));

        //                            content.Add(button);
        //                        }

        //                        switch (automationFocus)
        //                        {
        //                            case AutomationFocus.Export:
        //                                content.newParagraph();
        //                                HudLib.Label(content, DssRef.lang.Automation_AutomationFocus_Export);
        //                                content.newLine();
        //                                for (ExportAutoType type = 0; type < ExportAutoType.NUM; type++)
        //                                {

        //                                    var optionContent = new List<AbsRichBoxMember>(4);
        //                                    if (type == ExportAutoType.Resources)
        //                                    {
        //                                        optionContent.Add(new RbImage(SpriteName.WarsResource_RawFood));
        //                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Wood));
        //                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Stone));
        //                                        optionContent.Add(new RbSpace());
        //                                        optionContent.Add(new RbText(DssRef.lang.WarsResourceGroup_Resources, HudLib.SubOptionTextColor));
        //                                    }
        //                                    else
        //                                    {
        //                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Bow));
        //                                        optionContent.Add(new RbImage(SpriteName.WarsResource_Sword));
        //                                        optionContent.Add(new RbImage(SpriteName.WarsResource_PaddedArmor));
        //                                        optionContent.Add(new RbSpace());
        //                                        optionContent.Add(new RbText(DssRef.lang.WarsResourceGroup_Weapons, HudLib.SubOptionTextColor));
        //                                    }

        //                                    var button = new ArtOption(type == exportAutoType,
        //                                       optionContent,
        //                                       new RbAction1Arg<ExportAutoType>((ExportAutoType type) =>
        //                                       {
        //                                           exportAutoType = type;
        //                                       }, type, RbSoundType.Option), null);

        //                                    content.Add(button);
        //                                }
        //                                break;
        //                            case AutomationFocus.Military:
        //                                content.newParagraph();
        //                                HudLib.Label(content, DssRef.lang.CityAutomation_SoldierQuality);
        //                                content.newLine();
        //                                for (WarAutoQuality quality = 0; quality < WarAutoQuality.NUM; quality++)
        //                                {
        //                                    string caption;
        //                                    switch (quality)
        //                                    {
        //                                        default:
        //                                            caption = DssRef.lang.Hud_Low;
        //                                            break;
        //                                        case WarAutoQuality.Medium:
        //                                            caption = DssRef.lang.Hud_Medium;
        //                                            break;
        //                                        case WarAutoQuality.High:
        //                                            caption = DssRef.lang.Hud_High;
        //                                            break;
        //                                    }

        //                                    var button = new ArtOption(quality == warAutoQuality,
        //                                       new List<AbsRichBoxMember>
        //                                       {
        //                                            new RbText(caption, HudLib.SubOptionTextColor),
        //                                       },
        //                                       new RbAction1Arg<WarAutoQuality>((WarAutoQuality quality) =>
        //                                       {
        //                                           warAutoQuality = quality;
        //                                       }, quality, RbSoundType.Option), new RbTooltip(AutoConscriptLib.autoWarQualityToolTip, quality));

        //                                    content.Add(button);
        //                                }
        //                                content.newParagraph();
        //                                HudLib.Label(content, DssRef.lang.CityAutomation_SoldierWeaponType);
        //                                content.newLine();
        //                                for (WarAutoWeaponType weaponType = 0; weaponType < WarAutoWeaponType.NUM; weaponType++)
        //                                {
        //                                    string caption;
        //                                    SpriteName icon;
        //                                    switch (weaponType)
        //                                    {
        //                                        default:
        //                                            icon = SpriteName.NO_IMAGE;
        //                                            caption = DssRef.lang.WarsResourceGroup_AllWeaponTypes;
        //                                            break;
        //                                        case WarAutoWeaponType.Melee:
        //                                            icon = SpriteName.WarsResource_Sword;
        //                                            caption = DssRef.lang.WarsResourceGroup_MeleeHandWeapons;
        //                                            break;
        //                                        case WarAutoWeaponType.Ranged:
        //                                            icon = SpriteName.WarsResource_Bow;
        //                                            caption = DssRef.lang.WarsResourceGroup_RangedHandWeapons;
        //                                            break;
        //                                        case WarAutoWeaponType.Warmachine:
        //                                            icon = SpriteName.WarsResource_Ballista;
        //                                            caption = DssRef.lang.WarsResourceGroup_Warmachines;
        //                                            break;
        //                                    }

        //                                    List<AbsRichBoxMember> buttonContent = new List<AbsRichBoxMember>(3);
        //                                    if (icon != SpriteName.NO_IMAGE)
        //                                    {
        //                                        buttonContent.Add(new RbImage(icon));
        //                                        buttonContent.Add(new RbSpace());
        //                                    }
        //                                    buttonContent.Add(new RbText(caption, HudLib.SubOptionTextColor));

        //                                    var button = new ArtOption(weaponType == warAutoWeaponType,
        //                                       buttonContent,
        //                                       new RbAction1Arg<WarAutoWeaponType>((WarAutoWeaponType weaponType) =>
        //                                       {
        //                                           warAutoWeaponType = weaponType;
        //                                       }, weaponType, RbSoundType.Option));

        //                                    content.Add(button);
        //                                }

        //                                break;
        //                        }

        //                        content.Add(new RbSeperationLine());

        //                    }
        //                }


        //                //if (!player.profile.casualControls)
        //                //{
        //                //    terrainStructure.miningOverviewHud(player, content);
        //                //    new XP.TechnologyHud(player, this).technologyOverviewHud(content, faction);
        //                //}
        //                //technologyOverviewHud(content, player);
        //#if DEBUG
        //                //technologyHud(content, player);
        //#endif
        //                //if (!player.inTutorialMode)
        //                //{
        //                //    //Properties
        //                //    //if (nobelHouse)
        //                //    //{
        //                //    //    content.newLine();
        //                //    //    HudLib.BulletPoint(content);
        //                //    //    content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse));
        //                //    //}


        //                HudLib.ItemCount(content, SpriteName.WarsWorkerAdd, DssRef.lang.ResourceType_Children, children().ToString());
        //                        content.space();
        //                        if (interactive)
        //                        {
        //                            HudLib.InfoButton(content, new RbTooltip(childrenTooltip, this));
        //                        }

        //                        content.newLine();
        //                        content.Add(new RbImage(SpriteName.WarsUnitIcon_Immigrant));
        //                        content.space();
        //                        content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_Immigrants, immigrants.Int())));
        //                        content.Add(new RbTab(0.4f));
        //                        content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_Tent));
        //                        content.space();
        //                        content.Add(new RbText(buildingStructure.ImmigrationTent_count.ToString()));
        //                        content.space();
        //                        if (interactive)
        //                        {
        //                            HudLib.InfoButton(content, new RbTooltip(immigrantsTooltip, this));
        //                        }

        //                        content.newLine();
        //                        content.Add(new RbImage(SpriteName.WarsWorker));
        //                        content.space();
        //                        content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_Workers, workForce.amount)));
        //                        content.Add(new RbTab(0.4f));
        //                        content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_WorkerHuts));
        //                        content.space();
        //                        content.Add(new RbText(HousingCount_Workers.ToString()));

        //                        content.newLine();
        //                        content.Add(new RbImage(SpriteName.WarsGuard));
        //                        content.space();
        //                        content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_GuardCount, soldiersCount)));

        //                        if (!player.profile.casualControls)
        //                        {
        //                            content.Add(new RbTab(0.4f));
        //                            content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
        //                            content.space();
        //                            content.Add(new RbImage(SpriteName.WarsBuild_GuardOffice));
        //                            content.space();
        //                            content.Add(new RbText(HousingCount_Guard.ToString()));
        //                        }

        //                        if (!player.profile.casualControls)
        //                        {
        //                            content.newLine();
        //                            content.Add(new RbImage(SpriteName.WarsServiceMen));
        //                            content.space();
        //                            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.ResourceType_ServiceMen, freeServiceMen.amount)));
        //                            content.Add(new RbTab(0.4f));
        //                            content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
        //                            content.space();
        //                            content.Add(new RbImage(SpriteName.WarsServiceMenTotal));
        //                            content.space();
        //                            content.Add(new RbText(TotalServiceMen().ToString()));
        //                        }
        //                        //HudLib.ItemCount(content, SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers, TextLib.Divition_Large(workForce.amount, homesTotal()));
        //                        //HudLib.ItemCount(content, SpriteName.WarsGuard, DssRef.lang.Hud_GuardCount, TextLib.Divition_Large(guardCount, maxGuardSize));

        //                        CityEconomyData cityEconomy = new CityEconomyData(this);

        //                        content.icontext(SpriteName.WarsStrengthIcon, string.Format(DssRef.lang.Hud_StrengthRating, TextLib.OneDecimal(strengthValue)));
        //                        content.icontext(SpriteName.rtsIncomeTime, string.Format(DssRef.lang.Hud_TotalIncome, Money.CopperToGoldString_Large(cityEconomy.IncomeAndUpkeep_Total())));
        //                        //content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Hud_Upkeep, GuardUpkeep(maxGuardSize)));

        //                        {
        //                            content.newLine();
        //                            content.Add(new RbImage(SpriteName.rtsIncomeTime));
        //                            content.space();
        //                            content.Add(new RbImage(SpriteName.WarsWorker));
        //                            content.space();
        //                            var textCont = new RbText(string.Format(DssRef.lang.Economy_TaxIncome, Money.CopperToGoldString_Large(cityEconomy.taxIncome_copp)));
        //                            content.Add(textCont);
        //                            if (interactive)
        //                            {
        //                                content.space();
        //                                HudLib.InfoButton(content, new RbTooltip(HudLib.taxInfo, this));
        //                            }
        //                        }
        //                        if (!player.profile.casualControls)
        //                        {
        //                            content.newLine();
        //                            content.Add(new RbImage(SpriteName.rtsUpkeepTime));
        //                            content.space();
        //                            content.Add(new RbImage(SpriteName.WarsServiceMen));
        //                            content.space();
        //                            var textCont = new RbText(string.Format(DssRef.lang.Economy_ServicemenUpkeep, Money.CopperToGoldString_Dynamic(cityEconomy.servicemenUpkeep_copp)));
        //                            content.Add(textCont);
        //                            if (interactive)
        //                            {
        //                                content.space();
        //                                HudLib.InfoButton(content, new RbTooltip(HudLib.servicemenUpkeepInfo));
        //                            }
        //                        }
        //                        {
        //                            content.newLine();
        //                            content.Add(new RbImage(SpriteName.rtsUpkeepTime));
        //                            content.space();
        //                            content.Add(new RbImage(SpriteName.WarsGuard));
        //                            content.space();
        //                            var textCont = new RbText(string.Format(DssRef.lang.Economy_GuardUpkeep, Money.CopperToGoldString_Dynamic(cityEconomy.cityGuardUpkeep_copp)));
        //                            content.Add(textCont);
        //                            if (interactive)
        //                            {
        //                                content.space();
        //                                HudLib.InfoButton(content, new RbTooltip(HudLib.guardUpkeepInfo));
        //                            }
        //                        }

        //                        if (!player.profile.casualControls)
        //                        {
        //                            cultureToHud(player, content, interactive);
        //                        }
        //                        if (immigrants.HasValue())
        //                        {
        //                            content.icontext(SpriteName.WarsWorkerAdd, string.Format(DssRef.lang.Hud_Immigrants, immigrants.Int()));
        //                        }

        //                        if (!player.profile.casualControls)
        //                        {
        //                            terrainStructure.miningOverviewHud(player, content);
        //                            new XP.TechnologyHud(player, this).technologyOverviewHud(content, faction);

        //                        }

        //                    }



        //                    void automationToolTip(RichBoxContent content, object tag)
        //                    {
        //                        AutomationFocus focus = (AutomationFocus)tag;
        //                switch (focus)
        //                {
        //                    case AutomationFocus.NoFocus:
        //                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_NoFocus_Description, HudLib.InfoYellow_Light));
        //                        break;

        //                    case AutomationFocus.Food:
        //                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_WheatFarms));
        //                        content.space();
        //                        content.Add(new RbText(string.Format(DssRef.lang.BuildingType_ResourceFarm, DssRef.lang.Resource_TypeName_Wheat)));
        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_Cook));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.BuildingType_Cook));
        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_CoalPit));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.BuildingType_CoalPit));
        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsResource_Food));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.Resource_TypeName_Food));
        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_Postal));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.BuildingType_Postal));
        //                        break;

        //                    case AutomationFocus.Export:
        //                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_Postal));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.BuildingType_Postal));
        //                        //
        //                        //content.newLine();
        //                        //HudLib.BulletPoint(content);
        //                        //content.space();
        //                        //content.Add(new RbImage(SpriteName.WarsResource_Wood));
        //                        //content.space();
        //                        //content.Add(new RbText(DssRef.lang.Resource_TypeName_Wood));
        //                        ////
        //                        //content.newLine();
        //                        //HudLib.BulletPoint(content);
        //                        //content.space();
        //                        //content.Add(new RbImage(SpriteName.WarsResource_Stone));
        //                        //content.space();
        //                        //content.Add(new RbText(DssRef.lang.Resource_TypeName_Stone));
        //                        ////
        //                        //content.newLine();
        //                        //HudLib.BulletPoint(content);
        //                        //content.space();
        //                        //content.Add(new RbImage(SpriteName.));
        //                        //content.space();
        //                        //content.Add(new RbText(DssRef.lang.BuildingType_Postal));


        //                        break;

        //                    case AutomationFocus.Military:
        //                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_SoldierBarracks));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.BuildingType_SoldierBarracks));

        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsSoldierIcon));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.UnitType_Soldier));

        //                        break;

        //                    case AutomationFocus.Grow:
        //                        content.Add(new RbText(DssRef.lang.Automation_AutomationFocus_WillProduce, HudLib.TitleColor_Label));

        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_WorkerHuts));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.BuildingType_WorkerHut));

        //                        //
        //                        content.newLine();
        //                        HudLib.BulletPoint(content);
        //                        content.space();
        //                        content.Add(new RbImage(SpriteName.WarsBuild_WheatFarms));
        //                        content.space();
        //                        content.Add(new RbText(DssRef.lang.Resource_TypeName_Wheat));

        //                        break;
        //                }
        //            }
        //        }


        //public void immigrantsTooltip(RichBoxContent content, object tag)
        //{
        //    content.h2(DssRef.lang.Hud_Immigrants, HudLib.TitleColor_Head);

        //    content.newLine();
        //    HudLib.BulletPoint(content);
        //    content.space();
        //    content.Add(new RbImage(SpriteName.WarsUnitIcon_Soldier));
        //    content.space();
        //    content.Add(new RbText(DssRef.lang.Immigrants_DisbandedSoldiers));

        //    content.newLine();
        //    HudLib.BulletPoint(content);
        //    content.space();
        //    content.Add(new RbImage(SpriteName.WarsWorkerAdd));
        //    content.space();
        //    content.Add(new RbText(DssRef.lang.Immigrants_RefillWorkers));

        //    content.newLine();
        //    HudLib.BulletPoint(content);
        //    content.space();
        //    content.Add(new RbImage(SpriteName.WarsUnitIcon_Immigrant_RemoveTime));
        //    content.space();
        //    content.Add(new RbText(DssRef.lang.Immigrants_UnhousedAreLost));

        //    content.newParagraph();
        //    content.Add(new RbImage(SpriteName.WarsBuild_Tent));
        //    content.space();
        //    content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.BuildingType_ImmigrationTent, buildingStructure.ImmigrationTent_count), HudLib.TitleColor_TypeName));
        //    content.newLine();
        //    content.Add(new RbText(string.Format( DssRef.lang.BuildingType_ImmigrationTent_Description, DssConst.ImmigrantionTent_Capacity), HudLib.InfoYellow_Light));

        //}
        //public void childrenTooltip(RichBoxContent content, object tag)
        //{
        //    City city = (City)tag;
        //    content.text(string.Format(DssRef.lang.WorkForce_ChildToManTime, 2));

        //    content.newParagraph();
        //    content.h2(DssRef.lang.WorkForce_ChildBirthRequirements, HudLib.TitleColor_Head);

        //    {
        //        bool available = inBattle == false;
        //        content.newLine();
        //        HudLib.BulletPoint(content);
        //        content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
        //        content.hspace();
        //        content.Add(new RbImage(SpriteName.WarsRelationPeace));
        //        content.hspace();
        //        content.Add(new RbText(DssRef.lang.WorkForce_Peace, HudLib.ResourceCostColor(available)));

        //    }
        //    {
        //        bool available = city.homesUnused() > 0;
        //        content.newLine();
        //        HudLib.BulletPoint(content);
        //        content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
        //        content.hspace();
        //        content.Add(new RbImage(SpriteName.WarsBuild_WorkerHuts));
        //        content.hspace();
        //        content.Add(new RbText(string.Format(DssRef.lang.WorkForce_AvailableHomes, city.homesUnused()), HudLib.ResourceCostColor(available)));

        //    }

        //    if (!city.GetCasual())
        //    {

        //        {
        //            bool available = city.resourceAmount(CityResoureIndex.food) /*.res_food.amount*/ > 0;
        //            content.newLine();
        //            HudLib.BulletPoint(content);
        //            content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
        //            content.hspace();
        //            content.Add(new RbImage(SpriteName.WarsResource_Food));
        //            content.hspace();
        //            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Resource_TypeName_Food, city.resourceAmount(CityResoureIndex.food)/*city.res_food.amount*/), HudLib.ResourceCostColor(available)));
        //            //HudLib.ItemCount(content, DssRef.lang.Resource_TypeName_Food, city.res_food.amount.ToString()).overrideColor = HudLib.ResourceCostColor(city.res_food.amount > 0);
        //        }
        //        if (cityType < CityType.Capital)
        //        {
        //            bool available = homeUsers() < WorkersMaxLimit;
        //            content.newLine();
        //            HudLib.BulletPoint(content);
        //            content.Add(new RbImage(available ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
        //            content.hspace();
        //            content.Add(new RbImage(SpriteName.WarsCityHall));
        //            content.hspace();
        //            content.Add(new RbText(string.Format(DssRef.lang.CityHall_MaxSupportedWorkers, WorkersMaxLimit), HudLib.ResourceCostColor(available)));
        //        }

        //    }
        //}
        public void cultureToHud(LocalPlayer player, RichBoxContent content, bool interactive)
        {
            IconName.CityCulture(Culture, out string title, out string description);
            content.icontext(SpriteName.WarsCultureIcon, string.Format(DssRef.lang.CityCulture_CultureIsX, title));
            if (interactive)
            {
                content.space();
                HudLib.InfoButton(content, new RbTooltip(cultureToolTip));
            }
            else
            {
                content.newLine();
                HudLib.Description(content, description);
            }
        }

        public void biomToHud(LocalPlayer player, RichBoxContent content, bool interactive)
        {
            content.newLine();
            content.Add(new RbText(".Biom:", HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbText(Biom.ToString()));
        }

        void cultureToolTip(RichBoxContent content, object tag)
        {
            IconName.CityCulture(Culture, out string title, out string description);
            int factor = DssLib.CulturePercChangeFactor(Culture);
            if (factor >= 0)
            {
                content.text(string.Format(DssRef.lang.Hud_ChangeFactor, factor + "%"));
            }
            content.h2(title, HudLib.TitleColor_Head);
            content.text(description);

            content.newParagraph();
            content.text(DssRef.lang.CityCultureDescription, HudLib.InfoYellow_Light);
           
        }

        public void AddNeighborCity(WorldData world, int nCityIndex)
        {
            if (nCityIndex >= 0)
            {
                world.neighborCities.Add(myIndex, ref neighborCitiesCount, nCityIndex, true); 
            }
        }
       
        public static City Get(int index)
        {
            return DssRef.world.cities[index];
        }

        public void SetNeighborToPlayer()
        {
            Faction faction = GetFaction();

            EcsStaticArrayCounter neighbors = CityNeighbors();
            while (neighbors.Next(DssRef.world.cities, out City nCity))//
            {
                var cFaction = nCity.GetFaction();
                if (cFaction != faction && cFaction.player is Players.AiPlayer)
                {
                    cFaction.player.IsPlayerNeighbor = true;
                }
            }
        }

        public EcsStaticArrayCounter CityNeighbors()
        { 
            return new EcsStaticArrayCounter(DssRef.world.neighborCities, myIndex, neighborCitiesCount);
        }

        public bool HasPlayerNeighbor()
        {
            Faction faction = GetFaction();

            EcsStaticArrayCounter neighbors = CityNeighbors();

            while(neighbors.Next(DssRef.world.cities, out City nCity))
            {
                var cFaction = nCity.GetFaction();
                if (cFaction != faction && cFaction.player.IsPlayerNeighbor)
                {
                    return true;
                }
            }
            return false;
        }

        public override void setFaction(Faction newFaction, bool duringStartup, bool convert)
        {
            if (newFaction == null)
                return;

            Faction owner = GetFaction_Safe();
            if (owner != newFaction)
            {
                if (owner != null)
                {
                    previousOwner = owner.myIndex;
                    owner.lostCity_Time1 = owner.lostCity_Time0;
                    owner.lostCity_Time0 = this.myIndex;
                    owner.remove(this);
                    technology.destroyTechOnTakeOver();
                }

                factionIndex = newFaction.myIndex;
                
                

                if (!duringStartup)
                {
                    
                    newFaction.AddCity(this, false);
                    EditSubTile.OntileChange(tilePos);
                }

                OnNewOwner(newFaction, convert || duringStartup);                
            }
        }

        override public void OnNewOwner(Faction newFaction, bool convert)
        {

            if (DssRef.world != null)
            {
                DssRef.world.BordersUpdated = true;

                if (!convert)
                {
                    haltConscriptAndDelivery();
                }

                Ref.update.AddSyncAction(new SyncAction(() =>
                {
                    if (overviewModel != null)
                    {
                        createOverViewModel();
                    }

                    if (convert)
                    {
                        convertSoldiersToFaction(newFaction);
                    }
                    else
                    {
                        var first = groups.First();
                        if (first != null && first.factionIndex != newFaction.myIndex)
                        {
                            var counter = groups.counter();

                            while (counter.Next())
                            {

                                counter.sel.DeleteMe(DeleteReason.Disband, false);

                            }
                            groups.Clear();
                        }
                    }
                    
                }));

                if (overviewModel != null)
                {
                    Ref.update.AddSyncAction(new SyncAction(createOverViewModel));
                }

                nextAutoConscriptTime.setTimeFromNow(DssConst.TrainingTimeSec_Basic);
                workTemplate.setAllToFollowFactionAndUpdate(this, newFaction.workTemplate);
                //tradeTemplate.onFactionValueChange(newFaction.tradeTemplate);
                technology.addFactionUnlocked(newFaction.technology, true, false);

                if (newFaction.player != null && newFaction.player.IsLocalPlayer())
                {
                    DssRef.world.copyStockPile(null, newFaction, this, CopyPasteOption.FactionToCity, ResourceGroupType.NUM);
                }
                
            }
        }

        public void upgradeCityHallTooltip(RichBoxContent content, object tag)
        {
            bool available = canUpgradeCityHall(out CraftBlueprint blueprint, out int currentStaff, out int serviceHouses_required, out int serviceHouses_available);

            content.h2(DssRef.lang.Hud_PurchaseTitle_Cost, HudLib.TitleColor_Label);
            blueprint.toMenu(content, this);

            content.newParagraph();

            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain, HudLib.TitleColor_Label);

            content.newLine();
            string supportedWorkersString;
            int addGuardHousing;
            CityType toSize = cityType + 1;
            switch (toSize)
            {
                case CityType.Village:
                    supportedWorkersString = DssConst.VillageHall_MaxWorkForce.ToString();
                    addGuardHousing = DssConst.VillageHall_GuardHousing - DssConst.CampHall_GuardHousing;
                    break;
                case CityType.Town:
                    supportedWorkersString = DssConst.TownHall_MaxWorkForce.ToString();
                    addGuardHousing = DssConst.TownHall_GuardHousing - DssConst.VillageHall_GuardHousing;
                    break;
                default:
                case CityType.Capital:
                    supportedWorkersString = DssRef.lang.Hud_NoLimit;
                    addGuardHousing = DssConst.CapitalHall_GuardHousing - DssConst.TownHall_GuardHousing;
                    break;

            }
            //if (toSize == CityType.Town)
            //{
            //    supportedWorkersString = DssConst.TownHall_MaxWorkForce.ToString();
            //    addGuardHousing = DssConst.TownHall_GuardHousing - DssConst.VillageHall_GuardHousing;
            //}
            //else
            //{
            //    supportedWorkersString = DssRef.lang.Hud_NoLimit;
            //    addGuardHousing = DssConst.CapitalHall_GuardHousing - DssConst.TownHall_GuardHousing;
            //}
            HudLib.BulletPoint(content);
            content.Add(new RbText(string.Format(DssRef.lang.CityHall_MaxSupportedWorkers, supportedWorkersString)));

            content.newLine();
            HudLib.BulletPoint(content);
            content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.GuardHousingCount, TextLib.PlusMinus(addGuardHousing))));

            content.newParagraph();

            content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn, HudLib.TitleColor_Label);
            blueprint.listResources(content, this);
        }

        public bool CanUpgradeCityHall()
        {
            return canUpgradeCityHall(out _, out _, out _, out _);
        }

        bool canUpgradeCityHall(out CraftBlueprint blueprint, out int currentStaff, out int serviceHouses_required, out int serviceHouses_available)
        {
            CityType toSize = cityType + 1;

            switch (toSize)
            {
                case CityType.Campsite:
                    blueprint = null;
                    currentStaff = -1;
                    serviceHouses_required = -1;
                    serviceHouses_available = -1;
                    return false;

                case CityType.Village:
                    blueprint = CraftBuildingLib.CityHall_Village;
                    serviceHouses_required = DssConst.VillageHall_RequiredStaff;
                    currentStaff = 0;
                    break;
                
                case CityType.Town:
                    blueprint = CraftBuildingLib.CityHall_Town;
                    serviceHouses_required = DssConst.TownHall_RequiredStaff - DssConst.VillageHall_RequiredStaff;
                    currentStaff = DssConst.VillageHall_RequiredStaff;
                    break;
                
                case CityType.Capital:
                    blueprint = CraftBuildingLib.CityHall_Capital;
                    serviceHouses_required = DssConst.CapitalHall_RequiredStaff - DssConst.TownHall_RequiredStaff;
                    currentStaff = DssConst.TownHall_RequiredStaff;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("canUpgradeCityHall " + toSize);
            }
            //if (toSize == CityType.Town)
            //{
            //    blueprint = CraftBuildingLib.CityHall_Town;
            //    serviceHouses_required = DssConst.TownHall_RequiredStaff  - DssConst.VillageHall_RequiredStaff;
            //    currentStaff = DssConst.VillageHall_RequiredStaff;
                
            //}
            //else
            //{
            //    blueprint = CraftBuildingLib.CityHall_Capital;
            //    serviceHouses_required = DssConst.CapitalHall_RequiredStaff - DssConst.TownHall_RequiredStaff;
            //    currentStaff = DssConst.TownHall_RequiredStaff;
            //}

            serviceHouses_available = freeServiceMen.amount + currentStaff;

            return serviceHouses_available >= serviceHouses_required &&
                blueprint.available(this);
        }
        public void upgradeCityHall()
        {
            bool available = canUpgradeCityHall(out CraftBlueprint blueprint, out int currentStaff, out int serviceHouses_required, out int serviceHouses_available);

            if (available)
            {
                blueprint.payResources(this);
                //freeServiceMen.amount -= serviceHouses_required;
                cityType++;
                TerrainBuildingType hall;

                switch (cityType)
                {
                    default:
                    case CityType.Village:
                        hall = TerrainBuildingType.CityHall_Village;
                        break;
                    case CityType.Town:
                        hall = TerrainBuildingType.CityHall_Town;
                        break;
                    case CityType.Capital:
                        hall = TerrainBuildingType.CityHall_Capital;
                        break;

                }

                //if (cityType == CityType.Town)
                //{
                //    hall = TerrainBuildingType.CityHall_Town;
                //}
                //else
                //{
                //    hall = TerrainBuildingType.CityHall_Capital;
                //}
                SubTile subTile = new SubTile();
                subTile.SetType(TerrainMainType.Building, (int)hall, 1);
                new EditSubTile(cityHallSubtilePos, subTile, true, false, false).Submit();

                refreshCitySize();
            }
        }

        public Army recruitToClosestArmy()
        {
            return GetFaction().ClosestFriendlyArmy(position, 3.6f);
        }

        public override City GetCity()
        {
            return this;
        }

        public override bool defeatedBy(int attackerFaction)
        {
            return factionIndex == attackerFaction;
        }

        //public override bool defeated()
        //{
        //    return guardCount <= 0;
        //}

        public override bool CanMenuFocus()
        {
            return true;
        }

        public override bool aliveAndBelongTo(int faction)
        {
            return this.factionIndex == faction;
        }

        public override GameObjectType gameobjectType()
        {
            return GameObject.GameObjectType.City;
        }

        public override bool IsArmy()
        {
            return false;
        }
        public override bool IsCity()
        {
            return true;
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
        public UnitBuildType unitType;
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
            this.unitType = (UnitBuildType)r.ReadByte();
            available=r.ReadBoolean();
            goldCost = r.ReadUInt16();
        }
    }

    enum CityType
    {
        UnClaimed,
        Campsite,
        Village,
        Town,
        Capital,
        //Factory,
        NUM
    }
}

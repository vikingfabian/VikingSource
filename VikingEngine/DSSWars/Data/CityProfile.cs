using Microsoft.Xna.Framework.Graphics;
using Sentry;
using System;
using System.Collections.Generic;
using System.IO;
using VikingEngine.DataStream;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.Map2;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.UserGeneratedContent;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.XP;
using VikingEngine.Graphics;
using VikingEngine.Network;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.Core.DSSWars.Data
{

    struct WorkProfile
    {
        public WorkPriorityType workType;
        public WorkPriority workPriority;

        public bool bPriority, bWorkerCount;

        public WorkProfile(WorkPriorityType workType)
        {
            this.workType = workType;
            workPriority.unlocked = true;
            bPriority = true;
        }

        public WorkProfile(System.IO.BinaryReader r, int subversion)
        {
            read(r, subversion);
        }

        public void applyToCity(City city)
        {
            ref var citywork = ref city.workTemplate.GetRefWorkPriority(workType);
            if (bPriority)
            {
                citywork.value = workPriority.value;
                citywork.waitForStockpile = workPriority.waitForStockpile;
            }
            if (bWorkerCount)
            {
                citywork.useWorkerCountLimit = workPriority.useWorkerCountLimit;
                citywork.workerCountLimit = workPriority.workerCountLimit;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)workType);
            workPriority.writeGameState(w, false);
            w.Write(bPriority);
            w.Write(bWorkerCount);
        }
        public void read(System.IO.BinaryReader r, int subversion)
        {
            workType = (WorkPriorityType)r.ReadByte();
            workPriority.readGameState(r, subversion, false);
            bPriority = r.ReadBoolean();
            bWorkerCount = r.ReadBoolean();
        }
    }
    struct ResourceProfile
    {
        public ItemResourceType resourceType;
        public GroupedResource resource;
        
        public bool bStockpile, bRequestDelivery;

        public ResourceProfile(ItemResourceType resourceType)
        {
            this.resourceType = resourceType;
            bStockpile = true;
        }
        public ResourceProfile(System.IO.BinaryReader r, int subversion)
        {
            read(r, subversion);
        }

        public void applyToCity(City city)
        {
            ref var cityresource = ref city.GetRefGroupedResource(ItemPropertyColl.Get(resourceType).cityResourceIndex);
            if (bStockpile)
            {
                cityresource.useStockLimit = resource.useStockLimit;
                cityresource.stockPileLimit = resource.stockPileLimit;
            }
            if (bRequestDelivery)
            {
                cityresource.requestDelivery = resource.requestDelivery;
            }
        }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)resourceType);
            resource.writeNet(w);
            w.Write(bStockpile);
            w.Write(bRequestDelivery);
        }
        public void read(System.IO.BinaryReader r, int subversion)
        {
            resourceType = (ItemResourceType)r.ReadByte();
            resource.readNet(r);
            bStockpile = r.ReadBoolean();
            bRequestDelivery = r.ReadBoolean();
        }
    }

    struct BuidingProfile
    {
        public BuildAndExpandType buildType;
        public byte count;
        public bool accumulate;
        public byte buildPriority;
        public BuidingProfile(BuildAndExpandType buildingType)
        {
            this.buildType = buildingType;
            count = 1;
            buildPriority = 3;
        }
        public BuidingProfile(System.IO.BinaryReader r, int subversion)
        {
            read(r, subversion);
        }

        public void applyToCity(City city)
        {
            int _count = count;
            if (!accumulate)
            {
                _count -= city.buildingStructure.getCount(buildType);
            }
            if (_count > 0)
            {
                BuildControls.AutoPlaceBuilding(city, buildType, buildPriority, _count);
            }
        }
        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)buildType);
            w.Write(count);
            w.Write(accumulate);
            w.Write(buildPriority);
        }
        public void read(System.IO.BinaryReader r, int subversion)
        {
            buildType = (BuildAndExpandType)r.ReadByte();
            count = r.ReadByte();
            accumulate = r.ReadBoolean();
            buildPriority = r.ReadByte();
        }
    }

    

    class CityProfile
    {
        public Guid_8char id;
        public bool favorite, autoApply;
        public bool needsSave;
        public string name;
        public MapObjectTag tag;

        public List2<WorkProfile> workProfiles;
        public List2<ResourceProfile> resourceProfiles;
        public List2<BuidingProfile> buildProfiles;
        

        public CityProfile()
        { 
            id = Guid_8char.NewId();
            name = "City profile_" + id.ToString();
        }
        public void NameEditEvent(string result, object tag)
        {
            if (string.IsNullOrEmpty( result ) == false)
            {
                name = result;
            }
        }

        public void applyToCity(City city)
        {
            if (workProfiles.HasValue())
            {
                foreach (var workProfile in workProfiles)
                {
                    workProfile.applyToCity(city);
                }
            }
            if (resourceProfiles.HasValue())
            {
                foreach (var resProfile in resourceProfiles)
                {
                    resProfile.applyToCity(city);
                }
            }
            if (buildProfiles.HasValue())
            {
                foreach (var buildProfile in buildProfiles)
                {
                    buildProfile.applyToCity(city);
                }
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            new SaveVersion(SaveGamestate.Version, SaveGamestate.SubVersion).write(w);

            id.write(w);
            StreamLib.WriteString2(w, name);
            tag.write(w);

            w.Write(workProfiles.Count);
            if (workProfiles.HasValue())
            {
                foreach (var m in workProfiles)
                {
                    m.write(w);
                }
            }

            w.Write(resourceProfiles.Count);
            if (resourceProfiles.HasValue())
            {
                foreach (var m in resourceProfiles)
                {
                    m.write(w);
                }
            }

            w.Write(buildProfiles.Count);
            if (buildProfiles.HasValue())
            {
                foreach (var m in buildProfiles)
                {
                    m.write(w);
                }
            }

            Debug.WriteCheck(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            SaveVersion version = new SaveVersion();
            version.read(r);

            id = Guid_8char.Read(r);
            name = StreamLib.ReadString2(r);
            tag.read(r);

            int workProfilesCount = r.ReadInt32();
            if (workProfilesCount > 0)
            {
                workProfiles = new List2<WorkProfile>(Math.Min(8, workProfilesCount));
                for (int i = 0; i < workProfilesCount; ++i)
                {
                    workProfiles.Add(new WorkProfile(r, version.sub));
                }
            }

            int resourceProfilesCount = r.ReadInt32();
            if (resourceProfilesCount > 0)
            {
                resourceProfiles = new List2<ResourceProfile>(Math.Min(8, resourceProfilesCount));
                for (int i = 0; i < resourceProfilesCount; ++i)
                {
                    resourceProfiles.Add(new ResourceProfile(r, version.sub));
                }
            }

            int buidingProfilesCount = r.ReadInt32();
            if (buidingProfilesCount > 0)
            {
                buildProfiles = new List2<BuidingProfile>(Math.Min(8, buidingProfilesCount));
                for (int i = 0; i < buidingProfilesCount; ++i)
                {
                    buildProfiles.Add(new BuidingProfile(r, version.sub));
                }
            }

            Debug.ReadCheck(r);
        }

        //public void writeMeta(System.IO.BinaryWriter w)
        //{
            
        //}
        //public void readMeta(System.IO.BinaryReader r, int subversion)
        //{

        //}
    }

    class CityProfileStorage
    {
        public const string ProfileSavesRoot = "City profiles";
        const string FileEnd = ".cpr";

        public bool needsSave = false;

        public List2<CityProfile> profiles;

        public CityProfileStorage()
        {
            profiles = new List2<CityProfile>(8);
        }

        public FilePath ProfilePath(Guid_8char id)
        {
            string saveFileName = $"city profile_{id}";
            FilePath path = new FilePath(ProfileSavesRoot + FilePath.Dir + UGClib.WorkshopSavesLocalFolder, saveFileName, FileEnd, true, false);
            if (id != Guid_8char.Empty)
            {
                path.LocalDirectoryPath += FilePath.Dir + saveFileName;
            }
            return path;
        }

        public void destroyProfile(int index)
        {
            var path = ProfilePath(profiles[index].id);
            if (Directory.Exists(path.CompleteDirectory))
            {
                Directory.Delete(path.CompleteDirectory, true);
            }
            profiles.RemoveAt(index);
        }

        public FilePath MetaPath()
        {
            string saveFileName = $"city profiles_meta";
            FilePath path = new FilePath(ProfileSavesRoot, saveFileName, ".pmt", true, false);
            return path;
        }


        public void Save()
        {
            if (needsSave)
            {
                needsSave = false;
                bool _needSave = false;

                for (int i = 0; i < profiles.Count; i++)
                {
                    var profile = profiles[i];
                    if (profile.needsSave)
                    {
                        SaveProfile(i);
                        profiles.Array[i].needsSave = false;
                        _needSave = true;
                    }
                }

                if (_needSave)
                {
                    FilePath path = MetaPath();
                    new Timer.AsynchActionTrigger(() =>
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(path.CompleteDirectory);
                            DataStream.BeginReadWrite.BinaryIO(true, path, writeMeta, null, null, false);
                        }
                        catch (Exception ex)
                        {
                            BlueScreen.ThreadException = ex;
                        }
                    }, true);
                }
            }
        }

        public void writeMeta(System.IO.BinaryWriter w)
        {
            new SaveVersion(SaveGamestate.Version, SaveGamestate.SubVersion).write(w);
            
            for (int i = 0; i < profiles.Count; i++)
            {
                CityProfile profile = profiles[i];
                if (profile.favorite || profile.autoApply)
                {
                    w.Write(true);
                    profile.id.writeHash(w);
                    w.Write(profile.favorite);
                    w.Write(profile.autoApply);
                }                
            }
            w.Write(false);
            Debug.WriteCheck(w);
        }
        public void readMeta(System.IO.BinaryReader r)
        {
            SaveVersion version = new SaveVersion();
            version.read(r);

            Dictionary<int, int> hashToIndex = new Dictionary<int, int>();
            for (int i = 0; i < profiles.Count; i++)
            {
                hashToIndex.Add(profiles[i].id.GetHashCode(), i);
            }

            while (r.ReadBoolean())
            {
                int hash = Guid_8char.ReadHash(r);
                if (hashToIndex.TryGetValue(hash, out int ix))
                {
                    profiles.Array[ix].favorite = r.ReadBoolean();
                    profiles.Array[ix].autoApply = r.ReadBoolean();
                }
            }
                       
            Debug.ReadCheck(r);
        }

        public void SaveProfile(int profileIndex)
        {
            FilePath path = ProfilePath(profiles[profileIndex].id);

            new Timer.AsynchActionTrigger(() =>
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path.CompleteDirectory);

                    DataStream.BeginReadWrite.BinaryIO(true, path, profiles[profileIndex].write, null, null, false);
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
            }, true);
        }

        public void Load()
        {
            LoadProfileList_Async();
            //new Timer.AsynchActionTrigger(LoadProfileList_Async, true);
        }

        public void LoadProfileList_Async()
        {
            profiles.Clear();

            //TODO: if using workshop, these have to be moved over as a copy

            var list = FilePath.ListFilesInStorageDir2(ProfilePath(Guid_8char.Empty));
            foreach (var path in list)
            {
                var profile = LoadProfile_Async(path, null);
                profiles.Add(profile);
            }
            if (list.Count > 0)
            {
                FilePath metapath = MetaPath();
                DataStream.BeginReadWrite.BinaryIO(false, metapath, null, readMeta, null, false);
            }
        }

        public CityProfile LoadProfile_Async(FilePath path, Action<IconWorldData> onComplete)
        {
            CityProfile data = new CityProfile();
            DataStream.BeginReadWrite.BinaryIO(false, path, null, data.read, null, false);

            return data;
        }

        //public void SaveComplete(bool save, int player, bool completed, byte[] value)
        //{

        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.EntityComponent;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;

namespace VikingEngine.DSSWars.Resource
{
    struct StorageSize
    {
        public static readonly int COUNT = (int)StorageType.NUM_NONE;

        public int storageCount;
      
        public StorageSize()
        {
            storageCount = 0;
        }

        public void addStorage(City city, StorageType storageType, bool add)
        {
            storageCount += lib.BoolToLeftRight(add);
            refreshCapacity(city, storageType);
        }

        public void refreshCapacity(City city, StorageType storageType)
        {
            int capacity = DssConst.StorageStartSize + storageCount * DssConst.StorageBuildingSizeAdd;

            if (storageType == StorageType.FoodStorage && city.cityBiome == CityBiome.Frozen)
            {
                capacity *= 2;
            }

            Task.Run(() =>
            {
                for (ResourceGroupType group = 0; group < ResourceGroupType.NUM; group++)
                {
                    var resources = ResourceLib.ResourceGroupList(group);

                    foreach (var resource in resources)
                    {
                        var properties = ItemPropertyColl.Get(resource);
                        if (properties.storageType == storageType)
                        {                            
                            DssRef.world.cityResouces[city.resourceComponentStartIndex + properties.cityResourceIndex].UpdateCapacity(capacity);
                        }
                    }
                }
            });
        }

        public void write(System.IO.BinaryWriter w)
        { 
            w.Write((ushort)storageCount);
        }
        public void read(System.IO.BinaryReader r, int subVersion)
        {
            storageCount = r.ReadUInt16();
        }
    }

    enum StorageType
    {
        MaterialStorage, FoodStorage, WeaponStorage, ArmorStorage, AnimalStorage, NUM_NONE
    }
}

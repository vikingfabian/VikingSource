using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.DSSWars.Resource;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.Voxels;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace VikingEngine.DSSWars
{
    struct ElephantModelData
    {        
        public ItemResourceType animal;
        public ArmorLevel armor;
        public ItemResourceType balkong;
        public ItemResourceType siegeWeapon;

        public ElephantModelData()
        { }

        public ElephantModelData(Conscript.ConscriptProfile conscript)
        { 
            animal = conscript.animal;
            balkong = conscript.vehicle;
            if (ItemPropertyColl.Get(conscript.weapon).Filter_IsWarMashine)
            {
                siegeWeapon = conscript.weapon;
            }
            else
            { 
                siegeWeapon = ItemResourceType.NONE;
            }

        }

        public override int GetHashCode()
        {
            int result = HashCode.Combine(
                animal,
                armor,
                balkong,
                siegeWeapon
            );

            return result;
        }

        public bool CannonPhant()
        { 
            return animal == ItemResourceType.Oliphant && 
                (siegeWeapon == ItemResourceType.ManCannonIron || siegeWeapon == ItemResourceType.ManCannonBronze);
        }

        public override string ToString()
        {
            return $"ElephantModelData {{ Animal = {animal}, Armor = {armor}, Balkong = {balkong}, SiegeWeapon = {siegeWeapon} }}";
        }
    }

    class ElephantModelBuilder : Voxels.ModelBuilder
    {
        public static int WaitingCount = 0;
        static Dictionary<int, Graphics.AbsVoxelObj> models_loaded =
           new Dictionary<int, Graphics.AbsVoxelObj>();

        public static void Init()
        {
            ItemResourceType[] animals = [ItemResourceType.Elephant, ItemResourceType.WarElephant, ItemResourceType.Oliphant];
            ItemResourceType[] balkongs = [/*ItemResourceType.NONE,*/ ItemResourceType.Wagon2Wheel, ItemResourceType.Wagon4Wheel, ItemResourceType.WagonClosed, ItemResourceType.WagonIron, ItemResourceType.WagonSteel];
            ItemResourceType[] siegeWeapons = [ItemResourceType.NONE, ItemResourceType.Ballista, ItemResourceType.Manuballista, ItemResourceType.Catapult, ItemResourceType.SiegeCannonBronze, ItemResourceType.ManCannonBronze, ItemResourceType.SiegeCannonIron, ItemResourceType.ManCannonIron];

            ElephantModelData modelData = new ElephantModelData();
            foreach (var animal in animals)
            {
                modelData.animal = animal;
                foreach (var balkong in balkongs)
                {
                    modelData.balkong = balkong;
                    foreach (var weapon in siegeWeapons)
                    {
                        if (balkong != ItemResourceType.NONE || weapon == ItemResourceType.NONE)
                        {
                            modelData.siegeWeapon = weapon;

                            ElephantModelData buildData = modelData;
                            WaitingCount++;
                            Task.Run(() =>
                            {
                                try
                                {
                                    ElephantModelBuilder builder = new ElephantModelBuilder();
                                    var grid = builder.buildGrid(buildData);

                                    Ref.update.AddSyncAction(new SyncAction(() =>
                                    {
                                        models_loaded.Add(buildData.GetHashCode(), builder.BuildFromGrid(grid));
                                        WaitingCount--;
                                    }));
                                }
                                catch (Exception ex)
                                {
                                    BlueScreen.ThreadException = ex;
                                }
                            });
                        }
                    }
                }
            }
        }

        public static VoxelModelInstance_Pooled GetInstance(ElephantModelData modelData, float scale = 1f)
        {

            VoxelModelInstance_Pooled instance = DssRef.models.NextInstance_Pooled();
#if DEBUG
            instance.DebugName = modelData.ToString();
#endif
            instance.scale.X = scale;
            instance.scale.Y = 0;

            Graphics.AbsVoxelObj master = null;

            int id = modelData.GetHashCode();
            models_loaded.TryGetValue(id, out master);

            if (master == null)
            {
                id = new ElephantModelData() { animal = modelData.animal }.GetHashCode();
                models_loaded.TryGetValue(id, out master);
            }

            if (master != null)
            {
                instance.SetMaster(master.GetMaster());
                if (instance.scale.X > 0)
                {
                    instance.scale = VectorExt.V3(instance.SizeToScale * instance.scale.X);
                }
            }            

            Ref.draw.drawBatch.Add(instance);

            return instance;
        }

        VoxelObjGridDataAnimHD buildGrid(ElephantModelData modelData)
        {
            IntVector3 balkongOffset = IntVector3.Zero;
            VoxelModelName bodyModel;
            VoxelModelName balkongModel;
            VoxelModelName weaponModel = VoxelModelName.NUM_NON;

            switch (modelData.siegeWeapon)
            {
                case ItemResourceType.Ballista:
                case ItemResourceType.Catapult:
                    weaponModel = VoxelModelName.Phant_ballista;
                    break;
                case ItemResourceType.Manuballista:
                    weaponModel = VoxelModelName.Phant_manuballista;
                    break;
                case ItemResourceType.SiegeCannonBronze:
                    weaponModel = VoxelModelName.Phant_bronzesiege;
                    break;
                case ItemResourceType.ManCannonBronze:
                    if (modelData.animal == ItemResourceType.Oliphant)
                    {
                        weaponModel = VoxelModelName.Phant_bronzecannon_side;
                    }
                    else
                    {
                        weaponModel = VoxelModelName.Phant_bronzecannon;
                    }
                    break;
                case ItemResourceType.SiegeCannonIron:
                    weaponModel = VoxelModelName.Phant_ironsiege;
                    break;
                case ItemResourceType.ManCannonIron:
                    if (modelData.animal == ItemResourceType.Oliphant)
                    {
                        weaponModel = VoxelModelName.Phant_ironcannon_side;
                    }
                    else
                    {
                        weaponModel = VoxelModelName.Phant_ironcannon;
                    }
                    break;
               
            }

            switch (modelData.balkong)
            {
                default:
                    balkongModel = VoxelModelName.NUM_NON;
                    break;
                case ItemResourceType.Wagon2Wheel:
                    balkongModel = VoxelModelName.Phant_balkong2w;
                    break;
                case ItemResourceType.Wagon4Wheel:
                    balkongModel = VoxelModelName.Phant_balkong4w;
                    break;
                case ItemResourceType.WagonClosed:
                    balkongModel = VoxelModelName.Phant_balkong_enforced;
                    break;
                case ItemResourceType.WagonIron:
                    balkongModel = VoxelModelName.Phant_balkong_iron;
                    break;
                case ItemResourceType.WagonSteel:
                    balkongModel = VoxelModelName.Phant_balkong_steel;
                    break;

            }

            switch (modelData.animal)
            {
                default:
                    bodyModel = VoxelModelName.Phant_elephant;
                    break;
                case  ItemResourceType.WarElephant:
                    bodyModel = VoxelModelName.Phant_warelephant;
                    break;
                case  ItemResourceType.Oliphant:
                    bodyModel = VoxelModelName.Phant_oliphant;
                    balkongOffset.Y = 1;
                    break;

            }
            
            VoxelObjGridDataAnimHD grid = DssRef.models.rawModels_temporary[bodyModel].Clone();
            if (balkongModel != VoxelModelName.NUM_NON)
            {
                var balkong = DssRef.models.rawModels_temporary[balkongModel].Frames[0].GetVoxelArray();

                for (int frame = 0; frame < grid.Frames.Count; frame++)
                {
                    grid.Frame(frame).AddVoxels(balkong, balkongOffset);
                }
            }

            if (weaponModel != VoxelModelName.NUM_NON)
            {
                //weaponModel = VoxelModelName.Phant_ironcannon_side;

                var weapon = DssRef.models.rawModels_temporary[weaponModel].Frames[0].GetVoxelArray();

                for (int frame = 0; frame < grid.Frames.Count; frame++)
                {
                    grid.Frame(frame).AddVoxels(weapon, balkongOffset);
                }
            }
            return grid;
            
        }
    }
}

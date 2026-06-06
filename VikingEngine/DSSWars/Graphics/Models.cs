using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using VikingEngine.LootFest.Data;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
   
    class Models
    {

        public Dictionary<VoxelModelName, VoxelObjGridDataAnimHD> rawModels;
        public Dictionary<VoxelModelName, VoxelObjGridDataAnimHD> rawModels_temporary;
        public Dictionary<VoxelModelName, Graphics.VoxelModel> voxelModels = new Dictionary<VoxelModelName, Graphics.VoxelModel>();

        public Dictionary<VoxelModelName, WeaponModel> weaponModels;

        public ShieldModel BucklerShield, RoundShield, HeaterShield, TowerShield;

        List<VoxelModelData> loadedData = new List<VoxelModelData>();
        bool asycTaskComplete = false;

        public Texture2D[] waterTextures;
        public Texture2D[] seaTextures;
        //public Texture2D[] waterEdgeTextures;
        public Stack<Mesh> shipWaveModels = new Stack<Mesh>(64);

        public Models()
        {
            DssRef.models = this;
        }

        public void loadContent()
        {
            waterTextures = new Texture2D[4];
            for (int i = 1; i <= 4; ++i)
            {
                waterTextures[i - 1] = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "watertex_i" + i);
            }

            seaTextures = new Texture2D[4];
            for (int i = 1; i <= 4; ++i)
            {
                seaTextures[i - 1] = Ref.main.Content.Load<Texture2D>(DssLib.ContentDir + "seatex_i" + i);
            }

            //RAW
            List<VoxelModelName> loadRawModels = new List<VoxelModelName>
            {
                DssLib.WorkerModel,
                VoxelModelName.war_recruit,
                VoxelModelName.wars_shipcrew,
                VoxelModelName.wars_captain,

                VoxelModelName.banner,
                VoxelModelName.wars_flag,
                VoxelModelName.horsebanner,
                VoxelModelName.armystand,
                VoxelModelName.armystand_detail,
                VoxelModelName.cityicon,
                VoxelModelName.citybanner,
                VoxelModelName.pin,

                VoxelModelName.modsoldier_debug,
                VoxelModelName.modsoldier_body1,
                VoxelModelName.modsoldier_body_beef1,
                VoxelModelName.modsoldier_body3lady,
                VoxelModelName.modsoldier_face1,
                VoxelModelName.modsoldier_face_orc,
                VoxelModelName.modsoldier_face_skull,
         
                VoxelModelName.modsoldier_leg1,

                VoxelModelName.modsoldier_larm_empty1,
                VoxelModelName.modsoldier_larm_shield1,
                VoxelModelName.modsoldier_rarm_sword1,
                VoxelModelName.modsoldier_rarm_bow1v2,

                VoxelModelName.modsoldier_larm_empty2naked,
                VoxelModelName.modsoldier_larm_shield2naked,
                VoxelModelName.modsoldier_rarm_sword2naked,
                VoxelModelName.modsoldier_rarm_bow2naked,

                VoxelModelName.modsoldier_addons,
                VoxelModelName.modsoldier_face_access,
                VoxelModelName.modsoldier_hat_soldier_all,
                VoxelModelName.modsoldier_hat_custom_all,


        


            };
            rawModels = new Dictionary<VoxelModelName, VoxelObjGridDataAnimHD>(loadRawModels.Count);

            List<VoxelModelName> loadTemporaryRawModels = new List<VoxelModelName>
            {
                VoxelModelName.Phant_elephant,
                VoxelModelName.Phant_warelephant,
                VoxelModelName.Phant_oliphant,


                VoxelModelName.Phant_balkong2w,
                VoxelModelName.Phant_balkong4w,
                VoxelModelName.Phant_balkong_enforced,
                VoxelModelName.Phant_balkong_iron,
                VoxelModelName.Phant_balkong_steel,
                VoxelModelName.Phant_ballista,
                VoxelModelName.Phant_bronzecannon,
                VoxelModelName.Phant_bronzecannon_side,
                VoxelModelName.Phant_bronzesiege,
                VoxelModelName.Phant_ironcannon,
                VoxelModelName.Phant_ironcannon_side,
                VoxelModelName.Phant_ironsiege,
                VoxelModelName.Phant_manuballista,
            };

            rawModels_temporary = new Dictionary<VoxelModelName, VoxelObjGridDataAnimHD>(loadTemporaryRawModels.Count);

                List <VoxelModelName> loadWeaponModels = new List<VoxelModelName>
            {
                VoxelModelName.modweapon_sword1,
                
                
                VoxelModelName.modweapon_blunderbuss,
                VoxelModelName.modweapon_crossbow,
                VoxelModelName.modweapon_culvertin,
                VoxelModelName.modweapon_hammer,
                VoxelModelName.modweapon_handcannon,
                VoxelModelName.modweapon_javelin,
                VoxelModelName.modweapon_longbow,
                VoxelModelName.modweapon_mithrilbow,
                VoxelModelName.modweapon_mithrilsword,
                VoxelModelName.modweapon_rifle,
                VoxelModelName.modweapon_sharpstick,
                VoxelModelName.modweapon_settler,
                VoxelModelName.modweapon_shortbow,
                VoxelModelName.modweapon_sling,
                VoxelModelName.modweapon_spear,
                VoxelModelName.modweapon_twohand,

                VoxelModelName.modweapon_shortsword,
                VoxelModelName.modweapon_longsword,
                VoxelModelName.modweapon_bronzesword,

                //VoxelModelName.modshield_javelin,
                //VoxelModelName.modshield_roman,
                VoxelModelName.modshield_knightsmallside,
                //VoxelModelName.modshield_forward1,
            };

            var units = new AllUnits();
            units.AddRawModelsToLoad(loadRawModels);

            foreach (var model in units.AddUniqueModelsToLoad())
            {
                loadVoxelModel(model, false);
            }

            loadRawModelsToDic(loadRawModels, rawModels);
            loadRawModelsToDic(loadTemporaryRawModels, rawModels_temporary);

            //foreach (var modelName in loadRawModels)
            //{
            //    DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
            //    byte[] data = DataStream.FileToDiskManager.Read(path);
            //    Task.Run(() =>
            //    {
            //        try
            //        {
            //            System.IO.MemoryStream s = new System.IO.MemoryStream(data);
            //            System.IO.BinaryReader r = new System.IO.BinaryReader(s);

            //            var grids = VoxelObjDataLoader.LoadVoxelObjGridHD(r);
            //            var result = new VoxelObjGridDataAnimHD(grids);

            //            lock (rawModels)
            //            {
            //                rawModels.Add(modelName, result);
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            BlueScreen.ThreadException = ex;
            //        }
            //    });
            //}

            weaponModels = new Dictionary<VoxelModelName, WeaponModel>(loadWeaponModels.Count);
            foreach (var weaponName in loadWeaponModels)
            {
                weaponModels.Add(weaponName, new WeaponModel(weaponName));
            }

            BucklerShield = new ShieldModel(VoxelModelName.modshield_forward1, 0);
            RoundShield = new ShieldModel(VoxelModelName.modshield_forward1, 1);
            HeaterShield = new ShieldModel(VoxelModelName.modshield_forward1, 2);
            TowerShield = new ShieldModel(VoxelModelName.modshield_forward1, 3);


            //VOXEL
            loadVoxelModel(VoxelModelName.ErrorCube, false);
            loadVoxelModel(VoxelModelName.unclaimed_icon, false);
            loadVoxelModel(VoxelModelName.war_town1, false);
            loadVoxelModel(VoxelModelName.war_town2, false);
            loadVoxelModel(VoxelModelName.war_town3, false);
            loadVoxelModel(VoxelModelName.war_town_factory, false);
            loadVoxelModel(VoxelModelName.war_workerhut, false);
            loadVoxelModel(VoxelModelName.city_mine, false);
            loadVoxelModel(VoxelModelName.city_workstation, false);
            loadVoxelModel(VoxelModelName.city_meatstation, false);
            loadVoxelModel(VoxelModelName.city_storage, false);

            loadVoxelModel(VoxelModelName.city_dirtwall, false);
            loadVoxelModel(VoxelModelName.city_dirttower, false);
            loadVoxelModel(VoxelModelName.city_palisade, false);
            loadVoxelModel(VoxelModelName.city_woodwall, false);
            loadVoxelModel(VoxelModelName.city_woodtower, false);
            loadVoxelModel(VoxelModelName.city_stonewall, false);
            loadVoxelModel(VoxelModelName.city_stonetower, false);

            loadVoxelModel(VoxelModelName.city_stonehall, false);
            loadVoxelModel(VoxelModelName.city_tenthut, false);
            loadVoxelModel(VoxelModelName.city_workerhut, false);
            loadVoxelModel(VoxelModelName.city_workerhut_long, false);
            loadVoxelModel(VoxelModelName.city_guard_house, false);
            loadVoxelModel(VoxelModelName.city_pen, false);

            loadVoxelModel(VoxelModelName.city_cobblestone, false);
            loadVoxelModel(VoxelModelName.city_square, false);
            loadVoxelModel(VoxelModelName.city_smallhouse, false);
            loadVoxelModel(VoxelModelName.city_bighouse, false);
            loadVoxelModel(VoxelModelName.city_storehouse, false);
            loadVoxelModel(VoxelModelName.city_tavern, false);
            loadVoxelModel(VoxelModelName.city_bank, false);
            loadVoxelModel(VoxelModelName.city_postal, false);
            //loadVoxelModel(VoxelModelName.city_recruitment, false);
            loadVoxelModel(VoxelModelName.city_barracks, false);
            loadVoxelModel(VoxelModelName.city_carpenter, false);
            loadVoxelModel(VoxelModelName.city_nobelhouse, false);
            loadVoxelModel(VoxelModelName.city_logistic, false);
            loadVoxelModel(VoxelModelName.city_quarry, false);
            loadVoxelModel(VoxelModelName.city_trapper, false);
            loadVoxelModel(VoxelModelName.city_water, false);
            loadVoxelModel(VoxelModelName.city_tent, false);
            loadVoxelModel(VoxelModelName.city_research, false);

            loadVoxelModel(VoxelModelName.decor_statue, false);
            loadVoxelModel(VoxelModelName.decor_netstatue, false);
            loadVoxelModel(VoxelModelName.city_flagpole, false);
            loadVoxelModel(VoxelModelName.city_pavement, false);
            loadVoxelModel(VoxelModelName.city_garden, false);

            loadVoxelModel(VoxelModelName.Boar, false);
            loadVoxelModel(VoxelModelName.Pig, false);
            loadVoxelModel(VoxelModelName.Hen, false);
            loadVoxelModel(VoxelModelName.dog1, false);
            loadVoxelModel(VoxelModelName.hound1, false);
            loadVoxelModel(VoxelModelName.Pheasant, false);
            loadVoxelModel(VoxelModelName.Arrow, true);
            loadVoxelModel(VoxelModelName.slingstone, true);
            loadVoxelModel(VoxelModelName.boulder_proj, true);
            loadVoxelModel(VoxelModelName.little_javelin, true);
            loadVoxelModel(VoxelModelName.little_boltarrow, true);
            loadVoxelModel(VoxelModelName.war_cannonball, true);
            loadVoxelModel(VoxelModelName.war_gunblast, true);
            loadVoxelModel(VoxelModelName.war_ballista_proj, true);
            loadVoxelModel(VoxelModelName.wars_loading_anim, true);
            loadVoxelModel(VoxelModelName.wars_shipbuild, true);

            loadVoxelModel(VoxelModelName.wars_deserter, false);
            loadVoxelModel(VoxelModelName.horse_brown, false);
            loadVoxelModel(VoxelModelName.horse_white, false);
            loadVoxelModel(VoxelModelName.wildpig1, false);
            loadVoxelModel(VoxelModelName.hog1, false);
            loadVoxelModel(VoxelModelName.warhog1, false);
            loadVoxelModel(VoxelModelName.staghog1, false);
            loadVoxelModel(VoxelModelName.wolf1, false);
            loadVoxelModel(VoxelModelName.warg1, false);
            loadVoxelModel(VoxelModelName.alphawarg1, false);
            loadVoxelModel(VoxelModelName.wildcat1, false);
            loadVoxelModel(VoxelModelName.lion1, false);
            loadVoxelModel(VoxelModelName.warlion1, false);

            loadVoxelModel(VoxelModelName.Phant_elephant, false);
            loadVoxelModel(VoxelModelName.Phant_warelephant, false);
            loadVoxelModel(VoxelModelName.Phant_oliphant, false);

            loadVoxelModel(VoxelModelName.Fowl, false);
            loadVoxelModel(VoxelModelName.oxen1, false);
            loadVoxelModel(VoxelModelName.kineoxen1, false);
            loadVoxelModel(VoxelModelName.pony_brown, false);
            loadVoxelModel(VoxelModelName.pony_pink, false);
            loadVoxelModel(VoxelModelName.drafthorse_red, false);
            loadVoxelModel(VoxelModelName.warhorse_brown, false);
            


            loadVoxelModel(VoxelModelName.wagon_light, false);
            loadVoxelModel(VoxelModelName.wagon_light4, false);
            loadVoxelModel(VoxelModelName.wagon_coach, false);
            loadVoxelModel(VoxelModelName.wagon_ironcoach, false);

            loadVoxelModel(VoxelModelName.cannonwagon_siegebronze, false);
            loadVoxelModel(VoxelModelName.cannonwagon_manuballista, false);
            loadVoxelModel(VoxelModelName.cannonwagon_catapult, false);
            loadVoxelModel(VoxelModelName.cannonwagon_ballista, false);
            loadVoxelModel(VoxelModelName.cannonwagon_manbronze, false);
            loadVoxelModel(VoxelModelName.cannonwagon_maniron, false);
            loadVoxelModel(VoxelModelName.cannon4wagon_maniron, false);
            loadVoxelModel(VoxelModelName.cannoncoach_manbronze, false);
            loadVoxelModel(VoxelModelName.cannoncoach_maniron, false);
            loadVoxelModel(VoxelModelName.cannoncoach_siegeiron, false);

            loadVoxelModel(VoxelModelName.wars_shipmelee, false);
            loadVoxelModel(VoxelModelName.buildarea, false);
            loadVoxelModel(VoxelModelName.godfire, false);
            loadVoxelModel(VoxelModelName.wars_borderstick, false);

            foreach (var model in DetailMapTile.LoadModel())
            {
                loadVoxelModel(model, false);
            }

            asycTaskComplete = true;

            void loadVoxelModel(VoxelModelName modelName, bool centerY)
            {
                float yAdjust = 0;

                DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
                byte[] data = DataStream.FileToDiskManager.Read(path);
                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                System.IO.BinaryReader r = new System.IO.BinaryReader(s);

                Vector3 centerAdjust = new Vector3(0, yAdjust, 0);

                List<VoxelObjGridDataHD> loadedFrames = VoxelObjDataLoader.LoadVoxelObjGridHD(r);

                if (centerY)
                    centerAdjust += loadedFrames[0].CenterAdj();
                else
                    centerAdjust += loadedFrames[0].BottomCenterAdj();

                IntVector3 gridSz = loadedFrames[0].Size;

                List<Frame> framesData;
                IVerticeData verticeData = VoxelObjBuilder.BuildVerticesHD(loadedFrames, centerAdjust, out framesData);

                loadedData.Add(new VoxelModelData(modelName, verticeData, gridSz, framesData)); 
            }
        }

        void loadRawModelsToDic(List<VoxelModelName> loadRawModels, Dictionary<VoxelModelName, VoxelObjGridDataAnimHD> toDictionary)
        {
            foreach (var modelName in loadRawModels)
            {
                DataStream.FilePath path = VoxelObjDataLoader.ContentPath(modelName);
                byte[] data = DataStream.FileToDiskManager.Read(path);
                Task.Run(() =>
                {
                    try
                    {
                        System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                        System.IO.BinaryReader r = new System.IO.BinaryReader(s);

                        var grids = VoxelObjDataLoader.LoadVoxelObjGridHD(r);
                        var result = new VoxelObjGridDataAnimHD(grids);

                        lock (toDictionary)
                        {
                            toDictionary.Add(modelName, result);
                        }
                    }
                    catch (Exception ex)
                    {
                        BlueScreen.ThreadException = ex;
                    }
                });
            }

        }

        public void sychLoading()
        {
            if (loadedData != null && asycTaskComplete)
            {
                foreach (VoxelModelData data in loadedData)
                {
                    voxelModels.Add(data.name, data.sychedProcessing());
                }
                loadedData = null;
            }
            
        }
        public void recycle(ref Graphics.VoxelModelInstance instance, bool detailLayer, bool allowRecycle = true)
        {

            if (instance != null)
            {
                if (detailLayer)
                {
                    //if (allowRecycle)
                    //{
                    //    instance.Visible = false;
                    //    instance.Rotation = RotationQuarterion.Identity;
                    //    DssRef.state.modelPool(detailLayer).Push(instance);
                    //}
                    
                    instance.SetInRender(false);
                }
                else
                {

                    if (allowRecycle)
                    {
                        instance.Visible = false;
                        instance.Rotation = RotationQuarterion.Identity;
                        DssRef.state.modelPool(detailLayer).Push(instance);
                    }
                    else
                    {
                        instance.DeleteMe();
                    }
                }
            }
            
            instance = null;
        }

        public VoxelModelInstance_Pooled NextInstance_Pooled()
        {
            VoxelModelInstance_Pooled instance;
            if (DssRef.state.voxelModelInstancesPooled.TryPop(out instance))
            {
                instance.Pool_Reset();
            }
            else
            {
                instance = new VoxelModelInstance_Pooled(true);
            }
            return instance;
        }

        public VoxelModelInstance_Pooled ModelInstance_drawbatch(
            VoxelModelName name,
            float scale = 1f)
        {
            VoxelModelInstance_Pooled instance = NextInstance_Pooled();
           
#if DEBUG
            instance.DebugName = name.ToString();
#endif

            Graphics.VoxelModel master = voxelModels[name];
            instance.SetMaster(master);
            if (scale > 0)
            {
                instance.scale = VectorExt.V3(instance.SizeToScale * scale);
            }

            Ref.draw.drawBatch.Add(instance.master.modelIndex, instance);               
            
            return instance;        
        }

        public Graphics.VoxelModelInstance ErrorModel(float scale = 1f)
        {
            return new Graphics.VoxelModelInstance(voxelModels[VoxelModelName.ErrorCube], false) { scale = new Vector3(scale) };
        }

        public Graphics.VoxelModelInstance ModelInstance(            
            VoxelModelName name,
            bool detailLayer,
            float scale = 1f,
            bool allowRecycle = true,
            bool addToRender = true, 
            bool async = false)
        {
            
            Graphics.VoxelModelInstance instance;
            if (allowRecycle && addToRender &&
                DssRef.state.modelPool(detailLayer).TryPop(out instance))
            {
                instance.Visible = true;
                instance.Frame = 0;
                instance.SpottedArrayMemberIndex = -1;
                instance.inPlayerCamera = EightBit.AllTrue;
               
            }
            else
            {
                instance = new Graphics.VoxelModelInstance(null, false);
                if (addToRender)
                {                    
                    if (!detailLayer)
                    {
                        int lay = detailLayer ? DrawGame.UnitDetailLayer : DrawGame.MidLayer;

                        if (async)
                        {
                            Ref.update.AddSyncAction(new SyncAction1Arg<int>(instance.AddToRender, lay));
                        }
                        else
                        {
                            instance.AddToRender(lay);
                        }
                    }
                }
            }

#if DEBUG
            instance.DebugName = name.ToString();
#endif

            Graphics.VoxelModel master = voxelModels[name];
            instance.SetMaster(master);
            if (scale > 0)
            {
                instance.scale = VectorExt.V3(instance.SizeToScale * scale);
            }

            if (addToRender && detailLayer)
            {
                if (async)
                {
                    Ref.update.AddSyncAction(new SyncAction(() => {
                        Ref.draw.drawBatch.Add(instance.master.modelIndex, instance);
                    }));
                }
                else
                {
                    Ref.draw.drawBatch.Add(instance.master.modelIndex, instance);
                }
            }

            return instance;
        }

    }
    class VoxelModelData
    {
        public VoxelModelName name;

        IVerticeData verticeData;
        IntVector3 gridSz;
        List<Frame> framesData;

        public VoxelModelData(VoxelModelName name, IVerticeData verticeData, IntVector3 gridSz, List<Frame> framesData)
        {
            this.name = name;
            this.verticeData = verticeData;
            this.gridSz = gridSz;
            this.framesData = framesData;
        }

        public VoxelModel sychedProcessing()
        {
            Graphics.VoxelModel master = VoxelObjBuilder.BuildModelHD(verticeData, gridSz, framesData);

            return master;
        }
    }

}

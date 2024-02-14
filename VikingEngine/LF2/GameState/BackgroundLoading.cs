//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//using System.Threading;

//namespace Game1.LootFest
//{
//    interface ILF_LoaderFeedback
//    {
//        void DoneLoading(LoadOrder part);
//    }
//    class BackgroundLoading: LasyUpdate
//    {
//        public ILF_LoaderFeedback callbackObj;
//        static bool done;
//        static LoadOrder loading = (LoadOrder)0;
//        public Map.World WorldMap;
//        static LoadOrder lastCompleted = LoadOrder.Init;

//        public BackgroundLoading(ILF_LoaderFeedback callbackObj, bool basicLoadingDone)
//            : base(true)
//        {
//            this.callbackObj = callbackObj;
//            if (basicLoadingDone)
//            {
//                loading = LoadOrder.Map;
//            }
//            done = true;
//        }


//        /// <summary>
//        /// Will be run as a thread?
//        /// </summary>
//        void loadThis()
//        {

//            LoadOrder load = loading;
//            done = false;
//            switch (load)
//            {
//                case LoadOrder.DataImagesInit:
//                     LootfestLib.Images.Init();//mt
//                    break;
//                case LoadOrder.DataMaterialBuilderInit:
//                    Data.MaterialBuilder.InitMaterials();//ej
//                    break;
//                case LoadOrder.DataBlockInit:
//                    Data.Block.Init();//mt
//                    break;

//                case LoadOrder.MapPreparedFaceInit:
//                     Map.PreparedFace.Init();//mt
//                    break;
//                case LoadOrder.EngineSpaceGraphicsIn3DGeneratedVoxelObjInitEffect:
//                    Graphics.VoxelObj.InitEffect();//mt?
//                    break;
//                case LoadOrder.DataCharactersWorldCharactersInit:
//                    //Data.Characters.WorldCharacters.NewWorld();//mt
//                    break;

//                case LoadOrder.DataPickUpInit:
//                    //Data.PickUp.Init();//mt
//                    Map.Terrain.ObsticleBuilder.Init();
//                    break;
////--till introscene                
//                case LoadOrder.Map:
//                    WorldMap = new Map.World(true);//mt
//                    break;
//                case LoadOrder.PixelIcons:
//                   // LootfestLib.Images.CreateVoxelIcons();//ej
//                    break;

//                case LoadOrder.GameIsStartAble:
//                    //check for errors
//                    if (WorldMap == null)
//                    {
//                        throw new NullReferenceException();
//                    }
//                    //if (Data.Characters.WorldCharacters.NullEnemies)
//                    //{
//                    //    throw new NullReferenceException();
//                    //}
//                    //if (Data.Block.)
//                    //{
//                    //    throw new NullReferenceException();
//                    //}
                    
//                    break;
//                case LoadOrder.MenuBackground:

//                    break;
//                case LoadOrder.SoundMusic:

//                    break;

//            }
//            lastCompleted = load;
//            loading++;
//            done = true;
//            if (loading != LoadOrder.Init)
//            {
//                callbackObj.DoneLoading(loading);
//            }
//        }

//        bool isThread(LoadOrder loading)
//        {
//            return loading != LoadOrder.PixelIcons && loading != LoadOrder.DataMaterialBuilderInit;
//            //if (loading == LoadOrder.DataBlockInit)
//            //{
//            //    return true;
//            //}
//            //return false;
//        }
        
//        public override void Time_Update(float time)
//        {
//            if (done)
//            {
//                done = false;
//                //if (loading != LoadOrder.Init)
//                //{
//                //    callbackObj.DoneLoading(loading);
//                //}
//                switch (loading)
//                {
//                    case LoadOrder.GameIsStartAble:

//                        break;
//                    case LoadOrder.MenuBackground:

//                        break;
                    

//                }
                
//                if (loading == LoadOrder.Done)
//                {
//                    DeleteMe();
//                }
//                else
//                {
//                    if (isThread(loading))
//                    {
//                        Thread t = new Thread(loadThis);
//                        t.Start();
//                    }
//                    else
//                    {
//                        loadThis();
//                    }
//                }
                
//            }
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//        }
                
        
//    }
//    enum LoadOrder
//    {
//        Init,
//        DataImagesInit,//mt
//        DataMaterialBuilderInit,//ej

//        DataBlockInit,//mt
//        MapPreparedFaceInit,//mt

//        EngineSpaceGraphicsIn3DGeneratedVoxelObjInitEffect,//mt?
//        DataCharactersWorldCharactersInit,//mt
//        DataPickUpInit,//mt


//        Map,//kart load börjar här
//        PixelIcons,

//        GameIsStartAble,
//        MenuBackground,
//        SoundMusic,
//        Done,
//        NUM
//    }
//}

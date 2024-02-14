//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.LootFest.Map.Terrain.Generation;
//using VikingEngine.LootFest.Map;
//using VikingEngine.LootFest.GO;
//using VikingEngine.LootFest.Map.Terrain;
//using VikingEngine.LootFest.GO.Characters;
//using VikingEngine.EngineSpace.Maths;
//using VikingEngine.LootFest.GO.Characters.Monster3;

//namespace VikingEngine.LootFest
//{
//    class LobbyDebugArea : AbsRoomAttr
//    {

//        public LobbyDebugArea()
//            : base()//VoxelModelName.JointTest, 2, 20)
//        { }

//        protected override void OnCalculateSize(PcgRandom prng)
//        {
//            //base.OnCalculateSize();
//            //rect.size = new IntVector2(30);
//            //IntVector2 pos = rect.size / 2;
//            //AddGameObjectConstructor(pos, CreateTutorialObject);

//            if (PlatformSettings.DevBuild)
//            {
//                rect.size = new IntVector2(30);
//                //HandMadeTerrainInstance terrainModelTest = new HandMadeTerrainInstance(IntVector2.Zero, -1, VoxelModelName.Palace, Dir4.N);
//                //rect.size = terrainModelTest.GetRotatedPlaneSize();
//                //terrainModelTest.FirstLocal_ThenWorld_BottomCenterPos.PlaneCoordinates += rect.size / 2;
//                //AddTerrainObject(terrainModelTest);


//                //createAndRotateSpawners(terrainModelTest);
//            }
//            else
//            {
//                rect.size = new IntVector2(1);
//            }
//            //base.OnCalculateSize(prng);
//        }

//    public override void  OnLayoutComplete()
//{
//     base.OnLayoutComplete();
//            base.OnLayoutComplete();
//            //new SpawnPointDelegate(new WorldPosition(rect.Center, Map.WorldPosition.ChunkStandardHeight),
//            //    CreateTutorialObject, SpawnPointData.Empty, SpawnImportance.Must_0);

//            //var joints = modelInstance.GetJointInfos();
//            //var jList = joints.getAllJoints(forward);
//            ////List<Spawner> result = new List<Spawner>(jList.Count);

//            //foreach (var j in jList)
//            //{
//            //    //result.Add(new Spawner(typeof(OrcArcher), j.position, 3, 20, null, null));
//            //    var portal = new SpawnPointMonsterPortal(new Map.WorldPosition(VectorExt.V3plusV2(j.position, rect.pos)), 
//            //        new SuggestedSpawns(new SpawnPointData(GameObjectType.SpitChick, 0, 0)));
//            //    portal.maxActiveInstances = 1;
//            //}
//     //new SpawnPointDelegate(rect.pos, CreateTutorialObject, SpawnPointData.Empty, SpawnImportance.Must_0, false); 
//        }

//        protected void  CreateTutorialObject(GoArgs args)
//        {
//            new GO.NPC.SuitGranpa(args);

            
            
//            args.startWp.X -= 14;

//            //new GO.Characters.Boss.BirdRiderBoss(args, null);
//            //new GO.PickUp.SuitBox(args.startWp.PositionV3, GO.SuitType.Archer);
//            //args.startWp.X += 4;
//            //new GO.PickUp.SuitBox(args.startWp.PositionV3, GO.SuitType.Swordsman);
//            //args.startWp.X += 4;
//            //new GO.PickUp.SuitBox(args.startWp.PositionV3, GO.SuitType.SpearMan);
//            //args.startWp.X += 4;
//            //new GO.PickUp.SuitBox(args.startWp.PositionV3, GO.SuitType.BarbarianDane);
//            //args.startWp.X += 4;
//            //new GO.PickUp.SuitBox(args.startWp.PositionV3, GO.SuitType.BarbarianDual);
            
//            //args.startWp.X += 4;
//            //new GO.PickUp.SuitBox(args.startWp.PositionV3, GO.SuitType.ShapeShifter);

//            if (PlatformSettings.DevBuild)
//            {
//                //new GO.EnvironmentObj.Snowman(args);
//                //new GO.Characters.HumanionEnemy.ElfWardancer(args);

//                args.startWp.X += 4;
//                new Spider(args);
//                //new GO.Characters.HumanionEnemy.ElfArcher(args);
//               // new GO.EnvironmentObj.Snowman(args);
//                //new GO.Characters.Pitbull(args);
//                args.startWp.X += 4;
//                new Spider(args);
//                //new GO.EnvironmentObj.Snowman(args);

//                //args.startWp.X += 20;
//                //new GO.Characters.Horse(args);
//                //new GO.Characters.Hog3(args);
//                //new GO.Characters.Hog3(args);

//            }
//        }

//        //protected override List<Spawner> CreateSpawners(HandMadeTerrainMasterProfileData joints)
//        //{
//        //    //var jList = joints.getAllJoints(forward);
//        //    //List<Spawner> result = new List<Spawner>(jList.Count);

//        //    //foreach (var j in jList)
//        //    //{
//        //    //    result.Add(new Spawner(typeof(OrcArcher), j.position, 3, 20, null, null));
//        //    //}

//        //    //return result;
//        //    return null;
//        //}
//    }
//}

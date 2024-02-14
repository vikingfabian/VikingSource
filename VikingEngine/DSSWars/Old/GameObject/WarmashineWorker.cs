//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace VikingEngine.DeepSimStrategy.GameObject
//{
//    class WarmashineWorkerCollection
//    {
//        List<WarmashineWorker> members = new List<WarmashineWorker>(2);

//        public void Add(AbsPlayer2 player, float xDiff, float zDiff)
//        {
//            members.Add(new WarmashineWorker(player, new Vector3(xDiff, 0, zDiff)));
//        }

//        public void update(AbsSoldier parent)
//        {
//            foreach (var m in members)
//            {
//                m.update(parent);
//            }
//        }

//        public void DeleteMe()
//        {
//            foreach (var m in members)
//            {
//                m.DeleteMe();
//            }
//        }
//    }

//    class WarmashineWorker
//    {
//        WalkingAnimation walkingAnimation = WalkingAnimation.Standard;
//        Graphics.AbsVoxelObj model;
//        Vector3 diff;

//        public WarmashineWorker(AbsPlayer2 player, Vector3 diff)
//        {
//            this.diff = diff;
//            LootFest.VoxelModelName modelName;

//            //if (player.faction == Faction.Human)
//                modelName = LootFest.VoxelModelName.little_workerman;
//            //else
//            //    modelName = LootFest.VoxelModelName.little_workerorc;

//            model = LootFest.LfRef.modelLoad.AutoLoadModelInstance(modelName, AbsUnit.StandardModelScale * 0.9f, 0, false);
//        }

//        public void update(AbsSoldier parent)
//        {
//            if (parent.state.walking || parent.state.rotating)
//            {
//                float move = parent.walkingSpeedWithModifiers();
//                walkingAnimation.update(move, model);
//            }
//            else
//            {
//                model.Frame = 0;
//            }

//            model.Rotation = parent.model.Rotation;
//            model.position = parent.model.Rotation.TranslateAlongAxis(diff, parent.model.position);
//        }

//        public void DeleteMe()
//        {
//            model.DeleteMe();
//        }
//    }
//}

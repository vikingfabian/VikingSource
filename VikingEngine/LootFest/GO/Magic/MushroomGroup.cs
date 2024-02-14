//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.Magic
//{
//    class MushroomGroup : AbsUpdateable
//    {
//        const int NumMushrooms = 5;

//        Vector3 userPos; Rotation1D userFireDir;
//        bool rightDir = Ref.rnd.Bool();
//        int mushroomIndex = 0;
//        Timer.Basic timer;

//        public MushroomGroup(Vector3 userPos, Rotation1D userFireDir)
//            : base(true)
//        {
//            this.userFireDir = userFireDir; this.userPos = userPos;
//            nextMusroom();
//        }

//        public override void Time_Update(float time)
//        {
//            if (timer.Update())
//            {
//                nextMusroom();
//            }
//        }

//        void nextMusroom()
//        {
//            Rotation1D dir = userFireDir;
//            dir.Radians += lib.BoolToDirection(rightDir) * Ref.rnd.Float(0.28f, 0.3f) * mushroomIndex;
//            Vector3 pos = 
//                new Vector2toV3(
//                dir.Direction(lib.RandomPercentDifferance(3, 0.1f))) + 
//                userPos;

//            new Mushroom(pos);

//            timer.Set(Ref.rnd.Float(60, 120));
//            ++mushroomIndex;
//            if (mushroomIndex >= NumMushrooms)
//            {
//                this.DeleteMe();
//            }
//            rightDir = !rightDir;
//        }
//    }
//}

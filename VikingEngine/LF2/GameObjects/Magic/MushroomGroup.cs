using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Magic
{
    class MushroomGroup : AbsUpdateable
    {
        const int NumMushrooms = 5;

        Vector3 userPos; Rotation1D userFireDir;
        bool rightDir = lib.RandomBool();
        int mushroomIndex = 0;
        Timer.Basic timer;

        public MushroomGroup(Vector3 userPos, Rotation1D userFireDir)
            : base(true)
        {
            this.userFireDir = userFireDir; this.userPos = userPos;
            nextMusroom();
        }

        public override void Time_Update(float time)
        {
            if (timer.Update())
            {
                nextMusroom();
            }
        }

        void nextMusroom()
        {
            Rotation1D dir = userFireDir;
            dir.Radians += lib.BoolToDirection(rightDir) * lib.RandomFloat(0.28f, 0.3f) * mushroomIndex;
            Vector3 pos = 
                Map.WorldPosition.V2toV3(
                dir.Direction(lib.RandomPercentDifferance(GameObjects.Gadgets.WeaponGadget.Staff.MushroomPlacementDist, 0.1f))) + 
                userPos;

            new Mushroom(pos);

            timer.Set(lib.RandomFloat(60, 120));
            ++mushroomIndex;
            if (mushroomIndex >= NumMushrooms)
            {
                this.DeleteMe();
            }
            rightDir = !rightDir;
        }
    }
}

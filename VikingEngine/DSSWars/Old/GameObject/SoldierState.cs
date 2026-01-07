using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    struct SoldierState
    {
        public bool walking, walkingOrderComplete, rotating, attacking, idle;

        //public void write(System.IO.BinaryWriter w)
        //{
        //    EightBit bools = new EightBit();
        //    bools.Set(0, walking);
        //    bools.Set(1, walkingOrderComplete);
        //    bools.Set(2, rotating);
        //    bools.Set(3, attacking);
        //    bools.Set(4, idle);

        //    bools.write(w);
        //}
        //public void read(System.IO.BinaryReader r)
        //{
        //    EightBit bools = EightBit.FromStream(r);

        //    walking = bools.Get(0);
        //    walkingOrderComplete = bools.Get(1);
        //    rotating = bools.Get(2);
        //    attacking = bools.Get(3);
        //    idle = bools.Get(4);
        //}
    }
}

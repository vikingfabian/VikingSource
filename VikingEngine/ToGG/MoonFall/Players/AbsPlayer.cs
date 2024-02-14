using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.MoonFall.Players
{
    abstract class AbsPlayer
    {
        public House house;        

        public AbsPlayer(House house)
        {
            this.house = house;
            house.player = this;
        }
    }

    class AiPlayer : AbsPlayer
    {
        public AiPlayer(House house)
            :base(house)
        { }
    }
}

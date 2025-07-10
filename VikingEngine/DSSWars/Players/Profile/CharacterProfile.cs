using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players.Profile
{
    struct CharacterProfile
    {
        public int accessory1;
        public int accessory2;
        public int accessory3;

        public int face;
        public CharacterHatGenre hatGenre;
        public int hat;
        public CharacterProfile()
        { 
            accessory1 = -1;
        }
    }

    enum CharacterHatGenre
    { 
        FollowWeapon,
        FollowArmor,
        Uniform,
    }
}

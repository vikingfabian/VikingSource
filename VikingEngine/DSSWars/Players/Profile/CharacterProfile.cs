using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.CharacterCreator;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players.Profile
{
    struct CharacterProfile
    {
        public int StorageIndex = -1;

        public int accessory1;
        public int accessory2;
        public int accessory3;

        public int face;
        public CharacterHatGenre hatGenre;
        public int hat;

        public CharacterProfile(int index)
        {
            StorageIndex = index;
            accessory1 = -1;
            accessory2 = -1;
            accessory3 = -1;
        }

        public CharacterProfile(System.IO.BinaryReader r)
        {
            read(r);
        }

        public string DisplayName()
        { 
            return string.Format(DssRef.todoLang.Lobby_CharacterCreationNumbered, TextLib.IndexToString(StorageIndex));
        }

        public List<AbsRichBoxMember> RbButton(int flagIndex, bool rotating)
        {
            List<AbsRichBoxMember> result = new List<AbsRichBoxMember>(2);
            //result.Add(new RbTexture(flag.flagDesign.CreateTexture(flag)));
            result.Add(new CharacterRichBoxIcon(StorageIndex, flagIndex, rotating));
            result.Add(new RbSpace());
            result.Add(new RbText(string.Format(DssRef.todoLang.Lobby_CharacterCreationNumbered, StorageIndex + 1)));
            return result;
        }

        const int Version = 1;
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write(accessory1);
            w.Write(accessory2);
            w.Write(accessory3);

            w.Write(face);
            w.Write((int)hatGenre); // Enum stored as int
            w.Write(hat);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version > Version) { return; }

            accessory1 = r.ReadInt32();
            accessory2 = r.ReadInt32();
            accessory3 = r.ReadInt32();

            face = r.ReadInt32();
            hatGenre = (CharacterHatGenre)r.ReadInt32(); // Cast back from int to enum
            hat = r.ReadInt32();
        }
    }

    enum CharacterHatGenre
    { 
        FollowWeapon,
        FollowArmor,
        Uniform,
    }
}

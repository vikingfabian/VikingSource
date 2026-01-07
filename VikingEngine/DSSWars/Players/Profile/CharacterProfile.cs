using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameState.CharacterCreator;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players.Profile
{
    struct CharacterProfile
    {
        public int StorageIndex = -1;

        public CharacterHatGenre hatGenre = CharacterHatGenre.FollowWeapon;
        public int customHat;
        public FaceTheme face;
        public int body;
        public ArmsTheme arms;

        public int accessoryBack = -1;
        public int accessoryFace = -1;

        public float soldierScale = 1;

        public CharacterProfile(int index)
        {
            StorageIndex = index;
        }

        public CharacterProfile(int index, System.IO.BinaryReader r)
        {
            StorageIndex = index;
            read(r);
        }

        public string DisplayName()
        { 
            return string.Format(DssRef.lang.Lobby_CharacterCreationNumbered, TextLib.IndexToString(StorageIndex));
        }

        public DropDownOption RbButton(int flagIndex, bool rotating)
        {
            DropDownOption result = new DropDownOption();
            //result.Add(new RbTexture(flag.flagDesign.CreateTexture(flag)));
            result.Add(new CharacterRichBoxIcon(StorageIndex, flagIndex, rotating));
            result.Add(new RbSpace());
            result.Add(new RbText(string.Format(DssRef.lang.Lobby_CharacterCreationNumbered, StorageIndex + 1)));
            return result;
        }

        const int Version = 3;
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write(soldierScale);

            w.Write((int)hatGenre);
            w.Write(customHat);
            w.Write((int)face);
            
            w.Write(body);
            w.Write((int)arms);

            w.Write(accessoryBack);
            w.Write(accessoryFace);

        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version < 3 || version > Version) { return; }

            soldierScale = r.ReadSingle();

            hatGenre = (CharacterHatGenre)r.ReadInt32(); // Cast back from int to enum
            customHat = r.ReadInt32();
            face = (FaceTheme)r.ReadInt32();

            body = r.ReadInt32();
            arms = (ArmsTheme)r.ReadInt32();

            accessoryBack = r.ReadInt32();
            accessoryFace = r.ReadInt32();
        }
    }

    
}

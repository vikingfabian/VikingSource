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

        public int accessoryBack;
        public int accessoryFace;
        public int accessory3;

        public int face;
        public int body;
        public ArmsTheme arms;
        public CharacterHatGenre hatGenre;
        
        public int hat;
       

        public CharacterProfile(int index)
        {
            StorageIndex = index;
            accessoryBack = -1;
            accessoryFace = -1;
            accessory3 = -1;
        }

        public CharacterProfile(int index, System.IO.BinaryReader r)
        {
            StorageIndex = index;
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

        const int Version = 2;
        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            w.Write(accessoryBack);
            w.Write(accessoryFace);
            w.Write(accessory3);

            w.Write(face);
            w.Write((int)hatGenre); // Enum stored as int
            w.Write(hat);
            w.Write(body);
            w.Write((int)arms);
        }

        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version < 2 || version > Version) { return; }

            accessoryBack = r.ReadInt32();
            accessoryFace = r.ReadInt32();
            accessory3 = r.ReadInt32();

            face = r.ReadInt32();
            hatGenre = (CharacterHatGenre)r.ReadInt32(); // Cast back from int to enum
            hat = r.ReadInt32();

            body = r.ReadInt32();
            arms = (ArmsTheme)r.ReadInt32();
        }
    }

    
}

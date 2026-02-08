using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Players.Profile
{
    struct PlayerProfile
    {
        public const int MaxNamedCityCount = 4;

        //public int characterIndex;
        public string name;
        public CharacterProfile character = new CharacterProfile(-1);
        public FlagAndColor flag;

        public string city1;
        public string city2;
        public string city3;
        public string city4;

        public int StorageIndex = -1;
        public bool casualControls = false;

        public PlayerProfile(int index)
        { 
            StorageIndex = index;
        }

        public PlayerProfile(int characterIx, int flagIx)
        {
            character = DssRef.storage.characterStorage.profiles[characterIx];
            flag = DssRef.storage.flagStorage.flagDesigns[flagIx];
        }

        public PlayerProfile(FactionType factiontype, WorldMetaData worldMeta)
        {
            flag = new FlagAndColor(factiontype, -1, worldMeta);
        }
        public PlayerProfile(int index, System.IO.BinaryReader r)
        {
            StorageIndex = index;
            read(r);
        }

        public string DisplayName()
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Format(DssRef.lang.Lobby_PlayerProfileNumbered, TextLib.IndexToString(StorageIndex));
            }
            else
            {
                return LoadContent.CheckCharsSafety(name, LoadedFont.Regular);
            }
        }

        public void refreshProfiles(int index)
        {
            if (flag == null)
            {
                int flagIx = index;
                if (flagIx >= DssRef.storage.flagStorage.flagDesigns.Count)
                {
                    flagIx = 0;
                }

                flag = DssRef.storage.flagStorage.flagDesigns[flagIx];
            }
            else
            { 
                flag = DssRef.storage.flagStorage.flagDesigns[flag.StorageIndex];
            }

            if (character.StorageIndex < 0)
            {
                character.StorageIndex = index; 
            }

            if (character.StorageIndex >= DssRef.storage.characterStorage.profiles.Count)
            {
                character.StorageIndex = 0;
            }

            character = DssRef.storage.characterStorage.profiles[character.StorageIndex];
        }

        public DropDownOption RbButton()
        {
            DropDownOption result = new DropDownOption();
            result.Add(new RbTexture(flag.flagDesign.CreateTexture(flag)));
            result.Add(new RbSpace());
            result.Add(new RbText(DisplayName()));
            return result;
        }

        public void AddCity(string cityName)
        {
            if (string.IsNullOrEmpty(city1))
                city1 = cityName;
            else if (string.IsNullOrEmpty(city2))
                city2 = cityName;
            else if (string.IsNullOrEmpty(city3))
                city3 = cityName;
            else if (string.IsNullOrEmpty(city4))
                city4 = cityName;
            else
                throw new InvalidOperationException("Maximum of 4 cities already assigned.");
        }

        public void RenameCity(int index, string newName)
        {
            switch (index)
            {
                case 0: city1 = newName; break;
                case 1: city2 = newName; break;
                case 2: city3 = newName; break;
                case 3: city4 = newName; break;
                default: throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 3.");
            }
        }

        public void RemoveCity(int index)
        {
            switch (index)
            {
                case 0: city1 = null; break;
                case 1: city2 = null; break;
                case 2: city3 = null; break;
                case 3: city4 = null; break;
                default: throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 3.");
            }
        }

        const int Version = 3;


        public void writeBot(System.IO.BinaryWriter w)
        {
            flag.write(w);
        }

        public void readBot(System.IO.BinaryReader r)
        {
            flag.read(r);
        }

        public void write(System.IO.BinaryWriter w)
        {
            write(w, false);
        }
        public void write(System.IO.BinaryWriter w, bool net)
        {
            w.Write(Version);

            bool customFlag = flag.StorageIndex >= 0;
            bool customCharacter = character.StorageIndex >= 0;
           
            EightBit bools = new EightBit(
                customCharacter,
                customFlag,
                TextLib.HasValue(city1),
                TextLib.HasValue(city2),
                TextLib.HasValue(city3),
                TextLib.HasValue(city4),
                TextLib.HasValue(name),
                casualControls);

            bools.write(w);


            if (customCharacter)
            {
                w.Write((ushort)character.StorageIndex);
            }
            else
            {
                character.write(w);
            }            

            if (customFlag)
            {
                w.Write((ushort)flag.StorageIndex);
            }
            else
            { 
                flag.write(w);
            }

            if (TextLib.HasValue(city1)) StreamLib.WriteString(w, city1);
            if (TextLib.HasValue(city2)) StreamLib.WriteString(w, city2);
            if (TextLib.HasValue(city3)) StreamLib.WriteString(w, city3);
            if (TextLib.HasValue(city4)) StreamLib.WriteString(w, city4);
            if (TextLib.HasValue(name)) StreamLib.WriteString(w, name);

        }


        public void read(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            if (version < 2 || version > Version) { return; }

            EightBit bools = new EightBit(r);
            bool customCharacter = bools.Get(0);
            bool customFlag = bools.Get(1);

            bool hasCity1 = bools.Get(2);
            bool hasCity2 = bools.Get(3);
            bool hasCity3 = bools.Get(4);
            bool hasCity4 = bools.Get(5);
            bool hasName = bools.Get(6);
            casualControls = bools.Get(7);

            if (customCharacter)
            {
                int index = r.ReadUInt16();
                character = DssRef.storage.characterStorage.profiles[index];
            }
            else
            {
                character.read(r);
            }

            if (customFlag)
            {
                int index = r.ReadUInt16();
                flag = DssRef.storage.flagStorage.flagDesigns[index];
            }
            else
            { 
                flag.read(r);
            }

            city1 = hasCity1 ? StreamLib.ReadString(r) : null;
            city2 = hasCity2 ? StreamLib.ReadString(r) : null;
            city3 = hasCity3 ? StreamLib.ReadString(r) : null;
            city4 = hasCity4 ? StreamLib.ReadString(r) : null;
            name = hasName ? StreamLib.ReadString(r) : null;
            
            
        }

        public int NamedCityCount =>
            (string.IsNullOrEmpty(city1) ? 0 : 1) +
            (string.IsNullOrEmpty(city2) ? 0 : 1) +
            (string.IsNullOrEmpty(city3) ? 0 : 1) +
            (string.IsNullOrEmpty(city4) ? 0 : 1);

       

    }
}

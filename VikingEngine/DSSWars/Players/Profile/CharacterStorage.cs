using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine.DSSWars.Players.Profile
{
    class CharacterStorage
    {
        //const int ReservedProfilesCount = 1;
        public List<CharacterProfile> profiles;
        public int selectedIx = 0;

        public CharacterStorage()
        {
            const int StartCount = 8;
            profiles = new List<CharacterProfile>(StartCount);

            for (int i = 0; i < StartCount; ++i)
            {
                profiles.Add(new CharacterProfile(i));
            }
        }

        public CharacterProfile Selected()
        {
            return profiles[selectedIx];
        }
        public void SetSelected(CharacterProfile profile)
        {
            profiles[selectedIx] = profile;
        }

        public void Load()
        {
            var filePath = path(-1);

            string[] files = FileToDiskManager.SearchFilesInStorageDir(filePath, true);

            foreach (string file in files)
            {
                var num = file.Split('_')[1];
                int index = Convert.ToInt32(num) - 1;

                filePath.FileName = file;

                while (index >= profiles.Count)
                {
                    profiles.Add(new CharacterProfile());
                }

                FileToDiskManager.TryReadBinaryIO(filePath, new CharacterReader(index).read);
            }
        }

        public void SaveSelected()
        {
            Save(selectedIx);
        }
        public void Save(int index)
        {
            var filePath = path(index);
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            BeginReadWrite.BinaryIO(true, filePath, profiles[index].write, null, null, true);
        }

        FilePath path(int index)
        {
            string num = index >= 0 ? (index + 1).ToString() : string.Empty;
            return new FilePath(Ref.steam.UserCloudPath + System.IO.Path.AltDirectorySeparatorChar +
               "CharacterSave", "character_" + num, ".chs");
        }
    }

    class CharacterReader
    {
        int index;

        public CharacterReader(int index)
        { this.index = index; }

        public void read(System.IO.BinaryReader r)
        {
           DssRef.storage.characterStorage.profiles[index] = new CharacterProfile(r);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine.DSSWars.Players.Profile
{
    class ProfileStorage
    {
        public List<PlayerProfile> profiles;
        public int selected = 0;

        public ProfileStorage()
        {
            const int StartCount = 8;
            profiles = new List<PlayerProfile>(StartCount);

            for (int i = 0; i < StartCount; ++i)
            {
                profiles.Add(new PlayerProfile(i));
            }
        }

        public PlayerProfile Selected()
        { 
            return profiles[selected];
        }

        public void SetSelected(PlayerProfile profile)
        {
            profiles[selected] = profile;
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
                    profiles.Add(new PlayerProfile(profiles.Count));
                }

                FileToDiskManager.TryReadBinaryIO(filePath, new ProfileReader(index).read);
            }
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
               "ProfileSave", "profile_" + num, ".prs");
        }

        public void refreshProfiles()
        {
            for (int i = 0; i < profiles.Count; i++)
            {
                var profile = profiles[i];
                { 
                    profile.refreshProfiles(i);
                }
                profiles[i] = profile;
            }
        }
    }

    class ProfileReader
    {
        int index;

        public ProfileReader(int index)
        { this.index = index; }

        public void read(System.IO.BinaryReader r)
        {
            DssRef.storage.profileStorage.profiles[index] = new PlayerProfile(r);
        }
    }

}

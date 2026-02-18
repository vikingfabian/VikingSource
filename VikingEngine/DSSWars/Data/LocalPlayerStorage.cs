using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Data
{
    class LocalPlayerStorage
    {
        public InputSource prevInputSource;
        public InputSource inputSource;
        public int controllerIndex = 0;
        public int screenIndex = 0;
        public int profileIndex;
        public int index;
        public LocalPlayerStorage(int index)
        {
            this.index = index;
            inputSource = index == 0 ? InputSource.DefaultPC : InputSource.Empty;
            
            screenIndex = index;
            profileIndex = index;
        }

        public bool SimulateMouseProperty(object tag, bool set, bool value)
        {            
            if (set)
            {
                inputSource.useTouchAsMouseSim = value;
                DssRef.storage.Save(null);
            }

            return inputSource.useTouchAsMouseSim;
        }

        public PlayerProfile Profile()
        {
            var profile = DssRef.storage.profileStorage.profiles[profileIndex];
            return profile;
        }

        public FlagAndColor Flag()
        {
            var profile = DssRef.storage.profileStorage.profiles[profileIndex];
            return profile.flag;
        }
        public void checkDoublette(int myIndex, LocalPlayerStorage[] localPlayers)
        {
            if (checkDoublette_input(myIndex, localPlayers))
            {
                inputSource = InputSource.Empty;
            }

            if (checkDoublette_profile(myIndex, localPlayers))
            {
                profileIndex = 0;
                while (checkDoublette_profile(myIndex, localPlayers))
                {
                    profileIndex++;
                }
            }

            if (checkDoublette_screen(myIndex, localPlayers))
            {
                screenIndex = 0;
                while (checkDoublette_profile(myIndex, localPlayers))
                {
                    screenIndex++;
                }
            }
        }
        public bool checkDoublette_input(int myIndex, LocalPlayerStorage[] localPlayers)
        {
            if (inputSource.sourceType != InputSourceType.Num_None)
            {
                for (int i = 0; i < localPlayers.Length; ++i)
                {
                    if (i != myIndex)
                    {
                        if (localPlayers[i].inputSource.Equals(inputSource))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public bool checkDoublette_screen(int myIndex, LocalPlayerStorage[] localPlayers)
        {
            for (int i = 0; i < localPlayers.Length; ++i)
            {
                if (i != myIndex)
                {
                    if (localPlayers[i].screenIndex == screenIndex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public bool checkDoublette_profile(int myIndex, LocalPlayerStorage[] localPlayers)
        {
            for (int i = 0; i < localPlayers.Length; ++i)
            {
                if (i != myIndex)
                {
                    if (localPlayers[i].profileIndex == profileIndex)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(screenIndex);
            w.Write(profileIndex);
            inputSource.write(w);
        }
        public void read(System.IO.BinaryReader r, int version)
        {
            screenIndex = r.ReadInt32();
            profileIndex = Bound.Max(r.ReadInt32(), DssRef.storage.profileStorage.profiles.Count - 1);
            if (version >= 35)
            {
                prevInputSource.read(r);
            }
        }
    }
}

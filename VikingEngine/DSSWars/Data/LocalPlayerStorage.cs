using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Data
{
    class LocalPlayerStorage
    {
        public InputSource inputSource;
        public int controllerIndex = 0;
        public int screenIndex = 0;
        public int profile;
        public int index;
        public LocalPlayerStorage(int index)
        {
            this.index = index;
            inputSource = index == 0 ? InputSource.DefaultPC : InputSource.Empty;
            //controllerIndex = index - 1;
            screenIndex = index;
            profile = index;
        }

        public void checkDoublette(int myIndex, LocalPlayerStorage[] localPlayers)
        {
            if (checkDoublette_input(myIndex, localPlayers))
            {
                inputSource = InputSource.Empty;
            }

            if (checkDoublette_profile(myIndex, localPlayers))
            {
                profile = 0;
                while (checkDoublette_profile(myIndex, localPlayers))
                {
                    profile++;
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
            if (inputSource.sourceType != InputSourceType.Num_Non)
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
                    if (localPlayers[i].profile == profile)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void write(System.IO.BinaryWriter w)
        {
            //w.Write((int)inputSource);
            //w.Write(controllerIndex);

            w.Write(screenIndex);
            w.Write(profile);
        }
        public void read(System.IO.BinaryReader r, int version)
        {
            //inputSource = (InputSourceType)r.ReadInt32();
            //controllerIndex = r.ReadInt32();

            if (version <= 4)
            {
                new InputSource().read(r);
            }
            screenIndex = r.ReadInt32();
            profile = r.ReadInt32();
        }
    }
}

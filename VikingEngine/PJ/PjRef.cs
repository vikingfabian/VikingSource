using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    static class PjRef
    {
        public static bool XboxLayout;
        public static Input.InputSource HostingPlayerSource;
        public static bool HasSetHost = false;

        public static bool Dlc1Characters = false;
        public static bool Dlc2BETA = false;
        public static bool DlcZombie = false;

        public static void Init()
        {
            XboxLayout = PlatformSettings.TargetPlatform == ReleasePlatform.Xbox;
            if (PlatformSettings.DevBuild)
            {
               //XboxLayout = true;
            }

            if (XboxLayout)
            {
                HostingPlayerSource = new Input.InputSource(Input.InputSourceType.XController, 0);
            }
            else
            {
                HostingPlayerSource = new Input.InputSource(Input.InputSourceType.KeyboardMouse);
            }
        }

        public static Sound.SongData JoustSong;
        public static Sound.SongData LobbySong;
        public static Storage storage;
        public static PjEngine.Achievements achievements;
        public static List<Vector2> StartPositions;
#if PCGAME
        public static PjEngine.GameStats stats;
#endif

        public static bool host = true;
        public static bool PublicNetwork = false;

        public static void RefreshDlcStatus()
        {
#if PCGAME
            if (Ref.steam.isInitialized)
            {
                var dlc = Ref.steam.DLC;
                if (dlc != null)
                {
                    Dlc1Characters = dlc.JoustingCharacterPack;
                    Dlc2BETA = dlc.JoustingBlingPack;
                    DlcZombie = dlc.JoustingZombiePack;

                    if (PlatformSettings.DevBuild)
                    {
                        Debug.Log("===DLC UNLOCKED (" + dlc.DlcCount_FromApi.ToString() + "): " + Dlc1Characters.ToString() + ", " + Dlc2BETA.ToString() + " ===");
                    }
                }
            }
            //else
            //{
            //    Dlc1Characters = true;
            //    Dlc2BETA = true;
            //}
#endif
        }

        public static bool hasAllContentDLC
        {
            get { return Dlc1Characters || Dlc2BETA; }
        }

        public static bool HostingSession
        {
            get { return Ref.netSession.InMultiplayerSession && host; }
        }

        public static bool ViewExtraControllerInput()
        {
            return XboxLayout || Input.XInput.HasConnectedController();
        }
    }
}

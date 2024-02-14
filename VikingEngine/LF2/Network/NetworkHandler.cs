using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

//

namespace VikingEngine.LF2
{
    static class NetworkHandler
    {
        public const int JoinLocalSessionDialogue = 11111111;
        public const int JoinLiveSessionDialogue = 12121212;
        public const int ListMoreSessionsLink = 13131318;
        const short IndexBlockLenght = short.MaxValue / 32;

        /// <returns>Amount of available sessions</returns>
        public static int ListSessions(HUD.File file, bool limited)
        {
#if !PCGAME
            List<AvailableNetworkSession> sessions = Ref.netSession.SortAvailableSessions();
            if (sessions == null)
                return 0;
            int max = sessions.Count;
            if (limited && max > 4)
            {
                max = 2;
            }
            string pre = limited ? "Join " : TextLib.EmptyString;

            List<string> title = new List<string>{ "Join game" , null };

            for (int i = 0; i < max; i++)
            {
                title[1] = sessions[i].HostGamertag;
                Percent progress = Percent.Zero; 

                int? val = sessions[i].SessionProperties[(int)NetworkProperties.Progress];
                if (val != null)
                    progress.ByteVal = (byte)val;
                MainMenuState.LiveButton(file, new HUD.Link(JoinLocalSessionDialogue, i), title,
                    new List<string>
                    {
                        Data.WorldSummary.ProgressText(progress),
                        "Gamer count: " + sessions[i].CurrentGamerCount.ToString(),
                    });
                //file.AddTextLink(pre + sessions[i].Text, new HUD.Link(JoinLocalSessionDialogue, sessions[i].Index));//sessions[i].Index, JoinLocalSessionDialogue);
            }

            if (limited && ( max < sessions.Count || PlatformSettings.DebugOptions))
            {
                file.AddTextLink("More players(" + sessions.Count.ToString() + ")", ListMoreSessionsLink);
            }
            return max;
#else
            return 0;
#endif
        }
       
    }
}

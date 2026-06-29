using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest.Display;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Net
{
    struct ChatLogMessage
    {
        public ulong sender;
        public string senderName;
        public string message;

        public ChatLogMessage(AbsNetworkPeer peer, string message)
        { 
            sender = peer.fullId;
            senderName = LoadContent.CheckCharsSafety(peer.Gamertag, LoadedFont.Regular);
            this.message = LoadContent.CheckCharsSafety(message, LoadedFont.Regular);
        }

    }
    class ChatLog : List<ChatLogMessage>
    {
        public void AddMessage(ChatLogMessage message)
        {
            if (Count > 50)
            {
                arraylib.RemoveFirst_Unsafe(this, 5);
            }
            Add(message);
        }

        public void toolTip(RichBoxContent content, object tag)
        {
            DssRef.state.LocalHost().gameControls.input.TextChat.ToRichContent(content);
            content.space();
            content.Add(new RbText(".Text chat", HudLib.TitleColor_Action));

            content.newParagraph();
            content.h2(".Chat log", HudLib.TitleColor_Head);

            if (Count == 0)
            {

            }
            else
            {
                for (int i = Count - 1; i >= 0; --i)
                {
                    content.Add(new RbSeperationLine());

                    var message = this[i];
                    HudLib.LabelAndText(content, SpriteName.NO_IMAGE, message.senderName, message.message);
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState.VoxelEditor;
using VikingEngine.DSSWars.Net;
using VikingEngine.DSSWars.Players;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars.Interface.CutScene
{
    abstract class AbsTextInputScene : AbsCutScene
    {
        RichMenu menu;
        Graphics.Image bg;
        string title;
        protected void init(string title, object tag)
        {
            this.title = title;
            VectorRect area = VectorRect.FromCenterSize(Engine.Screen.CenterScreen,
                new Microsoft.Xna.Framework.Vector2(1, 0.5f) * HudLib.cutsceneGui.width);

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.CutSceneBgLayer, new PlayerData(PlayerData.AllPlayers));
            bg = menu.addBackground_Flat(Color.Black, 0.8f);

            var reciever = new ChatInput(this, tag);
            SteamInputManager.tryOpenSteamKeyboard(reciever);
        
        }
        public override void Time_Update(float time)
        {
            RichBoxContent content = new RichBoxContent();
            content.h1(title, HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbButton(new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.cmdSpyglass),
                    new RbSpace(),
                    new RbText(Ref.update.textInput.DisplayText(), Color.Black),
                },
                null)
            { overrideBgColor = Color.White, fillWidth = true });

            menu.Refresh(content);
        }

        virtual public void textInput_complete(string result, object tag)
        {
            Close();
        }
        public override void Close()
        {
            base.Close();
            menu.DeleteMe();
            bg.DeleteMe();
        }
    }

    ///// <param name="result">input, null is canceled</param>
    //public delegate void TextInputEvent(string result, object tag);
    class TextInputScene : AbsTextInputScene
    {
        TextInputEvent callback;
        public TextInputScene(string title, TextInputEvent callback, object tag = null)
        {
            this.callback = callback;
            init(title, tag);
        }

        public override void textInput_complete(string result, object tag)
        {
            callback.Invoke(result, tag);
            base.textInput_complete(result, tag);
        }
    }

    class TextChat : AbsTextInputScene
    {
       
        public TextChat()
        {
            init(".Text chat - everyone", null);
        }

        override public void textInput_complete(string result, object tag)
        {
            if (!string.IsNullOrEmpty(result))
            {
                var w = Ref.netSession.BeginWritingPacket(PacketType.TextChat, PacketReliability.Reliable);
                StreamLib.WriteString(w, result);

                RichBoxContent content = new RichBoxContent();
                DssRef.state.LocalHost().addNetGamerToHud(content, true, false);
                content.icontext(SpriteName.TextChatLetter, result);
                DssRef.state.LocalHost().hud.messages.Add(content, null);

                var message = new ChatLogMessage(Ref.steam.P2PManager.localPeer, result);
                ((PlayState)DssRef.state).chatLog.Add(message);
            }
            base.textInput_complete(result, tag);
            Close();
        }


        public override PlayerNetState NetState()
        {
            return PlayerNetState.TypingChat;
        }
    }

    class ChatInput : AbsTextInputUpdate
    {
        AbsTextInputScene textChat;
        public ChatInput(AbsTextInputScene textChat, object tag)
            : base()
        {

            this.textChat = textChat;
            init(string.Empty, null, tag);
            InitComplete();
        }

        public override void textInput_refresh(bool textLengthChanged)
        {

        }

        public override void textInput_complete(string result, object tag)
        {
            base.textInput_complete(result, tag);
            textChat.textInput_complete(result, tag);
        }
    }
}


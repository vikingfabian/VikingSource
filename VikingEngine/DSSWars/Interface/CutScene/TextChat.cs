//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Reflection.Metadata;
//using System.Text;
//using System.Threading.Tasks;
//using VikingEngine.DSSWars.Data;
//using VikingEngine.DSSWars.GameState.VoxelEditor;
//using VikingEngine.DSSWars.Players;
//using VikingEngine.Engine;
//using VikingEngine.HUD.RichBox;
//using VikingEngine.HUD.RichBox.Artistic;
//using VikingEngine.HUD.RichMenu;
//using VikingEngine.Input;
//using VikingEngine.Network;
//using VikingEngine.SteamWrapping;
//using static System.Net.Mime.MediaTypeNames;

//namespace VikingEngine.DSSWars.Interface.CutScene
//{
//    abstract class AbsTextInputScene
//    { }

//    class TextChat : AbsCutScene
//    {
//        RichMenu menu;
//        Graphics.Image bg;
//       public TextChat()
//        {
//            VectorRect area = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, 
//                new Microsoft.Xna.Framework.Vector2(1, 0.5f) * HudLib.cutsceneGui.width);

//            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.CutSceneBgLayer, new PlayerData(PlayerData.AllPlayers));
//            bg = menu.addBackground_Flat(Color.Black, 0.8f);

//            var reciever = new ChatInput(this);
//            SteamInputManager.tryOpenSteamKeyboard(reciever);
//        }

//        public override void Time_Update(float time)
//        {
//            RichBoxContent content = new RichBoxContent();
//            content.h1(".Text chat - everyone", HudLib.TitleColor_Head);
//            content.newLine();
//            content.Add(new RbButton(new List<AbsRichBoxMember> {
//                    new RbImage(SpriteName.cmdSpyglass),
//                    new RbSpace(),
//                    new RbText(Ref.update.textInput.DisplayText(), Color.Black),
//                },
//                null)
//            { overrideBgColor = Color.White, fillWidth = true });

//            menu.Refresh(content);
//        }

//        public void textInput_complete(string result, object tag)
//        {
//            if (!string.IsNullOrEmpty(result))
//            {
//                var w = Ref.netSession.BeginWritingPacket(PacketType.TextChat, PacketReliability.Reliable);
//                StreamLib.WriteString(w, result);

//                RichBoxContent content = new RichBoxContent();
//                DssRef.state.LocalHost().addNetGamerToHud(content, false);
//                content.icontext(SpriteName.LfChatBobbleIcon, result);
//                DssRef.state.LocalHost().hud.messages.Add(content, null);
//            }
//            Close();
//        }

//        public override void Close()
//        {
//            base.Close();
//            menu.DeleteMe();
//            bg.DeleteMe();
//        }

//        public override PlayerNetState NetState()
//        {
//            return PlayerNetState.TypingChat;
//        }
//    }

//    class ChatInput : AbsTextInputUpdate
//    {
//        TextChat textChat;
//        public ChatInput(TextChat textChat)
//            : base()
//        {

//            this.textChat = textChat;
//            init(string.Empty, null, null);
//            InitComplete();
//        }

//        public override void textInput_refresh(bool textLengthChanged)
//        {
            
//        }

//        public override void textInput_complete(string result, object tag)
//        {
//            base.textInput_complete(result, tag);
//            textChat.textInput_complete(result, tag);
//        }
//    }
//}

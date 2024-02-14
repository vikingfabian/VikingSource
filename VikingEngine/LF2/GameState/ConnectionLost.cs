using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameState
{
    class ExitState : Engine.GameState
    {
        Time viewTime = new Time(200);

        public ExitState()
            : base()
        {
            draw.ClrColor = Color.Black;

            Ref.lobby.disconnect(null);
        }

        public void viewMessage(string text)
        {
            viewTime.MilliSeconds = 1000;
            Graphics.Text2 txtimg = new Graphics.Text2(text, LoadedFont.Bold,
                Engine.Screen.CenterScreen, Engine.Screen.SmallIconSize, Color.Yellow, ImageLayers.Lay0);
            txtimg.OrigoAtCenter();
        }

        public void LostConnectionMessage(string reason)
        {
            viewTime.Seconds = 20;

            float textH = Engine.Screen.TextTitleHeight;

            var lost = new Graphics.Text2("Disconnected", LoadedFont.Regular, Engine.Screen.CenterScreen,
                textH, Color.Yellow, ImageLayers.Lay1);
            lost.Ypos -= textH;
            lost.OrigoAtCenterWidth();

            var reasonTxt = new Graphics.Text2(reason, LoadedFont.Regular, Engine.Screen.CenterScreen,
                textH, Color.White, ImageLayers.Lay1);
            reasonTxt.CheckCharsSafety();
            reasonTxt.OrigoAtCenterWidth();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (viewTime.CountDown() ||
                (anyExitKey() && UpdateCount > 16))
            {
                new MainMenuState();
            }
        }

        bool anyExitKey()
        {
            int pIx = -1;

            return Input.XInput.KeyDownEvent(Buttons.Back, ref pIx) ||
                Input.XInput.KeyDownEvent(Buttons.Start, ref pIx) ||
                Input.XInput.KeyDownEvent(Buttons.A, ref pIx) ||
                Input.XInput.KeyDownEvent(Buttons.B, ref pIx) ||

                Input.Keyboard.KeyDownEvent(Keys.Enter) ||
                Input.Keyboard.KeyDownEvent(Keys.Escape) ||
                Input.Keyboard.KeyDownEvent(Keys.Space);

        }


    }
    //class ConnectionLost : MainMenuState
    //{
    //    protected string endReason;
    //    public ConnectionLost(string endReason)
    //        :base(false)
    //    {

    //        //this.host = host;
    //        this.endReason = endReason;

    //        connectionLostMenu(true);
    //    }

    //    void connectionLostMenu(bool loading)
    //    {
    //        HUD.File file = new HUD.File();
    //        file.AddTitle("Connection Lost");
    //        string reason;
    //        switch (endReason)
    //        {
    //            case NetworkSessionEndReason.ClientSignedOut:
    //                reason = "You signed out";
    //                break;
    //            case NetworkSessionEndReason.Disconnected:
    //                reason = "Network problems";
    //                break;
    //            case NetworkSessionEndReason.HostEndedSession:
    //                reason = "Host ended his game";
    //                break;
    //            default:
    //                reason = "You got kicked out";
    //                break;
    //        }
    //        file.AddDescription(reason);
    //        //file.AddTextLink("Try to reconnect", (int)MainMenuLink.ReConnect);
    //        if (host != null && !Ref.netSession.LostSessionIsSystemLink)
    //            file.AddTextLink("Host GamerTag", (int)MainMenuLink.SeeGamerTag);

    //        file.AddTextLink(loading? "Loading..." : "Back", (int)MainMenuLink.Back);
    //        menu.File = file;
    //        inMainMenu = false;
    //    }

    //    protected override void worldsLoaded()
    //    {
    //        //base.worldsLoaded();
    //        connectionLostMenu(false);
    //    }

    //    protected override void seeGamerTag(int pIx)
    //    {
    //        XGuide.ViewGamerCard(host, pIx);
    //    }
    //}
}

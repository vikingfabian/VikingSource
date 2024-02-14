using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    interface IMessage
    {
        ulong SenderID { get; }
        string Sender { get; }
        string Subject { get; }
        //HUD.File OpenMessage();
    }

    struct MapRequest : IMessage
    {
        const string Sub = "Map request";
        string sender;
        public MapRequest(string sender)
        {
            this.sender = sender;
        }
        public string Sender { get { return sender; } }
        public ulong SenderID { get { throw new NotImplementedException(); } }
        public string Subject 
        { 
            get { 
            
                return Sub;
            }
        }

        //public HUD.File OpenMessage()
        //{
        //    HUD.File file = new HUD.File();
        //    file.AddTitle(Sub);
        //    file.AddDescription(sender + " wants you to send your whole map");
        //    file.AddTextLink("Deny request", (int)Players.Link.RequestMapDeny);
        //    file.AddTextLink("Send map", (int)Players.Link.SendMapToClients);
        //    return file;
        //}
    }
    struct BuildRequest : IMessage
    {
        const string Sub = "Build request";
        string sender;
        ulong id;
        public BuildRequest(string sender, ulong id)
        {
            this.id = id;
            this.sender = sender;
        }
        public string Sender { get { return sender; } }
        public ulong SenderID { get { return id; } }
        public string Subject
        {
            get
            {
                return Sub;
            }
        }

        //public HUD.File OpenMessage()
        //{
        //    HUD.File file = new HUD.File();
        //    file.AddTitle(Sub);
        //    file.AddDescription(sender + " wants build permission");
        //    file.AddTextLink("Deny request", (int)Players.Link.MessageDeny);
        //    file.AddTextLink("Give permission", (int)Players.Link.RequestBuildPermissionAccept);
        //    return file;
        //}
    }
}

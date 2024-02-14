using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
#if PCGAME
using System.Net.Sockets;
#endif
namespace VikingEngine
{
    #if PCGAME
    class Client
    {
        //byte id;
        //public byte ID
        //{ get { return id; } }
        //Socket socket;
        //System.Net.Sockets.TcpListener listener;
        //public bool Connected = false;

        //public Client(System.Net.Sockets.TcpListener listener, byte id)
        //{
        //    this.id = id;
        //    this.listener = listener;
        //    Thread t = new Thread(recieveConnection);
        //}

        //void recieveConnection()
        //{
        //    socket = listener.AcceptSocket();
        //    Connected = true;
        //    //give id
        //    byte[] buffer = new byte[] { id };
        //    socket.Send(buffer);
        //}

        //public System.IO.BinaryReader ReadFromClient()
        //{
        //    if (socket.ReceiveBufferSize > 0)
        //    {
        //        byte[] buffer = new byte[socket.ReceiveBufferSize];
        //        socket.Receive(buffer);
        //        System.IO.BinaryReader r = new System.IO.BinaryReader();
        //        r.BaseStream.Write(buffer, 0, buffer.Length);
        //        return r;
        //    }
        //    return null;
        //}

        //public void WriteToClient(System.IO.BinaryWriter w)
        //{
        //    if (socket != null)
        //    {
        //        byte[] buffer = new byte[w.Length]; w.BaseStream.Read(buffer, 0, w.Length);
                
        //        socket.Send(buffer);
        //    }
        //}

        //public override string ToString()
        //{
        //    return "PC client";
        //}

    }
#endif
}

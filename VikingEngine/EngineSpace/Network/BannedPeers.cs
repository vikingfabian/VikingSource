using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD;

namespace VikingEngine.Network
{
    class BannedPeers
    {
        List<StoredPeer> banned = new List<StoredPeer>(8);
        List<StoredPeer> recent = new List<StoredPeer>(8);

        public void write(System.IO.BinaryWriter w)
        {
            writeList(banned);
            //writeList(recent);

            void writeList(List<StoredPeer> list)
            {
                w.Write(list.Count);
                foreach (var m in list)
                {
                    m.write(w);
                }
            }
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            readList(banned);
            //readList(recent);

            void readList(List<StoredPeer> list)
            {
                list.Clear();

                int peersCount = r.ReadInt32();
                for (int i = 0; i < peersCount; ++i)
                {
                    list.Add(new StoredPeer(r));
                }
            }
        }

        public void add(AbsNetworkPeer np)
        {
            var peer = new StoredPeer(np);

            if (!banned.Contains(peer))
            {
                banned.Add(peer);

                Ref.gamesett.Save();
            }
        }

        public bool isBanned(AbsNetworkPeer peer)
        {
            foreach (var m in banned)
            {
                if (m.Equals(peer))
                {
                    return true;
                }
            }

            return false;
        }

        public void remove(StoredPeer peer)
        {
            banned.Remove(peer);
        }

        public void toMenu(GuiLayout layout, Action onRemoveBan)
        {
            new GuiLabel("Click to remove block", layout);
            foreach (var m in banned)
            {
                new GuiTextButton(m.Gamertag, "Unblock: " + m.ToString(),
                       new GuiAction2Arg<StoredPeer, Action>(unblockPlayer, m, onRemoveBan), true, layout);
            }
        }
        
        void unblockPlayer(StoredPeer user, Action onRemoveBan)
        {
            remove(user);
            onRemoveBan?.Invoke();
        }

        public bool HasMembers => banned.Count > 0;
    }
}

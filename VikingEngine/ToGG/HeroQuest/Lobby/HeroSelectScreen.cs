using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.HeroQuest.Lobby
{
    class HeroSelectScreen
    {
        public HeroSelectScreenMember localHost;
        HeroSelectScreenMember[] members;

        public int count = 0;

        public HeroSelectScreen(VectorRect area, int count)
        {
            float w = area.Width / 6;
            float spacing = w * 0.05f;
            w *= 0.96f;

            Vector2 sz = new Vector2(1f, 2f) * w;

            members = new HeroSelectScreenMember[count];
            for (int i = 0; i < count; ++i)
            {
                members[i] = new HeroSelectScreenMember(Table.CellPlacement(
                    area.Center, true, i, count, sz, new Vector2(spacing)));
            }
        }

        void refreshCount()
        {
            count = 0;
            foreach (var m in members)
            {
                if (m.Occupied)
                {
                    ++count;
                }
            }
        }

        public bool allReady()
        {
            foreach (var m in members)
            {
                if (m.Occupied && !m.readyStatus.IsReady)
                {
                    return false;
                }
            }

            return true;
        }

        public HeroSelectScreenMember setNext(Network.AbsNetworkPeer peer)
        {
            int non;
            if (peer != null && !findPeer(peer, out non))
            {
                if (peer.IsRemote)
                {
                    clearOutLocalInstances();
                }

                foreach (var m in members)
                {
                    if (!m.Occupied)
                    {
                        m.set(peer);
                        refreshCount();
                        return m;
                    }
                }
            }

            return null;
        }

        void clearOutLocalInstances()
        {
            bool needRefresh = false;

            foreach (var m in members)
            {
                if (m.peer != null && m.peer.IsInstance)
                {
                    m.DeleteMe();
                    needRefresh = true;
                }
            }

            if (needRefresh)
            {
                refreshCount();
            }
        }

        public void remove(Network.AbsNetworkPeer peer)
        {
            foreach (var m in members)
            {
                if (m.peer != null && m.peer.Equals(peer))
                {
                    m.DeleteMe();
                }
            }

            refreshCount();
        }

        public void setLocal(Network.AbsNetworkPeer peer, bool netHost)
        {
            if (peer == null)
            {
                localHost = members[0];
                peer = new Network.OfflinePeer();
                localHost.set(peer);
            }
            else
            {
                int ix;
                findPeer(peer, out ix);
                localHost = members[ix];
            }

            localHost.setLocal();

            if (netHost)
            {
                addLocalInstance(1, HqUnitType.KnightHero);
                addLocalInstance(2, HqUnitType.ElfHero);
            }

            refreshCount();

            void addLocalInstance(int index, HqUnitType unit)
            {
                var ins = members[index];//setNext(new Network.LocalInstancePeer(peer, index));
                ins.set(new Network.LocalInstancePeer(peer, index));
                ins.setLocal();
                ins.setVisuals(new Data.PlayerVisualSetup(unit));
            }
        }

        public HeroSelectScreenMember GetOrCreate(Network.AbsNetworkPeer peer)
        {
            int ix;
            if (findPeer(peer, out ix))
            {
                return members[ix];
            }
            else
            {
                return setNext(peer);
            }
        }

        bool findPeer(Network.AbsNetworkPeer peer, out int index)
        {
            for (int i = 0; i < members.Length; ++i)
            {
                if (members[i].peer != null && members[i].peer.Equals(peer))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        public void setVisuals(Data.PlayerSetup setup)
        {
            int index = 0;

            foreach (var m in members)
            {
                if (m.Occupied && m.Local)
                {
                    m.setVisuals(setup.visualSetups[index++]);
                }
            }
        }

        public void update()
        {
            foreach (var m in members)
            {
                m.update();
            }
        }
    }
}

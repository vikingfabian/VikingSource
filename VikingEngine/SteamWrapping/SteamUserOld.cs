#if PCGAME
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.SteamWrapping
{
    struct SteamUserOld
    {
        public string name;
        public CSteamID id;

        public SteamUserOld(CSteamID id)
        {
            this.id = id;
            name = SteamFriends.GetFriendPersonaName(id);
        }

        public void write(System.IO.BinaryWriter w)
        {
            StreamLib.WriteString(w, name);
            w.Write(id.m_SteamID);
        }

        public void read(System.IO.BinaryReader r)
        {
            name = StreamLib.ReadString_safe(r);
            id = new CSteamID(r.ReadUInt64());
        }

        public override bool Equals(object obj)
        {
            SteamUserOld other = (SteamUserOld)obj;
            
            return this.id == other.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }

    class SteamUserList
    {
        public List<SteamUserOld> members = new List<SteamUserOld>();



        public void Add(SteamUserOld user, int maxLength = int.MaxValue)
        {
            for (int i = 0; i < members.Count; ++i)
            {
                if (members[i].id == user.id)
                {
                    members.RemoveAt(i);
                    break;
                }
            }

            members.Add(user);

            while (members.Count > maxLength)
            {
                members.RemoveAt(0);
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(members.Count);
            foreach (var m in members)
            {
                m.write(w);
            }
        }

        public void read(System.IO.BinaryReader r)
        {
            int membersCount = r.ReadInt32();

            members.Clear();
            for (int i = 0; i < membersCount; ++i)
            {
                var user = new SteamUserOld();
                user.read(r);
                members.Add(user);
            }
        }
    }
}
#endif
#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.SteamWrapping
{
    struct SteamUser
    {
        public string name;
        public ulong id;

        public SteamUser(ulong id)
        {
            this.id = id;
            name = Valve.Steamworks.SteamAPI.SteamFriends().GetFriendPersonaName(id);
        }

        public void write(System.IO.BinaryWriter w)
        {
            SaveLib.WriteString(w, name);
            w.Write(id);
        }

        public void read(System.IO.BinaryReader r)
        {
            name = SaveLib.ReadString(r);
            id = r.ReadUInt64();
        }

        public override bool Equals(object obj)
        {
            SteamUser other = (SteamUser)obj;
            
            return this.id == other.id;
        }

        public override int GetHashCode()
        {
            return (int)id;
        }
    }

    class SteamUserList
    {
        public List<SteamUser> members = new List<SteamUser>();



        public void Add(SteamUser user, int maxLength = int.MaxValue)
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
                var user = new SteamUser();
                user.read(r);
                members.Add(user);
            }
        }
    }
}
#endif
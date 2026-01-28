using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine.CardDesign.CardData
{
    interface IHasId
    {  
        Id Id { get; }
    }

    struct Id
    {
        public static readonly Id Empty = new Id() { empty = true };

        public bool empty;
        public int hash;
        public NameAndDate created;
        public NameAndDate updated;

        public Id()
        {
            empty = true;
        }

        public static Id CreateNew(bool gameContent) 
        {
            NameAndDate nameAndDate = NameAndDate.Now(gameContent);
            
            return new Id()
            {
                empty = false,
                hash = HashCode.Combine(nameAndDate.date, nameAndDate.userId, Ref.rnd.Uint()),
                created = nameAndDate,
                updated = nameAndDate,
            };
        }
        public override bool Equals(object obj)
        {
            if (empty)
                return false;

            Id id2 = (Id)obj;

            return id2.hash == this.hash;
        }
        public static bool operator ==(Id value1, Id value2)
        {
            if (value1.empty || value2.empty)
                return false;

            return value1.hash == value2.hash;
        }
        public static bool operator !=(Id value1, Id value2)
        {
            if (value1.empty || value2.empty)
                return true;

            return value1.hash != value2.hash;
        }

        public override int GetHashCode()
        {
            return hash;
        }
    }

    struct NameAndDate
    {
        const ulong GameContentUserId = 1;
        public ulong userId;
        public DateTime date;

        public static NameAndDate Now(bool gameContent)
        {

            NameAndDate result = new NameAndDate()
            {
                date = DateTime.Now,
                userId = gameContent ? GameContentUserId : Ref.steam.userId.m_SteamID
            };

            return result;
        }
    }
}

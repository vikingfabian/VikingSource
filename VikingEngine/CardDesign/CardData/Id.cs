using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    interface IHasId
    {  
        Id Id { get; }
    }

    struct Id
    {
        static int NextId = 1;

        public int value;

        public Id()
        { }

        public Id(int value)
            { this.value = value; }

        public static Id CreateNew() 
        {
            return new Id(NextId++);
        }

        //void checkNext()
        //{
        //    if (NextId <= value)
        //    {
        //        NextId = value + 1;
        //    }
        //}
    }
}

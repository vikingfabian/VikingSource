using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars.Interface
{
    enum PostItType
    { 
        Resource,
    }

    struct PostIt
    {
        public PostItType type;
        public int id;

        public PostIt(ItemResourceType itemResource)
        { 
            type = PostItType.Resource;
            id = (int)itemResource;
        }
    }

    class CityPostIt : List<PostItType>
    { 
        
    }

    class PostItManager : Dictionary<int, CityPostIt>
    {
        public PostItManager() :
            base(8)
        { }

        //public bool 
    }
}

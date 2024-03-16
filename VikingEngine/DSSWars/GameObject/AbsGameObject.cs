using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
//

namespace VikingEngine.DSSWars.GameObject
{


    abstract class AbsGameObject
    {

        public int parentArrayIndex = -1;

        abstract public GameObjectType gameobjectType();
        
        abstract public Faction GetFaction();

        virtual public City GetCity() { return null; }

        virtual public Army GetArmy() { return null; }
        
    }

}


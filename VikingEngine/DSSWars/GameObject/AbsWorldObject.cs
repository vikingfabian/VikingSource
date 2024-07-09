using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsWorldObject: AbsGameObject
    {
        public Vector3 position = Vector3.Zero;
        public bool debugTagged = false;
        public bool isDeleted = false;
        

        

       

        abstract public bool defeatedBy(Faction attacker);

        virtual public bool defeated()
        {
            return isDeleted;
        }

        
        virtual public void stateDebugText(HUD.RichBox.RichBoxContent content)
        { }

        virtual public void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
        }

        virtual public void AddDebugTag()
        {
            lib.Invert(ref debugTagged);
            Debug.Log((debugTagged ? "Tagged: " : "Remove tag: ") + this.ToString());
        }
    }


    enum DeleteReason
    {
        Death,
        Transform,
        EmptyGroup,
        Disband,
        Desert,
    }
}

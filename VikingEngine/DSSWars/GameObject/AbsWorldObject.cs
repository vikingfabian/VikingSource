using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsWorldObject: AbsGameObject
    {
        public Vector3 position = Vector3.Zero;
        public bool debugTagged = false;
        
        
        abstract public bool defeatedBy(PFaction attackerFaction);

        virtual public bool defeated()
        {
            return isDeleted;
        }

        abstract public bool aliveAndBelongTo(PFaction faction);

        public override AbsWorldObject GetWorldObject()
        {
            return this;
        }

        public override Vector3 WorldPos()
        {
            return position;
        }
        virtual public void stateDebugText(HUD.RichBox.RichBoxContent content)
        { }

        virtual public void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
        }

        protected void debugTagButton(RichBoxContent content)
        {
#if DEBUG
            content.Button(string.Format("debug tag ({0})", debugTagged), new HUD.RichBox.RbAction(AddDebugTag), null, true);
#endif
        }

        virtual public void AddDebugTag()
        {
            lib.Invert(ref debugTagged);
            Debug.Log((debugTagged ? "Tagged: " : "Remove tag: ") + this.ToString());
        }

        public Vector2 posXZ()
        { return new Vector2(position.X, position.Z); }

        virtual public bool rectangleCollision(ScreenToSpaceRectangleBound rectangle)
        { 
            throw new NotImplementedException();
        }
    }


    enum DeleteReason
    {
        Death,
        Transform,
        EmptyGroup,
        Disband,
        Desert,
        CameraCulling,

        NetworkEvent,
        LostHost,
    }

    enum ConvertReason
    { 
        Assigned,
        Diplomacy,
        Gift,
        Claim,
        WarCapture,
    }
}

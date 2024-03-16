using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Battle;
using VikingEngine.HUD.RichBox;
//


namespace VikingEngine.DSSWars.GameObject
{
    /// <summary>
    /// Large scale objects
    /// </summary>
    abstract partial class AbsMapObject : AbsGroup
    {
        public Faction faction;

        /// <summary>
        /// Pågående strider, om order ges läggs inte battle till förrän armeerna är intill varandra
        /// </summary>
        
        //SpottedArrayCounter<AbsMapObject> battlesCounter;
        public bool enterRender_asynch = false;
        public bool inRender = false;

        public int previousWarAgainstFaction = -1;
        public float strengthValue=-1;
        public IntVector2 tilePos;

        public AbsMapObject()
        {
            //battlesCounter = new SpottedArrayCounter<AbsMapObject>(battles);
        }
        
        virtual public bool rayCollision(Ray ray)
        {
            return false;
        }

        public void asynchCullingUpdate(float time)
        {
            DssRef.state.culling.InRender_Asynch(ref enterRender_asynch, tilePos);
        }
        

        public void PauseUpdate()
        {
            updateDetailLevel();
        }

        protected void updateDetailLevel()
        {
            if (enterRender_asynch != inRender)
            {
                inRender = enterRender_asynch;
                setInRenderState();
            }
        }

        abstract protected void setInRenderState();

        virtual public void ExitBattleGroup()
        {
            battleGroup = null;
        }

        public float distanceTo(AbsMapObject obj)
        {
            return VectorExt.Length(position.X - obj.position.X, position.Z - obj.position.Z);
        }

        public float distanceTo(IntVector2 tilePos)
        {
            return VectorExt.Length(position.X - tilePos.X, position.Z - tilePos.Y);
        }

        public Map.Tile Tile()
        {
            return DssRef.world.tileGrid.Get(tilePos);
        }

        public bool LocalMember
        {
            get { return faction.Owner.IsLocal; }
        }

        //abstract public Faction Faction();

        virtual public void setFaction(Faction faction)
        {
            this.faction = faction;
            
            OnNewOwner();
        }

        override public Faction GetFaction()
        {
            return faction;
        }

        abstract public void OnNewOwner();

        public override AbsMapObject RelatedMapObject()
        {
            return this;
        }

        //public override bool Alive()
        //{
        //    return !isDeleted;
        //}
    }

    enum GameObjectType
    {
        Faction,
        City,
        Army,
        SoldierGroup,
        Soldier,
       
        NUM_NON,
    }
}

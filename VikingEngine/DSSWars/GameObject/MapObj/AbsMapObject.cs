using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD.RichBox;
//


namespace VikingEngine.DSSWars.GameObject
{
    /// <summary>
    /// Large scale objects
    /// </summary>
    abstract class AbsMapObject : AbsGroup
    {
        public Faction faction;

        /// <summary>
        /// Pågående strider, om order ges läggs inte battle till förrän armeerna är intill varandra
        /// </summary>
        public SpottedArray<AbsMapObject> battles = new SpottedArray<AbsMapObject>(4);
        SpottedArrayCounter<AbsMapObject> battlesCounter;
        public bool enterRender_asynch = false;
        public bool inRender = false;

        public int previousWarAgainstFaction = -1;
        public float strengthValue=-1;
        public IntVector2 tilePos;

        public AbsMapObject()
        {
            battlesCounter = new SpottedArrayCounter<AbsMapObject>(battles);
        }
        
        virtual public bool rayCollision(Ray ray)
        {
            return false;
        }

        public void asynchCullingUpdate(float time)
        {
            DssRef.state.culling.InRender_Asynch(ref enterRender_asynch, tilePos);
        }
        public bool InBattle()
        {
            return battles.Count > 0;
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

        virtual public void EnterPeaceEvent()
        { }


        public bool collectBattles_asynchMarker = false;
        static List<AbsMapObject> battlesUnitsBuffer = new List<AbsMapObject>();

        protected void collectBattles_asynch()
        {
            //if (objectType() == ObjectType.City && GetCity().index == 346)
            //{ 
            //    lib.DoNothing();
            //}
            //bool collectCities = this.objectType() == ObjectType.Army;
            //Remove completed battles

            if (defeated())
            {
                battles.Clear();
            }
            else
            {

                if (battles.Count > 0)
                {
                    var battlesC = battles.counter();
                    while (battlesC.Next())
                    {
                        battlesC.sel.collectBattles_asynchMarker = false;
                    }
                }


                DssRef.world.unitCollAreaGrid.collectMapObjectBattles(faction, tilePos, ref battlesUnitsBuffer, gameobjectType() == GameObjectType.Army);

                foreach (var m in battlesUnitsBuffer)
                {
                    bool inBattle = VectorExt.PlaneXZLength(m.position - position) <= DssLib.BattleConflictRadius;
                    if (inBattle)
                    {
                        m.collectBattles_asynchMarker = true;

                        battles.AddIfNotExists(m);
                        m.battles.AddIfNotExists(this);
                    }
                }
            }

            if (battles.Count > 0)
            {
                var battlesC = battles.counter();
                while (battlesC.Next())
                {
                    if (battlesC.sel.collectBattles_asynchMarker == false)
                    {
                        battlesC.RemoveAtCurrent();
                        if (battles.Count == 0)
                        {
                            this.EnterPeaceEvent();
                        }
                    }
                }
            }
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

        override public Faction Faction()
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
        City,
        Army,
        SoldierGroup,
        Soldier,
        NUM_NON,
    }
}

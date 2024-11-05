using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Work;
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

        public bool enterRender_overviewLayer_async = false;
        public bool enterRender_detailLayer_async = false;
        public bool inRender_overviewLayer = false;
        public bool inRender_detailLayer = false;

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

        virtual public void asynchCullingUpdate(float time, bool bStateA)
        {
            DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, tilePos);
        }
        

        public void PauseUpdate()
        {
            updateDetailLevel();
        }

        protected void updateDetailLevel()
        {
            if (enterRender_overviewLayer_async != inRender_overviewLayer)
            {
                inRender_overviewLayer = enterRender_overviewLayer_async;
                setInRenderState();
            }
            else if (enterRender_detailLayer_async != inRender_detailLayer)
            {
                inRender_detailLayer = enterRender_detailLayer_async;
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

        virtual public void tagSprites(out SpriteName back, out SpriteName art)
        { 
            throw new NotImplementedException();
        }
        public void tagToHud(RichBoxContent content)
        {
            tagSprites(out SpriteName back, out SpriteName art);
            if (back != CityTag.NoBackSprite)
            {
                content.Add(new RichBoxOverlapImage(
                    new RichBoxImage(back),
                    art, Vector2.Zero, 0.8f));
            }
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

        public override IntVector2 TilePos()
        {
            return tilePos;
        }
        public override Vector3 WorldPos()
        {
            return position;
        }

        protected void processAsynchWork(List<WorkerStatus> workerStatuses)
        {
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                var status = workerStatuses[i];
                if (status.work > WorkType.Idle &&
                    Ref.TotalGameTimeSec > status.processTimeStartStampSec + status.processTimeLengthSec)
                {
                    //Work complete
                    onWorkComplete_async(ref status);
                    workerStatuses[i] = status;
                }

            }
        }

        virtual protected void onWorkComplete_async(ref WorkerStatus status)
        {  
            throw new NotImplementedException();
        }

        
        //public override bool Alive()
        //{
        //    return !isDeleted;
        //}
    }

    
}

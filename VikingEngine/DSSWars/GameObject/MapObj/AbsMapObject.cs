using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
//using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.EngineSpace;
using VikingEngine.HUD.RichBox;
using VikingEngine.Network;
//


namespace VikingEngine.DSSWars.GameObject
{
    /// <summary>
    /// Large scale objects
    /// </summary>
    abstract partial class AbsMapObject : AbsGroup
    {
        //public Faction faction;

        /// <summary>
        /// Pågående strider, om order ges läggs inte battle till förrän armeerna är intill varandra
        /// </summary>

        public bool enterRender_overviewLayer_async = false;
        public bool enterRender_detailLayer_async = false;
        public bool inRender_overviewLayer = false;
        public bool inRender_detailLayer = false;

        public int previousWarAgainstFaction = -1;
        public float strengthValue = -1;
        public float mobilityValue = 0;

        public IntVector2 tilePos;
        public TimeStamp lastNetUpdate = new TimeStamp();
        public int previousIncome_copp = 0;
        public Money money = new Money(0);
        public bool IsNetHosted = true;
        public ObjectName name = new ObjectName();

        public AbsNetworkPeer NetHostingPeer()
        {
            if (IsNetHosted)
            {
                return Ref.netSession.LocalPeer();
            }
            else if (pfaction.TryGetPlayer(out var p) && p.IsRemotePlayer())
            {
                return p.GetRemotePlayer().networkPeer.peer;
            }
            else
            { 
                return Ref.netSession.Host();
            }
        }

        public MapObjectTag Tag = new MapObjectTag();

        public AbsMapObject()
        {
            
            //battlesCounter = new SpottedArrayCounter<AbsMapObject>(battles);
        }

        public PMapObject mapObjPointer()
        {
            return new PMapObject(gameobjectType(), pfaction, myIndex);
        }

        public override void NameEditEvent(string result, object tag)
        {
            name.setCustom(result);

            var w = Ref.netSession.BeginWritingPacket(PacketType.DssRename, PacketReliability.Reliable);
            mapObjPointer().write(w);
            name.write(w);
        }

        public static void NetReadRename(System.IO.BinaryReader r)
        {
            var pointer = new PMapObject(r);
            var obj = pointer.Get();
            if (obj != null)
            {
                obj.name.read(r, int.MaxValue);
            }
        }

        public void IndexToHud(RichBoxContent content)
        {
            content.Add(new RbText(string.Format(DssRef.lang.UnitId, myIndex.ToString() + (IsNetHosted ? " h" : " c")) , HudLib.SecondaryTextColor));
        }

        virtual public bool lowFood() { throw new NotImplementedException(); }
        public bool payGold(int cost)
        {
            if (DssRef.storage.ruleset_instance.centralGold)
            {
                var faction = pfaction.GetFaction();
                if (faction == null)
                {
                    return false;
                }
                return faction.payGold(cost, false, null);
            }
            else
            {
                return money.PayGold(cost, false);
            }
        }

        public bool payGold(int cost, bool allowDept)
        {
            if (DssRef.storage.ruleset_instance.centralGold)
            {
                var faction = pfaction.GetFaction();
                if (faction == null)
                {
                    return false;
                }
                return faction.payGold(cost, allowDept, null);
            }
            else
            {
                return money.PayGold(cost, allowDept);
            }
        }

        virtual public bool rayCollision(Ray ray)
        {
            return false;
        }

        virtual public void asynchCullingUpdate(float time, bool bStateA)
        {
            if (IsNetHosted || lastNetUpdate.belowTime_sec(20))
            {
                DssRef.state.culling.InRender_Asynch(ref enterRender_overviewLayer_async, ref enterRender_detailLayer_async, tilePos);
            }
            else
            {
                enterRender_overviewLayer_async = false;
                enterRender_detailLayer_async = false;
            }
        }
        

        virtual public void PauseUpdate()
        {
            updateDetailLevel();

            
        }

        virtual public void clientPauseUpdate()
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
                if (this.gameobjectType() == GameObjectType.Army)
                {
                    lib.DoNothing();
                }
                inRender_detailLayer = enterRender_detailLayer_async;
                setInRenderState();
            }
        }

        abstract public void setInRenderState();

        //virtual public void ExitBattleGroup()
        //{
        //    battleGroup = null;
        //}

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
        public override void toButtonContent(RichBoxContent content, bool dark)
        {
            content.Add(new RbText(Name(out _), dark ? HudLib.TitleColor_Name_Dark : HudLib.TitleColor_Name));
            content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
            TypeIcon(content);
            content.hspace();
            content.Add(new RbText(TypeName(), dark? HudLib.TitleColor_TypeName_Dark : HudLib.TitleColor_TypeName));

        }

        public void toTabContent(RichBoxContent content, bool dark)
        {
            //content.Add(new RbText(Name(out _), dark ? HudLib.TitleColor_Name_Dark : HudLib.TitleColor_Name));
            //content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
            TypeIcon(content);
            content.hspace();
            content.Add(new RbText(myIndex.ToString(), dark ? HudLib.TitleColor_TypeName_Dark : HudLib.TitleColor_TypeName));

        }
        virtual public void tagSprites(out SpriteName back, out SpriteName art)
        {
            back = Tag.TagBack();//Data.TagLib.BackSprite(tagBack);
            art = Tag.TagArt();//Data.TagLib.ArtSprite(tagArt);
        }
        public bool tagToHud(RichBoxContent content)
        {
            tagSprites(out SpriteName back, out SpriteName art);
            if (back != TagLib.NoBackSprite)
            {
                if (art == SpriteName.NO_IMAGE)
                {
                    content.Add(new RbImage(back));
                }
                else
                {
                    content.Add(new RbOverlapImage(
                        new RbImage(back),
                        art, Vector2.Zero, 0.8f));
                }
                return true;
            }

            return false;
        }

        public bool LocalMember
        {
            get { return pfaction.GetPlayer().IsLocal; }
        }

        //abstract public Faction Faction();

        virtual public void setFaction(Faction newFaction, bool duringStartup, bool convert, ConvertReason convertReason, bool netShare)
        {
            this.pfaction = newFaction.pfaction;
            
            OnNewOwner(newFaction, convert, convertReason);

            IsNetHosted = newFaction.IsNetHosted();
        }

        //override public Faction GetFaction()
        //{
        //    return faction;
        //}

        abstract public void OnNewOwner(Faction newFaction, bool convert, ConvertReason convertReason);

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

        protected void processAsynchWork(ref StructList<WorkerStatus> workerStatuses)
        {
            for (int i = 0; i < workerStatuses.Count; i++)
            {
                ref WorkerStatus status = ref workerStatuses.array[i];
                if (status.work > WorkType.Idle &&
                    Ref.TotalGameTimeSec > status.processTimeStartStampSec + status.processTimeLengthSec)
                {
                    //Work complete
                    onWorkComplete_async(ref status); //index out  of bounds here
                }

            }
        }

        virtual protected void onWorkComplete_async(ref WorkerStatus status)
        {  
            throw new NotImplementedException();
        }

        abstract public bool IsCity();
        abstract public bool IsArmy();
    }

    
}

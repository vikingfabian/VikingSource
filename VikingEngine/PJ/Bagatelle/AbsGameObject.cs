using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class AbsGameObject
    {
        static readonly Vector2 ShadowScaleUp = new Vector2(1.1f);
        public bool localMember = true;

        static CirkleCounterUp NextMovingItemLayer = new CirkleCounterUp(0, 127);
        static CirkleCounterUp NextPickupLayer = new CirkleCounterUp(0, 127);
        protected BagatellePlayState state;

        protected Graphics.Image boundImage;
        public Graphics.Image image, shadow;
        public VikingEngine.Physics.AbsBound2D bound;

        protected Vector2 velocity = Vector2.Zero;
        protected float rotationSpeed = 0;

        public float elasticity = 0.9f;

        public bool solidBound = true, pickupType = false, damagingType = false;
        public AbsGamer gamer = null;
        public LocalGamer localGamer;
        public RemoteGamer remoteGamer;
        

        public int networkId;

        public AbsGameObject()
        {   
        }

        protected void createShadow()
        {
            shadow = image.CloneMe() as Graphics.Image;
            shadow.Color = Color.Black;
            shadow.Opacity = 0.2f;
            shadow.Layer = ImageLayers.Background6;
        }

        protected void setGamer(LocalGamer localGamer)
        {
            this.localGamer = localGamer;
            gamer = localGamer;
        }
        protected void setGamer(RemoteGamer remoteGamer)
        {
            this.remoteGamer = remoteGamer;
            gamer = remoteGamer;
        }

        public void setMovingItemnLayer()
        {
            image.Layer = BagLib.MovingItemsLayer;
            image.ChangePaintLayer(-NextMovingItemLayer.Next());
        }
        public void setPickUpLayer()
        {
            image.Layer = BagLib.PickupsLayer;
            image.ChangePaintLayer(-NextPickupLayer.Next());
        }

        public AbsGameObject(int networkId, BagatellePlayState state)
        {
            init(networkId, state);
        }

        protected void init(int networkId, BagatellePlayState state)
        {
            this.networkId = networkId;
            this.state = state;
            state.gameobjects.AddOrReplace(networkId, this);
            //Debug.Log("##Add GO: " + this.ToString());
        }

        virtual public void update()
        { }

        public void updateShadow()
        {
            shadow.Position = image.Position + state.shadowOffset;
            
            shadow.Size = image.Size * ShadowScaleUp;
        }

        virtual public void OnCollsion(AbsGameObject otherObj, Physics.Collision2D coll, bool primaryCheck)
        { }

        virtual public void OnHitWaveCollision(Ball fromball, LocalGamer gamer)
        { }

        public bool isDeleted = false;

        virtual public void DeleteMe(bool local)
        {
            image.DeleteMe();
            if (shadow != null)
            {
                shadow.DeleteMe();
            }
            isDeleted = true;
        }

        virtual public void readDeleteMe(Network.ReceivedPacket packet)
        {
            DeleteMe(false);
            //Debug.Log("Read delete: " + this.ToString());
        }

        virtual public void PickUpEvent(AbsGameObject collectingItem, LocalGamer gamer)
        { }

        public Vector2 position
        {
            get { return image.Position; }
            set
            {
                image.Position = value;
                bound.Center = value;
            }
        }

        public void writeId(System.IO.BinaryWriter w)
        {
            w.Write(networkId);
        }
        public static int ReadId(System.IO.BinaryReader r)
        {
            return r.ReadInt32();
        }

        virtual public void netReadUpdate(System.IO.BinaryReader r)
        { }

        protected void beginNetWriteItemStatus(AbsGameObject affectingItem)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdItemStatus, Network.PacketReliability.Reliable);
            writeId(w);
            if (affectingItem == null)
            {
                w.Write(0);
            }
            else if (affectingItem is BumpWave)
            {
                ((BumpWave)affectingItem).ball.writeId(w);
            }
            else
            {
                w.Write(affectingItem.networkId);
            }
            netWriteItemStatus(w);
        }

        virtual protected void netWriteItemStatus(System.IO.BinaryWriter w)
        {
        }
        virtual public void netReadItemStatus(AbsGameObject affectingItem, System.IO.BinaryReader r)
        {
        }

        public override string ToString()
        {
            return this.GetType().Name + (localMember ? " L" : " R") + networkId.ToString();
        }

        virtual public Ball GetBall()
        {
            return null;
        }
        
        virtual public void onGaveDamage() { }

        virtual public GameObjectType Type { get { return GameObjectType.UNKNOWN; } }
    }

}

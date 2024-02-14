using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.MiniGolf
{
    abstract class AbsGameObject : IDeleteable
    {
        public bool isDeleted = false;

        protected Graphics.Image boundImage;
        public VikingEngine.Physics.AbsBound2D bound;
        public Graphics.Image image;
        public Vector2 velocity = Vector2.Zero;
        protected float rotationSpeed = 0;
        public bool inUpdateList = false;
        
        protected void startLayer()
        {
            Ref.draw.CurrentRenderLayer = Draw.ShadowObjLayer;
        }
        protected void endLayer()
        {
            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        virtual public void update()
        { }
        virtual public void update_asynch()
        { }

        virtual public void onFieldCollision(Physics.Collision2D coll, float otherElasticity)
        {}

        virtual public void onItemCollision(AbsItem item, Physics.Collision2D coll, bool mainBound)
        { }

        virtual public void onBallCollision(Ball otherBall, Physics.Collision2D coll)
        { }

        virtual public bool ableToCollideWith(AbsGameObject otherBall)
        {
            return true;
        }

        virtual public void takeDamage(DamageOrigin origin) { }

        virtual public void DeleteMe()
        {
            isDeleted = true;
            image.DeleteMe();
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
        }

        public bool IsAlive { get { return !isDeleted; } }

        abstract public ObjectType Type { get; }
    }
}

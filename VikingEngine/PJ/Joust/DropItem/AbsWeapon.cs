using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Joust.DropItem
{
    abstract class AbsWeapon
    {
        protected Gamer gamer;
        //protected Level level;

        public AbsWeapon(Gamer parent)
        {
            this.gamer = parent;
        }

        virtual public void update(List<Gamer> gamers, int gamerIx)
        { }

        virtual public void OnTakeDamage()
        {
            QuickDelete();
        }

        virtual public void OnBoundCollision(object collObj)
        {
            throw new NotImplementedException();
        }

        virtual public void OnFlap()
        { }

        virtual public void onAirTrick(bool begin)
        { }

        virtual public void onAirTrickUpdate()
        { }
        
        virtual public void QuickDelete() 
        {
            gamer.removeWeapon(this);//.weapons.Remove(this);           
        }

        virtual public bool overrideBoxPickup()
        {
            return false;
        }

        abstract public WeaponType Type { get; }
    }

    abstract class AbsLevelWeapon : AbsLevelObject
    {
        public Gamer parent;
        protected Vector2 speed;
        protected VectorRect outerBound;
        
        public AbsLevelWeapon(Gamer parent)
        {
            this.parent = parent;
         }

        public override void onGamerCollision(Gamer gamer)
        {
            Vector2 dirToGamer = gamer.Position - Bound.Center;
            dirToGamer.Normalize();

            Particles.PlayerCollisionParticle.Create(Bound.Center + dirToGamer * Bound.InnerCirkleRadius);
        }

        protected void updateLevelMove()
        {
            image.position += speed * Ref.DeltaTimeMs;
            Bound.Center = image.Position;

            if (outerBound.IntersectPoint(image.position) == false)
            {
                DeleteMe();
            }
        }

        public override bool CollideWithLyingGamer
        {
            get
            {
                return true;
            }
        }
    }

    enum WeaponType
    { 
        ShieldShells,
        LazerGun,
        ShieldBubble,
    }
}

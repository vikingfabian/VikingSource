using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Physics;

namespace VikingEngine.PJ.SpaceWar
{
    abstract class AbsGameObject
    {
        public Bound2DWrapper bound;
        public Graphics.Mesh model;
        protected Rotation1D rotation = Rotation1D.D0;
        protected Velocity velocity;
        public bool isDeleted = false;

        public AbsGameObject()
        { 
        }

        virtual public bool update()
        {
            return isDeleted;
        }

        protected void updateMovement()
        {
            model.Position += velocity.Value * Ref.DeltaTimeMs;

            bound.update(model.Position, rotation.radians);

            if (Ref.update.LasyUpdatePart == Engine.LasyUpdatePart.Part1)
            {
                if (model.X < WorldMap.DespawnArea.X)
                {
                    model.X = WorldMap.DespawnArea.Right;
                    outsideMapEvent();
                }
                else if (model.X > WorldMap.DespawnArea.Right)
                {
                    model.X = WorldMap.DespawnArea.X;
                    outsideMapEvent();
                }

                if (model.Z < WorldMap.DespawnArea.Y)
                {
                    model.Z = WorldMap.DespawnArea.Bottom;
                    outsideMapEvent();
                }
                else if (model.Z > WorldMap.DespawnArea.Bottom)
                {
                    model.Z = WorldMap.DespawnArea.Y;
                    outsideMapEvent();
                }
            }
        }

        virtual protected void outsideMapEvent()
        { }

        virtual public void takeDamage(Vector3 damageCenter)
        {
        }

        public void DeleteMe()
        {
            model.DeleteMe();
            isDeleted = true;
            bound.DeleteMe();
        }

        abstract public GameObjectType Type { get; }
        abstract public CollisionType CollisionType { get; }
    }

    enum GameObjectType
    {
        Asteroid,
        Coin,
        Lorry,
        NUM_NON,
    }

    enum CollisionType
    {
        NoCollision,
        BodyCollision,
        PickUp,
    }
}

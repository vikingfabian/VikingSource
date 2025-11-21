using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class EmptyPoint : AbsGameObject
    {
        Vector3 position;
        public EmptyPoint(Vector3 wp) 
        { 
            position = wp;
        }

        public override Microsoft.Xna.Framework.Vector3 WorldPos()
        {
            return position;
        }

        public override GameObjectType gameobjectType()
        {
             return GameObjectType.Point;
        }
    }
}

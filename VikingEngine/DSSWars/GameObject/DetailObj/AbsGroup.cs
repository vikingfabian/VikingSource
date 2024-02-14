﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsGroup : AbsGameObject
    {           
        public float groupRadius, attackRadius;

        virtual public Vector2 WorldPositionXZ()
        {
            return VectorExt.V3XZtoV2(position);
        }
        virtual public SoldierGroup GetGroup() { return null; }
        
        virtual public City GetCity() { return null; }

        virtual public Army GetArmy() { return null; }

        abstract public bool isMelee();
    }
}

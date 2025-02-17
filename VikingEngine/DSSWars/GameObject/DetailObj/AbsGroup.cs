using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsGroup : AbsWorldObject
    {           
        public float groupRadius, attackRadius;

        public float highTargetValueToOpponent = float.MaxValue;
        public int highTargetValueToOpponent_tagId = -1;
        //public IntVector2 battleGridPos, prevBattleGridPos;

        virtual public Vector2 WorldPositionXZ()
        {
            return VectorExt.V3XZtoV2(position);
        }
        virtual public SoldierGroup GetGroup() { return null; }
        
        

        //abstract public bool isMelee();
    }
}

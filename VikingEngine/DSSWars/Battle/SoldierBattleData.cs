using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Battle
{
    class SoldierBattleData
    {
        static List<AbsSoldierUnit> SoldierBuffer = new List<AbsSoldierUnit>(16);
        static List<AbsGroup> GroupBuffer = new List<AbsGroup>(16);
                
        List<AbsSoldierUnit> nearBodyCollisionUnits = new List<AbsSoldierUnit>(8);
        public float queueTime = 0;
        public void update(AbsSoldierUnit parent)
        {
            //1. Is the soldier queuing behind friendlies
            //2. Is he bumping into other people/items
            Vector2 collisionForce = Vector2.Zero;

            lock (nearBodyCollisionUnits)
            {
                foreach (var unit in nearBodyCollisionUnits)
                {
                    Physics.Collision2D intersection = parent.bound.Intersect2(unit.bound);
                    //Make sure friendly units dont push eachother forward
                    if (intersection.IsCollision)
                    {
                        if (parent.GetFaction() == unit.GetFaction())
                        {
                            if (Rotation1D.AngleDifference_Absolute(parent.rotation.radians, lib.V2ToAngle(-intersection.direction)) < MathExt.TauOver8)
                            {
                                //Is pushing friend, halt and queue
                                queueTime = 400;
                            }
                        }
                        collisionForce += intersection.direction;                        
                    }
                }
            }

            if (VectorExt.HasValue(collisionForce))
            {
                float collPush = 0.18f;
                if (queueTime > 0)
                {
                    collPush = 0.25f;
                }
                parent.position += VectorExt.V2toV3XZ(collPush * collisionForce);

                //collisionForce = Vector2.Zero;
            }
        }

        public bool InQueue(AbsSoldierUnit parent)
        {
            parent.bound.Center += parent.rotation.Direction(parent.bound.ExtremeRadius * 0.25f);

            lock (nearBodyCollisionUnits)
            {
                foreach (var unit in nearBodyCollisionUnits)
                {
                    if (parent.GetFaction() == unit.GetFaction())
                    {
                        Physics.Collision2D intersection = parent.bound.Intersect2(unit.bound);
                        if (intersection.IsCollision)
                        {
                            queueTime = 400;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void asycUpdate(AbsSoldierUnit parent) 
        {
            const float SoldierToGroupMaxDistance = 1.0f;

            SoldierBuffer.Clear();
            //Collect nearby collision bounds
            DssRef.world.unitCollAreaGrid.collectGroups(parent.tilePos, ref GroupBuffer, false);

            foreach (var group in GroupBuffer)
            {
                if (VectorExt.Length(group.position.X - parent.position.X, group.position.Z - parent.position.Z) < SoldierToGroupMaxDistance)
                {
                    if (group.gameobjectType() == GameObjectType.SoldierGroup)
                    {
                        var soldiers = group.GetGroup().soldiers;
                        if (soldiers != null)
                        {
                            var soldiersC = soldiers.counter();
                            while (soldiersC.Next())
                            {
                                if (parent.bound.AsynchCollect(soldiersC.sel.bound) &&
                                    soldiersC.sel != parent)
                                {
                                    SoldierBuffer.Add(soldiersC.sel);
                                }
                            }
                        }
                    }
                }
            }

            lock (nearBodyCollisionUnits)
            {
                nearBodyCollisionUnits.Clear();
                nearBodyCollisionUnits.AddRange(SoldierBuffer);
            }
        }

    }
}

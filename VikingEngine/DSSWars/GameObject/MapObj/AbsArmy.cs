using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    abstract partial class AbsArmy : AbsMapObject
    {
        protected bool army_isIdle = true;

        public SpottedArray<SoldierGroup> groups = new SpottedArray<SoldierGroup>(32);
        public Rotation1D rotation = Rotation1D.D180.Add(Ref.peRnd.Plus_MinusF(0.8f));
        public int goalId = 0;
        public bool walkGoalAsShip = false;
        public int soldiersCount = 0;

        public void AddSoldierGroup(SoldierGroup group)
        {
            //Hitta en plats bland alla grupper
            group.parentArrayIndex = groups.Add(group);
            group.army = this;
        }
        virtual public void remove(SoldierGroup group)
        {
            Debug.CrashIfThreaded();
            groups.RemoveAt_EqualSafeCheck(group, group.parentArrayIndex);
            
               
            
        }

        virtual public void asyncNearObjectsUpdate()
        {
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.asynchNearObjectsUpdate();
            }
        }

        abstract public bool IdleObjetive();

        abstract public bool IsCity();
        abstract public bool IsArmy();

    }
}

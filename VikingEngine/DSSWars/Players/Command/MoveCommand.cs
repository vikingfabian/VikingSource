using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Players.Command
{
    abstract class AbsCommand
    {
        public bool haltCommand = false;
        public bool clearOldPath = true;
        public AbsCommand(SoldierGroup group)
        { 
            group.command = this;
        }

        abstract public bool hasPathCommand(out bool pathTowardsUnit);

        virtual public Vector3 GoalPosition() { throw new NotImplementedException(); }

        virtual public AbsGroup AttackTarget() { return null;  }

        virtual public void OnMovePathComplete()
        { 
            haltCommand = true;
        }
    }

    class MoveCommand : AbsCommand
    {
        Vector3 goalWp;

        public MoveCommand(SoldierGroup group, Vector3 goalWp)
            :base(group)
        { 
            this.goalWp = goalWp;
            group.wakeupSoldiers();
        }

        public override bool hasPathCommand(out bool pathTowardsUnit)
        {
            pathTowardsUnit = false;
            return true;
        }

        public override Vector3 GoalPosition()
        {
            return goalWp;
        }
    }

    class AttackCommand : AbsCommand
    {
        AbsGroup target;

        public AttackCommand(SoldierGroup group, AbsGroup target)
            : base(group)
        {
            this.target = target;
            group.wakeupSoldiers();
        }

        public override bool hasPathCommand(out bool pathTowardsUnit)
        {
            pathTowardsUnit = true;
            return true;
        }

        public override AbsGroup AttackTarget()
        {
            return target;
        }
    }
}

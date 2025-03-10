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
        public AbsCommand nextCommand = null;

        public bool haltCommand = false;
        public bool clearOldPath = true;
        public AbsCommand(SoldierGroup group, bool queueCommand)
        {
            if (queueCommand && group.command != null)
            {
                group.command.placeLastInQueue(this);
            }
            else
            {
                group.command = this;
            }
        }

        abstract public bool hasPathCommand(out bool pathTowardsUnit);

        virtual public Vector3 GoalPosition() { throw new NotImplementedException(); }

        virtual public AbsGroup AttackTarget() { return null;  }

        virtual public void OnMovePathComplete(SoldierGroup group)
        {
            if (nextCommand != null)
            {
                group.cancelCommand();
            }
            else
            {
                haltCommand = true;
            }
        }

        virtual public void begin(SoldierGroup group) { }

        public void placeLastInQueue(AbsCommand next)
        {
            if (nextCommand == null)
            {
                nextCommand = next;
            }
            else
            {
                nextCommand.placeLastInQueue(next);
            }
        }
    }

    class EnterPostCommand : AbsCommand
    {
        IntVector2 subTile;

        public EnterPostCommand(SoldierGroup group, IntVector2 subTile, bool queueCommand)
            : base(group, queueCommand)
        {
            this.subTile = subTile;
            group.wakeupSoldiers();
        }

        public override void begin(SoldierGroup group)
        {
            base.begin(group);
            new GuardPostTransform(group, conv.IntVector2ToInt(subTile), false);
        }

        public override bool hasPathCommand(out bool pathTowardsUnit)
        {
            pathTowardsUnit = false;
            return false;
        }
    }

    class MoveCommand : AbsCommand
    {
        Vector3 goalWp;

        public MoveCommand(SoldierGroup group, Vector3 goalWp, bool queueCommand)
            :base(group, queueCommand)
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

        public AttackCommand(SoldierGroup group, AbsGroup target, bool queueCommand)
            : base(group, queueCommand)
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

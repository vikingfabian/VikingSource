using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Players.Command
{

    class NetClientCommand : MoveCommand
    { 
        public NetClientCommand(SoldierGroup group, Vector3 goalWp) 
            : base(group, goalWp, float.MinValue, false)
        {
        }
        public override void refreshGoal(Vector3 goalPos)
        {
            goalWp = goalPos;
        }
    }
    class MoveCommand : AbsCommand
    {
        protected Vector3 goalWp;

        public MoveCommand(SoldierGroup group, Vector3 goalWp, float goalRotation, bool queueCommand)
            :base(group, queueCommand)
        { 
            this.goalRotation = goalRotation;
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
        protected override CommandType GetCommandType()
        {
            return CommandType.Move;
        }
    }

    class AttackCommand : AbsCommand
    {       
        AbsGroup target;
        Vector2 targetGroupOffset;
        Vector3 goalWp;
        bool towardsUnit = false;

        public AttackCommand(SoldierGroup group, Vector2 targetGroupOffset, AbsGroup target, bool queueCommand)
            : base(group, queueCommand)
        {
            this.targetGroupOffset = targetGroupOffset;
            this.target = target;
            group.wakeupSoldiers();
            goalWp = target.position;
        }

        public override void asyncUpdate(SoldierGroup group)
        {
            float l = (target.position - group.position).Length();
            if (l < group.attackRadius + 0.25f)
            {
                towardsUnit = true;
                goalWp = target.position;
            }
            else
            {
                towardsUnit = false;
                goalWp = VectorExt.AddXZ(target.position, targetGroupOffset);
            }
             
        }

        public override bool hasPathCommand(out bool pathTowardsUnit)
        {
            pathTowardsUnit = towardsUnit;
            return true;
        }

        public override Vector3 GoalPosition()
        {
            return goalWp;
        }

        public override AbsGroup AttackTarget()
        {
            return target;
        }
        protected override CommandType GetCommandType()
        {
            return CommandType.Attack;
        }
    }
}

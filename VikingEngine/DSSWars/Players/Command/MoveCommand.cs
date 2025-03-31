using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.DSSWars.Players.Command
{
    enum CommandType
    { 
        Move,
        Attack,
        EnterPost,
    }

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

        public bool HasCommand(CommandType type)
        {
            if (type == this.GetCommandType())
            {
                return true;
            }
            if (nextCommand != null)
            {
                return nextCommand.HasCommand(type);
            }

            return false;
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

        virtual public bool isEnterPost(int postId)
        { 
            return false;
        }

        abstract protected CommandType GetCommandType();
    }

    class EnterPostCommand : AbsCommand
    {
        IntVector2 subTile;
        public int id;

        public EnterPostCommand(SoldierGroup group, IntVector2 subTile, bool queueCommand)
            : base(group, queueCommand)
        {
            this.subTile = subTile;
            this.id = conv.IntVector2ToInt(subTile);
        }

        public EnterPostCommand(SoldierGroup group, int postId, bool queueCommand)
            : base(group, queueCommand)
        {
            this.subTile = conv.IntToIntVector2(postId);
            this.id = postId;
            group.wakeupSoldiers();
        }

        public void claimPost(SoldierGroup group, City city, int defenceIndex)
        {
            if (arraylib.InBound(city.defenceBuildings, defenceIndex))
            {
                var defence = city.defenceBuildings[defenceIndex];

                defence.soldierGroupId = group.parentArrayIndex;

                city.defenceBuildings[defenceIndex] = defence;
            }
        }

        //void init(SoldierGroup group, City city)
        //{
            
        //    var defence = city.defenceBuildings.Array[id];
        //    defence.soldierGroupId = group.parentArrayIndex;
        //    city.defenceBuildings[id] = defence;
        //}


        public override void begin(SoldierGroup group)
        {
            base.begin(group);
            new GuardPostTransform(group, id, false);
        }

        public override bool hasPathCommand(out bool pathTowardsUnit)
        {
            pathTowardsUnit = false;
            return false;
        }
        public override bool isEnterPost(int postId)
        {
            return postId == id;
        }

        protected override CommandType GetCommandType()
        {
            return CommandType.EnterPost;
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
        protected override CommandType GetCommandType()
        {
            return CommandType.Move;
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
        protected override CommandType GetCommandType()
        {
            return CommandType.Attack;
        }
    }
}

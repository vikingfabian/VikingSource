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
        ClaimCity,
    }

    abstract class AbsCommand
    {
        public AbsCommand nextCommand = null;

        public bool haltCommand = false;
        public bool clearOldPath = true;
        public float goalRotation = float.MinValue;
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

        virtual public AbsGroup AttackTarget() { return null; }

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
            if (nextCommand != null)
            {
                return nextCommand.isEnterPost(postId);
            }
            return false;
        }

        abstract protected CommandType GetCommandType();

        virtual public void asyncUpdate(SoldierGroup group) { }
    }
}

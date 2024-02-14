using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.ToggEngine.QueAction
{
    class Que
    {
        AbsGenericPlayer player;
        QueType type;
        public List2<QueAction.AbsQueAction> queActions = new List2<QueAction.AbsQueAction>(4);
        Action update;

        public Que(AbsGenericPlayer player)
           : this(QueType.Player)
        {
            this.player = player;
        }

        public Que(QueType type)
        {
            this.type = type;
            switch (type)
            {
                case QueType.Game:
                    update = gameUpdate;
                    break;
                case QueType.Player:
                    update = playerUpdate;
                    break;
                case QueType.OtherPlayer:
                    update = otherPlayerUpdate;
                    break;
            }
            
        }

        /// <returns>Override general update</returns>
        public bool Update()
        {
            if (queActions.Count > 0)
            {
                update.Invoke();
                //return true;
            }

            return queActions.Count > 0;
        }

        void gameUpdate()
        {
            QueAction.AbsQueAction action = null;
            if (toggRef.NetHost)
            {
                action = queActions[0];
            }
            else
            {
                for (int i = 0; i < queActions.Count; ++i)
                {
                    if (queActions[i].state >= QueState.Started || queActions[i].readyToStart())
                    {
                        action = queActions[i];
                        break;
                    }
                }
            }

            IntVector2 camTarget;
            bool inCamCheck;
            if (action.CameraTarget(out camTarget, out inCamCheck))
            {
                toggRef.cam.spectate(camTarget, inCamCheck);
                //hqRef.players.localHost.mapControls.updateSpectateCamera(camTarget, inCamCheck);
            }

            runAction(action);
        }

        void runAction(AbsQueAction action)
        {
            if (action == null)
            {
                return;
            }

            if (action.state == QueState.QuedUp)
            {
                action.startSetup();
            }

            if (action.update() ||
                action.state == QueState.Completed)
            {
                onComplete(action);
            }
        }

        void onComplete(AbsQueAction action)
        {
            action.onRemove();
            action.state = QueState.Completed;
            queActions.Remove(action);

            player?.onActionComplete(action, queActions.Count == 0);
        }

        void playerUpdate()
        {
            QueAction.AbsQueAction action = queActions[0];

            IntVector2 camTarget;
            bool inCamCheck;
            if (action.CameraTarget(out camTarget, out inCamCheck))
            {
                toggRef.cam.spectate(camTarget, inCamCheck);
                //player.mapControls.updateSpectateCamera(camTarget, inCamCheck);
            }

            runAction(action);
        }

        void otherPlayerUpdate()
        {
            queActions.loopBegin();
            while (queActions.loopNext())
            {
                if (queActions.sel.state == QueState.Completed)
                {
                    queActions.loopRemove();
                }
            }
        }

        public void Add(QueAction.AbsQueAction qa)
        {
            qa.inQue = this;

            if (qa.IsTopPrio)
            {
                for (int i = 0; i < queActions.Count; ++i)
                {
                    if (!queActions[i].IsTopPrio &&
                        queActions[i].state == QueState.QuedUp)
                    {
                        queActions.Insert(i, qa);
                        return;
                    }
                }
            }

            queActions.Add(qa);
        }

    }

    enum QueType
    {
        Game,
        Player,
        OtherPlayer,
    }
}

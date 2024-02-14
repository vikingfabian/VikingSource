using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class MonsterRespawn : ToggEngine.QueAction.AbsQueAction
    {
        IntVector2 centerPos = IntVector2.Zero;
        bool beforeActions;

        public MonsterRespawn(bool beforeActions)
            : base()
        {
            this.beforeActions = beforeActions;
        }

        public override void onBegin()
        {
            base.onBegin();

            var units = hqRef.setup.conditions.monsterRespawnSession(
                hqRef.players.dungeonMaster.TurnsCount, beforeActions);

            if (arraylib.HasMembers(units))
            {
                hqRef.players.dungeonMaster.netWriteSpawn(units);

                foreach (var m in units)
                {
                    centerPos += m.squarePos;
                }

                centerPos /= units.Count;

                var w = Ref.netSession.BeginWritingPacket(
                    Network.PacketType.hqSpectatePos, Network.PacketReliability.Reliable);
                toggRef.board.WritePosition(w, centerPos);

                viewTime = new Time(1.6f, TimeUnit.Seconds);
            }
            else
            {
                viewTime.setZero();
            }
        }

        public override bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            camTarget = centerPos;
            inCamCheck = false;

            return true;
        }

        public override bool update()
        {
            return base.update();
        }

        public override bool NetShared => false;

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.MonsterRespawn;
    }

    
}

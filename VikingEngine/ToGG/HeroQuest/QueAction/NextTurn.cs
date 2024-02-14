using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class QueActionStartTurn : ToggEngine.QueAction.AbsQueAction
    {
        int nextTeam;

        public QueActionStartTurn(int nextTeam)
            : base()
        {
            this.nextTeam = nextTeam;
        }

        public QueActionStartTurn(System.IO.BinaryReader r)
           : base(r)
        {
        }

        protected override void netWrite(System.IO.BinaryWriter w)
        {
            base.netWrite(w);
            w.Write((byte)nextTeam);
            
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            nextTeam = r.ReadByte();
        }

        public override void onBegin()
        {
            hqRef.gamestate.startNextTurn(nextTeam);
        }

        public override bool update()
        {
            return true;
        }

        public override bool readyToStart()
        {
            return
                !isLocalAction ||
                (hqRef.gamestate.gamephase != GamePhase.Gameplay && 
                hqRef.gamestate.gamephase != GamePhase.Init &&
                hqRef.players.currentTeam != nextTeam);
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.StartTurn; } }
    }

    class QueActionEndTurn : ToggEngine.QueAction.AbsQueAction
    {
        int endTeam;

        public QueActionEndTurn(bool endByAi)
            : base()
        {
            this.endTeam = hqRef.players.currentTeam;
        }

        public QueActionEndTurn(System.IO.BinaryReader r)
           : base(r)
        {
        }

        protected override void netWrite(System.IO.BinaryWriter w)
        {
            base.netWrite(w);
            w.Write((byte)endTeam);

        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            endTeam = r.ReadByte();
        }

        public override void onBegin()
        {
            hqRef.gamestate.endTurn();
        }

        public override bool readyToStart()
        {
            return !isLocalAction || 
                hqRef.gamestate.gamephase == GamePhase.Gameplay || 
                hqRef.gamestate.gamephase == GamePhase.SpectateAi;
        }

        public override bool update()
        {
            return true;
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.EndTurn; } }
    }
}

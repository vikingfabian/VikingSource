using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.Commander.LevelSetup;

namespace VikingEngine.ToGG
{
    abstract class AbsPlayState : AbsToggState
    {
        
        protected Data.LoadMap loadingMap = null;
        public GameSetup gameSetup;
        public bool gameHasStarted = false;
        public UnitMoveAndAttackGUI hoverUnitMoveAndAttackDots = null;
        public ToggEngine.Display3D.UnitMessagesHandler unitMessages;
        public int nextQueIndex = 0;
        public ToggEngine.QueAction.Que que = new ToggEngine.QueAction.Que(ToggEngine.QueAction.QueType.Game);

        public AttackRollSpeed attackRollSpeed = new AttackRollSpeed();

        public AbsPlayState(GameMode mode)
            : base()
        {
            toggRef.mode = mode;
            toggRef.UnitsBlockLOS = mode == GameMode.Commander;
            HudLib.Init();
            ToggEngine.BattleEngine.DiceModel.Init();
            SlotMashineWheel.Init();
            new ToggEngine.Camera();
            

            toggRef.gamestate = this;
        }

        protected override void createDrawManager()
        {
            draw = new Draw();
        }

        public AbsUnit GetUnit(System.IO.BinaryReader r)
        {
            AbsGenericPlayer player;
            return GetUnit(r, out player);
        }

        public AbsUnit GetUnit(System.IO.BinaryReader r, out AbsGenericPlayer player)
        {
            player = toggRef.absPlayers.readGenericPlayer(r);
            if (player != null)
            {
                int unitId = r.ReadByte();
                return player.unitsColl.getUnitFromId(unitId);//.units.Array[r.ReadByte()];
            }

            return null;
        }

        public AbsUnit GetUnit(int globalPlayerIndex, int unitId, out AbsGenericPlayer player)
        {
            player = toggRef.absPlayers.GenPlayer(globalPlayerIndex);
            if (player != null)
            {
                return player.unitsColl.getUnitFromId(unitId);//player.unitsColl.units.Array[unitId];
            }

            return null;
        }

        virtual public void NewMapPrep()
        { }

        virtual public void MapLoadComplete()
        {
            loadingMap?.DeleteMe();
            loadingMap = null;
        }

        public bool LoadingMap => loadingMap != null;

        virtual public void debugAutoWin() { }

        virtual public void BeginNextPlayer() { }

        protected override bool DefaultMouseLock
        {
            get { return true; }
        }
    }
}

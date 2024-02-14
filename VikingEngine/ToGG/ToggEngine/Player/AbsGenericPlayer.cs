using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    abstract class AbsGenericPlayer
    {
        public Engine.AbsPlayerData pData;

        public UnitCollection unitsColl;

        public int TurnsCount = 0;
        public PlayerRelationVisuals relationVisuals;
        public MapControls mapControls = null;
        public AbsUnit movingUnit = null;
        //public bool movementPhaseIsLocked = false;
        public int totalDamageRecieved = 0;
        protected AttackerArrowGUI attackerArrow = null;
        public IntVector2 SpectatorTargetPos;
        public DiceRoll Dice = new DiceRoll();

        public ToggEngine.QueAction.Que que;

        public AbsGenericPlayer(Engine.AbsPlayerData pData)
        {
            this.pData = pData;
        }

        virtual public void onDestroyedUnit(AbsUnit attacker, AbsUnit destroyedUnit)
        {
            if (destroyedUnit.HasProperty(UnitPropertyType.Expendable))
            {
                return;
            }

            int points;

            if (destroyedUnit.HasProperty(UnitPropertyType.Valuable))
            {
                points = toggLib.VP_DestroyValuableUnit;
            }
            else if (destroyedUnit.cmd().data.underType == UnitUnderType.Special_TacticalBase)
            {
                points = toggLib.VP_DestroyEnemyBase;
            }
            else
            {
                points = toggLib.VP_DestroyUnit;
            }

            gainVictoryPoint(points, attacker);

            
            new VictoryPointEffect(destroyedUnit.squarePos);
        }

        virtual public void onDamagedUnit(AbsUnit attacker, AbsUnit defender)
        { }

        virtual public void onMovedUnit(AbsUnit unit)
        { }

        abstract public void nextPhaseButtonAction(bool forwardToNext);

        protected void turnStartGenericSetup()
        {
            TurnsCount++;
        }
        
        virtual protected void gainVictoryPoint(int points, AbsUnit collectingUnit)
        { }
        
        public AttackerArrowGUI getAttackerArrow()
        {
            if (attackerArrow == null)
            {
                attackerArrow = new AttackerArrowGUI();
            }
            return attackerArrow;
        }

        public void removeAttackerArrow()
        {
            if (attackerArrow != null)
            {
                attackerArrow.DeleteMe();
                attackerArrow = null;
            }
        }

        virtual public void onNewTile() { }
        virtual public void OnMapPan(Vector2 screenPan)
        {
            toggRef.hud?.extendedTooltip.onMapPan();
        }

        virtual public void onActionComplete(ToggEngine.QueAction.AbsQueAction action, bool emptyQue)
        { }

        abstract public bool IsScenarioOpponent { get; }
        public bool IsScenarioFriendly { get { return !IsScenarioOpponent; } }

        virtual public int AiActiveUnitsCount => -1;

        virtual public HeroQuest.Gadgets.ItemCollection specialArrows()
        {
            return null;
        }

        virtual public int addUnit(AbsUnit unit)
        {
           return unitsColl.Add(unit);
        }

        abstract public bool IsLocal { get; }

        abstract public bool LocalHumanPlayer { get; }
    }
}

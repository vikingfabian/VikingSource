using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players
{
    abstract class AbsHQPlayer : ToGG.AbsGenericPlayer
    {
        public int heroIndex = -1;
        public UnitCollection hqUnits;
        public PlayerNameDisplay nameDisplay;
        public AttackDisplay attackDisplay;
        public PlayerVisualData visualState = new PlayerVisualData();

        public bool readyToEndTurn_Confirmed;
        public bool hasStartedTurn = false;
                
       
        public ToggEngine.QueAction.Que otherPlayerLockQue;
        //public Gadgets.Backpack backpack;

        public AbsHQPlayer(Engine.AbsPlayerData pData)
            : base(pData)
        {
            hqUnits = new UnitCollection(this);
            unitsColl = hqUnits;

            hqRef.players.addPlayer(this);
            this.pData.teamIndex = pData.Type == Engine.PlayerType.Ai ? PlayerCollection.DungeonMasterTeam : PlayerCollection.HeroTeam;

            que = new ToggEngine.QueAction.Que(this);
            otherPlayerLockQue = new ToggEngine.QueAction.Que(ToggEngine.QueAction.QueType.OtherPlayer);
        }

        abstract public void update();

        virtual public void idleUpdate()
        {
            que.Update();
        }

        public void update_asynch(float time)
        {
            hqUnits.update_asynch(time);
        }

        public void assignPlayerIx(int globalPlayerIndex)
        {
            pData.globalPlayerIndex = globalPlayerIndex;
            hqUnits.playerGlobalIx = globalPlayerIndex;

            if (IsHero)
            {
                heroIndex = globalPlayerIndex - 1;
            }
        }

        virtual public void startGame()
        {
            if (IsHero)
            {
                nameDisplay = new PlayerNameDisplay(this);
            }
        }

        virtual public void onTurnStart()
        {
            hasStartedTurn = true;

            hqUnits.OnEvent(ToGG.Data.EventType.TurnStart, null);
        }

        virtual public void onTurnEnd()
        {
            hasStartedTurn = false;
            readyToEndTurn_Confirmed = false;

            unitsEndTurnActions();
            hqRef.playerHud.unitsGui.clear();
        }

        //virtual public void clearUnitActionMode() { }

        virtual protected void removeAttackDisplay()
        {
            if (attackDisplay != null)
            {
                if (!attackDisplay.hasAttacked)
                {
                    attackDisplay.cancelAttack();
                }

                attackDisplay.DeleteMe();
                attackDisplay = null;                
            }
        }

        void unitsEndTurnActions()
        {
            //TODO, ersätt med onEvent

            if (hqRef.netManager.host)
            {
                hqUnits.unitsCounter.Reset();
                while (hqUnits.unitsCounter.Next())
                {
                    var u = hqUnits.unitsCounter.sel.hq();

                    if (u.HasProperty(UnitPropertyType.Scratchy))
                    {
                        var targets = new PetTarget(u);
                        targets.collectAllTargets(u.squarePos);

                        foreach (var m in targets.targets)
                        {
                            new QueAction.PassiveSkillDamageUnit(u, m.hq(), 
                                ToGG.Data.Property.Scratchy.PetScratchDamage.NextDamage(this.Dice));
                        }
                    }
                }
            }
        }

        public Unit spawnUnit(HqUnitType type, IntVector2 pos, Net.INetRequestReciever netCallback)
        {
            if (toggRef.board.IsSpawnAvailableSquare(pos))
            {
                Unit u = new Unit(pos, type, this);
                new Net.SpawnUnitRequest(u, netCallback);

                return u;
            }

            netCallback?.NetRequestCallback(false);
            return null;
        }

        virtual public void onAttack()
        {   
        }

        public override void onDestroyedUnit(AbsUnit attacker, AbsUnit destroyedUnit)
        { }
           
       

        virtual public void OnEvent(ToGG.Data.EventType eventType, object tag)
        {
            hqUnits.unitsCounter.Reset();
            while (hqUnits.unitsCounter.Next())
            {
                hqUnits.unitsCounter.sel.hq().OnEvent(eventType, true, tag);
            }
        }

        public override void nextPhaseButtonAction(bool forwardToNext)
        {
            throw new NotImplementedException();
        }

        virtual public void add(Gadgets.AbsItem item, bool tryEquip, bool viewMessage)
        {
        }

        virtual public void lootItem(Gadgets.AbsItem item)
        { }

        virtual public void collectGadgetDefence(Unit unit, DefenceData defence, bool onCommit)
        {
            foreach (var m in unit.data.hero.equipment.list)
            {
                m.item?.collectDefence(defence, onCommit);
            }
        }

        

        virtual public void onNewUnit(AbsUnit unit)
        {
            //var hero = unit.hq().data.hero;
            //if (hero != null)
            //{
            //    //hero.equipment = HeroControls.backpack.equipment;

            //    //hero.equipment.quickbelt.setSlotCount(
            //    //    unit.hq().data.QuickBeltSize, this);

            //    unit.hq().data.startEquipment(HeroControls.backpack);
            //}            
        }

        virtual public void AlertedUnit(Unit unit)
        { }

        public override string ToString()
        {
            return pData.PublicName(LoadedFont.NUM_NON) + 
                TextLib.Parentheses(pData.globalPlayerIndex.ToString());
        }

        abstract public Unit HeroUnit { get; }

        abstract public Gadgets.Backpack Backpack();

        abstract public bool IsHero { get; }        

        abstract public bool IsDungeonMaster { get; }

        abstract public AbsHeroInstance HeroInstance { get; }
    }
}

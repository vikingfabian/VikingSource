using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.Players
{
    abstract class AbsCmdPlayer : AbsGenericPlayer
    {
        public List<AbsUnit> escapedUnits = new List<AbsUnit>();

        public int VictoryPoints = 0;
       
        public PlayerSettings settings;
        public Data.PlayerSetup setup;
        

        public AbsCmdPlayer(Engine.AbsPlayerData pData, int globalIndex, Data.PlayerSetup setup)
            :base(pData)
        {
            pData.globalPlayerIndex = globalIndex;
            this.pData.teamIndex = this.IsScenarioFriendly ? 0 : 1;
            unitsColl = new UnitCollection(pData.globalPlayerIndex);
            //que = new ToggEngine.QueAction.Que(this);

            this.setup = setup;
            
            settings = new PlayerSettings();
        }

        public void NetShareUnitSetup()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdShareUnitSetup, Network.PacketReliability.ReliableLasy);
            pData.writeGlobalIndex(w);            
        }        

        virtual protected Vector2 selectionPos { get { throw new NotImplementedException(); } }
        
        abstract public void Update();

        virtual public void UpdateSpectating() { }

        virtual public void onTurnStart(bool deployPhase)
        {
        }

        virtual public void EndTurn() 
        {
            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                unitsColl.unitsCounter.sel.OnTurnEnd();
                
                
            }


            //unitsColl.unitsCounter.Reset();
            //while (unitsColl.unitsCounter.Next())
            //{
            //    if (unitsColl.unitsCounter.sel.order != null)
            //    {
            //        throw new Exception();
            //    }
            //}
        }

        abstract public GamePhaseType GamePhase { get; }

        abstract public void StartPhase(GamePhaseType phase);

        public void collectBannerVP()
        {
            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                if (unitsColl.unitsCounter.sel.collectionIndex == 12)
                {
                    lib.DoNothing();
                }
                if (unitsColl.unitsCounter.sel.OnSquare.tileObjects.HasObject(TileObjectType.TacticalBanner))
                {
                    //Player has a unit on o VP location
                    //VictoryPoints += cmdLib.VP_TacticalBanner;
                    gainVictoryPoint(toggLib.VP_TacticalBanner, unitsColl.unitsCounter.sel);
                    new VictoryPointEffect(unitsColl.unitsCounter.sel.squarePos);
                }
            }

            cmdRef.gamestate.playersStatusHUD.Refresh();
        }

        protected override void gainVictoryPoint(int points, AbsUnit collectingUnit)
        {
            VictoryPoints += points;
            if (collectingUnit != null && collectingUnit.progress != null)
            {
                collectingUnit.progress.VPCollected += points;
            }
        }

        public override void onDestroyedUnit(AbsUnit attacker, AbsUnit destroyedUnit)
        {
            base.onDestroyedUnit(attacker, destroyedUnit);
            cmdRef.gamestate.playersStatusHUD.Refresh();
        }

        virtual public void onOpenMenu(bool open)
        { }

        public bool TopPlayer
        {
            get { return !toggLib.BottomPlayer(pData.globalPlayerIndex); }
        }

        public bool BottomPlayer
        {
            get { return toggLib.BottomPlayer(pData.globalPlayerIndex); }
        }

        public void OnTurnStart()
        {
            //movementPhaseIsLocked = false;

            turnStartGenericSetup();

            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                unitsColl.unitsCounter.sel.OnTurnStart();
            }

            OnEvent(EventType.TurnStart, true, null);
        }

        public void OnEvent(EventType eventType, bool local, object tag)
        {
            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                unitsColl.unitsCounter.sel.OnEvent(eventType, local, tag); 
            }
        }

        public List<AbsUnit> GetUnits(UnitUnderType utype)
        {
            List<AbsUnit> result = new List<AbsUnit>(4);

            var counter = unitsColl.unitsCounter.IClone();
            while (counter.Next())
            {
                if (counter.GetSelection.cmd().data.underType == utype)
                {
                    result.Add(counter.GetSelection);
                }
            }

            return result;
        }

        public List<AbsUnit> GetUnits(UnitPropertyType property)
        {
            List<AbsUnit> result = new List<AbsUnit>(8);

            var counter = unitsColl.unitsCounter.IClone();
            while (counter.Next())
            {
                if (counter.GetSelection.HasProperty(property))
                {
                    result.Add(counter.GetSelection);
                }
            }

            return result;
        }

        public List<AbsUnit> GetUnits(UnitType type)
        {
            
            List<AbsUnit> result = new List<AbsUnit>(8);

            var counter = unitsColl.unitsCounter.IClone();
            while (counter.Next())
            {
                if (counter.GetSelection.cmd().data.Type == type)
                {
                    result.Add(counter.GetSelection);
                }
            }

            return result;
        }

        public bool unitSetupComplete = false;
        
        override public void nextPhaseButtonAction(bool forwardToNext) 
        {
            nextPhase(forwardToNext);
        }

        virtual public void nextPhase(bool forwardToNext) { }

        virtual public bool IsPassive { get { return false; } }

        public override bool IsLocal => throw new NotImplementedException();

        //virtual public bool IsLocalUser => false;
    }
}

    

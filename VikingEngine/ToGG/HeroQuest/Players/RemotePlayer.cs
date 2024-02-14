using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Players
{
    class RemotePlayer : AbsHQPlayer
    {
        public bool isReady = false;

        //Graphics.Image pointer;
        //Graphics.ImageAdvanced pointerGamerIcon;
        //Graphics.Image item;
        
        MoveLinesGroup movelines = null;
        RemotePlayerPointer pointer;

        public Net.WaitingRequests waitingRequests = new Net.WaitingRequests();
        AbsHeroInstance heroInstance;

        public RemotePlayer(Network.AbsNetworkPeer peer)
            : base(new Engine.RemotePlayerData(peer))
        {
            relationVisuals.faceRight = true;
            peer.Tag = this;
            //heroInstance = new AbsHeroInstance(this);

            pointer = new RemotePlayerPointer(peer, true);

            movelines = new MoveLinesGroup(null);
            movelines.setFocus(2);
        }

        override  public void update()
        {
            pointer.Update();
        }

        public void readNetUpdate(System.IO.BinaryReader r)
        {
            pointer.netRead(r);
            EightBit states = EightBit.FromStream(r);
            
            bool moveArrows = states.Get(0);
            if (moveArrows)
            {
                movelines.unit = Unit.NetReadUnitId(r);
                movelines.Read(r);
            }
            else
            {
                movelines.setFocus(2);
            }

            bool bAttackArrow = states.Get(1);
            if (bAttackArrow)
            {
                getAttackerArrow().updateAttackerArrow(HeroUnit, toggRef.board.ReadPosition(r));//SaveLib.ReadVector2(r));
            }
            else
            {
                removeAttackerArrow();
            }

            bool bItemDrag = states.Get(2);
            if (bItemDrag)
            {
                var itemType = Gadgets.AbsItem.ReadItem(r);
                pointer.item.SetSpriteName(itemType.Icon);
                pointer.item.Visible = true;
            }
            else
            {
                pointer.item.Visible = false;
            }

            visualState.read(r);
            if (nameDisplay != null)
            {
                nameDisplay.refreshVisualState(visualState);
            }

            HeroUnit?.netReadStatus(r);
        }

        public override void OnMapPan(Vector2 screenPan)
        {
            base.OnMapPan(screenPan);
            pointer.pointer.Position += screenPan;
        }

        public override void onNewUnit(AbsUnit unit)
        {
            base.onNewUnit(unit);

            var hero = unit.hq().data.hero;
            if (hero != null)
            {
                heroInstance = new AbsHeroInstance(unit.hq(), this);
                //heroInstances.Add(new HeroInstance(hero, this), false);
            }
        }

        public override void OnEvent(EventType eventType, object tag)
        {
            switch (eventType)
            {
                case EventType.TurnStart:
                    pointer.Visible = true;
                    break;
                case EventType.TurnEnd:
                    pointer.Visible = false;
                    break;
            }
            base.OnEvent(eventType, tag);
        }

        public override bool IsScenarioOpponent
        {
            get
            {
                return false;
            }
        }

        override public Gadgets.Backpack Backpack()
        {
            return heroInstance.backpack;
        }

        override public Unit HeroUnit
        {
            get
            {
                return heroInstance.heroUnit;
            }
        }

        public override AbsHeroInstance HeroInstance => heroInstance;

        override public bool IsHero { get { return true; } }

        public override bool LocalHumanPlayer => false;
        public override bool IsDungeonMaster => false;

        public override bool IsLocal => false;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.Players;
using VikingEngine.HUD;

namespace VikingEngine.LF2
{
    delegate void GadgetLinkEvent(GadgetLink gadget, HUD.AbsMenu menu);

    struct GadgetLink : HUD.IMenuLink
    {
        public GadgetLinkEvent LinkEvent;
        public GameObjects.Gadgets.GadgetsCollection Coll;
        public GameObjects.Gadgets.IGadget Gadget;
        public Players.Player player;
        public VikingEngine.LF2.GameObjects.Gadgets.MenuFilter menuFilter;

        //public EquipGadgetLinkEvent EquipLinkEvent;

        public Players.EquipSetup Setup;
        public Players.EquippedButtonSlot Button;
        public GameObjects.Gadgets.GadgetAlternativeUseType Use;
        public int Index;

        public GameObjects.Gadgets.IGadget PickGadget()
        {
            return PickGadget(1);
        }
        public GameObjects.Gadgets.IGadget PickGadget(int amount)
        {
            Gadget.StackAmount = lib.SmallestOfTwoValues(Gadget.StackAmount, amount);
            Coll.RemoveItem(Gadget);
            return Gadget;
        }
        
        //    public EquipGadgetLink(EquipGadgetLinkEvent EquipLinkEvent, Players.Player player)
        //    {
        //        this.LinkEvent = EquipLinkEvent;
        //        this.player = player;
        //    }
        //public GadgetLink()
        //    : this(null, null, null, null)
        //{

        //}
        public GadgetLink(GadgetLinkEvent linkEvent)
            : this(linkEvent, null, null, null)
        {
        }
        
        public GadgetLink(GadgetLinkEvent linkEvent, Players.Player player)
            :this(linkEvent, null, null, player)
        {
            
        }
        public GadgetLink(GadgetLinkEvent linkEvent, GameObjects.Gadgets.GadgetsCollection coll,
            GameObjects.Gadgets.IGadget gadget, Players.Player player)
        {
            this.LinkEvent = linkEvent;
            this.Coll = coll;
            this.Gadget = gadget;
            this.player = player;

            menuFilter = GameObjects.Gadgets.MenuFilter.All;
            Setup = null;
            Button = null;
            Use = GameObjects.Gadgets.GadgetAlternativeUseType.Standard;
            Index = 0;
        }

        public void TriggerEvent(int playerIndex, HUD.AbsMenu menu)
        {
            LinkEvent(this, menu);
        }

        public bool HasLink { get { return LinkEvent != null; } }

        public LinkType Type { get { return LinkType.Action; } }
        public int Value1 { get { throw new NotImplementedException(); } }
        public int Value2 { get { throw new NotImplementedException(); } }
        public int Value3 { get { throw new NotImplementedException(); } }
        public int Value4 { get { throw new NotImplementedException(); } }
    }

    //Button equip link2: 1: Item type, 2: Index, 3: UnderIndex, 4: setupIndex && buttonIx
    //delegate void EquipGadgetLinkEvent(GadgetLink link, HUD.AbsMenu menu);
    
    //class EquipGadgetLink : GadgetLink
    //{
    //    public EquipGadgetLinkEvent EquipLinkEvent;

    //    public Players.EquipSetup Setup;
    //    public Players.EquippedButtonSlot Button;
    //    public EquippedAlternativeUse Use;
    //    public int Index;

    //    public EquipGadgetLink()
    //    { }
    //    public EquipGadgetLink(EquipGadgetLinkEvent EquipLinkEvent)
    //    {
    //        this.LinkEvent = EquipLinkEvent;
    //    }
    //    public EquipGadgetLink(EquipGadgetLinkEvent EquipLinkEvent, Players.Player player)
    //    {
    //        this.LinkEvent = EquipLinkEvent;
    //        this.player = player;
    //    }

    //    override public void TriggerEvent(int playerIndex, HUD.AbsMenu menu)
    //    {
    //        EquipLinkEvent(this);
    //    }

    //    override public bool HasLink { get { return EquipLinkEvent != null; } }
    //}
}

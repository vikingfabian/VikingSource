//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using VikingEngine.HUD;

//namespace VikingEngine.LootFest.GO.NPC
//{
//    delegate void NpcMenuLinkEvent(Players.Player p, int value1);  

//    struct MenuLink : IMenuLink
//    {
//        public NpcMenuLinkEvent eventAction;
//        int value1;
//        Players.Player p;

//        public MenuLink(GO.PlayerCharacter.AbsHero h, NpcMenuLinkEvent action)
//            : this(h.Player, action, 0)
//        { }

//        public MenuLink(GO.PlayerCharacter.AbsHero h, NpcMenuLinkEvent action, int index)
//            : this(h.Player, action, index)
//        { }

//        public MenuLink(Players.Player p, NpcMenuLinkEvent action, int index)
//        {
//            this.p = p;    
//            eventAction = action;
//            this.value1 = index;
//        }

//        public int Value1 { get { return value1; } }
//        public int Value2 { get { throw new NotImplementedException(); } }
//        public int Value3 { get { throw new NotImplementedException(); } }
//        public int Value4 { get { throw new NotImplementedException(); } }
//        public bool HasLink { get { return eventAction != null; } }
//        public LinkType Type { get { return LinkType.Action; } }
//        public void TriggerEvent(int playerIndex, AbsMenu menu)
//        {
//            eventAction(p, value1);
//        }
//    }
    
//    delegate void NpcMenuLink2ArgsEvent(Players.Player p, int value1, int value2);

//    struct Menu2ArgsLink : IMenuLink
//    {
//        public NpcMenuLink2ArgsEvent eventAction;
//        Players.Player p;

//        public Menu2ArgsLink(Players.Player p, NpcMenuLink2ArgsEvent action, int value1, int value2)
//        {
//            this.p = p;
//            eventAction = action;
//            this.value1 = value1;
//            this.value2 = value2;
//        }

//        int value1;
//        int value2;

//        public int Value1 { get { return value1; } set { value1 = value; } }
//        public int Value2 { get { return value2; } set { value2 = value; } }
//        public int Value3 { get { throw new NotImplementedException(); } }
//        public int Value4 { get { throw new NotImplementedException(); } }

//        public bool HasLink { get { return eventAction != null; } }
//        public LinkType Type { get { return LinkType.Action; } }
//        public void TriggerEvent(int playerIndex, AbsMenu menu)
//        {
//            eventAction(p, value1, value2);
//        }
//    }

//    delegate void NpcMenuLink4ArgsEvent(Players.Player p, int value1, int value2, int value3, int value4);

//    struct Menu4ArgsLink : IMenuLink
//    {
//        public NpcMenuLink4ArgsEvent eventAction;
//        Players.Player p;

//        public Menu4ArgsLink(Players.Player p, NpcMenuLink4ArgsEvent action, int value1, int value2, int value3, int value4)
//        {
//            this.p = p;
//            eventAction = action;
//            this.value1 = value1;
//            this.value2 = value2;
//            this.value3 = value3;
//            this.value4 = value4;
//        }

//        int value1;
//        int value2;
//        int value3;
//        int value4;

//        public int Value1 { get { return value1; } set { value1 = value; } }
//        public int Value2 { get { return value2; } set { value2 = value; } }
//        public int Value3 { get { return value3; } }
//        public int Value4 { get { return value4; } }

//        public bool HasLink { get { return eventAction != null; } }
//        public LinkType Type { get { return LinkType.Action; } }
//        public void TriggerEvent(int playerIndex, AbsMenu menu)
//        {
//            eventAction(p, value1, value2, value3, value4);
//        }
//    }
//}

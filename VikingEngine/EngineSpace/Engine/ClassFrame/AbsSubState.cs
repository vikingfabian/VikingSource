//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine
//{
//    /// <summary>
//    /// A temporary gamestate that steals all input until exited
//    /// </summary>
//    abstract class AbsPlayerSubState : IDeleteable
//    {
//        protected Input.AbsControllerInstance controller;
//        protected Engine.PlayerData player;
//        protected AbsPlayerSubState previousSubState;
//        //protected LootFest.Menu menu;
//        //protected LootFest.File mFile;

//        /// <param name="previousSubState">if started from another substate, null if not</param>
//        public AbsPlayerSubState(int playerIx, AbsPlayerSubState previousSubState)
//        {
//            this.previousSubState = previousSubState;
//            player = Engine.XGuide.GetPlayer(playerIx);
//            controller = player.Controller;
//        }

//        /// <returns>Itself, returns previousSubState when exiting</returns>
//        abstract public AbsPlayerSubState Update();

//        abstract public bool IsDeleted
//        {
//            get;
//        }
//        abstract public void DeleteMe();
//    }
//}

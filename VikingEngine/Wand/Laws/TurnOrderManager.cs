using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.Wand.Map;
using VikingEngine.DebugExtensions;
using VikingEngine.EngineSpace;
using VikingEngine.LootFest.Map;

namespace VikingEngine.Core.Wand.Laws
{
    class TurnOrderManager
    {
        StructList<IntVector2> pathResult = new StructList<IntVector2>(128);
        public AStarPathfinder pathfinder = new AStarPathfinder();
        Time waitTime = Time.Zero;
        public TurnState turnState = 0;

        bool waitForAsyncProcess = false;

        public void MovePlayer(IntVector2 move)
        {
            if (move.HasValue())
            {
                if (move.X != 0 && move.Y != 0)
                {
                    move.Y = 0;
                }

                var toPos = WandRef.world.hero.currentPos.MovePos(move);
                if (WandRef.world.map.CanMove(WandRef.world.hero.currentPos.tilePos, toPos.tilePos, out var door))
                {
                    turnState = TurnState.BeginTurn;
                    WandRef.world.hero.SetPos(toPos);

                    if (door.wallType == Map.ThinWall.Door)
                    {
                        door.wallPos.RemoveWall();
                    }
                }
            }
        }

        public void update()
        {
            if (waitForAsyncProcess)
            {
                return;
            }

            if (turnState > TurnState.WaitingForInput)
            {
                if (waitTime.CountDown())
                {
                    switch (turnState)
                    {
                        case TurnState.BeginTurn:
                            waitTime = new Time(0.3f, TimeUnit.Seconds);
                            turnState = TurnState.AnimatingEnemies;
                            break;
                        case TurnState.AnimatingEnemies:
                           
                            MoveEnemies();
                            turnState++;
                            break;
                        case TurnState.NUM:
                            turnState = 0;
                            break;
                    }
                }
            }
        }

        public void MoveEnemies()
        {
            waitForAsyncProcess = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    var creaturesC = WandRef.world.creatures.counter();
                    while (creaturesC.Next())
                    {
                        if (pathfinder.FindPath(WandRef.world.map,
                             creaturesC.sel.currentPos.tilePos, WandRef.world.hero.currentPos.tilePos, ref pathResult) && pathResult.Count > 1)
                        {
                            creaturesC.sel.SetPos(creaturesC.sel.currentPos.MoveTo(pathResult.Array[1]));
                        }
                    }
                    waitForAsyncProcess = false;
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }

            });
        }
    }

    enum TurnState
    { 
        WaitingForInput,
        BeginTurn,
        AnimatingPlayer,
        AnimatingEnemies,
        NUM
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;

namespace VikingEngine.Core.BlackBolts.Laws
{
    class RunExecuter
    {
        RunStep step = RunStep.CalcMove;
        float moveTween = 0;

        float MoveTime = 600;
        float HoldTime = 200;
        float time = 0;

        public void Start()
        { 
            
        }

        void resetTime()
        {
            moveTween = 0;
            time = 0;
        }

        public void Update() 
        {
            switch (step)
            {
                case RunStep.CalcMove:
                    {
                        //Reserve next position
                        var creaturesC = BlackRef.mapData.creatureList.counter();
                        while (creaturesC.Next())
                        {
                            var next = creaturesC.sel.currentPos.ForwardPos();
                            var ToTile = BlackRef.mapData.GetTile(next.tilePos);
                            if (ToTile.tileType == Map.TileType.Floor)
                            {
                                creaturesC.sel.nextPos = next;
                            }
                            else
                            {
                                creaturesC.sel.nextPos = creaturesC.sel.currentPos.TurnAroundPos();
                            }

                        }

                        //Clear map
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            BlackRef.mapData.tileGrid.array[i].nextPosList.Clear();
                        }

                        //Find colliding Objects
                        checkDirectWalkColl();

                        creaturesC.Reset();
                        while (creaturesC.Next())
                        {
                            BlackRef.mapData.tileGrid.Get(creaturesC.sel.nextPos.tilePos).nextPosList.Add(creaturesC.sel.pointer);
                        }

                        //Collide all objects moving to the same tile
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            if (BlackRef.mapData.tileGrid.array[i].nextPosList.Count > 1)
                            {
                                foreach (var pobj in BlackRef.mapData.tileGrid.array[i].nextPosList)
                                {
                                    var gameobject = pobj.Get();
                                    if (!gameobject.NoMovement())
                                    {
                                        gameobject.nextPos = gameobject.currentPos.TurnAroundPos();
                                    }
                                }
                            }
                        }

                        checkDirectWalkColl();

                        //Clear map
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            BlackRef.mapData.tileGrid.array[i].nextPosList.Clear();
                        }

                        //#Make belt moves
                        creaturesC.Reset();
                        while (creaturesC.Next())
                        {
                            creaturesC.sel.hasBeltMove = false;

                            var pMachine = BlackRef.mapData.tileGrid.Get(creaturesC.sel.nextPos.tilePos).pMachine;
                            if (pMachine.hasValue)
                            {
                                Belt machine = pMachine.GetStaticItem();
                                var moveTo = machine.currentPos.ForwardPos();

                                var toTile = BlackRef.mapData.tileGrid.Get(moveTo.tilePos);
                                //Quick check for impossible move
                                if (toTile.tileType != Map.TileType.Wall && !HasOpposingBelt(machine))
                                {
                                    creaturesC.sel.beltPos = new MapPlacement(moveTo.tilePos, creaturesC.sel.nextPos.direction);
                                    creaturesC.sel.hasBeltMove = true;
                                    toTile.nextPosList.Add(creaturesC.sel.pointer);
                                }
                            }

                            if (!creaturesC.sel.hasBeltMove)
                            {
                                BlackRef.mapData.tileGrid.Get(creaturesC.sel.nextPos.tilePos).nextPosList.Add(creaturesC.sel.pointer);
                            }
                        }

                        //Check belt move collisions
                        //Belt push is weak and will just stop if it collides with another unit
                        creaturesC.Reset();
                        while (creaturesC.Next())
                        {
                            if (creaturesC.sel.hasBeltMove)
                            {
                                var toTile = BlackRef.mapData.tileGrid.Get(creaturesC.sel.beltPos.tilePos);
                                if (toTile.nextPosList.Count > 1)
                                {
                                    creaturesC.sel.hasBeltMove = false;
                                }
                            }

                            if (creaturesC.sel.hasBeltMove)
                            {
                                creaturesC.sel.nextPos = creaturesC.sel.beltPos;
                            }
                        }

                        step++;
                    }
                    break;

                case RunStep.BeginMove:
                    resetTime();
                    step++;
                    break;

                case RunStep.AllMove:
                    {
                        time += Ref.DeltaGameTimeMs;
                        moveTween = time / MoveTime;
                        if (moveTween >= 1f)
                        {
                            moveTween = 1f;
                            step++;
                        }

                        var creaturesC = BlackRef.mapData.creatureList.counter();
                        while (creaturesC.Next())
                        {
                            creaturesC.sel.TweenUpdate(moveTween);
                        }

                        var itemsC = BlackRef.mapData.staticObjectList.counter();
                        while (itemsC.Next())
                        {
                            itemsC.sel.AnimateUpdate();
                        }
                    }
                    break;

                case RunStep.BeginHold:
                    {
                        //Clear map
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            BlackRef.mapData.tileGrid.array[i].pCreature = ObjectPointer.Empty;
                        }

                        //Complete all moves and refill map
                        var objectsC = BlackRef.mapData.creatureList.counter();
                        while (objectsC.Next())
                        {
                            objectsC.sel.FinalizeMove();

                        }
                        time = 0;
                        step++;
                    }
                    break;
                case RunStep.Hold:
                    time += Ref.DeltaGameTimeMs;
                    if (time >= HoldTime)
                    {
                        step = 0;
                    }
                    break;
            }
        }

        static bool HasOpposingBelt(Belt machine)
        {
            var moveTo = machine.currentPos.ForwardPos();
            var toTile = BlackRef.mapData.tileGrid.Get(moveTo.tilePos);
            if (toTile.pMachine.hasValue)
            {
                Belt otherMachine = toTile.pMachine.GetStaticItem();
                if (otherMachine.currentPos.ForwardPos().tilePos == machine.currentPos.tilePos)
                {
                    return true;
                }
            }

            return false;
        }

        private static void checkDirectWalkColl()
        {
            bool hasCollision = true;

            while (hasCollision)
            {
                hasCollision = false;
                var objectsC = BlackRef.mapData.creatureList.counter();
                while (objectsC.Next())
                {
                    //Moving directly into another unit check
                    if (!objectsC.sel.NoMovement() && BlackRef.mapData.tileGrid.Get(objectsC.sel.nextPos.tilePos).pCreature.hasValue)
                    {
                        //Is the other unit moving away?
                        var otherObj = BlackRef.mapData.tileGrid.Get(objectsC.sel.nextPos.tilePos).pCreature.Get();
                        if (otherObj.NoMovement() || otherObj.nextPos.tilePos == objectsC.sel.currentPos.tilePos)
                        {
                            //No moving into it
                            objectsC.sel.nextPos = objectsC.sel.currentPos.TurnAroundPos();
                            hasCollision = true;
                        }
                    }


                }
            }
        }
    }

    enum RunStep
    { 
        CalcMove,

        BeginMove,
        AllMove,

        BeginHold,
        Hold,

        NUM
    }
}

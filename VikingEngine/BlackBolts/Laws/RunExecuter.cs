using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
                        var objectsC = BlackRef.mapData.worldObjects.counter();
                        while (objectsC.Next())
                        {
                            var next = objectsC.sel.currentPos.ForwardPos();
                            var ToTile = BlackRef.mapData.GetTile(next.tilePos);
                            if (ToTile.tileType == Map.TileType.Floor)
                            {
                                objectsC.sel.nextPos = next;
                            }
                            else
                            {
                                objectsC.sel.nextPos = objectsC.sel.currentPos.TurnAroundPos();
                            }

                        }

                        //Clear map
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            BlackRef.mapData.tileGrid.array[i].nextPosList.Clear();
                        }

                        //Find colliding Objects
                        checkDirectWalkColl();

                        objectsC.Reset();
                        while (objectsC.Next())
                        {
                            BlackRef.mapData.tileGrid.Get(objectsC.sel.nextPos.tilePos).nextPosList.Add(objectsC.sel.pointer);
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

                        var objectsC = BlackRef.mapData.worldObjects.counter();
                        while (objectsC.Next())
                        {
                            objectsC.sel.TweenUpdate(moveTween);
                        }
                    }
                    break;

                case RunStep.BeginHold:
                    {
                        //Clear map
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            BlackRef.mapData.tileGrid.array[i].gameobject = ObjectPointer.Empty;
                        }

                        //Complete all moves and refill map
                        var objectsC = BlackRef.mapData.worldObjects.counter();
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

        private static void checkDirectWalkColl()
        {
            bool hasCollision = true;

            while (hasCollision)
            {
                hasCollision = false;
                var objectsC = BlackRef.mapData.worldObjects.counter();
                while (objectsC.Next())
                {
                    //Moving directly into another unit check
                    if (!objectsC.sel.NoMovement() && BlackRef.mapData.tileGrid.Get(objectsC.sel.nextPos.tilePos).gameobject.hasValue)
                    {
                        //Is the other unit moving away?
                        var otherObj = BlackRef.mapData.tileGrid.Get(objectsC.sel.nextPos.tilePos).gameobject.Get();
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

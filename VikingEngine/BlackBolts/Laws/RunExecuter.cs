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

        public const float MoveTime = 500;
        //float HoldTime = 200;
        float time = 0;

        public void Start()
        {
            var creaturesC = BlackRef.mapData.creatureList.counter();
            while (creaturesC.Next())
            {
                creaturesC.sel.refreshAnimation();
            }
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
                            creaturesC.sel.lockItem = false;

                            var next = creaturesC.sel.currentPos.ForwardPos();
                            var ToTile = BlackRef.mapData.GetTile(next.tilePos);
                            if (TileIsWalkable(ToTile))
                            {
                                creaturesC.sel.nextPos = next;
                            }
                            else
                            {
                                CheckPickUpEvent(creaturesC.sel, ToTile);
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
                            var posList = BlackRef.mapData.tileGrid.array[i].nextPosList;
                            if (posList.Count > 1)
                            {
                                //Check item trade
                                for (int posIx =0; posIx < posList.Count; ++posIx)
                                {
                                    var creature1 = posList[posIx].GetCreature();
                                    for (int otherposIx = posIx +1; otherposIx < posList.Count; ++otherposIx)
                                    {
                                        var creature2 = posList[otherposIx].GetCreature();
                                        checkItemTrade(creature1, creature2);
                                    }
                                }

                                //Turn around
                                foreach (var pobj in posList)
                                {
                                    var gameobject = pobj.GetCreature();
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
                        updateMove();
                    }
                    break;

                case RunStep.BeginBelt:
                    {
                        finalizeAllMoves();

                        //Clear map
                        for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
                        {
                            BlackRef.mapData.tileGrid.array[i].nextPosList.Clear();
                        }

                        var creaturesC = BlackRef.mapData.creatureList.counter();
                        //Calc all belt moves
                        creaturesC.Reset();
                        while (creaturesC.Next())
                        {
                            creaturesC.sel.hasBeltMove = false;

                            var pMachine = BlackRef.mapData.tileGrid.Get(creaturesC.sel.nextPos.tilePos).pMachine;
                            if (pMachine.hasValue)
                            {
                                Belt belt = pMachine.GetMachine() as Belt;
                                if (belt != null)
                                {
                                    var moveTo = belt.currentPos.ForwardPos();

                                    var toTile = BlackRef.mapData.tileGrid.Get(moveTo.tilePos);
                                    //Quick check for impossible move
                                    if (TileIsWalkable(toTile) && !HasOpposingBelt(belt))
                                    {
                                        creaturesC.sel.beltPos = new MapPlacement(moveTo.tilePos, creaturesC.sel.nextPos.direction);
                                        creaturesC.sel.hasBeltMove = true;
                                        toTile.nextPosList.Add(creaturesC.sel.pointer);
                                    }
                                }
                            }
                        }

                        //Check belt move collisions
                        //Belt push is weak and will just stop if it collides with another unit
                        creaturesC.Reset();
                        while (creaturesC.Next())
                        {
                            if (creaturesC.sel.hasBeltMove)
                            {
                                Tile toTile = BlackRef.mapData.tileGrid.Get(creaturesC.sel.beltPos.tilePos);
                                if (toTile.nextPosList.Count > 1 || HasStaticCreature_belts(toTile))
                                {
                                    creaturesC.sel.hasBeltMove = false;
                                }
                            }

                            if (creaturesC.sel.hasBeltMove)
                            {
                                creaturesC.sel.nextPos = creaturesC.sel.beltPos;
                            }
                        }

                        //Apply rotators
                        creaturesC.Reset();
                        while (creaturesC.Next())
                        {
                            Tile toTile = BlackRef.mapData.tileGrid.Get(creaturesC.sel.nextPos.tilePos);
                            if (toTile.pMachine.hasValue)
                            {
                                var spin = toTile.pMachine.GetMachine() as SpinPlate;
                                if (spin != null)
                                {
                                    creaturesC.sel.nextPos.Rotate(spin.rotateDir);
                                    creaturesC.sel.hasBeltMove = true;
                                }
                            }
                        }

                        resetTime();
                        step++;
                    }
                    break;
                case RunStep.RunBelts:
                    //time += Ref.DeltaGameTimeMs;
                    //if (time >= HoldTime)
                    //{
                    //    step = 0;
                    //}
                    {
                        updateMove();
                    }
                    break;

                case RunStep.FinalizeSteps:
                    finalizeAllMoves();
                    var mashinesC = BlackRef.mapData.machineList.counter();
                    while (mashinesC.Next())
                    {
                        mashinesC.sel.OnCykleEnd();
                    }
                    step = 0;
                    break;
            }
        }

        void checkItemTrade(Worker creature1, Worker creature2)
        {
            //Must face each other to trade items
            if (lib.OppositeDir(creature1.nextPos.direction) == creature2.nextPos.direction &&
                !creature1.lockItem && !creature2.lockItem)
            {
                var returnItem = creature2.HandoverItem(creature1.pResource);
                creature1.pResource = ObjectPointer.Empty;

                creature1.HandoverItem(returnItem);

                //if (creature1.pResource.hasValue)
                //{ 
                    creature1.lockItem = true;
                //}
                //if (creature2.pResource.hasValue)
                //{
                    creature2.lockItem = true;
                //}
            }
        }

        private bool TileIsWalkable(Tile ToTile)
        {
            if ( ToTile.tileType == Map.TileType.Wall)
                return false;

            if (ToTile.pMachine.hasValue)
            {
                return ToTile.pMachine.GetMachine().WalkableTile();
            }

            return true;
        }

        private void CheckPickUpEvent(Worker creature, Tile ToTile)
        {
            if (ToTile.pMachine.hasValue)
            {
                var machine = ToTile.pMachine.GetMachine();
                machine.ItemHandle(out bool mayPick, out bool mayDrop);

                if (creature.pResource.hasValue && mayDrop)
                {
                    ObjectPointer res = creature.pResource;
                    creature.pResource = ObjectPointer.Empty;

                    var returnItem = machine.HandoverItem(res);
                    if (returnItem.hasValue)
                    {
                        creature.HandoverItem(returnItem);
                    }
                }
                else if (mayPick && machine.pResource.hasValue && !creature.pResource.hasValue)
                {
                    creature.HandoverItem(machine.pResource);
                    machine.pResource = ObjectPointer.Empty;
                }
            }
        }

        private void finalizeAllMoves()
        {
            //Clear map
            for (int i = 0; i < BlackRef.mapData.tileGrid.array.Length; ++i)
            {
                BlackRef.mapData.tileGrid.array[i].pCreature = ObjectPointer.Empty;
            }

            //Complete all moves and refill map
            var creaturesC = BlackRef.mapData.creatureList.counter();
            while (creaturesC.Next())
            {
                creaturesC.sel.FinalizeMove();

            }
        }

        private void updateMove()
        {
            bool beltMove = step == RunStep.RunBelts;

            time += Ref.DeltaGameTimeMs;
            moveTween = time / MoveTime;
            if (moveTween >= 1f)
            {
                moveTween = 1f;
                step++;
                //if (step >= RunStep.NUM)
                //{
                //    step = 0;
                //}
            }

            var creaturesC = BlackRef.mapData.creatureList.counter();
            while (creaturesC.Next())
            {
                creaturesC.sel.TweenUpdate(beltMove, moveTween);
            }

            if (beltMove)
            {
                var itemsC = BlackRef.mapData.machineList.counter();
                while (itemsC.Next())
                {
                    itemsC.sel.AnimateUpdate();
                }
            }
        }

        static bool HasStaticCreature_belts(Tile tile)
        {
            if (tile.pCreature.hasValue)
            {
                return !tile.pCreature.GetCreature().hasBeltMove;
            }
            return false;
        }

        static bool HasOpposingBelt(Belt machine)
        {
            var moveTo = machine.currentPos.ForwardPos();
            var toTile = BlackRef.mapData.tileGrid.Get(moveTo.tilePos);
            if (toTile.pMachine.hasValue)
            {
                Belt otherMachine = toTile.pMachine.GetMachine() as Belt;
                if (otherMachine != null)
                {
                    if (otherMachine.currentPos.ForwardPos().tilePos == machine.currentPos.tilePos)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void checkDirectWalkColl()
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
                        var otherObj = BlackRef.mapData.tileGrid.Get(objectsC.sel.nextPos.tilePos).pCreature.GetCreature();
                        if (otherObj.NoMovement() || otherObj.nextPos.tilePos == objectsC.sel.currentPos.tilePos)
                        {
                            checkItemTrade(objectsC.sel, otherObj);

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

        BeginBelt,
        RunBelts,

        FinalizeSteps,
        NUM
    }
}

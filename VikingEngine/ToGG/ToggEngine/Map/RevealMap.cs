using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class RevealMap : AbsQuedTasks
    {
        List<IntVector2> revealedSquares = new List<IntVector2>(64);
        IntVector2 playerPos;
        bool animate;

        public RevealMap(IntVector2 playerPos, bool animate)
            :base(QuedTasksType.QueAndSynch, false)
        {
            this.playerPos = playerPos;
            this.animate = animate;

            beginAutoTasksRun();           
        }

        protected override void runQuedAsynchTask()
        {
            base.runQuedAsynchTask();
            
            checkAroundSquare(playerPos, revealedSquares);
        }

        void checkAroundSquare(IntVector2 center, List<IntVector2> revealedSquares)
        {
            Rectangle2 area = Rectangle2.FromCenterTileAndRadius(center, 1);

            BoardSquareContent sq;
            ForXYLoop loop = new ForXYLoop(area);

            while (loop.Next())
            {
                if (toggRef.board.tileGrid.TryGet(loop.Position, out sq))
                {
                    if (sq.hidden)
                    {
                        sq.hidden = false;
                        revealedSquares.Add(loop.Position);

                        if (loop.Position != center)
                        {
                            bool wallOff = sq.IsRoomDivider();

                            if (wallOff)
                            {
                                //Check for inside wall fog
                                checkInsideWallFog(loop.Position, revealedSquares);
                            }
                            else
                            {
                                checkAroundSquare(loop.Position, revealedSquares);
                            }
                        }
                    }
                }
            }
        }

        void checkInsideWallFog(IntVector2 center, List<IntVector2> revealedSquares)
        {
            foreach (var dir in IntVector2.Dir4Array)
            {
                IntVector2 npos = center + dir;
                if (squareInsideWall(npos))
                {
                    toggRef.board.tileGrid.Get(npos).hidden = false;
                    revealedSquares.Add(npos);

                    checkInsideWallFog(npos, revealedSquares);
                }
            }
        }

        bool squareInsideWall(IntVector2 pos)
        {
            BoardSquareContent sq;

            if (toggRef.board.tileGrid.TryGet(pos, out sq))
            {
                if (sq.hidden)
                {
                    foreach (var dir in IntVector2.Dir8Array)
                    {
                        if (toggRef.board.tileGrid.TryGet(pos + dir, out sq))
                        {
                            if (!sq.IsWall)
                            {
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }
            return false;
        }

        public override void runSyncAction()
        {
            base.runSyncAction();

            if (revealedSquares.Count > 0)
            {
                toggRef.board.fogModel.refresh();

                if (animate)
                {
                    foreach (var m in revealedSquares)
                    {
                        new AnimateFogSquare(m, playerPos);
                    }
                }
            }
        }
    }
}

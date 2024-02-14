using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Match3
{
    class RemoveBlocksEffect
    {
        public List<Brick> bricks = new List<Brick>(16);
        public List<Brick> stones = new List<Brick>(16);
        bool visible = true;
        int flashes = 0;
        Time nextFlashTimer = 0;

        public RemoveBlocksEffect(MatchCollection matches, BrickBox box)
        {
            foreach (var m in matches.matches)
            {
                foreach (var b in m.bricks)
                {
                    arraylib.ListAddIfNotExist(bricks, b);
                }
            }

            foreach (var b in bricks)
            {
                foreach (var dir in IntVector2.Dir4Array)
                {
                    Brick adjBrick;
                    if (box.grid.TryGet(dir + b.gridPos, out adjBrick))
                    {
                        if (adjBrick != null && adjBrick.color == BrickColor.Stone)
                        {
                            arraylib.ListAddIfNotExist(stones, adjBrick);
                        }
                    }
                }
            }
        }

        public bool update()
        {
            if (nextFlashTimer.CountDown())
            {
                lib.Invert(ref visible);
                
                if (++flashes >= 4)
                {
                    foreach (var m in bricks)
                    {
                        m.DeleteMe();
                    }
                    foreach (var m in stones)
                    {
                        m.DeleteMe();
                    }
                    return true;
                }
                else
                {
                    foreach (var m in bricks)
                    {
                        m.images.SetVisible(visible);
                    }

                    nextFlashTimer.MilliSeconds = 90;
                }
            }

            return false;
        }
    }
}

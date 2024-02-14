using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class BrickBox
    {
        static readonly Color BoxLightCol = new Color(111,115,130);//48, 96, 130);
        static readonly Color BoxDarkCol = new Color(80, 88, 108);

        public static readonly IntVector2 BrickCountSz = new IntVector2(8, 16);
        public VectorRect area;
        public Vector2 gridStart;

        public Gamer gamer;
        RemoveBlocksEffect removeEffect = null;
        public List2<Brick> activeBricks = new List2<Brick>(64);
        public Grid2D<Brick> grid;        
        bool brickLandings = false;
        public int comboCount = 0;
        public PcgRandom rnd;
        public int index;

        public BrickBox(int index, Vector2 topLeft)
        {
            this.index = index;
            grid = new Grid2D<Brick>(BrickCountSz);
            area = new VectorRect(topLeft, BrickCountSz.Vec * m3Ref.TileWidth);
            gridStart = topLeft + new Vector2(m3Ref.TileWidth * 0.5f);

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea,
                area.Position, area.Size, m3Lib.LayerBox);
            bg.Color = BoxLightCol;
            bg.Opacity = 0.9f;

            ForXYLoop loop = new ForXYLoop(BrickCountSz);
            while (loop.Next())
            {
                VectorRect brickArea = new VectorRect(topLeft + loop.Position.Vec * m3Ref.TileWidth, new Vector2(m3Ref.TileWidth));

                brickArea.AddPercentRadius(-0.1f);
                Graphics.Image brickBg = new Graphics.Image(SpriteName.WhiteArea, brickArea.Position, brickArea.Size, m3Lib.LayerBox-1);
                brickBg.Color = BoxDarkCol;
                brickBg.Opacity = 0.8f;
            }

            rnd = new PcgRandom(m3Ref.gamestate.seed);
        }

        public bool IsActive
        {
            get
            {
                return removeEffect != null || 
                    activeBricks.Count > 0 || 
                    brickLandings;
            }
        }

        public void update()
        {
            if (removeEffect != null)
            {
                if (removeEffect.update())
                {
                    removeEffect = null;

                    checkForHoles();

                    if (activeBricks.Count == 0)
                    {
                        comboCount = 0;
                    }
                    else
                    {
                        comboCount++;
                    }
                }
            }
            else if (activeBricks.Count > 0)
            {
                bool landSound = false;
                activeBricks.loopBegin();
                while(activeBricks.loopNext())//for (int i = activeBricks.Count - 1; i >= 0; --i)
                {
                    if (activeBricks.sel.update() == false)
                    {
                        activeBricks.sel.setInGrid();
                        //m3Ref.sounds.fallClonk.Play(activeBricks.selected.images.ParentPosition);
                        activeBricks.loopRemove();
                        brickLandings = true;
                        landSound = true;
                    }
                }

                if (landSound)
                {
                    m3Ref.sounds.fallClonk.Play(area.Center);
                }
            }
            else if (brickLandings)
            {
                brickLandings = false;
                var matches = new MatchCollection(this);
                if (matches.GotMatch)
                {
                    removeEffect = new RemoveBlocksEffect(matches, this);
                    matches.orderLengthLowToHigh();

                    var score = new PointScoring(matches, comboCount);
                    gamer.onScore(score);

                    if (comboCount == 0)
                    {
                        m3Ref.sounds.clear.Play(area.Center);
                    }
                    else
                    {
                        m3Ref.sounds.clearCombo.Play(area.Center);
                    }
                }
            }

            
        }

        void checkForHoles()
        {
            IntVector2 pos = IntVector2.Zero;
            for (pos.Y = BrickCountSz.Y - 2; pos.Y >= 0; --pos.Y)
            {
                for (pos.X = 0; pos.X < BrickCountSz.X; ++pos.X)
                {
                    Brick b = grid.Get(pos);
                    if (b != null)
                    {
                        if (grid.Get(VectorExt.AddY(b.gridPos, 1)) == null)
                        {
                            //Start falling
                            b.setAsFalling();
                        }
                    }
                }
            }
        }

        public void addFallingBrick(Brick brick)
        {
            if (brick.gridPos.Y == 0)
            {
                activeBricks.Insert(0, brick);
                return;
            }

            activeBricks.loopBegin(false);
            while (activeBricks.loopNext())// for (int i = activeBricks.Count - 1; i >= 0; --i)
            {
                if (brick.gridPos.Y <= activeBricks.sel.gridPos.Y)
                {
                    activeBricks.Insert(activeBricks.selIndex, brick);
                    return;
                }
            }

            activeBricks.Add(brick);
        }
        
        public void remove(Brick brick)
        {
            if (grid.InBounds(brick.gridPos))
            {
                if (grid.Get(brick.gridPos) == brick)
                {
                    grid.Set(brick.gridPos, null);
                }
            }
        }

        public Vector2 worldPos(IntVector2 gridPos)
        {
            return gridStart + gridPos.Vec * m3Ref.TileWidth;
        }

        public Brick get(IntVector2 pos, out bool bottomBound, out bool sideBounds)
        {
            bottomBound = pos.Y < BrickCountSz.Y;
            sideBounds = pos.X >= 0 && pos.X < BrickCountSz.X;
            if (bottomBound && sideBounds && pos.Y >= 0)
            {
                return grid.Get(pos);
            }
            return null;
        }

        public IntVector2 getTopFreePos(IntVector2 pos)
        {
            for (pos.Y = 0; pos.Y < BrickBox.BrickCountSz.Y; ++pos.Y)
            {
                if (grid.Get(pos) != null)
                {
                    break;
                }
            }

            pos.Y -= 1;
            return pos;
        }

        public bool isBlocked(IntVector2 pos)
        {
            bool bottomBound, sideBounds;
            var brick = get(pos, out bottomBound, out sideBounds);

            return brick != null || !bottomBound;
        }

        public bool hasBrick(IntVector2 pos)
        {
            Brick b;
            if (grid.TryGet(pos, out b))
            {
                return b != null;
            }
            return false;
        }

        public void debug_fill()
        {
            grid.LoopBegin();
            while (grid.LoopNext())
            {
                if (grid.LoopValueGet() == null)
                {
                    var b = new Brick(BrickColor.Stone);
                    b.box = this;
                    b.gridPos = grid.LoopPosition;
                    b.setInGrid();
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class FallingBlock
    {
        BrickBox box;
        List<FallingBlockMember> bricks = new List<FallingBlockMember>(3);
        Vector2 center;
        FallingState state = FallingState.TopSliding;
        int topSlidingXpos = -1;
        int topSlidingDir = 1;
       
        IntVector2 gridPos;
        float fallSpeedMultiplier;
        BrickMover brickMover;
        PreviewMarker preview;

        Graphics.ImageGroupParent2D inputDisplayDown, inputDisplayRotate = null;
        public int rotationCount = 0;

        public FallingBlock(BrickBox box)
        {
            this.box = box;
            bricks.Add(new FallingBlockMember(new Brick(m3Ref.gamestate.RndColor(box.rnd)), new IntVector2(-1, 0)));
            bricks.Add(new FallingBlockMember(new Brick(m3Ref.gamestate.RndColor(box.rnd)), new IntVector2(0, 0)));
            bricks.Add(new FallingBlockMember(new Brick(m3Ref.gamestate.RndColor(box.rnd)), new IntVector2(1, 0)));

            fallSpeedMultiplier = m3Ref.gamestate.fallSpeedMultiplier;
            preview = new PreviewMarker(box, bricks);
            updatePosition();
        }
        
        public bool update(Gamer gamer)
        {
            bool tap = gamer.gamerdata.button.DownEvent;

            if (state == FallingState.TopSliding)
            {
                if (gamer.gamerdata.button.DownEvent)
                {
                    if (topSlidingXpos >= 0 && topSlidingXpos < BrickBox.BrickCountSz.X)
                    {
                        //PULL BLOCK
                        gridPos = new IntVector2(topSlidingXpos, -1);
                        brickMover = new BrickMover();
                        brickMover.fallSpeedMultiplier = fallSpeedMultiplier;

                        foreach (var m in bricks)
                        {
                            m.createOutline();
                            m.brick.box = box;
                        }
                        //gamer.onBlockPull();
                        state = FallingState.Falling;
                        tap = false;

                        
                        if (checkGroundCollision(gamer)) //can turn to end state immediedly 
                        {
                            return true;
                        }
                        gridPos.Y++;
                    }
                }

                
                if (m3Ref.gamestate.blockMoveUpdate)//nextTopSlidePos >= 1f)
                {
                    //nextTopSlidePos -= 1f;
                    m3Ref.gamestate.topMoveHappened = true;
                    topSlidingXpos += topSlidingDir;
                    

                    if (topSlidingXpos >= BrickBox.BrickCountSz.X)
                    {
                        topSlidingXpos = BrickBox.BrickCountSz.X - 2;
                        topSlidingDir = -1;
                    }
                    updatePosition();

                    if (topSlidingXpos < 0)
                    {
                        gamer.onBlockMiss();
                        return true;
                    }
                }
            }

            if (state == FallingState.Falling)
            {
                if (tap)
                {
                    bool canRotate = true;
                    foreach (var m in bricks)
                    {
                        canRotate &= m.canRotate(box, gridPos);
                    }

                    if (canRotate)
                    {
                        rotationCount++;

                        foreach (var m in bricks)
                        {
                            m.rotate();
                        }
                        brickMover.onRotate();
                        
                        m3Ref.sounds.rotate.Play(center);
                        checkGroundCollision(gamer);
                    }
                    else
                    {
                        onGroundColl();
                    }
                }

                if (brickMover.update(ref gridPos))
                {
                    checkGroundCollision(gamer);
                }

                updatePosition();
            }
            

            return state == FallingState.GroundCollision;
        }

        bool checkGroundCollision(Gamer gamer)
        {
            foreach (var m in bricks)
            {
                m.brick.gridPos = gridPos + m.offset;
                if (m.brick.groundCollision())
                {
                    onGroundColl();
                    gamer.hasDroppedBlock = true;
                    return true;
                }
            }

            return false;
        }

        void onGroundColl()
        {
            state = FallingState.GroundCollision;
            
            foreach (var m in bricks)
            {
                m.brick.setAsFalling();
            }
        }
        

        void updatePosition()
        {
            if (state != FallingState.GroundCollision)
            {
                IntVector2 gridCenter = IntVector2.Zero;
                center = box.gridStart;

                if (state == FallingState.TopSliding)
                {
                    gridCenter = new IntVector2(topSlidingXpos, -1);
                    center.X += topSlidingXpos * m3Ref.TileWidth;
                    center.Y -= m3Ref.TileWidth;
                    refreshDownInput(center);
                }
                else
                {
                    inputDisplayDown?.DeleteMe();
                    inputDisplayDown = null;
                }

                if (state == FallingState.Falling)
                {
                    gridCenter = gridPos;
                    center = box.worldPos(gridPos);
                    refreshRotateInput(center);
                }
                else
                {
                    inputDisplayRotate?.DeleteMe();
                    inputDisplayRotate = null;
                }
                

                FindMinValue minVal = new FindMinValue(false);
                foreach (var m in bricks)
                {
                    m.updateGridPos(gridCenter);
                    m.updatePosition(center);
                    minVal.Next(m.brick.gridPos.X, 0);
                }

                refreshEndMarker();
            }
        }

        private void refreshRotateInput(Vector2 center)
        {
            if (box.gamer.viewRotationInput > 0)
            {
                if (inputDisplayRotate == null)
                {
                    Vector2 inputcenter = new Vector2(
                        lib.BoolToLeftRight(gridPos.X < BrickBox.BrickCountSz.X / 2) * 3f * m3Ref.TileWidth,
                        0);

                    Graphics.ImageAdvanced button;
                    Graphics.Image controllerIcon;

                    LobbyAvatar.InputIconAndIndex(box.gamer.gamerdata,
                        inputcenter,
                        Engine.Screen.SmallIconSizeV2,
                        Graphics.GraphicsLib.ToPaintLayer(m3Lib.LayerInput), out button, out controllerIcon);

                    Graphics.Image arrow = new Graphics.Image(SpriteName.m3RotateArrow,
                        inputcenter, button.size * 4f,
                        m3Lib.LayerInput + 1, true);

                    inputDisplayRotate = new Graphics.ImageGroupParent2D(arrow, button);
                    if (controllerIcon != null)
                    {
                        inputDisplayRotate.Add(controllerIcon);
                    }
                }

                inputDisplayRotate.ParentPosition = center;
            }
        }

        private void refreshDownInput(Vector2 center)
        {
            if (inputDisplayDown == null)
            {
                Graphics.Image arrowDown = new Graphics.Image(SpriteName.FatArrow16pix,
                    new Vector2(0, Engine.Screen.IconSize * 0.6f), Engine.Screen.IconSizeV2 * 0.5f,
                    m3Lib.LayerInput, true);
                arrowDown.Rotation = MathExt.TauOver2;

                Graphics.ImageAdvanced button;
                Graphics.Image controllerIcon;

                LobbyAvatar.InputIconAndIndex(box.gamer.gamerdata,
                    VectorExt.AddY(arrowDown.position, Engine.Screen.IconSize * 0.6f),
                    Engine.Screen.SmallIconSizeV2,
                    Graphics.GraphicsLib.ToPaintLayer(m3Lib.LayerInput), out button, out controllerIcon);

                inputDisplayDown = new Graphics.ImageGroupParent2D(arrowDown, button);
                if (controllerIcon != null)
                {
                    inputDisplayDown.Add(controllerIcon);
                }
            }

            inputDisplayDown.ParentPosition = center;
        }

        void refreshEndMarker()
        {
            if (topSlidingXpos >= 0 && topSlidingXpos < BrickBox.BrickCountSz.X)
            {
                preview.update(bricks);
            }
            else
            {
                preview.DeleteMe();
            }
        }

        public void DeleteMe()
        {
            preview.DeleteMe();
            inputDisplayDown?.DeleteMe();
            inputDisplayRotate?.DeleteMe();
            if (state == FallingState.TopSliding)
            {
                foreach (var m in bricks)
                {
                    m.DeleteMe();
                }
            }
            else
            {
                foreach (var m in bricks)
                {
                    m.removeOutline();
                }
            }

            box.gamer.viewRotationInput -= rotationCount;
        }

        public void turnGray()
        {
            foreach (var m in bricks)
            {
                m.turnGray();
            }
        }
        
        enum FallingState
        {
            TopSliding,
            Falling,
            GroundCollision,
        }
    }

    class FallingBlockMember
    {
        //public Vector2 center;
        public Brick brick;
        public IntVector2 offset;
        Graphics.Image outline;  

        public FallingBlockMember(Brick brick, IntVector2 offset)
        {
           
            this.brick = brick;
            this.offset = offset;
        }

        public void updateGridPos(IntVector2 center)
        {
            brick.gridPos = center + offset;
        }

        public void updatePosition(Vector2 center)
        {
            //this.center = center;
            center += offset.Vec * m3Ref.TileWidth;
            brick.images.ParentPosition = center;
            if (outline != null)
            {
                outline.position = center;
            }
        }

        public void rotate()
        {
            offset = VectorExt.RotateVector90DegreeRight(offset);
        }

        public bool canRotate(BrickBox box, IntVector2 gridPos)
        {
            IntVector2 check = VectorExt.RotateVector90DegreeRight(offset) + gridPos;
            return box.isBlocked(check) == false;
        }

        public void turnGray()
        {
            brick.turnGray();
        }

        public void DeleteMe()
        {
            brick.DeleteMe();
        }

        public void createOutline()
        {
            outline = new Graphics.Image(SpriteName.WhiteArea, brick.images.ParentPosition, new Vector2(m3Ref.TileWidth + Engine.Screen.BorderWidth),
               m3Lib.LayerBrickOutline, true);
            outline.Color = Color.Black;
        }

        public void removeOutline()
        {
            outline.DeleteMe();
        }
    }
}

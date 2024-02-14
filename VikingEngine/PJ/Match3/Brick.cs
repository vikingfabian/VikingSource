using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class Brick : AbsDeleteable
    {
        public BrickColor color;
        public BrickBox box;
        public Graphics.ImageGroupParent2D images;

        public bool falling = false;
        public IntVector2 gridPos;
        BrickMover fallMover = new BrickMover(int.MaxValue);
        public SpriteName previewSprite, graySprite;

        public Brick(BrickColor color)
        {
            this.color = color;

            SpriteName sprite = SpriteName.NO_IMAGE;

            switch (color)
            {
                case BrickColor.Blue:
                    sprite = SpriteName.m3BrickBlue;
                    previewSprite = SpriteName.m3PrevBrickBlue;
                    graySprite = SpriteName.m3GrayBrickBlue;
                    break;
                case BrickColor.Green:
                    sprite = SpriteName.m3BrickGreen;
                    previewSprite = SpriteName.m3PrevBrickGreen;
                    graySprite = SpriteName.m3GrayBrickGreen;
                    break;
                case BrickColor.Red:
                    sprite = SpriteName.m3BrickRed;
                    previewSprite = SpriteName.m3PrevBrickRed;
                    graySprite = SpriteName.m3GrayBrickRed;
                    break;
                case BrickColor.Yellow:
                    sprite = SpriteName.m3BrickYellow;
                    previewSprite = SpriteName.m3PrevBrickYellow;
                    graySprite = SpriteName.m3GrayBrickYellow;
                    break;
                case BrickColor.Orange:
                    sprite = SpriteName.m3BrickOrange;
                    previewSprite = SpriteName.m3PrevBrickOrange;
                    graySprite = SpriteName.m3GrayBrickOrange;
                    break;
                case BrickColor.Purple:
                    sprite = SpriteName.m3BrickPurple;
                    previewSprite = SpriteName.m3PrevBrickPurple;
                    graySprite = SpriteName.m3GrayBrickPurple;
                    break;

                case BrickColor.Stone:
                    sprite = SpriteName.m3BrickStone;
                    graySprite = SpriteName.m3GrayBrickStone;
                    previewSprite = SpriteName.MissingImage;
                    break;
            }

            images = new Graphics.ImageGroupParent2D(2);
            Graphics.Image outline = new Graphics.Image(sprite, Vector2.Zero, new Vector2(m3Ref.TileWidth), m3Lib.LayerBrick, true);
            
            images.Add(outline);
        }

        public Brick(BrickColor color, BrickBox box, IntVector2 pos)
            : this(color)
        {
            this.gridPos = pos;
            this.box = box;

            images.ParentPosition = box.worldPos(pos);

            setAsFalling();
        }

        public void setAsFalling()
        {
            if (!falling)
            {
                Brick b;
                if (box.grid.TryGet(gridPos, out b))
                {
                    if (b == this)
                    {
                        box.grid.Set(gridPos, null);
                    }
                }
                box.addFallingBrick(this);
                falling = true;
                updatePos();
            }
        }

        public void setInGrid()
        {
            falling = false;
            while (box.hasBrick(gridPos))
            {
                gridPos.Y--;
            }

            if (box.grid.InBounds(gridPos))
            {
                box.grid.Set(gridPos, this);
                updatePos();
            }
            else
            {
                if (gridPos.Y < 0)
                {
                    box.gamer.onDeath(this);
                }
                else
                {
                    DeleteMe();
                }
            }
        }

        public bool update()
        {
            fallMover.fallSpeedMultiplier = m3Ref.gamestate.fallSpeedMultiplier;
            fallMover.update(ref gridPos);

            if (groundCollision())
            {
                Input.InputLib.Vibrate(box.gamer.gamerdata.button, 0.05f, 0.05f, 200);
                falling = false;
                fallMover.fallDistPercent = 0f;
            }

            updatePos();

            return falling;
        }

        void updatePos()
        {
            Vector2 center = box.worldPos(gridPos);
            if (falling)
            {
                center.Y += fallMover.fallDistPercent * m3Ref.TileWidth;
            }
            images.ParentPosition = center;
        }

        public bool groundCollision()
        {
            return box.isBlocked(VectorExt.AddY(gridPos, 1));
        }

        public override void DeleteMe()
        {
            if (!isDeleted)
            {
                base.DeleteMe();
                images.DeleteMe();

                box?.remove(this);
            }
        }

        public void deathFlash()
        {
            const float FlashTime = 60;
            new Graphics.ColorFlash(images.images[0].image, Color.DarkRed, Color.White, 5, FlashTime);

            new Timer.TimedAction0ArgTrigger(turnGray, 4 * FlashTime); 
        }

        public void turnGray()
        {
            images.images[0].image.SetSpriteName(graySprite);
        }

        public override string ToString()
        {
            return "Brick(" + color.ToString() + ") " + gridPos.ToString() + " falling(" + falling.ToString() + ")";
        }
    }
}

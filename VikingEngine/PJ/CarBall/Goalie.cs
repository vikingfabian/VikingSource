using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class Goalie
    {
        Graphics.Image paddle;
        public Physics.RectangleBound bound;
        GoalieSpot fromSpot;
        public Gamer gamer;
        float moveDir;
        float speed;

        Graphics.ImageRelation inputIcon;
        public int ballKickDirX;

        public Goalie(GoalieSpot fromSpot, Gamer gamer)
        {
            this.speed = gamer.car.maxSpeed * 0.7f * fromSpot.speedMultiplier;
            this.fromSpot = fromSpot;
            this.gamer = gamer;

            float h = Convert.ToInt32(0.11f * cballRef.state.field.area.Height);

            ballKickDirX = gamer.leftTeam ? 1 : -1;
            paddle = new Graphics.Image(gamer.leftTeam? SpriteName.cballGoliePaddleBlue : SpriteName.cballGoliePaddleRed,
                fromSpot.image.position, new Vector2(h / SpriteSheet.CballGoliePaddleSz.Y * SpriteSheet.CballGoliePaddleSz.X, h),
                cballLib.LayerCar, true);
            moveDir = fromSpot.startDir;
            updateDir();

            bound = new Physics.RectangleBound(paddle.position, new Vector2(0.5f, 0.456f) * paddle.size);

            inputIcon = new Graphics.ImageRelation(
                new Graphics.Image(gamer.gamerdata.button.Icon, Vector2.Zero, Engine.Screen.SmallIconSizeV2, 
                ImageLayers.AbsoluteBottomLayer, true),
                paddle, new Vector2(Engine.Screen.SmallIconSize * lib.BoolToLeftRight(!gamer.leftTeam), 0));
            inputIcon.child.LayerBelow(paddle);

            cballRef.sounds.turnGoalie.Play(paddle.position);
        }

        public bool update()
        {
            paddle.Ypos += speed * moveDir * Ref.DeltaTimeMs;
            bound.Center = paddle.position;
            inputIcon.update();

            if (gamer.gamerdata.button.DownEvent)
            {
                cballRef.sounds.flap.Play(paddle.position, 1.2f);
                moveDir *= -1;
                updateDir();
            }

            if (fromSpot.moveRange.IsWithinRange(paddle.Ypos) == false)
            {
                //moveDir *= -1;
                //updateDir();
                DeleteMe();
                return true;
            }
            return false;
        }

        void updateDir()
        {
            paddle.spriteEffects = moveDir > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
        }

        public void DeleteMe()
        {
            paddle.DeleteMe();
            inputIcon.child.DeleteMe();

            cballRef.sounds.turn2.Play(paddle.position, 2f);
        }
    }
}

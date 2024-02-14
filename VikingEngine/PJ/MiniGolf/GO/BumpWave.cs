using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.PJ.MiniGolf.GO;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class BumpWave : AbsGameObject, IChildObject
    {
        public const float Opacity = 0.5f;
        public Ball ball;
        float goalScale;
        Physics.CircleBound circleBound;
        Collisions collisions;

        public BumpWave(Ball ball, Collisions collisions)
        {
            Ref.draw.CurrentRenderLayer = Draw.BackLayer;

            this.ball = ball;
            this.collisions = collisions;

            ball.childObjects.Add(this);
            image = new Graphics.Image(SpriteName.birdBumpWave, ball.image.Position,
                new Vector2(GolfRef.gamestate.BallScale * 1.0f), ImageLayers.AbsoluteBottomLayer, true);
            image.Opacity = Opacity;
            image.LayerBelow(ball.image);

            goalScale = image.Size1D * 2f;

            circleBound = new Physics.CircleBound(image.Position, 1);
            bound = circleBound;

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        public bool update()
        {
            image.Position = ball.image.Position;
            bound.Center = image.Position;
            image.Size1D += GolfRef.gamestate.BallScale * 12f * Ref.DeltaTimeSec;

            circleBound.radius = image.Size1D * 0.42f;
            //bound = circleBound;

            collisions.updateBumpCollisions(this);
            
            if (image.Size1D >= goalScale)
            {
                return true;
            }

            return false;
        }

        public void onRemoval()
        {
            image.DeleteMe();
        }

        override public ObjectType Type { get { return ObjectType.BumpWave; } }
    }
}

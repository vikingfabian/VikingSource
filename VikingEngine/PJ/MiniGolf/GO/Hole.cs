using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class Hole : AbsItem
    {
        public Hole(Vector2 pos)
            : base()
        {
            Ref.draw.CurrentRenderLayer = Draw.BackLayer;

            image = new Graphics.Image(SpriteName.golfHole, pos,
               new Vector2(GolfRef.gamestate.HoleScale), GolfLib.HoleLayer, true);
            bound = new Physics.CircleBound(image.Position, image.Size1D * 0.4f);
            
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage = new Graphics.Image(SpriteName.WhiteArea, bound.Center,
                    bound.HalfSize * 2f, ImageLayers.AbsoluteTopLayer, true);
                boundImage.Color = Color.Red;
                boundImage.Opacity = 0.5f;
            }

            spawnEffect();
            Ref.draw.CurrentRenderLayer = Draw.HudLayer;

            GolfRef.objects.Add(this);
        }

        void spawnEffect()
        {
            int count = 8;

            var dirs = VectorExt.CircleOfDirections(count, 0f, 0.4f);
            GolfRef.sounds.holeAppear.Play(image.position);

            foreach (var m in dirs)
            {
                new Graphics.ParticleImage(SpriteName.WhiteCirkle, image.Position, new Vector2(GolfRef.gamestate.BallScale * 0.6f),
                    GolfLib.HoleLayer - 1, m).particleData.setFadeout(300, 120);
            }
        }

        override public ObjectType Type { get { return ObjectType.Hole; } }
    }    
}

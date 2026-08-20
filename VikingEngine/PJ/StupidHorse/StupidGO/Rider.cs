using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.PJ.MiniGolf;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.PJ.StupidHorse.StupidGO
{
    class Rider
    {
        public Physics.CircleBound circleBound;
        HatImage hatimage;
        Graphics.Image horseImage, riderImage;
        Vector2 riderOffset;
        float riderRotation;

        HorseTrack track;
        protected Graphics.Image boundImage;
        public Rider(GamerData gamerData, StupidWorld world, HorseTrack track)
        {
            this.track = track;
            var animalSetup = AnimalSetup.Get(gamerData.joustAnimal);
            SpriteName animalTile = animalSetup.wingUpSprite;

            horseImage = new Graphics.Image(SpriteName.stupidHorse, track.start,
                world.horseScale, StupidLib.Layer_Horse, true);

            riderImage = new Graphics.Image(animalTile, track.start,
                world.riderScale, StupidLib.Layer_Horse -2, true);

            riderOffset = new Vector2(-0.06f, -0.24f) * world.horseScale;
            riderRotation = -0.15f;
            if (gamerData.hat != Hat.NoHat)
            {
                hatimage = new HatImage(gamerData.hat, riderImage, animalSetup);
            }

            updateRider();
        }

        void updateRider()
        {
            riderImage.Position = horseImage.Position + VectorExt.RotateVector(riderOffset, horseImage.Rotation);
            riderImage.spriteEffects = horseImage.spriteEffects;
            riderImage.Rotation = horseImage.Rotation + riderRotation;
        }

        protected void createBound()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero,
                    Vector2.One, ImageLayers.AbsoluteTopLayer, true);
                boundImage.Color = Color.Red;
                boundImage.Opacity = 0.5f;
            }

            circleBound = new Physics.CircleBound(riderImage.Position, 1);
            //bound = circleBound;
            circleBound.radius = riderImage.Width * PjLib.AnimalCharacterSzToBoundSz;
            
        }
    }
}

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

        GamerData gamerData;

        bool forwardDir;

        Time flipTime;

        public Rider(GamerData gamerData, StupidWorld world, HorseTrack track)
        {
            this.gamerData = gamerData;
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

            flipTime = new Time( 1,  TimeUnit.Seconds);
        }

        void updateRider()
        {
            var off = riderOffset;
            if (forwardDir == false)
            {
                off.X *= -1;
            }

            riderImage.Position = horseImage.Position + VectorExt.RotateVector(off, horseImage.Rotation);
            riderImage.spriteEffects = horseImage.spriteEffects;
            riderImage.Rotation = horseImage.Rotation + riderRotation;
        }

        public void Update(StupidHorseScene scene)
        {
            if (flipTime.CountDown())
            {
                forwardDir = !forwardDir;
                flipTime = new Time(Ref.rnd.Float(0.6f, 2), TimeUnit.Seconds);

                horseImage.spriteEffects = forwardDir? Microsoft.Xna.Framework.Graphics.SpriteEffects.None : Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            if (gamerData.button.DownEvent)
            {
                horseImage.Xpos += horseImage.size.X * 0.8f * lib.BoolToLeftRight(forwardDir);

                if (forwardDir)
                {
                    SoundManager.whip.Play();
                }
                else
                {
                    SoundManager.whip_fail.Play();
                }

                flipTime.MilliSeconds -= 100;

                float perc = (horseImage.Xpos - track.start.X) / (track.stop.X - track.start.X);
                track.number.TextString = (Bound.Set( Convert.ToInt32(perc * 200f), 0, 200)).ToString() + " m";    

                if (horseImage.Xpos < 10)
                {
                    horseImage.Xpos = 10;
                }

                if (horseImage.Xpos >= track.stop.X)
                {
                    horseImage.Xpos = track.stop.X;
                    scene.OnWinner(this);
                    SoundManager.success.Play();

                    int count = 8;

                    var dirs = VectorExt.CircleOfDirections(count, 0f, 0.4f);
                    //GolfRef.sounds.holeAppear.Play(riderImage.position);
                    updateRider();
                    foreach (var m in dirs)
                    {
                        var p = new Graphics.ParticleImage(SpriteName.WhiteArea, riderImage.Position, riderImage.size * 0.3f,
                            StupidLib.Layer_Horse - 4, m);
                        p.particleData.setFadeout(300, 120);
                        p.Color = Color.Yellow;
                    }
                }
                
                
            }

            updateRider();

           
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

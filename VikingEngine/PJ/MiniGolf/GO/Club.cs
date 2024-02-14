using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class Club
    {
        Ball ball;
        Graphics.Image arrow;
        
        PowerBar powerBar;
        bool fireKeyDown = false;
        Rotation1D angle;
        double tweakTime = 0;
        Time tweakDelay = new Time(400);

        public Club(Ball ball)
        {
            this.ball = ball;
            arrow = new Graphics.Image(SpriteName.golfArrowMark, Vector2.Zero,
                ArrowScale(), GolfLib.ClubLayer, true);
        }

        static Vector2 ArrowScale()
        {
            return new Vector2(GolfRef.gamestate.BallScale * 0.8f);
        }

        public bool update(Input.IButtonMap button)
        {
            if (powerBar == null)
            {
                if (!fireKeyDown)
                {
                    angle = GolfRef.gamestate.clubAngle;
                    
                    if (button.DownEvent)
                    {
                        fireKeyDown = true;
                        GolfRef.sounds.fireKeydown.Play(ball.image.position);
                        arrow.size *= 1.3f;
                        ball.setFlapSprite();
                    }
                }
                else
                {
                    if (tweakDelay.CountDown())
                    {
                        //arrow.size = ArrowScale();
                        tweakTime += Ref.DeltaTimeSec * 2.0;
                    }

                    if (button.UpEvent)
                    {
                        ball.defaultSprite();
                        powerBar = new PowerBar(ball, anglePlusTweak());
                        arrow.Visible = false;
                    }
                }

                updateArrow();
            }
            else
            {
                if (powerBar.update() || button.DownEvent)
                {
                    powerBar.fire(button);                   
                    return true;
                }
            }

            return false;
        }

        Rotation1D anglePlusTweak()
        {
            const double TweakAngle = 0.36;

            return new Rotation1D(angle.radians + Math.Sin(tweakTime) * TweakAngle);
        }

        void updateArrow()
        {
            Rotation1D a = anglePlusTweak();

            float r = ball.circleBound.radius * 2f;
            arrow.Position = a.Direction(r) + ball.image.Position;
            arrow.Rotation = a.Radians;
        }

        public void DeleteMe()
        {
            arrow.DeleteMe();
            powerBar?.DeleteMe();
        }
    }



    class PowerBar
    {
        const int ArrowCount = 10;
        static readonly IntervalF pitch = new IntervalF(-0.5f, 0.5f);

        Ball ball;

        bool outwardDir = true;
        int currentIndex;
        public float percentArrow = 0;


        Rotation1D direction;
        Vector2 dirVec;

        List<Arrow> arrows;


        //SoundEffectInstance sei;
        SoundEffectInstance sound;
        Timer.Basic pitchUpdate = new Timer.Basic(120, true);


        public PowerBar(Ball ball, Rotation1D angle)
        {
            //byte[] AudioData = new byte[48000];
            //SoundEffect se = new SoundEffect(AudioData, 48000, AudioChannels.Stereo);
            //sei = se.CreateInstance();
            //sei.Pitch = 0.5f;
            //sei.IsLooped = true;
            //sei.Play();
            //DynamicSoundEffectInstance


            sound = GolfRef.sounds.load.PlayInstance(ball.image.position, true, 0.3f, pitch.Min);


            this.ball = ball;
            direction = angle;//GolfRef.gamestate.prevClubAngle;
            dirVec = direction.Direction(1f);

            
            float powerStepA = GolfRef.gamestate.ClubForce.Length / (ArrowCount - 1);
            
            //int barBcount = 6;
            //float powerStepB = GolfRef.gamestate.ClubForceB.Length / (barBcount - 1);

            arrows = new List<Arrow>(ArrowCount);

            float power = GolfRef.gamestate.ClubForce.Min;

            //BAR A
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow1, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow2, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow3, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow4, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow5, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow6, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow7, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow8, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));
            power += powerStepA;
            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow9, 
                new IntervalF(power, power + powerStepA), IntervalF.Zero, angle));

            arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrow10, 
                new IntervalF(GolfRef.gamestate.ClubForce.Max, GolfRef.gamestate.ClubForce.Max), 
                IntervalF.Zero, angle));
            arraylib.Last(arrows).addMarker(true);

            ////BAR B
            //power = GolfRef.gamestate.ClubForceB.Min;
            //arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrowB5, new IntervalF(power, power + powerStepB), IntervalF.Zero));
            //power += powerStepB;
            //arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrowB5, new IntervalF(power, power + powerStepB), IntervalF.Zero));
            //power += powerStepB;
            //arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrowB6, new IntervalF(power, power + powerStepB), IntervalF.Zero));
            //power += powerStepB;
            //arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrowB6, new IntervalF(power, power + powerStepB), IntervalF.Zero));
            //power += powerStepB;
            //arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrowB7, new IntervalF(power, power + powerStepB), IntervalF.Zero));
            
            //arrows.Add(new Arrow(arrows.Count, SpriteName.golfArrowB7, new IntervalF(GolfRef.gamestate.ClubForceB.Max, GolfRef.gamestate.ClubForceB.Max), 
            //    IntervalF.Zero));
            //arraylib.Last(arrows).addMarker(false);

            updateArrowPlacement();
        }

        

        public bool update()
        {

            float add = Ref.DeltaTimeSec * 12f;
            bool arrowsChange = false;

            if (outwardDir)
            {
                percentArrow += add;
                if (percentArrow > 1f)
                {
                    arrowsChange = true;
                    percentArrow -= 1f;
                    currentIndex++;
                    if (currentIndex >= arrows.Count)
                    {
                        currentIndex = arrows.Count - 1;
                        outwardDir = false;
                        percentArrow = 1f;
                    }
                }
            }
            else
            {
                percentArrow -= add;
                if (percentArrow < 0f)
                {
                    arrowsChange = true;
                    percentArrow += 1f;
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        currentIndex = 0;
                        percentArrow = 0f;
                        return true;
                    }
                }
            }

            if (arrowsChange)
            {
                for (int i = 0; i < arrows.Count; ++i)
                {
                    arrows[i].image.Visible = i <= currentIndex;
                }

                
                sound.Pitch = pitch.GetFromPercent((float)currentIndex / ArrowCount);
            }

            //GolfRef.sounds.load.soundeffect.p
            //if (pitchUpdate.Update())
            //{
            //    sound.Pitch = pitch.GetFromPercent(percentArrow);
            //}
            updateArrowPlacement();

            return false;
        }

       // void 

        public void fire(Input.IButtonMap button)
        {
            float power = arrows[currentIndex].power.GetFromPercent(percentArrow);
            ball.addForce(direction, power);
            ball.addRotationForce(percentArrow);

            IntervalF volume = new IntervalF(2, 4);
            GolfRef.sounds.hit.Play(ball.image.position, volume.GetFromPercent(percentArrow));
            Input.InputLib.Vibrate(button, 0.15f * percentArrow + 0.05f, 0.1f * percentArrow + 0.05f, 300);
            SoundManager.PlaySound(LoadedSound.flap);

            Rotation1D coinDropDir = direction;
            coinDropDir.invert();

            float angleDiff = MathHelper.PiOver2 * 0.7f;
            coinDropDir.Add(-angleDiff * 0.5f);
            for (int i = 0; i < GolfLib.ClubStrikeCost; ++i)
            {
                new DropCoinAnimation(ball, coinDropDir, false);
                coinDropDir.Add(angleDiff);
            }
        }

        void updateArrowPlacement()
        {
            foreach (var m in arrows)
            {
                m.update(ball.image.Position, dirVec);
            }
        }

        public void DeleteMe()
        {
            foreach (var m in arrows)
            {
                m.DeleteMe();
            }
            //sei.Stop(true);0
            Engine.Sound.StopInstace(sound);
        }

        class Arrow
        {
            public Graphics.Image image;
            public Graphics.Image marker = null;
            public IntervalF power;
            public IntervalF liftPower;
            int index;

            public Arrow(int index, SpriteName sprite, IntervalF power, IntervalF liftPower, Rotation1D angle)
            {
                this.index = index;
                this.power = power;
                this.liftPower = liftPower;

                image = new Graphics.Image(sprite, Vector2.Zero, new Vector2(GolfRef.gamestate.BallScale * 0.8f), GolfLib.ClubLayer, true);
                image.Rotation = angle.radians;//GolfRef.gamestate.clubAngle.Radians;
                image.ChangePaintLayer(-index);

                image.Visible = false;
            }

            public void addMarker(bool a)
            {
                marker = (Graphics.Image)image.CloneMe();
                marker.SetSpriteName(a? SpriteName.golfArrowMark : SpriteName.golfArrowMarkWide);
                marker.Layer = GolfLib.ClubLayer - 1;
            }

            public void update(Vector2 center, Vector2 dir)
            {
                float r = GolfRef.gamestate.BallScale * 0.3f + image.Size.Y * 0.1f * index;
                image.Position = center + dir * r;

                if (marker != null)
                {
                    marker.Position = image.Position;
                }
            }

            public void DeleteMe()
            {
                image.DeleteMe();
                if (marker != null)
                {
                    marker.DeleteMe();
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    struct ParticleData
    {
        public Vector3 velocity;
        public Vector3 gravity;
        public Vector3 rotationSpeed;
        bool useGravity;
        public float velocityPercChange;
        public float scaleChange;

        public ParticleState state;

        public float inTime, midTime, outTime;
        public float inOpacityChange, outOpacityChange;
        public int outBrightnessChange;
        public bool realTime;

        float time;
        bool gravityUpdate;

        public void setFadeout(float delay, float fadeTime)
        {
            state = ParticleState.Mid;
            midTime = delay;
            outTime = fadeTime;
            outOpacityChange = -1f / fadeTime;
        }

        public void setFalling(float gravity)
        {
            useGravity = true;
            this.gravity.Y = gravity;
        }

        public void setRoundsPerSecond(float value)
        {
            rotationSpeed.X = value * MathHelper.TwoPi * 0.001f;
        }

        public void setLifeTime(float lifeTime)
        {
            state = ParticleState.Mid;
            midTime = lifeTime;
            outTime = 0;
        }

        void timeCheck()
        {
            if (realTime)
            {
                time = Ref.DeltaTimeMs;
                gravityUpdate = Ref.TimePassed16ms;
            }
            else
            {
                time = Ref.DeltaGameTimeMs;
                gravityUpdate = Ref.GameTimePassed16ms > 0;
            }
        }

        public bool update(Graphics.Mesh img)
        {
            timeCheck();

            if (state > 0)
            {
                img.Position += velocity * time;

                float scaleAdd;
                baseUpdate(img, out scaleAdd);

                img.Scale1D += scaleAdd;
            }
            return state == ParticleState.Removed;
        }

        public bool update(Graphics.Image img)
        {
            timeCheck();

            img.position.X += velocity.X * time;
            img.position.Y += velocity.Y * time;
            img.Rotation += rotationSpeed.X * time;

            if (useGravity && gravityUpdate)
            {
                velocity += gravity;
            }

            float scaleAdd;
            baseUpdate(img, out scaleAdd);
            img.Size1D += scaleAdd;
            //}
            return state == ParticleState.Removed;
        }

        void baseUpdate(AbsDraw img, out float scaleAdd)
        {
            if (gravityUpdate)
            {
                if (velocityPercChange != 0)
                {
                    velocity.X *= 1f + velocityPercChange;
                    velocity.Y *= 1f + velocityPercChange;
                }
            }

            scaleAdd = scaleChange * Ref.DeltaTimeMs;

            switch (state)
            {
                case ParticleState.In:
                    updateState(img, ref inTime, inOpacityChange);
                    break;
                case ParticleState.Mid:
                    updateState(img, ref midTime, 0);
                    break;
                case ParticleState.Out:                    
                    updateState(img, ref outTime, outOpacityChange);

                    if (gravityUpdate && outBrightnessChange != 0)
                    {
                        img.pureColor.R = Bound.Byte(img.pureColor.R + outBrightnessChange);
                        img.pureColor.G = Bound.Byte(img.pureColor.G + outBrightnessChange);
                        img.pureColor.B = Bound.Byte(img.pureColor.B + outBrightnessChange);
                    }
                    break;
            }
        }

        void updateState(AbsDraw img, ref float stateTime, float opacityChange)
        {
            stateTime -= time;
            img.Opacity += opacityChange * time;

            if (stateTime <= 0)
            {
                nextState(stateTime);
            }
        }

        void nextState(float timeLeft)
        {
            state++;

            switch (state)
            {
                case ParticleState.Mid:
                    {
                        if (midTime <= 0)
                        {
                            nextState(timeLeft);
                        }
                        else
                        {
                            midTime += timeLeft;
                        }
                    }
                    break;
                case ParticleState.Out:
                    {
                        if (outTime <= 0)
                        {
                            nextState(timeLeft);
                        }
                        else
                        {
                            outTime += timeLeft;
                        }
                    }
                    break;
                default:
                    state = ParticleState.Removed;
                    break;

            }
        }

        public Vector2 Velocity2D
        {
            set
            {
                velocity.X = value.X;
                velocity.Y = value.Y;
            }
        }

    }
    enum ParticleState
    {
        Non,
        In,
        Mid,
        Out,
        Removed,
    }
}

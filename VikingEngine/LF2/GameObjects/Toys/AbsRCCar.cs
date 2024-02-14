//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;

//namespace Game1.LootFest.GameObjects.Toys
//{
//    abstract class AbsRCCar : AbsRCVeihcle
//    {
//        protected float wheelTurning = 0;
//        protected float gas = 0;

//        public AbsRCCar(Characters.Hero parent)
//            : base(parent)
//        {
            
//        }
//        public AbsRCCar(System.IO.BinaryReader System.IO.BinaryReader, Network.AbsNetworkGamer sender)
//            : base(System.IO.BinaryReader, sender)
//        {

//        }
//        public override void Pad_Event(JoyStickValue e)
//        {
//            if (e.Stick == Stick.Right)
//                gas = -e.Direction.Y;
//            else
//                wheelTurning = e.Direction.X;
//        }
//        public override void PadUp_Event(Stick padIx, int contolIx)
//        {
//            if (padIx == Stick.Right)
//                gas = 0;
//            else
//                wheelTurning = 0;
//        }
//        public override void Button_Event(ButtonValue e)
//        {
//            base.Button_Event(e);
//            if (e.Button == numBUTTON.B)
//            {
//                gas = -lib.BoolToInt01(e.KeyDown);
//            }
//            if (e.Button == numBUTTON.X)
//            {
//                gas = lib.BoolToInt01(e.KeyDown);
//            }
//        }

//        const float MaxVelocity = 0.05f;
//        virtual protected float maxVelocity
//        { get { return MaxVelocity; } }
//        const float Acceleration = 0.005f;
//        virtual protected float acceleration
//        { get { return Acceleration; } }
//        virtual protected float baseSpeed
//        { get { return 0; } }

//        const float NaturalBreak = 0.8f;
//        virtual protected float naturalBreak
//        { get { return NaturalBreak; } }


//        const float MinSpeed = 0.002f;
//        virtual protected float minSpeed
//        { get { return MinSpeed; } }

//        const float AccelerationSlopeEffect = 0.003f;
//        virtual protected float accelerationSlopeEffect
//        { get { return AccelerationSlopeEffect; } }
//        const float NaturalBreakSlopeEffect = -0.2f;
//        virtual protected float naturalBreakSlopeEffect
//        { get { return NaturalBreakSlopeEffect; } }
//        const float MaxVelocitySlopeEffect = 0.03f;
//        virtual protected float maxVelocitySlopeEffect
//        { get { return MaxVelocitySlopeEffect; } }


//        public override void Time_Update(UpdateArgs args)
//        {
//            if (!LockControls)
//            {
                
                
                

//                if (!physics.PhysicsStatusFalling)
//                {
//                    float acc = acceleration;
//                    float nBreak = naturalBreak;
//                    float max = maxVelocity;

//                    //calculate slope affect
//                    Vector2 dir = rotation.Direction(1);
//                    dir.X *= goalXtilt;
//                    dir.Y *= goalZtilt;
//                    float slopeEffect = dir.X + dir.Y;
                    
//                    if (slopeEffect != 0)
//                    {
//                        slopeEffect *= -lib.FloatToDir(velocity);
//                        acc += accelerationSlopeEffect * slopeEffect;
//                        nBreak += naturalBreakSlopeEffect * slopeEffect;
//                        max += maxVelocitySlopeEffect * slopeEffect;
//                    }
//                    addTerrainEffect(ref acc, ref max);

//                    velocity *= nBreak;
//                    if (Math.Abs(velocity) < MinSpeed)
//                    {
//                        velocity = 0;
//                    }
//                    velocity = lib.SetBounds(velocity + acc * gas, -max, max);
//                    float velocityAndBase = velocity + baseSpeed;

//                    const float RotationAngle = 6f;
//                    rotation.Add(wheelTurning * RotationAngle * velocityAndBase);

//                    Velocity.Set(rotation, velocityAndBase);
//                }
                
//            }
//            base.Time_Update(args);
//        }
        

//        //protected override VoxelModelName imageName
//        //{
//        //    get { return VoxelModelName.Pig; }
//        //}
//        protected override float ProjectileDamage
//        {
//            get { return LightDamage; }
//        }
        
//    }
//}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.Graphics
//{
//    class Emitter : Update_Old
//    {
//        const bool REUSE_OBJECTS = false;

//        public List<CollitionPlane> CollitionPlanes = null;
//        Random rnd = Ref.rnd;
//        public Graphics.Point3D EmitterShape = new VikingEngine.Graphics.Point3D();
//        List<Graphics.Mesh> basicObjects = new List<VikingEngine.Graphics.Mesh>();
//        List<int> randomImages = new List<int>();
//        Graphics.Mesh randomBasicObject()
//        {
//            return basicObjects[rnd.Next(basicObjects.Count)];
//        }

//        List<Motion3d> particleMotions = new List<Motion3d>();
//        List<ParticleData> deadParticles = new List<ParticleData>();
//        //int particleLife; int particleLifeAdd;
//        RangeF lifeTime;
//        RangeV3 speed;
//        //Vector3 speedMin;
//        //Vector3 speedAdd;
//        Vector3 acceleration;
//        public float RandomSizeChange_Percent = 0;
//        public bool RandomStartXrot = false;
//        public Vector3 RandomRotation = Vector3.Zero;
//        public int FadingTime = 0;
//        public int FadeInTime = 0;

//        //float particlesMin;
//        //float particlesAdd;
//        RangeF particlesFreq;
//        float add = 0;

//        //Speed is legth/sec
//        public void SetParticleMotions(List<Motion3d> motions)
//        { particleMotions = motions; }
//        public void AddObject(Graphics.Mesh obj)
//        { basicObjects.Add(obj); }

//        //public void DeadParticle(ParticleData add)
//        //{ 
//        //    if (REUSE_OBJECTS)
//        //        deadParticles.Add(add); 
//        //}

//        public void Init_Emitter(Graphics.Mesh basicObject, Vector3 emitterPos,
//            Vector3 emitterSize, RangeF particlesPerSec, RangeF particleLifeTime,
//            RangeV3 startSpeedUnitPerSec, Vector3 accelerantionPerSec)
//        {
//             basicObjects.Add(basicObject);
//            basicObject.Visible = false;
//            EmitterShape.Position = emitterPos;
//            EmitterShape.Scale = emitterSize;
//            particlesFreq = particlesPerSec;
//            particlesFreq.Multiply(PublicConstants.Milli);
//            //particlesMin = particlesPerSec_Min * PublicConstants.MILLI;
//            //particlesAdd = (particlesPerSec_Max - particlesPerSec_Min) * PublicConstants.MILLI; ;
//            //particleLife = particleLifeTime_Min;
//            //particleLifeAdd = particleLifeTime_Max - particleLifeTime_Min;
//            //if (particleLifeAdd < 0) { particleLifeAdd = 0; }
//            lifeTime = particleLifeTime;
//            //speedMin = startSpeed_Min;
//            //speedAdd = startSpeed_Max - startSpeed_Min;
//            speed = startSpeedUnitPerSec;
//            acceleration = accelerantionPerSec;
//            AddToUpdateList(true);
//        }

//        public void RandomTilenNames(List<SpriteName> tiles)
//        {
//            foreach (SpriteName tile in tiles)
//            {
//                randomImages.Add(DataLib.Images.imagesNames[tile]);
//            }
//        }

//        Vector3 addRandomSizeChange(Vector3 startScale)
//        {
//            //if (RandomSizeChange != 0)
//            //{'

//                float sizeChange = RandomSizeChange_Percent * (float)rnd.NextDouble() + 1;
//                startScale.X *= sizeChange;
//                startScale.Y *= sizeChange;
//                startScale.Z *= sizeChange;

//                return startScale;
//            //}
//        }

//        public override void Time_Update(float time)
//        {
//            add += time * particlesFreq.GetRandom();//(particlesMin + particlesAdd * (float)rnd.NextDouble());


//            if ((int)add > 0)
//            {
//                //Ref.draw.AddToRenderlistEnd = false;
//                for (int i = 0; i < (int)add; i++)
//                {
//                    ParticleData data;
//                    Vector3 randomPos = Vector3.Zero;
//                        randomPos.X = EmitterShape.X + EmitterShape.ScaleX * (float)rnd.NextDouble();
//                        randomPos.Y = EmitterShape.Y + EmitterShape.ScaleY * (float)rnd.NextDouble();
//                        randomPos.Z = EmitterShape.Z + EmitterShape.ScaleZ * (float)rnd.NextDouble();
                    
                    
//                    //if (deadParticles.Count > 0)
//                    //{
//                    //    data = deadParticles[deadParticles.Count - 1];
//                    //    deadParticles.Remove(deadParticles[deadParticles.Count - 1]);
//                    //    Vector3 scale = randomBasicObject().Scale;
//                    //    if (RandomSizeChange != 0)
//                    //    {
//                    //        scale = addRandomSizeChange(scale);
//                    //    }
//                    //    data.Reset(randomPos, scale);
//                    //}
//                    //else
//                    //{
//                        data = new ParticleData(this);
//                        Graphics.Mesh particle = (Graphics.Mesh)randomBasicObject().CloneMe();
//                        particle.Position = randomPos;
//                        data.Particle = particle;

                    
//                        if (RandomSizeChange_Percent != 0)
//                        {
//                            particle.Scale = addRandomSizeChange(particle.Scale);
//                            //float sizeChange = RandomSizeChange * (float)rnd.NextDouble();
//                            //particle.Width += particle.Width * sizeChange;
//                            //particle.Height += particle.Height * sizeChange;
//                            //particle.Depth += particle.Depth * sizeChange;

//                        }
//                        if (FadeInTime > 0)
//                        {
//                            particle.Transparentsy = 0;
//                            //Motion fadeIn = currentState.NewMotion();
//                            Motion3d fadeIn = new Motion3d(//);
//                            //fadeIn.InitMotion3D(
//                            MotionType.TRANSPARENSY,  particle, Vector3.One,
//                                MotionRepeate.NO_REPEATE, FadeInTime, true);
//                            data.Motions.Add(fadeIn);
//                        }
//                        if (randomImages.Count > 0)
//                        {
//                            particle.TextureEffect.TextureSource.SetSource(ColorMapType.Color, randomImages[rnd.Next(randomImages.Count)]);
//                        }
//                        if (RandomStartXrot)
//                        {
//                            Vector3 xrot = Vector3.Zero;
//                            xrot.X = Ref.rnd.Rotation();
//                            particle.RotateWorld(xrot);
//                        }
//                        if (RandomRotation != Vector3.Zero)
//                        {
//                            Motion3d rotate = new Motion3d(//);
//                            //rotate.InitMotion3D(
//                            MotionType.ROTATE, particle,
//                                new Vector3(RandomRotation.X * (float)rnd.NextDouble(),
//                                    RandomRotation.Y * (float)rnd.NextDouble(),
//                                    RandomRotation.Z * (float)rnd.NextDouble()),
//                                     MotionRepeate.Loop, 1000, true);
//                        }
//                        //Start Speed
//                        Vector3 startSpeed = speed.GetRandom();
//                        //    speedMin;
//                        //startSpeed.X += speedAdd.X * (float)rnd.NextDouble();
//                        //startSpeed.Y += speedAdd.Y * (float)rnd.NextDouble();
//                        //startSpeed.Z += speedAdd.Z * (float)rnd.NextDouble();
//                        foreach (Motion3d m in particleMotions)
//                        {
//                            m.CloneMe(particle, true);
//                        }

//                        if (startSpeed != Vector3.Zero || acceleration != Vector3.Zero)
//                        {
//                            ParticleMotion motion = new ParticleMotion(//);
//                            //speed.InitParticle( 
//                                particle, startSpeed, acceleration, MotionRepeate.Loop, true);
//                            if (CollitionPlanes != null)
//                            {
//                                motion.AddPlanes(ref CollitionPlanes);
//                            }
//                            data.PMotion = motion;
//                        }
//                        //Life
//                        float life = lifeTime.GetRandom();//particleLife + rnd.Next(particleLifeAdd);
//                        data.Life = life;
//                        if (life > 0)
//                        {
//                            if (FadingTime > 0)
//                            {
//                                //Motion fadeOut = currentState.NewMotion();
//                                Motion3d fadeOut = new Motion3d(//);
//                                //fadeOut.InitMotion3D(
//                                MotionType.TRANSPARENSY,  particle, new Vector3(-1, 0, 0),
//                                    MotionRepeate.NO_REPEATE, FadingTime, false);
//                                Timer.UpdateTrigger startFade = new VikingEngine.Timer.UpdateTrigger(life - FadingTime, fadeOut, true);
//                                data.Motions.Add(fadeOut);
//                            }
//                        }
//                    //}
//                    //particles.Add(data);
//                }
//                add -= (int)add;
//                //Ref.draw.AddToRenderlistEnd = true;
//            }//End if add

//            //if (particleLife > 0)
//            //{
//            //    for (int p = 0; p < particles.Count; p++)
//            //    {
//            //        if (particles[p].TimeUpdate(time))
//            //        {
//            //            deadParticles.Add(particles[p]);
//            //            particles.Remove(particles[p]);
                        
//            //        }

//            //    }
//            //}

//            //return IsDeleted;
//        }
//        public override void DeleteMe()
//        {
//            foreach (ParticleData p in deadParticles)
//            { p.DeleteMe(); }
//            base.DeleteMe();
//        }
//    }
//    class ParticleData : Update_Old
//    {
//        Emitter parent;
//        public Graphics.Mesh Particle;
//        public Graphics.ParticleMotion PMotion;
//        public List<Graphics.AbsMotion> Motions = new List<AbsMotion>();
//        public Vector3 originalScale;
//        float LifeTime;
//        float LifeLeft;

//        bool fadeIn;

//        public ParticleData(Emitter _parent)
//        {
            
//            parent = _parent;
//            AddToUpdateList(true);
//        }

//        public override void DeleteMe()
//        {
//            Particle.DeleteMe();
//            base.DeleteMe();
//        }

//        public float Life
//        {
//            set { LifeTime = value; LifeLeft = value; }
//        }
//        public override void Time_Update(float time)
//        {
 	 
//            LifeLeft -= time;
//            if (LifeLeft <= 0)
//            {
//                //if (parent.bDeleteMe)
//                //{
//                    this.DeleteMe();
//                //}
//                //else
//                //{
//                //    parent.DeadParticle(this);
//                //    //Particle.Visible = false;
//                //}
                
//            }
//            //return IsDeleted;
//        }
//        public void Reset(Vector3 pos, Vector3 scale)
//        {
//            LifeLeft = LifeTime;
//            if (PMotion != null)
//            { PMotion.ResetMotion();  }
//            Particle.Position = pos;
//            Particle.Scale = scale;
//            Particle.Transparentsy = 1;
//            Particle.Visible = true;
//        }
//     }
//}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Graphics
{
    class ParticleMotion : Motion3d
    {
        Vector3 acceleration;
        List<CollitionPlane> planes = null;
        Vector3 steppingMem;

        public void AddPlanes(ref List<CollitionPlane> newplanes)
        {
            planes = newplanes;
        }

        public ParticleMotion(Mesh objImage, Vector3 SpeedPerSec, Vector3 AccelerationPerSec,
            MotionRepeate repeateType, bool addToUpdateList)
            :base(MotionType.PARTICLE, objImage, SpeedPerSec, repeateType, 1000, addToUpdateList)
        {
            //InitMotion3D();
            acceleration = AccelerationPerSec * 0.001f;
            steppingMem = Stepping;
        }
        public void ResetMotion()
        { Stepping = steppingMem; }
        //public override UpdateType uType
        //{
        //    get
        //    {
        //        return UpdateType.PaticleMotion;
        //    }
        //}

        public override void Time_Update(float time)
        {
            if (planes != null)
            {
                //Graphics.Mesh obj = this.obj3D();
                float[] objpos = new float[3] { mesh.X, mesh.Y, mesh.Z };
                float[] currentSpeed = new float[3] { Stepping.X, Stepping.Y, Stepping.Z };
                float[] planepos = null;
                float[] planeSz = null;


                for (int plane = 0; plane < planes.Count; plane++)
                {
                    CollitionPlane p = planes[plane];
                    if ((currentSpeed[p.PlaneXYZ] < 0 && p.MinPos) ||
                        (currentSpeed[p.PlaneXYZ] > 0 && !p.MinPos))
                    {
                        planepos = new float[3] { p.Postion.X, p.Postion.Y, p.Postion.Z };
                        planeSz = new float[3] { p.Size.X, p.Size.Y, p.Size.Z };
                        bool collition = true;
                        for (int dimention = 0; dimention < 3; dimention++)
                        {
                            if (p.PlaneXYZ == dimention)
                            {
                                if (p.MinPos)
                                {
                                    if (objpos[dimention] > planepos[dimention])
                                    {
                                        collition = false; break;
                                    }
                                }
                                else
                                {
                                    if (objpos[dimention] < planepos[dimention])
                                    {
                                        collition = false; break;
                                    }
                                }
                            }
                            else
                            {
                                if (!(objpos[dimention] >= planepos[dimention] && objpos[dimention] <= planepos[dimention] + planeSz[dimention]))
                                { collition = false; break; }
                            }
                        }
                        if (collition)
                        {
                            if (p.Dampening == 0)
                            { Stepping = Vector3.Zero; }
                            else
                            {
                                switch (p.PlaneXYZ)
                                {
                                    case 0:
                                        Stepping.X = -Stepping.X * p.Dampening;
                                        break;
                                    case 1:
                                        Stepping.Y = -Stepping.Y * p.Dampening;
                                        break;
                                    case 2:
                                        Stepping.Z = -Stepping.Z * p.Dampening;
                                        break;

                                }
                            }
                        }
                    }
                }
            }

            Stepping.X += (acceleration.X * time);
            Stepping.Y += (acceleration.Y * time);
            Stepping.Z += (acceleration.Z * time);

            base.Time_Update(time);

        }
    }
}

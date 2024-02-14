using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest;

namespace VikingEngine.Graphics
{
    class TopViewCamera : AbsCamera
    {
        /* Constants */
        const int ITERATIONS_PER_BLOCK = 3;
        const float STEP_LENGTH = 1f / ITERATIONS_PER_BLOCK;
       

        /* Properties */
        public override CameraType CamType
        {
            get { return CameraType.TopView; }
        }
        public bool UseTerrainCollisions = PlatformSettings.RunProgram == StartProgram.LootFest3;

        /* Fields */
        public bool InstantZoomIn = false;
        public bool InstantZoomOut = false;
        public float currentChaseLength = 0;

        /* Constructors */
        public TopViewCamera()
            : this(100, Vector2.Zero)
        { }
        public TopViewCamera(float zoom, Vector2 tilt)
            : this(zoom, tilt, Engine.Screen.Width, Engine.Screen.Height)
        { }
        public TopViewCamera(float zoom, Vector2 tilt, float screenW, float screenH)
            : base(zoom, tilt, screenW, screenH)
        {
            lib.DoNothing();
        }

        public TopViewCamera Clone()
        {
            TopViewCamera clone = new TopViewCamera()
            {
                currentZoom = currentZoom,
                NearPlane= NearPlane,
                FarPlane= FarPlane,
                angle = angle,
                aspectRatio = aspectRatio,
                tilt=tilt,
            };

            return clone;
        }

        /* Methods */
        public override void Time_Update(float time_ms)
        {
            base.Time_Update(time_ms);

            moveTowards(goalLookTarget);            
        }

        public void moveTowards(Vector3 goal)
        {
            Vector3 diff = goal - lookTarget;
            float l = diff.Length();

            const float MinChaseSpeed = 0.05f;
            if (l > MinChaseSpeed)
            {
                if (Ref.TimePassed16ms)
                {
                    currentChaseLength = l * positionChaseLengthPercentage;
                    currentChaseLength = Bound.Set(currentChaseLength, MinChaseSpeed, maxChaseLength);
                }

                lookTarget += VectorExt.SetLength(diff, currentChaseLength);
            }
            else
            {
                lookTarget = goalLookTarget;
            }

            if (float.NaN.Equals(lookTarget.Z))
            {
                throw new Exception();
            }
            
            positionFromRotation();
        }

        public override void instantMoveToTarget()
        {
            base.instantMoveToTarget();
            lookTarget = goalLookTarget;

            bool InstantZoomIn_s = InstantZoomIn;
            bool InstantZoomOut_s = InstantZoomOut;

            InstantZoomIn = true; InstantZoomOut = true;

            positionFromRotation();

            InstantZoomIn = InstantZoomIn_s;
            InstantZoomOut = InstantZoomOut_s;
        }

        public override void positionFromRotation()
        {

            Vector3 cameraOffsetDir = new Vector3(MathExt.Cosf(Tilt.X), MathExt.Cosf(Tilt.Y), MathExt.Sinf(Tilt.X));

            // rotate
            //{
            //    // sin and cos are flipped compared to regular unit sphere parametrization because of legacy code.
            //    cameraPosition.Y = Mgth.Cosf(Tilt.Y);
            //    cameraPosition.X = Mgth.Cosf(Tilt.X);
            //    cameraPosition.Z = Mgth.Sinf(Tilt.X);
            //}


            float usedZoomVariable = this.targetZoom;

            if (UseTerrainCollisions)
            {
                float targetZoom = this.targetZoom;

                if (LfRef.chunks != null)
                {
                    Vector3 lookDir = lookTarget - Position;
                    lookDir.Normalize();
                    Vector3 targetPos = lookTarget - lookDir * this.targetZoom;

                    int iterations = (int)(this.targetZoom * ITERATIONS_PER_BLOCK);
                    for (int i = 0; i < iterations; ++i)
                    {
                        Vector3 pos = MathExt.Lerp(targetPos, lookTarget, (float)i / (float)iterations);

                        // pick the 8 blocks closest to pos
                        IntVector3 block = new IntVector3(pos);
                        Vector3 frac = pos - block.Vec;
                        IntVector3 otherBlockDirs = new IntVector3(frac + Vector3.One * 0.5f) * 2 - 1;
                        for (int dim = 0; dim < 3; ++dim)
                        {
                            if (frac.GetDim(dim) < 0.5f)
                                block.AddDimension((Dimensions)dim, -1);
                        }

                        // make sure we do not collide with any of the 8 blocks
                        bool noCollision = true;
                        ForXYZLoop loop = new ForXYZLoop(new IntVector3(2));

                        WorldPosition minBlock = new WorldPosition(block);
                        while (loop.Next())
                        {
                            WorldPosition wp = minBlock.GetNeighborPos(loop.Position);
                            if (wp.CorrectPos)
                            {
                                Chunk chunk = wp.Screen;
                                if (chunk != null && chunk.DataGrid != null)
                                {
                                    uint data = chunk.Get(wp);
                                    if (data != 0)
                                    {
                                        noCollision = false;
                                        break;
                                    }
                                }
                            }
                        }

                        if (noCollision)
                        {
                            targetZoom = (pos - lookTarget).Length();
                            break;
                        }
                    }
                }

                //if (targetZoom < currentZoom)
                //{
                for (int i = 0; i < Ref.GameTimePassed16ms; ++i)
                {
                    currentZoom += (targetZoom - currentZoom) * zoomChaseLengthPercentage;
                }

                if (targetZoom < currentZoom)
                {
                    if (InstantZoomIn)
                    {
                        currentZoom = targetZoom;
                    }
                }
                else if(InstantZoomOut)
                {
                    currentZoom = targetZoom;
                }

                //if (InstantZoomIn && targetZoom < currentZoom)
                //{ currentZoom = targetZoom; }
                //else if (InstantZoomOut && )
                //{ currentZoom = targetZoom; }
                //else
                //{ currentZoom += (targetZoom - currentZoom) * ZoomChasePercSpeed; }
                //}
                //else
                //{
                //    if (InstantZoomOut)
                //        currentZoom = targetZoom;
                //    else
                //        currentZoom += (targetZoom - currentZoom) * ZoomChasePercSpeed;
                //}

                usedZoomVariable = currentZoom;
            }

            // zoom
            {
                cameraOffsetDir.Y *= usedZoomVariable;
                float L = usedZoomVariable * MathExt.Sinf(Tilt.Y);
                cameraOffsetDir.X *= L;
                cameraOffsetDir.Z *= L;

                Position = cameraOffsetDir + lookTarget;
            }
           
        }
    }
}

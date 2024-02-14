using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Physics
{
    class Bound2DWrapper
    {
        bool in3D;
        public List<AbsBound2D> bounds;
        Graphics.Mesh[] visualBounds3D = null;
        //Graphics.Image[] visualBounds = null;

        public Bound2DWrapper(bool in3D, params AbsBound2D[] bounds)
        {
            this.in3D = in3D;
            this.bounds = new List<AbsBound2D>(bounds);

            refreshVisualBounds();
        }

        public void refreshVisualBounds()
        {
            if (PlatformSettings.ViewCollisionBounds)
            {
                arraylib.DeleteAndNull(ref visualBounds3D);

                if (in3D)
                {
                    visualBounds3D = new Graphics.Mesh[bounds.Count];
                    for (int i = 0; i < bounds.Count; ++i)
                    {
                        visualBounds3D[i] = new Graphics.Mesh(LoadedMesh.plane, Vector3.Zero, VectorExt.V2toV3XZ(bounds[i].HalfSize * 2f), Graphics.TextureEffectType.Flat,
                            bounds[i].Type == Bound2DType.Circle ? SpriteName.WhiteCirkle : SpriteName.WhiteArea_LFtiles, Color.Red);
                        visualBounds3D[i].Opacity = 0.5f;
                    }
                }
            }
        }

        public void update(Vector3 pos, float rotation)
        {
            Vector2 planePos = VectorExt.V3XZtoV2(pos);
            foreach (var m in bounds)
            {
                m.update(planePos, rotation);
            }

            if (visualBounds3D != null)
            {
                for (int i = 0; i < bounds.Count; ++i)
                {
                    visualBounds3D[i].Visible = Input.Keyboard.Ctrl;

                    visualBounds3D[i].Position = VectorExt.V2toV3XZ(bounds[i].Center, pos.Y + 0.1f);
                    lib.Rotation1DToQuaterion(visualBounds3D[i], bounds[i].Rotation);
                }
            }
        }

        public bool Intersect(AbsBound2D otherBound)
        {
            foreach (var m in bounds)
            {
                if (m.Intersect(otherBound))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Intersect(Bound2DWrapper otherBound)
        {
            foreach (var m in bounds)
            {
                foreach (var other in otherBound.bounds)
                {
                    if (m.Intersect(other))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Collision2D Intersect2(Bound2DWrapper otherBound)
        {
            Collision2D result = Collision2D.NoCollision;

            foreach (var m in bounds)
            {
                foreach (var other in otherBound.bounds)
                {
                    result = m.Intersect2(other);
                    if (result.IsCollision)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        public bool ExtremeRadiusColl(AbsBound2D otherBound)
        {
            foreach (var m in bounds)
            {
                if (Physics.PhysicsLib2D.ExtremeRadiusColl(m, otherBound))
                {
                    return true;
                }
            }
            return false;
        }

        public AbsBound2D MainBound { get { return bounds[0]; } }

        public void DeleteMe()
        {
            arraylib.DeleteAndNull(ref visualBounds3D);
        }
    }
}

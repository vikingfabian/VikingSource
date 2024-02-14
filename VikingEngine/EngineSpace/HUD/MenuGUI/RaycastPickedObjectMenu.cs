using VikingEngine.Engine;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.LootFest.GO;
using VikingEngine.LootFest.GO.Bounds;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.HUD
{
    //struct Ray
    //{
    //    /* Fields */
    //    public Vector3 pos;
    //    public Vector3 dir;

    //    /* Constructors */
    //    public Ray(Vector3 pos, Vector3 dir)
    //    {
    //        this.pos = pos;
    //        this.dir = dir;
    //    }

    //    /* Family Methods */
    //    public override string ToString()
    //    {
    //        return pos.ToString() + " -> " + dir.ToString();
    //    }

    //    /* Novelty Methods */
    //    public bool IntersectAABB(AABB aabb, out float near_t)
    //    {
    //        Vector3 min = aabb.Min;
    //        Vector3 max = aabb.Max;

    //        near_t = float.MinValue;
    //        float far_t = float.MaxValue;

    //        for (int i = 0; i < 3; ++i)
    //        {
    //            float rpos = pos.GetDim(i);
    //            float rdir = dir.GetDim(i);
    //            float bmin = min.GetDim(i);
    //            float bmax = max.GetDim(i);

    //            if (rdir == 0) // ray perpendicular to axis
    //            {
    //                if (rpos < bmin || rpos > bmax)
    //                {
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                float t1 = (bmin - rpos) / rdir;
    //                float t2 = (bmax - rpos) / rdir;

    //                if (t1 > t2)
    //                {
    //                    float swap = t1;
    //                    t1 = t2;
    //                    t2 = swap;
    //                }
    //                if (t1 > near_t)
    //                {
    //                    near_t = t1;
    //                }
    //                if (t2 < far_t)
    //                {
    //                    far_t = t2;
    //                }
    //                if ((near_t > far_t) || (far_t < 0))
    //                {
    //                    return false;
    //                }
    //            }
    //        }

    //        return true;
    //    }
    //}

    //struct AABB
    //{
    //    /* Properties */
    //    public Vector3 Min { get { return center - halfSize; } }
    //    public Vector3 Max { get { return center + halfSize; } }

    //    /* Fields */
    //    public Vector3 center;
    //    public Vector3 halfSize;

    //    /* Constructors */
    //    public AABB(Vector3 center, Vector3 halfSize)
    //    {
    //        this.center = center;
    //        this.halfSize = halfSize;
    //    }
    //}

    class RaycastPickedObjectMenu : AbsUpdateable
    {
        /* Fields */
        Graphics.AbsCamera camera;
        Gui gui;
        GuiLayout layout;

        /* Constructors */
        public RaycastPickedObjectMenu(Graphics.AbsCamera camera, Gui gui)
            : base(true)
        {
            this.camera = camera;
            this.gui = gui;
        }

        /* Family Methods */
        public override void Time_Update(float time_ms)
        {
            if (Mouse.ButtonDownEvent(MouseButton.Left))
            {
                Ray ray = camera.CastRay(Input.Mouse.Position, Draw.graphicsDeviceManager.GraphicsDevice.Viewport);
                Debug.Log(ray.ToString());

                PlayState playState = Ref.gamestate as PlayState;
                ISpottedArrayCounter<AbsUpdateObj> counter = playState.GameObjCollection.AllMembersUpdateCounter;
                
                counter.Reset();
                while (counter.Next())
                {
                    AbsUpdateObj updateable = counter.GetSelection;
                    if (updateable is AbsVoxelObj)
                    {
                        AbsVoxelObj obj = (AbsVoxelObj)updateable;
                        if (obj.CollisionAndDefaultBound != null)
                        {
                            if (obj.CollisionAndDefaultBound.Bounds != null && obj.CollisionAndDefaultBound.Bounds.Length > 0)
                            {
                                AbsBound bounds = obj.CollisionAndDefaultBound.Bounds[0];

                                // ignore bounds type for now 
                                //if (obj.CollisionAndDefaultBound.Bounds[0].Type == LootFest.GO.Bounds.BoundShape.Cylinder)
                                {
                                    BoundingBox bb = new BoundingBox(bounds.center - bounds.halfSize, bounds.center + bounds.halfSize);
                                    //AABB aabb = new AABB(bounds.center, bounds.halfSize);
                                    
                                    float? t = ray.Intersects(bb);
                                    if (t.HasValue)
                                    {
                                        layout = GuiAutoReflection.PushClassInstanceLayout(obj, "Picked object", true, gui);
                                        layout.OnDelete += layout_OnDelete;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /* Event handlers */
        void layout_OnDelete()
        {
            DeleteMe();
            camera = null;
            gui = null;
            layout = null;
        }
    }
}

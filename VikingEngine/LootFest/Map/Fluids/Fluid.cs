//using VikingEngine.EngineSpace.Maths;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest.Map.Fluids
//{
//    class OctNode<T>
//    {
//        public T data;
//        public IntVector3 dataPos;

//        public OctNode<T> parent;
//        public IntVectorVolume volume;

//        public OctNode<T>[] children;

//        public OctNode(OctNode<T> parent, IntVectorVolume volume)
//        {
//            children = new OctNode<T>[8];
//            this.parent = parent;
//            this.volume = volume;
//        }

//        protected OctNode<T> FindChild(IntVector3 pos)
//        {
//            int index = 0;
//            if (pos.X >= volume.Position.X + volume.Size.X / 2)
//                index += 4;

//            if (pos.Y >= volume.Position.Y + volume.Size.Y / 2)
//                index += 2;

//            if (pos.Z >= volume.Position.Z + volume.Size.Z / 2)
//                index += 1;

//            return children[index];
//        }

//        public void SetAt(IntVector3 pos, T newData)
//        {
//            if (IsLeaf())
//            {
//                if (data == null || data.Equals(default(T)) || volume.Size == IntVector3.One)
//                {
//                    data = newData;
//                    dataPos = pos;
//                }
//                else
//                {
//                    Subdivide();
//                    FindChild(pos).SetAt(pos, newData);
//                }
//            }
//            else
//            {
//                FindChild(pos).SetAt(pos, newData);
//            }
//        }

//        public T GetDataAt(IntVector3 pos)
//        {
//            if (IsLeaf())
//            {
//                if ((data != null && !data.Equals(default(T))) && dataPos == pos)
//                    return data;
//                else
//                    return default(T);
//            }
//            else
//            {
//                return FindChild(pos).GetDataAt(pos);
//            }
//        }

//        public OctNode<T> GetAt(IntVector3 pos)
//        {
//            if (IsLeaf())
//            {
//                return this;
//            }
//            else
//            {
//                return FindChild(pos).GetAt(pos);
//            }
//        }

//        public void Subdivide()
//        {
//            if (IsLeaf())
//            {
//                IntVector3 size = volume.Size / 2;
//                if (size == IntVector3.Zero)
//                {
//                    return;
//                }
//                IntVector3 pos = volume.Position;

//                children[0] = new OctNode<T>(this, new IntVectorVolume(pos, size));
//                children[1] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(0, 0, size.Z), size));
//                children[2] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(0, size.Y, 0), size));
//                children[3] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(0, size.Y, size.Z), size));
//                children[4] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(size.X, 0, 0), size));
//                children[5] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(size.X, 0, size.Z), size));
//                children[6] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(size.X, size.Y, 0), size));
//                children[7] = new OctNode<T>(this, new IntVectorVolume(pos + new IntVector3(size.X, size.Y, size.Z), size));

//                if (data != null && !data.Equals(default(T)))
//                {
//                    OctNode<T> child = FindChild(dataPos);
//                    child.data = data;
//                    child.dataPos = dataPos;
//                    data = default(T);
//                    dataPos = IntVector3.Zero;
//                }
//            }
//        }

//        public void RemoveAt(IntVector3 pos)
//        {
//            OctNode<T> node = GetAt(pos);
//            node.data = default(T);

//            if (node.parent.Count() == 0)
//            {
//                node.parent.ClipBranches();
//            }
//        }

//        public void ClipBranches()
//        {
//            for(int i = 0; i < 8; ++i)
//            {
//                children[i] = null;
//            }
//        }

//        public int Count()
//        {
//            if (IsLeaf())
//            {
//                if (data != null && !data.Equals(default(T)))
//                    return 1;
//                else
//                    return 0;
//            }
//            else
//            {
//                int sum = 0;

//                for (int i = 0; i < 8; ++i)
//                    sum += children[i].Count();

//                return sum;
//            }
//        }

//        public bool IsLeaf()
//        {
//            return children[0] == null;
//        }

//        public List<OctNode<T>> GetAllLeaves()
//        {
//            List<OctNode<T>> result = new List<OctNode<T>>();

//            if (IsLeaf())
//            {
//                result.Add(this);
//            }
//            else
//            {
//                foreach(OctNode<T> child in children)
//                {
//                    result.AddRange(child.GetAllLeaves());
//                }
//            }

//            return result;
//        }
//    }

//    enum FluidType
//    {
//        Water,
//        Oil,
//        Lava,
//        QuickSand,
//    }

//    class FluidBlock
//    {
//        const float HEAT_RADIATION = 0.00001f;

//        public WorldPosition wp;
//        public float pressure;
//        float change;

//        float viscosity;

//        public Graphics.Mesh model;

//        float Yoffset;

//        FluidType type;

//        public FluidBlock(WorldPosition wp, float pressure, FluidType type)
//        {
//            this.wp = wp;
//            this.pressure = pressure;
//            change = 0;

//            switch(type)
//            {
//                case FluidType.Water:
//                    viscosity = 5f;
//                    model = new Graphics.Mesh(LoadedMesh.cube_repeating, wp.PositionV3, 
//                        new Graphics.TextureEffect(Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, Color.CornflowerBlue), 1f);
//                    model.Opacity = 0.5f;
//                    break;
//                case FluidType.Oil:
//                    viscosity = 20f;
//                    model = new Graphics.Mesh(LoadedMesh.cube_repeating, wp.PositionV3, new Graphics.TextureEffect(Graphics.TextureEffectType.FixedLight, SpriteName.WhiteArea, new Color(16, 0, 16)), 1f);
//                    model.Opacity = 0.9f;
//                    break;
//                case FluidType.Lava:
//                    viscosity = 15f;
//                    model = new Graphics.Mesh(LoadedMesh.cube_repeating, wp.WorldGrindex.Vec, new Graphics.TextureEffect(Graphics.TextureEffectType.FixedLight, SpriteName.MissingImage, Color.White), 1f);
//                    break;
//            }

//            model.Visible = false;
//            Yoffset = 0f;
//            this.type = type;

//            RecalculateModelSize();
//        }

//        FluidBlock GetBlockAt(IntVector3 pos, OctNode<FluidBlock> fluidBlocks, bool addIfMissing, float dt)
//        {
//            WorldPosition wp = new WorldPosition(pos);
//            if (false)//LfRef.chunks.GetScreen(wp).Get(wp) == byte.MinValue)
//            {
//                FluidBlock block = fluidBlocks.GetDataAt(pos);
//                if (block != null)
//                {
//                    return block;
//                }
//                else if (addIfMissing)
//                {
//                    block = new FluidBlock(wp, 0f, type);
//                    fluidBlocks.SetAt(pos, block);
//                    return block;
//                }
//            }
//            else
//            {
//                FluidBlock block = fluidBlocks.GetDataAt(pos);
//                if (block != null)
//                {
//                    block.model.DeleteMe();
//                    fluidBlocks.RemoveAt(pos);
//                }
//            }

//            return null;
//        }

//        float DrainAbove(FluidBlock from, float dt)
//        {
//            if (from != null)
//            {
//                if (from.pressure > 0f)
//                    return (dt / 1000f) / viscosity;
//            }
//            return 0f;
//        }

//        float FillBeneath(FluidBlock to, float dt)
//        {
//            if (to != null)
//            {
//                if (to.pressure < 1f)
//                    return -(dt / 1000f) / viscosity;
//            }
//            return 0f;
//        }

//        float ShareHorizontal(List<FluidBlock> adj, float dt)
//        {
//            float div = 0f;
//            foreach(FluidBlock fb in adj)
//            {
//                if (fb != null)
//                    div += fb.pressure - pressure;
//            }

//            return div * (dt / 1000f) / viscosity;
//        }

//        void RecalculateModelSize()
//        {
//            float t = MathHelper.Clamp(pressure, 0f, 1f);
//            model.ScaleY = MathExt.Lerp(0.1f, 1f, t);
//            Yoffset = (model.ScaleY - 1f) * 0.5f;
//            model.Position = new Vector3(model.GetPositionX, wp.WorldGrindex.Y + Yoffset, model.GetPositionZ);
//        }

//        public void Update(OctNode<FluidBlock> fluidBlocks, float dt, bool canAdd)
//        {
//            pressure += change - HEAT_RADIATION;
//            change = 0;
//            RecalculateModelSize();

//            FluidBlock above = GetBlockAt(wp.WorldGrindex + new IntVector3(CubeFace.Ypositive), fluidBlocks, false, dt);
//            FluidBlock below = GetBlockAt(wp.WorldGrindex + new IntVector3(CubeFace.Ynegative), fluidBlocks, canAdd, dt);

//            change += DrainAbove(above, dt);
//            change += FillBeneath(below, dt);

//            //if (below == null)
//            {
//                List<FluidBlock> adjacent = new List<FluidBlock>();
//                adjacent.Add(GetBlockAt(wp.WorldGrindex + new IntVector3(CubeFace.Xpositive), fluidBlocks, canAdd, dt));
//                adjacent.Add(GetBlockAt(wp.WorldGrindex + new IntVector3(CubeFace.Xnegative), fluidBlocks, canAdd, dt));
//                adjacent.Add(GetBlockAt(wp.WorldGrindex + new IntVector3(CubeFace.Zpositive), fluidBlocks, canAdd, dt));
//                adjacent.Add(GetBlockAt(wp.WorldGrindex + new IntVector3(CubeFace.Znegative), fluidBlocks, canAdd, dt));
//                change += ShareHorizontal(adjacent, dt);
//            }

//            change = MathHelper.Clamp(change, -pressure, 1f - pressure);

//            model.Visible = true;
//            if (pressure + change <= float.Epsilon)
//            {
//                model.DeleteMe();
//                fluidBlocks.RemoveAt(wp.WorldGrindex);
//            }
//        }
//    }

//    class FluidCollection : AbsUpdateable
//    {
//        OctNode<FluidBlock> fluidBlocks;

//        float timer;
//        const float ADD_DELTA = 1f;

//        WorldPosition lastCreated;

//        public FluidCollection(bool addToUpdateList, IntVectorVolume confinementArea)
//            : base(addToUpdateList)
//        {
//            fluidBlocks = new OctNode<FluidBlock>(null, confinementArea);
//            this.AddToUpdateList();
//        }

//        public void AddSource(WorldPosition wp, FluidType type)
//        {
//            lastCreated = wp;
//            fluidBlocks.SetAt(wp.WorldGrindex, new FluidBlock(wp, 1f, type));
//        }

//        public override void Time_Update(float time)
//        {
//            timer += time / 1000f;
//            bool canAdd = timer > ADD_DELTA;

//            float totalPressure = 0f;

//            List<OctNode<FluidBlock>> all = fluidBlocks.GetAllLeaves();
//            foreach (OctNode<FluidBlock> blockNode in all)
//            {
//                FluidBlock block = blockNode.data;
//                if (block != null)
//                {
//                    totalPressure += block.pressure;
//                    if (LfRef.chunks.GetScreen(block.wp).openstatus == ScreenOpenStatus.Mesh_3)
//                    {
//                        block.Update(fluidBlocks, time, canAdd);
//                    }
//                }
//            }

//            if (canAdd)
//            {
//                //AddSource(lastCreated);
//                timer -= ADD_DELTA;
//            }
//        }
//    }
//}

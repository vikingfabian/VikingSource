using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.ToGG.ToggEngine.Map
{
    class TileFurnishCollection : ToggEngine.GO.AbsTileObject
    {
        public MapFurnishType furnish;
        public List<AbsFurnishModel> models = new List<AbsFurnishModel>();

        bool isStompable = false;
        bool isCritter = false;

        public TileFurnishCollection(IntVector2 pos, object args)
            : base(pos)
        {
            if (args != null)
            {
                furnish = (MapFurnishType)args;
            }
        }

        public override void onLoadComplete()
        {
            base.onLoadComplete();
            initModels();
        }

        void initModels()
        {
            Ref.rnd.SetSeed(position.X * 11 + position.Y * 21);

            switch (furnish)
            {
                case MapFurnishType.Box:
                    {
                        isStompable = true;

                        Range count = new Range(1, 3);
                        List<Vector3> pos = positions(count.GetRandom(), 2);
                        foreach (var m in pos)
                        {
                            new BoxModel(m, position, this);
                        }
                    }
                    break;
                case MapFurnishType.Rat:
                    {
                        isCritter = true;

                        Range count = new Range(1, 2);
                        List<Vector3> pos = positions(count.GetRandom(), 2);
                        foreach (var m in pos)
                        {
                            new CritterRatModel(m, position, this);
                        }
                    }
                    break;
                case MapFurnishType.Sparrow:
                    {
                        isCritter = true;

                        Range count = new Range(2, 5);
                        List<Vector3> pos = positions(count.GetRandom(), 3);
                        foreach (var m in pos)
                        {
                            new CritterSparrowModel(m, position, this);
                        }
                    }
                    break;
            }
        }

        List<Vector3> positions(int count, int areaDivisions)
        {
            List<Vector3> result = new List<Vector3>(count);

            int split = areaDivisions + 1;
            float stepW = 1f / split;
            float randomAdj = stepW * 0.1f;

            List<Vector3> available = new List<Vector3>(MathExt.Square(areaDivisions));
            Vector3 center = toggRef.board.toWorldPos_TopLeft(this.position, 0);
            center.X += stepW;
            center.Z += stepW + 0.02f;

            ForXYLoop loop = new ForXYLoop(new IntVector2(areaDivisions));
            while (loop.Next())
            {
                Vector3 pos = center;
                pos.X += stepW * loop.Position.X + Ref.rnd.Plus_MinusF(randomAdj);
                pos.Z += stepW * loop.Position.Y + Ref.rnd.Plus_MinusF(randomAdj);

                available.Add(pos);
            }

            for (int i = 0; i < count; ++i)
            {
                Vector3 pos = arraylib.RandomListMemberPop(available);

                addInZorder(pos, result);
            }

            return result;
        }

        public override void newPosition(IntVector2 newpos)
        {
            IntVector2 diff = newpos - position;
            base.newPosition(newpos);

            Vector3 move = VectorExt.V3FromXZ(diff.Vec, 0);

            foreach (var m in models)
            {
                m.model.Position += move;
            }
        }

        void addInZorder(Vector3 pos, List<Vector3> result)
        {
            for (int resultIx = 0; resultIx < result.Count; ++resultIx)
            {
                if (pos.Z < result[resultIx].Z)
                {
                    result.Insert(resultIx, pos);
                    return;
                }
            }
            result.Add(pos);
        }

        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);
            w.Write((byte)furnish);
        }
        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);
            furnish = (MapFurnishType)r.ReadByte();

            initModels();
        }

        public override void stompOnTile(ToggEngine.GO.AbsUnit unit, IntVector2 onSquare, bool soundOnly, bool local)
        {
            if (onSquare == position)
            {
                if (models.Count > 0)
                {
                    if (local)
                    {
                        if (soundOnly)
                        {
                            if (!isCritter)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (!isStompable)
                            {
                                return;
                            }
                        }
                    }

                    foreach (var m in models)
                    {
                        m.onStomp(onSquare, local);
                    }

                    models.Clear();
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            foreach (var m in models)
            {
                m.DeleteModel();
            }
        }

        override public ToggEngine.TileObjectType Type { get { return ToggEngine.TileObjectType.Furnishing; } }
    }

    
    enum MapFurnishType
    {
        Box,
        Rat,
        Sparrow,

        NUM_NONE
    }
}

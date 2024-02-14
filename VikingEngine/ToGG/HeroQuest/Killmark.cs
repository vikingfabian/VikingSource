using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest
{
    class KillmarkTile : ToggEngine.GO.AbsTileObject
    {
        public CirkleCounterUp removeNext = new CirkleCounterUp(0, Killmark.PlacementCount -1);
        public List<Killmark> marks = new List<Killmark>(3);

        public KillmarkTile(IntVector2 pos)
            :base(pos)
        {
        }

        override public bool SaveToStorage { get { return false; } }
        override public TileObjectType Type { get { return TileObjectType.Killmarks; } }
    }

    class Killmark : ToggEngine.GO.AbsGameObject
    {
        static readonly Vector2[] Placements = new Vector2[]
               {
                    new Vector2(0.0f, -0.2f),
                    new Vector2(-0.3f, -0.3f),
                    new Vector2(0.3f, -0.25f),
                    new Vector2(-0.27f, -0.10f),
                    new Vector2(0.33f, -0.05f),
                    new Vector2(0.03f, 0.05f),
               };
        public static readonly int PlacementCount = Placements.Length;

        //Graphics.VoxelModel model;
        public int placementIndex;

        public Killmark(KillMarkVisuals visuals, IntVector2 pos)
            :base(true)
        {
            ushort seed = Ref.rnd.Ushort();
            init(visuals, pos, seed);

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqKillMark, Network.PacketReliability.Reliable);
            w.Write((byte)visuals);
            toggRef.board.WritePosition(w, pos);
            w.Write(seed);
        }

        public Killmark(Network.ReceivedPacket packet)
             : base(true)
        {
            KillMarkVisuals visuals = (KillMarkVisuals)packet.r.ReadByte();
            IntVector2 pos = toggRef.board.ReadPosition(packet.r);
            ushort seed = packet.r.ReadUInt16();

            init(visuals, pos, seed);
        }

        void init(KillMarkVisuals visuals, IntVector2 pos, ushort seed)
        {
            float scale = 0.32f;

            var sq = toggRef.board.tileGrid.Get(pos);
            KillmarkTile markTile = sq.tileObjects.GetObject(TileObjectType.Killmarks) as KillmarkTile;
            if (markTile == null)
            {
                markTile = new KillmarkTile(pos);
            }

            if (markTile.marks.Count < PlacementCount)
            {
                placementIndex = markTile.marks.Count;
                markTile.marks.Add(this);
            }
            else
            {//Replace old mark
                placementIndex = markTile.removeNext.Next();
                markTile.marks[placementIndex].DeleteModel();
                markTile.marks[placementIndex] = this;
            }

            Vector3 center = VectorExt.AddXZ(toggRef.board.toWorldPos_Center(pos, -scale * 2f), Placements[placementIndex]);
            Ref.rnd.SetSeed(seed);
            center.X += Ref.rnd.Plus_MinusF(0.05f);
            center.Z += Ref.rnd.Plus_MinusF(0.02f);


            SpriteName sprite = SpriteName.MissingImage;
            switch (visuals)
            {
                case KillMarkVisuals.Flower: sprite = SpriteName.KillMarkSkullflower; break;
                case KillMarkVisuals.Sword: sprite = SpriteName.KillMarkSwordNecklace; break;
                case KillMarkVisuals.GraveStone: sprite = SpriteName.KillMarkGraveStone; break;
                case KillMarkVisuals.Rose: sprite = SpriteName.KillMarkRose; break;
            }

            //model = new Graphics.VoxelModel(true);
            //model.Effect = cmdLib.ModelEffect;

            //ImageFile2 imageFile = DataLib.Images.GetImageSource(sprite);

            //List<PolygonColor> polygons = new List<PolygonColor>
            //{
            //    cmdLib.CamFacingPolygon(Vector3.Zero, new Vector2(scale), imageFile, Color.White)
            //};

            //model.BuildFromPolygons(new Graphics.PolygonsAndTrianglesColor(polygons, null),
            //    new List<int> { polygons.Count }, LoadedTexture.LF3Tiles);
            createModel(sprite, center, new Vector2(scale), false);

            //model.position = center;
        }

        public override void Time_Update(float time_ms)
        {
            //Slide upwards
            if (Ref.TimePassed16ms)
            {
                float diff = groundY - model.position.Y;
                model.position.Y += diff * 0.15f;

                if (diff <= 0.001f)
                {
                    model.position.Y = groundY;
                    DeleteMe();
                }
            }
        }

        
    }

    enum KillMarkVisuals
    {
        Sword,
        Flower,
        GraveStone,
        Rose,
    }
}

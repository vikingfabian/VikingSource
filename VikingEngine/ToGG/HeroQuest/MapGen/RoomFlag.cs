using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    class RoomFlag : ToggEngine.GO.AbsTileObject
    {
        public AbsRoomFlagSettings settings = null;
        Graphics.Text3DBillboard model;

        public List<IntVector2> tiles = new List<IntVector2>();
        public List<IntVector2> wallAdjTiles = new List<IntVector2>();
        public Rectangle2 tileArea;
        public bool heroStartArea = false;
        public float currrentSpawnDifficulty = 0;

        public RoomFlag(IntVector2 pos, object args)
            : base(pos)
        {
            if (args != null)
            {
                settings = (AbsRoomFlagSettings)args;
            }
            else
            {
                settings = new AreaTypeSpawn();
            }

            if (toggRef.InEditor)
            {
                model = new Graphics.Text3DBillboard(LoadedFont.Console, "RoomXXXX" + Environment.NewLine + "_", Color.White, Color.DarkBlue, Vector3.Zero,
                    0.2f, 1f, true);
                model.FaceCamera = false;
                refreshModel();
            }
            newPosition(pos);
        }

        public void refreshModel()
        {
            if (model != null)
            {
                model.TextString = "Room" + Environment.NewLine + settings.ShortName;
            }
        }

        public void collectTiles()
        {
            bool[,] usedTiles = new bool[toggRef.board.Size.X, toggRef.board.Size.Y];
            tiles.Clear();
            wallAdjTiles.Clear();
            heroStartArea = false;
            tileArea = new Rectangle2(position, IntVector2.One);

            collectAdjacentTiles(position);

            void collectAdjacentTiles(IntVector2 center)
            {
                if (toggRef.board.tileGrid.InBounds(center) &&
                    !usedTiles[center.X, center.Y] &&
                    position.SideLength(center) <= settings.maxTileRadius)
                {
                    usedTiles[center.X, center.Y] = true;
                    tiles.Add(center);
                    if (isWallTile(center))
                    {
                        wallAdjTiles.Add(center);
                    }
                    tileArea.includeTile(center);

                    foreach (var dir in IntVector2.Dir4Array)
                    {
                        IntVector2 adj = center + dir;
                        var sq = toggRef.Square(adj);
                        if (sq != null)
                        {
                            if (sq.tag.tagType == ToggEngine.Map.SquareTagType.HeroStart)
                            {
                                heroStartArea = true;
                            }
                            else if (sq.tag.tagType != ToggEngine.Map.SquareTagType.RoomDivider &&
                            sq.IsRoomDivider() == false)
                            {
                                collectAdjacentTiles(adj);
                            }
                        }
                    }
                }
            }

            bool isWallTile(IntVector2 tile)
            {
                bool wallAdj = false;

                foreach (var dir in IntVector2.Dir8Array)
                {
                    var sq = toggRef.Square(tile + dir);
                    if (sq != null)
                    {
                        if (dir.IsOrthogonal() && sq.IsWall)
                        {
                            wallAdj = true;
                        }

                        if (sq.tileObjects.HasObject(ToggEngine.TileObjectType.Door) ||
                            sq.tag.tagType == ToggEngine.Map.SquareTagType.RoomDivider)
                        {
                            return false;
                        }
                    }
                }

                return wallAdj;
            }
        }
        
        public override void Write(System.IO.BinaryWriter w)
        {
            base.Write(w);

            w.Write((byte)settings.Type);
            settings.Write(w);
        }
        public override void Read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            base.Read(r, version);

            SpawnSettingsType settingsType = (SpawnSettingsType)r.ReadByte();
            settings = AbsRoomFlagSettings.Create(settingsType);
            settings.Read(r, version);

            refreshModel();
        }

        public override void newPosition(IntVector2 newpos)
        {
            base.newPosition(newpos);
            if (model != null)
            {
                model.Position = toggRef.board.toWorldPos_Center(position, 0.05f);
                model.Z -= 0.2f;
            }
        }

        public override void DeleteMe()
        {
            model?.DeleteMe();
            base.DeleteMe();
        }

        public RoomFlag connectedFlag()
        {
            var sq = toggRef.Square(VectorExt.AddX(position, -1));
            if (sq != null)
            {
                return sq.tileObjects.GetObject(ToggEngine.TileObjectType.RoomFlag) as RoomFlag;
            }

            return null;
        }

        override public ToggEngine.TileObjectType Type { get { return ToggEngine.TileObjectType.RoomFlag; } }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DataStream;

namespace VikingEngine.ToGG.MoonFall
{
    class Map
    {
        const int Version = 0;
        const string FileName = "MoonFallMapdata";
        const string FileEnd = ".sav";
        public static float MapScale;

        Texture2D mapTex;
        Graphics.ImageAdvanced mapImage;
        public List<MapArea> areas = new List<MapArea>();
        int mapNumber = 1;
        public Vector2 cameraPos = Vector2.Zero;
        float texHeight;
        Vector2 texTopLeft;

        public float soldierHeight;
        public Map()
        {
            moonRef.map = this;
            MapScale = Engine.Screen.IconSize / 16f;

            new Timer.AsynchActionTrigger(load_asynch, true);

            texHeight = Engine.Screen.SafeArea.Height;
            texTopLeft = Engine.Screen.SafeArea.Position;

            soldierHeight = MathExt.Round(texHeight * 0.02f);
        }

        void load_asynch()
        {
            mapTex = Ref.main.Content.Load<Texture2D>(moonLib.ContentPath + "moonfallmap" + mapNumber.ToString());
            saveload(false);
            new Timer.Action0ArgTrigger(loadingComplete);
        }

        void loadingComplete()
        {
            Ref.draw.CurrentRenderLayer = Draw.MapLayer;
            {
                mapImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                 texTopLeft,
                 new Vector2(texHeight / mapTex.Height * mapTex.Width, texHeight),
                 ImageLayers.Background5, false);
                mapImage.Texture = mapTex;
                mapImage.SetFullTextureSource();

                foreach (var m in areas)
                {
                    m.createVisuals();
                }

            } Ref.draw.CurrentRenderLayer = Draw.HudLayer;

            moonRef.playState.mapLoadingComplete();
        }

        public void saveload(bool save)
        {
            DataStream.FilePath path = new DataStream.FilePath(null, FileName + mapNumber.ToString(), 
                FileEnd,
                save || PlatformSettings.DevBuild, false);

            if (!path.Storage)
            {
                path.LocalDirectoryPath = moonLib.ContentPath;//PjLib.ContentFolder;
            }

            if (save || path.Exists())
            {
                DataStream.BeginReadWrite.BinaryIO(save, path, write, read_asynch, null, save);
            }
        }

         

        public void write(System.IO.BinaryWriter w)
        {
            //MapArea.NextIndex = 0;
            //foreach (var m in areas)
            //{
            //    m.index = MapArea.NextIndex++;
            //}

            w.Write(Version);
            w.Write(areas.Count);
            foreach (var m in areas)
            {
                m.write(w);
            }

            foreach (var m in areas)
            {
                m.writeConnections(w);
            }
        }

        MemoryStreamHandler memory = null;
        public void read_asynch(System.IO.BinaryReader r)
        {
            memory = new MemoryStreamHandler();
            memory.readToMemory(r);

            new Timer.Action0ArgTrigger(read);
        }

        //public void SaveComplete(bool save, int player, bool completed, byte[] value)
        //{
        //    if (!save)
        //    {
        //        read();
        //    }
        //}

        public void read()
        {
            System.IO.BinaryReader r = memory.GetReader();
            memory = null;

            int version = r.ReadInt32();

            int areaCount = r.ReadInt32();
            for (int i = 0; i < areaCount; ++i)
            {
                var area = new MapArea(Vector2.Zero);
                area.read(r, version);

                areas.Add(area);
            }

            foreach (var m in areas)
            {
                m.readConnections(r, version);
            }
        }

        public MapNode pointingOnNode()
        {
            Vector2 pos = pointerPos;
            foreach (var ar in areas)
            {
                if (ar.selectionArea.IntersectPoint(pos))
                {
                    return ar;
                }

                foreach (var place in ar.placementNodes)
                {
                    if (place.selectionArea.IntersectPoint(pos))
                    {
                        return place;
                    }
                }
            }
            return null;
        }

        public Vector2 pointerPos
        {
            get { return Input.Mouse.Position - cameraPos; }
        }

        public Vector2 mapToRelativePos(Vector2 pos)
        {
            pos -= texTopLeft;
            return pos / texHeight;
        }

        public Vector2 relativeToMapPos(Vector2 pos)
        {
            pos *= texHeight;
            return pos + texTopLeft;
        }

        public void writeMapPos(System.IO.BinaryWriter w, Vector2 pos)
        {
            SaveLib.WriteVector(w, mapToRelativePos(pos));
        }
        public Vector2 readMapPos(System.IO.BinaryReader r)
        {
            var pos = relativeToMapPos(SaveLib.ReadVector2(r));
            return pos;
        }

        public bool isLoaded { get { return mapImage != null; } }
    }
}

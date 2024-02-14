using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Strategy
{
    class Map
    {
        const int Version = 0;
        const string FileName = "worldmapdata";
        const string FileEnd = ".sav";
        public static float MapScale;

        Texture2D mapTex;
        Graphics.ImageAdvanced mapImage;
        public List<MapArea> areas = new List<MapArea>();
        int mapNumber = 1;
        public Vector2 cameraPos = Vector2.Zero;

        public MapArea selectedArea = null;
        public AreaConnection selectedConnection = null;

        public Map()
        {
            StrategyRef.map = this;
            MapScale = Engine.Screen.IconSize / 16f;

            new Timer.AsynchActionTrigger(load_asynch, true);
        }

        void load_asynch()
        {
            mapTex = Ref.main.Content.Load<Texture2D>(PjLib.ContentFolder + "worldmap" + mapNumber.ToString());
            saveload(false);
            new Timer.Action0ArgTrigger(loadingComplete);
        }

        void loadingComplete()
        {
            StrategyLib.SetMapLayer();

            mapImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                Vector2.Zero, new Vector2(mapTex.Width, mapTex.Height) * MapScale, ImageLayers.Background5, false);
            mapImage.Texture = mapTex;
            mapImage.SetFullTextureSource();

            foreach (var m in areas)
            {
                m.createVisuals(selectedArea);
                m.refreshVisuals(true, selectedArea);
            }

            ((AbsStrategyState)Ref.gamestate).mapLoadingComplete();
        }

        public void saveload(bool save)
        {
            DataStream.FilePath path = new DataStream.FilePath(null, FileName + mapNumber.ToString(), FileEnd,
                save || PlatformSettings.DevBuild, false);

            if (!path.Storage)
            {
                path.LocalDirectoryPath = PjLib.ContentFolder;
            }

            if (save || path.Exists())
            {
                DataStream.BeginReadWrite.BinaryIO(save, path, WriteStream, ReadStream, null, save);
            }
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            MapArea.NextIndex = 0;
            foreach (var m in areas)
            {
                m.index = MapArea.NextIndex++;
            }

            w.Write(Version);
            w.Write(areas.Count);
            foreach (var m in areas)
            {
                m.write(w);
            }
        }

        public void ReadStream(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();

            int areaCount = r.ReadInt32();
            for (int i = 0; i < areaCount; ++i)
            {
                areas.Add(new MapArea());
            }

            foreach (var m in areas)
            {
                m.read(r, version, areas);
            }
        }

        public MapArea pointingOnArea()
        {
            Vector2 pos = pointerPos;
            foreach (var m in areas)
            {
                if (m.selectionArea.IntersectPoint(pos))
                {
                    return m;
                }
            }
            return null;
        }

        public Vector2 pointerPos
        {
            get { return Input.Mouse.Position - cameraPos; }
        }


        public bool isLoaded { get { return mapImage != null; } }
    }
}

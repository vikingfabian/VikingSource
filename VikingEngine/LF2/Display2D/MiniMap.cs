using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LF2.GameObjects.Characters;

namespace VikingEngine.LF2
{
    interface IMiniMapLocation
    {
        IntVector2 MapLocationChunk { get; }
        IntVector2 TravelEntrance { get; }
        SpriteName MiniMapIcon { get; }
        string MapLocationName { get; }
        bool TravelTo { get; }
        bool VisibleOnMiniMap { get; }
    }
   
    static class MiniMapData
    {
        public static List<IMiniMapLocation> Locations;
        public static void NewWorld()
        {
            Locations = new List<IMiniMapLocation>();
        }
    }
    class MiniMap : AbsUpdateable
    {
        
        const int LayerQuestPos = -5;
        const int LayerFlag = -4;
        const int LayerCameraDir = -3;
        const int LayerHeroPos = -2;
        public const int LayerRemoteGamer = -1;
        

        public static readonly Vector2 ChunkToPercentPos = 
            new Vector2(1f / Map.WorldPosition.WorldChunksX, 1f / Map.WorldPosition.WorldChunksY);
        const float ScrollSpeed = 0.0015f;
        const float ZoomSpeed = -0.01f;
        Graphics.RenderTargetImage mapTexture;
        VectorRect screenArea;
        MiniMapSettings settings;
        List<MapIcon> icons;
        MapIcon goalFlag;
        TextG locationName;
        Image locationNameBackg;
        Image pointer;
        Image travelSelection;
        Vector2 center;
        bool travel;
        public bool Travel
        {
            get { return travel; }
        }
        IMiniMapLocation travelLocation;
        public IMiniMapLocation TravelLocation
        {
            get { return travelLocation; }
        }
        public const int ChunksDivide = 4;
        public static readonly IntVector2 MinimapAreas = 
            new IntVector2(Map.WorldPosition.WorldChunksX / ChunksDivide, Map.WorldPosition.WorldChunksY / ChunksDivide);

        public const ImageLayers MapBackgroundLayer = ImageLayers.Lay6;

        public MiniMap(VectorRect screenArea, MiniMapSettings settings, Hero hero,
            bool travel, Graphics.AbsCamera camera, Map.MiniMapGoal goalFlagData, 
            bool viewNewLocationIcon, IntVector2 newLocationIconPos)
            :base(true)
        {
           this.screenArea = screenArea;
            MapIcon.IconSize = new Vector2(lib.SmallestOfTwoValues(Engine.Screen.Width, Engine.Screen.Height) * 0.06f);

            locationName = new TextG(LoadedFont.CartoonLarge, new Vector2(screenArea.Center.X, screenArea.Y + 92), Vector2.One * 0.8f,
                Align.CenterAll, TextLib.EmptyString, Color.Yellow, ImageLayers.AbsoluteTopLayer);
            locationNameBackg = new Image(SpriteName.WhiteArea, locationName.Center, new Vector2(400, 70), ImageLayers.Foreground1, true);
            locationNameBackg.Ypos -= 10;
            locationNameBackg.Color = Color.Black;
            locationNameBackg.Transparentsy = 0.5f;

            Color[] environmentColor = new Color[(int)Map.Terrain.EnvironmentType.NUM_NON];
            environmentColor[(int)Map.Terrain.EnvironmentType.Grassfield] = new Color(124, 197, 118);// Color.Green;
            environmentColor[(int)Map.Terrain.EnvironmentType.Forest] = new Color(145, 189, 60);//Color.DarkGreen;
            environmentColor[(int)Map.Terrain.EnvironmentType.Swamp] = new Color(151,165,32);//Color.Brown;
            environmentColor[(int)Map.Terrain.EnvironmentType.Desert] = new Color(213,172,83);//Color.SandyBrown;
            //const byte ReallyDarkGray = 60;
            environmentColor[(int)Map.Terrain.EnvironmentType.Burned] = new Color(170, 154, 118);//new Color(ReallyDarkGray,ReallyDarkGray,ReallyDarkGray);
            environmentColor[(int)Map.Terrain.EnvironmentType.Mountains] = new Color(131, 163, 115);// Color.Gray;

            //create background texture
            IntVector2 pos = IntVector2.Zero;
            IntVector2 chunkPos = IntVector2.Zero;
            List<AbsDraw> mapPixels = new List<AbsDraw>();
            const float MiniMapPixelWidth =4;
            mapPixels.Capacity = MinimapAreas.X * MinimapAreas.Y;
            for (pos.Y = 0; pos.Y < MinimapAreas.Y; pos.Y++)
            {
                chunkPos.Y = pos.Y * ChunksDivide;
                for (pos.X = 0; pos.X < MinimapAreas.X; pos.X++)
                {
                    if (LfRef.gamestate.Progress.GetVisitedArea(pos))
                    {
                        chunkPos.X = pos.X * ChunksDivide;
                        Color col = environmentColor[(int)LfRef.worldOverView.GetChunkData(chunkPos).Environment];
                        Image img = new Image(SpriteName.WhiteArea, pos.Vec * MiniMapPixelWidth, Vector2.One * MiniMapPixelWidth, ImageLayers.AbsoluteTopLayer);

                        img.Color = col;
                        img.DeleteMe();
                        mapPixels.Add(img);
                    }
                }
            }
            if (Ref.draw.IsSplitScreen)
                mapTexture = new RenderTargetImage_AreaLimited(Vector2.Zero, MinimapAreas.Vec * MiniMapPixelWidth, MapBackgroundLayer, screenArea);
            else
                mapTexture = new RenderTargetImage(Vector2.Zero, MinimapAreas.Vec * MiniMapPixelWidth, MapBackgroundLayer);
            mapTexture.ClearColor = Color.Black;
            mapTexture.DrawImagesToTarget(mapPixels, true);


            this.travel = travel;
            center = screenArea.Center;
            pointer = new Image(SpriteName.LFMenuPointer, center, new Vector2(32), ImageLayers.AbsoluteTopLayer);
            //pointer.Ypos += 16;
            pointer.CenterRelative = new Vector2(PublicConstants.Half, 0);

            this.settings = settings;
            
            
            //collect icons of buildings and such
            icons = new List<MapIcon>();
            foreach (IMiniMapLocation l in MiniMapData.Locations)
            {
                if (l.VisibleOnMiniMap && LfRef.gamestate.Progress.GetVisitedArea(l.MapLocationChunk / ChunksDivide))
                    icons.Add(new MapIcon(l));//new MapIcon(l.MiniMapIcon, l.MapLocationChunk, 0, l, 0));
            }
            this.settings.Position = hero.ScreenPos.Vec * ChunkToPercentPos;

            //other players
            List<Players.AbsPlayer> players = LfRef.gamestate.AllPlayers();
            foreach (Players.AbsPlayer p in players)
            {
                if (p != hero.Player)
                {
                    icons.Add(new GamerMapIcon(p, p.hero == hero));
                }
            }

            
            //visar kamerans riktning
            icons.Add(new MapIcon(hero.ScreenPos, SpriteName.MapCameraAngle, LayerCameraDir, camera.TiltX - MathHelper.PiOver2));

            //visar gubbens riktning
            icons.Add(new MapIcon(hero.ScreenPos, SpriteName.BoardPieceCenter, LayerHeroPos, hero.Rotation.Radians));//camera.TiltX - MathHelper.PiOver2));

            goalFlag = new MapIcon(goalFlagData.Chunk, SpriteName.IconMapFlag, LayerFlag, 0);
            goalFlag.Visible = goalFlagData.Visible;
            icons.Add(goalFlag);
            if (LfRef.gamestate.Progress.ViewMapGoal)
            {
                icons.Add(new MapIcon(LfRef.gamestate.Progress.questPos(), SpriteName.IconMapQuest, LayerQuestPos, 0));
            }

            if (viewNewLocationIcon)
                icons.Add(new NewLocationIcon(newLocationIconPos));

            //beräkna delen av kartan som visas
            
            travelSelection = new Image(SpriteName.InterfaceBorder, Vector2.Zero, MapIcon.IconSize * 1.4f, ImageLayers.Foreground3, true);
            travelSelection.Color = Color.CornflowerBlue;
        
            updateMap();

        }

        void updateMap()
        {
            mapTexture.Size = settings.Size;
            mapTexture.Position = (center) - mapTexture.Size * settings.Position;

            //make sure the map aint outside the screen
            mapTexture.ViewArea(screenArea, false);

            int closestIx = updateIcons();
           
            travelSelection.Position = icons[closestIx].Position;
            travelSelection.Visible = screenArea.IntersectPoint(travelSelection.Position);
            travelLocation = icons[closestIx].Location;
       
            IMiniMapLocation loc = icons[closestIx].Location;
            if (loc == null)
            {
                locationName.TextString = "--ERROR--";
            }
            else
                 locationName.TextString = loc.MapLocationName;
            locationName.Centertext(Align.CenterAll);
        }

        int updateIcons()
        {
            VectorRect backgroundArea = new VectorRect(mapTexture.Position, mapTexture.Size);
            float closest = float.MaxValue;
            int closestIx = 0;
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].Update(settings, screenArea, backgroundArea);

                if (icons[i].Location != null && (icons[i].Location.TravelTo || !travel))
                {
                    Vector2 diff = icons[i].RelPosition - settings.Position;
                    float l = diff.Length();
                    if (l < closest)
                    {
                        closest = l;
                        closestIx = i;
                    }
                }
            }
            return closestIx;
        }

        public override void Time_Update(float time)
        {
            updateIcons();
        }

        public void PlaceFlag(ref Map.MiniMapGoal goalFlagData)
        {
            IntVector2 chunk = PointingAtChunk();
            if (goalFlagData.Visible && goalFlagData.Chunk == chunk)
            {
                goalFlagData.Visible = false;
            }
            else
            {
                goalFlagData.Chunk = chunk;
                goalFlagData.Visible = true;
            }

            //update icon
            goalFlag.ChunkPos = chunk;
            goalFlag.Visible = goalFlagData.Visible;
            updateMap();

        }

        public IntVector2 PointingAtChunk()
        {
            return new IntVector2(settings.Position * new Vector2(Map.WorldPosition.WorldChunksX, Map.WorldPosition.WorldChunksY));
        }

        public void Scroll(Vector2 dir)
        {
            float move = ScrollSpeed / settings.Scale;
            settings.Position.X = Bound.Set(settings.Position.X + dir.X * move, 0, 1);
            settings.Position.Y = Bound.Set(settings.Position.Y + dir.Y * move, 0, 1);

            updateMap();
        }
        public void Scale(float add)
        {
            settings.Scale += add * ZoomSpeed;
            updateMap();
        }
       
        public MiniMapSettings DeleteMap()
        {
            this.DeleteMe();
            locationName.DeleteMe();
            locationNameBackg.DeleteMe();
            travelSelection.DeleteMe();
            pointer.DeleteMe();
            mapTexture.DeleteMe();
            foreach (MapIcon i in icons)
            {
                i.DeleteMe();
            }
            return settings;
        }
    }
    class MapIcon
    {
        public static Vector2 IconSize;// = new Vector2(32);
        Vector2 relPos;
        protected Image icon;
        protected Vector2 chunkPos;
        bool visible = true;

        public IntVector2 ChunkPos
        {
            get { return new IntVector2(chunkPos); }
            set 
            {
                chunkPos = value.Vec;
                updateRelPos();
            }
        }
        public bool Visible
        {
            set { visible = value; }
        }
        IMiniMapLocation location;
        public IMiniMapLocation Location
        {
            get { return location; }
        }

        public Vector2 RelPosition
        {
            get { return relPos; }
        }
        public Vector2 Position
        {
            get { return icon.Position; }
        }

        public MapIcon(IMiniMapLocation location)
            : this(location, 0, 0)
        { }
        public MapIcon(IMiniMapLocation location, int layerAdd, float rotation)
            :this(location.MapLocationChunk, location.MiniMapIcon, layerAdd, rotation)
        {
            this.location = location;
        }

        public MapIcon(IntVector2 pos, SpriteName imageTile, int layerAdd, float rotation)
        {
            chunkPos =pos.Vec;
            updateRelPos();
            icon = new Image(imageTile, 
                Vector2.Zero, IconSize, ImageLayers.Foreground2, true);
            icon.PaintLayer += layerAdd * PublicConstants.LayerMinDiff;
            icon.Rotation = rotation;
        }

        protected void updateRelPos()
        {
            relPos = chunkPos * MiniMap.ChunkToPercentPos;
        }

        virtual public void Update(MiniMapSettings settings, VectorRect safeAre, VectorRect backgroundPos)
        {
            icon.Position = backgroundPos.Position + backgroundPos.Size * relPos;
            icon.Visible = visible && safeAre.IntersectPoint(icon.Position);
        }
        virtual public void DeleteMe()
        {
            icon.DeleteMe();
        }
    }

    class NewLocationIcon : MapIcon
    {
        Graphics.Motion2d pulse;
        public NewLocationIcon(IntVector2 pos)
            : base(pos, SpriteName.InterfaceStar, -2, 0)
        {
            icon.Color = Color.Yellow;
            icon.Transparentsy = 0.8f;
            pulse = new Graphics.Motion2d(MotionType.SCALE, icon, 
                icon.Size * 0.3f, MotionRepeate.BackNForwardLoop, 400, true);
            chunkPos += new Vector2(0.4f);
        }
        public override void Update(MiniMapSettings settings, VectorRect safeArea, VectorRect backgroundPos)
        {
            base.Update(settings, safeArea, backgroundPos);
            icon.Position -= IconSize * 0.4f;
        }
        public override void DeleteMe()
        {
            base.DeleteMe();
            pulse.DeleteMe();
        }
    }

    class GamerMapIcon : MapIcon
    {
        Players.AbsPlayer player;
        //const int Layer = -1;
        Graphics.Line line;
        Graphics.Image flagPos;

        public GamerMapIcon(Players.AbsPlayer player, bool parent)
            : base(player.hero, 0, 0)
        {
            this.player = player;
            icon.DeleteMe();
            icon = new GamerPictureInstance(player.gamerPicture, 
                Vector2.Zero, IconSize, ImageLayers.Foreground2);
            icon.CenterImage();
            icon.PaintLayer += MiniMap.LayerRemoteGamer * PublicConstants.LayerMinDiff;

            if (!parent)
            {
                line = new Line(1, ImageLayers.Foreground2, Color.White, Vector2.Zero, Vector2.Zero);
                line.PaintLayer += (MiniMap.LayerRemoteGamer + 1) * PublicConstants.LayerMinDiff;
                line.Visible = false;

                flagPos = new Image(SpriteName.IconMapFlag, Vector2.Zero, new Vector2(16), ImageLayers.AbsoluteTopLayer, true);
                flagPos.Color = Color.LightGray;
                flagPos.PaintLayer = line.PaintLayer;
                flagPos.Visible = false;
            }
        }
        public override void Update(MiniMapSettings settings, VectorRect safeAre, VectorRect backgroundPos)
        {
            chunkPos = player.hero.WorldPosition.ChunkPosFloating;
            icon.Rotation = player.hero.Rotation.Radians;
            
            updateRelPos();
            base.Update(settings, safeAre, backgroundPos);
            icon.Visible = icon.Visible && player.hero.Alive;

            if (line != null)
            {
                //relPos = chunkPos * MiniMap.ChunkToPercentPos;
                Map.MiniMapGoal goal =  player.AbsPlayerProgress.mapFlag;
                line.Visible = goal.Visible;
                flagPos.Visible = goal.Visible;

                if (goal.Visible)
                {
                    flagPos.Position = backgroundPos.Position + backgroundPos.Size * goal.Chunk.Vec * MiniMap.ChunkToPercentPos;
                    line.PointPos(true, icon.Position);
                    line.PointPos(false, flagPos.Position);
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            if (line != null)
            {
                line.DeleteMe();
                flagPos.DeleteMe();
            }
        }
    }
    struct MiniMapSettings
    {
        public Vector2 Position;
        float scale;
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = Bound.Set(value, 1, 20);
            }
        }
        public Vector2 Size
        {
            get
            {
                return new Vector2(Scale * Map.WorldPosition.WorldChunksX, Scale * Map.WorldPosition.WorldChunksY);
            }
        }
    }
}

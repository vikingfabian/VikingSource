using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.Input;

namespace VikingEngine.DSSWars.Map
{
    
    class MapDetailLayerManager
    {
        public static readonly float SelectUnitZoomIn = FactionZoom.Min - 0.4f;
        public static MapDetailLayerManager[] CameraIndexToView;        

        List2<DetailLayer> layers;
        public DetailLayer current;
        public DetailLayer prevLayer;

        public static readonly IntervalF FullZoomRange = new IntervalF(1, 2500);
        public static IntervalF TutorialZoomRange;

        public const float OverviewZoomStart = 80f;
        static readonly float UnitMaxZoom = FullZoomRange.Max - 0.4f;

        static readonly float TerrainMaxZoom = 200;
        public static readonly float StartZoom= TerrainMaxZoom * 0.5f;

        const float CloseUpCamAngle = 0.85f;
        public const float NormalCamAngle = 0.78f;
        const float OverviewCamAngle = 0.65f;

        static readonly IntervalF DetailZoom = new IntervalF(2f, 4f) + FullZoomRange.Min;
        static readonly IntervalF OverviewZoom = new IntervalF(OverviewZoomStart - 2f, OverviewZoomStart);
        static readonly IntervalF FactionZoom = new IntervalF(-1.5f, 0f) + UnitMaxZoom;

        
        Engine.PlayerData player;
       
        //public bool DrawCloseUp, DrawNormalAndClose, DrawNormal, DrawOverview, DrawFullOverview;
        public float OverviewAndFactionsTransparentsy;
        public float OverviewScale = 1f;

        public MapDetailLayerManager(Engine.PlayerData player)
        {
            this.player = player;

            layers = new List2<DetailLayer>((int)MapDetailLayerType.NUM);
            {
                float zoomBuffer = FullZoomRange.Difference * 0.0025f;

                float minZoom = FullZoomRange.Min;
                float maxZoom = 40;//FullZoomRange.GetFromPercent(0.08f);

                layers.Add(new DetailLayer(MapDetailLayerType.UnitDetail1, minZoom, maxZoom, zoomBuffer));

                minZoom = maxZoom;
                maxZoom = TerrainMaxZoom;//FullZoomRange.GetFromPercent(0.5f);

                layers.Add(new DetailLayer(MapDetailLayerType.TerrainOverview2, minZoom, maxZoom, zoomBuffer));

                TutorialZoomRange = new IntervalF(minZoom + zoomBuffer, maxZoom - zoomBuffer);

                minZoom = maxZoom;
                maxZoom = 450;//FullZoomRange.GetFromPercent(0.75f);

                layers.Add(new DetailLayer(MapDetailLayerType.FactionColors3, minZoom, maxZoom, zoomBuffer), true);

                minZoom = maxZoom;
                maxZoom = FullZoomRange.Max;

                layers.Add(new DetailLayer(MapDetailLayerType.FullOverview4, minZoom, maxZoom, zoomBuffer));
            }

            setNewLayer();

            updateCamIndex();
            new AsynchUpdateable(asynchUpdate, "Units cam culling, " + player.localPlayerIndex.ToString(), player.localPlayerIndex);
            
        }


        public bool LockDetailLevel = false;
        BoundingSphere boundingSphere = new BoundingSphere();
        SpottedArrayCounter<GameObject.City> cityCounter = new SpottedArrayCounter<GameObject.City>(null);
        
        bool asynchUpdate(int id, float time)
        { 
           
            return false;
        }


        void updateCamIndex()
        {
            if (CameraIndexToView == null)
            {
                CameraIndexToView = new MapDetailLayerManager[DssLib.MaxLocalPlayers];
            }

            for (int i = 0; i < DssLib.MaxLocalPlayers; ++i)
            {
                if (Ref.draw.ActivePlayerScreens[i] == player)
                {
                    CameraIndexToView[i] = this;
                    return;
                }
            }

            throw new Exception("Could not find player view");
        }

        void setNewLayer()
        {
           
            prevLayer = current;
            current = layers.sel;

            if (prevLayer != null)
            {
                prevLayer.opacity = 1f;
                if (current.DrawDetailLayer)
                {
                    prevLayer.opacity = 1.75f;
                }
              
            }

            player.view.Camera.FarPlane = current.type < MapDetailLayerType.FactionColors3 ? 800 : 5000;
        }
        public void Update()
        {
            if (prevLayer != null)
            {
                prevLayer.opacity -= 3f * Ref.DeltaTimeSec;
                if (prevLayer.opacity <= 0)
                {
                    prevLayer.opacity = 0;
                    prevLayer = null;
                }
            }

            if (player.view.Camera.targetZoom < current.zoom.Min)
            {
                layers.selectPrev();
                setNewLayer();
            }
            else if (player.view.Camera.targetZoom >= current.zoom.Max)
            {
                layers.selectNext();
                setNewLayer();
            }

            if (player.view.Camera.TiltY != current.goalCamAngle)
            {
                float tiltSpeed = Ref.DeltaTimeMs * 0.001f;
                float diff = current.goalCamAngle - player.view.Camera.TiltY;
                if (tiltSpeed > Math.Abs(diff))
                {
                    player.view.Camera.TiltY = current.goalCamAngle;
                }
                else
                {
                    player.view.Camera.TiltY += tiltSpeed * lib.ToLeftRight(diff);
                }
            }
        }
    }

    class DetailLayer
    {
        const float CloseUpCamAngle = 0.85f;
        public const float NormalCamAngle = 0.78f;
        const float OverviewCamAngle = 0.65f;

        public bool DrawDetailLayer, DrawNormalAndClose, DrawNormal, DrawOverview, DrawFullOverview;
        //public float CloseUpTransparentsy, NormalAndCloseTransparentsy, NormalTransparentsy, OverviewTransparentsy, OverviewAndFactionsTransparentsy, FactionsTransparentsy;

        public float goalCamAngle;

        public IntervalF zoom;

        public float opacity = 1f;

        public MapDetailLayerType type;

        public DetailLayer(MapDetailLayerType type, float minZoom, float maxZoom, float zoomBuffer)
        {
            this.type = type;
            zoom = new IntervalF(minZoom - zoomBuffer, maxZoom + zoomBuffer);

            switch (type)
            {
                case MapDetailLayerType.UnitDetail1:
                    //CloseUp
                    goalCamAngle = CloseUpCamAngle;
                    DrawDetailLayer = true;
                   
                    break;

                default://case DrawUnitsLevel.Normal:
                    goalCamAngle = NormalCamAngle;
                    DrawNormal = true;
                    break;

                case MapDetailLayerType.FactionColors3:
                    goalCamAngle = OverviewCamAngle;
                    DrawOverview = true;
                    DrawFullOverview = false;
                    DrawDetailLayer = false;                  
                    break;

                case MapDetailLayerType.FullOverview4:
                    goalCamAngle = OverviewCamAngle;
                    DrawFullOverview = true;
                    DrawOverview = true;                   
                    break;
            }
        }
    }

    enum MapDetailLayerType
    {
        UnitDetail1,
        TerrainOverview2,
        FactionColors3,
        FullOverview4,
        NUM
    }
}

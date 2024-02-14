using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Input;
//xna
using VikingEngine.HUD;
using VikingEngine.LootFest.GO;
using VikingEngine.Input;

namespace VikingEngine.LootFest.Players 
{
    partial class Player : AbsPlayer, IMessageHandlerParent
    {

#region CONSTANTS

        public const float HeroCamChaseSpeed = 0.1318f;
        public const float MinHeroCamChaseSpeed = 0.05f;
        public const float MaxHeroCamChaseSpeed = 0.5f;
        public const int CharacterWidth = 2;
        public const int CharacterHeight = 4;
#endregion

#region VARIABLES
        public bool appearanceChanged = false;
        public const int ChatMessageMaxChars = 250;
        List<GamerName> gamerNames = new List<GamerName>();
        HUD.ButtonLayout buttonLayOut;

        Editor.VoxelDesigner voxelDesigner = null;
        Map.WorldPosition voxelDesignerStartPos;
        float controlLock = 0;

        protected bool localHost = false;
        public override bool LocalHost
        {
            get { return localHost; }
        }

        public Map.BackgroundScenery scenery;
#endregion
        
#region MENU
        protected MenuSystem2 menuSystem = null;
        HUD.MessageHandler messageHandler;
        public bool UsingInputDialogue { get { return false; } }
#endregion
                
        #region INTERACTIONS

        public int InteractionMenuTab = 0;
        public int InteractionStoreValue1 = 0;
        public int InteractionStoreValue2 = 0;

        public IntVector2[] lastLowResEdgePos = new IntVector2[4];
        
        PlayerGearManager gearManager;
        override public PlayerGearSetup Gear
        {
            get { return gearManager.setupStack[gearManager.setupStack.Count - 1]; }
        }
        
       
        #endregion
                

        public void restartFromKnockout()
        {
            hero.restartFromKnockout();
            
            CloseMenu();
            this.LockControls(true);
        }

        public void HideControls()
        {
            if (buttonLayOut != null)
            {
                buttonLayOut.DeleteMe();
                buttonLayOut = null;
            }
        }
                
        public bool LockMessageHandler
        {
            get
            {
                return inMenu;
            }
        }
        
        VectorRect screenArea;
        VectorRect safeScreenArea;
        public VectorRect SafeScreenArea
        {
            get { return safeScreenArea; }
        }
        public VectorRect ScreenArea
        {
            get { return screenArea; }
        }

        override public int PlayerIndex
        { get { 
            
            return pData.localPlayerIndex; } }
        
        public void UpdateSplitScreen(Engine.PlayerData controllerLink, int numPlayers, int screenIndex)
        {
            Graphics.AbsCamera creationCam = null;
            if (inEditor)
            {
                creationCam = localPData.view.Camera;
                localPData.view.Camera = storedCamera;
            }

            controllerLink.view.camType = (Storage.FPSview) ?
                Graphics.CameraType.FirstPerson : Graphics.CameraType.TopView;

            controllerLink.view.SetDrawArea(numPlayers, screenIndex, true, this);

            if (controllerLink.view.camType == Graphics.CameraType.FirstPerson)
            {
                ((Graphics.FirstPersonCamera)localPData.view.Camera).Person = hero;

            }
            refreshCamSettings();

            this.pData = controllerLink;

            screenArea = new VectorRect(controllerLink.view.DrawArea);

            const float SafePercent = 0.06f;
            Vector2 removeSafeBorder = SafePercent * Engine.Screen.ResolutionVec;
            safeScreenArea = screenArea;

            if (screenArea.Right == Engine.Screen.Width)
            {
                safeScreenArea.Width -= removeSafeBorder.X;
            }
            if (screenArea.Bottom == Engine.Screen.Height)
            {
                safeScreenArea.Height -= removeSafeBorder.Y;
            }

            if (screenArea.X == 0)
            {
                safeScreenArea.AddToLeftSide(-removeSafeBorder.X);
            }
            if (screenArea.Y == 0)
            {
                safeScreenArea.AddToTopSide(-removeSafeBorder.Y);
            }

            //float screenRatio = lib.SmallestValue(screenArea.Width, screenArea.Height) /
            //    lib.LargestValue(screenArea.Width, screenArea.Height);


            localPData.view.Camera.positionChaseLengthPercentage = HeroCamChaseSpeed;
     
            if (hero != null)
            {
                localPData.view.Camera.LookTarget = hero.Position;
                hero.updateRedBorder();
                hero.setCamera();
            }

            HideControls();

            if (inEditor)
            {
                
                storedCamera = localPData.view.Camera;
                if (creationCam.aspectRatio != localPData.view.Camera.aspectRatio)
                {
                    EndCreationMode();
                }
                else
                {
                    localPData.view.Camera = creationCam;
                }
            }

            if (statusDisplay != null)
            {
                statusDisplay.DeleteMe();
                statusDisplay = new PlayerStatusDisplay(this);
                statusDisplay.RefreshAll(this);
            }

            if (saveIcon != null) { saveIcon.DeleteMe(); }
            saveIcon = new Display.SaveIcon(this);

            if (pauseIcon != null)
            {
                pauseIcon.DeleteMe();
            }

            if (pauseBorder != null)
            {
                pauseBorder.DeleteMe();
                pauseBorder = null;
         
                Pause(true);
            }

            
        }


        static readonly IntervalF CameraAngleRangeX = new IntervalF(1.5f, 1.7f); 
        static readonly IntervalF[] CameraAngleY = new IntervalF[] 
        {
            new IntervalF(0.4f, 0.5f),
            new IntervalF(0.66f, 0.78f),
            new IntervalF(0.78f, 0.88f)
        };

        Vector2 cameraLookSpeed;
        Vector2 camGoalAngle;
        float camGoalAngleLength;
        const int AngleSamplesCount = 24;
        CirkleCounterUp currentAngleSample = new CirkleCounterUp(AngleSamplesCount - 1);
        struct CameraSampleValue
        {
            public Vector2 direction;
            public float distance;
            public CameraSampleValue(Vector2 direction, float distance)
            {
                this.direction = direction; this.distance = distance;
            }
        }
        CameraSampleValue[] characterGoalAngleSamples = new CameraSampleValue[AngleSamplesCount];
        const float MenuZoom1p = 20;
        
        public Player(Engine.PlayerData pData, int numPlayers, int screenIndex, bool localHost, PlayState gamestate)
            :base(pData)
        {
            this.localPData = pData;
            pData.Tag = this;
            if (pData.inputMap == null)
            {
                inputMap = new InputMap(pData.localPlayerIndex);
                pData.inputMap = inputMap;
            }
            else
            {
                inputMap = pData.inputMap as InputMap;
            }
            this.localHost = localHost;
            Storage = new PlayerStorage();
            
            scenery = new Map.BackgroundScenery();
            UpdateSplitScreen(pData, numPlayers, screenIndex);
           
            camGoalAngle = new Vector2(CameraAngleRangeX.Center, CameraAngleY[1].Center);

            messageHandler = new HUD.MessageHandler(2, SpriteName.WhiteArea, 4000, Vector2.Zero, this);
            messageHandler.MessageLayer = LfLib.Layer_Messages;
            updateMessageHandlerPos();
            
            pData.IsActive = true;

            Director.LightsAndShadows.Instance.LocalGamerJoinedEvent();

            //checkPrivateHomeLocation();

            
        }

        public void selectSaveFile()
        {
            exitCurrentMode(true);

            bool hasPreviousSaveFile = false;
            for (int i = 0; i < LfRef.storage.storages.Length; ++i)
            {
                if (LfRef.storage.storages[i] != null)
                {
                    hasPreviousSaveFile = true;
                    break;
                }
            }

            if (hasPreviousSaveFile && (!PlatformSettings.DevBuild || DebugSett.ViewSaveFilesMenu))
            {
                LfRef.gamestate.saveFilesMenu = new Display.SaveFilesMenu(this);
            }
            else
            {
                for (int i = 0; i < LfRef.storage.playerIndexPreviousSaveFile.Length; ++i)
                {
                    if (LfRef.storage.playerIndexPreviousSaveFile[i] < 0)
                    {
                        LfRef.storage.AssignToPlayer(i, this);
                        break;
                    }
                }
                Input.Mouse.Visible = false;
            }
        }

        public void refreshCamSettings()
        {
            const float StandardXspeed = 1f;
            const float StandardYspeed = 1f;
            const float SpeedStep = 0.01f;

            cameraLookSpeed = new Vector2(
                StandardXspeed + (Storage.CamSpeedX - Players.PlayerStorage.StandardCameraSpeedSetting) * SpeedStep,
                StandardYspeed + (Storage.CamSpeedY - Players.PlayerStorage.StandardCameraSpeedSetting) * SpeedStep);
            if (Storage.CamInvertY)
                cameraLookSpeed.Y = -cameraLookSpeed.Y;

            //pData.view.Camera.targetChaseLengthPercentage = Storage.CamChaseSpeed;

            if (localPData.view.camType == Graphics.CameraType.TopView)
            {
                localPData.view.Camera.FieldOfView = Storage.CamTopViewFOV;
                TopViewCamera topCam = localPData.view.Camera as TopViewCamera;
                topCam.UseTerrainCollisions = Storage.CamUseTerrainCollisions;
                topCam.InstantZoomIn = Storage.CamInstantZoomIn;
                topCam.InstantZoomOut = Storage.CamInstantZoomOut;
            }
            else
            {
                localPData.view.Camera.FieldOfView = Storage.CamFirstPersonFOV;
            }
        }

        const int EventTriggerStartupHelp = 0;
        

        public Vector3 CamTargetPosition()
        {
            return hero.Position;
        }
        public Rotation1D CamTargetMoveDir()
        {
            return hero.Rotation;
        }

        public void Print(string message)
        { 
            if (messageHandler != null)
                messageHandler.Print(message); 
        }
        public void Print(string message, SpriteName icon)
        { 
            if (messageHandler != null)
                messageHandler.Print(message, icon); 
        }


        public void PrintChat(HUD.ChatMessageData message, bool local, LoadedSound sound)
        {
            if (messageHandler != null)
            {
                message.Text = Engine.LoadContent.CheckCharsSafety(message.Text, VikingEngine.HUD.MessageHandler.StandardFont);
                message.Sender = Engine.LoadContent.CheckCharsSafety(message.Sender, VikingEngine.HUD.MessageHandler.StandardFont);

                messageHandler.PrintChat(message, safeScreenArea.Width, local);
                Music.SoundManager.PlayFlatSound(sound);
            }
        }
        
        bool warpLock = false;
        public bool WarpLoading
        {
            get { return warpLock && controlLock > 0; }
        }

        public void LockControls(bool warpLoading)
        {
            warpLock = warpLoading;
            if (warpLoading)
            {
                const float WarpLoadTime = 1500;
                const float WarpImmortalTime = WarpLoadTime + 1000;
                hero.ImmortalityTime = WarpImmortalTime;
                controlLock = WarpLoadTime;
                Graphics.TextS loading = new TextS(LoadedFont.Regular,
                     safeScreenArea.Center, VectorExt.V2(1f), Align.CenterAll, "Loading terrain",
                    Color.CornflowerBlue, ImageLayers.AbsoluteTopLayer);
                new Timer.TextFeed(500, loading);
                new Timer.Terminator(controlLock, loading);

            }
            else
                controlLock = 800;
        }
        public GO.PlayerCharacter.AbsHero CreateHero(bool joinedSessionStartUp)
        {
            gearManager = new PlayerGearManager(this);
            //if (PlatformSettings.DevBuild)
            //{
            //    hero = new GO.PlayerCharacter.HorseRidingHero(this);
            //}
            //else
            {
                hero = new GO.PlayerCharacter.Hero(this);
            }
            
            hero.UpdateAppearance(true);

            LfRef.gamestate.RefreshHeroesList();
            
            this.LockControls(true);

            if (!joinedSessionStartUp)
            {
                LfRef.gamestate.selectControllerMenu = new Display.SelectControllerMenu(this);
            }

            return hero;
        }

        public void setNewHero(GO.PlayerCharacter.AbsHero newHero, bool transformSmokeEffect)
        {
            GO.PlayerCharacter.AbsHero prevHero = hero;
            newHero.InheritPreviousPlayerChar(prevHero);
            if (localPData.view.camType == Graphics.CameraType.FirstPerson)
            {
                ((Graphics.FirstPersonCamera)localPData.view.Camera).Person = newHero;

            }
            prevHero.DeleteMe();
            newHero.ObjOwnerAndId = prevHero.ObjOwnerAndId;
           
            hero = newHero;
            Gear.onNewHero();
            hero.UpdateAppearance(true);
            
            LfRef.gamestate.RefreshHeroesList();
            if (transformSmokeEffect)
            {
                Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.RunningSmoke, hero.HeadPosition, 1.5f, 256, 3f);
            }

            //Hero target swap out
            var objects = LfRef.gamestate.GameObjCollection.localMembersUpdateCounter;
            objects.Reset();
            while (objects.Next())
            { objects.sel.onObjectSwapOut(prevHero, newHero); }

            hero.setCamera();
        }

        public void GuideUpEvent()
        {
            if (inGamePlay && Ref.netSession.InMultiplayerSession)
            {
                mainMenu();
            }
        }

        void changeCamera()
        {
            Storage.FPSview = !Storage.FPSview;
            LfRef.gamestate.UpdateSplitScreen();
        }

        
        /// <summary>
        /// Calculate the relative movement compared to the camera angle
        /// </summary>
        /// <returns>Direction to move the hero</returns>
        public Vector2 HeroMoveDir()
        {
            if (inGamePlay && !hero.LockControls)
            {
                Vector2 moveDir = inputMap.movement.direction;
                
                if (inputMap.camera.plusKeyIsDown)
                { moveDir = Vector2.Zero; }

                if (localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
                {
                    return ((Graphics.FirstPersonCamera)localPData.view.Camera).MoveCamera(moveDir);
                }
                else
                {
                    return VectorExt.RotateVector(moveDir, localPData.view.Camera.TiltX - MathHelper.PiOver2);
                }
            }
            else return Vector2.Zero;
        }

        const float ChangeZoomSpeed = 0.0017f;
        static readonly IntervalF ZoomBounds = new IntervalF(10, 130) * LfLib.ModelsScaleUp;
        void updateCamInput()
        {
            Vector2 camInputDir = inputMap.camera.direction;
            Vector2 camInputDirTime = inputMap.camera.directionAndTime;

            if (localPData.view.Camera.CamType == Graphics.CameraType.FirstPerson)
            {//FPS
                Vector2 look = camInputDirTime * 0.004f * cameraLookSpeed;
                look.X *= Gear.suit.MovementPerc;
                localPData.view.Camera.RotateCamera(look);
            }
            else
            {
                localPData.view.Camera.TiltX += camInputDirTime.X * 0.0033f * cameraLookSpeed.X;
                if (Math.Abs(camInputDir.Y) > 0.5f)
                {
                    localPData.view.Camera.TiltY = Bound.Set(
                        localPData.view.Camera.TiltY + camInputDirTime.Y * -0.0015f * cameraLookSpeed.Y, 
                        0.5f, 1.65f);
                }
                localPData.view.Camera.targetZoom = ZoomBounds.SetBounds(localPData.view.Camera.targetZoom + inputMap.cameraZoom.directionAndTime.Y * 0.05f);
               
            }
        }

        void appearancesAdded()
        {
            Print("Appearance options added");
        }
        void blocksAdded()
        {
            Print("More blocks added");
        }
        void RCsAdded()
        {
            Print("New RC toy added");
        }

        public bool toggleDebugMode(int index, bool set, bool value)
        {
            if (set)
                hero.DebugMode = value;
            return hero.DebugMode;
        }
        

        public void saveAndExitLink()
        {
            if (LfRef.gamestate.LocalHostingPlayer == this)
            {
                new GameState.SaveAndExit();
            }
            else
            {
                DeleteMe();
            }
        }
        public void exitWithoutSaveLink()
        {
            Ref.netSession.Disconnect(null);
            Ref.update.exitApplication = true;
        }


        public void HandleDamage()
        {
            exitCurrentMode(false);
            
            if (!hero.isMounted && hero.Dead)
            {
                menuSystem = new MenuSystem2(this);
                menuSystem.GameOverMenu();

                LockControls(false);
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.PlayerDied, 
                    Network.PacketReliability.ReliableLasy, PlayerIndex);

                hero.DeathAnimation(true);
            }
        }

        public override void DeleteMe()
        {
            Ref.update.AddToOrRemoveFromUpdate(this, false);
            clearGamerNames();
            HideControls();
            messageHandler.DeleteMe();
            LfRef.gamestate.RemoveScreenPlayer(this);
            base.DeleteMe();

            foreach (GamerName gn in gamerNames)
            {
                gn.DeleteMe();
            }

            localPData.IsActive = false;

            if (pauseBorder != null)
                pauseBorder.DeleteMe();
            if (pauseIcon != null)
                pauseIcon.DeleteMe();

            Director.LightsAndShadows.Instance.LocalGamerJoinedEvent();
            statusDisplay.DeleteMe();
            FreeStorageLock();
        }

        public void FreeStorageLock()
        {
            Storage.player = null;
        }

        public void SaveAndExit()
        {
            messageHandler = null;
            Save();
        }
        
        public bool inMenu { get { return menuSystem != null; } }

        public bool inEditor { get { return voxelDesigner != null; } }

        public bool inGamePlay
        {
            get
            {
                return menuSystem == null && voxelDesigner == null && cinematicMode.MilliSeconds <= 0;
            }
        }

        public bool inCinamticMode
        {
            get { return cinematicMode.MilliSeconds > 0; }
        }

        public override bool IsPausing
        {
            get { return inMenu && voxelDesigner == null; }
        }

        public void Save()
        {
            if (PlatformSettings.RunningWindows)
            {
                Ref.gamesett.Save();
            }
            LfRef.storage.Save();
            saveIcon.OnSave();

            if (LfRef.WorldHost)
            {
                var lvl = hero.Level;
                if (lvl != null)
                {
                    foreach (var m in lvl.designAreas.areas)
                    {
                        m.SaveHeader();
                    }
                }
            }
        }

        

        static readonly Range ZoomModeRange = new Range(
            (PlatformSettings.DebugLevel == BuildDebugLevel.Dev ? (int)ZoomMode.Debug : (int)ZoomMode.TopView), 
            (int)ZoomMode.NUM - 1);
        static readonly IntervalF CurrentZoomRange = new IntervalF(0, 1);

        const float ZoomMultiplier = 8;

        const float DebugMaxZoom = 80f * ZoomMultiplier;
        const float DebugMinZoom = 8.4f * ZoomMultiplier;
        const float TopViewMaxZoom = DebugMinZoom;
        const float TopViewMinZoom = 3 * ZoomMultiplier;
        const float IsoViewMaxZoom = TopViewMinZoom;
        const float IsoViewMinZoom = 2 * ZoomMultiplier;
        const float ShoulderViewMaxZoom = IsoViewMinZoom;
        const float ShoulderViewMinZoom = 1 * ZoomMultiplier;

        static readonly IntervalF ZoomRangeDebug = new IntervalF(DebugMinZoom, DebugMaxZoom);
        static readonly IntervalF ZoomRangeTop = new IntervalF(TopViewMinZoom, TopViewMaxZoom);
        static readonly IntervalF ZoomRangeIso = new IntervalF(IsoViewMinZoom, IsoViewMaxZoom);
        static readonly IntervalF ZoomRangeShoulder = new IntervalF(ShoulderViewMinZoom, ShoulderViewMaxZoom);

        const float DebugMaxAngle = 0.3f;
        const float DebugMinAngle = 0.8f;
        const float TopViewMaxAngle = DebugMinAngle;
        const float TopViewMinAngle = 1.1f;
        const float IsoViewMaxAngle = TopViewMinAngle;
        const float IsoViewMinAngle = 1.3f;
        const float ShoulderViewMaxAngle = IsoViewMinAngle;
        const float ShoulderViewMinAngle = 1.4f;

        static readonly IntervalF AngleRangeDebug = new IntervalF(DebugMinAngle, DebugMaxAngle);
        static readonly IntervalF AngleRangeTop = new IntervalF(TopViewMinAngle, TopViewMaxAngle);
        static readonly IntervalF AngleRangeIso = new IntervalF(IsoViewMinAngle, IsoViewMaxAngle);
        static readonly IntervalF AngleRangeShoulder = new IntervalF(ShoulderViewMinAngle, ShoulderViewMaxAngle);
   
        public void exitCurrentMode(bool hardExit)
        {
            if (hardExit)
            {
                //Hard exit will close all menues, no matter what
                if (hero.Dead)
                {
                    hero.restartFromKnockout();
                }
            }

            deleteInputDialogue();
            
            CloseMenu();

            if (voxelDesigner != null)
                EndCreationMode();

        }
        

        VectorRect menuArea()
        {
            float w = Engine.Screen.Width * 0.3f;
            if (w > safeScreenArea.Width)
            {
                w = safeScreenArea.Width;
            }
            return new VectorRect(
                safeScreenArea.Position + new Vector2(0, 10),
                new Vector2(w, safeScreenArea.Height));
        }

        Graphics.AbsCamera storedCamera;
        
        const string StillLoadingMessage = "Hold on, still loading";

        void updateLocalVisualMode(float time)
        {

            if (Engine.XGuide.InOverlay)//PlatformSettings.SteamAPI && Ref.steam.inOverlay)
            {
                setVisualMode(VisualMode.SteamGuide, true);
            }
            else if (visualMode == VisualMode.SteamGuide)
            {
                if (inMenu)
                {
                    setVisualMode(VisualMode.Menu, true);
                }
                else
                {
                    setVisualMode(VisualMode.Non, true);
                }
            }

        }

        public void Update(float time, PlayState gamestate)
        {
            if (voxelDesigner == null)
                updateInput();

            if (!inMenu)
            {
                updateCameraTargetChasing();
            }

            //localPData.inputMap.update();
            if (inEditor)
            {
                voxelDesigner.Time_Update(time);
            }

            localPData.view.Camera.Time_Update(time);
            foreach (GamerName gn in gamerNames)
            {
                gn.Update(localPData.view.Camera, localPData.view.Viewport, safeScreenArea);
            }

            if (Ref.update.LasyUpdatePart == LasyUpdatePart.Part6_Player)
            {
                //calculate the wanted camera angle
                //the camera should turn to show the direction of walking
                //höja vyn lite när man går emot kameran

                updateLocalVisualMode(time);
                if (!Engine.Update.IsRunningSlow)
                {
                    UpdateGamerNames();
                }

                if (localPData.LostController)
                {
                    if (!inMenu)
                    {
                        exitCurrentMode(false);
                        mainMenu();
                    }

                }
                if (controlLock > 0)
                {
                    controlLock -= Ref.update.LazyUpdateTime;
                    if (controlLock <= 0)
                    {
                        hero.CheckIfUnderGround();
                    }
                }
            }

            scenery.update(this);
            if (saveIcon != null)
            {
                saveIcon.Update();
            }

            if (interactDisplay != null)
            {
                if (interactDisplay.Update())
                { deleteInteractDisplay(); }
            }
        }

        

        public void updateCameraTargetChasing()
        {
            if (Ref.TimePassed16ms)
            {
                const float TargetInfront = 1f / AngleSamplesCount;
                if (hero != null)
                {
                    Rotation1D camRot = new Rotation1D(localPData.view.Camera.TiltX);
                    camRot.Add(-MathHelper.PiOver2);

                    //Debug.Log("cam update, cam tilt X:" + camRot.ToString() + ", hero rot:" + hero.Rotation.ToString() + ", diff=" + hero.Rotation.AngleDifference(camRot.Radians).ToString());
                    float camAndPlayerAlign = Math.Abs(hero.Rotation.AngleDifference(camRot.Radians)) / MathHelper.Pi;
                    characterGoalAngleSamples[currentAngleSample.Value] = new CameraSampleValue(hero.Rotation.Direction(TargetInfront),
                        (8 * camAndPlayerAlign + localPData.view.Camera.targetZoom * 0.06f) / AngleSamplesCount);

                    currentAngleSample.Next();
                }


                camGoalAngle = Vector2.Zero;
                camGoalAngleLength = 0f;
                for (int i = 0; i < AngleSamplesCount; i++)
                {
                    camGoalAngle += characterGoalAngleSamples[i].direction;
                    camGoalAngleLength += characterGoalAngleSamples[i].distance;
                }

                Vector3 target = hero.Position;
                target.Y += 2f;

                localPData.view.Camera.GoalLookTarget = target;
            }
        }

        public void UpdateGamerNames()
        {
            if (Storage.ViewPlayerNames)
            {

                foreach (GamerName gn in gamerNames)
                {
                    gn.Keep = false;
                }

                for (int i = 0; i < LfRef.AllHeroes.Count; ++i)
                {
                    GO.PlayerCharacter.AbsHero h = LfRef.AllHeroes[i];
                    if (!h.NetworkLocalMember)
                    {
                        if (h.PositionDiff(hero).Length() <= 60)
                        {
                            bool found = false;
                            foreach (GamerName gn in gamerNames)
                            {
                                if (gn.Hero == h)
                                {
                                    gn.Keep = true;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                gamerNames.Add(new GamerName(h, h.clientPlayer));
                        }
                    }
                }

                for (int i = gamerNames.Count - 1; i >= 0; i--)
                {
                    if (!gamerNames[i].Keep)
                    {
                        gamerNames[i].DeleteMe();
                        gamerNames.RemoveAt(i);
                    }
                }
            }
            else
            {
                clearGamerNames();
            }
        }

        
        override public bool Local { get { return true; } }

        public override SuitAppearance SuitAppearance
        {
            get
            {
                return getSuitAppearance(Gear.suit.Type);
            }
            set
            {
                setSuitAppearance(Gear.suit.Type, value);
            }
        }
        
    }

    enum TextInputType
    {
        NON,
        NameLocation,
    }
    
}

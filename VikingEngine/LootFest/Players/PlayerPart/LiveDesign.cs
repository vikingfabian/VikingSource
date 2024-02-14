using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna
using VikingEngine.Input;

namespace VikingEngine.LootFest.Players
{
    partial class Player
    {
        //GO.Toys.ToyType lastToyType = GO.Toys.ToyType.LightCar;
        //int changeRCcolorIx = 0;

        //void aimGolfBall()
        //{
        //    mode = PlayerMode.GolfAim;

        //}
        
        
//        void beginRCmode(GO.Toys.ToyType type)
//        {
//            CloseMenu();
//            switch (type)
//            {
//                case GO.Toys.ToyType.LightCar:
//                    rcToy = new GO.Toys.RCCar(hero);
//                    break;
//                case GO.Toys.ToyType.LightHelicoper:
//                    rcToy = new GO.Toys.RCHelicopter(hero);
//                    break;
//                case GO.Toys.ToyType.LightPlane:
//                    rcToy = new GO.Toys.RCPlane(hero, safeScreenArea);
//                    break;
//                case GO.Toys.ToyType.LightTank:
//                    rcToy = new GO.Toys.RCTank(hero);
//                    break;
//                case GO.Toys.ToyType.Ship:
//                    rcToy = new GO.Toys.RCShip(hero);
//                    break;
//            }
//            lastToyType = type;
//            rcToy.UpdateWorldPos();
//            mode = PlayerMode.RCtoy;

//            //change camera
//            checkRcCam();
//        }

//        void exitRCmode()
//        {
//#if CMODE
//            if (raceStarter != null && !raceStarter.IsDeleted)
//            { raceStarter.DeleteMe(); }

//            rcToy.DeleteMe();
//            if (rcToy.PositionDiff(hero).Length() > Map.WorldPosition.ChunkWith * 2)
//            {
//                //hero.resetMapLoading();
//                LockControls(true);
//            }
//            rcToy = null;

            
//            CloseMenu();


//            //bool fpCam = settings.GetRCCamType(rcToy.FlyingToy) == GO.Toys.RCCameraType.FirstPerson;
//            if (settings.FPSview != (ControllerLink.view.Camera.Type == Graphics.CameraType.FirstPerson))
//            {
//                LfRef.gamestate.updateSplitScreen();
//            }
//            if (settings.FPSview)
//                ((Graphics.FirstPersonCamera)ControllerLink.view.Camera).TargetOriented = false;
//#endif
//        }


//        void buyPlayerHealth()
//        {
//            if (buyFromDesignStore(HealthCost))
//            {
//                hero.addHealth(LootfestLib.AbsHeroHealth * 0.5f);
//                if (mode == PlayerMode.InMenu)
//                    openPage(MenuPageName.DesignShop);
//            }
//        }


//        void checkRcCam()
//        {
//#if CMODE
//            bool fpCam = settings.GetRCCamType(rcToy.FlyingToy) == GO.Toys.RCCameraType.FirstPerson;
//            if (fpCam != (ControllerLink.view.Camera.Type == Graphics.CameraType.FirstPerson))
//            {
//                LfRef.gamestate.updateSplitScreen();
//            }
//            if (fpCam)
//                ((Graphics.FirstPersonCamera)ControllerLink.view.Camera).TargetOriented = true;
//#endif
//        }
//#if CMODE
//        Players.BuilderBot bot;
//        void spawnCritter(bool fromMenu)
//        {
//            updateCritterList();
//            if (creatures.Count < MaxCritters)
//            {
//                if (!sharedCritterImage)
//                {
//                    networkShareCritterImage();
//                }
//                creatures.Add(new Creation.Creature(hero.WorldPosition, critterMaster, settings.CreatureScale, settings.CreatureAnimationSpeed, settings.CreatureAI, hero));
//                if (firstCritter)
//                {
//                    firstCritter = false;
//                    loadCritterImage();
//                }
//            }
//            else if (fromMenu)
//            {
//                mFile = new HUD.File();
//                mFile.AddDescription("You can't have more than " + MaxCritters.ToString() + " critters");
//                mFile.AddIconTextLink(SpriteName.LFIconGoBack, "OK", (int)Link.CritterMenu);
//                OpenMenuFile();
//            }

//        }
//#endif

//        List<VoxelObjName> availableSwords()
//        {
//            List<VoxelObjName> result = new List<VoxelObjName> { VoxelObjName.Sword1 };
//#if CMODE
//            if (settings.UnlockedTotalMinerPack)
//            {
//                result.Add(VoxelObjName.pickaxe);
//            }

//            if ( settings.SwordLevel > 0)
//            {
//                result.AddRange(new List<VoxelObjName> {
//                    VoxelObjName.Sword2, VoxelObjName.axe, VoxelObjName.broom, VoxelObjName.magicstaff,});
//                if ( settings.SwordLevel > 1)
//                {
//                    result.AddRange(new List<VoxelObjName> {
//                        VoxelObjName.Sword3, VoxelObjName.doubleaxe, VoxelObjName.pimpstick,});
//                }
//            }
//#endif
//            return result;
//        }
//        bool buyFromDesignStore(int cost)
//        {
//#if CMODE
//            if (Engine.XGuide.IsTrial)
//            {
//                mFile = new HUD.File();
//                mFile.AddTitle("Trial mode");
//                mFile.AddDescription("Can't buy from the ingame shop in trial mode");
//                mFile.AddTextLink("OK", (int)Link.LFDesignShop);
//                OpenMenuFile(mFile);// menu.File = mFile;
//            }
//            else if (settings.Coins >= cost)
//            {
//                Music.SoundManager.PlayFlatSound(LoadedSound.buy);
//                settings.Coins -= cost;
//                SettingsChanged();
//                return true;
//            }
//            else if (mode == PlayerMode.InMenu)
//            {
//                mFile = new HUD.File();
//                mFile.AddTitle("Not enough gold");
//                mFile.AddDescription("You have " + settings.Coins.ToString() + "g of " + cost.ToString() + "g");
//                mFile.AddTextLink("OK", (int)Link.LFDesignShop);
//                OpenMenuFile(mFile);// menu.File = mFile;
//            }
//#endif
//            return false;
//        }

        public bool IsInCreationMode = false;

        public void BeginCreationModeIfAllowed()
        {
            //if (LfRef.worldOverView.CanEdit(hero.WorldPosition.ChunkGrindex, this))
            //    BeginCreationMode();
            //else
            //{
            //    Print("No build permission");
            //}
        }

        public void BeginCreationMode()
        {
            IsInCreationMode = true;
            BeginCreationMode(hero.WorldPos.ChunkGrindex);
        }
        public void BeginCreationMode(IntVector2 chunkCenterPos)
        {
            if (hero.Alive)
            {
                if (localHost)
                {
                    LfRef.gamestate.clientPlayersCounter.Reset();
                    while (LfRef.gamestate.clientPlayersCounter.Next())
                    {
                        LfRef.gamestate.clientPlayersCounter.sel.removeStatusDisplay();
                    }
                }
                //pData.inputMap.SetGameStateLayout(ControllerActionSetType.EditorControls);
                //check if area is done loading
                IntVector2 pos = hero.ScreenPos;
                if (!LfRef.chunks.ChunksDataLoaded(pos))
                {
                    Print(StillLoadingMessage);
                    return;
                }


                Music.SoundManager.PlayFlatSound(LoadedSound.enter_build);

                CloseMenu();
                //mode = PlayerMode.Creation;
                //pick the area the player is standing at and send it to the designer
                voxelDesignerStartPos = Editor.VoxelDesigner.HeroPosToCreationStartPos(chunkCenterPos);

                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.ClientStartingEditing,
                    Network.PacketReliability.Reliable, PlayerIndex);
                Map.WorldPosition.WriteChunkGrindex_Static(hero.ScreenPos, w);


                storedCamera = localPData.view.Camera;
                voxelDesigner = new Editor.VoxelDesigner(
                    voxelDesignerStartPos, localPData.view.Camera, Storage.CamTopViewFOV, menuArea(), this);
                localPData.view.Camera.TiltX = storedCamera.TiltX;

                if (localHost)
                { //Clear out NPCs
                    foreach (var m in LfRef.net.lobbies)
                    {
                        m.remove();
                    }
                }
            }
        }

        public void EndCreationMode()
        {
            if (voxelDesigner != null)
            {
                if (localHost)
                {
                    LfRef.gamestate.clientPlayersCounter.Reset();
                    int ix = 0;
                    while (LfRef.gamestate.clientPlayersCounter.Next())
                    {
                        LfRef.gamestate.clientPlayersCounter.sel.createStatusDisplay(ix);
                        ix++;
                    }
                }
                //pData.inputMap.SetGameStateLayout(ControllerActionSetType.InGameControls);
                //pick the new design and add it to the map
                //Voxels.VoxelObjListData editChunk = voxelDesigner.voxels;
                //notify host
                if (Ref.netSession.IsClient)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.ClientEndingEditing,
                        Network.PacketReliability.ReliableLasy, PlayerIndex);
                }

                //mode = PlayerMode.Play;
                voxelDesigner.DeleteMe();
                voxelDesigner = null;
                storedCamera.TiltX = localPData.view.Camera.TiltX;
                localPData.view.Camera = storedCamera;

                hero.CheckIfUnderGround();

                LfRef.world.saveUpdate();
            }
            refreshCamSettings();
            IsInCreationMode = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        //GameObjects.Toys.ToyType lastToyType = GameObjects.Toys.ToyType.LightCar;
        //int changeRCcolorIx = 0;

        //void aimGolfBall()
        //{
        //    mode = PlayerMode.GolfAim;

        //}
        
        
//        void beginRCmode(GameObjects.Toys.ToyType type)
//        {
//            CloseMenu();
//            switch (type)
//            {
//                case GameObjects.Toys.ToyType.LightCar:
//                    rcToy = new GameObjects.Toys.RCCar(hero);
//                    break;
//                case GameObjects.Toys.ToyType.LightHelicoper:
//                    rcToy = new GameObjects.Toys.RCHelicopter(hero);
//                    break;
//                case GameObjects.Toys.ToyType.LightPlane:
//                    rcToy = new GameObjects.Toys.RCPlane(hero, safeScreenArea);
//                    break;
//                case GameObjects.Toys.ToyType.LightTank:
//                    rcToy = new GameObjects.Toys.RCTank(hero);
//                    break;
//                case GameObjects.Toys.ToyType.Ship:
//                    rcToy = new GameObjects.Toys.RCShip(hero);
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


//            //bool fpCam = settings.GetRCCamType(rcToy.FlyingToy) == GameObjects.Toys.RCCameraType.FirstPerson;
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
//                hero.addHealth(LootfestLib.HeroHealth * 0.5f);
//                if (mode == PlayerMode.InMenu)
//                    openPage(MenuPageName.DesignShop);
//            }
//        }


//        void checkRcCam()
//        {
//#if CMODE
//            bool fpCam = settings.GetRCCamType(rcToy.FlyingToy) == GameObjects.Toys.RCCameraType.FirstPerson;
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

//        List<VoxelModelName> availableSwords()
//        {
//            List<VoxelModelName> result = new List<VoxelModelName> { VoxelModelName.Sword1 };
//#if CMODE
//            if (settings.UnlockedTotalMinerPack)
//            {
//                result.Add(VoxelModelName.pickaxe);
//            }

//            if ( settings.SwordLevel > 0)
//            {
//                result.AddRange(new List<VoxelModelName> {
//                    VoxelModelName.Sword2, VoxelModelName.axe, VoxelModelName.broom, VoxelModelName.magicstaff,});
//                if ( settings.SwordLevel > 1)
//                {
//                    result.AddRange(new List<VoxelModelName> {
//                        VoxelModelName.Sword3, VoxelModelName.doubleaxe, VoxelModelName.pimpstick,});
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

        public void editPrivateHome(IntVector2 builderPos)
        {
            BeginCreationMode(builderPos + Map.Terrain.Area.PrivateHome.BuilderLocalPos + new IntVector2(1, 2));
        }

        public void BeginCreationModeIfAllowed()
        {
            if (LfRef.worldOverView.CanEdit(hero.WorldPosition.ChunkGrindex, this))
                BeginCreationMode();
            else
            {
                Print("No build permission");
            }
        }

        public void BeginCreationMode()
        {
            BeginCreationMode(hero.WorldPosition.ChunkGrindex);
        }
        public void BeginCreationMode(IntVector2 chunkCenterPos)
        {
            if (hero.Alive)
            {
                //if (LfRef.gamestate.LocalHostingPlayer.ClientPermissions != ClientPermissions.Spectator)
                //{
                    //check if area is done loading
                    IntVector2 pos = hero.ScreenPos;
                    if (!LfRef.chunks.ChunksDataLoaded(pos))
                    {
                        Print(StillLoadingMessage);
                        return;
                    }


                    Music.SoundManager.PlayFlatSound(LoadedSound.enter_build);

                    CloseMenu();
                    mode = PlayerMode.Creation;
                    //pick the area the player is standing at and send it to the designer
                    voxelDesignerStartPos = Editor.VoxelDesigner.HeroPosToCreationStartPos(chunkCenterPos);

                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_ClientStartingEditing,
                        Network.PacketReliability.Reliable, Index);
                    Map.WorldPosition.WriteChunkGrindex_Static(hero.ScreenPos, w); 


                    voxelDesigner = new Editor.VoxelDesigner(
                        voxelDesignerStartPos, localPData.view.Camera, menuArea(), this);
                    savedCamera = localPData.view.Camera;
                    localPData.view.Camera = voxelDesigner.Camera;
                    localPData.view.Camera.TiltX = savedCamera.TiltX;
                //}
                //else
                //{
                //    Print("No build permission");
                //}
            }
        }

        public void EndCreationMode()
        {
            if (voxelDesigner != null)
            {
                //pick the new design and add it to the map
                //Voxels.VoxelObjListData editChunk = voxelDesigner.voxels;
                //notify host
                if (Ref.netSession.IsClient)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_ClientEndingEditing,
                        Network.PacketReliability.ReliableLasy, Index);
                }

                mode = PlayerMode.Play;
                voxelDesigner.DeleteMe();
                voxelDesigner = null;
                savedCamera.TiltX = localPData.view.Camera.TiltX;
                localPData.view.Camera = savedCamera;

                hero.CheckIfUnderGround();
            }
        }
    }
}

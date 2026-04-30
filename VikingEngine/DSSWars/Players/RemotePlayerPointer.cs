using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Players
{
    class RemotePlayerPointer
    {
        PlayerNetState playerNetState = PlayerNetState.InMenu;
        MapDetailLayerType mapLayer = MapDetailLayerType.TerrainOverview2;
        Vector3 pointerGoalWp;
        Vector3 pointerWp;
        Vector3 pointerSpeed = Vector3.Zero;
        //Vector2 goalPointerPos = Vector2.Zero;
        Vector2 pointerIconPosDiff;

        public Graphics.Image pointer;
        Graphics.ImageAdvanced pointerGamerIcon;
        public Graphics.Image item;
        bool inGame;
        public SpriteName statusIcon;
        public SpriteName itemIcon = SpriteName.NO_IMAGE;

        public RemotePlayerPointer(Network.AbsNetworkPeer peer, bool inGame)
        {
            this.inGame = inGame;

            ImageLayers layer = inGame ? ImageLayers.Background4 : ImageLayers.Foreground1;

            pointer = new Graphics.Image(SpriteName.cmdClientPointer, Vector2.Zero,
                Engine.Screen.SmallIconSizeV2, layer, false);
            pointerGamerIcon = new Graphics.ImageAdvanced(SpriteName.defaultGamerIcon,
                Vector2.Zero, Engine.Screen.SmallIconSizeV2, ImageLayers.AbsoluteBottomLayer, false);
            pointerGamerIcon.LayerBelow(pointer);
            pointerIconPosDiff = pointer.Size * 0.4f;

            item = new Graphics.Image(SpriteName.MissingImage, Vector2.Zero,
                Engine.Screen.IconSizeV2 * 0.8f, ImageLayers.AbsoluteBottomLayer, true);
            item.LayerBelow(pointer);
            item.Visible = false;

            new SteamWrapping.LoadGamerIcon(pointerGamerIcon, peer, false);
        }

        public void Update(LocalPlayer playerView)
        {

            if (playerNetState != PlayerNetState.InMenu)
            {
                MapDetailLayerType viewLayer = playerView.mapLayer();
                if (viewLayer >= MapDetailLayerType.FullOverview4)
                {
                    viewLayer = MapDetailLayerType.FactionColors3;
                }

                //if (playerView.gameControls.map.camera.LookTarget != playerView.gameControls.map.camera.prevLookTarget)
                //{
                //    Vector2 moveCamera =
                //        Ref.draw.Camera.From3DToScreenPos(playerView.gameControls.map.camera.prevLookTarget, playerView.playerData.view.Viewport) -
                //        Ref.draw.Camera.From3DToScreenPos(playerView.gameControls.map.camera.LookTarget, playerView.playerData.view.Viewport);
                //    pointer.Position += moveCamera; 
                //}

                //goalPointerPos = Ref.draw.Camera.From3DToScreenPos(
                //        pointerGoalWp, playerView.playerData.view.Viewport);
                Vector3 diff = pointerGoalWp - pointerWp;

                if (diff.Length() > 0.1f)
                {
                    float expectedUpdates = (Ref.netSession.netUpdateRate / Ref.main.TargetElapsedTime.Milliseconds) * 1.5f;
                    pointerSpeed = diff / expectedUpdates;
                }
                else
                {
                    pointerSpeed = Vector3.Zero;
                }


                float transparent = viewLayer == mapLayer ? 1f : 0.5f;

                pointer.Visible = true;
                pointer.Opacity = transparent;
                pointerGamerIcon.Visible = true;
                pointerGamerIcon.Opacity = transparent;

                pointerWp += pointerSpeed;


                pointer.Position = Ref.draw.Camera.From3DToScreenPos(
                    pointerWp, playerView.playerData.view.Viewport);

                pointerGamerIcon.Position = pointer.Position + pointerIconPosDiff;

                if (playerNetState == PlayerNetState.Building)
                {
                    item.Visible = true;
                    item.Opacity = transparent;
                    item.Position = pointer.Position;
                }
                else
                {
                    item.Visible = false;
                }
            }
            else
            {
                pointer.Visible = false;
                pointerGamerIcon.Visible = false;
                item.Visible = false;
            }
        }

        public static void NetWriteLobbyPos(Vector2 pos, System.IO.BinaryWriter w)
        {
            //Använder % screen pos
            pos /= Engine.Screen.Area.Size;
            StreamLib.WriteVector(w, pos);
        }

        public static void netWrite(System.IO.BinaryWriter w, LocalPlayer player)
        {
            //var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqPlayerStatus, Network.PacketReliability.Unrelyable);
            w.Write((byte)player.playerNetState);
            if (player.playerNetState > PlayerNetState.InMenu)
            {
                if (player.playerNetState == PlayerNetState.Building)
                {
                    w.Write((byte)player.gameControls.build.CompressedBuildMode());
                }

                w.Write((byte)player.mapLayer());

                StreamLib.WriteVector(w, VectorExt.V3XZtoV2(player.gameControls.map.pointerPosWP));
            }
        }

        public void netRead(System.IO.BinaryReader r)
        {
            itemIcon = SpriteName.NO_IMAGE;
            playerNetState = (PlayerNetState)r.ReadByte();

            if (playerNetState > PlayerNetState.InMenu)
            {

                if (playerNetState == PlayerNetState.Building)
                {
                    Build.BuildAndExpandType build = (Build.BuildAndExpandType)r.ReadByte();
                    
                    switch (build)
                    {
                        case Build.BuildAndExpandType.NUM_NONE:
                            statusIcon = SpriteName.WarsHammer;
                            break;
                        case Build.BuildAndExpandType.DEMOLISH:
                            statusIcon = SpriteName.WarsHammerSub;

                            break;
                        default:
                            statusIcon = SpriteName.WarsHammerAdd;
                            IconName.Building(build, out itemIcon, out _);
                            item.SetSpriteName(itemIcon);
                            break;
                    }
                    
                    
                }
                else
                {
                    switch (playerNetState)
                    {
                        default:
                            statusIcon = SpriteName.WarsMapIcon;
                            break;
                        case PlayerNetState.Diplomacy:
                            statusIcon = SpriteName.WarsDiplomaticPoint;
                            break;
                        case PlayerNetState.City:
                            statusIcon = SpriteName.WarsCityHall;
                            break;
                        case PlayerNetState.Army:
                            statusIcon = SpriteName.WarsArmy;
                            break;

                    }
                }

                mapLayer = (Map.MapDetailLayerType)r.ReadByte();
                if (mapLayer >= MapDetailLayerType.FullOverview4)
                {
                    mapLayer = MapDetailLayerType.FactionColors3;
                }

                pointerGoalWp = VectorExt.V3FromXZ(StreamLib.ReadVector2(r), 0.1f);

                //if (inGame)
                //{
                //    goalPointerPos = Ref.draw.Camera.From3DToScreenPos(
                //        VectorExt.V3FromXZ(pointerPos, 0.1f), Engine.Draw.defaultViewport);
                //}
                //else
                //{
                //    goalPointerPos = pointerPos * Engine.Screen.Area.Size;
                //}

                
            }
            else
            {
                statusIcon = SpriteName.WarsHudHeadBarMenuIcon;
            }
        }

        //public bool Visible
        //{
        //    set
        //    {
        //        pointer.Visible = value;
        //        pointerGamerIcon.Visible = value;
        //        item.Visible = false;
        //    }
        //}

        public void DeleteMe()
        {
            pointer.DeleteMe();
            pointerGamerIcon.DeleteMe();
            item.DeleteMe();
        }
    }

    enum PlayerNetState
    {
        InMenu,

        Map,
        City,
        Building,
        Army,
        Diplomacy,
    }
}

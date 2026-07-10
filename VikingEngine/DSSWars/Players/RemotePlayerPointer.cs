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
        const float ColorEdgeSz = 2;

        PlayerNetState playerNetState = PlayerNetState.InMenu;
        MapDetailLayerType mapLayer = MapDetailLayerType.TerrainOverview2;
        Vector3 pointerGoalWp;
        Vector3 pointerWp;
        Vector3 pointerSpeed = Vector3.Zero;
        Vector2 pointerIconPosDiff, edgePosDiff;

        bool mouseOverHud;

        public Graphics.Image pointer;
        Graphics.ImageAdvanced pointerGamerIcon;
        public Graphics.Image colorFrame;
        public Graphics.Image item;
        //bool inGame;
        public SpriteName statusIcon;
        public SpriteName itemIcon = SpriteName.NO_IMAGE;

        public RemotePlayerPointer(Network.AbsNetworkPeer peer, bool inGame)
        {
            ImageLayers layer = inGame ? ImageLayers.Background4 : ImageLayers.Foreground1;

            pointer = new Graphics.Image(SpriteName.cmdClientPointer, Vector2.Zero,
                Engine.Screen.SmallIconSizeV2, layer, false);
            pointerGamerIcon = new Graphics.ImageAdvanced(SpriteName.defaultGamerIcon,
                Vector2.Zero, Engine.Screen.SmallIconSizeV2, ImageLayers.AbsoluteBottomLayer, false);
            pointerGamerIcon.LayerBelow(pointer);

            colorFrame = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, VectorExt.Add(pointerGamerIcon.size, ColorEdgeSz*2), ImageLayers.AbsoluteBottomLayer, false);
            colorFrame.LayerBelow(pointerGamerIcon);

            pointerIconPosDiff = pointer.Size * 0.4f;
            edgePosDiff = new Vector2(-ColorEdgeSz);

            item = new Graphics.Image(SpriteName.MissingImage, Vector2.Zero,
                Engine.Screen.IconSizeV2 * 0.6f, ImageLayers.AbsoluteBottomLayer, true);
            item.LayerBelow(pointer);
            item.Visible = false;

            new SteamWrapping.LoadGamerIcon(pointerGamerIcon, peer, false);
        }


        public void refreshDlc(bool supporterDLC)
        {
            pointer.SetSpriteName(SpriteName.cmdClientPointerMetallic);
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

                if (!mouseOverHud)
                {
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

                    pointerWp += pointerSpeed;
                }


                float transparent = viewLayer == mapLayer ? 1f : 0.5f;

                pointer.Visible = true;
                pointer.Opacity = transparent;
                pointerGamerIcon.Visible = true;
                pointerGamerIcon.Opacity = transparent;
                colorFrame.Visible = true;
                colorFrame.Opacity = transparent;
                               

                pointer.Position = Ref.draw.Camera.From3DToScreenPos(
                    pointerWp, playerView.playerData.view.Viewport);

                pointerGamerIcon.Position = pointer.Position + pointerIconPosDiff;
                colorFrame.Position = pointerGamerIcon.Position + edgePosDiff;

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
                colorFrame.Visible = false;
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
            
            w.Write((byte)player.playerNetState);
            if (player.playerNetState > PlayerNetState.InMenu)
            {

                if (player.playerNetState == PlayerNetState.Building)
                {
                    w.Write((byte)player.gameControls.build.CompressedBuildMode());
                }

                w.Write((byte)player.mapLayer());
                w.Write(player.hud.hudMouseOver());


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
                        case PlayerNetState.TypingChat:
                            statusIcon = SpriteName.LfChatBobbleIcon;
                            break;
                    }
                }

                mapLayer = (Map.MapDetailLayerType)r.ReadByte();
                mouseOverHud = r.ReadBoolean();
                if (mapLayer >= MapDetailLayerType.FullOverview4)
                {
                    mapLayer = MapDetailLayerType.FactionColors3;
                }

                pointerGoalWp = VectorExt.V3FromXZ(StreamLib.ReadVector2(r), 0.1f);

            }
            else
            {
                statusIcon = SpriteName.WarsHudHeadBarMenuIcon;
            }
        }

        public void DeleteMe()
        {
            pointer.DeleteMe();
            pointerGamerIcon.DeleteMe();
            colorFrame.DeleteMe();
            item.DeleteMe();
        }
    }

    enum PlayerNetState
    {
        InMenu,

        TypingChat,
        Map,
        City,
        Building,
        Army,
        Diplomacy,
    }
}

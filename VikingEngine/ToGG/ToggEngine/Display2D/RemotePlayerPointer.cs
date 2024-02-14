using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class RemotePlayerPointer
    {
        Vector2 pointerSpeed = Vector2.Zero;
        Vector2 goalPointerPos = Vector2.Zero;
        Vector2 pointerIconPosDiff;

        public Graphics.Image pointer;
        Graphics.ImageAdvanced pointerGamerIcon;
        public Graphics.Image item;
        bool inGame;

        public RemotePlayerPointer(Network.AbsNetworkPeer peer, bool inGame)
        {
            this.inGame = inGame;

            ImageLayers layer = inGame ? ImageLayers.Background4 : ImageLayers.Foreground1;

            pointer = new Graphics.Image(SpriteName.cmdClientPointer, goalPointerPos, 
                Engine.Screen.SmallIconSizeV2, layer, false);
            pointerGamerIcon = new Graphics.ImageAdvanced(SpriteName.defaultGamerIcon, 
                Vector2.Zero, Engine.Screen.SmallIconSizeV2, ImageLayers.AbsoluteBottomLayer, false);
            pointerGamerIcon.LayerBelow(pointer);
            pointerIconPosDiff = pointer.Size * 0.4f;

            item = new Graphics.Image(SpriteName.MissingImage, Vector2.Zero, 
                Engine.Screen.IconSizeV2, ImageLayers.AbsoluteBottomLayer, true);
            item.LayerBelow(pointer);
            item.Visible = false;

            new SteamWrapping.LoadGamerIcon(pointerGamerIcon, peer, false);
        }

        public void Update()
        {
            pointer.Position += pointerSpeed;
            item.Position = pointer.Position;
            pointerGamerIcon.Position = pointer.Position + pointerIconPosDiff;
        }

        public static void NetWriteLobbyPos(Vector2 pos, System.IO.BinaryWriter w)
        {
            //Använder % screen pos
            pos /= Engine.Screen.Area.Size;
            SaveLib.WriteVector(w, pos);
        }

        public void netRead(System.IO.BinaryReader r)
        {
            Vector2 pointerPos = SaveLib.ReadVector2(r);

            if (inGame)
            {
                goalPointerPos = Ref.draw.Camera.From3DToScreenPos(
                    VectorExt.V3FromXZ(pointerPos, 0.1f), Engine.Draw.defaultViewport);
            }
            else
            {
                goalPointerPos = pointerPos * Engine.Screen.Area.Size;
            }

            Vector2 diff = goalPointerPos - pointer.Position;

            if (diff.Length() > 2)
            {
                float expectedUpdates = (Ref.netSession.netUpdateRate / Ref.main.TargetElapsedTime.Milliseconds) * 1.5f;
                pointerSpeed = diff / expectedUpdates;
            }
            else
            {
                pointerSpeed = Vector2.Zero;
            }
        }

        public bool Visible
        {
            set
            {
                pointer.Visible = value;
                pointerGamerIcon.Visible = value;
                item.Visible = false;
            }
        }

        public void DeleteMe()
        {
            pointer.DeleteMe();
            pointerGamerIcon.DeleteMe();
            item.DeleteMe();
        }
    }
}

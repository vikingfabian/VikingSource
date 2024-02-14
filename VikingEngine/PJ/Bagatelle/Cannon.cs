using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class Cannon
    {
        public Graphics.Image image;
        IntervalF moveInterval;
        public int moveDir = 1;
        float speed, speedAdj;
        public float fireY;

        public Cannon(VectorRect activeArea, BagatellePlayState state)
        {
            image = new Graphics.Image(SpriteName.bagCannon, Vector2.Zero, state.BallScale * 0.6f * new Vector2(2f, 3f),
                ImageLayers.Lay3, true);

            float moveEdge = image.Width * 0.7f;
            moveInterval.Min = activeArea.X + moveEdge;
            moveInterval.Max = activeArea.Right - moveEdge;

            image.Xpos = moveInterval.Center;
            image.Ypos = activeArea.Y + state.BallScale * 0.4f;

            speed = activeArea.Height * 0.5f;
            speedAdj = speed * 0.05f;

            fireY = image.Ypos + image.Height * 0.4f;

            Vector2 railPos = new Vector2(activeArea.X, image.Ypos - image.Height * 0.45f);

            float railH = image.Height * 0.6f;
            float railW = (int)(railH / 64 * 158);

            Color railCol = ColorExt.GrayScale(0.8f);

            while (railPos.X < activeArea.Right)
            {
                Graphics.ImageAdvanced rail = new Graphics.ImageAdvanced(SpriteName.bagRails, railPos,
                    new Vector2(railW, railH), ImageLayers.Background4, false);
                railPos.X += railW;
                rail.Color = railCol;
            }
        }

        public void update()
        {
            image.Xpos += Ref.DeltaTimeSec * speed * moveDir;

            if (moveDir > 0)
            {
                if (image.Xpos >= moveInterval.Max)
                {
                    moveDir = -1;
                    image.Xpos = moveInterval.Max;

                    if (PjRef.HostingSession)
                    {
                       Ref.netSession.BeginWritingPacket(Network.PacketType.birdCannonMostRight, Network.PacketReliability.Reliable);
                    }
                }
            }
            else
            {
                if (image.Xpos <= moveInterval.Min)
                {
                    moveDir = 1;
                    image.Xpos = moveInterval.Min;
                }
            }
        }

        public void netReadCannonMostRight(Network.ReceivedPacket packet)
        {
            //moveDir = -1;
            float hostPosX = moveInterval.Max + TimeExt.MillsSecToSec(packet.sender.SendTime) * speed * moveDir;

            if (image.Xpos < hostPosX && moveDir < 0)
            {//too far ahead
                speed -= speedAdj;
            }
            else if (image.Xpos > hostPosX || moveDir > 0)
            {//too far behind
                speed += speedAdj;
            }
        }
    }
}

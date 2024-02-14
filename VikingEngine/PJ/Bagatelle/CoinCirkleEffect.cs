using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class CoinCirkleEffect : AbsInGameUpdateable
    {
        BagatellePlayState state;
        CoinCirkleEffectMember[] members;
        int time = 20;
        Vector2 scaleUp;
        Vector2 scale = Vector2.Zero;
        int nextId;

        public CoinCirkleEffect(Vector2 center, int count, BagatellePlayState state)
            : base(true)
        {
            this.state = state;
            nextId = state.NextGamerAssignedNetId(count);

            initCoinCirkleEffect(center, count);

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdCoinCirkleEffect, Network.PacketReliability.Reliable);
            w.Write(nextId);
            state.writePosition(center, w);
            w.Write((byte)count);
        }

        public CoinCirkleEffect(System.IO.BinaryReader r, BagatellePlayState state)
            : base(true)
        {
            this.state = state;

            nextId = r.ReadInt32();
            Vector2 center = state.readPosition(r);
            int count = r.ReadByte();//

            initCoinCirkleEffect(center, count);
        }

        void initCoinCirkleEffect(Vector2 center, int count)
        {
            members = new CoinCirkleEffectMember[count];

            Rotation1D dir = Rotation1D.D45;
            float angleStep = MathHelper.TwoPi / count;

            float radius;
            if (count <= 4)
            {
                radius = state.PegScale * 1.2f;
            }
            else
            {
                radius = state.PegScale * 2f;
            }

            for (int i = 0; i < count; ++i)
            {
                CoinCirkleEffectMember m = new CoinCirkleEffectMember();
                m.image = new Graphics.Image(SpriteName.birdCoin1, center, Vector2.One, BagLib.PickupsLayer, true);
                m.speed = dir.Direction(radius / time);
                dir.Add(angleStep);

                members[i] = m;
            }

            scaleUp = new Vector2(state.CoinScale / time);
        }

        public override void Time_Update(float time_ms)
        {
            scale += scaleUp;

            foreach (var m in members)
            {
                m.image.Position += m.speed;
                m.image.Size = scale;
            }

            if (--time <= 0)
            {
                foreach (var m in members)
                {
                    m.image.DeleteMe();
                    new Coin(m.image.Position, nextId++, state, GameObjectType.Coin);
                }

                this.DeleteMe();
            }
        }

        class CoinCirkleEffectMember
        {
            public Graphics.Image image;
            public Vector2 speed;
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class ItemMover : AbsInGameUpdateable
    {
        List<AbsGameObject> objects;

        Vector2 speed;
        VectorRect area;

        bool sinusMovement = false;
        float sinusCenterY;
        float sinusHeight;
       
        public int dictionaryId;
        bool local;
        BagatellePlayState state;

        public ItemMover(Vector2 pos, int dir, GameObjectType type, BagatellePlayState state)
            :base(true)
        {
            int networkId = state.board.NextItemIndex(20);

            init(networkId, pos, dir, type, state, true);

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdCreateItemMover, Network.PacketReliability.Reliable);
            w.Write((ushort)dictionaryId);
            w.Write(networkId);
            state.writePosition(pos, w);
            SaveLib.WriteDir(dir, w);
            w.Write((byte)type);
        }

        public ItemMover(System.IO.BinaryReader r, BagatellePlayState state)
            : base(true)
        {
            dictionaryId = r.ReadUInt16();
            int networkId = r.ReadInt32();
            Vector2 pos = state.readPosition(r);
            int dir = SaveLib.ReadDir(r);
            GameObjectType type = (GameObjectType)r.ReadByte();

            init(networkId, pos, dir, type, state, false);
        }

        void init(int networkId, Vector2 pos, int dir, GameObjectType type, BagatellePlayState state, bool local)
        {
            this.state = state;
            this.local = local;
            speed = new Vector2(dir * state.BallScale * 1.2f, 0f);
            area = state.gamePlayArea;
            area.AddRadius(Engine.Screen.IconSize);

            switch (type)
            {
                case GameObjectType.Spikes:
                    Spikes spikes = new Spikes(pos, networkId++, state);
                    objects = new List<AbsGameObject> { spikes };
                    break;
                case GameObjectType.BumpRefill:
                    {
                        var obj = new Coin(pos, networkId++, state, type);
                        objects = new List<AbsGameObject> { obj };
                    }
                    break;
                case GameObjectType.PowerUpBox:
                    {
                        var obj = new PowerUpBox(pos, networkId++, state);
                        objects = new List<AbsGameObject> { obj };
                    }
                    break;
                case GameObjectType.Snake:
                    {
                        const int TailCount = 12;

                        speed.X *= 1.5f;
                        sinusMovement = true;
                        sinusCenterY = pos.Y;
                        sinusHeight = state.PegScale * 0.4f;

                        objects = new List<AbsGameObject>(TailCount + 1);

                        SnakeHead head = new SnakeHead(pos, this, networkId++, dir, state);
                        objects.Add(head);

                        pos.X += state.PegScale * 0.2f * -dir;

                        for (int i = 0; i < TailCount; ++i)
                        {
                            pos.X += state.PegScale * 2f * -dir;
                            Peg tailPiece = new Peg(pos, networkId++, true, state);
                            objects.Add(tailPiece);
                        }
                    }
                    break;
                default:
                    {
                        const int TailCount = 8;

                        sinusMovement = true;
                        sinusCenterY = pos.Y;
                        sinusHeight = state.CoinScale * 0.3f;

                        objects = new List<AbsGameObject>(TailCount + 1);

                        Coin head = new Coin(pos, networkId++, state, GameObjectType.BigCoin10);
                        objects.Add(head);

                        pos.X += state.CoinScale * 0.2f * -dir;

                        for (int i = 0; i < TailCount; ++i)
                        {
                            pos.X += state.CoinScale * 1.2f * -dir;
                            Coin tailPiece = new Coin(pos, networkId++, state, GameObjectType.Coin);
                            objects.Add(tailPiece);
                        }
                    }
                    break;
            }

            foreach (var m in objects)
            {
                m.setMovingItemnLayer();
            }

            state.addItemMover(this, local);
        }

        public override void Time_Update(float time_ms)
        {
            for (int i = objects.Count - 1; i >= 0; --i)
            {
                Vector2 pos = objects[i].position + speed * Ref.DeltaGameTimeSec;
                //objects[i].position += speed * Ref.DeltaGameTimeSec;
                if (sinusMovement)
                {
                    pos.Y = sinusCenterY + (float)Math.Sin(pos.X / Engine.Screen.Height * 20f) * sinusHeight;
                }
                objects[i].position = pos;

                bool outOfBounds = false;
                if (speed.X > 0)
                {
                    outOfBounds = objects[i].image.Xpos > area.Right;
                }
                else
                {
                    outOfBounds = objects[i].image.Xpos < area.X;
                }

                if (outOfBounds ||
                     objects[i].isDeleted)
                {
                    if (objects[i].isDeleted == false)
                    {
                        objects[i].DeleteMe(true);
                    }

                    objects.RemoveAt(i);
                    if (objects.Count == 0)
                    {
                        DeleteMe();
                    }
                }
            }

            if (state.state == BagatellePlayState.State.AnnounceWinner)
            {
                foreach (var m in objects)
                {
                    m.updateShadow();
                }
            }
        }

        public void stopMover(bool local)
        {
            DeleteMe();

            if (local && Ref.netSession.InMultiplayerSession)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdStopItemMover, Network.PacketReliability.Reliable);
                w.Write((ushort)dictionaryId);
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            state.removeItemMover(this);
        }
    }
}

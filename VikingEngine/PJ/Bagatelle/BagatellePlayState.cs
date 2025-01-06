using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    
    class BagatellePlayState : AbsPJGameState
    {
        public float Gravity;
        public float BumbCollisionSpeedAdd;
        public float BallScale, CoinScale, PegScale, CoinPegScale;
        public float BumpWaveGoalScale, BumpWaveGoalScale_Big;
        public Vector2 shadowOffset;

        public Board board;
        SideBorders borders;

        int nextItemMoverId = 0;
        Dictionary<int, ItemMover> itemMovers = new Dictionary<int, ItemMover>(8);
        public DictionaryList<int, AbsGameObject> gameobjects = new DictionaryList<int, AbsGameObject>(64);
        protected List<LocalGamer> localgamers;
        Dictionary<int, RemoteGamer> remotegamers = new Dictionary<int, RemoteGamer>();
        public VectorRect gamePlayArea;

        public State state = 0;
        Time announceWinnerTime = new Time(4, TimeUnit.Seconds);
        int nextGamerAssignedNetId = 0;
        Texture2D bgTexture;
        Display.CountDownNumber countDownNumber;

        public BagatellePlayState(List2<GamerData> joinedGamers, Texture2D bgTexture, int matchCount)
            : base(true)  
        {
            this.bgTexture = bgTexture;
            

            this.matchCount = matchCount + 1;
            this.joinedLocalGamers = joinedGamers;

            if (PlatformSettings.PlayMusic)
            {
                Ref.music.nextRandomSong();
            }

            PjLib.BlackFade(true);
            
            nextGamerAssignedNetId = 10000;

            if (Ref.netSession.InMultiplayerSession)
            {
                nextGamerAssignedNetId += 1000 * Ref.netSession.LocalPeer().id;
            
                set1080pScreenArea();
            }

            
            gamePlayArea = activeScreenSafeArea;

           
            draw.ClrColor = Color.SaddleBrown;
            BallScale = activeScreenArea.Height * 0.06f;
            BumpWaveGoalScale = BallScale * 2.4f;
            BumpWaveGoalScale_Big = BumpWaveGoalScale * 1.5f;
            PegScale = BallScale * 0.6f;
            CoinScale = BallScale * 0.45f;
            CoinPegScale = PegScale;
            shadowOffset = new Vector2(0.05f, 0.053f) * BallScale;
            Gravity = 0.0001f * activeScreenArea.Height;
            BumbCollisionSpeedAdd = Gravity * 40;
                       

            float playerViewWidth = Convert.ToInt32(Engine.Screen.IconSize * 2f);

            gamePlayArea.AddToLeftSide(-playerViewWidth);
            borders = new SideBorders(gamePlayArea, activeScreenSafeArea);

            if (activeScreenArea.Height < Engine.Screen.Height)
            {//Top and bottom borders
                Graphics.Image topBorder = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, new Vector2(Engine.Screen.Width, activeScreenArea.Y), ImageLayers.Foreground3);
                topBorder.Color = Color.Black;

                Graphics.Image bottomBorder = new Graphics.Image(SpriteName.WhiteArea, new Vector2(0, activeScreenArea.Bottom), new Vector2(Engine.Screen.Width, Engine.Screen.Height - activeScreenArea.Bottom), ImageLayers.Foreground3);
                bottomBorder.Color = Color.Black;
            }
            //
            if (PjRef.host)
            {
                if (Ref.netSession.ableToConnect)
                {
                    writeGameStart();
                }
            }

            if (PjRef.host || this.matchCount > 1)
            {
                start();
            }

            Graphics.ImageAdvanced playAreaBg = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                new Vector2(gamePlayArea.X, activeScreenArea.Y), 
                new Vector2(gamePlayArea.Width, activeScreenArea.Height), ImageLayers.Background8,
                false, true);
            playAreaBg.Texture = bgTexture;
            playAreaBg.SetFullTextureSource();

            countDownNumber = new Display.CountDownNumber();
        }

        public int NextGamerAssignedNetId()
        {
            return nextGamerAssignedNetId++;
        }

        public int NextGamerAssignedNetId(int range)
        {
            int result = nextGamerAssignedNetId;
            nextGamerAssignedNetId += range;
            return result;
        }

        void start()
        {
            if (board == null)
            {
                board = new Board(gamePlayArea, this, BagLib.seed + (ulong)matchCount);

                if (joinedLocalGamers == null)
                {
                    joinedLocalGamers = new List2<GamerData>
                {
                    new GamerData(new Input.KeyboardButtonMap(Microsoft.Xna.Framework.Input.Keys.Space), JoustAnimal.Pig1),
                };
                }

                localgamers = new List<LocalGamer>(joinedLocalGamers.Count);

                for (int i = 0; i < joinedLocalGamers.Count; ++i)
                {
                    LocalGamer g = new LocalGamer(joinedLocalGamers[i], i, borders.getPlayerHudArea(joinedLocalGamers[i].GamerIndex), this);
                    localgamers.Add(g);
                }

                if (Ref.netSession.InMultiplayerSession)
                {
                    var remotes = Ref.netSession.RemoteGamers();
                    foreach (var remotePeer in remotes)
                    {
                        Player.RemoteGamerData group = (Player.RemoteGamerData)remotePeer.Tag;
                        if (group != null)
                        {
                            for (int i = 0; i < group.joinedGamers.Count; ++i)
                            {
                                RemoteGamer r = new RemoteGamer(i, remotePeer, borders.getPlayerHudArea(group.joinedGamers[i].GamerIndex), this);
                                remotegamers.Add(r.netIdHash(), r);
                            }
                        }
                    }
                }
            }
        }

        void writeGameStart()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdGameStart, Network.PacketReliability.Reliable);
            w.Write((byte)matchCount);
            w.Write(BagLib.seed);
            
            int nextGamerIx = joinedLocalGamers.Count;

            var remotes = Ref.netSession.RemoteGamers();
            w.Write((byte)remotes.Count);
            foreach (var remote in remotes)
            {
                w.Write(remote.FullId);
                w.Write((byte)nextGamerIx);

                if (remote.Tag == null)
                {
                    w.Write(false);
                }
                else
                {
                    w.Write(true);
                    VikingEngine.PJ.Player.RemoteGamerData remoteGroup = (VikingEngine.PJ.Player.RemoteGamerData)remote.Tag;

                    if (remoteGroup != null)
                    {
                        foreach (var gd in remoteGroup.joinedGamers)
                        {
                            gd.GamerIndex = nextGamerIx;
                            nextGamerIx++;
                        }
                    }
                }
            }
        }

        public void readGameStart(System.IO.BinaryReader r)
        {
            matchCount = r.ReadByte();
            BagLib.seed = r.ReadUInt64();

            int remoteCount = r.ReadByte();
            for (int i = 0; i < remoteCount; ++i)
            {
                ulong id = r.ReadUInt64();
                int index = r.ReadByte();

                if (r.ReadBoolean())
                {
                    if (id == Ref.netSession.LocalPeer().fullId)
                    {
                        foreach (var m in joinedLocalGamers)
                        {
                            m.GamerIndex = index;
                            index++;
                        }
                    }
                    else
                    {
                        var remote = Ref.netSession.GetPeer(id);
                        if (remote != null)
                        {
                            VikingEngine.PJ.Player.RemoteGamerData remoteData = (VikingEngine.PJ.Player.RemoteGamerData)remote.Tag;
                            if (remoteData != null)
                            {
                                foreach (var gd in remoteData.joinedGamers)
                                {
                                    gd.GamerIndex = index;
                                    index++;
                                }
                            }
                        }
                    }
                }
            }

            var remotes = Ref.netSession.RemoteGamers();
            foreach (var m in remotes)
            {
                VikingEngine.PJ.Player.RemoteGamerData remoteData = (VikingEngine.PJ.Player.RemoteGamerData)m.Tag;
                if (remoteData != null)
                {
                    for (int i = 1; i < remoteData.joinedGamers.Count; ++i)
                    {
                        remoteData.joinedGamers[i].GamerIndex = remoteData.joinedGamers[0].GamerIndex + i; 
                    }
                }
            }

            start();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);


            if (PlatformSettings.DevBuild && Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                Ref.music.nextRandomSong();
            }

            if (board != null)
            {
                if (baseClassUpdate()) return;

                switch (state)
                {
                    case State.CountDown:
                        updateGameObjects();
                        if (countDownNumber.update())
                        {
                            state++;
                        }
                        break;
                    case State.Playing:
                        updatePlay();
                        break;
                    case State.AnnounceWinner:
                        if (Ref.netSession.IsHostOrOffline)
                        {
                            if (announceWinnerTime.CountDown())
                            {
                                nextGameState();
                            }
                        }
                        break;
                }
                board.update();
            }

            Ref.music.Update();
        }

        void nextGameState()
        {
            if (matchCount >= BagLib.MatchCount)
            {
                new FinalScoreState();
            }
            else
            {
                new BagatellePlayState(joinedLocalGamers, bgTexture, matchCount);
            }
        }        

        void updatePlay()
        {
            int donePlaying = 0;
            foreach (var m in localgamers)
            {
                m.update();
                if (m.isDonePlaying())
                {
                    donePlaying++;
                }
            }
            foreach (var kv in remotegamers)
            {
                if (kv.Value.isDonePlaying())
                {
                    donePlaying++;
                }
            }

            updateGameObjects();

            if (donePlaying == localgamers.Count + remotegamers.Count)
            {
                state++;
                gamersLevelOver();
                Ref.music.stop(true);
            }
        }

        void updateGameObjects()
        {
            for (int i = gameobjects.Count - 1; i >= 0; --i)
            {
                if (gameobjects.list[i].isDeleted)
                {
                    gameobjects.RemoveAt(i);
                }
                else
                {
                    gameobjects.list[i].update();
                }
            }
        }

        void gamersLevelOver()
        {
            var gamers = allgamers();

            int lvlHighScore = 0;

            foreach (var m in gamers)
            {
                lvlHighScore = lib.LargestValue(m.levelScore, lvlHighScore);
            }

            foreach (var m in gamers)
            {
                m.onLevelOver(lvlHighScore);
            }
        }

        List<AbsGamer> allgamers()
        {
            List<AbsGamer> result = new List<AbsGamer>(localgamers.Count + remotegamers.Count);
            result.AddRange(localgamers);
            result.AddRange(remotegamers.Values);

            return result;
        }

        public override void NetworkReadPacket(Network.ReceivedPacket packet)
        {
            base.NetworkReadPacket(packet);

            switch (packet.type)
            {
                case Network.PacketType.birdUpdateBall:
                    {
                        AbsGameObject go;
                        if (gameobjects.dictionary.TryGetValue(AbsGameObject.ReadId(packet.r), out go))
                        {
                            go.netReadUpdate(packet.r);
                        }
                    }
                    break;
                case Network.PacketType.birdItemStatus:
                    {
                        AbsGameObject go;
                        if (gameobjects.dictionary.TryGetValue(AbsGameObject.ReadId(packet.r), out go))
                        {
                            int affectId = AbsGameObject.ReadId(packet.r);
                            AbsGameObject affectingItem = null;
                            if (affectId > 0)
                            {
                                gameobjects.dictionary.TryGetValue(affectId, out affectingItem);
                            }
                            go.netReadItemStatus(affectingItem, packet.r);
                        }
                    }
                    break;
                case Network.PacketType.birdBallBump:
                    {
                        AbsGameObject go;
                        if (gameobjects.dictionary.TryGetValue(AbsGameObject.ReadId(packet.r), out go))
                        {
                            go.GetBall().readBump(packet.r);
                            //((Ball)go).bump();
                        }
                    }
                    break;
                case Network.PacketType.birdBallSendHit:
                    {
                        AbsGameObject go;
                        if (gameobjects.dictionary.TryGetValue(AbsGameObject.ReadId(packet.r), out go))
                        {
                            if (go.localMember)
                            {
                                ((Ball)go).takeHit();
                            }
                        }
                    }
                    break;
                case Network.PacketType.birdBallKnockout:
                    {
                        AbsGameObject go;
                        if (gameobjects.dictionary.TryGetValue(AbsGameObject.ReadId(packet.r), out go))
                        {
                            ((Ball)go).knockoutEffects();
                        }
                    }
                    break;
                case Network.PacketType.birdSpawnBall:
                    {
                        RemoteGamer remote = readRemoteGamer(packet.r);
                        if (remote != null)
                        {
                            Ball b = new Ball(packet.r, remote, this);
                        }
                    }
                    break;
                case Network.PacketType.birdRemoveGameObject:
                    {
                        AbsGameObject go;
                        if (gameobjects.dictionary.TryGetValue(AbsGameObject.ReadId(packet.r), out go))
                        {
                            go.readDeleteMe(packet);
                        }
                    }
                    break;
                case Network.PacketType.birdGameStart:
                    if (state == State.CountDown)
                    {
                        throw new Exception("misplaced next state packet");
                    }
                    nextGameState();
                    break;
                case Network.PacketType.birdFinalScore:
                    if (state == State.CountDown)
                    {
                        throw new Exception("misplaced next state packet");
                    }
                    new FinalScoreState();
                    break;
                case Network.PacketType.birdCreateItemMover:
                    new ItemMover(packet.r, this);
                    break;
                case Network.PacketType.birdStopItemMover:
                    int moverId = packet.r.ReadUInt16();
                    ItemMover mover;
                    if (itemMovers.TryGetValue(moverId, out mover))
                    {
                        mover.stopMover(false);
                    }
                    break;
                case Network.PacketType.birdCoinCirkleEffect:
                    new CoinCirkleEffect(packet.r, this);
                    break;
                case Network.PacketType.birdCannonMostRight:
                    board.cannon.netReadCannonMostRight(packet);
                    break;
            }
        }

        public override void NetEvent_ConnectionLost(string reason)
        {
            base.NetEvent_ConnectionLost(reason);

            gamersLevelOver();
            new FinalScoreState();
        }
        public override void NetEvent_PeerLost(Network.AbsNetworkPeer gamer)
        {
            base.NetEvent_PeerLost(gamer);

            var remote = new List<RemoteGamer>(remotegamers.Values);

            foreach (var m in remote)
            {
                if (m.NetworkPeer.fullId == gamer.fullId)
                {
                    m.onLostGamer();
                    remotegamers.Remove(m.netIdHash());
                }
            }

            if (localgamers.Count + remotegamers.Count <= 1)
            {
                NetEvent_ConnectionLost("Lost peer: " + gamer.Gamertag);
            }
        }

        public override void NetUpdate()
        {
            if (board != null)
            {
                foreach (var gamer in localgamers)
                {
                    foreach (var ball in gamer.balls)
                    {
                        ball.netwriteUpdate();
                    }
                }
            }
        }

        public void addItemMover(ItemMover mover, bool local)
        {
            if (local)
            {
                mover.dictionaryId = nextItemMoverId++;
            }
            itemMovers.Add(mover.dictionaryId, mover);
        }
        public void removeItemMover(ItemMover mover)
        {
            itemMovers.Remove(mover.dictionaryId);
        }

        public void writePosition(Vector2 pos, System.IO.BinaryWriter w)
        {
            writeVector2(pos - gamePlayArea.Position, w);
        }
        public Vector2 readPosition(System.IO.BinaryReader r)
        {
            return readVector2(r) + gamePlayArea.Position;
        }

        public void writeVector2(Vector2 value, System.IO.BinaryWriter w)
        {
            w.Write(value.X / BallScale);
            w.Write(value.Y / BallScale);
        }
        public Vector2 readVector2(System.IO.BinaryReader r)
        {
            Vector2 result = Vector2.Zero;
            result.X = r.ReadSingle() * BallScale;
            result.Y = r.ReadSingle() * BallScale;

            return result;
        }

        public RemoteGamer readRemoteGamer(System.IO.BinaryReader r)
        {
            byte peerId; 
            int localIndex;
            AbsGamer.ReadGamer(r, out peerId, out localIndex);
            int hash = AbsGamer.IdToHash(peerId, localIndex);

            RemoteGamer result;
            if (remotegamers.TryGetValue(hash, out result) == false)
            {
                result = null;
                Debug.LogError("Could not find gamer " + peerId.ToString() + ", " + localIndex.ToString());
            }

            return result;
        }

        public enum State
        {
            CountDown,
            Playing,
            AnnounceWinner,
            Next,
        }
    }

    

    
}

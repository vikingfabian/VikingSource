using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.PJ
{
    static class PjLib
    {
        public static readonly PartyGameMode[] ModeViewOrder = 
        {
            PartyGameMode.Jousting,
            PartyGameMode.Bagatelle,

            PartyGameMode.MiniGolf,
            PartyGameMode.CarBall,
            
            PartyGameMode.Match3,

            PartyGameMode.SuperSmashBirds,
            PartyGameMode.Tank,
            PartyGameMode.SpacePirate,

            //PartyGameMode.Strategy,
            //PartyGameMode.SmashBirds,

            //PartyGameMode.RPG,
        };
        public const int SharedControllerMaxPlayers = 16;
        //mode art 222, 126

        public static readonly List<Buttons> XinputAvailableJoinButtons = new List<Buttons>
            {
                Buttons.A,
                Buttons.B,
                Buttons.X,
                Buttons.Y,
                Buttons.LeftShoulder,
                Buttons.RightShoulder,
                Buttons.LeftTrigger,
                Buttons.RightTrigger,
            };

        public static readonly PartyGameMode? DebugAutoStartMode = null;//PartyGameMode.SuperSmashBirds;

        public static readonly Color ZombieParticleColor = new Color(9, 200, 27);
        public static readonly Color ClearColor = Color.CornflowerBlue;
        public static readonly Color RedNumbersColor = new Color(1, 0.1f, 0.1f);

        public static readonly Color CoinPlusColor = Color.Yellow;
        public static readonly Color CoinMinusColor = Color.LightSalmon;


        public const int EndLevel = 20;

        public static float MusicVolume(bool joustSong)
        {
            return joustSong ? 0.7f : 0.22f;
        }

        
        public const int MaxRemotePlayers = 8;
        

        public const ImageLayers LayerFeather = ImageLayers.Foreground1;
        public const ImageLayers LayerMine = ImageLayers.Foreground2;
        public const ImageLayers LayerMineSplitter = ImageLayers.Foreground3;

        public const ImageLayers LayerFireBall = ImageLayers.Foreground4;
        public const ImageLayers LayerFireballParticle = ImageLayers.Foreground5;

        public const ImageLayers LayerBird = ImageLayers.Foreground6;

        public const ImageLayers LayerFlowerHead = ImageLayers.Foreground7;
        public const ImageLayers LayerFlowerPot = ImageLayers.Foreground8;

        
        public const ImageLayers LayerGround = ImageLayers.Lay2;
        public const ImageLayers LayerCoin = ImageLayers.Lay3;

        public const ImageLayers LayerCannonball = ImageLayers.Background1;
        public const ImageLayers LayerCannonballSmoke = ImageLayers.Background2;
        public const ImageLayers LayerPillar = ImageLayers.Background4;
        public const ImageLayers LayerLevelText = ImageLayers.Background4;

        public static readonly string ContentFolder = "PjContent" + DataStream.FilePath.Dir;
        public static readonly string SoundFolder = ContentFolder + "Sound" + DataStream.FilePath.Dir;
        public static readonly string MusicFolder = ContentFolder + "Music" + DataStream.FilePath.Dir;
        public const float FadeToBlackTime = 900f;
        
        public const float AnimalCharacterSzToBoundSz = 0.24f;
        

        public static void checkHostStatus()
        {
            if (Ref.netSession.IsClient && PjRef.host)
            {
                new GameState.JoinState(null);
            }
        }

        public static void NetWriteJoinedGamers(List<GamerData> joinedGamers)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdJoinedGamers, Network.PacketReliability.Reliable);
            w.Write(joinedGamers.Count);

            foreach (var m in joinedGamers)
            {
                m.netwrite(w);
            }
        }

        public static void NetReadJoindedGamers(Network.ReceivedPacket packet)
        {
            var gamer = GetRemote(packet.sender);

            int count = packet.r.ReadInt32();

            if (count == 0)
            {
                var defaultJoin = new GamerData();
                defaultJoin.networkPeer = packet.sender;
                gamer.joinedGamers = new List<GamerData> { defaultJoin };
            }
            else
            {
                var group = new List<GamerData>(count);

                for (int i = 0; i < count; ++i)
                {
                    GamerData gd;

                    if (gamer.joinedGamers != null && i < gamer.joinedGamers.Count)
                    {
                        gd = gamer.joinedGamers[i];
                    }
                    else
                    {
                        gd = new GamerData();
                    }

                    gd.netread(packet);
                    group.Add(gd);
                }

                gamer.joinedGamers = group;
            }
        }

        public static Player.RemoteGamerData GetRemote(Network.AbsNetworkPeer sender)
        {
            if (sender.Tag == null)
            {
                sender.Tag = new Player.RemoteGamerData(sender); ;
            }

            return (Player.RemoteGamerData)sender.Tag;
        }

        public static Graphics.Motion2d BlackFade(bool fadeOut, bool addToUpdate = true)
        {
            var area = Engine.Screen.Area;
            area.AddRadius(4f);
            var image = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.AbsoluteTopLayer);
            image.Color = Color.Black;
            image.Opacity = fadeOut ? 1f : 0f;
            Graphics.Motion2d fadeEffect = new Graphics.Motion2d(Graphics.MotionType.OPACITY, image, 
                fadeOut ? VectorExt.V2NegOne : Vector2.One, Graphics.MotionRepeate.NO_REPEAT, FadeToBlackTime, addToUpdate);
            return fadeEffect;
        }

        public static readonly Color MenuDarkBlueBG = new Color(41,70,132);
        
        public static void TryStartDlcPurchase(int dlcIndex)
        {
#if PCGAME
            if (Ref.steam != null && Ref.steam.DLC != null)
            {
                Ref.steam.DLC.OpenDlcStore(dlcIndex);
            }
#endif
        }

        public static void MemoryCleanUp()
        {
            Joust.JoustRef.ClearMem();
            CarBall.cballRef.ClearMem();
            Match3.m3Ref.ClearMem();
            MiniGolf.GolfRef.ClearMemory();
            Strategy.StrategyRef.map = null;
            SpaceWar.SpaceRef.ClearMem();

            GC.Collect();
        }

        public static void UpdateManagerInput(out bool startInput, out bool menuInput, out Input.InputSource user)
        {
            startInput = Input.Keyboard.KeyDownEvent(Keys.Enter);
            menuInput = Input.Keyboard.KeyDownEvent(Keys.Escape);
            user = Input.InputSource.DefaultPC;

            foreach (var m in Input.XInput.controllers)
            {
                if (m.KeyDownEvent(Buttons.Start))
                {
                    startInput = true;
                    user = new Input.InputSource(Input.InputSourceType.XController, m.Index);
                }

                if (m.BackButtonDownEvent())
                {
                    bool bdown = m.KeyDownEvent(Buttons.B);
                    menuInput = true;
                    user = new Input.InputSource(Input.InputSourceType.XController, m.Index);
                }
            }
        }
    }    

    enum PartyGameMode
    {
        Jousting,
        Strategy,
        Bagatelle,
        CarBall,
        MiniGolf,
        SuperSmashBirds,
        SpacePirate,
        Tank,
        Match3,
        MoneyRoll,
        MeatPie,
        NUM
    }

    enum GameModeAccessibility
    {
        DevOnly_1,
        DevAndDemo_2,
        Beta_3,
        Paid_4,
        Free_5,
        NUM
    }

    enum CoinValue
    {
        Value1,
        Value5,
        Value10,
        Value25,
        Value100,
    }
}

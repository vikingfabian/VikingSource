using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects.Toys
{
#if CMODE
    class RaceStarter : Update
    {
        const float CollectLapTimeDelay = 1600;
        Players.AbsPlayer owner;
        public Players.AbsPlayer Owner
        {
            get { return owner; }
        }
        
        List<GoalMemeber> goalTimes = new List<GoalMemeber>();
        List<GoalMemeber> goalTimesSorted = new List<GoalMemeber>();
        Timer.Basic announceTimer = new Timer.Basic();

        public static readonly float[] TimeList = new float[]
        {
            3000,
            5000,
            10000,
            20000,
            30000,
            60000,
            120000,
            300000,   
        };
        

        static List<RaceStarter> activeRaces = new List<RaceStarter>();
        const float Radius = 5;
        static readonly LootFest.ObjSingleBound Bound = LootFest.ObjSingleBound.QuickBoundingBox(Radius * PublicConstants.Half);
        static readonly LootFest.ObjSingleBound OutSideBound = LootFest.ObjSingleBound.QuickBoundingBox(Radius * 2);
        RaceMode mode = RaceMode.CountDown;
        List<RaceMember> members = new List<RaceMember>();
        Graphics.Mesh block;
        Players.PlayerSettings settings;
        Timer.Basic timer;
        byte startDir;
        bool hostedMember = false;
        bool playedSound = false;
        ToyType preferedVeichle;
        public ToyType PreferedVeichle
        {
            get { return preferedVeichle; }
        }

        public RaceStarter(Players.PlayerSettings settings, AbsRC startingToy)
            :base(true)
        {
       
            hostedMember = true;
            this.settings = settings;
            preferedVeichle =  (ToyType)startingToy.UnderType;
            basicInit(startingToy.Position, null);
            startDir = startingToy.Rotation.ByteDir;
            owner = startingToy.player;
            AddToy(startingToy);
            
            
            //to net
           System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RaceStarter,
                Network.PacketRelyability.RelyableLasy, owner.Index);
           
           w.Write(block.Position);
           w.Write(startDir);
           w.Write((byte)preferedVeichle);
           settings.NetworkWriteRace(w);
        }

        public RaceStarter(System.IO.BinaryReader r, Players.ClientPlayer owner)
            : base(true)
        {
            this.owner = owner;
            Vector3 pos = r.ReadVector3();
            startDir = r.ReadByte();
            preferedVeichle = (ToyType)r.ReadByte();
            settings = new Players.PlayerSettings(null, false, TextLib.EmptyString);
            settings.NetworkReadRace(r);
            
            basicInit(pos, owner);
        }

        void basicInit(Vector3 startPos, Players.ClientPlayer cp)
        {
            block = new Graphics.Mesh(LoadedMesh.cube_repeating,
                startPos, new Graphics.TextureEffect(TextureEffectType.LambertFixed, TileName.InterfaceBorder), Radius);
            block.Color = Color.Blue;
            
            new Graphics.Effects.Motion3d(MotionType.SCALE, block, lib.V3(0.5f), MotionRepeate.BackNForwardLoop,
                500, true);
            activeRaces.Add(this);
            float time = TimeList[settings.RaceCountDownTime]; //add latency
            if (cp != null)
            {
                time -= Ref.netSession.GetGamerFromID(cp.StaticNetworkIndex).RoundtripTime.Milliseconds * PublicConstants.Half;
            }
            timer = new Timer.Basic(time);

            LfRef.gamestate.LocalHostingPlayerPrint("Race starts in " +
                TextLib.TimeToText(time, false));

            for (currentTimeMark = 0; currentTimeMark < timeMarks.Length; currentTimeMark++)
            {
                if (TimeList[settings.RaceCountDownTime] > timeMarks[currentTimeMark])
                    break;
            }
        }

        static readonly float[] timeMarks = new float[] 
        {
            30000,
            10000,
            3000,
            2000,
            1000,
            float.MinValue,
        };
        int currentTimeMark;

        const string CountDownText = "Count down: ";
        public void AddToy(AbsRC toy)
        {
            //announce to the other members
            printGamerJoined(toy.GamerTag);

            if (toy.player != null)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.JoinedRace,
                    Network.PacketRelyability.RelyableLasy, toy.player.Index);
                w.Write(this.owner.StaticNetworkIndex);
            }


            //add
            members.Add(new RaceMember(toy));
            if (members.Count > 1)
            {
                toy.Print("Joined race"); 
                toy.Print(CountDownText + CountDown);
            }
            toy.Print(settings.RaceNumLaps.ToString() + "laps");
            toy.LockControls = true;
            toy.Race = this;
            toy.Position = block.Position;
            toy.Rotation = new Rotation1D(Rotation1D.ByteToRadians(startDir));
            toy.setImageDirFromRotation();
        }

        void printGamerJoined(string joiningGamer)
        {
            foreach (RaceMember rm in members)
            {
                rm.Toy.Print(joiningGamer + " joined race");
            }
        }

        public static void NetworkReadJoinedRace(System.IO.BinaryReader r, Network.AbsNetworkGamer gamer)
        {
            byte owner = r.ReadByte();
            foreach (RaceStarter rs in activeRaces)
            {
                rs.GamerJoined(owner, gamer);
            }
        }
        public void GamerJoined(byte owner, Network.AbsNetworkGamer gamer)
        {
            if (owner == this.owner.StaticNetworkIndex)
            {
                printGamerJoined(gamer.Gamertag);
            }
        }
        
        public override void Time_Update(float time)
        {
            if (mode != RaceMode.Racing)
            {
                if (timer.Update(time))
                {
                    mode++;
                    if (mode == RaceMode.Racing)
                    {
                        activeRaces.Remove(this);
                        //START
                        print("Go!");
                        foreach (RaceMember toy in members)
                        {
                            toy.Toy.LockControls = false;
                        }
                    }
                    else if (mode == RaceMode.Done)
                    {
                        block.DeleteMe();
                        timer.Set(2000);
                    }
                    else if (mode == RaceMode.Remove)
                    {
                        this.DeleteMe();
                    }
                }
            }
            if (mode != RaceMode.CountDown)
            {
                for (int i = members.Count -1; i >= 0; i--)
                {
                    RaceMember rm = members[i];
                    rm.Time += time;
                    rm.LapTime += time;
                    if (rm.OutsideCheckDone)
                    {
                        if (Intersect(rm.Toy))
                        {
                            //is insode the goal
                            //give feedback
                            rm.Toy.CheckPointFeedback();

                            rm.numLaps++;
                            rm.Toy.Print("Lap" + rm.numLaps.ToString() + ": " + TextLib.TimeToText(rm.LapTime, true));
                            rm.LapTime = 0;
                            if (rm.numLaps == settings.RaceNumLaps)
                            {
                                //goal!
                                goal(rm.Toy, rm.Time);
                                rm.Toy.Print("GOAL" + rm.numLaps.ToString() + ": " + TextLib.TimeToText(rm.Time, true));
                                block.Color = Color.Yellow;
                                mode = RaceMode.WaitIn;
                                timer.Set(TimeList[settings.RaceWaitInTime]);
                                members.RemoveAt(i);
                                rm.Toy.Race = null;
                            }

                            rm.OutsideCheckDone = false;
                        }
                    }
                    else
                    {
                        rm.OutsideCheckDone = OutSideBound.Intersect2(rm.Toy.CollisionBound, block.Position, rm.Toy.Position) == 0;
                    }
                }
                
            }
            if (hostedMember && announceTimer.Active)
            {
                if (announceTimer.Update(time))
                {
                    int winner = 0;
                    if (goalTimes.Count > 1)
                    {
                        winner = goalTimes[0].time < goalTimes[1].time ? 0 : 1;
                    }
                    GoalMemeber gm = goalTimes[winner];
                    goalTimesSorted.Add(gm);
                    goalTimes.RemoveAt(winner);
                    //announce winner
                    byte winposition = (byte)(goalTimesSorted.Count);
                    announceWinnerPos(winposition, gm.time, gm.player);
                }
            }
            else if (goalTimes.Count > 0)
            {
                announceTimer.Set(CollectLapTimeDelay);
            }

            if (mode== RaceMode.CountDown)
            {
                if (!playedSound && timer.TimeLeft <= 2000)      
                {
                    Music.SoundManager.PlayFlatSound(LoadedSound.ReadySetGo);
                    playedSound = true;
                }
                if (currentTimeMark < timeMarks.Length && timeMarks[currentTimeMark] >= timer.TimeLeft)
                {
                    print("Count down: " + TextLib.TimeToText(timeMarks[currentTimeMark], false));
                    currentTimeMark++;
                }
            }
            
        }

        public static void AnnounceWinnerPos(System.IO.BinaryReader r)
        {
            NetworkGamer gamer =  Ref.netSession.GetGamerFromID(r.ReadByte());
            if (gamer != null)
            {
                string name = gamer.Gamertag;
                byte winposition = r.ReadByte();
                float time = r.ReadSingle();
                printWinnerPos(name, winposition, time);
            }
        }

        void announceWinnerPos(byte winposition, float time, Players.AbsPlayer player)
        {
            printWinnerPos(player.Name, winposition, time);

            if (hostedMember)
            {
                //net share
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RaceAnnounceWinnerPos, 
                    Network.PacketRelyability.RelyableLasy, owner.Index);
                w.Write(player.StaticNetworkIndex);
                w.Write(winposition);
                w.Write(time);
            }
        }

        static void printWinnerPos(string name, byte winposition, float time)
        {
            List<string> winnerPos = new List<string> { "1st", "2nd", "3rd" };
            string pos;
            if (winposition <= winnerPos.Count)
            {
                pos = winnerPos[winposition - 1];
            }
            else
            {
                pos = winposition.ToString();
            }
            LfRef.gamestate.LocalHostingPlayer.PrintChat(pos + ": " + TextLib.TimeToText(time, true), name);//player.Name);
        }


        void goal(AbsRC toy, float time)
        {
            GoalMemeber gm = new GoalMemeber();
            gm.player = toy.player;
            gm.time = time;
            if (hostedMember)
            {
                goalTimes.Add(gm);
            }
            else
            {
                //share
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RaceGoal, owner.StaticNetworkIndex, 
                    Network.PacketRelyability.Relyable, LootfestLib.LocalHostIx);
                w.Write(time);
            }
        }
        public void Goal(System.IO.BinaryReader r, Players.ClientPlayer p)
        {
            GoalMemeber gm = new GoalMemeber();
            gm.player = p;
            gm.time = r.ReadSingle();
            goalTimes.Add(gm);
        }

        void print(string text)
        {
            foreach (RaceMember rc in members)
            {
                rc.Toy.Print(text);
            }
        }

        public override void DeleteMe()
        {
            block.DeleteMe();
            if (hostedMember)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.RaceStopped,
                    Network.PacketRelyability.RelyableLasy, owner.Index);
            }
            activeRaces.Remove(this);
            foreach (RaceMember rm in members)
            {
                rm.Toy.Race = null;
            }
            mode = RaceMode.Done;
            base.DeleteMe();
        }
        public bool Joinable
        {
            get { return mode == RaceMode.CountDown; }
        }
        public string CountDown
        {
            get { return TextLib.TimeToText(timer.TimeLeft, false); }
        }
        public static RaceStarter SearchRaces(AbsRC toy)
        {
            foreach (RaceStarter rs in activeRaces)
            {
                if (rs.Intersect(toy))
                    return rs;
            }
            return null;
        }
        public bool Intersect(AbsRC toy)
        {
            return Bound.Intersect2(toy.CollisionBound, block.Position, toy.Position) != 0;
        }

        enum RaceMode
        {
            CountDown,
            Racing,
            WaitIn,
            Done,
            Remove,
        }
    }
    struct GoalMemeber
    {
        public Players.AbsPlayer player;
        public float time;
    }
    class RaceMember
    {
        public AbsRC Toy;
        public int numLaps = 0;
        public float Time = 0;
        public float LapTime = 0;
        public bool OutsideCheckDone = false;
        public RaceMember(AbsRC Toy)
        {
            this.Toy = Toy;
        }
    }
#endif
}

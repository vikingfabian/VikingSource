using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.Map;

namespace VikingEngine.LootFest.Effects
{
    class BossDefeatedAnimation : AbsInGameUpdateable
    {
        //BlockMap.LevelEnum level;
        int state_0Start_1Yay_2Transport = 0;
        Time stateTimer = new Time(3, TimeUnit.Seconds);

        List<VikingEngine.LootFest.GO.PlayerCharacter.AbsHero> affectedHeroes;
        VikingEngine.LootFest.Players.BabyLocation location;

        public BossDefeatedAnimation(Network.ReceivedPacket packet)
            : base(true)//: this(Map.WorldPosition.NetworkRead(packet.r), false)
        {
            Map.WorldPosition wp = Map.WorldPosition.NetworkRead(packet.r);
            VikingEngine.LootFest.Players.BabyLocation location = (Players.BabyLocation)packet.r.ReadByte();

            init(wp, location);
            //runDuringPause = false;
        }

        public BossDefeatedAnimation(Map.WorldPosition wp, VikingEngine.LootFest.Players.BabyLocation location)
            : base(true)
        {
            //this.level = level;
            init(wp, location);
            
            //if (local)
            //{
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.BossDefeatedAnimation, 
                Network.PacketReliability.Reliable);
            //w.Write((byte)level);
            wp.Write(w);
            w.Write((byte)location);
            //}
        }

        void init(Map.WorldPosition wp, VikingEngine.LootFest.Players.BabyLocation location)
        {
            this.location = location;
            affectedHeroes = new List<GO.PlayerCharacter.AbsHero>(LfRef.LocalHeroes.Count);

            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                var h = LfRef.LocalHeroes.Array[i];

                if ((h.WorldPos.WorldXZ - wp.WorldXZ).SideLength() <= 100)
                {
                    affectedHeroes.Add(h);
                    h.player.Storage.progress.StoredBabyLocations.SetTrue(location);
                }
            }
        }

        public override void Time_Update(float time)    
        {
            if (stateTimer.CountDown())
            {
                state_0Start_1Yay_2Transport++;
                switch (state_0Start_1Yay_2Transport)
                {
                    case 1:
                        //for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
                        //{
                        //    if (LfRef.LocalHeroes[i].isInLevel(level))
                        //    {
                        //        LfRef.LocalHeroes[i].Express(VoxelModelName.express_thumbup);
                        //    }
                        //}
                        foreach (var m in affectedHeroes)
                        {
                            m.Express(VoxelModelName.express_thumbup);
                        }
                        stateTimer = new Time(2, TimeUnit.Seconds);
                        break;
                    case 2:

                        TeleportLocationId returnLocation = TeleportLocationId.Lobby;
                        if (location == Players.BabyLocation.Introduction)
                        {
                            returnLocation = TeleportLocationId.TutorialLobby;
                        }

                        foreach (var m in affectedHeroes)
                        {
                            m.teleportToLocation(returnLocation);
                        }
                        //for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
                        //{
                        //    if (LfRef.LocalHeroes[i].isInLevel(level))
                        //    {
                        //        //if (level == Map.WorldLevelEnum.EndBoss)
                        //        {
                        //            //LfRef.levels.GetLevel(Map.WorldLevelEnum.HappyNpcs, LfRef.LocalHeroes[i], teleportHero, null);
                        //            //var start = LfRef.worldOverView.GetLevel(Map.WorldLevelEnum.HappyNpcs).StartWorldPositionXZ;
                        //            //LfRef.LocalHeroes[i].TeleportTo(start, TeleportReason.JoinedEvent);
                        //        }
                        //        //else
                        //        {
                        //            LfRef.LocalHeroes[i].teleportToLocation(TeleportLocationId.Lobby);//.RestartTeleport(false);
                        //        }
                        //    }
                        //}
                        //level.ResetProgress();

                        //if (Engine.XGuide.IsTrial)
                        //{
                        //    new Timer.TimedAction0ArgTrigger(LfRef.gamestate.LocalHostingPlayer.endOfTrialMenu, lib.SecondsToMS(3f));
                        //}

                        this.DeleteMe();
                        break;
                }
            }
        }

        //void teleportHero(AbsWorldLevel level, VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero, object args)
        //{
        //    hero.TeleportFromNowhereTo(level.StartWorldPositionXZ, TeleportReason.JoinedEvent);
        //}
    }
}

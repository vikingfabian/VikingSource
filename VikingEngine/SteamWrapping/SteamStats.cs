#if PCGAME
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class SteamStats
    {
        TimeStamp prevCollectTime;
        public AbsGameStats gamestats;
        SteamCallResult<GlobalStatsReceived_t> globalStatsReceivedCallback;
        

        public SteamStats(AbsGameStats gamestats)
        {
            this.gamestats = gamestats;
        }

        public void OnUserStatsRecieved(UserStatsReceived_t caller)
        {
            //statsInitialized = true;
            prevCollectTime = TimeStamp.Now();
            gamestats.getStats();
            
        }

        public void upload()
        {
            gamestats.collectValues(prevCollectTime);
            prevCollectTime = TimeStamp.Now();
            var values = gamestats.listValues();
            foreach (var m in values)
            {
                m.setStat();
            }
            SteamAPI.SteamUserStats().StoreStats();
        }

        void beginRequestGlobalStats()
        {
            //funkar ej
            globalStatsReceivedCallback = new SteamCallResult<GlobalStatsReceived_t>(onGlobalStatsReceived);
            var apiCall = SteamAPI.SteamUserStats().RequestGlobalStats(365);
            globalStatsReceivedCallback.Set(apiCall);
        }

        void onGlobalStatsReceived(GlobalStatsReceived_t caller, bool ioFailure)
        {
            long global = 0;
            bool succeed = SteamAPI.SteamUserStats().GetGlobalStat("test", out global);
            lib.DoNothing();
        }
    }

    abstract class AbsGameStats
    {
        abstract public List<IStatsValue> listValues();
        abstract public void getStats();
        abstract public void collectValues(TimeStamp timePassed);
    }

    class TestGameStats : AbsGameStats
    {
        StatsInt testint = new StatsInt("testint");
        StatsFloat testfloat = new StatsFloat("testfloat");

        public override List<IStatsValue> listValues()
        {
            return new List<IStatsValue>
            {
                testint,
                testfloat,
            };
        }

        public override void getStats()
        {
            testint.getStat();
            testfloat.getStat();
        }

        public override void collectValues(TimeStamp timePassed)
        {
        }
    }

    interface IStatsValue
    {
        bool getStat();
        bool setStat();
        bool getUserStats(ulong user);
    }

    struct StatsInt : IStatsValue
    {
        public int value;
        public int valueAtGameStart;
        public string name;

        public StatsInt(string name)
        {
            this.name = name;
            this.value = 0;
            valueAtGameStart = 0;
        }

        public bool getStat()
        {
            bool result = SteamAPI.SteamUserStats().GetStat_Int(name, ref value);
            valueAtGameStart = value;
            return result;
        }

        public bool setStat()
        {
            return SteamAPI.SteamUserStats().SetStat_Int(name, value);
        }

        public bool getUserStats(ulong user)
        {
            return SteamAPI.SteamUserStats().GetUserStat_Int(user, name, ref value);
        }

        public void addToStartValue(int add)
        {
            value = valueAtGameStart + add;
        }

        public override string ToString()
        {
            return name + "(" + value.ToString() + ")";
        }

        public bool Bool { get { return value != 0; } set { this.value = value ? 1 : 0; } }
    }

    struct StatsFloat : IStatsValue
    {
        public float value;
        public float valueAtGameStart;
        public string name;

        public StatsFloat(string name)
        {
            this.name = name;
            this.value = 0;
            valueAtGameStart = 0;
        }

        public bool getStat()
        {
            return SteamAPI.SteamUserStats().GetStat_Float(name, ref value);
        }

        public bool setStat()
        {
            return SteamAPI.SteamUserStats().SetStat_Float(name, value);
        }

        public bool getUserStats(ulong user)
        {
            bool result = SteamAPI.SteamUserStats().GetUserStat_Float(user, name, ref value);
            valueAtGameStart = value;
            return result;
        }

        public void addToStartValue(float add)
        {
            value = valueAtGameStart + add;
        }

        public override string ToString()
        {
            return name + "(" + value.ToString() + ")";
        }
    }
}
#endif
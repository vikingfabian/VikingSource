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
        //TimeStamp prevCollectTime;
        float prevTotalTimeSec;
        public AbsGameStats gamestats;
        SteamCallResult<GlobalStatsReceived_t> globalStatsReceivedCallback;        

        public SteamStats(AbsGameStats gamestats)
        {
            this.gamestats = gamestats;
        }

        public void OnUserStatsRecieved(UserStatsReceived_t caller)
        {
            //statsInitialized = true;
            //prevCollectTime = TimeStamp.Now();
            prevTotalTimeSec = Ref.TotalTimeSec;
            gamestats.getStats();
            
        }

        public void upload()
        {
            gamestats.collectValues(prevTotalTimeSec);
            //prevCollectTime = TimeStamp.Now();
            prevTotalTimeSec = Ref.TotalTimeSec;
#if DEBUG
            if (!PlatformSettings.Debug_SteamStats)
            {
                return;
            }
#endif
            var values = gamestats.collectTimedValues();
            foreach (var m in values)
            {
                m.setStat();
            }
            SteamAPI.SteamUserStats().StoreStats();
        }

        public void initializeAllStatsOnSteam()
        {
            gamestats.initAndSetStats();
            upload();

        }

        public void beginRequestGlobalStats()
        {
            //funkar ej
            globalStatsReceivedCallback = new SteamCallResult<GlobalStatsReceived_t>(onGlobalStatsReceived);
            var apiCall = SteamAPI.SteamUserStats().RequestGlobalStats(30);
            globalStatsReceivedCallback.Set(apiCall);
        }

        void onGlobalStatsReceived(GlobalStatsReceived_t caller, bool ioFailure)
        {
            //long global = 0;
            //bool succeed = SteamAPI.SteamUserStats().GetGlobalStat("startnew_story", out global);

            var globalStats = gamestats.listGlobalStats();
            Debug.Log("##STEAM STATS##");
            foreach (var m in globalStats)
            {
                bool succeed = SteamAPI.SteamUserStats().GetGlobalStat(m.Name, out long global);
                if (succeed)
                {
                    Debug.Log(m.Name + ": " + global.ToString());
                }
            }
            lib.DoNothing();
        }
    }

    abstract class AbsGameStats
    {
        abstract public List<IStatsValue> collectTimedValues();
        abstract public List<IStatsValue> listGlobalStats();
        abstract public void getStats();
        abstract public void initAndSetStats();
        abstract public void collectValues(float prevTotalTimeSec);
    }

    class TestGameStats : AbsGameStats
    {
        StatsInt testint = new StatsInt("testint");
        StatsFloat testfloat = new StatsFloat("testfloat");

        public override List<IStatsValue> collectTimedValues()
        {
            return new List<IStatsValue>
            {
                testint,
                testfloat,
            };
        }
        public override List<IStatsValue> listGlobalStats()
        {
            throw new NotImplementedException();
        }

        public override void getStats()
        {
            testint.getStat();
            testfloat.getStat();
        }
        public override void initAndSetStats()
        {
            throw new NotImplementedException();
        }

        public override void collectValues(float prevTotalTimeSec)
        {
        }
    }

    interface IStatsValue
    {
        bool getStat();
        bool setStat();
        bool getUserStats(ulong user);

        public string Name { get; }
    }

    struct StatsInt : IStatsValue
    {
        public int value;
        public int valueAtGameStart;
        public string name;
        public bool hasSet;

        public StatsInt(string name)
        {
            hasSet = false;
            this.name = name;
            this.value = 0;
            valueAtGameStart = 0;
        }

        public void set(int value)
        {
            this.value = value;
            setStat();
        }

        public void add(int add)
        {
            this.value += add;
            setStat();
        }

        public void addOne()
        {
            this.value ++;
            setStat();
        }

        public void addOne_ifUnset()
        {
            if (!hasSet)
            {
                this.value++;
                setStat();
            }
        }

        public bool getStat()
        {
            bool result = SteamAPI.SteamUserStats().GetStat_Int(name, ref value);
            valueAtGameStart = value;
            return result;
        }

        public bool setStat()
        {
            hasSet = true;
#if DEBUG
            if (!PlatformSettings.Debug_SteamStats)
            { 
                return false;
            }
#endif
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

        public string Name => name;
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

        public void set(float value)
        {
            this.value = value;
            setStat();
        }

        public void add(float add)
        {
            this.value += add;
            setStat();
        }

        public bool getStat()
        {
            return SteamAPI.SteamUserStats().GetStat_Float(name, ref value);
        }

        public bool setStat()
        {
#if DEBUG
            if (!PlatformSettings.Debug_SteamStats)
            {
                return false;
            }
#endif
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

        public string Name => name;
    }
}
#endif
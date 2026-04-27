using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;

namespace VikingEngine.DSSWars.Data
{
    struct VictoryStats
    {
        public static readonly VictoryStats Empty = new VictoryStats();

        public int achievedCount;
        public int MaxDifficulty;

        public void addVictory(int difficulty)
        { 
            achievedCount++;
            MaxDifficulty = Math.Max(difficulty, MaxDifficulty);
        }
    }

    struct MetaProgression
    {
        const int Version = 0;

        const string Key_Test = "test";
        const string Key_TotalGameTimeMinutes = "GameTimeMinutes";
        const string Key_UnlockedDangerous = "UnlockedDangerous";

        const string Key_Act1_Victory_Boss = "Act1Boss";
        const string Key_Act1_Victory_Domination = "Act1Domination";
        const string Key_Act1_Victory_WorldPeace = "Act1Peace";

        const string Key_MaxDifficulty = "_Difficulty";

        const string Key_Act1_Victory_Boss_Difficulty = Key_Act1_Victory_Boss + Key_MaxDifficulty;
        const string Key_Act1_Victory_Domination_Difficulty = Key_Act1_Victory_Domination + Key_MaxDifficulty;
        const string Key_Act1_Victory_WorldPeace_Difficulty = Key_Act1_Victory_WorldPeace + Key_MaxDifficulty;

        ushort testValue = 313;

        public long totalGameTimeMinutes = 0;
        public bool unlockedDangerousSettings = false;

        public VictoryStats Act1_Victory_Boss = VictoryStats.Empty;
        public VictoryStats Act1_Victory_Domination = VictoryStats.Empty;
        public VictoryStats Act1_Victory_WorldPeace = VictoryStats.Empty;


        public MetaProgression()
        { }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);

            List<TwoStrings> keyValues = new List<TwoStrings>
            {
                new TwoStrings(Key_Test, testValue.ToString()),
                new TwoStrings(Key_TotalGameTimeMinutes, totalGameTimeMinutes.ToString()),
                new TwoStrings(Key_UnlockedDangerous, unlockedDangerousSettings.ToString()),
            };

            AddVictory(Act1_Victory_Boss, Key_Act1_Victory_Boss);
            AddVictory(Act1_Victory_Domination, Key_Act1_Victory_Domination);
            AddVictory(Act1_Victory_WorldPeace, Key_Act1_Victory_WorldPeace);

            w.Write(keyValues.Count);
            foreach (TwoStrings kv in keyValues)
            { 
                kv.write(w);
            }

            void AddVictory(VictoryStats victory, string key)
            {
                if (victory.achievedCount > 0)
                {
                    keyValues.Add(new TwoStrings(key, victory.achievedCount.ToString()));
                    keyValues.Add(new TwoStrings(key + Key_MaxDifficulty, key + victory.MaxDifficulty.ToString()));
                }
            }
        }
        public void read(System.IO.BinaryReader r)
        {
            var version = r.ReadInt32();

            int keyValuesCount = r.ReadInt32();
            for (int i = 0; i < keyValuesCount; i++)
            {
                var kv = new TwoStrings();
                kv.read(r);
                
                switch (kv.String1)
                {
                    case Key_Test:
                        ushort.TryParse(kv.String2, out testValue);
                        break;
                    case Key_TotalGameTimeMinutes:
                        long.TryParse(kv.String2, out totalGameTimeMinutes);
                        break;
                    case Key_UnlockedDangerous:
                        bool.TryParse(kv.String2, out unlockedDangerousSettings);
                        break;

                    case Key_Act1_Victory_Boss:
                        {
                            if (int.TryParse(kv.String2, out var value))
                            {
                                Act1_Victory_Boss.achievedCount = value;
                            }
                        }
                        break;
                    case Key_Act1_Victory_Boss_Difficulty:
                        {
                            if (int.TryParse(kv.String2, out var value))
                            {
                                Act1_Victory_Boss.MaxDifficulty = value;
                            }
                        }
                        break;

                    case Key_Act1_Victory_Domination:
                        {
                            if (int.TryParse(kv.String2, out var value))
                            {
                                Act1_Victory_Domination.achievedCount = value;
                            }
                        }
                        break;
                    case Key_Act1_Victory_Domination_Difficulty:
                        {
                            if (int.TryParse(kv.String2, out var value))
                            {
                                Act1_Victory_Domination.MaxDifficulty = value;
                            }
                        }
                        break;


                    case Key_Act1_Victory_WorldPeace:
                        {
                            if (int.TryParse(kv.String2, out var value))
                            {
                                Act1_Victory_WorldPeace.achievedCount = value;
                            }
                        }
                        break;
                    case Key_Act1_Victory_WorldPeace_Difficulty:
                        {
                            if (int.TryParse(kv.String2, out var value))
                            {
                                Act1_Victory_WorldPeace.MaxDifficulty = value;
                            }
                        }
                        break;

                }
            }
        }
    }
}

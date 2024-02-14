using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//xna
using VikingEngine.DataStream;
using Microsoft.Xna.Framework;
using VikingEngine.SteamWrapping;
using VikingEngine.LootFest.Players;

namespace VikingEngine.LootFest.Data
{
    class Progress
    {
        public bool inBossBattle = false;

        public Progress()
        {
            LfRef.progress = this;
        }

        public void BeginBossBattle()
        {
            inBossBattle = true;
        }

        public void EndBossBattle(VikingEngine.LootFest.Players.BabyLocation location, Map.WorldPosition babySpawnPos)
        {
            inBossBattle = false;

            //if (begin)
            //{
            //}
            //else
            //{
                //level.progress.completed = true;

                //if (level.bossSpawnPos.IsEmpty())
                //{
                //    throw new Exception();
                //}

                new GO.PickUp.Baby(new GO.GoArgs(babySpawnPos, (int)location));
                //for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
                //{
                //    //if (LfRef.LocalHeroes[i].LevelEnum == level.LevelEnum)
                //    //{
                //    //    LfRef.LocalHeroes[i].Player.Storage.completedLevels[(int)level.LevelEnum].completed = true;
                //    //    LfRef.LocalHeroes[i].Player.Storage.refreshUnlockedLevels();

                //    //    // Achievements and unlocks
                //    //    //if (level.LevelEnum == Map.WorldLevelEnum.EndBoss)
                //    //    //{
                //    //    //    Achievements.UnlockAchievement(AchievementIndex.DefeatFinalBoss, LfRef.LocalHeroes[i].Player);
                //    //    //}
                //    //    //if (level.LevelEnum == arraylib.LastListMemeber<Map.WorldLevelEnum>(LfLib.EnemyLevels))
                //    //    //{
                //    //    //    Achievements.UnlockAchievement(AchievementIndex.CompletAllLevels, LfRef.LocalHeroes[i].Player);
                //    //    //}
                //    //}
                //}
            //}
        }

        public bool GotUnlock(Players.PlayerStorage storage, UnlockType type)
        {
            if (PlatformSettings.DevBuild && DebugSett.GotAllUnlocks)
            {
                return true;
            }

            switch (type)
            {
                case UnlockType.Cape:
                    return storage.progress.achievements[(int)AchievementIndex.CaptureAllCardTypes];
                case UnlockType.Cards:
                    return false;//storage.progress.completedLevels[(int)VikingEngine.LootFest.BlockMap.LevelEnum.EndBoss].completed;
            }
            throw new NotImplementedException("Got Unlock: " + type.ToString());
        }

        public void UnlockEvent(Players.Player p, UnlockType type)
        {
            p.Print("Unlocked: " + type.ToString());
        }

        public void UnlockProgressPoint(Players.ProgressPoint p)
        {
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                LfRef.LocalHeroes[i].player.Storage.unlockPoint(p);
            }
        }
    }

    

    
}
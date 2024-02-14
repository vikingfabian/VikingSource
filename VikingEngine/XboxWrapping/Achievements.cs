#if XBOX
using System;
using System.Threading.Tasks;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.System;

namespace VikingEngine.XboxWrapping
{
    class Achievements
    {
        const uint Complete = 100;

        public void test(XboxGamer gamer)
        {
            int AchievementId = 1;

            var task = Task.Factory.StartNew(() =>
            {
                try
                {                
                    gamer.context.AchievementService.UpdateAchievementAsync(
                        gamer.user.XboxUserId, AchievementId.ToString(), Complete);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            });
        }

        public void Unlock(string achievementId)
        {
            XboxGamer gamer = Ref.xbox.gamer;

            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    gamer.context.AchievementService.UpdateAchievementAsync(
                        gamer.user.XboxUserId, achievementId, Complete);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            });
        }
    }    
}

#endif

#if XBOX
using Microsoft.Xbox.Services.Presence;
using System;

namespace VikingEngine.XboxWrapping
{
    class RichPresence
    {
        string newPresence = null, prevPresence = null;
        TimeStamp prevPresenceTime;

        public void Set(string presenceName)
        {
            newPresence = presenceName;
            Update();
        }

        public void Update()
        {
            if (newPresence != null &&
                prevPresenceTime.secPassed(6f) &&
                newPresence != prevPresence)
            {
                //locked to only update once per second
                Publish(newPresence);
                prevPresence = newPresence;
                newPresence = null;

                prevPresenceTime = TimeStamp.Now();
            }
        }

        public void Publish(string presenceName)
        {
            if (Ref.xbox.HasSignedInGamer)
            {
                try
                {
                    PresenceData data = new PresenceData(Ref.xbox.gamer.context.AppConfig.ServiceConfigurationId, presenceName);
                    Ref.xbox.gamer.context.PresenceService.SetPresenceAsync(true, data);

                    Debug.Log("PRESENCE: " + presenceName); 
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}
#endif
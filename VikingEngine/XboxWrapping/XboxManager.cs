#if XBOX
using System;
using System.Collections.Generic;
using Microsoft.Xbox;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Statistics.Manager;
using Microsoft.Xbox.Services.System;
//using Windows.Xbox.Storage; 
using Windows.Gaming.XboxLive.Storage;
using Windows.Storage.Streams;
using Windows.ApplicationModel;
using Microsoft.Xbox.Services.Presence;

namespace VikingEngine.XboxWrapping
{
    class XboxManager
    {
        StatisticManager StatsManager = null;
        public XboxGamer gamer;
        public Achievements achievements = new Achievements();
        public RichPresence presence = new RichPresence();
        public bool runningOnXbox;

        Timer.Basic refreshSignInTimer = new Timer.Basic(1000, true);

        public XboxManager()
        {
            Ref.xbox = this;

            runningOnXbox = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Contains("Xbox");

            Windows.ApplicationModel.Core.CoreApplication.Suspending +=
                new EventHandler<Windows.ApplicationModel.SuspendingEventArgs>(appSuspendEvent);

            Windows.ApplicationModel.Core.CoreApplication.Resuming +=
                new EventHandler<object>(appResumeEvent);

            //Windows.ApplicationModel.Core.CoreApplication.EnteredBackground +=
            //    new EventHandler<EnteredBackgroundEventArgs>(appEnteredBackgroundEvent);

            //Windows.ApplicationModel.Core.CoreApplication.LeavingBackground +=
            //    new EventHandler<LeavingBackgroundEventArgs>(appLeavingBackgroundEvent);


            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Activated +=
                new Windows.Foundation.TypedEventHandler<Windows.UI.Core.CoreWindow, Windows.UI.Core.WindowActivatedEventArgs>(appWindowActivatedEvent);
            

            PlatformSettings.XboxVersion = "Xbox ONE " + GetAppVersion();
        }

        void appWindowActivatedEvent(Windows.UI.Core.CoreWindow window, Windows.UI.Core.WindowActivatedEventArgs args)
        {
            Engine.XGuide.OnEnteredBackground(window.ActivationMode != Windows.UI.Core.CoreWindowActivationMode.ActivatedInForeground);
        }

        void appSuspendEvent(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            //Quick save is recommended
            Engine.XGuide.OnSuspend(false);
            Debug.Log("----SUSPEND");
        }

        void appResumeEvent(object sender, object args)
        {
            //Check user change
            Engine.XGuide.OnResume();
            Debug.Log("----RESUME");
        }

        //void appEnteredBackgroundEvent(object sender, EnteredBackgroundEventArgs e)
        //{
        //    Engine.XGuide.OnEnteredBackground(true);
        //}

        //void appLeavingBackgroundEvent(object sender, LeavingBackgroundEventArgs e)
        //{
        //    Engine.XGuide.OnEnteredBackground(false);
        //}

        void appActivatedEvent(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            Engine.XGuide.OnEnteredBackground(e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated);
        }

        public void onSignIn(XboxGamer gamer)
        {
            StatsManager = StatisticManager.SingletonInstance;
            StatsManager.AddLocalUser(gamer.user);
            StatsManager.DoWork();

            Ref.gamestate.GamerSignedInEvent(gamer);
            PJ.PjRef.storage.saveLoad(false, true);
        }
        public void onSignOut(XboxGamer gamer)
        {
            Ref.gamestate.GamerSignedOutEvent(gamer);
        }

        public void onGameStartup()
        {
            //TODO find all users
            gamer = new XboxGamer();
        }

        public void update()
        {
            presence.Update();

            //if (refreshSignInTimer.Update())
            //{
            //    if (!HasSignedInGamer)
            //    {
            //        gamer.SignIn();
            //    }
            //}
            //foreach (var m in Input.XInput.controllers)
            //{
            //    if (m.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.Y))
            //    {
            //        //debugStorage(true);
            //        achievements.test(gamer);
            //    }
            //    //if (m.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.Y))
            //    //{
            //    //    debugContentLoad();
            //    //}
            //}

            //Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().IsMain

            //Debug.Log(Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().IsHosted.ToString());

            //Debug.Log(Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.ActivationMode.ToString());
        }

        public bool HasSignedInGamer => gamer != null && gamer.signedIn;


        public static string GetAppVersion()
        {

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

        }


        //void Update()
        //{
        //    updateRichPresence();
        //}

        //string newPresence = null, prevPresence = null;
        //TimeStamp prevPresenceTime;

        //public void SetRichPresence(string presenceName)
        //{
        //    newPresence = presenceName;
        //    updateRichPresence();
        //}

        //void updateRichPresence()
        //{
        //    if (newPresence != null && 
        //        prevPresenceTime.secondsPassed(1f))
        //    { 
        //        //locked to only update once per second
        //        if (newPresence != prevPresence)
        //        {
        //            PublishRichPresence(newPresence);
        //            prevPresence = newPresence;
        //            newPresence = null;

        //            prevPresenceTime = TimeStamp.Now();
        //        }
        //    }
        //}

        //public void PublishRichPresence(string presenceName)
        //{
        //    if (HasSignedInGamer)
        //    {
        //        try
        //        {
        //            PresenceData data = new PresenceData(gamer.context.AppConfig.ServiceConfigurationId, presenceName);
        //            gamer.context.PresenceService.SetPresenceAsync(true, data);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogError(e.Message);
        //        }
        //    }
        //}
    }

}
#endif
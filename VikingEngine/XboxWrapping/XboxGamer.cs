#if XBOX
using System;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.System;
using Windows.Services.Store;

namespace VikingEngine.XboxWrapping
{
    class XboxGamer : Engine.AbsApiGamer
    {
        public XboxLiveUser user;
        public XboxLiveContext context;
        StoreAppLicense appLicense = null;
        StoreContext storeContext;

        private bool attemptingSignIn = false;

        public bool signedIn = false;
        string name = "NoN";

        public XboxGamer()
        {
            SignIn();
            XboxLiveUser.SignOutCompleted += SignOutCompleted;
        }

        public async void SignIn(bool attempt_silent = true)
        {
            if (attemptingSignIn)
                return;

            attemptingSignIn = true;
            user = new XboxLiveUser();

            if (!user.IsSignedIn)
            {
                var coreDispatcher = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher;
                if (attempt_silent)
                {
                    try
                    {
                        await user.SignInSilentlyAsync(coreDispatcher);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("SignInSilentlyAsync: " + e.Message);
                    }
                }
                if (!user.IsSignedIn)
                {
                    Debug.Log("Silent Sign-In failed, requesting sign in");
                    try
                    {
                        await user.SignInAsync(coreDispatcher);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("SingInAsync: " + e.Message);
                    }
                }
            }

            WriteInfo();
            attemptingSignIn = false;
            signedIn = user.IsSignedIn;

            if (user.IsSignedIn)
            {
                try
                {
                    context = new XboxLiveContext(user);
                    name = user.Gamertag;

                    Ref.xbox.onSignIn(this);
                    InitializeLicense();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }

        }


        /// <summary>
        /// This function gets license for the user... are we in trial mode or full mode?
        /// </summary>
        private async void InitializeLicense()
        {
            //if (storeContext == null && isMultiUser == true)
            //    context = StoreContext.GetForUser(windowsUser);
            //else 
            if (storeContext == null)
                storeContext = StoreContext.GetDefault();

            appLicense = await storeContext.GetAppLicenseAsync();

            // Register for the licenced changed event.
            storeContext.OfflineLicensesChanged += context_OfflineLicensesChanged;
        }

        private async void context_OfflineLicensesChanged(StoreContext sender, object args)
        {
            //Reload the license:
            appLicense = await sender.GetAppLicenseAsync();
        }
        /// <summary>
        /// Returns true if the signed in user hasn't purchased the full version of the game
        /// </summary>
        public bool IsTrialMode()
        {
            if (appLicense != null && appLicense.IsActive)
                return appLicense.IsTrial;

            return false;
        }

        private void SignOutCompleted(object sender, SignOutCompletedEventArgs e)
        {
            context = null;
            signedIn = false;
            name = "NoN";

            Ref.xbox.onSignOut(this);
        }

        public void WriteInfo()
        {


            Debug.Log("############ Xbox Live Info ############");
            Debug.Log(user.Gamertag + " SignedIn(" + user.IsSignedIn + ")");

            //if (PlatformSettings.DevBuild)
            //{
            //    switch (TextLib.LastLetters(user.Gamertag, 2))
            //    {
            //        case "84":
            //            Debug.Log("Nils Rune 0");
            //            break;
            //        case "38":
            //            Debug.Log("Nils Rune 1");
            //            break;
            //        case "52":
            //            Debug.Log("Viking");
            //            break;
            //    }
            //}

            Debug.Log(user.XboxUserId);
            Debug.Log(user.WebAccountId);
            Debug.Log("############ Xbox Live Info ############");
        }

        public override string Name()
        {
            return name;
        }
    }
}
#endif
using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Microsoft.Xna.Framework;

namespace Android
{
    [Activity(
        Label = "@string/app_name",
        MainLauncher = true,
        Icon = "@drawable/icon",
        AlwaysRetainTaskState = true,
        LaunchMode = LaunchMode.SingleInstance,
        ScreenOrientation = ScreenOrientation.SensorLandscape,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize
    )]
    public class Activity1 : AndroidGameActivity
    {
        private VikingEngine.MainGame _game;
        private View _view;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                //AppDomain.CurrentDomain.UnhandledException += (sender, ex) =>
                //{
                //    SaveCrashReport(ex.ExceptionObject?.ToString());
                //};

                base.OnCreate(bundle);

                new VikingEngine.SteamWrapping.SteamManager();
                _game = new VikingEngine.MainGame();
                _view = _game.Services.GetService(typeof(View)) as View;

                SetContentView(_view);
                _game.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                SaveCrashReport(ex.Message + System.Environment.NewLine + ex.StackTrace);

                throw;
            }
        }

        private void SaveCrashReport(string logText)
        {
            try
            {
                // Writes to: /storage/emulated/0/Android/data/<Your.Package.Name>/files/crash_log.txt
                string folder = GetExternalFilesDir(null)?.AbsolutePath ?? FilesDir?.AbsolutePath;
                string filePath = Path.Combine(folder, "last_crash.txt");

                File.WriteAllText(filePath, $"--- CRASH LOG {DateTime.Now} ---\n\n" + logText);
            }
            catch
            {
                // Fallback if writing fails
            }
        }
    }
}

using Steamworks;
using VikingEngine;
using VikingEngine.SteamWrapping;

//var steam = new VikingEngine.SteamWrapping.SteamManager();
new SteamManager();
//if (SteamAPI.Init())//PlatformSettings.SteamAPI)
//{
    
//    //steam.Initialize();
//}
using var game = new VikingEngine.MainGame();
game.Run();
SteamAPI.Shutdown();
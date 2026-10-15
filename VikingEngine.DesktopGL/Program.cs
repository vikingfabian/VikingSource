using Steamworks;
using VikingEngine;
using VikingEngine.SteamWrapping;

PlatformSettings.LoadResourcesInBackground = false;

new SteamManager();

using var game = new VikingEngine.MainGame();
game.Run();

SteamAPI.Shutdown();
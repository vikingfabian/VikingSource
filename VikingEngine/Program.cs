using VikingEngine;

var steam = new VikingEngine.SteamWrapping.SteamManager();
if (PlatformSettings.SteamAPI)
{
    steam.Initialize();
}
using var game = new VikingEngine.MainGame();
game.Run();
Ref.steam.Shutdown();
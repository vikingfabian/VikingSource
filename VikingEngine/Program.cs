using VikingEngine;

if (PlatformSettings.SteamAPI)
{
    new VikingEngine.SteamWrapping.SteamManager().Initialize();
}
using var game = new VikingEngine.MainGame();
game.Run();
Ref.steam.Shutdown();
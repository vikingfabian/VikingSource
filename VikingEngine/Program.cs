using Steamworks;
using VikingEngine;
using VikingEngine.SteamWrapping;


new SteamManager();

using var game = new VikingEngine.MainGame();
game.Run();

SteamAPI.Shutdown();
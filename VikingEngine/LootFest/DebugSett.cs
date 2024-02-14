using VikingEngine.LootFest.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest
{
    static class DebugSett
    {
        public static readonly bool UseCameraMenuMove = true;
        public static readonly bool EnableKeyrebindMenu = true;
        public static readonly bool UseDeferredRenderer = false;
        public static readonly bool DebugDeferredRenderer = false;
        public static readonly bool SwapCoordinatesMGOptionInMenu = true;
        public static readonly bool MarkLevelRoomCorners = false;
        public static readonly bool Debug3DParticles = false;
        public static readonly bool DebugNetwork = true;
        public const bool DebugAllAppearanceUnlocked = true;

        public const bool GotAllUnlocks = false;
        public const VikingEngine.Input.InputSourceType AutoSelectInputController = Input.InputSourceType.XController;
        public const bool ViewSaveFilesMenu = false;
        public const bool SpawnEnemies = true;
        public const bool SpawnNPCs = true;

        public static readonly TeleportLocationId? StartLocation = TeleportLocationId.Creative;//LevelEnum? StartArea = LevelEnum.Tutorial;
        public const bool UseLevelBounds = true;
        public static readonly int? WorldSeed = null; //Null är random
        public const bool MapGeneratingPngImg = false; //printar ut png bilder, kommer förlänga kart laddningen
        public const bool QuickStartCardGame = false;
        public const bool BlockMapEditor = false;
        public const bool DebugChunkLoading = false;

        public static readonly VikingEngine.LootFest.GO.Gadgets.ItemType? StartItem = null;//VikingEngine.LootFest.GO.Gadgets.ItemType.PickAxe;
    }
}

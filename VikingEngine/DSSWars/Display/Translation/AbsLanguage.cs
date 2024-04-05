using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Display.Translation
{
    abstract class AbsLanguage
    {
        public abstract string Lobby_Start { get; }
        public abstract string Lobby_LocalMultiplayerEdit { get; }

        public abstract string Lobby_LocalMultiplayerTitle { get; }
        public abstract string Lobby_LocalMultiplayerControllerRequired { get; }
        public abstract string Lobby_NextScreen { get; }

        public abstract string Lobby_ProfilesSelectTitle { get; }
        public abstract string Lobby_ProfileNumbered { get; }
        public abstract string Lobby_ProfileEdit { get; }

        public abstract string Lobby_MapSizeTitle { get; }

        public abstract string Lobby_MapSizeOptTiny { get; }
        public abstract string Lobby_MapSizeOptSmall { get; }
        public abstract string Lobby_MapSizeOptMedium { get; }
        public abstract string Lobby_MapSizeOptLarge { get; }
        public abstract string Lobby_MapSizeOptHuge { get; }
        public abstract string Lobby_MapSizeOptEpic { get; }

        public abstract string Lobby_MapSizeDesc { get; }

        public abstract string Lobby_ExitGame { get; }

        public abstract string Lobby_GameVersion { get; }

        public abstract string Player_DefaultName { get; }

        public abstract string ProfileEditor_Description { get; }

        public abstract string ProfileEditor_Bucket { get; }

        public abstract string ProfileEditor_OptionsMenu { get; }

        public abstract string ProfileEditor_FlagColorsTitle { get; }        
        public abstract string ProfileEditor_MainColor { get; }
        public abstract string ProfileEditor_Detail1Color { get; }
        public abstract string ProfileEditor_Detail2Color { get; }
        public abstract string ProfileEditor_PeopleColorsTitle { get; }
        public abstract string ProfileEditor_SkinColor { get; }
        public abstract string ProfileEditor_HairColor { get; }
        public abstract string ProfileEditor_PickColor { get; }
        public abstract string ProfileEditor_MoveImage { get; }

        public abstract string ProfileEditor_MoveImageLeft { get; }
        public abstract string ProfileEditor_MoveImageRight { get; }
        public abstract string ProfileEditor_MoveImageUp { get; }
        public abstract string ProfileEditor_MoveImageDown { get; }

        public abstract string ProfileEditor_DiscardAndExit { get; }
        public abstract string ProfileEditor_DiscardAndExitDescription { get; }
        public abstract string ProfileEditor_SaveAndExit { get; }


        public abstract string Hud_TotalIncome { get; }

        public abstract string Hud_Upkeep { get; }

        public abstract string Hud_WorkForce { get; }

        public abstract string Hud_GuardCount { get; }

        public abstract string Hud_StrengthRating { get; }

        public abstract string Hud_Immigrants { get; }

        public abstract string Building_NobelHouse { get; }

        public abstract string Building_DarkFactory { get; }

        public abstract string Settings_TotalDifficulty { get; }

        public abstract string Settings_DifficultyLevel { get; }


        public abstract string EndGameStatistics_Time { get; }
        public abstract string EndGameStatistics_SoldiersRecruited { get; }
        public abstract string EndGameStatistics_FriendlySoldiersLost { get; }
        public abstract string EndGameStatistics_EnemySoldiersKilled { get; }
        public abstract string EndGameStatistics_SoldiersDeserted { get; }
        public abstract string EndGameStatistics_CitiesCaptured { get; }
        public abstract string EndGameStatistics_CitiesLost { get; }
        public abstract string EndGameStatistics_BattlesWon { get; }
        public abstract string EndGameStatistics_BattlesLost { get; }
        public abstract string EndGameStatistics_WarsStartedByYou { get; }
        public abstract string EndGameStatistics_WarsStartedByEnemy { get; }
        public abstract string EndGameStatistics_AlliedFactions { get; }
        public abstract string EndGameStatistics_ServantFactions { get; }


    }
}

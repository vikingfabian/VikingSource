using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.DSSWars.Presentation
{
    class TodoTranslation
    {
        public string Technology_CannotReassign => "Tech cannot be reassigned until the research is done";
        public string Diplomacy_DeclareWarAgainst => "You will declare war against";
        public string Diplomacy_AllyCount => "Number of allies";
        public string Diplomacy_CostPerAlly => "Cost increases by {0} per ally";

        public string Event_ChanceOfFailure => "{0}% chance of failure";
        public string EventMessage_Event_Title => "Event";
        public string EventMessage_TheCohalition => "The cohalition";

        public string EventMessage_DarkHorde => "Dark hordes";
        public string EventMessage_DarkHordeKiller_Title => "Dark horde killer";
        public string EventMessage_DarkHordeKiller_Message => "Champion knigts have joined your service";

        public string Settings_ModelWaterFoam => "Water foam";
        public string Settings_ModelShadow => "Shadows";
        public string Settings_ModelShadowMapSize => "Shadows map size";
        public string Settings_Brightness => "Brightness";
        public string Settings_Mode_No_Achivements => "Achivements are not available.";
        public string Settings_FrameRate => "Frame rate";

        /// <summary>
        /// Steam Achievements
        /// </summary>
        public string Settings_ImportNoAchievement => "Block achievements on imported save files";

        public string Settings_Mode_Spectator_Description => "Just watch, or interfere with god powers.";
        public string GodPower => "God power";

        public string Building_TreeSprout_Description => "Plant a tree";
        public string Building_TreeSprout_Soft => "Soft tree sprout";
        public string Building_TreeSprout_Hard => "Hard tree sprout";

        public string GeneralSetting_SetAll => "Apply to all";

        public string Hud_All => "All";

        public string Hud_Previous => "Previuos";

        public string Hud_EffectWillStack => "The effect will stack";

        public string Info_WhenFoodRunsOut => "When food runs out, cities and armies will automatically purchase it from the black market.";

        //Factions
        /// <summary>
        /// Theme: Wood-elves who guard enchanted forests. Secretive, druidic, tied to nature spirits
        /// </summary>
        public string FactionName_SylvaranGlade => "Sylvaran Glade";

        /// <summary>
        /// Theme: Marsh-dwellers, human clans who thrive in bogs and waterways, masters of ambush.
        /// </summary>
        public string FactionName_DrelmirePact => "Drelmire Pact";

        /// <summary>
        /// Theme: Stubborn mountain dwarves, famed for masterwork steel and siegecraft.
        /// </summary>
        public string FactionName_KhazrunForgeclan => "Khazrûn Forgeclan";

        /// <summary>
        /// Nomadic steppe riders, swift raiders and proud cavalry culture.
        /// </summary>
        public string FactionName_VeylanHorselords => "Veylan Horselords";

        /// <summary>
        /// Theme: A human religious order devoted to the Eternal Flame. Zealous and uncompromising.
        /// </summary>
        public string FactionName_ThalosCovenant => "Thalos Covenant";

        /// <summary>
        /// Theme: Coastal defenders, human mariners and sea-watchers, sworn to protect against pirates.
        /// </summary>
        public string FactionName_NerathianTideguard => "Nerathian Tideguard";

        /// <summary>
        ///  Theme: Desert-dwellers, scarred nomads once driven from their homeland. Fierce survivalists.
        /// </summary>
        public string FactionName_SkaruunExiles => "Skaruun Exiles";

        /// <summary>
        ///Theme: Dragon-worshipping cult/kingdom, ruled by dragonblooded warlords.
        /// </summary>
        public string FactionName_DraktharDominion => "Drakthar Dominion";

        /// <summary>
        /// Theme: Brutal mercenary brotherhood, sellswords bound by strict contracts.
        /// </summary>
        public string FactionName_MalrekIronbound => "Malrek Ironbound";

        //-----


        /// <summary>
        /// Theme: A modest barony nestled in fertile valleys, proud of its ancient stone keeps.
        /// </summary>
        public string FactionName_BranthollowBarony => "Branthollow Barony";

        /// <summary>
        /// Theme: Grain-rich plains kingdom, known for horse-breeding and wheat harvests.
        /// </summary>
        public string FactionName_DunwadeHold => "Dunwade Hold";

        /// <summary>
        /// Theme: Borderland march-lords, stern folk living in fortified towns along contested lands.
        /// </summary>
        public string FactionName_CaerwynMarches => "Caerwyn Marches";

        /// <summary>
        /// Theme: Mining folk in a rugged valley, semi-independent but loyal to their lords.
        /// </summary>
        public string FactionName_StonevaleFreehold => "Stonevale Freehold";

        /// <summary>
        /// Theme: Small forested domain, famed for herbalists and bowmen.
        /// </summary>
        public string FactionName_GlenmereLordship => "Glenmere Lordship";

        /// <summary>
        /// Theme: A minor princely house clinging to its old glory, proud but weakened.
        /// </summary>
        public string FactionName_ArveldonPrincipality => "Arveldon Principality";

        /// <summary>
        /// Theme: Coastal duchy of fisherfolk and shipwrights, always at odds with pirates.
        /// </summary>
        public string FactionName_WestmereReaches => "Westmere Reaches";

        /// <summary>
        /// Theme: Small marcher state, thorny hedges and palisades mark their borders.
        /// </summary>
        public string FactionName_ThornwickWardens => "Thornwick Wardens";

        /// <summary>
        /// Theme: A sleepy lakeside domain, romanticized in ballads but of little power.
        /// </summary>
        public string FactionName_EvermereFief => "Evermere Fief";

        /// <summary>
        /// Theme: Forest hillfolk, stubborn and hearty, famed for boar-hunting feasts.
        /// </summary>
        public string FactionName_BryndralHollow => "Bryndral Hollow";

        /// <summary>
        /// Theme: Warrior tribe from a desert coastal region
        /// </summary>
        public string FactionName_Mendog => "Mendog";

        /// <summary>
        /// Theme: Warrior tribe from a desert coastal region
        /// </summary>
        public string FactionName_Minde=> "Minde";

        /// <summary>
        /// A proud family of royal knights
        /// </summary>
        public string FactionName_FloKingdom=> "Flo kingdom";

        /// <summary>
        /// A macon family with the secrets to advanced buildings
        /// </summary>
        public string FactionName_CarolusKeksenmark=> "Carolus Keksenmark";


        /// <summary>
        /// Theme: A confederation of hobbit villages along winding streams, known for gardens, festivals, and fiercely defended borders when threatened.
        /// </summary>
        public string FactionName_BramblebrookHill => "Bramblebrook Hill";

        /// <summary>
        /// Theme: Hill-dwelling hobbits in cozy burrows, famous for cider, storytelling, and their legendary hospitality (and occasional trickery).
        /// </summary>
        public string FactionName_Tumblehill => "Tumblehill";

        /// <summary>
        /// Theme: A democracy run house with focus on politics and military might. Looks down on any outsiders.
        /// </summary>
        public string FactionName_Etheleorthe => "Etheleorðe";
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Presentation
{
    partial class AbsLanguage
    {
        // Faction names
        //--
        abstract public string FactionName_Ellium { get; }
        abstract public string FactionName_GrakPushdug { get; }
        abstract public string FactionName_Draugost { get; }

        public abstract string FactionName_Starshield { get; }
        public abstract string FactionName_Bluepeak { get; }
        public abstract string FactionName_Hoft { get; }
        public abstract string FactionName_RiverStallion { get; }
        public abstract string FactionName_Sivo { get; }

        public abstract string FactionName_AelthrenConclave { get; }
        public abstract string FactionName_VrakasundEnclave { get; }
        public abstract string FactionName_Tormürd { get; }
        public abstract string FactionName_ElderysFyrd { get; }
        public abstract string FactionName_Hólmgar { get; }
        public abstract string FactionName_RûnothalOrder { get; }
        public abstract string FactionName_GrimwardEotain { get; }
        public abstract string FactionName_SkaeldraHaim { get; }
        public abstract string FactionName_MordwynnCompact { get; }
        public abstract string FactionName_AethmireSovren { get; }

        public abstract string FactionName_ThurlanKin { get; }
        public abstract string FactionName_ValestennOrder { get; }
        public abstract string FactionName_Mournfold { get; }
        public abstract string FactionName_OrentharTribes { get; }
        public abstract string FactionName_SkarnVael { get; }
        public abstract string FactionName_Glimmerfell { get; }
        public abstract string FactionName_BleakwaterFold { get; }
        public abstract string FactionName_Oathmaeren { get; }
        public abstract string FactionName_Elderforge { get; }
        public abstract string FactionName_MarhollowCartel { get; }

        public abstract string FactionName_TharvaniDominion { get; }
        public abstract string FactionName_KystraAscendancy { get; }
        public abstract string FactionName_GildenmarkUnion { get; }
        public abstract string FactionName_AurecanEmpire { get; }
        public abstract string FactionName_BronzeReach { get; }
        public abstract string FactionName_ElbrethGuild { get; }
        public abstract string FactionName_ValosianSenate { get; }
        public abstract string FactionName_IronmarchCompact { get; }
        public abstract string FactionName_KaranthCollective { get; }
        public abstract string FactionName_VerdicAlliance { get; }

        public abstract string FactionName_OrokhCircles { get; }
        public abstract string FactionName_TannagHorde { get; }
        public abstract string FactionName_BraghkRaiders { get; }
        public abstract string FactionName_ThurvanniStonekeepers { get; }
        public abstract string FactionName_KolvrenHunters { get; }
        public abstract string FactionName_JorathBloodbound { get; }
        public abstract string FactionName_UlrethSkycallers { get; }
        public abstract string FactionName_GharjaRavagers { get; }
        public abstract string FactionName_RavkanShield { get; }
        public abstract string FactionName_FenskaarTidewalkers { get; }

        public abstract string FactionName_HroldaniStormguard { get; }
        public abstract string FactionName_SkirnirWolfkin { get; }
        public abstract string FactionName_ThalgarBearclaw { get; }
        public abstract string FactionName_VarnokRimeguard { get; }
        public abstract string FactionName_KorrakFirehand { get; }
        public abstract string FactionName_MoongladeGat { get; }
        public abstract string FactionName_DraskarSons { get; }
        public abstract string FactionName_YrdenFlamekeepers { get; }
        public abstract string FactionName_BrundirWarhorns { get; }
        public abstract string FactionName_OltunBonecarvers { get; }
        public abstract string FactionName_HaskariEmber { get; }
        public abstract string FactionName_ZalfrikThunderborn { get; }
        public abstract string FactionName_BjorunStonetender { get; }
        public abstract string FactionName_MyrdarrIcewalkers { get; }
        public abstract string FactionName_SkelvikSpear { get; }
        public abstract string FactionName_VaragThroatcallers { get; }
        public abstract string FactionName_Durakai { get; }
        public abstract string FactionName_FjornfellWarhowl { get; }
        public abstract string FactionName_AshgroveWard { get; }
        public abstract string FactionName_HragmarHorncarvers { get; }

        //Shadow update

        /// <summary>
        /// Theme: Wood-elves who guard enchanted forests. Secretive, druidic, tied to nature spirits
        /// </summary>
        public abstract string FactionName_SylvaranGlade { get; }

        /// <summary>
        /// Theme: Marsh-dwellers, human clans who thrive in bogs and waterways, masters of ambush.
        /// </summary>
        public abstract string FactionName_DrelmirePact { get; }

        /// <summary>
        /// Theme: Stubborn mountain dwarves, famed for masterwork steel and siegecraft.
        /// </summary>
        public abstract string FactionName_KhazrunForgeclan { get; }

        /// <summary>
        /// Nomadic steppe riders, swift raiders and proud cavalry culture.
        /// </summary>
        public abstract string FactionName_VeylanHorselords { get; }

        /// <summary>
        /// Theme: A human religious order devoted to the Eternal Flame. Zealous and uncompromising.
        /// </summary>
        public abstract string FactionName_ThalosCovenant { get; }

        /// <summary>
        /// Theme: Coastal defenders, human mariners and sea-watchers, sworn to protect against pirates.
        /// </summary>
        public abstract string FactionName_NerathianTideguard { get; }

        /// <summary>
        /// Theme: Desert-dwellers, scarred nomads once driven from their homeland. Fierce survivalists.
        /// </summary>
        public abstract string FactionName_SkaruunExiles { get; }

        /// <summary>
        /// Theme: Dragon-worshipping cult/kingdom, ruled by dragonblooded warlords.
        /// </summary>
        public abstract string FactionName_DraktharDominion { get; }

        /// <summary>
        /// Theme: Brutal mercenary brotherhood, sellswords bound by strict contracts.
        /// </summary>
        public abstract string FactionName_MalrekIronbound { get; }

        // -----

        /// <summary>
        /// Theme: A modest barony nestled in fertile valleys, proud of its ancient stone keeps.
        /// </summary>
        public abstract string FactionName_BranthollowBarony { get; }

        /// <summary>
        /// Theme: Grain-rich plains kingdom, known for horse-breeding and wheat harvests.
        /// </summary>
        public abstract string FactionName_DunwadeHold { get; }

        /// <summary>
        /// Theme: Borderland march-lords, stern folk living in fortified towns along contested lands.
        /// </summary>
        public abstract string FactionName_CaerwynMarches { get; }

        /// <summary>
        /// Theme: Mining folk in a rugged valley, semi-independent but loyal to their lords.
        /// </summary>
        public abstract string FactionName_StonevaleFreehold { get; }

        /// <summary>
        /// Theme: Small forested domain, famed for herbalists and bowmen.
        /// </summary>
        public abstract string FactionName_GlenmereLordship { get; }

        /// <summary>
        /// Theme: A minor princely house clinging to its old glory, proud but weakened.
        /// </summary>
        public abstract string FactionName_ArveldonPrincipality { get; }

        /// <summary>
        /// Theme: Coastal duchy of fisherfolk and shipwrights, always at odds with pirates.
        /// </summary>
        public abstract string FactionName_WestmereReaches { get; }

        /// <summary>
        /// Theme: Small marcher state, thorny hedges and palisades mark their borders.
        /// </summary>
        public abstract string FactionName_ThornwickWardens { get; }

        /// <summary>
        /// Theme: A sleepy lakeside domain, romanticized in ballads but of little power.
        /// </summary>
        public abstract string FactionName_EvermereFief { get; }

        /// <summary>
        /// Theme: Forest hillfolk, stubborn and hearty, famed for boar-hunting feasts.
        /// </summary>
        public abstract string FactionName_BryndralHollow { get; }

        /// <summary>
        /// Theme: Warrior tribe from a desert coastal region
        /// </summary>
        public abstract string FactionName_Mendog { get; }

        /// <summary>
        /// Theme: Warrior tribe from a desert coastal region
        /// </summary>
        public abstract string FactionName_Minde { get; }

        /// <summary>
        /// A proud family of royal knights
        /// </summary>
        public abstract string FactionName_FloKingdom { get; }

        /// <summary>
        /// A macon family with the secrets to advanced buildings
        /// </summary>
        public abstract string FactionName_CarolusKeksenmark { get; }

        /// <summary>
        /// Theme: A confederation of hobbit villages along winding streams, known for gardens, festivals, and fiercely defended borders when threatened.
        /// </summary>
        public abstract string FactionName_BramblebrookHill { get; }

        /// <summary>
        /// Theme: Hill-dwelling hobbits in cozy burrows, famous for cider, storytelling, and their legendary hospitality (and occasional trickery).
        /// </summary>
        public abstract string FactionName_Tumblehill { get; }

        /// <summary>
        /// Theme: A democracy run house with focus on politics and military might. Looks down on any outsiders.
        /// </summary>
        public abstract string FactionName_Etheleorthe { get; }
    }
}

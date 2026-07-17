using System;
using System.Collections.Generic;
using System.IO;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Command;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.LootFest.Players;


namespace VikingEngine.DSSWars.Players
{
    partial class AiPlayer : AbsPlayer
    {
        public Time nextDecisionTimer = new Time(1000);

        const int PurchaseOrderType_None = 0;
        const int PurchaseOrderType_Army = 1;
        
        string name;

        //Center attack focus and buy focus on the main army
        Army mainArmy = null;
        const int MainArmyState_StartNew = 0;
        const int MainArmyState_BuySoldiers = 1;
        const int MainArmyState_CollectSupport = 2;
        const int MainArmyState_Defend = 3;
        const int MainArmyState_Attack = 4;
        int mainArmyState = MainArmyState_StartNew;
        PFaction mainArmyWar = PFaction.Empty;

        public AiConscript aiConscript = AiConscript.Default;
        public bool armyAi_enabled = true;
        protected int diplomacyPoints = 0;
        public int warCount = -1;

        public override void AssignFaction(Faction faction)
        {
            base.AssignFaction(faction);
            faction.factiontype = FactionType.DefaultAi;
            faction.availableForPlayer = true;
            faction.player = this;
           
            if (Ref.netSession.IsHost)
            {
                SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
                {
                    citySel.IsNetHosted = true;
                }
            }
        }

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);

           //w.Write(IsPlayerNeighbor);
            w.Write((byte)aggressionLevel);
            //w.Write(protectedPlayer);  
            var bools = new EightBit(IsPlayerNeighbor, protectedFromBotAttacks, personality_loner, protectedFromDelete, mayAttackPlayer);
            bools.write(w);

            w.Write(Bound.Byte(diplomacyPoints));

            profile.writeBot(w);
        }
        public override void readGameState(BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            base.readGameState(r, subversion, pointers);

            readAiPlayerGameState(r, subversion);
            if (subversion < 88)
            {
                var faction = pfaction.GetFaction();
                mayAttackPlayer = faction.factiontype != FactionType.DarkFollower && faction.factiontype != FactionType.UnitedKingdom;
            }
            if (subversion >= 72)
            {
                diplomacyPoints = r.ReadByte();
            }
            if (subversion >= 74)
            {
                profile.readBot(r);
                SetProfile(profile);
            }

            refreshPublicIndex();

           
        }

        void refreshPublicIndex()
        {
            var faction = pfaction.GetFaction();
            switch (faction.factiontype)
            {
                case FactionType.DarkFollower:
                    DssRef.settings.Faction_DarkFollower = faction.pfaction;
                    break;

                case FactionType.Barbarians:
                    DssRef.settings.Faction_Barbarian = faction.pfaction;
                    break;

                case FactionType.UnitedKingdom:
                    DssRef.settings.Faction_UnitedKingdom = faction.pfaction;
                    break;

                case FactionType.GreenWood:
                    DssRef.settings.Faction_GreenWood = faction.pfaction;
                    break;
                case FactionType.SouthHara:
                    DssRef.settings.Faction_SouthHara = faction.pfaction;
                    break;

                case FactionType.DyingMonger:
                    DssRef.settings.Faction_DyingMonger = faction.pfaction;
                    break;

                case FactionType.DyingHate:
                    DssRef.settings.Faction_DyingHate = faction.pfaction;
                    break;

                case FactionType.DyingDestru:
                    DssRef.settings.Faction_DyingDestru = faction.pfaction;
                    break;
            }
        }

        public AiPlayer(Faction faction, bool newGame)
            : base(faction, newGame)
        {
            SetProfile(new Profile.PlayerProfile(faction.factiontype, DssRef.world.metaData, faction.myIndex));
       
            switch (faction.factiontype)
            {
                case FactionType.Ellium:
                    defaultSetup();
                    techSetup();
                    name = DssRef.todoLang.FactionName_Ellium;
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.diplomaticSide = DiplomaticSide.Light;
                    break;
                case FactionType.GrakPushdug:
                    defaultSetup();
                    techSetup();
                    name = DssRef.todoLang.FactionName_GrakPushdug;
                    aggressionLevel = AggressionLevel2_RandomAttacks;
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    break;
                case FactionType.Draugost:
                    defaultSetup();
                    techSetup();
                    name = DssRef.todoLang.FactionName_Draugost;
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.diplomaticSide = DiplomaticSide.None;
                    break;

                case FactionType.AerimAngren:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_AerimAngren;
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.diplomaticSide = DiplomaticSide.Light;
                    break;

                case FactionType.DragonGem:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_DragonGem;
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    break;

                case FactionType.Hælfolc:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Hælfolc;
                    aggressionLevel = AggressionLevel2_RandomAttacks;
                    break;

                case FactionType.Tomten:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Tomten;
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    break;

                case FactionType.Etheleorthe:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Etheleorthe;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    break;
                case FactionType.BranthollowBarony:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BranthollowBarony;
                    break;

                case FactionType.DunwadeHold:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_DunwadeHold;
                    break;

                case FactionType.CaerwynMarches:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_CaerwynMarches;
                    break;

                case FactionType.StonevaleFreehold:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_StonevaleFreehold;
                    break;

                case FactionType.GlenmereLordship:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_GlenmereLordship;
                    break;

                case FactionType.ArveldonPrincipality:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ArveldonPrincipality;
                    break;

                case FactionType.WestmereReaches:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_WestmereReaches;
                    break;

                case FactionType.ThornwickWardens:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ThornwickWardens;
                    break;

                case FactionType.EvermereFief:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_EvermereFief;
                    break;

                case FactionType.BryndralHollow:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BryndralHollow;
                    break;
                case FactionType.Mendog:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Mendog;
                    break;
                case FactionType.Minde:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Minde;
                    break;
                case FactionType.FloKingdom:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_FloKingdom;
                    break;
                case FactionType.CarolusKeksenmark:
                    defaultSetup();
                    techSetup();

                    name = DssRef.lang.FactionName_CarolusKeksenmark;
                    break;

                case FactionType.SylvaranGlade:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_SylvaranGlade;
                    break;

                case FactionType.DrelmirePact:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_DrelmirePact;
                    break;

                case FactionType.KhazrunForgeclan:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_KhazrunForgeclan;
                    break;

                case FactionType.VeylanHorselords:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_VeylanHorselords;
                    break;

                case FactionType.ThalosCovenant:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ThalosCovenant;
                    break;

                case FactionType.NerathianTideguard:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_NerathianTideguard;
                    break;

                case FactionType.SkaruunExiles:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_SkaruunExiles;
                    break;

                case FactionType.DraktharDominion:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_DraktharDominion;
                    break;

                case FactionType.MalrekIronbound:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_MalrekIronbound;
                    break;

                case FactionType.Starshield:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Starshield;
                    break;
                case FactionType.Bluepeak:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Bluepeak;
                    break;
                case FactionType.Hoft:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Hoft;
                    break;
                case FactionType.RiverStallion:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_RiverStallion;
                    break;
                case FactionType.Sivo:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Sivo;
                    break;

                case FactionType.AelthrenConclave:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_AelthrenConclave;
                    break;
                case FactionType.VrakasundEnclave:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_VrakasundEnclave;
                    break;
                case FactionType.Tormürd:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Tormürd;
                    break;
                case FactionType.ElderysFyrd:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ElderysFyrd;
                    break;
                case FactionType.Hólmgar:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Hólmgar;
                    break;
                case FactionType.RûnothalOrder:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_RûnothalOrder;
                    break;

                case FactionType.GrimwardEotain:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_GrimwardEotain;
                    break;
                case FactionType.SkaeldraHaim:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_SkaeldraHaim;
                    break;
                case FactionType.MordwynnCompact:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_MordwynnCompact;
                    break;
                case FactionType.AethmireSovren:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_AethmireSovren;
                    break;

                 case FactionType.ThurlanKin:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ThurlanKin;
                    break;
                case FactionType.ValestennOrder:
                    defaultSetup();
                    name = DssRef.lang.FactionName_ValestennOrder;
                    break;
                case FactionType.Mournfold:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Mournfold;
                    break;
                case FactionType.OrentharTribes:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_OrentharTribes;
                    break;
                case FactionType.SkarnVael:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_SkarnVael;
                    break;
                case FactionType.Glimmerfell:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Glimmerfell;
                    break;
                case FactionType.BleakwaterFold:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BleakwaterFold;
                    break;
                case FactionType.Oathmaeren:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Oathmaeren;
                    break;
                case FactionType.Elderforge:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Elderforge;
                    break;
                case FactionType.MarhollowCartel:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_MarhollowCartel;
                    break;


                case FactionType.TharvaniDominion:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_TharvaniDominion;
                    break;
                case FactionType.KystraAscendancy:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_KystraAscendancy;
                    break;
                case FactionType.GildenmarkUnion:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_GildenmarkUnion;
                    break;
                case FactionType.AurecanEmpire:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_AurecanEmpire;
                    break;
                case FactionType.BronzeReach:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BronzeReach;
                    break;
                case FactionType.ElbrethGuild:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ElbrethGuild;
                    break;
                case FactionType.ValosianSenate:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ValosianSenate;
                    break;
                case FactionType.IronmarchCompact:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_IronmarchCompact;
                    break;
                case FactionType.KaranthCollective:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_KaranthCollective;
                    break;
                case FactionType.VerdicAlliance:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_VerdicAlliance;
                    break;

                case FactionType.OrokhCircles:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_OrokhCircles;
                    break;
                case FactionType.TannagHorde:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_TannagHorde;
                    break;
                case FactionType.BraghkRaiders:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BraghkRaiders;
                    break;
                case FactionType.ThurvanniStonekeepers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ThurvanniStonekeepers;
                    break;
                case FactionType.KolvrenHunters:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_KolvrenHunters;
                    break;
                case FactionType.JorathBloodbound:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_JorathBloodbound;
                    break;
                case FactionType.UlrethSkycallers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_UlrethSkycallers;
                    break;
                case FactionType.GharjaRavagers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_GharjaRavagers;
                    break;
                case FactionType.RavkanShield:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_RavkanShield;
                    break;
                case FactionType.FenskaarTidewalkers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_FenskaarTidewalkers;
                    break;


                case FactionType.HroldaniStormguard:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_HroldaniStormguard;
                    break;
                case FactionType.SkirnirWolfkin:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_SkirnirWolfkin;
                    break;
                case FactionType.ThalgarBearclaw:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ThalgarBearclaw;
                    break;
                case FactionType.VarnokRimeguard:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_VarnokRimeguard;
                    break;
                case FactionType.KorrakFirehand:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_KorrakFirehand;
                    break;
                case FactionType.MoongladeGat:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_MoongladeGat;
                    break;
                case FactionType.DraskarSons:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_DraskarSons;
                    break;
                case FactionType.YrdenFlamekeepers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_YrdenFlamekeepers;
                    break;
                case FactionType.BrundirWarhorns:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BrundirWarhorns;
                    break;
                case FactionType.OltunBonecarvers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_OltunBonecarvers;
                    break;

                case FactionType.HaskariEmber:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_HaskariEmber;
                    break;
                case FactionType.ZalfrikThunderborn:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_ZalfrikThunderborn;
                    break;
                case FactionType.BjorunStonetender:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BjorunStonetender;
                    break;
                case FactionType.MyrdarrIcewalkers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_MyrdarrIcewalkers;
                    break;
                case FactionType.SkelvikSpear:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_SkelvikSpear;
                    break;
                case FactionType.VaragThroatcallers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_VaragThroatcallers;
                    break;
                case FactionType.Durakai:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Durakai;
                    break;
                case FactionType.FjornfellWarhowl:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_FjornfellWarhowl;
                    break;
                case FactionType.AshgroveWard:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_AshgroveWard;
                    break;
                case FactionType.HragmarHorncarvers:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_HragmarHorncarvers;
                    break;


                case FactionType.Player:
                case FactionType.DefaultAi:
                    defaultSetup();
                    techSetup();
                    name = string.Format(DssRef.lang.FactionName_GenericAi, faction.pfaction.factionIndex);
                    break;

                case FactionType.DarkLord:
                    aiConscript = AiConscript.Orcs;
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.growthMultiplier = 1.5f;
                    name = DssRef.lang.FactionName_DarkLord;
                    faction.storyFaction = true;
                    protectedFromDelete = true;

                    techSetup();
                    faction.technology.advancedBuilding.points = TechnologyTemplate.FactionUnlock;
                    faction.technology.advancedCasting.points = TechnologyTemplate.FactionUnlock;
                    faction.technology.iron.points = TechnologyTemplate.FactionUnlock;
                    faction.technology.steel.points = TechnologyTemplate.FactionUnlock;
                    faction.technology.blackPowder.points = TechnologyTemplate.FactionUnlock;
                    faction.technology.gunPowder.points = TechnologyTemplate.FactionUnlock;

                    break;

                case FactionType.DarkFollower:
                    aiConscript = AiConscript.Orcs;
                    DssRef.settings.Faction_DarkFollower = faction.pfaction;
                    name = DssRef.lang.FactionName_DarkFollower;

                    if (!quickMatchSetup())
                    {
                        faction.diplomaticSide = DiplomaticSide.Dark;
                        aggressionLevel = AggressionLevel3_FocusedAttacks;
                        faction.growthMultiplier = 1.5f;
                        faction.storyFaction = true;
                        faction.addGold_factionWide(DssConst.HeadCityStartMaxWorkForce * 10);
                        techSetup();
                        faction.technology.blackPowder.points = TechnologyTemplate.FactionUnlock;
                    }

                    mayAttackPlayer = false;
                    break;

                case FactionType.Barbarians:
                    aiConscript = AiConscript.Orcs;
                    faction.diplomaticSide = DiplomaticSide.Dark;

                    DssRef.settings.Faction_Barbarian = faction.pfaction;
                    protectedFromDelete = true;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.growthMultiplier = 1.5f;
                    name = DssRef.lang.FactionName_Barbarian;
                    faction.storyFaction = false;
                    faction.hasDeserters = false;

                    techSetup();
                    break;

                case FactionType.UnitedKingdom:
                    name = DssRef.lang.FactionName_UnitedKingdom;

                    if (!quickMatchSetup())
                    {
                        faction.diplomaticSide = DiplomaticSide.Dark;
                        DssRef.settings.Faction_UnitedKingdom = faction.pfaction;
                        aggressionLevel = AggressionLevel1_RevengeOnly;
                        
                        faction.storyFaction = true;
                        personality_loner = true;

                        techSetup();
                        faction.technology.advancedBuilding.points = TechnologyTemplate.FactionUnlock;
                        faction.technology.steel.points = TechnologyTemplate.FactionUnlock;
                    }
                    mayAttackPlayer = false;
                    break;

                case FactionType.GreenWood:
                    aiConscript = AiConscript.Green;
                    DssRef.settings.Faction_GreenWood = faction.pfaction;
                    name = DssRef.lang.FactionName_Greenwood;

                    if (!quickMatchSetup())
                    {
                        faction.diplomaticSide = DiplomaticSide.Light;

                        personality_loner = true;
                        aggressionLevel = AggressionLevel1_RevengeOnly;
                        faction.growthMultiplier = 0.75f;
                        
                        profile.flag.factionFlavorType = FactionFlavorType.Forest;

                        techSetup();
                        faction.technology.steel.points = TechnologyTemplate.FactionUnlock;
                    }

                    mayAttackPlayer = false;
                    break;

                case FactionType.EasternEmpire:
                    name = DssRef.lang.FactionName_EasternEmpire;

                    if (!quickMatchSetup())
                    {
                        aggressionLevel = AggressionLevel1_RevengeOnly;

                        techSetup();
                        faction.technology.advancedBuilding.points = TechnologyTemplate.FactionUnlock;
                    }
                    break;

                case FactionType.NordicRealm:
                    aiConscript = AiConscript.Viking;
                    faction.grouptype = FactionGroupType.Nordic;
                    name = DssRef.lang.FactionName_NordicRealm;

                    if (!quickMatchSetup())
                    {   
                        faction.diplomaticSide = DiplomaticSide.Light;
                        aggressionLevel = AggressionLevel3_FocusedAttacks;
                     
                        techSetup();
                    }
                    break;

                case FactionType.BearClaw:
                    aiConscript = AiConscript.Viking;
                    faction.grouptype = FactionGroupType.Nordic;
                    name = DssRef.lang.FactionName_BearClaw;

                    if (!quickMatchSetup())
                    {
                        aggressionLevel = AggressionLevel3_FocusedAttacks;

                        //addStartCitiesBuyOption(UnitType.Viking);
                        techSetup();
                    }
                    break;

                case FactionType.NordicSpur:
                    aiConscript = AiConscript.Viking;
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    name = DssRef.lang.FactionName_NordicSpur;
                    //addStartCitiesBuyOption(UnitType.Viking);
                    techSetup();
                    break;

                case FactionType.IceRaven:
                    aiConscript = AiConscript.Viking;
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    name = DssRef.lang.FactionName_IceRaven;
                    //addStartCitiesBuyOption(UnitType.Viking);
                    techSetup();
                    break;

                case FactionType.DragonSlayer:
                    aiConscript = AiConscript.DragonSlayer;
                    faction.grouptype = FactionGroupType.Nordic;
                    aggressionLevel = Ref.rnd.Chance(0.4) ? AggressionLevel2_RandomAttacks : AggressionLevel1_RevengeOnly;
                    name = DssRef.lang.FactionName_Dragonslayer;
                    //addStartCitiesBuyOption(UnitType.CrossBow);
                    techSetup();
                    faction.technology.catapult.points = TechnologyTemplate.FactionUnlock;
                    break;

                case FactionType.SouthHara:
                    aiConscript = AiConscript.Orcs;
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_SouthHara = faction.pfaction;
                    protectedFromDelete = true;

                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.growthMultiplier = 1.1f;
                    faction.hasDeserters = false;
                    name = DssRef.lang.FactionName_SouthHara;
                    faction.storyFaction = true;
                    faction.addGold_factionWide(DssConst.HeadCityStartMaxWorkForce * 5);

                    techSetup();
                    faction.technology.catapult.points = TechnologyTemplate.FactionUnlock;
                    faction.technology.blackPowder.points = TechnologyTemplate.FactionUnlock;
                    break;

                case FactionType.DyingMonger:
                    name = DssRef.lang.FactionName_Monger;

                    if (!quickMatchSetup())
                    {
                        faction.diplomaticSide = DiplomaticSide.Dark;
                        DssRef.settings.Faction_DyingMonger = faction.pfaction;

                        aggressionLevel = AggressionLevel1_RevengeOnly;
                        faction.growthMultiplier = 4f;
                        faction.hasDeserters = false;
                        
                        faction.addGold_factionWide(DssConst.HeadCityStartMaxWorkForce * 1000);

                        techSetup();
                    }
                    break;

                case FactionType.DyingHate:
                    faction.diplomaticSide = DiplomaticSide.Dark;
                    DssRef.settings.Faction_DyingHate = faction.pfaction;

                    aggressionLevel = AggressionLevel1_RevengeOnly;
                    faction.growthMultiplier = 4f;
                    faction.hasDeserters = false;
                    name = DssRef.lang.FactionName_Hatu;
                    faction.addGold_factionWide(DssConst.HeadCityStartMaxWorkForce * 1000);
                    techSetup();
                    break;

                case FactionType.DyingDestru:
                    name = DssRef.lang.FactionName_Destru;

                    if (!quickMatchSetup())
                    {
                        faction.diplomaticSide = DiplomaticSide.Dark;
                        DssRef.settings.Faction_DyingDestru = faction.pfaction;

                        aggressionLevel = AggressionLevel1_RevengeOnly;
                        faction.growthMultiplier = 4f;
                        faction.hasDeserters = false;
                        
                        faction.addGold_factionWide(DssConst.HeadCityStartMaxWorkForce * 1000);
                        techSetup();
                    }
                    break;

                case FactionType.BramblebrookHill:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_BramblebrookHill;
                    aggressionLevel = AggressionLevel0_Passive;
                    faction.diplomaticSide = DiplomaticSide.Light;
                    mayAttackPlayer = false;
                    break;
                case FactionType.Tumblehill:
                    defaultSetup();
                    techSetup();
                    name = DssRef.lang.FactionName_Tumblehill;
                    aggressionLevel = AggressionLevel0_Passive;
                    faction.diplomaticSide = DiplomaticSide.Light;
                    mayAttackPlayer = false;
                    break;

                default:
                    throw new NotImplementedException("ai player " + faction.factiontype);
            }

            refreshPublicIndex();

            //apply tech on all cities

            bool quickMatchSetup()
            {
                if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
                {
                    defaultSetup();
                    techSetup();
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                    faction.addGold_factionWide(DssConst.HeadCityStartMaxWorkForce * 10);
                    return true;
                }

                return false;
            }

            void defaultSetup()
            {
                var chance = Ref.rnd.Double();
                if (profile.flag.factionFlavorType == FactionFlavorType.Other)
                {
                    chance *= 1.5f;
                }
                else
                {
                    chance *= 0.75f;
                }

                if (chance < 0.08)
                {
                    aggressionLevel = AggressionLevel3_FocusedAttacks;
                }
                else if (chance < 0.25)
                {
                    aggressionLevel = AggressionLevel2_RandomAttacks;
                }
                else if (chance < 0.4)
                {
                    aggressionLevel = AggressionLevel1_RevengeOnly;
                }
                else
                {
                    aggressionLevel = AggressionLevel0_Passive;
                }

                if (faction.mainCity != null)
                {
                    switch (profile.flag.factionFlavorType)
                    {

                        case FactionFlavorType.Forest:
                            faction.diplomaticSide = DiplomaticSide.Light;
                            aggressionLevel = AggressionLevel1_RevengeOnly;
                            personality_loner = true;
                            faction.growthMultiplier = 0.75f;
                            aiConscript = AiConscript.Green;
                            break;

                        case FactionFlavorType.Mystical:
                            faction.diplomaticSide = DiplomaticSide.Dark;
                            faction.growthMultiplier = 1.2f;
                            break;

                        case FactionFlavorType.Warrior:
                            aggressionLevel = Bound.Max(aggressionLevel + 1, AggressionLevel3_FocusedAttacks);
                            break;
                    }

                }
            }

            void techSetup()
            {
                // Initialize techs with appropriate unlocks
                faction.technology.advancedBuilding.points = TechnologyTemplate.SetRandom(
                    faction.technology.advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
                faction.technology.advancedFarming.points = TechnologyTemplate.SetRandom(
                    faction.technology.advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
                faction.technology.advancedCasting.points = TechnologyTemplate.SetRandom(
                    faction.technology.advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
                faction.technology.iron.points = TechnologyTemplate.FactionUnlock; // Stays the same
                faction.technology.steel.points = TechnologyTemplate.SetRandom(
                    faction.technology.steel.points, XpLib.Unlock.SteelUnlock);
                faction.technology.catapult.points = TechnologyTemplate.SetRandom(
                    faction.technology.catapult.points, XpLib.Unlock.CatapultUnlock);
                faction.technology.blackPowder.points = TechnologyTemplate.SetRandom(
                    faction.technology.blackPowder.points, XpLib.Unlock.BlackPowderUnlock);

                if (profile.flag.factionFlavorType == FactionFlavorType.City)
                {
                    if (Ref.rnd.Chance(0.8))
                    {
                        faction.technology.advancedBuilding.points = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.advancedBuilding.points, XpLib.Unlock.AdvancedBuildingUnlock);
                    }
                }

                if (profile.flag.factionFlavorType == FactionFlavorType.Mountain)
                {
                    if (Ref.rnd.Chance(0.8))
                    {
                        faction.technology.steel.points = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.steel.points, XpLib.Unlock.SteelUnlock);
                    }

                    if (Ref.rnd.Chance(0.6))
                    {
                        faction.technology.catapult.points = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.catapult.points, XpLib.Unlock.CatapultUnlock);
                    }
                }

                if (profile.flag.factionFlavorType == FactionFlavorType.People)
                {
                    faction.technology.iron.points = 0;
                    faction.technology.steel.points = 0;

                    if (Ref.rnd.Chance(0.6))
                    {
                        faction.technology.advancedFarming.points = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.advancedFarming.points, XpLib.Unlock.AdvancedFarmingUnlock);
                    }
                }

                if (faction.diplomaticSide == DiplomaticSide.Dark)
                {
                    if (Ref.rnd.Chance(0.6))
                    {
                        faction.technology.advancedCasting.points = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.advancedCasting.points, XpLib.Unlock.AdvancedCastingUnlock);
                    }

                    if (Ref.rnd.Chance(0.6))
                    {
                        faction.technology.blackPowder.points  = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.blackPowder.points, XpLib.Unlock.BlackPowderUnlock);
                    }

                    if (Ref.rnd.Chance(0.4))
                    {
                        faction.technology.steel.points = TechnologyTemplate.FactionUnlock;
                    }
                    else
                    {
                        TechnologyTemplate.MultiplyProgress(ref faction.technology.steel.points, XpLib.Unlock.SteelUnlock, 0.3);
                    }

                    faction.technology.advancedFarming.points = 0;
                }
            }

            //void techSetup()
            //{
            //    faction.technology.advancedBuilding = TechnologyTemplate.SetRandom(faction.technology.advancedBuilding, TechnologyTemplate.AdvancedBuildingUnlock);
            //    faction.technology.advancedFarming = TechnologyTemplate.SetRandom(faction.technology.advancedFarming);
            //    faction.technology.advancedCasting = TechnologyTemplate.SetRandom(faction.technology.advancedCasting);
            //    faction.technology.iron = TechnologyTemplate.FactionUnlock;
            //    faction.technology.steel = TechnologyTemplate.SetRandom(faction.technology.steel);
            //    faction.technology.catapult = TechnologyTemplate.SetRandom(faction.technology.catapult);
            //    faction.technology.blackPowder = TechnologyTemplate.SetRandom(faction.technology.blackPowder);

            //    if (faction.profile.factionFlavorType == FactionFlavorType.City)
            //    {
            //        if (Ref.rnd.Chance(0.8))
            //        {
            //            faction.technology.advancedBuilding = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.advancedBuilding); //= MathExt.MultiplyInt(faction.technology.steel, 2);
            //        }
            //    }

            //    if (faction.profile.factionFlavorType == FactionFlavorType.Mountain)
            //    {
            //        if (Ref.rnd.Chance(0.8))
            //        {
            //            faction.technology.steel = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.steel);
            //        }

            //        if (Ref.rnd.Chance(0.6))
            //        {
            //            faction.technology.catapult = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.catapult);
            //        }
            //    }

            //    if (faction.profile.factionFlavorType == FactionFlavorType.People)
            //    {
            //        faction.technology.iron = TechnologyTemplate.Start.iron;
            //        faction.technology.steel = TechnologyTemplate.Start.steel;

            //        if (Ref.rnd.Chance(0.6))
            //        {
            //            faction.technology.advancedFarming = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.advancedFarming);
            //        }
            //    }

            //    if (faction.diplomaticSide == DiplomaticSide.Dark)
            //    {
            //        if (Ref.rnd.Chance(0.6))
            //        {
            //            faction.technology.advancedCasting = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.advancedCasting);
            //        }

            //        if (Ref.rnd.Chance(0.6))
            //        {
            //            faction.technology.blackPowder = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.blackPowder);
            //        }

            //        if (Ref.rnd.Chance(0.4))
            //        {
            //            faction.technology.steel = TechnologyTemplate.FactionUnlock;
            //        }
            //        else
            //        {
            //            TechnologyTemplate.MultiplyProgress(ref faction.technology.steel, 0.3);
            //        }

            //        faction.technology.advancedFarming = TechnologyTemplate.Start.advancedFarming;
            //    }
            //}
        }

        //public void refreshAggression()
        //{
        //    int prioAdd = 0;
        //    if (aggressionLevel >= AggressionLevel2_RandomAttacks)
        //    {
        //        faction.workTemplate.craft_heavymailarmor.value = 5;
        //    }
        //    else if (aggressionLevel == AggressionLevel1_RevengeOnly)
        //    {
        //        prioAdd = -1;
        //    }
        //    else
        //    {
        //        prioAdd = -2;
        //    }

        //    faction.workTemplate.craft_mailarmor.value = 4 + prioAdd;
        //    faction.workTemplate.craft_paddedarmor.value = 3 + prioAdd;

        //    faction.workTemplate.craft_sword.value = 5 + prioAdd;
        //    faction.workTemplate.craft_bow.value = 4 + prioAdd;
        //    faction.workTemplate.craft_sharpstick.value = 3 + prioAdd;
        //}



        //void addStartCitiesBuyOption(UnitType unitType)
        //{
        //    var typeData = DssRef.profile.Get(unitType);
        //    var citiesC = faction.cities.counter();

        //    while (citiesC.Next())
        //    {
        //        citiesC.sel.cityPurchaseOptions.Add(new CityPurchaseOption()
        //        {
        //            unitType = unitType,
        //            goldCost = typeData.goldCost,
        //        });
        //    }
        //}
        
        public override void createStartUnits(double unitCountMulti, bool settlerGuard)
        {
            var faction = pfaction.GetFaction();
            if (faction.cities.Count > 0)
            {
                if (quickMatchUnits(faction.cities.Count > 1))
                {
                    return;
                }
                if (faction.mainCity == null)
                {
                    faction.refreshMainCity();
                    if (faction.mainCity == null)
                    {
                        return;
                    }
                }

                Army mainArmy = null;

                switch (faction.factiontype)
                {
                    default:
                        if (settlerGuard)
                        {
                            settlerGuardUnits();
                            return;
                        }

                        switch (profile.flag.factionFlavorType)
                        {
                            default:                                
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Swordsman, mainArmy.position);//, UnitType.Soldier, false);
                                }
                                break;

                            case FactionFlavorType.Mystical:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(2, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Pikeman, mainArmy.position);
                                }
                                for (int i = 0; i < MathExt.MultiplyInt(2, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_CrossbowMan, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.Sea:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Sailor, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.Mountain:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Dwarf, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.Horse:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Knight, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.Noble:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_FootKnight, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.City:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(2, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Swordsman, mainArmy.position);
                                }
                                for (int i = 0; i < MathExt.MultiplyInt(1, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_StandardBallista, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.Forest:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(2, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_GreenSoldier, mainArmy.position);
                                }
                                break;

                            case FactionFlavorType.People:
                                mainArmy = startMainArmy();
                                for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_Farmer, mainArmy.position);
                                }
                                for (int i = 0; i < MathExt.MultiplyInt(1, unitCountMulti); ++i)
                                {
                                    new SoldierGroup(mainArmy, DssLib.SoldierProfile_StandardArcher, mainArmy.position);
                                }
                                break;
                        }

                        break;
                    case FactionType.DarkFollower:
                        {
                            
                            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                            while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
                            {
                                int count = MathExt.MultiplyInt(citySel.cityType == CityType.Town ? 5 : 2, unitCountMulti);
                               
                                IntVector2 pos = citySel.ArmySpawnTilePos();
                                var army = faction.NewArmy(pos);
                                
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_Pikeman, army.position);
                                }
                                for (int i = 0; i < count; ++i)
                                {
                                    new SoldierGroup(army, DssLib.SoldierProfile_CrossbowMan, army.position);
                                }

                                army.setAsStartArmy();
                            }
                        }
                        break;

                    case FactionType.UnitedKingdom:
                        {
                            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                            while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
                            {
                                if (citySel.cityType == CityType.Town)
                                {
                                    IntVector2 pos = citySel.ArmySpawnTilePos();
                                    var army = faction.NewArmy(pos);

                                    for (int i = 0; i < MathExt.MultiplyInt(10, unitCountMulti); ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard, army.position);
                                    }

                                    army.setAsStartArmy();
                                }
                            }
                        }
                        break;

                    case FactionType.GreenWood:
                        if (settlerGuard)
                        {
                            settlerGuardUnits();
                            return;
                        }

                        mainArmy = startMainArmy();
                        for (int i = 0; i < MathExt.MultiplyInt(4, unitCountMulti); ++i)
                        {
                            new SoldierGroup(mainArmy, DssLib.SoldierProfile_GreenSoldier, mainArmy.position);//UnitType.GreenSoldier, false);
                        }
                        break;

                    case FactionType.NordicRealm:
                    case FactionType.BearClaw:
                    case FactionType.NordicSpur:
                    case FactionType.IceRaven:
                        if (settlerGuard)
                        {
                            settlerGuardUnits();
                            return;
                        }

                        mainArmy = startMainArmy();
                        for (int i = 0; i < MathExt.MultiplyInt(3, unitCountMulti); ++i)
                        {
                            new SoldierGroup(mainArmy, DssLib.SoldierProfile_Viking, mainArmy.position);
                        }
                        break;

                    case FactionType.DyingMonger:
                    case FactionType.DyingHate:
                    case FactionType.DyingDestru:
                        {
                            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                            while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City citySel))
                            {
                                if (citySel.cityType == CityType.Town)
                                {
                                    IntVector2 pos = citySel.ArmySpawnTilePos();
                                    var army = faction.NewArmy(pos);

                                    for (int i = 0; i < MathExt.MultiplyInt(10, unitCountMulti); ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_HonorGuard, army.position);
                                    }
                                    for (int i = 0; i < MathExt.MultiplyInt(20, unitCountMulti); ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_StandardArcher, army.position); 
                                    }
                                    for (int i = 0; i < MathExt.MultiplyInt(20, unitCountMulti); ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_StandardBallista, army.position);
                                    }
                                    for (int i = 0; i < MathExt.MultiplyInt(60, unitCountMulti); ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_Swordsman, army.position); 
                                    }
                                    for (int i = 0; i < MathExt.MultiplyInt(20, unitCountMulti); ++i)
                                    {
                                        new SoldierGroup(army, DssLib.SoldierProfile_Knight, army.position);
                                    }

                                    army.setAsStartArmy();
                                }
                            }
                        }
                        break;
                }

                if (mainArmy != null)
                {
                    mainArmy.setAsStartArmy();
                }
            }

            Army startMainArmy()
            {
                
                IntVector2 onTile = faction.mainCity.ArmySpawnTilePos();
                return faction.NewArmy(onTile);
            }
        }


        //private void createPurchaseOrder(City city, int maxPurchaseCount)
        //{
        //    purchaseCount = Ref.peRnd.Int(5, maxPurchaseCount);
        //    purchaseOrder = PurchaseOrderType_Army;
        //    purchaseOrderIndex1 = city.myIndex;

        //    if (city.GetPlayer() == this)
        //    {
                
        //        buySoldiers(city, false, true);
                
        //    }

        //}
        //void purchase()
        //{
        //    //const int PurchaseOrderType_MergeArmies = 4;

        //    //const int PurchaseOrderFocus_None = 0;
        //    //const int PurchaseOrderFocus_Defend = 1;
        //    //const int PurchaseOrderFocus_QuickDefend = 2;
        //    //const int PurchaseOrderFocus_AttackCity = 3;
        //    //const int PurchaseOrderFocus_SeaTravel = 4;

        //    //int purchaseOrder = PurchaseOrderType_None;
        //    //int purchaseOrderFocus = PurchaseOrderFocus_None;
        //    //int purchaseOrderIndex1 = -1;
        //    //int purchaseOrderIndex2 = -1;
        //    //bool purchaseIsMainArmy = false;

        //    if (purchaseOrder == PurchaseOrderType_MergeArmies)
        //    {
        //        var armiesCounter = faction.armies.counter();
        //        int found = 0;
        //        Army army1 = null;
        //        Army army2 = null;

        //        while (armiesCounter.Next() && found < 2)
        //        {
        //            if (armiesCounter.sel.myIndex == purchaseOrderIndex1)
        //            {
        //                army1 = armiesCounter.sel;
        //                ++found;
        //            }
        //            else if (armiesCounter.sel.myIndex == purchaseOrderIndex2)
        //            {
        //                army2 = armiesCounter.sel;
        //                ++found;
        //            }
        //        }

        //        if (army1 != null && army2 != null)
        //        {
        //            army1.mergeArmies(army2);
        //        }
        //    }
        //    else
        //    {
        //        var city = DssRef.world.cities[purchaseOrderIndex1];
        //        if (city.GetPlayer() == this)
        //        {
        //            switch (purchaseOrder)
        //            {
        //                case PurchaseOrderType_Army:
        //                    buySoldiers(city, false, true);
        //                    break;
        //            }

        //        }
        //    }
        //    purchaseOrder = PurchaseOrderType_None;
        //}

        //public override void Update()
        //{
        //    base.Update();

        //    if (IsLocal)
        //    {
        //        if (faction.factiontype == FactionType.SouthHara)
        //        {
        //            lib.DoNothing();
        //        }
        //        if (purchaseOrder !=  PurchaseOrderType_None)
        //        {
                    

        //            if (purchaseOrder == PurchaseOrderType_MergeArmies)
        //            {
        //                var armiesCounter = faction.armies.counter();
        //                int found = 0;
        //                Army army1 = null;
        //                Army army2 = null;

        //                while (armiesCounter.Next() && found < 2)
        //                {
        //                    if (armiesCounter.sel.myIndex == purchaseOrderIndex1)
        //                    {
        //                        army1 = armiesCounter.sel;
        //                        ++found;
        //                    }
        //                    else if (armiesCounter.sel.myIndex == purchaseOrderIndex2)
        //                    {
        //                        army2 = armiesCounter.sel;
        //                        ++found;
        //                    }
        //                }

        //                if (army1!= null && army2 != null)
        //                {
        //                    army1.mergeArmies(army2);
        //                }
        //            }
        //            else
        //            {
        //                var city = DssRef.world.cities[purchaseOrderIndex1];
        //                if (city.GetPlayer() == this)
        //                {
        //                    switch (purchaseOrder)
        //                    {
        //                        case PurchaseOrderType_Army:
        //                            buySoldiers(city, false, true);
        //                            break;
        //                    }

        //                }
        //            }
        //            purchaseOrder = PurchaseOrderType_None;
        //        }
        //    }
        //}

        public override void onGameStart(bool newGame)
        {
            base.onGameStart(newGame);
            
            switch (profile.flag.factionFlavorType)
            {
                case FactionFlavorType.Mountain:
                    var faction = pfaction.GetFaction();
                    if (faction.mainCity != null)
                    {
                        faction.mainCity.AddGroupedResource(EntityComponent.CityResourceIndex.iron, 100, false);
                        faction.mainCity.AddGroupedResource(EntityComponent.CityResourceIndex.shortsword, 60, false);
                        faction.mainCity.AddGroupedResource(EntityComponent.CityResourceIndex.heavyMailArmor, 60, false);
                    }
                    break;
            }
        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();
            ignorePlayerCapture = false;
            if (Ref.peRnd.ChanceF(0.1f))
            {
                diplomacyPoints++;
            }
        }
        


        override public void aiPlayerAsynchUpdate(float time)
        {
            //if (faction.factiontype == FactionType.Barbarians)
            //{
            //    lib.DoNothing();
            //}

            if (StartupSettings.RunAI && nextDecisionTimer.CountDownGameTime(time))
            {
                if (!pfaction.TryGetFaction(out var faction))
                {
                    return;
                }
                
                if (updateDecitionTimer(faction))
                {
                    return;
                }

                bool protect = Ref.peRnd.Chance(0.6);

                List<PFaction> wars = DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectWars(faction);
                warCount = wars.Count;
                bool inWar = aggressionLevel >= AggressionLevel2_RandomAttacks ||
                    (aggressionLevel == AggressionLevel1_RevengeOnly && wars.Count > 0);

                refreshWorkPriority_async(inWar);

                if (inWar && Ref.rnd.Chance(aggressionLevel == AggressionLevel2_RandomAttacks ? 0.05 : 0.3) &&
                    !mainArmyLockedInTravel())
                {
                    mainArmy_AsyncUpdate(wars);
                }
                else if (protect && faction.cities.Count > 0)
                {
                    City city = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);

                    if (city != null && buySoldiersBalanceCheck_asynch(city, inWar, 0.02, out bool guardOnly))
                    {
                        Ref.update.AddSyncAction(new SyncAction(() =>
                        {
                            buySoldiers(city, inWar, guardOnly, true);
                        }));
                    }
                }
                else if (inWar)
                {
                    searchAttackTarget(wars);
                }

                MergeArmiesCheck();

                decisionTimerSizeCheck();

                diplomacyCheck(wars);

                armiesWithSettlerUpdate();

                settlerCheck();

                BlackMarketResources.AiPurchaseUpdate(faction.cities.GetRandom(Ref.rnd, DssRef.world.cities), faction);
            }
        }

        // <returns>Empty faction</returns>
        private bool updateDecitionTimer(Faction faction)
        {
            float timeMulti = 1;
            if (faction.cities.Count == 0)
            {
                mainArmy = null;
                if (faction.armies.Count == 0)
                {
                    timeMulti = 20;
                    return true;
                }
                else
                {
                    timeMulti = 0.75f;
                }
            }
            else if (faction.cities.Count <= 2)
            {
                timeMulti = 4;
            }
            else if (faction.cities.Count <= 6)
            {
                timeMulti = 2;
            }
            nextDecisionTimer.MilliSeconds = Ref.peRnd.Float(5000, 10000) * timeMulti;
            return false;
        }

        void settlerCheck()
        {
            var faction = pfaction.GetFaction();
            if (faction.cities.Count > 0)
            {
                City city = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
                                
                if (city != null &&
                    city.cityType > CityType.Campsite &&
                    city.homesUnused() < 20)
                {
                    if (city.SettlerBp().canCraftCount(city) >= 1)
                    {
                        EcsStaticArrayCounter neighbors = city.CityNeighbors();
                        while (neighbors.Next(DssRef.world.cities, out City nCity))
                        {
                            if (nCity.cityType == CityType.UnClaimed && Ref.peRnd.ChanceF(0.5f))
                            {
                                Ref.update.AddSyncAction(new SyncAction1Arg<City>(city.aiConscriptSettler, nCity));
                                //city.conscriptSettler(nCity, true);
                                return;
                            }
                        }
                    }
                }
            }
        }

        void armiesWithSettlerUpdate()
        {
            var armiesC = pfaction.GetFaction().armies.counter();
            while (armiesC.Next())
            {
                if (DssRef.world.tileGrid.TryGet(armiesC.sel.tilePos, out var tile))
                {
                    var city = tile.City();
                    if (city.cityType == CityType.UnClaimed &&
                        city.tilePos.SideLength(armiesC.sel.tilePos) <= 2 &&
                        armiesC.sel.HasSettler(out var settler))
                    {
                        SettlerCommandTarget.OrderSettler(settler, city.cityHallSubtilePos);
                    }
                }
            }
        }

        void diplomacyCheck(List<PFaction> wars)
        {
            if (diplomacyPoints >= 200)
            {
                diplomacyPoints = 0;
                var faction = pfaction.GetFaction();
                if (wars.Count > 0 && Ref.peRnd.Chance(0.4 - aggressionLevel * 0.1 + wars.Count * 0.05))
                {
                    //Declare peace
                    PFaction opponent = arraylib.RandomListMember(wars);
                    //Faction enemyFaction = DssRef.world.faction(opponent);
                    botToBotPeaceDeclaration(wars, opponent.GetFaction());
                }
                else if (Ref.peRnd.Chance(0.2) && !faction.quickMatchFaction)
                {
                    if (wars.Count > 0 && Ref.peRnd.Chance(0.8))
                    {
                        PFaction rndEnemy = arraylib.RandomListMember(wars);

                        Faction enemyFaction = rndEnemy.GetFaction();

                        if (enemyFaction != null &&
                            enemyFaction.MyPlusAllianceStrengthValue() * 1.5f > faction.MyPlusAllianceStrengthValue()) //Check threat level
                        {
                            findAlliances(enemyFaction, true);
                        }
                    }
                    else if (aggressionLevel >= AggressionLevel2_RandomAttacks &&
                        !personality_loner)
                    {
                        List<PFaction> threats = DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectThreats(faction, Ref.rnd.Float(2f, 6f));
                        if (threats.Count > 0)
                        {
                            PFaction enemyFaction = arraylib.RandomListMember(threats);
                            if (!DssRef.world.diplomacy.GetRelation(pfaction, enemyFaction).InWar())
                            {
                                findAlliances(enemyFaction.GetFaction(), false);
                            }
                        }
                    }
                }
            }


            void findAlliances(Faction enemyFaction, bool reasonWar)
            {
                if (enemyFaction == null)
                {
                    return;
                }

                DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectAlliances.Clear();

                var factions = DssRef.world.factions.counter();
                var faction = pfaction.GetFaction();

                while (factions.Next())
                {
                    if (factions.sel != faction &&
                        factions.sel.player.IsBot() &&
                        !factions.sel.quickMatchFaction)
                    {
                        var relation = DssRef.world.diplomacy.GetRelation(this.pfaction, factions.sel.pfaction);

                        if (relation.Relation >= RelationType.RelationType0_Neutral &&
                            relation.Relation < RelationType.RelationType3_Ally &&
                            faction.SameOrNeutralSide(factions.sel.diplomaticSide) &&
                            shareWarOrThreat(factions.sel.pfaction, enemyFaction.pfaction, reasonWar))
                        {
                            DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectAlliances.Add(factions.sel.pfaction);
                        }
                    }
                }

                if (DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectAlliances.Count > 0)
                {
                    PFaction newAlly = arraylib.RandomListMember(DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectAlliances);
                    //var allyFaction = DssRef.world.faction(newAlly);
                    botToBotAllyDeclaration(enemyFaction, newAlly.GetFaction(), false);
                }
            }

            bool shareWarOrThreat(PFaction maybeFriendPFaction, PFaction enemyFactionIx, bool reasonWar)
            {
                var relation = DssRef.world.diplomacy.GetRelation(maybeFriendPFaction, enemyFactionIx);
                if (relation.Relation <= RelationType.RelationTypeN1_Enemies)
                {
                    return true;
                }

                if ((!reasonWar || Ref.rnd.Chance(0.005)) && maybeFriendPFaction.TryGetFaction(out var maybeFriendFaction))
                {
                     
                    var maybeFriendBot = maybeFriendFaction.player.GetAiPlayer();
                    if (maybeFriendBot.aggressionLevel >= AggressionLevel2_RandomAttacks &&
                        !maybeFriendBot.personality_loner &&
                        DssRef.world.diplomacy.aiPlayerAsynchUpdate_collectThreats(maybeFriendFaction).Contains(enemyFactionIx))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void tryEndBotWars(List<PFaction> wars)
        {
            foreach (var war in wars)
            {
                //Faction enemyFaction = DssRef.world.faction(war);
                if (war.TryGetFaction(out var enemyFaction) && enemyFaction.player.IsBot())
                {
                    var rel = DssRef.world.diplomacy.GetRelation(pfaction, war);
                    if (rel.Relation <= RelationType.RelationTypeN2_Truce && rel.Relation > RelationType.RelationTypeN5_TotalWar)
                    {
                        botToBotPeaceDeclaration(null, enemyFaction);
                    }
                }
            }
        }

        public void botToBotPeaceDeclaration(List<PFaction> wars, Faction enemyFaction)
        {
            var faction = pfaction.GetFaction();
            if (enemyFaction != null)
            {
                if (enemyFaction.player.IsBot() &&
                    !DssRef.world.diplomacy.OppositeDiplomaticSides(faction, enemyFaction) &&
                    (wars == null || wars.Count > 1 || faction.militaryStrength < enemyFaction.militaryStrength) &&
                    !DssRef.world.diplomacy.InplayerAlliance(faction.pfaction))
                {
                    faction.shareRelationWithAllAllies(enemyFaction.pfaction, RelationType.RelationType1_Peace);
                }
            }
        }

        public bool mayAlly_checkWars(AiPlayer allyplayer)
        {
            int totwarcount = allyplayer.GetAiPlayer().warCount + warCount;
            if (totwarcount >= 12)
            {
                return Ref.rnd.Chance(0.005);
            }
            if (totwarcount >= 6)
            {
                return Ref.rnd.Chance(0.05);
            }
            return true;
        }

        public void botToBotAllyDeclaration(Faction enemyFaction, Faction allyFaction, bool tryEndOtherWars)
        {
            if (allyFaction != null && 
                mayAlly_checkWars( allyFaction.player.GetAiPlayer()) &&
                DssRef.world.diplomacy.aiPlayerAsynchUpdate_mayAlly_checkConflict(pfaction, allyFaction.pfaction, enemyFaction.pfaction, tryEndOtherWars))
            {
                const int TinyFaction = 2;
                const int LargeFaction = 8;

                var faction = pfaction.GetFaction();

                if (allyFaction.cities.Count <= TinyFaction && faction.cities.Count >= LargeFaction)
                {
                    Ref.update.AddSyncAction(new SyncAction1Arg<Faction>(allyFaction.mergeTo, faction));
#if DEBUG
                    //Ref.update.AddSyncAction(new SyncAction(() =>
                    //{
                    //    DssRef.state.LocalHost().hud.messages.Add("Bot merge", $"between {allyFaction.PlayerName} and {faction.PlayerName}, reason ({(reasonWar ? "share war" : "threat")})");
                    //}));
#endif
                }
                else
                {
                    ref var alliance = ref DssRef.world.diplomacy.GetRefRelation_Safe(pfaction, allyFaction.pfaction);
                    alliance.SetRelation(PFaction.Empty, PFaction.Empty, RelationType.RelationType3_Ally, PFaction.Empty, out _, false);
                    
                    alliance.allyAgainst = enemyFaction.myIndex;
                    allyFaction.player.GetAiPlayer().diplomacyPoints = 0;
#if DEBUG
                    //Ref.update.AddSyncAction(new SyncAction(() =>
                    //{
                    //    DssRef.state.LocalHost().hud.messages.Add("Bot alliance", $"between {allyFaction.PlayerName} and {faction.PlayerName}, reason ({(reasonWar ? "share war" : "threat")})");
                    //}));
#endif
                }
            }
        }

        void decisionTimerSizeCheck()
        {
            float multiply = 1;

            switch (pfaction.GetFaction().Size())
            {
                case FactionSize.Tiny:
                    multiply = 1.6f;
                    break;
                case FactionSize.Big:
                    multiply = 0.6f;
                    break;
                case FactionSize.Giant:
                    multiply = 0.2f;
                    break;

            }

            nextDecisionTimer.MilliSeconds*= multiply;
        }


        //void async_buildUpCheck()
        //{
        //    if (purchaseOrder == PurchaseOrderType_None &&
        //        faction.gold > DssLib.GroupDefaultCost * 10)
        //    { 
        //        var city = faction.cities.GetRandomUnsafe(Ref.rnd);

        //        if (city != null /*&& !city.InBattle()*/)
        //        {
        //            int friendCount = 0;
        //            int enemyCount = 0;

        //            foreach (var n in city.neighborCities)
        //            {
        //                if (DssRef.world.cities[n].faction == faction)
        //                {
        //                    friendCount++;
        //                }
        //                else
        //                { 
        //                    enemyCount++;
        //                }
        //            }

        //            //if (friendCount > enemyCount)
        //            //{
        //            //    //purchaseOrder = PurchaseOrderType_CityWorkers;
        //            //}
        //            //else
        //            //{
        //            //    purchaseOrder = PurchaseOrderType_CityGuard;
        //            //}

        //            purchaseOrderIndex1 = city.parentArrayIndex;
        //        }
        //    }
        //}

        

        void mainArmy_AsyncUpdate(List<PFaction> wars)
        {
            if (armyAi_enabled)
            {
                if (emptyMainArmy())
                {
                    var faction = pfaction.GetFaction();
                    mainArmyState = MainArmyState_StartNew;
                    if (faction.armies.Count > 0)
                    {
                        //Try find large army
                        const int Trials = 3;
                        for (int i = 0; i < Trials; i++)
                        {
                            var army = faction.armies.GetRandomUnsafe(Ref.rnd);
                            if (army != null && army.IdleObjetive() && army.groups.Count >= 5)
                            {
                                mainArmy = army;
                                mainArmyState = MainArmyState_CollectSupport;
                                break;
                            }
                        }
                    }
                }


                if (mainArmyState == MainArmyState_StartNew)
                {
                    //bool haveIncome = faction.NetIncome() >= 0 &&
                    //    faction.gold >= DssLib.GroupDefaultCost * 5;
                    City city = null;
                    city = cityCloseToCityInDanger(cityInDanger());

                    //if (city != null)
                    //{
                    //    purchaseOrderFocus = PurchaseOrderFocus_Defend;
                    //}
                    //else

                    if (city == null)
                    {
                        PFaction war = findMainWar(wars);

                        if (war.HasValue())
                        {
                            //find close city
                            city = cityCloseToOpponent(war);
                        }
                        else
                        {
                            city = cityCloseToNewTarget();
                        }
                        //purchaseOrderFocus = PurchaseOrderFocus_AttackCity;
                    }

                    //if (haveIncomeForArmyPurchase(true))
                    if (city != null && buySoldiersBalanceCheck_asynch(city, true, 0.05, out bool guardOnly) && !guardOnly)
                    {
                        //Start fresh
                        mainArmy = null;

                        nextDecisionTimer.MilliSeconds += Ref.peRnd.Int(4000, 15000);
                        //mainArmyBuyAtCity(city);
                        
                    }
                    else
                    {
                        Army army = StrongIdleArmy();

                        if (army != null)
                        {
                            mainArmy = army;
                            mainArmyState = MainArmyState_BuySoldiers;
                        }
                    }
                }
                else if (mainArmyState == MainArmyState_BuySoldiers)
                {
                    AbsMapObject city = null;

                    //Begin with defence check
                    city = cityInDanger();

                    if (city != null)
                    {
                        //Purchase some support for the city
                        //buyDefenceAtCity((City)city);
                        mainArmyBuyAtCity((City)city, true);

                        float l = city.distanceTo(mainArmy);
                        float percDist = 1f - l / 64;
                        double chance = 0.2 + percDist;

                        if (Ref.rnd.Chance(chance))
                        {
                            mainArmyState = MainArmyState_Defend;

                            nextDecisionTimer.MilliSeconds += 4000;
                            if (city.distanceTo(mainArmy) > 2)
                            {
                                mainArmy.Ai_Order_MoveTo(city.tilePos);
                            }
                        }
                        else
                        {
                            city = null;
                        }
                    }

                    if (city == null)
                    {
                        PFaction war = findMainWar(wars);

                        if (war.HasValue())
                        {
                            var opponent = war.GetFaction();
                            city = AttackFactionAtWar(mainArmy, opponent);
                        }
                        else
                        {
                            //Start new war
                            city = AttackRandom(wars.Count, mainArmy);
                            if (city != null)
                            {
                                mainArmyWar = city.pfaction;
                            }
                        }

                        mainArmyState = MainArmyState_Attack;
                    }

                    if (city != null)
                    {
                        collectLooseArmies(city.tilePos);
                    }
                    else
                    {
                        mainArmyState = MainArmyState_CollectSupport;
                    }
                }
                else if (mainArmyState == MainArmyState_Attack ||
                    mainArmyState == MainArmyState_Defend)
                {
                    if (mainArmy.IdleObjetive())
                    {
                        mainArmyState = MainArmyState_CollectSupport;
                    }
                }
                else if (mainArmyState == MainArmyState_CollectSupport)
                {
                    if (Ref.rnd.Chance(0.2))
                    {
                        mainArmy = null;
                    }
                    else
                    {
                        if (DssRef.world.tileGrid.TryGet(mainArmy.tilePos, out Tile tile))
                        {
                            var city = tile.City();
                            if (city.pfaction == pfaction)
                            {
                                if (city.distanceTo(mainArmy) <= 2)
                                {
                                    collectLooseArmies(city.tilePos);
                                    //mainArmyBuyAtCity(city);
                                    mainArmyState = MainArmyState_BuySoldiers;
                                    mainArmyBuyAtCity(city, false);
                                    //collectLooseArmies(city.tilePos);
                                }
                                else
                                {
                                    mainArmy.Ai_Order_MoveTo(city.tilePos);
                                }
                            }
                        }

                    }
                }
            }
        }

        private PFaction findMainWar(List<PFaction> wars)
        {
            PFaction war = PFaction.Empty;
            if (wars.Count > 0)
            {
                if (wars.Contains(mainArmyWar))
                {
                    war = mainArmyWar;
                }
                else
                {
                    war = arraylib.RandomListMember(wars);
                }
            }

            return war;
        }

        bool emptyMainArmy()
        {
            if (mainArmy == null ||
                 mainArmy.isDeleted ||
                 mainArmy.groups.Count < 4)
            {
                mainArmy = null;
                return true;
            }
            return false;
        }

        bool mainArmyLockedInTravel()
        {
            if (emptyMainArmy())
            {
                return false;
            }
            if (mainArmyState == MainArmyState_Attack || mainArmyState == MainArmyState_Defend)
            {
                return !mainArmy.IdleObjetive();
            }
            return false;
        }

        void mainArmyBuyAtCity(City city, bool defensive)
        {
            if (buySoldiersBalanceCheck_asynch(city, !defensive, 0.02, out bool guardsOnly))
            {
                if (!guardsOnly || defensive)
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        buySoldiers(city, !defensive, guardsOnly, true);
                    }));
                }
            }
        }

        //private void mainArmyBuyAtCity(City city)
        //{
           
        //            mainArmyState = MainArmyState_BuySoldiers;

        //            purchaseIsMainArmy = true;
        //            purchaseOrder = PurchaseOrderType_Army;
        //            purchaseOrderIndex1 = city.myIndex;

        //            collectLooseArmies(city.tilePos);
           
        //}

        //void buyDefenceAtCity(City city)
        //{   
            
        //        if (buySoldiersBalanceCheck_asynch(city, false, 0.02))
        //        {
        //            purchaseOrder = PurchaseOrderType_Army;
        //            purchaseOrderFocus = PurchaseOrderFocus_QuickDefend;
        //            purchaseOrderIndex1 = city.myIndex;
        //        }

        //    //}
        //}

        void collectLooseArmies(IntVector2 toPos)
        {
            DssRef.world.unitCollAreaGrid.collectArmies(pfaction, toPos, 2, DssRef.world.unitCollAreaGrid.armies_aiUpdate);

            foreach (var a in DssRef.world.unitCollAreaGrid.armies_aiUpdate)
            {
                var army = a as Army;
                double chance = army.objective == ArmyObjective.None ? 0.8 : 0.1;
                if (a != mainArmy && Ref.rnd.Chance(chance))
                {
                    army.Ai_Order_MoveTo(toPos);
                }
            }
        }

        City cityCloseToCityInDanger(City inDanger)
        {
            if (inDanger == null)
            {
                return null;
            }

            City city = null;

            EcsStaticArrayCounter neighbors = inDanger.CityNeighbors();
            while (neighbors.Next(DssRef.world.cities, out City nCity))//foreach (int m in inDanger.neighborCities)
            {
                //City c = DssRef.world.cities[m];
                if (nCity.pfaction == pfaction)
                {
                    if (city == null)
                    {
                        city = nCity;
                    }
                    else if (nCity.workForce.amount > city.workForce.amount)
                    {
                        city = nCity;
                    }
                }
            }

            return city;
        }

        City cityInDanger()
        { 
            City checkCity1 = null;
            City checkCity2 = null;
            var faction = pfaction.GetFaction();

            checkCity1 = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
            if (checkCity1 == null)
            {
                return null;
            }

            if (check(checkCity1))
            {
                return checkCity1;
            }

            checkCity2 = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
            if (checkCity2 == null)
            {
                return null;
            }

            if (checkCity2 != checkCity1 && check(checkCity2))
            {
                return checkCity2;
            }

            return null;

            bool check(City city)
            {
                DssRef.world.unitCollAreaGrid.collectOpponentArmies(pfaction, city.tilePos, 1, DssRef.world.unitCollAreaGrid.armies_aiUpdate);


                foreach (var army in DssRef.world.unitCollAreaGrid.armies_aiUpdate)
                {
                    if (army.IsArmy())
                    {
                        float dist = city.distanceTo(army);

                        if (dist <= 4)
                        {
                            return true;
                        }

                        if (DssRef.world.diplomacy.GetRelation(pfaction, army.pfaction).InWar())
                        {
                            if (dist <= 8)
                            {
                                return true;
                            }

                            var armyarmy = army as Army;
                            if (armyarmy.attackTarget == city ||
                                city.distanceTo(armyarmy.walkGoal) <= 4)
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        public bool IsWarBorderCity(City city, bool inWarOnly)
        {
            EcsStaticArrayCounter neighbors = city.CityNeighbors();
            while (neighbors.Next(DssRef.world.cities, out City nCity))//foreach (var nIx in city.neighborCities)
            {
                //var nCity = DssRef.world.cities[nIx];
                if (nCity.pfaction != pfaction
                    && nCity.pfaction.TryGetFaction(out _))
                {
                    //if (nCity.pfaction != faction.myIndex)
                    //{
                        var relation = DssRef.world.diplomacy.GetRelation(nCity.pfaction, pfaction);
                    if (relation.Relation <= RelationType.RelationTypeN1_Enemies)
                    {
                        return true;
                    }
                    else
                    {

                        if (!inWarOnly &&
                            relation.Relation <= RelationType.RelationType0_Neutral &&
                            nCity.pfaction.TryGetFaction(out var nf) && 
                            pfaction.TryGetFaction(out var f) &&
                            nf.militaryStrength > f.militaryStrength * 2)
                        {
                            return true;
                        }
                    }
                    //}
                }
            }

            return false;
        }

        City cityCloseToOpponent(PFaction opponent)
        {
            Faction otherFaction = opponent.GetFaction();
            var faction = pfaction.GetFaction();
            if (otherFaction == null)
            { 
                return null;
            }

            City myClosestCity = null;
            float closestDistance = float.MaxValue;
           
            const int TrialCount = 3;
            for (int i = 0; i < TrialCount; ++i)
            {
                City myCity = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
                if (myCity == null)
                {
                    return null;
                }

                for (int j = 0; j < TrialCount; ++j)
                {
                    City otherCity = otherFaction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
                    if (otherCity == null)
                    {
                        return null;
                    }

                    float l = (otherCity.tilePos - myCity.tilePos).Length();

                    if (l < closestDistance)
                    { 
                        closestDistance = l;
                        myClosestCity = myCity;
                    }
                }
                
            }

            return myClosestCity;
        }

        City cityCloseToNewTarget()
        {
            City city=null;
            Faction weakestOpponent = null;
            var faction = pfaction.GetFaction();

            const int TrialCount = 3;
            for (int i = 0; i < TrialCount; ++i)
            {
                City myCity = faction.cities.GetRandom(Ref.rnd, DssRef.world.cities);
                if (myCity == null)
                {
                    return null;
                }

                EcsStaticArrayCounter neighbors = myCity.CityNeighbors();
                while (neighbors.Next(DssRef.world.cities, out City nCity))
                {
                    var cityFaction = nCity.pfaction.GetFaction();
                    if (cityFaction != null && cityFaction != faction && cityFaction != weakestOpponent)
                    {
                        if (DssRef.difficulty.aiAggressivity >= AiAggressivity.Medium &&
                            cityFaction.player.IsLocalPlayer())
                        {
                            return myCity;
                        }

                        if (weakestOpponent == null)
                        {
                            city = myCity;
                            weakestOpponent = cityFaction;
                        }
                        else if (cityFaction.militaryStrength < weakestOpponent.militaryStrength)
                        {
                            city = myCity;
                            weakestOpponent = cityFaction;
                        }
                    }
                }
            }

            return city;
        }

        private void MergeArmiesCheck()
        {
            var faction = pfaction.GetFaction();
            var armyC = faction.armies.counter();
            while (armyC.Next())
            {
                //if (armyC.sel.ai.objective == ArmyObjective.None)
                //{
                var otherArmy = DssRef.world.unitCollAreaGrid.AdjacenToArmy(pfaction, armyC.sel.pointer(), armyC.sel.tilePos, Army.MaxTradeDistance +1);
                if (otherArmy != null)
                {
                    Army army1, army2;
                    //purchaseOrder = PurchaseOrderType_MergeArmies;

                    if (armyC.sel.groups.Count > otherArmy.groups.Count)
                    {
                        army2 = armyC.sel;
                        army1 = otherArmy;
                    }
                    else
                    {
                        army1 = armyC.sel;
                        army2 = otherArmy;
                    }


                    //        var armiesCounter = faction.armies.counter();
                    //        int found = 0;
                    //        Army army1 = null;
                    //        Army army2 = null;

                    //        while (armiesCounter.Next() && found < 2)
                    //        {
                    //            if (armiesCounter.sel.myIndex == purchaseOrderIndex1)
                    //            {
                    //                army1 = armiesCounter.sel;
                    //                ++found;
                    //            }
                    //            else if (armiesCounter.sel.myIndex == purchaseOrderIndex2)
                    //            {
                    //                army2 = armiesCounter.sel;
                    //                ++found;
                    //            }
                    //        }

                    //        if (army1 != null && army2 != null)
                    //        {
                    //            army1.mergeArmies(army2);
                    //        }

                    //Army army1 = armyC.sel;
                    //Army army2 = otherArmy;

                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        if (army1 != null && army2 != null)
                        {
                            army1.mergeArmies(army2);
                        }
                    }));

                    break;
                }
                //}
            }
        }

        void searchAttackTarget(List<PFaction> wars)
        {
            var faction = pfaction.GetFaction();
            if (armyAi_enabled && faction.armies.Count > 0)
            {
                Army army = StrongIdleArmy();

                if (faction.cities.Count == 0 && Ref.rnd.Chance(0.5))
                {
                    AttackRandom(wars.Count, army);
                }
                else if (army != null &&
                    (army != mainArmy || Ref.rnd.Chance(0.25)))
                {
                    if (
                        wars.Count == 0 ||
                        (aggressionLevel == AggressionLevel2_RandomAttacks && Ref.rnd.Chance(0.5))
                        )
                    {
                        //Start new war
                        AttackRandom(wars.Count, army);
                    }
                    else
                    {
                        var opponent = arraylib.RandomListMember(wars).GetFaction();
                        if (opponent != null)
                        {
                            AttackFactionAtWar(army, opponent);
                        }
                    }
                }
            }
        }

        private Army StrongIdleArmy()
        {
            var faction = pfaction.GetFaction();
            if (faction.armies.Count == 0)
            {
                return null;
            }
            
            Army army = null;
           
            for (int i = 0; i < 3; i++)
            {
                var army2 = faction.armies.GetRandomUnsafe(Ref.rnd);
                if (army2 != null && army2.IdleObjetive())
                {
                    if (army == null ||
                        army2.strengthValue > army.strengthValue)
                    {
                        army = army2;
                    }
                }
            }
            

            return army;
        }

        AbsMapObject AttackFactionAtWar(Army army, Faction opponent)
        {
            if (DssRef.state.events.RunAi() && army != null && opponent != null)
            {
                var areaPos = UnitCollAreaGrid.ToAreaPos(army.tilePos);
                DssRef.world.unitCollAreaGrid.collectCitiesAndArmies(areaPos, 2, army.strengthValue * 0.8f, DssRef.world.unitCollAreaGrid.mapObjects_aiUpdate,
                    PFaction.Empty, opponent.pfaction);
                if (DssRef.world.unitCollAreaGrid.mapObjects_aiUpdate.Count > 0)
                {
                    AbsMapObject result = arraylib.RandomListMember(DssRef.world.unitCollAreaGrid.mapObjects_aiUpdate);
                    army.Ai_Order_Attack(result);
                    return result;
                }
            }
            return null;
        }

        

        City AttackRandom(int warCount, Army army)
        {
            
            if (DssRef.state.events.RunAi() && army != null)
            {
                //var faction = pfaction.GetFaction();

                var areaPos = UnitCollAreaGrid.ToAreaPos(army.tilePos);

                int compareCityCount = 4;

                DssRef.world.unitCollAreaGrid.collectCities_fromArea(areaPos,
                    compareCityCount, DssRef.world.unitCollAreaGrid.cities_aiUpdate,
                    pfaction,  PFaction.Empty);

                //TODO pick random city
                foreach (var city in DssRef.world.unitCollAreaGrid.cities_aiUpdate)
                {
                    if (city.cityType > CityType.UnClaimed && army.strengthValue > city.strengthValue + city.ai_armyDefenceValue)
                    {
                        if (DssRef.world.diplomacy.botMayStartWar(pfaction.GetFaction(), city.pfaction.GetFaction(), warCount))//mayAttackFaction(city.GetFaction()))
                        {
                            army.Ai_Order_Attack(city);
                            return city;
                        }
                    }
                }
            }
            return null;
        }

        //bool mayAttackFaction(Faction otherFaction)
        //{
        //    if (otherFaction == null)
        //    { return false; }

        //    if (DssRef.world.diplomacy.InplayerAlliance(faction))
        //    {
        //        RelationType playerRel = DssRef.world.diplomacy.GetRelationType(faction, otherFaction);
        //        return playerRel <= RelationType.RelationTypeN3_War;
        //    }

        //    if (otherFaction.player.IsLocalPlayer() && 
        //        (DssRef.difficulty.peaceful || !DssRef.state.events.MayAttackPlayer() || !mayAttackPlayer))
        //    {
        //        RelationType playerRel = DssRef.world.diplomacy.GetRelationType(faction, otherFaction);
        //        return playerRel <= RelationType.RelationTypeN3_War;
        //    }

        //    if (otherFaction.player.protectedFromBotAttacks)
        //    {
        //        if (faction.Size() >= FactionSize.Big && Ref.peRnd.Chance(0.25))
        //        { 
        //            return true;
        //        }
        //        return false;
        //    }

        //    var relation = DssRef.world.diplomacy.GetRelationType(faction, otherFaction);

        //    if (relation <= RelationType.RelationType0_Neutral)
        //    {
        //        if (relation == RelationType.RelationTypeN2_Truce)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    else if (relation == RelationType.RelationType1_Peace ||
        //        relation == RelationType.RelationType2_Good) 
        //    {
        //        DiplomaticRelation rel = faction.diplomaticRelations[otherFaction.myIndex];
        //        if (rel.RelationEnd_GameTimeSec.HasTime())
        //        {
        //            return false;
        //        }
        //        return Ref.peRnd.Chance(0.05);
        //    }
        //    return false;
        //}

        public override void onNewRelation(bool isActuator, PFaction otherPFaction, DiplomaticRelation rel, RelationType previousRelation, bool fromAllianceTrade, bool localAction)
        {
            base.onNewRelation(isActuator, otherPFaction, rel, previousRelation, fromAllianceTrade, localAction);
        
            if (rel.Relation <= RelationType.RelationTypeN4_War)
            {
                var faction = pfaction.GetFaction();
                var otherFaction = otherPFaction.GetFaction();

                if (otherFaction.factiontype == FactionType.Player &&
                    DssRef.difficulty.aiAggressivity == AiAggressivity.High)
                {
                    protectedFromBotAttacks = true;
                }
                else if (otherFaction.factiontype == FactionType.DarkLord &&
                    faction.diplomaticSide == DiplomaticSide.None)
                {
                    faction.diplomaticSide = DiplomaticSide.Light;
                }
            }
        }

        public override bool IsLocal => true;
        public override bool IsBot()
        {
            return true;
        }
        public override bool IsLocalPlayer()
        {
            return false;
        }
        public override bool IsHumanPlayer()
        {
            return false;
        }
        public override AiPlayer GetAiPlayer()
        {
            return this;
        }
        public override string Name
        {
            get 
            {
#if DEBUG
                var faction = pfaction.GetFaction();
                if (faction != null && faction.isAlive)
                {
                    return name;
                }
                else
                { 
                    return "(x)" + name;
                }
#else
                return name;
#endif
            }
        }
    }

   
}

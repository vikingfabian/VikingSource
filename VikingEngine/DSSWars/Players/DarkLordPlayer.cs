﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.Players
{
    class DarkLordPlayer : AiPlayer
    {
        List<Faction> darkLordAllies;
        int maxDiplomacy;
        int diplomacyPoints = 0;
        bool hasEntered = false;

        public int factoriesLeft;

        public AbsDetailUnit darkLordUnit = null;

        public DarkLordPlayer(Faction faction)
            : base(faction)
        {
            switch (DssRef.difficulty.bossSize)
            {
                case BossSize.Small:
                    factoriesLeft = 2;
                    maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 16;
                    break;
                case BossSize.Medium:
                    factoriesLeft = 3;
                    maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 22;
                    break;
                case BossSize.Large:
                    factoriesLeft = 3;
                    maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 34;
                    break;
                case BossSize.Huge:
                    factoriesLeft = 4;
                    maxDiplomacy = DssConst.HeadCityStartMaxWorkForce * 40;
                    break;
            }
        }

        public override void writeGameState(BinaryWriter w)
        {
            base.writeGameState(w);

            w.Write(arraylib.SafeCount(darkLordAllies));
            if (darkLordAllies != null)
            {
                foreach (var ally in darkLordAllies)
                {
                    w.Write((ushort)ally.parentArrayIndex);
                }
            }

            w.Write(maxDiplomacy);
            w.Write(diplomacyPoints);
            w.Write(hasEntered);
            w.Write(factoriesLeft);

            Debug.WriteCheck(w);
        }

        public override void readGameState(BinaryReader r, int version, ObjectPointerCollection pointers)
        {
            base.readGameState(r, version, pointers);

            int darkLordAlliesCount = r.ReadInt32();
            if (darkLordAlliesCount > 0)
            {
                darkLordAllies = new List<Faction>(darkLordAlliesCount);
                for (int i = 0; i < darkLordAlliesCount; i++)
                {
                    var f = DssRef.world.factions.Array[r.ReadUInt16()];
                    darkLordAllies.Add(f);
                }
            }

            maxDiplomacy = r.ReadInt32();
            diplomacyPoints = r.ReadInt32();
            hasEntered = r.ReadBoolean();
            factoriesLeft = r.ReadInt32();

            Debug.ReadCheck(r);
        }

        public void EnterMap(Faction takeOverFaction, List<Faction> darkLordAllies)
        {
            if (DssRef.difficulty.PercDifficulty > 100)
            {
                Ref.music.stop(true);
                Ref.music.PlaySong(Data.Music.IAmYourDoom, false);
            }

            faction.gold = DssConst.HeadCityStartMaxWorkForce * 10;

            this.darkLordAllies = darkLordAllies;
            Faction greenwood = DssRef.world.factions.Array[DssRef.settings.Faction_GreenWood];
           
            foreach (var ally in darkLordAllies)
            {
               var relation = DssRef.diplomacy.SetRelationType(faction, ally, RelationType.RelationType3_Ally);//.secret = false;

                if (relation != null)
                {
                    relation.secret = false;
                }

                foreach (var p in DssRef.state.localPlayers)
                {
                    DssRef.diplomacy.SetRelationType(p.faction, ally, RelationType.RelationTypeN4_TotalWar).SpeakTerms = SpeakTerms.SpeakTermsN2_None;
                }

                if (greenwood != null)
                {
                    DssRef.diplomacy.SetRelationType(greenwood, ally, RelationType.RelationTypeN4_TotalWar);
                }                
            }

            darkLordAllies.Remove(faction);
            darkLordAllies.Remove(takeOverFaction);

            makeServant(takeOverFaction, true);

            foreach (var f in darkLordAllies)
            {
                if (f.factiontype == FactionType.DefaultAi)
                {
                    makeServant(f, false);
                }
            }

            diplomacyPoints /= 4;

            hasEntered = true;
        }

        //public override void Update()
        //{
        //    base.Update();

        //    if (hasEntered)
        //    { 
                
        //    }
        //}

        public override void aiPlayerAsynchUpdate(float time)
        {
            base.aiPlayerAsynchUpdate(time);


            if (hasEntered)
            {
                var city = faction.cities.GetRandomUnsafe(Ref.rnd);
                if (city != null)
                {
                    foreach (var n in city.neighborCities)
                    {
                        var nFaction = DssRef.world.cities[n].faction;
                        if (nFaction != faction &&
                            nFaction.diplomaticSide != DiplomaticSide.Light &&
                            !DssRef.diplomacy.PositiveRelationWithPlayer(nFaction))
                        {
                            lock (darkLordAllies)
                            {
                                if (!darkLordAllies.Contains(nFaction))
                                {
                                    darkLordAllies.Add(nFaction);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void oneSecUpdate()
        {
            base.oneSecUpdate();

            if (hasEntered)
            {
                if (maxDiplomacy > 0)
                {
                    diplomacyPoints += DssConst.HeadCityStartMaxWorkForce / 20;

                    if (diplomacyPoints >= 0)
                    {
                        Faction ally = null;
                        lock (darkLordAllies)
                        {
                            ally = arraylib.RandomListMemberPop(darkLordAllies);
                        }

                        if (ally != null && ally.cities.Count > 0)
                        {
                            makeServant(ally, true);
                        }
                    }
                }

                if (faction.cities.Count == 0)
                {
                    DssRef.state.events.onAllDarkCitiesDestroyed();
                }
            }
        }

        void makeServant(Faction takeOverFaction, bool factory)
        {
            int cost = takeOverFaction.citiesEconomy.workerCount;
            diplomacyPoints -= cost;
            maxDiplomacy -= cost;

            takeOverFaction.mergeTo(faction);

            if (factory && factoriesLeft > 0 && takeOverFaction.mainCity != null)
            {
                --factoriesLeft;
                takeOverFaction.mainCity.setFactoryType(true);
            }
        }

        protected override bool buySoldiers(City city, bool aggresive, bool commit)
        {
            bool result = base.buySoldiers(city, aggresive, commit);

            if (commit && DssRef.state.events.nextEvent == EventType.DarkLordInPerson)
            {
                city.conscriptArmy(DssLib.SoldierProfile_HonorGuard.conscript, 4);

                ConscriptProfile profile = new ConscriptProfile();
                profile.specialization = SpecializationType.DarkLord;
                city.conscriptArmy(profile, 1);

                DssRef.state.events.nextEvent = EventType.KillTheDarkLord;
            }

            return result;
        }
    }

}

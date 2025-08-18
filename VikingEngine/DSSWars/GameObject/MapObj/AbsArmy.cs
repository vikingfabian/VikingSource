using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Interface;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars.GameObject
{
    

    abstract partial class AbsArmy : AbsMapObject
    {
        protected bool army_isIdle = true;

        public SpottedArray<SoldierGroup> groups = new SpottedArray<SoldierGroup>(32);
        public Rotation1D rotation = Rotation1D.D180.Add(Ref.peRnd.Plus_MinusF(0.2f));
        public int goalId = 0;
        public bool walkGoalAsShip = false;
        public int soldiersCount = 0;
        public int mostCenterGroup = -1;

        protected int soldierCountBeforeBattle = -1;
        protected float strengthBeforeBattle = -1;

        public bool inBattle = false;
        InBattleWith inBattleWith = new InBattleWith();

        public void AddSoldierGroup(SoldierGroup group)
        {
            //Hitta en plats bland alla grupper
            group.myIndex = groups.Add(group);
            group.army = new WeakReference<AbsArmy>(this);
            group.factionIndex = factionIndex;
        }
        virtual public void remove(SoldierGroup group)
        {
            Debug.CrashIfThreaded();
            groups.RemoveAt_EqualSafeCheck(group, group.myIndex);            
        }
        public override void setFaction(Faction newFaction, bool duringStartup)
        {
            base.setFaction(newFaction, duringStartup);

            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.factionIndex = newFaction.myIndex;
            }
        }
        public void asyncBattleUpdate()
        {
            int mostCenter = -1;
            float distanctToCenter = float.MaxValue;
            InBattleWith battles;
            if (inBattle)
            {
                battles = inBattleWith;
                battles.groupsInBattle = 0;
            }
            else
            {
                battles = new InBattleWith();
            }

            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.asyncBattleUpdate(ref battles);
                float dist = (groupsC.sel.position - position).Length();
                if (dist < distanctToCenter)
                {
                    distanctToCenter = dist;
                    mostCenter = groupsC.CurrentIndex;
                }
            }

            inBattleWith = battles;
            mostCenterGroup = mostCenter;

            if (inBattle)
            {
                if (battles.groupsInBattle == 0)
                {
                    DssRef.state.events.onBattleEnd_async(this, inBattleWith);
                    inBattle = false;
                    if (GetPlayer().IsLocalPlayer())
                    {
                        float strengthLost = strengthBeforeBattle - strengthValue;
                        if (strengthLost >= Achievements.Defeating_victory_strengthLost && groups.Count > 0)
                        {
                            DssRef.achieve.UnlockAchievement_async(AchievementIndex.defeating_victory);
                        }

                        int menLost = soldierCountBeforeBattle - soldiersCount;
                        if (menLost >= Achievements.SlaughteredCount)
                        {
                            DssRef.achieve.UnlockAchievement_async(AchievementIndex.slaughtered);
                        }
                    }
                }
            }
            else if (battles.groupsInBattle >= 2)
            {
                inBattle = true;
                strengthBeforeBattle = strengthValue;
                soldierCountBeforeBattle = soldiersCount;
                if (GetPlayer().IsLocalPlayer())
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        var localplayer = GetPlayer().GetLocalPlayer();
                        if (localplayer.battleMessageCheck(tilePos))
                        {
                            RichBoxContent content = new RichBoxContent();
                            MessageGroup_Ingame.Title(content, DssRef.lang.Hud_Battle);

                            var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
                            MessageGroup_Ingame.ControllerInputIcons(localplayer, gotoBattleButtonContent);
                            gotoBattleButtonContent.Add(new RbText(TypeName()));

                            content.Add(new ArtButton(RbButtonStyle.Primary, gotoBattleButtonContent,
                                new RbAction1Arg<AbsGameObject>(localplayer.hud.messages.goToMapObject, this)));

                            localplayer.hud.messages.Add(content);
                        }
                    }));
                }
            }
        }


        //protected void net_writeGroups(System.IO.BinaryWriter w)
        //{
        //    w.Write((ushort)groups.Count);
        //    var groupsC = groups.counter();
        //    while (groupsC.Next())
        //    {
        //        w.Write((ushort)groupsC.sel.parentArrayIndex);
        //        groupsC.sel.writeNet(w);
        //    }
        //}

        //protected void net_readGroups(System.IO.BinaryReader r)
        //{
        //    int groupsCount = r.ReadUInt16();
        //    for (int i = 0; i < groupsCount; i++)
        //    {
        //        int index = r.ReadUInt16();
        //        var group = groups.GetIndex_Safe(index);
        //        bool needInit = false;
        //        if (group == null)
        //        {
        //            needInit = true;
        //            if (IsCity())
        //            {
        //                group = new GuardGroup(this);
        //            }
        //            else
        //            {
        //                group = new SoldierGroup(this);
        //            }
        //            groups.HardSet(group, index);
        //        }

        //        group.readNet(r, needInit);
        //        group.net_onUpdate();
        //        group.net_updateclient(DssRef.state.culling.playerInDetailView);
        //    }
        //}

        protected void writeGroups(System.IO.BinaryWriter w)
        {
            w.Write((ushort)groups.Count);
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.writeGameState(w);               
            }

            Debug.WriteCheck(w);
        }
        public void readGroups(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
        {
            int groupsCount = r.ReadUInt16();

            if (IsCity())
            {
                for (int i = 0; i < groupsCount; i++)
                {
                    GuardGroup group = new GuardGroup(this, r, subVersion, pointers);
                    
                }
            }
            else
            {
                for (int i = 0; i < groupsCount; i++)
                {
                    SoldierGroup group = new SoldierGroup(this, r, subVersion, pointers);
                    
                }
            }

            if (subVersion >= 62)
            {
                Debug.ReadCheck(r);
            }
        }

        

        virtual public void asyncNearObjectsUpdate()
        {
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.asynchNearObjectsUpdate();
            }
        }

        abstract public bool IdleObjetive();

        abstract public bool IsCity();
        abstract public bool IsArmy();

    }
}

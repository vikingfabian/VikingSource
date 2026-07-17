using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Net;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.GameObject
{
    

    abstract partial class AbsArmy : AbsMapObject, IEquatable<PArmy>
    {
        public bool Equals(PArmy other)
        {
            return pfaction == other.pfaction && other.armyIndex == myIndex;
        }
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
        public InBattleWith inBattleWith = new InBattleWith();
        public GameTimeStamp lastTimeTradedBetweenPlayers = GameTimeStamp.None;

        public void tradeBetweenPlayers_toHud(LocalPlayer player, RichBoxContent content)
        {
            if (pfaction == player.pfaction && player.alliedFactions.Count > 0)
            {   
                content.Add(new RbSeperationLine());
                HudLib.Label(content, "Gift to player");
                content.hspace();

                if (lastTimeTradedBetweenPlayers.TimeOut())
                {
                    lock (player.alliedFactions)
                    {
                        foreach (var m in player.alliedFactions)
                        {
                            //var f = DssRef.world.faction(m);
                            if (pfaction.TryGetFaction(out var f))
                            {
                                RichBoxContent buttonContent = new RichBoxContent();
                                f.toHud(buttonContent, RelationType.NONE, true, true);

                                content.Add(new ArtButton(RbButtonStyle.Primary, buttonContent, new RbAction1Arg<Faction>(
                                    (Faction selected) =>
                                    {
                                        lastTimeTradedBetweenPlayers.setTimeFromNow(TimeExt.MinuteInSeconds * 10);

                                        setFaction(selected, false, true, ConvertReason.Gift, true);

                                        player.gameControls.clearSelection();

                                    }, f), null));
                            }
                        }
                    }
                }
                else
                {
                    content.Add(new RbText(HudLib.TimeSpan_LongText(lastTimeTradedBetweenPlayers.TimeSpan_Left()), HudLib.NotAvailableColor));
                }
            }
        }

        public PArmy pointer()
        {
            return new PArmy(pfaction, myIndex);
        }
        public PMapObject mapObjPointer()
        {
            return new PMapObject(gameobjectType(), pfaction, myIndex);
        }
        public void AddSoldierGroup(SoldierGroup group)
        {
            //Hitta en plats bland alla grupper
            group.myIndex = groups.Add(group);
            soldiersCount += group.soldierCount;
            if (soldiersCount <= 0)
            {
                lib.DoNothing();
            }
            group.army = new WeakReference<AbsArmy>(this);
            group.pfaction = pfaction;
        }

        const int GroupsPerPacket = 8;
        public void netWriteGroups(Network.PacketReliability reliability, ref int packetCount, bool isHandOver)
        {
            int groupIndex = 0;
            int packetIndex = 0;
            while (groupIndex < groups.Array.Length)
            {
                var w = Ref.netSession.BeginWritingPacket_Asynch(IsArmy() ? Network.PacketType.DssSoldierGroupStatus_Army : Network.PacketType.DssSoldierGroupStatus_City, reliability, out var packet);
                {
                    w.Write(isHandOver);
                    Net.ObjectId.NetWriteMapObjId(w, this);

                    w.Write((byte)packetIndex);
                    Debug.WriteCheck(w);
                    for (int i = 0; i < GroupsPerPacket; i++)
                    {
                        var group = groups.GetIndex_Safe(groupIndex);
                        if (group != null)
                        {
                            w.Write(true);

                            group.writeNet(w);
                            Debug.WriteCheck(w);
                            //NetWriteGroup(w, group);
                        }
                        else
                        {
                            w.Write(false);
                        }
                        groupIndex++;
                    }
                    Debug.WriteCheck(w);

                } packet.EndWrite_Asynch();
                packetIndex++;
            }

            lastNetUpdate.setNow();
          
        }
       
        public static void NetReadGroups(bool bArmy, System.IO.BinaryReader r)
        {
            bool isHandOver = r.ReadBoolean();

            if (ObjectId.NetReadMapObjId(r, out Faction faction, bArmy, true, out AbsArmy mapObj, out bool needInit))
            {
                if (mapObj != null && (!mapObj.IsNetHosted || isHandOver))
                {  
                    int packetIndex = r.ReadByte();
                    Debug.ReadCheck(r);
                    var rpos = r.BaseStream.Position;
                    for (int i = 0; i < GroupsPerPacket; i++)
                    {
                        int groupIndex = packetIndex * GroupsPerPacket + i;
                        if (r.ReadBoolean())
                        {
                            mapObj.NetReadGroup(r, groupIndex);
                            if (Debug.ReadCheck_returnIfError(r))
                            {
                                r.BaseStream.Position = rpos;
                                mapObj.NetReadGroup(r, groupIndex);
                            }
                        }
                        else
                        {
                            var group = mapObj.groups.PullIndex_Safe(groupIndex);
                            if (group != null)
                            {
                                //if (mapObj.IsArmy() || mapObj.IsNetHosted)
                                //{
                                //    lib.DoNothing();
                                //}

                                group.DeleteMe(DeleteReason.NetworkEvent, false);
                            }
                        }
                    }
                    Debug.ReadCheck(r);
                }
            }
        }
        public void NetReadGroup(System.IO.BinaryReader r, int index)
        {
            //var group = army.groups.GetIndex_Safe(index);
            //bool needInit = false;
            //if (group == null)
            //{
            //    needInit = true;
            //    if (army.IsCity())
            //    {
            //        group = new GuardGroup(army);
            //    }
            //    else
            //    {
            //        group = new SoldierGroup(army);
            //    }
            //    army.groups.HardSet(group, index);
            //    group.myIndex = index;
            //    if (!group.pfaction.HasValue())
            //    {
            //        throw new Exception();
            //    }
            //}
            var group = NetGetGroup(index, true, out var needInit);
            group.readNet(this, r, needInit);
            group.net_onUpdate();
        }

        public SoldierGroup NetGetGroup(int index, bool createIfMissing, out bool needInit)
        {
            var group = groups.GetIndex_Safe(index);
            needInit = false;
            
            if (group == null && createIfMissing)
            {
                needInit = true;
                if (IsCity())
                {
                    group = new GuardGroup(this);
                }
                else
                {
                    group = new SoldierGroup(this);
                }
                groups.HardSet(group, index);
                group.myIndex = index;
                //if (!group.pfaction.HasValue())
                //{
                //    throw new Exception();
                //}
            }

            return group;
        }

        virtual public void remove(SoldierGroup group)
        {
            //Debug.CrashIfThreaded();
            if (IsNetHosted || debugTagged)//pfaction == DssRef.state.LocalHost().pfaction)
            {
                lib.DoNothing();
            }
            groups.RemoveAt_EqualSafeCheck(group, group.myIndex);            
        }
        public override void setFaction(Faction newFaction, bool duringStartup, bool convert, ConvertReason convertReason, bool netShare)
        {
            base.setFaction(newFaction, duringStartup, convert, convertReason, netShare);

            convertSoldiersToFaction(newFaction.pfaction);
        }

        public void convertSoldiersToFaction(PFaction newFaction)
        {
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.pfaction = pfaction;
            }
        }

        override public void clientPauseUpdate()
        {
            base.clientPauseUpdate();

            if (inRender_detailLayer)
            {

                if (groups.Count > 0)
                {

                    var groupsC = groups.counter();

                    while (groupsC.Next())
                    {
                        groupsC.sel.clientPauseUpdate();
                    }
                }
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

            if (IsNetHosted)
            {
                if (inBattle)
                {
                    if (battles.groupsInBattle == 0)
                    {
                        DssRef.state.events?.onBattleEnd_async(this, inBattleWith);
                        inBattle = false;
                        if (pfaction.GetPlayer().IsLocalPlayer() && !DssRef.achieve.achivementsAreModeBlocked())
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

                            if (battles.attackingCity)
                            {
                                groupsC.Reset();
                                while (groupsC.Next())
                                {
                                    if (groupsC.sel.soldierConscript.conscript.weapon == Resource.ItemResourceType.SiegeCannonBronze)
                                    {
                                        DssRef.achieve.UnlockAchievement_async(AchievementIndex.ottoman);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (battles.groupsInBattle >= 2)
                {
                    inBattle = true;
                    strengthBeforeBattle = strengthValue;
                    soldierCountBeforeBattle = soldiersCount;
                    if (pfaction.TryGetLocalPlayer(out _))
                    {
                        Ref.update.AddSyncAction(new SyncAction(() =>
                        {
                            var localplayer = pfaction.GetPlayer().GetLocalPlayer();
                            if (localplayer.battleMessageCheck(tilePos))
                            {
                                RichBoxContent content = new RichBoxContent();
                                MessageGroup_Ingame.Title(content, DssRef.lang.Hud_Battle);

                                var gotoButtonContent = new RichBoxContent();
                                MessageGroup_Ingame.ControllerInputIcons(localplayer, gotoButtonContent);
                                this.toButtonContent(gotoButtonContent, true);

                                content.Add(new ArtButton(RbButtonStyle.Primary, gotoButtonContent,
                                    new RbAction1Arg<AbsGameObject>(localplayer.hud.messages.goToMapObject, this, RbSoundType.Default))
                                { fillWidth = true });

                                localplayer.hud.messages.Add(content);
                            }
                        }));
                    }
                }
            }
        }

        

        protected void writeSoldierGroups(System.IO.BinaryWriter w)
        {
            w.Write((ushort)groups.Count);
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.writeGameState(w, true);               
            }

            Debug.WriteCheck(w);
        }
        public void readSoldierGroups(System.IO.BinaryReader r, int subVersion, ObjectPointerCollection pointers)
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


        public override AbsArmy GetAbsArmy()
        {
            return this;
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

       

    }
}

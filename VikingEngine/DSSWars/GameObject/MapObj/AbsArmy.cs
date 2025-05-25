using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Defence;
using VikingEngine.DSSWars.Display;
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
        protected bool inBattle = false;

        public void AddSoldierGroup(SoldierGroup group)
        {
            //Hitta en plats bland alla grupper
            group.parentArrayIndex = groups.Add(group);
            group.army = this;
        }
        virtual public void remove(SoldierGroup group)
        {
            Debug.CrashIfThreaded();
            groups.RemoveAt_EqualSafeCheck(group, group.parentArrayIndex);            
        }

        public void asyncBattleUpdate()
        {
            int mostCenter = -1;
            float distanctToCenter = float.MaxValue;

            int groupsInBattle = 0;
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.asyncBattleUpdate(ref groupsInBattle);
                float dist = (groupsC.sel.position - position).Length();
                if (dist < distanctToCenter)
                {
                    distanctToCenter = dist;
                    mostCenter = groupsC.CurrentIndex;
                }
            }

            mostCenterGroup = mostCenter;

            if (inBattle)
            {
                if (groupsInBattle == 0)
                {
                    inBattle = false;
                }
            }
            else if (groupsInBattle >= 2)
            {
                inBattle = true;
                if (faction.player.IsLocalPlayer())
                {
                    Ref.update.AddSyncAction(new SyncAction(() =>
                    {
                        var localplayer = faction.player.GetLocalPlayer();
                        if (localplayer.battleMessageCheck(tilePos))
                        {
                            RichBoxContent content = new RichBoxContent();
                            MessageGroup.Title(content, DssRef.lang.Hud_Battle);

                            var gotoBattleButtonContent = new List<AbsRichBoxMember>(6);
                            MessageGroup.ControllerInputIcons(localplayer, gotoBattleButtonContent);
                            gotoBattleButtonContent.Add(new RbText(TypeName()));

                            content.Add(new ArtButton(RbButtonStyle.Primary, gotoBattleButtonContent,
                                new RbAction1Arg<AbsGameObject>(localplayer.hud.messages.goToMapObject, this)));

                            localplayer.hud.messages.Add(content);
                        }
                    }));
                }
            }
        }

        protected void writeGroups(System.IO.BinaryWriter w)
        {
            w.Write((ushort)groups.Count);
            var groupsC = groups.counter();
            while (groupsC.Next())
            {
                groupsC.sel.writeGameState(w);
               
            }
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

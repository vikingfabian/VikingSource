using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Work;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.SteamWrapping;

//

namespace VikingEngine.DSSWars.GameObject
{
    struct UnitBaseData
    {
        public int factionIndex = -1;
        public int myIndex = -1;
        public bool isDeleted = false;
        public Vector3 position = Vector3.Zero;
        public bool debugTagged = false;

        public UnitBaseData() { }

        public Faction GetFaction()
        {

            if (factionIndex < 0)
            {
                return null;
            }

            return DssRef.world.faction(factionIndex);
        }

        public bool TryGetFaction(out Faction faction)
        {
            if (factionIndex >= 0 && factionIndex < DssRef.world.factions.Count)
            {
                faction = DssRef.world.factions.Array[factionIndex];
                return true;
            }
            faction = null;
            return false;
        }

        public Faction GetFaction_Safe()
        {
            return DssRef.world?.faction(factionIndex);
        }

        public Players.AbsPlayer GetPlayer()
        {

            if (factionIndex < 0 || factionIndex >= DssRef.world.factions.Array.Length)
            {
                return null;
            }

            return DssRef.world.factions.Array[factionIndex]?.player;
        }

        public bool TryGetPlayer(out Players.AbsPlayer player)
        {

            if (factionIndex < 0 || factionIndex >= DssRef.world.factions.Array.Length)
            {
                player = null;
            }
            else
            {
                player = DssRef.world.factions.Array[factionIndex]?.player;
            }
            return player != null;
        }

        public bool GetCasual()
        {
            //if (factionIndex > 0)
            //{
            var f = DssRef.world.faction(factionIndex);
            return f != null && f.player != null && f.player.profile.casualControls;
            //}
            //return false;
        }

        public Vector2 posXZ()
        { return new Vector2(position.X, position.Z); }

    }

    abstract class AbsGameObject
    {
        public int factionIndex = -1;
        public int myIndex = -1;
        public bool isDeleted = false;
        public Vector3 position = Vector3.Zero;
        public bool debugTagged = false;

        abstract public GameObjectType gameobjectType();

        //virtual public AbsWorldObject GetWorldObject() 
        //{
        //    return null;
        //}

        virtual public bool IsDeleted()
        {
            return isDeleted;
        }
        virtual public bool IsGuardGroup()
        {
            return false;
        }
        virtual public bool IsSoldiers()
        {
            return false;
        }

        public bool HasFaction()
        { 
            return factionIndex >= 0 && factionIndex < DssRef.world.factions.Count;
        }

        public bool HasPlayer()
        {
            if (factionIndex >= 0 && factionIndex < DssRef.world.factions.Count)
            {
                var f = DssRef.world.factions.Array[factionIndex];
                return f != null && f.player != null;
            }
            return false;
        }

        public bool HasAliveFaction()
        {
            if (factionIndex >= 0 && factionIndex < DssRef.world.factions.Count)
            { 
                return DssRef.world.factions.Array[factionIndex] != null && DssRef.world.factions.Array[factionIndex].isAlive; 
            }
            return false;
        }

        virtual public Faction GetFaction_NoChecks()
        {
            if (factionIndex < 0 || factionIndex >= DssRef.world.factions.Count)
            {
                return null;
            }

            return DssRef.world.factions.Array[factionIndex];
        }

        virtual public Faction GetFaction()
        {

            if (factionIndex < 0)
            {
                return null;
            }

            return DssRef.world.faction(factionIndex);
        }

        public bool TryGetFaction(out Faction faction)
        {
            if (factionIndex >= 0 && factionIndex < DssRef.world.factions.Count)
            {
                faction = DssRef.world.factions.Array[factionIndex];
                return true;
            }
            faction = null;
            return false;
        }

        virtual public Faction GetFaction_Safe()
        {
            return DssRef.world?.faction(factionIndex);
        }

        public Players.AbsPlayer GetPlayer()
        {

            if (factionIndex < 0 || factionIndex >= DssRef.world.factions.Array.Length)
            {
                return null;
            }

            return DssRef.world.factions.Array[factionIndex]?.player;
        }

        public bool TryGetPlayer(out Players.AbsPlayer player)
        {

            if (factionIndex < 0 || factionIndex >= DssRef.world.factions.Array.Length)
            {
                player = null;
            }
            else
            {
                player = DssRef.world.factions.Array[factionIndex]?.player;
            }
            return player != null;
        }

        public bool GetCasual()
        {
            //if (factionIndex > 0)
            //{
            var f = DssRef.world.faction(factionIndex);
            return f != null && f.player != null && f.player.profile.casualControls;
            //}
            //return false;
        }

        virtual public City GetCity() { return null; }

        virtual public AbsArmy GetAbsArmy() { return null; }
        virtual public Army GetArmy() { return null; }
        virtual public AbsSoldierUnit GetSoldier() { return null; }
        virtual public SoldierGroup GetSoldierGroup() { return null; }

        virtual public ArmyCollection GetMapCollection() { return null; }
        virtual public DetailObjectCollection GetDetailCollection() { return null; }

        virtual public WorkerUnit GetWorker() { return null; }

        virtual public AbsMapObject RelatedMapObject() { return null; }

        virtual public IntVector2 TilePos() 
        { 
            throw new NotImplementedException();
        }

        //virtual public Vector3 WorldPos()
        //{
        //    throw new NotImplementedException();
        //}

        virtual public string TypeName() { return null; }


        virtual public void TypeIcon(RichBoxContent content) {  }

        virtual public string Name(out bool mayEdit) {
            mayEdit = false;
            return null; 
        }

        virtual public void selectionGui(Players.LocalPlayer player, Graphics.ImageGroup guiModels)
        { }
        virtual public void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        { }

        public void beginEditName()
        {
            var reciever = new DSSWars.Players.PlayerControls.TextInput(this);
            SteamInputManager.tryOpenSteamKeyboard(reciever);
        }

        virtual public void NameEditEvent(string result, object tag)
        {
            throw new NotImplementedException();
        }

        virtual public void toTooltip(ObjectHudArgs args)
        {
            string name = Name(out _);
            if (name != null)
            {
                args.content.text(name).overrideColor = HudLib.TitleColor_Name;
            }
            args.content.h2(TypeName()).overrideColor = HudLib.TitleColor_TypeName;
        }

        virtual public void toButtonContent(RichBoxContent content, bool dark)
        {
            content.Add(new RbText(Name(out _), dark ? HudLib.TitleColor_Name_Dark : HudLib.TitleColor_Name));
            content.Add(new RbImage(SpriteName.warsBulletSeperationPoint));
            content.Add(new RbText(TypeName(), dark ? HudLib.TitleColor_TypeName_Dark : HudLib.TitleColor_TypeName));
        }

        public void nameToHud(RichBoxContent content, bool mayInteract)
        { 
            string name = Name(out bool mayEdit);
            if (name != null)
            {
                if (Ref.update.textInput != null &&
                    !Ref.update.textInput.Exiting &&
                    Ref.update.textInput.recieverId == Name(out _))
                {
                    content.Add(new RbButton(new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.InterfaceTextInput),
                        new RbSpace(),
                        new RbText(Ref.update.textInput.DisplayText(), Color.Black),
                    }, null, null)
                    { overrideBgColor = Color.White });

                    content.newLine();
                }
                else
                {
                    if (mayEdit && mayInteract)
                    {
                        var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                            new RbAction(beginEditName), null);
                        content.Add(editButton);
                        content.space();
                    }
                    content.Add(new RbText(name, Color.LightYellow));
                    content.newLine();
                }
            }
        }
        public void ownerToHud(Interface.ObjectHudArgs args, bool divider)
        {
            var faction = GetFaction();
            if (args.player != null && faction != null && faction != args.player.faction)
            {
                var relation = DssRef.world.diplomacy.GetRelation(args.player.faction, faction).Relation;

                //args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsGovernmentIcon));
                args.content.space(0.5f);
                args.content.Add(new RbImage(Diplomacy.RelationSprite(relation)));

                if (faction.player.IsRemotePlayer())
                {
                    args.content.space(0.5f);
                    args.content.Add(new RbGamerIcon(((RemotePlayer)faction.player).networkPeer.peer, 0.8f));
                }

                args.content.space(0.5f);
                args.content.Add(new RbText(faction.PlayerName, HudLib.TitleColor_Name));

                if (divider)
                {
                    args.content.Add(new RbSeperationLine());
                }
            }
        }

        virtual public void toHud(Interface.ObjectHudArgs args)
        {
            nameToHud(args.content, true);
            args.content.Add(new RbBeginTitle());
            args.content.Add(GetFaction().FlagTextureToHud());
            TypeIcon(args.content);
            args.content.Add(new RbText(TypeName()));

            //if (args.ShowFull)
            {
                if (PlatformSettings.DevBuild)
                {
                    args.content.text("agg " + GetFaction().player.aggressionLevel.ToString());
                }
                if (GetFaction() != args.player.faction)
                {
                    var relation = DssRef.world.diplomacy.GetRelation(args.player.faction, GetFaction()).Relation;

                    args.content.newLine();
                    args.content.Add(new RbText(GetFaction().PlayerName, Color.LightYellow));
                    args.content.newLine();
                    args.content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
                    args.content.Add(new RbText(Diplomacy.RelationString(relation), Color.LightBlue));

                }
                args.content.Add(new RbSeperationLine());
            }
        }
        virtual public bool CanMenuFocus() { return false; }
        virtual public bool aliveAndBelongTo(Faction faction) { return true; }

        virtual public bool IsCollection() { return false; }
        virtual public int CollectionCount() { return 0; }
        //abstract public bool IsDeleted();

        abstract public bool defeatedBy(int attackerFaction);

        virtual public bool defeated()
        {
            return isDeleted;
        }

        abstract public bool aliveAndBelongTo(int faction);

        //public override AbsWorldObject GetWorldObject()
        //{
        //    return this;
        //}

        //public override Vector3 WorldPos()
        //{
        //    return position;
        //}
        virtual public void stateDebugText(HUD.RichBox.RichBoxContent content)
        { }

        virtual public void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
        }

        protected void debugTagButton(RichBoxContent content)
        {
#if DEBUG
            content.Button(string.Format("debug tag ({0})", debugTagged), new HUD.RichBox.RbAction(AddDebugTag), null, true);
#endif
        }

        virtual public void AddDebugTag()
        {
            lib.Invert(ref debugTagged);
            Debug.Log((debugTagged ? "Tagged: " : "Remove tag: ") + this.ToString());
        }

        public Vector2 posXZ()
        { return new Vector2(position.X, position.Z); }

        virtual public bool rectangleCollision(ScreenToSpaceRectangleBound rectangle)
        {
            throw new NotImplementedException();
        }
    }
    enum GameObjectType
    {
        Faction,
        City,
        Army,
        SoldierGroup,
        Soldier,
        Battle,
        Worker,

        ObjectCollection,
        DetailCollection,
        LocationPin,
        Point,
        NONE,
        NUM,
    }
    enum DeleteReason
    {
        Death,
        Transform,
        EmptyGroup,
        Disband,
        Desert,
        CameraCulling,
    }
}


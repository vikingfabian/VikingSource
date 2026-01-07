using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

//

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsGameObject
    {
        public int factionIndex = -1;
        public int myIndex = -1;
        public bool isDeleted = false;
        
        abstract public GameObjectType gameobjectType();

        virtual public AbsWorldObject GetWorldObject() 
        {
            return null;
        }

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
#if DEBUG
            if (factionIndex < 0)
            {
                throw new Exception();
            }
#endif
            return DssRef.world.faction(factionIndex);
        }

        virtual public Faction GetFaction_Safe()
        {
            return DssRef.world?.faction(factionIndex);
        }

        public Players.AbsPlayer GetPlayer()
        {

            if (factionIndex < 0)
            {
                return null;
            }

            return DssRef.world.factions.Array[factionIndex]?.player;
        }

        public bool GetCasual()
        {
            //if (factionIndex > 0)
            //{
            var f = DssRef.world.faction(factionIndex);
            return f != null && f.player.profile.casualControls;
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

        virtual public Vector3 WorldPos()
        {
            throw new NotImplementedException();
        }

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
            new DSSWars.Players.PlayerControls.TextInput(this);
            //new TextInputState(Name(out _), NameEditEvent, null);
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



        protected void nameToHud(RichBoxContent content, bool mayInteract)
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
        protected void ownerToHud(Interface.ObjectHudArgs args, bool divider)
        {
            var faction = GetFaction();
            if (args.player != null && faction != null && faction != args.player.faction)
            {
                var relation = DssRef.diplomacy.GetRelationType(args.player.faction, faction);

                args.content.newLine();
                args.content.Add(new RbImage(SpriteName.WarsGovernmentIcon));
                args.content.space(0.5f);
                args.content.Add(new RbImage(Diplomacy.RelationSprite(relation)));
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
                    var relation = DssRef.diplomacy.GetRelationType(args.player.faction, GetFaction());

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
}


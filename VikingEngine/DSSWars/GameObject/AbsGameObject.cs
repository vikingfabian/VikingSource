using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Work;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall.GO;
//

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsGameObject
    {

        public int parentArrayIndex = -1;

        abstract public GameObjectType gameobjectType();
        
        abstract public Faction GetFaction();

        virtual public City GetCity() { return null; }

        virtual public Army GetArmy() { return null; }
        virtual public AbsSoldierUnit GetSoldier() { return null; }
        virtual public SoldierGroup GetSoldierGroup() { return null; }

        virtual public MapObjectCollection GetCollection() { return null; }

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
            new TextInput(Name(out _), NameEditEvent, null);
        }

        virtual protected void NameEditEvent(string result, object tag)
        {
            throw new NotImplementedException();
        }

        virtual public void toHud(Display.ObjectHudArgs args)
        {
            string name = Name(out bool mayEdit);
            if (name != null)
            {
                if (mayEdit)
                {
                    var editButton = new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.InterfaceTextInput) },
                        new RbAction(beginEditName), null);
                    args.content.Add(editButton);
                    args.content.space();
                }
                var nameText = new RbText(name);
                nameText.overrideColor = Color.LightYellow;
                args.content.Add(nameText);
                args.content.newLine();
            }
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
        NONE,
        NUM,
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
//

namespace VikingEngine.DSSWars.GameObject
{

    abstract class AbsGameObject
    {
        public Vector3 position = Vector3.Zero;
        public bool debugTagged = false;
        public bool isDeleted = false;
        public int parentArrayIndex = -1;

        abstract public GameObjectType gameobjectType();

        virtual public void selectionGui(Players.LocalPlayer player, Graphics.ImageGroup guiModels)
        { }

        virtual public void selectionFrame(bool hover, Selection selection)
        { }

        abstract public bool defeatedBy(Faction attacker);

        virtual public bool defeated()
        {
            return isDeleted;
        }

        abstract public bool aliveAndBelongTo(Faction faction);

        virtual public void toHud(Display.ObjectHudArgs args)
        {
            args.content.Add(new RichBoxBeginTitle());
            args.content.Add(Faction().FlagTextureToHud());
            args.content.Add(new RichBoxText(Name()));

            if (PlatformSettings.DevBuild)
            {
                args.content.text("agg " + Faction().player.aggressionLevel.ToString());
            }
            if (Faction() != args.player.faction)
            {
                var relation = DssRef.diplomacy.GetRelationType(args.player.faction, Faction());

                args.content.Add(new RichBoxNewLine());
                args.content.Add(new RichBoxText(Faction().PlayerName, Color.LightYellow));
                args.content.newLine();
                args.content.Add(new RichBoxImage(Diplomacy.RelationSprite(relation)));
                args.content.Add(new RichBoxText(Diplomacy.RelationString(relation), Color.LightBlue));
                
            }
            args.content.Add(new RichBoxSeperationLine());
        }

        abstract public string Name();

        abstract public Faction Faction();

        abstract public AbsMapObject RelatedMapObject();

        virtual public City GetCity() { return null; }

        virtual public Army GetArmy() { return null; }

        virtual public void stateDebugText(HUD.RichBox.RichBoxContent content)
        { }

        virtual public void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
        }

        virtual public void tagObject()
        {
            lib.Invert(ref debugTagged);
            Debug.Log((debugTagged? "Tagged: " : "Remove tag: ") + this.ToString());
        }

        //int spottedArrayMemberIndex = -1;
        //public int SpottedArrayMemberIndex { get { return spottedArrayMemberIndex; } set { spottedArrayMemberIndex = value; } }
        //virtual public bool SpottedArrayUseIndex { get { return spottedArrayMemberIndex>= 0; } }
    }

    enum DeleteReason
    { 
        Death,
        Transform,
        EmptyGroup,
        Disband,
        Desert,
    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.GameObject
{
    abstract class AbsWorldObject: AbsGameObject
    {
        public Vector3 position = Vector3.Zero;
        public bool debugTagged = false;
        public bool isDeleted = false;
        

        

       

        abstract public bool defeatedBy(Faction attacker);

        virtual public bool defeated()
        {
            return isDeleted;
        }

        abstract public bool aliveAndBelongTo(int faction);

        //virtual public void toHud(ObjectHudArgs args)
        //{
        //    string name = Name();

        //    if (name != null)
        //    {
        //        args.content.text(name).overrideColor = Color.LightYellow;
        //        args.content.newLine();
        //    }

        //    args.content.Add(new RichBoxBeginTitle());
        //    args.content.Add(GetFaction().FlagTextureToHud());
        //    args.content.Add(new RichBoxText(TypeName()));

        //    if (PlatformSettings.DevBuild)
        //    {
        //        args.content.text("agg " + GetFaction().player.aggressionLevel.ToString());
        //    }
        //    if (GetFaction() != args.player.faction)
        //    {
        //        var relation = DssRef.diplomacy.GetRelationType(args.player.faction, GetFaction());

        //        args.content.newLine();
        //        args.content.Add(new RichBoxText(GetFaction().PlayerName, Color.LightYellow));
        //        args.content.newLine();
        //        args.content.Add(new RichBoxImage(Diplomacy.RelationSprite(relation)));
        //        args.content.Add(new RichBoxText(Diplomacy.RelationString(relation), Color.LightBlue));

        //    }
        //    args.content.Add(new RichBoxSeperationLine());
        //}

        
        virtual public void stateDebugText(HUD.RichBox.RichBoxContent content)
        { }

        virtual public void DeleteMe(DeleteReason reason, bool removeFromParent)
        {
            isDeleted = true;
        }

        protected void debugTagButton(RichBoxContent content)
        {
            content.Button(string.Format("debug tag ({0})", debugTagged), new HUD.RichBox.RbAction(AddDebugTag), null, true);
        }

        virtual public void AddDebugTag()
        {
            lib.Invert(ref debugTagged);
            Debug.Log((debugTagged ? "Tagged: " : "Remove tag: ") + this.ToString());
        }
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

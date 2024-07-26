using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Players;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.Players;
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

        virtual public string Name() { return null; }

        virtual public void selectionGui(Players.LocalPlayer player, Graphics.ImageGroup guiModels)
        { }
        virtual public void selectionFrame(bool hover, Selection selection)
        { }

        virtual public void toHud(Display.ObjectHudArgs args)
        {
            string name = Name();
            if (name != null)
            {
                args.content.text(name).overrideColor = Color.LightYellow;
                args.content.newLine();
            }
            args.content.Add(new RichBoxBeginTitle());
            args.content.Add(GetFaction().FlagTextureToHud());
            args.content.Add(new RichBoxText(TypeName()));

            if (PlatformSettings.DevBuild)
            {
                args.content.text("agg " + GetFaction().player.aggressionLevel.ToString());
            }
            if (GetFaction() != args.player.faction)
            {
                var relation = DssRef.diplomacy.GetRelationType(args.player.faction, GetFaction());

                args.content.newLine();
                args.content.Add(new RichBoxText(GetFaction().PlayerName, Color.LightYellow));
                args.content.newLine();
                args.content.Add(new RichBoxImage(Diplomacy.RelationSprite(relation)));
                args.content.Add(new RichBoxText(Diplomacy.RelationString(relation), Color.LightBlue));

            }
            args.content.Add(new RichBoxSeperationLine());
        }

        abstract public bool aliveAndBelongTo(Faction faction);
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

        NUM_NON,
    }
}


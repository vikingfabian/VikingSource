using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.GameObject
{
    class MapObjectCollection : AbsWorldObject
    {
        Faction faction;
        public List<AbsMapObject> objects = new List<AbsMapObject>(8);

        public MapObjectCollection(Faction faction)
        { 
            this.faction = faction;
        }

        public override void selectionFrame(bool hover, Selection selection)
        {
            selection.BeginGroupModel(false);

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].GetArmy().selectionFramePlacement(out var pos, out var scale);
                selection.setGroupModel(i, pos, scale, hover, true, false);
            }
        }

        public override void selectionGui(LocalPlayer player, ImageGroup guiModels)
        {
            foreach (var obj in objects)
            {
                obj.GetArmy().hoverAndSelectInfo(guiModels, player.playerData.localPlayerIndex);
            }

        }

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.ObjectCollection;
        }
        public override MapObjectCollection GetCollection()
        {
            return this;
        }

        public override bool aliveAndBelongTo(Faction faction)
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (!objects[i].aliveAndBelongTo(faction))
                { 
                    objects.RemoveAt(i);
                }
            }
            

            return objects.Count > 0;
        }
        public override Faction GetFaction()
        {
            return faction;
        }

        public override AbsMapObject RelatedMapObject()
        {
            throw new NotImplementedException();
        }

        public override bool defeatedBy(Faction attacker)
        {
            throw new NotImplementedException();
        }

        public void set(List<AbsMapObject> newObjects)
        { 
            this.objects.Clear();
            if (newObjects.Count > 0)
            {
                lib.DoNothing();
            }
            this.objects.AddRange(newObjects);
        }
    }
}

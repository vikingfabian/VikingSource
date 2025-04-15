using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.GameObject
{
    class MapObjectCollection : AbsGOCollection
    {
        //Faction faction;
        public List<AbsMapObject> objects = new List<AbsMapObject>(8);

        public MapObjectCollection(Faction faction)
        {
            this.faction = faction;
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            selection.groupModels_terrian.BeginGroupModel();

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].GetArmy().selectionFramePlacement(out var pos, out var scale);
                selection.groupModels_terrian.setGroupModel(i, pos, scale, hover, true, false);
            }
        }

        public override void selectionGui(LocalPlayer player, ImageGroup guiModels)
        {
            foreach (var obj in objects)
            {
                obj.GetArmy().hoverAndSelectInfo(player, guiModels);
            }

        }

        public override void toHud(ObjectHudArgs args)
        {
            GroupPresentation(args, false);
            //args.content.h2(string.Format(DssRef.lang.Hud_ObjectsAndCount, DssRef.lang.UnitType_CollectionOfArmies, objects.Count), HudLib.TitleColor_TypeName);

            for (int i = 0; i < objects.Count;++i)
            {
                objects[i].GetArmy().toGroupHud(args.content);
                if (i < objects.Count-1)
                {
                    args.content.Add(new RbSeperationLine());
                }
            }
        }

        //public void Tooltip(RichBoxContent content)
        //{
        //    content.Add(new RbText(string.Format(DssRef.lang.Hud_ObjectsAndCount, DssRef.lang.UnitType_CollectionOfArmies, objects.Count), HudLib.TitleColor_TypeName));
        //}
        public override void toTooltip(ObjectHudArgs args)
        {
            if ( CollectionCount() == 1)
            {
                objects.First().toTooltip(args);
            }
            else if (CollectionCount() > 1)
            {
                GroupPresentation(args, true);
            }
        }
        public override GameObjectType gameobjectType()
        {
            return GameObjectType.ObjectCollection;
        }
        public override MapObjectCollection GetMapCollection()
        {
            return this;
        }

        public override bool aliveAndBelongTo(int faction)
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
        


        public void set(List<AbsMapObject> newObjects)
        { 
            this.objects.Clear();
            if (newObjects.Count > 0)
            {
                lib.DoNothing();
            }
            this.objects.AddRange(newObjects);
        }

        public override Vector3 WorldPos()
        {
            Vector3 result = new Vector3();
            for (int i = 0; i < objects.Count; i++)
            {
                result += objects[i].WorldPos();
            }
            return result / objects.Count;
        }


        public override string Name(out bool mayEdit)
        {
            mayEdit = false;


            int armyCount = 0;
            for (int i = objects.Count -1; i >=0; i--)
            {
                if (objects[i].defeated())
                {
                    objects.RemoveAt(i);
                }
                else
                {
                    switch (objects[i].gameobjectType())
                    {
                        case GameObjectType.Army:
                            armyCount++;
                            break;
                    }
                }
            }

            if (objects.Count == 0)
            {
                return DssRef.lang.Hud_EmptyList;
            }
            else if (objects.Count == 1)
            {
                return objects[0].Name(out _);
            }
            else
            {
                return objects[0].Name(out _) + " +" + (armyCount-1).ToString();
            }
        }

        public override int CollectionCount()
        {
            return objects.Count;
        }

        public override string TypeName()
        {
            return DssRef.lang.UnitType_CollectionOfArmies;
        }

    }
}

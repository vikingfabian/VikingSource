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

namespace VikingEngine.DSSWars.GameObject
{
    class DetailObjectCollection: AbsWorldObject
    {
        Faction faction;
        public List<SoldierGroup> objects = new List<SoldierGroup>(8);

        public DetailObjectCollection(Faction faction)
        {
            this.faction = faction;
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            selection.groupModels_detail.BeginGroupModel();

            int selectionIx = 0;
            for (int i = 0; i < objects.Count; i++)
            {
                var soldiers_sp = objects[i].soldiers;
                if (soldiers_sp != null)
                {
                    var soldiersC = soldiers_sp.counter();

                    while (soldiersC.Next())
                    { 
                        soldiersC.sel.selectionFramePlacement(out var pos, out var scale);
                        selection.groupModels_detail.setGroupModel(selectionIx++, pos, scale, hover, true, false);
                    }
                }                
            }
        }

        public override void selectionGui(LocalPlayer player, ImageGroup guiModels)
        {
            //foreach (var obj in objects)
            //{
            //    obj.hoverAndSelectInfo(player, guiModels);
            //}

        }

        public override void toHud(ObjectHudArgs args)
        {

            args.content.h2(string.Format(".Soldier group, count: {0}", objects.Count));

            for (int i = 0; i < objects.Count; ++i)
            {
                objects[i].toGroupHud(args.content);
                if (i < objects.Count - 1)
                {
                    args.content.Add(new RbSeperationLine());
                }
            }
        }

        public void Tooltip(RichBoxContent content)
        {
            content.text(string.Format(".Soldier group, count: {0}", objects.Count));
        }

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.DetailCollection;
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

        public override bool defeated()
        {
            return objects.Count == 0;
        }

        public void set(List<SoldierGroup> newObjects)
        {
            this.objects.Clear();
            //if (newObjects.Count > 0)
            //{
            //    lib.DoNothing();
            //}
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


            int groupCount = 0;
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].defeated())
                {
                    objects.RemoveAt(i);
                }
                else
                {
                    switch (objects[i].gameobjectType())
                    {
                        case GameObjectType.SoldierGroup:
                            groupCount++;
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
                return objects[0].Name(out _) + " +" + (groupCount - 1).ToString();
            }
        }

        public override DetailObjectCollection GetDetailCollection()
        {
            return this;
        }

        public override string TypeName()
        {
            return DssRef.todoLang.Conscript_Soldiers_ArmyType;
        }
    }
}

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.GameObject
{
    class DetailObjectCollection: AbsGOCollection
    {
        //protected Faction faction;
        public List<SoldierGroup> armyGroups = new List<SoldierGroup>(8);
        public List<SoldierGroup> guardGroups = new List<SoldierGroup>(8);

        public DetailObjectCollection(Faction faction)
        {
            this.faction = faction;
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            selection.groupModels_detail.BeginGroupModel();

            int selectionIx = 0;
            list(armyGroups);
            list(guardGroups);


            void list(List<SoldierGroup> objects)
            {
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
        }

        public override void selectionGui(LocalPlayer player, ImageGroup guiModels)
        {
            //foreach (var obj in objects)
            //{
            //    obj.hoverAndSelectInfo(player, guiModels);
            //}

        }


        public override void toTooltip(ObjectHudArgs args)
        {
            if (CollectionCount() == 1)
            {
                first().toTooltip(args);
            }
            else
            {
                if (armyGroups.Count > 0)
                {
                    args.content.Add(new RbImage(SpriteName.WarsArmy));
                    args.content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Conscript_Soldiers_ArmyType, armyGroups.Count)));

                    if (guardGroups.Count > 0)
                    {
                        HudLib.BulletSeperationPoint(args.content);
                    }
                }
                if (guardGroups.Count > 0)
                {
                    args.content.Add(new RbImage(SpriteName.WarsGuard));
                    args.content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Conscript_Soldiers_GuardType, guardGroups.Count)));
                }
            }
        }
        public override void toHud(ObjectHudArgs args)
        {
            GroupPresentation(args, false);
            //args.content.h2(string.Format(DssRef.todoLang.Hud_ObjectsAndCount, DssRef.todoLang.UnitType_CollectionOfSoldiers, objects.Count), HudLib.TitleColor_TypeName);
            list(armyGroups);
            list(guardGroups);

            void list(List<SoldierGroup> objects)
            {
                for (int i = 0; i < objects.Count; ++i)
                {
                    if (objects[i].defeated())
                    {
                        arraylib.RemoveCurrentInForwardLoop(objects, ref i);
                    }
                    else
                    {
                        args.content.newLine();
                        objects[i].toGroupHud(args.content);
                        if (i < objects.Count - 1)
                        {
                            args.content.Add(new RbSeperationLine());
                        }
                    }
                }
            }
        }

        //public void Tooltip(RichBoxContent content)
        //{
           
        //    //content.Add(new RbText( string.Format(DssRef.todoLang.Hud_ObjectsAndCount, DssRef.todoLang.UnitType_CollectionOfSoldiers, objects.Count), HudLib.TitleColor_TypeName));
        //}

        public override GameObjectType gameobjectType()
        {
            return GameObjectType.DetailCollection;
        }
  

        public override bool aliveAndBelongTo(int faction)
        {
            list(armyGroups);
            list(guardGroups);

            void list(List<SoldierGroup> objects)
            {
                for (int i = objects.Count - 1; i >= 0; i--)
                {
                    if (!objects[i].aliveAndBelongTo(faction))
                    {
                        objects.RemoveAt(i);
                    }
                }
            }


            return armyGroups.Count + guardGroups.Count > 0 ;
        }


        public void set(List<SoldierGroup> newObjects)
        {
            armyGroups.Clear();
            guardGroups.Clear();

            foreach (var obj in newObjects)
            {
                if (obj.IsArmyGroup())
                {
                    armyGroups.Add(obj);
                }
                else
                {
                    guardGroups.Add(obj);
                }
            }
        }

        public override Vector3 WorldPos()
        {
            Vector3 result = new Vector3();
            
            for (int i = 0; i < armyGroups.Count; i++)
            {
                result += armyGroups[i].WorldPos();
            }

            for (int i = 0; i < guardGroups.Count; i++)
            {
                result += guardGroups[i].WorldPos();
            }

            return result / (armyGroups.Count + guardGroups.Count);
        }


        public override string Name(out bool mayEdit)
        {
            mayEdit = false;

            var obj = first();
            if (obj != null)
            {
                if (CollectionCount() == 1)
                {
                    return obj.TypeName();
                }
                else
                {
                    return obj.TypeName() + " +" + (CollectionCount() - 1).ToString();
                }
            }
            else
            {
                return DssRef.lang.Hud_EmptyList;
            }
            //    int groupCount = 0;
            //    for (int i = objects.Count - 1; i >= 0; i--)
            //    {
            //        if (objects[i].defeated())
            //        {
            //            objects.RemoveAt(i);
            //        }
            //        else
            //        {
            //            switch (objects[i].gameobjectType())
            //            {
            //                case GameObjectType.SoldierGroup:
            //                    groupCount++;
            //                    break;
            //            }
            //        }
            //    }

            //    if (objects.Count == 0)
            //    {
            //        return DssRef.lang.Hud_EmptyList;
            //    }
            //    else if (objects.Count == 1)
            //    {
            //        return objects[0].Name(out _);
            //    }
            //    else
            //    {
            //        return objects[0].Name(out _) + " +" + (groupCount - 1).ToString();
            //    }
            //}
        }

        public SoldierGroup first()
        {
            if (armyGroups.Count > 0)
            {
                return armyGroups[0];
            }
            else if (guardGroups.Count > 0)
            {
                return guardGroups[0];
            }

            return null;
        }

        public override DetailObjectCollection GetDetailCollection()
        {
            return this;
        }

        public override int CollectionCount()
        {
            return armyGroups.Count + guardGroups.Count;
        }

        public override string TypeName()
        {
            return DssRef.todoLang.UnitType_CollectionOfSoldiers;
        }
    }
}

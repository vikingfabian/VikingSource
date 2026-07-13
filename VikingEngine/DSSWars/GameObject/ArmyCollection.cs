using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.GameObject
{
    class ArmyCollection : AbsGOCollection
    {
        public List<ArmyControlsMember> objects = new List<ArmyControlsMember>(8);

        public ArmyCollection(Army army)
        {
            lock (objects)
            {
                objects.Add(new ArmyControlsMember(army));
            }
        }

        public ArmyCollection(PFaction faction)
        {
            this.pfaction = pfaction;
        }

        public override void selectionFrame(LocalPlayer player, bool hover, Selection selection)
        {
            selection.groupModels_terrian.BeginGroupModel();

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].army.selectionFramePlacement(out var pos, out var scale);
                selection.groupModels_terrian.setGroupModel(i, pos, scale, hover, true, false);
            }
        }

        public override void selectionGui(LocalPlayer player, ImageGroup guiModels)
        {
            foreach (var obj in objects)
            {
                obj.army.hoverAndSelectInfo(player, guiModels);
            }

        }

        public override void toHud(ObjectHudArgs args)
        {
            refreshAlive();
            GroupPresentation(args, false);
            
            for (int i = 0; i < objects.Count;++i)
            {
                Army obj = objects[i].army;

                args.content.newLine();

                args.content.Add(new ArtButton(RbButtonStyle.Outline,
                    new List<AbsRichBoxMember> { new RbImage(SpriteName.ButtonDisabledCross) { color = HudLib.NotAvailableColor } },
                    new RbAction1Arg<AbsMapObject>(removeClick, obj),
                    new RbTooltip_Text(DssRef.lang.Hud_RemoveFromList)));

                args.content.Add(new ArtButton(RbButtonStyle.Outline,
                    new List<AbsRichBoxMember> { new RbImage(SpriteName.ClickCirkleEffect) { color = HudLib.AvailableColor } },
                    new RbAction1Arg<AbsMapObject>(selectClick, obj),
                    new RbTooltip_Text(DssRef.lang.InputActionName_ControllerSelect)));

                obj.GetAbsArmy().toGroupHud(args.content);
                
                args.content.Add(new RbSeperationLine());
                
            }
            //args.content.Add(new RbSeperationLine());
            new ArmyMenu(args.player, this, args.content);
        }

        void removeClick(AbsMapObject obj)
        {
            remove(obj.GetArmy());
            obj.pfaction.GetPlayer().GetLocalPlayer().hud.needRefresh = true;
        }

        void remove(Army army)
        {
            for (int i = 0; i < objects.Count; ++i)
            {
                if (objects[i].army == army)
                {
                    lock (objects)
                    {
                        objects[i].DeleteMe();
                        objects.RemoveAt(i);
                    }
                    return;
                }
            }
        }

       

        void selectClick(AbsMapObject obj)
        {            
            obj.pfaction.GetPlayer().GetLocalPlayer().gameControls.mapSelect(obj);

            DeleteMembers(false);
        }

        public override void toTooltip(ObjectHudArgs args)
        {
            if ( CollectionCount() == 1)
            {
                objects.First().army.toTooltip(args);
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
        public override ArmyCollection GetMapCollection()
        {
            return this;
        }

        void refreshAlive()
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].army.isDeleted)
                {
                    lock (objects)
                    {
                        objects[i].DeleteMe();
                        objects.RemoveAt(i);
                    }
                }
            }
        }

        public override bool aliveAndBelongTo(PFaction faction)
        {
            refreshAlive();

            return objects.Count > 0;
        }
        
        public void set(List<AbsMapObject> newObjects)
        {
            lock (objects)
            {
                this.objects.Clear();

                foreach (AbsMapObject item in newObjects)
                {
                    objects.Add(new ArmyControlsMember(item.GetArmy()));
                }
            }
        }

        public override Vector3 WorldPos()
        {
            Vector3 result = new Vector3();
            for (int i = 0; i < objects.Count; i++)
            {
                result += objects[i].army.WorldPos();
            }
            return result / objects.Count;
        }

        public override string Name(out bool mayEdit)
        {
            mayEdit = false;


            int armyCount = 0;
            for (int i = objects.Count -1; i >=0; i--)
            {
                if (objects[i].army.defeated())
                {
                    lock (objects)
                    {
                        objects.RemoveAt(i);
                    }
                }
                else
                {
                    switch (objects[i].army.gameobjectType())
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
                return objects[0].army.Name(out _);
            }
            else
            {
                return objects[0].army.Name(out _) + " +" + (armyCount-1).ToString();
            }
        }

        public void disbandArmyAction()
        {
            if (objects.Count > 0)
            {
                foreach (var obj in objects)
                {
                    obj.army.disbandArmyAction();
                }

                objects[0].army.pfaction.GetPlayer().GetLocalPlayer().gameControls.clearSelection();

                DeleteMembers(false);
            }
        }
        public void DeleteMembers(bool clear)
        {
            foreach (var m in objects)
            {
                m.DeleteMe();
            }

            if (clear)
            {
                lock (objects)
                {
                    objects.Clear();
                }
            }
        }

        public override bool IsDeleted()
        {
            return objects.Count == 0;
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

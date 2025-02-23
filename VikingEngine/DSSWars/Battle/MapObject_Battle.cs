﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Battle;
using VikingEngine.DSSWars.Display;

namespace VikingEngine.DSSWars.GameObject
{
    partial class AbsMapObject
    {
        public BattleGroup battleGroup = null;
        public IntVector2 battleGridForward;
        public Rotation1D battleDirection;

        public SpottedArray<AbsMapObject> battles = new SpottedArray<AbsMapObject>(4);

        public bool collectBattles_asynchMarker = false;
        static List<AbsMapObject> battlesUnitsBuffer = new List<AbsMapObject>();

        protected void collectBattles_asynch()
        {
            if (battleGroup == null)
            {
                DssRef.world.unitCollAreaGrid.collectMapObjectBattles(faction, tilePos, ref battlesUnitsBuffer, gameobjectType() == GameObjectType.Army);

                foreach (var m in battlesUnitsBuffer)
                {
                    if (!m.isDeleted)
                    {
                        bool inBattle = VectorExt.PlaneXZLength(m.position - position) <= DssLib.BattleConflictRadius;
                        if (inBattle)
                        {
                            if (m.battleGroup != null &&
                                battleGroup == null)
                            {
                                m.battleGroup.addPart(this);
                            }
                            else if (battleGroup != null &&
                                m.battleGroup == null)
                            {
                                battleGroup.addPart(m);
                            }
                            else if (battleGroup == null && m.battleGroup == null)
                            {
                                new BattleGroup(this, m);
                            }
                        }
                    }
                }
            }
        }
        protected void old_collectBattles_asynch()
        {
            //if (objectType() == ObjectType.City && GetCity().index == 346)
            //{ 
            //    lib.DoNothing();
            //}
            //bool collectCities = this.objectType() == ObjectType.Army;
            //Remove completed battles

            if (defeated())
            {
                battles.Clear();
            }
            else
            {

                if (battles.Count > 0)
                {
                    var battlesC = battles.counter();
                    while (battlesC.Next())
                    {
                        battlesC.sel.collectBattles_asynchMarker = false;
                    }
                }


                DssRef.world.unitCollAreaGrid.collectMapObjectBattles(faction, tilePos, ref battlesUnitsBuffer, gameobjectType() == GameObjectType.Army);

                foreach (var m in battlesUnitsBuffer)
                {
                    bool inBattle = VectorExt.PlaneXZLength(m.position - position) <= DssLib.BattleConflictRadius;
                    if (inBattle)
                    {
                        m.collectBattles_asynchMarker = true;

                        battles.AddIfNotExists(m);
                        m.battles.AddIfNotExists(this);
                    }
                }
            }

            if (battles.Count > 0)
            {
                var battlesC = battles.counter();
                while (battlesC.Next())
                {
                    if (battlesC.sel.collectBattles_asynchMarker == false)
                    {
                        battlesC.RemoveAtCurrent();
                        if (battles.Count == 0)
                        {
                            this.ExitBattleGroup();
                        }
                    }
                }
            }
        }

        virtual public void OnBattleJoin(BattleGroup group)
        { 
            this.battleGroup = group;
        }

        public bool InBattle()
        {
            return battleGroup != null;
            //return battles.Count > 0;
        }
        public override void toHud(ObjectHudArgs args)
        {
            base.toHud(args);
            if (args.ShowFull)
            {
#if DEBUG
                debugTagButton(args.content);
                //args.content.Button("debug tag", new HUD.RichBox.RbAction(AddDebugTag), null, true);
#endif

                if (battleGroup != null)
                {
                    args.content.icontext(SpriteName.WarsRelationWar, battleGroup.TypeName()).overrideColor = Microsoft.Xna.Framework.Color.Red;
                }

            }
        }

        //void toggleDebugTag()
        //{
        //    debugTagged = !debugTagged;
        //}
    }
}

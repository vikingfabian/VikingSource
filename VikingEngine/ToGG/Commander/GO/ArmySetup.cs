using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.GO
{
    /// <summary>
    /// The player can buy units to an army setup and save it
    /// </summary>
    class ArmySetup
    {
        public ArmyRace race;
       public int selectedIndex;
       public List<ArmySetupMember> members;

       public ArmySetup(ArmyRace race)
       {
            this.race = race;

           switch (race)
           {
               case ArmyRace.Human:
                   members = new List<ArmySetupMember>()
                        {
                            new ArmySetupMember(UnitType.Human_Spearman, 2),
                            new ArmySetupMember(UnitType.Human_HeavySpearman, 2),
                            new ArmySetupMember(UnitType.Human_LongBow, 2),
                            new ArmySetupMember(UnitType.Human_ShortBow, 2),
                            new ArmySetupMember(UnitType.Human_Cavalry, 2),
                            new ArmySetupMember(UnitType.Human_HeavyCavalry, 1),
                            //new ArmySetupMember(UnitType.Human_Chariot, 1),
                            //new ArmySetupMember(UnitType.Human_Hero, 1),
                            new ArmySetupMember(UnitType.TacticalBase, 1),
                        };
                   break;
               case ArmyRace.Undead:
                   members = new List<ArmySetupMember>()
                        {
                            new ArmySetupMember(UnitType.Undead_Spearman, 2),
                            new ArmySetupMember(UnitType.Undead_Warrior, 2),
                            new ArmySetupMember(UnitType.Undead_HeavyBeast, 2),
                            //new ArmySetupMember(UnitType.Undead_LongBow, 2),
                            new ArmySetupMember(UnitType.Undead_ShortBow, 3),
                            new ArmySetupMember(UnitType.Undead_Cavalry, 1),
                            new ArmySetupMember(UnitType.Undead_HeavyCavalry, 1),
                            //new ArmySetupMember(UnitType.Undead_Hero, 1),
                            new ArmySetupMember(UnitType.TacticalBase, 1),
                        };
                   break;
               case ArmyRace.Elf:
                   members = new List<ArmySetupMember>()
                        {
                            //new ArmySetupMember(UnitType.Elf_Spearman, 2),
                            new ArmySetupMember(UnitType.Elf_Faun, 2),
                            new ArmySetupMember(UnitType.Elf_Wardacer, 2),
                            new ArmySetupMember(UnitType.Elf_LongBow, 2),
                            new ArmySetupMember(UnitType.Elf_ShortBow, 2),
                            new ArmySetupMember(UnitType.Elf_Cavalry, 2),
                            new ArmySetupMember(UnitType.Elf_HeavyCavalry, 1),
                            //new ArmySetupMember(UnitType.Elf_Hero, 1),
                            new ArmySetupMember(UnitType.TacticalBase, 1),
                        };
                   break;
               case ArmyRace.Orc:
                   members = new List<ArmySetupMember>()
                        {
                            new ArmySetupMember(UnitType.Orc_Spearman, 3),
                            new ArmySetupMember(UnitType.Orc_LightInfantery, 3),
                            new ArmySetupMember(UnitType.Orc_HeavyInfantery, 2),
                            new ArmySetupMember(UnitType.Orc_ShortBow, 4),
                            //new ArmySetupMember(UnitType.Orc_ShortBow, 2),
                            new ArmySetupMember(UnitType.Orc_Cavalry, 2),
                            new ArmySetupMember(UnitType.Orc_HeavyCavalry, 1),
                            //new ArmySetupMember(UnitType.Orc_Hero, 1),
                            new ArmySetupMember(UnitType.TacticalBase, 1),
                        };
                   break;

           }
       }


        public ArmySetupMember Selected
        {
            get
            {
                if (selectedIndex >= 0 && selectedIndex < members.Count)
                {
                    return members[selectedIndex];
                }
                return null;
            }
            set
            {
                for (int i = 0; i < members.Count; ++i)
                {
                    if (members[i].type == value.type)
                    {
                        selectedIndex = i;
                        return;
                    }
                }

                Unselect();
            }
        }

        

        //public List<CardMenuButton> cardMenuFile()
        //{
        //    List<CardMenuButton> buttons = new List<CardMenuButton>(members.Count);

        //    foreach (ArmySetupMember u in members)
        //    {
        //        if (u.LeftToPlace > 0)
        //        {
        //            CardMenuButton button = new CardMenuButton(HudLib.cardSize, HudLib.BgLayer);
        //            UnitData unit = u.GetData();

        //            unit.Card(button, u.LeftToPlace);

        //            buttons.Add(button);
        //        }
        //    }

        //    return buttons;
        //}

        

        //public ArmySetupMember Select(int index)
        //{
        //    selectedIndex = index;
        //    return Selected;
        //}

        public void OnPlaceUnit(AbsUnit previousUnit)
        {
            if (previousUnit != null)
            {
                foreach (ArmySetupMember m in members)
                {
                    if (m.type == previousUnit.cmd().data.Type)
                    {
                        m.Placed--;
                        break;
                    }
                }
                previousUnit.DeleteMe();
            }

            if (Selected != null)
            {
                Selected.Placed++;
                if (Selected.LeftToPlace <= 0)
                {
                    if (!NextMember())
                        Unselect();
                }
            }
            else
            {
                lib.DoNothing();
            }
        }

        public void RemoveUnit(AbsUnit u)
        {
            for (int i = 0; i < members.Count; ++i)//each (ArmySetupMember m in members)
            {
                if (members[i].type == u.cmd().data.Type)
                {
                    members[i].Placed--;
                    if (Selected == null)
                        selectedIndex = i;
                    return;
                }
            }
        }

        void addUnit(UnitType type)
        {
            //var iType = type;
            foreach (ArmySetupMember m in members)
            {
                if (m.type == type)
                {
                    m.Count++;
                    return;
                }
            }

            members.Add(new ArmySetupMember(type, 1));
        }

        /// <returns>Could move to next</returns>
        public bool NextMember()
        {
            int prev = selectedIndex;
            nextMember(selectedIndex, 1);
            return prev != selectedIndex;
        }
        public void PreviousMember()
        {
            nextMember(selectedIndex, -1);
        }

        void nextMember(int startIndex, int dir)
        {
            selectedIndex = Bound.SetRollover(selectedIndex + dir, 0, members.Count -1);
            if (selectedIndex == startIndex)
            {
                selectedIndex = startIndex;
                return;
            }

            if (Selected.LeftToPlace > 0)
            {
                return;
            }
            else
            {
                nextMember(startIndex, dir);
            }
        }

        public void Unselect()
        {
            selectedIndex = -1;
        }

        public bool Complete()
        {
            foreach (ArmySetupMember u in members)
            {
                if (u.LeftToPlace > 0)
                    return false;
            }
            return true;
        }
    }

    class ArmySetupMember
    {
        public UnitType type;
        public int Count, Placed;

        public ArmySetupMember(UnitType type, int count)
        {
            this.type = type;
            this.Count = count;
        }

        public int LeftToPlace
        {
            get { return Count - Placed; }
        }

        public AbsUnitData GetData()
        {
            return cmdRef.units.GetUnit(type);
        }

        public override string ToString()
        {
            return "Army setup: " + type.ToString() + " " + Placed.ToString() + "/" + Count.ToString();
        }
    }
}

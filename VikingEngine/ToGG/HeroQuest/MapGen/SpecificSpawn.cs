using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    class SpecificSpawn : AbsRoomFlagSettings
    {
        public List<HqUnitType> monsters = new List<HqUnitType>();

        public override void toEditorSettings(GuiLayout layout)
        {
            base.toEditorSettings(layout);

            new GuiTextButton("+Add monster", null, listGroupsToAdd, true, layout);
            if (monsters.Count > 0)
            {
                new GuiSectionSeparator(layout);

                new GuiLabel("Monsters: Click to remove", layout);
                for (int i = 0; i < monsters.Count; ++i)
                {
                    new GuiTextButton("-" + monsters[i].ToString(), null,
                        new GuiActionIndex(removeGroupLink, i), false, layout);
                }
            }
        }

        void listGroupsToAdd()
        {
            GuiLayout layout = new GuiLayout("Units", toggRef.menu.menu, GuiLayoutMode.MultipleColumns, null);
            {
                foreach (var utype in HeroQuest.Data.AllUnitsData.EditorReadyUnits)
                {
                    var data = hqRef.unitsdata.Get(utype);
                    new GuiBigIcon(data.modelSettings.image, data.Name, new GuiAction1Arg<HqUnitType>(addGroupLink, utype), true, layout);
                }
            }
            layout.End();
        }

        void addGroupLink(HqUnitType group)
        {
            monsters.Add(group);
            settingsToMenu();
        }

        public override void getSpawnGroups(PcgRandom rnd, int count, 
            ref MonsterGroupType previousMonsterGroup, List<MonsterGroupType> result)
        {
            //do nothing
        }

        void removeGroupLink(int index)
        {
            monsters.RemoveAt(index);
            settingsToMenu();
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);

            w.Write((byte)monsters.Count);
            foreach (var m in monsters)
            {
                w.Write((ushort)m);
            }
        }

        public override void Read(BinaryReader r, FileVersion version)
        {
            base.Read(r, version);

            monsters.Clear();
            int groupCount = r.ReadByte();
            for (int i = 0; i < groupCount; ++i)
            {
                monsters.Add((HqUnitType)r.ReadInt16());
            }
        }

        public override AbsRoomFlagSettings Clone()
        {
            SpecificSpawn clone = new SpecificSpawn();
            copyData(clone);
            clone.monsters.AddRange(this.monsters);

            return clone;
        }

        public override SpawnSettingsType Type => SpawnSettingsType.Specific;

        public override string ShortName => "SP:" + monsters.Count.ToString();
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    class MonsterGroupSpawn : AbsRoomFlagSettings
    {
        public bool isBossRoom = false;
        public List<MonsterGroupType> groups = new List<MonsterGroupType>();

        public override void toEditorSettings(GuiLayout layout)
        {
            base.toEditorSettings(layout);

            //new GuiCheckbox("Boss room", null, bossProperty, layout);
            new GuiTextButton("+Add monster group", null, listGroupsToAdd, true, layout);
            if (groups.Count > 0)
            {
                new GuiSectionSeparator(layout);

                new GuiLabel("Groups: Click to remove", layout);
                for (int i = 0; i < groups.Count; ++i)
                {
                    new GuiTextButton("-" + groups[i].ToString(), null,
                        new GuiActionIndex(removeGroupLink, i), false, layout);
                }
            }
        }

        bool bossProperty(int index, bool set, bool value)
        {
            if (set) isBossRoom = value;
            return isBossRoom;
        }

        void listGroupsToAdd()
        {
            GuiLayout layout = new GuiLayout("Add", toggRef.menu.menu);
            {
                MonsterGroupType[] available = new MonsterGroupType[]
                {
                    MonsterGroupType.Relaxed,
                    MonsterGroupType.Goblins,
                    MonsterGroupType.OrcsAndGoblins,
                    MonsterGroupType.Skeletons,
                    MonsterGroupType.Spiders,
                    MonsterGroupType.LizardsAndSnakes,
                    MonsterGroupType.Beasts,
                };

                foreach (var m in available)
                {
                    if (!groups.Contains(m))
                    {
                        new GuiTextButton(m.ToString(), null, new GuiAction1Arg<MonsterGroupType>(addGroupLink, m),
                            false, layout);
                    }
                }
            }
            layout.End();
        }

        public override void getSpawnGroups(PcgRandom rnd, int count,
            ref MonsterGroupType previousMonsterGroup, List<MonsterGroupType> result)
        {
            for (int i = 0; i < count; ++i)
            {
                int trials = FindDifferentGroupTrials;

                MonsterGroupType group = MonsterGroupType.NUM_NONE;
                do
                {
                    group = arraylib.RandomListMember(groups, rnd);
                    --trials;

                } while (trials > 0 && group == previousMonsterGroup);

                previousMonsterGroup = group;
                result.Add(group);
            }
        }

        void addGroupLink(MonsterGroupType group)
        {
            groups.Add(group);
            settingsToMenu();
        }

        void removeGroupLink(int index)
        {
            groups.RemoveAt(index);
            settingsToMenu();
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);

            w.Write(isBossRoom);

            w.Write((byte)groups.Count);
            foreach (var m in groups)
            {
                w.Write((byte)m);
            }
        }

        public override void Read(BinaryReader r, FileVersion version)
        {
            base.Read(r, version);

            isBossRoom = r.ReadBoolean();

            groups.Clear();
            int groupCount = r.ReadByte();
            for (int i = 0; i < groupCount; ++i)
            {
                groups.Add((MonsterGroupType)r.ReadByte());
            }
        }

        public override AbsRoomFlagSettings Clone()
        {
            MonsterGroupSpawn clone = new MonsterGroupSpawn();
            copyData(clone);
            clone.isBossRoom = this.isBossRoom;
            clone.groups.AddRange(this.groups);

            return clone;
        }

        public override SpawnSettingsType Type => SpawnSettingsType.Group;

        public override string ShortName => "GP:" + groups.Count.ToString();
    }
}

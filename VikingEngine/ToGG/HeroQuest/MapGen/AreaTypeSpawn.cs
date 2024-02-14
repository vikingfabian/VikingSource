using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    class AreaTypeSpawn : AbsRoomFlagSettings
    {
        public AreaType area = AreaType.Default;

        public override void toEditorSettings(GuiLayout layout)
        {
            base.toEditorSettings(layout);

            var areaOptions = new List<GuiOption<AreaType>>();
            for (AreaType ar = 0; ar < AreaType.NUM; ++ar)
            {
                areaOptions.Add(new GuiOption<AreaType>(ar));
            }

            new GuiOptionsList<AreaType>(SpriteName.NO_IMAGE, "Area", areaOptions, areaProperty, layout);
        }

        AreaType areaProperty(bool set, AreaType value)
        {
            if (set)
            {
                area = value;
            }
            return area;
        }

        public override void getSpawnGroups(PcgRandom rnd, int count,
            ref MonsterGroupType previousMonsterGroup, List<MonsterGroupType> result)
        {
            int tier = GetTier(rnd);
            var areaSetup = MapSpawnLib.AreaSetups[(int)area];
            MonsterGroupType[] array = areaSetup.Get(tier);

            for (int i = 0; i < count; ++i)
            {
                int trials = FindDifferentGroupTrials;

                MonsterGroupType group = MonsterGroupType.NUM_NONE;
                do
                {
                    group = arraylib.RandomListMember(array, rnd);
                    --trials;

                } while (trials > 0 && group == previousMonsterGroup);

                previousMonsterGroup = group;
                result.Add(group);
            }
        }

        public override void Write(BinaryWriter w)
        {
            base.Write(w);
            w.Write((byte)area);
        }

        public override void Read(BinaryReader r, FileVersion version)
        {
            base.Read(r, version);
            area = (AreaType)r.ReadByte();
        }

        public override AbsRoomFlagSettings Clone()
        {
            AreaTypeSpawn clone = new AreaTypeSpawn();
            copyData(clone);
            clone.area = this.area;

            return clone;
        }

        public override SpawnSettingsType Type => SpawnSettingsType.Area;

        public override string ShortName => "AR:" + ((int)area).ToString();
    }
}

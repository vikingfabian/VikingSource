using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DataStream;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.HeroQuest.MapGen
{
    abstract class AbsRoomFlagSettings
    {
        protected const int FindDifferentGroupTrials = 20;
        public SpawnDifficulty difficulty = SpawnDifficulty.Medium;
        public int spawnChance = 100;
        public int maxTileRadius = MapSpawnLib.FlagDefaultRadius;
        public float spawnCountMultiply = 1;

        virtual public void toEditorSettings(GuiLayout layout)
        {
            bool isSpecific = this is SpecificSpawn;

            new GuiLabel("Room flag type: " + Type.ToString(), layout);
            new GuiTextButton("Change type", null, listTypes, true, layout);

            if (!isSpecific)
            {
                var difficultyOptions = new List<GuiOption<SpawnDifficulty>>();
                {
                    for (SpawnDifficulty dif = 0; dif < SpawnDifficulty.NUM; ++dif)
                    {
                        difficultyOptions.Add(new GuiOption<SpawnDifficulty>(dif));
                    }

                    new GuiOptionsList<SpawnDifficulty>(SpriteName.NO_IMAGE, "Difficulty",
                        difficultyOptions, difficultyProperty, layout);
                }
            }

            new GuiIntSlider(SpriteName.NO_IMAGE, "Max radius",
                maxRadiusProperty, new IntervalF(1, 40), false, layout);
            new GuiIntSlider(SpriteName.NO_IMAGE, "Spawn Chance (%)",
                spawnChanceProperty, new IntervalF(0, 100), false, layout);

            if (!isSpecific)
            {
                new GuiFloatSlider(SpriteName.NO_IMAGE, "Spawns per room size",
                spawnCountMultiplyProperty, new IntervalF(0.1f, 8), false, layout);
            }
        }

        public void settingsToMenu()
        {
            toggRef.menu.menu.PopAllLayouts();
            GuiLayout layout = new GuiLayout("Room flag settings", toggRef.menu.menu);
            {
                toEditorSettings(layout);

                new GuiSectionSeparator(layout);
                new GuiTextButton("OK", null, toggRef.menu.CloseMenu, false, layout);
            }
            layout.End();
        }

        public float spawnCountMultiplyProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref spawnCountMultiply, value);
        }

        SpawnDifficulty difficultyProperty(bool set, SpawnDifficulty value)
        {
            if (set)
            {
                difficulty = value;
            }
            return difficulty;
        }

        int spawnChanceProperty(bool set, int value)
        {
            if (set)
            {
                spawnChance = value;
            }
            return spawnChance;
        }

        int maxRadiusProperty(bool set, int value)
        {
            if (set)
            {
                maxTileRadius = value;
            }
            return maxTileRadius;
        }

        public void listTypes()
        {
            GuiLayout layout = new GuiLayout("Spawn Type", toggRef.menu.menu);
            {
                new GuiTextButton("Area", "Will spawn enemies that fit the environmenet",
                    new GuiAction1Arg<SpawnSettingsType>(changeType, SpawnSettingsType.Area), false, layout);
                new GuiTextButton("Groups", "Spawn selected enemy groups",
                    new GuiAction1Arg<SpawnSettingsType>(changeType, SpawnSettingsType.Group), false, layout);
                new GuiTextButton("Specific", "The monsters you choose",
                    new GuiAction1Arg<SpawnSettingsType>(changeType, SpawnSettingsType.Specific), false, layout);
            }
            layout.End();
        }

        void changeType(SpawnSettingsType newType)
        {
            if (newType != this.Type)
            {
                AbsRoomFlagSettings settings = Create(newType);

                toggRef.editor.roomFlag = settings;
                settings.settingsToMenu();
            }
        }

        public static AbsRoomFlagSettings Create(SpawnSettingsType type)
        {
            AbsRoomFlagSettings settings;

            switch (type)
            {
                case SpawnSettingsType.Area:
                    settings = new AreaTypeSpawn();
                    break;
                case SpawnSettingsType.Group:
                    settings = new MonsterGroupSpawn();
                    break;
                case SpawnSettingsType.Specific:
                    settings = new SpecificSpawn();
                    break;

                default:
                    throw new NotImplementedException();
            }

            return settings;
        }

        abstract public AbsRoomFlagSettings Clone();

        protected void copyData(AbsRoomFlagSettings clone)
        {
            clone.difficulty = this.difficulty;
            clone.spawnChance = this.spawnChance;
            clone.maxTileRadius = this.maxTileRadius;
            clone.spawnCountMultiply = this.spawnCountMultiply;
        }


        virtual public void Write(System.IO.BinaryWriter w)
        {
            w.Write((byte)difficulty);
            w.Write((byte)maxTileRadius);
            w.Write((byte)spawnChance);
            w.Write(spawnCountMultiply);
        }
        virtual public void Read(System.IO.BinaryReader r, FileVersion version)
        {
            difficulty = (SpawnDifficulty)r.ReadByte();
            maxTileRadius = r.ReadByte();
            spawnChance = r.ReadByte();
            spawnCountMultiply = r.ReadSingle();
            //spawnCountMultiply = 1f;
        }

        abstract public SpawnSettingsType Type { get; }

        abstract public string ShortName { get; }

        abstract public void getSpawnGroups(PcgRandom rnd, int count, ref MonsterGroupType previousMonsterGroup, List<MonsterGroupType> result);

        public int GetTier(PcgRandom rnd)
        {
            switch (difficulty)
            {
                case SpawnDifficulty.Random:
                    if (rnd.Chance(0.5))
                    {
                        return 1;
                    }
                    else
                    {
                        return rnd.Int(3);
                    }

                case SpawnDifficulty.VeryLow:
                    return 0;

                case SpawnDifficulty.Low:
                    if (rnd.Chance(0.9))
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }

                case SpawnDifficulty.Medium:
                    double rndVal = rnd.Double();

                    if (rndVal < 0.15)
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                    

                case SpawnDifficulty.Hard:
                    if (rnd.Chance(0.7))
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }

                case SpawnDifficulty.VeryHard:
                    return 2;

                default:
                    throw new NotImplementedException();
            }
        }

        public float DifficultyValue(PcgRandom rnd)
        {
            switch (difficulty)
            {
                case SpawnDifficulty.Random:
                    return 1f + rnd.Plus_MinusF(0.5f);

                case SpawnDifficulty.VeryLow: return 0.4f;
                case SpawnDifficulty.Low: return 0.7f;
                case SpawnDifficulty.Medium: return 1f;
                case SpawnDifficulty.Hard: return 1.2f;
                case SpawnDifficulty.VeryHard: return 1.4f;

                default:
                    throw new NotImplementedException();
            }
        }
    }


    enum SpawnSettingsType
    {
        //None,
        Area,
        Group,
        Specific,
    }

    enum SpawnDifficulty
    {
        Random,
        VeryLow,
        Low,
        Medium,
        Hard,
        VeryHard,
        NUM
    }

    enum AreaType
    {
        Default,
        SmallCave,
        GoblinForest,
        //Forest,
        NUM
    }

    enum MonsterGroupType
    {
        Relaxed,
        Goblins,
        GoblinRobbers,
        OrcsAndGoblins,
        OrcGuards,
        WolfRiders,
        Skeletons,
        Spiders,
        LizardsAndSnakes,
        Beasts,
        NUM_NONE
    }

    enum LootSpawnLevel
    {
        None,
        Low,
        Medium,
        High,
        VeryHigh,
        Random,
    }
}

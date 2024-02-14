using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Data.Characters
{
    struct QuestTip
    {
        public QuestTipType Type;
        public int Level;

        public QuestTip(QuestTipType type, int lvl)
        {
            Type = type;
            Level = lvl;
        }
        public QuestTip(System.IO.BinaryReader r)
            : this()
        {
            Read(r);
        }

        public void Write(System.IO.BinaryWriter w)
        {
            TwoHalfByte type_lvl = new TwoHalfByte((byte)Type, (byte)Level);
            type_lvl.WriteStream(w);
        }

        public void Read(System.IO.BinaryReader r)
        {
            TwoHalfByte type_lvl = TwoHalfByte.FromStream(r);
            Type = (QuestTipType)type_lvl.Value1;
            Level = type_lvl.Value2;
        }
        public void Activate(LF2.GameObjects.Characters.Hero hero, Object speaker)
        {
#if !CMODE
            string text = null;
            bool getMapLocation = false;
            BossKnowledge bk = new BossKnowledge();
            IntVector2 maplocation = IntVector2.Zero;
            switch (Type)
            {
                case QuestTipType.BossImmunity:
                    bk.Immunity = true;
                    LfRef.gamestate.Progress.UnlockBossInfo(bk, Level);
                    text = TextLib.LargeFirstLetter(magicianName()) + " is rumored to be immune to " + LfRef.worldOverView.Boss(Level).Immune.ToString();
                    break;
                case QuestTipType.BossWeakness:
                    bk.Weakness = true;
                    LfRef.gamestate.Progress.UnlockBossInfo(bk, Level);
                    text = "I heard that " + magicianName() + "'s weakness is " +  LfRef.worldOverView.Boss(Level).Weakness.ToString();
                    break;
                case QuestTipType.BossLocation:
                    bk.Location = true;
                    LfRef.gamestate.Progress.UnlockBossInfo(bk, Level);
                    text = "People are claiming that " + LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Castle][Level].Name +" is ruled by " + magicianName();
                    getMapLocation = true;
                    maplocation = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Castle][Level].position;
                    break;
                case QuestTipType.Nest:
                    text = "Monsters seem to come from this area:";
                    getMapLocation = true;
                    maplocation = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.EnemySpawn][Level].position;
                    break;
                case QuestTipType.Village:
                    text = "The people at the" + LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Village][Level].Name + " might have heard something";
                    getMapLocation = true;
                    maplocation = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Village][Level].position;
                    break;
                case QuestTipType.City:
                    text = "News always travels to the " + LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.Village][Level].Name;
                    getMapLocation = true;
                    maplocation = LfRef.worldOverView.AreasInfo[Map.Terrain.AreaType.City][Level].position;
                    break;

            }
            List<IQuestDialoguePart> say = new List<IQuestDialoguePart>
            {
                new QuestDialogueSpeach(text, LoadedSound.Dialogue_DidYouKnow),
            };

            if (getMapLocation)
            {
                LfRef.gamestate.Progress.SetVisitedArea(maplocation, null, true);
                say.Add(new QuestDialogueMapLocationMessage());
                hero.Player.NewMapLocation(maplocation);
            }

            new QuestDialogue(
              say, speaker, hero.Player);
#endif
        }

        string magicianName()
        {
            string text = null;
            switch (Level)
            {
                case 0:
                    text = "first";
                    break;
                case 1:
                    text = "second";
                    break;
                case 2:
                    text = "third";
                    break;
                case 3:
                    text = "fourth";
                    break;
                case 4:
                    text = "fifth";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return "the " + text + " magician";
        }
    }
    enum QuestTipType
    {
        NON,
        Nest,
        Village,
        City,
        BossLocation,
        BossWeakness,
        BossImmunity,
    }
}

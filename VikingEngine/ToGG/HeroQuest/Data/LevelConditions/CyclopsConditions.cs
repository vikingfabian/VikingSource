using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.ToGG.HeroQuest.Data.LevelConditions
{
    class CyclopsConditions: DefaultLevelConditions
    {
        public CyclopsConditions()
        {
            doom = new DoomData(12);
            doom.goldChest = doom.TotalSkullCount - 2;
            doom.silverChest = doom.goldChest - 4;
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            missionObjectivesTitle(rb);

            rb.Add(new RbText("Defeat the Cyclops boss"));

            return rb;
        }

        override public bool EnemyLootDrop => false;
    }
}

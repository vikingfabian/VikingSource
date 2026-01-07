using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.HeroQuest.MapGen;
using VikingEngine.ToGG.HeroQuest.Players.Ai;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class NagaBossConditions : DefaultLevelConditions
    {
        public NagaBossConditions()
        {
            doom = new DoomData(16);
            doom.goldChest = doom.TotalSkullCount - 3;
            doom.silverChest = doom.goldChest - 5;
        }

        override public List<AbsRichBoxMember> questDescription()
        {
            List<AbsRichBoxMember> rb = new List<AbsRichBoxMember>();
            missionObjectivesTitle(rb);

            rb.Add(new RbText("Defeat the Naga boss"));

            return rb;
        }

        public override void environmentSpawn(SpawnManager spawnManager)
        {
            var levers = toggRef.board.metaData.tags.list(1);

            int rndLever = spawnManager.rnd.Int(levers.Count);

            for (int i = 0; i < levers.Count; ++i)
            {
                new Lever(levers[i], rndLever == i ? 1 : 0);
            }
        }

        override public bool EnemyLootDrop => true;
    }
}

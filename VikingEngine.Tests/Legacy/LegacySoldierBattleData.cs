using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.GameObject;

namespace VikingEngine.Tests.Legacy
{
    /// <summary>
    /// Snapshot of the legacy unpooled SoldierBattleData before Phase 3 object pooling.
    /// Allocates a new instance + List<AbsSoldierUnit>(8) on every battle state entry.
    /// </summary>
    class LegacySoldierBattleData
    {
        public List<AbsSoldierUnit> nearBodyCollisionUnits = new List<AbsSoldierUnit>(8);
        public float queueTime = 0;
        public int maxBlock;
        public int blocks;
        public GameTimeStamp lastBlockTime;
        public Vector2 collisionForce = Vector2.Zero;

        public LegacySoldierBattleData(int maxBlocks)
        {
            maxBlock = maxBlocks;
            blocks = maxBlock;
            lastBlockTime = GameTimeStamp.Now();
        }
    }
}

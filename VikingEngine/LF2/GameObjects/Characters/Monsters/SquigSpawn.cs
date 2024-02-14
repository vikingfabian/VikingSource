using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.Characters.Monsters
{
    class SquigSpawn : Squig
    {
        static readonly IntervalF ScaleRange = new IntervalF(1.4f, 1.8f);
        protected override IntervalF scaleRange
        {
            get { return ScaleRange; }
        }

        public SquigSpawn(Map.WorldPosition startPos)
            : base(startPos, Ref.rnd.RandomChance(70)? 0 : 1)
        {
            Health = LootfestLib.SquigSpawnHealth;
        }
        public SquigSpawn(System.IO.BinaryReader r)
            : base(r)
        {
            //Health = LootfestLib.SquigSpawnHealth;
        }
        override protected void createProjectile(Vector3 pos)
        {
            new WeaponAttack.SquigBullet(pos, rotation, WeaponAttack.WeaponUtype.SquigSpawnBullet);
        }
        public override CharacterUtype CharacterType
        {
            get { return CharacterUtype.SquigSpawn; }
        }
        protected override Monster2Type monsterType
        {
            get { return Monster2Type.SquigSpawn; }
        }

        const float WalkingSpeed = 0.009f;
        protected override float walkingSpeed
        {
            get { return WalkingSpeed; }
        }
        protected override float runningSpeed
        {
            get { return WalkingSpeed; }
        }
    }
}

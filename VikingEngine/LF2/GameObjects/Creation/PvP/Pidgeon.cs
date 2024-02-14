using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Creation.PvP
{
//    #if CMODE
//    class Pidgeon :GameObjects.Characters.AbsEnemy
//    {
//        const float Size = 0.08f;
//        static readonly GameObjects.LootFest.AbsObjBound Bound = GameObjects.LootFest.AbsObjBound.QuickBoundingBox(6 * Size);

//        public Pidgeon(Vector3 heroPos, bool blueTeam)
//            :base()
//        {
//            heroPos.Y += 2;
//            basicInit(heroPos);
//           // image.Position = heroPos;

//            Vector2 dir = Vector2.One;
//            King goal =  LfRef.gamestate.FatKing.GetKing(!blueTeam);
//            if (goal != null)
//            {
//                dir = PositionDiff(goal);
                
//            }
//            dir.Normalize();
//            Speed = dir * 0.02f;
//            moveImage(Speed, 100);
//            NetworkShare();
//        }
//        public Pidgeon(System.IO.BinaryReader packetReader)
//            : base(packetReader)
//        {
//            basicInit(Vector3.Zero);
//        }
//        void basicInit(Vector3 pos)
//        {
//            ImageSetup(VoxelModelName.Pigeon, pos, lib.V3(Size));
//            CollisionBound = Bound;
//        }

//        //public override void  HandleColl3D(LootFest.CollisionIntersection3D collData)
//        //{
 	
//        //    TakeDamage(new GameObjects.Weapons.DamageData(20, GameObjects.Weapons.WeaponTargetType.NON, 0), true);
//        //}

//        static readonly GameObjects.Weapons.DamageData ContactDamage = new GameObjects.Weapons.DamageData(2, GameObjects.Weapons.WeaponTargetType.Enemy, 0);
//        protected override GameObjects.Weapons.DamageData contactDamage
//        {
//            get
//            {
//                return ContactDamage;
//            }
//        }
//        protected override void moveImage(Vector2 speed, float time)
//        {
//            image.Position.X += speed.X * time;
//            image.Position.Z += speed.Y * time;
//            image.Position.Y -= 0.001f * time;
//            if (image.Position.Y <= 1)
//            {
//                DeleteMe();
//            }
//        }
//        public override Data.Characters.EnemyType EnemyType
//        {
//            get { return Data.Characters.EnemyType.Pigeon; }
//        }
//        protected override LoadedSound HurtSound
//        {
//            get
//            {
//                return LoadedSound.rc_explode;
//            }
//        }
//        public override GameObjects.Characters.CharacterUtype CharacterType
//        {
//            get { return GameObjects.Characters.CharacterUtype.NPC; }
//        }
//    }
//#endif
}

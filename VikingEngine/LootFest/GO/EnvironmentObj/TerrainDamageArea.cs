using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class TerrainDamageArea : AbsNoImageObj
    {
        public TerrainDamageArea(Map.WorldVolume volume)
            :base(new GoArgs(volume.Position))
        {
            WorldPos = volume.Position;
            VectorVolume vol = volume.VolumeF;
            CollisionAndDefaultBound = new GO.Bounds.ObjectBound(new BoundData2(new VikingEngine.Physics.StaticBoxBound(volume.VolumeF), Vector3.Zero));
           
        }

        public override void Time_Update(UpdateArgs args)
        {
            if (checkOutsideUpdateArea_ActiveChunk())
            {
                DeleteMe();
            }
            //base.Time_Update(args);
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            checkHeroCollision(true, false, null);
            //checkCharacterCollision(args.localMembersCounter); 
        }


        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            new Process.UnthreadedDamage(new WeaponAttack.DamageData(LfLib.BasicDamage, WeaponAttack.WeaponUserType.Neutral, NetworkId.Empty, Magic.MagicElement.NoMagic, WeaponAttack.SpecialDamage.TerrainDamageFloor, false), character);
            //base.handleCharacterColl(character, collisionData);
            return false;
        }

        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.Neutral;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.TerrainDamageArea; }
        }
    }
}

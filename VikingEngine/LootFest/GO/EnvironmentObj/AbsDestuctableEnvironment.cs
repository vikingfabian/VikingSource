using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Characters;


namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    abstract class AbsDestuctableEnvironment : AbsGameObject
    {
        GO.NetworkShare netShare = new NetworkShare(true, true, false, false);

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            WritePosition(image.position, w);
        }
        public AbsDestuctableEnvironment(GoArgs args)
            : base(args)
        {
            if (!args.LocalMember)
            {
                image.position = args.startPos;//ReadPosition(args.reader);
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            immortalityTime.CountDown();
            if (managedGameObject)
            {
                checkSleepingState();
            }
            else
            {
                if (checkOutsideUpdateArea_ClosestHero())
                {
                    this.DeleteMe();
                }
            }
        }
        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            return base.willReceiveDamage(damage);
        }
        
        public override bool SolidBody
        {
            get
            {
                return false;
            }
        }
        public override void DeleteMe(bool local)
        {
            if (local && Health <= 0 && !localMember)
            { //only share if killed by client
                netShare.DeleteByClient = true;
            }
            base.DeleteMe(local);
        }
        public override void AsynchGOUpdate(UpdateArgs args)
        {
            //do nothing
        }
        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
        {
            // base.HandleColl3D(collData, ObjCollision);
        }

        public override ObjPhysicsType PhysicsType
        {
            get
            {
                return ObjPhysicsType.NO_PHYSICS;
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
        
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.PassiveEnemy;
            }
        }
        public override bool IsWeaponTarget
        {
            get
            {
                return true;
            }
        }
        protected override RecieveDamageType recieveDamageType
        {
            get {  return RecieveDamageType.ReceiveDamage; }
        }
    }
}

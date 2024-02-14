using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Characters;


namespace VikingEngine.LF2.GameObjects.EnvironmentObj
{
    abstract class AbsDestuctableEnvironment : AbsGameObject
    {
        GameObjects.NetworkShare netShare = new NetworkShare(true, true, false, false);

        public AbsDestuctableEnvironment(int areaLevel)
        {
            
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(image.position, w);
        }
        public AbsDestuctableEnvironment(System.IO.BinaryReader r)
            : base(r)
        {
            image.position = ReadPosition(r);
        }

        public override void Time_Update(UpdateArgs args)
        {
            //base.Time_Update(args);
            immortalityTime.CountDown();
            updateHealthBar();
            if (checkOutsideUpdateArea_ClosestHero())
            {
                this.DeleteMe();
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
        public override void AIupdate(UpdateArgs args)
        {
            //do nothing
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            // base.HandleColl3D(collData, ObjCollision);
        }
        protected override bool ViewHealthBar
        {
            get
            {
                return true;
            }
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
        public override ObjectType Type
        {
            get
            {
                return ObjectType.EnvironmentObj;
            }
        }
        public override int UnderType
        {
            get
            {
                return (int)environmentType;
            }
        }
        abstract protected EnvironmentType environmentType { get; }

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

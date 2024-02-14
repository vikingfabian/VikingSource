using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects
{
    abstract class AbsUpdateObj : IDeleteable, ISpottedArrayMember, ILightSource
    {
        protected AbsPhysics physics;
        //NETWORK
        public ByteVector2 ObjOwnerAndId;
        virtual public ByteVector2 DamageObjIndex { get { return ObjOwnerAndId; } }
        protected Rotation1D clientGoalRotation;
        byte clientNetReadRotation;
        protected Vector3 clientGoalPosition;
        protected float clientSpeedLength;
        /// <summary>
        /// if the object is updated here, otherwise it will just be a AI free dummie
        /// </summary>
        protected bool localMember = true;
        public bool NetworkLocalMember
        { get { return localMember; } }

        protected byte updatePrio = 255;

        protected Velocity oldVelocity;
        public Velocity Velocity = new Velocity();
        protected Rotation1D rotation = new Rotation1D();
        virtual public Rotation1D Rotation 
        { get { return rotation; } set { rotation = value; } }
        virtual public Rotation1D FireDir
        {
            get { return rotation; }
        }
        virtual public RotationQuarterion FireDir3D
        {
            get { throw new NotImplementedException(); }
        }

        public float Health = 1;
        public bool fullyInitialized = false;
        protected bool halfUpdateRandomBool = lib.RandomBool();
        public LF2.AbsObjBound CollisionBound;
        protected LF2.AbsObjBound TerrainInteractBound = null; //null if same as CollisionBound
        public LF2.AbsObjBound GetGroundInteractBound
        {
            get
            {
                if (TerrainInteractBound == null) return CollisionBound;
                return TerrainInteractBound;
            }
        }

        public Map.WorldPosition WorldPosition = new Map.WorldPosition();

        /// <summary>
        /// If you stop when trying to walk through it
        /// </summary>
        virtual public bool SolidBody
        {
            get { return false; }
        }

        public IntVector2 ScreenPos
        {
            get { return WorldPosition.ChunkGrindex; }
        }
        virtual public bool Alive
        { 
            get
            { 
                return Health > 0; 
            }
        }

        
        virtual protected void setImageDirFromSpeed()
        {
        }

        public float distanceToObject(AbsUpdateObj otherObj)
        {
            return PositionDiff3D(otherObj).Length();
        }

        public float DistaceBetweenBounds(AbsUpdateObj otherObj)
        {
            return Bound.Min(this.CollisionBound.MainBound.Bound.OuterBound.IntersectDepth(
                otherObj.CollisionBound.MainBound.Bound.OuterBound).Depth, 0);
        }

        public Vector2 PositionDiff(AbsUpdateObj otherObj)
        {
            if (otherObj == null)
                return Vector2.Zero;
            Vector2 diff = otherObj.PlanePos;
            diff.X -= Position.X;
            diff.Y -= Position.Z;
            return diff;
        }
        public Vector3 PositionDiff3D(AbsUpdateObj otherObj)
        {
            if (otherObj == null)
                return Vector3.Zero;
            return otherObj.Position - Position;
        }
        protected void addPhysics()
        {
            switch (this.PhysicsType)
            {
                case GameObjects.ObjPhysicsType.NO_PHYSICS:
                    physics = new NoPhysics();
                    break;
                case GameObjects.ObjPhysicsType.Projectile:
                    physics = new ProjectilePhysics(this);
                    break;
                case GameObjects.ObjPhysicsType.CharacterSimple:
                    physics = new CharacterPhysicsSimple(this);
                    break;
                case GameObjects.ObjPhysicsType.Character:
                    physics = new CharacterPhysics(this);
                    break;
                case GameObjects.ObjPhysicsType.Character2:
                    physics = new CharacterPhysics2(this);
                    break;
                case GameObjects.ObjPhysicsType.CharacterAdvanced:
                    physics = new CharacterPhysicsAdvanced(this);
                    break;
                case GameObjects.ObjPhysicsType.Hero:
                    physics = new HeroPhysics(this);
                    break;
                case ObjPhysicsType.BouncingObj:
                    physics = new BouncingObjPhysics(this);
                    break;
                case ObjPhysicsType.BouncingObj2:
                    physics = new BouncingObjPhysics2(this);
                    break;
                case GameObjects.ObjPhysicsType.FlyingObj:
                    physics = new FlyingObjPhysics2(this);
                    break;
            }
        }
        /// <summary>
        /// if at least 6chunks away from a hero
        /// </summary>
        /// <returns>if outside the active area</returns>
        public bool checkOutsideUpdateArea_ClosestHero()
        {
            if (!this.IsDeleted)
            {
                if (distanceToObject(ClosestHero(this.Position)) >= 6f * Map.WorldPosition.ChunkWidth)
                {
                    return true;
                }
            }
            return false;
        }
        /// <returns>chunk is unloaded</returns>
        public bool checkOutsideUpdateArea_ActiveChunk()
        {
            return checkOutsideUpdateArea_ActiveChunk(WorldPosition.ChunkGrindex);
        }

        /// <returns>chunk is unloaded</returns>
        public bool checkOutsideUpdateArea_ActiveChunk(IntVector2 chunkGrindex)
        {
            if (!this.IsDeleted)
            {
                Map.Chunk s = LfRef.chunks.GetScreenUnsafe(chunkGrindex);
                return s == null || s.Openstatus < Map.ScreenOpenStatus.DataGridComplete;
            }
            return false;
        }

        protected GameObjects.Characters.Hero GetClosestHero()
        {
            return ClosestHero(this.Position);
        }
        public void UpdateLocalOwnerId(byte localHostId)
        {
            ObjOwnerAndId.X = localHostId;
        }

        public static GameObjects.Characters.Hero ClosestHero(Vector3 position)
        {
            //List<GameObjects.Characters.Hero> heroes = LfRef.gamestate.AllHeroes();
            if (LfRef.AllHeroes.Count == 1)
                return LfRef.AllHeroes[0];

            LowestValue shortestDist = new LowestValue(false);

            for (int i = 0; i < LfRef.AllHeroes.Count; i++)
            {
                Vector3 diff = LfRef.AllHeroes[i].Position - position;
                shortestDist.Next(diff.Length(), i); 
            }
            return LfRef.AllHeroes[shortestDist.LowestMemberIndex];
        }

        protected GameObjects.Characters.AbsCharacter closestGoodGuy(ISpottedArrayCounter<GameObjects.AbsUpdateObj> objects)
        {
            return ClosestGoodGuy(this.Position, objects);
        }
        public static GameObjects.Characters.AbsCharacter ClosestGoodGuy(Vector3 position, ISpottedArrayCounter<GameObjects.AbsUpdateObj> objects)
        {
            LowestValue shortestDist = new LowestValue(false);
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetMember.Type == ObjectType.Character && WeaponAttack.WeaponLib.IsFoeTarget(WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero, objects.GetMember, true))
                {
                    Vector3 diff = objects.GetMember.Position - position;
                    shortestDist.Next(diff.Length(), objects.CurrentIndex);
                }

            }

            if (!shortestDist.hasValue) 
                return null;
            return (GameObjects.Characters.AbsCharacter)objects.GetFromIndex(shortestDist.LowestMemberIndex);
        }

        

        /// <returns>if the object is in to close range</returns>
        protected bool moveTowardsObject(AbsUpdateObj otherObj, float minDist, float speed)
        {
            return moveTowardsObject(otherObj, minDist, speed, 0);
        }
        protected bool moveTowardsObject(AbsUpdateObj otherObj, float minDist, float speed, float rndAngleAdd)
        {
            if (minDist <= 0 || distanceToObject(otherObj) > minDist)
            {
                rotation.Radians = AngleDirToObject(otherObj);
                if (rndAngleAdd != 0)
                    rotation.Radians += Ref.rnd.Plus_MinusF(rndAngleAdd);
                this.Velocity.Set(Rotation, speed);// = rotation.Direction(speed);
                if (speed < 0)
                {
                    rotation.Radians += MathHelper.Pi;
                }
                return false;
            }
            this.Velocity.SetZeroPlaneSpeed(); //= Vector2.Zero;
            return true;
        }

        protected Map.WalkingPath walkingPathAwayFromObject(AbsUpdateObj otherObj, float rndAngleDiff)
        {
            Rotation1D dir = new Rotation1D(AngleDirToObject(otherObj) + Rotation1D.D180.Radians + Ref.rnd.Plus_MinusF(rndAngleDiff));
            return LfRef.chunks.PathFinding(Position, Map.WorldPosition.V2toV3(dir.Direction(16), otherObj.Position.Y));
        }
        protected Map.WalkingPath walkingPathTowardsObject(AbsUpdateObj otherObj)
        {
            return LfRef.chunks.PathFinding(Position, otherObj.Position);
        }
        protected void rotateTowardsObject(AbsVoxelObj towards)
        {
            rotation.Radians = AngleDirToObject(towards);
            setImageDirFromRotation();
        }
        protected void rotateTowardsObject(AbsVoxelObj towards, float maxTurnSpeed, float time)
        {
            rotateTowardsObject(towards, maxTurnSpeed, time, this.Velocity.PlaneLength());
        }
        protected void rotateTowardsObject(AbsVoxelObj towards, float maxTurnSpeed, float time, float speed)
        {
            if (towards != null)
            {
                float goalDir = AngleDirToObject(towards);
                rotateTowardsGoalDir(goalDir, maxTurnSpeed, time, speed);
            }
        }

        abstract public bool VisibleInCam(int camIx);
        MiniHealthBarManager healthBar = null;
        virtual protected bool ViewHealthBar { get { return false; } }

        protected void rotateTowardsGoalDir(float goalDir, float maxTurnSpeed, float time)
        {
            rotateTowardsGoalDir(goalDir, maxTurnSpeed, time, this.Velocity.PlaneLength());
        }
        protected void rotateTowardsGoalDir(float goalDir, float maxTurnSpeed, float time, float speed)
        {
            float diff = rotation.AngleDifference(goalDir);

            if (diff != 0)
            {
                maxTurnSpeed *= time;
                if (Math.Abs(diff) > maxTurnSpeed)
                {
                    diff = maxTurnSpeed * lib.FloatToDir(diff);
                }
                rotation.Radians += diff;
                this.Velocity.Set(rotation, speed);
            }
        }
        /// <summary>
        /// The direction in radians to another game object
        /// </summary>
        public float AngleDirToObject(AbsUpdateObj otherObj)
        {
            Vector2 diff = PositionDiff(otherObj);
            return lib.V2ToAngle(diff);
        }

        public bool LookingAtObject(AbsUpdateObj otherObj, float maxAngle)
        {
            return Math.Abs(rotation.AngleDifference(AngleDirToObject(otherObj))) <= maxAngle;
        }

        public Vector2 SpeedTowardsObj(AbsUpdateObj otherObj)
        {
            Vector2 posDiff = PositionDiff(otherObj);
            return lib.SpeedTowardsPoint(Velocity.PlaneValue, posDiff) + lib.SpeedTowardsPoint(otherObj.Velocity.PlaneValue, -posDiff);
        }
        /// <summary>
        /// The angle this object is faceing towards another object
        /// </summary>
        protected float angleDiff(AbsUpdateObj otherObj)
        {
            return rotation.AngleDifference(AngleDirToObject(otherObj));
        }
        
        /// <summary>
        /// For objects that their natural direction is other than up on the image
        /// </summary>
        virtual protected float adjustRotation
        { get { return MathHelper.Pi; } }
        virtual public void Time_Update(UpdateArgs args)
        {
            if (!localMember)
            {
                updateClientDummieMotion(args.time);
            }
            updateHealthBar();
        }
        protected void updateHealthBar()
        {
            if (healthBar != null)
            {
                if (healthBar.Update())
                {
                    healthBar.DeleteMe();
                    healthBar = null;
                }
            }
        }

        protected void updateClientDummieMotion(float time)
        {
            Vector3 posDiff = clientGoalPosition - Position;
            float length = posDiff.Length();
            float speed = 0;

            const float MinLenght = 0.04f;

            if (length >= MinLenght)
            {
                const float LengthToSpeed = 0.18f;
                speed = lib.SmallestOfTwoValues(LengthToSpeed * length, clientSpeedLength * time);

                posDiff.Normalize();
                Position += posDiff * speed;
            }
            else
            {
                clientSpeedLength = 0;
            }


            clientSpeed(speed * 0.04f);

            if (NetRotationType == NetworkClientRotationUpdateType.Plane1D || NetRotationType == NetworkClientRotationUpdateType.FromSpeed)
            {
                if (length >= MinLenght)
                {
                    clientGoalRotation = Rotation1D.FromDirection(Map.WorldPosition.V3toV2(posDiff));
                }
                else if (NetRotationType == NetworkClientRotationUpdateType.Plane1D)
                {
                    clientGoalRotation.ByteDir = clientNetReadRotation;
                }
                rotateTowardClientGoalRot();
                setImageDirFromRotation();
            }  
        }

        protected void rotateTowardClientGoalRot()
        {
            float rotDiff = rotation.AngleDifference(clientGoalRotation);
            if (Math.Abs(rotDiff) <= 0.01f)
            {
                rotation = clientGoalRotation;
            }
            else
            {
                const float RotationSpeed = 0.4f;
                rotation.Add(rotDiff * RotationSpeed);
            }
        }

        /// <summary>
        /// Threaded update for extra calculations, is unrelyable in timestep, not allowed to create garbage
        /// </summary>
        virtual public void AIupdate(GameObjects.UpdateArgs args)
        { }
        virtual public void setImageDirFromRotation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Forces like exposions will affect some objects that fly away
        /// </summary>
        /// <param name="force"></param>
        virtual public void Force(Vector3 center, float force) { }
        virtual protected void clientSpeed(float speed)
        { }
        protected void characterCollCheck(ISpottedArrayCounter<GameObjects.AbsUpdateObj> counter)
        {
            if (PlatformSettings.ViewErrorWarnings && this.CollisionBound == null)
                throw new Exception("Obj mising bound: " + this.ToString());

            counter.Reset();
            while (counter.Next())
            {
                bool willDamage = counter.GetMember.IsWeaponTarget && 
                    WeaponAttack.WeaponLib.IsFoeTarget(this.WeaponTargetType, this.DamageObjIndex, counter.GetMember, false);

                if (willDamage)
                {
                    LF2.ObjBoundCollData collision = this.CollisionBound.Intersect2(counter.GetMember.CollisionBound);
                    if (collision != null)
                    { //Hit
                        if (handleCharacterColl(counter.GetMember, collision)) return;
                    }
                }

            }
        }

        

        abstract public Vector3 Position { get; set; }
        virtual public float ExspectedHeight { get { return 3; } }
        abstract public float X { get; set; }
        abstract public float Y { get; set; }
        abstract public float Z { get; set; }
        abstract public Vector2 PlanePos { get; set; }
        virtual public void Time_LasyUpdate(ref float time)
        {
            
        }

        public void UpdateWorldYPos()
        {
            WorldPosition.Y = (int)Position.Y;
        }

        virtual protected Vector3 worldPosOffset { get { return Vector3.Zero; } }
        virtual public bool UpdateWorldPos()
        {
            Vector3 pos = Position;
            if ((int)pos.X != WorldPosition.LocalBlockX || (int)pos.Z != WorldPosition.LocalBlockZ || (int)pos.Y != WorldPosition.WorldGrindex.Y)
            {
                IntVector2 chunk = WorldPosition.ChunkGrindex;

                WorldPosition.WorldGrindex = IntVector3.FromV3(pos + worldPosOffset);
                //new Debug.HeightCheck(WorldPosition);
                if (chunk != WorldPosition.ChunkGrindex)
                {
                    EnteredNewTile();
                }
                return true;
            }
            return false;
        }

        virtual public void OutSideActiveArea()
        {
            DeleteMe();
        }
        
        virtual public void WeaponAttackFeedback(WeaponAttack.WeaponTrophyType weaponType, int numHits, int numKills)
        { }
        protected void checkHeroCollision(bool localOnly)
        {
            StaticList<Characters.Hero> heroes;
            if (localOnly)
                heroes = LfRef.LocalHeroes;
            else
                heroes = LfRef.AllHeroes;//LfRef.gamestate.AllHeroes(); //LfRef.Heroes;

            for (int i = 0; i < heroes.Count; i++)
            {
                LF2.ObjBoundCollData collisionData = this.CollisionBound.Intersect2(heroes[i].CollisionBound);
                if (collisionData != null)
                { //Hit
                    handleCharacterColl(heroes[i], collisionData);
                }
            }
        }
        protected void SolidBodyAndCollisionDamageCheck(ISpottedArrayCounter<GameObjects.AbsUpdateObj> active)
        {
            while (active.Next())
            {
                if (active.GetMember.SolidBody && active.GetMember != this)
                {
                    if (LF2.AbsObjBound.SolidBodyIntersect(this, active.GetMember))
                    {
                        if (WeaponAttack.WeaponLib.IsFoeTarget(this, active.GetMember, false))
                        {
                            handleCharacterColl(active.GetMember, null);
                        }
                        return;
                    }
                }
            }
        }
        /// <returns>End search loop</returns>
        virtual protected bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            character.TakeDamage(WeaponAttack.DamageData.BasicCollDamage, true);
            return true;
        }
        
        virtual public bool IsHero()
        {
            return false;
        }
        
        protected void AngleToSpeed(Rotation1D angle, float velocity)
        {
            Velocity.Set(angle, velocity); 
            setImageDirFromSpeed();
        }

        virtual protected bool UseHeightObsticle
        { get { return true; } }

        virtual protected void EnteredNewTile()
        { }

        protected bool obsticleCollisionLasy()
        {
            return false;
        }
      
        /// <param name="ObjCollision">Null if terrain coll</param>
        virtual public void HandleColl3D(VikingEngine.Physics.Collision3D collData, AbsUpdateObj ObjCollision)//Physics.CollisionIntersection3D collData)
        {
            removeForce();
            obsticlePushBack(collData);
        }

        protected void removeForce()
        {
            if (physics != null)
                physics.removeForce();
        }

        /// <summary>
        /// Mainly for when AI detects an obsticle in it walking path
        /// </summary>
        virtual public void HandleObsticle(bool wallNotPit)
        {
            removeForce();
        }

        virtual public void HandleTerrainColl3D(LF2.TerrainColl collSata, Vector3 oldPos)
        {  }

        protected void obsticlePushBack(Physics.Collision3D collData)
        {
            const float StandardPushback = 1.04f;
            obsticlePushBack(collData, StandardPushback);
        }
        protected void obsticlePushBack(Physics.Collision3D collData, float bounce)
        {

            Vector3 depth = CollisionBound.IntersectionDepthAndDir(collData) * bounce;
            depth.Y = 0;
            if (PlatformSettings.ViewCollisionBounds)
            {
                Vector3 viewPos = Position;
                viewPos.Y += 2;
                Engine.ParticleHandler.AddParticles(ParticleSystemType.BulletTrace, new ParticleInitData(viewPos, depth * 100));
            }
            Position -= depth;

            UpdateBound();
        }

        public void UpdateBound()
        {
            if (CollisionBound != null)
            CollisionBound.UpdatePosition2(this);
            if (TerrainInteractBound != null)
                TerrainInteractBound.UpdatePosition2(this);
        }

        virtual protected bool RunDetailedObsticleCheck
        {
            get { return false; }
        }

        public bool IsDeleted { get; set; }
       
        virtual protected VikingEngine.Graphics.TextureEffectType MeshEffect
        { get { return Graphics.TextureEffectType.Flat; } }

        public void DeleteMe()
        {
            this.DeleteMe(true);
        }

        virtual public void DeleteMe(bool local)
        {

            if (local)
            {
                bool netShare = false;

                if (localMember)
                {
                    if (NetworkShareSettings.DeleteByHost)
                    {
                        netShare = true;
                    }
                }
                else
                {
                    if (NetworkShareSettings.DeleteByClient)
                    {
                        netShare = true;
                    }
                }

                if (netShare)
                {
                    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_RemoveGameObject, Network.SendPacketToOptions.SendToAll, Network.PacketReliability.Reliable, null);
                    ObjOwnerAndId.WriteStream(w);
                    w.Write(Alive);
                    
                }
            }
            
            Health = 0;
            IsDeleted = true;

            if (healthBar != null) healthBar.DeleteMe();
            if (LightSourceType != Graphics.LightParticleType.NUM_NON)
                Director.LightsAndShadows.Instance.AddLight(this, false);
        }

        
        //public byte OwnerNetworkID
        //{
        //    get
        //    {
        //        return ObjOwnerAndId.Y;
        //        //return Network.NetLib.ObjIndexToOwnerID(objOwnerAndId);
        //    }
        //}
        /// <param name="local">If the damage wasnt dealt over the net</param>
        public bool TakeDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (PlatformSettings.ViewErrorWarnings)
            {
                if (damage.Damage <= 0)
                {
                    Debug.DebugLib.Print(Debug.PrintCathegoryType.Warning, this.ToString() + " Recieved Zero damage");
                }
            }
            if (willReceiveDamage(damage))
            {
                handleDamage(damage, local);
                return true;
            }
            return false;
        }
        virtual protected void BlockSplatter()
        {
            throw new NotImplementedException("BlockSplatter");
        }
        virtual protected bool willReceiveDamage(WeaponAttack.DamageData damage)
        { return recieveDamageType > RecieveDamageType.WeaponBounce; }

        virtual protected RecieveDamageType recieveDamageType { get { return RecieveDamageType.NoRecieving; } }

        float startHealth;
        virtual protected void handleDamage(WeaponAttack.DamageData damage, bool local)
        {

            if (startHealth == default(float))
            {
                startHealth = Health;
            }
            
            if (recieveDamageType == RecieveDamageType.ReceiveDamage)
                Health -= damage.Damage;
            
            if (Health <= 0)
            {
                if (localMember || (local && NetworkShareSettings.DeleteByClient))
                {
                    DeathEvent(local, damage);
                }
                else
                {
                    Health = 1f;
                }
            }

            if (local)
            {
                if (localMember)
                {
                    if (HasNetworkClient)
                    {
                        if (Alive)
                        {
                            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_HostShareDamageVisuals,
                                Network.PacketReliability.Reliable);
                            ObjOwnerAndId.WriteStream(w);
                            damage.WriteVisualDamage(w);
                        }
                    }
                }
                else
                {
                    if (Alive)
                    {
                        System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_TakeDamage, Network.PacketReliability.Reliable, LootfestLib.LocalHostIx);
                        ObjOwnerAndId.WriteStream(w);
                        damage.WriteStream(w);
                    }
                }
            }

            if (Alive)
            {
                if (ViewHealthBar)
                {
                    if (healthBar == null)
                        healthBar = new MiniHealthBarManager(this, startHealth);
                    else
                        healthBar.TakenDamageEvent();
                }
            }
        }

        

        public void NetworkReadDamage(System.IO.BinaryReader r)
        {
            this.TakeDamage(WeaponAttack.DamageData.FromStream(r), false);
        }
        public void NetworkReadVisualDamage(System.IO.BinaryReader r)
        {
            //bool alive = r.ReadBoolean();
            //if (alive)
            //{
                WeaponAttack.DamageData dam = WeaponAttack.DamageData.NoN;
                dam.ReadVisualDamage(r);
                this.TakeDamage(dam, false);
            //}
            //else
            //{
            //    Health = 0;
            //    BlockSplatter();
            //}
        }

        public void NetworkReadDeleteMe(System.IO.BinaryReader r)
        {
            bool alive = r.ReadBoolean();
            DeleteMe(false);
            if (!alive)
            {
                Health = 0;
                //BlockSplatter();
                this.DeathEvent(false, WeaponAttack.DamageData.NoN);
            }
        }

        virtual protected void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            DeleteMe();
        }
        virtual protected void HitsObject(AbsVoxelObj victim) //bool kill)
        { }
        virtual protected void MissesObject()
        { }
       
        virtual public ObjPhysicsType PhysicsType
        { get { return ObjPhysicsType.NO_PHYSICS; } }
        abstract public ObjectType Type { get; }
        abstract public int UnderType { get; }
        virtual public WeaponAttack.WeaponUserType WeaponTargetType { get { return WeaponAttack.WeaponUserType.NON; } }
        virtual public bool IsWeaponTarget { get { return recieveDamageType != RecieveDamageType.NoRecieving; } }
            
        #region NETWORK
        virtual public NetworkShare NetworkShareSettings { get { return LF2.GameObjects.NetworkShare.FullExceptClientDel; } }
        virtual public bool NetShareSett_HealthUpdate { get { return this.IsWeaponTarget; } }

        virtual public bool HasNetId { get { return NetworkShareSettings.Creation; } }

        public void NetworkUpdatePacket(Network.PacketReliability relyable)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(
                Network.PacketType.LF2_GameObjUpdate,
                relyable, null);
            ObjOwnerAndId.WriteStream(w);
            NetWriteUpdate(w);

            
        }

        virtual public void NetWriteUpdate(System.IO.BinaryWriter w)
        {
            WritePosition(Position, w);
            if (NetRotationType == NetworkClientRotationUpdateType.Plane1D)
                w.Write(rotation.ByteDir);
            else if (NetRotationType == NetworkClientRotationUpdateType.Full3D)
                LowResRotation3d.WriteStream(w);

            //if (NetShareSett_HealthUpdate)
            //    writeHealth(w);
        }
        virtual public void NetReadUpdate(System.IO.BinaryReader r)
        {
#if XBOX
            try
            {
#endif
                clientGoalPosition = ReadPosition(r);
                Vector3 clientPosDiff = clientGoalPosition - Position;

                float length = clientPosDiff.Length();
                if (length <= 0.2f || length > 50)
                {
                    //does basically never happen, gets no update while idle
                    Position = clientGoalPosition;
                    clientSpeedLength = 0;
                }
                else
                {
                    clientSpeedLength = length * 0.2f / Ref.netSession.UpdateRate;
                }

                if (NetRotationType == NetworkClientRotationUpdateType.Plane1D)
                    clientNetReadRotation = r.ReadByte();
                else if (NetRotationType == NetworkClientRotationUpdateType.Full3D)
                    LowResRotation3d = ByteVector3.FromStream(r);

                //NetworkReadDamage(r);
#if XBOX
            }
            catch (System.IO.EndOfStreamException e)
            {
                Debug.LogError("NetReadUpdate EndOfStream, " + e.Message);
            }
#endif
        }


        const int RWPosMaxHeight = Map.WorldPosition.ChunkHeight * 2;

        public static void WritePosition(Vector3 pos, System.IO.BinaryWriter w)
        {
            Vector2 percentPos = Map.WorldPosition.V3toV2(pos) / Map.WorldPosition.WorldSizeX;
            percentPos *= PublicConstants.UInt16Size;
            float percentHeight = (pos.Y + 0.25f) / RWPosMaxHeight;
            percentHeight *= PublicConstants.ByteSize;
            w.Write((ushort)percentPos.X);
            w.Write((byte)percentHeight);
            w.Write((ushort)percentPos.Y);
        }
        public static Vector3 ReadPosition(System.IO.BinaryReader r)
        {
            const float ShortToWP = (float)Map.WorldPosition.WorldSizeX / PublicConstants.UInt16Size;
            const float ByteToHeight = (float)RWPosMaxHeight / PublicConstants.ByteSize;

            return new Vector3(
                r.ReadUInt16() * ShortToWP,
                r.ReadByte() * ByteToHeight,
                r.ReadUInt16() * ShortToWP);
        }

        const float StreamHealthMultiplier = 10;
        void writeHealth(System.IO.BinaryWriter w)
        {
            w.Write((ushort)(Health * StreamHealthMultiplier));
        }
        void readHealth(System.IO.BinaryReader r)
        {
            Health = (float)r.ReadUInt16() / StreamHealthMultiplier;
        }

        virtual protected ByteVector3 LowResRotation3d
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        virtual protected NetworkClientRotationUpdateType NetRotationType
        { get { return NetworkClientRotationUpdateType.Plane1D; } }

        //NET SEND OBJECT
        

        //public System.IO.BinaryWriter NetworkSendAttack()
        //{
        //    System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.ShareCharacterAttack, Network.PacketReliability.Reliable, LootfestLib.LocalHostIx);
        //    ObjToNetPacket(w);
        //    return w;
        //}
        //virtual public void NetworkRecieveAttack(System.IO.BinaryReader r)
        //{
        //}

        public AbsUpdateObj()
        {
            localMember = true;
            if (autoAddToUpdate)
            {
                LfRef.gamestate.GameObjCollection.AddGameObject(this);
            }
            basicInit();
        }
        virtual public void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            if (ObjOwnerAndId.Y == 0)
            {
                Debug.LogError( "Cant share an object with no ID, " + this.ToString());
            }
            //obj receive this
            ObjOwnerAndId.WriteStream(w);

            if (NetShareSett_HealthUpdate)
                writeHealth(w);
        }
        //NET RECIEVE OBJECT
        public AbsUpdateObj(System.IO.BinaryReader r)
        {
            Health = float.MaxValue;
            localMember = false;
            ObjOwnerAndId = ByteVector2.FromStream(r);

            if (PlatformSettings.ViewErrorWarnings)
            {
                if (ObjOwnerAndId.Y == 0 && !(this is Characters.Hero))
                {
                    Debug.LogError( "Gameobject got id 0: " + this.ToString());
                }
            }

            if (NetShareSett_HealthUpdate)
            {
                readHealth(r);
                startHealth = Health;
            }
            LfRef.gamestate.GameObjCollection.AddGameObject(this);
            
            basicInit();
        }
        virtual protected void basicInit() 
        {
            if (LightSourceType != Graphics.LightParticleType.NUM_NON)
                Director.LightsAndShadows.Instance.AddLight(this, true);
        }
        virtual protected bool autoAddToUpdate { get { return true; } }

        public void NetworkShareObject()
        {
            NetworkShareObject( Network.SendPacketToOptions.SendToAll);
        }
        public void NetworkShareObject(Network.SendPacketToOptions to)
        {
            if (NetworkShareSettings.Creation)
            {
#if WINDOWS
               // Debug.DebugForm.PrintOutput("Send game object: " + this.ToString() + ", ObjIx: " + this.ObjOwnerAndId.ToString(), Debug.DebugOutput.Network);
#endif
                HasNetworkClient = true;
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_AddGameObject, to, Network.PacketReliability.Reliable, LootfestLib.LocalHostIx);
                w.Write((byte)this.Type);
                w.Write((byte)this.UnderType);
                ObjToNetPacket(w);
                
                if (to.To != Network.SendPacketTo.All)
                {
                    updatePositionToNewbie = 40;
                }
            }
        }

        protected System.IO.BinaryWriter NetworkWriteObjectState(AiState state)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_GameObjectState, Network.PacketReliability.Reliable);
            ObjOwnerAndId.WriteStream(w);
            w.Write((byte)state);
            return w;
        }

        protected System.IO.BinaryWriter BeginWriteObjectStateAsynch(AiState state)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginAsynchPacket();
            ObjOwnerAndId.WriteStream(w);
            w.Write((byte)state);
            return w;
        }

        protected void EndWriteObjectStateAsynch(System.IO.BinaryWriter w)
        {
            Ref.netSession.EndAsynchPacket(w, Network.PacketType.LF2_GameObjectState, Network.SendPacketTo.All, 0, Network.PacketReliability.Reliable, null);
        }


        public void NetworkReadObjectState(System.IO.BinaryReader r)
        {
            AiState state = (AiState)r.ReadByte();
            networkReadObjectState(state, r);
        }
        virtual protected void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        { 
            
        }

        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }
        
        protected int updatePositionToNewbie = 0;

        public bool HasNetworkClient = false;

#endregion
        virtual public InteractType ObjInteractType { get { throw new NotImplementedException(); } }
            


       

        public virtual void clientStartPosition()
        {
            if (WorldPosition.ChunkGrindex.X == 0)
                WorldPosition = Map.WorldPosition.WorldCenter;
        }

        virtual public Vector3 LightSourcePosition { get { return Position; } }
        virtual public float LightSourceRadius { get { return 32; } }//2; } }
        virtual public LightParticleType LightSourceType { get { return Graphics.LightParticleType.NUM_NON; } }
        virtual public LightSourcePrio LightSourcePrio { get { return Graphics.LightSourcePrio.Medium; } }

        int lightSourceArrayIndex = -1;
        public int LightSourceArrayIndex { get { return lightSourceArrayIndex; } set { lightSourceArrayIndex = value; } }
        public float LightSourceDistanceToGamer { get; set; }

        public override string ToString()
        {
            return "GameObject" +   this.GetType().ToString().Remove(0, this.GetType().Namespace.Length) + ObjOwnerAndId.ToString();
        }

        //virtual protected bool Immortal { get { return false; } }
    }

   
}

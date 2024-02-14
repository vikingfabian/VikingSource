using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna
using VikingEngine.Physics;
using VikingEngine.LootFest.GO.Bounds;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsUpdateObj : AbsChildObject, IDeleteable, ISpottedArrayMember, ILightSource
    {
        public int characterLevel = 0;
        public AbsPhysics physics;
        public VikingEngine.LootFest.GO.Physics.RectangleAreaBoundary boundary = null;
        //NETWORK
        public NetworkId ObjOwnerAndId;
        virtual public NetworkId DamageObjIndex { get { return ObjOwnerAndId; } }
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

        public int updatesCount = 0;
        
        
        virtual public Rotation1D FireDir(GameObjectType weaponType)
        {
            return rotation;
        }
        virtual public RotationQuarterion FireDir3D(GameObjectType weaponType)
        {
            throw new NotImplementedException(); 
        }

        public float Health = 1;
        protected bool halfUpdateRandomBool = Ref.rnd.Bool();
        public ObjectBound CollisionAndDefaultBound;
        public ObjectBound TerrainInteractBound = null; //null if same as CollisionBound
        public ObjectBound DamageBound = null; //null if same as CollisionBound
        public AbsLevelCollider levelCollider;

        public VikingEngine.LootFest.Director.BossManager bossmanager = null; 

        public BlockMap.LevelEnum LevelEnum
        {
            get
            {
                if (levelCollider == null || levelCollider.level == null)
                { return LevelEnum.NUM_NON; }

                return levelCollider.level.LevelEnum;
            }
        }
        public BlockMap.AbsLevel Level
        {
            get 
            {
                if (levelCollider != null)
                { return levelCollider.level; }
                
                return null;
            }
            set
            {
                if (levelCollider != null)
                { levelCollider.level = value; }
            }
        }

        /// <summary>Will sleep instead of being removed </summary>
        public bool managedGameObject = false;

        public float modelScale = float.MinValue;

        public ObjectBound GetGroundInteractBound
        {
            get
            {
                if (TerrainInteractBound == null) return CollisionAndDefaultBound;
                return TerrainInteractBound;
            }
        }

        public Map.WorldPosition startWp;
        public Map.WorldPosition WorldPos = Map.WorldPosition.EmptyPos;
        

        /// <summary>
        /// If you stop when trying to walk through it
        /// </summary>
        virtual public bool SolidBody
        {
            get { return false; }
        }

        virtual public bool CharacterLevelsHasDifferentBounds { get { return false; } }
        public int BoundLevel
        {
            get
            {
                if (CharacterLevelsHasDifferentBounds)
                {
                    return characterLevel;
                }
                else
                {
                    return 0;
                }
            }
        }

        virtual public System.IO.FileShare BoundSaveAccess
        {
            get { return System.IO.FileShare.None; }
        }
        protected void loadBounds()
        {
            LfRef.bounds.LoadBound(this);
        }

        public IntVector2 ScreenPos
        {
            get { return WorldPos.ChunkGrindex; }
        }
        virtual public bool Alive
        { 
            get
            { 
                return Health > 0; 
            }
        }
        public bool Dead { get { return !Alive; } }

        
        virtual public void setImageDirFromSpeed()
        {
        }

        public void SetAsManaged()
        {
            managedGameObject = true;
        }

        virtual public void setSpawnArgument(AbsSpawnArgument spawnArg)
        {
            spawnArg.ApplyTo(this);
        }

        public float planeDistanceToObject(AbsUpdateObj otherObj)
        {
            Vector3 diff = PositionDiff3D(otherObj);
            return VectorExt.Length(diff.X, diff.Z);
        }

        public float distanceToObject(AbsUpdateObj otherObj)
        {
            return PositionDiff3D(otherObj).Length();
        }

        public float DistaceBetweenBounds(AbsUpdateObj otherObj)
        {
            return Bound.Min(this.CollisionAndDefaultBound.MainBound.outerBound.IntersectDepth(
                otherObj.CollisionAndDefaultBound.MainBound.outerBound).Depth, 0);
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
        virtual protected void addPhysics()
        {
            switch (this.PhysicsType)
            {
                case GO.ObjPhysicsType.NO_PHYSICS:
                    physics = new NoPhysics();
                    break;
                case GO.ObjPhysicsType.Projectile:
                    physics = new ProjectilePhysics(this);
                    break;
                case GO.ObjPhysicsType.GroundAi:
                    physics = new AiPhysicsLf3((Characters.AbsCharacter)this, false);
                    break;
                case GO.ObjPhysicsType.FlyingAi:
                    physics = new AiPhysicsLf3((Characters.AbsCharacter)this, true);
                    break;
                case GO.ObjPhysicsType.CharacterSimple:
                    physics = new CharacterPhysicsSimple(this);
                    break;
                case GO.ObjPhysicsType.Character:
                    physics = new CharacterPhysics(this);
                    break;
                case GO.ObjPhysicsType.Character2:
                    physics = new CharacterPhysics2(this);
                    break;
                case GO.ObjPhysicsType.CharacterAdvanced:
                    physics = new CharacterPhysicsAdvanced(this);
                    break;
                case GO.ObjPhysicsType.Hero:
                    physics = new HeroPhysics(this);
                    break;
                case ObjPhysicsType.BouncingObj:
                    physics = new BouncingObjPhysics(this);
                    break;
                case ObjPhysicsType.BouncingObj2:
                    physics = new BouncingObjPhysics2(this);
                    break;
                case GO.ObjPhysicsType.FlyingObj:
                    physics = new FlyingObjPhysics2(this);
                    break;
            }

            
        }

        protected void addLevelCollider()
        {
            if (LevelCollType == ObjLevelCollType.Standard)
            {
                levelCollider = new LevelCollider(this);
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
                if (distanceToObject(ClosestHero(this.Position, false)) >= 6f * Map.WorldPosition.ChunkWidth)
                {
                    return true;
                }
            }
            return false;
        }
        /// <returns>chunk is unloaded</returns>
        public bool checkOutsideUpdateArea_ActiveChunk()
        {
            return checkOutsideUpdateArea_ActiveChunk(WorldPos.ChunkGrindex);
        }

        /// <returns>chunk is unloaded</returns>
        public bool checkOutsideUpdateArea_ActiveChunk(IntVector2 chunkGrindex)
        {
            if (!this.IsDeleted)
            {
                Map.Chunk s = LfRef.chunks.GetScreenUnsafe(chunkGrindex);
                if (s == null || !s.generatedGameObjects)//(!s.neighbor_generatedGameObjects && !s.generatedGameObjects))
                {
                    return true;
                }
            }
            return false;
        }

        
        /// <returns>chunk is unloaded</returns>
        public bool checkOutsideUpdateArea_StartChunk()
        {
            return checkOutsideUpdateArea_ActiveChunk(startWp.ChunkGrindex);
        }

        public void checkSleepingState()
        {
            Map.Chunk s = LfRef.chunks.GetScreenUnsafe(WorldPos.ChunkGrindex);
            bool active = s != null && s.generatedGameObjects;

            if (active == IsSleepState)
            {
                sleep(!active);
            }
        }


        protected GO.PlayerCharacter.AbsHero GetClosestHero(bool localOnly)
        {
            return ClosestHero(this.Position, localOnly);
        }
        public void UpdateLocalOwnerId(byte localHostId)
        {
            ObjOwnerAndId.hostingPlayer = localHostId;
        }

        public static GO.PlayerCharacter.AbsHero ClosestHero(Vector3 position, bool localOnly)
        {
            StaticList<GO.PlayerCharacter.AbsHero> heroes;
            if (localOnly)
            {
                heroes = LfRef.LocalHeroes;
            }
            else
            {
                heroes = LfRef.AllHeroes;
            }

            if (heroes.Count == 1)
                return heroes[0];

            FindMinValue shortestDist = new FindMinValue(false);

            for (int i = 0; i < heroes.Count; i++)
            {
                Vector3 diff = heroes[i].Position - position;
                shortestDist.Next(diff.Length(), i); 
            }
            return heroes[shortestDist.minMemberIndex];
        }

        protected GO.Characters.AbsCharacter closestGoodGuy(ISpottedArrayCounter<GO.AbsUpdateObj> objects)
        {
            return ClosestGoodGuy(this.Position, objects);
        }
        public static GO.Characters.AbsCharacter ClosestGoodGuy(Vector3 position, ISpottedArrayCounter<GO.AbsUpdateObj> objects)
        {
            FindMinValue shortestDist = new FindMinValue(false);
            objects.Reset();
            while (objects.Next())
            {
                if (objects.GetSelection is GO.Characters.AbsCharacter && WeaponAttack.WeaponLib.IsFoeTarget(WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty, objects.GetSelection, true))
                {
                    Vector3 diff = objects.GetSelection.Position - position;
                    shortestDist.Next(diff.Length(), objects.CurrentIndex);
                }

            }

            if (!shortestDist.hasValue) 
                return null;
            return (GO.Characters.AbsCharacter)objects.GetFromIndex(shortestDist.minMemberIndex);
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
                this.Velocity.Set(Rotation, speed);
                if (speed < 0)
                {
                    rotation.Radians += MathHelper.Pi;
                }
                return false;
            }
            else
            {
                this.Velocity.SetZeroPlaneSpeed();
                return true;
            }
        }

        protected Map.WalkingPath walkingPathAwayFromObject(AbsUpdateObj otherObj, float rndAngleDiff)
        {
            Rotation1D dir = new Rotation1D(AngleDirToObject(otherObj) + Rotation1D.D180.Radians + Ref.rnd.Plus_MinusF(rndAngleDiff));
            return LfRef.chunks.PathFinding(Position, VectorExt.V2toV3XZ(dir.Direction(16), otherObj.Position.Y));
        }
        protected Map.WalkingPath walkingPathTowardsObject(AbsUpdateObj otherObj)
        {
            return LfRef.chunks.PathFinding(Position, otherObj.Position);
        }
        protected void rotateTowardsObject(AbsUpdateObj towards)
        {
            rotation.Radians = AngleDirToObject(towards);
            setImageDirFromRotation();
        }
        public void rotateTowardsObject(AbsUpdateObj towards, float maxTurnSpeed)
        {
            rotateTowardsObject(towards, maxTurnSpeed, this.Velocity.PlaneLength());
        }
        protected void rotateTowardsObject(AbsUpdateObj towards, float maxTurnSpeed, float speed)
        {
            if (towards != null)
            {
                float goalDir = AngleDirToObject(towards);
                rotateTowardsGoalDir(goalDir, maxTurnSpeed, speed);
            }
        }

        abstract public bool VisibleInCam(int camIx);

        public bool rotateTowardsGoalDir(float goalDir, float maxTurnSpeed)
        {
            return rotateTowardsGoalDir(goalDir, maxTurnSpeed, this.Velocity.PlaneLength());
        }
        public bool rotateTowardsGoalDir(float goalDir, float maxTurnSpeed, float speed)
        {
            //bool complete = true;
            float diff = rotation.AngleDifference(goalDir);

            return rotateTowardsGoalDir(goalDir, diff, maxTurnSpeed, speed);
        }

        public bool rotateTowardsGoalDir(float goalDir, float angleDiff, float maxTurnSpeed, float speed)
        {
            bool complete = true;
            
            if (angleDiff != 0)
            {
                maxTurnSpeed *= Ref.DeltaTimeMs;
                if (Math.Abs(angleDiff) > maxTurnSpeed)
                {
                    rotation.Radians += maxTurnSpeed * lib.ToLeftRight(angleDiff);
                    complete = false;
                }
                else
                {
                    rotation.Radians = goalDir;
                }
                if (speed != 0)
                {
                    this.Velocity.Set(rotation, speed);
                }
            }

            return complete;
        }
        /// <summary>
        /// The direction in radians to another game object
        /// </summary>
        public float AngleDirToObject(AbsUpdateObj otherObj)
        {
            Vector2 diff = PositionDiff(otherObj);
            return lib.V2ToAngle(diff);
        }

        public float AngleDirToObject(Vector3 otherObj)
        {
            return lib.V2ToAngle(new Vector2(otherObj.X - X, otherObj.Z - Z));
        }

        public bool LookingTowardObject(AbsUpdateObj otherObj, float maxAngle)
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
            if (localMember)
            {
                if (levelCollider != null)
                {
                    levelCollider.updateCollisions();
                }
            }
            else if (NetworkShareSettings.Update)
            {
                updateClientDummieMotion(args.time);
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
                speed = lib.SmallestValue(LengthToSpeed * length, clientSpeedLength * time);

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
                    clientGoalRotation = Rotation1D.FromDirection(VectorExt.V3XZtoV2(posDiff));
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
        virtual public void AsynchGOUpdate(GO.UpdateArgs args)
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
        protected void characterCollCheck(ISpottedArrayCounter<GO.AbsUpdateObj> counter)
        {
            if (PlatformSettings.ViewErrorWarnings && this.CollisionAndDefaultBound == null)
                throw new Exception("Obj mising bound: " + this.ToString());

            counter.Reset();
            while (counter.Next())
            {
                //if (counter.GetMember is VikingEngine.LootFest.GO.EnvironmentObj.HitTarget)
                //{
                //    lib.DoNothing();
                //}

                bool willDamage = counter.GetSelection.IsWeaponTarget && 
                    WeaponAttack.WeaponLib.IsFoeTarget(this.WeaponTargetType, this.DamageObjIndex, counter.GetSelection, false);

                if (willDamage)
                {
                    var collision = this.CollisionAndDefaultBound.Intersect2(counter.GetSelection.CollisionAndDefaultBound);
                    if (collision != null)
                    { //Hit
                        if (handleCharacterColl(counter.GetSelection, collision, false)) return;
                    }
                }

            }
        }

        abstract public Vector3 Position { get; set; }
        virtual public float Scale1D { get { return 1f; } }

        virtual public float ExspectedHeight { get { return CollisionAndDefaultBound.MainBound.halfSize.Y * 2f; } }
        abstract public float X { get; set; }
        abstract public float Y { get; set; }
        abstract public float Z { get; set; }
        abstract public Vector2 PlanePos { get; set; }
        virtual public void Time_LasyUpdate(ref float time)
        {
            
        }

        public void UpdateWorldYPos()
        {
            WorldPos.Y = (int)Position.Y;
        }

        virtual protected Vector3 worldPosOffset { get { return Vector3.Zero; } }
        virtual public bool UpdateWorldPos()
        {
            var prevPos = WorldPos.WorldGrindex;
            WorldPos.PositionV3 = Position;
            return prevPos != WorldPos.WorldGrindex;
        }

        virtual public void OutSideActiveArea()
        {
            DeleteMe();
        }
        
        virtual public void WeaponAttackFeedback(GameObjectType weaponType, int numHits, int numKills)
        { }
        protected void checkHeroCollision(bool localOnly, bool triggerInteractionDialogue, AbsUpdateObj interactingObj)
        {
            StaticList<PlayerCharacter.AbsHero> heroes;
            if (localOnly)
                heroes = LfRef.LocalHeroes;
            else
                heroes = LfRef.AllHeroes;

            for (int i = 0; i < heroes.Count; i++)
            {
                var collisionData = this.CollisionAndDefaultBound.Intersect2(heroes[i].CollisionAndDefaultBound);
                if (collisionData != null)
                { //Hit
                    if (triggerInteractionDialogue)
                    { heroes[i].InteractPrompt_ver2(interactingObj); }
                    else
                    { handleCharacterColl(heroes[i], collisionData, false); }
                }
            }
        }

        protected void checkCharacterCollision(ISpottedArrayCounter<GO.AbsUpdateObj> counter, bool useDamageBound)
        {
            ObjectBound myBound;
            if (useDamageBound && DamageBound != null)
            {
                myBound = DamageBound;
            }
            else
            {
                useDamageBound = false;
                myBound = CollisionAndDefaultBound;
            }

            counter.Reset();
            while (counter.Next())
            {
                if (counter.GetSelection is GO.Characters.AbsCharacter && WeaponAttack.WeaponLib.IsFoeTarget(this.WeaponTargetType, counter.GetSelection.WeaponTargetType, false))
                {
                    var collisionData = myBound.Intersect2(counter.GetSelection.CollisionAndDefaultBound);
                    if (collisionData != null)
                    {
                        handleCharacterColl(counter.GetSelection, collisionData, useDamageBound);
                    }
                }
            }
        }

        

        protected void SolidBodyAndCollisionDamageCheck(ISpottedArrayCounter<GO.AbsUpdateObj> active)
        {
            while (active.Next())
            {
                if (active.GetSelection.SolidBody && active.GetSelection != this)
                {
                    if (LootFest.AbsObjBound.SolidBodyIntersect(this, active.GetSelection))
                    {
                        if (WeaponAttack.WeaponLib.IsFoeTarget(this, active.GetSelection, false))
                        {
                            handleCharacterColl(active.GetSelection, null, false);
                        }
                        return;
                    }
                }
            }
        }
        /// <returns>End search loop</returns>
        virtual protected bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
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
        virtual public void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)//Physics.CollisionIntersection3D collData)
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
        virtual public void HandleObsticle(bool wallNotPit, AbsUpdateObj ObjCollision)
        {
            removeForce();
        }

        virtual public void HandleTerrainColl3D(LootFest.TerrainColl collSata, Vector3 oldPos)
        {  }

        protected void obsticlePushBack(GO.Bounds.BoundCollisionResult collData)
        {
            const float StandardPushback = 1.04f;
            obsticlePushBack(collData, StandardPushback);
        }
        protected void obsticlePushBack(GO.Bounds.BoundCollisionResult collData, float bounce)
        {

            Vector3 depth = CollisionAndDefaultBound.IntersectionDepthAndDir(collData) * bounce;
            depth.Y = 0;
            Position -= depth;

            UpdateBound();
        }

        public void UpdateBound()
        {
            if (CollisionAndDefaultBound != null)
                CollisionAndDefaultBound.UpdatePosition2(this);
            if (TerrainInteractBound != null)
                TerrainInteractBound.UpdatePosition2(this);
            if (DamageBound != null)
                DamageBound.UpdatePosition2(this);
        }

        

        virtual protected bool RunDetailedObsticleCheck
        {
            get { return false; }
        }

        public bool IsDeleted { get; set; }
        public bool IsKilled { get; set; }
        public bool IsSleepState = false;

        virtual protected VikingEngine.Graphics.TextureEffectType MeshEffect
        { get { return Graphics.TextureEffectType.Flat; } }

        public void DeleteMe()
        {
            this.DeleteMe(true);
        }

        virtual public void DeleteMe(bool local)
        {
            if (this is Critter)
            {
                lib.DoNothing();
            }
            
            Health = 0;
            IsDeleted = true;


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
                    netWriteDamageAndRemoval(WeaponAttack.DamageData.NoN);
                }
            }
            
            if (LightSourceType != Graphics.LightParticleType.NUM_NON)
                Director.LightsAndShadows.Instance.AddLight(this, false);
        }

        protected void netWriteDamageAndRemoval(WeaponAttack.DamageData damage)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.GameObjDamageAndRemoval, Network.SendPacketToOptions.SendToAll, Network.PacketReliability.Reliable, null);
            ObjOwnerAndId.write(w);

            damage.writeDamage(w);
            w.Write(IsKilled);
            w.Write(IsDeleted);
        }

        public void netReadDamageAndRemoval(System.IO.BinaryReader r)
        {
            WeaponAttack.DamageData damage = WeaponAttack.DamageData.NoN;
            damage.ReadDamage(r);

            bool isKilled = r.ReadBoolean();
            bool isDeleted = r.ReadBoolean();

            if (isDeleted)
            { //Remove GO
                DeleteMe(false);
                this.IsKilled = isKilled;
                if (IsKilled)
                {
                    Health = 0;
                    this.DeathEvent(false, WeaponAttack.DamageData.NoN);
                    BlockSplatter();
                }
            }
            else
            { //View some damage
                if (damage.Damage > 0f)
                {
                    this.TakeDamage(damage, false);
                }
            }
        }


        virtual public void sleep(bool setToSleep)
        {
            IsSleepState = setToSleep;
        }

        /// <param name="local">If the damage wasnt dealt over the net</param>
        public bool TakeDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (PlatformSettings.ViewErrorWarnings)
            {
                if (damage.Damage <= 0)
                {
                    Debug.LogWarning(this.ToString() + " Recieved Zero damage");
                }
            }
            if (willReceiveDamage(damage))
            {
                handleDamage(damage, local);
                return true;
            }
            return false;
        }
        virtual public void BlockSplatter()
        {
            
            //throw new NotImplementedException("BlockSplatter");
        }
        virtual protected bool willReceiveDamage(WeaponAttack.DamageData damage)
        { return recieveDamageType > RecieveDamageType.WeaponBounce; }

        virtual protected RecieveDamageType recieveDamageType { get { return RecieveDamageType.NoRecieving; } }

        //protected float startHealth;
        virtual protected void handleDamage(WeaponAttack.DamageData damage, bool local)
        {

            if (recieveDamageType == RecieveDamageType.ReceiveDamage)
            {
                
                Health -= damage.Damage;
                onTookDamage(damage, local);
            }


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
                if ((!localMember || HasNetworkClient) && Alive)
                {
                    netWriteDamageAndRemoval(damage);
                }
            }
        }

        virtual protected void onTookDamage(WeaponAttack.DamageData damage, bool local)
        {

        }

        virtual protected void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            IsKilled = true;
            DeleteMe();
        }
        virtual protected void HitsObject(AbsVoxelObj victim)
        { }
        virtual protected void MissesObject()
        { }
       
        virtual public ObjPhysicsType PhysicsType
        { get { return ObjPhysicsType.NO_PHYSICS; } }

        virtual public ObjLevelCollType LevelCollType
        { get { return ObjLevelCollType.None; } }

        abstract public GameObjectType Type { get; }

        virtual public WeaponAttack.WeaponUserType WeaponTargetType { get { return WeaponAttack.WeaponUserType.NON; } }
        virtual public bool IsWeaponTarget { get { return recieveDamageType != RecieveDamageType.NoRecieving; } }
            
        #region NETWORK
        virtual public NetworkShare NetworkShareSettings { get { return LootFest.GO.NetworkShare.FullExceptClientDel; } }
        virtual public bool NetShareSett_HealthUpdate { get { return this.IsWeaponTarget; } }

        virtual public bool HasNetId { get { return NetworkShareSettings.Creation; } }

        public void NetworkUpdatePacket(Network.PacketReliability relyable)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(
                Network.PacketType.GameObjUpdate,
                relyable, null);
            ObjOwnerAndId.write(w);
            NetWriteUpdate(w);
        }

        virtual public void NetWriteUpdate(System.IO.BinaryWriter w)
        {
            WritePosition(Position, w);
            if (NetRotationType == NetworkClientRotationUpdateType.Plane1D)
                w.Write(rotation.ByteDir);
            else if (NetRotationType == NetworkClientRotationUpdateType.Full3D)
                LowResRotation3d.write(w);
        }
        virtual public void NetReadUpdate(System.IO.BinaryReader r)
        {
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
                    clientSpeedLength = length * 0.2f / Ref.netSession.netUpdateRate;
                }

                if (NetRotationType == NetworkClientRotationUpdateType.Plane1D)
                    clientNetReadRotation = r.ReadByte();
                else if (NetRotationType == NetworkClientRotationUpdateType.Full3D)
                    LowResRotation3d = ByteVector3.FromStream(r);
        }


        const int RWPosMaxHeight = Map.WorldPosition.ChunkHeight * 2;

        public static void WritePosition(Vector3 pos, System.IO.BinaryWriter w)
        {
            Vector2 percentPos = VectorExt.V3XZtoV2(pos) / Map.WorldPosition.WorldBlocksWidth;
            percentPos *= PublicConstants.UInt16Size;
            float percentHeight = (pos.Y + 0.25f) / RWPosMaxHeight;
            percentHeight *= PublicConstants.ByteSize;
            w.Write((ushort)percentPos.X);
            w.Write((byte)percentHeight);
            w.Write((ushort)percentPos.Y);
        }
        public static Vector3 ReadPosition(System.IO.BinaryReader r)
        {
            const float ShortToWP = (float)Map.WorldPosition.WorldBlocksWidth / PublicConstants.UInt16Size;
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


        virtual public void netWriteGameObject(System.IO.BinaryWriter w)
        {
            //obj receive this
            ObjOwnerAndId.write(w);

            if (NetShareSett_HealthUpdate)
                writeHealth(w);
        }
        //NET RECIEVE OBJECT
        public AbsUpdateObj(GoArgs args)
        {
            if (args.LocalMember)
            {
                ObjOwnerAndId = new NetworkId(LfRef.gamestate.LocalHostingPlayer.pData.netId());
                startWp = args.startWp;
                localMember = true;
                if (autoAddToUpdate)
                {
                    LfRef.gamestate.GameObjCollection.AddGameObject(this);
                }
            }
            else
            { //Client gameobject
                Health = float.MaxValue;
                localMember = false;
                ObjOwnerAndId = new NetworkId(args.reader);

                if (PlatformSettings.ViewErrorWarnings)
                {
                    if (ObjOwnerAndId.id == 0 && !(this is PlayerCharacter.AbsHero))
                    {
                        //Debug.Log(DebugLogType.Error, "Gameobject got id 0: " + this.ToString());
                    }
                }

                if (NetShareSett_HealthUpdate)
                {
                    readHealth(args.reader);
                    //startHealth = Health;
                }
                LfRef.gamestate.GameObjCollection.AddGameObject(this);
            }


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
                //Debug.Log("Share game object: " + Type.ToString() + ", ObjIx: " + this.ObjOwnerAndId.ToString());

                HasNetworkClient = true;
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.AddGameObject, to, Network.PacketReliability.Reliable, LfLib.LocalHostIx);
                w.Write((ushort)this.Type);
                w.Write((byte)characterLevel);
                WritePosition(Position, w);
               // w.Write((byte)this.UnderType);
                netWriteGameObject(w);
                
                if (to.To != Network.SendPacketTo.All)
                {
                    updatePositionToNewbie = 40;
                }
            }
        }

        

        protected System.IO.BinaryWriter NetworkWriteObjectState(AiState state)
        {
            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.GameObjectState, Network.PacketReliability.Reliable);
            ObjOwnerAndId.write(w);
            w.Write((byte)state);
            return w;
        }

        //protected System.IO.BinaryWriter BeginWriteObjectStateAsynch(AiState state)
        //{
        //    System.IO.BinaryWriter w = Ref.netSession.BeginAsynchPacket();
        //    ObjOwnerAndId.write(w);
        //    w.Write((byte)state);
        //    return w;
        //}

        //protected void EndWriteObjectStateAsynch(System.IO.BinaryWriter w)
        //{
        //    Ref.netSession.EndAsynchPacket(w, Network.PacketType.GameObjectState, Network.SendPacketTo.All, 0, Network.PacketReliability.Reliable, null);
        //}


        public void NetworkReadObjectState(System.IO.BinaryReader r)
        {
            AiState state = (AiState)r.ReadByte();
            networkReadObjectState(state, r);
        }
        virtual protected void networkReadObjectState(AiState state, System.IO.BinaryReader r)
        { }

        public int SpottedArrayMemberIndex { get; set; }
        public bool SpottedArrayUseIndex { get { return true; } }
        
        protected int updatePositionToNewbie = 0;

        public bool HasNetworkClient = false;

        public bool IsOrHasClient { get { return !localMember || HasNetworkClient; } }

#endregion


        virtual public void listenToVirtualSound_asynch(VikingEngine.LootFest.Director.VirtualSoundSphere sound)
        { }
       

        public virtual void clientStartPosition()
        {
            if (WorldPos.ChunkGrindex.X == 0)
                WorldPos = Map.WorldPosition.WorldCenter;
        }

        virtual public Vector3 LightSourcePosition { get { return Position; } }
        virtual public float LightSourceRadius { get { return 32; } }
        virtual public LightParticleType LightSourceType { get { return Graphics.LightParticleType.NUM_NON; } }
        virtual public LightSourcePrio LightSourcePrio { get { return Graphics.LightSourcePrio.Medium; } }

        int lightSourceArrayIndex = -1;
        public int LightSourceArrayIndex { get { return lightSourceArrayIndex; } set { lightSourceArrayIndex = value; } }
        public float LightSourceDistanceToGamer { get; set; }

        public override string ToString()
        {
            return "GO: " + Type.ToString() + "(" + characterLevel.ToString() + ") - " + ObjOwnerAndId.ToString();
        }
        virtual public MountType MountType
        {
            get { return MountType.NumNone; }
        }
        virtual public AbsUpdateObj AssignedRiderOrMount
        {
            get { return null; }
        }

        virtual public SpriteName InteractVersion2_interactIcon { get { throw new NotImplementedException(this.ToString() + " interact icon"); } }
        virtual public void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start) { throw new NotImplementedException(this.ToString() + " interact event"); }
        virtual public bool Interact_AutoInteract { get { return false; } }
        virtual public bool Interact_Enabled { get { return true; } }

        protected void Interact2_SearchPlayer(bool threaded)
        {
            PlayerCharacter.AbsHero hero = LfRef.LocalHeroes[Ref.rnd.Int(LfRef.LocalHeroes.Count)];
            if (Interact2_HeroCollision(hero))
            {
                if (threaded)
                    new Timer.Action1ArgTrigger<PlayerCharacter.AbsHero>(Interact2_PromptHero, hero);
                else
                    Interact2_PromptHero(hero);
            }
        }
        virtual protected void Interact2_PromptHero(PlayerCharacter.AbsHero hero)
        {
            hero.InteractPrompt_ver2(this);
        }
        virtual protected bool Interact2_HeroCollision(PlayerCharacter.AbsHero hero)
        {
            return PositionDiff(hero).Length() < 28;
        }


        /// <summary>
        /// Landing on the ground from falling
        /// </summary>
        virtual public void onGroundPounce(float fallSpeed) { }

        virtual public void stunForce(float power, float takeDamage, bool headStomp, bool local)
        { }

        virtual public void onResetBoss()
        {  
        }

        virtual public float GivesBravery { get { return 1; } }

        virtual public RotationQuarterion RotationQuarterion
        {
            get { throw new NotImplementedException(); }
        }

        public override void ChildObject_OnParentRemoval(Characters.AbsCharacter parent)
        {
            DeleteMe();
        }

        virtual public Vector3 HeadPosition { get { return Position; } }

        virtual protected Vector3 expressionEffectPosOffset { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Combines expressionEffectPosOffset with current pos
        /// </summary>
        public Vector3 expressionEffectPos()
        {
            Vector3 offset = expressionEffectPosOffset;

            Vector3 result = Position;
            Vector2 forwardDir = rotation.Direction(offset.Z);
            result.X += forwardDir.X;
            result.Y += offset.Y;
            result.Z += forwardDir.Y;

            return result;
        }

        virtual public void onObjectSwapOut(AbsUpdateObj original, AbsUpdateObj replacedWith) { }

        virtual public void onLevelDestroyed(BlockMap.AbsLevel level)
        {
            //if (this.levelEnum == level.LevelEnum)
            //{
            //    if (PlatformSettings.DevBuild)
            //    { Debug.Log("onLevelDestroyed removes go: " + this.ToString()); }
            //    DeleteMe(true);
            //}
            //else if (managedGameObject && this.levelEnum == Map.WorldLevelEnum.NUM_NON && PlatformSettings.DevBuild)
            //{
            //    Debug.LogError("Managed GO don't know it's level: " + this.ToString());
            //}
        }

        virtual public CardType CardCaptureType
        {
            get { return CardType.NumNon; }
        }
        virtual public bool canBeCardCaptured
        {
            get { return false; }
        }
        virtual public Graphics.AbsVoxelObj getModel()
        {
            return null;
        }

        //public void RestrictToArea(Rectangle2 bounds, BlockMap.LevelEnum level)
        //{
        //    //managedGameObject = true;
        //    //this.levelEnum = level;
        //    SetAsManaged();
        //    bounds.AddRadius(1);
        //    boundary = new GO.Physics.RectangleAreaBoundary(bounds);
        //}
    }

   
}

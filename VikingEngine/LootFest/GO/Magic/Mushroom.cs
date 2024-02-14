using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Magic
{
    class Mushroom : AbsVoxelObj
    {
       // //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(122,224,129), new Vector3(1.2f));
        
        const float GrowTime = 400;
        const float LifeTime = 6000;
        static readonly IntervalF GrowSpeedRange = new IntervalF(0.02f, 0.025f);
        const float PoisionCloudRadius = 6;

        float growSpeed;
        const float FadeSpeed = 0.02f;

        MushroomState state = MushroomState.Growing;
        Timer.Basic stateTimer = new Timer.Basic(GrowTime);

        public Mushroom(GoArgs args)
            : base(args)
        {
            growSpeed = GrowSpeedRange.GetRandom();

            WorldPos = args.startWp;
            WorldPos.SetAtClosestFreeY(0);
            //position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition);

            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.poision_mushroom, 0, 1, false);//LootfestLib.Images.StandardObjInstance(VoxelObjName.poision_mushroom);
            image.position = WorldPos.BlockTopFaceV3();
            image.scale = Vector3.Zero;

            
        }

        public override void Time_Update(UpdateArgs args)
        {
#if !CMODE
            if (stateTimer.Update(args.time))
            {

                if (state == MushroomState.Growing)
                {
                    float r = image.scale.X * 3.4f;
                    CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(r, r);//LootFest.ObjSingleBound.QuickBoundingBox(image.Scale.X / image.OneScale);
                    CollisionAndDefaultBound.UpdatePosition2(this); 
                    stateTimer.Set(LifeTime);
                }
                state++;
            }

            if (state == MushroomState.Growing)
            {
                image.scale += Vector3.One * growSpeed;
            }
            else if (state == MushroomState.FadeOut)
            {
                //if (damageCheck)
                //{
                //    for (int i = 0; i < 4; i++)
                //    {
                //        Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Poision,
                //            new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(image.Position, 4) ));
                //    }
                //}

                image.scale.Y -= FadeSpeed;
                if (image.scale.Y <= 0.02f)
                {
                    state++;
                    DeleteMe();
                }
            }
            //base.Time_Update(args);
#endif
        }
        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            if (state == MushroomState.Waiting || damageCheck)
            {
                characterCollCheck(args.allMembersCounter);
            }
        }

        bool damageCheck = false;
        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(0.5f, WeaponAttack.WeaponUserType.Player, NetworkId.Empty, 
             Magic.MagicElement.Poision);
        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (character.WeaponTargetType != WeaponAttack.WeaponUserType.NON)
            {
                if (damageCheck)
                { //Someone walked into the poision cloud
                    
                    new Process.UnthreadedDamage(Damage, character);
                }
                else
                { //Someone stomped on the mushroom, fill an area will deadly gas
                    new GameObjectEventTrigger(gasCloudBound, this);
                    return true;
                }
                
            }
            return false;
        }

        void gasCloudBound()
        {
            stateTimer.Set(1); //Make sure FadeOut state will start
            CollisionAndDefaultBound.DeleteMe();
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBound(PoisionCloudRadius, PoisionCloudRadius);//QuickBoundingBox(6 * image.Scale.X / image.OneScale);
            CollisionAndDefaultBound.UpdatePosition2(this);
            Vector3 aoePos = image.position; aoePos.Y += 2;
            new Effects.CirkularAOE(aoePos, PoisionCloudRadius, Color.LightGreen, Graphics.ParticleSystemType.Poision);
            damageCheck = true;
        }
        
        public override WeaponAttack.WeaponUserType WeaponTargetType
        {
            get
            {
                return WeaponAttack.WeaponUserType.NON;
            }
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.Mushroom; }
        }
    
        protected override RecieveDamageType recieveDamageType
        {
            get { return RecieveDamageType.NoRecieving; }
        }

        enum MushroomState
        {
            Growing,
            Waiting,
           // DamageCheck,
            FadeOut,
            Removed,
        }
    }
}

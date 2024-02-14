using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Magic
{
    class Mushroom : AbsVoxelObj
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(122,224,129), new Vector3(1.2f));
        
        const float GrowTime = 400;
        const float LifeTime = 6000;
        static readonly IntervalF GrowSpeedRange = new IntervalF(0.02f, 0.025f);
        const float PoisionCloudRadius = 6;

        float growSpeed;
        const float FadeSpeed = 0.02f;

        MushroomState state = MushroomState.Growing;
        Timer.Basic stateTimer = new Timer.Basic(GrowTime);

        public Mushroom(Vector3 position)
            : base()
        {
            growSpeed = GrowSpeedRange.GetRandom();

            WorldPosition = new Map.WorldPosition(position);
            WorldPosition.SetFromGroundY(0);
            //position.Y = LfRef.chunks.GetScreen(WorldPosition).GetGroundY(WorldPosition);

            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.poision_mushroom, TempImage, 0, 1);//LootfestLib.Images.StandardObjInstance(VoxelModelName.poision_mushroom);
            image.position = WorldPosition.BlockTopFaceV3();
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
                    CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(r, r);//LootFest.ObjSingleBound.QuickBoundingBox(image.Scale.X / image.OneScale);
                    CollisionBound.UpdatePosition2(this); 
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
                //            new Graphics.ParticleInitData(lib.RandomV3(image.Position, 4) ));
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
        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (state == MushroomState.Waiting || damageCheck)
            {
                characterCollCheck(args.allMembersCounter);
            }
        }

        bool damageCheck = false;
        static readonly WeaponAttack.DamageData Damage = new WeaponAttack.DamageData(LootfestLib.PoisionMushroomDamage, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, 
            Gadgets.GoodsType.NONE, Magic.MagicElement.Poision);
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
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
            CollisionBound.DeleteMe();
            CollisionBound = LF2.ObjSingleBound.QuickCylinderBound(PoisionCloudRadius, PoisionCloudRadius);//QuickBoundingBox(6 * image.Scale.X / image.OneScale);
            CollisionBound.UpdatePosition2(this);
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

        public override ObjectType Type
        {
            get { return ObjectType.Magic; }
        }
        public override int UnderType
        {
            get { return 0; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.PickUp
{
    class CardCapture : AbsHeroPickUp
    {
        public CardType capture;

        public CardCapture(GoArgs args, CardType capture)
            : base(args)
        {
            this.capture = capture;
            physics.SpeedY = 0.02f;
        }

        protected override VoxelModelName imageType
        {
            get { return VoxelModelName.card; }
        }
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255, 228, 0), new Vector3(1, 1, 0.2f));
        //protected override Data.TempBlockReplacementSett tempImage
        //{
        //    get { return TempImage; }
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.CardCapture; }
        }
        public override CardType CardCaptureType
        {
            get
            {
                return capture;
            }
        }
        protected override bool giveStartSpeed
        {
            get
            {
                return false;
            }
        }
        protected override bool autoMoveTowardsHero
        {
            get
            {
                return false;
            }
        }

        protected override void checkActiveUpdate()
        {
            //base.checkActiveUpdate();
            if (checkOutsideUpdateArea_ActiveChunk())
            {
                DeleteMe();
            }
        }

        public override void Force(Vector3 center, float force)
        {
            //do nothing
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }
    }

    class CardCaptureAnimation : AbsUpdateable
    {
        Graphics.AbsVoxelObj model;
        CardType CardCaptureType;
        float scaleSpeed;
        float scaleAccelerate;

        public CardCaptureAnimation(AbsUpdateObj character)
            : base(true)
        {
            CardCaptureType = character.CardCaptureType;
            model = character.getModel();
            model.AddToRender();

            scaleSpeed = 15f * model.Scale1D;
            scaleAccelerate = -5f * model.Scale1D;
        }

        public override void Time_Update(float time)
        {
            model.Scale1D += scaleSpeed * Ref.DeltaTimeSec;
            if (Ref.TimePassed16ms)
            {
                scaleSpeed += scaleAccelerate;
                Engine.ParticleHandler.AddExpandingParticleArea(Graphics.ParticleSystemType.WeaponSparks, model.position, 4f, 8, -0.5f);
                
            }
            if (model.Scale1D <= 0.05f)
            {
                new CardCapture(new GoArgs( model.position + Vector3.Up * 2f), CardCaptureType);
                DeleteMe();
            }
        }
        public override void DeleteMe()
        {
            model.DeleteMe();
            base.DeleteMe();
        }
    }
    
}

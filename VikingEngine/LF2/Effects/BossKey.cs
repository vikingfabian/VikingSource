using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Effects
{
    class BossKey : ItemTriumph
    {
        public BossKey(Vector3 chestPos, GameObjects.Characters.Hero hero)
            :base(chestPos, hero)
        {
        }
        public BossKey(System.IO.BinaryReader r, Director.GameObjCollection objColl)
            :base(r, objColl)
        {  }
        override protected VoxelModelName imageName { get { return VoxelModelName.bosskey; } }

        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(255,197,59), new Vector3(0.4f, 1.5f, 0.2f));
        protected override Data.IReplacementImage tempImage
        {
            get { return TempImage; }
        }

        override protected float imageScale { get { return 2; } }
        override protected EffectNetType effectNetType { get { return EffectNetType.BossKey; } }
    }

    class BossDeathItem : ItemTriumph
    {
        public BossDeathItem(Vector3 magicianPos)
            :base(magicianPos, GameObjects.AbsUpdateObj.ClosestHero(magicianPos))
        { }
        public BossDeathItem(System.IO.BinaryReader r, Director.GameObjCollection objColl)
            :base(r, objColl)
        {  }
        override protected VoxelModelName imageName { get { return VoxelModelName.stone_heart; } }
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.Black, new Vector3(1.2f, 1.5f, 0.5f));
        protected override Data.IReplacementImage tempImage
        {
            get { return TempImage; }
        }

        override protected float imageScale { get { return 2; } }
        override protected EffectNetType effectNetType { get { return EffectNetType.BossDeathItem; } }
    }

    abstract class ItemTriumph: AbsUpdateable
    {
        Vector3 rotationSpeed = new Vector3(0.05f, 0, 0);
        static readonly float ViewTime = lib.SecondsToMS(4);
        const float RaiseSpeed = 0.002f;
        bool raiseMode = true;
        GameObjects.AbsUpdateObj hero;
        Graphics.AbsVoxelObj image;
        Timer.Basic raiseTimer = new Timer.Basic(ViewTime);
        Timer.Basic safeTimer = new Timer.Basic(lib.MinutesToMS(2));

        public ItemTriumph(Vector3 chestPos, GameObjects.Characters.Hero hero)
            :base(true)
        {
            this.hero = hero;
            basicInit(chestPos);

            System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_CreateEffect, Network.PacketReliability.Reliable);
            w.Write((byte)effectNetType);
            hero.ObjOwnerAndId.WriteStream(w);
            GameObjects.AbsUpdateObj.WritePosition(chestPos, w);
        }
        public ItemTriumph(System.IO.BinaryReader r, Director.GameObjCollection objColl)
            : base(false)
        {
            hero = objColl.GetActiveOrClientObjFromIndex(r);
            if (hero == null)
            {
                return;
            }
            Vector3 startPos = GameObjects.AbsUpdateObj.ReadPosition(r);
            basicInit(startPos);
            AddToUpdateList();
        }


        void basicInit(Vector3 startPos)
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(imageName, tempImage, imageScale, 0);
            image.position = startPos;
        }

        abstract protected VoxelModelName imageName { get; }
        abstract protected Data.IReplacementImage tempImage { get; }
        abstract protected float imageScale { get; }

        public override void Time_Update(float time)
        {
            image.Rotation.RotateWorld(rotationSpeed);
            Engine.ParticleHandler.AddParticleArea(Graphics.ParticleSystemType.GoldenSparkle, image.position, 0.7f, 2);
            if (raiseMode)
            {
                image.position.Y += RaiseSpeed * time;
                if (raiseTimer.Update(time))
                {
                    raiseMode = false;
                }
            }
            else
            {//fly towards the player
                const float FlySpeed = 0.05f;
                Vector3 diff = hero.Position - image.position;
                if (diff.Length() < FlySpeed * time)
                {
                    this.DeleteMe();
                }
                else
                {
                    diff.Normalize();
                    image.position += diff * FlySpeed * time;
                }
            }

            if (safeTimer.Update(time))
            { //to make sure it wont hunt the hero forever
                DeleteMe();
            }
        }
        public override void DeleteMe()
        {
            image.DeleteMe();
            base.DeleteMe();
        }

        abstract protected EffectNetType effectNetType { get; }
        
    }

}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Bagatelle
{
    class Peg : AbsGameObject
    {
        public const int SizeUpFramesTime = 6;
        static readonly Color PegFlashCol = new Color(255, 174, 254);
        static readonly Color SnakePegFlashCol = new Color(201, 254, 174);

        public const float PegElasticity = 0.7f;
        const int PegHealth = 5;
        int health = PegHealth;

        Sound.SoundSettings hitSound = new Sound.SoundSettings(LoadedSound.bass_pluck, 0.4f);
        int sizeUpTimer = 0;
        float scale;
        bool snakePeg;

        public Peg(Vector2 pos, int networkId, bool snakePeg, BagatellePlayState state)
            :base(networkId, state)
        {
            this.snakePeg = snakePeg;
            scale = BagLib.PegSprite.toImageSize(state.PegScale);

            image = new Graphics.Image(pegImage(health, false), pos,
               new Vector2(scale), BagLib.PegsLayer, true);
            createShadow();
            bound = new Physics.CircleBound(image.Position, state.PegScale * 0.5f);

            elasticity = PegElasticity;
        }

        SpriteName pegImage(int health, bool on)
        {
            if (snakePeg)
            {
                switch (health)
                {
                    default:
                        return on ? SpriteName.bagSnakePeg1_on : SpriteName.bagSnakePeg1_off;
                    case 4:
                        return on ? SpriteName.bagSnakePeg2_on : SpriteName.bagSnakePeg2_off;
                    case 3:
                        return on ? SpriteName.bagSnakePeg3_on : SpriteName.bagSnakePeg3_off;
                    case 2:
                        return on ? SpriteName.bagSnakePeg4_on : SpriteName.bagSnakePeg4_off;
                    case 1:
                        return on ? SpriteName.bagSnakePeg5_on : SpriteName.bagSnakePeg5_off;
                }
            }
            else
            {
                switch (health)
                {
                    default:
                        return on ? SpriteName.bagPeg1_on : SpriteName.bagPeg1_off;
                    case 4:
                        return on ? SpriteName.bagPeg2_on : SpriteName.bagPeg2_off;
                    case 3:
                        return on ? SpriteName.bagPeg3_on : SpriteName.bagPeg3_off;
                    case 2:
                        return on ? SpriteName.bagPeg4_on : SpriteName.bagPeg4_off;
                    case 1:
                        return on ? SpriteName.bagPeg5_on : SpriteName.bagPeg5_off;
                }
            }
        }

        public override void update()
        {
            base.update();
            if (sizeUpTimer > 0)
            {
                if (--sizeUpTimer <= 0)
                {
                    image.Size1D = scale;
                    image.SetSpriteName(pegImage(health, false));
                }
            }

            updateShadow();
        }

        public override void OnCollsion(AbsGameObject otherObj, Physics.Collision2D coll, bool primaryCheck)
        {
            base.OnCollsion(otherObj, coll, primaryCheck);
            
            takeHit(1);
        }

        void takeHit(int damage)
        {
            health -= damage;
            onTakeHit(true);
            beginNetWriteItemStatus(null);
        }

        void onTakeHit(bool local)
        {
            if (health <= 0)
            {
                DeleteMe(local);
                pickupType = true;
            }
            else
            {
                image.SetSpriteName(pegImage(health, true));
                hitSound.pitchAdd = 0.2f - health * 0.1f;
                hitSound.Play(image.Position);
                sizeUpTimer = SizeUpFramesTime;
                image.Size1D = scale * 1.2f;

            }

            new PegFlashAnimation(image, snakePeg? SnakePegFlashCol : PegFlashCol);
        }

        protected override void netWriteItemStatus(System.IO.BinaryWriter w)
        {
            base.netWriteItemStatus(w);
            w.Write((byte)VikingEngine.Bound.Min(health, 0));
        }
        public override void netReadItemStatus(AbsGameObject affectingItem, System.IO.BinaryReader r)
        {
 	        base.netReadUpdate(r);
            health = r.ReadByte();

            onTakeHit(false);
        }
        
        public override void PickUpEvent(AbsGameObject collectingItem, LocalGamer gamer)
        {
            gamer.collectPoint(1, collectingItem, this);
        }

        public override void OnHitWaveCollision(Ball ball, LocalGamer gamer)
        {
            takeHit(BumpWave.CollisionDamage);

            if (pickupType)
            {
                PickUpEvent(ball, gamer);
            }
           
        }
    }


}

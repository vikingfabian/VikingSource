using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class Shield : AbsVoxelObj //, Process.ILoadImage
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(1.2f, 1.2f, 0.1f));
        Characters.AbsCharacter parent;
        float hideTime = 0;

        public Shield(Gadgets.Shield data, Characters.AbsCharacter parent) //mata in färg
        {
            const float ShieldScale = 1.6f;
            this.parent = parent;
            VoxelModelName img;
            float boundScale;
            switch (data.Type)
            {
                default:
                    img = VoxelModelName.Shield1;
                    boundScale = 0.4f;
                    break;
                case Gadgets.ShieldType.Round:
                    img = VoxelModelName.Shield2;
                    boundScale = 0.6f;
                    break;
                case Gadgets.ShieldType.Square:
                    img = VoxelModelName.Shield3;
                    boundScale = 0.7f;
                    break;
            }
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(img, TempImage, ShieldScale, 0);

            CollisionBound = new LF2.ObjSingleBound(new LF2.BoundData2(new Physics.Box1axisBound(
                new VectorVolume(Vector3.Zero, new Vector3(boundScale, boundScale, 0.3f)),Rotation1D.D0), new Vector3(0, 1, 0))); //QuickBoundingBox(boundScale);
        }


        public void Hide(float time, UseHands hands)
        {
            if (hands == UseHands.OneHand)
            {
                hideTime = time;
            }
            else if (hands == UseHands.TwoHands)
            {
                hideTime = float.MaxValue;
            }

            if (hands != UseHands.QuickDraw)
                image.Visible = false;
        }

        public bool WeaponShieldCheck(AbsUpdateObj weapon)
        {
            if (image.Visible)
            {
                if (CollisionBound.Intersect2(weapon.CollisionBound) != null)
                {
                    //obj.DeleteMe();
                    Music.SoundManager.PlaySound(LoadedSound.weaponclink, image.position);
                    return true;
                }
            }
            return false;
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            return base.handleCharacterColl(character, collisionData);
        }

        public override void Time_Update(UpdateArgs args)
        {
            
            if (image.Visible)
            {
                updatePosition();
            }
            else
            {
                hideTime -= args.time;
                if (hideTime <= 0)
                {
                    image.Visible = true;
                    updatePosition();
                }
            }
        }
        void updatePosition()
        {
            const float Distance = 0.7f;
            const float HeightAjd = -0.3f;
            const float RotationAdd = -0.22f; //adjust to left side
            rotation = parent.FireDir;
            rotation.Radians += RotationAdd;
            image.position = parent.Position + Map.WorldPosition.V2toV3(rotation.Direction(Distance), +HeightAjd);
            //image.Rotation = parent.RotationQuarterion;
            Map.WorldPosition.Rotation1DToQuaterion(image, parent.FireDir.Radians + MathHelper.Pi);

            CollisionBound.UpdatePosition2(this);
        }

        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }
        public override int UnderType
        {
            get { return -1; }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GameObjects.NetworkShare.None;
            }
        }
    }
}

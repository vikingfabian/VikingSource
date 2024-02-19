using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.Gadgets;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class Shield : AbsVoxelObj, Process.ILoadImage
    {
        const float ShieldScale = 1.6f;
        public const float StandardShieldDistance = 0.76f;
        ////static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(1.2f, 1.2f, 0.1f));
        
        public float shieldDistance;
        float hideTime = 0;

        public Shield(Characters.AbsCharacter parent, Players.PlayerStorage pStorage) //mata in färg
            :base(GoArgs.Empty)
        {
            

            setParent(parent);

            //VoxelModelName img;
            float boundScale;
            
            //img = VoxelModelName.Shield2;
            boundScale = 0.6f;

            image = new Graphics.VoxelModelInstance(null);//VoxelModelInstance(LfRef.Images.StandardModel_TempBlock);//LfRef.modelLoad.AutoLoadModelInstance(img, TempImage, ShieldScale, 0, false);
            image.Scale1D = ShieldScale * image.SizeToScale;

            CollisionAndDefaultBound = new Bounds.ObjectBound(new LootFest.BoundData2(new VikingEngine.Physics.Box1axisBound(
                new VectorVolumeC(Vector3.Zero, new Vector3(boundScale, boundScale, 0.3f)),Rotation1D.D0), new Vector3(0, 1, 0))); //QuickBoundingBox(boundScale);

            refreshModel(pStorage);
        }

        public void refreshModel(Players.PlayerStorage pStorage)
        {
            VoxelModelName model = VoxelModelName.NUM_NON;

            switch (pStorage.shieldType)
            {
                case Players.ShieldType.Round1:
                    model = VoxelModelName.shield_round1;
                    break;
                case Players.ShieldType.Round2:
                    model = VoxelModelName.shield_round2;
                    break;
                case Players.ShieldType.Round3:
                    model = VoxelModelName.shield_round3;
                    break;
                case Players.ShieldType.Round4:
                    model = VoxelModelName.shield_round4;
                    break;
                case Players.ShieldType.Spartan1:
                    model = VoxelModelName.shield_spartan1;
                    break;
                case Players.ShieldType.Spartan2:
                    model = VoxelModelName.shield_spartan2;
                    break;
                case Players.ShieldType.Spartan3:
                    model = VoxelModelName.shield_spartan3;
                    break;
                case Players.ShieldType.Keit1:
                    model = VoxelModelName.shield_keit1;
                    break;

            }

            //new Process.ModifiedImage(this, model,
            //        new List<ByteVector2>
            //    {
            //        new ByteVector2((byte)Data.MaterialType.RGB_red, pStorage.ShieldMainColor),
            //        new ByteVector2((byte)Data.MaterialType.RGB_green, pStorage.ShieldDetailColor),
            //        new ByteVector2((byte)Data.MaterialType.pale_skin, pStorage.ShieldEdgeColor),

            //    }, null, Vector3.Zero, 0);
        }

        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            image.SetMaster(original);
            image.Scale1D = ShieldScale * image.SizeToScale;
        }

        public void setParent(Characters.AbsCharacter parent)
        {
            parent.AddChildObject(this);

            if (parent is PlayerCharacter.HorseRidingHero)
            {
                shieldDistance = 0.8f;
            }
            else
            {
                shieldDistance = StandardShieldDistance;
            }
        }

        public void Hide(float time)
        {
            hideTime = time;
            image.Visible = false;
        }

        public bool WeaponShieldCheck(AbsUpdateObj weapon)
        {
            if (image.Visible)
            {
                if (CollisionAndDefaultBound.Intersect2(weapon.CollisionAndDefaultBound) != null)
                {
                    Music.SoundManager.WeaponClink(image.position);
                    return true;
                }
            }
            return false;
        }

        public override void Time_Update(UpdateArgs args)
        { }

        public void update()
        {
            if (!image.Visible)
            {
                hideTime -= Ref.DeltaTimeMs;
                if (hideTime <= 0)
                {
                    image.Visible = true;
                }
            }
        }

        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
            if (image.Visible)
            {
                
                const float HeightAjd = -0.3f;
                const float RotationAdd = -0.22f; //adjust to left side
                rotation = parent.FireDir(GameObjectType.PlayerShield);
                rotation.Radians += RotationAdd;
                image.position = parent.HandWeaponPosition + VectorExt.V2toV3XZ(rotation.Direction(shieldDistance), +HeightAjd);
                Map.WorldPosition.Rotation1DToQuaterion(image, parent.FireDir(GameObjectType.PlayerShield).Radians + MathHelper.Pi);

                CollisionAndDefaultBound.UpdatePosition2(this);
            }
            return IsDeleted;
        }


        public override GameObjectType Type
        {
            get { return GameObjectType.PlayerShield; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return GO.NetworkShare.None;
            }
        }

        protected override bool  autoAddToUpdate
        {
	        get 
	        { 
		         return false;
	        }
        }
    }
}

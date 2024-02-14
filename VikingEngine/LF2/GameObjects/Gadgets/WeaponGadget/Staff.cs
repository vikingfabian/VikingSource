using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Gadgets.WeaponGadget
{

    /* Fire, skjut eldboll, cirkel med eld
     * Evil, rökmoln som rör sig fram ,Svart kråka anfaller närmsta med explosion, vänder mot dig om fiende ej hittas
     * Poision, en svamp, svamp fält
     * Lightning, skjuter spark, stor blixt 
     */

    class Staff : HandWeapon
    {

        public Staff(System.IO.BinaryReader r, byte version)
        {
            ReadStream(r, version, GadgetSaveCategory.NUM_NONE);
        }

        public Staff(Data.Gadgets.CraftingTemplate template)
        { //From crafting
            staffBasicInit();

            templateToPower(template);
            powerToDamage();
        }
        public override void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write((byte)weaponType);
            w.Write(power);

        }
        public override void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory)
        {
            weaponType = (Data.Gadgets.BluePrint)r.ReadByte();
            staffBasicInit();
            power = r.ReadByte();
            powerToDamage();
        }
        public Staff(Data.Gadgets.BluePrint type)
        {//Only for testing
            weaponType = type;
            staffBasicInit();
        }

        void staffBasicInit()
        {
            Magic.MagicElement magic;
            //VoxelModelName model;

            switch (weaponType)
            {
                case Data.Gadgets.BluePrint.FireStaff:
                    magic = Magic.MagicElement.Fire;
                    //model = VoxelModelName.staff_fire;
                    break;
                case Data.Gadgets.BluePrint.EvilStaff:
                    magic = Magic.MagicElement.Evil;
                   // model = VoxelModelName.staff_evil;
                    break;
                case Data.Gadgets.BluePrint.PoisionStaff:
                    magic = Magic.MagicElement.Poision;
                    //model = VoxelModelName.staff_poision;
                    break;
                case Data.Gadgets.BluePrint.LightningStaff:
                    magic = Magic.MagicElement.Lightning;
                    //model = VoxelModelName.staff_light;
                    break;
                default:
                    throw new NotImplementedException();

            }
            damage = new WeaponAttack.DamageData(0, WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, GoodsType.Wood, magic);
            reloadtime = 200;
            attacktime = 400;

            scale = 0.16f;
            
        }

        protected override void CreateMesh()
        {
            //Magic.MagicType magic;
            VoxelModelName model;

            switch (weaponType)
            {
                case Data.Gadgets.BluePrint.FireStaff:
                    //magic = Magic.MagicType.Fire;
                    model = VoxelModelName.staff_fire;
                    break;
                case Data.Gadgets.BluePrint.EvilStaff:
                   // magic = Magic.MagicType.Evil;
                    model = VoxelModelName.staff_evil;
                    break;
                case Data.Gadgets.BluePrint.PoisionStaff:
                   // magic = Magic.MagicType.Poision;
                    model = VoxelModelName.staff_poision;
                    break;
                case Data.Gadgets.BluePrint.LightningStaff:
                    //magic = Magic.MagicType.Lightning;
                    model = VoxelModelName.staff_light;
                    break;
                default:
                    throw new NotImplementedException();

            }
            LoadMasterImage(model);
        }

        public const float MushroomPlacementDist = 5;
        public override Vector2 Use(Characters.Hero parent, Microsoft.Xna.Framework.Vector3 target, GadgetAlternativeUseType altUse, bool localUse)
        {
           
            if (altUse == GadgetAlternativeUseType.Staff_fire)
            { //Small magic attack
                if (parent.SpendMagic(LootfestLib.StaffBlastMagicUse))
                {
                    switch (damage.Magic)
                    {
                        case Magic.MagicElement.Fire:
                            //create magic effect
                            
                        case Magic.MagicElement.Lightning:
                            //börjar med ett litet sprak sen kommer en kraftig smäll

                            break;
                        case Magic.MagicElement.Poision:
                            //växer upp svampar som avger gift vid beröring, minläggare
                            Vector3 mpos = parent.Position + Map.WorldPosition.V2toV3(parent.FireDir.Direction(MushroomPlacementDist));
                            new Magic.Mushroom(mpos);
                            break;
                    }
                }
            }
            else  if (altUse == GadgetAlternativeUseType.Staff_blast)
            {//Large magic attack
                if (parent.SpendMagic(LootfestLib.StaffBlastMagicUse))
                {
                    switch (damage.Magic)
                    {
                        case Magic.MagicElement.Fire:
                            //create magic effect
                            Vector3 fireBallStartPos = parent.Position;
                            fireBallStartPos += Map.WorldPosition.V2toV3(parent.Rotation.Direction(2));
                            Rotation1D dir = parent.Rotation;
                            dir.Add(Ref.rnd.Plus_MinusF(0.15f));
                            new Magic.FireBall(modifiedDamage, parent.Position, dir);
                            break;
                        case Magic.MagicElement.Lightning:
                            //börjar med ett litet sprak sen kommer en kraftig smäll

                            new Elements.Thunder(target);
                            break;
                        case Magic.MagicElement.Poision:
                            //växer upp svampar som avger gift vid beröring, minläggare
                            new Magic.MushroomGroup(parent.Position, parent.FireDir);
                            break;
                    }
                }
            }

            return base.Use(parent, target, altUse, localUse);
        }

        public override List<GadgetAlternativeUse> AlternativeUses(Players.PlayerProgress progress)
        {
            return new List<GadgetAlternativeUse>
                {
                    new GadgetAlternativeUse( GadgetAlternativeUseType.Standard, "Poke"),
                    new GadgetAlternativeUse( GadgetAlternativeUseType.Staff_fire, "Magic fire"),
                    new GadgetAlternativeUse( GadgetAlternativeUseType.Staff_blast, "Magic blast"),
                };
        }

        public override bool UsesTargeting
        {
            get { return damage.Magic == Magic.MagicElement.Lightning; }
        }

        public override bool UsesAmmo
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            return damage.Magic.ToString() + " staff";
        }

       

        override public GadgetSaveCategory SaveCategory
        {
            get { return GadgetSaveCategory.Staff; }
        }

        public override UseHands Hands
        {
            get { return UseHands.TwoHands; }
        }
        public override SpriteName Icon
        {
            get { return SpriteName.WeaponStaff; }
        }
        public override string GadgetInfo
        {
            get
            {
                return "The staff is a powerful magic weapon";
            }
        }
    }
}

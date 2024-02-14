using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    static class WeaponLib
    {
        public static bool PlayerToPlayerDamage = false;
        public static bool ToyToToyDamage = true;

        public static bool IsFoeTarget(AbsUpdateObj user1, AbsUpdateObj user2, bool aiTargeting)
        {
            return IsFoeTarget(user1.WeaponTargetType, user2.WeaponTargetType, aiTargeting);
        }

        /// <param name="aiTargeting">AI is activly looking for targets to attack</param>
        /// <returns>The two characters will damage eachother in combat</returns>
        public static bool IsFoeTarget(WeaponUserType attacker, WeaponUserType defender, bool aiTargeting)
        {
            bool result = false;
            switch (attacker)
            {
                case WeaponUserType.Player:
                    result = defender == WeaponUserType.Enemy || defender == WeaponUserType.PassiveEnemy || defender == WeaponUserType.Critter ||
                        (PlayerToPlayerDamage &&
                        (defender == WeaponUserType.Player));
                    break;
                case WeaponUserType.Enemy:
                    result = defender == WeaponUserType.Player || defender == WeaponUserType.Friendly || (defender == WeaponUserType.Critter && !aiTargeting);
                    break;
                case WeaponUserType.PassiveEnemy:
                    goto case WeaponUserType.Enemy;

                case WeaponUserType.Friendly:
                    result = defender == WeaponUserType.Enemy || (defender == WeaponUserType.PassiveEnemy && !aiTargeting);
                    break;
                case WeaponUserType.NON: //hurts everything
                    return defender != WeaponUserType.NON;
                case WeaponUserType.Critter:
                    result = (defender == WeaponUserType.Enemy || defender == WeaponUserType.PassiveEnemy) && !aiTargeting;
                    break;
            }
            return result;
        }


        public static bool IsFoeTarget(WeaponUserType user, NetworkId userIndex, AbsUpdateObj target, bool aiTargeting)
        {
            bool result = IsFoeTarget(user, target.WeaponTargetType, aiTargeting); 
            return result && (userIndex.id <= 0 || userIndex != target.DamageObjIndex);
        }
    }

    struct DamageData
    {
        public WeaponAttack.WeaponUserType User;
        public NetworkId UserIndex;

        public static readonly DamageData BasicCollDamage = new DamageData(LfLib.BasicDamage, WeaponUserType.Enemy, NetworkId.Empty);
        public static readonly DamageData NoN = new DamageData(0, WeaponUserType.NON, NetworkId.Empty);
        public Magic.MagicElement Magic;
        //public GO.Gadgets.Quality MagicLevel;
        public float Damage;
        public WeaponPush Push;
        public Rotation1D PushDir;
        /// <summary>
        /// To prevent attacks like lightning to go on forever
        /// </summary>
        public bool Secondary;
        //public Gadgets.GoodsType Material;
        public SpecialDamage Special;
        public int recievingBoundIndex;

        enum DamageDataIncludes { User, UserIndex, Material, Magic, Special, Push, NUM };

        public string DamageText()
        {
            return "Damage: " + Convert.ToInt32(Damage).ToString();
        }

        public override string ToString()
        {
            return "Damage " + Damage.ToString() + ", Usertype " + User.ToString() + ", UserIx " + UserIndex.ToString() + ", Magic " + Magic.ToString() + 
                ", Push " + Push.ToString() + ", special " + Special.ToString();
        }
        //public void WriteVisualDamage(System.IO.BinaryWriter w)
        //{
        //    w.Write((byte)Damage);
        //    w.Write((byte)Magic);
        //}
        //public void ReadVisualDamage(System.IO.BinaryReader r)
        //{
        //  Damage = r.ReadByte();
        //    Magic = (GO.Magic.MagicElement)r.ReadByte();
            
        //}

        public void writeDamage(System.IO.BinaryWriter w)
        {
            w.Write((byte)(Damage * 4));
        }
        public void ReadDamage(System.IO.BinaryReader r)
        {
            Damage = (float)r.ReadByte() / 4;
        }
        //public void WriteStream(System.IO.BinaryWriter w)
        //{
        //    w.Write((byte)Damage);
        //    EightBit includes = new EightBit();

        //    bool includesUser = User != WeaponUserType.Enemy;
        //    bool includesUserIndex = UserIndex.id > 0;
        //    //bool includesMaterial = Material != Gadgets.GoodsType.NONE;
        //    bool includesMagic = Magic != GO.Magic.MagicElement.NoMagic;
        //    bool includesSpecial = Special != SpecialDamage.NONE;
        //    bool includesPush = Push != WeaponPush.NON;

        //    includes.Set((int)DamageDataIncludes.User, includesUser);
        //    includes.Set((int)DamageDataIncludes.UserIndex, includesUserIndex);
        //   // includes.Set((int)DamageDataIncludes.Material, includesMaterial);
        //    includes.Set((int)DamageDataIncludes.Magic, includesMagic);
        //    includes.Set((int)DamageDataIncludes.Special, includesSpecial);
        //    includes.Set((int)DamageDataIncludes.Push, includesPush);

        //    includes.WriteStream(w);
        //    if (includesUser)
        //        w.Write((byte)User);
        //    if (includesUserIndex)
        //        UserIndex.write(w);
        //    //if (includesMaterial)
        //    //    w.Write((byte)Material);
        //    if (includesMagic)
        //    {
        //        w.Write((byte)Magic);
        //        //w.Write((byte)MagicLevel);
        //    }
        //    if (includesSpecial)
        //        w.Write((byte)Special);
        //    if (includesPush)
        //    {
        //        w.Write((byte)Push);
        //        w.Write(PushDir.ByteDir);
        //    }
        //}
        //public void ReadStream(System.IO.BinaryReader r)
        //{
        //    Damage = r.ReadByte();
        //    EightBit includes = EightBit.FromStream(r);
        //    if (includes.Get((int)DamageDataIncludes.User))
        //        User = (WeaponAttack.WeaponUserType)r.ReadByte();
        //    if (includes.Get((int)DamageDataIncludes.UserIndex))
        //        UserIndex = new NetworkId(r);//ByteVector2.FromStream(r);
        //    //if (includes.Get((int)DamageDataIncludes.Material))
        //    //    Material = (Gadgets.GoodsType)r.ReadByte();
        //    if (includes.Get((int)DamageDataIncludes.Magic))
        //    {
        //        Magic = (GO.Magic.MagicElement)r.ReadByte();
        //        //MagicLevel = (Gadgets.Quality)r.ReadByte();
        //    }
        //    if (includes.Get((int)DamageDataIncludes.Special))
        //        Special = (SpecialDamage)r.ReadByte();
        //    if (includes.Get((int)DamageDataIncludes.Push))
        //    {
        //        Push = (WeaponPush)r.ReadByte();
        //        PushDir.ByteDir = r.ReadByte();
        //    }
        //}
        //public static DamageData FromStream(System.IO.BinaryReader r)
        //{
        //    DamageData result = DamageData.BasicCollDamage;
        //    //result.ReadStream(r);
        //    return result;
        //}

        public DamageData(float damage)
            : this(damage, WeaponUserType.NON, NetworkId.Empty)
        { }


        public DamageData(float damage, WeaponAttack.WeaponUserType user, NetworkId userIndex)
            : this(damage, user, userIndex, GO.Magic.MagicElement.NoMagic, SpecialDamage.NONE, false)
        { }
        public DamageData(float damage, WeaponAttack.WeaponUserType user, NetworkId userIndex, GO.Magic.MagicElement magic)
            :this(damage, user, userIndex, magic, SpecialDamage.NONE, false)
        { }
        public DamageData(float damage, WeaponAttack.WeaponUserType user, NetworkId userIndex, 
            GO.Magic.MagicElement magic, SpecialDamage special, bool secondary)
            : this(damage, user, userIndex, magic, special, WeaponPush.NON, Rotation1D.D0, secondary)
        {
        }
        public DamageData(float damage, WeaponAttack.WeaponUserType user, NetworkId userIndex,
            GO.Magic.MagicElement magic,
            SpecialDamage special, WeaponPush push, Rotation1D pushDir)
            : this(damage, user, userIndex, magic, special, push, pushDir, false)
        { }

        public DamageData(float damage, WeaponAttack.WeaponUserType user, NetworkId userIndex, 
            GO.Magic.MagicElement magic, 
            SpecialDamage special, WeaponPush push, Rotation1D pushDir, bool secondary)
        {
            this.PushDir = pushDir;
            this.UserIndex = userIndex;
            this.User = user;
            this.Damage = damage;
            //this.Material = material;
            this.Magic = magic;
            //this.MagicLevel = Gadgets.Quality.Medium;
            this.Secondary = secondary;
            this.Push = push;
            this.Special = special;
            recievingBoundIndex = -1;
        }
        public static WeaponUserType oppositUser(WeaponUserType target)
        {
            switch (target)
            {
                case WeaponUserType.Enemy:
                    return WeaponUserType.Player;
                case WeaponUserType.Friendly:
                    return WeaponUserType.Enemy;
                case WeaponUserType.Player:
                    return WeaponUserType.Enemy;

            }
            return target;
        }

        public void AddHeroSpecials(PlayerCharacter.AbsHero parent, bool meleeWeapon)
        {
#if !CMODE
            //if (Magic != GO.Magic.MagicElement.NoMagic)
            //{
            //    if (parent.Player.Progress.GotSkill(GO.Magic.MagicRingSkill.Magic_boost))
            //    {
            //        Special = SpecialDamage.TinyBoost;
            //    }
            //    switch (Magic)
            //    {
            //        case GO.Magic.MagicElement.Evil:
            //            if (parent.Player.Progress.GotSkill(GO.Magic.MagicRingSkill.Evil_boost))
            //            {
            //                Special = SpecialDamage.SmallBoost;
            //            }
            //            break;
            //        case GO.Magic.MagicElement.Fire:
            //            if (parent.Player.Progress.GotSkill(GO.Magic.MagicRingSkill.Fire_boost))
            //            {
            //                Special = SpecialDamage.SmallBoost;
            //            }
            //            break;
            //        case GO.Magic.MagicElement.Lightning:
            //            if (parent.Player.Progress.GotSkill(GO.Magic.MagicRingSkill.Lighting_boost))
            //            {
            //                Special = SpecialDamage.SmallBoost;
            //            }
            //            break;
            //        case GO.Magic.MagicElement.Poision:
            //            if (parent.Player.Progress.GotSkill(GO.Magic.MagicRingSkill.Poision_boost))
            //            {
            //                Special = SpecialDamage.SmallBoost;
            //            }
            //            break;
            //    }
            //}

            //if (parent.Player.Progress.GotSkill(GO.Magic.MagicRingSkill.Barbarian_swing) && meleeWeapon)
            //{
            //    Push++;
            //}
#endif
           
        }
    }
    //enum WeaponUtype
    //{
    //    Impulse,
    //    //Projectile,
    //    GravityArrow,
    //    GoldenArrow,
    //    SlingStone,
    //    BabyAxe,
    //    SkeletonBone,
    //    ThrowingSpear,
    //    GoblinSpear,
    //    //Spear,
    //    ToyProjectile,
    //    ToyBoatProjectile,
    //    FlyingPetProjectile,
    //    ProjectileEvilBurst,
    //    ProjectilePoisionBurst,
    //    Bomb,
    //    ExplodingBarrel,
    //    EvilMissile,
    //    TurretBullet,
    //    SquigBullet,
    //    ScorpionBullet1,
    //    ScorpionBullet2,
    //    EntRoot,

    //    FireGoblinBall,
    //    GruntStone,
    //    SquigSpawnBullet,
    //    HumanoidArrow,
    //    BombLightSpark,
    //    MagicianLightSpark,
    //    MagicianPoisonBomb,
    //    MagicianThrowSword,
    //    MagicianFireball,
    //    FireBreath,
    //    Explosion,

    //    NecroBall,
    //    NecroMetroite,
    //    WiseLadyAttack,
    //    FatEgg,
    //    BatSonar,
    //}
    //enum WeaponTrophyType
    //{
    //    Arrow_Slingstone,
    //    Javelin,
    //    Sword,
    //    Axe,
    //    Spear,
    //    //TwoHandMelee,
    //    Other,
    //}
    enum FriendlyFireType
    {
        HurtsAll,
        AllButOwner, //fixa ID
        NoFriendly,
    }
    enum WeaponUserType
    {
        Player,
        Enemy,
        /// <summary>
        /// Should not be attacked by friendly AI
        /// </summary>
        PassiveEnemy,
        //Toy,
        Friendly,
        Neutral,
        Critter,
        //Chaotic, //against all
        NON,
    }
    enum WeaponPush
    {
        NON,
        Small,
        Normal,
        Large,
        Huge,
        GoFlying,
        NUM
    }
    enum SpecialDamage
    {
        NONE,
        TinyBoost,
        SmallBoost,
        IgnoreShield,
        TerrainDamageFloor,
        CardThrow,
        ShieldBreakAttack,
        PickAxe,
        //HandWeaponProjectile, //magic that fires out from the sword
        //PickAxe,
        //Sickle,
        //Condition,
       // MagicBurst,
    }
}

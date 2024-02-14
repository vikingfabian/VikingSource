using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LF2.GameObjects.Magic;

namespace VikingEngine.LF2.GameObjects.Gadgets
{
    class Jevelery : IGadget
    {
        MagicRingSkill skill;
        public MagicRingSkill Skill { get { return skill; } }

        public Jevelery(MagicRingSkill skill)
        {
            this.skill = skill;
        }
        public Jevelery(System.IO.BinaryReader r, byte version)
        {
            ReadStream(r, version, WeaponGadget.GadgetSaveCategory.NUM_NONE);
        }

        public void WriteStream(System.IO.BinaryWriter w)
        { w.Write((byte)skill); }

        public void ReadStream(System.IO.BinaryReader r, byte version, GameObjects.Gadgets.WeaponGadget.GadgetSaveCategory saveCategory)
        { skill = (MagicRingSkill)r.ReadByte(); }

        public GadgetType GadgetType { get { return Gadgets.GadgetType.Jevelery; } }

        public SpriteName Icon
        {
            get
            {
                return MagicRingSkillIcon(skill);
            }
        }

        public static SpriteName MagicRingSkillIcon(Magic.MagicRingSkill skill)
        {
            switch (skill)
            {
                case Magic.MagicRingSkill.Apple_lover:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Barbarian_swing:
                    return SpriteName.RingBasic;
                case Magic.MagicRingSkill.Butcher:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Elven_warrior:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Evil_aura:
                    return SpriteName.RingBlack;
                case Magic.MagicRingSkill.Evil_boost:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Evil_touch:
                    return SpriteName.RingBlack;
                case Magic.MagicRingSkill.Fire_boost:
                    return SpriteName.RingRed;
                case Magic.MagicRingSkill.First_string:
                    return SpriteName.RingBasic;
                case Magic.MagicRingSkill.First_swing:
                    return SpriteName.RingBasic;
                case Magic.MagicRingSkill.Healer:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Hobbit_skill:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Librarian:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Light_hand:
                    return SpriteName.RingBasic;
                case Magic.MagicRingSkill.Lighting_boost:
                    return SpriteName.RingBlue;
                case Magic.MagicRingSkill.Magic_boost:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Paladins:
                    return SpriteName.RingRed;
                case Magic.MagicRingSkill.Poision_boost:
                    return SpriteName.RingGreen;
                case Magic.MagicRingSkill.Projectile_evil_burst:
                    return SpriteName.RingBlack;
                case Magic.MagicRingSkill.Projectile_fire_burst:
                    return SpriteName.RingRed;
                case Magic.MagicRingSkill.Projectile_lightning_burst:
                    return SpriteName.RingBlue;
                case Magic.MagicRingSkill.Projectile_poision_burst:
                    return SpriteName.RingGreen;
                case Magic.MagicRingSkill.Recylcling_bowman:
                    return SpriteName.RingBasic;
                case Magic.MagicRingSkill.Ring_of_protection:
                    return SpriteName.RingWhite;
                case Magic.MagicRingSkill.Spear_rush:
                    return SpriteName.RingBasic;
                case Magic.MagicRingSkill.Javelin_master:
                    return SpriteName.RingBasic;
            }

            return SpriteName.RingBasic;
        }

        public void EquipEvent()
        { }
        public override string ToString()
        {
            return MagicRingSkillName(skill);
        }

        public static string MagicRingSkillName(Magic.MagicRingSkill skill)
        {
            return TextLib.EnumName(skill.ToString()) + " ring";
        }

        public string GadgetInfo
        {
            get
            {
                string skillInfo = TextLib.EmptyString;
                switch (skill)
                {
                    case Magic.MagicRingSkill.Recylcling_bowman:
                        skillInfo = "You can reuse some of the arrows you fire";
                        break;
                    case Magic.MagicRingSkill.Magic_boost:
                        skillInfo = "Tiny enhancement of all magic attacks";
                        break;
                    case Magic.MagicRingSkill.Apple_lover:
                        skillInfo = "Apples heal more";
                        break;
                    case Magic.MagicRingSkill.Barbarian_swing:
                        skillInfo = "Push enemies harder when hitting them";
                        break;
                    case Magic.MagicRingSkill.Butcher:
                        skillInfo = "Get better skin quality from killed creatures";
                        break;
                    case Magic.MagicRingSkill.Elven_warrior:
                        skillInfo = "Wooden swords are as efficient as a one in iron";
                        break;
                    case Magic.MagicRingSkill.Evil_aura:
                        skillInfo = "Tiny damage to all standing close to you";
                        break;
                    case Magic.MagicRingSkill.Evil_boost:
                        skillInfo = "Small enhancement of all evil magic attacks";
                        break;
                    case Magic.MagicRingSkill.Evil_touch:
                        skillInfo = "Small damage to all who you walk into";//skada även vänner men det skadar även dig
                        break;
                    case Magic.MagicRingSkill.Fire_boost:
                        skillInfo = "Small enhancement of all fire magic attacks";
                        break;
                    case Magic.MagicRingSkill.First_swing:
                        skillInfo = "Powerful first blow if you rest (from attacking) for a while, with hand weapons";
                        break;
                    case Magic.MagicRingSkill.First_string:
                        skillInfo = "Powerful first projectiles if you rest (from attacking) for a while";
                        break;
                    case Magic.MagicRingSkill.Hobbit_skill:
                        skillInfo = "All food give more health";
                        break;
                    case Magic.MagicRingSkill.Healer:
                        skillInfo = "Eating food will heal nearby friends";
                        break;
                    case Magic.MagicRingSkill.Librarian:
                        skillInfo = "Small enhancement of all scrolls";
                        break;
                    case Magic.MagicRingSkill.Light_hand:
                        skillInfo = "Hand weapons are faster";
                        break;
                    case Magic.MagicRingSkill.Lighting_boost:
                        skillInfo = "Small enhancement of all lightning magic attacks";
                        break;
                    case Magic.MagicRingSkill.Paladins:
                        skillInfo = "Magic hand weapons will fire";
                        break;
                    case Magic.MagicRingSkill.Poision_boost:
                        skillInfo = "Small enhancement of all poision magic attacks";
                        break;
                    case Magic.MagicRingSkill.Projectile_evil_burst:
                        skillInfo = "Arrows hurt all close to it upon impact";
                        break;
                    case Magic.MagicRingSkill.Projectile_fire_burst:
                        skillInfo = "Arrows explodes upon impact";
                        break;
                    case Magic.MagicRingSkill.Projectile_lightning_burst:
                        skillInfo = "Arrows spread lighting upon impact";
                        break;
                    case Magic.MagicRingSkill.Projectile_poision_burst:
                        skillInfo = "Arrows leave a cloud of poision where it lands";
                        break;
                    case Magic.MagicRingSkill.Ring_of_protection:
                        skillInfo = "Your health lasts longer";
                        break;
                    case Magic.MagicRingSkill.Spear_rush:
                        skillInfo = "Alternative attack for spears";
                        break;
                    case Magic.MagicRingSkill.Javelin_master:
                        skillInfo = "Throwing spears give more damage";
                        break;
                 
                }
                return "Wear the ring to gain powers. " + TextLib.EnumName(skill.ToString()) + ": " + skillInfo;
            }
        }

        public ushort ItemHashTag
        {
            get
            {
                ushort result = GadgetLib.GadgetTypeHash(this.GadgetType);
                result += (ushort)Skill;
                return result;
            }
        }

        public bool EquipAble { get { return true; } }
        public int StackAmount
        {
            get { return 1; }
            set
            { //do nothing
            }
        }

        public bool Scrappable { get { return true; } }
        public GadgetList ScrapResult()
        {
            throw new NotImplementedException();
        }
        public int Weight { get { return LootfestLib.JeveleryWeight; } }
        public bool Empty { get { return false; } }
    }
}
